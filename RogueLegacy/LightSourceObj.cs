// Rogue Legacy Randomizer - LightSourceObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;

namespace RogueLegacy
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
