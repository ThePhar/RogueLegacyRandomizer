// 
// RogueLegacyArchipelago - ArchClient.cs
// Last Modified 2021-12-26
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.TypeDefinitions;

namespace RogueCastle.Archipelago
{
    public class ArchClient
    {
        private ArchipelagoSession m_session;
        private DeathLinkService m_deathLinkService;
        private string m_seed;
        private NetworkPlayer m_player;
        private SlotData m_data;
        private List<string> m_tags;
        private List<int> m_missingLocations;
        private List<int> m_checkedLocations;

        public bool IsConnected
        {
            get
            {
                return m_session != null && m_session.Socket.Connected;
            }
        }

        public int Slot { get; private set; }

        public Dictionary<int, bool> CheckedLocations { get; set; }
        public Dictionary<int, NetworkItem> LocationCache { get; set; }
        public ArchipelagoSession Session
        {
            get { return m_session; }
        }

        /// <summary>
        /// Attempts to create a new session and connect
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="slotName"></param>
        /// <param name="password"></param>
        public void Connect(string hostname, int port, string slotName, string password = null)
        {
            // Disconnect our current session if we are connected.
            if (IsConnected)
                m_session.Socket.Disconnect();

            try
            {
                m_session = ArchipelagoSessionFactory.CreateSession(hostname, port);

                // Establish handlers.
                m_session.Socket.PacketReceived += PacketReceivedHandler;
                m_session.Items.ItemReceived += (packet) =>
                {
                    var item = packet.PeekItem();

                    SkillObj skill;
                    switch (item.Item)
                    {
                        // SKILLS
                        case 4444000: // Smithy
                            skill = SkillSystem.GetSkill(SkillType.Smithy);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444001: // Architect
                            skill = SkillSystem.GetSkill(SkillType.Architect);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444002: // Enchantress
                            skill = SkillSystem.GetSkill(SkillType.Enchanter);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444003: // Progressive Knight
                            skill = SkillSystem.GetSkill(SkillType.KnightUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444004: // Progressive Mage
                            skill = SkillSystem.GetSkill(SkillType.MageUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444005: // Progressive Barbarian
                            skill = SkillSystem.GetSkill(SkillType.BarbarianUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444006: // Progressive Knave
                            skill = SkillSystem.GetSkill(SkillType.AssassinUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444007: // Progressive Shinobi
                            skill = SkillSystem.GetSkill(SkillType.NinjaUnlock);
                            if (skill.CurrentLevel > 0)
                            {
                                skill = SkillSystem.GetSkill(SkillType.NinjaUp);
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }
                            else
                            {
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }

                            break;
                        case 4444008: // Progressive Miner
                            skill = SkillSystem.GetSkill(SkillType.BankerUnlock);
                            if (skill.CurrentLevel > 0)
                            {
                                skill = SkillSystem.GetSkill(SkillType.BankerUp);
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }
                            else
                            {
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }

                            break;
                        case 4444009: // Progressive Lich
                            skill = SkillSystem.GetSkill(SkillType.LichUnlock);
                            if (skill.CurrentLevel > 0)
                            {
                                skill = SkillSystem.GetSkill(SkillType.LichUp);
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }
                            else
                            {
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }

                            break;
                        case 4444010: // Progressive Spell Thief
                            skill = SkillSystem.GetSkill(SkillType.SpellswordUnlock);
                            if (skill.CurrentLevel > 0)
                            {
                                skill = SkillSystem.GetSkill(SkillType.SpellSwordUp);
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }
                            else
                            {
                                skill.CanPurchase = true;
                                SkillSystem.LevelUpTrait(skill, false);
                            }

                            break;
                        case 4444011: // Dragon
                            skill = SkillSystem.GetSkill(SkillType.SuperSecret);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444012: // Traitor
                            // TODO: Implement!
                            break;
                        case 4444013: // Health Up
                            skill = SkillSystem.GetSkill(SkillType.HealthUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444014: // Mana Up
                            skill = SkillSystem.GetSkill(SkillType.ManaUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444015: // Attack Up
                            skill = SkillSystem.GetSkill(SkillType.AttackUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444016: // Magic Damage Up
                            skill = SkillSystem.GetSkill(SkillType.MagicDamageUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444017: // Armor Up
                            skill = SkillSystem.GetSkill(SkillType.ArmorUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444018: // Equip Up
                            skill = SkillSystem.GetSkill(SkillType.EquipUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444019: // Crit Chance Up
                            skill = SkillSystem.GetSkill(SkillType.CritChanceUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444020: // Crit Damage Up
                            skill = SkillSystem.GetSkill(SkillType.CritDamageUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444021: // Down Strike Up
                            skill = SkillSystem.GetSkill(SkillType.DownStrikeUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444022: // Gold Gain Up
                            skill = SkillSystem.GetSkill(SkillType.GoldGainUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444023: // Potion Up
                            skill = SkillSystem.GetSkill(SkillType.PotionUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444024: // Invuln Time  Up
                            skill = SkillSystem.GetSkill(SkillType.InvulnerabilityTimeUp);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444025: // Mana Cost Down
                            skill = SkillSystem.GetSkill(SkillType.ManaCostDown);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444026: // Death Defy
                            skill = SkillSystem.GetSkill(SkillType.DeathDodge);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444027: // Haggle
                            skill = SkillSystem.GetSkill(SkillType.PricesDown);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;
                        case 4444028: // Randomize Children
                            skill = SkillSystem.GetSkill(SkillType.RandomizeChildren);
                            skill.CanPurchase = true;
                            SkillSystem.LevelUpTrait(skill, false);
                            break;

                        // RUNES
                        case 4444032: // Sprint (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.Dash] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.Dash] = 1;
                            break;
                        case 4444033: // Sprint (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.Dash] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.Dash] = 1;
                            break;
                        case 4444034: // Sprint (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.Dash] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.Dash] = 1;
                            break;
                        case 4444035: // Sprint (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.Dash] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.Dash] = 1;
                            break;
                        case 4444036: // Sprint (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.Dash] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.Dash] = 1;
                            break;
                        case 4444037: // Vault (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.DoubleJump] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.DoubleJump] = 1;
                            break;
                        case 4444038: // Vault (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.DoubleJump] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.DoubleJump] = 1;
                            break;
                        case 4444039: // Vault (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.DoubleJump] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.DoubleJump] = 1;
                            break;
                        case 4444040: // Vault (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.DoubleJump] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.DoubleJump] = 1;
                            break;
                        case 4444041: // Vault (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.DoubleJump] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.DoubleJump] = 1;
                            break;
                        case 4444042: // Bounty (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.GoldGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.GoldGain] = 1;
                            break;
                        case 4444043: // Bounty (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.GoldGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.GoldGain] = 1;
                            break;
                        case 4444044: // Bounty (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.GoldGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.GoldGain] = 1;
                            break;
                        case 4444045: // Bounty (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.GoldGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.GoldGain] = 1;
                            break;
                        case 4444046: // Bounty (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.GoldGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.GoldGain] = 1;
                            break;
                        case 4444047: // Siphon (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.ManaGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.ManaGain] = 1;
                            break;
                        case 4444048: // Siphon (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.ManaGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.ManaGain] = 1;
                            break;
                        case 4444049: // Siphon (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.ManaGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.ManaGain] = 1;
                            break;
                        case 4444050: // Siphon (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.ManaGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.ManaGain] = 1;
                            break;
                        case 4444051: // Siphon (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.ManaGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.ManaGain] = 1;
                            break;
                        case 4444052: // Retaliation (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.DamageReturn] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.DamageReturn] = 1;
                            break;
                        case 4444053: // Retaliation (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.DamageReturn] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.DamageReturn] = 1;
                            break;
                        case 4444054: // Retaliation (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.DamageReturn] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.DamageReturn] = 1;
                            break;
                        case 4444055: // Retaliation (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.DamageReturn] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.DamageReturn] = 1;
                            break;
                        case 4444056: // Retaliation (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.DamageReturn] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.DamageReturn] = 1;
                            break;
                        case 4444057: // Grace (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.RoomLevelDown] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.RoomLevelDown] = 1;
                            break;
                        case 4444058: // Grace (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.RoomLevelDown] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.RoomLevelDown] = 1;
                            break;
                        case 4444059: // Grace (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.RoomLevelDown] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.RoomLevelDown] = 1;
                            break;
                        case 4444060: // Grace (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.RoomLevelDown] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.RoomLevelDown] = 1;
                            break;
                        case 4444061: // Grace (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.RoomLevelDown] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.RoomLevelDown] = 1;
                            break;
                        case 4444062: // Balance (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.ManaHpGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.ManaHpGain] = 1;
                            break;
                        case 4444063: // Balance (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.ManaHpGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.ManaHpGain] = 1;
                            break;
                        case 4444064: // Balance (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.ManaHpGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.ManaHpGain] = 1;
                            break;
                        case 4444065: // Balance (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.ManaHpGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.ManaHpGain] = 1;
                            break;
                        case 4444066: // Balance (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.ManaHpGain] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.ManaHpGain] = 1;
                            break;
                        case 4444067: // Curse (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.RoomLevelUp] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.RoomLevelUp] = 1;
                            break;
                        case 4444068: // Curse (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.RoomLevelUp] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.RoomLevelUp] = 1;
                            break;
                        case 4444069: // Curse (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.RoomLevelUp] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.RoomLevelUp] = 1;
                            break;
                        case 4444070: // Curse (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.RoomLevelUp] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.RoomLevelUp] = 1;
                            break;
                        case 4444071: // Curse (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.RoomLevelUp] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.RoomLevelUp] = 1;
                            break;
                        case 4444072: // Vampire (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.Vampirism] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.Vampirism] = 1;
                            break;
                        case 4444073: // Vampire (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.Vampirism] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.Vampirism] = 1;
                            break;
                        case 4444074: // Vampire (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.Vampirism] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.Vampirism] = 1;
                            break;
                        case 4444075: // Vampire (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.Vampirism] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.Vampirism] = 1;
                            break;
                        case 4444076: // Vampire (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.Vampirism] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.Vampirism] = 1;
                            break;
                        case 4444077: // Sky (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.Flight] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.Flight] = 1;
                            break;
                        case 4444078: // Sky (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.Flight] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.Flight] = 1;
                            break;
                        case 4444079: // Sky (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.Flight] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.Flight] = 1;
                            break;
                        case 4444080: // Sky (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.Flight] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.Flight] = 1;
                            break;
                        case 4444081: // Sky (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.Flight] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.Flight] = 1;
                            break;
                        case 4444082: // Haste (Sword)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.MovementSpeed] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Sword][EquipmentAbilityType.MovementSpeed] = 1;
                            break;
                        case 4444083: // Haste (Helm)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.MovementSpeed] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Helm][EquipmentAbilityType.MovementSpeed] = 1;
                            break;
                        case 4444084: // Haste (Chest)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.MovementSpeed] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Chest][EquipmentAbilityType.MovementSpeed] = 1;
                            break;
                        case 4444085: // Haste (Limbs)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.MovementSpeed] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Limbs][EquipmentAbilityType.MovementSpeed] = 1;
                            break;
                        case 4444086: // Haste (Cape)
                            if (Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.MovementSpeed] < 1)
                                Game.PlayerStats.GetRuneArray[EquipmentCategoryType.Cape][EquipmentAbilityType.MovementSpeed] = 1;
                            break;

                        // Blueprints
                        case 4444087: // Squire Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Squire] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Squire] = 1;
                            break;
                        case 4444088: // Knight Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Knight] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Knight] = 1;
                            break;
                        case 4444089: // Blood Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Blood] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Blood] = 1;
                            break;
                        case 4444090: // Silver Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Silver] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Silver] = 1;
                            break;
                        case 4444091: // Ranger Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Ranger] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Ranger] = 1;
                            break;
                        case 4444092: // Sage Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Sage] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Sage] = 1;
                            break;
                        case 4444093: // Guardian Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Guardian] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Guardian] = 1;
                            break;
                        case 4444094: // Sky Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Sky] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Sky] = 1;
                            break;
                        case 4444095: // Retribution Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Retribution] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Retribution] = 1;
                            break;
                        case 4444096: // Imperial Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Imperial] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Imperial] = 1;
                            break;
                        case 4444097: // Dragon Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Dragon] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Dragon] = 1;
                            break;
                        case 4444098: // Holy Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Holy] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Holy] = 1;
                            break;
                        case 4444099: // Royal Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Royal] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Royal] = 1;
                            break;
                        case 4444100: // Slayer Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Slayer] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Slayer] = 1;
                            break;
                        case 4444101: // Dark Sword
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Dark] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Sword][EquipmentBaseType.Dark] = 1;
                            break;
                        case 4444102: // Squire Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Squire] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Squire] = 1;
                            break;
                        case 4444103: // Knight Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Knight] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Knight] = 1;
                            break;
                        case 4444104: // Blood Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Blood] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Blood] = 1;
                            break;
                        case 4444105: // Silver Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Silver] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Silver] = 1;
                            break;
                        case 4444106: // Ranger Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Ranger] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Ranger] = 1;
                            break;
                        case 4444107: // Sage Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Sage] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Sage] = 1;
                            break;
                        case 4444108: // Guardian Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Guardian] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Guardian] = 1;
                            break;
                        case 4444109: // Sky Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Sky] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Sky] = 1;
                            break;
                        case 4444110: // Retribution Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Retribution] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Retribution] = 1;
                            break;
                        case 4444111: // Imperial Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Imperial] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Imperial] = 1;
                            break;
                        case 4444112: // Dragon Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Dragon] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Dragon] = 1;
                            break;
                        case 4444113: // Holy Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Holy] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Holy] = 1;
                            break;
                        case 4444114: // Royal Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Royal] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Royal] = 1;
                            break;
                        case 4444115: // Slayer Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Slayer] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Slayer] = 1;
                            break;
                        case 4444116: // Dark Helm
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Dark] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Helm][EquipmentBaseType.Dark] = 1;
                            break;
                        case 4444117: // Squire Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Squire] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Squire] = 1;
                            break;
                        case 4444118: // Knight Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Knight] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Knight] = 1;
                            break;
                        case 4444119: // Blood Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Blood] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Blood] = 1;
                            break;
                        case 4444120: // Silver Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Silver] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Silver] = 1;
                            break;
                        case 4444121: // Ranger Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Ranger] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Ranger] = 1;
                            break;
                        case 4444122: // Sage Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Sage] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Sage] = 1;
                            break;
                        case 4444123: // Guardian Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Guardian] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Guardian] = 1;
                            break;
                        case 4444124: // Sky Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Sky] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Sky] = 1;
                            break;
                        case 4444125: // Retribution Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Retribution] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Retribution] = 1;
                            break;
                        case 4444126: // Imperial Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Imperial] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Imperial] = 1;
                            break;
                        case 4444127: // Dragon Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Dragon] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Dragon] = 1;
                            break;
                        case 4444128: // Holy Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Holy] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Holy] = 1;
                            break;
                        case 4444129: // Royal Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Royal] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Royal] = 1;
                            break;
                        case 4444130: // Slayer Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Slayer] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Slayer] = 1;
                            break;
                        case 4444131: // Dark Chest
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Dark] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Chest][EquipmentBaseType.Dark] = 1;
                            break;
                        case 4444132: // Squire Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Squire] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Squire] = 1;
                            break;
                        case 4444133: // Knight Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Knight] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Knight] = 1;
                            break;
                        case 4444134: // Blood Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Blood] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Blood] = 1;
                            break;
                        case 4444135: // Silver Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Silver] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Silver] = 1;
                            break;
                        case 4444136: // Ranger Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Ranger] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Ranger] = 1;
                            break;
                        case 4444137: // Sage Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Sage] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Sage] = 1;
                            break;
                        case 4444138: // Guardian Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Guardian] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Guardian] = 1;
                            break;
                        case 4444139: // Sky Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Sky] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Sky] = 1;
                            break;
                        case 4444140: // Retribution Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Retribution] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Retribution] = 1;
                            break;
                        case 4444141: // Imperial Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Imperial] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Imperial] = 1;
                            break;
                        case 4444142: // Dragon Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Dragon] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Dragon] = 1;
                            break;
                        case 4444143: // Holy Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Holy] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Holy] = 1;
                            break;
                        case 4444144: // Royal Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Royal] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Royal] = 1;
                            break;
                        case 4444145: // Slayer Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Slayer] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Slayer] = 1;
                            break;
                        case 4444146: // Dark Limbs
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Dark] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Limbs][EquipmentBaseType.Dark] = 1;
                            break;
                        case 4444147: // Squire Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Squire] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Squire] = 1;
                            break;
                        case 4444148: // Knight Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Knight] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Knight] = 1;
                            break;
                        case 4444149: // Blood Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Blood] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Blood] = 1;
                            break;
                        case 4444150: // Silver Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Silver] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Silver] = 1;
                            break;
                        case 4444151: // Ranger Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Ranger] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Ranger] = 1;
                            break;
                        case 4444152: // Sage Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Sage] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Sage] = 1;
                            break;
                        case 4444153: // Guardian Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Guardian] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Guardian] = 1;
                            break;
                        case 4444154: // Sky Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Sky] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Sky] = 1;
                            break;
                        case 4444155: // Retribution Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Retribution] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Retribution] = 1;
                            break;
                        case 4444156: // Imperial Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Imperial] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Imperial] = 1;
                            break;
                        case 4444157: // Dragon Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Dragon] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Dragon] = 1;
                            break;
                        case 4444158: // Holy Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Holy] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Holy] = 1;
                            break;
                        case 4444159: // Royal Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Royal] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Royal] = 1;
                            break;
                        case 4444160: // Slayer Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Slayer] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Slayer] = 1;
                            break;
                        case 4444161: // Dark Cape
                            if (Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Dark] < 1)
                                Game.PlayerStats.GetBlueprintArray[EquipmentCategoryType.Cape][EquipmentBaseType.Dark] = 1;
                            break;

                        // Extra
                        // {"Random Stat Increase", 4444029},
                        // {"Random Triple Stat Increase", 4444030},
                        // {"Gold Bonus", 4444031},

                        case 4444029: // Single Stat
                        case 4444030: // Trip Stats
                            var iterations = item.Item == 4444029 ? 1 : 3;
                            for (var i = 0; i < iterations; i++)
                            {
                                var randomInt = CDGMath.RandomInt(1, 100);
                                var chance = 0;
                                var j = 0;
                                while (j < GameEV.STATDROP_CHANCE.Length)
                                {
                                    chance += GameEV.STATDROP_CHANCE[j];
                                    if (randomInt <= chance)
                                    {
                                        if (j == 0)
                                        {
                                            Game.PlayerStats.BonusStrength++;
                                            break;
                                        }

                                        if (j == 1)
                                        {
                                            Game.PlayerStats.BonusMagic++;
                                            break;
                                        }

                                        if (j == 2)
                                        {
                                            Game.PlayerStats.BonusDefense++;
                                            break;
                                        }

                                        if (j == 3)
                                        {
                                            Game.PlayerStats.BonusHealth++;
                                            break;
                                        }

                                        if (j == 4)
                                        {
                                            Game.PlayerStats.BonusMana++;
                                            break;
                                        }

                                        Game.PlayerStats.BonusWeight++;
                                        break;
                                    }

                                    j++;
                                }
                            }

                            break;

                        case 4444031: // Gold
                            var gold = CDGMath.RandomInt(500, 5000);
                            Game.PlayerStats.Gold += gold;
                            break;
                    }

                    packet.DequeueItem();
                };

                // Attempt to connect to AP.
                var result = m_session.TryConnectAndLogin("Rogue Legacy", slotName, LevelEV.AP_VERSION, m_tags, password: password);

                // Send a GetDataPackage packet to update our local cache of item names.
                m_session.Socket.SendPacket(new GetDataPackagePacket());

                // Error checking.
                if (!result.Successful)
                {
                    var failure = (LoginFailure) result;
                    var screenManager = Game.ScreenManager;
                    var errorUuid = Guid.NewGuid().ToString();

                    // Add why we failed to connect.
                    Console.WriteLine("Failed to connect to server.");
                    DialogueManager.AddText(errorUuid, Enumerable.Repeat("Failed to Connect", failure.Errors.Length).ToArray(), failure.Errors);
                    screenManager.DialogueScreen.SetDialogue(errorUuid);
                    screenManager.DisplayScreen(ScreenType.Dialogue, true);
                }
            }
            catch (Exception ex)
            {
                var screenManager = Game.ScreenManager;
                var errorUuid = Guid.NewGuid().ToString();

                // Print exception message.
                Console.WriteLine(ex);
                DialogueManager.AddText(errorUuid, new []{"An Exception Occurred"}, new []{ex.Message});
                screenManager.DialogueScreen.SetDialogue(errorUuid);
                screenManager.DisplayScreen(ScreenType.Dialogue, true);
            }
        }

        /// <summary>
        /// Disconnects from a currently connected AP server and re-initializes ArchClient fields.
        /// </summary>
        public void DisconnectAndReset()
        {
            if (IsConnected)
                m_session.Socket.Disconnect();

            m_session = null;
            m_deathLinkService = null;
            m_seed = null;
            Slot = 1;
            m_player = new NetworkPlayer();
            m_data = null;
            m_tags = new List<string>{"AP"};
            m_missingLocations = new List<int>();
            m_checkedLocations = new List<int>();
            LocationCache = new Dictionary<int, NetworkItem>();

            // Load Save Data
            LoadSaveData();
        }

        /// <summary>
        /// Starts a the game.
        /// </summary>
        public void StartGame()
        {
            SoundManager.PlaySound("Game_Start");

            var newGame = !Game.PlayerStats.CharacterFound;
            var heroIsDead = Game.PlayerStats.IsDead;
            var startingRoom = Game.PlayerStats.LoadStartingRoom;

            // TODO: Implement Manor Buying and what Not.
            for (var i = 4445008; i < 4445039; i++)
            {
                Program.Game.ArchClient.Session.Locations.CompleteLocationChecks(i);
                Program.Game.ArchClient.CheckedLocations[i] = true;
            }

            if (newGame)
            {
                Game.PlayerStats.CharacterFound = true;
                Game.PlayerStats.Gold = 0;
                Game.PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, 5);
                Game.PlayerStats.EnemiesKilledInRun.Clear();

                // Rename Sir Lee to the player's name and initial gender.
                Game.PlayerStats.IsFemale = m_data.StartingGender == StartingGender.Lady;
                Game.PlayerStats.PlayerName = string.Format("{1} {0}", m_player.Name, Game.PlayerStats.IsFemale ? "Lady" : "Sir");

                Program.Game.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
                Game.ScreenManager.DisplayScreen(ScreenType.StartingRoom, true);
            }
            else
            {
                if (heroIsDead)
                    Game.ScreenManager.DisplayScreen(ScreenType.Lineage, true);
                else
                    Game.ScreenManager.DisplayScreen(startingRoom ? ScreenType.StartingRoom : ScreenType.Level, true);
            }

            SoundManager.StopMusic(0.2f);
        }

        /// <summary>
        /// Main packet receiver handler.
        /// </summary>
        /// <param name="basePacket">The packet sent to the client.</param>
        private void PacketReceivedHandler(ArchipelagoPacketBase basePacket)
        {
            // Connected Packet Handler
            var connected = basePacket as ConnectedPacket;
            if (connected != null)
                ConnectedPacketHandler(connected);

            // RoomInfo Packet Handler
            var roomInfo = basePacket as RoomInfoPacket;
            if (roomInfo != null)
                RoomInfoPacketHandler(roomInfo);
        }

        /// <summary>
        /// Handler for connected packets.
        /// </summary>
        /// <param name="packet">Connected Packet</param>
        private void ConnectedPacketHandler(ConnectedPacket packet)
        {
            Slot = packet.Slot;
            m_missingLocations = packet.MissingChecks;
            m_checkedLocations = packet.LocationsChecked;
            CreateLocationsDictionary();

            // Create save file if it doesn't exist.
            Game.ChangeSlot(m_seed, Slot);
            Program.Game.SaveManager.CreateSaveDirectory();

            // Load player/slot specific data.
            m_player = packet.Players[Slot - 1];
            m_data = new SlotData(packet.SlotData);

            // Our our player's name to the name pool, so their potential children can be named after them.
            if (Game.PlayerStats.IsFemale && !Game.FemaleNameArray.Contains(m_player.Name))
                Game.FemaleNameArray.Add(m_player.Name);
            else if (!Game.PlayerStats.IsFemale && !Game.NameArray.Contains(m_player.Name))
                Game.NameArray.Add(m_player.Name);

            // Load Settings
            SetSettings();

            // Load DeathLink
            if (m_data.DeathLink)
            {
                m_tags.Add("DeathLink");
                m_session.UpdateTags(m_tags);
                m_deathLinkService = m_session.CreateDeathLinkServiceAndEnable();

                // Attach to death link handler.
                m_deathLinkService.OnDeathLinkReceived += DeathLinkHandler;
            }

            List<int> locations = new List<int>();
            foreach (var location in Locations.IdTable.Values)
            {
                Console.WriteLine(location);
                locations.Add(location);
            }

            m_session.Locations.ScoutLocationsAsync(PrepareLocationCache, locations.ToArray());

            // Load the game.
            StartGame();
        }

        private void PrepareLocationCache(LocationInfoPacket locationInfo)
        {
            var locations = locationInfo.Locations;
            foreach (var networkItem in locations)
            {
                Console.WriteLine("{0} - {1} - {2}", networkItem.Item, networkItem.Location, networkItem.Player);
                LocationCache.Add(networkItem.Location, networkItem);
            }
        }

        /// <summary>
        /// Handler for room info packets.
        /// </summary>
        /// <param name="packet">RoomInfo Packet</param>
        private void RoomInfoPacketHandler(RoomInfoPacket packet)
        {
            m_seed = packet.SeedName;
        }

        /// <summary>
        /// Handler for DeathLink messages.
        /// </summary>
        /// <param name="deathLink">DeathLink Message</param>
        private void DeathLinkHandler(DeathLink deathLink)
        {
            // Do not death link on ourselves.
            if (deathLink.Source == m_player.Name)
                return;

            Console.WriteLine("Received a DeathLink!");
            var screen = Game.ScreenManager.GetLevelScreen();
            if (screen != null && screen.Player != null)
            {
                // TODO: Create custom object for handling outside players for DeathLink.
                screen.Player.Kill();
            }
        }

        /// <summary>
        /// Set our settings into our game.
        /// </summary>
        private void SetSettings()
        {
            Game.PlayerStats.TimesCastleBeaten = m_data.Difficulty;
        }

        /// <summary>
        /// Create a locations dictionary to keep track of what locations we opened and which we did not.
        /// </summary>
        private void CreateLocationsDictionary()
        {
            CheckedLocations = new Dictionary<int, bool>();

            foreach (var location in m_missingLocations)
            {
                CheckedLocations.Add(location, false);
            }

            foreach (var location in m_checkedLocations)
            {
                CheckedLocations.Add(location, true);
            }
        }

        /// <summary>
        /// Load Save Data for this slot.
        /// </summary>
        private static void LoadSaveData()
        {
            Game.PlayerStats.TutorialComplete = true;
            Game.NameArray = Game.DefaultNameArray.Clone();
            Game.FemaleNameArray = Game.DefaultFemaleNameArray.Clone();
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
            Game.ScreenManager.Player.Reset();
            Program.Game.SaveManager.LoadFiles(null, SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
            Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
        }
    }
}
