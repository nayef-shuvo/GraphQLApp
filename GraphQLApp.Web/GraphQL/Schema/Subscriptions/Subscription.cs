using GraphQLApp.Users;

namespace GraphQLApp.GraphQL.Schema.Subscriptions;

public class Subscription
{
    [Subscribe]
    public UserDto CreateUser([EventMessage] UserDto dto) => dto;
}