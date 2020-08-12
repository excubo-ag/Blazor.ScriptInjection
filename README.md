# Excubo.Blazor.ScriptInjection

![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.ScriptInjection)
![Nuget](https://img.shields.io/nuget/dt/Excubo.Blazor.ScriptInjection)
![GitHub](https://img.shields.io/github/license/excubo-ag/Blazor.ScriptInjection)

This library brings the `script` tag to Blazor. Since components can occur any number of times, the usage of `script` tags in components is usually frowned upon.
The `AddScript` component in `Excubo.Blazor.ScriptInjection` is different, as it makes sure that the source file is only put into the page's body once.

This can be used to lazily load javascript sources for any component that requires javascript.
As a component library author, you can use this to relieve your users of the burden of adding script tags to their pages' `<head>`.
Simply add `<AddScript Src="_content/My.Library/code.js" />` to any component in your library that requires that file.
`Excubo.Blazor.ScriptInjection` makes sure the script gets loaded once and only once, regardless of how many components you have, and regardless of how many components your users use.

## Changelog

### Version 3.X.Y

Since `<Script />` fails in Blazor under some circumstances ([aspnetcore#24159](https://github.com/dotnet/aspnetcore/issues/24159)), `Script` is now deprecated in favor of `AddScript`.
`Script` will be removed starting with v4.0.0.

### Version 2.X.Y

Since version 2.0.0, you get a utility function that only returns when the script has been loaded. This makes sure that your javascript code is available before you try to execute it.
This behavior is optional (but enabled by default). If you do not need it at all, simply disable it when you add the `ScriptInjection` service.
This notification adds a payload of 409 bytes to your page (214 bytes, if your web server supports gzipped files and you activate the gzipped option on the service: `services.AddScriptInjection(gzipped_bootstrap: true)`).

## Usage

### 1. Install the nuget package Excubo.Blazor.ScriptInjection

Excubo.Blazor.ScriptInjection is distributed [via nuget.org](https://www.nuget.org/packages/Excubo.Blazor.ScriptInjection/).
![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.ScriptInjection)

#### Package Manager:
```ps
Install-Package Excubo.Blazor.ScriptInjection -Version 3.0.1
```

#### .NET Cli:
```cmd
dotnet add package Excubo.Blazor.ScriptInjection --version 3.0.1
```

#### Package Reference
```xml
<PackageReference Include="Excubo.Blazor.ScriptInjection" Version="3.0.1" />
```

### 2. Add ScriptInjection to your services

```cs
    //...
    //using Excubo.Blazor.ScriptInjection;
    services.AddScriptInjection(); // Tip: Use Excubo.Analyzers.DependencyInjectionValidation for warnings when you forget such a dependency
    // if your web server supports gzipped javascript files, it's recommended to use the following instead:
    services.AddScriptInjection(gzipped_bootstrap: true);
    // Alternative: if you do not require notification on load of the scripts, disable it completely:
    services.AddScriptInjection(onload_notification: false); // default is true.
    //...
```

### 3. Use the AddScript component

```html
<h3>My component requiring some js</h3>
<AddScript Src="path/to/code.js" Async="true" Defer="false" />
<!--...-->
```

Note: `Async` and `Defer` are `false` by default. It is recommended to use these options wherever possible to improve page performance.

### 4. Wait for the script to be loaded (optional)

Most likely, you need to be sure that the script you added is actually loaded before calling any method in it:

```html
@inject IJSRuntime js
@inject IScriptInjectionTracker script_injection_tracker
<AddScript Src="path/to/code.js" Async="true" Defer="false" />

<button @onclick="Run">Execute code in path/to/code.js</button>

@code {
    private async Task Run()
    {
        await script_injection_tracker.LoadedAsync("path/to/code.js");
        await js.InvokeVoidAsync("console.log", "we know that our file has been loaded successfully! now we execute something");
        await js.InvokeVoidAsync("code.sayhello", "world");
    }
}
```

If you never require to call `IScriptInjectionTracker.LoadedAsync`, then you can safely disable the onload notification with `services.AddScriptInjection(onload_notification: false);`.

## Recommendations for component library authors

To make use of this library in your component library, there are two easy steps to follow (a third one being optional):

### 1. Add ScriptInjection to your service extension method

```cs
using Excubo.Blazor.ScriptInjection;
using Microsoft.Extensions.DependencyInjection;

namespace MyLibrary
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddMyLibrary(this IServiceCollection services)
        {
            return services
                //... add your own services here
                .AddScriptInjection();
        }
    }
}
```

### 2. Use the AddScript component

Make sure you use the `_content/[Name of Library]` prefix in the `Src` parameter.

```html
<h3>My reusable component in an awesome library (requiring some js)</h3>
<AddScript Src="_content/MyLibrary/path/to/code.js" Async="true" Defer="false" />
<!--...-->
```

### 3. Optionally, tell your users that they can remove the awkward script tag from their `_Host.cshtml` / `index.html` file.

Now that your making sure that the javascript code is loaded only when it's needed, you can tell your users that they can remove some clutter from their `_Host.cshtml` / `index.html` file.
This is only relevant for updated libraries, not when you use `Excubo.Blazor.ScriptInjection` from version 1 of your component library.
