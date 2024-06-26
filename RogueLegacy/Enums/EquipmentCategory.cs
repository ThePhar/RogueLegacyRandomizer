using System;

namespace RogueLegacy.Enums;

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
