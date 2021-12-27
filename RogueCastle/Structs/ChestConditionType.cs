// 
// RogueLegacyArchipelago - ChestConditionType.cs
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
    public static class ChestConditionType
    {
        public const int None               = 0;
        public const int KillAllEnemies     = 1;
        public const int HealthBelow15      = 2;
        public const int DontLook           = 3;
        public const int NoJumping          = 4;
        public const int NoSound            = 5;
        public const int NoFloor            = 6;
        public const int NoAttackingEnemies = 7;
        public const int ReachIn5Seconds    = 8;
        public const int TakeNoDamage       = 9;
        public const int InvisibleChest     = 10;
    }
}
