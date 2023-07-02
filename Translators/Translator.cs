using System.Collections.Generic;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

public abstract class Translator
{
    public enum Status
    {
        Idling,
        Translating,
        AttemptFailed
    }
    
    public Mod Mod;
    internal Dictionary<string, string> Lookup = new();
    internal Status TranslateStatus = Status.Idling;

    public Translator(Mod mod) {
        Mod = mod;
    }

    public abstract void Translate(string text, string targetLang);
}