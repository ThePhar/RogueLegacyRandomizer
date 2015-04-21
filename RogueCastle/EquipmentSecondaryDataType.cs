/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

namespace RogueCastle
{
    public class EquipmentSecondaryDataType
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

        public static string ToString(int equipmentSecondaryDataType)
        {
            switch (equipmentSecondaryDataType)
            {
                case 1:
                    return "Critical Chance";
                case 2:
                    return "Critical Damage";
                case 3:
                    return "Gold Bonus";
                case 4:
                    return "Damage Return";
                case 5:
                    return "XP Bonus";
                case 6:
                    return "AirAttack";
                case 7:
                    return "Vampirism";
                case 8:
                    return "Siphon";
                case 9:
                    return "Air Jump";
                case 10:
                    return "Move Speed";
                case 11:
                    return "Air Dash";
                case 12:
                    return "Block";
                case 13:
                    return "Float";
                case 14:
                    return "Can attack projectiles";
                case 15:
                    return "Flight";
                default:
                    return "None";
            }
        }
    }
}