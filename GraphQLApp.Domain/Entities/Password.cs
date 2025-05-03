using GraphQLApp.Base;
using GraphQLApp.Base.Abstractions;

namespace GraphQLApp.Entities;

public class Password : BaseEntity<int>, ISoftDelete
{
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public int UserId { get; set; }
    public bool IsDeleted { get; set; }

    public User User { get; set; } = null!;
}