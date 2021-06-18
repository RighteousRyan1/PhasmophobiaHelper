using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace PhasmophobiaHelper
{
    public static class Utils
    {
        public static MouseState mouseState;
        public static MouseState mouseStateOld;

        public static KeyboardState keyboardState;
        public static KeyboardState keyboardStateOld;

        public static bool ClickStart(bool right = false)
        {
            if (right)
                return mouseState.RightButton == ButtonState.Pressed && mouseStateOld.RightButton == ButtonState.Released;
            return mouseState.LeftButton == ButtonState.Pressed && mouseStateOld.LeftButton == ButtonState.Released;
        }
        public static bool ClickRelease(bool right = false)
        {
            if (right)
                return mouseStateOld.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released;
            return mouseStateOld.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released;
        }

        public static bool KeyJustPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && keyboardStateOld.IsKeyUp(key);
        }
        public static bool KeyRelease(Keys key)
        {
            return keyboardState.IsKeyUp(key) && keyboardStateOld.IsKeyDown(key);
        }
        private static Type[] Types => Assembly.GetExecutingAssembly().GetTypes();
        public static List<T> GetChildren<T>() where T : class
        {
            var TypesOf = Types;
            List<T> Buffer = new List<T>();

            for (int i = 0; i < TypesOf.Length; i++)
            {
                if (TypesOf[i].IsSubclassOf(typeof(T)))
                {
                    Buffer.Add(Activator.CreateInstance(TypesOf[i]) as T);
                }
            }

            return Buffer;
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
            {
                return max;
            }
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            return value;
        }
        public static T Clamp<T>(ref T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
            {
                return max;
            }
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            return value;
        }
        public static void PlaySoundInstance(this SoundEffectInstance sfx, SoundEffect fromSound)
        {
            sfx = fromSound.CreateInstance();
            sfx.Volume = Main.appVolume;
            sfx?.Play();
        }
        public static Vector2 GetSize(this Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height);
        }
        public static T PickRandom<T>(T[] input)
        {
            int rand = new Random().Next(0, input.Length - 1);

            return input[rand];
        }

        private static List<int> chosenTs = new List<int>();
        public static List<T> PickRandom<T>(T[] input, int amount)
        {
            List<T> values = new List<T>();
            for (int i = 0; i < amount; i++)
            {
            ReRoll:
                int rand = new Random().Next(0, input.Length - 1);

                if (!chosenTs.Contains(rand))
                {
                    chosenTs.Add(rand);
                    values.Add(input[rand]);
                }
                else
                    goto ReRoll;
            }
            chosenTs.Clear();
            return values;
        }
        public static void Apply3DToWithPosition(this SoundEffectInstance sfx, AudioListener listener, AudioEmitter emitter, Vector3 position)
        {
            emitter.Position = position;
            listener.Position = new Vector3(0, 0, 0);
            sfx.Apply3D(listener, emitter);
        }
    }
}
