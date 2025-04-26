using GraphQLApp.Configurations;

var builder = WebApplication.CreateBuilder(args);

ServiceRegistrar.Register(builder);

var app = builder.Build();

MiddlewareRegistrar.Configure(app);

await app.RunAsync();