using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(Src) || !Uri.IsWellFormedUriString(Src, UriKind.RelativeOrAbsolute))
            {
                return;
            }
            if (ScriptInjectionTracker.NeedsInjection("self"))
            {
                builder.AddMarkupContent(0, @"<script type=""text/javascript"" src=""_content/Excubo.Blazor.ScriptInjection/bootstrap.js""></script>");
            }
            if (ScriptInjectionTracker.NeedsInjection(Src))
            {
                const string type = "text/javascript";
                builder.OpenElement(1, "script");
                builder.AddAttribute(2, "src", Src);
                if (Async)
                {
                    builder.AddAttribute(3, "async", true);
                }
                if (Defer)
                {
                    builder.AddAttribute(4, "defer", true);
                }
                builder.AddAttribute(5, "type", type);
                if (ScriptInjectionTracker.OnloadNotification)
                {
                    builder.AddAttribute(6, "onload", $"window.Excubo.ScriptInjection.Notify('{Src}')");
                }
                builder.CloseElement();
            }
        }
        [Inject] private IJSRuntime js { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && ScriptInjectionTracker.OnloadNotification && !ScriptInjectionTracker.Initialized)
            {
                ScriptInjectionTracker.Initialized = true;
                while (!await js.InvokeAsync<bool>("hasOwnProperty", "Excubo"))
                {
                    await Task.Delay(10);
                }
                while (!await js.InvokeAsync<bool>("Excubo.hasOwnProperty", "ScriptInjection"))
                {
                    await Task.Delay(10);
                }
                await js.InvokeVoidAsync("Excubo.ScriptInjection.Register", DotNetObjectReference.Create(ScriptInjectionTracker));
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
