// RogueLegacyRandomizer - ItemCode.cs
// Last Modified 2023-07-30 8:32 AM by
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

namespace Randomizer.Definitions;

public static class ItemCode
{
    public const long BLACKSMITH             = 90_000;
    public const long ENCHANTRESS            = 90_001;
    public const long ARCHITECT              = 90_002;
    public const long PROGRESSIVE_KNIGHT     = 90_003;
    public const long PROGRESSIVE_MAGE       = 90_004;
    public const long PROGRESSIVE_BARBARIAN  = 90_005;
    public const long PROGRESSIVE_KNAVE      = 90_006;
    public const long PROGRESSIVE_SHINOBI    = 90_007;
    public const long PROGRESSIVE_MINER      = 90_008;
    public const long PROGRESSIVE_LICH       = 90_009;
    public const long PROGRESSIVE_SPELLTHIEF = 90_010;
    public const long DRAGON                 = 90_096;
    public const long TRAITOR                = 90_097;

    public const long HEALTH                 = 90_013;
    public const long MANA                   = 90_014;
    public const long ATTACK                 = 90_015;
    public const long MAGIC_DAMAGE           = 90_016;
    public const long ARMOR                  = 90_017;
    public const long EQUIP                  = 90_018;
    public const long CRIT_CHANCE            = 90_019;
    public const long CRIT_DAMAGE            = 90_020;
    public const long DOWN_STRIKE            = 90_021;
    public const long GOLD_GAIN              = 90_022;
    public const long POTION_EFFICIENCY      = 90_023;
    public const long INVULN_TIME            = 90_024;
    public const long MANA_COST_DOWN         = 90_025;
    public const long DEATH_DEFIANCE         = 90_026;
    public const long HAGGLING               = 90_027;
    public const long RANDOMIZE_CHILDREN     = 90_028;

    public const long TRIP_STAT_INCREASE     = 90_030;
    public const long GOLD_1000              = 90_031;
    public const long GOLD_3000              = 90_032;
    public const long GOLD_5000              = 90_033;

    public const long TRAP_TELEPORT          = 90_150;
    public const long TRAP_HEDGEHOG          = 90_151;
    public const long TRAP_VERTIGO           = 90_152;
    public const long TRAP_GENETIC_LOTTERY   = 90_153;

    public const long SHRINE_CHARON          = 90_160;
    public const long SHRINE_HYPERION        = 90_161;
    public const long SHRINE_HERMES          = 90_162;
    public const long SHRINE_HELIOS          = 90_163;
    public const long SHRINE_CALYPSO         = 90_164;
    public const long SHRINE_NERDY           = 90_165;
    public const long SHRINE_PHAR            = 90_169;

    public const long BLACKSMITH_SWORD       = 90_105;
    public const long BLACKSMITH_HELM        = 90_106;
    public const long BLACKSMITH_CHEST       = 90_107;
    public const long BLACKSMITH_LIMBS       = 90_108;
    public const long BLACKSMITH_CAPE        = 90_109;
    public const long ENCHANTRESS_SWORD      = 90_100;
    public const long ENCHANTRESS_HELM       = 90_101;
    public const long ENCHANTRESS_CHEST      = 90_102;
    public const long ENCHANTRESS_LIMBS      = 90_103;
    public const long ENCHANTRESS_CAPE       = 90_104;

    public const long EQUIPMENT_PROGRESSIVE  = 90_055;
    public const long EQUIPMENT_SQUIRE       = 90_040;
    public const long EQUIPMENT_SILVER       = 90_041;
    public const long EQUIPMENT_GUARDIAN     = 90_042;
    public const long EQUIPMENT_IMPERIAL     = 90_043;
    public const long EQUIPMENT_ROYAL        = 90_044;
    public const long EQUIPMENT_KNIGHT       = 90_045;
    public const long EQUIPMENT_RANGER       = 90_046;
    public const long EQUIPMENT_SKY          = 90_047;
    public const long EQUIPMENT_DRAGON       = 90_048;
    public const long EQUIPMENT_SLAYER       = 90_049;
    public const long EQUIPMENT_BLOOD        = 90_050;
    public const long EQUIPMENT_SAGE         = 90_051;
    public const long EQUIPMENT_RETRIBUTION  = 90_052;
    public const long EQUIPMENT_HOLY         = 90_053;
    public const long EQUIPMENT_DARK         = 90_054;

    public const long RUNE_VAULT             = 90_060;
    public const long RUNE_SPRINT            = 90_061;
    public const long RUNE_VAMPIRE           = 90_062;
    public const long RUNE_SKY               = 90_063;
    public const long RUNE_SIPHON            = 90_064;
    public const long RUNE_RETALIATION       = 90_065;
    public const long RUNE_BOUNTY            = 90_066;
    public const long RUNE_HASTE             = 90_067;
    public const long RUNE_CURSE             = 90_068;
    public const long RUNE_GRACE             = 90_069;
    public const long RUNE_BALANCE           = 90_070;

    public const long FOUNTAIN_PIECE         = 90_180;
    public const long SPENDING               = 90_190;

    public static ItemType GetItemType(this long itemCode)
    {
        return itemCode switch
        {
            BLACKSMITH             => ItemType.Skill,
            ENCHANTRESS            => ItemType.Skill,
            ARCHITECT              => ItemType.Skill,
            PROGRESSIVE_KNIGHT     => ItemType.Skill,
            PROGRESSIVE_MAGE       => ItemType.Skill,
            PROGRESSIVE_BARBARIAN  => ItemType.Skill,
            PROGRESSIVE_KNAVE      => ItemType.Skill,
            PROGRESSIVE_SHINOBI    => ItemType.Skill,
            PROGRESSIVE_MINER      => ItemType.Skill,
            PROGRESSIVE_LICH       => ItemType.Skill,
            PROGRESSIVE_SPELLTHIEF => ItemType.Skill,
            DRAGON                 => ItemType.Skill,
            TRAITOR                => ItemType.Skill,
            HEALTH                 => ItemType.Skill,
            MANA                   => ItemType.Skill,
            ATTACK                 => ItemType.Skill,
            MAGIC_DAMAGE           => ItemType.Skill,
            ARMOR                  => ItemType.Skill,
            EQUIP                  => ItemType.Skill,
            CRIT_CHANCE            => ItemType.Skill,
            CRIT_DAMAGE            => ItemType.Skill,
            DOWN_STRIKE            => ItemType.Skill,
            GOLD_GAIN              => ItemType.Skill,
            POTION_EFFICIENCY      => ItemType.Skill,
            INVULN_TIME            => ItemType.Skill,
            MANA_COST_DOWN         => ItemType.Skill,
            DEATH_DEFIANCE         => ItemType.Skill,
            HAGGLING               => ItemType.Skill,
            RANDOMIZE_CHILDREN     => ItemType.Skill,
            TRIP_STAT_INCREASE     => ItemType.Stats,
            GOLD_1000              => ItemType.Gold,
            GOLD_3000              => ItemType.Gold,
            GOLD_5000              => ItemType.Gold,
            EQUIPMENT_PROGRESSIVE  => ItemType.Blueprint,
            EQUIPMENT_SQUIRE       => ItemType.Blueprint,
            EQUIPMENT_SILVER       => ItemType.Blueprint,
            EQUIPMENT_GUARDIAN     => ItemType.Blueprint,
            EQUIPMENT_IMPERIAL     => ItemType.Blueprint,
            EQUIPMENT_ROYAL        => ItemType.Blueprint,
            EQUIPMENT_KNIGHT       => ItemType.Blueprint,
            EQUIPMENT_RANGER       => ItemType.Blueprint,
            EQUIPMENT_SKY          => ItemType.Blueprint,
            EQUIPMENT_DRAGON       => ItemType.Blueprint,
            EQUIPMENT_SLAYER       => ItemType.Blueprint,
            EQUIPMENT_BLOOD        => ItemType.Blueprint,
            EQUIPMENT_SAGE         => ItemType.Blueprint,
            EQUIPMENT_RETRIBUTION  => ItemType.Blueprint,
            EQUIPMENT_HOLY         => ItemType.Blueprint,
            EQUIPMENT_DARK         => ItemType.Blueprint,
            BLACKSMITH_SWORD       => ItemType.Blueprint,
            BLACKSMITH_HELM        => ItemType.Blueprint,
            BLACKSMITH_CHEST       => ItemType.Blueprint,
            BLACKSMITH_LIMBS       => ItemType.Blueprint,
            BLACKSMITH_CAPE        => ItemType.Blueprint,
            RUNE_VAULT             => ItemType.Rune,
            RUNE_SPRINT            => ItemType.Rune,
            RUNE_VAMPIRE           => ItemType.Rune,
            RUNE_SKY               => ItemType.Rune,
            RUNE_SIPHON            => ItemType.Rune,
            RUNE_RETALIATION       => ItemType.Rune,
            RUNE_BOUNTY            => ItemType.Rune,
            RUNE_HASTE             => ItemType.Rune,
            RUNE_CURSE             => ItemType.Rune,
            RUNE_GRACE             => ItemType.Rune,
            RUNE_BALANCE           => ItemType.Rune,
            ENCHANTRESS_SWORD      => ItemType.Rune,
            ENCHANTRESS_HELM       => ItemType.Rune,
            ENCHANTRESS_CHEST      => ItemType.Rune,
            ENCHANTRESS_LIMBS      => ItemType.Rune,
            ENCHANTRESS_CAPE       => ItemType.Rune,
            FOUNTAIN_PIECE         => ItemType.Fountain,
            TRAP_TELEPORT          => ItemType.Trap,
            TRAP_HEDGEHOG          => ItemType.Trap,
            TRAP_VERTIGO           => ItemType.Trap,
            TRAP_GENETIC_LOTTERY   => ItemType.Trap,
            _                      => ItemType.NetworkItem,
        };
    }

    public enum ItemType
    {
        Blueprint,
        Rune,
        Skill,
        Stats,
        Gold,
        NetworkItem,
        Trap,
        Fountain
    }
}