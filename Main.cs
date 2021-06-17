﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhasmophobiaHelper.AssetClasses;
using PhasmophobiaHelper.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PhasmophobiaHelper
{
    public class Main : Game
    {
        public GraphicsDeviceManager GD { get; set; }
        public static SpriteBatch Batch { get; set; }

        public static Main Instance;

        public static Vector2 MouseCoords { get; set; }

        public static List<UIButton> UIButtons = new List<UIButton>();

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

        public static GameTime LastUpdateTime { get; set; }

        public enum ScreenMode
        {
            Default,
            TraitRoller,
            EquipmentRandomizer
        }

        public static ScreenMode MenuMode = ScreenMode.Default;

        public static bool defaultMenu;
        public static bool traitRollerMenu;
        public static bool randEquipmentMenu;

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
            MaxTraits = Traits.ToArray().Length - 1;

            _barPos = 100f;

            Window.Title = "Phasmophobia Helper";

            UIButtons = Utils.GetChildren<UIButton>();

            foreach (var button in UIButtons)
            {
                button.AutoDefaults();
                button.Initialize();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Batch = new SpriteBatch(GraphicsDevice);

            FontAssets.LoadFonts();
            TextureAssets.LoadTextures();
            SoundAssets.LoadSounds();
        }

        protected override void Update(GameTime gameTime)
        {
            defaultMenu = MenuMode == ScreenMode.Default;
            traitRollerMenu = MenuMode == ScreenMode.TraitRoller;
            randEquipmentMenu = MenuMode == ScreenMode.EquipmentRandomizer;
            LastUpdateTime = gameTime;
            SoundAssets.UpdateSoundVolumes();
            appVolume = _barPos / 100;
            mouseRight = Utils.mouseState.RightButton == ButtonState.Pressed;
            mouseLeft = Utils.mouseState.LeftButton == ButtonState.Pressed;
            isCurWindow = IsActive;
            MouseCoords = Utils.mouseState.Position.ToVector2();

            Utils.mouseState = Mouse.GetState();
            Utils.keyboardState = Keyboard.GetState();

            foreach (var button in UIButtons)
            {
                if (button.ShouldDraw)
                    button.UpdateButton();
            }

            if (!defaultMenu)
            {
                if (Utils.KeyJustPressed(Keys.Escape))
                {
                    Utils.PlaySoundInstance(SoundAssets.UILeave, SoundAssets.SFXUILeave);
                    MenuMode = ScreenMode.Default;
                }
            }
            if (randEquipmentMenu)
            {
                if (Utils.KeyJustPressed(Keys.Enter))
                {
                    chosenEquips = Utils.PickRandom(ButtonEquipmentRandomizer.totalEquipment, ButtonNumRoll.RandNum);
                    Utils.PlaySoundInstance(SoundAssets.UITick[0], SoundAssets.SFXUITick[0]);
                    DrawResults = true;
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
            }
        }

        public Color clearColor = new Color(60, 60, 60);
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(clearColor);

            Batch.Begin(DefaultSort, DefaultBlend);
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
            }
        }
    }
}
