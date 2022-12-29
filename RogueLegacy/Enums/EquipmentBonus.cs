using System;

namespace RogueLegacy.Enums;

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
