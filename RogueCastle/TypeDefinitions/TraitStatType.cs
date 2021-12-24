// 
// RogueLegacyArchipelago - TraitStatType.cs
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
    public static class TraitStatType
    {
        public const int PlayerMaxHealth = 0;

        /// <summary>
        /// Returns a given trait's stat.
        /// </summary>
        /// <param name="traitType">Skill Type Identifier</param>
        /// <returns></returns>
        public static float GetTraitStat(SkillType traitType)
        {
            switch (traitType)
            {
                case SkillType.HealthUp:
                case SkillType.HealthUpFinal:
                    return Game.ScreenManager.Player.MaxHealth;

                case SkillType.InvulnerabilityTimeUp:
                    return Game.ScreenManager.Player.InvincibilityTime;

                case SkillType.DeathDodge:
                    return SkillSystem.GetSkill(SkillType.DeathDodge).ModifierAmount * 100f;

                case SkillType.AttackUp:
                case SkillType.DamageUpFinal:
                    return Game.ScreenManager.Player.Damage;

                case SkillType.DownStrikeUp:
                    return SkillSystem.GetSkill(SkillType.DownStrikeUp).ModifierAmount * 100f;

                case SkillType.CritChanceUp:
                    return Game.ScreenManager.Player.TotalCritChance;

                case SkillType.CritDamageUp:
                    return Game.ScreenManager.Player.TotalCriticalDamage * 100f;

                case SkillType.MagicDamageUp:
                    return Game.ScreenManager.Player.TotalMagicDamage;

                case SkillType.ManaUp:

                case SkillType.ManaUpFinal:
                    return Game.ScreenManager.Player.MaxMana;

                case SkillType.ManaCostDown:
                    return SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount * 100f;

                case SkillType.EquipUp:

                case SkillType.EquipUpFinal:
                    return Game.ScreenManager.Player.MaxWeight;

                case SkillType.ArmorUp:
                    return Game.ScreenManager.Player.TotalArmor;

                case SkillType.GoldGainUp:
                    return Game.ScreenManager.Player.TotalGoldBonus;

                case SkillType.PricesDown:
                    return SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount * 100f;

                case SkillType.PotionUp:
                    return (0.1f + SkillSystem.GetSkill(SkillType.PotionUp).ModifierAmount) * 100f;

                case SkillType.AttackSpeedUp:
                    return SkillSystem.GetSkill(SkillType.AttackSpeedUp).ModifierAmount * 10f;

                case SkillType.XpGainUp:
                    return Game.ScreenManager.Player.TotalXPBonus;

                case SkillType.ManaRegenUp:
                    return Game.ScreenManager.Player.ManaGain;

                default:
                    return -1f;
            }
        }
    }
}
