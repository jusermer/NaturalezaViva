// Application/Interfaces/IUserService.cs
using NaturalezaViva.Application.DTOs.Users;

namespace NaturalezaViva.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> UpdateProfileAsync(Guid userId, UserUpdateDto request);
    Task UpdateStatusAsync(Guid userId, UserStatusUpdateRequestDto request);
}