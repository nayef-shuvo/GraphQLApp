namespace GraphQLApp.Base.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}