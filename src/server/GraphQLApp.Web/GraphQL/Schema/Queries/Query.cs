using GraphQLApp.Base;
using GraphQLApp.Users;

namespace GraphQLApp.GraphQL.Schema.Queries;

public class Query : IScopedDependency
{
    private readonly IUserService _userService;

    public Query(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var result = await _userService.GetAllAsync();

        return result.IsSuccess
            ? result.Value!
            : throw new GraphQLException(new Error(result.Error!, "USER_FETCH_FAILED"));
    }

    public async Task<UserDto> GetUserById(string id)
    {
        var result = await _userService.GetByIdAsync(id);

        return result.IsSuccess
            ? result.Value!
            : throw new GraphQLException(new Error(result.Error!, "USER_NOT_FOUND"));
    }
}