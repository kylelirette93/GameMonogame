using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace GameMonogame
{
    public class Player : GameEntity
    {
        float flapForce = -40.0f;
        float gravity = 12f;
        float maxGravity = 12;
        float gravityDownSpeed = 14f;
        bool isJumpPressed;


        public Player(Game1 game) : base(game)
        {
            sprite = gameManager.Content.Load<Texture2D>("player_sprite");
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
        }

        public void HandleInput()
        {
            
        }
        public void Draw()
        {
            spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y,
            sprite.Width, sprite.Height),
            new Rectangle(40, 40, 50, 50), Color.White, 0f,
            new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f),
            SpriteEffects.None,
            0.0f); 
        }

        
    }
}
