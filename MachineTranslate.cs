using Terraria.ModLoader;

namespace MachineTranslate;

public class MachineTranslate : Mod
{
    internal static ModKeybind TranslateKeybind;
    internal static ModKeybind OpenWebsiteKeybind;

    public override void Load() {
        TranslateKeybind = KeybindLoader.RegisterKeybind(this, "Translate (翻译)", "T");
        OpenWebsiteKeybind = KeybindLoader.RegisterKeybind(this, "Open Website (打开网站)", "OemComma");
    }

    public override void Unload() {
        TranslateKeybind = null;
        OpenWebsiteKeybind = null;
    }
}