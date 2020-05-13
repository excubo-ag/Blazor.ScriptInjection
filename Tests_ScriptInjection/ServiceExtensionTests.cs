using Moq;
using NUnit.Framework;
using Excubo.Blazor.ScriptInjection;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}