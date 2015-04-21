using System;
namespace RogueCastle
{
	internal class SpecialItemType
	{
		public const byte None = 0;
		public const byte FreeEntrance = 1;
		public const byte LoseCoins = 2;
		public const byte Revive = 3;
		public const byte SpikeImmunity = 4;
		public const byte GoldPerKill = 5;
		public const byte Compass = 6;
		public const byte Total = 7;
		public const byte Glasses = 8;
		public const byte EyeballToken = 9;
		public const byte SkullToken = 10;
		public const byte FireballToken = 11;
		public const byte BlobToken = 12;
		public const byte LastBossToken = 13;
		public static string ToString(byte itemType)
		{
			switch (itemType)
			{
			case 1:
				return "Charon's Obol";
			case 2:
				return "Hedgehog's Curse";
			case 3:
				return "Hyperion's Ring";
			case 4:
				return "Hermes' Boots";
			case 5:
				return "Helios' Blessing";
			case 6:
				return "Calypso's Compass";
			case 8:
				return "Nerdy Glasses";
			case 9:
				return "Khidr's Obol";
			case 10:
				return "Alexander's Obol";
			case 11:
				return "Ponce De Leon's Obol";
			case 12:
				return "Herodotus' Obol";
			case 13:
				return "Traitor's Obol";
			}
			return "";
		}
		public static string SpriteName(byte itemType)
		{
			switch (itemType)
			{
			case 1:
				return "BonusRoomObolIcon_Sprite";
			case 2:
				return "BonusRoomHedgehogIcon_Sprite";
			case 3:
				return "BonusRoomRingIcon_Sprite";
			case 4:
				return "BonusRoomBootsIcon_Sprite";
			case 5:
				return "BonusRoomBlessingIcon_Sprite";
			case 6:
				return "BonusRoomCompassIcon_Sprite";
			case 8:
				return "BonusRoomGlassesIcon_Sprite";
			case 9:
				return "ChallengeIcon_Eyeball_Sprite";
			case 10:
				return "ChallengeIcon_Skull_Sprite";
			case 11:
				return "ChallengeIcon_Fireball_Sprite";
			case 12:
				return "ChallengeIcon_Blob_Sprite";
			case 13:
				return "ChallengeIcon_LastBoss_Sprite";
			}
			return "";
		}
	}
}
