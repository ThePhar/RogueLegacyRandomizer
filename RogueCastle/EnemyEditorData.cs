using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public struct EnemyEditorData
	{
		public byte Type;
		public string SpriteName;
		public Vector2 BasicScale;
		public Vector2 AdvancedScale;
		public Vector2 ExpertScale;
		public Vector2 MinibossScale;
		public EnemyEditorData(byte enemyType)
		{
			EnemyObj enemyObj = EnemyBuilder.BuildEnemy((int)enemyType, null, null, null, GameTypes.EnemyDifficulty.BASIC, false);
			EnemyObj enemyObj2 = EnemyBuilder.BuildEnemy((int)enemyType, null, null, null, GameTypes.EnemyDifficulty.ADVANCED, false);
			EnemyObj enemyObj3 = EnemyBuilder.BuildEnemy((int)enemyType, null, null, null, GameTypes.EnemyDifficulty.EXPERT, false);
			EnemyObj enemyObj4 = EnemyBuilder.BuildEnemy((int)enemyType, null, null, null, GameTypes.EnemyDifficulty.MINIBOSS, false);
			this.Type = enemyType;
			this.SpriteName = enemyObj.SpriteName;
			this.BasicScale = enemyObj.Scale;
			this.AdvancedScale = enemyObj2.Scale;
			this.ExpertScale = enemyObj3.Scale;
			this.MinibossScale = enemyObj4.Scale;
		}
	}
}
