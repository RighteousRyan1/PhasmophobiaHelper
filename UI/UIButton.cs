using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhasmophobiaHelper.AssetClasses;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PhasmophobiaHelper.UI
{
    public class UIButton
    {
        public virtual bool OnClick() => false;
        public virtual void Holding() { }
        public virtual bool OnRelease() => false;
        public virtual void Initialize() { }
        public virtual void Update() { }
        // public virtual Rectangle ClickBox => new Rectangle();
        public virtual bool CanBeClicked => true;
        public bool IsHovered { get; private set; }

        public virtual Vector2 DrawPosition => Vector2.One;

        public virtual Color Color
        {
            get => Color.White;
            set { }
        }

        public virtual string Name => "N/A";

        public virtual bool ShouldDraw => true;

        public virtual SpriteFont Font => FontAssets.OctoberCrow;
        public virtual SpriteEffects SpriteFX => default;

        public Rectangle HoverBox { get; private set; }

        private float scale;
        public float rotation;


        private bool _newHover;
        private bool _oldHover;
        internal void UpdateButton()
        {
            Vector2 origin = Font.MeasureString(Name) / 2;

            HoverBox = new Rectangle((int)(DrawPosition.X - origin.X), (int)(DrawPosition.Y - origin.Y), (int)Font.MeasureString(Name).X, (int)Font.MeasureString(Name).Y);

            IsHovered = HoverBox.Contains(Main.MouseCoords.ToPoint());

            _newHover = IsHovered;

            if (Main.isCurWindow)
            {
                if (Main.LastGameTime.TotalGameTime.TotalMilliseconds > 0.001)
                {
                    if (_newHover && !_oldHover && CanBeClicked)
                    {
                        int choice = new Random().Next(0, 3);

                        Utils.PlaySoundInstance(SoundAssets.UITick[choice], SoundAssets.SFXUITick[choice]);
                    }

                    if (Utils.ClickStart() && IsHovered && CanBeClicked)
                    {
                        Utils.PlaySoundInstance(SoundAssets.UIEnter, SoundAssets.SFXUIEnter);
                        OnClick();
                    }
                }
            }

            if (CanBeClicked)
            {
                if (IsHovered)
                {
                    if (scale < 1.1825f)
                        scale += 0.0175f;
                }
                else
                {
                    if (scale > 1.0175f)
                        scale -= 0.0175f;
                }
            }

            if (Utils.ClickRelease() && IsHovered)
                OnRelease();

            Update();

            _oldHover = _newHover;
        }

        public void Draw(bool beginBatch = true)
        {
            if (beginBatch) Main.Batch.Begin(Main.DefaultSort, Main.DefaultBlend);

            Main.Batch.DrawString(Font, Name, DrawPosition, Color, rotation, Font.MeasureString(Name) / 2, scale, SpriteFX, 0f);
            //Main.Batch.Draw(TextureAssets.WhitePixel, HoverBox, Color.White * 0.25f);

            if (beginBatch) Main.Batch.End();
        }
        public override string ToString()
        {
            return "{ IsHovered: " + IsHovered + " | Scale: " + scale + " | Name: " + Name + " }";
        }
        internal void AutoDefaults()
        {
            scale = 1f;
            rotation = 0f;
        }
    }
}
