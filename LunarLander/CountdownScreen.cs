using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CountdownScreen : GameScreen
{
    private SpriteFont _spriteFont;
    private int _countdown;

    public CountdownScreen(GameScreen nextScreen, GameState gameState)
    {
        _nextScreen = nextScreen;
        _gameState = gameState;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_spriteFont, _countdown.ToString(), new Vector2(100, 100), Color.White);
        spriteBatch.End();
    }

    public override void Initialize()
    {
        _countdown = 3;
    }

    public override void LoadContent(Game game)
    {
        _spriteFont = game.Content.Load<SpriteFont>("Arial");
    }

    public override void Update(GameTime gameTime)
    {
        // Decrease the countdown by 1 every second
        if (gameTime.TotalGameTime.TotalSeconds >= 1)
        {
            _countdown--;
            gameTime.TotalGameTime = TimeSpan.Zero; // Reset the game time
        }

        // Countdown finished, transition to the next screen
        if (_countdown == 0)
        {
            _gameState.PopScreen();
            _gameState.PushScreen(_nextScreen);
        }
    }
    private GameScreen _nextScreen;
    private GameState _gameState;
}