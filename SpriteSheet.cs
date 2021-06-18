using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PhasmophobiaHelper.AssetClasses;

namespace PhasmophobiaHelper
{
    /// <summary>
    /// Incomplete - Still finishing!
    /// </summary>
    public class SpriteSheet
    {
        public virtual void LoadDefault() { }
        public virtual Texture2D SheetTexture => TextureAssets.WhitePixel;
        public virtual int AnimationSpeed => 60;
        public virtual short FrameCount => 1;
        public virtual short Rows => 1;
        public int CurFrame { get; private set; }
        public Rectangle frame;

        public float scale;
        public Color Color => Color.White;

        public virtual Vector2 DrawPosition => Vector2.Zero;

        public virtual bool ShouldDraw => true;

        internal void Update()
        {
            if (Main.LastGameTime.TotalGameTime.Ticks % AnimationSpeed == 0)
            {
                frame.Y += SheetTexture.Height / FrameCount;
                CurFrame++;
            }
        }
        internal void Draw()
        {
            Main.Batch.Draw(SheetTexture, DrawPosition, frame, Color, 0f, SheetTexture.GetSize() / 2, scale, default, 0f);
        }

        internal void AutoDefaults()
        {
            scale = 1f;
            frame = new Rectangle(0, 0, SheetTexture.Width, SheetTexture.Height);
        }
    }
    /*class sheet : SpriteSheet
    {
        public override int AnimationSpeed => 30;
        public override Texture2D SheetTexture => TextureAssets.UISliderBar;
        public override short FrameCount => 5;
        public override Vector2 DrawPosition => new Vector2(100, 100);
    }*/
}