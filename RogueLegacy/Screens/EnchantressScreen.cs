// Rogue Legacy Randomizer - EnchantressScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class EnchantressScreen : Screen
    {
        private const int STARTING_CATEGORY_INDEX = 6;

        private SpriteObj[]       _activeIconArray;
        private KeyIconTextObj    _cancelText;
        private KeyIconTextObj    _confirmText;
        private int               _currentCategoryIndex;
        private int               _currentEquipmentIndex;
        private TextObj           _descriptionText;
        private ObjContainer      _enchantressUI;
        private TextObj           _equipmentDescriptionText;
        private TextObj           _equipmentTitleText;
        private SpriteObj         _equippedIcon;
        private bool              _inCategoryMenu = true;
        private KeyIconTextObj    _instructionsText;
        private TextObj           _instructionsTitleText;
        private bool              _lockControls;
        private List<SpriteObj[]> _masterIconArray;
        private KeyIconTextObj    _navigationText;
        private List<SpriteObj>   _newIconList;
        private int               _newIconListIndex;
        private TextObj           _playerMoney;
        private Cue               _rainSound;
        private SpriteObj         _selectionIcon;
        private ObjContainer      _unlockCostContainer;

        public EnchantressScreen()
        {
            _currentCategoryIndex = STARTING_CATEGORY_INDEX;
            _masterIconArray = new List<SpriteObj[]>();
            for (var i = 0; i < 5; i++)
            {
                _masterIconArray.Add(new SpriteObj[11]);
            }
        }

        public float     BackBufferOpacity { get; set; }
        public PlayerObj Player            { get; set; }

        private int CurrentCategoryIndex => _currentCategoryIndex - 6;

        public override void LoadContent()
        {
            _enchantressUI = new ObjContainer("BlacksmithUI_Character");
            _enchantressUI.Position = new Vector2(660f, 360f);
            _playerMoney = new TextObj(Game.GoldFont);
            _playerMoney.Align = Types.TextAlign.Left;
            _playerMoney.Text = "1000";
            _playerMoney.FontSize = 30f;
            _playerMoney.OverrideParentScale = true;
            _playerMoney.Position = new Vector2(210f, -225f);
            _playerMoney.AnchorY = 10f;
            _enchantressUI.AddChild(_playerMoney);
            _enchantressUI.GetChildAt(_enchantressUI.NumChildren - 3).ChangeSprite("EnchantressUI_Title_Sprite");
            for (var i = 0; i < _enchantressUI.NumChildren; i++)
            {
                _enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
            }

            _selectionIcon = new SpriteObj("BlacksmithUI_SelectionIcon_Sprite");
            _selectionIcon.PlayAnimation();
            _selectionIcon.Scale = Vector2.Zero;
            _selectionIcon.AnimationDelay = 0.1f;
            _selectionIcon.ForceDraw = true;
            _equipmentDescriptionText = new TextObj(Game.JunicodeFont);
            _equipmentDescriptionText.Align = Types.TextAlign.Centre;
            _equipmentDescriptionText.FontSize = 12f;
            _equipmentDescriptionText.Position = new Vector2(230f, -20f);
            _equipmentDescriptionText.Text = "Select a category";
            _equipmentDescriptionText.WordWrap(190);
            _equipmentDescriptionText.Scale = Vector2.Zero;
            _enchantressUI.AddChild(_equipmentDescriptionText);
            foreach (var current in _masterIconArray)
            {
                var absPosition = _enchantressUI.GetChildAt(6).AbsPosition;
                absPosition.X += 85f;
                var x = absPosition.X;
                var num = 70f;
                var num2 = 80f;
                for (var j = 0; j < current.Length; j++)
                {
                    current[j] = new SpriteObj("BlacksmithUI_QuestionMarkIcon_Sprite");
                    current[j].Position = absPosition;
                    current[j].Scale = Vector2.Zero;
                    current[j].ForceDraw = true;
                    absPosition.X += num;
                    if (absPosition.X > x + num * 4f)
                    {
                        absPosition.X = x;
                        absPosition.Y += num2;
                    }
                }
            }

            InitializeTextObjs();
            _equippedIcon = new SpriteObj("BlacksmithUI_EquippedIcon_Sprite");
            _confirmText = new KeyIconTextObj(Game.JunicodeFont);
            _confirmText.Text = "to close map";
            _confirmText.FontSize = 12f;
            _confirmText.Position = new Vector2(50f, 550f);
            _confirmText.ForceDraw = true;
            _cancelText = new KeyIconTextObj(Game.JunicodeFont);
            _cancelText.Text = "to re-center on player";
            _cancelText.FontSize = 12f;
            _cancelText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40f);
            _cancelText.ForceDraw = true;
            _navigationText = new KeyIconTextObj(Game.JunicodeFont);
            _navigationText.Text = "to move map";
            _navigationText.FontSize = 12f;
            _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 80f);
            _navigationText.ForceDraw = true;
            _newIconList = new List<SpriteObj>();
            for (var k = 0; k < 55; k++)
            {
                var spriteObj = new SpriteObj("BlacksmithUI_NewIcon_Sprite");
                spriteObj.Visible = false;
                spriteObj.Scale = new Vector2(1.1f, 1.1f);
                _newIconList.Add(spriteObj);
            }

            base.LoadContent();
        }

        private void InitializeTextObjs()
        {
            _descriptionText = new TextObj(Game.JunicodeFont);
            _descriptionText.FontSize = 9f;
            _instructionsTitleText = new TextObj();
            _instructionsTitleText.Font = Game.JunicodeFont;
            _instructionsTitleText.FontSize = 10f;
            _instructionsTitleText.TextureColor = new Color(237, 202, 138);
            _instructionsTitleText.Text = "Instructions:";
            _instructionsText = new KeyIconTextObj();
            _instructionsText.Font = Game.JunicodeFont;
            _instructionsText.FontSize = 10f;
            _unlockCostContainer = new ObjContainer();
            var textObj = new TextObj();
            textObj.Font = Game.JunicodeFont;
            textObj.FontSize = 10f;
            textObj.TextureColor = Color.Yellow;
            textObj.Position = new Vector2(50f, 9f);
            _unlockCostContainer.AddChild(new SpriteObj("BlacksmithUI_CoinBG_Sprite"));
            _unlockCostContainer.AddChild(textObj);
            _descriptionText.Position = new Vector2(_enchantressUI.X + 140f,
                _enchantressUI.Y - _enchantressUI.Height / 2 + 20f + 40f);
            _instructionsTitleText.Position = new Vector2(_enchantressUI.X + 140f,
                _descriptionText.Bounds.Bottom + 20);
            _instructionsText.Position = new Vector2(_instructionsTitleText.X, _instructionsTitleText.Bounds.Bottom);
            _unlockCostContainer.Position = new Vector2(_enchantressUI.X + 114f, 485f);
            _equipmentTitleText = new TextObj(Game.JunicodeFont);
            _equipmentTitleText.ForceDraw = true;
            _equipmentTitleText.FontSize = 10f;
            _equipmentTitleText.DropShadow = new Vector2(2f, 2f);
            _equipmentTitleText.TextureColor = new Color(237, 202, 138);
            _equipmentTitleText.Position = new Vector2(_enchantressUI.X + 140f, _descriptionText.Y - 50f);
            _descriptionText.Visible = false;
            _instructionsTitleText.Visible = false;
            _instructionsText.Visible = false;
            _unlockCostContainer.Visible = false;
        }

        private void DisplayCategory(int equipmentType)
        {
            var duration = 0.2f;
            var num = 0f;
            if (_activeIconArray != null)
            {
                for (var i = 0; i < 11; i++)
                {
                    Tween.StopAllContaining(_activeIconArray[i], false);
                    Tween.To(_activeIconArray[i], duration, Back.EaseIn, "delay", num.ToString(), "ScaleX", "0",
                        "ScaleY", "0");
                }
            }

            _activeIconArray = _masterIconArray[equipmentType];
            num = 0.25f;
            for (var j = 0; j < 11; j++)
            {
                Tween.To(_activeIconArray[j], duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY",
                    "1");
            }

            foreach (var current in _newIconList)
            {
                Tween.StopAllContaining(current, false);
                current.Scale = Vector2.Zero;
                Tween.To(current, duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY", "1");
            }

            UpdateNewIcons();
            _equippedIcon.Scale = Vector2.Zero;
            Tween.StopAllContaining(_equippedIcon, false);
            Tween.To(_equippedIcon, duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY", "1");
        }

        public void EaseInMenu()
        {
            var duration = 0.4f;
            Tween.To(_enchantressUI.GetChildAt(0), duration, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_selectionIcon, duration, Back.EaseOut, "delay", "0.25", "ScaleX", "1", "ScaleY", "1");
            var num = 0.2f;
            for (var i = 6; i < _enchantressUI.NumChildren - 3; i++)
            {
                num += 0.05f;
                Tween.To(_enchantressUI.GetChildAt(i), duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1",
                    "ScaleY", "1");
            }

            Tween.To(_enchantressUI.GetChildAt(_enchantressUI.NumChildren - 1), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.To(_enchantressUI.GetChildAt(_enchantressUI.NumChildren - 2), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.To(_enchantressUI.GetChildAt(_enchantressUI.NumChildren - 3), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.AddEndHandlerToLastTween(this, "EaseInComplete");
        }

        public void EaseInComplete()
        {
            _lockControls = false;
        }

        private void EaseOutMenu()
        {
            foreach (var current in _newIconList)
            {
                current.Visible = false;
            }

            _equippedIcon.Visible = false;
            Tween.To(_confirmText, 0.2f, Linear.EaseNone, "Opacity", "0");
            Tween.To(_cancelText, 0.2f, Linear.EaseNone, "Opacity", "0");
            Tween.To(_navigationText, 0.2f, Linear.EaseNone, "Opacity", "0");
            var num = 0.4f;
            var num2 = 0f;
            Tween.To(_enchantressUI.GetChildAt(_enchantressUI.NumChildren - 2), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            Tween.To(_enchantressUI.GetChildAt(_enchantressUI.NumChildren - 3), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            Tween.To(_enchantressUI.GetChildAt(_enchantressUI.NumChildren - 4), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            for (var i = 6; i < 11; i++)
            {
                if (_currentCategoryIndex == i)
                {
                    Tween.To(_selectionIcon, num, Back.EaseIn, "delay", num2.ToString(), "ScaleX", "0", "ScaleY", "0");
                }

                Tween.To(_enchantressUI.GetChildAt(i), num, Back.EaseIn, "delay", num2.ToString(), "ScaleX", "0",
                    "ScaleY", "0");
                num2 += 0.05f;
            }

            for (var j = 1; j < 6; j++)
            {
                _enchantressUI.GetChildAt(j).Scale = Vector2.Zero;
            }

            for (var k = 0; k < _activeIconArray.Length; k++)
            {
                Tween.To(_activeIconArray[k], num, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            }

            Tween.To(_enchantressUI.GetChildAt(0), num, Back.EaseIn, "delay", "0.3", "ScaleX", "0", "ScaleY", "0");
            Tween.RunFunction(num + 0.35f, ScreenManager, "HideCurrentScreen");
        }

        private void UpdateIconStates()
        {
            for (var i = 0; i < Game.PlayerStats.GetRuneArray.Count; i++)
            for (var j = 0; j < Game.PlayerStats.GetRuneArray[i].Length; j++)
            {
                var b = Game.PlayerStats.GetRuneArray[i][j];
                if (b == 0)
                {
                    _masterIconArray[i][j].ChangeSprite("BlacksmithUI_QuestionMarkIcon_Sprite");
                }
                else
                {
                    _masterIconArray[i][j].ChangeSprite(((EquipmentAbility) j).Icon());
                    _masterIconArray[i][j].Opacity = 0.2f;
                }

                if (b >= 3)
                {
                    _masterIconArray[i][j].Opacity = 1f;
                }
            }
        }

        public override void OnEnter()
        {
            if (_rainSound != null)
            {
                _rainSound.Dispose();
            }

            if (DateTime.Now.Month != 12 && DateTime.Now.Month != 1)
            {
                _rainSound = SoundManager.PlaySound("Rain1_Filtered");
            }
            else
            {
                _rainSound = SoundManager.PlaySound("snowloop_filtered");
            }

            if (Game.PlayerStats.TotalRunesFound >= 55)
            {
                GameUtil.UnlockAchievement("LOVE_OF_KNOWLEDGE");
            }

            _lockControls = true;
            SoundManager.PlaySound("ShopMenuOpen");
            _confirmText.Opacity = 0f;
            _cancelText.Opacity = 0f;
            _navigationText.Opacity = 0f;
            Tween.To(_confirmText, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(_cancelText, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(_navigationText, 0.2f, Linear.EaseNone, "Opacity", "1");
            _confirmText.Text = "[Input:" + 0 + "]  select/equip";
            _cancelText.Text = "[Input:" + 2 + "]  cancel/close menu";
            if (!InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                _navigationText.Text = "Arrow keys to navigate";
            }
            else
            {
                _navigationText.Text = "[Button:LeftStick] to navigate";
            }

            _currentEquipmentIndex = 0;
            _inCategoryMenu = true;
            _selectionIcon.Position = _enchantressUI.GetChildAt(6).AbsPosition;
            _currentCategoryIndex = 6;
            UpdateIconStates();
            DisplayCategory(0);
            EaseInMenu();
            Tween.To(this, 0.2f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            UpdateIconSelectionText();
            base.OnEnter();
        }

        public override void OnExit()
        {
            if (_rainSound != null)
            {
                _rainSound.Stop(AudioStopOptions.Immediate);
            }

            for (var i = 0; i < _enchantressUI.NumChildren; i++)
            {
                _enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
            }

            foreach (var current in _masterIconArray)
            {
                for (var j = 0; j < current.Length; j++)
                {
                    current[j].Scale = Vector2.Zero;
                }
            }

            _selectionIcon.Scale = Vector2.Zero;
            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
            var flag = true;
            var getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
            for (var k = 0; k < getEquippedRuneArray.Length; k++)
            {
                var b = getEquippedRuneArray[k];
                if (b == -1)
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                GameUtil.UnlockAchievement("LOVE_OF_CHANGE");
            }

            base.OnExit();
        }

        public override void HandleInput()
        {
            if (!_lockControls)
            {
                if (_inCategoryMenu)
                {
                    CategorySelectionInput();
                }
                else
                {
                    EquipmentSelectionInput();
                }
            }

            base.HandleInput();
        }

        private void CategorySelectionInput()
        {
            var currentCategoryIndex = _currentCategoryIndex;
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                _currentCategoryIndex--;
                if (_currentCategoryIndex < 6)
                {
                    _currentCategoryIndex = 10;
                }
            }
            else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
            {
                _currentCategoryIndex++;
                if (_currentCategoryIndex > 10)
                {
                    _currentCategoryIndex = 6;
                }
            }

            if (currentCategoryIndex != _currentCategoryIndex)
            {
                SoundManager.PlaySound("ShopBSMenuMove");
                _selectionIcon.Position = _enchantressUI.GetChildAt(_currentCategoryIndex).AbsPosition;
                for (var i = 1; i < 6; i++)
                {
                    if (i == 1)
                    {
                        _enchantressUI.GetChildAt(i).Scale = new Vector2(1f, 1f);
                    }
                    else
                    {
                        _enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
                    }
                }

                if (_currentCategoryIndex != 6)
                {
                    _enchantressUI.GetChildAt(_currentCategoryIndex - 5).Scale = new Vector2(1f, 1f);
                }
                else
                {
                    _enchantressUI.GetChildAt(_currentCategoryIndex - 5).Scale = Vector2.Zero;
                }

                DisplayCategory(_currentCategoryIndex - 6);
            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                _lockControls = true;
                Tween.To(this, 0.2f, Linear.EaseNone, "delay", "0.5", "BackBufferOpacity", "0");
                EaseOutMenu();
                Tween.RunFunction(0.13f, typeof(SoundManager), "PlaySound", "ShopMenuClose");
            }

            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                _inCategoryMenu = false;
                _currentEquipmentIndex = 0;
                _selectionIcon.Position = _activeIconArray[_currentEquipmentIndex].AbsPosition;
                var b = Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][_currentEquipmentIndex];
                if (b == 1)
                {
                    Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][_currentEquipmentIndex] = 2;
                }

                UpdateNewIcons();
                UpdateIconSelectionText();
                SoundManager.PlaySound("ShopMenuConfirm");
            }
        }

        private void EquipmentSelectionInput()
        {
            var currentEquipmentIndex = _currentEquipmentIndex;
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                if (_currentEquipmentIndex > 4)
                {
                    _currentEquipmentIndex -= 5;
                }

                if (_currentEquipmentIndex < 0)
                {
                    _currentEquipmentIndex = 0;
                }
            }

            if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
            {
                if (_currentEquipmentIndex < 6)
                {
                    _currentEquipmentIndex += 5;
                }

                if (_currentEquipmentIndex > 10)
                {
                    _currentEquipmentIndex -= 5;
                }
            }

            if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
            {
                _currentEquipmentIndex--;
                if ((_currentEquipmentIndex + 1) % 5 == 0)
                {
                    _currentEquipmentIndex++;
                }
            }

            if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
            {
                _currentEquipmentIndex++;
                if (_currentEquipmentIndex % 5 == 0 || _currentEquipmentIndex > 10)
                {
                    _currentEquipmentIndex--;
                }
            }

            if (currentEquipmentIndex != _currentEquipmentIndex)
            {
                var b = Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][_currentEquipmentIndex];
                if (b == 1)
                {
                    Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][_currentEquipmentIndex] = 2;
                }

                UpdateNewIcons();
                UpdateIconSelectionText();
                _selectionIcon.Position = _activeIconArray[_currentEquipmentIndex].AbsPosition;
                SoundManager.PlaySound("ShopBSMenuMove");
            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                SoundManager.PlaySound("ShopMenuCancel");
                _inCategoryMenu = true;
                _selectionIcon.Position = _enchantressUI.GetChildAt(_currentCategoryIndex).AbsPosition;
                UpdateIconSelectionText();
            }

            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                var num = _currentCategoryIndex - 6;
                int num2 = Game.PlayerStats.GetRuneArray[num][_currentEquipmentIndex];
                int num3 = Game.PlayerStats.GetEquippedRuneArray[num];
                if (num2 < 3 && num2 > 0)
                {
                    var abilityCost = Game.EquipmentSystem.GetAbilityCost(num, _currentEquipmentIndex);
                    if (Game.PlayerStats.Gold >= abilityCost)
                    {
                        SoundManager.PlaySound("ShopMenuUnlock");
                        Game.PlayerStats.Gold -= abilityCost;
                        Game.PlayerStats.GetRuneArray[num][_currentEquipmentIndex] = 3;
                        Game.PlayerStats.GetEquippedRuneArray[num] = (sbyte) _currentEquipmentIndex;
                        Player.AttachedLevel.UpdatePlayerHUDAbilities();
                        var spriteObj = _masterIconArray[num][_currentEquipmentIndex];
                        spriteObj.Opacity = 1f;
                        num2 = 3;
                        UpdateIconSelectionText();
                    }
                }

                if (num3 != _currentEquipmentIndex && num2 == 3)
                {
                    _equippedIcon.Scale = new Vector2(1f, 1f);
                    _equippedIcon.Position = _activeIconArray[_currentEquipmentIndex].AbsPosition;
                    _equippedIcon.Position += new Vector2(18f, 18f);
                    Game.PlayerStats.GetEquippedRuneArray[num] = (sbyte) _currentEquipmentIndex;
                    Player.AttachedLevel.UpdatePlayerHUDAbilities();
                    SoundManager.PlaySound("ShopBSEquip");
                    UpdateIconSelectionText();
                    UpdateNewIcons();
                    return;
                }

                if (num3 == _currentEquipmentIndex)
                {
                    _equippedIcon.Scale = Vector2.Zero;
                    Game.PlayerStats.GetEquippedRuneArray[num] = -1;
                    UpdateNewIcons();
                }
            }
        }

        private void UpdateIconSelectionText()
        {
            _equipmentDescriptionText.Position = new Vector2(-1000f, -1000f);
            _descriptionText.Visible = false;
            _instructionsTitleText.Visible = false;
            _instructionsText.Visible = false;
            _unlockCostContainer.Visible = false;
            _equipmentTitleText.Visible = false;
            if (_inCategoryMenu)
            {
                _equipmentDescriptionText.Text = "Select a category";
                return;
            }

            if (Game.PlayerStats.GetRuneArray[_currentCategoryIndex - 6][_currentEquipmentIndex] == 0)
            {
                _equipmentDescriptionText.Position = new Vector2(230f, -20f);
                _equipmentDescriptionText.Text = "Rune needed";
                return;
            }

            if (Game.PlayerStats.GetRuneArray[_currentCategoryIndex - 6][_currentEquipmentIndex] < 3)
            {
                _equipmentDescriptionText.Text = "Purchase Info Here";
                (_unlockCostContainer.GetChildAt(1) as TextObj).Text =
                    Game.EquipmentSystem.GetAbilityCost(_currentCategoryIndex - 6, _currentEquipmentIndex) +
                    " to unlock";
                _unlockCostContainer.Visible = true;
                _descriptionText.Visible = true;
                _instructionsTitleText.Visible = true;
                _instructionsText.Visible = true;
                _equipmentTitleText.Visible = true;
                _descriptionText.Opacity = 0.5f;
                _instructionsTitleText.Opacity = 0.5f;
                _instructionsText.Opacity = 0.5f;
                _equipmentTitleText.Opacity = 0.5f;
                UpdateEquipmentDataText();
                return;
            }

            _descriptionText.Visible = true;
            _instructionsTitleText.Visible = true;
            _instructionsText.Visible = true;
            _equipmentTitleText.Visible = true;
            _descriptionText.Opacity = 1f;
            _instructionsTitleText.Opacity = 1f;
            _instructionsText.Opacity = 1f;
            _equipmentTitleText.Opacity = 1f;
            UpdateEquipmentDataText();
        }

        private void UpdateEquipmentDataText()
        {
            _equipmentTitleText.Text = (EquipmentAbility) _currentEquipmentIndex + " Rune\n(" +
                                        ((EquipmentCategory) _currentCategoryIndex - 6).AltName() + ")";
            _descriptionText.Text = ((EquipmentAbility) _currentEquipmentIndex).Description();
            _descriptionText.WordWrap(195);
            _descriptionText.Y = _equipmentTitleText.Y + 60f;
            _instructionsTitleText.Position = new Vector2(_enchantressUI.X + 140f,
                _descriptionText.Bounds.Bottom + 20);
            _instructionsText.Text = ((EquipmentAbility) _currentEquipmentIndex).Instructions();
            _instructionsText.WordWrap(200);
            _instructionsText.Position = new Vector2(_instructionsTitleText.X, _instructionsTitleText.Bounds.Bottom);
        }

        private void UpdateNewIcons()
        {
            UpdateMoneyText();
            _newIconListIndex = 0;
            foreach (var current in _newIconList)
            {
                current.Visible = false;
            }

            for (var i = 0; i < Game.PlayerStats.GetRuneArray[CurrentCategoryIndex].Length; i++)
            {
                var b = Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][i];
                if (b == 1)
                {
                    var arg_7A_0 = _masterIconArray[CurrentCategoryIndex][i];
                    var spriteObj = _newIconList[_newIconListIndex];
                    spriteObj.Visible = true;
                    spriteObj.Position = _masterIconArray[CurrentCategoryIndex][i].AbsPosition;
                    spriteObj.X -= 20f;
                    spriteObj.Y -= 30f;
                    _newIconListIndex++;
                }
            }

            var b2 = Game.PlayerStats.GetEquippedRuneArray[CurrentCategoryIndex];
            if (b2 > -1)
            {
                _equippedIcon.Position = new Vector2(_activeIconArray[b2].AbsPosition.X + 18f,
                    _activeIconArray[b2].AbsPosition.Y + 18f);
                _equippedIcon.Visible = true;
                return;
            }

            _equippedIcon.Visible = false;
        }

        private void UpdateMoneyText()
        {
            _playerMoney.Text = Game.PlayerStats.Gold.ToString();
            var levelScreen = Game.ScreenManager.GetLevelScreen();
            if (levelScreen != null)
            {
                levelScreen.UpdatePlayerHUD();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _enchantressUI.Draw(Camera);
            _selectionIcon.Draw(Camera);
            _descriptionText.Draw(Camera);
            _instructionsTitleText.Draw(Camera);
            _instructionsText.Draw(Camera);
            _unlockCostContainer.Draw(Camera);
            _equipmentTitleText.Draw(Camera);
            foreach (var current in _masterIconArray)
            {
                for (var i = 0; i < current.Length; i++)
                {
                    current[i].Draw(Camera);
                }
            }

            _navigationText.Draw(Camera);
            _cancelText.Draw(Camera);
            _confirmText.Draw(Camera);
            _equippedIcon.Draw(Camera);
            foreach (var current2 in _newIconList)
            {
                current2.Draw(Camera);
            }

            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Enchantress Screen");
            if (_rainSound != null)
            {
                _rainSound.Dispose();
            }

            _rainSound = null;
            _enchantressUI.Dispose();
            _enchantressUI = null;
            _equipmentDescriptionText.Dispose();
            _equipmentDescriptionText = null;
            _selectionIcon.Dispose();
            _selectionIcon = null;
            _equipmentTitleText.Dispose();
            _equipmentTitleText = null;
            _activeIconArray = null;
            foreach (var current in _masterIconArray)
            {
                for (var i = 0; i < current.Length; i++)
                {
                    current[i].Dispose();
                    current[i] = null;
                }

                Array.Clear(current, 0, current.Length);
            }

            _masterIconArray.Clear();
            _masterIconArray = null;
            _descriptionText.Dispose();
            _descriptionText = null;
            _unlockCostContainer.Dispose();
            _unlockCostContainer = null;
            _instructionsText.Dispose();
            _instructionsText = null;
            _instructionsTitleText.Dispose();
            _instructionsTitleText = null;
            _equippedIcon.Dispose();
            _equippedIcon = null;
            Player = null;
            _confirmText.Dispose();
            _confirmText = null;
            _cancelText.Dispose();
            _cancelText = null;
            _navigationText.Dispose();
            _navigationText = null;
            _playerMoney = null;
            foreach (var current2 in _newIconList)
            {
                current2.Dispose();
            }

            _newIconList.Clear();
            _newIconList = null;
            base.Dispose();
        }
    }
}
