namespace Excubo.Blazor.ScriptInjection
{
    public interface IScriptInjectionTracker
    {
        bool NeedsInjection(string uri);
    }
}