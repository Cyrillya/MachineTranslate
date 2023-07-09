using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace MachineTranslate.ConfigElements;

public class UsualLangCodePortal : LinkPortal
{
    public override string GetUrl() => Language.ActiveCulture.Name is "zh-Hans"
        ? "https://developer.aliyun.com/article/54171"
        : "https://developers.google.com/admin-sdk/directory/v1/languages";
}

public class AllLangCodePortal : LinkPortal
{
    public override string GetUrl() => Language.ActiveCulture.Name is "zh-Hans"
        ? "https://baike.baidu.com/item/%E8%AF%AD%E8%A8%80%E4%BB%A3%E7%A0%81/6594123?fr=ge_ala#3"
        : "https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes";
}

public class GoogleSolutionPortal : LinkPortal
{
    public override string GetUrl() => "https://hcfy.app/blog/2022/09/28/ggg";
}

public class BaiduPortal : LinkPortal
{
    public override string GetUrl() => "https://fanyi-api.baidu.com/";
}

public class BaiduTutorialPortal : LinkPortal
{
    public override string GetUrl() => "https://hcfy.app/docs/services/baidu-api";
}

public class CaiyunPortal : LinkPortal
{
    public override string GetUrl() => "https://dashboard.caiyunapp.com/";
}

public class CaiyunTutorialPortal : LinkPortal
{
    public override string GetUrl() => "https://hcfy.app/docs/services/caiyun-api";
}

public abstract class LinkPortal : ConfigElement
{
    public abstract string GetUrl();

    public override void LeftClick(UIMouseEvent evt) {
        base.LeftClick(evt);

        Utils.OpenToURL(GetUrl());
        
        // Steam browser bugged for hcfy webpages
        // Helper.OpenUrl(GetUrl());
    }
}