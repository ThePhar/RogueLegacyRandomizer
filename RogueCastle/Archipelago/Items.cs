// 
// RogueLegacyArchipelago - Items.cs
// Last Modified 2021-12-26
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using System.Linq;

namespace RogueCastle.Archipelago
{
    public static class Items
    {
        private static readonly Dictionary<string, int> Table = new Dictionary<string, int>
        {
            // Vendors
            {"Smithy", 4444000},
            {"Architect", 4444001},
            {"Enchantress", 4444002},

            // Classes
            {"Progressive Knight", 4444003},
            {"Progressive Mage", 4444004},
            {"Progressive Barbarian", 4444005},
            {"Progressive Knave", 4444006},
            {"Progressive Shinobi", 4444007},
            {"Progressive Miner", 4444008},
            {"Progressive Lich", 4444009},
            {"Progressive Spell Thief", 4444010},
            {"Dragon", 4444011},
            {"Traitor", 4444012},

            // Skills
            {"Health Up", 4444013},
            {"Mana Up", 4444014},
            {"Attack Up", 4444015},
            {"Magic Damage Up", 4444016},
            {"Armor Up", 4444017},
            {"Equip Up", 4444018},
            {"Crit Chance Up", 4444019},
            {"Crit Damage Up", 4444020},
            {"Down Strike Up", 4444021},
            {"Gold Gain Up", 4444022},
            {"Potion Up", 4444023},
            {"Invulnerability Time Up", 4444024},
            {"Mana Cost Down", 4444025},
            {"Death Defy", 4444026},
            {"Haggle", 4444027},
            {"Randomize Children", 4444028},

            // Extra
            {"Random Stat Increase", 4444029},
            {"Random Triple Stat Increase", 4444030},
            {"Gold Bonus", 4444031},

            // Runes
            {"Sprint (Sword)", 4444032},
            {"Sprint (Helm)", 4444033},
            {"Sprint (Chest)", 4444034},
            {"Sprint (Limbs)", 4444035},
            {"Sprint (Cape)", 4444036},
            {"Vault (Sword)", 4444037},
            {"Vault (Helm)", 4444038},
            {"Vault (Chest)", 4444039},
            {"Vault (Limbs)", 4444040},
            {"Vault (Cape)", 4444041},
            {"Bounty (Sword)", 4444042},
            {"Bounty (Helm)", 4444043},
            {"Bounty (Chest)", 4444044},
            {"Bounty (Limbs)", 4444045},
            {"Bounty (Cape)", 4444046},
            {"Siphon (Sword)", 4444047},
            {"Siphon (Helm)", 4444048},
            {"Siphon (Chest)", 4444049},
            {"Siphon (Limbs)", 4444050},
            {"Siphon (Cape)", 4444051},
            {"Retaliation (Sword)", 4444052},
            {"Retaliation (Helm)", 4444053},
            {"Retaliation (Chest)", 4444054},
            {"Retaliation (Limbs)", 4444055},
            {"Retaliation (Cape)", 4444056},
            {"Grace (Sword)", 4444057},
            {"Grace (Helm)", 4444058},
            {"Grace (Chest)", 4444059},
            {"Grace (Limbs)", 4444060},
            {"Grace (Cape)", 4444061},
            {"Balance (Sword)", 4444062},
            {"Balance (Helm)", 4444063},
            {"Balance (Chest)", 4444064},
            {"Balance (Limbs)", 4444065},
            {"Balance (Cape)", 4444066},
            {"Curse (Sword)", 4444067},
            {"Curse (Helm)", 4444068},
            {"Curse (Chest)", 4444069},
            {"Curse (Limbs)", 4444070},
            {"Curse (Cape)", 4444071},
            {"Vampire (Sword)", 4444072},
            {"Vampire (Helm)", 4444073},
            {"Vampire (Chest)", 4444074},
            {"Vampire (Limbs)", 4444075},
            {"Vampire (Cape)", 4444076},
            {"Sky (Sword)", 4444077},
            {"Sky (Helm)", 4444078},
            {"Sky (Chest)", 4444079},
            {"Sky (Limbs)", 4444080},
            {"Sky (Cape)", 4444081},
            {"Haste (Sword)", 4444082},
            {"Haste (Helm)", 4444083},
            {"Haste (Chest)", 4444084},
            {"Haste (Limbs)", 4444085},
            {"Haste (Cape)", 4444086},

            // Blueprints
            {"Squire Sword", 4444087},
            {"Knight Sword", 4444088},
            {"Blood Sword", 4444089},
            {"Silver Sword", 4444090},
            {"Ranger Sword", 4444091},
            {"Sage Sword", 4444092},
            {"Guardian Sword", 4444093},
            {"Sky Sword", 4444094},
            {"Retribution Sword", 4444095},
            {"Imperial Sword", 4444096},
            {"Dragon Sword", 4444097},
            {"Holy Sword", 4444098},
            {"Royal Sword", 4444099},
            {"Slayer Sword", 4444100},
            {"Dark Sword", 4444101},
            {"Squire Helm", 4444102},
            {"Knight Helm", 4444103},
            {"Blood Helm", 4444104},
            {"Silver Helm", 4444105},
            {"Ranger Helm", 4444106},
            {"Sage Helm", 4444107},
            {"Guardian Helm", 4444108},
            {"Sky Helm", 4444109},
            {"Retribution Helm", 4444110},
            {"Imperial Helm", 4444111},
            {"Dragon Helm", 4444112},
            {"Holy Helm", 4444113},
            {"Royal Helm", 4444114},
            {"Slayer Helm", 4444115},
            {"Dark Helm", 4444116},
            {"Squire Chest", 4444117},
            {"Knight Chest", 4444118},
            {"Blood Chest", 4444119},
            {"Silver Chest", 4444120},
            {"Ranger Chest", 4444121},
            {"Sage Chest", 4444122},
            {"Guardian Chest", 4444123},
            {"Sky Chest", 4444124},
            {"Retribution Chest", 4444125},
            {"Imperial Chest", 4444126},
            {"Dragon Chest", 4444127},
            {"Holy Chest", 4444128},
            {"Royal Chest", 4444129},
            {"Slayer Chest", 4444130},
            {"Dark Chest", 4444131},
            {"Squire Limbs", 4444132},
            {"Knight Limbs", 4444133},
            {"Blood Limbs", 4444134},
            {"Silver Limbs", 4444135},
            {"Ranger Limbs", 4444136},
            {"Sage Limbs", 4444137},
            {"Guardian Limbs", 4444138},
            {"Sky Limbs", 4444139},
            {"Retribution Limbs", 4444140},
            {"Imperial Limbs", 4444141},
            {"Dragon Limbs", 4444142},
            {"Holy Limbs", 4444143},
            {"Royal Limbs", 4444144},
            {"Slayer Limbs", 4444145},
            {"Dark Limbs", 4444146},
            {"Squire Cape", 4444147},
            {"Knight Cape", 4444148},
            {"Blood Cape", 4444149},
            {"Silver Cape", 4444150},
            {"Ranger Cape", 4444151},
            {"Sage Cape", 4444152},
            {"Guardian Cape", 4444153},
            {"Sky Cape", 4444154},
            {"Retribution Cape", 4444155},
            {"Imperial Cape", 4444156},
            {"Dragon Cape", 4444157},
            {"Holy Cape", 4444158},
            {"Royal Cape", 4444159},
            {"Slayer Cape", 4444160},
            {"Dark Cape", 4444161},
        };

        public static IDictionary<string, int> IdTable
        {
            get { return Table; }
        }

        public static IDictionary<int, string> NameTable
        {
            get { return Table.ToDictionary(kp => kp.Value, kp => kp.Key); }
        }
    }
}
