using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class GroundCheckLogicAction : LogicAction
	{
		private CharacterObj m_obj;
		public override void Execute()
		{
			if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive)
			{
				this.m_obj = (this.ParentLogicSet.ParentGameObj as CharacterObj);
				this.SequenceType = Types.Sequence.Serial;
				base.Execute();
			}
		}
		public override void Update(GameTime gameTime)
		{
			this.ExecuteNext();
			base.Update(gameTime);
		}
		public override void ExecuteNext()
		{
			if (this.m_obj.IsTouchingGround)
			{
				base.ExecuteNext();
			}
		}
		public override object Clone()
		{
			return new GroundCheckLogicAction();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_obj = null;
				base.Dispose();
			}
		}
	}
}
