using GraphQLApp.Base.Abstractions;

namespace GraphQLApp.Base;

public class BaseEntity<T> : IBaseEntity<T> where T : struct
{
    public T Id { get; set; }
}