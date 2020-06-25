using Excubo.Blazor.ScriptInjection;
using NUnit.Framework;

namespace Excubo.Blazor.Tests_ScriptInjection
{
    public class ScriptInjectionTrackerTests
    {
        [Test]
        public void Test()
        {
            var sit = new ScriptInjectionTracker(onload_notification: true, gzipped_bootstrap: false);
            sit.MarkLoaded("hello");
            Assert.DoesNotThrowAsync(async () => await sit.LoadedAsync("hello"));
        }
    }
}
