using Excubo.Analyzers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Excubo.Blazor.ScriptInjection
{
    public static class ServiceExtension
    {
        [Exposes(typeof(ScriptInjectionTracker)), As(typeof(IScriptInjectionTracker))]
        [Exposes(typeof(Script))]
        public static IServiceCollection AddScriptInjection(this IServiceCollection services)
        {
            return services.AddScoped<IScriptInjectionTracker, ScriptInjectionTracker>();
        }
    }
}
