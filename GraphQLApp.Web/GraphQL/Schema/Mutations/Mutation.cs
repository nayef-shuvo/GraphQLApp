using GraphQLApp.Base;
using GraphQLApp.Users;

namespace GraphQLApp.GraphQL.Schema.Mutations;

public class Mutation : IScopedDependency
{
    private readonly IUserService _userService;

    public Mutation(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDto> CreateAsync(CreateUpdateUserDto dto)
    {
        var result = await _userService.AddAsync(dto);

        return result.IsSuccess
            ? result.Value!
            : throw new GraphQLException(result.Error!);
    }

    public async Task<UserDto> UpdateAsync(string id, CreateUpdateUserDto dto)
    {
        var result = await _userService.UpdateAsync(id, dto);

        return result.IsSuccess
            ? result.Value!
            : throw new GraphQLException(result.Error!);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _userService.DeleteAsync(id);

        return result.IsSuccess
            ? true
            : throw new GraphQLException(result.Error!);
    }
}