using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PhasmophobiaHelper.AssetClasses
{
    public static class FontAssets
    {
        public static SpriteFont OctoberCrow;
        public static SpriteFont Arial;
        public static void LoadFonts()
        {
            Main main = Main.Instance;

            OctoberCrow = main.Content.Load<SpriteFont>(Path.Combine("Fonts", "OctoberCrow"));

            Arial = main.Content.Load<SpriteFont>(Path.Combine("Fonts", "Arial"));
        }
    }
}
