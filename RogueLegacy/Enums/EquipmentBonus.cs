// Rogue Legacy Randomizer - EquipmentBonus.cs
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
    public enum EquipmentBonus
    {
        None,
        CritChance,
        CritDamage,
        GoldBonus,
        DamageReturn,
        XPBonus,
        AirAttack,
        Vampirism,
        Siphon,
        AirJump,
        MoveSpeed,
        AirDash,
        Block,
        Float,
        AttackProjectiles,
        Flight
    }

    public static class EquipmentBonusExtensions
    {
        public static string Name(this EquipmentBonus bonus)
        {
            return bonus switch
            {
                EquipmentBonus.CritChance        => "Critical Chance",
                EquipmentBonus.CritDamage        => "Critical Damage",
                EquipmentBonus.GoldBonus         => "Gold Bonus",
                EquipmentBonus.DamageReturn      => "Damage Return",
                EquipmentBonus.XPBonus           => "XP Bonus",
                EquipmentBonus.AirAttack         => "Air Attack",
                EquipmentBonus.Vampirism         => "Vampirism",
                EquipmentBonus.Siphon            => "Siphon",
                EquipmentBonus.AirJump           => "Air Jump",
                EquipmentBonus.MoveSpeed         => "Move Speed",
                EquipmentBonus.AirDash           => "Air Dash",
                EquipmentBonus.Block             => "Block",
                EquipmentBonus.Float             => "Float",
                EquipmentBonus.AttackProjectiles => "Can Attack Projectiles",
                EquipmentBonus.Flight            => "Flight",
                _                                => throw new ArgumentException($"Unsupported EquipmentBonus Type in Name(): {nameof(bonus)}")
            };
        }
    }
}
