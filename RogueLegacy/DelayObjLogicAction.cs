// Rogue Legacy Randomizer - DelayObjLogicAction.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public class DelayObjLogicAction : LogicAction
    {
        private float m_delayCounter;
        private GameObj m_delayObj;

        public DelayObjLogicAction(GameObj delayObj)
        {
            m_delayObj = delayObj;
            m_delayCounter = 0f;
        }

        public override void Execute()
        {
            if (ParentLogicSet != null && ParentLogicSet.IsActive)
            {
                SequenceType = Types.Sequence.Serial;
                m_delayCounter = m_delayObj.X;
                base.Execute();
            }
        }

        public override void Update(GameTime gameTime)
        {
            m_delayCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            ExecuteNext();
            base.Update(gameTime);
        }

        public override void ExecuteNext()
        {
            if (m_delayCounter <= 0f)
            {
                base.ExecuteNext();
            }
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override object Clone()
        {
            return new DelayObjLogicAction(m_delayObj);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_delayObj = null;
                base.Dispose();
            }
        }
    }
}
