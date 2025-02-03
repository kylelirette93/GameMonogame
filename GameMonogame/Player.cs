using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;


namespace GameMonogame
{
    internal class Player : GameEntity
    {
        private float flapForce = -40.0f;
        private float gravity = 12.0f;
        private float maxGravity = 12.0f;
        private float gravityDownSpeed = 14f;
        private bool isJumpPressed;
        private bool canJump;
        private KeyboardState previousState;
        private SoundEffect flapSound;

        private float rotation;
        // Max tilt angle when flapping.
        private float flapRotation = 0.90f;
        // Max tilt angle when falling.
        private float fallRotation = 1.57f;
        // Speed of which the sprite returns to normal.
        private float gravityRotationSpeed = 0.5f;

        private Texture2D playerColliderTexture;


        public Player(GameManager game, Vector2 initialPosition) : base(game, initialPosition)
        {
            gameManager = game;
            canJump = true;
            rotation = 0.0f;
            movementDirection = new Vector2(0, gravityDownSpeed);
            sprite = gameManager.Content.Load<Texture2D>("player_sprite");
            flapSound = gameManager.Content.Load<SoundEffect>("flap");
            playerColliderTexture = new Texture2D(gameManager.GraphicsDevice, 1, 1);
            playerColliderTexture.SetData(new Color[] { Color.Transparent });
            collider = new Rectangle((int)initialPosition.X, (int)initialPosition.Y, sprite.Width, sprite.Height);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            AdjustPlayerCollider();

            KeyboardState state = Keyboard.GetState();
            if (canJump)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    if (!isJumpPressed)
                    {
                        // Apply forward rotation when jumping.


                        gravity = flapForce;
                        isJumpPressed = true;

                        flapSound.Play();

                        rotation = flapRotation;
                    }
                }
                else
                {
                    isJumpPressed = false;

                    if (rotation > 0)
                    {
                        rotation -= gravityRotationSpeed * deltaTime;
                        if (rotation < 0) rotation = 0;
                    }
                }
            }

            position.Y += gravity * deltaTime;

            if (gravity < maxGravity)
            {
                gravity += deltaTime * gravityDownSpeed;
            }

            previousState = state;  
        }
        public void AdjustPlayerCollider()
        {
            // Offset collider to compensate for rotation.
            collider.Width = sprite.Width / 2;
            collider.Height = sprite.Height / 2;
            collider.X -= 20;
            collider.Y -= 16;
        }

        public void Die()
        {
            // Have player fall off screen.
            movementDirection = new Vector2(0, gravityDownSpeed);
            rotation = fallRotation;
            canJump = false;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawCollider(playerColliderTexture, collider);
            if (sprite != null)
            {
                // Draw the sprite with rotation applied.
                gameManager.SpriteBatch.Draw(
                    sprite,
                    new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height),
                    null,
                    Color.White,
                    rotation, 
                    // Center the rotation on the sprite
                    new Vector2(sprite.Width / 2, sprite.Height / 2), 
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }   
}
