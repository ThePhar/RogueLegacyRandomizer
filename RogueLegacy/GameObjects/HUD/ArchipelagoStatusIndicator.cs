using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLegacy.GameObjects.HUD;

public class ArchipelagoStatusIndicator : DrawableGameComponent
{
    private readonly ContentManager _content;
    private          SpriteBatch    _spriteBatch;
    private          SpriteFont     _spriteFont;
    private          string         _status = "Unknown";

    public ArchipelagoStatusIndicator(Game game) : base(game)
    {
        _content = Game.Content;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spriteFont = _content.Load<SpriteFont>(@"Fonts\Arial12");
    }

    protected override void UnloadContent()
    {
        _content.Unload();
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_spriteFont, _status, new Vector2(64f, 33f), Color.Black);
        _spriteBatch.DrawString(_spriteFont, _status, new Vector2(65f, 32f), Color.White);
        _spriteBatch.End();
    }
}
