using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PhasmophobiaHelper.AssetClasses
{
    public static class FXAssets
    {
        public static Effect BlackVignette;

        internal static float oIntensity_BV;
        internal static float oTime_BV;
        public static void LoadFX()
        {
            Main main = Main.Instance;

            BlackVignette = main.Content.Load<Effect>("Effects/BlackVignette");
        }
        internal static void UpdateFX()
        {
            if (oIntensity_BV > 0.02f)
                oIntensity_BV -= 0.02f;
            if (!ButtonBGSounds.on || Main.appVolume == 0f)
                oIntensity_BV = 0f;
            oTime_BV = (float)Main.LastGameTime.TotalGameTime.TotalMilliseconds;

            var timeParams = BlackVignette.Parameters["oGlobalTime"];
            timeParams.SetValue(oTime_BV);
            var intenseParams = BlackVignette.Parameters["oIntensity"];
            intenseParams.SetValue(oIntensity_BV);
        }
    }
}
