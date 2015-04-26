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
    public class CrepuscularRays : BasePostProcessingEffect
    {
        public LightSourceMask lsMask;
        public LightRay rays;

        public CrepuscularRays(Game game, Vector2 lightScreenSourcePos, string lightSourceImage, float lightSourceSize,
            float density, float decay, float weight, float exposure) : base(game)
        {
            lsMask = new LightSourceMask(game, lightScreenSourcePos, lightSourceImage, lightSourceSize);
            rays = new LightRay(game, lightScreenSourcePos, density, decay, weight, exposure);
            AddPostProcess(lsMask);
            AddPostProcess(rays);
        }

        public Vector2 lightSource
        {
            get { return rays.lighScreenSourcePos; }
            set
            {
                lsMask.lighScreenSourcePos = value;
                rays.lighScreenSourcePos = value;
            }
        }

        public float X
        {
            get { return rays.lighScreenSourcePos.X; }
            set
            {
                lsMask.lighScreenSourcePos.X = value;
                rays.lighScreenSourcePos.X = value;
            }
        }

        public float Y
        {
            get { return rays.lighScreenSourcePos.Y; }
            set
            {
                lsMask.lighScreenSourcePos.Y = value;
                rays.lighScreenSourcePos.Y = value;
            }
        }

        public Texture lightTexture
        {
            get { return lsMask.lishsourceTexture; }
            set { lsMask.lishsourceTexture = value; }
        }

        public float LightSourceSize
        {
            get { return lsMask.lightSize; }
            set { lsMask.lightSize = value; }
        }

        public float Density
        {
            get { return rays.Density; }
            set { rays.Density = value; }
        }

        public float Decay
        {
            get { return rays.Decay; }
            set { rays.Decay = value; }
        }

        public float Weight
        {
            get { return rays.Weight; }
            set { rays.Weight = value; }
        }

        public float Exposure
        {
            get { return rays.Exposure; }
            set { rays.Exposure = value; }
        }
    }
}