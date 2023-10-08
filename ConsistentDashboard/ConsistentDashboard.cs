using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using FrooxEngine;
using Elements.Core;

namespace ConsistentDashboard
{
    public class ConsistentDashboard : ResoniteMod
    {
        public override string Name => "ConsistentDashboard";
        public override string Author => "art0007i";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/art0007i/ConsistentDashboard/";

        [AutoRegisterConfigKey]
        public static ModConfigurationKey<bool> KEY_ENABLE = new("enable", "When enabled the mod will set the dashboard size to a set resolution.", () => true);
        [AutoRegisterConfigKey]
        public static ModConfigurationKey<int2> KEY_RESOLUTION = new("resolution", "<color=orange>⚠Messing with this option may result in brekaing your dash.⚠</color> The resolution the dashboard will be set to while the mod is enabled.", () => new(2560, 1440));

        public static ModConfiguration config;

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            Harmony harmony = new Harmony("me.art0007i.ConsistentDashboard");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(RadiantDash), nameof(RadiantDash.AutoAdaptWidth), MethodType.Getter)]
        class ConsistentDashboardPatch
        {
            public static bool Prefix(ref bool __result)
            {
                if (!config.GetValue(KEY_ENABLE)) return true;
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(RadiantDash), nameof(RadiantDash.BASE_RESOLUTION), MethodType.Getter)]
        class ConsistentDashboardResolutionPatch
        {
            public static bool Prefix(ref int2 __result)
            {
                if (!config.GetValue(KEY_ENABLE)) return true;
                __result = config.GetValue(KEY_RESOLUTION);
                return false;
            }
        }
    }
}