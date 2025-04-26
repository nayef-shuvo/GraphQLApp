using System.ComponentModel.DataAnnotations;

namespace GraphQLApp.Dtos;

public class UserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;
}
