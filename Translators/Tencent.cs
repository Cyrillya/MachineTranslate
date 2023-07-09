using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

/// <summary>
/// 由Github大佬akl7777777的免密钥API改写而来，十分感谢！！！
/// 大佬页面: https://github.com/akl7777777
/// </summary>
public class Tencent : Translator
{
    private const string TranslatorUrl = "https://interpreter.cyapi.cn/v1/translator";
    private const string AuthorizationToken = "ssdj273ksdiwi923bsd9";

    private static readonly Random Random = new Random();

    public static async Task Translate(Query query, Action<Response> completion) {
        try {
            var targetLanguage = query.DetectTo;
            var sourceLanguage = query.DetectFrom;

            if (string.IsNullOrEmpty(targetLanguage)) {
                throw new Exception("UnsupportedLanguage: 不支持该语种");
            }

            var sourceLang = sourceLanguage ?? "ZH";
            var targetLang = targetLanguage ?? "EN";
            var translateText = query.Text ?? "";

            if (!string.IsNullOrEmpty(translateText)) {
                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("x-authorization", "token " + AuthorizationToken);
                    client.DefaultRequestHeaders.Add("user-agent",
                        "caiyunInterpreter/5 CFNetwork/1404.0.5 Darwin/22.3.0");

                    var postData = InitData(sourceLang, targetLang);
                    postData["source"] = translateText;
                    postData["request_id"] = GetRandomNumber();

                    var jsonPayload = JsonConvert.SerializeObject(postData);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(TranslatorUrl, content);

                    response.EnsureSuccessStatusCode();

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);

                    if (data != null && !string.IsNullOrEmpty(data.Target)) {
                        completion(new Response {
                            Result = new Result {
                                From = query.DetectFrom,
                                To = query.DetectTo,
                                Target = data.Target
                            }
                        });
                    }
                    else {
                        var errMsg = data != null ? JsonConvert.SerializeObject(data) : "未知错误";
                        completion(new Response {
                            Error = new Error {
                                Type = "unknown",
                                Message = errMsg,
                                Addtion = errMsg
                            }
                        });
                    }
                }
            }
        }
        catch (Exception e) {
            throw new Exception("接口请求错误 ==> " + e.Message);
        }
    }

    private static int GetRandomNumber() {
        const int minValue = 100000;
        const int maxValue = 999999;
        return Random.Next(minValue, maxValue) * 1000;
    }

    private static Dictionary<string, object> InitData(string sourceLang, string targetLang) {
        return new Dictionary<string, object> {
            ["source"] = "",
            ["detect"] = true,
            ["os_type"] = "ios",
            ["device_id"] = "F1F902F7-1780-4C88-848D-71F35D88A602",
            ["trans_type"] = $"{sourceLang}2{targetLang}",
            ["media"] = "text",
            ["request_id"] = 424238335,
            ["user_id"] = "",
            ["dict"] = true
        };
    }

    public class TranslationResponse
    {
        public string Target { get; set; }
    }

    public Tencent(Mod mod) : base(mod) {
    }

    public override void Translate(string text, string targetLang, Action<string> finishedCallback) {
        async void TranslateInner() {
            try {
                string result = "";
                var lines = text.Split('\n');
                foreach (var line in lines) {
                    await Translate(new Query {
                        DetectFrom = "auto",
                        DetectTo = targetLang,
                        Text = line
                    }, response => { result += response.Result.Target + "\n "; });
                    Thread.Sleep(100); // 防止请求过多
                }

                Lookup[text] = result.Trim();
                TranslateStatus = Status.Idling;
                finishedCallback?.Invoke(Lookup[text]);
            }
            catch (Exception e) {
                Mod.Logger.Warn(e);
                TranslateStatus = Status.AttemptFailed;
            }
        }

        TranslateInner();
    }
}