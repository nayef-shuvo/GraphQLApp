using GraphQLApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApp.Configurations;

public class MiddlewareRegistrar
{
    private const string GraphQlUrl = "/api/graphql";

    public static void Configure(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseWebSockets();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGraphQL(GraphQlUrl);
    }
}