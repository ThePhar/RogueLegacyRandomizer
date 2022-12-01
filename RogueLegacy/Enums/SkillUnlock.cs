// Rogue Legacy Randomizer - SkillUnlock.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

namespace RogueLegacy.Enums;

public enum SkillUnlock : byte
{
    None,
    Blacksmith,
    Enchantress,
    Architect,
    Ninja,
    Banker,
    SpellSword,
    Lich,
    KnightUp,
    WizardUp,
    BarbarianUp,
    NinjaUp,
    AssassinUp,
    BankerUp,
    SpellSwordUp,
    LichUp,
    Dragon,
    Traitor,
    NetworkItem
}

public static class SkillUnlockExtensions
{
    public static string Description(this SkillUnlock unlockType)
    {
        return unlockType switch
        {
            SkillUnlock.Blacksmith   => "The Blacksmith can build the finest equipment in the world, turning you into a veritably virtuous violent-villain vaporizer.\nGathering blueprints will give him an even wider array of assorted armaments for your armory.",
            SkillUnlock.Enchantress  => "The Enchantress can empower your body with magical runes, making you better, stronger, faster, jumpier.\nFind runes to increase her repertoire of body modifying talents.\n\nHer crystal ball is just for show.",
            SkillUnlock.Architect    => "The Architect can lock a castle down and prevent it from changing.\n\nLike the layout of a castle?\nLOCK IT DOWN!\nJust make sure you can afford his fees.",
            SkillUnlock.Ninja        => "The Shinobi. A super fast, super deadly warrior, who wears a super head band. They can't take a lot of hits, but they can dish out the pain. Believe it!",
            SkillUnlock.Banker       => "Trained more in the art of making money than killing enemies. Miners are best used for scouring the castle for treasure chests, and avoiding confrontation.",
            SkillUnlock.SpellSword   => "The Spellthief drains mana from the enemies they hit. This grants them access to an infinite supply of mana in the heat of a battle. Rock on!",
            SkillUnlock.Lich         => "Masters of life and death. The lich has mammoth potential. Every kill grants the Lich permanent health (to a cap), making them truly dangerous beings.",
            SkillUnlock.KnightUp     => "Promote your Knights into Paladins. Through rigorous training, Paladins have learned how to block blows from any direction.",
            SkillUnlock.WizardUp     => "Transform your measly Mages into Archmages. Master all of the arcane arts, and change whatever spells you have on the fly.",
            SkillUnlock.BarbarianUp  => "Convert your Barbarians into Barbarian KINGS! Learn the secret shouts of the bears, and shout at things until they explode.",
            SkillUnlock.NinjaUp      => "Go from a measly Shinobi, to the all powerful Hokage! Master the art of looking like you're hit, but secretly turning into a log at the last minute.  Good thing you carry a lot of logs on you.",
            SkillUnlock.AssassinUp   => "Train your Knaves to become full fledged Assassins. Use the power of the shadows to slip secretly past enemy ranks.",
            SkillUnlock.BankerUp     => "All that digging has turned your Miners into Spelunkers! Now that spiffy light on your head actually does something! (Not really).",
            SkillUnlock.SpellSwordUp => "Turn your Spellthieves into Spellswords, and grant yourself the power of summoning empowered spells.\nYou call that a axe? THIS is a axe.",
            SkillUnlock.LichUp       => "Raise your Liches into Lich Kings, and grant yourself the power of both mind AND matter! With a single press, convert your permanent health, into permanent mana.",
            SkillUnlock.Dragon       => "You have discovered the lost art of the dragons. Though you cannot jump, you have unlimited flight, and can cast forth powerful fireballs at your foes.",
            SkillUnlock.Traitor      => "You have discovered the hidden arts known only by Johannes the Traitor. All shall bow before your might!",
            _                        => ""
        };
    }
}
