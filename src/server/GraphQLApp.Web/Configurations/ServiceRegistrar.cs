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

        var sqidsConfig = builder.Configuration.GetSection("Sqids").Get<SqidsOptions>();
        builder.Services.AddSingleton(new SqidsEncoder<int>(sqidsConfig ?? new SqidsOptions { MinLength = 5 }));

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
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddInMemorySubscriptions();

        services.AddDependencies();
    }
}