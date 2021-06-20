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
        public virtual bool CanBeClicked => true;
        public virtual Vector2 Origin => Font.MeasureString(Name) / 2;
        public bool IsHovered { get; private set; }
        public virtual Vector2 DrawPosition { get; internal set; }
        public virtual Color Color
        {
            get => Color.White;
            set { }
        }
        public virtual string Name { get; internal set; }
        public virtual bool ShouldDraw { get => true; private set { } }
        public virtual SpriteFont Font { get => FontAssets.OctoberCrow; private set { } }
        public virtual SpriteEffects SpriteFX => default;
        public Rectangle HoverBox { get; private set; }
        public bool CurrentlyClicked { get; private set; }
        public float maxScale = 1.2f;
        public float scale;
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
                        CurrentlyClicked = true;
                        Clicked?.Invoke(this, new EventArgs());
                    }
                    else
                        CurrentlyClicked = false;
                }
            }

            if (CanBeClicked)
            {
                if (IsHovered)
                {
                    if (scale < maxScale)
                        scale += 0.0175f;
                }
                else
                {
                    if (scale > 1f)
                        scale -= 0.0175f;
                }
            }

            if (Utils.ClickRelease() && IsHovered)
                OnRelease();

            if (!ShouldDraw)
                scale = 1f;

            Update();

            _oldHover = _newHover;
        }
        public void Draw(bool beginBatch = true)
        {
            if (beginBatch) Main.Batch.Begin(Main.DefaultSort, Main.DefaultBlend);

            Main.Batch.DrawString(Font, Name, DrawPosition, Color, rotation, Origin, scale, SpriteFX, 0f);
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
        public event EventHandler Clicked;
    }
}
