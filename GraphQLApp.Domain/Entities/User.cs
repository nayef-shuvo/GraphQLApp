using GraphQLApp.Base;
using GraphQLApp.Base.Abstractions;

namespace GraphQLApp.Entities;

public class User : BaseEntity<int>, ISoftDelete
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsDeleted { get; set; }
}
