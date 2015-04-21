using DS2DEngine;
using System;
namespace RogueCastle
{
	public class FireProjectileLogicAction : LogicAction
	{
		private ProjectileManager m_projectileManager;
		private ProjectileData m_data;
		public FireProjectileLogicAction(ProjectileManager projectileManager, ProjectileData data)
		{
			this.m_projectileManager = projectileManager;
			this.m_data = data.Clone();
		}
		public override void Execute()
		{
			if (this.ParentLogicSet != null && this.ParentLogicSet.IsActive)
			{
				GameObj arg_20_0 = this.ParentLogicSet.ParentGameObj;
				this.m_projectileManager.FireProjectile(this.m_data);
				base.Execute();
			}
		}
		public override object Clone()
		{
			return new FireProjectileLogicAction(this.m_projectileManager, this.m_data);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_projectileManager = null;
				this.m_data = null;
				base.Dispose();
			}
		}
	}
}
