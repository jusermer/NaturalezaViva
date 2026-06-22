// Domain/Interfaces/IPasswordHasher.cs
namespace NaturalezaViva.Domain.Interfaces;

public interface IPasswordHasher
{
    string Hash(string plainPassword);
    bool Verify(string plainPassword, string hashedPassword);
}
