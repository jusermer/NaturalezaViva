using NaturalezaViva.Application.DTOs.Auth;
using NaturalezaViva.Application.DTOs.Users;
using NaturalezaViva.Application.Interfaces;
using NaturalezaViva.Domain.Entities;
using NaturalezaViva.Domain.Interfaces;
using NaturalezaViva.Domain.ValueObjects;
using NaturalezaViva.Application.Mappers;

namespace NaturalezaViva.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    // ─── Register ───────────────────────────────────────────────
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequest request)
    {
        var email = new Email(request.Email);

        if (await _userRepository.ExistsAsync(email))
            throw new InvalidOperationException("El email ya está registrado.");

        if (!Enum.TryParse<DocumentType>(request.DocumentType, out var docType))
            throw new ArgumentException($"Tipo de documento inválido: {request.DocumentType}");

        var document = new Document(docType, request.DocumentNumber);

        if (await _userRepository.ExistsByDocumentAsync(document))
            throw new InvalidOperationException("El documento ya está registrado.");

        var user = User.Create(
            name: request.Name,
            email: email,
            document: document,
            plainPassword: request.Password,
            hasher: _passwordHasher);

        await _userRepository.AddAsync(user);

        var token = _tokenGenerator.GenerateJwt(user);

        return new AuthResponseDto
        {
            Token = token,
            User = UserMapper.ToResponseDto(user)
        };
    }

    // ─── Login ──────────────────────────────────────────────────
    public async Task<AuthResponseDto> LoginAsync(LoginRequest request)
    {
        var email = new Email(request.Email);
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null || !user.PasswordHash.Verify(request.Password, _passwordHasher))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("El usuario está inactivo.");

        var token = _tokenGenerator.GenerateJwt(user);

        return new AuthResponseDto
        {
            Token = token,
            User = UserMapper.ToResponseDto(user)
        };
    }

    // ─── Validar token ──────────────────────────────────────────
    public Task<bool> ValidateTokenAsync(string token)
    {
        var isValid = _tokenGenerator.ValidateJwt(token);
        return Task.FromResult(isValid);
    }

    // ─── Forgot password ────────────────────────────────────────
    public async Task SendPasswordResetTokenAsync(ForgotPasswordRequest request)
    {
        var email = new Email(request.Email);
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
            return; // no revelamos si el email existe o no, por seguridad

        var resetToken = _tokenGenerator.GeneratePasswordResetToken(user);

        // Aquí normalmente inyectarías IEmailSender (interfaz de Application)
        // await _emailSender.SendPasswordResetEmailAsync(user.Email.Value, resetToken);
    }

    // ─── Reset password ─────────────────────────────────────────
    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var userId = _tokenGenerator.ValidatePasswordResetToken(request.Token);

        if (userId is null)
            throw new UnauthorizedAccessException("Token inválido o expirado.");

        var user = await _userRepository.GetByIdAsync(userId.Value);

        if (user is null)
            throw new InvalidOperationException("Usuario no encontrado.");

        user.ChangePassword(request.NewPassword, _passwordHasher);

        await _userRepository.UpdateAsync(user);
    }

}