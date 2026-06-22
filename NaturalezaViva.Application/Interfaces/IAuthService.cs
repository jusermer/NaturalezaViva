using NaturalezaViva.Application.DTOs.Auth;

namespace NaturalezaViva.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequest request);
    Task<AuthResponseDto> LoginAsync(LoginRequest request);
    Task<bool> ValidateTokenAsync(string token);
    Task SendPasswordResetTokenAsync(ForgotPasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
}