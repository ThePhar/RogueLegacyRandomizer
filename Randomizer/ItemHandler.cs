//  RogueLegacyRandomizer - ItemHandler.cs
//  Last Modified 2023-10-24 4:07 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer.Definitions;
using RogueLegacy;
using RogueLegacy.Enums;
using Game = RogueLegacy.Game;

namespace Randomizer;

public class ItemHandler
{
    public Dictionary<int, NetworkItem> ReceivedItems { get; set; } = new();

    private static PlayerObj          Player  => Game.ScreenManager.Player;
    private static ArchipelagoManager Manager => Program.Game.ArchipelagoManager;

    public void CheckReceivedItemQueue()
    {
        if (Manager.ItemQueue.Count == 0)
        {
            return;
        }

        var (index, item) = Manager.ItemQueue.Dequeue();
        if (!HasReceivedItem(index))
        {
            GainItem(item);
            ReceivedItems.Add(index, item);
            Program.Game.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
        }
    }

    private bool HasReceivedItem(int index)
    {
        return ReceivedItems.ContainsKey(index);
    }

    private static void GainItem(NetworkItem item)
    {
        var stats = new float[] { -1, -1, -1 };
        switch (item.Item)
        {
            #region Vendors

            case ItemCode.BLACKSMITH:
                IncreaseSkillLevel(SkillType.Smithy);
                break;

            case ItemCode.ENCHANTRESS:
                IncreaseSkillLevel(SkillType.Enchanter);
                break;

            case ItemCode.ARCHITECT:
                IncreaseSkillLevel(SkillType.Architect);
                break;

            #endregion

            #region Classes

            case ItemCode.PROGRESSIVE_KNIGHT:
                IncreaseClassLevel(SkillType.KnightUnlock, SkillType.KnightUp);
                break;

            case ItemCode.PROGRESSIVE_MAGE:
                IncreaseClassLevel(SkillType.MageUnlock, SkillType.MageUp);
                break;

            case ItemCode.PROGRESSIVE_BARBARIAN:
                IncreaseClassLevel(SkillType.BarbarianUnlock, SkillType.BarbarianUp);
                break;

            case ItemCode.PROGRESSIVE_KNAVE:
                IncreaseClassLevel(SkillType.AssassinUnlock, SkillType.AssassinUp);
                break;

            case ItemCode.PROGRESSIVE_SHINOBI:
                IncreaseClassLevel(SkillType.NinjaUnlock, SkillType.NinjaUp);
                break;

            case ItemCode.PROGRESSIVE_MINER:
                IncreaseClassLevel(SkillType.BankerUnlock, SkillType.BankerUp);
                break;

            case ItemCode.PROGRESSIVE_LICH:
                IncreaseClassLevel(SkillType.LichUnlock, SkillType.LichUp);
                break;

            case ItemCode.PROGRESSIVE_SPELLTHIEF:
                IncreaseClassLevel(SkillType.SpellswordUnlock, SkillType.SpellSwordUp);
                break;

            case ItemCode.DRAGON:
                IncreaseSkillLevel(SkillType.SuperSecret);
                break;

            case ItemCode.TRAITOR:
                IncreaseSkillLevel(SkillType.Traitorous);
                break;

            #endregion

            #region Skills

            case ItemCode.HEALTH:
            {
                var previousMaxHealth = Player.MaxHealth;
                IncreaseSkillLevel(SkillType.HealthUp, 5);
                Player.CurrentHealth += Player.MaxHealth - previousMaxHealth;
                break;
            }

            case ItemCode.MANA:
            {
                var previousMaxMana = Player.MaxMana;
                IncreaseSkillLevel(SkillType.ManaUp, 5);
                Player.CurrentMana += Player.MaxMana - previousMaxMana;
                break;
            }

            case ItemCode.ATTACK:
                IncreaseSkillLevel(SkillType.AttackUp, 5);
                break;

            case ItemCode.MAGIC_DAMAGE:
                IncreaseSkillLevel(SkillType.MagicDamageUp, 5);
                break;

            case ItemCode.ARMOR:
                IncreaseSkillLevel(SkillType.ArmorUp, 5);
                break;

            case ItemCode.EQUIP:
                IncreaseSkillLevel(SkillType.EquipUp, 5);
                break;

            case ItemCode.CRIT_CHANCE:
                IncreaseSkillLevel(SkillType.CritChanceUp, 5);
                break;

            case ItemCode.CRIT_DAMAGE:
                IncreaseSkillLevel(SkillType.CritDamageUp, 5);
                break;

            case ItemCode.DOWN_STRIKE:
                IncreaseSkillLevel(SkillType.DownStrikeUp, 5);
                break;

            case ItemCode.GOLD_GAIN:
                IncreaseSkillLevel(SkillType.GoldGainUp, 5);
                break;

            case ItemCode.POTION_EFFICIENCY:
                IncreaseSkillLevel(SkillType.PotionUp, 5);
                break;

            case ItemCode.INVULN_TIME:
                IncreaseSkillLevel(SkillType.InvulnTimeUp, 5);
                break;

            case ItemCode.MANA_COST_DOWN:
                IncreaseSkillLevel(SkillType.ManaCostDown, 5);
                break;

            case ItemCode.DEATH_DEFIANCE:
                IncreaseSkillLevel(SkillType.DeathDodge, 5);
                break;

            case ItemCode.HAGGLING:
                IncreaseSkillLevel(SkillType.PricesDown, 5);
                break;

            case ItemCode.RANDOMIZE_CHILDREN:
                IncreaseSkillLevel(SkillType.RandomChildren);
                break;

            #endregion

            #region SlotGains

            case ItemCode.BLACKSMITH_SWORD:
                IncreaseSkillLevel(SkillType.BlacksmithSword);
                break;

            case ItemCode.BLACKSMITH_HELM:
                IncreaseSkillLevel(SkillType.BlacksmithHelm);
                break;

            case ItemCode.BLACKSMITH_CHEST:
                IncreaseSkillLevel(SkillType.BlacksmithChest);
                break;

            case ItemCode.BLACKSMITH_LIMBS:
                IncreaseSkillLevel(SkillType.BlacksmithLimbs);
                break;

            case ItemCode.BLACKSMITH_CAPE:
                IncreaseSkillLevel(SkillType.BlacksmithCape);
                break;

            case ItemCode.ENCHANTRESS_SWORD:
                IncreaseSkillLevel(SkillType.EnchantressSword);
                break;

            case ItemCode.ENCHANTRESS_HELM:
                IncreaseSkillLevel(SkillType.EnchantressHelm);
                break;

            case ItemCode.ENCHANTRESS_CHEST:
                IncreaseSkillLevel(SkillType.EnchantressChest);
                break;

            case ItemCode.ENCHANTRESS_LIMBS:
                IncreaseSkillLevel(SkillType.EnchantressLimbs);
                break;

            case ItemCode.ENCHANTRESS_CAPE:
                IncreaseSkillLevel(SkillType.EnchantressCape);
                break;

            #endregion

            #region Runes

            case ItemCode.RUNE_VAULT:
                UnlockRune(EquipmentAbility.Vault);
                break;

            case ItemCode.RUNE_SPRINT:
                UnlockRune(EquipmentAbility.Sprint);
                break;

            case ItemCode.RUNE_VAMPIRE:
                UnlockRune(EquipmentAbility.Vampire);
                break;

            case ItemCode.RUNE_SKY:
                UnlockRune(EquipmentAbility.Sky);
                break;

            case ItemCode.RUNE_SIPHON:
                UnlockRune(EquipmentAbility.Siphon);
                break;

            case ItemCode.RUNE_RETALIATION:
                UnlockRune(EquipmentAbility.Retaliation);
                break;

            case ItemCode.RUNE_BOUNTY:
                UnlockRune(EquipmentAbility.Bounty);
                break;

            case ItemCode.RUNE_HASTE:
                UnlockRune(EquipmentAbility.Haste);
                break;

            case ItemCode.RUNE_CURSE:
                UnlockRune(EquipmentAbility.Curse);
                break;

            case ItemCode.RUNE_GRACE:
                UnlockRune(EquipmentAbility.Grace);
                break;

            case ItemCode.RUNE_BALANCE:
                UnlockRune(EquipmentAbility.Balance);
                break;

            #endregion

            #region Equipment

            case ItemCode.EQUIPMENT_PROGRESSIVE:
                UnlockProgressiveEquipment();
                break;

            case ItemCode.EQUIPMENT_SQUIRE:
                UnlockEquipment(EquipmentBase.Squire);
                break;

            case ItemCode.EQUIPMENT_SILVER:
                UnlockEquipment(EquipmentBase.Silver);
                break;

            case ItemCode.EQUIPMENT_GUARDIAN:
                UnlockEquipment(EquipmentBase.Guardian);
                break;

            case ItemCode.EQUIPMENT_IMPERIAL:
                UnlockEquipment(EquipmentBase.Imperial);
                break;

            case ItemCode.EQUIPMENT_ROYAL:
                UnlockEquipment(EquipmentBase.Royal);
                break;

            case ItemCode.EQUIPMENT_KNIGHT:
                UnlockEquipment(EquipmentBase.Knight);
                break;

            case ItemCode.EQUIPMENT_RANGER:
                UnlockEquipment(EquipmentBase.Ranger);
                break;

            case ItemCode.EQUIPMENT_SKY:
                UnlockEquipment(EquipmentBase.Sky);
                break;

            case ItemCode.EQUIPMENT_DRAGON:
                UnlockEquipment(EquipmentBase.Dragon);
                break;

            case ItemCode.EQUIPMENT_SLAYER:
                UnlockEquipment(EquipmentBase.Slayer);
                break;

            case ItemCode.EQUIPMENT_BLOOD:
                UnlockEquipment(EquipmentBase.Blood);
                break;

            case ItemCode.EQUIPMENT_SAGE:
                UnlockEquipment(EquipmentBase.Sage);
                break;

            case ItemCode.EQUIPMENT_RETRIBUTION:
                UnlockEquipment(EquipmentBase.Retribution);
                break;

            case ItemCode.EQUIPMENT_HOLY:
                UnlockEquipment(EquipmentBase.Holy);
                break;

            case ItemCode.EQUIPMENT_DARK:
                UnlockEquipment(EquipmentBase.Dark);
                break;

            #endregion

            #region Filler

            case ItemCode.GOLD_1000:
                Game.PlayerStats.Gold += 1000;
                break;

            case ItemCode.GOLD_3000:
                Game.PlayerStats.Gold += 3000;
                break;

            case ItemCode.GOLD_5000:
                Game.PlayerStats.Gold += 5000;
                break;

            case ItemCode.TRIP_STAT_INCREASE:
                for (var i = 0; i < 3; i++)
                {
                    var stat = CDGMath.RandomInt(4, 9);
                    stats[i] = stat;
                    switch (stat)
                    {
                        case 4:
                            Game.PlayerStats.BonusStrength++;
                            continue;
                        case 5:
                            Game.PlayerStats.BonusMagic++;
                            continue;
                        case 6:
                            Game.PlayerStats.BonusDefense++;
                            continue;
                        case 7:
                            var previousMaxHealth = Player.MaxHealth;
                            Game.PlayerStats.BonusHealth++;
                            Player.CurrentHealth += Player.MaxHealth - previousMaxHealth;
                            continue;
                        case 8:
                            var previousMaxMana = Player.MaxMana;
                            Game.PlayerStats.BonusMana++;
                            Player.CurrentMana += Player.MaxMana - previousMaxMana;
                            continue;
                        case 9:
                            Game.PlayerStats.BonusWeight++;
                            continue;
                    }
                }

                break;

            #endregion

            #region Traps

            case ItemCode.TRAP_HEDGEHOG:
                Game.PlayerStats.SpecialItem = (byte) SpecialItemType.HedgehogsCurse;
                Game.ScreenManager.GetLevelScreen().UpdatePlayerHUDSpecialItem();
                break;

            case ItemCode.TRAP_VERTIGO:
                Game.PlayerStats.HasVertigo = !Game.PlayerStats.HasVertigo;
                break;

            case ItemCode.TRAP_GENETIC_LOTTERY:
                Game.PlayerStats.Class = (byte) ClassExtensions.RandomClass();

                var spells = ((ClassType) Game.PlayerStats.Class).SpellList();
                Game.PlayerStats.Spell = (byte) spells[CDGMath.RandomInt(0, spells.Length - 1)];

                var traits = TraitHelper.ReturnRandomTraits();
                Game.PlayerStats.Traits = new Vector2((float) traits[0], (float) traits[1]);

                var currentlyFemale = Game.PlayerStats.IsFemale;
                Game.PlayerStats.IsFemale = CDGMath.RandomInt(0, 1) == 0;
                if (currentlyFemale != Game.PlayerStats.IsFemale)
                {
                    Game.PlayerStats.PlayerName = Game.PlayerStats.IsFemale
                        ? Game.PlayerStats.PlayerName.Replace("Sir ", "Lady ")
                        : Game.PlayerStats.PlayerName.Replace("Lady ", "Sir ");
                }
                break;

            #endregion

            case ItemCode.FOUNTAIN_PIECE:
                Game.PlayerStats.FountainPieces += 1;
                break;
        }

        // Add item to received items HUD.
        var tupleStats = new Tuple<float, float, float, float>(stats[0], stats[1], stats[2], 0);
        Game.ScreenManager.GetLevelScreen().AddReceivedItem(
            item.Item.GetItemType(),
            item.Item,
            Manager.GetPlayerName(item.Player),
            tupleStats
        );
    }

    private static void IncreaseSkillLevel(SkillType skillType, int levels = 1)
    {
        // Ensure we can level up skill.
        var skill = SkillSystem.GetSkill(skillType);
        skill.CanPurchase = true;

        skill.MaxLevel += levels;
        if (skill.MaxLevel > skill.MaxMaxLevel)
        {
            skill.MaxLevel = skill.MaxMaxLevel;
        }

        for (var i = 0; i < levels; i++)
        {
            if (!RandomizerData.RequireSkillPurchasing || !SkillSystem.IsSkillScreenSkill(skill.Trait))
            {
                SkillSystem.LevelUpTrait(skill, false);
            }
        }
    }

    private static void IncreaseClassLevel(SkillType baseClass, SkillType upgradeClass)
    {
        var baseSkill = SkillSystem.GetSkill(baseClass);
        IncreaseSkillLevel(baseSkill.CurrentLevel > 0 ? upgradeClass : baseClass);
    }

    private static void UnlockRune(EquipmentAbility ability)
    {
        var unlockStatus = (byte) (RandomizerData.RequireVendorPurchasing ? 1 : 3);
        Game.PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) ability] = unlockStatus;
        Game.PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) ability] = unlockStatus;
        Game.PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) ability] = unlockStatus;
        Game.PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) ability] = unlockStatus;
        Game.PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) ability] = unlockStatus;
    }

    private static void UnlockEquipment(EquipmentBase @base)
    {
        var unlockStatus = (byte) (RandomizerData.RequireVendorPurchasing ? 1 : 3);
        Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) @base] = unlockStatus;
        Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) @base] = unlockStatus;
        Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) @base] = unlockStatus;
        Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) @base] = unlockStatus;
        Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) @base] = unlockStatus;
    }

    private static void UnlockProgressiveEquipment()
    {
        var unlockStatus = (byte) (RandomizerData.RequireVendorPurchasing ? 1 : 3);
        var equipmentOrder = new[]
        {
            EquipmentBase.Squire,
            EquipmentBase.Knight,
            EquipmentBase.Blood,
            EquipmentBase.Silver,
            EquipmentBase.Ranger,
            EquipmentBase.Sage,
            EquipmentBase.Guardian,
            EquipmentBase.Sky,
            EquipmentBase.Retribution,
            EquipmentBase.Imperial,
            EquipmentBase.Dragon,
            EquipmentBase.Holy,
            EquipmentBase.Royal,
            EquipmentBase.Slayer,
            EquipmentBase.Dark,
        };

        foreach (var equipment in equipmentOrder)
        {
            if (Game.PlayerStats.GetBlueprintArray[0][(int) equipment] != 0)
            {
                continue;
            }

            Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) equipment] = unlockStatus;
            Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) equipment] = unlockStatus;
            Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) equipment] = unlockStatus;
            Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) equipment] = unlockStatus;
            Game.PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) equipment] = unlockStatus;
            break;
        }
    }
}
