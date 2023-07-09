using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using MachineTranslate.Translators;
using Steamworks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MachineTranslate;

public static class Helper
{
    public static bool IsTranslatable(this string text) {
        if (string.IsNullOrWhiteSpace(text)) return false;
        if (text.All(t => char.IsDigit(t) || char.IsControl(t) || char.IsPunctuation(t))) return false;
        return true;
    }

    public static string GetKeybindAssigned(this ModKeybind keybind) {
        if (Main.dedServ || keybind == null)
            return string.Empty;

        var assignedKeys = keybind.GetAssignedKeys();
        return assignedKeys.Count == 0
            ? $"{Language.GetTextValue("LegacyMenu.195")}"
            : string.Join(" / ", assignedKeys);
    }

    public static void OpenUrl(string url) {
        if (string.IsNullOrEmpty(url)) {
            return;
        }

        if (!Core.Config.UseSteamBrowser) {
            Utils.OpenToURL(url);
            return;
        }

        try {
            SteamFriends.ActivateGameOverlayToWebPage(url);
        }
        catch {
            Utils.OpenToURL(url);
        }
    }

    public static void Translate(string text, string targetLang = null, Action<string> finishedCallback = null) {
        try {
            var translator = Core.GetCurrentTranslator();
            if (translator.TranslateStatus is Translator.Status.Translating) return;

            translator.TranslateStatus = Translator.Status.Translating;
            translator.Translate(text, targetLang ?? Core.TargetLang, finishedCallback);
        }
        catch (Exception e) {
            Core.Config.Mod.Logger.Warn(e);
        }
    }

    public static HttpClient GetHttpClient() {
        switch (Core.Config.UseProxy) {
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

        string proxyType = Core.Config.UseProxy is Config.Proxy.Http ? "http" : "socks5";
        string ip = Core.Config.Ip;
        if (ip == "localhost")
            ip = Environment.MachineName;
        var proxy = new WebProxy {
            Address = new Uri($"{proxyType}://{ip}:{Core.Config.Host}"),
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,
        };

        // Proxy credentials
        if (Core.Config.UseCredential) {
            proxy.Credentials = new NetworkCredential(Core.Config.Username, Core.Config.Password);
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
}