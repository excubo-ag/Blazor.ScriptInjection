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
            set => src = System.Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute) ? value : throw new ArgumentException("Invalid URI");
        }
        [Parameter]
        public bool Async { get; set; }
        [Parameter]
        public bool Defer { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ScriptInjectionTracker.NeedsInjection(Src))
            {
                builder.AddMarkupContent(0, $"<script src=\"{Src}\" {(Async ? "async" : "")} {(Defer ? "defer" : "")} type=\"text/javascript\"></script>");
            }
            base.BuildRenderTree(builder);
        }
    }
}
