using DS2DEngine;
using System;
namespace RogueCastle
{
	public class JumpLogicAction : LogicAction
	{
		private float m_overriddenHeight;
		public JumpLogicAction(float overriddenHeight = 0f)
		{
			this.m_overriddenHeight = overriddenHeight;
		}
		public override void Execute()
		{
			if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive)
			{
				CharacterObj characterObj = this.ParentLogicSet.ParentGameObj as CharacterObj;
				if (characterObj != null)
				{
					if (this.m_overriddenHeight > 0f)
					{
						characterObj.AccelerationY = -this.m_overriddenHeight;
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
			return new JumpLogicAction(this.m_overriddenHeight);
		}
	}
}
