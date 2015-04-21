/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

namespace RogueCastle
{
    public class EquipmentBaseType
    {
        public const int Bronze = 0;
        public const int Silver = 1;
        public const int Gold = 2;
        public const int Imperial = 3;
        public const int Royal = 4;
        public const int Knight = 5;
        public const int Earthen = 6;
        public const int Sky = 7;
        public const int Dragon = 8;
        public const int Eternal = 9;
        public const int Blood = 10;
        public const int Amethyst = 11;
        public const int Spike = 12;
        public const int Holy = 13;
        public const int Dark = 14;
        public const int Total = 15;

        public static string ToString(int equipmentBaseType)
        {
            switch (equipmentBaseType)
            {
                case 0:
                    return "Squire";
                case 1:
                    return "Silver";
                case 2:
                    return "Guardian";
                case 3:
                    return "Imperial";
                case 4:
                    return "Royal";
                case 5:
                    return "Knight";
                case 6:
                    return "Ranger";
                case 7:
                    return "Sky";
                case 8:
                    return "Dragon";
                case 9:
                    return "Slayer";
                case 10:
                    return "Blood";
                case 11:
                    return "Sage";
                case 12:
                    return "Retribution";
                case 13:
                    return "Holy";
                case 14:
                    return "Dark";
                default:
                    return "";
            }
        }
    }
}