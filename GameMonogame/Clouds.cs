using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic; 

namespace GameMonogame
{
    internal class Clouds : GameEntity
    {
        private int width;
        private int height;
        private float speed = 5f;
        private float nextCloudX;
         
        public Clouds(GameManager game, Vector2 initialPosition) : base(game, initialPosition)
        {
            gameManager = game;
            position = initialPosition;
            width = gameManager.Graphics.PreferredBackBufferWidth;
            height = gameManager.Graphics.PreferredBackBufferHeight;
            sprite = gameManager.Content.Load<Texture2D>("background");
            nextCloudX = width;
        }


        public override void Update(float deltaTime)
        {

            position.X -= speed * deltaTime;
            nextCloudX -= speed * deltaTime;  

            // When the first cloud goes off-screen, reset its position.
            if (position.X + width < 0)
            {
                // Move the first cloud to where the second one is.
                position.X = nextCloudX + width; 
            }

            // When the second cloud goes off-screen, reset its position as well.
            if (nextCloudX + width < 0)
            {
                // Move the second cloud to where the first one is.
                nextCloudX = position.X + width; 
            }

        }
        public override void Draw(GameTime gameTime)
        {
            // Override the method to draw the texture twice.
            // We also don't want clouds to have colliders.
            if (sprite != null)
            {
                gameManager.SpriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
                // Draw a second texture to create a loop.
                gameManager.SpriteBatch.Draw(sprite, new Rectangle((int)nextCloudX, (int)position.Y, width, height), Color.White);
            }
        }
    }
}
  