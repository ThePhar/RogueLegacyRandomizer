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
        public Vector2 BonusRooms;
        public bool BossInArea;
        public int BossLevel;
        public byte BossType;
        public Color Color;
        public Vector2 EnemyLevel;
        public int EnemyLevelScale;
        public bool IsFinalArea;
        public GameTypes.LevelType LevelType;
        public bool LinkToCastleOnly;
        public Color MapColor;
        public string Name;
        public Vector2 SecretRooms;
        public Vector2 TotalRooms;
    }
}