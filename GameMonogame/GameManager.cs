using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        // Game entitie references.
        Clouds background;
        Player player;
        PipeManager pipeManager;

        // List of game entities.
        public List<GameEntity> GameEntities { get { return _gameEntities; } set { _gameEntities = value; } }
        private List<GameEntity> _gameEntities = new List<GameEntity>();
        public List<GameEntity> EntitiesToAdd { get { return _entitiesToAdd; } set { _entitiesToAdd = value; } }
        private List<GameEntity> _entitiesToAdd = new List<GameEntity>();
        public List<GameEntity> EntitiesToRemove { get { return _entitiesToRemove; } set { _entitiesToRemove = value; } }
        private List<GameEntity> _entitiesToRemove = new List<GameEntity>();

        // Managers for score and UI.
        public ScoreManager ScoreManager { get { return scoreManager; } }
        ScoreManager scoreManager;
        UIManager uiManager;

        // List of entities to remove from the game.
        private List<GameEntity> toRemove = new List<GameEntity>();
        private int screenHeight;

        // Random instance for pipe generation.
        Random random = new Random();
        float deltaTime;
        KeyboardState keyboardState;

        // Track the game state.
        public GameState CurrentState { get { return _currentState; } set { _currentState = value; } }
        private GameState _currentState;

        public enum GameState
        {
            Init,
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
            _currentState = GameState.Init;
        }

        protected override void LoadContent()
        {
            // Load sprite batch and font for score.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var scoreFont = Content.Load<SpriteFont>("scoreFont");
            var gameFont = Content.Load<SpriteFont>("gameFont");
            var instructionFont = Content.Load<SpriteFont>("instructionFont");

            // Initialize UIManager
            uiManager = new UIManager(this, _spriteBatch, scoreFont, gameFont, instructionFont);
            SoundManager.LoadSounds(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Calculate delta time.
            deltaTime = gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            switch (_currentState)
            {
                case GameState.Init:
                    background = new Clouds(this, new Vector2(0, 0));
                    player = new Player(this, new Vector2(100, 100));
                    pipeManager = new PipeManager(this, new Vector2(100, 100));
                    _gameEntities.Add(player);
                    _gameEntities.Add(pipeManager);

                    // Initialize score manager.
                    scoreManager = new ScoreManager(this);
                    CurrentState = GameState.Waiting;
                    break;
                case GameState.Waiting:
                    background.Update(deltaTime);
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        _currentState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    background.Update(deltaTime);
                    UpdateEntities(deltaTime);
                    break;
                case GameState.GameOver:
                    player.Update(deltaTime);
                    if (keyboardState.IsKeyDown(Keys.R))
                    {
                        RestartGame();
                    }
                    break;
            }  
            base.Update(gameTime);
        }

        private void UpdateEntities(float deltaTime)
        {
            if (_entitiesToAdd.Count > 0)
            {
                _gameEntities.AddRange(_entitiesToAdd);
                _entitiesToAdd.Clear();
            }
            // Responsible for updating all entities.
            foreach (var entity in _gameEntities)
            {
                entity.Update(deltaTime);
            }
            var entitiesToRemove = _gameEntities.Where(entity => entity is Pipe pipe && pipe.HasPassed).ToList();

            // Remove entities marked for removal.
            foreach (var entity in entitiesToRemove)
            {
                _gameEntities.Remove(entity);
            }
        }

        private void RestartGame()
        {
            // Reset game entities, score, and pipe manager.
            _gameEntities.Clear();
            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();
            background = new Clouds(this, new Vector2(0, 0));
            player = new Player(this, new Vector2(100, 100));
            pipeManager = new PipeManager(this, new Vector2(100, 100));
            _gameEntities.Add(player);
            _gameEntities.Add(pipeManager);
            scoreManager.ResetScore();
            pipeManager.ClearAll();
            _currentState = GameState.Playing;
        }
     
        public void GameOver()
        {
            SoundManager.PlaySound("loseSound");
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

            _spriteBatch.End();

            uiManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
} 