using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RogueCastle.Enums
{
    public enum Spell
    {
        None,
        Dagger,
        Axe,
        Bomb,
        TimeStop,
        CrowStorm,
        QuantumTranslocator,
        Displacer,
        Chakram,
        Scythe,
        BladeWall,
        FlameBarrier,
        Conflux,
        DragonFire,
        RapidDagger,
        DragonFireNeo,
        Shout = 20,
        Laser = 100
    }

    public static class SpellExtensions
    {
        public static string Name(this Spell spell)
        {
            return spell switch
            {
                Spell.Dagger              => "Dagger",
                Spell.Axe                 => "Axe",
                Spell.Bomb                => "Bomb",
                Spell.TimeStop            => "Time Stop",
                Spell.CrowStorm           => "Crow Storm",
                Spell.QuantumTranslocator => "Quantum Translocator",
                Spell.Displacer           => "Displacer",
                Spell.Chakram             => "Chakram",
                Spell.Scythe              => "Scythe",
                Spell.BladeWall           => "Blade Wall",
                Spell.FlameBarrier        => "Flame Barrier",
                Spell.Conflux             => "Conflux",
                Spell.DragonFire          => "Dragon Fire",
                Spell.DragonFireNeo       => "Dragon Fire",
                Spell.RapidDagger         => "Rapid Dagger",
                Spell.Laser               => "B.E.A.M.",
                _                         => ""
            };
        }
        public static string Description(this Spell spell)
        {
            return spell switch
            {
                Spell.Dagger              => $"[Input:{(int) Button.PlayerSpell}]  Fires a dagger directly in front of you.",
                Spell.Axe                 => $"[Input:{(int) Button.PlayerSpell}]  Throws a giant axe in an arc.",
                Spell.Bomb                => $"[Input:{(int) Button.PlayerSpell}]  Summons a bomb that explodes after a while.",
                Spell.TimeStop            => $"[Input:{(int) Button.PlayerSpell}]  Toggles freezing all enemies on-screen. ",
                Spell.CrowStorm           => $"[Input:{(int) Button.PlayerSpell}]  Hits all enemies on screen. Costly.",
                Spell.QuantumTranslocator => $"[Input:{(int) Button.PlayerSpell}]  Drops and teleports to your shadow.",
                Spell.Displacer           => $"[Input:{(int) Button.PlayerSpell}]  Sends out a marker which teleports you.",
                Spell.Chakram             => $"[Input:{(int) Button.PlayerSpell}]  Throws a chakram which comes back to you.",
                Spell.Scythe              => $"[Input:{(int) Button.PlayerSpell}]  Send Scythes flying out from above you.",
                Spell.BladeWall           => $"[Input:{(int) Button.PlayerSpell}]  Summon a Grand Blade to defend you.",
                Spell.FlameBarrier        => $"[Input:{(int) Button.PlayerSpell}]  Encircles you with protective fire.",
                Spell.Conflux             => $"[Input:{(int) Button.PlayerSpell}]  Launches orbs that bounce everywhere.",
                Spell.DragonFire          => $"[Input:{(int) Button.PlayerSpell}]  Shoot fireballs at your enemies.",
                Spell.DragonFireNeo       => $"[Input:{(int) Button.PlayerSpell}]  Shoot fireballs at your enemies.",
                Spell.RapidDagger         => $"[Input:{(int) Button.PlayerSpell}]  Fire a barrage of daggers.",
                Spell.Laser               => $"[Input:{(int) Button.PlayerSpell}]  Fire a laser that blasts everyone it touches.",
                _                         => ""
            };
        }
        public static string Icon(this Spell spell)
        {
            return spell switch
            {
                Spell.Dagger              => "DaggerIcon_Sprite",
                Spell.Axe                 => "AxeIcon_Sprite",
                Spell.Bomb                => "TimeBombIcon_Sprite",
                Spell.TimeStop            => "TimeStopIcon_Sprite",
                Spell.CrowStorm           => "NukeIcon_Sprite",
                Spell.QuantumTranslocator => "TranslocatorIcon_Sprite",
                Spell.Displacer           => "DisplacerIcon_Sprite",
                Spell.Chakram             => "BoomerangIcon_Sprite",
                Spell.Scythe              => "DualBladesIcon_Sprite",
                Spell.BladeWall           => "CloseIcon_Sprite",
                Spell.FlameBarrier        => "DamageShieldIcon_Sprite",
                Spell.Conflux             => "BounceIcon_Sprite",
                Spell.DragonFire          => "DragonFireIcon_Sprite",
                Spell.DragonFireNeo       => "DragonFireIcon_Sprite",
                Spell.RapidDagger         => "RapidDaggerIcon_Sprite",
                _                         => "DaggerIcon_Sprite"
            };
        }

        public static Vector3 GetNext3Spells()
        {
            var spellList = Class.Archmage.GetSpellList();
            var list = spellList.ToList();
            var index = list.IndexOf((Spell) Game.PlayerStats.Spell);

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
