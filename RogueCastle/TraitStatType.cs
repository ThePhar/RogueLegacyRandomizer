using System;
namespace RogueCastle
{
	internal class TraitStatType
	{
		public const int PlayerMaxHealth = 0;
		public static float GetTraitStat(SkillType traitType)
		{
			switch (traitType)
			{
			case SkillType.Health_Up:
			case SkillType.Health_Up_Final:
				return (float)Game.ScreenManager.Player.MaxHealth;
			case SkillType.Invuln_Time_Up:
				return Game.ScreenManager.Player.InvincibilityTime;
			case SkillType.Death_Dodge:
				return SkillSystem.GetSkill(SkillType.Death_Dodge).ModifierAmount * 100f;
			case SkillType.Attack_Up:
			case SkillType.Damage_Up_Final:
				return (float)Game.ScreenManager.Player.Damage;
			case SkillType.Down_Strike_Up:
				return SkillSystem.GetSkill(SkillType.Down_Strike_Up).ModifierAmount * 100f;
			case SkillType.Crit_Chance_Up:
				return Game.ScreenManager.Player.TotalCritChance;
			case SkillType.Crit_Damage_Up:
				return Game.ScreenManager.Player.TotalCriticalDamage * 100f;
			case SkillType.Magic_Damage_Up:
				return (float)Game.ScreenManager.Player.TotalMagicDamage;
			case SkillType.Mana_Up:
			case SkillType.Mana_Up_Final:
				return Game.ScreenManager.Player.MaxMana;
			case SkillType.Mana_Cost_Down:
				return SkillSystem.GetSkill(SkillType.Mana_Cost_Down).ModifierAmount * 100f;
			case SkillType.Equip_Up:
			case SkillType.Equip_Up_Final:
				return (float)Game.ScreenManager.Player.MaxWeight;
			case SkillType.Armor_Up:
				return Game.ScreenManager.Player.TotalArmor;
			case SkillType.Gold_Gain_Up:
				return Game.ScreenManager.Player.TotalGoldBonus;
			case SkillType.Prices_Down:
				return SkillSystem.GetSkill(SkillType.Prices_Down).ModifierAmount * 100f;
			case SkillType.Potion_Up:
				return (0.1f + SkillSystem.GetSkill(SkillType.Potion_Up).ModifierAmount) * 100f;
			case SkillType.Attack_Speed_Up:
				return SkillSystem.GetSkill(SkillType.Attack_Speed_Up).ModifierAmount * 10f;
			case SkillType.XP_Gain_Up:
				return Game.ScreenManager.Player.TotalXPBonus;
			case SkillType.Mana_Regen_Up:
				return Game.ScreenManager.Player.ManaGain;
			}
			return -1f;
		}
	}
}
