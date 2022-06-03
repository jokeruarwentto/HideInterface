using UnityEngine;
using Wetstone.API;

namespace HideInterface;

public class HideInterface : MonoBehaviour
{
    /// <summary>
    ///     Most of the variables of this class.
    /// </summary>
    public static HideInterface Instance;

    private GameObject GameInterface;
    private Keybinding ToggleInterfaceKeybind;
    private GameObject VersionWatermark;

    /// <summary>
    ///     The Awake function from MonoBehaviour, an in-built Unity class,
    ///     this is here that we prepare all the data we need for HideInterface.
    /// </summary>
    private void Awake()
    {
        Instance = this;

        ToggleInterfaceKeybind = KeybindManager.Register(new KeybindingDescription
        {
            Id = "me.arwent.HideInterface",
            Category = "Arwent Cauro's Projects",
            Name = "Toggle Interface",
            DefaultKeybinding = KeyCode.F11
        });
    }

    /// <summary>
    ///     The Update function from MonoBehaviour, an in-built Unity class,
    ///     this is here that we are looking for input and hiding interface.
    /// </summary>
    private void Update()
    {
        if (!Plugin.Enabled.Value)
                return;

        if (GameInterface == null && GameObject.Find("HUDCanvas(Clone)") is var _GameInterface)
            GameInterface = _GameInterface;

        if (Plugin.IncludeVersionWatermark.Value)
            if (VersionWatermark == null && GameObject.Find("VersionString") is var _VersionWatermark)
                VersionWatermark = _VersionWatermark;

        if (Input.GetKeyDown(ToggleInterfaceKeybind.Primary) || Input.GetKeyDown(ToggleInterfaceKeybind.Secondary))
        {
            if (VersionWatermark != null) VersionWatermark.SetActive(!VersionWatermark.active);
            if (GameInterface != null) GameInterface.SetActive(!GameInterface.active);
        }
    }
}