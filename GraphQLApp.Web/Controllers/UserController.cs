using GraphQLApp.Entities;
using GraphQLApp.Repositories;
using GraphQLApp.Users;
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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userRepository.GetAllAsync(includeDeleted: true);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
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
            Email = userDto.Email,
        };

        await _userRepository.InsertAsync(user);

        return Ok(user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return BadRequest("User not found");

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;

        await _userRepository.UpdateAsync(user);

        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userRepository.DeleteAsync(id);

        return Created();
    }
}
