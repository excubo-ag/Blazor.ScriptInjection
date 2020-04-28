using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace Excubo.Blazor.ScriptInjection
{
    public class Script : ComponentBase
    {
        [Inject]
        private IScriptInjectionTracker ScriptInjectionTracker { get; set; }
        private string src;
        [Parameter]
        public string Src
        {
            get => src;
            set => src = Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute) ? value : throw new ArgumentException("Invalid URI");
        }
        [Parameter]
        public bool Async { get; set; }
        [Parameter]
        public bool Defer { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ScriptInjectionTracker.NeedsInjection(Src))
            {
                var async_attribute = Async ? "async" : "";
                var defer_attribute = Defer ? "defer" : "";
                const string type = "text/javascript";
                builder.AddMarkupContent(0, $"<script src=\"{Src}\" {async_attribute} {defer_attribute} type=\"{type}\"></script>");
            }
            base.BuildRenderTree(builder);
        }
    }
}
