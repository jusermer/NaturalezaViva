using NaturalezaViva.Domain.ValueObjects;
using Xunit;

namespace NaturalezaViva.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Constructor_ConEmailValido_CreaInstanciaCorrectamente()
    {
        // Arrange
        var emailValido = "usuario@naturalezaviva.com";

        // Act
        var email = new Email(emailValido);

        // Assert
        Assert.Equal("usuario@naturalezaviva.com", email.Value);
    }

    [Fact]
    public void Constructor_ConMayusculasYEspacios_NormalizaElValor()
    {
        // Arrange
        var emailConEspacios = "  USUARIO@NaturalezaViva.com  ";

        // Act
        var email = new Email(emailConEspacios);

        // Assert
        Assert.Equal("usuario@naturalezaviva.com", email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("no-es-un-email")]
    [InlineData("@sin-usuario.com")]
    [InlineData("sin-dominio@")]
    public void Constructor_ConEmailInvalido_LanzaArgumentException(string emailInvalido)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(emailInvalido));
    }

    [Fact]
    public void Equals_ConDosEmailsConElMismoValor_SonIguales()
    {
        // Arrange
        var email1 = new Email("test@naturalezaviva.com");
        var email2 = new Email("TEST@NaturalezaViva.com"); // mismo valor, distinto casing

        // Act & Assert
        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
    }
}
