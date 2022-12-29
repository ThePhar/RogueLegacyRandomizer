using System;

namespace RogueLegacy.Enums;

public enum EquipmentBase
{
    Squire      = 0,
    Silver      = 1,
    Guardian    = 2,
    Imperial    = 3,
    Royal       = 4,
    Knight      = 5,
    Ranger      = 6,
    Sky         = 7,
    Dragon      = 8,
    Slayer      = 9,
    Blood       = 10,
    Sage        = 11,
    Retribution = 12,
    Holy        = 13,
    Dark        = 14
}

public static class EquipmentBaseExtensions
{
    public static string Name(this EquipmentBase equipment)
    {
        return equipment switch
        {
            EquipmentBase.Squire      => "Squire",
            EquipmentBase.Silver      => "Silver",
            EquipmentBase.Guardian    => "Guardian",
            EquipmentBase.Imperial    => "Imperial",
            EquipmentBase.Royal       => "Royal",
            EquipmentBase.Knight      => "Knight",
            EquipmentBase.Ranger      => "Ranger",
            EquipmentBase.Sky         => "Sky",
            EquipmentBase.Dragon      => "Dragon",
            EquipmentBase.Slayer      => "Slayer",
            EquipmentBase.Blood       => "Blood",
            EquipmentBase.Sage        => "Sage",
            EquipmentBase.Retribution => "Retribution",
            EquipmentBase.Holy        => "Holy",
            EquipmentBase.Dark        => "Dark",
            _                         => throw new ArgumentException($"Unsupported EquipmentBase Type in Name(): {nameof(equipment)}")
        };
    }
}
