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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Archipelago
{
    public static class ItemDefinitions
    {
        // Vendors
        public static readonly ItemData Blacksmith            = new ItemData(90000, "Blacksmith",                   ItemType.Skill);
        public static readonly ItemData Enchantress           = new ItemData(90001, "Enchantress",                  ItemType.Skill);
        public static readonly ItemData Architect             = new ItemData(90002, "Architect",                    ItemType.Skill);

        // Classes
        public static readonly ItemData Paladin               = new ItemData(90081, "Paladins",                     ItemType.Skill);
        public static readonly ItemData Archmage              = new ItemData(90083, "Archmages",                    ItemType.Skill);
        public static readonly ItemData BarbarianKing         = new ItemData(90085, "Barbarian Kings",              ItemType.Skill);
        public static readonly ItemData Assassin              = new ItemData(90087, "Assassins",                    ItemType.Skill);
        public static readonly ItemData Dragon                = new ItemData(90096, "Dragons",                      ItemType.Skill);
        public static readonly ItemData Traitors              = new ItemData(90097, "Traitors",                     ItemType.Skill);
        public static readonly ItemData ProgressiveShinobi    = new ItemData(90007, "Progressive Shinobis",         ItemType.Skill);
        public static readonly ItemData ProgressiveMiner      = new ItemData(90008, "Progressive Miners",           ItemType.Skill);
        public static readonly ItemData ProgressiveLich       = new ItemData(90009, "Progressive Liches",           ItemType.Skill);
        public static readonly ItemData ProgressiveSpellthief = new ItemData(90010, "Progressive Spellthieves",     ItemType.Skill);

        // Skills
        public static readonly ItemData HealthUp              = new ItemData(90013, "Health Up",                    ItemType.Skill);
        public static readonly ItemData ManaUp                = new ItemData(90014, "Mana Up",                      ItemType.Skill);
        public static readonly ItemData AttackUp              = new ItemData(90015, "Attack Up",                    ItemType.Skill);
        public static readonly ItemData MagicDamageUp         = new ItemData(90016, "Magic Damage Up",              ItemType.Skill);
        public static readonly ItemData ArmorUp               = new ItemData(90017, "Armor Up",                     ItemType.Skill);
        public static readonly ItemData EquipUp               = new ItemData(90018, "Equip Up",                     ItemType.Skill);
        public static readonly ItemData CritChanceUp          = new ItemData(90019, "Crit Chance Up",               ItemType.Skill);
        public static readonly ItemData CritDamageUp          = new ItemData(90020, "Crit Damage Up",               ItemType.Skill);
        public static readonly ItemData DownStrikeUp          = new ItemData(90021, "Down Strike Up",               ItemType.Skill);
        public static readonly ItemData GoldGainUp            = new ItemData(90022, "Gold Gain Up",                 ItemType.Skill);
        public static readonly ItemData PotionEfficiencyUp    = new ItemData(90023, "Potion Efficiency Up",         ItemType.Skill);
        public static readonly ItemData InvulnTimeUp          = new ItemData(90024, "Invulnerability Time Up",      ItemType.Skill);
        public static readonly ItemData ManaCostDown          = new ItemData(90025, "Mana Cost Down",               ItemType.Skill);
        public static readonly ItemData DeathDefiance         = new ItemData(90026, "Death Defiance",               ItemType.Skill);
        public static readonly ItemData Haggling              = new ItemData(90027, "Haggling",                     ItemType.Skill);
        public static readonly ItemData RandomizeChildren     = new ItemData(90028, "Randomize Children",           ItemType.Skill);

        // Blueprints
        public static readonly ItemData SquireArmor           = new ItemData(90040, "Squire Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData SilverArmor           = new ItemData(90041, "Silver Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData GuardianArmor         = new ItemData(90042, "Guardian Armor Blueprints",    ItemType.Blueprint);
        public static readonly ItemData ImperialArmor         = new ItemData(90043, "Imperial Armor Blueprints",    ItemType.Blueprint);
        public static readonly ItemData RoyalArmor            = new ItemData(90044, "Royal Armor Blueprints",       ItemType.Blueprint);
        public static readonly ItemData KnightArmor           = new ItemData(90045, "Knight Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData RangerArmor           = new ItemData(90046, "Ranger Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData SkyArmor              = new ItemData(90047, "Sky Armor Blueprints",         ItemType.Blueprint);
        public static readonly ItemData DragonArmor           = new ItemData(90048, "Dragon Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData SlayerArmor           = new ItemData(90049, "Slayer Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData BloodArmor            = new ItemData(90050, "Blood Armor Blueprints",       ItemType.Blueprint);
        public static readonly ItemData SageArmor             = new ItemData(90051, "Sage Armor Blueprints",        ItemType.Blueprint);
        public static readonly ItemData RetributionArmor      = new ItemData(90052, "Retribution Armor Blueprints", ItemType.Blueprint);
        public static readonly ItemData HolyArmor             = new ItemData(90053, "Holy Armor Blueprints",        ItemType.Blueprint);
        public static readonly ItemData DarkArmor             = new ItemData(90054, "Dark Armor Blueprints",        ItemType.Blueprint);

        // Runes
        public static readonly ItemData VaultRunes            = new ItemData(90060, "Vault Runes",                  ItemType.Rune);
        public static readonly ItemData SprintRunes           = new ItemData(90061, "Sprint Runes",                 ItemType.Rune);
        public static readonly ItemData VampireRunes          = new ItemData(90062, "Vampire Runes",                ItemType.Rune);
        public static readonly ItemData SkyRunes              = new ItemData(90063, "Sky Runes",                    ItemType.Rune);
        public static readonly ItemData SiphonRunes           = new ItemData(90064, "Siphon Runes",                 ItemType.Rune);
        public static readonly ItemData RetaliationRunes      = new ItemData(90065, "Retaliation Runes",            ItemType.Rune);
        public static readonly ItemData BountyRunes           = new ItemData(90066, "Bounty Runes",                 ItemType.Rune);
        public static readonly ItemData HasteRunes            = new ItemData(90067, "Haste Runes",                  ItemType.Rune);
        public static readonly ItemData CurseRunes            = new ItemData(90068, "Curse Runes",                  ItemType.Rune);
        public static readonly ItemData GraceRunes            = new ItemData(90069, "Grace Runes",                  ItemType.Rune);
        public static readonly ItemData BalanceRunes          = new ItemData(90070, "Balance Runes",                ItemType.Rune);

        // Misc. Items
        public static readonly ItemData TripStatIncrease      = new ItemData(90030, "Triple Stat Increase",         ItemType.Stats);
        public static readonly ItemData Gold1000              = new ItemData(90031, "1000 Gold",                    ItemType.Gold);
        public static readonly ItemData Gold3000              = new ItemData(90032, "1000 Gold",                    ItemType.Gold);
        public static readonly ItemData Gold5000              = new ItemData(90033, "1000 Gold",                    ItemType.Gold);

        public static IEnumerable<ItemData> GetAllItems()
        {
            return typeof(ItemDefinitions)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => field.FieldType == typeof(ItemData))
                .Select(field => (ItemData) field.GetValue(null))
                .ToList();
        }

        public static ItemData GetItem(int code)
        {
            return GetAllItems().First(item => item.Code == code);
        }

        public static ItemData GetItem(string name)
        {
            return GetAllItems().First(item => item.Name == name);
        }
    }
}
