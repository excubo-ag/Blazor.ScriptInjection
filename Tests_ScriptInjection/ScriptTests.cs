using Bunit;
using Excubo.Blazor.ScriptInjection;
using System;
using Xunit;

namespace Tests_ScriptInjection
{
    public class ScriptTests : ComponentTestFixture
    {
        [Fact]
        public void AddOnceEmpty()
        {
            Services.AddScriptInjection();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>());
            cut.MarkupMatches("<script src=\"\" type=\"text/javascript\"></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUri(string uri)
        {
            Services.AddScriptInjection();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri)));
            cut.MarkupMatches($"<script src=\"{uri}\" type=\"text/javascript\"></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncEnabled(string uri)
        {
            Services.AddScriptInjection();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri), (nameof(Script.Async), true)));
            cut.MarkupMatches($"<script src=\"{uri}\" async type=\"text/javascript\"></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithDeferEnabled(string uri)
        {
            Services.AddScriptInjection();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri), (nameof(Script.Defer), true)));
            cut.MarkupMatches($"<script src=\"{uri}\" defer type=\"text/javascript\"></script>");
        }
        [InlineData("code.js")]
        [InlineData("_content/My.Component.Library.Package/code.min.js")]
        [Theory]
        public void AddOnceValidUriWithAsyncAndDeferEnabled(string uri)
        {
            Services.AddScriptInjection();
            IRenderedComponent<Script> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<Script>((nameof(Script.Src), uri), (nameof(Script.Async), true), (nameof(Script.Defer), true)));
            cut.MarkupMatches($"<script src=\"{uri}\" async defer type=\"text/javascript\"></script>");
        }
        [InlineData("_content/My.Component.Library\".Package/code.min.js")]
        [Theory]
        public void InvalidUri(string uri)
        {
            Services.AddScriptInjection();
            NUnit.Framework.Assert.Throws<InvalidOperationException>(() => _ = RenderComponent<Script>((nameof(Script.Src), uri)));
        }
        [Fact]
        public void AddTwiceValidUri()
        {
            Services.AddScriptInjection();
            IRenderedComponent<MultipleScriptContainer> cut = null;
            NUnit.Framework.Assert.DoesNotThrow(() => cut = RenderComponent<MultipleScriptContainer>());
            cut.MarkupMatches($"<script src=\"hello.js\" type=\"text/javascript\"></script>");
        }
        [Fact]
        public void FailWithoutScripts()
        {
            Assert.Throws<InvalidOperationException>(() => RenderComponent<Script>());
        }
    }
}
