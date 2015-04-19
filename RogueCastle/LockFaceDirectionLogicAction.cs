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
