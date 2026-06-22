// Application/Interfaces/IEmailSender.cs
namespace NaturalezaViva.Application.Interfaces;

public interface IEmailSender
{
    Task SendPasswordResetEmailAsync(string to, string resetToken);
    Task SendWelcomeEmailAsync(string to, string name);
}