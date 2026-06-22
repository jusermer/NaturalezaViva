using System.ComponentModel.DataAnnotations;

namespace NaturalezaViva.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de documento es requerido")]
    public string DocumentType { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es requerido")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; } = string.Empty;
}