// Rogue Legacy Randomizer - SpellType.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Linq;
using Microsoft.Xna.Framework;

namespace RogueLegacy.Enums
{
    public enum SpellType
    {
        None                = 0,
        Dagger              = 1,
        Axe                 = 2,
        Bomb                = 3,
        TimeStop            = 4,
        CrowStorm           = 5,
        QuantumTranslocator = 6,
        Displacer           = 7,
        Chakram             = 8,
        Scythe              = 9,
        BladeWall           = 10,
        FlameBarrier        = 11,
        Conflux             = 12,
        DragonFire          = 13,
        RapidDagger         = 14,
        DragonFireNeo       = 15,
        Shout               = 20,
        Laser               = 100
    }

    public static class SpellExtensions
    {
        public static string Name(this SpellType spell)
        {
            return spell switch
            {
                SpellType.Dagger              => "Dagger",
                SpellType.Axe                 => "Axe",
                SpellType.Bomb                => "Bomb",
                SpellType.TimeStop            => "Time Stop",
                SpellType.CrowStorm           => "Crow Storm",
                SpellType.QuantumTranslocator => "Quantum Translocator",
                SpellType.Displacer           => "Displacer",
                SpellType.Chakram             => "Chakram",
                SpellType.Scythe              => "Scythe",
                SpellType.BladeWall           => "Blade Wall",
                SpellType.FlameBarrier        => "Flame Barrier",
                SpellType.Conflux             => "Conflux",
                SpellType.DragonFire          => "Dragon Fire",
                SpellType.DragonFireNeo       => "Dragon Fire",
                SpellType.RapidDagger         => "Rapid Dagger",
                SpellType.Laser               => "B.E.A.M.",
                _                             => ""
            };
        }

        public static string Description(this SpellType spell)
        {
            return spell switch
            {
                SpellType.Dagger              => $"{InputType.PlayerSpell.Input()}  Fires a dagger directly in front of you.",
                SpellType.Axe                 => $"{InputType.PlayerSpell.Input()}  Throws a giant axe in an arc.",
                SpellType.Bomb                => $"{InputType.PlayerSpell.Input()}  Summons a bomb that explodes after a while.",
                SpellType.TimeStop            => $"{InputType.PlayerSpell.Input()}  Toggles freezing all enemies on-screen. ",
                SpellType.CrowStorm           => $"{InputType.PlayerSpell.Input()}  Hits all enemies on screen. Costly.",
                SpellType.QuantumTranslocator => $"{InputType.PlayerSpell.Input()}  Drops and teleports to your shadow.",
                SpellType.Displacer           => $"{InputType.PlayerSpell.Input()}  Sends out a marker which teleports you.",
                SpellType.Chakram             => $"{InputType.PlayerSpell.Input()}  Throws a chakram which comes back to you.",
                SpellType.Scythe              => $"{InputType.PlayerSpell.Input()}  Send Scythes flying out from above you.",
                SpellType.BladeWall           => $"{InputType.PlayerSpell.Input()}  Summon a Grand Blade to defend you.",
                SpellType.FlameBarrier        => $"{InputType.PlayerSpell.Input()}  Encircles you with protective fire.",
                SpellType.Conflux             => $"{InputType.PlayerSpell.Input()}  Launches orbs that bounce everywhere.",
                SpellType.DragonFire          => $"{InputType.PlayerSpell.Input()}  Shoot fireballs at your enemies.",
                SpellType.DragonFireNeo       => $"{InputType.PlayerSpell.Input()}  Shoot fireballs at your enemies.",
                SpellType.RapidDagger         => $"{InputType.PlayerSpell.Input()}  Fire a barrage of daggers.",
                SpellType.Laser               => $"{InputType.PlayerSpell.Input()}  Fire a laser that blasts everyone it touches.",
                _                             => ""
            };
        }

        public static string Icon(this SpellType spell)
        {
            return spell switch
            {
                SpellType.Dagger              => "DaggerIcon_Sprite",
                SpellType.Axe                 => "AxeIcon_Sprite",
                SpellType.Bomb                => "TimeBombIcon_Sprite",
                SpellType.TimeStop            => "TimeStopIcon_Sprite",
                SpellType.CrowStorm           => "NukeIcon_Sprite",
                SpellType.QuantumTranslocator => "TranslocatorIcon_Sprite",
                SpellType.Displacer           => "DisplacerIcon_Sprite",
                SpellType.Chakram             => "BoomerangIcon_Sprite",
                SpellType.Scythe              => "DualBladesIcon_Sprite",
                SpellType.BladeWall           => "CloseIcon_Sprite",
                SpellType.FlameBarrier        => "DamageShieldIcon_Sprite",
                SpellType.Conflux             => "BounceIcon_Sprite",
                SpellType.DragonFire          => "DragonFireIcon_Sprite",
                SpellType.DragonFireNeo       => "DragonFireIcon_Sprite",
                SpellType.RapidDagger         => "RapidDaggerIcon_Sprite",
                _                             => "DaggerIcon_Sprite"
            };
        }

        // TODO: Should be moved to Archmage definition file.
        public static Vector3 ArchmageSpellList()
        {
            var spellList = ClassType.Archmage.SpellList();
            var list = spellList.ToList();
            var index = list.IndexOf((SpellType) Game.PlayerStats.Spell);

            list.Clear();

            var spells = new byte[3];
            for (var i = 0; i < 3; i++)
            {
                spells[i] = (byte) spellList[index];
                index++;

                // Don't overflow.
                if (index >= spellList.Length)
                {
                    index = 0;
                }
            }

            return new Vector3(spells[0], spells[1], spells[2]);
        }
    }
}
