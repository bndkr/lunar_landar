using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections.Generic;
using System;

public enum LunarLevel
{
    Easy,
    Hard
}

public class LunarScreen : GameScreen
{
    public LunarScreen(GameState gameState, GraphicsDevice device, LunarLevel level)
    {
        _gameState = gameState;
        _device = device;
        _level = level;
        _terrain = new Terrain(device.Viewport.Width, device.Viewport.Height, (level == LunarLevel.Easy) ? 2 : 1, (level == LunarLevel.Easy) ? 150 : 100);
        _basicEffect = new BasicEffect(_device);
        _basicEffect.VertexColorEnabled = true;
        _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, _device.Viewport.Width, _device.Viewport.Height, 0, 0, 1);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_nebula, new Rectangle(0, 0, _device.Viewport.Width, _device.Viewport.Height), Color.White);
        spriteBatch.End();

        foreach (var pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _terrain.Draw(spriteBatch);
        }

        spriteBatch.Begin();
        if (_gameState._crashed)
        {
            spriteBatch.DrawString(_spriteFont, "You Crashed! Press Enter to return to main menu.", new Vector2(200, 100), Color.White);
        }
        else
        {
            spriteBatch.Draw(_lander, new Vector2(_gameState._landerX, _gameState._landerY), null, Color.White, (float)Math.Atan2(_gameState._landerRotationVectorY, _gameState._landerRotationVectorX), new Vector2(_lander.Width / 2, _lander.Height / 2), 1.0f, SpriteEffects.None, 0);

        }
        if (_gameState._gameOver)
        {
            spriteBatch.DrawString(_spriteFont, "You Win! Press Enter to return to main menu.", new Vector2(100, 100), Color.White);
        }
        else
        {
            var degrees = MathHelper.ToDegrees((float)(Math.Atan2(_gameState._landerRotationVectorY, _gameState._landerRotationVectorX) + MathHelper.ToRadians(90)));
            var speedSquared = _gameState._landerVelocityX * _gameState._landerVelocityX + _gameState._landerVelocityY * _gameState._landerVelocityY;
            spriteBatch.DrawString(_spriteFont, "Fuel: " + _gameState.Fuel, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(_spriteFont, "Velocity: " + Math.Sqrt(speedSquared) * 2, new Vector2(10, 50), Math.Sqrt(speedSquared) < 1 ? Color.Green : Color.White);
            spriteBatch.DrawString(_spriteFont, "Rotation Degrees: " + degrees, new Vector2(10, 110), Math.Abs(degrees) < 10 ? Color.Green : Color.White);
        }
        spriteBatch.End();

        _gameState._particleSystem.Draw(spriteBatch, new Vector2(0, 0));
    }

    public override void Initialize()
    {
    }

    public override void LoadContent(Game game)
    {
        _spriteFont = game.Content.Load<SpriteFont>("Arial");
        _lander = game.Content.Load<Texture2D>("lander");
        _nebula = game.Content.Load<Texture2D>("nebula");
    }

    public override void Update(GameTime game)
    {
        var keyboardState = Keyboard.GetState();
        Keys leftKey = _gameState.getKeyBindings().Find((keyBind) => keyBind.Action == KeyAction.RotateLeft).Key;
        Keys rightKey = _gameState.getKeyBindings().Find((keyBind) => keyBind.Action == KeyAction.RotateRight).Key;
        Keys thrustKey = _gameState.getKeyBindings().Find((keyBind) => keyBind.Action == KeyAction.Thrust).Key;
        if (keyboardState.IsKeyDown(leftKey))
        {
            _gameState.RotateLanderLeft();
        }
        if (keyboardState.IsKeyDown(rightKey))
        {
            _gameState.RotateLanderRight();
        }
        if (keyboardState.IsKeyDown(thrustKey))
        {
            _gameState.ThrustLander();
        }
        else
        {
            if (!_gameState._crashed && !_gameState._gameOver)
                _gameState._particleSystem.StopThrust();
        }
        if (_gameState._crashed || _gameState._gameOver)
        {
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                _gameState.ResetLander();
                _gameState.PopScreensToMainMenu();
            }
        }
        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            _gameState.PushScreen(new MenuScreen(_gameState, new List<MenuItem>
            {
                new MenuItem { Name = "Resume", Action = (gameState) => gameState.PopScreen() },
                new MenuItem { Name = "Quit", Action = (gameState) => {
                    gameState.ResetLander();
                    gameState.PopScreen();
                    gameState.PopScreen();
                     }}
            }));
        }
        _gameState._particleSystem.Update(game);
        if (!_gameState._crashed && !_gameState._gameOver)
        {
            _gameState.UpdateLander();
        }
        if (_terrain.CollidesWithRectangle(_gameState._landerX + 40, _device.Viewport.Height - (int)_gameState._landerY, 80, 80) && !_gameState._crashed)
        {
            if (_gameState._landerVelocityX * _gameState._landerVelocityX + _gameState._landerVelocityY * _gameState._landerVelocityY < 1 &&
            _terrain.IsXValueInLandingZone(_gameState._landerX + 40))
            {
                if (_level == LunarLevel.Easy)
                {
                    _gameState.PushScreen(new MenuScreen(_gameState, new List<MenuItem>
                    {
                        new MenuItem { Name = "You Landed Safely! Press enter to continue", Action = (gameState) => {
                            gameState.PopScreen();
                            gameState.ResetLander();
                            gameState.PushScreen(new CountdownScreen(new LunarScreen(gameState, _device, LunarLevel.Hard), gameState));
                        }}
                    }));
                }
                else
                {
                    _gameState._gameOver = true;
                }
            }
            else
            {
                _gameState._particleSystem.ShipCrash(new Vector2(_gameState._landerX, _gameState._landerY));
                _gameState._crashed = true;
                //     _gameState.PushScreen(new MenuScreen(_gameState, new List<MenuItem>
                // {
                //     new MenuItem { Name = "You Crashed!", Action = (gameState) => {} },
                //     new MenuItem { Name = "Quit", Action = (gameState) => {
                //         gameState.ResetLander();
                //         gameState.PopScreen();
                //         gameState.PopScreen(); }}
                // }));

            }

        }
    }

    private GameState _gameState;
    private SpriteFont _spriteFont;
    private Terrain _terrain;
    private GraphicsDevice _device;
    private BasicEffect _basicEffect;
    private Texture2D _lander;
    private Texture2D _nebula;
    private LunarLevel _level;
}