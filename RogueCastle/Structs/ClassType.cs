// 
// RogueLegacyArchipelago - ClassType.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using DS2DEngine;

namespace RogueCastle.Structs
{
    public static class ClassType
    {
        public const byte Knight        = 0;
        public const byte Mage          = 1;
        public const byte Barbarian     = 2;
        public const byte Knave         = 3;
        public const byte Shinobi       = 4;
        public const byte Miner         = 5;
        public const byte Spellthief    = 6;
        public const byte Lich          = 7;
        public const byte Paladin       = 8;
        public const byte Archmage      = 9;
        public const byte BarbarianKing = 10;
        public const byte Assassin      = 11;
        public const byte Hokage        = 12;
        public const byte Spelunker     = 13;
        public const byte Spellsword    = 14;
        public const byte LichKing      = 15;
        public const byte Dragon        = 16;
        public const byte Traitor       = 17;
        public const byte TotalUniques  = 10;
        public const byte Total         = 18;

        /// <summary>
        /// Returns the string representation of a given class's name. Takes into account character gender if specified.
        /// </summary>
        /// <param name="classType">Class Identifier</param>
        /// <param name="isFemale">Is this character female?</param>
        /// <returns></returns>
        public static string ToString(byte classType, bool isFemale = false)
        {
            switch (classType)
            {
                case Knight:
                    return "Knight";

                case Mage:
                    return "Mage";

                case Barbarian:
                    return "Barbarian";

                case Knave:
                    return "Knave";

                case Shinobi:
                    return "Shinobi";

                case Miner:
                    return "Miner";

                case Spellthief:
                    return "Spellthief";

                case Lich:
                    return "Lich";

                case Paladin:
                    return "Paladin";

                case Archmage:
                    return "Archmage";

                case BarbarianKing:
                    return isFemale ? "Barbarian Queen" : "Barbarian King";

                case Assassin:
                    return "Assassin";

                case Hokage:
                    return "Hokage";

                case Spelunker:
                    return isFemale ? "Spelunkette" : "Spelunker";

                case Spellsword:
                    return "Spellsword";

                case LichKing:
                    return isFemale ? "Lich Queen" : "Lich King";

                case Dragon:
                    return "Dragon";

                case Traitor:
                    return "Traitor";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a string describing this character's features.
        /// </summary>
        /// <param name="classType">Class Identifier</param>
        /// <returns></returns>
        public static string Description(byte classType)
        {
            switch (classType)
            {
                case Knight:
                    return "Your standard hero. Pretty good at everything.";

                case Mage:
                    return "A powerful spellcaster. Every kill gives you mana.";

                case Barbarian:
                    return "A walking tank. This hero can take a beating.";

                case Knave:
                    return "A risky hero. Low stats but can land devastating critical strikes.";

                case Shinobi:
                    return "A fast hero. Deal massive damage, but you cannot crit.";

                case Miner:
                    return "A hero for hoarders. Very weak, but has a huge bonus to gold.";

                case Spellthief:
                    return "A hero for experts. Hit enemies to restore mana.";

                case Lich:
                    return "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.";

                case Paladin:
                    return "Your standard hero. Pretty good at everything.\nSPECIAL: Guardian's Shield.";

                case Archmage:
                    return "A powerful spellcaster. Every kill gives you mana.\nSPECIAL: Spell Cycle.";

                case BarbarianKing:
                    return "A walking tank. This hero can take a beating.\nSPECIAL: Barbarian Shout.";

                case Assassin:
                    return "A risky hero. Low stats but can land devastating critical strikes.\nSPECIAL: Mist Form.";

                case Hokage:
                    return "A fast hero. Deal massive damage, but you cannot crit.\nSPECIAL: Replacement Technique.";

                case Spelunker:
                    return "A hero for hoarders. Very weak, but has a huge bonus to gold.\nSPECIAL: Ordinary Headlamp.";

                case Spellsword:
                    return "A hero for experts. Hit enemies to restore mana.\nSPECIAL: Empowered Spell.";

                case LichKing:
                    return "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.\nSPECIAL: HP Conversion.";

                case Dragon:
                    return "You are a man-dragon.";

                case Traitor:
                    return "Fountain text here.";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a string describing more detailed information about this character's features for the Profile Card.
        /// </summary>
        /// <param name="classType">Class Identifier</param>
        /// <returns></returns>
        public static string ProfileCardDescription(byte classType)
        {
            switch (classType)
            {
                case Knight:
                    return "100% Base stats.";

                case Mage:
                    return string.Format("Every kill gives you {0} mana.\nLow Str and HP. High Int and MP.", 6);

                case Barbarian:
                    return "Huge HP. Low Str and MP.";

                case Knave:
                    return string.Format("+{0}% Crit. Chance, +{1}% Crit. Damage.\nLow HP, MP, and Str.", 15.000001f,
                        125f);

                case Shinobi:
                    return string.Format(
                        "Huge Str, but you cannot land critical strikes.\n +{0}% Move Speed. Low HP and MP.",
                        30.0000019f);

                case Miner:
                    return string.Format("+{0}% Gold gain.\nVery weak in all other stats.", 30.0000019f);

                case Spellthief:
                    return string.Format("{0}% of damage dealt is converted into mana.\nLow Str, HP, and MP.",
                        30.0000019f);

                case Lich:
                    return "Kills are converted into max HP.\nVery low Str, HP and MP. High Int.";

                case Paladin:
                    return "SPECIAL: Guardian's Shield.\n100% Base stats.";

                case Archmage:
                    return string.Format(
                        "SPECIAL: Spell Cycle.\nEvery kill gives you {0} mana.\nLow Str and HP. High Int and MP.", 6);

                case BarbarianKing:
                    return "SPECIAL: Barbarian Shout.\nHuge HP. Low Str and MP.";

                case Assassin:
                    return string.Format(
                        "SPECIAL: Mist Form.\n+{0}% Crit. Chance, +{1}% Crit. Damage.\nLow HP, MP, and Str.",
                        15.000001f, 125f);

                case Hokage:
                    return string.Format(
                        "SPECIAL: Replacement Technique.\nHuge Str, but you cannot land critical strikes.\n +{0}% Move Speed. Low HP and MP.",
                        30.0000019f);

                case Spelunker:
                    return string.Format("SPECIAL: Ordinary Headlamp.\n+{0}% Gold gain.\nVery weak in all other stats.",
                        30.0000019f);

                case Spellsword:
                    return string.Format(
                        "SPECIAL: Empowered Spell.\n{0}% of damage dealt is converted into mana.\nLow Str, HP, and MP.",
                        30.0000019f);

                case LichKing:
                    return
                        "SPECIAL: HP Conversion.\nKills are converted into max HP.\nVery low Str, HP and MP. High Int.";

                case Dragon:
                    return "You are a man-dragon";

                case Traitor:
                    return "Fountain text here";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a random class identifier based on what the player has unlocked.
        /// </summary>
        /// <returns></returns>
        public static byte GetRandomClass()
        {
            var list = new List<byte>
            {
                Knight,
                Mage,
                Barbarian,
                Knave
            };

            if (SkillSystem.GetSkill(SkillType.NinjaUnlock).ModifierAmount > 0f)
                list.Add(Shinobi);

            if (SkillSystem.GetSkill(SkillType.BankerUnlock).ModifierAmount > 0f)
                list.Add(Miner);

            if (SkillSystem.GetSkill(SkillType.SpellswordUnlock).ModifierAmount > 0f)
                list.Add(Spellthief);

            if (SkillSystem.GetSkill(SkillType.LichUnlock).ModifierAmount > 0f)
                list.Add(Lich);

            if (SkillSystem.GetSkill(SkillType.SuperSecret).ModifierAmount > 0f)
                list.Add(Dragon);

            if (Game.PlayerStats.ChallengeLastBossBeaten)
                list.Add(Traitor);

            // Upgraded versions are 8 positions away in the table.
            var randomClass = list[CDGMath.RandomInt(0, list.Count - 1)];
            if (Upgraded(randomClass))
                randomClass += 8;

            return randomClass;
        }

        /// <summary>
        /// Returns true if this class type has been upgraded.
        /// </summary>
        /// <param name="classType">Class Identifier</param>
        /// <returns></returns>
        public static bool Upgraded(byte classType)
        {
            switch (classType)
            {
                case Knight:
                    return SkillSystem.GetSkill(SkillType.KnightUp).ModifierAmount > 0f;

                case Mage:
                    return SkillSystem.GetSkill(SkillType.MageUp).ModifierAmount > 0f;

                case Barbarian:
                    return SkillSystem.GetSkill(SkillType.BarbarianUp).ModifierAmount > 0f;

                case Knave:
                    return SkillSystem.GetSkill(SkillType.AssassinUp).ModifierAmount > 0f;

                case Shinobi:
                    return SkillSystem.GetSkill(SkillType.NinjaUp).ModifierAmount > 0f;

                case Miner:
                    return SkillSystem.GetSkill(SkillType.BankerUp).ModifierAmount > 0f;

                case Spellthief:
                    return SkillSystem.GetSkill(SkillType.SpellSwordUp).ModifierAmount > 0f;

                case Lich:
                    return SkillSystem.GetSkill(SkillType.LichUp).ModifierAmount > 0f;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns an array of all possible spells a given class can cast.
        /// </summary>
        /// <param name="classType">Class Identifier</param>
        /// <returns></returns>
        public static byte[] GetSpellList(byte classType)
        {
            switch (classType)
            {
                case Knight:
                case Paladin:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.BladeWall,
                        SpellType.Conflux
                    };

                case Mage:
                case Archmage:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.TimeStop,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.BladeWall,
                        SpellType.FlameBarrier,
                        SpellType.Conflux
                    };

                case Barbarian:
                case BarbarianKing:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.BladeWall
                    };

                case Knave:
                case Assassin:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.QuantumTranslocator,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.Conflux
                    };

                case Shinobi:
                case Hokage:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.QuantumTranslocator,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.BladeWall,
                        SpellType.Conflux
                    };

                case Miner:
                case Spelunker:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.FlameBarrier,
                        SpellType.Conflux
                    };

                case Spellthief:
                case Spellsword:
                    return new[]
                    {
                        SpellType.Axe,
                        SpellType.Dagger,
                        SpellType.Chakram,
                        SpellType.Scythe,
                        SpellType.BladeWall,
                        SpellType.FlameBarrier
                    };

                case Lich:
                case LichKing:
                    return new[]
                    {
                        SpellType.CrowStorm,
                        SpellType.FlameBarrier,
                        SpellType.Conflux
                    };

                case Dragon:
                    return new[]
                    {
                        SpellType.DragonFire
                    };

                case Traitor:
                    return new[]
                    {
                        SpellType.RapidDagger
                    };

                default:
                    return null;
            }
        }
    }
}
