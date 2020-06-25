using Excubo.Analyzers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Excubo.Blazor.ScriptInjection
{
    public static class ServiceExtension
    {
        [Exposes(typeof(ScriptInjectionTracker)), As(typeof(IScriptInjectionTracker))]
        [Exposes(typeof(Script))]
        [IgnoreDependency(typeof(IJSRuntime))]
        public static IServiceCollection AddScriptInjection(this IServiceCollection services, bool onload_notification = true, bool gzipped_bootstrap = false)
        {
            return services.AddScoped<IScriptInjectionTracker>((sp) => new ScriptInjectionTracker(onload_notification, gzipped_bootstrap));
        }
    }
}
