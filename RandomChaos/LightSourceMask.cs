// 
//  Rogue Legacy Randomizer - LightSourceMask.cs
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
    public class LightSourceMask : BasePostProcess
    {
        public readonly string LightSourceAsset;

        public Vector2 LightScreenSourcePos;
        public float   LightSize;
        public Texture LightSourceTexture;

        public LightSourceMask(Game game, Vector2 sourcePos, string lightSourceAsset, float lightSize = 1500f) : base(game)
        {
            UsesVertexShader = true;
            LightScreenSourcePos = sourcePos;
            LightSourceAsset = lightSourceAsset;
            LightSize = lightSize;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Effect == null)
            {
                Effect = Game.Content.Load<Effect>("Shaders/LightSourceMask");
                LightSourceTexture = Game.Content.Load<Texture2D>(LightSourceAsset);
            }

            Effect.Parameters["screenRes"].SetValue(new Vector2(16f, 9f));
            Effect.Parameters["halfPixel"].SetValue(HalfPixel);
            Effect.CurrentTechnique = Effect.Techniques["LightSourceMask"];
            Effect.Parameters["flare"].SetValue(LightSourceTexture);
            Effect.Parameters["SunSize"].SetValue(LightSize);
            Effect.Parameters["lightScreenPosition"].SetValue(LightScreenSourcePos);
            base.Draw(gameTime);
        }
    }
}
