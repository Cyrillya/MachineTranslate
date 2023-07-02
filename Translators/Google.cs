using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

public class Google : Translator
{
    public Google(Mod mod) : base(mod) {
    }

    public override void Translate(string text, string targetLang) {
        async void TranslateInner() {
            string baseUrl = !string.IsNullOrEmpty(Core.Config.MirrorUrl) && Core.Config.UseMirror
                ? Core.Config.MirrorUrl
                : "http://translate.google.com/";

            if (!baseUrl.EndsWith('/')) {
                baseUrl += '/';
            }
            if (!baseUrl.StartsWith("http")) {
                Mod.Logger.Warn($"Invalid url syntax: {baseUrl}");
            }

            try {
                string url = $"{baseUrl}translate_a/single?client=gtx&dt=t&dj=1&ie=UTF-8&sl=auto&tl={targetLang}&q={text}";
                using var client = Core.GetHttpClient();
                client.Timeout = TimeSpan.FromSeconds(6);
                string returnedJson = await client.GetStringAsync(url);
                if (JsonConvert.DeserializeObject(returnedJson) is JObject jo && jo.TryGetValue("sentences", out var result)) {
                    string translated = result.Aggregate("", (current, child) => current + child["trans"]);
                    Lookup[text] = translated.Trim();
                    TranslateStatus = Status.Idling;
                }
            }
            catch (Exception e) {
                Mod.Logger.Warn(e);
                TranslateStatus = Status.AttemptFailed;
            }
        }

        TranslateInner();
    }
}