// Rogue Legacy Randomizer - LockFaceDirectionLogicAction.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLegacy
{
    public class LockFaceDirectionLogicAction : LogicAction
    {
        private readonly int m_forceDirection;
        private readonly bool m_lockFace;

        public LockFaceDirectionLogicAction(bool lockFace, int forceDirection = 0)
        {
            m_lockFace = lockFace;
            m_forceDirection = forceDirection;
        }

        public override void Execute()
        {
            if (ParentLogicSet != null && ParentLogicSet.IsActive)
            {
                var characterObj = ParentLogicSet.ParentGameObj as CharacterObj;
                if (characterObj != null)
                {
                    characterObj.LockFlip = m_lockFace;
                    if (m_forceDirection > 0)
                    {
                        characterObj.Flip = SpriteEffects.None;
                    }
                    else if (m_forceDirection < 0)
                    {
                        characterObj.Flip = SpriteEffects.FlipHorizontally;
                    }
                }

                base.Execute();
            }
        }

        public override object Clone()
        {
            return new LockFaceDirectionLogicAction(m_lockFace, m_forceDirection);
        }
    }
}
