using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Excubo.Blazor.ScriptInjection
{
    public class AddScript : ComponentBase
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
            if (ScriptInjectionTracker.OnloadNotification && ScriptInjectionTracker.NeedsInjection("self"))
            {
                var bootstrap_src = ScriptInjectionTracker.GzippedBootstrap ? "bootstrap.min.js.gz" : "bootstrap.min.js";
                builder.OpenElement(0, "script");
                builder.AddAttribute(1, "src", $"_content/Excubo.Blazor.ScriptInjection/{bootstrap_src}");
                builder.AddAttribute(2, "type", "text/javascript");
                builder.CloseElement();
            }
            if (ScriptInjectionTracker.NeedsInjection(Src))
            {
                const string type = "text/javascript";
                builder.OpenElement(3, "script");
                builder.AddAttribute(4, "src", Src);
                if (Async)
                {
                    builder.AddAttribute(5, "async", true);
                }
                if (Defer)
                {
                    builder.AddAttribute(6, "defer", true);
                }
                builder.AddAttribute(7, "type", type);
                if (ScriptInjectionTracker.OnloadNotification)
                {
                    builder.AddAttribute(8, "onload", $"window.Excubo.ScriptInjection.Notify('{Src}')");
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
                while (!await js.InvokeAsync<bool>("window.hasOwnProperty", "Excubo"))
                {
                    await Task.Delay(10);
                }
                while (!await js.InvokeAsync<bool>("Excubo.hasOwnProperty", "ScriptInjection"))
                {
                    await Task.Delay(10);
                }
            }
            await js.InvokeVoidAsync("Excubo.ScriptInjection.Register", DotNetObjectReference.Create(ScriptInjectionTracker));
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}