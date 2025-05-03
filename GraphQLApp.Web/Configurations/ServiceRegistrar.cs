using GraphQLApp.Data;
using GraphQLApp.Extensions;
using GraphQLApp.GraphQL;
using GraphQLApp.Repositories;
using Microsoft.EntityFrameworkCore;
using Sqids;

namespace GraphQLApp.Configurations;

public static class ServiceRegistrar
{
    public static void Register(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        RegisterServices(builder.Services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAuthorization();

        services.AddScoped(typeof(IRepository<,>), typeof(EfCoreRepository<,>));

        services.AddGraphQLServer()
            .AddQueryType<Query>();

        services.AddSingleton(new SqidsEncoder<int>(new SqidsOptions
        {
            MinLength = 10
        }));

        services.AddDependencies();
    }
}