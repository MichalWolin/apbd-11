using cw10.DTOs;
using cw10.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw10.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IDbService _service;

    public UserController(IDbService dbService)
    {
        _service = dbService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(User user)
    {
        if (await _service.DoesUserExist(user.Login))
            return BadRequest("User with login = " + user.Login + " already exists!");

        await _service.AddNewUser(user.Login, user.Password);

        return Ok();
    }
}