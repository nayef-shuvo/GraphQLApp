using GraphQLApp.Common;
using GraphQLApp.Entities;
using GraphQLApp.Repositories;
using GraphQLApp.Users;

namespace GraphQLApp.Services;

public class UserService : IUserService
{
    private readonly IRepository<User, int> _userRepository;
    private readonly IIdObfuscationService _obfuscationService;

    public UserService(IRepository<User, int> userRepository, IIdObfuscationService obfuscationService)
    {
        _userRepository = userRepository;
        _obfuscationService = obfuscationService;
    }


    public async Task<Result<UserDto>> GetByIdAsync(string id)
    {
        var intId = _obfuscationService.Decode(id);
        var user = await _userRepository.GetByIdAsync(intId);

        if (user is null)
            return Result<UserDto>.Failure("User not found");

        var userDto = MapToDto(user);
        return Result<UserDto>.Success(userDto);
    }

    public async Task<Result<IList<UserDto>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = users.Select(MapToDto).ToList();

        return Result<IList<UserDto>>.Success(userDtos);
    }

    public async Task<Result<UserDto>> AddAsync(CreateUpdateUserDto dto)
    {
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
        };

        user = await _userRepository.InsertAsync(user);
        var userDto = MapToDto(user);

        return Result<UserDto>.Success(userDto);
    }

    public async Task<Result<UserDto>> UpdateAsync(string id, CreateUpdateUserDto dto)
    {
        var intId = _obfuscationService.Decode(id);
        var user = await _userRepository.GetByIdAsync(intId);

        if (user is null)
            return Result<UserDto>.Failure("Entity not found");

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;

        user = await _userRepository.UpdateAsync(user);

        var userDto = MapToDto(user);

        return Result<UserDto>.Success(userDto);
    }

    public async Task<Result> DeleteAsync(string id)
    {
        var intId = _obfuscationService.Decode(id);
        var user = await _userRepository.GetByIdAsync(intId);

        if (user is null)
            return Result.Failure("User not found");

        await _userRepository.DeleteAsync(user);

        return Result.Success();
    }

    private UserDto MapToDto(User user) => new()
    {
        Id = _obfuscationService.Encode(user.Id),
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email
    };
}