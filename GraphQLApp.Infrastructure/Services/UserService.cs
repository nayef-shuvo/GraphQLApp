using GraphQLApp.Entities;
using GraphQLApp.Exceptions;
using GraphQLApp.Repositories;
using GraphQLApp.Users;
using Sqids;

namespace GraphQLApp.Services;

public class UserService : IUserService
{
    private readonly SqidsEncoder<int> _sqids;
    private readonly IRepository<User, int> _userRepository;

    public UserService(SqidsEncoder<int> sqids, IRepository<User, int> userRepository)
    {
        _sqids = sqids;
        _userRepository = userRepository;
    }


    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var intId = _sqids.Decode(id)[0];
        var user = await _userRepository.GetByIdAsync(intId);

        if (user is null)
            throw new EntityNotFoundException();

        var userDto = new UserDto
        {
            Id = _sqids.Encode(user.Id),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return userDto;
    }

    public async Task<IList<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var userDtos = users.Select(user => new UserDto
        {
            Id = _sqids.Encode(user.Id),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        }).ToList();

        return userDtos;
    }

    public async Task<UserDto> AddAsync(CreateUpdateUserDto dto)
    {
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
        };

        user = await _userRepository.InsertAsync(user);

        return new UserDto
        {
            Id = _sqids.Encode(user.Id),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<UserDto> UpdateAsync(string id, CreateUpdateUserDto dto)
    {
        var intId = _sqids.Decode(id)[0];
        var user = await _userRepository.GetByIdAsync(intId);

        if (user is null)
            throw new EntityNotFoundException();

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;

        user = await _userRepository.UpdateAsync(user);

        return new UserDto
        {
            Id = _sqids.Encode(user.Id),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task DeleteAsync(string id)
    {
        var intId = _sqids.Decode(id)[0];
        var user = await _userRepository.GetByIdAsync(intId);

        if (user is null)
            throw new EntityNotFoundException();
    }
}