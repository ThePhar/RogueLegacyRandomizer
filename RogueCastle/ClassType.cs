using System.Collections.Generic;
using DS2DEngine;

namespace RogueCastle
{
	internal class ClassType
	{
		public const byte Knight = 0;
		public const byte Wizard = 1;
		public const byte Barbarian = 2;
		public const byte Assassin = 3;
		public const byte Ninja = 4;
		public const byte Banker = 5;
		public const byte SpellSword = 6;
		public const byte Lich = 7;
		public const byte Knight2 = 8;
		public const byte Wizard2 = 9;
		public const byte Barbarian2 = 10;
		public const byte Assassin2 = 11;
		public const byte Ninja2 = 12;
		public const byte Banker2 = 13;
		public const byte SpellSword2 = 14;
		public const byte Lich2 = 15;
		public const byte TotalUniques = 8;
		public const byte Total = 16;
		public const byte Dragon = 16;
		public const byte Traitor = 17;
		public static string ToString(byte classType, bool isFemale)
		{
			switch (classType)
			{
			case 0:
				return "Knight";
			case 1:
				return "Mage";
			case 2:
				return "Barbarian";
			case 3:
				return "Knave";
			case 4:
				return "Shinobi";
			case 5:
				return "Miner";
			case 6:
				return "Spellthief";
			case 7:
				return "Lich";
			case 8:
				return "Paladin";
			case 9:
				return "Archmage";
			case 10:
				if (isFemale)
				{
					return "Barbarian Queen";
				}
				return "Barbarian King";
			case 11:
				return "Assassin";
			case 12:
				return "Hokage";
			case 13:
				if (isFemale)
				{
					return "Spelunkette";
				}
				return "Spelunker";
			case 14:
				return "Spellsword";
			case 15:
				if (isFemale)
				{
					return "Lich Queen";
				}
				return "Lich King";
			case 16:
				return "Dragon";
			case 17:
				return "Traitor";
			default:
				return "";
			}
		}
		public static string Description(byte classType)
		{
			switch (classType)
			{
			case 0:
				return "Your standard hero. Pretty good at everything.";
			case 1:
				return "A powerful spellcaster. Every kill gives you mana.";
			case 2:
				return "A walking tank. This hero can take a beating.";
			case 3:
				return "A risky hero. Low stats but can land devastating critical strikes.";
			case 4:
				return "A fast hero. Deal massive damage, but you cannot crit.";
			case 5:
				return "A hero for hoarders. Very weak, but has a huge bonus to gold.";
			case 6:
				return "A hero for experts. Hit enemies to restore mana.";
			case 7:
				return "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.";
			case 8:
				return "Your standard hero. Pretty good at everything.\nSPECIAL: Guardian's Shield.";
			case 9:
				return "A powerful spellcaster. Every kill gives you mana.\nSPECIAL: Spell Cycle.";
			case 10:
				return "A walking tank. This hero can take a beating.\nSPECIAL: Barbarian Shout.";
			case 11:
				return "A risky hero. Low stats but can land devastating critical strikes.\nSPECIAL: Mist Form.";
			case 12:
				return "A fast hero. Deal massive damage, but you cannot crit.\nSPECIAL: Replacement Technique.";
			case 13:
				return "A hero for hoarders. Very weak, but has a huge bonus to gold.\nSPECIAL: Ordinary Headlamp.";
			case 14:
				return "A hero for experts. Hit enemies to restore mana.\nSPECIAL: Empowered Spell.";
			case 15:
				return "Feed off the dead. Gain permanent life for every kill up to a cap. Extremely intelligent.\nSPECIAL: HP Conversion.";
			case 16:
				return "You are a man-dragon";
			case 17:
				return "Fountain text here";
			default:
				return "";
			}
		}
		public static string ProfileCardDescription(byte classType)
		{
			switch (classType)
			{
			case 0:
				return "100% Base stats.";
			case 1:
				return "Every kill gives you " + 6 + " mana.\nLow Str and HP.  High Int and MP.";
			case 2:
				return "Huge HP.  Low Str and MP.";
			case 3:
				return string.Concat(new object[]
				{
					"+",
					15.000001f,
					"% Crit. Chance, +",
					125f,
					"% Crit. Damage.\nLow HP, MP, and Str."
				});
			case 4:
				return "Huge Str, but you cannot land critical strikes.\n +" + 30.0000019f + "% Move Speed.  Low HP and MP.";
			case 5:
				return "+" + 30.0000019f + "% Gold gain.\nVery weak in all other stats.";
			case 6:
				return 30.0000019f + "% of damage dealt is converted into mana.\nLow Str, HP, and MP.";
			case 7:
				return "Kills are coverted into max HP.\nVery low Str, HP and MP.  High Int.";
			case 8:
				return "SPECIAL: Guardian's Shield.\n100% Base stats.";
			case 9:
				return "SPECIAL: Spell Cycle.\nEvery kill gives you " + 6 + " mana.\nLow Str and HP.  High Int and MP.";
			case 10:
				return "SPECIAL: Barbarian Shout.\nHuge HP.  Low Str and MP.";
			case 11:
				return string.Concat(new object[]
				{
					"SPECIAL: Mist Form\n+",
					15.000001f,
					"% Crit. Chance, +",
					125f,
					"% Crit. Damage.\nLow HP, MP, and Str."
				});
			case 12:
				return "SPECIAL: Replacement Technique.\nHuge Str, but you cannot land critical strikes.\n +" + 30.0000019f + "% Move Speed.  Low HP and MP.";
			case 13:
				return "SPECIAL: Ordinary Headlamp.\n+" + 30.0000019f + "% Gold gain.\nVery weak in all other stats.";
			case 14:
				return "SPECIAL: Empowered Spell.\n" + 30.0000019f + "% of damage dealt is converted into mana.\nLow Str, HP, and MP.";
			case 15:
				return "SPECIAL: HP Conversion.\nKills are coverted into max HP.\nVery low Str, HP and MP.  High Int.";
			case 16:
				return "You are a man-dragon";
			case 17:
				return "Fountain text here";
			default:
				return "";
			}
		}
		public static byte GetRandomClass()
		{
			List<byte> list = new List<byte>();
			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(3);
			if (SkillSystem.GetSkill(SkillType.Ninja_Unlock).ModifierAmount > 0f)
			{
				list.Add(4);
			}
			if (SkillSystem.GetSkill(SkillType.Banker_Unlock).ModifierAmount > 0f)
			{
				list.Add(5);
			}
			if (SkillSystem.GetSkill(SkillType.Spellsword_Unlock).ModifierAmount > 0f)
			{
				list.Add(6);
			}
			if (SkillSystem.GetSkill(SkillType.Lich_Unlock).ModifierAmount > 0f)
			{
				list.Add(7);
			}
			if (SkillSystem.GetSkill(SkillType.SuperSecret).ModifierAmount > 0f)
			{
				list.Add(16);
			}
			if (Game.PlayerStats.ChallengeLastBossBeaten)
			{
				list.Add(17);
			}
			byte b = list[CDGMath.RandomInt(0, list.Count - 1)];
			if (Upgraded(b))
			{
				b += 8;
			}
			return b;
		}
		public static bool Upgraded(byte classType)
		{
			switch (classType)
			{
			case 0:
				return SkillSystem.GetSkill(SkillType.Knight_Up).ModifierAmount > 0f;
			case 1:
				return SkillSystem.GetSkill(SkillType.Mage_Up).ModifierAmount > 0f;
			case 2:
				return SkillSystem.GetSkill(SkillType.Barbarian_Up).ModifierAmount > 0f;
			case 3:
				return SkillSystem.GetSkill(SkillType.Assassin_Up).ModifierAmount > 0f;
			case 4:
				return SkillSystem.GetSkill(SkillType.Ninja_Up).ModifierAmount > 0f;
			case 5:
				return SkillSystem.GetSkill(SkillType.Banker_Up).ModifierAmount > 0f;
			case 6:
				return SkillSystem.GetSkill(SkillType.SpellSword_Up).ModifierAmount > 0f;
			case 7:
				return SkillSystem.GetSkill(SkillType.Lich_Up).ModifierAmount > 0f;
			default:
				return false;
			}
		}
		public static byte[] GetSpellList(byte classType)
		{
			switch (classType)
			{
			case 0:
			case 8:
				return new byte[]
				{
					2,
					1,
					8,
					9,
					10,
					12
				};
			case 1:
			case 9:
				return new byte[]
				{
					2,
					1,
					4,
					8,
					9,
					10,
					11,
					12
				};
			case 2:
			case 10:
				return new byte[]
				{
					2,
					1,
					8,
					9,
					10
				};
			case 3:
			case 11:
				return new byte[]
				{
					2,
					1,
					6,
					8,
					9,
					12
				};
			case 4:
			case 12:
				return new byte[]
				{
					2,
					1,
					6,
					8,
					9,
					10,
					12
				};
			case 5:
			case 13:
				return new byte[]
				{
					2,
					1,
					8,
					9,
					11,
					12
				};
			case 6:
			case 14:
				return new byte[]
				{
					2,
					1,
					8,
					9,
					10,
					11
				};
			case 7:
			case 15:
				return new byte[]
				{
					5,
					11,
					12
				};
			case 16:
				return new byte[]
				{
					13
				};
			case 17:
				return new byte[]
				{
					14
				};
			default:
				return null;
			}
		}
	}
}
