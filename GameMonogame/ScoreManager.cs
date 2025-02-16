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
    public class ScoreManager
    {
        private int _score;
        public int Score { get { return _score; } set { _score = value; } }
        private GameManager gameManager;
        private bool hasScored = false;

        public ScoreManager(GameManager game)
        {
            gameManager = game;
        }

        public void AddScore(int score)
        {
            _score += score;          
            SoundManager.PlaySound("scoreSound");
        }

        public void ResetScore()
        {
            _score = 0;
        }
    }
}
