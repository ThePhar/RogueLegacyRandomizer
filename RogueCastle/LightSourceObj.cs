/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;

namespace RogueCastle
{
    public class LightSourceObj : SpriteObj
    {
        private float m_growthDifference;
        private float m_growthRate;

        public LightSourceObj() : base("LightSource_Sprite")
        {
            m_growthRate = 0.7f + CDGMath.RandomFloat(-0.1f, 0.1f);
            m_growthDifference = 0.05f + CDGMath.RandomFloat(0f, 0.05f);
            Opacity = 1f;
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new LightSourceObj();
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
        }
    }
}