/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randomchaos2DGodRays
{
    public class BasePostProcess
    {
        public Vector2 HalfPixel;
        public Texture2D BackBuffer;
        public Texture2D orgBuffer;
        public bool Enabled = true;
        protected Effect effect;
        protected Game Game;
        public RenderTarget2D newScene;
        private ScreenQuad sq;
        public bool UsesVertexShader;

        protected SpriteBatch spriteBatch
        {
            get { return (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch)); }
        }

        public BasePostProcess(Game game)
        {
            Game = game;
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (!Enabled) return;
            if (sq == null)
            {
                sq = new ScreenQuad(Game);
                sq.Initialize();
            }
            effect.CurrentTechnique.Passes[0].Apply();
            sq.Draw();
        }
    }
}