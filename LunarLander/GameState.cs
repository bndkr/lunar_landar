using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using LunarLander;
using Microsoft.Xna.Framework.Input;
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
        menuEntries.Add(new MenuItem { Name = "Start Game", Action = (gameState) => gameState.PushScreen(new LunarScreen(gameState, game.GraphicsDevice)) });
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
    }

    public void RotateLanderLeft()
    {

    }

    public void RotateLanderRight()
    {

    }
    public void ThrustLander()
    {

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

    public List<KeyBindAction> getKeyBindings()
    {
        return _keyActions;
    }
    bool _readyToClose = false;

    private List<KeyBindAction> _keyActions = new List<KeyBindAction>();
    private Stack<GameScreen> _screens = new Stack<GameScreen>();
    private Game1 _game;
}