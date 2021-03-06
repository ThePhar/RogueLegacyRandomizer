//
//  Rogue Legacy Randomizer - ArchipelagoStatusIndicator.cs
//  Last Modified 2022-01-03
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using Archipelago;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
    public class ArchipelagoStatusIndicator : DrawableGameComponent
    {
        private readonly ContentManager m_content;
        private SpriteBatch m_spriteBatch;
        private SpriteFont m_spriteFont;
        private string m_status = "";

        public ArchipelagoStatusIndicator(Game game) : base(game)
        {
            m_content = Game.Content;
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_spriteFont = m_content.Load<SpriteFont>("Fonts\\Arial12");
        }

        protected override void UnloadContent()
        {
            m_content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            switch (Program.Game.ArchipelagoManager.ConnectionStatus)
            {
                case ConnectionStatus.Disconnected:
                    m_status = "Disconnected";
                    break;

                case ConnectionStatus.Disconnecting:
                    m_status = "Disconnecting";
                    break;

                case ConnectionStatus.Connecting:
                    m_status = "Connecting";
                    break;

                case ConnectionStatus.Connected:
                    m_status = "Connected";
                    break;

                default:
                    m_status = "Unknown Status " + (int) Program.Game.ArchipelagoManager.ConnectionStatus;
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.DrawString(m_spriteFont, m_status, new Vector2(64f, 33f), Color.Black);
            m_spriteBatch.DrawString(m_spriteFont, m_status, new Vector2(65f, 32f), Color.White);
            m_spriteBatch.End();
        }
    }
}
