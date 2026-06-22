using NaturalezaViva.Application.DTOs.Users;

namespace NaturalezaViva.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public UserResponseDto User { get; set; } = null!;
}