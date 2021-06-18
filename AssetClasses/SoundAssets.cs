using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace PhasmophobiaHelper.AssetClasses
{
    public static class SoundAssets
    {
        public static SoundEffect SFXUIEnter;
        public static SoundEffectInstance UIEnter;
        public static SoundEffect SFXUILeave;
        public static SoundEffectInstance UILeave;

        public static SoundEffect SFXMenuLoop;
        public static SoundEffectInstance MenuLoop;

        public static SoundEffect[] SFXUITick = new SoundEffect[4];
        public static SoundEffectInstance[] UITick = new SoundEffectInstance[4];

        public static AudioEmitter emitter;
        public static AudioListener listener;
        
        public static void LoadSounds()
        {
            Main main = Main.Instance;

            emitter = new AudioEmitter();
            listener = new AudioListener();

            SFXUIEnter = main.Content.Load<SoundEffect>("Sounds/UIEnter");
            UIEnter = SFXUIEnter.CreateInstance();
            SFXUILeave = main.Content.Load<SoundEffect>("Sounds/UILeave");
            UILeave = SFXUILeave.CreateInstance();

            SFXMenuLoop = main.Content.Load<SoundEffect>("Sounds/MenuLoop");
            MenuLoop = SFXMenuLoop.CreateInstance();
            MenuLoop.Apply3D(listener, emitter);
            MenuLoop.IsLooped = true;
            MenuLoop?.Play();

            for (int i = 0; i < SFXUITick.Length - 1; i++)
            {
                SFXUITick[i] = main.Content.Load<SoundEffect>($"Sounds/UITick{i + 1}");
                UITick[i] = SFXUITick[i].CreateInstance();
            }
        }
        public static void UpdateSoundVolumes()
        {
            SoundEffect.MasterVolume = Main.appVolume;
            if (Main.Instance.IsActive) MenuLoop.Volume = ButtonBGSounds.on ? Main.appVolume : 0f;
        }
    }
}
