// 
//  Rogue Legacy Randomizer - CrepuscularRays.cs
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
    public class CrepuscularRays : BasePostProcessingEffect
    {
        public LightSourceMask LightSourceMask;
        public LightRay        LightRay;

        public CrepuscularRays(Game game, Vector2 lightScreenSourcePos, string lightSourceImage, float lightSourceSize, float density, float decay,
            float weight, float exposure) : base(game)
        {
            LightSourceMask = new LightSourceMask(game, lightScreenSourcePos, lightSourceImage, lightSourceSize);
            LightRay = new LightRay(game, lightScreenSourcePos, density, decay, weight, exposure);
            AddPostProcess(LightSourceMask);
            AddPostProcess(LightRay);
        }

        public Vector2 LightSource
        {
            get => LightRay.LightScreenSourcePos;
            set
            {
                LightSourceMask.LightScreenSourcePos = value;
                LightRay.LightScreenSourcePos = value;
            }
        }

        public float X
        {
            get => LightRay.LightScreenSourcePos.X;
            set
            {
                LightSourceMask.LightScreenSourcePos.X = value;
                LightRay.LightScreenSourcePos.X = value;
            }
        }

        public float Y
        {
            get => LightRay.LightScreenSourcePos.Y;
            set
            {
                LightSourceMask.LightScreenSourcePos.Y = value;
                LightRay.LightScreenSourcePos.Y = value;
            }
        }

        public Texture LightTexture
        {
            get => LightSourceMask.LightSourceTexture;
            set => LightSourceMask.LightSourceTexture = value;
        }

        public float LightSourceSize
        {
            get => LightSourceMask.LightSize;
            set => LightSourceMask.LightSize = value;
        }

        public float Density
        {
            get => LightRay.Density;
            set => LightRay.Density = value;
        }

        public float Decay
        {
            get => LightRay.Decay;
            set => LightRay.Decay = value;
        }

        public float Weight
        {
            get => LightRay.Weight;
            set => LightRay.Weight = value;
        }

        public float Exposure
        {
            get => LightRay.Exposure;
            set => LightRay.Exposure = value;
        }
    }
}
