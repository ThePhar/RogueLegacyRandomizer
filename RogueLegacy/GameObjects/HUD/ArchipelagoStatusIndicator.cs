// Rogue Legacy Randomizer - ArchipelagoStatusIndicator.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using Archipelago;
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
        _spriteFont = _content.Load<SpriteFont>("Fonts\\Arial12");
    }

    protected override void UnloadContent()
    {
        _content.Unload();
    }

    public override void Update(GameTime gameTime)
    {
        _status = Program.Game.ArchipelagoManager.ConnectionStatus switch
        {
            ConnectionStatus.Disconnected  => "Disconnected",
            ConnectionStatus.Disconnecting => "Disconnecting",
            ConnectionStatus.Connecting    => "Connecting",
            ConnectionStatus.Connected     => "Connected",
            _                              => $"Unknown {Program.Game.ArchipelagoManager.ConnectionStatus}"
        };
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_spriteFont, _status, new Vector2(64f, 33f), Color.Black);
        _spriteBatch.DrawString(_spriteFont, _status, new Vector2(65f, 32f), Color.White);
        _spriteBatch.End();
    }
}
