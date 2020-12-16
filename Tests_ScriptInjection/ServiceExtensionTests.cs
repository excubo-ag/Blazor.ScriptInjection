using Excubo.Blazor.ScriptInjection;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Excubo.Blazor.Tests_ScriptInjection
{
    public class ServiceExtensionTests
    {
        [Test]
        public void TestEqual()
        {
            var services = new Mock<IServiceCollection>()
                .Object;
            Assert.AreEqual(services, services.AddScriptInjection());
            Assert.AreEqual(services, services.AddScriptInjection(onload_notification: false));
            Assert.AreEqual(services, services.AddScriptInjection(onload_notification: true));
        }
    }
}