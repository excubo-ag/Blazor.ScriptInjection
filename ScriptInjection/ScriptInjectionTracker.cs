using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Excubo.Blazor.ScriptInjection
{
    internal class ScriptInjectionTracker : IScriptInjectionTracker
    {
        public ScriptInjectionTracker(bool onload_notification)
        {
            OnloadNotification = onload_notification;
        }
        private readonly HashSet<string> set = new HashSet<string>();
        private readonly Dictionary<string, (Task Task, SemaphoreSlim Semaphore)> load_markers = new Dictionary<string, (Task Task, SemaphoreSlim Semaphore)>();
        private readonly object sync = new object();
        public bool NeedsInjection(string uri)
        {
            lock (sync)
            {
                if (set.Add(uri))
                {
                    load_markers.Add(uri, CreateBlockUntilLoadedAsync());
                    return true;
                }
                return false;
            }
        }
        public bool Initialized { get; set; }
        public bool OnloadNotification { get; }

        [JSInvokable]
        public void Loaded(string uri)
        {
            if (!load_markers.ContainsKey(uri))
            {
                return;
            }
            var (_, semaphore) = load_markers[uri];
            semaphore.Release();
        }
        public Task LoadedAsync(string uri)
        {
            if (!load_markers.ContainsKey(uri))
            {
                load_markers.Add(uri, CreateBlockUntilLoadedAsync());
            }
            return load_markers[uri].Task;
        }
        private (Task Task, SemaphoreSlim Semaphore) CreateBlockUntilLoadedAsync()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            return (Task: WaitAsync(semaphore), Semaphore: semaphore);
        }
        private Task WaitAsync(SemaphoreSlim semaphore)
        {
            return semaphore.WaitAsync();
        }
    }
}