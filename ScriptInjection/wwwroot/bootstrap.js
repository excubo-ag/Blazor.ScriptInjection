window.Excubo = window.Excubo || {};
window.Excubo.ScriptInjection = window.Excubo.ScriptInjection || {
    Notify: (e) => {
        const si = window.Excubo.ScriptInjection;
        if (si.Reference == undefined) {
            si.Loaded = si.Loaded || new Set();
            si.Loaded.add(e);
        } else {
            si.Reference.invokeMethodAsync('MarkLoaded', e);
        }
    },
    Register: (dotnet_reference) => {
        const si = window.Excubo.ScriptInjection;
        si.Reference = dotnet_reference;
        if (si.Loaded != undefined) {
            for (let item of si.Loaded) {
                dotnet_reference.invokeMethodAsync('MarkLoaded', item);
            }
        }
    }
};