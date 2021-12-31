// 
// RogueLegacyArchipelago - EquipmentAbilityType.cs
// Last Modified 2021-12-27
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
        public const int Vault = 0;
        public const int Sprint = 1;
        public const int Vampire = 2;
        public const int Sky = 3;
        public const int Siphon = 4;
        public const int Retaliation = 5;
        public const int Bounty = 6;
        public const int Haste = 7;
        public const int Curse = 8;
        public const int Grace = 9;
        public const int Balance = 10;
        public const int ArchitectFee = 20;
        public const int NewGamePlusGoldBonus = 21;
        public const int Total = 11;

        /// <summary>
        ///     Returns the string representation of a given ability's name.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string ToString(int type)
        {
            switch (type)
            {
                case Vault:
                    return "Vault";

                case Sprint:
                    return "Sprint";

                case Vampire:
                    return "Vampire";

                case Sky:
                    return "Sky";

                case Siphon:
                    return "Siphon";

                case Retaliation:
                    return "Retaliation";

                case Bounty:
                    return "Bounty";

                case Haste:
                    return "Haste";

                case Curse:
                    return "Curse";

                case Grace:
                    return "Grace";

                case Balance:
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
        ///     Returns a string describing this ability's features.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string Description(int type)
        {
            switch (type)
            {
                case Vault:
                    return
                        "Grants you the power to jump in the air. Multiple runes stack, allowing for multiple jumps.";

                case Sprint:
                    return "Gain the power to dash short distances. Multiple runes stack allowing for multiple dashes.";

                case Vampire:
                    return
                        "Killing enemies will drain them of their health. Multiple runes stack for increased life drain.";

                case Sky:
                    return "Gain the power of flight. Multiple runes stack for longer flight duration.";

                case Siphon:
                    return "Steals mana from slain enemies. Multiple runes stack for increased mana drain.";

                case Retaliation:
                    return "Returns damage taken from enemies. Multiple runes stack increasing the damage return.";

                case Bounty:
                    return
                        "Increase the amount of gold you get from coins. Multiple runes stack for increased gold gain.";

                case Haste:
                    return "Increase your base move speed. Multiple runes stack making you even faster.";

                case Curse:
                    return "Harder enemies, but greater rewards. Multiple runes stack making enemies even harder.";

                case Grace:
                    return
                        "Enemies scale slower, easier but lesser rewards. Multiple runes stack slowing enemy scaling more.";

                case Balance:
                    return "Slaying enemies grants both HP and MP. Multiple runes stack for increased hp/mp drain.";

                default:
                    return "";
            }
        }

        /// <summary>
        ///     Returns a shortened string describing this ability's features.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <param name="amount">Rune Power Amount</param>
        /// <returns></returns>
        public static string ShortDescription(int type, float amount)
        {
            switch (type)
            {
                case Vault:
                    return amount > 1f
                        ? string.Format("Air jump {0} times", amount)
                        : string.Format("Air jump {0} time", amount);

                case Sprint:
                    return amount > 1f
                        ? string.Format("Dash up to {0} times", amount)
                        : string.Format("Dash up to {0} time", amount);

                case Vampire:
                    return string.Format("Gain back {0} HP for every kill", amount);

                case Sky:
                    return amount > 1f
                        ? string.Format("Fly for {0} seconds", amount)
                        : string.Format("Fly for {0} second", amount);

                case Siphon:
                    return string.Format("Gain back {0} MP for every kill", amount);

                case Retaliation:
                    return string.Format("Return {0}% damage after getting hit", amount);

                case Bounty:
                    return string.Format("Each gold drop is {0}% more valuable", amount);

                case Haste:
                    return string.Format("Move {0}% faster", amount);

                case Curse:
                    var scale = (int) (amount / 4f * 2.75f);
                    return string.Format("Enemies start {0} levels higher", scale);

                case Grace:
                    return amount > 1f
                        ? string.Format("Enemies scale {0} units slower", amount)
                        : string.Format("Enemies scale {0} unit slower", amount);

                case Balance:
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
        ///     Returns a string describing how to use a particular ability.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string Instructions(int type)
        {
            switch (type)
            {
                case Vault:
                    return string.Format("Press [Input:{0}] while in air.", InputMapType.PlayerJump1);

                case Sprint:
                    return string.Format("[Input:{0}] or [Input:{1}] to dash.", InputMapType.PlayerDashLeft,
                        InputMapType.PlayerDashRight);

                case Vampire:
                    return "Kill enemies to regain health.";

                case Sky:
                    return string.Format("Hold [Input:{0}] while in air.", InputMapType.PlayerJump1);

                case Siphon:
                    return "Kill enemies to regain mana.";

                case Retaliation:
                    return "Damage returned to enemies.";

                case Bounty:
                    return "Coins give more gold.";

                case Haste:
                    return "Move faster.";

                case Curse:
                    return "Enemies are harder.";

                case Grace:
                    return "Enemies scale slower.";

                case Balance:
                    return "Kill enemies to regain health and mana.";

                default:
                    return "";
            }
        }

        /// <summary>
        ///     Returns the resource name for this ability.
        /// </summary>
        /// <param name="type">Ability Identifier</param>
        /// <returns></returns>
        public static string Icon(int type)
        {
            switch (type)
            {
                case Vault:
                    return "EnchantressUI_DoubleJumpIcon_Sprite";

                case Sprint:
                    return "EnchantressUI_DashIcon_Sprite";

                case Vampire:
                    return "EnchantressUI_VampirismIcon_Sprite";

                case Sky:
                    return "EnchantressUI_FlightIcon_Sprite";

                case Siphon:
                    return "EnchantressUI_ManaGainIcon_Sprite";

                case Retaliation:
                    return "EnchantressUI_DamageReturnIcon_Sprite";

                case Bounty:
                    return "Icon_Gold_Gain_Up_Sprite";

                case Haste:
                    return "EnchantressUI_SpeedUpIcon_Sprite";

                case Curse:
                    return "EnchantressUI_CurseIcon_Sprite";

                case Grace:
                    return "EnchantressUI_BlessingIcon_Sprite";

                case Balance:
                    return "EnchantressUI_BalanceIcon_Sprite";

                default:
                    return "";
            }
        }
    }
}