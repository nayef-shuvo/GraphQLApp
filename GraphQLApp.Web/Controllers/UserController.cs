using GraphQLApp.Dtos;
using GraphQLApp.Entities;
using GraphQLApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLApp.Controllers;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository<User, Guid> _userRepository;

    public UserController(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return NotFound("User not found");

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email
        };

        await _userRepository.InsertAsync(user);

        return Ok(user);
    }
}