using System.Diagnostics;
using System.IO;
using MachineTranslate.Contents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace MachineTranslate.ConfigElements;

public class OpenCachedTextFile : OpenFile
{
    protected override string FileName => "MachineTranslate_CachedContent.json";

    public override void Click(UIMouseEvent evt) {
        SaveKeyValueLookup.SaveLookup();
        base.Click(evt);
    }
}

public class OpenFile : ConfigElement
{
    protected virtual string FileName => "MachineTranslate_Config.json";
    protected string FullPath => Path.Combine(ConfigManager.ModConfigPath, FileName);

    public override void OnBind() {
        base.OnBind();
        Height.Set(36f, 0f);
        DrawLabel = false;
        
        Append(new UIText(TextDisplayFunction(), 0.4f, true) {
            TextOriginX = 0.5f,
            TextOriginY = 0.5f,
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill
        });
    }
    
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var dimensions = GetDimensions();
        float num = dimensions.Width + 1f;
        var pos = new Vector2(dimensions.X, dimensions.Y);
        var color = IsMouseHovering ? UICommon.DefaultUIBlue : UICommon.DefaultUIBlue.MultiplyRGBA(new Color(180, 180, 180));
        DrawPanel2(spriteBatch, pos, TextureAssets.SettingsPanel.Value, num, dimensions.Height, color);

        base.DrawSelf(spriteBatch);
    }

    public override void Click(UIMouseEvent evt) {
        base.Click(evt);

        if (!File.Exists(FullPath)) return;
        Process.Start(new ProcessStartInfo(FullPath)
        {
            UseShellExecute = true
        });
    }
}