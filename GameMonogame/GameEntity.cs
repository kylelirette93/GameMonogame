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
        // Protected variables that game entities can access.
        protected Texture2D sprite;
        protected Vector2 position;

        // Property to access collider in pipe manager.
        public Rectangle Collider { get { return collider; } }
        protected Rectangle collider;
        protected float movementSpeed;
        protected Vector2 movementDirection;
        protected Vector2 previousPosition;
        protected Texture2D _colliderTexture;
        protected GameManager gameManager;


        public GameEntity(GameManager game, Vector2 initialPosition)
        {
            gameManager = game;
            position = initialPosition;
            movementDirection = Vector2.Zero;
            _colliderTexture = new Texture2D(gameManager.GraphicsDevice, 1, 1);
            _colliderTexture.SetData(new Color[] { Color.Transparent });
            if (sprite != null)
            {
                collider = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            }
        }

        public virtual void Update(float deltaTime)
        {
            // Default update for game entities.
            previousPosition = position;

            position += movementDirection * deltaTime;
   
            if (position != previousPosition && sprite != null)
            {
               collider.X = (int)position.X;
               collider.Y = (int)position.Y;
            }
        }

        public void DrawCollider(Texture2D colliderTexture, Rectangle collider)
        {
            gameManager.SpriteBatch.Draw(colliderTexture, collider, Color.Transparent);
        }


        public virtual void OnCollide(GameEntity otherEntity)
        {
            if (this is Player && otherEntity is Pipe)
            {
                gameManager.GameOver();
            }
        }

        public virtual void Draw(GameTime gameTime) 
        {
            // Draw the game entities collider.
            DrawCollider(_colliderTexture, collider);
            if (sprite != null) 
            {
                gameManager.SpriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y,
                    sprite.Width, sprite.Height), Color.White);
            }
        }
    }
    }
