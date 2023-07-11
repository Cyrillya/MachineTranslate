using System.ComponentModel;
using MachineTranslate.ConfigElements;
using Terraria.ModLoader.Config;

namespace MachineTranslate;

[Label("$Mods.MachineTranslate.Configs.Config.DisplayName")]
public class Config : ModConfig
{
    public enum TransApi
    {
        [Label("$Mods.MachineTranslate.Configs.TransApi.Bing")]
        Bing,

        [Label("$Mods.MachineTranslate.Configs.TransApi.Google")]
        Google,

        [Label("$Mods.MachineTranslate.Configs.TransApi.Baidu")]
        Baidu,

        [Label("$Mods.MachineTranslate.Configs.TransApi.Caiyun")]
        Caiyun,

        [Label("$Mods.MachineTranslate.Configs.TransApi.CaiyunNoToken")]
        CaiyunNoToken,

        [Label("$Mods.MachineTranslate.Configs.TransApi.Tencent")]
        Tencent,

        [Label("$Mods.MachineTranslate.Configs.TransApi.Volcengine")]
        Volcengine
    }

    public enum TransWebsite
    {
        [Label("$Mods.MachineTranslate.Configs.TransWebsite.DeepL")]
        DeepL,

        [Label("$Mods.MachineTranslate.Configs.TransWebsite.Bing")]
        Bing,

        [Label("$Mods.MachineTranslate.Configs.TransWebsite.Google")]
        Google,

        [Label("$Mods.MachineTranslate.Configs.TransWebsite.Baidu")]
        Baidu
    }

    public enum Proxy
    {
        [Label("$Mods.MachineTranslate.Configs.Proxy.Default")]
        Default,

        [Label("$Mods.MachineTranslate.Configs.Proxy.Http")]
        Http,

        [Label("$Mods.MachineTranslate.Configs.Proxy.Socks")]
        Socks,

        [Label("$Mods.MachineTranslate.Configs.Proxy.No")]
        No
    }

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Label("$Mods.MachineTranslate.Configs.Config.OpenFile.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.OpenFile.Tooltip")]
    [CustomModConfigItem(typeof(OpenFile))]
    public object OpenFile;

    [Label("$Mods.MachineTranslate.Configs.Config.OpenCachedTextFile.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.OpenCachedTextFile.Tooltip")]
    [CustomModConfigItem(typeof(OpenCachedTextFile))]
    public object OpenCachedTextFile;

    [Label("$Mods.MachineTranslate.Configs.Config.UsualLangCodePortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.UsualLangCodePortal.Tooltip")]
    [CustomModConfigItem(typeof(UsualLangCodePortal))]
    public object UsualLangCodePortal;

    [Label("$Mods.MachineTranslate.Configs.Config.AllLangCodePortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.AllLangCodePortal.Tooltip")]
    [CustomModConfigItem(typeof(AllLangCodePortal))]
    public object AllLangCodePortal;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Common")]
    [Label("$Mods.MachineTranslate.Configs.Config.TranslateApi.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.TranslateApi.Tooltip")]
    [DrawTicks]
    [DefaultValue(TransApi.Google)]
    public TransApi TranslateApi;

    [Label("$Mods.MachineTranslate.Configs.Config.CustomTargetLang.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.CustomTargetLang.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string CustomTargetLang;

    [Label("$Mods.MachineTranslate.Configs.Config.TranslateWebsite.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.TranslateWebsite.Tooltip")]
    [DrawTicks]
    [DefaultValue(TransWebsite.DeepL)]
    public TransWebsite TranslateWebsite;

    [Label("$Mods.MachineTranslate.Configs.Config.UseSteamBrowser.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.UseSteamBrowser.Tooltip")]
    [DefaultValue(true)]
    public bool UseSteamBrowser;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Monitor")]
    [Label("$Mods.MachineTranslate.Configs.Config.AutoTranslateChat.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.AutoTranslateChat.Tooltip")]
    [DefaultValue(false)]
    public bool AutoTranslateChat;

    [Label("$Mods.MachineTranslate.Configs.Config.MonitorText.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.MonitorText.Tooltip")]
    [DefaultValue(true)]
    public bool MonitorText;

    [Label("$Mods.MachineTranslate.Configs.Config.MonitorTooltipDisplay.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.MonitorTooltipDisplay.Tooltip")]
    [DefaultValue(true)]
    public bool MonitorTooltipDisplay;

    [Label("$Mods.MachineTranslate.Configs.Config.TranslatedTextTooltip.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.TranslatedTextTooltip.Tooltip")]
    [DefaultValue(true)]
    public bool TranslatedTextTooltip;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Google")]
    [Label("$Mods.MachineTranslate.Configs.Config.UseMirror.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.UseMirror.Tooltip")]
    public bool UseMirror;

    [Label("$Mods.MachineTranslate.Configs.Config.MirrorUrl.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.MirrorUrl.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string MirrorUrl;

    [Label("$Mods.MachineTranslate.Configs.Config.GooglePortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.GooglePortal.Tooltip")]
    [CustomModConfigItem(typeof(GoogleSolutionPortal))]
    public object GooglePortal;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Bing")]
    [Label("$Mods.MachineTranslate.Configs.Config.BingSourceLang.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.BingSourceLang.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string BingSourceLang;

    [Label("$Mods.MachineTranslate.Configs.Config.BingTranAppId.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.BingTranAppId.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string BingTranAppId;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Baidu")]
    [Label("$Mods.MachineTranslate.Configs.Config.BaiduTranAppId.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.BaiduTranAppId.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string BaiduTranAppId;

    [Label("$Mods.MachineTranslate.Configs.Config.BaiduTranSecretKey.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.BaiduTranSecretKey.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string BaiduTranSecretKey;

    [Label("$Mods.MachineTranslate.Configs.Config.BaiduPortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.BaiduPortal.Tooltip")]
    [CustomModConfigItem(typeof(BaiduPortal))]
    public object BaiduPortal;

    [Label("$Mods.MachineTranslate.Configs.Config.BaiduTutorialPortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.BaiduTutorialPortal.Tooltip")]
    [CustomModConfigItem(typeof(BaiduTutorialPortal))]
    public object BaiduTutorialPortal;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Caiyun")]
    [Label("$Mods.MachineTranslate.Configs.Config.CaiyunTranToken.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.CaiyunTranToken.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string CaiyunTranToken;

    [Label("$Mods.MachineTranslate.Configs.Config.CaiyunPortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.CaiyunPortal.Tooltip")]
    [CustomModConfigItem(typeof(CaiyunPortal))]
    public object CaiyunPortal;

    [Label("$Mods.MachineTranslate.Configs.Config.CaiyunTutorialPortal.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.CaiyunTutorialPortal.Tooltip")]
    [CustomModConfigItem(typeof(CaiyunTutorialPortal))]
    public object CaiyunTutorialPortal;

    [Header("$Mods.MachineTranslate.Configs.Config.Headers.Proxy")]
    [Label("$Mods.MachineTranslate.Configs.Config.UseProxy.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.UseProxy.Tooltip")]
    [DefaultValue(Proxy.Default)]
    [DrawTicks]
    public Proxy UseProxy;

    [Label("$Mods.MachineTranslate.Configs.Config.Ip.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.Ip.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    [DefaultValue("localhost")]
    public string Ip;

    [Label("$Mods.MachineTranslate.Configs.Config.Host.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.Host.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string Host;

    [Label("$Mods.MachineTranslate.Configs.Config.UseCredential.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.UseCredential.Tooltip")]
    public bool UseCredential;

    [Label("$Mods.MachineTranslate.Configs.Config.Username.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.Username.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string Username;

    [Label("$Mods.MachineTranslate.Configs.Config.Password.Label")]
    [Tooltip("$Mods.MachineTranslate.Configs.Config.Password.Tooltip")]
    [CustomModConfigItem(typeof(Input))]
    public string Password;

    public override void OnLoaded() {
        Core.Config = this;
    }
}