// 
// RogueLegacyArchipelago - EquipmentCategoryType.cs
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
    public static class EquipmentCategoryType
    {
        public const int Sword = 0;
        public const int Helm = 1;
        public const int Chest = 2;
        public const int Limbs = 3;
        public const int Cape = 4;

        public const int Total = 5;

        /// <summary>
        /// Returns the string representation of a given equipment type.
        /// </summary>
        /// <param name="equipmentType">Equipment Type Identifier</param>
        /// <returns></returns>
        public static string ToString(int equipmentType)
        {
            switch (equipmentType)
            {
                case Sword:
                    return "Sword";

                case Helm:
                    return "Helm";

                case Chest:
                    return "Chest";

                case Limbs:
                    return "Limbs";

                case Cape:
                    return "Cape";

                default:
                    return "None";
            }
        }

        /// <summary>
        /// Returns an alternative string representation of a given equipment type.
        /// </summary>
        /// <param name="equipmentType">Equipment Type Identifier</param>
        /// <returns></returns>
        public static string ToString2(int equipmentType)
        {
            switch (equipmentType)
            {
                case Sword:
                    return "Sword";

                case Helm:
                    return "Helm";

                case Chest:
                    return "Chestplate";

                case Limbs:
                    return "Bracers";

                case Cape:
                    return "Cape";

                default:
                    return "None";
            }
        }
    }
}
