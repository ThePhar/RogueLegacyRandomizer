// 
// RogueLegacyArchipelago - EquipmentAbilityType.cs
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
    public static class EquipmentAbilityType
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
        public const int ManaHpGain = 10;
        public const int ArchitectFee = 20;
        public const int NewGamePlusGoldBonus = 21;

        public const int Total = 11;

        /// <summary>
        /// Returns the string representation of a given ability's name.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string ToString(int type)
        {
            switch (type)
            {
                case DoubleJump:
                    return "Vault";

                case Dash:
                    return "Sprint";

                case Vampirism:
                    return "Vampire";

                case Flight:
                    return "Sky";

                case ManaGain:
                    return "Siphon";

                case DamageReturn:
                    return "Retaliation";

                case GoldGain:
                    return "Bounty";

                case MovementSpeed:
                    return "Haste";

                case RoomLevelUp:
                    return "Curse";

                case RoomLevelDown:
                    return "Grace";

                case ManaHpGain:
                    return "Balance";

                case ArchitectFee:
                    return "Architect's Fee";

                case NewGamePlusGoldBonus:
                    return "NG+ Bonus";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a string describing this ability's features.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string Description(int type)
        {
            switch (type)
            {
                case DoubleJump:
                    return "Grants you the power to jump in the air. Multiple runes stack, allowing for multiple jumps.";

                case Dash:
                    return "Gain the power to dash short distances. Multiple runes stack allowing for multiple dashes.";

                case Vampirism:
                    return "Killing enemies will drain them of their health. Multiple runes stack for increased life drain.";

                case Flight:
                    return "Gain the power of flight. Multiple runes stack for longer flight duration.";

                case ManaGain:
                    return "Steals mana from slain enemies. Multiple runes stack for increased mana drain.";

                case DamageReturn:
                    return "Returns damage taken from enemies. Multiple runes stack increasing the damage return.";

                case GoldGain:
                    return "Increase the amount of gold you get from coins. Multiple runes stack for increased gold gain.";

                case MovementSpeed:
                    return "Increase your base move speed. Multiple runes stack making you even faster.";

                case RoomLevelUp:
                    return "Harder enemies, but greater rewards. Multiple runes stack making enemies even harder.";

                case RoomLevelDown:
                    return "Enemies scale slower, easier but lesser rewards. Multiple runes stack slowing enemy scaling more.";

                case ManaHpGain:
                    return "Slaying enemies grants both HP and MP. Multiple runes stack for increased hp/mp drain.";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a shortened string describing this ability's features.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <param name="amount">Rune Power Amount</param>
        /// <returns></returns>
        public static string ShortDescription(int type, float amount)
        {
            switch (type)
            {
                case DoubleJump:
                    return amount > 1f
                        ? string.Format("Air jump {0} times", amount)
                        : string.Format("Air jump {0} time", amount);

                case Dash:
                    return amount > 1f
                        ? string.Format("Dash up to {0} times", amount)
                        : string.Format("Dash up to {0} time", amount);

                case Vampirism:
                    return string.Format("Gain back {0} HP for every kill", amount);

                case Flight:
                    return amount > 1f
                        ? string.Format("Fly for {0} seconds", amount)
                        : string.Format("Fly for {0} second", amount);

                case ManaGain:
                    return string.Format("Gain back {0} MP for every kill", amount);

                case DamageReturn:
                    return string.Format("Return {0}% damage after getting hit", amount);

                case GoldGain:
                    return string.Format("Each gold drop is {0}% more valuable", amount);

                case MovementSpeed:
                    return string.Format("Move {0}% faster", amount);

                case RoomLevelUp:
                    var scale = (int) (amount / 4f * 2.75f);
                    return string.Format("Enemies start {0} levels higher", scale);

                case RoomLevelDown:
                    return amount > 1f
                        ? string.Format("Enemies scale {0} units slower", amount)
                        : string.Format("Enemies scale {0} unit slower", amount);

                case ManaHpGain:
                    return "Mana HP";

                case ArchitectFee:
                    return "Earn 60% total gold in the castle.";

                case NewGamePlusGoldBonus:
                    return string.Format("Bounty increased by {0}%", amount);

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns a string describing how to use a particular ability.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string Instructions(int type)
        {
            switch (type)
            {
                case DoubleJump:
                    return string.Format("Press [Input:{0}] while in air.", InputMapType.PlayerJump1);

                case Dash:
                    return string.Format("[Input:{0}] or [Input:{1}] to dash.", InputMapType.PlayerDashLeft, InputMapType.PlayerDashRight);

                case Vampirism:
                    return "Kill enemies to regain health.";

                case Flight:
                    return string.Format("Hold [Input:{0}] while in air.", InputMapType.PlayerJump1);

                case ManaGain:
                    return "Kill enemies to regain mana.";

                case DamageReturn:
                    return "Damage returned to enemies.";

                case GoldGain:
                    return "Coins give more gold.";

                case MovementSpeed:
                    return "Move faster.";

                case RoomLevelUp:
                    return "Enemies are harder.";

                case RoomLevelDown:
                    return "Enemies scale slower.";

                case ManaHpGain:
                    return "Kill enemies to regain health and mana.";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the resource name for this ability.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string Icon(int type)
        {
            switch (type)
            {
                case DoubleJump:
                    return "EnchantressUI_DoubleJumpIcon_Sprite";

                case Dash:
                    return "EnchantressUI_DashIcon_Sprite";

                case Vampirism:
                    return "EnchantressUI_VampirismIcon_Sprite";

                case Flight:
                    return "EnchantressUI_FlightIcon_Sprite";

                case ManaGain:
                    return "EnchantressUI_ManaGainIcon_Sprite";

                case DamageReturn:
                    return "EnchantressUI_DamageReturnIcon_Sprite";

                case GoldGain:
                    return "Icon_Gold_Gain_Up_Sprite";

                case MovementSpeed:
                    return "EnchantressUI_SpeedUpIcon_Sprite";

                case RoomLevelUp:
                    return "EnchantressUI_CurseIcon_Sprite";

                case RoomLevelDown:
                    return "EnchantressUI_BlessingIcon_Sprite";

                case ManaHpGain:
                    return "EnchantressUI_BalanceIcon_Sprite";

                default:
                    return "";
            }
        }
    }
}
