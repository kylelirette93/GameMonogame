using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMonogame
{
    public class GameEntity
    {
        protected Texture2D sprite;
        protected Vector2 position;
        protected Game1 gameManager;
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected Rectangle collider;
        protected float movementSpeed;
        protected Vector2 movementDirection;
        public float deltaTime;

        public GameEntity(Game1 game)
        {
            gameManager = game;
            spriteBatch = game.SpriteBatch;
            movementDirection = new Vector2(-10, 0);
            deltaTime = ElapsedGameTime.Milliseconds * 0.001f;
        }

        protected virtual void Update(float deltaTime)
        {
            position += movementDirection * movementSpeed;
        }
    }
}
