using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace HideInterface.Helpers;

public struct PluginConfigurationData
{
    [XmlAnyElement("EnabledComment")]
    public XmlComment EnabledComment
    {
        get => new XmlDocument().CreateComment(
            "Turn this to false to disable mod, otherwise use true to enable it.");
        set { }
    }

    public bool Enabled { get; set; } = true;

    [XmlAnyElement("KeyCodeComment")]
    public XmlComment KeyCodeComment
    {
        get => new XmlDocument().CreateComment(
            "The key you wish for toggling your interface.");
        set { }
    }

    [XmlAnyElement("KeyCodeComment2")]
    public XmlComment KeyCodeComment2
    {
        get => new XmlDocument().CreateComment(
            "Please refers to https://docs.unity3d.com/ScriptReference/KeyCode.html Properties to get correct KeyCode.");
        set { }
    }

    public KeyCode KeyCode { get; set; } = KeyCode.F11;

    [XmlAnyElement("IncludeVersionWatermarkComment")]
    public XmlComment IncludeVersionWatermarkComment
    {
        get => new XmlDocument().CreateComment(
            "If you want to hide version watermark too at the bottom right with your game interface.");
        set { }
    }

    public bool IncludeVersionWatermark { get; set; } = false;

    public PluginConfigurationData()
    {
    }
}

public static class PluginConfiguration
{
    /// <summary>
    ///     Most of the variables of this class.
    ///     Data => The configuration data of the plugin that we read with Read() method.
    /// </summary>
    private static PluginConfigurationData Data { get; set; }

    /// <summary>
    ///     Create configuration file of the plugin using default settings.
    /// </summary>
    public static void Initialize()
    {
        var serializer = new XmlSerializer(typeof(PluginConfigurationData));
        using var writer = new StreamWriter(Path.Combine(Plugin.PluginPath, Plugin.PluginConfigurationFileName));
        serializer.Serialize(writer, new PluginConfigurationData());
    }

    /// <summary>
    ///     Check if the configuration file does exists.
    /// </summary>
    /// <returns>A boolean if the configuration file exists.</returns>
    public static bool Exists()
    {
        return File.Exists(Path.Combine(Plugin.PluginPath, Plugin.PluginConfigurationFileName));
    }

    /// <summary>
    ///     Read the plugin configuration file.
    ///     If not existing, then it initializing it.
    /// </summary>
    public static void Read()
    {
        if (!Exists())
        {
            Plugin.Logger.LogWarning(
                $"No configuration file found at [{Path.Combine(Plugin.PluginPath, Plugin.PluginConfigurationFileName)}], initializing it with default settings.");
            Initialize();
        }

        var serializer = new XmlSerializer(typeof(PluginConfigurationData));
        using var reader = new FileStream(Path.Combine(Plugin.PluginPath, Plugin.PluginConfigurationFileName),
            FileMode.Open);
        Data = (PluginConfigurationData) serializer.Deserialize(reader);
    }

    /// <summary>
    ///     Gets the loaded PluginConfigurationData using Read() method.
    /// </summary>
    /// <returns>A PluginConfigurationData object.</returns>
    public static PluginConfigurationData Get()
    {
        return Data;
    }
}