using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalezaViva.Application.DTOs.Auth;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "El token es requerido")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string NewPassword { get; set; } = string.Empty;
}