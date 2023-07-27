// RogueLegacyRandomizer - ClassType.cs
// Last Modified 2023-07-27 12:40 AM by
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;

namespace RogueLegacy.Enums;

public enum ClassType
{
    Knight        = 0,
    Mage          = 1,
    Barbarian     = 2,
    Knave         = 3,
    Shinobi       = 4,
    Miner         = 5,
    Spellthief    = 6,
    Lich          = 7,
    Paladin       = 8,
    Archmage      = 9,
    BarbarianKing = 10,
    Assassin      = 11,
    Hokage        = 12,
    Spelunker     = 13,
    Spellsword    = 14,
    LichKing      = 15,
    Dragon        = 16,
    Traitor       = 17
}

public static class ClassExtensions
{
    public static string Name(this ClassType @class, bool isFemale = false)
    {
        return @class switch
        {
            ClassType.Knight        => "Knight",
            ClassType.Mage          => "Mage",
            ClassType.Barbarian     => "Barbarian",
            ClassType.Knave         => "Knave",
            ClassType.Shinobi       => "Shinobi",
            ClassType.Miner         => "Miner",
            ClassType.Spellthief    => "Spellthief",
            ClassType.Lich          => "Lich",
            ClassType.Paladin       => "Paladin",
            ClassType.Archmage      => "Archmage",
            ClassType.BarbarianKing => isFemale ? "Barbarian Queen" : "Barbarian King",
            ClassType.Assassin      => "Assassin",
            ClassType.Hokage        => "Hokage",
            ClassType.Spelunker     => isFemale ? "Spelunkette" : "Spelunker",
            ClassType.Spellsword    => "Spellsword",
            ClassType.LichKing      => isFemale ? "Lich Queen" : "Lich King",
            ClassType.Dragon        => "Dragon",
            ClassType.Traitor       => "Traitor",
            _                       => throw new ArgumentException($"Unsupported Class Type in Name(): {nameof(@class)}")
        };
    }

    public static string Description(this ClassType @class)
    {
        return @class switch
        {
            ClassType.Knight        => "Your standard hero. Pretty good at everything.",
            ClassType.Mage          => "A powerful spellcaster. Every kill gives you mana.",
            ClassType.Barbarian     => "A walking tank. This hero can take a beating.",
            ClassType.Knave         => "A risky hero. Low stats but can land devastating critical strikes.",
            ClassType.Shinobi       => "A fast hero. Deal massive damage, but you cannot crit.",
            ClassType.Miner         => "A hero for hoarders. Very weak, but has a huge bonus to gold.",
            ClassType.Spellthief    => "A hero for experts. Hit enemies to restore mana.",
            ClassType.Lich          => "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.",
            ClassType.Paladin       => "Your standard hero. Pretty good at everything.\nSPECIAL: Guardian's Shield.",
            ClassType.Archmage      => "A powerful spellcaster. Every kill gives you mana.\nSPECIAL: Spell Cycle.",
            ClassType.BarbarianKing => "A walking tank. This hero can take a beating.\nSPECIAL: Barbarian Shout.",
            ClassType.Assassin      => "A risky hero. Low stats but can land devastating critical strikes.\nSPECIAL: Mist Form.",
            ClassType.Hokage        => "A fast hero. Deal massive damage, but you cannot crit.\nSPECIAL: Replacement Technique.",
            ClassType.Spelunker     => "A hero for hoarders. Very weak, but has a huge bonus to gold.\nSPECIAL: Ordinary Headlamp.",
            ClassType.Spellsword    => "A hero for experts. Hit enemies to restore mana.\nSPECIAL: Empowered Spell.",
            ClassType.LichKing      => "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.\nSPECIAL: HP Conversion.",
            ClassType.Dragon        => "You are a man-dragon.",
            ClassType.Traitor       => "Fountain text here.",
            _                       => throw new ArgumentException($"Unsupported Class Type in Description(): {nameof(@class)}")
        };
    }

    public static string ProfileCardDescription(this ClassType @class)
    {
        return @class switch
        {
            ClassType.Knight        => $"100% Base stats.",
            ClassType.Mage          => $"Every kill gives you {6f} mana.\nLow Str and HP. High Int and MP.",
            ClassType.Barbarian     => $"Huge HP. Low Str and MP.",
            ClassType.Knave         => $"+{15f}% Crit. Chance, +{125f}% Crit. Damage.\nLow HP, MP, and Str.",
            ClassType.Shinobi       => $"Huge Str, but you cannot land critical strikes.\n +{30f}% Move Speed. Low HP and MP.",
            ClassType.Miner         => $"+{30f}% Gold gain.\nVery weak in all other stats.",
            ClassType.Spellthief    => $"{30f}% of damage dealt is converted into mana.\nLow Str, HP, and MP.",
            ClassType.Lich          => $"Kills are converted into max HP.\nVery low Str, HP and MP. High Int.",
            ClassType.Paladin       => $"SPECIAL: Guardian's Shield.\n100% Base stats.",
            ClassType.Archmage      => $"SPECIAL: Spell Cycle.\nEvery kill gives you {6f} mana.\nLow Str and HP. High Int and MP.",
            ClassType.BarbarianKing => $"SPECIAL: Barbarian Shout.\nHuge HP. Low Str and MP.",
            ClassType.Assassin      => $"SPECIAL: Mist Form.\n+{15f}% Crit. Chance, +{125f}% Crit. Damage.\nLow HP, MP, and Str.",
            ClassType.Hokage        => $"SPECIAL: Replacement Technique.\nHuge Str, but you cannot land critical strikes.\n+{30f}% Move Speed. Low HP and MP.",
            ClassType.Spelunker     => $"SPECIAL: Ordinary Headlamp.\n+{30f}% Gold gain.\nVery weak in all other stats.",
            ClassType.Spellsword    => $"SPECIAL: Empowered Spell.\n{30f}% of damage dealt is converted into mana.\nLow Str, HP, and MP.",
            ClassType.LichKing      => $"SPECIAL: HP Conversion.\nKills are converted into max HP.\nVery low Str, HP and MP. High Int.",
            ClassType.Dragon        => $"You are a man-dragon",
            ClassType.Traitor       => $"Fountain text here",
            _                       => throw new ArgumentException($"Unsupported Class Type in ProfileCardDescription(): {nameof(@class)}")
        };
    }

    public static bool Upgraded(this ClassType @class)
    {
        return @class switch
        {
            ClassType.Knight     => SkillSystem.GetSkill(SkillType.KnightUp).ModifierAmount > 0f,
            ClassType.Mage       => SkillSystem.GetSkill(SkillType.MageUp).ModifierAmount > 0f,
            ClassType.Barbarian  => SkillSystem.GetSkill(SkillType.BarbarianUp).ModifierAmount > 0f,
            ClassType.Knave      => SkillSystem.GetSkill(SkillType.AssassinUp).ModifierAmount > 0f,
            ClassType.Shinobi    => SkillSystem.GetSkill(SkillType.NinjaUp).ModifierAmount > 0f,
            ClassType.Miner      => SkillSystem.GetSkill(SkillType.BankerUp).ModifierAmount > 0f,
            ClassType.Spellthief => SkillSystem.GetSkill(SkillType.SpellSwordUp).ModifierAmount > 0f,
            ClassType.Lich       => SkillSystem.GetSkill(SkillType.LichUp).ModifierAmount > 0f,
            _                    => false
        };
    }

    public static ClassType Upgrade(this ClassType @class)
    {
        return @class switch
        {
            ClassType.Knight     => ClassType.Paladin,
            ClassType.Mage       => ClassType.Archmage,
            ClassType.Barbarian  => ClassType.BarbarianKing,
            ClassType.Knave      => ClassType.Assassin,
            ClassType.Shinobi    => ClassType.Hokage,
            ClassType.Miner      => ClassType.Spelunker,
            ClassType.Spellthief => ClassType.Spellsword,
            ClassType.Lich       => ClassType.LichKing,
            _                    => @class
        };
    }

    public static SpellType[] SpellList(this ClassType @class)
    {
        switch (@class)
        {
            case ClassType.Knight:
            case ClassType.Paladin:
                return new[]
                {
                    SpellType.Axe,
                    SpellType.Dagger,
                    SpellType.Chakram,
                    SpellType.Scythe,
                    SpellType.BladeWall,
                    SpellType.Conflux
                };

            case ClassType.Mage:
            case ClassType.Archmage:
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

            case ClassType.Barbarian:
            case ClassType.BarbarianKing:
                return new[]
                {
                    SpellType.Axe,
                    SpellType.Dagger,
                    SpellType.Chakram,
                    SpellType.Scythe,
                    SpellType.BladeWall
                };

            case ClassType.Knave:
            case ClassType.Assassin:
                return new[]
                {
                    SpellType.Axe,
                    SpellType.Dagger,
                    SpellType.QuantumTranslocator,
                    SpellType.Chakram,
                    SpellType.Scythe,
                    SpellType.Conflux
                };

            case ClassType.Shinobi:
            case ClassType.Hokage:
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

            case ClassType.Miner:
            case ClassType.Spelunker:
                return new[]
                {
                    SpellType.Axe,
                    SpellType.Dagger,
                    SpellType.Chakram,
                    SpellType.Scythe,
                    SpellType.FlameBarrier,
                    SpellType.Conflux
                };

            case ClassType.Spellthief:
            case ClassType.Spellsword:
                return new[]
                {
                    SpellType.Axe,
                    SpellType.Dagger,
                    SpellType.Chakram,
                    SpellType.Scythe,
                    SpellType.BladeWall,
                    SpellType.FlameBarrier
                };

            case ClassType.Lich:
            case ClassType.LichKing:
                return new[]
                {
                    SpellType.CrowStorm,
                    SpellType.FlameBarrier,
                    SpellType.Conflux
                };

            case ClassType.Dragon:
                return new[]
                {
                    SpellType.DragonFire
                };

            case ClassType.Traitor:
                return new[]
                {
                    SpellType.RapidDagger
                };

            default:
                return null;
        }
    }

    public static ClassType RandomClass()
    {
        var classes = new List<ClassType>();

        if (SkillSystem.GetSkill(SkillType.KnightUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Knight);
        }

        if (SkillSystem.GetSkill(SkillType.MageUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Mage);
        }

        if (SkillSystem.GetSkill(SkillType.AssassinUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Knave);
        }

        if (SkillSystem.GetSkill(SkillType.BarbarianUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Barbarian);
        }

        if (SkillSystem.GetSkill(SkillType.NinjaUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Shinobi);
        }

        if (SkillSystem.GetSkill(SkillType.BankerUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Miner);
        }

        if (SkillSystem.GetSkill(SkillType.SpellswordUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Spellthief);
        }

        if (SkillSystem.GetSkill(SkillType.LichUnlock).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Lich);
        }

        if (SkillSystem.GetSkill(SkillType.SuperSecret).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Dragon);
        }

        if (SkillSystem.GetSkill(SkillType.Traitorous).ModifierAmount > 0f)
        {
            classes.Add(ClassType.Traitor);
        }

        var @class = classes[CDGMath.RandomInt(0, classes.Count - 1)];
        if (Upgraded(@class))
        {
            @class = Upgrade(@class);
        }

        return @class;
    }
}