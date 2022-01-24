// 
//  Rogue Legacy Randomizer - SpecialItemType.cs
//  Last Modified 2022-01-24
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;

namespace RogueCastle.Enums
{
    public enum SpecialItemType
    {
        None,
        FreeEntrance,
        LoseCoins,
        Revive,
        SpikeImmunity,
        GoldPerKill,
        Compass,
        Glasses       = 8,
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
                SpecialItemType.FreeEntrance  => "Charon's Obol",
                SpecialItemType.LoseCoins     => "Hedgehog's Curse",
                SpecialItemType.Revive        => "Hyperion's Ring",
                SpecialItemType.SpikeImmunity => "Hermes' Boots",
                SpecialItemType.GoldPerKill   => "Helios' Blessing",
                SpecialItemType.Compass       => "Calypso's Compass",
                SpecialItemType.Glasses       => "Nerdy Glasses",
                SpecialItemType.EyeballToken  => "Khidr's Obol",
                SpecialItemType.SkullToken    => "Alexander's Obol",
                SpecialItemType.FireballToken => "Ponce de Leon's Obol",
                SpecialItemType.BlobToken     => "Herodotus' Obol",
                SpecialItemType.LastBossToken => "Traitor's Obol",
                _                             => throw new ArgumentException($"Unsupported SpecialItem Type in Name(): {nameof(itemType)}")
            };
        }

        public static string SpriteName(this SpecialItemType itemType)
        {
            return itemType switch
            {
                SpecialItemType.FreeEntrance  => "BonusRoomObolIcon_Sprite",
                SpecialItemType.LoseCoins     => "BonusRoomHedgehogIcon_Sprite",
                SpecialItemType.Revive        => "BonusRoomRingIcon_Sprite",
                SpecialItemType.SpikeImmunity => "BonusRoomBootsIcon_Sprite",
                SpecialItemType.GoldPerKill   => "BonusRoomBlessingIcon_Sprite",
                SpecialItemType.Compass       => "BonusRoomCompassIcon_Sprite",
                SpecialItemType.Glasses       => "BonusRoomGlassesIcon_Sprite",
                SpecialItemType.EyeballToken  => "ChallengeIcon_Eyeball_Sprite",
                SpecialItemType.SkullToken    => "ChallengeIcon_Skull_Sprite",
                SpecialItemType.FireballToken => "ChallengeIcon_Fireball_Sprite",
                SpecialItemType.BlobToken     => "ChallengeIcon_Blob_Sprite",
                SpecialItemType.LastBossToken => "ChallengeIcon_LastBoss_Sprite",
                _                             => throw new ArgumentException($"Unsupported SpecialItem Type in SpriteName(): {nameof(itemType)}")
            };
        }
    }
}
