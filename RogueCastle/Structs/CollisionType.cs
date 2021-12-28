// 
// RogueLegacyArchipelago - CollisionType.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace RogueCastle.Structs
{
    public static class CollisionType
    {
        public const int Null             = 0;
        public const int Wall             = 1;
        public const int Player           = 2;
        public const int Enemy            = 3;
        public const int EnemyWall        = 4;
        public const int WallForPlayer    = 5;
        public const int WallForEnemy     = 6;
        public const int PlayerTrigger    = 7;
        public const int EnemyTrigger     = 8;
        public const int GlobalTrigger    = 9;
        public const int GlobalDamageWall = 10;
    }
}
