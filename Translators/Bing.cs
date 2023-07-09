using System;
using System.Xml;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

public class Bing : Translator
{
    public Bing(Mod mod) : base(mod) {
    }

    public override void Translate(string text, string targetLang, Action<string> finishedCallback) {
        async void TranslateInner() {
            string url =
                $"https://api.microsofttranslator.com/v2/Http.svc/Translate?appId={Core.Config.BingTranAppId}&from={Core.Config.BingSourceLang}&to={targetLang}&text={text}";
            using var client = Helper.GetHttpClient();
            client.Timeout = TimeSpan.FromSeconds(6);
            try {
                var response = await client.GetAsync(url);
                var returnedXml = await response.Content.ReadAsStreamAsync();
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(returnedXml);
                SetLookupValue(text, xmlDoc.InnerText);
                TranslateStatus = Status.Idling;
                finishedCallback?.Invoke(xmlDoc.InnerText);
            }
            catch (Exception e) {
                Mod.Logger.Warn(e);
                TranslateStatus = Status.AttemptFailed;
            }
        }

        TranslateInner();
    }
}