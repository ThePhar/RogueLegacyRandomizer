// 
//  Rogue Legacy Randomizer - LightRay.cs
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
    public class LightRay : BasePostProcess
    {
        public float   Decay;
        public float   Density;
        public float   Exposure;
        public Vector2 LightScreenSourcePos;
        public float   Weight;

        public LightRay(Game game, Vector2 sourcePos, float density = 0.5f, float decay = 0.95f, float weight = 1f, float exposure = 0.15f)
            : base(game)
        {
            LightScreenSourcePos = sourcePos;
            Density = density;
            Decay = decay;
            Weight = weight;
            Exposure = exposure;
            UsesVertexShader = true;
        }

        public override void Draw(GameTime gameTime)
        {
            Effect ??= Game.Content.Load<Effect>("Shaders/LightRays");
            Effect.CurrentTechnique = Effect.Techniques["LightRayFX"];
            Effect.Parameters["halfPixel"].SetValue(HalfPixel);
            Effect.Parameters["Density"].SetValue(Density);
            Effect.Parameters["Decay"].SetValue(Decay);
            Effect.Parameters["Weight"].SetValue(Weight);
            Effect.Parameters["Exposure"].SetValue(Exposure);
            Effect.Parameters["lightScreenPosition"].SetValue(LightScreenSourcePos);
            base.Draw(gameTime);
        }
    }
}
