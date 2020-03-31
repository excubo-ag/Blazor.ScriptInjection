## Excubo.Blazor.ScriptInjection

![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.ScriptInjection)
![Nuget](https://img.shields.io/nuget/dt/Excubo.Blazor.ScriptInjection)
![GitHub](https://img.shields.io/github/license/excubo-ag/Blazor.ScriptInjection)

This library brings the `script` tag to Blazor. Since components can occur any number of times, the usage of `script` tags in components is usually frowned upon. The `Script` component in `Excubo.Blazor.ScriptInjection` is different, as it makes sure that the source file is only put into the page's body once.

This can be used to lazily load javascript sources for any component that requires javascript. As a component library author, you can use this to relieve your users of the burden of adding script tags to their pages' `<head>`. Simply add `<Script Src="_content/My.Library/code.js">` to any component in your library that requires that file. `Excubo.Blazor.ScriptInjection` makes sure the script gets loaded once and only once, regardless of how many components you have, and regardless of how many components your users use.

### Usage

#### 1. Add ScriptInjection to your services

```cs
    //...
    //using Excubo.Blazor.ScriptInjection;
    services.AddScriptInjection();
    //...
```

#### 2. Use the Script component

```html
<h3>My component requiring some js</h3>
<Script Src="path/to/code.js" Async="true" Defer="false" />
<!--...-->
```

Note: `Async` and `Defer` are `false` by default. It is recommended to use these options wherever possible to improve page performance. 

### Recommendations for component library authors

To make use of this library in your component library, there are two easy steps to follow (a third one being optional):

#### 1. Add ScriptInjection to your service extension method

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

#### 2. Use the Script component

Make sure you use the `_content/[Name of Library]` prefix in the `Src` parameter.

```html
<h3>My reusable component in an awesome library (requiring some js)</h3>
<Script Src="_content/MyLibrary/path/to/code.js" Async="true" Defer="false" />
<!--...-->
```

#### 3. Optionally, tell your users that they can remove the awkward script tag from their `_Host.cshtml` / `index.html` file.

Now that your making sure that the javascript code is loaded only when it's needed, you can tell your users that they can remove some clutter from their `_Host.cshtml` / `index.html` file. This is only relevant for updated libraries, not when you use `Excubo.Blazor.ScriptInjection` from version 1 of your component library.
