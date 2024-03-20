using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using LunarLander;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
public enum KeyAction
{
    RotateLeft,
    RotateRight,
    Thrust
}

public class KeyBindAction
{
    public KeyBindAction(String name, KeyAction action, Keys key)
    {
        Name = name;
        Action = action;
        Key = key;
    }
    public String Name { get; set; }
    public KeyAction Action { get; set; }
    public Keys Key { get; set; }
}

public class GameState
{
    public GameState(Game1 game)
    {
        _game = game;
        var menuEntries = new List<MenuItem>();
        menuEntries.Add(new MenuItem { Name = "Start Game", Action = (gameState) => { 
            gameState.PushScreen(new CountdownScreen(new LunarScreen(gameState, game.GraphicsDevice, LunarLevel.Easy), gameState));
        } });
        menuEntries.Add(new MenuItem
        {
            Name = "Options",
            Action = (gameState) =>
            {
                gameState.PushScreen(new MenuScreen(gameState, new List<MenuItem>
                {
                    new MenuItem { Name = "Sound", Action = (gameState) => { } },
                    new MenuItem { Name = "Controls", Action = (gameState) => {
                        gameState.PushScreen(new ControlsScreen(gameState));
                    } },
                    new MenuItem { Name = "Back", Action = (gameState) => gameState.PopScreen() }
                }));
            }
        });
        menuEntries.Add(new MenuItem { Name = "Leaderboard", Action = (gameState) => gameState.PushScreen(new LeaderBoardScreen(gameState)) });
        menuEntries.Add(new MenuItem { Name = "Credits", Action = (gameState) => gameState.PushScreen(new CreditsScreen(gameState)) });
        menuEntries.Add(new MenuItem { Name = "Exit", Action = (gameState) => gameState.Close() });
        _screens.Push(new MenuScreen(this, menuEntries));
        // default controls
        _keyActions.Add(new KeyBindAction("Rotate Left", KeyAction.RotateLeft, Keys.Left));
        _keyActions.Add(new KeyBindAction("Rotate Right", KeyAction.RotateRight, Keys.Right));
        _keyActions.Add(new KeyBindAction("Apply Thrust", KeyAction.Thrust, Keys.Up));
        _particleSystem = new LunarParticleSystem(_game);
        ResetLander();
    }

    public void RotateLanderLeft()
    {
        if (_gameOver || _crashed)
            return;
        var radians = 0.05;
        var originalX = _landerRotationVectorX;
        var originalY = _landerRotationVectorY;
        _landerRotationVectorX = (float)(Math.Cos(-radians) * originalX - Math.Sin(-radians) * originalY);
        _landerRotationVectorY = (float)(Math.Sin(-radians) * originalX + Math.Cos(-radians) * originalY);
    }

    public void RotateLanderRight()
    {
        if (_gameOver || _crashed)
            return;
        var radians = 0.05;
        var originalX = _landerRotationVectorX;
        var originalY = _landerRotationVectorY;
        _landerRotationVectorX = (float)(Math.Cos(radians) * originalX - Math.Sin(radians) * originalY);
        _landerRotationVectorY = (float)(Math.Sin(radians) * originalX + Math.Cos(radians) * originalY);
    }
    public void ThrustLander()
    {
        if (_gameOver || _crashed)
            return;
        _landerVelocityX += _landerRotationVectorX * 0.05f;
        _landerVelocityY += _landerRotationVectorY * 0.05f;
        Fuel--;
        _particleSystem.Thrust(new Vector2(_landerX, _landerY), new Vector2(_landerRotationVectorX, _landerRotationVectorY));
    }

    public void ResetLander()
    {
        _landerX = 100;
        _landerY = 100;
        _landerRotationVectorX = 1;
        _landerRotationVectorY = 0;
        _landerVelocityX = 0;
        _landerVelocityY = 0;
        _crashed = false;
        _gameOver = false;
        Fuel = 100;
        _particleSystem.Reset();
    }

    public void BindKey(Keys key, KeyAction action)
    {
        if (_keyActions.Find((ka) => ka.Action == action) != null)
        {
            var ka = _keyActions.Find((ka) => ka.Action == action);
            ka.Key = key;
        }
    }
    public void Close()
    {
        _readyToClose = true;
    }

    public GameScreen GetScreen()
    {
        if (_screens.Count != 0)
            return _screens.Peek();
        throw new InvalidOperationException("No active screen");
    }
    public bool IsReadyToClose()
    {
        return _readyToClose;
    }

    public void PushScreen(GameScreen screen)
    {
        _screens.Push(screen);
        _screens.Peek().Initialize();
        _screens.Peek().LoadContent(_game);
    }
    public void PopScreen()
    {
        if (_screens.Count > 1)
            _screens.Pop();
    }

    public void PopScreensToMainMenu()
    {
        while (_screens.Count > 1)
            _screens.Pop();
    }

    public bool SafeLanding()
    {
        return _landerVelocityX * _landerVelocityX + _landerVelocityY * _landerVelocityY < 1
        && Math.Abs(Math.Atan2(_landerRotationVectorY, _landerRotationVectorX)) < MathHelper.ToRadians(5);
    }

    public void UpdateLander()
    {
        if (_gameOver)
            return;
        _landerX += _landerVelocityX;
        _landerY += _landerVelocityY;
        _landerVelocityY += 0.01f;
    }
    public List<KeyBindAction> getKeyBindings()
    {
        return _keyActions;
    }
    bool _readyToClose = false;

    private List<KeyBindAction> _keyActions = new List<KeyBindAction>();
    private Stack<GameScreen> _screens = new Stack<GameScreen>();
    private Game1 _game;

    private int _score = 0;
    public float _landerX;
    public float _landerY;
    public float _landerRotationVectorX;
    public float _landerRotationVectorY;
    public float _landerVelocityX;
    public float _landerVelocityY;
    public int Fuel = 1000;
    public bool _gameOver = false;
    public LunarParticleSystem _particleSystem;
    public bool _crashed = false;

}