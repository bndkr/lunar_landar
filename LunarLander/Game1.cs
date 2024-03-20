using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LunarLander;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        gameState = new GameState(this);
        _graphics.PreferredBackBufferWidth = 1200;
        _graphics.PreferredBackBufferHeight = 800;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        gameState.GetScreen().Initialize();
        gameState.GetScreen().LoadContent(this);
    }

    protected override void Update(GameTime gameTime)
    {
        gameState.GetScreen().Update(gameTime);
        if (gameState.IsReadyToClose())
            Exit();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        gameState.GetScreen().Draw(_spriteBatch);
        base.Draw(gameTime);
    }
    GameState gameState;
}
