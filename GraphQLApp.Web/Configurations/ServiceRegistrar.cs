using GraphQLApp.Data;
using GraphQLApp.Extensions;
using GraphQLApp.GraphQL.Schema.Mutations;
using GraphQLApp.GraphQL.Schema.Queries;
using GraphQLApp.GraphQL.Schema.Subscriptions;
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

        RegisterServices(builder);
    }

    private static void RegisterServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAuthorization();

        services.AddScoped(typeof(IRepository<,>), typeof(EfCoreRepository<,>));

        services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddInMemorySubscriptions();

        services.AddSingleton(new SqidsEncoder<int>(new SqidsOptions
        {
            MinLength = 10,
            Alphabet = builder.Configuration.GetValue<string>("Sqids:Alphabet")!
        }));

        services.AddDependencies();
    }
}