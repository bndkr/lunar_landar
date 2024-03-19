using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class LeaderBoardScreen : GameScreen
{
    public LeaderBoardScreen(GameState gameState)
    {
        _gameState = gameState;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_spriteFont, "Leaderboard", new Vector2(100, 100), Color.White);
        spriteBatch.DrawString(_spriteFont, "1. Ben Decker", new Vector2(100, 150), Color.White);
        spriteBatch.DrawString(_spriteFont, "2. Ben Decker", new Vector2(100, 200), Color.White);
        spriteBatch.DrawString(_spriteFont, "3. Ben Decker", new Vector2(100, 250), Color.White);
        spriteBatch.DrawString(_spriteFont, "Press the 'any' key to return", new Vector2(100, 300), Color.White);
        spriteBatch.End();
    }

    public override void Initialize()
    {
    }

    public override void LoadContent(Game game)
    {
        _spriteFont = game.Content.Load<SpriteFont>("Arial");
    }

    public override void Update(GameTime game)
    {
        if (Keyboard.GetState().GetPressedKeys().Length == 0)
        {
            _listeningForKey = true;
        }
        if (_listeningForKey && Keyboard.GetState().GetPressedKeys().Length > 0)
        {
            _gameState.PopScreen();
        }
    }
    private SpriteFont _spriteFont;
    private bool _listeningForKey = false;
    private GameState _gameState;
}