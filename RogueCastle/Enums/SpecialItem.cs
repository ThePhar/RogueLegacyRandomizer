using System;

namespace RogueCastle.Enums
{
    public enum SpecialItem
    {
        None,
        FreeEntrance,
        LoseCoins,
        Revive,
        SpikeImmunity,
        GoldPerKill,
        Compass,
        Glasses,
        EyeballToken,
        SkullToken,
        FireballToken,
        BlobToken,
        LastBossToken
    }

    public static class SpecialItemExtensions
    {
        public static string ToString(this SpecialItem item)
        {
            return item switch
            {
                SpecialItem.FreeEntrance  => "Charon's Obol",
                SpecialItem.LoseCoins     => "Hedgehog's Curse",
                SpecialItem.Revive        => "Hyperion's Ring",
                SpecialItem.SpikeImmunity => "Hermes' Boots",
                SpecialItem.GoldPerKill   => "Helios' Blessing",
                SpecialItem.Compass       => "Calypso's Compass",
                SpecialItem.Glasses       => "Nerdy Glasses",
                SpecialItem.EyeballToken  => "Khidr's Obol",
                SpecialItem.SkullToken    => "Alexander's Obol",
                SpecialItem.FireballToken => "Ponce de Leon's Obol",
                SpecialItem.BlobToken     => "Herodotus' Obol",
                SpecialItem.LastBossToken => "Traitor's Obol",
                _                         => throw new ArgumentException($"Unsupported SpecialItem Type in Description(): {nameof(item)}")
            };
        }
        public static string SpriteName(this SpecialItem item)
        {
            return item switch
            {
                SpecialItem.FreeEntrance  => "BonusRoomObolIcon_Sprite",
                SpecialItem.LoseCoins     => "BonusRoomHedgehogIcon_Sprite",
                SpecialItem.Revive        => "BonusRoomRingIcon_Sprite",
                SpecialItem.SpikeImmunity => "BonusRoomBootsIcon_Sprite",
                SpecialItem.GoldPerKill   => "BonusRoomBlessingIcon_Sprite",
                SpecialItem.Compass       => "BonusRoomCompassIcon_Sprite",
                SpecialItem.Glasses       => "BonusRoomGlassesIcon_Sprite",
                SpecialItem.EyeballToken  => "ChallengeIcon_Eyeball_Sprite",
                SpecialItem.SkullToken    => "ChallengeIcon_Skull_Sprite",
                SpecialItem.FireballToken => "ChallengeIcon_Fireball_Sprite",
                SpecialItem.BlobToken     => "ChallengeIcon_Blob_Sprite",
                SpecialItem.LastBossToken => "ChallengeIcon_LastBoss_Sprite",
                _                         => throw new ArgumentException($"Unsupported SpecialItem Type in SpriteName(): {nameof(item)}")
            };
        }
    }
}