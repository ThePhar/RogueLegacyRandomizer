using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class CheckWallLogicAction : LogicAction
	{
		private EnemyObj m_obj;
		public override void Execute()
		{
			if (ParentLogicSet != null && ParentLogicSet.IsActive)
			{
				m_obj = (ParentLogicSet.ParentGameObj as EnemyObj);
				SequenceType = Types.Sequence.Serial;
				base.Execute();
			}
		}
		public override void Update(GameTime gameTime)
		{
			ExecuteNext();
			base.Update(gameTime);
		}
		public override void ExecuteNext()
		{
			base.ExecuteNext();
		}
		public override object Clone()
		{
			return new CheckWallLogicAction();
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_obj = null;
				base.Dispose();
			}
		}
	}
}
