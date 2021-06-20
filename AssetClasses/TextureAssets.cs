using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PhasmophobiaHelper.AssetClasses
{
    public static class TextureAssets
    {
        public static Texture2D WhitePixel;
        public static Texture2D UISlider;
        public static Texture2D UISliderBar;
        public static Texture2D PhasmoLogo;
        public static Texture2D RidgeviewPorch;
        public static void LoadTextures()
        {
            Main main = Main.Instance;

            RidgeviewPorch = main.Content.Load<Texture2D>("Textures/RidgeviewPorch");
            PhasmoLogo = main.Content.Load<Texture2D>("Textures/PhasmoLogo");
            WhitePixel = main.Content.Load<Texture2D>("Textures/WhitePixel");

            UISlider = main.Content.Load<Texture2D>("Textures/UISlider");
            UISliderBar = main.Content.Load<Texture2D>("Textures/UISliderBar");
        }
    }
}
