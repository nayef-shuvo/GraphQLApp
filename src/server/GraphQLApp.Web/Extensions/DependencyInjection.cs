using GraphQLApp.Base;

namespace GraphQLApp.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(x => x is { IsClass: true, IsAbstract: false });

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                if (interfaces.Contains(typeof(IScopedDependency)))
                    Register(services, type, ServiceLifetime.Scoped);
                else if (interfaces.Contains(typeof(ITransientDependency)))
                    Register(services, type, ServiceLifetime.Transient);
                else if (interfaces.Contains(typeof(ISingletonDependency)))
                    Register(services, type, ServiceLifetime.Singleton);
            }
        }

        return services;
    }

    private static void Register(IServiceCollection services, Type type, ServiceLifetime lifetime)
    {
        var serviceTypes = type.GetInterfaces()
            .Where(i => i != typeof(IScopedDependency) && i != typeof(ITransientDependency) &&
                        i != typeof(ISingletonDependency))
            .ToList();

        foreach (var serviceType in serviceTypes)
        {
            var descriptor = new ServiceDescriptor(serviceType, type, lifetime);

            var existing = services.FirstOrDefault(d => d.ServiceType == serviceType);
            if (existing != null)
                services.Remove(existing);

            services.Add(descriptor);
        }

        if (!serviceTypes.Any())
        {
            var descriptor = new ServiceDescriptor(type, type, lifetime);

            var existing = services.FirstOrDefault(d => d.ServiceType == type);
            if (existing != null)
                services.Remove(existing);

            services.Add(descriptor);
        }
    }
}