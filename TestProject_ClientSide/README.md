# Excubo.Blazor.ScriptInjection in Blazor ClientSide (wasm) projects

In this example project, we want to use some code in `wwwroot/lazy.js` in our index page.

Usage is absolutely straightforward:

## 1. Added script injection to `Program.cs`

```cs
//...
//using Excubo.Blazor.ScriptInjection;
builder.Services.AddScriptInjection();
//...
```

## 2. Added the file `lazy.js` to `wwwroot`

```js
window.helloworld = { again: () => { console.log("hello world"); } };
```

## 3. Write our index page:

```html
@page "/"
@using Excubo.Blazor.ScriptInjection
<!--to make sure our script is loaded:-->
@inject IScriptInjectionTracker sit
<!--to make the actual call to js:-->
@inject IJSRuntime js
<!--the script we want-->
<Script Src="lazy.js" />

<button @onclick="Run">Execute code in lazy.js</button>

@code {
    private async Task Run()
    {
        // wait until load completed
        await sit.LoadedAsync("lazy.js");
        await js.InvokeVoidAsync("console.log", "foo");
        // execute code we saw above
        await js.InvokeVoidAsync("helloworld.again");
    }
}
```
