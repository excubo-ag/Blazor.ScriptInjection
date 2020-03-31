using Microsoft.Extensions.DependencyInjection;

namespace Excubo.Blazor.ScriptInjection
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddScriptInjection(this IServiceCollection services)
        {
            return services.AddScoped<IScriptInjectionTracker, ScriptInjectionTracker>();
        }
    }
}
