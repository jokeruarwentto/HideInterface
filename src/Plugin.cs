using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using UnhollowerRuntimeLib;
using Wetstone.API;
using Object = UnityEngine.Object;

namespace HideInterface;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("xyz.molenzwiebel.wetstone")]
[Reloadable]
public class Plugin : BasePlugin
{
    /// <summary>
    ///     Most of the variables we need for the plugin.
    /// </summary>
    public static ManualLogSource Logger;

    public static ConfigEntry<bool> Enabled;
    public static ConfigEntry<bool> IncludeVersionWatermark;

    /// <summary>
    ///     The initialization of the plugin.
    /// </summary>
    public Plugin()
    {
        Logger = Log;
        Enabled = Config.Bind("General", "Enabled", true, "Whether to enable this plugin.");
        IncludeVersionWatermark = Config.Bind("General", "IncludeVersionWatermark", true, "Hide the version watermark at the bottom right of the screen when toggling interface.");
    }

    /// <summary>
    ///     The loading method from BasePlugin of BepInEx.
    /// </summary>
    public override void Load()
    {
        try
        {
            Log.LogInfo($"Loading [{PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}]");

            Register();

            Log.LogInfo($"Loaded successfully [{PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}]");
        }
        catch (Exception exception)
        {
            Log.LogError($"Something went wrong during \"{PluginInfo.PLUGIN_NAME}\" loading...");
            Log.LogError(exception.Message);
        }
    }

    /// <summary>
    ///     Registering the HideInterface class, and add it has component of the BepInEx plugin.
    /// </summary>
    private void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<HideInterface>();
        AddComponent<HideInterface>();
    }

    /// <summary>
    ///     The unloading method from BasePlugin of BepInEx.
    /// </summary>
    public override bool Unload()
    {
        Object.Destroy(HideInterface.Instance);
        return true;
    }
}