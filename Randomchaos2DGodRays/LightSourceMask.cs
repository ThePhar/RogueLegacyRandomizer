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
    public class LightSourceMask : BasePostProcess
    {
        public Vector2 lighScreenSourcePos;
        public float lightSize = 1500f;
        public string lightSourceasset;
        public Texture lishsourceTexture;

        public LightSourceMask(Game game, Vector2 sourcePos, string lightSourceasset, float lightSize) : base(game)
        {
            UsesVertexShader = true;
            lighScreenSourcePos = sourcePos;
            this.lightSourceasset = lightSourceasset;
            this.lightSize = lightSize;
        }

        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
            {
                effect = Game.Content.Load<Effect>("Shaders/LightSourceMask");
                lishsourceTexture = Game.Content.Load<Texture2D>(lightSourceasset);
            }

            effect.Parameters["screenRes"].SetValue(new Vector2(16f, 9f));
            effect.Parameters["halfPixel"].SetValue(HalfPixel);
            effect.CurrentTechnique = effect.Techniques["LightSourceMask"];
            effect.Parameters["flare"].SetValue(lishsourceTexture);
            effect.Parameters["SunSize"].SetValue(lightSize);
            effect.Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);
            base.Draw(gameTime);
        }
    }
}