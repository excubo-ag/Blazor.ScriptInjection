using Excubo.Analyzers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Excubo.Blazor.ScriptInjection
{
    public static class ServiceExtension
    {
        [Exposes(typeof(ScriptInjectionTracker)), As(typeof(IScriptInjectionTracker))]
        [Exposes(typeof(Script))]
        public static IServiceCollection AddScriptInjection(this IServiceCollection services, bool onload_notification = true)
        {
            return services.AddScoped<IScriptInjectionTracker>((sp) => (ScriptInjectionTracker) ActivatorUtilities.CreateInstance(sp, typeof(ScriptInjectionTracker), onload_notification));
        }
    }
}
