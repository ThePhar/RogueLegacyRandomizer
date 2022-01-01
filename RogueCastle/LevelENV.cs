//
//  RogueLegacyArchipelago - LevelENV.cs
//  Last Modified 2021-12-30
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using RogueCastle.Structs;

namespace RogueCastle
{
    public static class LevelENV
    {
        public const bool LinkToCastleOnly = true;
        public const byte TotalJournalEntries = 25;
        public const byte CastleBossRoom = 1;
        public const byte TowerBossRoom = 6;
        public const byte DungeonBossRoom = 7;
        public const byte GardenBossRoom = 5;
        public const byte LastBossRoom = 2;
        public const float FrameLimit = 0.025f;
        public const float EnemyLevelFakeMultiplier = 2.75f;
        public const int EnemyLevelDifficultyMod = 32;
        public const int RoomLevelMod = 4;
        public const int EnemyExpertLevelMod = 4;
        public const int EnemyMiniBossLevelMod = 7;
        public const int LastBossMode1LevelMod = 8;
        public const int LastBossMode2LevelMod = 10;
        public const int CastleRoomLevelBoost = 0;
        public const int GardenRoomLevelBoost = 2;
        public const int TowerRoomLevelBoost = 4;
        public const int DungeonRoomLevelBoost = 6;
        public const int NewGamePlusLevelBase = 128;
        public const int NewGamePlusLevelAppreciation = 128;
        public const int NewGamePlusMiniBossLevelBase = 0;
        public const int NewGamePlusMiniBossLevelAppreciation = 0;
        public const int LevelCastleLeftDoor = 90;
        public const int LevelCastleRightDoor = 90;
        public const int LevelCastleTopDoor = 90;
        public const int LevelCastleBottomDoor = 90;
        public const int LevelGardenLeftDoor = 70;
        public const int LevelGardenRightDoor = 100;
        public const int LevelGardenTopDoor = 45;
        public const int LevelGardenBottomDoor = 45;
        public const int LevelTowerLeftDoor = 45;
        public const int LevelTowerRightDoor = 45;
        public const int LevelTowerTopDoor = 100;
        public const int LevelTowerBottomDoor = 60;
        public const int LevelDungeonLeftDoor = 55;
        public const int LevelDungeonRightDoor = 55;
        public const int LevelDungeonTopDoor = 45;
        public const int LevelDungeonBottomDoor = 100;
        public const string GameName = "Rogue Legacy Randomizer";
        public static readonly Version GameVersion = Version.Parse("0.5.4");

        static LevelENV()
        {
            // Enemy Lists
            CastleEnemyList = new[]
            {
                EnemyType.Skeleton,
                EnemyType.Knight,
                EnemyType.FireWizard,
                EnemyType.IceWizard,
                EnemyType.Eyeball,
                EnemyType.BouncySpike,
                EnemyType.SwordKnight,
                EnemyType.Zombie,
                EnemyType.Fireball,
                EnemyType.Portrait,
                EnemyType.Starburst,
                EnemyType.HomingTurret
            };
            GardenEnemyList = new[]
            {
                EnemyType.Skeleton,
                EnemyType.Blob,
                EnemyType.BallAndChain,
                EnemyType.EarthWizard,
                EnemyType.FireWizard,
                EnemyType.Eyeball,
                EnemyType.Fairy,
                EnemyType.ShieldKnight,
                EnemyType.BouncySpike,
                EnemyType.Wolf,
                EnemyType.Plant,
                EnemyType.SkeletonArcher,
                EnemyType.Starburst,
                EnemyType.Horse
            };
            TowerEnemyList = new[]
            {
                EnemyType.Knight,
                EnemyType.BallAndChain,
                EnemyType.IceWizard,
                EnemyType.Eyeball,
                EnemyType.Fairy,
                EnemyType.ShieldKnight,
                EnemyType.BouncySpike,
                EnemyType.Wolf,
                EnemyType.Ninja,
                EnemyType.Plant,
                EnemyType.Fireball,
                EnemyType.SkeletonArcher,
                EnemyType.Portrait,
                EnemyType.Starburst,
                EnemyType.HomingTurret,
                EnemyType.Mimic
            };
            DungeonEnemyList = new[]
            {
                EnemyType.Skeleton,
                EnemyType.Knight,
                EnemyType.Blob,
                EnemyType.BallAndChain,
                EnemyType.EarthWizard,
                EnemyType.FireWizard,
                EnemyType.IceWizard,
                EnemyType.Eyeball,
                EnemyType.Fairy,
                EnemyType.BouncySpike,
                EnemyType.SwordKnight,
                EnemyType.Zombie,
                EnemyType.Ninja,
                EnemyType.Plant,
                EnemyType.Fireball,
                EnemyType.Starburst,
                EnemyType.HomingTurret,
                EnemyType.Horse
            };
            DementiaFlightList = new[]
            {
                EnemyType.FireWizard,
                EnemyType.IceWizard,
                EnemyType.Eyeball,
                EnemyType.Fairy,
                EnemyType.BouncySpike,
                EnemyType.Fireball,
                EnemyType.Starburst
            };
            DementiaGroundList = new[]
            {
                EnemyType.Skeleton,
                EnemyType.Knight,
                EnemyType.Blob,
                EnemyType.BallAndChain,
                EnemyType.SwordKnight,
                EnemyType.Zombie,
                EnemyType.Ninja,
                EnemyType.Plant,
                EnemyType.HomingTurret,
                EnemyType.Horse
            };

            // Enemy Difficulty Lists
            CastleEnemyDifficultyList = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            GardenEnemyDifficultyList = new byte[] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            TowerEnemyDifficultyList = new byte[] { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 };
            DungeonEnemyDifficultyList = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            // Asset Swap Lists
            CastleAssetSwapList = new[]
            {
                "BreakableBarrel1_Character",
                "BreakableBarrel2_Character",
                "CastleAssetKnightStatue_Character",
                "CastleAssetWindow1_Sprite",
                "CastleAssetWindow2_Sprite",
                "CastleBGPillar_Character",
                "CastleAssetWeb1_Sprite",
                "CastleAssetWeb2_Sprite",
                "CastleAssetBackTorch_Character",
                "CastleAssetSideTorch_Character",
                "CastleAssetChandelier1_Character",
                "CastleAssetChandelier2_Character",
                "CastleAssetCandle1_Character",
                "CastleAssetCandle2_Character",
                "CastleAssetFireplace_Character",
                "CastleAssetBookcase_Sprite",
                "CastleAssetBookCase2_Sprite",
                "CastleAssetBookCase3_Sprite",
                "CastleAssetUrn1_Character",
                "CastleAssetUrn2_Character",
                "BreakableChair1_Character",
                "BreakableChair2_Character",
                "CastleAssetTable1_Character",
                "CastleAssetTable2_Character",
                "CastleDoorOpen_Sprite",
                "CastleAssetFrame_Sprite"
            };
            DungeonAssetSwapList = new[]
            {
                "BreakableCrate1_Character",
                "BreakableBarrel1_Character",
                "CastleAssetDemonStatue_Character",
                "DungeonSewerGrate1_Sprite",
                "DungeonSewerGrate2_Sprite",
                "",
                "CastleAssetWeb1_Sprite",
                "CastleAssetWeb2_Sprite",
                "DungeonChainRANDOM2_Character",
                "",
                "DungeonHangingCell1_Character",
                "DungeonHangingCell2_Character",
                "DungeonTorch1_Character",
                "DungeonTorch2_Character",
                "DungeonMaidenRANDOM3_Character",
                "DungeonPrison1_Sprite",
                "DungeonPrison2_Sprite",
                "DungeonPrison3_Sprite",
                "",
                "",
                "DungeonBucket1_Character",
                "DungeonBucket2_Character",
                "DungeonTable1_Character",
                "DungeonTable2_Character",
                "DungeonDoorOpen_Sprite",
                ""
            };
            TowerAssetSwapList = new[]
            {
                "BreakableCrate1_Character",
                "BreakableCrate2_Character",
                "CastleAssetAngelStatue_Character",
                "TowerHoleRANDOM9_Sprite",
                "TowerHoleRANDOM9_Sprite",
                "",
                "TowerLever1_Sprite",
                "TowerLever2_Sprite",
                "CastleAssetBackTorchUnlit_Character",
                "CastleAssetSideTorchUnlit_Character",
                "DungeonChain1_Character",
                "DungeonChain2_Character",
                "TowerTorch_Character",
                "TowerPedestal2_Character",
                "CastleAssetFireplaceNoFire_Character",
                "BrokenBookcase1_Sprite",
                "BrokenBookcase2_Sprite",
                "",
                "TowerBust1_Character",
                "TowerBust2_Character",
                "TowerChair1_Character",
                "TowerChair2_Character",
                "TowerTable1_Character",
                "TowerTable2_Character",
                "TowerDoorOpen_Sprite",
                "CastleAssetFrame_Sprite"
            };
            GardenAssetSwapList = new[]
            {
                "GardenUrn1_Character",
                "GardenUrn2_Character",
                "CherubStatue_Character",
                "GardenFloatingRockRANDOM5_Sprite",
                "GardenFloatingRockRANDOM5_Sprite",
                "GardenPillar_Character",
                "",
                "",
                "GardenFairy_Character",
                "",
                "GardenVine1_Character",
                "GardenVine2_Character",
                "GardenLampPost1_Character",
                "GardenLampPost2_Character",
                "GardenFountain_Character",
                "GardenBush1_Sprite",
                "GardenBush2_Sprite",
                "",
                "",
                "",
                "GardenMushroom1_Character",
                "GardenMushroom2_Character",
                "GardenTrunk1_Character",
                "GardenTrunk2_Character",
                "GardenDoorOpen_Sprite",
                ""
            };

            // Default Environment Variables
            TestRoomLevelType = LevelType.Castle;
            ShowEnemyRadii = false;
            EnablePlayerDebug = false;
            UnlockAllAbilities = false;
            TestRoomReverse = false;
            RunTestRoom = false;
            ShowDebugText = false;
            LoadSplashScreen = true;
            ShowSaveLoadDebugText = false;
            DeleteSaveFile = false;
            CloseTestRoomDoors = false;
            RunTutorial = false;
            RunDemoVersion = false;
            DisableSaving = false;
            RunCrashLogs = true;
            WeakenBosses = false;
            EnableOffscreenControl = true;
            EnableBackupSaving = false;
            CreateRetailVersion = true;
            ShowFps = false;
            SaveFrames = false;
            ShowArchipelagoStatus = false;
            RunConsole = false;
        }

        public static LevelType TestRoomLevelType { get; set; }
        public static byte[] DementiaFlightList { get; set; }
        public static byte[] DementiaGroundList { get; set; }
        public static byte[] CastleEnemyList { get; set; }
        public static byte[] GardenEnemyList { get; set; }
        public static byte[] TowerEnemyList { get; set; }
        public static byte[] DungeonEnemyList { get; set; }
        public static byte[] CastleEnemyDifficultyList { get; set; }
        public static byte[] GardenEnemyDifficultyList { get; set; }
        public static byte[] TowerEnemyDifficultyList { get; set; }
        public static byte[] DungeonEnemyDifficultyList { get; set; }
        public static string[] CastleAssetSwapList { get; set; }
        public static string[] DungeonAssetSwapList { get; set; }
        public static string[] TowerAssetSwapList { get; set; }
        public static string[] GardenAssetSwapList { get; set; }
        public static bool ShowEnemyRadii { get; set; }
        public static bool EnablePlayerDebug { get; set; }
        public static bool UnlockAllAbilities { get; set; }
        public static bool TestRoomReverse { get; set; }
        public static bool RunTestRoom { get; set; }
        public static bool ShowDebugText { get; set; }
        public static bool LoadSplashScreen { get; set; }
        public static bool ShowSaveLoadDebugText { get; set; }
        public static bool DeleteSaveFile { get; set; }
        public static bool CloseTestRoomDoors { get; set; }
        public static bool RunTutorial { get; set; }
        public static bool RunDemoVersion { get; set; }
        public static bool DisableSaving { get; set; }
        public static bool RunCrashLogs { get; set; }
        public static bool WeakenBosses { get; set; }
        public static bool EnableOffscreenControl { get; set; }
        public static bool EnableBackupSaving { get; set; }
        public static bool CreateRetailVersion { get; set; }
        public static bool ShowFps { get; set; }
        public static bool SaveFrames { get; set; }
        public static bool ShowArchipelagoStatus { get; set; }
        public static bool RunConsole { get; set; }
    }
}
