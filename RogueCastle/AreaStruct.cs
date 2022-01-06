// 
// RogueLegacyArchipelago - AreaStruct.cs
// Last Modified 2021-12-27
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
        public Color Color;
        public Color MapColor;
        public Zone Zone;
        public Vector2 BonusRooms;
        public Vector2 EnemyLevel;
        public Vector2 SecretRooms;
        public Vector2 TotalRooms;
        public bool BossInArea;
        public bool IsFinalArea;
        public bool LinkToCastleOnly;
        public byte BossType;
        public int BossLevel;
        public int EnemyLevelScale;
        public string Name;
    }
}