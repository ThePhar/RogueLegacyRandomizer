using System;

namespace RogueCastle.Enums
{
    public enum EquipmentBase
    {
        Squire,
        Silver,
        Guardian,
        Imperial,
        Royal,
        Knight,
        Ranger,
        Sky,
        Dragon,
        Slayer,
        Blood,
        Sage,
        Retribution,
        Holy,
        Dark,
    }

    public static class EquipmentBaseExtensions
    {
        public static string ToString(this EquipmentBase equipment)
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
                _                         => throw new ArgumentException($"Unsupported EquipmentBase Type in ToString(): {nameof(equipment)}")
            };
        }
    }
}