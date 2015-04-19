using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public struct AreaStruct
	{
		public string Name;
		public GameTypes.LevelType LevelType;
		public Vector2 EnemyLevel;
		public Vector2 TotalRooms;
		public Vector2 BonusRooms;
		public Vector2 SecretRooms;
		public int BossLevel;
		public int EnemyLevelScale;
		public bool BossInArea;
		public bool IsFinalArea;
		public Color Color;
		public Color MapColor;
		public bool LinkToCastleOnly;
		public byte BossType;
	}
}
