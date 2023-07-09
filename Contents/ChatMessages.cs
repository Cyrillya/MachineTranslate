using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MachineTranslate.Contents;

public class ChatMessages : ILoadable
{
    public void Load(Mod mod) {
        On_ChatHelper.DisplayMessage += (orig, text, color, author) => {
            orig.Invoke(text, color, author);

            if (!Core.Config.AutoTranslateChat || author == Main.myPlayer) return;

            string textDisplayed = text.ToString();
            if (author < byte.MaxValue) {
                textDisplayed = NameTagHandler.GenerateTag(Main.player[author].name) + " " + textDisplayed;
            }

            Helper.Translate(textDisplayed, null,
                result => {
                    Main.NewText(Language.GetTextValue("Mods.MachineTranslate.TransResult", result), Color.Pink);
                });
        };
    }

    public void Unload() {
    }
}