using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameMonogame
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        public static void LoadSounds(ContentManager content)
        {
            soundEffects["flapSound"] = content.Load<SoundEffect>("flapSound");
            soundEffects["loseSound"] = content.Load<SoundEffect>("loseSound");
            soundEffects["scoreSound"] = content.Load<SoundEffect>("scoreSound");
        }

        public static void PlaySound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                soundEffects[soundName].Play();
            }
            else
            {
                throw new ArgumentException($"Sound {soundName} not found.");
            }
        }
    }
}
