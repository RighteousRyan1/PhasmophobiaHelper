using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhasmophobiaHelper.AssetClasses;
using PhasmophobiaHelper.UI;
using System;

namespace PhasmophobiaHelper
{
    public sealed class ButtonTraitRoller : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            Main.MenuMode = Main.Menu.TraitRoller;
            return base.OnClick();
        }
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 4, Main.screenHeight / 4);
        public override string Name => "Trait Roller";
        public override bool ShouldDraw => Main.defaultMenu;
    }
    public sealed class ButtonEquipmentRandomizer : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public static string[] totalEquipment =
        {
            "EMF Reader",
            "Thermometer",
            "Spirit Box",
            "Photo Camera",
            "Ghost Writing Book",
            "Video Camera",
            "UV Flashlight",
            "Flashlight",
            "Strong Flashlight",
            "Candle",
            "Crucifix",
            "Glowstick",
            "Head-Mounted Camera",
            "Infared Light Sensor",
            "Lighter",
            "Smudge Stick",
            "Motion Sensor",
            "Parabolic Microphone",
            "Salt",
            "Sanity Pills",
            "Sound Sensor",
            "Tripod",
        };

        public override bool OnClick()
        {
            Main.MenuMode = Main.Menu.EquipmentRandomizer;
            return base.OnClick();
        }
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 4 + Main.screenWidth / 2, Main.screenHeight / 4);
        public override string Name => "Equipment Randomizer";
        public override bool ShouldDraw => Main.defaultMenu;
    }
    public sealed class ButtonLocationRandomizer : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;
        
        public override bool OnClick()
        {
            Main.MenuMode = Main.Menu.LocationRandomizer;
            return base.OnClick();
        }

        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 4, Main.screenHeight / 4 + Main.screenHeight / 2);
        public override string Name => "Location Randomizer";
        public override bool ShouldDraw => Main.defaultMenu;
    }
    public sealed class ButtonMisc : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            Main.MenuMode = Main.Menu.Misc;
            return base.OnClick();
        }
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 4 + Main.screenWidth / 2, Main.screenHeight / 4 + Main.screenHeight / 2);
        public override string Name => "Misc";
        public override bool ShouldDraw => Main.defaultMenu;
    }
    public sealed class ButtonNumRoll : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public static int RandNum;

        public static int Max;

        public static string[] Traits { get; set; }
        public override bool OnClick()
        {
            return base.OnClick();
        }
        public override bool CanBeClicked => false;
        public override void Update()
        {
            if (Main.randEquipmentMenu)
                Max = ButtonEquipmentRandomizer.totalEquipment.Length - 1;
            if (Main.traitRollerMenu)
                Max = (int)Math.Round((double)(Main.Traits.Count / 2));
            // Traits = File.ReadAllLines(Main.traitsLocation);
            if (Utils.KeyJustPressed(Keys.Up) && RandNum < Max)
            {
                Utils.PlaySoundInstance(SoundAssets.UITick[0], SoundAssets.SFXUITick[0]);
                RandNum++;
            }
            if (Utils.KeyJustPressed(Keys.Down) && RandNum > 1)
            {
                Utils.PlaySoundInstance(SoundAssets.UITick[1], SoundAssets.SFXUITick[1]);
                RandNum--;
            }

            RandNum = Utils.Clamp(RandNum, 1, Main.traitRollerMenu ? (int)Math.Round((double)(Main.Traits.Count / 2)) : ButtonEquipmentRandomizer.totalEquipment.Length - 1);
        }
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 2, 30);
        public override string Name
        {
            get
            {
                if (Main.traitRollerMenu)
                    return "Total Traits: " + RandNum + $" (Max {(int)Math.Round((double)(Main.Traits.Count / 2))})";
                if (Main.randEquipmentMenu)
                    return "Total Equipment: " + RandNum + " (Max 21)";
                return "Total X";
            }
        }
        public override bool ShouldDraw => Main.traitRollerMenu || Main.randEquipmentMenu;
    }
    public sealed class ButtonBGSounds : UIButton
    {
        public static bool on = true;
        public override bool OnClick()
        {
            on = !on;
            return base.OnClick();
        }
        public override string Name => "BG Noise: " + (on ? "On" : "Off");
        public override Vector2 DrawPosition => new Vector2(100, Main.screenHeight - 20);
    }
}
