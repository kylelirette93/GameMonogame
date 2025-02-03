using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMonogame
{
    internal class GameEntity
    {
        public Texture2D Sprite { get { return sprite; } }
        protected Texture2D sprite;
        protected Vector2 position;
        public Vector2 Position { get { return position; } }
        protected GameManager gameManager;

        protected Rectangle collider;
        public Rectangle Collider { get { return collider; } }
        protected float movementSpeed;
        protected Vector2 movementDirection;
        Vector2 previousPosition;
        Texture2D _colliderTexture;


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
            gameManager.SpriteBatch.Draw(colliderTexture, collider, Color.Red);
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
            DrawCollider(_colliderTexture, collider);
            if (sprite != null) 
            {
                gameManager.SpriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y,
                    sprite.Width, sprite.Height), Color.White);
            }
        }
    }
    }
