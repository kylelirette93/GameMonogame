using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameMonogame
{
    internal class ScoreManager : GameComponent
    {
        int _score;
        public int Score { get { return _score; } set { _score = value; } }
        private SoundEffect scoreSound;

        public ScoreManager(Game game) : base(game)
        {
            Score = 0;
            scoreSound = game.Content.Load<SoundEffect>("scoreSound");
        }

        

        public void AddScore(int score)
        {
            Score += score;
            scoreSound.Play();
            return;
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (spriteBatch == null || font == null)
            {
                return; 
            }

            // Draw the score to corner of the screen.
            Vector2 position = new Vector2(0, 0);
            Vector2 xOffset = new Vector2(2, 2);
            spriteBatch.DrawString(font, "Score: " + Score, position + xOffset, Color.Black);
            spriteBatch.DrawString(font, "Score: " + Score, position, Color.White);
        }
    }
}
