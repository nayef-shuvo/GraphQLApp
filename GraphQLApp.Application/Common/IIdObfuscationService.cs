using GraphQLApp.Base;

namespace GraphQLApp.Common;

public interface IIdObfuscationService : ISingletonDependency
{
    string Encode(int id);
    int Decode(string obfuscatedId);
}