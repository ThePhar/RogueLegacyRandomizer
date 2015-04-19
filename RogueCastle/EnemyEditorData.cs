using Microsoft.Xna.Framework;

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
			EnemyObj enemyObj = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.BASIC, false);
			EnemyObj enemyObj2 = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.ADVANCED, false);
			EnemyObj enemyObj3 = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.EXPERT, false);
			EnemyObj enemyObj4 = EnemyBuilder.BuildEnemy(enemyType, null, null, null, GameTypes.EnemyDifficulty.MINIBOSS, false);
			Type = enemyType;
			SpriteName = enemyObj.SpriteName;
			BasicScale = enemyObj.Scale;
			AdvancedScale = enemyObj2.Scale;
			ExpertScale = enemyObj3.Scale;
			MinibossScale = enemyObj4.Scale;
		}
	}
}
