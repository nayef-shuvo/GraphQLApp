namespace GraphQLApp.Base.Abstractions;

public interface IBaseEntity<T> where T : struct
{
    T Id { get; set; }
}