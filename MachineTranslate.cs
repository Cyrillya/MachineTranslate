using Terraria.ModLoader;

namespace MachineTranslate;

public class MachineTranslate : Mod
{
    internal static ModKeybind TranslateKeybind;
    internal static ModKeybind OpenWebsiteKeybind;

    public override void Load() {
        TranslateKeybind = KeybindLoader.RegisterKeybind(this, "Translate", "T");
        OpenWebsiteKeybind = KeybindLoader.RegisterKeybind(this, "OpenWebsite", "OemComma");
    }

    public override void Unload() {
        TranslateKeybind = null;
        OpenWebsiteKeybind = null;
    }
}