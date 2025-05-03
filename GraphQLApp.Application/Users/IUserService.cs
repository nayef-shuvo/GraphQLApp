using GraphQLApp.Base;
using GraphQLApp.Common;

namespace GraphQLApp.Users;

public interface IUserService : IScopedDependency
{
    Task<Result<UserDto>> GetByIdAsync(string id);
    Task<Result<IList<UserDto>>> GetAllAsync();
    Task<Result<UserDto>> AddAsync(CreateUpdateUserDto dto);
    Task<Result<UserDto>> UpdateAsync(string id, CreateUpdateUserDto dto);
    Task<Result> DeleteAsync(string id);
}