using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cw10.Data;
using cw10.DTOs;
using cw10.Services;
using cw11.Helpers;
using cw11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = cw11.Models.LoginRequest;
using RegisterRequest = cw11.Models.RegisterRequest;

namespace cw10.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IDbService _service;

    public UserController(IConfiguration configuration, IDbService service)
    {
        _configuration = configuration;
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterStudent(RegisterRequest model)
    {
        _service.AddNewUser(model);

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetStudents()
    {
        var claimsFromAccessToken = User.Claims;
        return Ok("Secret data");
    }

    [AllowAnonymous]
    [HttpGet("anon")]
    public IActionResult GetAnonData()
    {
        return Ok("Public data");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        User user = _service.GetUser(loginRequest.Login).Result;

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }


        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "pgago"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
            //Add additional data here
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5050",
            audience: "https://localhost:5050",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        _service.RefreshToken(user.Login);

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenRequest refreshToken)
    {
        User user = _service.GetUser(refreshToken).Result;
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "pgago"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
            //Add additional data here
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:5050",
            audience: "https://localhost:5050",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        _service.RefreshToken(user.Login);

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = user.RefreshToken
        });
    }
}