using NaturalezaViva.Domain.Interfaces;
using NaturalezaViva.Domain.ValueObjects;
using Xunit;

namespace NaturalezaViva.Domain.Tests.ValueObjects;

/// <summary>
/// Hasher falso para pruebas — no usa BCrypt real, solo simula el comportamiento
/// para no acoplar los tests de dominio a la infraestructura.
/// </summary>
public class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string plainPassword) => $"hashed:{plainPassword}";

    public bool Verify(string plainPassword, string hashedPassword) =>
        hashedPassword == $"hashed:{plainPassword}";
}

public class PasswordHashTests
{
    private readonly IPasswordHasher _hasher = new FakePasswordHasher();

    [Fact]
    public void Create_ConPasswordValida_GeneraHashCorrectamente()
    {
        // Arrange
        var plainPassword = "password123";

        // Act
        var passwordHash = PasswordHash.Create(plainPassword, _hasher);

        // Assert
        Assert.Equal("hashed:password123", passwordHash.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("corta")] // menos de 8 caracteres
    public void Create_ConPasswordInvalida_LanzaArgumentException(string passwordInvalida)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PasswordHash.Create(passwordInvalida, _hasher));
    }

    [Fact]
    public void Verify_ConPasswordCorrecta_RetornaTrue()
    {
        // Arrange
        var passwordHash = PasswordHash.Create("password123", _hasher);

        // Act
        var resultado = passwordHash.Verify("password123", _hasher);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void Verify_ConPasswordIncorrecta_RetornaFalse()
    {
        // Arrange
        var passwordHash = PasswordHash.Create("password123", _hasher);

        // Act
        var resultado = passwordHash.Verify("password-equivocada", _hasher);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void FromHash_ReconstruyeDesdeUnHashExistente()
    {
        // Arrange — simula lo que hace EF Core al leer desde la base de datos
        var hashGuardado = "hashed:password123";

        // Act
        var passwordHash = PasswordHash.FromHash(hashGuardado);

        // Assert
        Assert.Equal(hashGuardado, passwordHash.Value);
        Assert.True(passwordHash.Verify("password123", _hasher));
    }
}
