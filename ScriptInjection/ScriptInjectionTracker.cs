using System.Collections.Generic;

namespace Excubo.Blazor.ScriptInjection
{
    internal class ScriptInjectionTracker : IScriptInjectionTracker
    {
        private readonly HashSet<string> set = new HashSet<string>();
        private readonly object sync = new object();
        public bool NeedsInjection(string uri)
        {
            lock (sync)
            {
                return set.Add(uri);
            }
        }
    }
}