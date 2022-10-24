// Rogue Legacy Randomizer - ItemDefinitions.cs
// Last Modified 2022-10-24
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Archipelago.Definitions
{
    public static class ItemDefinitions
    {
        // Vendors
        public static readonly ItemData Blacksmith            = new(ItemCode.BLACKSMITH, "Blacksmith", ItemType.Skill);
        public static readonly ItemData Enchantress           = new(ItemCode.ENCHANTRESS, "Enchantress", ItemType.Skill);
        public static readonly ItemData Architect             = new(ItemCode.ARCHITECT, "Architect", ItemType.Skill);

        // Classes
        public static readonly ItemData Knight                = new(ItemCode.KNIGHT, "Knights", ItemType.Skill);
        public static readonly ItemData Paladin               = new(ItemCode.PALADIN, "Paladins", ItemType.Skill);
        public static readonly ItemData Mage                  = new(ItemCode.MAGE, "Mages", ItemType.Skill);
        public static readonly ItemData Archmage              = new(ItemCode.ARCHMAGE, "Archmages", ItemType.Skill);
        public static readonly ItemData Barbarian             = new(ItemCode.BARBARIAN, "Barbarians", ItemType.Skill);
        public static readonly ItemData BarbarianKing         = new(ItemCode.BARBARIAN_KING, "Barbarian Kings", ItemType.Skill);
        public static readonly ItemData Knave                 = new(ItemCode.KNAVE, "Knaves", ItemType.Skill);
        public static readonly ItemData Assassin              = new(ItemCode.ASSASSIN, "Assassins", ItemType.Skill);
        public static readonly ItemData Shinobi               = new(ItemCode.SHINOBI, "Shinobis", ItemType.Skill);
        public static readonly ItemData Hokage                = new(ItemCode.HOKAGE, "Hokages", ItemType.Skill);
        public static readonly ItemData Miner                 = new(ItemCode.MINER, "Miners", ItemType.Skill);
        public static readonly ItemData Spelunker             = new(ItemCode.SPELUNKER, "Spelunker", ItemType.Skill);
        public static readonly ItemData Lich                  = new(ItemCode.LICH, "Liches", ItemType.Skill);
        public static readonly ItemData LichKing              = new(ItemCode.LICH_KING, "Lich Kings", ItemType.Skill);
        public static readonly ItemData Spellthief            = new(ItemCode.SPELLTHIEF, "Spellthieves", ItemType.Skill);
        public static readonly ItemData Spellsword            = new(ItemCode.SPELLSWORD, "Spellswords", ItemType.Skill);
        public static readonly ItemData Dragon                = new(ItemCode.DRAGON, "Dragons", ItemType.Skill);
        public static readonly ItemData Traitors              = new(ItemCode.TRAITOR, "Traitors", ItemType.Skill);
        public static readonly ItemData ProgressiveKnight     = new(ItemCode.PROGRESSIVE_KNIGHT, "Progressive Knights", ItemType.Skill);
        public static readonly ItemData ProgressiveMage       = new(ItemCode.PROGRESSIVE_MAGE, "Progressive Mages", ItemType.Skill);
        public static readonly ItemData ProgressiveBarbarian  = new(ItemCode.PROGRESSIVE_BARBARIAN, "Progressive Barbarians", ItemType.Skill);
        public static readonly ItemData ProgressiveKnave      = new(ItemCode.PROGRESSIVE_KNAVE, "Progressive Knaves", ItemType.Skill);
        public static readonly ItemData ProgressiveShinobi    = new(ItemCode.PROGRESSIVE_SHINOBI, "Progressive Shinobis", ItemType.Skill);
        public static readonly ItemData ProgressiveMiner      = new(ItemCode.PROGRESSIVE_MINER, "Progressive Miners", ItemType.Skill);
        public static readonly ItemData ProgressiveLich       = new(ItemCode.PROGRESSIVE_LICH, "Progressive Liches", ItemType.Skill);
        public static readonly ItemData ProgressiveSpellthief = new(ItemCode.PROGRESSIVE_SPELLTHEIF, "Progressive Spellthieves", ItemType.Skill);

        // Skills
        public static readonly ItemData HealthUp              = new(ItemCode.HEALTH, "Health Up", ItemType.Skill);
        public static readonly ItemData ManaUp                = new(ItemCode.MANA, "Mana Up", ItemType.Skill);
        public static readonly ItemData AttackUp              = new(ItemCode.ATTACK, "Attack Up", ItemType.Skill);
        public static readonly ItemData MagicDamageUp         = new(ItemCode.MAGIC_DAMAGE, "Magic Damage Up", ItemType.Skill);
        public static readonly ItemData ArmorUp               = new(ItemCode.ARMOR, "Armor Up", ItemType.Skill);
        public static readonly ItemData EquipUp               = new(ItemCode.EQUIP, "Equip Up", ItemType.Skill);
        public static readonly ItemData CritChanceUp          = new(ItemCode.CRIT_CHANCE, "Crit Chance Up", ItemType.Skill);
        public static readonly ItemData CritDamageUp          = new(ItemCode.CRIT_DAMAGE, "Crit Damage Up", ItemType.Skill);
        public static readonly ItemData DownStrikeUp          = new(ItemCode.DOWN_STRIKE, "Down Strike Up", ItemType.Skill);
        public static readonly ItemData GoldGainUp            = new(ItemCode.GOLD_GAIN, "Gold Gain Up", ItemType.Skill);
        public static readonly ItemData PotionEfficiencyUp    = new(ItemCode.POTION_EFFICIENCY, "Potion Efficiency Up", ItemType.Skill);
        public static readonly ItemData InvulnTimeUp          = new(ItemCode.INVULN_TIME, "Invulnerability Time Up", ItemType.Skill);
        public static readonly ItemData ManaCostDown          = new(ItemCode.MANA_COST_DOWN, "Mana Cost Down", ItemType.Skill);
        public static readonly ItemData DeathDefiance         = new(ItemCode.DEATH_DEFIANCE, "Death Defiance", ItemType.Skill);
        public static readonly ItemData Haggling              = new(ItemCode.HAGGLING, "Haggling", ItemType.Skill);
        public static readonly ItemData RandomizeChildren     = new(ItemCode.RANDOMIZE_CHILDREN, "Randomize Children", ItemType.Skill);

        // Blueprints
        public static readonly ItemData ProgressiveArmor      = new(ItemCode.EQUIPMENT_PROGRESSIVE, "Progressive Blueprints", ItemType.Blueprint);
        public static readonly ItemData SquireArmor           = new(ItemCode.EQUIPMENT_SQUIRE, "Squire Blueprints", ItemType.Blueprint);
        public static readonly ItemData SilverArmor           = new(ItemCode.EQUIPMENT_SILVER, "Silver Blueprints", ItemType.Blueprint);
        public static readonly ItemData GuardianArmor         = new(ItemCode.EQUIPMENT_GUARDIAN, "Guardian Blueprints", ItemType.Blueprint);
        public static readonly ItemData ImperialArmor         = new(ItemCode.EQUIPMENT_IMPERIAL, "Imperial Blueprints", ItemType.Blueprint);
        public static readonly ItemData RoyalArmor            = new(ItemCode.EQUIPMENT_ROYAL, "Royal Blueprints", ItemType.Blueprint);
        public static readonly ItemData KnightArmor           = new(ItemCode.EQUIPMENT_KNIGHT, "Knight Blueprints", ItemType.Blueprint);
        public static readonly ItemData RangerArmor           = new(ItemCode.EQUIPMENT_RANGER, "Ranger Blueprints", ItemType.Blueprint);
        public static readonly ItemData SkyArmor              = new(ItemCode.EQUIPMENT_SKY, "Sky Blueprints", ItemType.Blueprint);
        public static readonly ItemData DragonArmor           = new(ItemCode.EQUIPMENT_DRAGON, "Dragon Blueprints", ItemType.Blueprint);
        public static readonly ItemData SlayerArmor           = new(ItemCode.EQUIPMENT_SLAYER, "Slayer Blueprints", ItemType.Blueprint);
        public static readonly ItemData BloodArmor            = new(ItemCode.EQUIPMENT_BLOOD, "Blood Blueprints", ItemType.Blueprint);
        public static readonly ItemData SageArmor             = new(ItemCode.EQUIPMENT_SAGE, "Sage Blueprints", ItemType.Blueprint);
        public static readonly ItemData RetributionArmor      = new(ItemCode.EQUIPMENT_RETRIBUTION, "Retribution Armor Blueprints", ItemType.Blueprint);
        public static readonly ItemData HolyArmor             = new(ItemCode.EQUIPMENT_HOLY, "Holy Armor Blueprints", ItemType.Blueprint);
        public static readonly ItemData DarkArmor             = new(ItemCode.EQUIPMENT_DARK, "Dark Armor Blueprints", ItemType.Blueprint);

        // Runes
        public static readonly ItemData VaultRunes            = new(ItemCode.RUNE_VAULT, "Vault Runes", ItemType.Rune);
        public static readonly ItemData SprintRunes           = new(ItemCode.RUNE_SPRINT, "Sprint Runes", ItemType.Rune);
        public static readonly ItemData VampireRunes          = new(ItemCode.RUNE_VAMPIRE, "Vampire Runes", ItemType.Rune);
        public static readonly ItemData SkyRunes              = new(ItemCode.RUNE_SKY, "Sky Runes", ItemType.Rune);
        public static readonly ItemData SiphonRunes           = new(ItemCode.RUNE_SIPHON, "Siphon Runes", ItemType.Rune);
        public static readonly ItemData RetaliationRunes      = new(ItemCode.RUNE_RETALIATION, "Retaliation Runes", ItemType.Rune);
        public static readonly ItemData BountyRunes           = new(ItemCode.RUNE_BOUNTY, "Bounty Runes", ItemType.Rune);
        public static readonly ItemData HasteRunes            = new(ItemCode.RUNE_HASTE, "Haste Runes", ItemType.Rune);
        public static readonly ItemData CurseRunes            = new(ItemCode.RUNE_CURSE, "Curse Runes", ItemType.Rune);
        public static readonly ItemData GraceRunes            = new(ItemCode.RUNE_GRACE, "Grace Runes", ItemType.Rune);
        public static readonly ItemData BalanceRunes          = new(ItemCode.RUNE_BALANCE, "Balance Runes", ItemType.Rune);

        // Misc. Items
        public static readonly ItemData TripStatIncrease      = new(ItemCode.TRIP_STAT_INCREASE, "Triple Stat Increase", ItemType.Stats);
        public static readonly ItemData Gold1000              = new(ItemCode.GOLD_1000, "1000 Gold", ItemType.Gold);
        public static readonly ItemData Gold3000              = new(ItemCode.GOLD_3000, "3000 Gold", ItemType.Gold);
        public static readonly ItemData Gold5000              = new(ItemCode.GOLD_5000, "5000 Gold", ItemType.Gold);
        public static readonly ItemData RageTrap              = new(ItemCode.RAGE_TRAP, "Rage Trap", ItemType.Trap);

        public static IEnumerable<ItemData> GetAllItems()
        {
            return typeof(ItemDefinitions)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => field.FieldType == typeof(ItemData))
                .Select(field => (ItemData) field.GetValue(null))
                .ToList();
        }

        public static ItemData GetItem(long code)
        {
            return GetAllItems().First(item => item.Code == code);
        }

        public static ItemData GetItem(string name)
        {
            return GetAllItems().First(item => item.Name == name);
        }

        public static ItemType GetItemType(this long code)
        {
            try
            {
                return GetItem(code).Type;
            }
            catch
            {
                return ItemType.NetworkItem;
            }
        }
    }
}
