//
//  RogueLegacyArchipelago - GetItemScreen.cs
//  Last Modified 2021-12-29
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using Archipelago;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;
using Screen = DS2DEngine.Screen;

namespace RogueCastle
{
    public class GetItemScreen : Screen
    {
        // Goodbye Magic Numbers
        private const float ItemFoundYOffset = 70f;
        private const float NetworkItemFoundYOffset = 105f;
        private const float ItemFoundPlayerYOffset = 75f;
        private readonly Vector2 m_itemEndPos;
        private Cue m_buildUpSound;
        private KeyIconTextObj m_continueText;
        private TextObj m_itemFoundPlayerText;
        private SpriteObj m_itemFoundSprite;
        private TextObj m_itemFoundText;
        private Vector2 m_itemInfo;
        private bool m_itemSpinning;
        private SpriteObj m_itemSprite;
        private Vector2 m_itemStartPos;
        private byte m_itemType;
        private SpriteObj m_levelUpBGImage;
        private SpriteObj[] m_levelUpParticles;

        private bool m_lockControls;
        private int m_network_item;
        private string m_network_player;
        private string m_songName;
        private float m_storedMusicVolume;
        private SpriteObj m_tripStat1;
        private TextObj m_tripStat1FoundText;
        private SpriteObj m_tripStat2;
        private TextObj m_tripStat2FoundText;
        private Vector2 m_tripStatData;

        public GetItemScreen()
        {
            DrawIfCovered = true;
            BackBufferOpacity = 0f;
            m_itemEndPos = new Vector2(660f, 410f);
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_levelUpBGImage = new SpriteObj("BlueprintFoundBG_Sprite")
            {
                ForceDraw = true,
                Visible = false
            };
            m_levelUpParticles = new SpriteObj[10];

            for (var i = 0; i < m_levelUpParticles.Length; i++)
            {
                m_levelUpParticles[i] = new SpriteObj("LevelUpParticleFX_Sprite");
                m_levelUpParticles[i].AnimationDelay = 0.0416666679f;
                m_levelUpParticles[i].ForceDraw = true;
                m_levelUpParticles[i].Visible = false;
            }

            m_itemSprite = new SpriteObj("BlueprintIcon_Sprite")
            {
                ForceDraw = true,
                OutlineWidth = 2
            };
            m_tripStat1 = m_itemSprite.Clone() as SpriteObj;
            m_tripStat2 = m_itemSprite.Clone() as SpriteObj;

            // Item Found Texts
            m_itemFoundText = new TextObj(Game.JunicodeFont)
            {
                FontSize = 18f,
                Align = Types.TextAlign.Centre,
                Text = "",
                Position = m_itemEndPos,
                ForceDraw = true,
                OutlineWidth = 2
            };
            m_itemFoundText.Y += ItemFoundYOffset;
            m_tripStat1FoundText = m_itemFoundText.Clone() as TextObj;
            m_tripStat2FoundText = m_itemFoundText.Clone() as TextObj;
            m_itemFoundPlayerText = m_itemFoundText.Clone() as TextObj;
            m_itemFoundPlayerText.Y += ItemFoundPlayerYOffset;
            m_itemFoundPlayerText.FontSize = 12f;

            m_itemFoundSprite = new SpriteObj("BlueprintFoundText_Sprite")
            {
                ForceDraw = true,
                Visible = false
            };
            m_continueText = new KeyIconTextObj(Game.JunicodeFont)
            {
                FontSize = 14f,
                Text = "to continue",
                Align = Types.TextAlign.Centre,
                ForceDraw = true
            };
            m_continueText.Position = new Vector2(1320 - m_continueText.Width, 720 - m_continueText.Height - 10);

            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            m_itemStartPos = (Vector2) objList[0];
            m_itemType = Convert.ToByte(objList[1]);
            m_itemInfo = (Vector2) objList[2];

            switch (m_itemType)
            {
                case (int) ItemCategory.TripStatDrop:
                    m_tripStatData = (Vector2) objList[3];
                    break;

                case (int) ItemCategory.ReceiveNetworkItem:
                case (int) ItemCategory.GiveNetworkItem:
                    m_tripStatData = (Vector2) objList[3];
                    m_network_player = (string) objList[4];
                    m_network_item = (int) objList[5];
                    break;
            }

            base.PassInData(objList);
        }

        public override void OnEnter()
        {
            m_tripStat1.Visible = false;
            m_tripStat2.Visible = false;
            m_tripStat1.Scale = Vector2.One;
            m_tripStat2.Scale = Vector2.One;

            if (m_itemType != (int) ItemCategory.FountainPiece)
            {
                (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
            }

            m_itemFoundPlayerText.Visible = false;

            m_itemSprite.Rotation = 0f;
            m_itemSprite.Scale = Vector2.One;
            m_itemStartPos.X -= Camera.TopLeftCorner.X;
            m_itemStartPos.Y -= Camera.TopLeftCorner.Y;
            m_storedMusicVolume = SoundManager.GlobalMusicVolume;
            m_songName = SoundManager.GetCurrentMusicName();
            m_lockControls = true;
            m_continueText.Opacity = 0f;
            m_continueText.Text = string.Format("[Input:{0}]  to continue", (int) Button.MenuConfirm1);

            // Item Found Text
            m_itemFoundText.Position = m_itemEndPos;

            if (m_itemType != (int) ItemCategory.GiveNetworkItem)
            {
                m_itemFoundText.Y += ItemFoundYOffset;
            }
            else
            {
                m_itemFoundText.Y += NetworkItemFoundYOffset;
            }

            m_itemFoundText.Scale = Vector2.Zero;
            m_itemFoundPlayerText.Position = m_itemEndPos;
            m_itemFoundPlayerText.Y += ItemFoundPlayerYOffset;
            m_itemFoundPlayerText.Scale = Vector2.Zero;

            // Trip Stats
            m_tripStat1FoundText.Position = m_itemFoundText.Position;
            m_tripStat2FoundText.Position = m_itemFoundText.Position;
            m_tripStat1FoundText.Scale = Vector2.Zero;
            m_tripStat2FoundText.Scale = Vector2.Zero;
            m_tripStat1FoundText.Visible = false;
            m_tripStat2FoundText.Visible = false;
            switch (m_itemType)
            {
                case (int) ItemCategory.Blueprint:
                    m_itemSpinning = true;
                    m_itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                    m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    m_itemFoundText.Text = $"{((EquipmentBase) m_itemInfo.Y)} {((EquipmentCategory) m_itemInfo.X).ToString2()}";
                    break;

                case (int) ItemCategory.Rune:
                    m_itemSpinning = true;
                    m_itemSprite.ChangeSprite("RuneIcon_Sprite");
                    m_itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                    m_itemFoundText.Text = $"{((EquipmentAbility) m_itemInfo.Y)} Rune ({((EquipmentCategory) m_itemInfo.X).ToString2()})";
                    m_itemSprite.AnimationDelay = 0.05f;
                    break;

                case (int) ItemCategory.StatDrop:
                case (int) ItemCategory.TripStatDrop:
                    m_itemSprite.ChangeSprite(GetStatSpriteName((int) m_itemInfo.X));
                    m_itemFoundText.Text = GetStatText((int) m_itemInfo.X);
                    m_itemSprite.AnimationDelay = 0.05f;
                    m_itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                    if (m_itemType == 6)
                    {
                        m_tripStat1FoundText.Visible = true;
                        m_tripStat2FoundText.Visible = true;
                        m_tripStat1.ChangeSprite(GetStatSpriteName((int) m_tripStatData.X));
                        m_tripStat2.ChangeSprite(GetStatSpriteName((int) m_tripStatData.Y));
                        m_tripStat1.Visible = true;
                        m_tripStat2.Visible = true;
                        m_tripStat1.AnimationDelay = 0.05f;
                        m_tripStat2.AnimationDelay = 0.05f;
                        Tween.RunFunction(0.1f, m_tripStat1, "PlayAnimation", true);
                        Tween.RunFunction(0.2f, m_tripStat2, "PlayAnimation", true);
                        m_tripStat1FoundText.Text = GetStatText((int) m_tripStatData.X);
                        m_tripStat2FoundText.Text = GetStatText((int) m_tripStatData.Y);
                        m_itemFoundText.Y += 50f;
                        m_tripStat1FoundText.Y = m_itemFoundText.Y + 50f;
                    }

                    break;

                case (int) ItemCategory.Spell:
                    m_itemSprite.ChangeSprite(((Spell) m_itemInfo.X).Icon());
                    m_itemFoundSprite.ChangeSprite("SpellFoundText_Sprite");
                    m_itemFoundText.Text = ((Spell) m_itemInfo.X).ToString();
                    break;

                case (int) ItemCategory.SpecialItem:
                    m_itemSprite.ChangeSprite(((SpecialItem) m_itemInfo.X).SpriteName());
                    m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    m_itemFoundText.Text = ((SpecialItem) m_itemInfo.X).ToString();
                    break;

                case 7:
                    m_itemSprite.ChangeSprite(GetMedallionImage((int) m_itemInfo.X));
                    m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    m_itemFoundText.Text = m_itemInfo.X == 19f
                        ? "Medallion completed!"
                        : "You've collected a medallion piece!";
                    break;

                case (int) ItemCategory.GiveNetworkItem:
                    m_itemSpinning = true;
                    m_itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                    m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    m_itemFoundText.Text = Program.Game.ArchipelagoManager.GetItemName(m_network_item);
                    m_itemFoundPlayerText.Visible = true;
                    m_itemFoundPlayerText.Text = string.Format("You found {0}'s", m_network_player);
                    m_itemFoundText.TextureColor = Color.Yellow;

                    switch (m_network_item.GetItemType())
                    {
                        case ItemType.Rune:
                            m_itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                            m_itemSprite.ChangeSprite("RuneIcon_Sprite");
                            break;

                        case ItemType.Skill:
                            m_itemSpinning = false;
                            m_itemSprite.ChangeSprite(GetSkillPlateIcon(m_network_item));
                            break;

                        case ItemType.Stats:
                            m_itemSpinning = false;
                            m_itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                            break;

                        case ItemType.Gold:
                            m_itemSpinning = false;
                            m_itemSprite.ChangeSprite("MoneyBag_Sprite");
                            break;
                    }

                    break;

                case (int) ItemCategory.ReceiveNetworkItem:
                    m_itemFoundPlayerText.Visible = true;
                    switch (m_network_item.GetItemType())
                    {
                        case ItemType.Blueprint:
                            m_itemFoundText.Y += 40f;
                            m_itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                            m_itemSpinning = true;
                            m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                            m_itemFoundText.Text = Program.Game.ArchipelagoManager.GetItemName(m_network_item);
                            m_itemFoundPlayerText.Text = string.Format("You received from {0}", m_network_player);
                            m_itemFoundText.TextureColor = Color.Yellow;
                            break;

                        case ItemType.Rune:
                            m_itemFoundText.Y += 40f;
                            m_itemSpinning = true;
                            m_itemSprite.ChangeSprite("RuneIcon_Sprite");
                            m_itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                            m_itemFoundText.Text = Program.Game.ArchipelagoManager.GetItemName(m_network_item);
                            m_itemFoundPlayerText.Text = string.Format("You received from {0}", m_network_player);
                            m_itemSprite.AnimationDelay = 0.05f;
                            break;

                        case ItemType.Skill:
                            m_itemFoundText.Y += 40f;
                            m_itemSprite.ChangeSprite(GetSkillPlateIcon(m_network_item));
                            m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                            m_itemFoundText.Text = Program.Game.ArchipelagoManager.GetItemName(m_network_item);
                            m_itemFoundPlayerText.Text = string.Format("You received from {0}", m_network_player);
                            m_itemFoundText.TextureColor = Color.Yellow;
                            break;

                        case ItemType.Stats:
                            m_itemFoundText.Y += 50f;
                            m_itemSprite.ChangeSprite(GetStatSpriteName((int) m_itemInfo.X));
                            m_itemFoundText.Text = GetStatText((int) m_itemInfo.X);
                            m_itemSprite.AnimationDelay = 0.05f;
                            m_itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                            m_itemFoundPlayerText.Text = string.Format("You received from {0}", m_network_player);
                            m_tripStat1FoundText.Visible = true;
                            m_tripStat2FoundText.Visible = true;
                            m_tripStat1.ChangeSprite(GetStatSpriteName((int) m_tripStatData.X));
                            m_tripStat2.ChangeSprite(GetStatSpriteName((int) m_tripStatData.Y));
                            m_tripStat1.Visible = true;
                            m_tripStat2.Visible = true;
                            m_tripStat1.AnimationDelay = 0.05f;
                            m_tripStat2.AnimationDelay = 0.05f;
                            Tween.RunFunction(0.1f, m_tripStat1, "PlayAnimation", true);
                            Tween.RunFunction(0.2f, m_tripStat2, "PlayAnimation", true);
                            m_tripStat1FoundText.Text = GetStatText((int) m_tripStatData.X);
                            m_tripStat2FoundText.Text = GetStatText((int) m_tripStatData.Y);
                            m_tripStat1FoundText.Y = m_itemFoundText.Y + 50f;
                            m_tripStat2FoundText.Y = m_itemFoundText.Y + 100f;
                            m_tripStat1FoundText.TextureColor = Color.Yellow;
                            m_tripStat2FoundText.TextureColor = Color.Yellow;

                            break;

                        case ItemType.Gold:
                            m_itemFoundText.Y += 40f;
                            m_itemSprite.ChangeSprite("MoneyBag_Sprite");
                            m_itemSprite.AnimationSpeed = 0;
                            m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                            m_itemFoundText.Text = string.Format("{0} Gold", (int) m_itemInfo.Y);
                            m_itemFoundPlayerText.Text = string.Format("You received from {0}", m_network_player);
                            m_itemFoundText.TextureColor = Color.Yellow;
                            break;
                    }

                    break;
            }

            m_itemSprite.PlayAnimation();
            ItemSpinAnimation();
            base.OnEnter();
        }

        private string GetSkillPlateIcon(int item)
        {
            if (item == ItemDefinitions.Blacksmith.Code)
            {
                return SkillSystem.GetSkill(Skill.Smithy).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Architect.Code)
            {
                return SkillSystem.GetSkill(Skill.Architect).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Enchantress.Code)
            {
                return SkillSystem.GetSkill(Skill.Enchanter).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Paladin.Code)
            {
                return SkillSystem.GetSkill(Skill.KnightUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Archmage.Code)
            {
                return SkillSystem.GetSkill(Skill.MageUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.BarbarianKing.Code)
            {
                return SkillSystem.GetSkill(Skill.BarbarianUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Assassin.Code)
            {
                return SkillSystem.GetSkill(Skill.AssassinUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveShinobi.Code)
            {
                return SkillSystem.GetSkill(Skill.NinjaUp).CurrentLevel > 0
                    ? SkillSystem.GetSkill(Skill.NinjaUp).IconName.Replace("Locked", "")
                    : SkillSystem.GetSkill(Skill.NinjaUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveMiner.Code)
            {
                return SkillSystem.GetSkill(Skill.BankerUp).CurrentLevel > 0
                    ? SkillSystem.GetSkill(Skill.BankerUp).IconName.Replace("Locked", "")
                    : SkillSystem.GetSkill(Skill.BankerUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveLich.Code)
            {
                return SkillSystem.GetSkill(Skill.LichUp).CurrentLevel > 0
                    ? SkillSystem.GetSkill(Skill.LichUp).IconName.Replace("Locked", "")
                    : SkillSystem.GetSkill(Skill.LichUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveSpellthief.Code)
            {
                return SkillSystem.GetSkill(Skill.SpellSwordUp).CurrentLevel > 0
                    ? SkillSystem.GetSkill(Skill.SpellSwordUp).IconName.Replace("Locked", "")
                    : SkillSystem.GetSkill(Skill.SpellswordUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Dragon.Code)
            {
                return SkillSystem.GetSkill(Skill.SuperSecret).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Dragon.Code)
            {
                // TODO: Make Traitor icon
                return SkillSystem.GetSkill(Skill.SuperSecret).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.HealthUp.Code)
            {
                return SkillSystem.GetSkill(Skill.HealthUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ManaUp.Code)
            {
                return SkillSystem.GetSkill(Skill.ManaUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.AttackUp.Code)
            {
                return SkillSystem.GetSkill(Skill.AttackUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.MagicDamageUp.Code)
            {
                return SkillSystem.GetSkill(Skill.MagicDamageUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ArmorUp.Code)
            {
                return SkillSystem.GetSkill(Skill.ArmorUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.EquipUp.Code)
            {
                return SkillSystem.GetSkill(Skill.EquipUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.CritChanceUp.Code)
            {
                return SkillSystem.GetSkill(Skill.CritChanceUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.CritDamageUp.Code)
            {
                return SkillSystem.GetSkill(Skill.CritDamageUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.DownStrikeUp.Code)
            {
                return SkillSystem.GetSkill(Skill.DownStrikeUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.GoldGainUp.Code)
            {
                return SkillSystem.GetSkill(Skill.GoldGainUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.PotionEfficiencyUp.Code)
            {
                return SkillSystem.GetSkill(Skill.PotionUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.InvulnTimeUp.Code)
            {
                return SkillSystem.GetSkill(Skill.InvulnerabilityTimeUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ManaCostDown.Code)
            {
                return SkillSystem.GetSkill(Skill.ManaCostDown).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.DeathDefiance.Code)
            {
                return SkillSystem.GetSkill(Skill.DeathDodge).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.Haggling.Code)
            {
                return SkillSystem.GetSkill(Skill.PricesDown).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.RandomizeChildren.Code)
            {
                return SkillSystem.GetSkill(Skill.RandomizeChildren).IconName.Replace("Locked", "");
            }

            return "BlueprintIcon_Sprite";
        }

        private void ItemSpinAnimation()
        {
            m_itemSprite.Scale = Vector2.One;
            m_itemSprite.Position = m_itemStartPos;
            m_buildUpSound = SoundManager.PlaySound("GetItemBuildupStinger");
            Tween.To(typeof(SoundManager), 1f, Tween.EaseNone, "GlobalMusicVolume",
                (m_storedMusicVolume * 0.1f).ToString());
            m_itemSprite.Scale = new Vector2(35f / m_itemSprite.Height, 35f / m_itemSprite.Height);
            Tween.By(m_itemSprite, 0.5f, Back.EaseOut, "Y", "-150");
            Tween.RunFunction(0.7f, this, "ItemSpinAnimation2");
            m_tripStat1.Scale = Vector2.One;
            m_tripStat2.Scale = Vector2.One;
            m_tripStat1.Position = m_itemStartPos;
            m_tripStat2.Position = m_itemStartPos;
            Tween.By(m_tripStat1, 0.5f, Back.EaseOut, "Y", "-150", "X", "50");
            m_tripStat1.Scale = new Vector2(35f / m_tripStat1.Height, 35f / m_tripStat1.Height);
            Tween.By(m_tripStat2, 0.5f, Back.EaseOut, "Y", "-150", "X", "-50");
            m_tripStat2.Scale = new Vector2(35f / m_tripStat2.Height, 35f / m_tripStat2.Height);
        }

        public void ItemSpinAnimation2()
        {
            Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", "GetItemStinger3");
            if (m_buildUpSound != null && m_buildUpSound.IsPlaying)
            {
                m_buildUpSound.Stop(AudioStopOptions.AsAuthored);
            }

            Tween.To(m_itemSprite, 0.2f, Quad.EaseOut, "ScaleX", "0.1", "ScaleY", "0.1");
            Tween.AddEndHandlerToLastTween(this, "ItemSpinAnimation3");
            Tween.To(m_tripStat1, 0.2f, Quad.EaseOut, "ScaleX", "0.1", "ScaleY", "0.1");
            Tween.To(m_tripStat2, 0.2f, Quad.EaseOut, "ScaleX", "0.1", "ScaleY", "0.1");
        }

        public void ItemSpinAnimation3()
        {
            var scale = m_itemSprite.Scale;
            m_itemSprite.Scale = Vector2.One;
            var num = 130f / m_itemSprite.Height;
            m_itemSprite.Scale = scale;
            Tween.To(m_itemSprite, 0.2f, Tween.EaseNone, "ScaleX", num.ToString(), "ScaleY", num.ToString());
            Tween.To(m_itemSprite, 0.2f, Tween.EaseNone, "X", 660.ToString(), "Y", 390.ToString());
            Tween.To(m_itemFoundText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_itemFoundPlayerText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_continueText, 0.3f, Linear.EaseNone, "Opacity", "1");
            scale = m_tripStat1.Scale;
            m_tripStat1.Scale = Vector2.One;
            num = 130f / m_tripStat1.Height;
            m_tripStat1.Scale = scale;
            Tween.To(m_tripStat1, 0.2f, Tween.EaseNone, "ScaleX", num.ToString(), "ScaleY", num.ToString());
            Tween.To(m_tripStat1, 0.2f, Tween.EaseNone, "X", 830.ToString(), "Y", 390.ToString());
            scale = m_tripStat2.Scale;
            m_tripStat2.Scale = Vector2.One;
            num = 130f / m_tripStat2.Height;
            m_tripStat2.Scale = scale;
            Tween.To(m_tripStat2, 0.2f, Tween.EaseNone, "ScaleX", num.ToString(), "ScaleY", num.ToString());
            Tween.To(m_tripStat2, 0.2f, Tween.EaseNone, "X", 490.ToString(), "Y", 390.ToString());
            Tween.To(m_tripStat1FoundText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_tripStat2FoundText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            for (var i = 0; i < m_levelUpParticles.Length; i++)
            {
                m_levelUpParticles[i].AnimationDelay = 0f;
                m_levelUpParticles[i].Visible = true;
                m_levelUpParticles[i].Scale = new Vector2(0.1f, 0.1f);
                m_levelUpParticles[i].Opacity = 0f;
                m_levelUpParticles[i].Position = new Vector2(660f, 360f);
                m_levelUpParticles[i].Position += new Vector2(CDGMath.RandomInt(-100, 100), CDGMath.RandomInt(-50, 50));
                var duration = CDGMath.RandomFloat(0f, 0.5f);
                Tween.To(m_levelUpParticles[i], 0.2f, Linear.EaseNone, "delay", duration.ToString(), "Opacity", "1");
                Tween.To(m_levelUpParticles[i], 0.5f, Linear.EaseNone, "delay", duration.ToString(), "ScaleX", "2",
                    "ScaleY", "2");
                Tween.To(m_levelUpParticles[i], duration, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(m_levelUpParticles[i], "PlayAnimation", false);
            }

            m_itemFoundSprite.Position = new Vector2(660f, 190f);
            m_itemFoundSprite.Scale = Vector2.Zero;
            m_itemFoundSprite.Visible = true;
            Tween.To(m_itemFoundSprite, 0.5f, Back.EaseOut, "delay", "0.05", "ScaleX", "1", "ScaleY", "1");
            m_levelUpBGImage.Position = m_itemFoundSprite.Position;
            m_levelUpBGImage.Y += 30f;
            m_levelUpBGImage.Scale = Vector2.Zero;
            m_levelUpBGImage.Visible = true;
            Tween.To(m_levelUpBGImage, 0.5f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            if (m_itemSpinning)
            {
                m_itemSprite.Rotation = -25f;
            }

            m_itemSpinning = false;
            Tween.RunFunction(0.5f, this, "UnlockControls");
        }

        public void UnlockControls()
        {
            m_lockControls = false;
        }

        public void ExitScreenTransition()
        {
            if ((int) m_itemInfo.X == 19)
            {
                m_itemInfo = Vector2.Zero;
                var list = new List<object>();
                list.Add(17);
                Game.ScreenManager.DisplayScreen(19, true, list);
                return;
            }

            m_lockControls = true;
            Tween.To(typeof(SoundManager), 1f, Tween.EaseNone, "GlobalMusicVolume", m_storedMusicVolume.ToString());
            Tween.To(m_itemSprite, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_itemFoundText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_itemFoundPlayerText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(this, 0.4f, Back.EaseIn, "BackBufferOpacity", "0");
            Tween.To(m_levelUpBGImage, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_itemFoundSprite, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_continueText, 0.4f, Linear.EaseNone, "delay", "0.1", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager as RCScreenManager, "HideCurrentScreen");
            Tween.To(m_tripStat1, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_tripStat2, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_tripStat1FoundText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_tripStat2FoundText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
        }

        public override void Update(GameTime gameTime)
        {
            if (m_itemSpinning)
            {
                m_itemSprite.Rotation += 1200f * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (!m_lockControls &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                 Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)))
            {
                ExitScreenTransition();
            }

            base.HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearClamp, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            m_levelUpBGImage.Draw(Camera);
            var levelUpParticles = m_levelUpParticles;
            for (var i = 0; i < levelUpParticles.Length; i++)
            {
                var spriteObj = levelUpParticles[i];
                spriteObj.Draw(Camera);
            }

            m_itemFoundSprite.Draw(Camera);
            m_itemFoundText.Draw(Camera);
            m_itemFoundPlayerText.Draw(Camera);
            m_tripStat1FoundText.Draw(Camera);
            m_tripStat2FoundText.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            m_itemSprite.Draw(Camera);
            m_tripStat1.Draw(Camera);
            m_tripStat2.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_continueText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        private string GetStatSpriteName(int type)
        {
            switch (type)
            {
                case 4:
                    return "Sword_Sprite";

                case 5:
                    return "MagicBook_Sprite";

                case 6:
                    return "Shield_Sprite";

                case 7:
                    return "Heart_Sprite";

                case 8:
                    return "ManaCrystal_Sprite";

                case 9:
                    return "Backpack_Sprite";

                default:
                    return "";
            }
        }

        private string GetStatText(int type)
        {
            switch (type)
            {
                case (int) ItemDrop.StatStrength:
                    return "Strength Increased: +" + 1;

                case (int) ItemDrop.StatMagic:
                    return "Magic Damage Increased: +" + 1;

                case (int) ItemDrop.StatDefense:
                    return "Armor Increased: +" + 2;

                case (int) ItemDrop.StatMaxHealth:
                    return "HP Increased: +" + 5;

                case (int) ItemDrop.StatMaxMana:
                    return "MP Increased: +" + 5;

                case (int) ItemDrop.StatWeight:
                    return "Max Weight Load Increased: +" + 5;

                default:
                    return "";
            }
        }

        private string GetMedallionImage(int medallionType)
        {
            switch (medallionType)
            {
                case 15:
                    return "MedallionPiece1_Sprite";

                case 16:
                    return "MedallionPiece2_Sprite";

                case 17:
                    return "MedallionPiece3_Sprite";

                case 18:
                    return "MedallionPiece4_Sprite";

                case 19:
                    return "MedallionPiece5_Sprite";

                default:
                    return "";
            }
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Get Item Screen");
                m_continueText.Dispose();
                m_continueText = null;
                m_levelUpBGImage.Dispose();
                m_levelUpBGImage = null;
                var levelUpParticles = m_levelUpParticles;
                for (var i = 0; i < levelUpParticles.Length; i++)
                {
                    var spriteObj = levelUpParticles[i];
                    spriteObj.Dispose();
                }

                Array.Clear(m_levelUpParticles, 0, m_levelUpParticles.Length);
                m_levelUpParticles = null;
                m_buildUpSound = null;
                m_itemSprite.Dispose();
                m_itemSprite = null;
                m_itemFoundSprite.Dispose();
                m_itemFoundSprite = null;
                m_itemFoundText.Dispose();
                m_itemFoundText = null;
                m_itemFoundPlayerText.Dispose();
                m_itemFoundPlayerText = null;
                m_tripStat1.Dispose();
                m_tripStat2.Dispose();
                m_tripStat1 = null;
                m_tripStat2 = null;
                m_tripStat1FoundText.Dispose();
                m_tripStat2FoundText.Dispose();
                m_tripStat1FoundText = null;
                m_tripStat2FoundText = null;
                base.Dispose();
            }
        }
    }
}