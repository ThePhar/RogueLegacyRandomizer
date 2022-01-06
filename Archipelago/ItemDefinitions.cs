using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Archipelago
{
    public static class ItemDefinitions
    {
        // Vendors
        public static readonly ItemData Blacksmith            = new(90000, "Blacksmith",                   ItemType.Skill);
        public static readonly ItemData Enchantress           = new(90001, "Enchantress",                  ItemType.Skill);
        public static readonly ItemData Architect             = new(90002, "Architect",                    ItemType.Skill);

        // Classes
        public static readonly ItemData Paladin               = new(90081, "Paladins",                     ItemType.Skill);
        public static readonly ItemData Archmage              = new(90083, "Archmages",                    ItemType.Skill);
        public static readonly ItemData BarbarianKing         = new(90085, "Barbarian Kings",              ItemType.Skill);
        public static readonly ItemData Assassin              = new(90087, "Assassins",                    ItemType.Skill);
        public static readonly ItemData Dragon                = new(90096, "Dragons",                      ItemType.Skill);
        public static readonly ItemData Traitors              = new(90097, "Traitors",                     ItemType.Skill);
        public static readonly ItemData ProgressiveShinobi    = new(90007, "Progressive Shinobis",         ItemType.Skill);
        public static readonly ItemData ProgressiveMiner      = new(90008, "Progressive Miners",           ItemType.Skill);
        public static readonly ItemData ProgressiveLich       = new(90009, "Progressive Liches",           ItemType.Skill);
        public static readonly ItemData ProgressiveSpellthief = new(90010, "Progressive Spellthieves",     ItemType.Skill);

        // Skills
        public static readonly ItemData HealthUp              = new(90013, "Health Up",                    ItemType.Skill);
        public static readonly ItemData ManaUp                = new(90014, "Mana Up",                      ItemType.Skill);
        public static readonly ItemData AttackUp              = new(90015, "Attack Up",                    ItemType.Skill);
        public static readonly ItemData MagicDamageUp         = new(90016, "Magic Damage Up",              ItemType.Skill);
        public static readonly ItemData ArmorUp               = new(90017, "Armor Up",                     ItemType.Skill);
        public static readonly ItemData EquipUp               = new(90018, "Equip Up",                     ItemType.Skill);
        public static readonly ItemData CritChanceUp          = new(90019, "Crit Chance Up",               ItemType.Skill);
        public static readonly ItemData CritDamageUp          = new(90020, "Crit Damage Up",               ItemType.Skill);
        public static readonly ItemData DownStrikeUp          = new(90021, "Down Strike Up",               ItemType.Skill);
        public static readonly ItemData GoldGainUp            = new(90022, "Gold Gain Up",                 ItemType.Skill);
        public static readonly ItemData PotionEfficiencyUp    = new(90023, "Potion Efficiency Up",         ItemType.Skill);
        public static readonly ItemData InvulnTimeUp          = new(90024, "Invulnerability Time Up",      ItemType.Skill);
        public static readonly ItemData ManaCostDown          = new(90025, "Mana Cost Down",               ItemType.Skill);
        public static readonly ItemData DeathDefiance         = new(90026, "Death Defiance",               ItemType.Skill);
        public static readonly ItemData Haggling              = new(90027, "Haggling",                     ItemType.Skill);
        public static readonly ItemData RandomizeChildren     = new(90028, "Randomize Children",           ItemType.Skill);

        // Blueprints
        public static readonly ItemData SquireArmor           = new(90040, "Squire Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData SilverArmor           = new(90041, "Silver Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData GuardianArmor         = new(90042, "Guardian Armor Blueprints",    ItemType.Blueprint);
        public static readonly ItemData ImperialArmor         = new(90043, "Imperial Armor Blueprints",    ItemType.Blueprint);
        public static readonly ItemData RoyalArmor            = new(90044, "Royal Armor Blueprints",       ItemType.Blueprint);
        public static readonly ItemData KnightArmor           = new(90045, "Knight Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData RangerArmor           = new(90046, "Ranger Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData SkyArmor              = new(90047, "Sky Armor Blueprints",         ItemType.Blueprint);
        public static readonly ItemData DragonArmor           = new(90048, "Dragon Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData SlayerArmor           = new(90049, "Slayer Armor Blueprints",      ItemType.Blueprint);
        public static readonly ItemData BloodArmor            = new(90050, "Blood Armor Blueprints",       ItemType.Blueprint);
        public static readonly ItemData SageArmor             = new(90051, "Sage Armor Blueprints",        ItemType.Blueprint);
        public static readonly ItemData RetributionArmor      = new(90052, "Retribution Armor Blueprints", ItemType.Blueprint);
        public static readonly ItemData HolyArmor             = new(90053, "Holy Armor Blueprints",        ItemType.Blueprint);
        public static readonly ItemData DarkArmor             = new(90054, "Dark Armor Blueprints",        ItemType.Blueprint);

        // Runes
        public static readonly ItemData VaultRunes            = new(90060, "Vault Runes",                  ItemType.Rune);
        public static readonly ItemData SprintRunes           = new(90061, "Sprint Runes",                 ItemType.Rune);
        public static readonly ItemData VampireRunes          = new(90062, "Vampire Runes",                ItemType.Rune);
        public static readonly ItemData SkyRunes              = new(90063, "Sky Runes",                    ItemType.Rune);
        public static readonly ItemData SiphonRunes           = new(90064, "Siphon Runes",                 ItemType.Rune);
        public static readonly ItemData RetaliationRunes      = new(90065, "Retaliation Runes",            ItemType.Rune);
        public static readonly ItemData BountyRunes           = new(90066, "Bounty Runes",                 ItemType.Rune);
        public static readonly ItemData HasteRunes            = new(90067, "Haste Runes",                  ItemType.Rune);
        public static readonly ItemData CurseRunes            = new(90068, "Curse Runes",                  ItemType.Rune);
        public static readonly ItemData GraceRunes            = new(90069, "Grace Runes",                  ItemType.Rune);
        public static readonly ItemData BalanceRunes          = new(90070, "Balance Runes",                ItemType.Rune);

        // Misc. Items
        public static readonly ItemData TripStatIncrease      = new(90030, "Triple Stat Increase",         ItemType.Stats);
        public static readonly ItemData Gold1000              = new(90031, "1000 Gold",                    ItemType.Gold);
        public static readonly ItemData Gold3000              = new(90032, "1000 Gold",                    ItemType.Gold);
        public static readonly ItemData Gold5000              = new(90033, "1000 Gold",                    ItemType.Gold);

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

        public static ItemType GetItemType(this int code)
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