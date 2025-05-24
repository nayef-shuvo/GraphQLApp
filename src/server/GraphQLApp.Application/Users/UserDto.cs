using GraphQLApp.Common;

namespace GraphQLApp.Users;

public class UserDto : EntityDto<string>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}