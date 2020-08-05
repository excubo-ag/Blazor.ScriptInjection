using Bunit;
using Bunit.TestDoubles.JSInterop;
using Excubo.Blazor.ScriptInjection;
using System;
using Xunit;

namespace Excubo.Blazor.Tests_ScriptInjection
{
    public class ScriptNotificationBootstrapGzippedTests : TestContext
    {
        [Fact]
        public void AddOnceEmpty()
        {
            _ = Services.AddScriptInjection(gzipped_bootstrap: true);
            var js = Services.AddMockJSRuntime();
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>());
            cut.MarkupMatches("");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUri(string uri)
        {
            _ = Services.AddScriptInjection(gzipped_bootstrap: true);
            var js = Services.AddMockJSRuntime();
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.min.js.gz""></script><script src=""{uri}"" type=""text/javascript"" onload=""window.Excubo.ScriptInjection.Notify('{uri}')""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncEnabled(string uri)
        {
            _ = Services.AddScriptInjection(gzipped_bootstrap: true);
            var js = Services.AddMockJSRuntime();
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri), (nameof(AddScript.Async), true)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.min.js.gz""></script><script src=""{uri}"" async type=""text/javascript"" onload=""window.Excubo.ScriptInjection.Notify('{uri}')""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithDeferEnabled(string uri)
        {
            _ = Services.AddScriptInjection(gzipped_bootstrap: true);
            var js = Services.AddMockJSRuntime();
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri), (nameof(AddScript.Defer), true)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.min.js.gz""></script><script src=""{uri}"" defer type=""text/javascript"" onload=""window.Excubo.ScriptInjection.Notify('{uri}')""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncAndDeferEnabled(string uri)
        {
            _ = Services.AddScriptInjection(gzipped_bootstrap: true);
            var js = Services.AddMockJSRuntime();
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri), (nameof(AddScript.Async), true), (nameof(AddScript.Defer), true)));
            cut.MarkupMatches($@"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.min.js.gz""></script><script src=""{uri}"" async defer type=""text/javascript"" onload=""window.Excubo.ScriptInjection.Notify('{uri}')""></script>");
        }
        [InlineData("_content/My.Component.Library\".Package/code.min.js")]
        [Theory]
        public void InvalidUri(string uri)
        {
            _ = Services.AddScriptInjection();
            var js = Services.AddMockJSRuntime();
            _ = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => _ = RenderComponent<AddScript>((nameof(AddScript.Src), uri)));
        }
        [Fact]
        public void AddTwiceValidUri()
        {
            _ = Services.AddScriptInjection(gzipped_bootstrap: true);
            var js = Services.AddMockJSRuntime();
            IRenderedComponent<MultipleScriptContainer> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<MultipleScriptContainer>());
            cut.MarkupMatches($@"
<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.min.js.gz""></script>
<script src=""hello.js"" type=""text/javascript"" onload=""window.Excubo.ScriptInjection.Notify('hello.js')""></script>
<script src=""world.js"" type=""text/javascript"" async onload=""window.Excubo.ScriptInjection.Notify('world.js')""></script>");
        }
        [Fact]
        public void FailWithoutScripts()
        {
            _ = Assert.Throws<InvalidOperationException>(() => RenderComponent<AddScript>());
        }
    }
}
