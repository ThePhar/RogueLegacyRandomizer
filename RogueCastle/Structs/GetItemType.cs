// 
// RogueLegacyArchipelago - GetItemType.cs
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
    public static class GetItemType
    {
        public const byte None           = 0;
        public const byte Blueprint      = 1;
        public const byte Rune           = 2;
        public const byte StatDrop       = 3;
        public const byte Spell          = 4;
        public const byte SpecialItem    = 5;
        public const byte TripStatDrop   = 6;
        public const byte FountainPiece  = 7;
        public const byte NetworkItem    = 8;
        public const byte GetNetworkItem = 9;
        public const byte SkillDrop      = 9;
        public const byte TripSkillDrop  = 10;
    }
}
