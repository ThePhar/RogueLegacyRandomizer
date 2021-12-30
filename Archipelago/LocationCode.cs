// 
//  RogueLegacyArchipelago - LocationCode.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace Archipelago
{
    public static class LocationCodeConstants
    {
        // Diaries
        public const int DiaryStartIndex = 91300;

        // Fairy Chests
        public const int FairyCastleStartIndex = 91400;
        public const int FairyGardenStartIndex = 91450;
        public const int FairyTowerStartIndex = 91500;
        public const int FairyDungeonStartIndex = 91550;

        // Regular Chests
        public const int ChestCastleStartIndex = 91600;
        public const int ChestGardenStartIndex = 91700;
        public const int ChestTowerStartIndex = 91800;
        public const int ChestDungeonStartIndex = 91900;
    }

    public enum LocationCode
    {
        #region Manor Upgrades

        // Manor Upgrades
        ManorGroundRoad = 91000,
        ManorMainBase = 91001,
        ManorMainWindowBottom = 91002,
        ManorMainWindowTop = 91003,
        ManorMainRoof = 91004,
        ManorLeftWingBase = 91005,
        ManorLeftWingWindow = 91006,
        ManorLeftWingRoof = 91007,
        ManorLeftBigBase = 91008,
        ManorLeftBigUpper1 = 91009,
        ManorLeftBigUpper2 = 91010,
        ManorLeftBigWindows = 91011,
        ManorLeftBigRoof = 91012,
        ManorLeftFarBase = 91013,
        ManorLeftFarRoof = 91014,
        ManorLeftExtension = 91015,
        ManorLeftTree1 = 91016,
        ManorLeftTree2 = 91017,
        ManorRightWingBase = 91018,
        ManorRightWingWindow = 91019,
        ManorRightWingRoof = 91020,
        ManorRightBigBase = 91021,
        ManorRightBigUpper = 91022,
        ManorRightBigRoof = 91023,
        ManorRightHighBase = 91024,
        ManorRightHighUpper = 91025,
        ManorRightHighTower = 91026,
        ManorRightExtension = 91027,
        ManorRightTree = 91028,
        ManorObservatoryBase = 91029,
        ManorObservatoryTelescope = 91030,

        #endregion

        #region Boss

        // Boss
        KhindrBossChest = 91100,
        // Khindr2BossChest = 91101,
        AlexanderBossChest = 91102,
        // Alexander2BossChest = 91103,
        LeonBossChest = 91104,
        // LeonBoss2Chest = 91105,
        HerodotusBossChest = 91106,
        // Herodotus2BossChest = 91107,

        #endregion

        #region Miscellaneous

        // Misc.
        JukeboxReward = 91200,

        #endregion
    }
}
