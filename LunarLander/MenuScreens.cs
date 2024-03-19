using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
public delegate void MenuAction(GameState gameState);

public class MenuItem
{
    public string Name;
    public MenuAction Action;
}

public class MenuScreen : GameScreen
{
    public MenuScreen(GameState gameState, List<MenuItem> items)
    {
        _gameState = gameState;
        _menuEntries = items;
    }
    public override void Initialize()
    {
    }
    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down))
        {
            _selectedEntry++;
            if (_selectedEntry >= _menuEntries.Count)
            {
                _selectedEntry = 0;
            }
            _lastKeyPressTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
        else if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
        {
            _selectedEntry--;
            if (_selectedEntry < 0)
            {
                _selectedEntry = _menuEntries.Count - 1;
            }
            _lastKeyPressTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
        else if (keyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter))
        {
            _menuEntries[_selectedEntry].Action(_gameState);
        }

        _previousKeyboardState = keyboardState;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        for (int i = 0; i < _menuEntries.Count; i++)
        {
            var entry = _menuEntries[i];
            Color color = (i == _selectedEntry) ? Color.Red : Color.White;
            spriteBatch.DrawString(_spriteFont, entry.Name, new Vector2(100, 100 + 50 * i), color);
        }
        spriteBatch.End();
    }
    public override void LoadContent(Game game)
    {
        _spriteFont = game.Content.Load<SpriteFont>("Arial");
    }
    private int _selectedEntry = 0;
    protected SpriteFont _spriteFont;
    private List<MenuItem> _menuEntries = new List<MenuItem>();

    private KeyboardState _previousKeyboardState;
    private double _lastKeyPressTime = 0;
    private GameState _gameState;
}