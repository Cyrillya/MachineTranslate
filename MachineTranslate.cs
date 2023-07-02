using Terraria.ModLoader;

namespace MachineTranslate;

public class MachineTranslate : Mod
{
    internal static ModKeybind TranslateKeybind;

    public override void Load() {
        TranslateKeybind = KeybindLoader.RegisterKeybind(this, "Translate", "T");
    }

    public override void Unload() {
        TranslateKeybind = null;
    }
}