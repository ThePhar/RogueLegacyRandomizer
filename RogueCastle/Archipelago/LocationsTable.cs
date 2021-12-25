// 
// RogueLegacyArchipelago - LocationsTable.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace RogueCastle.Archipelago
{
    public static class LocationsTable
    {
        public const int FairyChestLocationStartIndex = 44200;
        public const int FairyChestLocationEndIndex = 44254;
        public const int FairyChestTotalLocations = FairyChestLocationEndIndex - FairyChestLocationStartIndex + 1;
        public const int ChestLocationStartIndex = 44300;
        public const int ChestLocationEndIndex = 44551;
        public const int ChestLocationTotalLocations = ChestLocationEndIndex - ChestLocationStartIndex + 1;
    }
}
