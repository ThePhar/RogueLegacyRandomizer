// Rogue Legacy Randomizer - FrameRateCounter.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLegacy
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private readonly ContentManager content;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private int frameCounter;
        private int frameRate;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public FrameRateCounter(Game game) : base(game)
        {
            content = Game.Content;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts\\FpsFont");
        }

        protected override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1.0))
            {
                elapsedTime -= TimeSpan.FromSeconds(1.0);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
            var text = string.Format("fps: {0}", frameRate);
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, text, new Vector2(1220f, 33f), Color.Black);
            spriteBatch.DrawString(spriteFont, text, new Vector2(1221f, 32f), Color.White);
            spriteBatch.End();
        }
    }
}
