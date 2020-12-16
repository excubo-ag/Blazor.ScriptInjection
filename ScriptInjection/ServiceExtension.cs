using Excubo.Analyzers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Excubo.Blazor.ScriptInjection
{
    public static class ServiceExtension
    {
        [Exposes(typeof(ScriptInjectionTracker)), As(typeof(IScriptInjectionTracker))]
#pragma warning disable CS0618 // Type or member is obsolete
        [Exposes(typeof(Script))]
#pragma warning restore CS0618 // Type or member is obsolete
        [Exposes(typeof(AddScript))]
        [IgnoreDependency(typeof(IJSRuntime))]
        public static IServiceCollection AddScriptInjection(this IServiceCollection services, bool onload_notification = true, bool gzipped_bootstrap = false)
        {
            return services.AddScoped<IScriptInjectionTracker>((sp) => new ScriptInjectionTracker(onload_notification, gzipped_bootstrap));
        }
    }
}