using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class DelayObjLogicAction : LogicAction
	{
		private GameObj m_delayObj;
		private float m_delayCounter;
		public DelayObjLogicAction(GameObj delayObj)
		{
			this.m_delayObj = delayObj;
			this.m_delayCounter = 0f;
		}
		public override void Execute()
		{
			if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive)
			{
				this.SequenceType = Types.Sequence.Serial;
				this.m_delayCounter = this.m_delayObj.X;
				base.Execute();
			}
		}
		public override void Update(GameTime gameTime)
		{
			this.m_delayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			this.ExecuteNext();
			base.Update(gameTime);
		}
		public override void ExecuteNext()
		{
			if (this.m_delayCounter <= 0f)
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
			return new DelayObjLogicAction(this.m_delayObj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_delayObj = null;
				base.Dispose();
			}
		}
	}
}
