// 
//  RogueLegacyArchipelago - ItemCode.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;

namespace Archipelago
{
    public static class ItemCodeExtensions
    {
        public static LegacyItemType GetItemType(this int item)
        {
            if (item >= 90000 && item <= 90028)
            {
                return LegacyItemType.Skill;
            }

            if (item >= 90030 && item <= 90030)
            {
                return LegacyItemType.Stats;
            }

            if (item >= 90031 && item <= 90033)
            {
                return LegacyItemType.Gold;
            }

            if (item >= 90060 && item <= 90070)
            {
                return LegacyItemType.Rune;
            }

            if (item >= 90040 && item <= 90054)
            {
                return LegacyItemType.Blueprint;
            }

            throw new NotImplementedException(string.Format("ITEM: {0} is not implemented.", item));
        }
    }

    public enum LegacyItemType
    {
        Blueprint,
        Rune,
        Skill,
        SpecialSkill,
        Stats,
        Gold,
        Other
    }

    public enum ItemCode
    {
        Blacksmith = 90000,
        Enchantress = 90001,
        Architect = 90002,
        Paladins = 90003,
        Archmages = 90004,
        BarbarianKings = 90005,
        Assassins = 90006,
        ProgressiveShinobi = 90007,
        ProgressiveMiner = 90008,
        ProgressiveLich = 90009,
        ProgressiveSpellthief = 90010,
        Dragons = 90011,
        Traitors = 90012,
        HealthUp = 90013,
        ManaUp = 90014,
        AttackUp = 90015,
        MagicDamageUp = 90016,
        ArmorUp = 90017,
        EquipUp = 90018,
        CritChanceUp = 90019,
        CritDamageUp = 90020,
        DownStrikeUp = 90021,
        GoldGainUp = 90022,
        PotionEfficiencyUp = 90023,
        InvulnerabilityTimeUp = 90024,
        ManaCostDown = 90025,
        DeathDefiance = 90026,
        Haggling = 90027,
        RandomizeChildren = 90028,
        TripleStatIncreases = 90030,
        Gold1000 = 90031,
        Gold3000 = 90032,
        Gold5000 = 90033,
        SquireArmorBlueprints = 90040,
        SilverArmorBlueprints = 90041,
        GuardianArmorBlueprints = 90042,
        ImperialArmorBlueprints = 90043,
        RoyalArmorBlueprints = 90044,
        KnightArmorBlueprints = 90045,
        RangerArmorBlueprints = 90046,
        SkyArmorBlueprints = 90047,
        DragonArmorBlueprints = 90048,
        SlayerArmorBlueprints = 90049,
        BloodArmorBlueprints = 90050,
        SageArmorBlueprints = 90051,
        RetributionArmorBlueprints = 90052,
        HolyArmorBlueprints = 90053,
        DarkArmorBlueprints = 90054,
        VaultRunes = 90060,
        SprintRunes = 90061,
        VampireRunes = 90062,
        SkyRunes = 90063,
        SiphonRunes = 90064,
        RetaliationRunes = 90065,
        BountyRunes = 90066,
        HasteRunes = 90067,
        CurseRunes = 90068,
        GraceRunes = 90069,
        BalanceRunes = 90070
    }
}