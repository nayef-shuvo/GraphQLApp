using GraphQLApp.Data;
using GraphQLApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApp.Configurations;

public static class ServiceRegistrar
{
    public static void Register(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        RegisterServices(builder.Services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(EfCoreRepository<,>));
    }
}