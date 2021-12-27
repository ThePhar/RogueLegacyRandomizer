// 
// RogueLegacyArchipelago - AreaStruct.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using Microsoft.Xna.Framework;
using RogueCastle.Structs;

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
