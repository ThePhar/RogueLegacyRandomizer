// Rogue Legacy Randomizer - EquipmentCategory.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;

namespace RogueLegacy.Enums
{
    public enum EquipmentCategory
    {
        Sword,
        Helm,
        Chest,
        Limbs,
        Cape
    }

    public static class EquipmentCategoryExtensions
    {
        public static string Name(this EquipmentCategory category)
        {
            return category switch
            {
                EquipmentCategory.Sword => "Sword",
                EquipmentCategory.Helm  => "Helm",
                EquipmentCategory.Chest => "Chest",
                EquipmentCategory.Limbs => "Limbs",
                EquipmentCategory.Cape  => "Cape",
                _                       => throw new ArgumentException($"Unsupported EquipmentCategory Type in Name(): {nameof(category)}")
            };
        }

        public static string AltName(this EquipmentCategory category)
        {
            return category switch
            {
                EquipmentCategory.Sword => "Sword",
                EquipmentCategory.Helm  => "Helm",
                EquipmentCategory.Chest => "Chestplate",
                EquipmentCategory.Limbs => "Bracers",
                EquipmentCategory.Cape  => "Cape",
                _                       => throw new ArgumentException($"Unsupported EquipmentCategory Type in AltName(): {nameof(category)}")
            };
        }
    }
}
