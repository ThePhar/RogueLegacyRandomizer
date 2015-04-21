using DS2DEngine;
using System;
namespace RogueCastle
{
	public class EnemyOrbObj : GameObj
	{
		public int OrbType;
		public bool ForceFlying
		{
			get;
			set;
		}
		protected override GameObj CreateCloneInstance()
		{
			return new EnemyOrbObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			EnemyOrbObj enemyOrbObj = obj as EnemyOrbObj;
			enemyOrbObj.OrbType = this.OrbType;
			enemyOrbObj.ForceFlying = this.ForceFlying;
		}
	}
}
