using NaturalezaViva.Domain.Entities;

namespace NaturalezaViva.Application.Interfaces;

public interface ITokenGenerator
{
    string GenerateJwt(User user);
    bool ValidateJwt(string token);
    string GeneratePasswordResetToken(User user);
    Guid? ValidatePasswordResetToken(string token);
}