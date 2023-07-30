// RogueLegacyRandomizer - LocationCode.cs
// Last Modified 2023-07-30 4:03 PM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.ComponentModel;
using RogueLegacy.Enums;

namespace Randomizer.Definitions;

public static class LocationCode
{
    public const int MAX_CHESTS       = 50;
    public const int MAX_FAIRY_CHESTS = 15;
    public const int MAX_DIARIES      = 24;

    public const long MANOR_GROUND_ROAD           = 91_000;
    public const long MANOR_MAIN_BASE             = 91_001;
    public const long MANOR_MAIN_BOTTOM_WINDOW    = 91_002;
    public const long MANOR_MAIN_TOP_WINDOW       = 91_003;
    public const long MANOR_MAIN_ROOFTOP          = 91_004;
    public const long MANOR_LEFT_WING_BASE        = 91_005;
    public const long MANOR_LEFT_WING_WINDOW      = 91_006;
    public const long MANOR_LEFT_WING_ROOFTOP     = 91_007;
    public const long MANOR_LEFT_BIG_BASE         = 91_008;
    public const long MANOR_LEFT_BIG_UPPER1       = 91_009;
    public const long MANOR_LEFT_BIG_UPPER2       = 91_010;
    public const long MANOR_LEFT_BIG_WINDOWS      = 91_011;
    public const long MANOR_LEFT_BIG_ROOFTOP      = 91_012;
    public const long MANOR_LEFT_FAR_BASE         = 91_013;
    public const long MANOR_LEFT_FAR_ROOF         = 91_014;
    public const long MANOR_LEFT_EXTENSION        = 91_015;
    public const long MANOR_LEFT_TREE1            = 91_016;
    public const long MANOR_LEFT_TREE2            = 91_017;
    public const long MANOR_RIGHT_WING_BASE       = 91_018;
    public const long MANOR_RIGHT_WING_WINDOW     = 91_019;
    public const long MANOR_RIGHT_WING_ROOFTOP    = 91_020;
    public const long MANOR_RIGHT_BIG_BASE        = 91_021;
    public const long MANOR_RIGHT_BIG_UPPER       = 91_022;
    public const long MANOR_RIGHT_BIG_ROOFTOP     = 91_023;
    public const long MANOR_RIGHT_HIGH_BASE       = 91_024;
    public const long MANOR_RIGHT_HIGH_UPPER      = 91_025;
    public const long MANOR_RIGHT_HIGH_TOWER      = 91_026;
    public const long MANOR_RIGHT_EXTENSION       = 91_027;
    public const long MANOR_RIGHT_TREE            = 91_028;
    public const long MANOR_OBSERVATORY_BASE      = 91_029;
    public const long MANOR_OBSERVATORY_TELESCOPE = 91_030;
    public const long CASTLE_BOSS_REWARD          = 91_100;
    public const long GARDEN_BOSS_REWARD          = 91_102;
    public const long TOWER_BOSS_REWARD           = 91_104;
    public const long DUNGEON_BOSS_REWARD         = 91_106;
    public const long JUKEBOX_REWARD              = 91_200;
    public const long PORTRAIT_REWARD             = 91_201;
    public const long CHEST_GAME_REWARD           = 91_202;
    public const long CARNIVAL_GAME_REWARD        = 91_203;
    public const long SECRET_ROOM_LEFT            = 91_204;
    public const long SECRET_ROOM_RIGHT           = 91_205;
    public const long MINI_BOSS_BARBATOS_AMON     = 91_206;
    public const long MINI_BOSS_BOTIS             = 91_207;
    public const long MINI_BOSS_STOLAS_FOCALOR    = 91_208;
    public const long MINI_BOSS_SALLOS            = 91_209;
    public const long MINI_BOSS_BERITH_HALPHAS    = 91_210;

    public const long CASTLE_STARTING_CHEST          = 91_600;
    public const long GARDEN_STARTING_CHEST          = 91_700;
    public const long TOWER_STARTING_CHEST           = 91_800;
    public const long DUNGEON_STARTING_CHEST         = 91_900;
    public const long UNIVERSAL_STARTING_CHEST       = 92_000;
    public const long CASTLE_STARTING_FAIRY_CHEST    = 91_400;
    public const long GARDEN_STARTING_FAIRY_CHEST    = 91_450;
    public const long TOWER_STARTING_FAIRY_CHEST     = 91_500;
    public const long DUNGEON_STARTING_FAIRY_CHEST   = 91_550;
    public const long UNIVERSAL_STARTING_FAIRY_CHEST = 92_200;
    public const long STARTING_DIARY                 = 91_300;
    public const long FINAL_DIARY                    = 91_324;

    public static bool TryGetChestLocation(RandomizerData randomizerData, int chest, Zone zone, out long location)
    {
        var universalChests = randomizerData.UniversalChests;
        var maxChests = universalChests ? randomizerData.ChestsPerZone * 4 : randomizerData.ChestsPerZone;

        if (chest >= maxChests)
        {
            location = -1;
            return false;
        }

        if (universalChests)
            location = UNIVERSAL_STARTING_CHEST + chest;
        else
            location = zone switch
            {
                Zone.Castle  => CASTLE_STARTING_CHEST + chest,
                Zone.Garden  => GARDEN_STARTING_CHEST + chest,
                Zone.Tower   => TOWER_STARTING_CHEST + chest,
                Zone.Dungeon => DUNGEON_STARTING_CHEST + chest,
                _            => throw new InvalidEnumArgumentException($"Zone {zone} is invalid.")
            };

        return true;
    }

    public static bool TryGetFairyChestLocation(RandomizerData randomizerData, int chest, Zone zone, out long location)
    {
        var universalChests = randomizerData.UniversalFairyChests;
        var maxChests = universalChests ? randomizerData.FairyChestsPerZone * 4 : randomizerData.FairyChestsPerZone;

        if (chest >= maxChests)
        {
            location = -1;
            return false;
        }

        if (universalChests)
            location = UNIVERSAL_STARTING_FAIRY_CHEST + chest;
        else
            location = zone switch
            {
                Zone.Castle  => CASTLE_STARTING_FAIRY_CHEST + chest,
                Zone.Garden  => GARDEN_STARTING_FAIRY_CHEST + chest,
                Zone.Tower   => TOWER_STARTING_FAIRY_CHEST + chest,
                Zone.Dungeon => DUNGEON_STARTING_FAIRY_CHEST + chest,
                _            => throw new InvalidEnumArgumentException($"Zone {zone} is invalid.")
            };

        return true;
    }

    public static bool TryGetDiaryLocation(int diary, out long location)
    {
        if (diary >= MAX_DIARIES)
        {
            location = -1;
            return false;
        }

        location = STARTING_DIARY + diary;
        return true;
    }
}