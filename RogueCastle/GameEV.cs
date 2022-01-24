// 
//  Rogue Legacy Randomizer - GameEV.cs
//  Last Modified 2022-01-24
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using RogueCastle.Enums;

using static RogueCastle.Enums.InputType;

namespace RogueCastle
{
    internal class GameEV
    {
        public const int SKILL_LEVEL_COST_INCREASE = 10;
        public const float GATEKEEPER_TOLL_COST = 1f;
        public const float MANA_OVER_TIME_TIC_RATE = 0.33f;
        public const float FLIGHT_SPEED_MOD = 0.15f;
        public const int ARMOR_DIVIDER = 200;
        public const float NEWGAMEPLUS_GOLDBOUNTY = 0.5f;
        public const float HAZARD_DAMAGE_PERCENT = 0.2f;
        public const int FART_CHANCE = 91;
        public const float ARCHITECT_FEE = 0.6f;
        public const int RUNE_VAMPIRISM_HEALTH_GAIN = 2;
        public const int RUNE_MANA_GAIN = 2;
        public const float RUNE_FLIGHT = 0.6f;
        public const float RUNE_MOVEMENTSPEED_MOD = 0.2f;
        public const float RUNE_DAMAGERETURN_MOD = 0.5f;
        public const float RUNE_GOLDGAIN_MOD = 0.1f;
        public const int RUNE_MANAHPGAIN = 1;
        public const float ROOM_VITA_CHAMBER_REGEN_AMOUNT = 0.3f;
        public const int RUNE_CURSE_ROOM_LEVEL_GAIN = 8;
        public const float RUNE_GRACE_ROOM_LEVEL_LOSS = 0.75f;
        public const int BASE_ERA = 700;
        public const float SPELLSWORD_SPELLDAMAGE_MOD = 2f;
        public const float SPELLSWORD_MANACOST_MOD = 2f;
        public const float SPELLSWORD_SPELL_SCALE = 1.75f;
        public const float SPELLSWORD_ATTACK_MANA_CONVERSION = 0.3f;
        public const int LICH_HEALTH_GAIN_PER_KILL = 4;
        public const int LICH_HEALTH_GAIN_PER_KILL_LESSER = 4;
        public const float LICH_HEALTH_CONVERSION_PERCENT = 0.5f;
        public const float LICH_MAX_HP_OFF_BASE = 1f;
        public const float LICH_MAX_MP_OFF_BASE = 2f;
        public const float ASSASSIN_ACTIVE_MANA_DRAIN = 7f;
        public const float ASSASSIN_ACTIVE_INITIAL_COST = 5f;
        public const int KNIGHT_BLOCK_DRAIN = 25;
        public const float TANOOKI_ACTIVE_MANA_DRAIN = 6f;
        public const float TANOOKI_ACTIVE_INITIAL_COST = 25f;
        public const float BARBARIAN_SHOUT_INITIAL_COST = 20f;
        public const float BARBARIAN_SHOUT_KNOCKBACK_MOD = 3f;
        public const float DRAGON_MANAGAIN = 4f;
        public const float SPELUNKER_LIGHT_DRAIN = 0f;
        public const int MAGE_MANA_GAIN = 6;
        public const int NINJA_TELEPORT_COST = 5;
        public const int NINJA_TELEPORT_DISTANCE = 350;
        public const float TIMESTOP_ACTIVE_MANA_DRAIN = 8f;
        public const float DAMAGESHIELD_ACTIVE_MANA_DRAIN = 6f;
        public const int ENEMY_ITEMDROP_CHANCE = 2;
        public const float ITEM_MANADROP_AMOUNT = 0.1f;
        public const float ITEM_HEALTHDROP_AMOUNT = 0.1f;
        public const int ITEM_STAT_STRENGTH_AMOUNT = 1;
        public const int ITEM_STAT_MAGIC_AMOUNT = 1;
        public const int ITEM_STAT_ARMOR_AMOUNT = 2;
        public const int ITEM_STAT_MAXHP_AMOUNT = 5;
        public const int ITEM_STAT_MAXMP_AMOUNT = 5;
        public const int ITEM_STAT_WEIGHT_AMOUNT = 5;
        public const float TRAIT_GIGANTISM = 3f;
        public const float TRAIT_DWARFISM = 1.35f;
        public const float TRAIT_HYPERGONADISM = 2f;
        public const float TRAIT_ECTOMORPH = 1.85f;
        public const float TRAIT_ENDOMORPH = 0.5f;
        public const float TRAIT_MOVESPEED_AMOUNT = 0.3f;
        public const float TRAIT_DEMENTIA_SPAWN_CHANCE = 0.2f;

        public static int[] BREAKABLE_ITEMDROP_CHANCE =
        {
            3,
            4,
            36,
            1,
            56
        };

        public static int[] CHEST_TYPE_CHANCE;
        public static int[] BRONZECHEST_ITEMDROP_CHANCE;
        public static int[] SILVERCHEST_ITEMDROP_CHANCE;
        public static int[] GOLDCHEST_ITEMDROP_CHANCE;
        public static int[] STATDROP_CHANCE;

        public static string[] TutorialDownStrikeHint => new[]
        {
            "Down Attack this",
            "Hello, world",
            "When is Hollow Knight AP?",
            "The Game",
            "Look ma, I'm on TV!",
            "This isn't Tourian",
            "I'm a hint text!",
            "Insert funny meme here",
            "Up Attack this"
        };

        public static string[] OtherAPGames => new[]
        {
            "A Link to the Past",
            "Factorio",
            "Final Fantasy",
            "Minecraft",
            "Ocarina of Time",
            "Risk of Rain 2",
            "Secret of Evermore",
            "Slay the Spire",
            "Subnautica",
            "Super Metroid",
            "Timespinner",
            "VVVVVV",
            "Raft",
            "Super Mario 64"
        };

        public static string[] GameHints => new[]
        {
            "The Forest is always to the right side of the Castle.",
            "The Maya is always at the top of the Castle.",
            "The Darkness is always at the bottom of the Castle.",
            "This death was Phar's fault anyway.",
            "If you're having trouble with a boss, try using different runes.",
            $"Vault runes let you to jump in the air with {PlayerJump1.Input()}",
            $"Sprint runes let you dash with {PlayerDashLeft.Input()} or {PlayerDashRight.Input()}",
            "Each class has pros and cons. Make sure to change your play-style accordingly.",
            "Exploring and finding chests is the best way to earn gold.",
            "Harder areas offer greater rewards.",
            $"Sky runes let you fly by pressing {PlayerJump1.Input()} while in the air.",
            "Vampirism and Siphon runes are very powerful when stacked.",
            "Mastering mobility runes makes you awesome.",
            "Make sure to expand your manor. You never know what new unlocks can be revealed.",
            "All classes can be upgraded with unique class abilities.",
            $"Unlocked class abilities can be activated with {PlayerBlock.Input()}",
            "Upgrade your classes early to obtain powerful class abilities.",
            "If you are having trouble with a room, see if you can bypass it instead.",
            "Buying equipment is the fastest way to raise your stats.",
            "Purchasing equipment is cheaper and more flexible than raising your base stats.",
            "You should have picked the other child.",
            "Runes are very powerful. Equip runes at the Enchantress, and don't forget to use them!",
            "Turn off Death Link if you don't want to be murdered by your fellow players.",
            "Learn the nuances of your spell to maximize their potential.",
            "Try to hit enemies near the apex of the axe's arc in order to hit them multiple times.",
            "Avoid picking up the conflux orbs after casting it to maximize damage.",
            "Dodge the chakrams return trip in order to maximize its damage.",
            "Better to use mana to kill enemies than to take unnecessary damage.",
            "Learning enemy 'tells' is integral to surviving the castle.",
            "Spike traps check for a pulse to tell the dead from the living.",
            $"Press {MenuMap.Input()} to open the map.",
            "If you fail a Fairy chest room, the Architect can give you a second chance.",
            "The Architect has a hefty fee for those who use his service.",
            "Bosses drop large amounts of gold on their death.",
            "Bury me with my money.",
            "If you are having trouble, try equipping Grace runes.",
            $"In options you can enable Quick Drop to down-strike and drop with {PlayerDown1.Input()}",
            "The architect is very useful for practicing against bosses.",
            "The third row of equipment usually has major tradeoffs. Be careful.",
            "Certain runes work better with certain bosses.",
            "You should practice fighting bosses using the architect.",
            "Health is a very important stat to raise.",
            "Retribution runes can damage invulnerable objects.",
            "At least this isn't death link... right?",
            "Class abilities are very powerful if used correctly.",
            "Some classes have advantages over certain bosses.",
            $"You should have played {OtherAPGames[CDGMath.RandomInt(0, OtherAPGames.Length - 1)]} instead."
        };

        static GameEV()
        {
            // Note: this type is marked as 'beforefieldinit'.
            var array = new int[3];
            array[0] = 87;
            array[1] = 13;
            CHEST_TYPE_CHANCE = array;
            BRONZECHEST_ITEMDROP_CHANCE = new[]
            {
                85,
                0,
                15
            };
            SILVERCHEST_ITEMDROP_CHANCE = new[]
            {
                22,
                5,
                73
            };
            GOLDCHEST_ITEMDROP_CHANCE = new[]
            {
                0,
                20,
                80
            };
            STATDROP_CHANCE = new[]
            {
                15,
                15,
                15,
                25,
                25,
                5
            };
        }
    }
}
