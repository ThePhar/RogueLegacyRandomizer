// 
// RogueLegacyArchipelago - SpecialItemType.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace RogueCastle.Structs
{
    public static class SpecialItemType
    {
        public const byte None = 0;
        public const byte FreeEntrance = 1;
        public const byte LoseCoins = 2;
        public const byte Revive = 3;
        public const byte SpikeImmunity = 4;
        public const byte GoldPerKill = 5;
        public const byte Compass = 6;
        public const byte Glasses = 8;
        public const byte EyeballToken = 9;
        public const byte SkullToken = 10;
        public const byte FireballToken = 11;
        public const byte BlobToken = 12;
        public const byte LastBossToken = 13;

        public const byte Total = 7;

        /// <summary>
        /// Returns the string representation of a given special item type's name.
        /// </summary>
        /// <param name="itemType">Special Item Identifier</param>
        /// <returns></returns>
        public static string ToString(byte itemType)
        {
            switch (itemType)
            {
                case FreeEntrance:
                    return "Charon's Obol";

                case LoseCoins:
                    return "Hedgehog's Curse";

                case Revive:
                    return "Hyperion's Ring";

                case SpikeImmunity:
                    return "Hermes' Boots";

                case GoldPerKill:
                    return "Helios' Blessing";

                case Compass:
                    return "Calypso's Compass";

                case Glasses:
                    return "Nerdy Glasses";

                case EyeballToken:
                    return "Khidr's Obol";

                case SkullToken:
                    return "Alexander's Obol";

                case FireballToken:
                    return "Ponce De Leon's Obol";

                case BlobToken:
                    return "Herodotus' Obol";

                case LastBossToken:
                    return "Traitor's Obol";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the resource name for this special item.
        /// </summary>
        /// <param name="itemType">Special Item Identifier</param>
        /// <returns></returns>
        public static string SpriteName(byte itemType)
        {
            switch (itemType)
            {
                case FreeEntrance:
                    return "BonusRoomObolIcon_Sprite";

                case LoseCoins:
                    return "BonusRoomHedgehogIcon_Sprite";

                case Revive:
                    return "BonusRoomRingIcon_Sprite";

                case SpikeImmunity:
                    return "BonusRoomBootsIcon_Sprite";

                case GoldPerKill:
                    return "BonusRoomBlessingIcon_Sprite";

                case Compass:
                    return "BonusRoomCompassIcon_Sprite";

                case Glasses:
                    return "BonusRoomGlassesIcon_Sprite";

                case EyeballToken:
                    return "ChallengeIcon_Eyeball_Sprite";

                case SkullToken:
                    return "ChallengeIcon_Skull_Sprite";

                case FireballToken:
                    return "ChallengeIcon_Fireball_Sprite";

                case BlobToken:
                    return "ChallengeIcon_Blob_Sprite";

                case LastBossToken:
                    return "ChallengeIcon_LastBoss_Sprite";

                default:
                    return "";
            }
        }
    }
}
