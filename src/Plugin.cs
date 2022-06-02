using System;
using System.IO;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HideInterface.Helpers;
using UnhollowerRuntimeLib;

namespace HideInterface;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    /// <summary>
    ///     Most of the variables we need for the plugin.
    /// </summary>
    public static ManualLogSource Logger;

    public static string PluginPath;
    public static string PluginConfigurationFileName;

    /// <summary>
    ///     The initialization of the plugin.
    /// </summary>
    public Plugin()
    {
        Logger = Log;
        PluginPath = Path.GetDirectoryName(typeof(Plugin).Assembly.Location);
        PluginConfigurationFileName = $"{PluginInfo.PLUGIN_GUID}.Configuration.xml";
    }

    /// <summary>
    ///     The loading method from BasePlugin of BepInEx.
    /// </summary>
    public override void Load()
    {
        try
        {
            Log.LogInfo($"Loading [{PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}] from [{PluginPath}]");

            PluginConfiguration.Read();
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
}