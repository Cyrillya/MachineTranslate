using System.Diagnostics;
using System.IO;
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

public class OpenFile : ConfigElement
{
    private const string FileName = "MachineTranslate_Config.json";
    private static readonly string FullPath = Path.Combine(ConfigManager.ModConfigPath, FileName);

    public override void OnBind() {
        base.OnBind();
        Height.Set(36f, 0f);
        DrawLabel = false;
        
        Append(new UIText(Label, 0.4f, true) {
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

    public override void LeftClick(UIMouseEvent evt) {
        base.LeftClick(evt);

        // 使用系统默认的方法打开URL
        var psi = new ProcessStartInfo
        {
            FileName = FullPath,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}