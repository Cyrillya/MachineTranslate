using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

public class Caiyun : Translator
{
    public Caiyun(Mod mod) : base(mod) {
    }

    public override void Translate(string text, string targetLang, Action<string> finishedCallback) {
        async void TranslateInner() {
            const string url = "http://api.interpreter.caiyunai.com/v1/translator";
            string token = Core.Config.CaiyunTranToken;

            if (targetLang is not ("zh" or "en")) {
                Mod.Logger.Warn("Target language not supported. Caiyun translation only supports chinese and english.");
            }

            var payload = new {
                source = text,
                trans_type = $"auto2{targetLang}",
                request_id = "demo",
                detect = true
            };
            
            try {
                using var client = Helper.GetHttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
                client.Timeout = TimeSpan.FromSeconds(6);

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject(responseJson) is JObject jo &&
                    jo.TryGetValue("target", out var result)) {
                    Lookup[text] = result.ToString();
                    TranslateStatus = Status.Idling;
                    finishedCallback?.Invoke(Lookup[text]);
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