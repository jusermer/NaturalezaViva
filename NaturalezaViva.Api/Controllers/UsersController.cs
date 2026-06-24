using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaturalezaViva.Application.DTOs.Users;
using NaturalezaViva.Application.Interfaces;

namespace NaturalezaViva.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]   
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProfile(Guid id, UserUpdateDto request)
    {
        var user = await _userService.UpdateProfileAsync(id, request);
        return Ok(user);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UserStatusUpdateRequestDto request)
    {
        await _userService.UpdateStatusAsync(id, request);
        return NoContent();
    }
}