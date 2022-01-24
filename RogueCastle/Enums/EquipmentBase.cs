// 
//  Rogue Legacy Randomizer - EquipmentBase.cs
//  Last Modified 2022-01-24
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

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
}
