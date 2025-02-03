using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;


namespace GameMonogame
{
    internal class Pipe : GameEntity
    {
        int gapSize = 80;
        public Rectangle topOfPipe { get { return topPipeRect; } }
        Rectangle topPipeRect;
        public Rectangle bottomOfPipe { get { return bottomPipeRect; } }

        // To determine if player passed a pipe.
        public bool HasPassed { get; set; } = false;
        Rectangle bottomPipeRect;
        Texture2D topColliderTexture;
        Texture2D bottomColliderTexture;
        int pipeHeight = 440;
        int screenHeight;
        int gapY;

        public Pipe(GameManager game, Vector2 initialPosition) : base(game, initialPosition)
        {
            gameManager = game;
            movementDirection = new Vector2(-14f, 0);
            sprite = gameManager.Content.Load<Texture2D>("pipe_sprite");
            topColliderTexture = new Texture2D(gameManager.GraphicsDevice, 1, 1);
            bottomColliderTexture = new Texture2D(gameManager.GraphicsDevice, 1, 1);
            topColliderTexture.SetData(new Color[] { Color.Transparent });
            bottomColliderTexture.SetData(new Color[] { Color.Transparent });

            UpdateColliders();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            UpdateColliders();
        }

        private void UpdateColliders()
        {
            // Y cordinate of the center of the gap.
            gapY = (int)position.Y + (sprite.Height / 2 - gapSize / 2);

            // Top pipe collider.
            topPipeRect = new Rectangle((int)position.X, gapY - pipeHeight, sprite.Width, pipeHeight);

            // Bottom pipe collider.
            bottomPipeRect = new Rectangle((int)position.X, gapY + gapSize, sprite.Width, pipeHeight);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawCollider(topColliderTexture, topPipeRect);
            DrawCollider(bottomColliderTexture, bottomPipeRect);
        }
    }
}
