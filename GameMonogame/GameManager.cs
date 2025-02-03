using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace GameMonogame
{
    public class GameManager : Game
    {
        // Properties for access to the GraphicsDeviceManager and SpriteBatch.
        private GraphicsDeviceManager _graphics;
        public GraphicsDeviceManager Graphics { get { return _graphics; } }
        private SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        // Game entities.
        Clouds background;
        Player player;
        // List to hold all game entities.
        private List<GameEntity> _gameEntities = new List<GameEntity>();

        // Score manager and font.
        ScoreManager scoreManager;
        SpriteFont scoreFont;

        SoundEffect loseSound;

        // Fonts for game state and instructions.
        SpriteFont gameFont;
        SpriteFont instructionFont;

        // List of entities to remove from the game.
        private List<GameEntity> toRemove = new List<GameEntity>();
        int screenHeight;

        // Variables for pipe generation.
        private float _lastPipePositionX = 500;
        private float _pipeSpawnTimer = 0f;
        private float _pipeSpawnInterval = 2f;
        private int _pipeMinY = -300;
        private int _pipeMaxY = -50;
        private int _pipeSpawnDistance = 250;
        private int _randomYPosition;

        // Random instance for pipe generation.
        Random random = new Random();

        // Track the game state.
        private GameState _currentState;
        enum GameState
        {
            Waiting,
            Playing,
            GameOver
        }

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Initialize entities.
            background = new Clouds(this, new Vector2(0, 0));
            player = new Player(this, new Vector2(100, 100));
            _gameEntities.Add(player);

            // Initialize score manager.
            scoreManager = new ScoreManager(this);    
            
            _currentState = GameState.Waiting;
        }

        protected override void LoadContent()
        {
            // Load sprite batch and font for score.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            scoreFont = Content.Load<SpriteFont>("Score");
            gameFont = Content.Load<SpriteFont>("Game");
            instructionFont = Content.Load<SpriteFont>("Instruction");
            loseSound = Content.Load<SoundEffect>("lose");
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            _pipeSpawnTimer += deltaTime;

            var keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (_currentState == GameState.Waiting)
            {
                // Have the clouds move while waiting.
                background.Update(deltaTime);
                
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    _currentState = GameState.Playing;
                }
            }
            else if (_currentState == GameState.Playing)
            {
                background.Update(deltaTime);
                // Call update method for each entity.
                UpdateEntities(deltaTime);
                RemoveOffscreenPipes();
                SpawnPipes();
                CheckCollisions();
            }
            else if (_currentState == GameState.GameOver)
            {
                player.Update(deltaTime);
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {         
                    RestartGame();
                }

            }

            base.Update(gameTime);
        }

        private void UpdateEntities(float deltaTime)
        {
            foreach (var entity in _gameEntities)
            {
                entity.Update(deltaTime);
            }
        }

        private void RestartGame()
        {
            _gameEntities.Clear();
            background = new Clouds(this, new Vector2(0, 0));
            player = new Player(this, new Vector2(100, 100));
            _gameEntities.Add(player);
            _currentState = GameState.Playing;
            scoreManager.ResetScore();
            _lastPipePositionX = 500;
        }
        private void RemoveOffscreenPipes()
        {
            foreach (var entity in _gameEntities.Where(entity => entity is Pipe pipe && pipe.Position.X + pipe.Sprite.Width < 0).ToList())
            {
                Pipe pipe = (Pipe)entity;
                pipe.HasPassed = false;
                _gameEntities.Remove(entity);           
            }
        }
        private void SpawnPipes()
        {
            if (_pipeSpawnTimer >= _pipeSpawnInterval)
            {
                // Spawn the next pipe ahead of the last pipe.
                _lastPipePositionX += _pipeSpawnDistance;
                _randomYPosition = random.Next(_pipeMinY, _pipeMaxY);
                CreatePipe(new Vector2(_lastPipePositionX, _randomYPosition));

                _pipeSpawnTimer = 0;
            }
        }

        public void CreatePipe(Vector2 pipePosition)
        {
            // Instantiate a new pipe and add it to list of entities.
            Pipe pipe = new Pipe(this, pipePosition);
            _gameEntities.Add(pipe);
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < _gameEntities.Count; i++)
            {
                for (int j = i + 1; j < _gameEntities.Count; j++)
                {
                    GameEntity entityA = _gameEntities[i];
                    GameEntity entityB = _gameEntities[j];

                    // Check for player and pipe.
                    if (entityA is Player player && entityB is Pipe pipe)
                    {
                        // Check if player collides with top or bottom of pipe.
                        if (player.Collider.Intersects(pipe.topOfPipe) ||
                            player.Collider.Intersects(pipe.bottomOfPipe))
                        {
                            // Player collides with pipe. End the game.
                            player.OnCollide(pipe);
                            _currentState = GameState.GameOver;
                            return; 
                        }

                        if (player.Collider.Right > pipe.topOfPipe.Right && player.Collider.Right > 
                            pipe.bottomOfPipe.Right && !pipe.HasPassed)
                        {
                            // Player has passed the pipe, increment score.
                            scoreManager.AddScore(1);
                            pipe.HasPassed = true;
                        }
                    }               
                }
            }
        }

        public void GameOver()
        {
            loseSound.Play();
            player.Die();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);

            _spriteBatch.Begin();

            if (background != null)
            {
                background.Draw(gameTime);
            }

            

            foreach (var entity in _gameEntities)
            {
                entity.Draw(gameTime);
            }
            if (scoreManager != null && scoreFont != null)
            {
                scoreManager.Draw(_spriteBatch, scoreFont);
            }
            if (_currentState == GameState.Waiting)
            {
                _spriteBatch.DrawString(gameFont, "Flappy Bird", new Vector2(250, 150) + new Vector2(2, 2), Color.Black);
                _spriteBatch.DrawString(gameFont, "Flappy Bird", new Vector2(250, 150), Color.White);
                _spriteBatch.DrawString(instructionFont, "Press Space to start", new Vector2(260, 400), Color.Black);
            }

            if (_currentState == GameState.GameOver)             {
                _spriteBatch.DrawString(gameFont, $"     GAME OVER!\n" +
                    $"     Score: {scoreManager.Score}\nPress R to restart", new Vector2(150, 150) + new Vector2(2, 2), Color.Black);
                _spriteBatch.DrawString(gameFont, $"     GAME OVER!\n" +
                   $"     Score: {scoreManager.Score}\nPress R to restart", new Vector2(150, 150), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
} 