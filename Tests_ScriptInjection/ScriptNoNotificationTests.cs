using Bunit;
using Bunit.JSInterop;
using Excubo.Blazor.ScriptInjection;
using System;
using Xunit;

namespace Excubo.Blazor.Tests_ScriptInjection
{
    public class ScriptNoNotificationTests : TestContext
    {
        [Fact]
        public void AddOnceEmpty()
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>());
            cut.MarkupMatches("");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUri(string uri)
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri)));
            cut.MarkupMatches($@"<script src=""{uri}"" type=""text/javascript""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncEnabled(string uri)
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri), (nameof(AddScript.Async), true)));
            cut.MarkupMatches($@"<script src=""{uri}"" async type=""text/javascript""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithDeferEnabled(string uri)
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri), (nameof(AddScript.Defer), true)));
            cut.MarkupMatches($@"<script src=""{uri}"" defer type=""text/javascript""></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncAndDeferEnabled(string uri)
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            IRenderedComponent<AddScript> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<AddScript>((nameof(AddScript.Src), uri), (nameof(AddScript.Async), true), (nameof(AddScript.Defer), true)));
            cut.MarkupMatches($@"<script src=""{uri}"" async defer type=""text/javascript""></script>");
        }
        [InlineData("_content/My.Component.Library\".Package/code.min.js")]
        [Theory]
        public void InvalidUri(string uri)
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            _ = NUnit.Framework.Assert.Throws<InvalidOperationException>(() => _ = RenderComponent<AddScript>((nameof(AddScript.Src), uri)));
        }
        [Fact]
        public void AddTwiceValidUri()
        {
            _ = Services.AddScriptInjection(onload_notification: false);
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            IRenderedComponent<MultipleScriptContainer> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<MultipleScriptContainer>());
            cut.MarkupMatches($@"
<script src=""hello.js"" type=""text/javascript""></script>
<script src=""world.js"" type=""text/javascript"" async></script>");
        }
        [Fact]
        public void FailWithoutScripts()
        {
            JSInterop.Setup<bool>("window.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.Setup<bool>("Excubo.hasOwnProperty", (_) => true).SetResult(true);
            JSInterop.SetupVoid("Excubo.ScriptInjection.Register", (_) => true);
            _ = Assert.Throws<InvalidOperationException>(() => RenderComponent<AddScript>());
        }
    }
}