using Bunit;
using Bunit.Mocking.JSInterop;
using Excubo.Blazor.ScriptInjection;
using System;
using Xunit;

namespace Excubo.Blazor.Tests_ScriptInjection
{
    public class ScriptNoNotificationTests : ComponentTestFixture
    {
        [Fact]
        public void AddOnceEmpty()
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>());
            cut.MarkupMatches("");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUri(string uri)
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.js""></script><script src=""{uri}"" type=""text/javascript""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncEnabled(string uri)
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri), (nameof(Script.Async), true)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.js""></script><script src=""{uri}"" async type=""text/javascript""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithDeferEnabled(string uri)
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri), (nameof(Script.Defer), true)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.js""></script><script src=""{uri}"" defer type=""text/javascript""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncAndDeferEnabled(string uri)
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri), (nameof(Script.Async), true), (nameof(Script.Defer), true)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.js""></script><script src=""{uri}"" async defer type=""text/javascript""></script>");
        }
        [InlineData("_content/My.Component.Library\".Package/code.min.js")]
        [Theory]
        public void InvalidUri(string uri)
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            NUnit.Framework.Assert.Throws<InvalidOperationException>(() => _ = RenderComponent<Script>((nameof(Script.Src), uri)));
        }
        [Fact]
        public void AddTwiceValidUri()
        {
            Services.AddScriptInjection(onload_notification: false);
            var js = Services.AddMockJsRuntime();
            IRenderedComponent<MultipleScriptContainer> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<MultipleScriptContainer>());
            cut.MarkupMatches($@"
<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.js""></script>
<script src=""hello.js"" type=""text/javascript""></script>
<script src=""world.js"" type=""text/javascript"" async></script>");
        }
        [Fact]
        public void FailWithoutScripts()
        {
            Assert.Throws<InvalidOperationException>(() => RenderComponent<Script>());
        }
    }
}
