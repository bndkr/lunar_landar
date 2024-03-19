using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public abstract class GameScreen
{
    public abstract void Initialize();
    public abstract void Update(GameTime game);
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void LoadContent(Game game);
}
