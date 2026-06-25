using NaturalezaViva.Application.DTOs.Users;
using NaturalezaViva.Application.Interfaces;
using NaturalezaViva.Application.Mappers;
using NaturalezaViva.Domain.Exceptions;
using NaturalezaViva.Domain.Interfaces;
using NaturalezaViva.Domain.ValueObjects;

namespace NaturalezaViva.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException("Usuario no encontrado.");

        return UserMapper.ToResponseDto(user);
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(UserMapper.ToResponseDto).ToList();
    }

    public async Task<UserResponseDto> UpdateProfileAsync(Guid userId, UserUpdateDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            throw new NotFoundException("Usuario no encontrado.");

        var newEmail = new Email(request.Email);

        if (user.Email.Value != newEmail.Value)
        {
            var emailExists = await _userRepository.ExistsAsync(newEmail);
            if (emailExists)
                throw new DomainException("El email ya está en uso por otro usuario.");
        }

        user.UpdateName(request.Name);
        user.UpdateEmail(newEmail);

        await _userRepository.UpdateAsync(user);

        return UserMapper.ToResponseDto(user);
    }

    public async Task UpdateStatusAsync(Guid userId, UserStatusUpdateRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            throw new NotFoundException("Usuario no encontrado.");

        if (request.IsActive)
            user.Activate();
        else
            user.Deactivate();

        await _userRepository.UpdateAsync(user);
    }
}