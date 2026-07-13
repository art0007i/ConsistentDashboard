using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.NET.Common;
using BepInExResoniteShim;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;

namespace ConsistentDashboard;

[ResonitePlugin(PluginMetadata.GUID, PluginMetadata.NAME, PluginMetadata.VERSION, PluginMetadata.AUTHORS, PluginMetadata.REPOSITORY_URL)]
[BepInDependency(BepInExResoniteShim.PluginMetadata.GUID, BepInDependency.DependencyFlags.HardDependency)]
public class Plugin : BasePlugin
{
#nullable disable
    internal static new ManualLogSource Log;
    internal static ConfigEntry<bool> Enabled;
    internal static ConfigEntry<int2> Resolution;
#nullable enable

    public override void Load()
    {
        Log = base.Log;

        Enabled = Config.Bind("General", "Enabled", true,
            "When enabled the mod will set the dashboard size to a set resolution.");
        Resolution = Config.Bind("General", "Resolution", new int2(2560, 1440),
            "<color=orange>⚠Messing with this option may result in brekaing your dash.⚠</color> The resolution the dashboard will be set to while the mod is enabled.");
        
        HarmonyInstance.PatchAll();
    }
    
    [HarmonyPatch(typeof(RadiantDash), nameof(RadiantDash.AutoAdaptWidth), MethodType.Getter)]
    class ConsistentDashboardPatch
    {
        public static bool Prefix(ref bool __result)
        {
            if (!Enabled.Value) return true;
            __result = false;
            return false;
        }
    }
    [HarmonyPatch(typeof(RadiantDash), nameof(RadiantDash.BASE_RESOLUTION), MethodType.Getter)]
    class ConsistentDashboardResolutionPatch
    {
        public static bool Prefix(ref int2 __result)
        {
            if (!Enabled.Value) return true;
            __result = Resolution.Value;
            return false;
        }
    }
}
