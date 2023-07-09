using System;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MachineTranslate.Contents;

public class TranslateCommand : ModCommand
{
    public override void Action(CommandCaller caller, string input, string[] args) {
        if (args.Length < 1) {
            throw new UsageException("At least one argument was expected.\n" +
                                     Language.GetTextValue("Mods.MachineTranslate.Commands.TranslateCommand.Usage"));
        }

        string content = args[..].Aggregate((a, b) => a + " " + b);
        if (!content.IsTranslatable()) return;
                
        var lookup = Core.GetCurrentLookup();
        
        if (lookup.TryGetValue(content, out var value)) {
            Main.NewText(Language.GetTextValue("Mods.MachineTranslate.TransResult", value), Color.Pink);
            return;
        }

        Helper.Translate(content, null, result =>  {
            Main.NewText(Language.GetTextValue("Mods.MachineTranslate.TransResult", result), Color.Pink);
            Platform.Get<IClipboard>().Value = result;
        });
    }

    public override string Command => "trans";

    public override string Description =>
        Language.GetTextValue("Mods.MachineTranslate.Commands.TranslateCommand.Description");

    public override string Usage => Language.GetTextValue("Mods.MachineTranslate.Commands.TranslateCommand.Usage");
    public override CommandType Type => CommandType.Chat;
}

public class WebsiteCommand : ModCommand
{
    public override void Action(CommandCaller caller, string input, string[] args) {
        string url = Core.Config.TranslateWebsite switch {
            Config.TransWebsite.DeepL => "https://www.deepl.com/translator",
            Config.TransWebsite.Google => "https://translate.google.com/",
            Config.TransWebsite.Bing => "https://www.bing.com/translator",
            Config.TransWebsite.Baidu => "https://fanyi.baidu.com/",
            _ => throw new ArgumentOutOfRangeException()
        };
        Helper.OpenUrl(url);
    }

    public override string Command => "tweb";

    public override string Description =>
        Language.GetTextValue("Mods.MachineTranslate.Commands.WebsiteCommand.Description");

    public override string Usage => Language.GetTextValue("Mods.MachineTranslate.Commands.WebsiteCommand.Usage");
    public override CommandType Type => CommandType.Chat;
}