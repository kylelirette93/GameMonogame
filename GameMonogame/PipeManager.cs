using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;


namespace GameMonogame
{
    public class PipeManager : GameEntity
    {
        // Variables for pipe generation.
        private GameManager _gameManager;
        private float _lastPipePositionX = 500;
        private float _pipeSpawnTimer = 0f;
        private float _pipeSpawnInterval = 2f;
        private int _pipeMinY = -300;
        private int _pipeMaxY = -50;
        private int _pipeSpawnDistance = 250;
        private int _randomYPosition;
        private List<GameEntity> _pipesToRemove = new List<GameEntity>();
        Random random = new Random();

        public PipeManager(GameManager game, Vector2 initialPosition) : base(game, initialPosition)
        {
            _gameManager = game;
        }

        public override void Update(float deltaTime)
        {
            // Handles pipe spawning and removal.
            _pipeSpawnTimer += deltaTime;
            RemoveOffscreenPipes();
            SpawnPipes();
            CheckCollisions();
        }

        private void RemoveOffscreenPipes()
        {
            foreach (var entity in _gameManager.GameEntities.Where(entity => entity is Pipe pipe && pipe.HasPassed))
            {
                gameManager.EntitiesToRemove.Add(entity);
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

                _pipeSpawnTimer = 0f;
            }
        }

        private void CreatePipe(Vector2 pipePosition)
        {
            // Instantiate a new pipe and add it to list of entities.
            Pipe pipe = new Pipe(_gameManager, pipePosition);
            _gameManager.EntitiesToAdd.Add(pipe);
        }

        private void CheckCollisions()
        {
            // Iterate through the list of game entities.
            for (int i = 0; i < gameManager.GameEntities.Count; i++)
            {
                for (int j = i + 1; j < gameManager.GameEntities.Count; j++)
                {
                    GameEntity entityA = gameManager.GameEntities[i];
                    GameEntity entityB = gameManager.GameEntities[j];

                    // Check for player and pipe.
                    if (entityA is Player player && entityB is Pipe pipe)
                    {
                        // Check if player collides with top or bottom of pipe.
                        if (player.Collider.Intersects(pipe.topOfPipe) ||
                            player.Collider.Intersects(pipe.bottomOfPipe))
                        {
                            // Player collides with pipe. End the game.
                            player.OnCollide(pipe);
                            gameManager.CurrentState = GameManager.GameState.GameOver;
                            return;
                        }

                        if (player.Collider.Right > pipe.topOfPipe.Right && !pipe.HasScored)
                        {
                            // Player has passed the pipe, increment score once.
                            _gameManager.ScoreManager.AddScore(1);
                            pipe.HasScored = true;
                        }
                    }
                }
            }
        }

        public void ClearAll()
        {
            // Clear all the pipes and reset pipe spawn position.
           _gameManager.GameEntities.RemoveAll(entity => entity is Pipe);
            _lastPipePositionX = 500;
        }
    }
}
