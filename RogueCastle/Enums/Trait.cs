using System;
using System.Collections.Generic;
using System.Linq;
using DS2DEngine;

namespace RogueCastle.Enums
{
    public enum Trait
    {
        None,
        ColorBlind,
        Gay,
        NearSighted,
        FarSighted,
        Dyslexia,
        Gigantism,
        Dwarfism,
        Baldness,
        Endomorph,
        Ectomorph,
        Alzheimers,
        Dextrocardia,
        Coprolalia,
        Hyperactive,
        OCD,
        Hypergonadism,
        Hypogonadism,
        StereoBlind,
        IBS,
        Vertigo,
        TunnelVision,
        Ambilevous,
        PAD,
        Alektorophobia,
        Hypochondriac,
        Dementia,
        Flexible,
        EideticMemory,
        Nostalgic,
        CIP,
        Savant,
        TheOne,
        Clumsy,
        EHS,
        Glaucoma,
        Adopted = 100  // Hilarious that this is a trait in the game.
    }

    public static class TraitExtensions
    {
        public static TraitRarity Rarity(this Trait trait)
        {
            return trait switch
            {
                Trait.ColorBlind     => TraitRarity.Uncommon,
                Trait.Gay            => TraitRarity.Common,
                Trait.NearSighted    => TraitRarity.Uncommon,
                Trait.FarSighted     => TraitRarity.Rare,
                Trait.Dyslexia       => TraitRarity.Rare,
                Trait.Gigantism      => TraitRarity.Common,
                Trait.Dwarfism       => TraitRarity.Common,
                Trait.Baldness       => TraitRarity.Common,
                Trait.Endomorph      => TraitRarity.Common,
                Trait.Ectomorph      => TraitRarity.Uncommon,
                Trait.Alzheimers     => TraitRarity.Rare,
                Trait.Dextrocardia   => TraitRarity.Uncommon,
                Trait.Coprolalia     => TraitRarity.Common,
                Trait.Hyperactive    => TraitRarity.Common,
                Trait.OCD            => TraitRarity.Common,
                Trait.Hypergonadism  => TraitRarity.Common,
                Trait.Hypogonadism   => TraitRarity.Rare,
                Trait.StereoBlind    => TraitRarity.Common,
                Trait.IBS            => TraitRarity.Uncommon,
                Trait.Vertigo        => TraitRarity.Rare,
                Trait.TunnelVision   => TraitRarity.Uncommon,
                Trait.Ambilevous     => TraitRarity.Uncommon,
                Trait.PAD            => TraitRarity.Uncommon,
                Trait.Alektorophobia => TraitRarity.Uncommon,
                Trait.Hypochondriac  => TraitRarity.Rare,
                Trait.Dementia       => TraitRarity.Rare,
                Trait.Flexible       => TraitRarity.Uncommon,
                Trait.EideticMemory  => TraitRarity.Uncommon,
                Trait.Nostalgic      => TraitRarity.Rare,
                Trait.CIP            => TraitRarity.Rare,
                Trait.Savant         => TraitRarity.Uncommon,
                Trait.TheOne         => TraitRarity.Rare,
                Trait.Clumsy         => TraitRarity.Uncommon,
                Trait.EHS            => TraitRarity.Uncommon,
                Trait.Glaucoma       => TraitRarity.Uncommon,
                _                    => TraitRarity.NoRarity
            };
        }
        public static string ToString(this Trait trait)
        {
            return trait switch
            {
                Trait.ColorBlind     => "Color Blind",
                Trait.Gay            => "Gay",
                Trait.NearSighted    => "Near-Sighted",
                Trait.FarSighted     => "Far-Sighted",
                Trait.Dyslexia       => "Dyslexia",
                Trait.Gigantism      => "Gigantism",
                Trait.Dwarfism       => "Dwarfism",
                Trait.Baldness       => "Baldness",
                Trait.Endomorph      => "Endomorph",
                Trait.Ectomorph      => "Ectomorph",
                Trait.Alzheimers     => "Alzheimers",
                Trait.Dextrocardia   => "Dextrocardia",
                Trait.Coprolalia     => "Coprolalia",
                Trait.Hyperactive    => "ADHD",
                Trait.OCD            => "O.C.D.",
                Trait.Hypergonadism  => "Hypergonadism",
                Trait.Hypogonadism   => "Muscle Wk.",
                Trait.StereoBlind    => "Stereo Blind",
                Trait.IBS            => "I.B.S.",
                Trait.Vertigo        => "Vertigo",
                Trait.TunnelVision   => "Tunnel Vision",
                Trait.Ambilevous     => "Ambilevous",
                Trait.PAD            => "P.A.D.",
                Trait.Alektorophobia => "Alektorophobia",
                Trait.Hypochondriac  => "Hypochondriac",
                Trait.Dementia       => "Dementia",
                Trait.Flexible       => "Flexible",
                Trait.EideticMemory  => "Eid. Mem.",
                Trait.Nostalgic      => "Nostalgic",
                Trait.CIP            => "C.I.P.",
                Trait.Savant         => "Savant",
                Trait.TheOne         => "The One",
                Trait.Clumsy         => "Clumsy",
                Trait.EHS            => "EHS",
                Trait.Glaucoma       => "Glaucoma",
                _                    => throw new ArgumentException($"Unsupported Trait Type in ToString(): {nameof(trait)}")
            };
        }
        public static string Description(this Trait trait, bool isFemale = false)
        {
            return trait switch
            {
                Trait.ColorBlind     => "You can't see colors due to monochromacy.",
                Trait.Gay            => isFemale ? "You like the ladies." : "You are a fan of the man.",
                Trait.NearSighted    => "Anything far away is blurry.",
                Trait.FarSighted     => "Anything close-up is blurry.",
                Trait.Dyslexia       => "You hvae trboule raednig tinhgs.",
                Trait.Gigantism      => "You were born to be a basketball player.",
                Trait.Dwarfism       => "You never get to ride roller-coasters.",
                Trait.Baldness       => "The bald and the beautiful.",
                Trait.Endomorph      => "You're so heavy, enemies can't knock you back.",
                Trait.Ectomorph      => "You're skinny, so every hit sends you flying.",
                Trait.Alzheimers     => "You have trouble remembering where you are.",
                Trait.Dextrocardia   => "Your MP and HP pools are swapped.  Who knew?",
                Trait.Coprolalia     => "%#&@!",
                Trait.Hyperactive    => "So energetic! You move faster.",
                Trait.OCD            => "Must. Clear. House. Break stuff to restore MP.",
                Trait.Hypergonadism  => "You're perma-roided. Attacks knock enemies further.",
                Trait.Hypogonadism   => "You have weak limbs.  Enemies won't get knocked back.",
                Trait.StereoBlind    => "You can't see in 3D.",
                Trait.IBS            => "Even the most valiant heroes can suffer from irritable bowels.",
                Trait.Vertigo        => "Welcome to Barfs-ville.",
                Trait.TunnelVision   => "No peripheral vision.",
                Trait.Ambilevous     => "You've got two left hands, and can't cast spells properly.",
                Trait.PAD            => "Peripheral Arterial Disease. No foot pulse.",
                Trait.Alektorophobia => "Chickens freak you out.",
                Trait.Hypochondriac  => "You tend to EXAGGERATE.",
                Trait.Dementia       => "You are insane.",
                Trait.Flexible       => "You are very flexible.",
                Trait.EideticMemory  => "You remember things with extreme clarity.",
                Trait.Nostalgic      => "You miss the good old days.",
                Trait.CIP            => "Congenital Insensitivity to Pain. Know no pain.",
                Trait.Savant         => "You're very talented. With a few issues.",
                Trait.TheOne         => "There is no spork.",
                Trait.Clumsy         => "You break a lot of things.",
                Trait.EHS            => "You conduct electricity really well.",
                Trait.Glaucoma       => "It's so dark.",
                _                    => throw new ArgumentException($"Unsupported Trait Type in Description(): {nameof(trait)}")
            };
        }
        public static string ProfileCardDescription(this Trait trait, bool isFemale = false)
        {
            return trait switch
            {
                Trait.ColorBlind     => "You can't see colors.",
                Trait.Gay            => isFemale ? "You like the ladies." : "You are a fan of the man.",
                Trait.NearSighted    => "Anything far away is blurry.",
                Trait.FarSighted     => "Anything close-up is blurry.",
                Trait.Dyslexia       => "You hvae trboule raednig tinhgs.",
                Trait.Gigantism      => "You are huge.",
                Trait.Dwarfism       => "You are tiny.",
                Trait.Baldness       => "You are bald.",
                Trait.Endomorph      => "Can't get knocked back.",
                Trait.Ectomorph      => "Hits send you flying.",
                Trait.Alzheimers     => "Where are you?",
                Trait.Dextrocardia   => "MP + HP pools swapped.",
                Trait.Coprolalia     => "%#&@!",
                Trait.Hyperactive    => "You move faster.",
                Trait.OCD            => "Break stuff to restore MP.",
                Trait.Hypergonadism  => "You knock enemies out of the park.",
                Trait.Hypogonadism   => "You can't knock enemies back.",
                Trait.StereoBlind    => "You can't see in 3D.",
                Trait.IBS            => "You fart a lot.",
                Trait.Vertigo        => "Everything is upside down.",
                Trait.TunnelVision   => "No early indicators.",
                Trait.Ambilevous     => "Spells come out your back.",
                Trait.PAD            => "No foot pulse.",
                Trait.Alektorophobia => "You are scared of chickens.",
                Trait.Hypochondriac  => "Exaggerate the damage you take.",
                Trait.Dementia       => "You see things that aren't there.",
                Trait.Flexible       => "You turn while fighting.",
                Trait.EideticMemory  => "Remember enemy placement.",
                Trait.Nostalgic      => "Everything is old-timey.",
                Trait.CIP            => "No visible health bar.",
                Trait.Savant         => "Randomized spells.",
                Trait.TheOne         => "There is no spork.",
                Trait.Clumsy         => "You break stuff and have no balance.",
                Trait.EHS            => "Platforms stay open.",
                Trait.Glaucoma       => "It's so dark.",
                _                    => throw new ArgumentException($"Unsupported Trait Type in ProfileCardDescription(): {nameof(trait)}")
            };
        }
        public static bool ConflictsWith(this Trait a, Trait b)
        {
            return a switch
            {
                Trait.Hypergonadism when b == Trait.Hypogonadism => true,
                Trait.Hypogonadism when b == Trait.Hypergonadism => true,
                Trait.Endomorph when b == Trait.Ectomorph        => true,
                Trait.Ectomorph when b == Trait.Endomorph        => true,
                Trait.Gigantism when b == Trait.Dwarfism         => true,
                Trait.Dwarfism when b == Trait.Gigantism         => true,
                Trait.NearSighted when b == Trait.FarSighted     => true,
                Trait.FarSighted when b == Trait.NearSighted     => true,
                Trait.ColorBlind when b == Trait.Nostalgic       => true,
                Trait.Nostalgic when b == Trait.ColorBlind       => true,
                _                                                => false
            };
        }

        public static Trait[] CreateRandomTraits()
        {
            var traits = new[] { Trait.None, Trait.None };
            int[] rarityThresholds = { 48, 85, 100 };
            int[] quantityThresholds = { 94, 55 };

            // Determine Quantity of Traits.
            var quantityCheck = CDGMath.RandomInt(0, 100);
            var traitCount = quantityThresholds.Count(quantityThreshold => quantityCheck < quantityThreshold);

            for (var i = 0; i < traitCount; i++)
            {
                var rarity = TraitRarity.Rare;
                var rarityCheck = CDGMath.RandomInt(0, 100);
                var possibleTraits = new List<Trait>();

                // Determine Rarity
                foreach (var rarityThreshold in rarityThresholds)
                {
                    // Check threshold and if we do not reach the threshold, "reduce" the rarity of possible traits by one (technically incrementing decreases rarity).
                    if (rarityCheck > rarityThreshold)
                    {
                        rarity += 1;
                        continue;
                    }

                    break;
                }

                // Add traits of our determined rarity to possible traits pool.
                foreach (Trait trait in Enum.GetValues(typeof(Trait)))
                {
                    if (rarity == Rarity(trait))
                    {
                        possibleTraits.Add(trait);
                    }
                }

                // Add trait to item pool.
                if (i == 0)
                {
                    traits[i] = possibleTraits[CDGMath.RandomInt(0, possibleTraits.Count - 1)];
                }
                else do
                {
                    traits[i] = possibleTraits[CDGMath.RandomInt(0, possibleTraits.Count - 1)];
                } while (traits[1] == traits[0] || traits[0].ConflictsWith(traits[1]));
            }

            return traits;
        }
    }
}