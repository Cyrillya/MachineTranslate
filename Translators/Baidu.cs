using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.ModLoader;

namespace MachineTranslate.Translators;

public class Baidu : Translator
{
    public Baidu(Mod mod) : base(mod) {
    }

    public override void Translate(string text, string targetLang) {
        async void TranslateInner() {
            var rd = new Random();
            string q = text; // 原文
            string from = "auto"; // 源语言
            string to = targetLang; // 目标语言
            string appId = Core.Config.BaiduTranAppId;
            string salt = rd.Next(100000).ToString();
            string secretKey = Core.Config.BaiduTranSecretKey;
            string sign = EncryptString(appId + q + salt + secretKey);

            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;

            try {
                using var client = Core.GetHttpClient();
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string returnedJson = await response.Content.ReadAsStringAsync();

                if (JsonConvert.DeserializeObject(returnedJson) is JObject jo &&
                    jo.TryGetValue("trans_result", out var result)) {
                    string translated = result.Aggregate("", (current, child) => current + "\n " + child["dst"]);
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

    /// <summary>
    ///  计算MD5值
    /// </summary>
    private static string EncryptString(string str) {
        var md5 = MD5.Create();
        // 将字符串转换成字节数组
        byte[] byteOld = Encoding.UTF8.GetBytes(str);
        // 调用加密方法
        byte[] byteNew = md5.ComputeHash(byteOld);
        // 将加密结果转换为字符串
        var sb = new StringBuilder();
        foreach (byte b in byteNew) {
            // 将字节转换成16进制表示的字符串，
            sb.Append(b.ToString("x2"));
        }

        // 返回加密的字符串
        return sb.ToString();
    }
}