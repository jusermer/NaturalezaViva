using Microsoft.AspNetCore.Mvc;
using NaturalezaViva.Application.DTOs.Auth;
using NaturalezaViva.Application.Interfaces;

namespace NaturalezaViva.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        await _authService.SendPasswordResetTokenAsync(request);
        return Ok(new { message = "Si el email existe, se enviará un link de recuperación." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        await _authService.ResetPasswordAsync(request);
        return Ok(new { message = "Contraseña actualizada correctamente." });
    }
}