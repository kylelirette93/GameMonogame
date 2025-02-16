using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameMonogame
{
    public class UIManager
    {
        private GameManager _gameManager;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private SpriteFont _gameFont;
        private SpriteFont _instructionFont;
        private Vector2 _scoreTextPosition = new Vector2(0, 0);
        private Vector2 _dropShadowOffset = new Vector2(2, 2);
        private Vector2 _titlePosition = new Vector2(300, 150);
        private Vector2 _startInstructionPosition = new Vector2(225, 400);
        private Vector2 _gameOverPosition = new Vector2(200, 150);

        // Reusable strings to help with memory allocation.
        private readonly string _titleText = "Flappy Bird";
        private readonly string _startInstructionText = "Press Space to start";
        private readonly string _gameOverTextTemplate = "     GAME OVER!\n     Score: {0}\n Press R to restart\n  Press ESC to quit";
        private string _gameOverText;

        public UIManager(GameManager gameManager, SpriteBatch spriteBatch, SpriteFont font, SpriteFont gameFont, SpriteFont instructionFont)
        {
            _gameManager = gameManager;
            _spriteBatch = spriteBatch;
            _font = font;
            _gameFont = gameFont;
            _instructionFont = instructionFont;
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // Display UI Text based on game state.
            if (_gameManager.CurrentState == GameManager.GameState.Waiting)
            {
                _spriteBatch.DrawString(_font, _titleText, _titlePosition + _dropShadowOffset, Color.Black);
                _spriteBatch.DrawString(_font, _titleText, _titlePosition, Color.White);
                _spriteBatch.DrawString(_font, _startInstructionText, _startInstructionPosition, Color.Black);
            }
            if (_gameManager.CurrentState == GameManager.GameState.Playing)
            {
                _spriteBatch.DrawString(_font, $"Score: {_gameManager.ScoreManager.Score}", _scoreTextPosition + _dropShadowOffset, Color.Black);
                _spriteBatch.DrawString(_font, $"Score: {_gameManager.ScoreManager.Score}", _scoreTextPosition, Color.White);
            }
            if (_gameManager.CurrentState == GameManager.GameState.GameOver)
            {
                _gameOverText = string.Format(_gameOverTextTemplate, _gameManager.ScoreManager.Score);
                _spriteBatch.DrawString(_font, _gameOverText, _gameOverPosition + _dropShadowOffset, Color.Black);
                _spriteBatch.DrawString(_font, _gameOverText, _gameOverPosition, Color.White);
            }

            _spriteBatch.End();
        }
    }
}
