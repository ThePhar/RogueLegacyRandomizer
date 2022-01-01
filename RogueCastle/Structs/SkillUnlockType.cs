// 
// RogueLegacyArchipelago - SkillUnlockType.cs
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
    public static class SkillUnlockType
    {
        public const byte None = 0;
        public const byte Blacksmith = 1;
        public const byte Enchantress = 2;
        public const byte Architect = 3;
        public const byte Ninja = 4;
        public const byte Banker = 5;
        public const byte SpellSword = 6;
        public const byte Lich = 7;
        public const byte KnightUp = 8;
        public const byte WizardUp = 9;
        public const byte BarbarianUp = 10;
        public const byte NinjaUp = 11;
        public const byte AssassinUp = 12;
        public const byte BankerUp = 13;
        public const byte SpellSwordUp = 14;
        public const byte LichUp = 15;
        public const byte Dragon = 16;
        public const byte Traitor = 17;
        public const byte NetworkItem = 18;

        /// <summary>
        ///     Returns the full unlock description for this skill unlock.
        /// </summary>
        /// <param name="unlockType">Unlock Type Identifier</param>
        /// <returns></returns>
        public static string Description(byte unlockType)
        {
            switch (unlockType)
            {
                case Blacksmith:
                    return
                        "The Blacksmith can build the finest equipment in the world, turning you into a veritably virtuous violent-villain vaporizer.\nGathering blueprints will give him an even wider array of assorted armaments for your armory.";

                case Enchantress:
                    return
                        "The Enchantress can empower your body with magical runes, making you better, stronger, faster, jumpier.\nFind runes to increase her repertoire of body modifying talents.\n\nHer crystal ball is just for show.";

                case Architect:
                    return
                        "The Architect can lock a castle down and prevent it from changing.\n\nLike the layout of a castle?\nLOCK IT DOWN!\nJust make sure you can afford his fees.";

                case Ninja:
                    return
                        "The Shinobi. A super fast, super deadly warrior, who wears a super head band. They can't take a lot of hits, but they can dish out the pain. Believe it!";

                case Banker:
                    return
                        "Trained more in the art of making money than killing enemies. Miners are best used for scouring the castle for treasure chests, and avoiding confrontation.";

                case SpellSword:
                    return
                        "The Spellthief drains mana from the enemies they hit. This grants them access to an infinite supply of mana in the heat of a battle. Rock on!";

                case Lich:
                    return
                        "Masters of life and death. The lich has mammoth potential. Every kill grants the Lich permanent health (to a cap), making them truly dangerous beings.";

                case KnightUp:
                    return
                        "Promote your Knights into Paladins. Through rigorous training, Paladins have learned how to block blows from any direction.";

                case WizardUp:
                    return
                        "Transform your measly Mages into Archmages. Master all of the arcane arts, and change whatever spells you have on the fly.";

                case BarbarianUp:
                    return
                        "Convert your Barbarians into Barbarian KINGS! Learn the secret shouts of the bears, and shout at things until they explode.";

                case NinjaUp:
                    return
                        "Go from a measly Shinobi, to the all powerful Hokage! Master the art of looking like you're hit, but secretly turning into a log at the last minute.  Good thing you carry a lot of logs on you.";

                case AssassinUp:
                    return
                        "Train your Knaves to become full fledged Assassins. Use the power of the shadows to slip secretly past enemy ranks.";

                case BankerUp:
                    return
                        "All that digging has turned your Miners into Spelunkers! Now that spiffy light on your head actually does something! (Not really).";

                case SpellSwordUp:
                    return
                        "Turn your Spellthieves into Spellswords, and grant yourself the power of summoning empowered spells.\nYou call that a axe? THIS is a axe.";

                case LichUp:
                    return
                        "Raise your Liches into Lich Kings, and grant yourself the power of both mind AND matter! With a single press, convert your permanent health, into permanent mana.";

                case Dragon:
                    return
                        "You have discovered the lost art of the dragons. Though you cannot jump, you have unlimited flight, and can cast forth powerful fireballs at your foes.";

                case Traitor:
                    return
                        "You have discovered the hidden arts known only by Johannes the Traitor. All shall bow before your might!";

                default:
                    return "";
            }
        }
    }
}