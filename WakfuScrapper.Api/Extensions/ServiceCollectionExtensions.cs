using System.Reflection;
using WakfuScrapper.Api.Attributes;
using WakfuScrapper.Api.Features.ArmorFeature;

namespace WakfuScrapper.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAllServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddHttpContextAccessor();
        services.AddHttpClient();

        services.AddAllServicesFromAssembly();
    }

    public static void AddAllServicesFromAssembly(this IServiceCollection services)
    {
        var serviceClassList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract &&
                        t.Namespace?.StartsWith(AppDomain.CurrentDomain.FriendlyName + ".Features") == true &&
                        t.GetCustomAttribute<ServiceAvailableAttribute>() != null)
            .ToList();

        foreach (var service in serviceClassList)
        {
            var serviceAttribute = service.GetCustomAttribute<ServiceAvailableAttribute>();
            switch (serviceAttribute.Type)
            {
                case ServiceType.Scoped:
                    services.AddScoped(service);
                    break;
                case ServiceType.Transient:
                    services.AddTransient(service);
                    break;
                case ServiceType.Singleton:
                    services.AddSingleton(service);
                    break;
            }
        }
    }
}