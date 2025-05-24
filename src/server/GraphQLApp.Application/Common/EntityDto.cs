namespace GraphQLApp.Common;

public class EntityDto<T> : IEntityDto<T>
{
    public T Id { get; set; } = default!;
}