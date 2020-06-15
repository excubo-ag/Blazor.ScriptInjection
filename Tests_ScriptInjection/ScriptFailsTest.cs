using Bunit;
using Excubo.Blazor.ScriptInjection;
using NUnit.Framework;
using System;

namespace Excubo.Blazor.Tests_ScriptInjection
{
    public class ScriptFailsTest : ComponentTestFixture
    {
        [Test]
        public void FailWithoutScripts()
        {
            Assert.Throws<InvalidOperationException>(() => RenderComponent<Script>());
        }
    }
}
