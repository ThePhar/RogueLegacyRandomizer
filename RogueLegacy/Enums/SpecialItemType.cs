// Rogue Legacy Randomizer - SpecialItemType.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;

namespace RogueLegacy.Enums;

public enum SpecialItemType
{
    None,
    CharonsObol,
    HedgehogsCurse,
    HyperionsRing,
    HermesBoots,
    HeliosBlessing,
    CalypsosCompass,
    NerdyGlasses  = 8,
    EyeballToken  = 9,
    SkullToken    = 10,
    FireballToken = 11,
    BlobToken     = 12,
    LastBossToken = 13
}

public static class SpecialItemExtensions
{
    public static string Name(this SpecialItemType itemType)
    {
        return itemType switch
        {
            SpecialItemType.CharonsObol     => "Charon's Obol",
            SpecialItemType.HedgehogsCurse  => "Hedgehog's Curse",
            SpecialItemType.HyperionsRing   => "Hyperion's Ring",
            SpecialItemType.HermesBoots     => "Hermes' Boots",
            SpecialItemType.HeliosBlessing  => "Helios' Blessing",
            SpecialItemType.CalypsosCompass => "Calypso's Compass",
            SpecialItemType.NerdyGlasses    => "Nerdy Glasses",
            SpecialItemType.EyeballToken    => "Khidr's Obol",
            SpecialItemType.SkullToken      => "Alexander's Obol",
            SpecialItemType.FireballToken   => "Ponce de Leon's Obol",
            SpecialItemType.BlobToken       => "Herodotus' Obol",
            SpecialItemType.LastBossToken   => "Traitor's Obol",
            _                               => throw new ArgumentException($"Unsupported SpecialItem Type in Name(): {nameof(itemType)}")
        };
    }

    public static string SpriteName(this SpecialItemType itemType)
    {
        return itemType switch
        {
            SpecialItemType.CharonsObol     => "BonusRoomObolIcon_Sprite",
            SpecialItemType.HedgehogsCurse  => "BonusRoomHedgehogIcon_Sprite",
            SpecialItemType.HyperionsRing   => "BonusRoomRingIcon_Sprite",
            SpecialItemType.HermesBoots     => "BonusRoomBootsIcon_Sprite",
            SpecialItemType.HeliosBlessing  => "BonusRoomBlessingIcon_Sprite",
            SpecialItemType.CalypsosCompass => "BonusRoomCompassIcon_Sprite",
            SpecialItemType.NerdyGlasses    => "BonusRoomGlassesIcon_Sprite",
            SpecialItemType.EyeballToken    => "ChallengeIcon_Eyeball_Sprite",
            SpecialItemType.SkullToken      => "ChallengeIcon_Skull_Sprite",
            SpecialItemType.FireballToken   => "ChallengeIcon_Fireball_Sprite",
            SpecialItemType.BlobToken       => "ChallengeIcon_Blob_Sprite",
            SpecialItemType.LastBossToken   => "ChallengeIcon_LastBoss_Sprite",
            _                               => throw new ArgumentException($"Unsupported SpecialItem Type in SpriteName(): {nameof(itemType)}")
        };
    }
}
