using DS2DEngine;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class LockFaceDirectionLogicAction : LogicAction
	{
		private int m_forceDirection;
		private bool m_lockFace;
		public LockFaceDirectionLogicAction(bool lockFace, int forceDirection = 0)
		{
			this.m_lockFace = lockFace;
			this.m_forceDirection = forceDirection;
		}
		public override void Execute()
		{
			if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive)
			{
				CharacterObj characterObj = this.ParentLogicSet.ParentGameObj as CharacterObj;
				if (characterObj != null)
				{
					characterObj.LockFlip = this.m_lockFace;
					if (this.m_forceDirection > 0)
					{
						characterObj.Flip = SpriteEffects.None;
					}
					else if (this.m_forceDirection < 0)
					{
						characterObj.Flip = SpriteEffects.FlipHorizontally;
					}
				}
				base.Execute();
			}
		}
		public override object Clone()
		{
			return new LockFaceDirectionLogicAction(this.m_lockFace, this.m_forceDirection);
		}
	}
}
