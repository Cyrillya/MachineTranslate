using System;
using System.Collections.Generic;
using MachineTranslate.Contents;
using MachineTranslate.Translators;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MachineTranslate;

public class Core : ModSystem
{
    private static Mod _mod;
    private static string _targetLang;
    internal static Config Config;
    internal static Dictionary<Config.TransApi, Translator> TranslatorLookup;

    internal static string TargetLang =>
        !string.IsNullOrEmpty(Config.CustomTargetLang) ? Config.CustomTargetLang : _targetLang;

    public override void Load() {
        _targetLang = "en";
        _mod = Mod;
        TranslatorLookup = new Dictionary<Config.TransApi, Translator> {
            {Config.TransApi.Baidu, new Baidu(_mod)},
            {Config.TransApi.Bing, new Bing(_mod)},
            {Config.TransApi.Google, new Google(_mod)},
            {Config.TransApi.Caiyun, new Caiyun(_mod)},
            {Config.TransApi.CaiyunNoToken, new CaiyunNoToken(_mod)},
            {Config.TransApi.Tencent, new Tencent(_mod)},
            {Config.TransApi.Volcengine, new Volcengine(_mod)}
        };
    }


    public override void Unload() {
        TranslatorLookup.Clear();
        TranslatorLookup = null;
        _mod = null;
        Config = null;
    }

    public override void OnWorldLoad() {
        _targetLang = Language.ActiveCulture.LegacyId switch {
            1 => "en",
            2 => "de",
            3 => "it",
            4 => "fr",
            5 => "es",
            6 => "ru",
            7 => "zh",
            8 => "pt",
            9 => "pl",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Translator GetCurrentTranslator() {
        return TranslatorLookup[Config.TranslateApi];
    }

    public static Dictionary<string, string> GetCurrentLookup() {
        return TranslatorLookup[Config.TranslateApi].Lookup;
    }

    public override void PostDrawInterface(SpriteBatch spriteBatch) {
        /*
            Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value,
                "Harry Potter is a series of seven fantasy novels written by British author J. K. Rowling. The novels chronicle the lives of a young wizard, Harry Potter, and his friends Hermione Granger and Ron Weasley, all of whom are students at Hogwarts School of Witchcraft and Wizardry. The main story arc concerns Harry's conflict with Lord Voldemort, a dark wizard who intends to become immortal, overthrow the wizard governing body known as the Ministry of Magic and subjugate all wizards and Muggles (non-magical people).", 800, 500, Color.White, Color.Black, Vector2.Zero,
                0.8f);
        */
        
        if (!MachineTranslate.OpenWebsiteKeybind.JustPressed) return;
        string url = Config.TranslateWebsite switch {
            Config.TransWebsite.DeepL => "https://www.deepl.com/translator",
            Config.TransWebsite.Google => "https://translate.google.com/",
            Config.TransWebsite.Bing => "https://www.bing.com/translator",
            Config.TransWebsite.Baidu => "https://fanyi.baidu.com/",
            _ => throw new ArgumentOutOfRangeException()
        };
        Helper.OpenUrl(url);
    }

    /*
    public static async void TranslateYoudao(string text) {
        string url = $"http://fanyi.youdao.com/translate?&doctype=json&type=auto2{TargetLang}&i={text}";
        try {
            using var client = new HttpClient();
            string returnedJson = await client.GetStringAsync(url);
            if (returnedJson.Contains('$')) return;
            var jo = (JObject) JsonConvert.DeserializeObject(returnedJson);
            if (jo is not null && jo.TryGetValue("translateResult", out var result)) {
                string translated = result.Aggregate("", (current, child) => current + "\n " + child[0]?["tgt"]);
                YoudaoTranLookup.Add(text, translated.Trim());
            }
        }
        catch (Exception e) {
            _mod.Logger.Warn(e);
        }
    }
    */
}