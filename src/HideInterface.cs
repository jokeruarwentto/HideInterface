using System.IO;
using HideInterface.Helpers;
using UnityEngine;

namespace HideInterface;

public class HideInterface : MonoBehaviour
{
    /// <summary>
    ///     Most of the variables of this class.
    /// </summary>
    public static HideInterface Instance;

    private bool ConfigurationFileChanged;

    private FileSystemWatcher FileWatcher;
    private GameObject GameInterface;
    private GameObject VersionWatermark;

    /// <summary>
    ///     The Awake function from MonoBehaviour, an in-built Unity class,
    ///     this is here that we prepare all the data we need for HideInterface.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        InitializeFileWatcher();
    }

    /// <summary>
    ///     The Update function from MonoBehaviour, an in-built Unity class,
    ///     this is here that we are looking for input and hiding interface.
    /// </summary>
    private void Update()
    {
        if (ConfigurationFileChanged)
            if (!PluginConfiguration.Get().Enabled)
                Plugin.Logger.LogInfo("Do something there.");

        if (GameInterface == null && GameObject.Find("HUDCanvas(Clone)") is var _GameInterface)
            GameInterface = _GameInterface;

        if (PluginConfiguration.Get().IncludeVersionWatermark)
            if (VersionWatermark == null && GameObject.Find("VersionString") is var _VersionWatermark)
                VersionWatermark = _VersionWatermark;

        if (Input.GetKeyDown(PluginConfiguration.Get().KeyCode))
        {
            if (VersionWatermark != null) VersionWatermark.SetActive(!VersionWatermark.active);
            if (GameInterface != null) GameInterface.SetActive(!GameInterface.active);
        }
    }

    /// <summary>
    ///     This is the system we need to hot reload our configuration file.
    /// </summary>
    private void InitializeFileWatcher()
    {
        FileWatcher = new FileSystemWatcher(Plugin.PluginPath)
        {
            NotifyFilter = NotifyFilters.LastWrite,
            Filter = Plugin.PluginConfigurationFileName
        };

        FileWatcher.Changed += (_, _) =>
        {
            PluginConfiguration.Read();
            ConfigurationFileChanged = true;
        };

        FileWatcher.EnableRaisingEvents = true;

#if DEBUG
        Plugin.Logger.LogInfo(
            $"FileWatcher now watch [{Path.Combine(Plugin.PluginPath, Plugin.PluginConfigurationFileName)}].");
#endif
    }
}