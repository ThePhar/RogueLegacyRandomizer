//
// RogueLegacyArchipelago - TraitType.cs
// Last Modified 2021-12-24
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
//
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle.Structs
{
    public static class TraitType
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
        public const byte Ocd = 15;
        public const byte Hypergonadism = 16;
        public const byte Hypogonadism = 17;
        public const byte StereoBlind = 18;
        public const byte Ibs = 19;
        public const byte Vertigo = 20;
        public const byte TunnelVision = 21;
        public const byte Ambilevous = 22;
        public const byte Pad = 23;
        public const byte Alektorophobia = 24;
        public const byte Hypochondriac = 25;
        public const byte Dementia = 26;
        public const byte Hypermobility = 27;
        public const byte EideticMemory = 28;
        public const byte Nostalgic = 29;
        public const byte Cip = 30;
        public const byte Savant = 31;
        public const byte TheOne = 32;
        public const byte NoFurniture = 33;
        public const byte PlatformsOpen = 34;
        public const byte Glaucoma = 35;
        public const byte Adopted = 100;

        public const byte Total = 36;

        public static class TraitRarity
        {
            public const byte NoRarity = 0;
            public const byte Common = 1;
            public const byte Uncommon = 2;
            public const byte Rare = 3;
        }

        /// <summary>
        /// Returns a rarity identifier for a given trait.
        /// </summary>
        /// <param name="traitType">Trait Identifier</param>
        /// <returns></returns>
        public static byte Rarity(byte traitType)
        {
            switch (traitType)
            {
                case ColorBlind:
                    return TraitRarity.Uncommon;

                case Gay:
                    return TraitRarity.Common;

                case NearSighted:
                    return TraitRarity.Uncommon;

                case FarSighted:
                    return TraitRarity.Rare;

                case Dyslexia:
                    return TraitRarity.Rare;

                case Gigantism:
                    return TraitRarity.Common;

                case Dwarfism:
                    return TraitRarity.Common;

                case Baldness:
                    return TraitRarity.Common;

                case Endomorph:
                    return TraitRarity.Common;

                case Ectomorph:
                    return TraitRarity.Uncommon;

                case Alzheimers:
                    return TraitRarity.Rare;

                case Dextrocardia:
                    return TraitRarity.Uncommon;

                case Tourettes:
                    return TraitRarity.Common;

                case Hyperactive:
                    return TraitRarity.Common;

                case Ocd:
                    return TraitRarity.Common;

                case Hypergonadism:
                    return TraitRarity.Common;

                case Hypogonadism:
                    return TraitRarity.Rare;

                case StereoBlind:
                    return TraitRarity.Common;

                case Ibs:
                    return TraitRarity.Uncommon;

                case Vertigo:
                    return TraitRarity.Rare;

                case TunnelVision:
                    return TraitRarity.Uncommon;

                case Ambilevous:
                    return TraitRarity.Uncommon;

                case Pad:
                    return TraitRarity.Uncommon;

                case Alektorophobia:
                    return TraitRarity.Uncommon;

                case Hypochondriac:
                    return TraitRarity.Rare;

                case Dementia:
                    return TraitRarity.Rare;

                case Hypermobility:
                    return TraitRarity.Uncommon;

                case EideticMemory:
                    return TraitRarity.Uncommon;

                case Nostalgic:
                    return TraitRarity.Rare;

                case Cip:
                    return TraitRarity.Rare;

                case Savant:
                    return TraitRarity.Uncommon;

                case TheOne:
                    return TraitRarity.Rare;

                case NoFurniture:
                    return TraitRarity.Uncommon;

                case PlatformsOpen:
                    return TraitRarity.Uncommon;

                case Glaucoma:
                    return TraitRarity.Uncommon;

                default:
                    return TraitRarity.NoRarity;
            }
        }

        /// <summary>
        /// Returns the string representation of a given trait's name.
        /// </summary>
        /// <param name="traitType">Trait Identifier</param>
        /// <returns></returns>
        public static string ToString(byte traitType)
        {
            switch (traitType)
            {
                case ColorBlind:
                    return "Color Blind";

                case Gay:
                    return "Gay";

                case NearSighted:
                    return "Near-Sighted";

                case FarSighted:
                    return "Far-Sighted";

                case Dyslexia:
                    return "Dyslexia";

                case Gigantism:
                    return "Gigantism";

                case Dwarfism:
                    return "Dwarfism";

                case Baldness:
                    return "Baldness";

                case Endomorph:
                    return "Endomorph";

                case Ectomorph:
                    return "Ectomorph";

                case Alzheimers:
                    return "Alzheimers";

                case Dextrocardia:
                    return "Dextrocardia";

                case Tourettes:
                    return "Coprolalia";

                case Hyperactive:
                    return "ADHD";

                case Ocd:
                    return "O.C.D.";

                case Hypergonadism:
                    return "Hypergonadism";

                case Hypogonadism:
                    return "Muscle Wk.";

                case StereoBlind:
                    return "Stereo Blind";

                case Ibs:
                    return "I.B.S.";

                case Vertigo:
                    return "Vertigo";

                case TunnelVision:
                    return "Tunnel Vision";

                case Ambilevous:
                    return "Ambilevous";

                case Pad:
                    return "P.A.D.";

                case Alektorophobia:
                    return "Alektorophobia";

                case Hypochondriac:
                    return "Hypochondriac";

                case Dementia:
                    return "Dementia";

                case Hypermobility:
                    return "Flexible";

                case EideticMemory:
                    return "Eid. Mem.";

                case Nostalgic:
                    return "Nostalgic";

                case Cip:
                    return "C.I.P.";

                case Savant:
                    return "Savant";

                case TheOne:
                    return "The One";

                case NoFurniture:
                    return "Clumsy";

                case PlatformsOpen:
                    return "EHS";

                case Glaucoma:
                    return "Glaucoma";

                default:
                    return "NULL";
            }
        }

        /// <summary>
        /// Returns the description for a given trait.
        /// </summary>
        /// <param name="traitType">Trait Identifier</param>
        /// <param name="isFemale">Is player female?</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static string Description(byte traitType, bool isFemale = false)
        {
            switch (traitType)
            {
                case ColorBlind:
                    return "You can't see colors due to monochromacy.";

                case Gay:
                    return isFemale
                        ? "You like the ladies."
                        : "You are a fan of the man.";

                case NearSighted:
                    return "Anything far away is blurry.";

                case FarSighted:
                    return "Anything close-up is blurry.";

                case Dyslexia:
                    return "You hvae trboule raednig tinhgs.";

                case Gigantism:
                    return "You were born to be a basketball player.";

                case Dwarfism:
                    return "You never get to ride roller-coasters.";

                case Baldness:
                    return "The bald and the beautiful.";

                case Endomorph:
                    return "You're so heavy, enemies can't knock you back.";

                case Ectomorph:
                    return "You're skinny, so every hit sends you flying.";

                case Alzheimers:
                    return "You have trouble remembering where you are.";

                case Dextrocardia:
                    return "Your MP and HP pools are swapped.  Who knew?";

                case Tourettes:
                    return "%#&@!";

                case Hyperactive:
                    return "So energetic! You move faster.";

                case Ocd:
                    return "Must. Clear. House. Break stuff to restore MP.";

                case Hypergonadism:
                    return "You're perma-roided. Attacks knock enemies further.";

                case Hypogonadism:
                    return "You have weak limbs.  Enemies won't get knocked back.";

                case StereoBlind:
                    return "You can't see in 3D.";

                case Ibs:
                    return "Even the most valiant heroes can suffer from irritable bowels.";

                case Vertigo:
                    return "Welcome to Barfs-ville.";

                case TunnelVision:
                    return "No peripheral vision.";

                case Ambilevous:
                    return "You've got two left hands, and can't cast spells properly.";

                case Pad:
                    return "Peripheral Arterial Disease. No foot pulse.";

                case Alektorophobia:
                    return "Chickens freak you out.";

                case Hypochondriac:
                    return "You tend to EXAGGERATE.";

                case Dementia:
                    return "You are insane.";

                case Hypermobility:
                    return "You are very flexible.";

                case EideticMemory:
                    return "You remember things with extreme clarity.";

                case Nostalgic:
                    return "You miss the good old days.";

                case Cip:
                    return "Congenital Insensitivity to Pain. Know no pain.";

                case Savant:
                    return "You're very talented. With a few issues.";

                case TheOne:
                    return "There is no spork.";

                case NoFurniture:
                    return "You break a lot of things.";

                case PlatformsOpen:
                    return "You conduct electricity really well.";

                case Glaucoma:
                    return "It's so dark.";

                default:
                    return "NULL";
            }
        }

        /// <summary>
        /// Returns a shortened description for the Profile card.
        /// </summary>
        /// <param name="traitType">Trait Identifier</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static string ProfileCardDescription(byte traitType)
        {
            switch (traitType)
            {
                case ColorBlind:
                    return "You can't see colors.";

                case Gay:
                    return Game.PlayerStats.IsFemale
                        ? "You like the ladies."
                        : "You are a fan of the man.";

                case NearSighted:
                    return "Anything far away is blurry.";

                case FarSighted:
                    return "Anything close-up is blurry.";

                case Dyslexia:
                    return "You hvae trboule raednig tinhgs.";

                case Gigantism:
                    return "You are huge.";

                case Dwarfism:
                    return "You are tiny.";

                case Baldness:
                    return "You are bald.";

                case Endomorph:
                    return "Can't get knocked back.";

                case Ectomorph:
                    return "Hits send you flying.";

                case Alzheimers:
                    return "Where are you?";

                case Dextrocardia:
                    return "MP + HP pools swapped.";

                case Tourettes:
                    return "%#&@!";

                case Hyperactive:
                    return "You move faster.";

                case Ocd:
                    return "Break stuff to restore MP.";

                case Hypergonadism:
                    return "You knock enemies out of the park.";

                case Hypogonadism:
                    return "You can't knock enemies back.";

                case StereoBlind:
                    return "You can't see in 3D.";

                case Ibs:
                    return "You fart a lot.";

                case Vertigo:
                    return "Everything is upside down.";

                case TunnelVision:
                    return "No early indicators.";

                case Ambilevous:
                    return "Spells come out your back.";

                case Pad:
                    return "No foot pulse.";

                case Alektorophobia:
                    return "You are scared of chickens.";

                case Hypochondriac:
                    return "Exaggerate the damage you take.";

                case Dementia:
                    return "You see things that aren't there.";

                case Hypermobility:
                    return "You turn while fighting.";

                case EideticMemory:
                    return "Remember enemy placement.";

                case Nostalgic:
                    return "Everything is old-timey.";

                case Cip:
                    return "No visible health bar.";

                case Savant:
                    return "Randomized spells.";

                case TheOne:
                    return "There is no spork.";

                case NoFurniture:
                    return "You break stuff and have no balance.";

                case PlatformsOpen:
                    return "Platforms stay open.";

                case Glaucoma:
                    return "It's so dark.";

                default:
                    return "NULL";
            }
        }

        // Scary function that creates traits.
        public static Vector2 CreateRandomTraits()
        {
            var zero = Vector2.Zero;
            var num = 0;
            var num2 = 2;
            var num3 = 94;
            var num4 = 39;
            var num5 = 1;
            var num6 = CDGMath.RandomInt(0, 100);
            for (var i = 0; i < num2; i++)
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
            int[] array =
            {
                48,
                37,
                15
            };
            byte b = 0;
            var num7 = 0;
            var num8 = CDGMath.RandomInt(0, 100);
            var list = new List<byte>();
            if (num > 0)
            {
                for (var j = 0; j < 3; j++)
                {
                    num7 += array[j];
                    if (num8 <= num7)
                    {
                        b = (byte) (j + 1);
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
                for (var k = 0; k < 3; k++)
                {
                    num7 += array[k];
                    if (num8 <= num7)
                    {
                        b = (byte) (k + 1);
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
                } while (zero.Y == zero.X || TraitConflict(zero));
            }
            return zero;
        }

        /// <summary>
        /// Returns true if two given traits would conflict with each other.
        /// </summary>
        /// <param name="traits"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public static bool TraitConflict(Vector2 traits)
        {
            // TODO: Probably rework this to not be a terrible float comparison? Why CellarDoor?
            return
                // Hypergonadism and Hypogonadism.
                (traits.X == Hypergonadism && traits.Y == Hypogonadism) ||
                (traits.Y == Hypergonadism && traits.X == Hypogonadism) ||

                // Endomorph and Ectomorph
                (traits.X == Endomorph && traits.Y == Ectomorph) ||
                (traits.Y == Endomorph && traits.X == Ectomorph) ||

                // Gigantism and Dwarfism
                (traits.X == Gigantism && traits.Y == Dwarfism) ||
                (traits.Y == Gigantism && traits.X == Dwarfism) ||

                // NearSighted and FarSighted
                (traits.X == NearSighted && traits.Y == FarSighted) ||
                (traits.Y == NearSighted && traits.X == FarSighted) ||

                // ColorBlind and Nostalgic
                (traits.X == ColorBlind && traits.Y == Nostalgic) ||
                (traits.Y == ColorBlind && traits.X == Nostalgic);
        }
    }
}
