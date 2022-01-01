// 
// RogueLegacyArchipelago - ChestTracker.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace RogueCastle
{
    public class ChestTracker
    {
        public int CastleChests;
        public int CastleFairyChests;
        public int DungeonChests;
        public int DungeonFairyChests;
        public int GardenChests;
        public int GardenFairyChests;
        public int TowerChests;
        public int TowerFairyChests;

        // This is the worst case I've ever written. Do not look.
        public ChestTracker(params int[] amount)
        {
            CastleChests = 0;
            GardenChests = 0;
            TowerChests = 0;
            DungeonChests = 0;
            CastleFairyChests = 0;
            GardenFairyChests = 0;
            TowerFairyChests = 0;
            DungeonFairyChests = 0;

            for (var i = 0; i < amount.Length; i++)
                switch (i)
                {
                    case 0:
                        CastleChests = amount[i];
                        break;

                    case 1:
                        GardenChests = amount[i];
                        break;

                    case 2:
                        TowerChests = amount[i];
                        break;

                    case 3:
                        DungeonChests = amount[i];
                        break;

                    case 4:
                        CastleFairyChests = amount[i];
                        break;

                    case 5:
                        GardenFairyChests = amount[i];
                        break;

                    case 6:
                        TowerFairyChests = amount[i];
                        break;

                    case 7:
                        DungeonFairyChests = amount[i];
                        break;
                }
        }
    }
}