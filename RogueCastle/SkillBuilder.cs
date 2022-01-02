//
//  Rogue Legacy Randomizer - SkillBuilder.cs
//  Last Modified 2021-12-31
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using Microsoft.Xna.Framework;
using RogueCastle.Structs;

namespace RogueCastle
{
    internal class SkillBuilder
    {
        public static SkillObj BuildSkill(SkillType skillType)
        {
            var skillObj = new SkillObj("Icon_SwordLocked_Sprite");
            switch (skillType)
            {
                case SkillType.Filler:
                    skillObj.Name = "Filler";
                    skillObj.Description =
                        "This is a filler trait used to link to other traits. This text should never be visible.";
                    break;

                case SkillType.HealthUp:
                    skillObj.Name = "Health Up";
                    skillObj.Description = "Improve your cardio workout. A better heart means better health.";
                    skillObj.PerLevelModifier = 10f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 40;
                    skillObj.MaxLevel = 75;
                    skillObj.IconName = "Icon_Health_UpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.InvulnerabilityTimeUp:
                    skillObj.Name = "Invuln Time Up";
                    skillObj.Description =
                        "Strengthen your adrenal glands and be invulnerable  like Bane. Let the games begin!";
                    skillObj.PerLevelModifier = 0.1f;
                    skillObj.BaseCost = 750;
                    skillObj.Appreciation = 1700;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "Icon_InvulnTimeUpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " sec";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.DeathDodge:
                    skillObj.Name = "Death Defy";
                    skillObj.Description = "Release your inner cat, and avoid death. Sometimes.";
                    skillObj.PerLevelModifier = 0.015f;
                    skillObj.BaseCost = 750;
                    skillObj.Appreciation = 1500;
                    skillObj.MaxLevel = 10;
                    skillObj.IconName = "Icon_DeathDefyLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.AttackUp:
                    skillObj.Name = "Attack Up";
                    skillObj.Description =
                        "A proper gym will allow you to really  strengthen your arms and butt muscles.";
                    skillObj.PerLevelModifier = 2f;
                    skillObj.BaseCost = 100;
                    skillObj.Appreciation = 85;
                    skillObj.MaxLevel = 75;
                    skillObj.IconName = "Icon_SwordLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " str";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.DownStrikeUp:
                    skillObj.Name = "Down Strike Up";
                    skillObj.Description =
                        "A pogo practice room has its benefits. Deal more damage with consecutive down strikes.";
                    skillObj.PerLevelModifier = 0.05f;
                    skillObj.BaseCost = 750;
                    skillObj.Appreciation = 1500;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "Icon_Attack_UpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.CritChanceUp:
                    skillObj.Name = "Crit Chance Up";
                    skillObj.Description =
                        "Teaching yourself about the weaknesses of enemies allows you to strike with deadly efficiency.";
                    skillObj.PerLevelModifier = 0.02f;
                    skillObj.BaseCost = 150;
                    skillObj.Appreciation = 125;
                    skillObj.MaxLevel = 25;
                    skillObj.IconName = "Icon_Crit_Chance_UpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.CritDamageUp:
                    skillObj.Name = "Crit Damage Up";
                    skillObj.Description = "Practice the deadly strikes to be even deadlier. Enemies will be so dead.";
                    skillObj.PerLevelModifier = 0.05f;
                    skillObj.BaseCost = 150;
                    skillObj.Appreciation = 125;
                    skillObj.MaxLevel = 25;
                    skillObj.IconName = "Icon_Crit_Damage_UpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.MagicDamageUp:
                    skillObj.Name = "Magic Damage Up";
                    skillObj.Description =
                        "Learn the secrets of the universe, so you can use it to kill with spells better.";
                    skillObj.PerLevelModifier = 2f;
                    skillObj.BaseCost = 100;
                    skillObj.Appreciation = 85;
                    skillObj.MaxLevel = 75;
                    skillObj.IconName = "Icon_MagicDmgUpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " int";
                    skillObj.DisplayStat = false;
                    skillObj.StatType = 0;
                    break;

                case SkillType.ManaUp:
                    skillObj.Name = "Mana Up";
                    skillObj.Description = "Increase your mental fortitude in order to increase your mana pool. ";
                    skillObj.PerLevelModifier = 10f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 40;
                    skillObj.MaxLevel = 75;
                    skillObj.IconName = "Icon_ManaUpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " mp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.ManaCostDown:
                    skillObj.Name = "Mana Cost Down";
                    skillObj.Description = "Practice your basics to reduce mana costs when casting spells.";
                    skillObj.PerLevelModifier = 0.05f;
                    skillObj.BaseCost = 750;
                    skillObj.Appreciation = 1700;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "Icon_ManaCostDownLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.Smithy:
                    skillObj.Name = "Smithy";
                    skillObj.Description = "Unlock the smithy and gain access to phat loot.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "0";
                    skillObj.DisplayStat = false;
                    skillObj.StatType = 0;
                    break;

                case SkillType.Enchanter:
                    skillObj.Name = "Enchantress";
                    skillObj.Description = "Unlock the enchantress and gain access to her magical runes and powers.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_EnchanterLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "0";
                    skillObj.DisplayStat = false;
                    skillObj.StatType = 0;
                    break;

                case SkillType.Architect:
                    skillObj.Name = "Architect";
                    skillObj.Description = "Unlock the architect and gain the powers to lock down the castle.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_ArchitectLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "0";
                    skillObj.DisplayStat = false;
                    skillObj.StatType = 0;
                    break;

                case SkillType.EquipUp:
                    skillObj.Name = "Equip Up";
                    skillObj.Description =
                        "Upgrading your carry capacity will allow you to wear better and heavier armor.";
                    skillObj.PerLevelModifier = 10f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 40;
                    skillObj.MaxLevel = 50;
                    skillObj.IconName = "Icon_Equip_UpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " weight";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.ArmorUp:
                    skillObj.Name = "Armor Up";
                    skillObj.Description = "Strengthen your innards through natural means to reduce incoming damage.";
                    skillObj.PerLevelModifier = 4f;
                    skillObj.BaseCost = 125;
                    skillObj.Appreciation = 105;
                    skillObj.MaxLevel = 50;
                    skillObj.IconName = "Icon_ShieldLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " armor";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.GoldGainUp:
                    skillObj.Name = "Gold Gain Up";
                    skillObj.Description = "Improve your looting skills, and get more bang for your buck.";
                    skillObj.PerLevelModifier = 0.1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 2150;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "Icon_Gold_Gain_UpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.PricesDown:
                    skillObj.Name = "Haggle";
                    skillObj.Description = "Lower Charon's toll by learning how to barter with death itself.";
                    skillObj.PerLevelModifier = 0.1f;
                    skillObj.BaseCost = 500;
                    skillObj.Appreciation = 1000;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "Icon_HaggleLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.PotionUp:
                    skillObj.Name = "Potion Up";
                    skillObj.Description = "Gut cleansing leads to noticable improvements from both potions and meat.";
                    skillObj.PerLevelModifier = 0.01f;
                    skillObj.BaseCost = 750;
                    skillObj.Appreciation = 1750;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "Icon_PotionUpLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = "% hp/mp";
                    skillObj.DisplayStat = false;
                    skillObj.StatType = 0;
                    break;

                case SkillType.RandomizeChildren:
                    skillObj.Name = "Randomize Children";
                    skillObj.Description =
                        "Use the power of science to make a whole new batch of babies. Just... don't ask.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 5000;
                    skillObj.Appreciation = 5000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_RandomizeChildrenLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 9 + "] to randomize your children";
                    skillObj.UnitOfMeasurement = "%";
                    skillObj.DisplayStat = false;
                    skillObj.StatType = 0;
                    break;

                case SkillType.LichUnlock:
                    skillObj.Name = "Unlock Lich";
                    skillObj.Description = "Release the power of the Lich! A being of massive potential.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 850;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_LichUnlockLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.BankerUnlock:
                    skillObj.Name = "Unlock Miner";
                    skillObj.Description = "Unlock the skills of the Miner and raise your family fortune";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 400;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SpelunkerUnlockLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.SpellswordUnlock:
                    skillObj.Name = "Unlock Spell Thief";
                    skillObj.Description = "Unlock the Spellthief, and  become a martial  mage.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 850;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SpellswordUnlockLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.NinjaUnlock:
                    skillObj.Name = "Unlock Shinobi";
                    skillObj.Description = "Unlock the Shinobi, the fleetest of fighters.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 400;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_NinjaUnlockLocked_Sprite";
                    skillObj.InputDescription = " ";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.KnightUp:
                    skillObj.Name = "Upgrade Knight";
                    skillObj.Description = "Turn your knights into Paladins. A ferocious forefront fighter.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 50;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_KnightUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to block all incoming damage.";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.MageUp:
                    skillObj.Name = "Upgrade Mage";
                    skillObj.Description =
                        "Unlock the latent powers of the Mage and transform them into the all powerful Archmage";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 300;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_WizardUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to switch spells";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.AssassinUp:
                    skillObj.Name = "Upgrade Knave";
                    skillObj.Description = "Learn the dark arts, and turn the Knave into an Assassin";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 300;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_AssassinUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to turn to mist";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.BankerUp:
                    skillObj.Name = "Upgrade Miner";
                    skillObj.Description = "Earn your geology degree and go from Miner to Spelunker. Spiffy.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1750;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SpelunkerUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to turn on your headlamp";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.BarbarianUp:
                    skillObj.Name = "Upgrade Barbarian";
                    skillObj.Description = "Become a Barbarian King.  The king of freemen. That makes no sense.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 300;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_BarbarianUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 +
                                                "] to cast an epic shout that knocks virtually everything away.";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.LichUp:
                    skillObj.Name = "Upgrade Lich";
                    skillObj.Description = "Royalize your all-powerful Liches, and turn them into Lich Kings.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1500;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_LichUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to convert max hp into max mp";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.NinjaUp:
                    skillObj.Name = "Upgrade Shinobi";
                    skillObj.Description =
                        "Become the leader of your village, and turn your Shinobi into a Hokage. Believe it!";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 750;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_NinjaUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to flash";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.SpellSwordUp:
                    skillObj.Name = "Upgrade Spell Thief";
                    skillObj.Description = "Ride the vortexes of magic, and turn your Spellthiefs into Spellswords.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1500;
                    skillObj.Appreciation = 0;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SpellswordUpLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 13 + "] to cast empowered spells";
                    skillObj.UnitOfMeasurement = " hp";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    break;

                case SkillType.SuperSecret:
                    skillObj.Name = "Beastiality";
                    skillObj.Description = "Half man, half ******, all awesome.";
                    skillObj.PerLevelModifier = 10f;
                    skillObj.BaseCost = 5000;
                    skillObj.Appreciation = 30;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_Display_Boss_RoomsLocked_Sprite";
                    skillObj.InputDescription = "Press [Input:" + 10 + "] to awesome.";
                    skillObj.UnitOfMeasurement = "hp";
                    skillObj.DisplayStat = true;
                    break;

                case SkillType.Traitorous:
                    skillObj.Name = "Traitorous";
                    skillObj.Description = "[REDACTED]";
                    skillObj.PerLevelModifier = 10f;
                    skillObj.BaseCost = 8000;
                    skillObj.Appreciation = 30;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_Display_Boss_RoomsLocked_Sprite";
                    skillObj.InputDescription = "Fountain text here.";
                    skillObj.UnitOfMeasurement = "hp";
                    break;

                case SkillType.StoutHeart:
                    skillObj.Name = "Stout Heart";
                    skillObj.Description = "Your have viking ancestry.  \n\nIncrease your starting endurance.";
                    skillObj.PerLevelModifier = 20f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 500;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.QuickOfBreath:
                    skillObj.Name = "Quick of Breath";
                    skillObj.Description =
                        "QUICK OF BREATH \nYou're a heavy breather.  Bad for stalking, good for walking! \n\nIncrease your natural endurance regeneration.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 500;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.BornToRun:
                    skillObj.Name = "Born to Run";
                    skillObj.Description =
                        "You were infused with tiger blood at a young age.  You have now been infused with the power to release tiger blood when stabbed. \nRunning drains less endurance.";
                    skillObj.Position = new Vector2(50f, 100f);
                    skillObj.PerLevelModifier = 0.01f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.OutTheGate:
                    skillObj.Name = "Out the Gate";
                    skillObj.Description =
                        "You're an early waker. If leveling was like waking up.\n Gain bonus HP and MP every time you level.";
                    skillObj.PerLevelModifier = 35f;
                    skillObj.BaseCost = 2500;
                    skillObj.Appreciation = 5000;
                    skillObj.MaxLevel = 2;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Perfectionist:
                    skillObj.Name = "Perfectionist";
                    skillObj.Description = "OCD finally comes in handy. \nGain more gold.";
                    skillObj.Position = new Vector2(150f, 50f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Guru:
                    skillObj.Name = "Guru";
                    skillObj.Description = "You are Zen-like. \n Regain endurance faster while still.";
                    skillObj.Position = new Vector2(50f, 50f);
                    skillObj.PerLevelModifier = 5f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.IronLung:
                    skillObj.Name = "Iron Lung";
                    skillObj.Description = "Generic SKILL.  Increase total Endurance.";
                    skillObj.Position = new Vector2(50f, 200f);
                    skillObj.PerLevelModifier = 25f;
                    skillObj.BaseCost = 500;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 50;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.SwordMaster:
                    skillObj.Name = "Sword Master";
                    skillObj.Description = "You fight with finesse \n Attacks Drain X% less endurance.";
                    skillObj.Position = new Vector2(50f, 150f);
                    skillObj.PerLevelModifier = 0.1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 2;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Tank:
                    skillObj.Name = "Tank";
                    skillObj.Description = "Generic SKILL.  Increase Health";
                    skillObj.Position = new Vector2(50f, 200f);
                    skillObj.PerLevelModifier = 25f;
                    skillObj.BaseCost = 500;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 50;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Vampire:
                    skillObj.Name = "Vampire";
                    skillObj.Description = "You suck... Blood. \n Restore a small amount of life with every hit.";
                    skillObj.Position = new Vector2(50f, 250f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.SecondChance:
                    skillObj.Name = "Second Chance";
                    skillObj.Description =
                        "Come back to life, just like Jesus. But you're still not jesus. \n Revive once after dying.";
                    skillObj.Position = new Vector2(50f, 300f);
                    skillObj.PerLevelModifier = 0.25f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.PeaceOfMind:
                    skillObj.Name = "Peace of Mind";
                    skillObj.Description =
                        "Clearing a room is like clearing your mind.  I don't know how. \nRegain helath for every room fully cleared.";
                    skillObj.Position = new Vector2(50f, 250f);
                    skillObj.PerLevelModifier = 10f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.CartographyNinja:
                    skillObj.Name = "Cartography Ninja";
                    skillObj.Description = "Cartography /n Each percentage of map revealed adds 0.1 damage.";
                    skillObj.Position = new Vector2(100f, 50f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.StrongMan:
                    skillObj.Name = "Strong Man";
                    skillObj.Description = "Generic SKILL.  Increase Attack Damage.";
                    skillObj.Position = new Vector2(100f, 50f);
                    skillObj.PerLevelModifier = 2f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 50;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Suicidalist:
                    skillObj.Name = "Suicidalist";
                    skillObj.Description =
                        "You're a very, very sore loser. \n Deal massive damage to all enemies on screen upon death.";
                    skillObj.Position = new Vector2(100f, 100f);
                    skillObj.PerLevelModifier = 100f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.CritBarbarian:
                    skillObj.Name = "Crit Barbarian";
                    skillObj.Description =
                        "You have learned that hitting the balls deals massive damage. \n Crits deal more damage.";
                    skillObj.Position = new Vector2(100f, 150f);
                    skillObj.PerLevelModifier = 0.1f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Magician:
                    skillObj.Name = "Magician";
                    skillObj.Description = "GENERIC SKILL.";
                    skillObj.Position = new Vector2(100f, 250f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 50;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Keymaster:
                    skillObj.Name = "Keymaster";
                    skillObj.Description = "Oh. They were in my back pocket. \nGain 2 extra keys.";
                    skillObj.Position = new Vector2(100f, 300f);
                    skillObj.PerLevelModifier = 2f;
                    skillObj.BaseCost = 2000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.OneTimeOnly:
                    skillObj.Name = "One Time Only";
                    skillObj.Description =
                        "Like a pheonix you are reborn from your crappy ashes. \n Regain all HP and MP.";
                    skillObj.Position = new Vector2(150f, 100f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 100;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.CuttingOutEarly:
                    skillObj.Name = "Cutting Out Early";
                    skillObj.Description =
                        "Retire, and invest your money wisely.  End your game early, and gain a bonus to gold found.";
                    skillObj.Position = new Vector2(150f, 100f);
                    skillObj.PerLevelModifier = 0.25f;
                    skillObj.BaseCost = 100;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Quaffer:
                    skillObj.Name = "Quaffer";
                    skillObj.Description = "CHUG CHUG CHUG! \n Drink potions instantly.";
                    skillObj.Position = new Vector2(150f, 150f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.SpellSword:
                    skillObj.Name = "Spellsword";
                    skillObj.Description =
                        "You were born with absolute power in your fingertips. \nAll spells deal more damage.";
                    skillObj.Position = new Vector2(100f, 200f);
                    skillObj.PerLevelModifier = 5f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Sorcerer:
                    skillObj.Name = "Sorcerer";
                    skillObj.Description =
                        "You were born with arcane energy coarsing through your veins.  Ow. \nSpells cost less to cast.";
                    skillObj.Position = new Vector2(100f, 250f);
                    skillObj.PerLevelModifier = 5f;
                    skillObj.BaseCost = 700;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 5;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.WellEndowed:
                    skillObj.Name = "Well Endowed";
                    skillObj.Description = "By law, you are now the best man. \nGive birth to more children.";
                    skillObj.Position = new Vector2(150f, 100f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 100;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 2;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.TreasureHunter:
                    skillObj.Name = "Treasure Hunter";
                    skillObj.Description =
                        "Your parents said learning how to sift for gold was useless for a farmer.  Whose laughing now? \n Display treasure rooms at the start of the game.";
                    skillObj.Position = new Vector2(150f, 250f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 100;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 2;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.MortarMaster:
                    skillObj.Name = "Mortar Master";
                    skillObj.Description = "War is hell.  Luckily you were never in one. \n Fire more mortars.";
                    skillObj.Position = new Vector2(150f, 300f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 3;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.ExplosiveExpert:
                    skillObj.Name = "Explosive Expert";
                    skillObj.Description =
                        "As a child, you showed an affinity for blowing things up. \n Bombs have a larger radius.";
                    skillObj.Position = new Vector2(200f, 50f);
                    skillObj.PerLevelModifier = 5f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 3;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.Icicle:
                    skillObj.Name = "Icicle ";
                    skillObj.Description =
                        "You're great grandfather was a snowman.  He taught you nothing. \n Icicles pierce through more enemies.";
                    skillObj.Position = new Vector2(200f, 100f);
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.Appreciation = 100;
                    skillObj.MaxLevel = 3;
                    skillObj.IconName = "IconBootLocked_Sprite";
                    break;

                case SkillType.ManorGroundRoad:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 250;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorMainBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 50;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorMainWindowBottom:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 250;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorMainWindowTop:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorMainRoof:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftWingBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftWingWindow:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 800;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftWingRoof:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 800;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftBigBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftBigUpper1:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1250;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftBigUpper2:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftBigWindows:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftBigRoof:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftFarBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1750;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftFarRoof:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftExtension:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftTree1:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorLeftTree2:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightWingBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightWingWindow:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 800;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightWingRoof:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 800;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightBigBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightBigUpper:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1250;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightBigRoof:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightHighBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightHighUpper:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightHighTower:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 3000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightExtension:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorRightTree:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 1500;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorObservatoryBase:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 2000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;

                case SkillType.ManorObservatoryTelescope:
                    skillObj.Name = "Manor Renovation";
                    skillObj.Description =
                        "If you're going to leave your children GENDER, you might as well make sure they have a nice place to live.\n\nEach level unlocks additional items.";
                    skillObj.PerLevelModifier = 1f;
                    skillObj.BaseCost = 3000;
                    skillObj.MaxLevel = 1;
                    skillObj.IconName = "Icon_SmithyLocked_Sprite";
                    skillObj.DisplayStat = true;
                    skillObj.StatType = 0;
                    skillObj.CanPurchase = true;
                    break;
            }

            skillObj.TraitType = skillType;
            return skillObj;
        }

        public static SkillLinker GetSkillLinker(int xIndex, int yIndex)
        {
            var result = default(SkillLinker);
            if (xIndex == 5 && yIndex == 9)
            {
                result.TopLink = new Vector2(5f, 8f);
                result.BottomLink = new Vector2(6f, 9f);
                result.LeftLink = new Vector2(4f, 5f);
                result.RightLink = new Vector2(6f, 5f);
            }
            else if (xIndex == 8 && yIndex == 8)
            {
                result.TopLink = new Vector2(5f, 9f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(2f, 8f);
                result.RightLink = new Vector2(8f, 7f);
            }
            else if (xIndex == 3 && yIndex == 4)
            {
                result.TopLink = new Vector2(2f, 3f);
                result.BottomLink = new Vector2(3f, 6f);
                result.LeftLink = new Vector2(2f, 4f);
                result.RightLink = new Vector2(5f, 6f);
            }
            else if (xIndex == 4 && yIndex == 6)
            {
                result.TopLink = new Vector2(3f, 6f);
                result.BottomLink = new Vector2(4f, 5f);
                result.LeftLink = new Vector2(1f, 5f);
                result.RightLink = new Vector2(5f, 8f);
            }
            else if (xIndex == 2 && yIndex == 8)
            {
                result.TopLink = new Vector2(4f, 5f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(2f, 7f);
                result.RightLink = new Vector2(6f, 9f);
            }
            else if (xIndex == 2 && yIndex == 5)
            {
                result.TopLink = new Vector2(2f, 4f);
                result.BottomLink = new Vector2(2f, 6f);
                result.LeftLink = new Vector2(0f, 5f);
                result.RightLink = new Vector2(3f, 6f);
            }
            else if (xIndex == 1 && yIndex == 5)
            {
                result.TopLink = new Vector2(0f, 5f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(0f, 6f);
                result.RightLink = new Vector2(4f, 6f);
            }
            else if (xIndex == 8 && yIndex == 5)
            {
                result.TopLink = new Vector2(8f, 4f);
                result.BottomLink = new Vector2(7f, 6f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 2 && yIndex == 6)
            {
                result.TopLink = new Vector2(2f, 5f);
                result.BottomLink = new Vector2(2f, 7f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(4f, 5f);
            }
            else if (xIndex == 5 && yIndex == 8)
            {
                result.TopLink = new Vector2(5f, 7f);
                result.BottomLink = new Vector2(5f, 9f);
                result.LeftLink = new Vector2(4f, 6f);
                result.RightLink = new Vector2(6f, 6f);
            }
            else if (xIndex == 5 && yIndex == 6)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(5f, 7f);
                result.LeftLink = new Vector2(3f, 4f);
                result.RightLink = new Vector2(8f, 3f);
            }
            else if (xIndex == 3 && yIndex == 6)
            {
                result.TopLink = new Vector2(3f, 4f);
                result.BottomLink = new Vector2(4f, 6f);
                result.LeftLink = new Vector2(2f, 5f);
                result.RightLink = new Vector2(5f, 7f);
            }
            else if (xIndex == 7 && yIndex == 6)
            {
                result.TopLink = new Vector2(8f, 5f);
                result.BottomLink = new Vector2(6f, 6f);
                result.LeftLink = new Vector2(5f, 7f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 6 && yIndex == 6)
            {
                result.TopLink = new Vector2(7f, 6f);
                result.BottomLink = new Vector2(6f, 5f);
                result.LeftLink = new Vector2(5f, 8f);
                result.RightLink = new Vector2(8f, 6f);
            }
            else if (xIndex == 8 && yIndex == 6)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(8f, 7f);
                result.LeftLink = new Vector2(6f, 6f);
                result.RightLink = new Vector2(9f, 6f);
            }
            else if (xIndex == 8 && yIndex == 4)
            {
                result.TopLink = new Vector2(8f, 1f);
                result.BottomLink = new Vector2(8f, 5f);
                result.LeftLink = new Vector2(8f, 3f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 8 && yIndex == 7)
            {
                result.TopLink = new Vector2(8f, 6f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(6f, 9f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 2 && yIndex == 7)
            {
                result.TopLink = new Vector2(2f, 6f);
                result.BottomLink = new Vector2(2f, 7f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(2f, 8f);
            }
            else if (xIndex == 8 && yIndex == 3)
            {
                result.TopLink = new Vector2(8f, 2f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(5f, 6f);
                result.RightLink = new Vector2(8f, 4f);
            }
            else if (xIndex == 8 && yIndex == 2)
            {
                result.TopLink = new Vector2(7f, 2f);
                result.BottomLink = new Vector2(8f, 3f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(8f, 1f);
            }
            else if (xIndex == 9 && yIndex == 6)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(8f, 6f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 2 && yIndex == 4)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(2f, 5f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(3f, 4f);
            }
            else if (xIndex == 5 && yIndex == 7)
            {
                result.TopLink = new Vector2(5f, 6f);
                result.BottomLink = new Vector2(5f, 8f);
                result.LeftLink = new Vector2(3f, 6f);
                result.RightLink = new Vector2(7f, 6f);
            }
            else if (xIndex == 4 && yIndex == 5)
            {
                result.TopLink = new Vector2(4f, 6f);
                result.BottomLink = new Vector2(2f, 8f);
                result.LeftLink = new Vector2(2f, 6f);
                result.RightLink = new Vector2(5f, 9f);
            }
            else if (xIndex == 6 && yIndex == 5)
            {
                result.TopLink = new Vector2(6f, 6f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(5f, 9f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 2 && yIndex == 3)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(3f, 4f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 7 && yIndex == 2)
            {
                result.TopLink = new Vector2(7f, 1f);
                result.BottomLink = new Vector2(8f, 2f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 7 && yIndex == 1)
            {
                result.BottomLink = new Vector2(7f, 2f);
                result.TopLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(-1f, -1f);
            }
            else if (xIndex == 0 && yIndex == 5)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(1f, 5f);
                result.LeftLink = new Vector2(0f, 6f);
                result.RightLink = new Vector2(2f, 5f);
            }
            else if (xIndex == 0 && yIndex == 6)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(-11f, -1f);
                result.LeftLink = new Vector2(-1f, -1f);
                result.RightLink = new Vector2(0f, 5f);
            }
            else if (xIndex == 6 && yIndex == 9)
            {
                result.TopLink = new Vector2(5f, 9f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(2f, 8f);
                result.RightLink = new Vector2(8f, 7f);
            }
            else if (xIndex == 8 && yIndex == 1)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(8f, 4f);
                result.LeftLink = new Vector2(8f, 2f);
                result.RightLink = new Vector2(8f, 0f);
            }
            else if (xIndex == 8 && yIndex == 0)
            {
                result.TopLink = new Vector2(-1f, -1f);
                result.BottomLink = new Vector2(-1f, -1f);
                result.LeftLink = new Vector2(8f, 1f);
                result.RightLink = new Vector2(-1f, -1f);
            }

            return result;
        }
    }
}
