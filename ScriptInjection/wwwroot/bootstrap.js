window.Excubo = window.Excubo || {};
window.Excubo.ScriptInjection = window.Excubo.ScriptInjection || {
    Notify: (e) => {
        const si = window.Excubo.ScriptInjection;
        if (si.Reference == undefined) {
            si.Loaded = si.Loaded || new Set();
            si.Loaded.add(e);
        } else {
            si.Reference.invokeMethodAsync('Loaded', e);
        }
    },
    Register: (dotnet_reference) => {
        const si = window.Excubo.ScriptInjection;
        si.Reference = dotnet_reference;
        if (si.Loaded != undefined) {
            for (let item of si.Loaded) {
                dotnet_reference.invokeMethodAsync('Loaded', item);
            }
            console.log(JSON.stringify(si.Loaded) + ' notified with delay');
        }
    }
};