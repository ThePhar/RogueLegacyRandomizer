// 
// RogueLegacyArchipelago - EquipmentSecondaryDataType.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace RogueCastle.TypeDefinitions
{
    public static class EquipmentSecondaryDataType
    {
        public const int None = 0;
        public const int CritChance = 1;
        public const int CritDamage = 2;
        public const int GoldBonus = 3;
        public const int DamageReturn = 4;
        public const int XpBonus = 5;
        public const int AirAttack = 6;
        public const int Vampirism = 7;
        public const int ManaDrain = 8;
        public const int DoubleJump = 9;
        public const int MoveSpeed = 10;
        public const int AirDash = 11;
        public const int Block = 12;
        public const int Float = 13;
        public const int AttackProjectiles = 14;
        public const int Flight = 15;

        public const int Total = 16;

        /// <summary>
        /// Returns the string representation of a given equipment characteristic.
        /// </summary>
        /// <param name="equipmentSecondaryDataType">Equipment Characteristic Identifier</param>
        /// <returns></returns>
        public static string ToString(int equipmentSecondaryDataType)
        {
            switch (equipmentSecondaryDataType)
            {
                case CritChance:
                    return "Critical Chance";

                case CritDamage:
                    return "Critical Damage";

                case GoldBonus:
                    return "Gold Bonus";

                case DamageReturn:
                    return "Damage Return";

                case XpBonus:
                    return "XP Bonus";

                case AirAttack:
                    return "AirAttack";

                case Vampirism:
                    return "Vampirism";

                case ManaDrain:
                    return "Siphon";

                case DoubleJump:
                    return "Air Jump";

                case MoveSpeed:
                    return "Move Speed";

                case AirDash:
                    return "Air Dash";

                case Block:
                    return "Block";

                case Float:
                    return "Float";

                case AttackProjectiles:
                    return "Can attack projectiles";

                case Flight:
                    return "Flight";

                default:
                    return "None";
            }
        }
    }
}
