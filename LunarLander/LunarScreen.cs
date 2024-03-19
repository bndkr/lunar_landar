using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics.PackedVector;


public class LunarScreen : GameScreen
{
    public LunarScreen(GameState gameState, GraphicsDevice device)
    {
        _gameState = gameState;
        _device = device;
        _terrain = new Terrain(device.Viewport.Width, device.Viewport.Height);
        _basicEffect = new BasicEffect(_device);
        _basicEffect.VertexColorEnabled = true;
        _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, _device.Viewport.Width, _device.Viewport.Height, 0, 0, 1);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        // var basicEffect = new BasicEffect(_device);
        // basicEffect.VertexColorEnabled = true;
        // basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, _device.Viewport.Width, _device.Viewport.Height, 0, 0, 1);

        foreach (var pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _terrain.Draw(spriteBatch);
        }
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
    }

    private GameState _gameState;
    private SpriteFont _spriteFont;
    private Terrain _terrain;
    private GraphicsDevice _device;
    private BasicEffect _basicEffect;
}