/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class LockFaceDirectionLogicAction : LogicAction
	{
		private int m_forceDirection;
		private bool m_lockFace;
		public LockFaceDirectionLogicAction(bool lockFace, int forceDirection = 0)
		{
			m_lockFace = lockFace;
			m_forceDirection = forceDirection;
		}
		public override void Execute()
		{
			if (ParentLogicSet != null && ParentLogicSet.IsActive)
			{
				CharacterObj characterObj = ParentLogicSet.ParentGameObj as CharacterObj;
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
