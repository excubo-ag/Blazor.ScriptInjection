using System.Threading.Tasks;

namespace Excubo.Blazor.ScriptInjection
{
    public interface IScriptInjectionTracker
    {
        public bool GzippedBootstrap { get; }
        bool OnloadNotification { get; }
        bool Initialized { get; set; }
        bool NeedsInjection(string uri);
        Task LoadedAsync(string uri);
    }
}