using GraphQLApp.Base;
using GraphQLApp.Users;

namespace GraphQLApp.GraphQL;

public class Query : IScopedDependency
{
    private readonly IUserService _userService;

    public Query(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await _userService.GetAllAsync();
        return users;
    }
}