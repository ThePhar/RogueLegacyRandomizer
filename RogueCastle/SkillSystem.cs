using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class SkillSystem
	{
		private const int Trait_ObservatoryTelescope = 0;
		private const int Trait_ObservatoryBase = 1;
		private const int Trait_RightHighTower = 2;
		private const int Trait_RightHighUpper = 3;
		private const int Trait_RightHighBase = 4;
		private const int Trait_RightExtension = 5;
		private const int Trait_RightBigRoof = 6;
		private const int Trait_RightBigUpper = 7;
		private const int Trait_RightWingRoof = 8;
		private const int Trait_RightBigBase = 9;
		private const int Trait_RightWingBase = 10;
		private const int Trait_RightWingWindow = 11;
		private const int Trait_LeftExtensionBase = 12;
		private const int Trait_LeftFarRoof = 13;
		private const int Trait_LeftFarBase = 14;
		private const int Trait_LeftBigRoof = 15;
		private const int Trait_LeftBigUpper2 = 16;
		private const int Trait_LeftBigWindows = 17;
		private const int Trait_LeftBigUpper = 18;
		private const int Trait_LeftBigBase = 19;
		private const int Trait_LeftWingRoof = 20;
		private const int Trait_LeftWingBase = 21;
		private const int Trait_LeftWingWindow = 22;
		private const int Trait_GroundRoad = 23;
		private const int Trait_MainRoof = 24;
		private const int Trait_MainBase = 25;
		private const int Trait_FrontWindowTop = 26;
		private const int Trait_FrontWindowBottom = 27;
		private const int Trait_LeftTree1 = 28;
		private const int Trait_LeftTree2 = 29;
		private const int Trait_RightTree1 = 30;
		private const int TOTAL = 31;
		private static SkillType StartingTrait = SkillType.Smithy;
		private static SkillObj m_blankTrait;
		private static SkillType[,] m_skillTypeArray;
		private static Vector2[,] m_skillPositionArray;
		private static int[,] m_manorPieceArray;
		private static SkillLinker[,] m_skillLinkerArray;
		private static List<SkillObj> m_skillArray;
		private static bool m_iconsVisible;
		public static List<SkillObj> SkillArray
		{
			get
			{
				return m_skillArray;
			}
		}
		public static bool IconsVisible
		{
			get
			{
				return m_iconsVisible;
			}
		}
		public static void Initialize()
		{
			m_blankTrait = new SkillObj("Icon_Sword_Sprite");
			if (m_skillTypeArray.Length != m_skillPositionArray.Length)
			{
				throw new Exception("Cannot create Trait System. The type array is not the same length as the position array.");
			}
			m_skillArray = new List<SkillObj>();
			for (int i = 2; i < 34; i++)
			{
				SkillObj skillObj = SkillBuilder.BuildSkill((SkillType)i);
				skillObj.Position = GetSkillPosition(skillObj);
				m_skillArray.Add(skillObj);
			}
			GetSkill(StartingTrait).Visible = true;
			m_skillLinkerArray = new SkillLinker[10, 10];
			for (int j = 0; j < 10; j++)
			{
				for (int k = 0; k < 10; k++)
				{
					m_skillLinkerArray[j, k] = SkillBuilder.GetSkillLinker(j, k);
				}
			}
		}
		public static void LevelUpTrait(SkillObj trait, bool giveGoldBonus)
		{
			Game.PlayerStats.CurrentLevel++;
			trait.CurrentLevel++;
			UpdateTraitSprite(trait);
			if (trait.TraitType == SkillType.Gold_Flat_Bonus && giveGoldBonus)
			{
				Game.PlayerStats.Gold += (int)trait.ModifierAmount;
			}
			bool flag = true;
			foreach (SkillObj current in SkillArray)
			{
				if (current.CurrentLevel < 1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				GameUtil.UnlockAchievement("FEAR_OF_DECISIONS");
			}
			if (Game.PlayerStats.CurrentLevel >= 50)
			{
				GameUtil.UnlockAchievement("FEAR_OF_WEALTH");
			}
		}
		public static void ResetAllTraits()
		{
			foreach (SkillObj current in m_skillArray)
			{
				current.CurrentLevel = 0;
				current.Visible = false;
			}
			GetSkill(StartingTrait).Visible = true;
			Game.PlayerStats.CurrentLevel = 0;
		}
		public static void UpdateAllTraitSprites()
		{
			foreach (SkillObj current in m_skillArray)
			{
				UpdateTraitSprite(current);
			}
		}
		public static void UpdateTraitSprite(SkillObj trait)
		{
			string text = trait.IconName;
			if (trait.CurrentLevel > 0 && trait.CurrentLevel < trait.MaxLevel)
			{
				text = text.Replace("Locked", "");
			}
			else if (trait.CurrentLevel > 0 && trait.CurrentLevel >= trait.MaxLevel)
			{
				text = text.Replace("Locked", "Max");
			}
			trait.ChangeSprite(text);
		}
		public static List<SkillObj> GetAllConnectingTraits(SkillObj trait)
		{
			int typeArrayColumns = GetTypeArrayColumns();
			int typeArrayRows = GetTypeArrayRows();
			Vector2 traitTypeIndex = GetTraitTypeIndex(trait);
			SkillObj[] array = new SkillObj[4];
			if (traitTypeIndex.X + 1f < typeArrayColumns)
			{
				array[0] = GetSkill((int)traitTypeIndex.X + 1, (int)traitTypeIndex.Y);
			}
			if (traitTypeIndex.X - 1f >= 0f)
			{
				array[1] = GetSkill((int)traitTypeIndex.X - 1, (int)traitTypeIndex.Y);
			}
			if (traitTypeIndex.Y - 1f >= 0f)
			{
				array[2] = GetSkill((int)traitTypeIndex.X, (int)traitTypeIndex.Y - 1);
			}
			if (traitTypeIndex.Y + 1f < typeArrayRows)
			{
				array[3] = GetSkill((int)traitTypeIndex.X, (int)traitTypeIndex.Y + 1);
			}
			List<SkillObj> list = new List<SkillObj>();
			SkillObj[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				SkillObj skillObj = array2[i];
				if (skillObj != null)
				{
					list.Add(skillObj);
				}
			}
			return list;
		}
		public static SkillObj GetSkill(SkillType skillType)
		{
			foreach (SkillObj current in m_skillArray)
			{
				if (current.TraitType == skillType)
				{
					return current;
				}
			}
			return m_blankTrait;
		}
		public static SkillObj GetSkill(int indexX, int indexY)
		{
			return GetSkill(m_skillTypeArray[indexY, indexX]);
		}
		public static Vector2 GetTraitTypeIndex(SkillObj trait)
		{
			Vector2 result = new Vector2(-1f, -1f);
			SkillType traitType = trait.TraitType;
			for (int i = 0; i < m_skillTypeArray.GetLength(1); i++)
			{
				for (int j = 0; j < m_skillTypeArray.GetLength(0); j++)
				{
					if (m_skillTypeArray[j, i] == traitType)
					{
						result = new Vector2(i, j);
					}
				}
			}
			return result;
		}
		public static Vector2 GetSkillPosition(SkillObj skill)
		{
			Vector2 traitTypeIndex = GetTraitTypeIndex(skill);
			return m_skillPositionArray[(int)traitTypeIndex.Y, (int)traitTypeIndex.X];
		}
		public static int GetTypeArrayRows()
		{
			return m_skillTypeArray.GetLength(0);
		}
		public static int GetTypeArrayColumns()
		{
			return m_skillTypeArray.GetLength(1);
		}
		public static SkillObj[] GetSkillArray()
		{
			return m_skillArray.ToArray();
		}
		public static int GetManorPiece(SkillObj trait)
		{
			Vector2 traitTypeIndex = GetTraitTypeIndex(trait);
			return m_manorPieceArray[(int)traitTypeIndex.Y, (int)traitTypeIndex.X];
		}
		public static SkillLinker GetSkillLink(int x, int y)
		{
			return m_skillLinkerArray[x, y];
		}
		public static void HideAllIcons()
		{
			foreach (SkillObj current in m_skillArray)
			{
				current.Opacity = 0f;
			}
			m_iconsVisible = false;
		}
		public static void ShowAllIcons()
		{
			foreach (SkillObj current in m_skillArray)
			{
				current.Opacity = 1f;
			}
			m_iconsVisible = true;
		}
		static SkillSystem()
		{
			// Note: this type is marked as 'beforefieldinit'.
			SkillType[,] array = new SkillType[10, 10];
			array[0, 8] = SkillType.Randomize_Children;
			array[1, 7] = SkillType.SuperSecret;
			array[1, 8] = SkillType.Mana_Cost_Down;
			array[2, 7] = SkillType.Invuln_Time_Up;
			array[2, 8] = SkillType.SpellSword_Up;
			array[3, 2] = SkillType.Down_Strike_Up;
			array[3, 8] = SkillType.Spellsword_Unlock;
			array[4, 2] = SkillType.Ninja_Up;
			array[4, 3] = SkillType.Armor_Up;
			array[4, 8] = SkillType.Potion_Up;
			array[5, 0] = SkillType.Lich_Up;
			array[5, 1] = SkillType.Lich_Unlock;
			array[5, 2] = SkillType.Prices_Down;
			array[5, 4] = SkillType.Attack_Up;
			array[5, 6] = SkillType.Magic_Damage_Up;
			array[5, 8] = SkillType.Assassin_Up;
			array[6, 0] = SkillType.Death_Dodge;
			array[6, 2] = SkillType.Ninja_Unlock;
			array[6, 3] = SkillType.Barbarian_Up;
			array[6, 4] = SkillType.Architect;
			array[6, 5] = SkillType.Equip_Up;
			array[6, 6] = SkillType.Enchanter;
			array[6, 7] = SkillType.Mage_Up;
			array[6, 8] = SkillType.Banker_Unlock;
			array[6, 9] = SkillType.Banker_Up;
			array[7, 2] = SkillType.Crit_Chance_Up;
			array[7, 5] = SkillType.Knight_Up;
			array[7, 8] = SkillType.Gold_Gain_Up;
			array[8, 2] = SkillType.Crit_Damage_Up;
			array[8, 5] = SkillType.Health_Up;
			array[9, 5] = SkillType.Smithy;
			array[9, 6] = SkillType.Mana_Up;
			m_skillTypeArray = array;
			Vector2[,] array2 = new Vector2[10, 10];
			array2[0, 0] = new Vector2(0f, 0f);
			array2[0, 1] = new Vector2(0f, 0f);
			array2[0, 2] = new Vector2(0f, 0f);
			array2[0, 3] = new Vector2(0f, 0f);
			array2[0, 4] = new Vector2(0f, 0f);
			array2[0, 5] = new Vector2(0f, 0f);
			array2[0, 6] = new Vector2(0f, 0f);
			array2[0, 7] = new Vector2(0f, 0f);
			array2[0, 8] = new Vector2(860f, 125f);
			array2[0, 9] = new Vector2(0f, 0f);
			array2[1, 0] = new Vector2(0f, 0f);
			array2[1, 1] = new Vector2(0f, 0f);
			array2[1, 2] = new Vector2(0f, 0f);
			array2[1, 3] = new Vector2(0f, 0f);
			array2[1, 4] = new Vector2(0f, 0f);
			array2[1, 5] = new Vector2(0f, 0f);
			array2[1, 6] = new Vector2(0f, 0f);
			array2[1, 7] = new Vector2(655f, -100f);
			array2[1, 8] = new Vector2(735f, 95f);
			array2[1, 9] = new Vector2(0f, 0f);
			array2[2, 0] = new Vector2(0f, 0f);
			array2[2, 1] = new Vector2(0f, 0f);
			array2[2, 2] = new Vector2(0f, 0f);
			array2[2, 3] = new Vector2(0f, 0f);
			array2[2, 4] = new Vector2(0f, 0f);
			array2[2, 5] = new Vector2(0f, 0f);
			array2[2, 6] = new Vector2(0f, 0f);
			array2[2, 7] = new Vector2(655f, 50f);
			array2[2, 8] = new Vector2(655f, 125f);
			array2[2, 9] = new Vector2(0f, 0f);
			array2[3, 0] = new Vector2(0f, 0f);
			array2[3, 1] = new Vector2(0f, 0f);
			array2[3, 2] = new Vector2(365f, 150f);
			array2[3, 3] = new Vector2(0f, 0f);
			array2[3, 4] = new Vector2(0f, 0f);
			array2[3, 5] = new Vector2(0f, 0f);
			array2[3, 6] = new Vector2(0f, 0f);
			array2[3, 7] = new Vector2(0f, 0f);
			array2[3, 8] = new Vector2(655f, 200f);
			array2[3, 9] = new Vector2(0f, 0f);
			array2[4, 0] = new Vector2(0f, 0f);
			array2[4, 1] = new Vector2(0f, 0f);
			array2[4, 2] = new Vector2(185f, 250f);
			array2[4, 3] = new Vector2(365f, 250f);
			array2[4, 4] = new Vector2(0f, 0f);
			array2[4, 5] = new Vector2(0f, 0f);
			array2[4, 6] = new Vector2(0f, 0f);
			array2[4, 7] = new Vector2(0f, 0f);
			array2[4, 8] = new Vector2(735f, 200f);
			array2[4, 9] = new Vector2(0f, 0f);
			array2[5, 0] = new Vector2(110f, 360f);
			array2[5, 1] = new Vector2(110f, 460f);
			array2[5, 2] = new Vector2(185f, 360f);
			array2[5, 3] = new Vector2(0f, 0f);
			array2[5, 4] = new Vector2(275f, 555f);
			array2[5, 5] = new Vector2(0f, 0f);
			array2[5, 6] = new Vector2(735f, 555f);
			array2[5, 7] = new Vector2(0f, 0f);
			array2[5, 8] = new Vector2(735f, 280f);
			array2[5, 9] = new Vector2(0f, 0f);
			array2[6, 0] = new Vector2(40f, 410f);
			array2[6, 1] = new Vector2(0f, 0f);
			array2[6, 2] = new Vector2(185f, 555f);
			array2[6, 3] = new Vector2(275f, 360f);
			array2[6, 4] = new Vector2(275f, 460f);
			array2[6, 5] = new Vector2(505f, 315f);
			array2[6, 6] = new Vector2(735f, 460f);
			array2[6, 7] = new Vector2(735f, 360f);
			array2[6, 8] = new Vector2(860f, 460f);
			array2[6, 9] = new Vector2(938f, 415f);
			array2[7, 0] = new Vector2(0f, 0f);
			array2[7, 1] = new Vector2(0f, 0f);
			array2[7, 2] = new Vector2(185f, 680f);
			array2[7, 3] = new Vector2(0f, 0f);
			array2[7, 4] = new Vector2(0f, 0f);
			array2[7, 5] = new Vector2(505f, 410f);
			array2[7, 6] = new Vector2(0f, 0f);
			array2[7, 7] = new Vector2(0f, 0f);
			array2[7, 8] = new Vector2(860f, 680f);
			array2[7, 9] = new Vector2(0f, 0f);
			array2[8, 0] = new Vector2(0f, 0f);
			array2[8, 1] = new Vector2(0f, 0f);
			array2[8, 2] = new Vector2(275f, 680f);
			array2[8, 3] = new Vector2(0f, 0f);
			array2[8, 4] = new Vector2(0f, 0f);
			array2[8, 5] = new Vector2(505f, 490f);
			array2[8, 6] = new Vector2(0f, 0f);
			array2[8, 7] = new Vector2(0f, 0f);
			array2[8, 8] = new Vector2(0f, 0f);
			array2[8, 9] = new Vector2(0f, 0f);
			array2[9, 0] = new Vector2(0f, 0f);
			array2[9, 1] = new Vector2(0f, 0f);
			array2[9, 2] = new Vector2(0f, 0f);
			array2[9, 3] = new Vector2(0f, 0f);
			array2[9, 4] = new Vector2(0f, 0f);
			array2[9, 5] = new Vector2(505f, 590f);
			array2[9, 6] = new Vector2(505f, 680f);
			array2[9, 7] = new Vector2(0f, 0f);
			array2[9, 8] = new Vector2(0f, 0f);
			array2[9, 9] = new Vector2(0f, 0f);
			m_skillPositionArray = array2;
			m_manorPieceArray = new int[,]
			{

				{
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					0,
					-1
				},

				{
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					2,
					1,
					-1
				},

				{
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					-1,
					2,
					3,
					-1
				},

				{
					-1,
					-1,
					15,
					-1,
					-1,
					-1,
					-1,
					-1,
					4,
					-1
				},

				{
					-1,
					-1,
					16,
					17,
					-1,
					-1,
					-1,
					-1,
					6,
					-1
				},

				{
					13,
					14,
					18,
					-1,
					22,
					-1,
					11,
					-1,
					7,
					-1
				},

				{
					12,
					-1,
					19,
					20,
					21,
					25,
					10,
					9,
					8,
					5
				},

				{
					-1,
					-1,
					29,
					-1,
					-1,
					27,
					-1,
					-1,
					31,
					-1
				},

				{
					-1,
					-1,
					30,
					-1,
					-1,
					28,
					-1,
					-1,
					-1,
					-1
				},

				{
					-1,
					-1,
					-1,
					-1,
					-1,
					26,
					24,
					-1,
					-1,
					-1
				}
			};
			m_iconsVisible = true;
		}
	}
}
