using GraphQLApp.Base;
using GraphQLApp.GraphQL.Schema.Subscriptions;
using GraphQLApp.Users;
using HotChocolate.Subscriptions;

namespace GraphQLApp.GraphQL.Schema.Mutations;

public class Mutation : IScopedDependency
{
    private readonly IUserService _userService;
    private readonly ITopicEventSender _topicEventSender;

    public Mutation(IUserService userService, ITopicEventSender topicEventSender)
    {
        _userService = userService;
        _topicEventSender = topicEventSender;
    }

    public async Task<UserDto> CreateAsync(CreateUpdateUserDto dto)
    {
        var result = await _userService.AddAsync(dto);

        if (result.IsFailure)
            throw new GraphQLException(result.Error!);

        var userDto = result.Value!;
        await _topicEventSender.SendAsync(nameof(Subscription.CreateUser), userDto);

        return userDto;
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