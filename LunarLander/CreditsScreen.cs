using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class CreditsScreen : GameScreen
{
    public CreditsScreen(GameState gameState)
        : base()
    {
        _gameState = gameState;
    }
    public override void Initialize()
    {
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_spriteFont, "Lunar Lander", new Vector2(100, 100), Color.White);
        spriteBatch.DrawString(_spriteFont, "Created by:", new Vector2(100, 150), Color.White);
        spriteBatch.DrawString(_spriteFont, "Ben Decker", new Vector2(100, 200), Color.White);
        spriteBatch.DrawString(_spriteFont, "Press the 'any' key to return", new Vector2(100, 250), Color.White);
        spriteBatch.End();
    }
    public override void Update(GameTime gameTime)
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
    public override void LoadContent(Game game)
    {
        _spriteFont = game.Content.Load<SpriteFont>("Arial");
    }
    private SpriteFont _spriteFont;
    private GameState _gameState;
    private bool _listeningForKey = false;
}