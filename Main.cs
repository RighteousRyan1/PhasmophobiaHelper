using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhasmophobiaHelper.AssetClasses;
using PhasmophobiaHelper.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace PhasmophobiaHelper
{
    public class Main : Game
    {
        public GraphicsDeviceManager GD { get; set; }
        public static SpriteBatch Batch { get; set; }
        public static Main Instance;
        public static Vector2 MouseCoords { get; set; }
        public static List<UIButton> UIButtons = new List<UIButton>();
        public static List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();
        public static SpriteSortMode DefaultSort = SpriteSortMode.Deferred;
        public static BlendState DefaultBlend = BlendState.AlphaBlend;
        public static List<string> Traits
        {
            get
            {
                List<string> traitsStrList = new List<string>();

                using (StringReader reader = new StringReader(Properties.Resources.traits))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        traitsStrList.Add(line);
                    }
                }

                return traitsStrList;
            }
        }
        public static string[][] hidingSpots =
        {
            //SmallHouses
            new string[]
            {
                "Bedrooms",
                "Lockers",
                "Bathrooms",
                "The Basement",
                "The Garage",
                "Kitchen"
            },
            // HighSchool
            new string[]
            {
                "Classrooms",
                "Janitor Rooms",
                "The Gym"
            },
            // Asylum
            new string[]
            {
                "Bedrooms",
                "Cooking Rooms"
            }
        };
        public static int MaxTraits;
        public static float appVolume;
        public static int screenWidth;
        public static int screenHeight;
        public static Vector2 ScreenDimensions;
        public static bool isCurWindow;
        public static bool mouseRight;
        public static bool mouseLeft;
        public static bool DrawResults;
        private static List<string> chosenEquips = new List<string>();
        private static List<string> traitsChosen = new List<string>();
        public static GameTime LastGameTime { get; set; }
        public static RenderTarget2D ScreenTarget;
        public enum Menu
        {
            Default,
            TraitRoller,
            EquipmentRandomizer,
            LocationRandomizer,
            Misc
        }
        public enum Map
        {
            Tanglewood,
            Ridgeview,
            Willow,
            Edgefield,
            HighSchool,
            Prison,
            Asylum
        }
        public static Map MapMode;
        public static Menu MenuMode = Menu.Default;
        public static bool defaultMenu;
        public static bool traitRollerMenu;
        public static bool randEquipmentMenu;
        public static bool locationRandomizerMenu;
        public static bool miscMenu;
        public Color clearColor = new Color(40, 40, 40);
        public static string version = "v1.2.1";
        public Main()
        {
            Instance = this;
            GD = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            GD.PreferredBackBufferWidth += 400;
            GD.PreferredBackBufferHeight += 400;
            GD.ApplyChanges();
            ScreenTarget = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);
            MaxTraits = Traits.ToArray().Length - 1;

            _barPos = 100f;

            Window.Title = "Phasmophobia Helper";

            UIButtons = Utils.GetChildren<UIButton>();
            SpriteSheets = Utils.GetChildren<SpriteSheet>();

            foreach (var button in UIButtons)
            {
                button.AutoDefaults();
                button.Initialize();
            }
            try
            {
                string s = "";

                if (File.ReadAllLines("config.json").Length > 0)
                {
                    s = File.ReadAllLines("config.json")[0].Replace("\"", "").Replace("Volume", "").Replace(":", "").Replace(",", "");
                    _barPos = (float)Math.Round((float.Parse(s) * 100));
                }
                string s2 = "";
                if (File.ReadAllLines("config.json").Length > 1)
                {
                    s2 = File.ReadAllLines("config.json")[1].Replace("\"", "").Replace("MusicOn", "").Replace(":", "").Replace(",", "");
                    ButtonBGSounds.on = bool.Parse(s2);
                }
            }
            catch(AccessViolationException e)
            {
                Console.WriteLine(e.Message);
            }

            base.Initialize();
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            string jsonFile = "config.json";
            try
            {
                File.WriteAllText(jsonFile, $"\"Volume\": \"{appVolume}\",\n\"MusicOn\": \"{ButtonBGSounds.on}\"");
            }
            catch(AccessViolationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        protected override void LoadContent()
        {
            Batch = new SpriteBatch(GraphicsDevice);

            FontAssets.LoadFonts();
            TextureAssets.LoadTextures();
            SoundAssets.LoadSounds();
            FXAssets.LoadFX();
        }
        private static bool areSheetsInitialized;
        private static bool IsHouse { get; set; }
        private static bool IsMediumMap { get; set; }
        private static bool IsLargeMap { get; set; }

        private static string displayedLocation;
        protected override void Update(GameTime gameTime)
        {
            #region Field Updating
            IsHouse = MapMode == Map.Edgefield || MapMode == Map.Ridgeview || MapMode == Map.Willow || MapMode == Map.Tanglewood;
            IsMediumMap = MapMode == Map.Prison || MapMode == Map.HighSchool;
            IsLargeMap = MapMode == Map.Asylum;
            defaultMenu = MenuMode == Menu.Default;
            traitRollerMenu = MenuMode == Menu.TraitRoller;
            randEquipmentMenu = MenuMode == Menu.EquipmentRandomizer;
            locationRandomizerMenu = MenuMode == Menu.LocationRandomizer;
            miscMenu = MenuMode == Menu.Misc;
            LastGameTime = gameTime;
            SoundAssets.UpdateSoundVolumes();
            appVolume = _barPos / 100;
            mouseRight = Utils.mouseState.RightButton == ButtonState.Pressed;
            mouseLeft = Utils.mouseState.LeftButton == ButtonState.Pressed;
            isCurWindow = IsActive;
            MouseCoords = Utils.mouseState.Position.ToVector2();
            Utils.mouseState = Mouse.GetState();
            Utils.keyboardState = Keyboard.GetState();
            #endregion

            if (!miscMenu)
                SubButtonChallengeRoller.chosen = false;
            foreach (var button in UIButtons)
            {
                if (button.ShouldDraw)
                    button.UpdateButton();

                if (!areSheetsInitialized)
                {
                    button.AutoDefaults();
                    button.Initialize();

                    areSheetsInitialized = true;
                }
            }
            if (!defaultMenu)
            {
                if (Utils.KeyJustPressed(Keys.Escape))
                {
                    Utils.PlaySoundInstance(SoundAssets.UILeave, SoundAssets.SFXUILeave);
                    MenuMode = Menu.Default;
                }
            }
            if (randEquipmentMenu || locationRandomizerMenu)
            {
                if (Utils.KeyJustPressed(Keys.Enter))
                {
                    chosenEquips = Utils.PickRandom(ButtonEquipmentRandomizer.totalEquipment, ButtonNumRoll.RandNum);
                    Utils.PlaySoundInstance(SoundAssets.UITick[0], SoundAssets.SFXUITick[0]);
                    DrawResults = true;

                    if (IsHouse)
                    {
                        displayedLocation = Utils.PickRandom(hidingSpots[0]);
                    }
                    if (IsMediumMap)
                    {
                        displayedLocation = Utils.PickRandom(hidingSpots[1]);
                    }
                    if (IsLargeMap)
                    {
                        displayedLocation = Utils.PickRandom(hidingSpots[2]);
                    }
                }
            }
            if (traitRollerMenu)
            {
                if (Utils.KeyJustPressed(Keys.Enter))
                {
                    traitsChosen = Utils.PickRandom(Traits.ToArray(), ButtonNumRoll.RandNum);
                    Utils.PlaySoundInstance(SoundAssets.UITick[0], SoundAssets.SFXUITick[0]);
                    DrawResults = true;
                }
            }
            void MakeVersionNumberRectangle()
            {
                Rectangle hover = new Rectangle(6, 6, 60, 30);

                if (hover.Contains(MouseCoords.ToPoint()))
                {
                    if (Utils.ClickStart())
                    {
                        Process.Start(new ProcessStartInfo("https://github.com/RighteousRyan1/PhasmophobiaHelper/releases/tag/" + version.Replace("v", string.Empty))
                        {
                            UseShellExecute = true
                        });
                    }
                }
            }
            MakeVersionNumberRectangle();
            Utils.mouseStateOld = Utils.mouseState;
            Utils.keyboardStateOld = Utils.keyboardState;
            screenWidth = Window.ClientBounds.Width;
            screenHeight = Window.ClientBounds.Height;
            ScreenDimensions = new Vector2(screenWidth, screenHeight);
            if (clearColor.R > 0)
            {
                clearColor.R--;
                clearColor.G--;
                clearColor.B--;
            }
            if (clearColor.R == 0)
            {
                byte randByte = 60;
                clearColor.R = randByte;
                clearColor.G = randByte;
                clearColor.B = randByte;
                FXAssets.oIntensity_BV = 1f * appVolume;
            }
            FXAssets.UpdateFX();
        }
        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive)
                SoundAssets.MenuLoop.Volume = 0f;

            GraphicsDevice.Clear(ButtonBGSounds.on ? clearColor : Color.Black);
            GraphicsDevice.SetRenderTarget(ScreenTarget); // setrendertarget(target)
            Batch.Begin(DefaultSort, DefaultBlend, null, null, null, null); // begin(noeffect)
            Batch.Draw(TextureAssets.RidgeviewPorch, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            if (defaultMenu)
                Batch.Draw(TextureAssets.PhasmoLogo, new Vector2(screenWidth / 2, screenHeight / 2), null, Color.White, 0f, TextureAssets.PhasmoLogo.GetSize() / 2, 1f, default, default);
            // draw...
            Batch.DrawString(FontAssets.OctoberCrow, version, new Vector2(6, 6), Color.White, 0f,
              Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            foreach (var sheet in SpriteSheets)
            {
                if (sheet.ShouldDraw)
                    sheet.Draw();
            }
            if (defaultMenu)
            {
                DrawResults = false;
                ButtonNumRoll.RandNum = 0;
                DrawVolumeSlider();
            }
            DrawResult();
            foreach (var button in UIButtons)
            {
                if (button.ShouldDraw)
                {
                    //Batch.DrawString(FontAssets.Arial, button.ToString(), button.DrawPosition + new Vector2(0, 30), Color.White, 0f,
                    //  FontAssets.Arial.MeasureString(button.ToString()) / 2, 0.75f, SpriteEffects.None, 1f);
                    button.Draw(false);
                }
            }
            DrawInfo();
            Batch.End(); // end...
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(ButtonBGSounds.on ? clearColor : Color.Black);
            Batch.Begin(DefaultSort, null, null, null, null, FXAssets.BlackVignette);

            Batch.Draw(ScreenTarget, new Rectangle(0, 0, ScreenTarget.Width, ScreenTarget.Height), Color.White);

            Batch.End();

            base.Draw(gameTime);
        }

        private static float _barPos;
        public static void DrawVolumeSlider()
        {
            Texture2D slider = TextureAssets.UISlider;
            Texture2D sliderBar = TextureAssets.UISliderBar;

            Batch.Draw(slider, new Vector2(ScreenDimensions.X / 2, ScreenDimensions.Y - 20), null, Color.White, 0f, slider.GetSize() / 2, 1f, default, 0f);
            Batch.Draw(sliderBar, new Vector2(ScreenDimensions.X / 2 - (slider.GetSize().X / 2) + _barPos, ScreenDimensions.Y - 20), null, Color.White, 0f, sliderBar.GetSize() / 2, 1f, default, 0f);

            Vector2 sliderPos = new Vector2(ScreenDimensions.X / 2 - (slider.GetSize().X / 2), ScreenDimensions.Y - 26);

            Rectangle sliderHover = new Rectangle((int)sliderPos.X, (int)sliderPos.Y, 101, 12);

            // Batch.Draw(TextureAssets.WhitePixel, sliderHover, Color.White * 0.25f);
            Batch.DrawString(FontAssets.OctoberCrow, $"{_barPos}%", new Vector2(ScreenDimensions.X / 2 + 80, ScreenDimensions.Y - 20), Color.White, 0f,
              FontAssets.OctoberCrow.MeasureString($"{_barPos}%") / 2, 0.5f, SpriteEffects.None, 1f);
            Batch.DrawString(FontAssets.OctoberCrow, "Volume", new Vector2(ScreenDimensions.X / 2, ScreenDimensions.Y - 40), Color.White, 0f,
              FontAssets.OctoberCrow.MeasureString("Volume") / 2, 0.6f, SpriteEffects.None, 1f);
            if (sliderHover.Contains(MouseCoords.ToPoint()))
            {
                if (mouseLeft)
                {
                    _barPos = MouseCoords.X - (screenWidth / 2 - (slider.GetSize().X / 2));
                }
            }
        }
        public static void DrawInfo()
        {
            if (!defaultMenu)
            {
                string str = $"Press 'Escape' to return";
                string str2 = $"Press 'Enter' to display randomizer roll";
                var pos = new Vector2(screenWidth / 2, screenHeight - 20);
                Batch.DrawString(FontAssets.OctoberCrow, str2, pos, Color.White, 0f, FontAssets.OctoberCrow.MeasureString(str2) / 2, 0.5f, default, 0f);
                Batch.DrawString(FontAssets.OctoberCrow, str, pos - new Vector2(0, 20), Color.White, 0f, FontAssets.OctoberCrow.MeasureString(str) / 2, 0.5f, default, 0f);
            }

        }
        public static void DrawResult()
        {
            if (DrawResults)
            {
                if (randEquipmentMenu)
                {
                    int i = 0;
                    foreach (var str in chosenEquips)
                    {
                        i++;
                        float getY = screenHeight / 7 + (i - 1) * 35;
                        Vector2 getGoodCoords = getY < screenHeight ? new Vector2(screenWidth / 2, getY) : new Vector2(screenWidth / 2, screenHeight / 7 + (i - 13) * 35);
                        Batch.DrawString(FontAssets.OctoberCrow, str, getGoodCoords, Color.White, 0f, FontAssets.OctoberCrow.MeasureString(str) / 2, 0.75f, default, 0f);
                    }
                }
                if (traitRollerMenu)
                {
                    int j = 0;
                    foreach (var str in traitsChosen)
                    {
                        j++;
                        float getY = screenHeight / 7 + (j - 1) * 35;
                        Vector2 getGoodCoords = getY < screenHeight ? new Vector2(screenWidth / 2, getY) : new Vector2(screenWidth / 2, screenHeight / 7 + (j - 13) * 35);
                        Batch.DrawString(FontAssets.OctoberCrow, str, getGoodCoords, Color.White, 0f, FontAssets.OctoberCrow.MeasureString(str) / 2, 0.6f, default, 0f);
                    }
                }
                if (locationRandomizerMenu)
                {
                    if (IsHouse)
                    {
                        Batch.DrawString(FontAssets.OctoberCrow, "You cannot hide in... " + displayedLocation + ".", new Vector2(screenWidth / 2, screenHeight / 2), Color.White, 0f, 
                            FontAssets.OctoberCrow.MeasureString("You cannot hide in... " + displayedLocation + ".") / 2, 1.75f, default, 0f);
                    }
                    if (IsMediumMap)
                    {
                        Batch.DrawString(FontAssets.OctoberCrow, "You cannot hide in... " + displayedLocation + ".", new Vector2(screenWidth / 2, screenHeight / 2), Color.White, 0f,
                            FontAssets.OctoberCrow.MeasureString("You cannot hide in... " + displayedLocation + ".") / 2, 1.75f, default, 0f);
                    }
                    if (IsLargeMap)
                    {
                        Batch.DrawString(FontAssets.OctoberCrow, "You cannot hide in... " + displayedLocation + ".", new Vector2(screenWidth / 2, screenHeight / 2), Color.White, 0f,
                            FontAssets.OctoberCrow.MeasureString("You cannot hide in... " + displayedLocation + ".") / 2, 1.75f, default, 0f);
                    }
                }
            }
        }
    }
}
