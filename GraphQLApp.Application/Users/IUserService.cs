using GraphQLApp.Base;

namespace GraphQLApp.Users;

public interface IUserService : IScopedDependency
{
    Task<UserDto?> GetByIdAsync(string id);
    Task<IList<UserDto>> GetAllAsync();
    Task<UserDto> AddAsync(CreateUpdateUserDto dto);
    Task<UserDto> UpdateAsync(string id, CreateUpdateUserDto dto);
    Task DeleteAsync(string id);
}