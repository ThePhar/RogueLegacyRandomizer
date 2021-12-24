// 
// RogueLegacyArchipelago - EquipmentBaseType.cs
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
    public static class EquipmentBaseType
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

        /// <summary>
        /// Returns the string representation of a given equipment base's name.
        /// </summary>
        /// <param name="equipmentBaseType">Equipment Base Identifier</param>
        /// <returns></returns>
        public static string ToString(int equipmentBaseType)
        {
            switch (equipmentBaseType)
            {
                case Bronze:
                    return "Squire";

                case Silver:
                    return "Silver";

                case Gold:
                    return "Guardian";

                case Imperial:
                    return "Imperial";

                case Royal:
                    return "Royal";

                case Knight:
                    return "Knight";

                case Earthen:
                    return "Ranger";

                case Sky:
                    return "Sky";

                case Dragon:
                    return "Dragon";

                case Eternal:
                    return "Slayer";

                case Blood:
                    return "Blood";

                case Amethyst:
                    return "Sage";

                case Spike:
                    return "Retribution";

                case Holy:
                    return "Holy";

                case Dark:
                    return "Dark";

                default:
                    return "";
            }
        }
    }
}
