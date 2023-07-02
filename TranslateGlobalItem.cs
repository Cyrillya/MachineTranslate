using System.Collections.Generic;
using System.Linq;
using MachineTranslate.Translators;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MachineTranslate;

public class TranslateGlobalItem : GlobalItem
{
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
        string tooltipText =
            (from t in tooltips where t.Name.StartsWith("Tooltip") select t.Text).Aggregate("",
                (current, text) => current + " \n " + text);

        if (tooltipText is "") return;

        var lookup = Core.GetCurrentLookup();
        
        if (lookup.TryGetValue(tooltipText, out var value)) {
            tooltips.Add(new TooltipLine(Mod, "TooltipTranslated", ' ' + value) {
                OverrideColor = Color.Pink
            });
            return;
        }

        var translator = Core.GetCurrentTranslator();
        string statusMsg = translator.TranslateStatus switch {
            Translator.Status.Idling => "按下 T 以翻译简介",
            Translator.Status.Translating => "翻译中...",
            Translator.Status.AttemptFailed => "上一次翻译失败，请重试或更换翻译API。按下 T 以翻译简介",
        };
        tooltips.Add(new TooltipLine(Mod, "TooltipTranslated", statusMsg) {
            OverrideColor = Color.Gray
        });

        if (global::MachineTranslate.MachineTranslate.TranslateKeybind.JustPressed) {
            Core.Translate(tooltipText);
        }
    }
}