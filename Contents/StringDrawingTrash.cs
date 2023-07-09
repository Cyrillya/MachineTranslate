using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MachineTranslate.Translators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

namespace MachineTranslate.Contents;

public class StringDrawingTrash : ILoadable
{
    private static bool _drawingChat;

    public void Load(Mod mod) {
        On_RemadeChatMonitor.DrawChat += (orig, self, chat) => {
            _drawingChat = true;
            orig.Invoke(self, chat);
            _drawingChat = false;
        };

        On_ChatManager
                .DrawColorCodedString_SpriteBatch_DynamicSpriteFont_TextSnippetArray_Vector2_Color_float_Vector2_Vector2_refInt32_float_bool +=
            On_ChatManagerOnDrawColorCodedString_TextSnippetArray;

        On_ChatManager
                .DrawColorCodedString_SpriteBatch_DynamicSpriteFont_string_Vector2_Color_float_Vector2_Vector2_float_bool +=
            On_ChatManagerOnDrawColorCodedString_String;

        On_Utils.DrawBorderStringFourWay += On_UtilsOnDrawBorderStringFourWay;
    }

    /// <summary>
    /// Called if any text hovering is detected
    /// </summary>
    private static void HoverDetected(string text) {
        if (Main.gameMenu || Main.ingameOptionsWindow) return;
        if (!text.IsTranslatable()) return;
        if (text.StartsWith(Language.GetTextValue("Mods.MachineTranslate.TransResult", ""))) return;

        text = text.ReplaceLineEndings(""); // wordwrapped text support
        var lookup = Core.GetCurrentLookup();

        if (lookup.TryGetValue(text, out var value)) {
            if (Core.Config.TranslatedTextTooltip) {
                string finalText = FontAssets.MouseText.Value.CreateWrappedText(value, Main.screenWidth * 0.3f);
                UICommon.TooltipMouseText(finalText);
            }
            // don't show the translated text in tooltip
            else if (MachineTranslate.TranslateKeybind.JustPressed ||
                     (_drawingChat && Main.keyState.PressingControl())) {
                Main.NewText(Language.GetTextValue("Mods.MachineTranslate.TransResult", value), Color.Pink);
            }

            return;
        }

        // the tooltip
        if (Core.Config.MonitorTooltipDisplay) {
            string keyString = _drawingChat ? "Ctrl" : MachineTranslate.TranslateKeybind.GetKeybindAssigned();
            var translator = Core.GetCurrentTranslator();
            string statusMsg = translator.TranslateStatus switch {
                Translator.Status.Idling => Language.GetTextValue("Mods.MachineTranslate.TransTooltip",
                    keyString),
                Translator.Status.Translating => Language.GetTextValue("Mods.MachineTranslate.Translating"),
                Translator.Status.AttemptFailed => Language.GetTextValue("Mods.MachineTranslate.TransFailed",
                    keyString),
                _ => throw new ArgumentOutOfRangeException()
            };

            UICommon.TooltipMouseText(statusMsg);
        }

        if (MachineTranslate.TranslateKeybind.JustPressed || (_drawingChat && Main.keyState.PressingControl())) {
            Helper.Translate(text, null,
                result => {
                    if (!Core.Config.TranslatedTextTooltip)
                        Main.NewText(Language.GetTextValue("Mods.MachineTranslate.TransResult", result), Color.Pink);
                });
        }
    }

    #region Vanilla Text Drawing Method

    // detect texts being drawn
    private Vector2 On_ChatManagerOnDrawColorCodedString_TextSnippetArray(
        On_ChatManager.
            orig_DrawColorCodedString_SpriteBatch_DynamicSpriteFont_TextSnippetArray_Vector2_Color_float_Vector2_Vector2_refInt32_float_bool
            orig, SpriteBatch sb, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor,
        float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, float maxWidth, bool ignoreColors) {
        if (!Core.Config.MonitorText || baseColor == Color.Transparent || ignoreColors)
            return orig.Invoke(sb, font, snippets, position, baseColor, rotation, origin, scale, out hoveredSnippet,
                maxWidth, ignoreColors);

        string finalText = "";
        bool isHovered = false;
        var mouse = Main.MouseScreen;
        var vector = position;
        var result = vector;
        float x = font.MeasureString(" ").X;
        var color = baseColor;
        float num3 = 0f;
        for (int i = 0; i < snippets.Length; i++) {
            var textSnippet = snippets[i];
            finalText += textSnippet.Text;
            if (!ignoreColors)
                color = textSnippet.GetVisibleColor();

            var num2 = textSnippet.Scale;

            if (textSnippet.UniqueDraw(justCheckingString: true, out var size, sb, vector, color,
                    scale.X * num2)) {
                if (mouse.Between(vector, vector + size))
                    isHovered = true;

                vector.X += size.X;

                result.X = Math.Max(result.X, vector.X);
                continue;
            }

            string[] array = textSnippet.Text.Split('\n');
            array = Regex.Split(textSnippet.Text, "(\n)");
            bool flag = true;
            foreach (string text in array) {
                string[] array2 = Regex.Split(text, "( )");
                array2 = text.Split(' ');
                if (text == "\n") {
                    vector.Y += (float) font.LineSpacing * num3 * scale.Y;
                    vector.X = position.X;
                    result.Y = Math.Max(result.Y, vector.Y);
                    num3 = 0f;
                    flag = false;
                    continue;
                }

                for (int k = 0; k < array2.Length; k++) {
                    if (k != 0)
                        vector.X += x * scale.X * num2;

                    if (maxWidth > 0f) {
                        float num4 = font.MeasureString(array2[k]).X * scale.X * num2;
                        if (vector.X - position.X + num4 > maxWidth) {
                            vector.X = position.X;
                            vector.Y += (float) font.LineSpacing * num3 * scale.Y;
                            result.Y = Math.Max(result.Y, vector.Y);
                            num3 = 0f;
                        }
                    }

                    if (num3 < num2)
                        num3 = num2;

                    Vector2 vector2 = font.MeasureString(array2[k]) * scale * num2;
                    if (mouse.Between(vector, vector + vector2))
                        isHovered = true;

                    vector.X += vector2.X * scale.X * num2;
                    result.X = Math.Max(result.X, vector.X);
                }

                if (array.Length > 1 && flag) {
                    vector.Y += (float) font.LineSpacing * num3 * scale.Y;
                    vector.X = position.X;
                    result.Y = Math.Max(result.Y, vector.Y);
                    num3 = 0f;
                }

                flag = true;
            }
        }

        if (isHovered) {
            HoverDetected(finalText);
        }

        return orig.Invoke(sb, font, snippets, position, baseColor, rotation, origin, scale, out hoveredSnippet,
            maxWidth, ignoreColors);
    }

    private Vector2 On_ChatManagerOnDrawColorCodedString_String(
        On_ChatManager.
            orig_DrawColorCodedString_SpriteBatch_DynamicSpriteFont_string_Vector2_Color_float_Vector2_Vector2_float_bool
            orig, SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 position, Color baseColor,
        float rotation, Vector2 origin, Vector2 baseScale, float maxWidth, bool ignoreColors) {
        if (!Core.Config.MonitorText || baseColor == Color.Transparent || ignoreColors)
            return orig.Invoke(sb, font, text, position, baseColor, rotation, origin, baseScale, maxWidth,
                ignoreColors);

        string finalText = "";
        bool isHovered = false;

        Vector2 vector = position;
        Vector2 result = vector;
        string[] array = text.Split('\n');
        float x = font.MeasureString(" ").X;
        float num = 1f;
        float num2 = 0f;
        string[] array2 = array;
        for (int i = 0; i < array2.Length; i++) {
            string[] array3 = array2[i].Split(':');
            foreach (string text2 in array3) {
                string[] array4 = text2.Split(' ');
                for (int k = 0; k < array4.Length; k++) {
                    if (k != 0)
                        vector.X += x * baseScale.X * num;

                    if (maxWidth > 0f) {
                        float num3 = font.MeasureString(array4[k]).X * baseScale.X * num;
                        if (vector.X - position.X + num3 > maxWidth) {
                            vector.X = position.X;
                            vector.Y += font.LineSpacing * num2 * baseScale.Y;
                            result.Y = Math.Max(result.Y, vector.Y);
                            num2 = 0f;
                        }
                    }

                    if (num2 < num)
                        num2 = num;

                    var size = font.MeasureString(array4[k]) * baseScale * num;
                    if (Main.MouseScreen.Between(vector, vector + size))
                        isHovered = true;

                    vector.X += size.X;
                    result.X = Math.Max(result.X, vector.X);

                    finalText += array4[k];
                }
            }

            vector.X = position.X;
            vector.Y += font.LineSpacing * num2 * baseScale.Y;
            result.Y = Math.Max(result.Y, vector.Y);
            num2 = 0f;
        }

        if (isHovered) {
            HoverDetected(finalText);
        }

        return orig.Invoke(sb, font, text, position, baseColor, rotation, origin, baseScale, maxWidth, ignoreColors);
    }

    private void On_UtilsOnDrawBorderStringFourWay(On_Utils.orig_DrawBorderStringFourWay orig, SpriteBatch sb,
        DynamicSpriteFont font, string text, float x, float y, Color textColor, Color borderColor, Vector2 origin,
        float scale) {
        if (!Core.Config.MonitorText) {
            orig.Invoke(sb, font, text, x, y, textColor, borderColor, origin, scale);
            return;
        }

        var pos = new Vector2(x, y);
        var size = font.MeasureString(text) * scale;
        if (Main.MouseScreen.Between(pos, pos + size))
            HoverDetected(text);

        orig.Invoke(sb, font, text, x, y, textColor, borderColor, origin, scale);
    }

    #endregion

    public void Unload() {
    }
}