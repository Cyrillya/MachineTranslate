using System;
using System.Collections.Generic;
using System.Linq;
using MachineTranslate.Translators;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MachineTranslate.Contents;

public class TranslateGlobalItem : GlobalItem
{
    /// <summary>
    /// Tooltip translation
    /// </summary>
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
        string tooltipText =
            (from t in tooltips where t.Name.StartsWith("Tooltip") select t.Text).Aggregate("",
                (current, text) => current + " \n " + text);

        if (!tooltipText.IsTranslatable()) return;

        var translator = Core.GetCurrentTranslator();
        
        if (translator.TryGetLookupValue(tooltipText, out var value)) {
            tooltips.Add(new TooltipLine(Mod, "TooltipTranslated", ' ' + value) {
                OverrideColor = Color.Pink
            });
            return;
        }

        string statusMsg = translator.TranslateStatus switch {
            Translator.Status.Idling => Language.GetTextValue("Mods.MachineTranslate.TransTooltip",
                MachineTranslate.TranslateKeybind.GetKeybindAssigned()),
            Translator.Status.Translating => Language.GetTextValue("Mods.MachineTranslate.Translating"),
            Translator.Status.AttemptFailed => Language.GetTextValue("Mods.MachineTranslate.TransFailed",
                MachineTranslate.TranslateKeybind.GetKeybindAssigned()),
            _ => throw new ArgumentOutOfRangeException()
        };
        tooltips.Add(new TooltipLine(Mod, "TooltipTranslated", statusMsg) {
            OverrideColor = Color.Gray
        });

        if (MachineTranslate.TranslateKeybind.JustPressed) {
            Helper.Translate(tooltipText);
        }
    }
}