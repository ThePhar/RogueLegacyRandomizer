// 
// RogueLegacyArchipelago - SpellType.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Linq;
using Microsoft.Xna.Framework;

namespace RogueCastle.Structs
{
    public static class SpellType
    {
        public const byte None = 0;
        public const byte Dagger = 1;
        public const byte Axe = 2;
        public const byte TimeBomb = 3;
        public const byte TimeStop = 4;
        public const byte Nuke = 5;
        public const byte Translocator = 6;
        public const byte Displacer = 7;
        public const byte Boomerang = 8;
        public const byte DualBlades = 9;
        public const byte Close = 10;
        public const byte DamageShield = 11;
        public const byte Bounce = 12;
        public const byte DragonFire = 13;
        public const byte RapidDagger = 14;
        public const byte DragonFireNeo = 15;
        public const byte Shout = 20;
        public const byte Laser = 100;

        public const byte Total = 16;

        /// <summary>
        /// Returns the string representation of a given spell's name.
        /// </summary>
        /// <param name="spellType">Spell Identifier</param>
        /// <returns></returns>
        public static string ToString(byte spellType)
        {
            switch (spellType)
            {
                case Dagger:
                    return "Dagger";

                case Axe:
                    return "Axe";

                case TimeBomb:
                    return "Bomb";

                case TimeStop:
                    return "Time Stop";

                case Nuke:
                    return "Crow Storm";

                case Translocator:
                    return "Quantum Translocator";

                case Displacer:
                    return "Displacer";

                case Boomerang:
                    return "Chakram";

                case DualBlades:
                    return "Scythe";

                case Close:
                    return "Blade Wall";

                case DamageShield:
                    return "Flame Barrier";

                case Bounce:
                    return "Conflux";

                case DragonFire:
                case DragonFireNeo:
                    return "Dragon Fire";

                case RapidDagger:
                    return "Rapid Dagger";

                case Laser:
                    return "B.E.A.M.";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a string describing this spell's features.
        /// </summary>
        /// <param name="spellType">Spell Identifier</param>
        /// <returns></returns>
        public static string Description(byte spellType)
        {
            switch (spellType)
            {
                case Dagger:
                    return string.Format("[Input:{0}]  Fires a dagger directly in front of you.", InputMapType.PlayerSpell1);

                case Axe:
                    return string.Format("[Input:{0}]  Throws a giant axe in an arc.", InputMapType.PlayerSpell1);

                case TimeBomb:
                    return string.Format("[Input:{0}]  Summons a bomb that explodes after a while.", InputMapType.PlayerSpell1);

                case TimeStop:
                    return string.Format("[Input:{0}]  Toggles freezing all enemies on-screen. ", InputMapType.PlayerSpell1);

                case Nuke:
                    return string.Format("[Input:{0}]  Hits all enemies on screen. Costly.", InputMapType.PlayerSpell1);

                case Translocator:
                    return string.Format("[Input:{0}]  Drops and teleports to your shadow.", InputMapType.PlayerSpell1);

                case Displacer:
                    return string.Format("[Input:{0}]  Sends out a marker which teleports you.", InputMapType.PlayerSpell1);

                case Boomerang:
                    return string.Format("[Input:{0}]  Throws a chakram which comes back to you.", InputMapType.PlayerSpell1);

                case DualBlades:
                    return string.Format("[Input:{0}]  Send Scythes flying out from above you.", InputMapType.PlayerSpell1);

                case Close:
                    return string.Format("[Input:{0}]  Summon a Grand Blade to defend you.", InputMapType.PlayerSpell1);

                case DamageShield:
                    return string.Format("[Input:{0}]  Encircles you with protective fire.", InputMapType.PlayerSpell1);

                case Bounce:
                    return string.Format("[Input:{0}]  Launches orbs that bounce everywhere.", InputMapType.PlayerSpell1);

                case DragonFire:
                case DragonFireNeo:
                    return string.Format("[Input:{0}]  Shoot fireballs at your enemies.", InputMapType.PlayerSpell1);

                case RapidDagger:
                    return string.Format("[Input:{0}]  Fire a barrage of daggers.", InputMapType.PlayerSpell1);

                case Laser:
                    return string.Format("[Input:{0}]  Fire a laser that blasts everyone it touches.", InputMapType.PlayerSpell1);

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the resource name for this spell's icon.
        /// </summary>
        /// <param name="spellType">Spell Identifier</param>
        /// <returns></returns>
        public static string Icon(byte spellType)
        {
            switch (spellType)
            {
                case Dagger:
                    return "DaggerIcon_Sprite";

                case Axe:
                    return "AxeIcon_Sprite";

                case TimeBomb:
                    return "TimeBombIcon_Sprite";

                case TimeStop:
                    return "TimeStopIcon_Sprite";

                case Nuke:
                    return "NukeIcon_Sprite";

                case Translocator:
                    return "TranslocatorIcon_Sprite";

                case Displacer:
                    return "DisplacerIcon_Sprite";

                case Boomerang:
                    return "BoomerangIcon_Sprite";

                case DualBlades:
                    return "DualBladesIcon_Sprite";

                case Close:
                    return "CloseIcon_Sprite";

                case DamageShield:
                    return "DamageShieldIcon_Sprite";

                case Bounce:
                    return "BounceIcon_Sprite";

                case DragonFire:
                case DragonFireNeo:
                    return "DragonFireIcon_Sprite";

                case RapidDagger:
                    return "RapidDaggerIcon_Sprite";

                default:
                    return "DaggerIcon_Sprite";
            }
        }

        /// <summary>
        /// Returns a list of the next three spells in the Archmage SpellList array.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetNext3Spells()
        {
            var spellList = ClassType.GetSpellList(ClassType.Archmage);
            var list = spellList.ToList();
            var index = list.IndexOf(Game.PlayerStats.Spell);

            list.Clear();

            var spells = new byte[3];
            for (var i = 0; i < 3; i++)
            {
                spells[i] = spellList[index];
                index++;

                // Don't overflow.
                if (index >= spellList.Length)
                    index = 0;
            }

            return new Vector3(spells[0], spells[1], spells[2]);
        }
    }
}
