using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

/// <summary>
/// 由Github大佬akl7777777的免密钥API改写而来，十分感谢！！！
/// 大佬页面: https://github.com/akl7777777
/// </summary>
public class Volcengine : Translator
{
    private const string Url = "https://translate.volcengine.com/crx/translate/v1";

    public static async Task Translate(Query query, Action<Response> completion) {
        try {
            var targetLanguage = query.DetectTo;
            var sourceLanguage = query.DetectFrom;

            if (string.IsNullOrEmpty(targetLanguage)) {
                var err = new Error {
                    Type = "unsupportLanguage",
                    Message = "不支持该语种"
                };
                throw new Exception(err.Type + " " + err.Message);
            }

            var sourceLang = sourceLanguage ?? "ZH";
            var targetLang = targetLanguage ?? "EN";
            var translateText = query.Text ?? "";

            if (!string.IsNullOrEmpty(translateText)) {
                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var body = new {
                        source_language = sourceLang,
                        target_language = targetLang,
                        text = translateText
                    };

                    var jsonPayload = JsonConvert.SerializeObject(body);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Url, content);

                    response.EnsureSuccessStatusCode();

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);

                    if (data != null && !string.IsNullOrEmpty(data.Translation)) {
                        completion(new Response {
                            Result = new Result {
                                From = query.DetectFrom,
                                To = query.DetectTo,
                                Target = data.Translation
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
            Console.WriteLine("接口请求错误 ==> " + e.Message);
            var err = new Error {
                Type = "network",
                Message = "接口请求错误 - " + e.Message
            };
            throw new Exception(err.Type + " " + err.Message);
        }
    }

    public class TranslationResponse
    {
        public string Translation { get; set; }
    }

    public Volcengine(Mod mod) : base(mod) {
    }

    public override void Translate(string text, string targetLang, Action<string> finishedCallback) {
        async void TranslateInner() {
            try {
                await Translate(new Query {
                    DetectFrom = "auto",
                    DetectTo = targetLang,
                    Text = text
                }, response => {
                    Lookup[text] = response.Result.Target.Trim();
                    TranslateStatus = Status.Idling;
                    finishedCallback?.Invoke(Lookup[text]);
                });
            }
            catch (Exception e) {
                Mod.Logger.Warn(e);
                TranslateStatus = Status.AttemptFailed;
            }
        }

        TranslateInner();
    }
}

public class Query
{
    public string DetectFrom { get; set; }
    public string DetectTo { get; set; }
    public string Text { get; set; }
}

public class Response
{
    public Result Result { get; set; }
    public Error Error { get; set; }
}

public class Result
{
    public string From { get; set; }
    public string To { get; set; }
    public string Target { get; set; }
}

public class Error
{
    public string Type { get; set; }
    public string Message { get; set; }
    public string Addtion { get; set; }
}