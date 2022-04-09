// 
//  Rogue Legacy Randomizer - BasePostProcess.cs
//  Last Modified 2022-04-08
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RandomChaos
{
    public class BasePostProcess
    {
        public    Texture2D      BackBuffer;
        public    bool           Enabled = true;
        public    Vector2        HalfPixel;
        public    RenderTarget2D NewScene;
        public    Texture2D      OrgBuffer;
        protected Effect         Effect;
        protected Game           Game;
        protected bool           UsesVertexShader;
        private   ScreenQuad     _screenQuad;

        public BasePostProcess(Game game)
        {
            Game = game;
        }

        protected SpriteBatch SpriteBatch => (SpriteBatch) Game.Services.GetService(typeof(SpriteBatch));

        public virtual void Draw(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            if (_screenQuad == null)
            {
                _screenQuad = new ScreenQuad(Game);
                _screenQuad.Initialize();
            }

            Effect.CurrentTechnique.Passes[0].Apply();
            _screenQuad.Draw();
        }
    }
}
