using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MachineTranslate.Translators;
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

    public static void Translate(string text) {
        try {
            var translator = GetCurrentTranslator();
            translator.TranslateStatus = Translator.Status.Translating;
            translator.Translate(text, TargetLang);
        }
        catch (Exception e) {
            _mod.Logger.Warn(e);
        }
    }

    public static HttpClient GetHttpClient() {
        switch (Config.UseProxy) {
            case Config.Proxy.Default:
                return new HttpClient(handler: new HttpClientHandler {
                    UseCookies = false
                }, disposeHandler: true);
            case Config.Proxy.No:
                return new HttpClient(handler: new HttpClientHandler {
                    UseProxy = false,
                    UseCookies = false
                }, disposeHandler: true);
        }

        string proxyType = Config.UseProxy is Config.Proxy.Http ? "http" : "socks5";
        string ip = Config.Ip;
        if (ip == "localhost")
            ip = Environment.MachineName;
        var proxy = new WebProxy {
            Address = new Uri($"{proxyType}://{ip}:{Config.Host}"),
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,
        };

        // Proxy credentials
        if (Config.UseCredential) {
            proxy.Credentials = new NetworkCredential(Config.Username, Config.Password);
        }

        // Create a client handler that uses the proxy
        var httpClientHandler = new HttpClientHandler {
            Proxy = proxy,
            UseProxy = true,
            UseCookies = false
        };

        // Disable SSL verification
        // httpClientHandler.ServerCertificateCustomValidationCallback =
        //     HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
        return client;
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