// Rogue Legacy Randomizer - JumpLogicAction.cs
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
    public class JumpLogicAction : LogicAction
    {
        private readonly float m_overriddenHeight;

        public JumpLogicAction(float overriddenHeight = 0f)
        {
            m_overriddenHeight = overriddenHeight;
        }

        public override void Execute()
        {
            if (ParentLogicSet != null && ParentLogicSet.IsActive)
            {
                var characterObj = ParentLogicSet.ParentGameObj as CharacterObj;
                if (characterObj != null)
                {
                    if (m_overriddenHeight > 0f)
                    {
                        characterObj.AccelerationY = -m_overriddenHeight;
                    }
                    else
                    {
                        characterObj.AccelerationY = -characterObj.JumpHeight;
                    }
                }

                base.Execute();
            }
        }

        public override object Clone()
        {
            return new JumpLogicAction(m_overriddenHeight);
        }
    }
}
