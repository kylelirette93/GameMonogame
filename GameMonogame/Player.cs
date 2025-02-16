using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameMonogame
{
    public class Player : GameEntity
    {
        // Physics variables.
        private float flapForce = -40.0f;
        private float gravity = 12.0f;
        private float maxGravity = 12.0f;
        private float gravityDownSpeed = 14f;

        // Input variables.
        private bool isJumpPressed;
        private bool canJump;
        private KeyboardState previousState;
        private KeyboardState currentState;

        // Rotation variables.
        private float rotation;
        private float flapRotation = 0.90f;
        private float fallRotation = 1.57f;
        private float gravityRotationSpeed = 0.5f;

        // Collider variables.
        private Texture2D playerColliderTexture;
        int colliderXOffset = 20;
        int colliderYOffset = 16;


        public Player(GameManager game, Vector2 initialPosition) : base(game, initialPosition)
        {
            // Initialize player variables.
            gameManager = game;
            canJump = true;
            rotation = 0.0f;
            movementDirection = new Vector2(0, gravityDownSpeed);
            sprite = gameManager.Content.Load<Texture2D>("player_sprite");
            playerColliderTexture = new Texture2D(gameManager.GraphicsDevice, 1, 1);
            playerColliderTexture.SetData(new Color[] { Color.Transparent });
            collider = new Rectangle((int)initialPosition.X - colliderXOffset, (int)initialPosition.Y - colliderYOffset, sprite.Width / 2, sprite.Height / 2);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            UpdatePlayerCollider();

            currentState = Keyboard.GetState();
            if (canJump)
            {
                if (currentState.IsKeyDown(Keys.Space))
                {
                    if (!isJumpPressed)
                    {
                        // Apply forward rotation when jumping.
                        gravity = flapForce;
                        isJumpPressed = true;
                        SoundManager.PlaySound("flapSound");
                        rotation = flapRotation;
                    }
                }
                else
                {
                    isJumpPressed = false;

                    // Give's the bird a rotating effect when falling.
                    if (rotation > 0)
                    {
                        rotation -= gravityRotationSpeed * deltaTime;
                        if (rotation < 0) rotation = 0;
                    }
                }
            }
            // Apply constant gravity to player.
            position.Y += gravity * deltaTime;

            if (gameManager.CurrentState != GameManager.GameState.GameOver) 
            {
                // Check if player has collided with the ground or ceiling.
                if (position.Y > gameManager.Graphics.PreferredBackBufferHeight || position.Y < 0)
                {
                    gameManager.GameOver();
                    gameManager.CurrentState = GameManager.GameState.GameOver;
                }
            }

            if (gravity < maxGravity)
            {
                gravity += deltaTime * gravityDownSpeed;
            }
            previousState = currentState;  
        }
        private void UpdatePlayerCollider()
        {
            // Update collider position based on player's position.
            collider.X = (int)position.X - colliderXOffset;
            collider.Y = (int)position.Y - colliderYOffset;
        }

        public void Die()
        {
            // Have player fall off screen on death.
            movementDirection = new Vector2(0, gravityDownSpeed);
            rotation = fallRotation;
            canJump = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if (sprite != null)
            {
                // Draw the sprite with rotation applied.
                gameManager.SpriteBatch.Draw(
                    sprite,
                    new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height),
                    null,
                    Color.White,
                    rotation, 
                    // Center the rotation on the sprite.
                    new Vector2(sprite.Width / 2, sprite.Height / 2), 
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }   
}
