// Application/DTOs/Users/UserUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace NaturalezaViva.Application.DTOs.Users;

public class UserUpdateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;
}