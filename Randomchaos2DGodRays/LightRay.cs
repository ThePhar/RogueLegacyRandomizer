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
    public class LightRay : BasePostProcess
    {
        public float Decay = 0.95f;
        public float Density = 0.5f;
        public float Exposure = 0.15f;
        public Vector2 lighScreenSourcePos;
        public float Weight = 1f;

        public LightRay(Game game, Vector2 sourcePos, float density, float decay, float weight, float exposure)
            : base(game)
        {
            lighScreenSourcePos = sourcePos;
            Density = density;
            Decay = decay;
            Weight = weight;
            Exposure = exposure;
            UsesVertexShader = true;
        }

        public override void Draw(GameTime gameTime)
        {
            if (effect == null)
            {
                effect = Game.Content.Load<Effect>("Shaders/LightRays");
            }
            effect.CurrentTechnique = effect.Techniques["LightRayFX"];
            effect.Parameters["halfPixel"].SetValue(HalfPixel);
            effect.Parameters["Density"].SetValue(Density);
            effect.Parameters["Decay"].SetValue(Decay);
            effect.Parameters["Weight"].SetValue(Weight);
            effect.Parameters["Exposure"].SetValue(Exposure);
            effect.Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);
            base.Draw(gameTime);
        }
    }
}