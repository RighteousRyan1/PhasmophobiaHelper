using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhasmophobiaHelper.AssetClasses;
using PhasmophobiaHelper.UI;
using System;
using System.Diagnostics;

namespace PhasmophobiaHelper.UI
{
    // Crying in my pants, gotta make classes for all of this becuase of bad impl :c
    public sealed class SubButtonTanglewood : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 5, Main.screenHeight / 4 + Main.screenHeight / 2);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "Tanglewood";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }
    public sealed class SubButtonRidgeview : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 5 * 2, Main.screenHeight / 4 + Main.screenHeight / 2);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "Ridgeview";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }
    public sealed class SubButtonWillow : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 5 * 3, Main.screenHeight / 4 + Main.screenHeight / 2);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "Willow";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }
    public sealed class SubButtonEdgefield : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 5 * 4, Main.screenHeight / 4 + Main.screenHeight / 2);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "Edgefield";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }
    public sealed class SubButtonHighSchool : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 3, Main.screenHeight / 4 + Main.screenHeight / 2 + 45);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "High School";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }
    public sealed class SubButtonPrison : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 2, Main.screenHeight / 4 + Main.screenHeight / 2 + 45);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "Prison";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }
    public sealed class SubButtonAsylum : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;

        public override bool OnClick()
        {
            bool canParse = Enum.TryParse<Main.Map>(Name.Replace(" ", string.Empty), out var map);

            if (canParse)
            {
                Main.MapMode = map;
                Debug.WriteLine($"Successfully changed MapMode to {map}.");
            }
            else
                Debug.WriteLine($"Failed to change MapMode.");
            return base.OnClick();
        }
        public override void Update()
        {
            if (Main.MapMode.ToString() == Name.Replace(" ", string.Empty))
                if (scale < 1.2f)
                    scale += 0.0175f;
        }
        public override bool CanBeClicked => Main.MapMode.ToString() != Name.Replace(" ", string.Empty);
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth - Main.screenWidth / 3, Main.screenHeight / 4 + Main.screenHeight / 2 + 45);
        public override Color Color => Main.MapMode.ToString() == Name.Replace(" ", string.Empty) ? Color.Yellow : Color.White;
        public override string Name => "Asylum";
        public override bool ShouldDraw => Main.locationRandomizerMenu;
    }

    public sealed class SubButtonChallengeRoller : UIButton
    {
        public override SpriteFont Font => FontAssets.OctoberCrow;
        public static string[] challenges =
        {
            "0% Sanity",
            "Only Candles",
            "No Evidence",
            "Only Glowsticks",
            "No Holdable Light"
        };
        public static bool chosen;
        public static string chosenString;
        public override bool OnClick()
        {
            chosen = true;
            chosenString = Utils.PickRandom(challenges);
            return base.OnClick();
        }
        public override Vector2 DrawPosition => new Vector2(Main.screenWidth / 5, 150);
        public override string Name => chosen ? $"Challenge: {chosenString}" : "Random Challenge";
        public override bool ShouldDraw => Main.miscMenu;
    }
}