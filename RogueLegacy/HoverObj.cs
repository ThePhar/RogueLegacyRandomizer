// Rogue Legacy Randomizer - HoverObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public class HoverObj : SpriteObj
    {
        public float Amplitude = 1f;
        public float HoverSpeed = 1f;
        private Vector2 m_startPos;

        public HoverObj(string spriteName) : base(spriteName) { }

        public void SetStartingPos(Vector2 pos)
        {
            m_startPos = pos;
        }

        public override void Draw(Camera2D camera)
        {
            Y = m_startPos.Y + (float) Math.Sin(Game.TotalGameTimeSeconds * HoverSpeed) * Amplitude;
            base.Draw(camera);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new HoverObj(SpriteName);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var hoverObj = obj as HoverObj;
            hoverObj.HoverSpeed = HoverSpeed;
            hoverObj.Amplitude = Amplitude;
            hoverObj.SetStartingPos(m_startPos);
        }
    }
}
