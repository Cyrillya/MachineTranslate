using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MachineTranslate.Contents;

public class SaveKeyValueLookup : ModSystem
{
    private const string FileName = "MachineTranslate_CachedContent.json";
    public static readonly string FullPath = Path.Combine(ConfigManager.ModConfigPath, FileName);

    public override void OnModLoad() {
        Task.Run(LoadLookup);
    }

    public override void OnModUnload() {
        SaveLookup();
    }

    public override void OnWorldUnload() {
        Task.Run(SaveLookup);
    }

    private static Action LoadLookup => () => {
        bool jsonFileExists = File.Exists(FullPath);
        string json = jsonFileExists
            ? File.ReadAllText(FullPath)
            : "{\"Baidu\": {},\"Bing\": {},\"Google\": {},\"Caiyun\": {},\"CaiyunNoToken\": {},\"Tencent\": {},\"Volcengine\": {}}";

        try {
            var apiResultsLookup = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            foreach (var (api, lookup) in apiResultsLookup) {
                if (Core.TranslatorLookup.TryGetValue(Enum.Parse<Config.TransApi>(api), out var translator)) {
                    translator.SetLookupRaw(lookup);
                }
            }
        }
        catch (Exception e) when (jsonFileExists && e is JsonReaderException or JsonSerializationException) {
            File.Delete(FullPath);
        }
    };

    internal static Action SaveLookup => () => {
        var apiResultsLookup = new Dictionary<string, Dictionary<string, string>>();
        foreach (var (api, translator) in Core.TranslatorLookup) {
            apiResultsLookup.Add(Enum.GetName(api) ?? throw new InvalidOperationException(), translator.GetLookupRaw());
        }

        Directory.CreateDirectory(ConfigManager.ModConfigPath);
        string json = JsonConvert.SerializeObject(apiResultsLookup);
        File.WriteAllText(FullPath, json);
    };
}