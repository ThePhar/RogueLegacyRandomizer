using DS2DEngine;

namespace RogueCastle
{
	public class JumpLogicAction : LogicAction
	{
		private float m_overriddenHeight;
		public JumpLogicAction(float overriddenHeight = 0f)
		{
			m_overriddenHeight = overriddenHeight;
		}
		public override void Execute()
		{
			if (ParentLogicSet != null && ParentLogicSet.IsActive)
			{
				CharacterObj characterObj = ParentLogicSet.ParentGameObj as CharacterObj;
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
