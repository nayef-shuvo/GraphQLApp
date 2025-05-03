using GraphQLApp.Common;
using Sqids;

namespace GraphQLApp.Services;

public class SqidsObfuscationService : IIdObfuscationService
{
    private readonly SqidsEncoder<int> _sqids;

    public SqidsObfuscationService(SqidsEncoder<int> sqids)
    {
        _sqids = sqids;
    }

    public string Encode(int id) => _sqids.Encode(id);
    public int Decode(string id) => _sqids.Decode(id.AsSpan())[0];
}