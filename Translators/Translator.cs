using System;
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
    private Dictionary<string, string> _lookup = new();
    internal Status TranslateStatus = Status.Idling;

    public Translator(Mod mod) {
        Mod = mod;
    }

    public abstract void Translate(string text, string targetLang, Action<string> finishedCallback);

    public void SetLookupRaw(Dictionary<string, string> lookup) => _lookup = lookup;

    public Dictionary<string, string> GetLookupRaw() => _lookup;

    public void SetLookupValue(string key, string value) => _lookup[$"{Core.TargetLang}-{key}"] = value;

    public string GetLookupValue(string key) => _lookup[$"{Core.TargetLang}-{key}"];

    public bool TryGetLookupValue(string key, out string value) =>
        _lookup.TryGetValue($"{Core.TargetLang}-{key}", out value);
}