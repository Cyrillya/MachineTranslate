using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using MachineTranslate.ConfigElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace MachineTranslate;

public class Config : ModConfig
{
    public enum TransApi
    {
        Bing,
        Google,
        Baidu,
        Caiyun,
        CaiyunNoToken,
        Tencent,
        Volcengine
    }
    
    public enum Proxy
    {
        Default,
        Http,
        Socks,
        No
    }

    public override ConfigScope Mode => ConfigScope.ClientSide;
    
    [CustomModConfigItem(typeof(OpenFile))] public object OpenFile;
    [CustomModConfigItem(typeof(UsualLangCodePortal))] public object UsualLangCodePortal;
    [CustomModConfigItem(typeof(AllLangCodePortal))] public object AllLangCodePortal;

    [DrawTicks] [DefaultValue(TransApi.Google)]
    public TransApi TranslateApi;
    [CustomModConfigItem(typeof(Input))] public string CustomTargetLang;

    [Header("Google")]
    public bool UseMirror;
    [CustomModConfigItem(typeof(Input))] public string MirrorUrl;
    [CustomModConfigItem(typeof(GoogleSolutionPortal))] public object GooglePortal;

    [Header("Bing")]
    [CustomModConfigItem(typeof(Input))] public string BingSourceLang;
    [CustomModConfigItem(typeof(Input))] public string BingTranAppId;

    [Header("Baidu")]
    [CustomModConfigItem(typeof(Input))] public string BaiduTranAppId;
    [CustomModConfigItem(typeof(Input))] public string BaiduTranSecretKey;
    [CustomModConfigItem(typeof(BaiduPortal))] public object BaiduPortal;
    [CustomModConfigItem(typeof(BaiduTutorialPortal))] public object BaiduTutorialPortal;

    [Header("Caiyun")]
    [CustomModConfigItem(typeof(Input))] public string CaiyunTranToken;
    [CustomModConfigItem(typeof(CaiyunPortal))] public object CaiyunPortal;
    [CustomModConfigItem(typeof(CaiyunTutorialPortal))] public object CaiyunTutorialPortal;

    [Header("Proxy")]
    [DefaultValue(Proxy.Default)] [DrawTicks] public Proxy UseProxy;
    [CustomModConfigItem(typeof(Input))] [DefaultValue("localhost")] public string Ip;
    [CustomModConfigItem(typeof(Input))] public string Host;
    public bool UseCredential;
    [CustomModConfigItem(typeof(Input))] public string Username;
    [CustomModConfigItem(typeof(Input))] public string Password;

    public override void OnLoaded() {
        Core.Config = this;
    }
}