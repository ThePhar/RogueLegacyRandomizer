// 
//  Rogue Legacy Randomizer - GetItemScreen.cs
//  Last Modified 2022-01-23
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using Archipelago.Definitions;
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
        private const float ItemFoundYOffset        = 70f;
        private const float NetworkItemFoundYOffset = 105f;
        private const float ItemFoundPlayerYOffset  = 75f;

        private readonly Vector2        _itemEndPos;
        private          Cue            _buildUpSound;
        private          KeyIconTextObj _continueText;
        private          TextObj        _itemFoundPlayerText;
        private          SpriteObj      _itemFoundSprite;
        private          TextObj        _itemFoundText;
        private          Vector2        _itemInfo;
        private          bool           _itemSpinning;
        private          SpriteObj      _itemSprite;
        private          Vector2        _itemStartPos;
        private          byte           _itemType;
        private          SpriteObj      _levelUpBGImage;
        private          SpriteObj[]    _levelUpParticles;
        private          bool           _lockControls;
        private          int            _network_item;
        private          string         _network_player;
        private          string         _songName;
        private          float          _storedMusicVolume;
        private          SpriteObj      _tripStat1;
        private          TextObj        _tripStat1FoundText;
        private          SpriteObj      _tripStat2;
        private          TextObj        _tripStat2FoundText;
        private          Vector2        _tripStatData;

        public GetItemScreen()
        {
            DrawIfCovered = true;
            BackBufferOpacity = 0f;
            _itemEndPos = new Vector2(660f, 410f);
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            _levelUpBGImage = new SpriteObj("BlueprintFoundBG_Sprite")
            {
                ForceDraw = true,
                Visible = false
            };
            _levelUpParticles = new SpriteObj[10];

            for (var i = 0; i < _levelUpParticles.Length; i++)
            {
                _levelUpParticles[i] = new SpriteObj("LevelUpParticleFX_Sprite")
                {
                    AnimationDelay = 0.0416666679f,
                    ForceDraw = true,
                    Visible = false
                };
            }

            _itemSprite = new SpriteObj("BlueprintIcon_Sprite")
            {
                ForceDraw = true,
                OutlineWidth = 2
            };
            _tripStat1 = _itemSprite.Clone() as SpriteObj;
            _tripStat2 = _itemSprite.Clone() as SpriteObj;

            // Item Found Texts
            _itemFoundText = new TextObj(Game.JunicodeFont)
            {
                FontSize = 18f,
                Align = Types.TextAlign.Centre,
                Text = "",
                Position = _itemEndPos,
                ForceDraw = true,
                OutlineWidth = 2
            };
            _itemFoundText.Y += ItemFoundYOffset;
            _tripStat1FoundText = _itemFoundText.Clone() as TextObj;
            _tripStat2FoundText = _itemFoundText.Clone() as TextObj;
            _itemFoundPlayerText = _itemFoundText.Clone() as TextObj;
            _itemFoundPlayerText.Y += ItemFoundPlayerYOffset;
            _itemFoundPlayerText.FontSize = 12f;

            _itemFoundSprite = new SpriteObj("BlueprintFoundText_Sprite")
            {
                ForceDraw = true,
                Visible = false
            };
            _continueText = new KeyIconTextObj(Game.JunicodeFont)
            {
                FontSize = 14f,
                Text = "to continue",
                Align = Types.TextAlign.Centre,
                ForceDraw = true
            };
            _continueText.Position = new Vector2(1320 - _continueText.Width, 720 - _continueText.Height - 10);

            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            _itemStartPos = (Vector2) objList[0];
            _itemType = Convert.ToByte(objList[1]);
            _itemInfo = (Vector2) objList[2];

            switch (_itemType)
            {
                case (int) ItemCategory.TripStatDrop:
                    _tripStatData = (Vector2) objList[3];
                    break;

                case (int) ItemCategory.ReceiveNetworkItem:
                case (int) ItemCategory.GiveNetworkItem:
                    _tripStatData = (Vector2) objList[3];
                    _network_player = (string) objList[4];
                    _network_item = (int) objList[5];
                    break;
            }

            base.PassInData(objList);
        }

        public override void OnEnter()
        {
            _tripStat1.Visible = false;
            _tripStat2.Visible = false;
            _tripStat1.Scale = Vector2.One;
            _tripStat2.Scale = Vector2.One;

            if (_itemType != (int) ItemCategory.FountainPiece)
            {
                (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
            }

            _itemFoundPlayerText.Visible = false;

            _itemSprite.Rotation = 0f;
            _itemSprite.Scale = Vector2.One;
            _itemStartPos.X -= Camera.TopLeftCorner.X;
            _itemStartPos.Y -= Camera.TopLeftCorner.Y;
            _storedMusicVolume = SoundManager.GlobalMusicVolume;
            _songName = SoundManager.GetCurrentMusicName();
            _lockControls = true;
            _continueText.Opacity = 0f;
            _continueText.Text = $"[Input:{(int) Button.MenuConfirm1}]  to continue";

            // Item Found Text
            _itemFoundText.Position = _itemEndPos;

            if (_itemType != (int) ItemCategory.GiveNetworkItem)
            {
                _itemFoundText.Y += ItemFoundYOffset;
            }
            else
            {
                _itemFoundText.Y += NetworkItemFoundYOffset;
            }

            _itemFoundText.Scale = Vector2.Zero;
            _itemFoundPlayerText.Position = _itemEndPos;
            _itemFoundPlayerText.Y += ItemFoundPlayerYOffset;
            _itemFoundPlayerText.Scale = Vector2.Zero;

            // Trip Stats
            _tripStat1FoundText.Position = _itemFoundText.Position;
            _tripStat2FoundText.Position = _itemFoundText.Position;
            _tripStat1FoundText.Scale = Vector2.Zero;
            _tripStat2FoundText.Scale = Vector2.Zero;
            _tripStat1FoundText.Visible = false;
            _tripStat2FoundText.Visible = false;
            switch (_itemType)
            {
                case (int) ItemCategory.Blueprint:
                    _itemSpinning = true;
                    _itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                    _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    _itemFoundText.Text =
                        $"{(EquipmentBase) _itemInfo.Y} {((EquipmentCategory) _itemInfo.X).ToString2()}";
                    break;

                case (int) ItemCategory.Rune:
                    _itemSpinning = true;
                    _itemSprite.ChangeSprite("RuneIcon_Sprite");
                    _itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                    _itemFoundText.Text =
                        $"{(EquipmentAbility) _itemInfo.Y} Rune ({((EquipmentCategory) _itemInfo.X).ToString2()})";
                    _itemSprite.AnimationDelay = 0.05f;
                    break;

                case (int) ItemCategory.StatDrop:
                case (int) ItemCategory.TripStatDrop:
                    _itemSprite.ChangeSprite(GetStatSpriteName((int) _itemInfo.X));
                    _itemFoundText.Text = GetStatText((int) _itemInfo.X);
                    _itemSprite.AnimationDelay = 0.05f;
                    _itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                    if (_itemType == 6)
                    {
                        _tripStat1FoundText.Visible = true;
                        _tripStat2FoundText.Visible = true;
                        _tripStat1.ChangeSprite(GetStatSpriteName((int) _tripStatData.X));
                        _tripStat2.ChangeSprite(GetStatSpriteName((int) _tripStatData.Y));
                        _tripStat1.Visible = true;
                        _tripStat2.Visible = true;
                        _tripStat1.AnimationDelay = 0.05f;
                        _tripStat2.AnimationDelay = 0.05f;
                        Tween.RunFunction(0.1f, _tripStat1, "PlayAnimation", true);
                        Tween.RunFunction(0.2f, _tripStat2, "PlayAnimation", true);
                        _tripStat1FoundText.Text = GetStatText((int) _tripStatData.X);
                        _tripStat2FoundText.Text = GetStatText((int) _tripStatData.Y);
                        _itemFoundText.Y += 50f;
                        _tripStat1FoundText.Y = _itemFoundText.Y + 50f;
                    }

                    break;

                case (int) ItemCategory.Spell:
                    _itemSprite.ChangeSprite(((Spell) _itemInfo.X).Icon());
                    _itemFoundSprite.ChangeSprite("SpellFoundText_Sprite");
                    _itemFoundText.Text = ((Spell) _itemInfo.X).ToString();
                    break;

                case (int) ItemCategory.SpecialItem:
                    _itemSprite.ChangeSprite(((SpecialItem) _itemInfo.X).SpriteName());
                    _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    _itemFoundText.Text = ((SpecialItem) _itemInfo.X).ToString();
                    break;

                case 7:
                    _itemSprite.ChangeSprite(GetMedallionImage((int) _itemInfo.X));
                    _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    _itemFoundText.Text = _itemInfo.X == 19f
                        ? "Medallion completed!"
                        : "You've collected a medallion piece!";
                    break;

                case (int) ItemCategory.GiveNetworkItem:
                    _itemSpinning = true;
                    _itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                    _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                    _itemFoundText.Text = Program.Game.ArchipelagoManager.GetItemName(_network_item);
                    _itemFoundPlayerText.Visible = true;
                    _itemFoundPlayerText.Text = $"You found {_network_player}'s";
                    _itemFoundText.TextureColor = Color.Yellow;

                    switch (_network_item.GetItemType())
                    {
                        case ItemType.Rune:
                            _itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                            _itemSprite.ChangeSprite("RuneIcon_Sprite");
                            break;

                        case ItemType.Skill:
                            _itemSpinning = false;
                            _itemSprite.ChangeSprite(GetSkillPlateIcon(_network_item, out _));
                            break;

                        case ItemType.Stats:
                            _itemSpinning = false;
                            _itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                            break;

                        case ItemType.Gold:
                            _itemSpinning = false;
                            _itemSprite.ChangeSprite("MoneyBag_Sprite");
                            break;
                    }

                    break;

                case (int) ItemCategory.ReceiveNetworkItem:
                    _itemFoundPlayerText.Visible = true;
                    switch (_network_item.GetItemType())
                    {
                        case ItemType.Blueprint:
                            _itemFoundText.Y += 40f;
                            _itemSprite.ChangeSprite("BlueprintIcon_Sprite");
                            _itemSpinning = true;
                            _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                            _itemFoundText.Text = GetBlueprintName(_network_item);
                            _itemFoundPlayerText.Text = $"You received from {_network_player}";
                            _itemFoundText.TextureColor = Color.Yellow;
                            break;

                        case ItemType.Rune:
                            _itemFoundText.Y += 40f;
                            _itemSpinning = true;
                            _itemSprite.ChangeSprite("RuneIcon_Sprite");
                            _itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
                            _itemFoundText.Text = Program.Game.ArchipelagoManager.GetItemName(_network_item);
                            _itemFoundPlayerText.Text = $"You received from {_network_player}";
                            _itemSprite.AnimationDelay = 0.05f;
                            break;

                        case ItemType.Skill:
                            _itemFoundText.Y += 40f;
                            _itemSprite.ChangeSprite(GetSkillPlateIcon(_network_item, out var itemName));
                            _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                            _itemFoundText.Text = itemName;
                            _itemFoundPlayerText.Text = $"You received from {_network_player}";
                            _itemFoundText.TextureColor = Color.Yellow;
                            break;

                        case ItemType.Stats:
                            _itemFoundText.Y += 50f;
                            _itemSprite.ChangeSprite(GetStatSpriteName((int) _itemInfo.X));
                            _itemFoundText.Text = GetStatText((int) _itemInfo.X);
                            _itemSprite.AnimationDelay = 0.05f;
                            _itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
                            _itemFoundPlayerText.Text = $"You received from {_network_player}";
                            _tripStat1FoundText.Visible = true;
                            _tripStat2FoundText.Visible = true;
                            _tripStat1.ChangeSprite(GetStatSpriteName((int) _tripStatData.X));
                            _tripStat2.ChangeSprite(GetStatSpriteName((int) _tripStatData.Y));
                            _tripStat1.Visible = true;
                            _tripStat2.Visible = true;
                            _tripStat1.AnimationDelay = 0.05f;
                            _tripStat2.AnimationDelay = 0.05f;
                            Tween.RunFunction(0.1f, _tripStat1, "PlayAnimation", true);
                            Tween.RunFunction(0.2f, _tripStat2, "PlayAnimation", true);
                            _tripStat1FoundText.Text = GetStatText((int) _tripStatData.X);
                            _tripStat2FoundText.Text = GetStatText((int) _tripStatData.Y);
                            _tripStat1FoundText.Y = _itemFoundText.Y + 50f;
                            _tripStat2FoundText.Y = _itemFoundText.Y + 100f;
                            _tripStat1FoundText.TextureColor = Color.Yellow;
                            _tripStat2FoundText.TextureColor = Color.Yellow;

                            break;

                        case ItemType.Gold:
                            _itemFoundText.Y += 40f;
                            _itemSprite.ChangeSprite("MoneyBag_Sprite");
                            _itemSprite.AnimationSpeed = 0;
                            _itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
                            _itemFoundText.Text = $"{(int) _itemInfo.Y} Gold";
                            _itemFoundPlayerText.Text = $"You received from {_network_player}";
                            _itemFoundText.TextureColor = Color.Yellow;
                            break;
                    }

                    break;
            }

            _itemSprite.PlayAnimation();
            ItemSpinAnimation();
            base.OnEnter();
        }

        private string GetBlueprintName(int item)
        {
            var text = Program.Game.ArchipelagoManager.GetItemName(item);
            if (text != ItemDefinitions.ProgressiveArmor.Name)
            {
                return text;
            }

            var index = -1;
            while (index <= 15 && Game.PlayerStats.GetBlueprintArray[0][index + 1] > 0)
            {
                index++;
            }

            return Program.Game.ArchipelagoManager.GetItemName(ItemDefinitions.SquireArmor.Code + index);
        }

        private string GetSkillPlateIcon(int item, out string itemName)
        {
            itemName = Program.Game.ArchipelagoManager.GetItemName(_network_item);

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

            if (item == ItemDefinitions.ProgressiveKnight.Code)
            {
                if (SkillSystem.GetSkill(Skill.KnightUp).CurrentLevel > 0)
                {
                    itemName = "Paladins";
                    return SkillSystem.GetSkill(Skill.KnightUp).IconName.Replace("Locked", "");
                }

                itemName = "Knights";
                return SkillSystem.GetSkill(Skill.KnightUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveMage.Code)
            {
                if (SkillSystem.GetSkill(Skill.MageUp).CurrentLevel > 0)
                {
                    itemName = "Archmages";
                    return SkillSystem.GetSkill(Skill.MageUp).IconName.Replace("Locked", "");
                }

                itemName = "Mages";
                return SkillSystem.GetSkill(Skill.MageUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveBarbarian.Code)
            {
                if (SkillSystem.GetSkill(Skill.BarbarianUp).CurrentLevel > 0)
                {
                    itemName = "Barbarian Kings and Queens";
                    return SkillSystem.GetSkill(Skill.BarbarianUp).IconName.Replace("Locked", "");
                }

                itemName = "Barbarians";
                return SkillSystem.GetSkill(Skill.BarbarianUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveKnave.Code)
            {
                if (SkillSystem.GetSkill(Skill.AssassinUp).CurrentLevel > 0)
                {
                    itemName = "Assassins";
                    return SkillSystem.GetSkill(Skill.AssassinUp).IconName.Replace("Locked", "");
                }

                itemName = "Knaves";
                return SkillSystem.GetSkill(Skill.AssassinUp).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveShinobi.Code)
            {
                if (SkillSystem.GetSkill(Skill.NinjaUp).CurrentLevel > 0)
                {
                    itemName = "Hokages";
                    return SkillSystem.GetSkill(Skill.NinjaUp).IconName.Replace("Locked", "");
                }

                itemName = "Shinobis";
                return SkillSystem.GetSkill(Skill.NinjaUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveMiner.Code)
            {
                if (SkillSystem.GetSkill(Skill.BankerUp).CurrentLevel > 0)
                {
                    itemName = "Spelunkers and Spelunkettes";
                    return SkillSystem.GetSkill(Skill.BankerUp).IconName.Replace("Locked", "");
                }

                itemName = "Miners";
                return SkillSystem.GetSkill(Skill.BankerUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveLich.Code)
            {
                if (SkillSystem.GetSkill(Skill.LichUp).CurrentLevel > 0)
                {
                    itemName = "Lich Kings and Queens";
                    return SkillSystem.GetSkill(Skill.LichUp).IconName.Replace("Locked", "");
                }

                itemName = "Liches";
                return SkillSystem.GetSkill(Skill.LichUnlock).IconName.Replace("Locked", "");
            }

            if (item == ItemDefinitions.ProgressiveSpellthief.Code)
            {
                if (SkillSystem.GetSkill(Skill.SpellSwordUp).CurrentLevel > 0)
                {
                    itemName = "Spellswords";
                    return SkillSystem.GetSkill(Skill.SpellSwordUp).IconName.Replace("Locked", "");
                }

                itemName = "Spellthieves";
                return SkillSystem.GetSkill(Skill.SpellswordUnlock).IconName.Replace("Locked", "");
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
            _itemSprite.Scale = Vector2.One;
            _itemSprite.Position = _itemStartPos;
            _buildUpSound = SoundManager.PlaySound("GetItemBuildupStinger");
            Tween.To(typeof(SoundManager), 1f, Tween.EaseNone, "GlobalMusicVolume",
                (_storedMusicVolume * 0.1f).ToString());
            _itemSprite.Scale = new Vector2(35f / _itemSprite.Height, 35f / _itemSprite.Height);
            Tween.By(_itemSprite, 0.5f, Back.EaseOut, "Y", "-150");
            Tween.RunFunction(0.7f, this, "ItemSpinAnimation2");
            _tripStat1.Scale = Vector2.One;
            _tripStat2.Scale = Vector2.One;
            _tripStat1.Position = _itemStartPos;
            _tripStat2.Position = _itemStartPos;
            Tween.By(_tripStat1, 0.5f, Back.EaseOut, "Y", "-150", "X", "50");
            _tripStat1.Scale = new Vector2(35f / _tripStat1.Height, 35f / _tripStat1.Height);
            Tween.By(_tripStat2, 0.5f, Back.EaseOut, "Y", "-150", "X", "-50");
            _tripStat2.Scale = new Vector2(35f / _tripStat2.Height, 35f / _tripStat2.Height);
        }

        public void ItemSpinAnimation2()
        {
            Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", "GetItemStinger3");
            if (_buildUpSound != null && _buildUpSound.IsPlaying)
            {
                _buildUpSound.Stop(AudioStopOptions.AsAuthored);
            }

            Tween.To(_itemSprite, 0.2f, Quad.EaseOut, "ScaleX", "0.1", "ScaleY", "0.1");
            Tween.AddEndHandlerToLastTween(this, "ItemSpinAnimation3");
            Tween.To(_tripStat1, 0.2f, Quad.EaseOut, "ScaleX", "0.1", "ScaleY", "0.1");
            Tween.To(_tripStat2, 0.2f, Quad.EaseOut, "ScaleX", "0.1", "ScaleY", "0.1");
        }

        public void ItemSpinAnimation3()
        {
            var scale = _itemSprite.Scale;
            _itemSprite.Scale = Vector2.One;
            var num = 130f / _itemSprite.Height;
            _itemSprite.Scale = scale;
            Tween.To(_itemSprite, 0.2f, Tween.EaseNone, "ScaleX", num.ToString(), "ScaleY", num.ToString());
            Tween.To(_itemSprite, 0.2f, Tween.EaseNone, "X", 660.ToString(), "Y", 390.ToString());
            Tween.To(_itemFoundText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_itemFoundPlayerText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_continueText, 0.3f, Linear.EaseNone, "Opacity", "1");
            scale = _tripStat1.Scale;
            _tripStat1.Scale = Vector2.One;
            num = 130f / _tripStat1.Height;
            _tripStat1.Scale = scale;
            Tween.To(_tripStat1, 0.2f, Tween.EaseNone, "ScaleX", num.ToString(), "ScaleY", num.ToString());
            Tween.To(_tripStat1, 0.2f, Tween.EaseNone, "X", 830.ToString(), "Y", 390.ToString());
            scale = _tripStat2.Scale;
            _tripStat2.Scale = Vector2.One;
            num = 130f / _tripStat2.Height;
            _tripStat2.Scale = scale;
            Tween.To(_tripStat2, 0.2f, Tween.EaseNone, "ScaleX", num.ToString(), "ScaleY", num.ToString());
            Tween.To(_tripStat2, 0.2f, Tween.EaseNone, "X", 490.ToString(), "Y", 390.ToString());
            Tween.To(_tripStat1FoundText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_tripStat2FoundText, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            for (var i = 0; i < _levelUpParticles.Length; i++)
            {
                _levelUpParticles[i].AnimationDelay = 0f;
                _levelUpParticles[i].Visible = true;
                _levelUpParticles[i].Scale = new Vector2(0.1f, 0.1f);
                _levelUpParticles[i].Opacity = 0f;
                _levelUpParticles[i].Position = new Vector2(660f, 360f);
                _levelUpParticles[i].Position += new Vector2(CDGMath.RandomInt(-100, 100), CDGMath.RandomInt(-50, 50));
                var duration = CDGMath.RandomFloat(0f, 0.5f);
                Tween.To(_levelUpParticles[i], 0.2f, Linear.EaseNone, "delay", duration.ToString(), "Opacity", "1");
                Tween.To(_levelUpParticles[i], 0.5f, Linear.EaseNone, "delay", duration.ToString(), "ScaleX", "2",
                    "ScaleY", "2");
                Tween.To(_levelUpParticles[i], duration, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(_levelUpParticles[i], "PlayAnimation", false);
            }

            _itemFoundSprite.Position = new Vector2(660f, 190f);
            _itemFoundSprite.Scale = Vector2.Zero;
            _itemFoundSprite.Visible = true;
            Tween.To(_itemFoundSprite, 0.5f, Back.EaseOut, "delay", "0.05", "ScaleX", "1", "ScaleY", "1");
            _levelUpBGImage.Position = _itemFoundSprite.Position;
            _levelUpBGImage.Y += 30f;
            _levelUpBGImage.Scale = Vector2.Zero;
            _levelUpBGImage.Visible = true;
            Tween.To(_levelUpBGImage, 0.5f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            if (_itemSpinning)
            {
                _itemSprite.Rotation = -25f;
            }

            _itemSpinning = false;
            Tween.RunFunction(0.5f, this, "UnlockControls");
        }

        public void UnlockControls()
        {
            _lockControls = false;
        }

        public void ExitScreenTransition()
        {
            if ((int) _itemInfo.X == 19)
            {
                _itemInfo = Vector2.Zero;
                var list = new List<object>();
                list.Add(17);
                Game.ScreenManager.DisplayScreen(19, true, list);
                return;
            }

            _lockControls = true;
            Tween.To(typeof(SoundManager), 1f, Tween.EaseNone, "GlobalMusicVolume", Game.GameConfig.MusicVolume.ToString());
            Tween.To(_itemSprite, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_itemFoundText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(_itemFoundPlayerText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(this, 0.4f, Back.EaseIn, "BackBufferOpacity", "0");
            Tween.To(_levelUpBGImage, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_itemFoundSprite, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(_continueText, 0.4f, Linear.EaseNone, "delay", "0.1", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager as RCScreenManager, "HideCurrentScreen");
            Tween.To(_tripStat1, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_tripStat2, 0.4f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_tripStat1FoundText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
            Tween.To(_tripStat2FoundText, 0.4f, Back.EaseIn, "delay", "0.1", "ScaleX", "0", "ScaleY", "0");
        }

        public override void Update(GameTime gameTime)
        {
            if (_itemSpinning)
            {
                _itemSprite.Rotation += 1200f * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (!_lockControls &&
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
            _levelUpBGImage.Draw(Camera);
            var levelUpParticles = _levelUpParticles;
            foreach (var spriteObj in levelUpParticles)
            {
                spriteObj.Draw(Camera);
            }

            _itemFoundSprite.Draw(Camera);
            _itemFoundText.Draw(Camera);
            _itemFoundPlayerText.Draw(Camera);
            _tripStat1FoundText.Draw(Camera);
            _tripStat2FoundText.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _itemSprite.Draw(Camera);
            _tripStat1.Draw(Camera);
            _tripStat2.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _continueText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        private string GetStatSpriteName(int type)
        {
            return type switch
            {
                4 => "Sword_Sprite",
                5 => "MagicBook_Sprite",
                6 => "Shield_Sprite",
                7 => "Heart_Sprite",
                8 => "ManaCrystal_Sprite",
                9 => "Backpack_Sprite",
                _ => ""
            };
        }

        private string GetStatText(int type)
        {
            return type switch
            {
                (int) ItemDrop.StatStrength  => "Strength Increased: +" + 1,
                (int) ItemDrop.StatMagic     => "Magic Damage Increased: +" + 1,
                (int) ItemDrop.StatDefense   => "Armor Increased: +" + 2,
                (int) ItemDrop.StatMaxHealth => "HP Increased: +" + 5,
                (int) ItemDrop.StatMaxMana   => "MP Increased: +" + 5,
                (int) ItemDrop.StatWeight    => "Max Weight Load Increased: +" + 5,
                _                            => ""
            };
        }

        private string GetMedallionImage(int medallionType)
        {
            return medallionType switch
            {
                15 => "MedallionPiece1_Sprite",
                16 => "MedallionPiece2_Sprite",
                17 => "MedallionPiece3_Sprite",
                18 => "MedallionPiece4_Sprite",
                19 => "MedallionPiece5_Sprite",
                _  => ""
            };
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Get Item Screen");
            _continueText.Dispose();
            _continueText = null;
            _levelUpBGImage.Dispose();
            _levelUpBGImage = null;
            var levelUpParticles = _levelUpParticles;
            foreach (var spriteObj in levelUpParticles)
            {
                spriteObj.Dispose();
            }

            Array.Clear(_levelUpParticles, 0, _levelUpParticles.Length);
            _levelUpParticles = null;
            _buildUpSound = null;
            _itemSprite.Dispose();
            _itemSprite = null;
            _itemFoundSprite.Dispose();
            _itemFoundSprite = null;
            _itemFoundText.Dispose();
            _itemFoundText = null;
            _itemFoundPlayerText.Dispose();
            _itemFoundPlayerText = null;
            _tripStat1.Dispose();
            _tripStat2.Dispose();
            _tripStat1 = null;
            _tripStat2 = null;
            _tripStat1FoundText.Dispose();
            _tripStat2FoundText.Dispose();
            _tripStat1FoundText = null;
            _tripStat2FoundText = null;
            base.Dispose();
        }
    }
}
