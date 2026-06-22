using NaturalezaViva.Application.DTOs.Users;
using NaturalezaViva.Domain.Entities;

namespace NaturalezaViva.Application.Mappers;

public static class UserMapper
{
    public static UserResponseDto ToResponseDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email.Value,
        DocumentType = user.Document.Type.ToString(),
        DocumentNumber = user.Document.Number,
        Role = user.Role.ToString(),
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}