using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

public class ControlsScreen : GameScreen
{
    public ControlsScreen(GameState gameState)
    {
        _gameState = gameState;
        _keyBindings = _gameState.getKeyBindings();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_spriteFont, "Controls", new Vector2(100, 100), Color.White);
        int i = 0;
        foreach (var item in _keyBindings)
        {
            Color color = (i == _selectedEntry) ? Color.Red : Color.White;
            string key = item.Key.ToString();
            spriteBatch.DrawString(_spriteFont, item.Name + " - " + key, new Vector2(100, 150 + 50 * i), color);
            i++;
        }
        if (_listeningForKey)
        {
            spriteBatch.DrawString(_spriteFont, "Press a key for " + _keyBindings[_selectedEntry].Name, new Vector2(100, 150 + 50 * i), Color.Red);
        
        }
        spriteBatch.End();
    }

    public override void Initialize()
    {
    }

    public override void LoadContent(Game game)
    {
        _spriteFont = game.Content.Load<SpriteFont>("Arial");
    }

    public override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down) && !_listeningForKey)
        {
            _selectedEntry++;
            if (_selectedEntry >= _keyBindings.Count)
            {
                _selectedEntry = 0;
            }
            _lastKeyPressTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
        else if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up) && !_listeningForKey)
        {
            _selectedEntry--;
            if (_selectedEntry < 0)
            {
                _selectedEntry = _keyBindings.Count - 1;
            }
            _lastKeyPressTime = gameTime.TotalGameTime.TotalMilliseconds;
        }
        else if (keyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter) && !_listeningForKey && _clearedForInput)
        {
            _readyToListenForKey = true;
        }

        if (keyboardState.GetPressedKeyCount() == 0)
        {
            _clearedForInput = true;
        }

        if (keyboardState.GetPressedKeyCount() == 0 && _readyToListenForKey)
        {
            _listeningForKey = true;
            _readyToListenForKey = false;
        }

        if (_listeningForKey)
        {
            foreach (var key in Enum.GetValues(typeof(Keys)))
            {
                if (keyboardState.IsKeyDown((Keys)key))
                {
                    _keyBindings[_selectedEntry].Key = (Keys)key;
                    _listeningForKey = false;
                }
            }
        }
        _previousKeyboardState = keyboardState;
    }
    private SpriteFont _spriteFont;
    private GameState _gameState;
    private int _selectedEntry = 0;
    private List<KeyBindAction> _keyBindings;
    private KeyboardState _previousKeyboardState;
    private double _lastKeyPressTime = 0;
    private bool _listeningForKey = false;
    private bool _readyToListenForKey = false;
    private bool _clearedForInput = false;
}