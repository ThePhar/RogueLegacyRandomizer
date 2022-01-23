//
//  Rogue Legacy Randomizer - Class.cs
//  Last Modified 2022-01-23
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using DS2DEngine;

namespace RogueCastle.Enums
{
    public enum Class
    {
        Knight,
        Mage,
        Barbarian,
        Knave,
        Shinobi,
        Miner,
        Spellthief,
        Lich,
        Paladin,
        Archmage,
        BarbarianKing,
        Assassin,
        Hokage,
        Spelunker,
        Spellsword,
        LichKing,
        Dragon,
        Traitor
    }

    public static class ClassExtensions
    {
        public static string ToString(this Class @class, bool isFemale = false)
        {
            return @class switch
            {
                Class.Knight        => "Knight",
                Class.Mage          => "Mage",
                Class.Barbarian     => "Barbarian",
                Class.Knave         => "Knave",
                Class.Shinobi       => "Shinobi",
                Class.Miner         => "Miner",
                Class.Spellthief    => "Spellthief",
                Class.Lich          => "Lich",
                Class.Paladin       => "Paladin",
                Class.Archmage      => "Archmage",
                Class.BarbarianKing => isFemale ? "Barbarian Queen" : "Barbarian King",
                Class.Assassin      => "Assassin",
                Class.Hokage        => "Hokage",
                Class.Spelunker     => isFemale ? "Spelunkette" : "Spelunker",
                Class.Spellsword    => "Spellsword",
                Class.LichKing      => isFemale ? "Lich Queen" : "Lich King",
                Class.Dragon        => "Dragon",
                Class.Traitor       => "Traitor",
                _                   => throw new ArgumentException($"Unsupported Class Type in ToString(): {nameof(@class)}")
            };
        }

        public static string Description(this Class @class)
        {
            return @class switch
            {
                Class.Knight        => "Your standard hero. Pretty good at everything.",
                Class.Mage          => "A powerful spellcaster. Every kill gives you mana.",
                Class.Barbarian     => "A walking tank. This hero can take a beating.",
                Class.Knave         => "A risky hero. Low stats but can land devastating critical strikes.",
                Class.Shinobi       => "A fast hero. Deal massive damage, but you cannot crit.",
                Class.Miner         => "A hero for hoarders. Very weak, but has a huge bonus to gold.",
                Class.Spellthief    => "A hero for experts. Hit enemies to restore mana.",
                Class.Lich          => "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.",
                Class.Paladin       => "Your standard hero. Pretty good at everything.\nSPECIAL: Guardian's Shield.",
                Class.Archmage      => "A powerful spellcaster. Every kill gives you mana.\nSPECIAL: Spell Cycle.",
                Class.BarbarianKing => "A walking tank. This hero can take a beating.\nSPECIAL: Barbarian Shout.",
                Class.Assassin      => "A risky hero. Low stats but can land devastating critical strikes.\nSPECIAL: Mist Form.",
                Class.Hokage        => "A fast hero. Deal massive damage, but you cannot crit.\nSPECIAL: Replacement Technique.",
                Class.Spelunker     => "A hero for hoarders. Very weak, but has a huge bonus to gold.\nSPECIAL: Ordinary Headlamp.",
                Class.Spellsword    => "A hero for experts. Hit enemies to restore mana.\nSPECIAL: Empowered Spell.",
                Class.LichKing      => "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.\nSPECIAL: HP Conversion.",
                Class.Dragon        => "You are a man-dragon.",
                Class.Traitor       => "Fountain text here.",
                _                   => throw new ArgumentException($"Unsupported Class Type in Description(): {nameof(@class)}")
            };
        }

        public static string ProfileCardDescription(this Class @class)
        {
            return @class switch
            {
                Class.Knight        => $"100% Base stats.",
                Class.Mage          => $"Every kill gives you {6f} mana.\nLow Str and HP. High Int and MP.",
                Class.Barbarian     => $"Huge HP. Low Str and MP.",
                Class.Knave         => $"+{15f}% Crit. Chance, +{125f}% Crit. Damage.\nLow HP, MP, and Str.",
                Class.Shinobi       => $"Huge Str, but you cannot land critical strikes.\n +{30f}% Move Speed. Low HP and MP.",
                Class.Miner         => $"+{30f}% Gold gain.\nVery weak in all other stats.",
                Class.Spellthief    => $"{30f}% of damage dealt is converted into mana.\nLow Str, HP, and MP.",
                Class.Lich          => $"Kills are converted into max HP.\nVery low Str, HP and MP. High Int.",
                Class.Paladin       => $"SPECIAL: Guardian's Shield.\n100% Base stats.",
                Class.Archmage      => $"SPECIAL: Spell Cycle.\nEvery kill gives you {6f} mana.\nLow Str and HP. High Int and MP.",
                Class.BarbarianKing => $"SPECIAL: Barbarian Shout.\nHuge HP. Low Str and MP.",
                Class.Assassin      => $"SPECIAL: Mist Form.\n+{15f}% Crit. Chance, +{125f}% Crit. Damage.\nLow HP, MP, and Str.",
                Class.Hokage        => $"SPECIAL: Replacement Technique.\nHuge Str, but you cannot land critical strikes.\n+{30f}% Move Speed. Low HP and MP.",
                Class.Spelunker     => $"SPECIAL: Ordinary Headlamp.\n+{30f}% Gold gain.\nVery weak in all other stats.",
                Class.Spellsword    => $"SPECIAL: Empowered Spell.\n{30f}% of damage dealt is converted into mana.\nLow Str, HP, and MP.",
                Class.LichKing      => $"SPECIAL: HP Conversion.\nKills are converted into max HP.\nVery low Str, HP and MP. High Int.",
                Class.Dragon        => $"You are a man-dragon",
                Class.Traitor       => $"Fountain text here",
                _                   => throw new ArgumentException($"Unsupported Class Type in ProfileCardDescription(): {nameof(@class)}")
            };
        }

        public static bool Upgraded(this Class @class)
        {
            return @class switch
            {
                Class.Knight     => SkillSystem.GetSkill(Skill.KnightUp).ModifierAmount > 0f,
                Class.Mage       => SkillSystem.GetSkill(Skill.MageUp).ModifierAmount > 0f,
                Class.Barbarian  => SkillSystem.GetSkill(Skill.BarbarianUp).ModifierAmount > 0f,
                Class.Knave      => SkillSystem.GetSkill(Skill.AssassinUp).ModifierAmount > 0f,
                Class.Shinobi    => SkillSystem.GetSkill(Skill.NinjaUp).ModifierAmount > 0f,
                Class.Miner      => SkillSystem.GetSkill(Skill.BankerUp).ModifierAmount > 0f,
                Class.Spellthief => SkillSystem.GetSkill(Skill.SpellSwordUp).ModifierAmount > 0f,
                Class.Lich       => SkillSystem.GetSkill(Skill.LichUp).ModifierAmount > 0f,
                _                => false
            };
        }

        // TODO: Move this somewhere else?
        public static Spell[] GetSpellList(this Class @class)
        {
            switch (@class)
            {
                case Class.Knight:
                case Class.Paladin:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.BladeWall,
                        Spell.Conflux
                    };

                case Class.Mage:
                case Class.Archmage:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.TimeStop,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.BladeWall,
                        Spell.FlameBarrier,
                        Spell.Conflux
                    };

                case Class.Barbarian:
                case Class.BarbarianKing:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.BladeWall
                    };

                case Class.Knave:
                case Class.Assassin:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.QuantumTranslocator,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.Conflux
                    };

                case Class.Shinobi:
                case Class.Hokage:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.QuantumTranslocator,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.BladeWall,
                        Spell.Conflux
                    };

                case Class.Miner:
                case Class.Spelunker:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.FlameBarrier,
                        Spell.Conflux
                    };

                case Class.Spellthief:
                case Class.Spellsword:
                    return new[]
                    {
                        Spell.Axe,
                        Spell.Dagger,
                        Spell.Chakram,
                        Spell.Scythe,
                        Spell.BladeWall,
                        Spell.FlameBarrier
                    };

                case Class.Lich:
                case Class.LichKing:
                    return new[]
                    {
                        Spell.CrowStorm,
                        Spell.FlameBarrier,
                        Spell.Conflux
                    };

                case Class.Dragon:
                    return new[]
                    {
                        Spell.DragonFire
                    };

                case Class.Traitor:
                    return new[]
                    {
                        Spell.RapidDagger
                    };

                default:
                    return null;
            }
        }

        public static Class GetRandomClass()
        {
            var list = new List<Class>();

            if (SkillSystem.GetSkill(Skill.KnightUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Knight);
            }

            if (SkillSystem.GetSkill(Skill.MageUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Mage);
            }

            if (SkillSystem.GetSkill(Skill.AssassinUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Knave);
            }

            if (SkillSystem.GetSkill(Skill.BarbarianUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Barbarian);
            }

            if (SkillSystem.GetSkill(Skill.NinjaUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Shinobi);
            }

            if (SkillSystem.GetSkill(Skill.BankerUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Miner);
            }

            if (SkillSystem.GetSkill(Skill.SpellswordUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Spellthief);
            }

            if (SkillSystem.GetSkill(Skill.LichUnlock).ModifierAmount > 0f)
            {
                list.Add(Class.Lich);
            }

            if (SkillSystem.GetSkill(Skill.SuperSecret).ModifierAmount > 0f)
            {
                list.Add(Class.Dragon);
            }

            if (SkillSystem.GetSkill(Skill.Traitorous).ModifierAmount > 0f)
            {
                list.Add(Class.Traitor);
            }

            // Upgraded versions are 8 positions away in the table.
            var randomClass = list[CDGMath.RandomInt(0, list.Count - 1)];
            if (Upgraded(randomClass))
            {
                randomClass += 8;
            }

            return randomClass;
        }
    }
}
