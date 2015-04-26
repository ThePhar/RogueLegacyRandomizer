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
        public Texture2D BackBuffer;
        protected Effect effect;
        public bool Enabled = true;
        protected Game Game;
        public Vector2 HalfPixel;
        public RenderTarget2D newScene;
        public Texture2D orgBuffer;
        private ScreenQuad sq;
        public bool UsesVertexShader;

        public BasePostProcess(Game game)
        {
            Game = game;
        }

        protected SpriteBatch spriteBatch
        {
            get { return (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch)); }
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