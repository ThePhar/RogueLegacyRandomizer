// 
//  Rogue Legacy Randomizer - ProfileCardScreen.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens
{
    public class ProfileCardScreen : Screen
    {
        private TextObj        _author;
        private ObjContainer   _backCard;
        private KeyIconTextObj _cancelText;
        private TextObj        _classDescription;
        private List<TextObj>  _dataList1;
        private List<TextObj>  _dataList2;
        private List<TextObj>  _equipmentList;
        private TextObj        _equipmentTitle;
        private ObjContainer   _frontCard;
        private TextObj        _frontTrait1;
        private TextObj        _frontTrait2;
        private TextObj        _levelClass;
        private Color          _lichColour1 = new(255, 255, 255, 255);
        private Color          _lichColour2 = new(198, 198, 198, 255);
        private TextObj        _money;
        private SpriteObj      _playerBG;
        private PlayerHUDObj   _playerHUD;
        private bool           _playerInAir;
        private TextObj        _playerName;
        private ObjContainer   _playerSprite;
        private TextObj        _playerStats;
        private List<TextObj>  _runeBackDescriptionList;
        private List<TextObj>  _runeBackTitleList;
        private TextObj        _runesTitle;
        private Color          _skinColour1 = new(231, 175, 131, 255);
        private Color          _skinColour2 = new(199, 109, 112, 255);
        private SpriteObj      _spellIcon;
        private SpriteObj      _tombStoneSprite;

        public ProfileCardScreen()
        {
            _equipmentList = new List<TextObj>();
            _runeBackTitleList = new List<TextObj>();
            _runeBackDescriptionList = new List<TextObj>();
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            _frontCard = new ObjContainer("CardFront_Character");
            _frontCard.ForceDraw = true;
            _frontCard.Position = new Vector2(145f, 30f);
            _frontCard.GetChildAt(0).TextureColor = Color.Red;
            _frontCard.GetChildAt(2).TextureColor = Color.Red;
            LoadFrontCard();
            _backCard = new ObjContainer("CardBack_Character");
            _backCard.ForceDraw = true;
            _backCard.Position = new Vector2(_frontCard.X + _backCard.Width + 100f, _frontCard.Y);
            _backCard.AddChild(_playerName.Clone() as GameObj);
            _backCard.GetChildAt(0).TextureColor = Color.Red;
            _backCard.GetChildAt(2).TextureColor = Color.Red;
            LoadBackCard();
            _playerSprite = new ObjContainer("PlayerIdle_Character");
            _playerSprite.ForceDraw = true;
            _playerSprite.Scale = new Vector2(2f, 2f);
            _playerSprite.OutlineWidth = 2;
            _tombStoneSprite = new SpriteObj("Tombstone_Sprite");
            _tombStoneSprite.ForceDraw = true;
            _tombStoneSprite.Scale = new Vector2(3f, 3f);
            _tombStoneSprite.OutlineWidth = 2;
            _spellIcon = new SpriteObj(((SpellType) 12).Icon());
            _spellIcon.Position = new Vector2(350f, 295f);
            _spellIcon.OutlineWidth = 2;
            _spellIcon.ForceDraw = true;
            _cancelText = new KeyIconTextObj(Game.JunicodeFont);
            _cancelText.Text = "to exit options";
            _cancelText.Align = Types.TextAlign.Right;
            _cancelText.DropShadow = new Vector2(2f, 2f);
            _cancelText.FontSize = 12f;
            _cancelText.Position = new Vector2(1290f, 650f);
            _cancelText.ForceDraw = true;
            base.LoadContent();
        }

        private void LoadFrontCard()
        {
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Text = "";
            textObj.FontSize = 10f;
            textObj.ForceDraw = true;
            textObj.TextureColor = Color.Black;
            _playerName = textObj.Clone() as TextObj;
            _playerName.Text = "Sir Archibald the IV";
            _playerName.Position = new Vector2(50f, 43f);
            _frontCard.AddChild(_playerName);
            _money = textObj.Clone() as TextObj;
            _money.Position = new Vector2(_frontCard.GetChildAt(3).X + 30f, _playerName.Y);
            _money.Text = "0";
            _frontCard.AddChild(_money);
            _levelClass = textObj.Clone() as TextObj;
            _levelClass.Text = "Lvl 43 Knight";
            _levelClass.Position = new Vector2(_playerName.X, 370f);
            _frontCard.AddChild(_levelClass);
            _playerBG = new SpriteObj("CardDungeonBG_Sprite");
            _playerBG.Position = new Vector2(45f, 80f);
            _frontCard.AddChildAt(1, _playerBG);
            _playerHUD = new PlayerHUDObj();
            _playerHUD.ForceDraw = true;
            _playerHUD.ShowBarsOnly = true;
            _playerHUD.SetPosition(new Vector2(_frontCard.X + 46f, _frontCard.Y + 64f));
            _frontCard.AddChild(_playerHUD);
            _frontTrait1 = new TextObj(Game.JunicodeFont);
            _frontTrait1.FontSize = 7f;
            _frontTrait1.TextureColor = Color.Black;
            _frontTrait1.Position = new Vector2(50f, 550f);
            _frontTrait1.Text = "Color Blind";
            _frontCard.AddChild(_frontTrait1);
            _frontTrait2 = _frontTrait1.Clone() as TextObj;
            _frontTrait2.Y -= 20f;
            _frontTrait2.Text = "Myopic";
            _frontCard.AddChild(_frontTrait2);
            _classDescription = new TextObj(Game.JunicodeFont);
            _classDescription.FontSize = 8f;
            _classDescription.TextureColor = Color.Black;
            _classDescription.Text = "0";
            _classDescription.Position = new Vector2(50f, 410f);
            _frontCard.AddChild(_classDescription);
            _author = new TextObj(Game.JunicodeFont);
            _author.FontSize = 8f;
            _author.TextureColor = Color.White;
            _author.Text = "Glauber Kotaki";
            _author.X = _playerName.X;
            _author.Y = 590f;
            _frontCard.AddChild(_author);
            _playerStats = textObj.Clone() as TextObj;
            _playerStats.Text = "10/10";
            _playerStats.Align = Types.TextAlign.Centre;
            _playerStats.Position = new Vector2(387f, 579f);
            _frontCard.AddChild(_playerStats);
        }

        private void LoadBackCard()
        {
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Text = "";
            textObj.FontSize = 9f;
            textObj.ForceDraw = true;
            textObj.TextureColor = Color.Black;
            _dataList1 = new List<TextObj>();
            _dataList2 = new List<TextObj>();
            string[] array =
            {
                "Health",
                "Mana",
                "Armor",
                "Weight"
            };
            string[] array2 =
            {
                "Strength",
                "Intelligence",
                "Crit. Chance",
                "Crit. Damage"
            };
            var num = 90;
            for (var i = 0; i < array.Length; i++)
            {
                var textObj2 = textObj.Clone() as TextObj;
                textObj2.Align = Types.TextAlign.Right;
                textObj2.Text = array[i];
                textObj2.Position = new Vector2(120f, num);
                _backCard.AddChild(textObj2);
                var textObj3 = textObj.Clone() as TextObj;
                textObj3.Text = "0";
                textObj3.Position = new Vector2(textObj2.X + 20f, num);
                _dataList1.Add(textObj3);
                _backCard.AddChild(textObj3);
                var textObj4 = textObj.Clone() as TextObj;
                textObj4.Align = Types.TextAlign.Right;
                textObj4.Text = array2[i];
                textObj4.Position = new Vector2(330f, num);
                _backCard.AddChild(textObj4);
                var textObj5 = textObj.Clone() as TextObj;
                textObj5.Text = "0";
                textObj5.Position = new Vector2(textObj4.X + 20f, num);
                _dataList2.Add(textObj5);
                _backCard.AddChild(textObj5);
                num += 20;
            }

            _equipmentTitle = textObj.Clone() as TextObj;
            _equipmentTitle.FontSize = 12f;
            _equipmentTitle.Text = "Equipment:";
            _equipmentTitle.Position = new Vector2(50f, 180f);
            _backCard.AddChild(_equipmentTitle);
            _runesTitle = textObj.Clone() as TextObj;
            _runesTitle.FontSize = 12f;
            _runesTitle.Text = "Enchantments:";
            _runesTitle.Position = new Vector2(_equipmentTitle.X, 330f);
            _backCard.AddChild(_runesTitle);
            for (var j = 0; j < Game.PlayerStats.GetEquippedArray.Length; j++)
            {
                var textObj6 = textObj.Clone() as TextObj;
                textObj6.Text = "test";
                textObj6.Position = new Vector2(80f, _equipmentTitle.Y + 50f);
                _equipmentList.Add(textObj6);
                _backCard.AddChild(textObj6);
            }

            for (var k = 0; k < 10; k++)
            {
                var textObj7 = textObj.Clone() as TextObj;
                textObj7.X = 60f;
                textObj7.Text = ((EquipmentAbility) k).ToString();
                textObj7.FontSize = 7f;
                _runeBackTitleList.Add(textObj7);
                _backCard.AddChild(textObj7);
                var textObj8 = textObj.Clone() as TextObj;
                textObj8.X = textObj7.Bounds.Right + 10;
                textObj8.FontSize = 7f;
                _runeBackDescriptionList.Add(textObj8);
                _backCard.AddChild(textObj8);
            }

            var textObj9 = textObj.Clone() as TextObj;
            textObj9.X = 60f;
            textObj9.Text = ((EquipmentAbility) 20).ToString();
            textObj9.FontSize = 7f;
            _runeBackTitleList.Add(textObj9);
            _backCard.AddChild(textObj9);
            var textObj10 = textObj.Clone() as TextObj;
            textObj10.X = textObj9.Bounds.Right + 10;
            textObj10.FontSize = 7f;
            _runeBackDescriptionList.Add(textObj10);
            _backCard.AddChild(textObj10);
            var textObj11 = textObj.Clone() as TextObj;
            textObj11.X = 60f;
            textObj11.Text = ((EquipmentAbility) 21).ToString();
            textObj11.FontSize = 7f;
            _runeBackTitleList.Add(textObj11);
            _backCard.AddChild(textObj11);
            var textObj12 = textObj.Clone() as TextObj;
            textObj12.X = textObj11.Bounds.Right + 10;
            textObj12.FontSize = 7f;
            _runeBackDescriptionList.Add(textObj12);
            _backCard.AddChild(textObj12);
        }

        public override void OnEnter()
        {
            SoundManager.PlaySound("StatCard_In");
            LoadCardColour();
            _spellIcon.ChangeSprite(((SpellType) Game.PlayerStats.Spell).Icon());
            string[] array =
            {
                "CardCastleBG_Sprite",
                "CardGardenBG_Sprite",
                "CardDungeonBG_Sprite",
                "CardTowerBG_Sprite"
            };
            _playerBG.ChangeSprite(array[CDGMath.RandomInt(0, 3)]);
            _frontCard.Y = 1500f;
            _backCard.Y = 1500f;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.7");
            Tween.To(_frontCard, 0.4f, Back.EaseOut, "Y", "30");
            Tween.To(_backCard, 0.4f, Back.EaseOut, "delay", "0.2", "Y", "30");
            var player = (ScreenManager as RCScreenManager).Player;
            LoadFrontCardStats(player);
            LoadBackCardStats(player);
            ChangeParts(player);
            _playerHUD.Update(player);
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                _cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
            }
            else
            {
                _cancelText.ForcedScale = new Vector2(1f, 1f);
            }

            _cancelText.Text = "[Input:" + 2 + "] to exit profile card";
            _cancelText.Opacity = 0f;
            Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
            string[] array2 =
            {
                "Glauber Kotaki",
                "Kenny Lee",
                "Teddy Lee",
                "Gordon McGladdery",
                "Judson Cowan"
            };
            _author.Text = array2[CDGMath.RandomInt(0, array2.Length - 1)];
            Array.Clear(array2, 0, array2.Length);
            base.OnEnter();
        }

        private void ChangeParts(PlayerObj player)
        {
            string[] array;
            if (Game.PlayerStats.Class == 16)
            {
                array = new[]
                {
                    "Idle",
                    "Walking",
                    "LevelUp",
                    "Dash",
                    "FrontDash"
                };
            }
            else
            {
                array = new[]
                {
                    "Idle",
                    "Attacking3",
                    "Walking",
                    "LevelUp",
                    "Dash",
                    "FrontDash"
                };
            }

            string[] array2;
            if (Game.PlayerStats.Class == 16)
            {
                array2 = new[]
                {
                    "Jumping",
                    "Falling"
                };
            }
            else
            {
                array2 = new[]
                {
                    "Jumping",
                    "AirAttack",
                    "Falling"
                };
            }

            if (CDGMath.RandomInt(0, 1) == 0)
            {
                _playerInAir = true;
                SetPlayerStyle(array2[CDGMath.RandomInt(0, array2.Length - 1)]);
            }
            else
            {
                _playerInAir = false;
                SetPlayerStyle(array[CDGMath.RandomInt(0, array.Length - 1)]);
            }

            for (var i = 0; i < player.NumChildren; i++)
            {
                var spriteObj = player.GetChildAt(i) as SpriteObj;
                var spriteObj2 = _playerSprite.GetChildAt(i) as SpriteObj;
                spriteObj2.TextureColor = spriteObj.TextureColor;
            }

            var text = (_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
            var startIndex = text.IndexOf("_") - 1;
            text = text.Remove(startIndex, 1);
            if (Game.PlayerStats.Class == 16)
            {
                text = text.Replace("_", 6 + "_");
            }
            else if (Game.PlayerStats.Class == 17)
            {
                text = text.Replace("_", 7 + "_");
            }
            else
            {
                text = text.Replace("_", Game.PlayerStats.HeadPiece + "_");
            }

            _playerSprite.GetChildAt(12).ChangeSprite(text);
            var text2 = (_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
            startIndex = text2.IndexOf("_") - 1;
            text2 = text2.Remove(startIndex, 1);
            text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
            _playerSprite.GetChildAt(4).ChangeSprite(text2);
            var text3 = (_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
            startIndex = text3.IndexOf("_") - 1;
            text3 = text3.Remove(startIndex, 1);
            text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
            _playerSprite.GetChildAt(9).ChangeSprite(text3);
            var text4 = (_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
            startIndex = text4.IndexOf("_") - 1;
            text4 = text4.Remove(startIndex, 1);
            text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
            _playerSprite.GetChildAt(3).ChangeSprite(text4);
        }

        public void SetPlayerStyle(string animationType)
        {
            _playerSprite.ChangeSprite("Player" + animationType + "_Character");
            var player = (ScreenManager as RCScreenManager).Player;
            for (var i = 0; i < _playerSprite.NumChildren; i++)
            {
                _playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
                _playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
            }

            _playerSprite.GetChildAt(16).Visible = false;
            _playerSprite.Scale = player.Scale;
            if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
            {
                _playerSprite.GetChildAt(7).Visible = false;
            }

            _playerSprite.GetChildAt(14).Visible = false;
            if (Game.PlayerStats.SpecialItem == 8)
            {
                _playerSprite.GetChildAt(14).Visible = true;
            }

            if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Shield_Sprite");
            }
            else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Lamp_Sprite");
            }
            else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Beard_Sprite");
            }
            else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Headband_Sprite");
            }
            else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Horns_Sprite");
            }
            else
            {
                _playerSprite.GetChildAt(15).Visible = false;
            }

            _playerSprite.GetChildAt(0).Visible = false;
            if (Game.PlayerStats.Class == 16)
            {
                _playerSprite.GetChildAt(0).Visible = true;
                _playerSprite.GetChildAt(12)
                    .ChangeSprite(string.Concat("Player", animationType, "Head", 6, "_Sprite"));
            }

            if (!Game.PlayerStats.IsFemale)
            {
                _playerSprite.GetChildAt(5).Visible = false;
                _playerSprite.GetChildAt(13).Visible = false;
            }
            else
            {
                _playerSprite.GetChildAt(5).Visible = true;
                _playerSprite.GetChildAt(13).Visible = true;
            }

            if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
            {
                _playerSprite.Scale = new Vector2(3f, 3f);
            }

            if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
            {
                _playerSprite.Scale = new Vector2(1.35f, 1.35f);
            }

            if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
            {
                _playerSprite.ScaleX *= 0.825f;
                _playerSprite.ScaleY *= 1.25f;
            }

            if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
            {
                _playerSprite.ScaleX *= 1.25f;
                _playerSprite.ScaleY *= 1.175f;
            }

            if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
            {
                _playerSprite.OutlineColour = Color.White;
            }
            else
            {
                _playerSprite.OutlineColour = Color.Black;
            }

            _playerSprite.CalculateBounds();
            _playerSprite.Y = 435f - (_playerSprite.Bounds.Bottom - _playerSprite.Y);
        }

        public void ExitScreenTransition()
        {
            SoundManager.PlaySound("StatCard_Out");
            Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
            _frontCard.Y = 30f;
            _backCard.Y = 30f;
            Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.3", "BackBufferOpacity", "0");
            Tween.To(_frontCard, 0.4f, Back.EaseIn, "Y", "1500");
            Tween.To(_backCard, 0.4f, Back.EaseIn, "delay", "0.2", "Y", "1500");
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
            OnExit();
        }

        private void LoadCardColour()
        {
            var textureColor = Color.Red;
            var textureColor2 = Color.Red;
            switch (Game.PlayerStats.Class)
            {
                case 0:
                case 8:
                    textureColor2 = textureColor = Color.White;
                    break;

                case 1:
                case 9:
                    textureColor2 = textureColor = Color.Blue;
                    break;

                case 2:
                case 10:
                    textureColor2 = textureColor = Color.Red;
                    break;

                case 3:
                case 11:
                    textureColor2 = textureColor = Color.Green;
                    break;

                case 4:
                case 12:
                    textureColor2 = textureColor = Color.Gray;
                    break;

                case 5:
                case 13:
                    textureColor2 = textureColor = Color.Gold;
                    break;

                case 6:
                case 14:
                    textureColor = Color.Blue;
                    textureColor2 = Color.Red;
                    break;

                case 7:
                case 15:
                    textureColor2 = textureColor = Color.Black;
                    break;

                case 16:
                    textureColor = Color.White;
                    textureColor2 = Color.Green;
                    break;
            }

            _frontCard.GetChildAt(0).TextureColor = textureColor;
            _frontCard.GetChildAt(3).TextureColor = textureColor;
            _backCard.GetChildAt(0).TextureColor = textureColor2;
            _backCard.GetChildAt(2).TextureColor = textureColor2;
            _frontCard.GetChildAt(2).TextureColor = new Color(235, 220, 185);
            _backCard.GetChildAt(1).TextureColor = new Color(235, 220, 185);
        }

        private void LoadFrontCardStats(PlayerObj player)
        {
            _frontTrait1.Visible = false;
            _frontTrait2.Visible = false;
            var b = (byte) Game.PlayerStats.Traits.X;
            if (b != 0)
            {
                _frontTrait1.Text = ((Trait) b) + ": " + ((Trait) b).ProfileCardDescription();
                _frontTrait1.Visible = true;
            }

            var b2 = (byte) Game.PlayerStats.Traits.Y;
            if (b2 != 0)
            {
                _frontTrait2.Y = _frontTrait1.Y;
                if (b != 0)
                {
                    _frontTrait2.Y -= 20f;
                }

                _frontTrait2.Text = ((Trait) b2) + ": " + ((Trait) b2).ProfileCardDescription();
                _frontTrait2.Visible = true;
            }

            _playerName.Text = Game.PlayerStats.PlayerName;
            _playerStats.Text = (int) (player.Damage / 20f) + "/" + (int) (player.MaxHealth / 50f);
            _levelClass.Text = string.Concat("Lv. ", Game.PlayerStats.CurrentLevel, " - ",
                ((ClassType) Game.PlayerStats.Class).Name(Game.PlayerStats.IsFemale));
            _money.Text = Game.PlayerStats.Gold.ToString();
            _classDescription.Text = ((ClassType) Game.PlayerStats.Class).ProfileCardDescription();
        }

        private void LoadBackCardStats(PlayerObj player)
        {
            for (var i = 0; i < _dataList1.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        _dataList1[i].Text = player.MaxHealth.ToString();
                        _dataList2[i].Text = player.Damage.ToString();
                        break;

                    case 1:
                        _dataList1[i].Text = player.MaxMana.ToString();
                        _dataList2[i].Text = player.TotalMagicDamage.ToString();
                        break;

                    case 2:
                    {
                        _dataList1[i].Text = string.Concat(player.TotalArmor.ToString(), "(",
                            (int) (player.TotalDamageReduc * 100f), "%)");
                        var num = player.TotalCritChance * 100f;
                        _dataList2[i].Text = (int) Math.Round(num, MidpointRounding.AwayFromZero) + "%";
                        break;
                    }

                    case 3:
                        _dataList1[i].Text = player.CurrentWeight + "/" + player.MaxWeight;
                        _dataList2[i].Text = (int) (player.TotalCriticalDamage * 100f) + "%";
                        break;
                }
            }

            var getEquippedArray = Game.PlayerStats.GetEquippedArray;
            var num2 = (int) _equipmentTitle.Y + 40;
            for (var j = 0; j < Game.PlayerStats.GetEquippedArray.Length; j++)
            {
                _equipmentList[j].Visible = false;
                _equipmentList[j].Y = num2;
                if (getEquippedArray[j] != -1)
                {
                    _equipmentList[j].Text = (EquipmentBase) getEquippedArray[j] + " " +
                                              ((EquipmentCategory) j).AltName();
                    _equipmentList[j].Visible = true;
                    num2 += 20;
                }
            }

            num2 = (int) _runesTitle.Y + 40;
            for (var k = 0; k < _runeBackTitleList.Count; k++)
            {
                _runeBackTitleList[k].Y = num2;
                _runeBackDescriptionList[k].Y = num2;
                _runeBackTitleList[k].Visible = false;
                _runeBackDescriptionList[k].Visible = false;
                var num3 = 0f;
                switch (k)
                {
                    case 0:
                        num3 = player.TotalDoubleJumps;
                        break;

                    case 1:
                        num3 = player.TotalAirDashes;
                        break;

                    case 2:
                        num3 = player.TotalVampBonus;
                        break;

                    case 3:
                        num3 = player.TotalFlightTime;
                        break;

                    case 4:
                        num3 = player.ManaGain;
                        break;

                    case 5:
                        num3 = player.TotalDamageReturn * 100f;
                        break;

                    case 6:
                        num3 = player.TotalGoldBonus * 100f;
                        break;

                    case 7:
                        num3 = player.TotalMovementSpeedPercent * 100f - 100f;
                        break;

                    case 8:
                        num3 = Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8;
                        break;

                    case 9:
                        num3 = Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f;
                        break;
                }

                if (num3 > 0f)
                {
                    _runeBackDescriptionList[k].Text = "(" + ((EquipmentAbility) k).ShortDescription(num3) + ")";
                    _runeBackTitleList[k].Visible = true;
                    _runeBackDescriptionList[k].Visible = true;
                    num2 += 20;
                }
            }

            if (Game.PlayerStats.HasArchitectFee)
            {
                _runeBackDescriptionList[_runeBackDescriptionList.Count - 2].Text = "(" +
                                                                                      ((EquipmentAbility) 20).ShortDescription(0f) + ")";
                _runeBackDescriptionList[_runeBackDescriptionList.Count - 2].Visible = true;
                _runeBackTitleList[_runeBackDescriptionList.Count - 2].Visible = true;
                num2 += 20;
            }

            if (Game.PlayerStats.TimesCastleBeaten > 0)
            {
                _runeBackDescriptionList[_runeBackDescriptionList.Count - 1].Text = "(" +
                                                                                      ((EquipmentAbility) 21).ShortDescription(50 * Game.PlayerStats.TimesCastleBeaten) +
                                                                                      ")";
                _runeBackDescriptionList[_runeBackDescriptionList.Count - 1].Visible = true;
                _runeBackTitleList[_runeBackDescriptionList.Count - 1].Visible = true;
                if (Game.PlayerStats.HasArchitectFee)
                {
                    _runeBackDescriptionList[_runeBackDescriptionList.Count - 1].Y = num2;
                    _runeBackTitleList[_runeBackDescriptionList.Count - 1].Y = num2;
                }
            }

            (_backCard.GetChildAt(3) as TextObj).Text = Game.PlayerStats.PlayerName;
        }

        public override void HandleInput()
        {
            if (_frontCard.Y == 30f &&
                (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) || Game.GlobalInput.JustPressed(7)))
            {
                ExitScreenTransition();
            }

            base.HandleInput();
        }

        public override void Draw(GameTime gametime)
        {
            _playerHUD.SetPosition(new Vector2(_frontCard.X + 46f, _frontCard.Y + 64f));
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _frontCard.Draw(Camera);
            _backCard.Draw(Camera);
            _cancelText.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            if (Game.PlayerStats.IsDead)
            {
                _tombStoneSprite.Position = new Vector2(_frontCard.X + 240f, _frontCard.Y + 280f);
                _tombStoneSprite.Draw(Camera);
            }
            else
            {
                if (_playerInAir)
                {
                    _playerSprite.Position = new Vector2(_frontCard.X + 180f, _frontCard.Y + 202f);
                }
                else
                {
                    _playerSprite.Position = new Vector2(_frontCard.X + 160f,
                        _frontCard.Y + 280f - (_playerSprite.Bounds.Bottom - _playerSprite.Y));
                }

                _playerSprite.Draw(Camera);
                Game.ColourSwapShader.Parameters["desiredTint"].SetValue(
                    _playerSprite.GetChildAt(12).TextureColor.ToVector4());
                if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
                {
                    Game.ColourSwapShader.Parameters["Opacity"].SetValue(_playerSprite.Opacity);
                    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(_skinColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(_lichColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(_skinColour2.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(_lichColour2.ToVector4());
                }
                else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
                {
                    Game.ColourSwapShader.Parameters["Opacity"].SetValue(_playerSprite.Opacity);
                    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(_skinColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(_skinColour2.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
                }
                else
                {
                    Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
                    Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(_skinColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(_skinColour1.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(_skinColour2.ToVector4());
                    Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(_skinColour2.ToVector4());
                }

                Camera.End();
                Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                    Game.ColourSwapShader);
                _playerSprite.GetChildAt(12).Draw(Camera);
                Camera.End();
                Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                    null);
                if (Game.PlayerStats.IsFemale)
                {
                    _playerSprite.GetChildAt(13).Draw(Camera);
                }

                _playerSprite.GetChildAt(15).Draw(Camera);
            }

            _spellIcon.Position = new Vector2(_frontCard.X + 380f, _frontCard.Y + 320f);
            _spellIcon.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Profile Card Screen");
                _frontCard.Dispose();
                _frontCard = null;
                _backCard.Dispose();
                _backCard = null;
                _playerName = null;
                _money = null;
                _levelClass = null;
                _playerHUD = null;
                _frontTrait1 = null;
                _frontTrait2 = null;
                _playerBG = null;
                _classDescription = null;
                _author = null;
                _playerStats = null;
                _equipmentTitle = null;
                _runesTitle = null;
                _equipmentList.Clear();
                _equipmentList = null;
                _runeBackTitleList.Clear();
                _runeBackTitleList = null;
                _runeBackDescriptionList.Clear();
                _runeBackDescriptionList = null;
                _playerSprite.Dispose();
                _playerSprite = null;
                _spellIcon.Dispose();
                _spellIcon = null;
                _tombStoneSprite.Dispose();
                _tombStoneSprite = null;
                _cancelText.Dispose();
                _cancelText = null;
                _dataList1.Clear();
                _dataList1 = null;
                _dataList2.Clear();
                _dataList2 = null;
                base.Dispose();
            }
        }
    }
}
