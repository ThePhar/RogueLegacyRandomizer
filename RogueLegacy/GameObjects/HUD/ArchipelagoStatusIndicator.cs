// RogueLegacyRandomizer - ArchipelagoStatusIndicator.cs
// Last Modified 2023-07-27 12:14 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;

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
        _status = ArchipelagoManager.Status.ToString();
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_spriteFont, _status, new Vector2(64f, 33f), Color.Black);
        _spriteBatch.DrawString(_spriteFont, _status, new Vector2(65f, 32f), Color.White);
        _spriteBatch.End();
    }
}