using System;

namespace RogueCastle.Enums
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
        public static string ToString(this EquipmentCategory category)
        {
            return category switch
            {
                EquipmentCategory.Sword => "Sword",
                EquipmentCategory.Helm  => "Helm",
                EquipmentCategory.Chest => "Chest",
                EquipmentCategory.Limbs => "Limbs",
                EquipmentCategory.Cape  => "Cape",
                _                       => throw new ArgumentException($"Unsupported EquipmentCategory Type in ToString(): {nameof(category)}")
            };
        }
        public static string ToString2(this EquipmentCategory category)
        {
            return category switch
            {
                EquipmentCategory.Sword => "Sword",
                EquipmentCategory.Helm  => "Helm",
                EquipmentCategory.Chest => "Chestplate",
                EquipmentCategory.Limbs => "Bracers",
                EquipmentCategory.Cape  => "Cape",
                _                       => throw new ArgumentException($"Unsupported EquipmentCategory Type in ToString2(): {nameof(category)}")
            };
        }
    }
}