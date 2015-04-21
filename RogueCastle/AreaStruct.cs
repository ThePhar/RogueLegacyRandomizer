/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

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
