namespace GraphQLApp.Common;

public interface IEntityDto<T>
{
    T Id { get; set; }
}