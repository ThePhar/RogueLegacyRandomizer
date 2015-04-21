/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class TraitType
	{
		public const byte None = 0;
		public const byte ColorBlind = 1;
		public const byte Gay = 2;
		public const byte NearSighted = 3;
		public const byte FarSighted = 4;
		public const byte Dyslexia = 5;
		public const byte Gigantism = 6;
		public const byte Dwarfism = 7;
		public const byte Baldness = 8;
		public const byte Endomorph = 9;
		public const byte Ectomorph = 10;
		public const byte Alzheimers = 11;
		public const byte Dextrocardia = 12;
		public const byte Tourettes = 13;
		public const byte Hyperactive = 14;
		public const byte OCD = 15;
		public const byte Hypergonadism = 16;
		public const byte Hypogonadism = 17;
		public const byte StereoBlind = 18;
		public const byte IBS = 19;
		public const byte Vertigo = 20;
		public const byte TunnelVision = 21;
		public const byte Ambilevous = 22;
		public const byte PAD = 23;
		public const byte Alektorophobia = 24;
		public const byte Hypochondriac = 25;
		public const byte Dementia = 26;
		public const byte Hypermobility = 27;
		public const byte EideticMemory = 28;
		public const byte Nostalgic = 29;
		public const byte CIP = 30;
		public const byte Savant = 31;
		public const byte TheOne = 32;
		public const byte NoFurniture = 33;
		public const byte PlatformsOpen = 34;
		public const byte Glaucoma = 35;
		public const byte Total = 36;
		public const byte Adopted = 100;
		public static byte Rarity(byte traitType)
		{
			switch (traitType)
			{
			case 1:
				return 2;
			case 2:
				return 1;
			case 3:
				return 2;
			case 4:
				return 3;
			case 5:
				return 3;
			case 6:
				return 1;
			case 7:
				return 1;
			case 8:
				return 1;
			case 9:
				return 1;
			case 10:
				return 2;
			case 11:
				return 3;
			case 12:
				return 2;
			case 13:
				return 1;
			case 14:
				return 1;
			case 15:
				return 1;
			case 16:
				return 1;
			case 17:
				return 3;
			case 18:
				return 1;
			case 19:
				return 2;
			case 20:
				return 3;
			case 21:
				return 2;
			case 22:
				return 2;
			case 23:
				return 2;
			case 24:
				return 2;
			case 25:
				return 3;
			case 26:
				return 3;
			case 27:
				return 2;
			case 28:
				return 2;
			case 29:
				return 3;
			case 30:
				return 3;
			case 31:
				return 2;
			case 32:
				return 3;
			case 33:
				return 2;
			case 34:
				return 2;
			case 35:
				return 2;
			default:
				return 0;
			}
		}
		public static string ToString(byte traitType)
		{
			switch (traitType)
			{
			case 1:
				return "Color Blind";
			case 2:
				return "Gay";
			case 3:
				return "Near-Sighted";
			case 4:
				return "Far-Sighted";
			case 5:
				return "Dyslexia";
			case 6:
				return "Gigantism";
			case 7:
				return "Dwarfism";
			case 8:
				return "Baldness";
			case 9:
				return "Endomorph";
			case 10:
				return "Ectomorph";
			case 11:
				return "Alzheimers";
			case 12:
				return "Dextrocardia";
			case 13:
				return "Coprolalia";
			case 14:
				return "ADHD";
			case 15:
				return "O.C.D.";
			case 16:
				return "Hypergonadism";
			case 17:
				return "Muscle Wk.";
			case 18:
				return "Stereo Blind";
			case 19:
				return "I.B.S.";
			case 20:
				return "Vertigo";
			case 21:
				return "Tunnel Vision";
			case 22:
				return "Ambilevous";
			case 23:
				return "P.A.D.";
			case 24:
				return "Alektorophobia";
			case 25:
				return "Hypochondriac";
			case 26:
				return "Dementia";
			case 27:
				return "Flexible";
			case 28:
				return "Eid. Mem.";
			case 29:
				return "Nostalgic";
			case 30:
				return "C.I.P.";
			case 31:
				return "Savant";
			case 32:
				return "The One";
			case 33:
				return "Clumsy";
			case 34:
				return "EHS";
			case 35:
				return "Glaucoma";
			default:
				return "NULL";
			}
		}
		public static string Description(byte traitType, bool isFemale)
		{
			switch (traitType)
			{
			case 1:
				return "You can't see colors due to monochromacy.";
			case 2:
				if (isFemale)
				{
					return "You like the ladies.";
				}
				return "You are a fan of the man.";
			case 3:
				return "Anything far away is blurry.";
			case 4:
				return "Anything close-up is blurry.";
			case 5:
				return "You hvae trboule raednig tinhgs.";
			case 6:
				return "You were born to be a basketball player.";
			case 7:
				return "You never get to ride rollercoasters.";
			case 8:
				return "The bald and the beautiful.";
			case 9:
				return "You're so heavy, enemies can't knock you back.";
			case 10:
				return "You're skinny, so every hit sends you flying.";
			case 11:
				return "You have trouble remembering where you are.";
			case 12:
				return "Your MP and HP pools are swapped.  Who knew?";
			case 13:
				return "%#&@!";
			case 14:
				return "So energetic! You move faster.";
			case 15:
				return "Must. Clear. House. Break stuff to restore MP.";
			case 16:
				return "You're perma-roided. Attacks knock enemies further.";
			case 17:
				return "You have weak limbs.  Enemies won't get knocked back.";
			case 18:
				return "You can't see in 3D.";
			case 19:
				return "Even the most valiant heroes can suffer from irritable bowels.";
			case 20:
				return "Welcome to Barfs-ville.";
			case 21:
				return "No peripheral vision.";
			case 22:
				return "You've got two left hands, and can't cast spells properly.";
			case 23:
				return "Peripheral Arterial Disease. No foot pulse.";
			case 24:
				return "Chickens freak you out.";
			case 25:
				return "You tend to EXAGGERATE.";
			case 26:
				return "You are insane.";
			case 27:
				return "You are very flexible.";
			case 28:
				return "You remember things with extreme clarity.";
			case 29:
				return "You miss the good old days.";
			case 30:
				return "Congenital Insensitivity to Pain. Know no pain.";
			case 31:
				return "You're very talented. With a few issues.";
			case 32:
				return "There is no spork.";
			case 33:
				return "You break a lot of things.";
			case 34:
				return "You conduct electricity really well.";
			case 35:
				return "It's so dark.";
			default:
				return "NULL";
			}
		}
		public static string ProfileCardDescription(byte traitType)
		{
			switch (traitType)
			{
			case 1:
				return "You can't see colors.";
			case 2:
				if (Game.PlayerStats.IsFemale)
				{
					return "You like the ladies.";
				}
				return "You are a fan of the man.";
			case 3:
				return "Anything far away is blurry.";
			case 4:
				return "Anything close-up is blurry.";
			case 5:
				return "You hvae trboule raednig tinhgs.";
			case 6:
				return "You are huge.";
			case 7:
				return "You are tiny.";
			case 8:
				return "You are bald.";
			case 9:
				return "Can't get knocked back.";
			case 10:
				return "Hits send you flying.";
			case 11:
				return "Where are you?";
			case 12:
				return "MP + HP pools swapped.";
			case 13:
				return "%#&@!";
			case 14:
				return "You move faster.";
			case 15:
				return "Break stuff to restore MP.";
			case 16:
				return "You knock enemies out of the park.";
			case 17:
				return "You can't knock enemies back.";
			case 18:
				return "You can't see in 3D.";
			case 19:
				return "You fart a lot.";
			case 20:
				return "Everything is upside down.";
			case 21:
				return "No early indicators.";
			case 22:
				return "Spells come out your back.";
			case 23:
				return "No foot pulse.";
			case 24:
				return "You are scared of chickens.";
			case 25:
				return "Exaggerate the damage you take.";
			case 26:
				return "You see things that aren't there.";
			case 27:
				return "You turn while fighting.";
			case 28:
				return "Remember enemy placement.";
			case 29:
				return "Everything is old-timey.";
			case 30:
				return "No visible health bar.";
			case 31:
				return "Randomized spells.";
			case 32:
				return "There is no spork.";
			case 33:
				return "You break stuff and have no balance.";
			case 34:
				return "Platforms stay open.";
			case 35:
				return "It's so dark.";
			default:
				return "NULL";
			}
		}
		public static Vector2 CreateRandomTraits()
		{
			Vector2 zero = Vector2.Zero;
			int num = 0;
			int num2 = 2;
			int num3 = 94;
			int num4 = 39;
			int num5 = 1;
			int num6 = CDGMath.RandomInt(0, 100);
			for (int i = 0; i < num2; i++)
			{
				if (num6 < num3)
				{
					num++;
				}
				num3 -= num4;
				if (num3 < num5)
				{
					num3 = num5;
				}
			}
			int[] array = new[]
			{
				48,
				37,
				15
			};
			byte b = 0;
			int num7 = 0;
			int num8 = CDGMath.RandomInt(0, 100);
			List<byte> list = new List<byte>();
			if (num > 0)
			{
				for (int j = 0; j < 3; j++)
				{
					num7 += array[j];
					if (num8 <= num7)
					{
						b = (byte)(j + 1);
						break;
					}
				}
				for (byte b2 = 0; b2 < 36; b2 += 1)
				{
					if (b == Rarity(b2))
					{
						list.Add(b2);
					}
				}
				zero.X = list[CDGMath.RandomInt(0, list.Count - 1)];
			}
			if (num > 1)
			{
				b = 0;
				num7 = 0;
				num8 = CDGMath.RandomInt(0, 100);
				list.Clear();
				for (int k = 0; k < 3; k++)
				{
					num7 += array[k];
					if (num8 <= num7)
					{
						b = (byte)(k + 1);
						break;
					}
				}
				for (byte b3 = 0; b3 < 36; b3 += 1)
				{
					if (b == Rarity(b3))
					{
						list.Add(b3);
					}
				}
				do
				{
					zero.Y = list[CDGMath.RandomInt(0, list.Count - 1)];
				}
				while (zero.Y == zero.X || TraitConflict(zero));
			}
			return zero;
		}
		public static bool TraitConflict(Vector2 traits)
		{
			bool result = false;
			if ((traits.X == 16f && traits.Y == 17f) || (traits.X == 17f && traits.Y == 16f))
			{
				result = true;
			}
			if ((traits.X == 9f && traits.Y == 10f) || (traits.X == 10f && traits.Y == 9f))
			{
				result = true;
			}
			if ((traits.X == 6f && traits.Y == 7f) || (traits.X == 7f && traits.Y == 6f))
			{
				result = true;
			}
			if ((traits.X == 3f && traits.Y == 4f) || (traits.X == 4f && traits.Y == 3f))
			{
				result = true;
			}
			if ((traits.X == 1f && traits.Y == 29f) || (traits.X == 29f && traits.Y == 1f))
			{
				result = true;
			}
			return result;
		}
	}
}
