using Microsoft.AspNetCore.Mvc;
using NewsApi.Models;
using NewsApi.Models.Dtos;
using NewsApi.Repositories.Abstract;
using NewsApi.Tokens;

namespace NewsApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly ITokenGenerator _tokenGenerator;
    
    public AuthController(IUserRepository repo, ITokenGenerator tokenGenerator)
    {
        _repo = repo;
        _tokenGenerator = tokenGenerator;
    }
    
    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromForm] RegisterRequest request)
    {
        var user = new User() { Username = request.Username, Password = request.Password, RoleId = request.RoleId };
        try
        {
            var registeredUser = await _repo.RegisterUserAsync(user);
            return Ok($"User with ID {registeredUser.Id} registered successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Log the user in by comparing credentials
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var user = new User() { Username = request.Username, Password = request.Password };
        try
        {
            var loggedInUser = await _repo.LoginUserAsync(user.Username, user.Password);
            var token = _tokenGenerator.GenerateToken(loggedInUser);
            return Ok(token);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}