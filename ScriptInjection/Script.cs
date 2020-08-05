using System;

namespace Excubo.Blazor.ScriptInjection
{
    [Obsolete("Starting with V3.0.0, <Script></Script> is considered deprecated, as using it as <Script /> " +
        "is not working correctly (https://github.com/dotnet/aspnetcore/issues/24159)." +
        "Use <AddScript /> instead." +
        "This component is scheduled to be removed starting with V4.0.0.")]
    public class Script : AddScript
    {
    }
}
