/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

namespace RogueCastle
{
	public class EquipmentAbilityType
	{
		public const int DoubleJump = 0;
		public const int Dash = 1;
		public const int Vampirism = 2;
		public const int Flight = 3;
		public const int ManaGain = 4;
		public const int DamageReturn = 5;
		public const int GoldGain = 6;
		public const int MovementSpeed = 7;
		public const int RoomLevelUp = 8;
		public const int RoomLevelDown = 9;
		public const int ManaHPGain = 10;
		public const int Total = 11;
		public const int ArchitectFee = 20;
		public const int NewGamePlusGoldBonus = 21;
		public static string ToString(int type)
		{
			switch (type)
			{
			case 0:
				return "Vault";
			case 1:
				return "Sprint";
			case 2:
				return "Vampire";
			case 3:
				return "Sky";
			case 4:
				return "Siphon";
			case 5:
				return "Retaliation";
			case 6:
				return "Bounty";
			case 7:
				return "Haste";
			case 8:
				return "Curse";
			case 9:
				return "Grace";
			case 10:
				return "Balance";
			case 20:
				return "Architect's Fee";
			case 21:
				return "NG+ Bonus";
			}
			return "";
		}
		public static string Description(int type)
		{
			switch (type)
			{
			case 0:
				return "Grants you the power to jump in the air. Multiple runes stack, allowing for multiple jumps.";
			case 1:
				return "Gain the power to dash short distances. Multiple runes stack allowing for multiple dashes.";
			case 2:
				return "Killing enemies will drain them of their health. Multiple runes stack for increased life drain.";
			case 3:
				return "Gain the power of flight. Multiple runes stack for longer flight duration.";
			case 4:
				return "Steals mana from slain enemies. Multiple runes stack for increased mana drain.";
			case 5:
				return "Returns damage taken from enemies. Multiple runes stack increasing the damage return.";
			case 6:
				return "Increase the amount of gold you get from coins. Multiple runes stack for increased gold gain.";
			case 7:
				return "Increase your base move speed. Multiple runes stack making you even faster.";
			case 8:
				return "Harder enemies, but greater rewards. Multiple runes stack making enemies even harder.";
			case 9:
				return "Enemies scale slower, easier but lesser rewards. Multiple runes stack slowing enemy scaling more.";
			case 10:
				return "Slaying enemies grants both HP and MP. Multiple runes stack for increased hp/mp drain.";
			default:
				return "";
			}
		}
		public static string ShortDescription(int type, float amount)
		{
			switch (type)
			{
			case 0:
				if (amount > 1f)
				{
					return "Air jump " + amount + " times";
				}
				return "Air jump " + amount + " time";
			case 1:
				if (amount > 1f)
				{
					return "Dash up to " + amount + " times";
				}
				return "Dash up to " + amount + " time";
			case 2:
				return "Gain back " + amount + " hp for every kill";
			case 3:
				if (amount > 1f)
				{
					return "Fly for " + amount + " seconds";
				}
				return "Fly for " + amount + " second";
			case 4:
				return "Gain back " + amount + " mp for every kill";
			case 5:
				return "Return " + amount + "% damage after getting hit";
			case 6:
				return "Each gold drop is " + amount + "% more valuable";
			case 7:
				return "Move " + amount + "% faster";
			case 8:
				return "Enemies start " + (int)(amount / 4f * 2.75f) + " levels higher";
			case 9:
				if (amount > 1f)
				{
					return "Enemies scale " + amount + " units slower";
				}
				return "Enemies scale " + amount + " unit slower";
			case 10:
				return "Mana HP";
			case 20:
				return "Earn 60% total gold in the castle.";
			case 21:
				return "Bounty increased by " + amount + "%";
			}
			return "";
		}
		public static string Instructions(int type)
		{
			switch (type)
			{
			case 0:
				return "Press [Input:" + 10 + "] while in air.";
			case 1:
				return string.Concat(new object[]
				{
					"[Input:",
					14,
					"] or [Input:",
					15,
					"] to dash."
				});
			case 2:
				return "Kill enemies to regain health.";
			case 3:
				return "Press [Input:" + 10 + "] while in air.";
			case 4:
				return "Kill enemies to regain mana.";
			case 5:
				return "Damage returned to enemies.";
			case 6:
				return "Coins give more gold.";
			case 7:
				return "Move faster.";
			case 8:
				return "Enemies are harder.";
			case 9:
				return "Enemies scale slower.";
			case 10:
				return "Kill enemies to regain health and mana.";
			default:
				return "";
			}
		}
		public static string Icon(int type)
		{
			switch (type)
			{
			case 0:
				return "EnchantressUI_DoubleJumpIcon_Sprite";
			case 1:
				return "EnchantressUI_DashIcon_Sprite";
			case 2:
				return "EnchantressUI_VampirismIcon_Sprite";
			case 3:
				return "EnchantressUI_FlightIcon_Sprite";
			case 4:
				return "EnchantressUI_ManaGainIcon_Sprite";
			case 5:
				return "EnchantressUI_DamageReturnIcon_Sprite";
			case 6:
				return "Icon_Gold_Gain_Up_Sprite";
			case 7:
				return "EnchantressUI_SpeedUpIcon_Sprite";
			case 8:
				return "EnchantressUI_CurseIcon_Sprite";
			case 9:
				return "EnchantressUI_BlessingIcon_Sprite";
			case 10:
				return "EnchantressUI_BalanceIcon_Sprite";
			default:
				return "";
			}
		}
	}
}
