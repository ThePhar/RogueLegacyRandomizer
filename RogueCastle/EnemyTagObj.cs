using DS2DEngine;
using System;
namespace RogueCastle
{
	public class EnemyTagObj : GameObj
	{
		public string EnemyType
		{
			get;
			set;
		}
		protected override GameObj CreateCloneInstance()
		{
			return new EnemyTagObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			EnemyTagObj enemyTagObj = obj as EnemyTagObj;
			enemyTagObj.EnemyType = this.EnemyType;
		}
	}
}
