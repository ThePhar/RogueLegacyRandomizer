// 
// RogueLegacyArchipelago - LegacyItems.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;

namespace Archipelago.Legacy
{
    public class LegacyItems : Dictionary<string, int>
    {
        public LegacyItems()
        {
            // Skills
            Add("Smithy",                      4444000);
            Add("Architect",                   4444001);
            Add("Enchantress",                 4444002);
            Add("Progressive Knight",          4444003);
            Add("Progressive Mage",            4444004);
            Add("Progressive Barbarian",       4444005);
            Add("Progressive Knave",           4444006);
            Add("Progressive Shinobi",         4444007);
            Add("Progressive Miner",           4444008);
            Add("Progressive Lich",            4444009);
            Add("Progressive Spell Thief",     4444010);
            Add("Dragon",                      4444011);
            Add("Traitor",                     4444012);
            Add("Health Up",                   4444013);
            Add("Mana Up",                     4444014);
            Add("Attack Up",                   4444015);
            Add("Magic Damage Up",             4444016);
            Add("Armor Up",                    4444017);
            Add("Equip Up",                    4444018);
            Add("Crit Chance Up",              4444019);
            Add("Crit Damage Up",              4444020);
            Add("Down Strike Up",              4444021);
            Add("Gold Gain Up",                4444022);
            Add("Potion Up",                   4444023);
            Add("Invulnerability Time Up",     4444024);
            Add("Mana Cost Down",              4444025);
            Add("Death Defy",                  4444026);
            Add("Haggle",                      4444027);
            Add("Randomize Children",          4444028);
            Add("Random Stat Increase",        4444029);
            Add("Random Triple Stat Increase", 4444030);
            Add("Gold Bonus",                  4444031);

            // Runes
            Add("Sprint (Sword)",              4444032);
            Add("Sprint (Helm)",               4444033);
            Add("Sprint (Chest)",              4444034);
            Add("Sprint (Limbs)",              4444035);
            Add("Sprint (Cape)",               4444036);
            Add("Vault (Sword)",               4444037);
            Add("Vault (Helm)",                4444038);
            Add("Vault (Chest)",               4444039);
            Add("Vault (Limbs)",               4444040);
            Add("Vault (Cape)",                4444041);
            Add("Bounty (Sword)",              4444042);
            Add("Bounty (Helm)",               4444043);
            Add("Bounty (Chest)",              4444044);
            Add("Bounty (Limbs)",              4444045);
            Add("Bounty (Cape)",               4444046);
            Add("Siphon (Sword)",              4444047);
            Add("Siphon (Helm)",               4444048);
            Add("Siphon (Chest)",              4444049);
            Add("Siphon (Limbs)",              4444050);
            Add("Siphon (Cape)",               4444051);
            Add("Retaliation (Sword)",         4444052);
            Add("Retaliation (Helm)",          4444053);
            Add("Retaliation (Chest)",         4444054);
            Add("Retaliation (Limbs)",         4444055);
            Add("Retaliation (Cape)",          4444056);
            Add("Grace (Sword)",               4444057);
            Add("Grace (Helm)",                4444058);
            Add("Grace (Chest)",               4444059);
            Add("Grace (Limbs)",               4444060);
            Add("Grace (Cape)",                4444061);
            Add("Balance (Sword)",             4444062);
            Add("Balance (Helm)",              4444063);
            Add("Balance (Chest)",             4444064);
            Add("Balance (Limbs)",             4444065);
            Add("Balance (Cape)",              4444066);
            Add("Curse (Sword)",               4444067);
            Add("Curse (Helm)",                4444068);
            Add("Curse (Chest)",               4444069);
            Add("Curse (Limbs)",               4444070);
            Add("Curse (Cape)",                4444071);
            Add("Vampire (Sword)",             4444072);
            Add("Vampire (Helm)",              4444073);
            Add("Vampire (Chest)",             4444074);
            Add("Vampire (Limbs)",             4444075);
            Add("Vampire (Cape)",              4444076);
            Add("Sky (Sword)",                 4444077);
            Add("Sky (Helm)",                  4444078);
            Add("Sky (Chest)",                 4444079);
            Add("Sky (Limbs)",                 4444080);
            Add("Sky (Cape)",                  4444081);
            Add("Haste (Sword)",               4444082);
            Add("Haste (Helm)",                4444083);
            Add("Haste (Chest)",               4444084);
            Add("Haste (Limbs)",               4444085);
            Add("Haste (Cape)",                4444086);

            // Blueprints
            Add("Squire Sword",                4444087);
            Add("Knight Sword",                4444088);
            Add("Blood Sword",                 4444089);
            Add("Silver Sword",                4444090);
            Add("Ranger Sword",                4444091);
            Add("Sage Sword",                  4444092);
            Add("Guardian Sword",              4444093);
            Add("Sky Sword",                   4444094);
            Add("Retribution Sword",           4444095);
            Add("Imperial Sword",              4444096);
            Add("Dragon Sword",                4444097);
            Add("Holy Sword",                  4444098);
            Add("Royal Sword",                 4444099);
            Add("Slayer Sword",                4444100);
            Add("Dark Sword",                  4444101);
            Add("Squire Helm",                 4444102);
            Add("Knight Helm",                 4444103);
            Add("Blood Helm",                  4444104);
            Add("Silver Helm",                 4444105);
            Add("Ranger Helm",                 4444106);
            Add("Sage Helm",                   4444107);
            Add("Guardian Helm",               4444108);
            Add("Sky Helm",                    4444109);
            Add("Retribution Helm",            4444110);
            Add("Imperial Helm",               4444111);
            Add("Dragon Helm",                 4444112);
            Add("Holy Helm",                   4444113);
            Add("Royal Helm",                  4444114);
            Add("Slayer Helm",                 4444115);
            Add("Dark Helm",                   4444116);
            Add("Squire Chest",                4444117);
            Add("Knight Chest",                4444118);
            Add("Blood Chest",                 4444119);
            Add("Silver Chest",                4444120);
            Add("Ranger Chest",                4444121);
            Add("Sage Chest",                  4444122);
            Add("Guardian Chest",              4444123);
            Add("Sky Chest",                   4444124);
            Add("Retribution Chest",           4444125);
            Add("Imperial Chest",              4444126);
            Add("Dragon Chest",                4444127);
            Add("Holy Chest",                  4444128);
            Add("Royal Chest",                 4444129);
            Add("Slayer Chest",                4444130);
            Add("Dark Chest",                  4444131);
            Add("Squire Limbs",                4444132);
            Add("Knight Limbs",                4444133);
            Add("Blood Limbs",                 4444134);
            Add("Silver Limbs",                4444135);
            Add("Ranger Limbs",                4444136);
            Add("Sage Limbs",                  4444137);
            Add("Guardian Limbs",              4444138);
            Add("Sky Limbs",                   4444139);
            Add("Retribution Limbs",           4444140);
            Add("Imperial Limbs",              4444141);
            Add("Dragon Limbs",                4444142);
            Add("Holy Limbs",                  4444143);
            Add("Royal Limbs",                 4444144);
            Add("Slayer Limbs",                4444145);
            Add("Dark Limbs",                  4444146);
            Add("Squire Cape",                 4444147);
            Add("Knight Cape",                 4444148);
            Add("Blood Cape",                  4444149);
            Add("Silver Cape",                 4444150);
            Add("Ranger Cape",                 4444151);
            Add("Sage Cape",                   4444152);
            Add("Guardian Cape",               4444153);
            Add("Sky Cape",                    4444154);
            Add("Retribution Cape",            4444155);
            Add("Imperial Cape",               4444156);
            Add("Dragon Cape",                 4444157);
            Add("Holy Cape",                   4444158);
            Add("Royal Cape",                  4444159);
            Add("Slayer Cape",                 4444160);
            Add("Dark Cape",                   4444161);
        }
    }
}
