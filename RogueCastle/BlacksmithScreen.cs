// 
// RogueLegacyArchipelago - BlacksmithScreen.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using RogueCastle.TypeDefinitions;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class BlacksmithScreen : Screen
    {
        private const int m_startingCategoryIndex = 6;
        private ObjContainer[] m_activeIconArray;
        private TextObj m_addPropertiesText;
        private TextObj m_addPropertiesTitleText;
        private ObjContainer m_blacksmithUI;
        private KeyIconTextObj m_cancelText;
        private KeyIconTextObj m_confirmText;
        private int m_currentCategoryIndex;
        private int m_currentEquipmentIndex;
        private TextObj m_equipmentDescriptionText;
        private TextObj m_equipmentTitleText;
        private SpriteObj m_equippedIcon;
        private bool m_inCategoryMenu = true;
        private bool m_lockControls;
        private List<ObjContainer[]> m_masterIconArray;
        private KeyIconTextObj m_navigationText;
        private List<SpriteObj> m_newIconList;
        private int m_newIconListIndex;
        private TextObj m_playerMoney;
        private Cue m_rainSound;
        private SpriteObj m_selectionIcon;
        private ObjContainer m_textInfoStatContainer;
        private ObjContainer m_textInfoStatModContainer;
        private ObjContainer m_textInfoTitleContainer;
        private ObjContainer m_unlockCostContainer;

        public BlacksmithScreen()
        {
            m_currentCategoryIndex = 6;
            m_masterIconArray = new List<ObjContainer[]>();
            for (var i = 0; i < 5; i++)
            {
                m_masterIconArray.Add(new ObjContainer[15]);
            }
        }

        public float BackBufferOpacity { get; set; }
        public PlayerObj Player { get; set; }

        private int CurrentCategoryIndex
        {
            get { return m_currentCategoryIndex - 6; }
        }

        public override void LoadContent()
        {
            m_blacksmithUI = new ObjContainer("BlacksmithUI_Character");
            m_blacksmithUI.Position = new Vector2(660f, 360f);
            m_playerMoney = new TextObj(Game.GoldFont);
            m_playerMoney.Align = Types.TextAlign.Left;
            m_playerMoney.Text = "1000";
            m_playerMoney.FontSize = 30f;
            m_playerMoney.OverrideParentScale = true;
            m_playerMoney.Position = new Vector2(210f, -225f);
            m_playerMoney.AnchorY = 10f;
            m_blacksmithUI.AddChild(m_playerMoney);
            for (var i = 0; i < m_blacksmithUI.NumChildren; i++)
            {
                m_blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
            }
            m_selectionIcon = new SpriteObj("BlacksmithUI_SelectionIcon_Sprite");
            m_selectionIcon.PlayAnimation();
            m_selectionIcon.Scale = Vector2.Zero;
            m_selectionIcon.AnimationDelay = 0.1f;
            m_selectionIcon.ForceDraw = true;
            m_equipmentDescriptionText = new TextObj(Game.JunicodeFont);
            m_equipmentDescriptionText.Align = Types.TextAlign.Centre;
            m_equipmentDescriptionText.FontSize = 12f;
            m_equipmentDescriptionText.Position = new Vector2(230f, -20f);
            m_equipmentDescriptionText.Text = "Select a category";
            m_equipmentDescriptionText.WordWrap(190);
            m_equipmentDescriptionText.Scale = Vector2.Zero;
            m_blacksmithUI.AddChild(m_equipmentDescriptionText);
            foreach (var current in m_masterIconArray)
            {
                var absPosition = m_blacksmithUI.GetChildAt(6).AbsPosition;
                absPosition.X += 85f;
                var x = absPosition.X;
                var num = 70f;
                var num2 = 80f;
                for (var j = 0; j < current.Length; j++)
                {
                    current[j] = new ObjContainer("BlacksmithUI_QuestionMarkIcon_Character");
                    current[j].Position = absPosition;
                    current[j].Scale = Vector2.Zero;
                    current[j].ForceDraw = true;
                    absPosition.X += num;
                    if (absPosition.X > x + num*4f)
                    {
                        absPosition.X = x;
                        absPosition.Y += num2;
                    }
                }
            }
            InitializeTextObjs();
            m_equippedIcon = new SpriteObj("BlacksmithUI_EquippedIcon_Sprite");
            m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
            m_confirmText.Text = "to close map";
            m_confirmText.FontSize = 12f;
            m_confirmText.Position = new Vector2(50f, 550f);
            m_confirmText.ForceDraw = true;
            m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
            m_cancelText.Text = "to re-center on player";
            m_cancelText.FontSize = 12f;
            m_cancelText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 40f);
            m_cancelText.ForceDraw = true;
            m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
            m_navigationText.Text = "to move map";
            m_navigationText.FontSize = 12f;
            m_navigationText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 80f);
            m_navigationText.ForceDraw = true;
            m_newIconList = new List<SpriteObj>();
            for (var k = 0; k < 25; k++)
            {
                var spriteObj = new SpriteObj("BlacksmithUI_NewIcon_Sprite");
                spriteObj.Visible = false;
                spriteObj.Scale = new Vector2(1.1f, 1.1f);
                m_newIconList.Add(spriteObj);
            }
            base.LoadContent();
        }

        private void InitializeTextObjs()
        {
            m_textInfoTitleContainer = new ObjContainer();
            m_textInfoStatContainer = new ObjContainer();
            m_textInfoStatModContainer = new ObjContainer();
            string[] array =
            {
                "Health",
                "Mana",
                "Damage",
                "Magic",
                "Armor",
                "Weight"
            };
            var zero = Vector2.Zero;
            var textObj = new TextObj();
            textObj.Font = Game.JunicodeFont;
            textObj.FontSize = 10f;
            textObj.Text = "0";
            textObj.ForceDraw = true;
            for (var i = 0; i < array.Length; i++)
            {
                textObj.Position = zero;
                m_textInfoTitleContainer.AddChild(textObj.Clone() as TextObj);
                m_textInfoStatContainer.AddChild(textObj.Clone() as TextObj);
                m_textInfoStatModContainer.AddChild(textObj.Clone() as TextObj);
                (m_textInfoTitleContainer.GetChildAt(i) as TextObj).Align = Types.TextAlign.Right;
                (m_textInfoTitleContainer.GetChildAt(i) as TextObj).Text = array[i];
                zero.Y += m_textInfoTitleContainer.GetChildAt(i).Height - 5;
            }
            m_addPropertiesTitleText = new TextObj();
            m_addPropertiesTitleText.Font = Game.JunicodeFont;
            m_addPropertiesTitleText.FontSize = 8f;
            m_addPropertiesTitleText.TextureColor = new Color(237, 202, 138);
            m_addPropertiesTitleText.Text = "Additional Properties:";
            m_addPropertiesText = new TextObj();
            m_addPropertiesText.Font = Game.JunicodeFont;
            m_addPropertiesText.FontSize = 8f;
            m_unlockCostContainer = new ObjContainer();
            var textObj2 = new TextObj();
            textObj2.Font = Game.JunicodeFont;
            textObj2.FontSize = 10f;
            textObj2.TextureColor = Color.Yellow;
            textObj2.Position = new Vector2(50f, 9f);
            m_unlockCostContainer.AddChild(new SpriteObj("BlacksmithUI_CoinBG_Sprite"));
            m_unlockCostContainer.AddChild(textObj2);
            m_equipmentTitleText = new TextObj(Game.JunicodeFont);
            m_equipmentTitleText.ForceDraw = true;
            m_equipmentTitleText.FontSize = 12f;
            m_equipmentTitleText.DropShadow = new Vector2(2f, 2f);
            m_equipmentTitleText.TextureColor = new Color(237, 202, 138);
            m_textInfoTitleContainer.Position = new Vector2(m_blacksmithUI.X + 205f,
                m_blacksmithUI.Y - m_blacksmithUI.Height/2 + 45f);
            m_textInfoStatContainer.Position = new Vector2(m_textInfoTitleContainer.X + 15f, m_textInfoTitleContainer.Y);
            m_textInfoStatModContainer.Position = new Vector2(m_textInfoStatContainer.X + 75f, m_textInfoStatContainer.Y);
            m_addPropertiesTitleText.Position = new Vector2(m_blacksmithUI.X + 140f,
                m_textInfoStatModContainer.Bounds.Bottom + 5);
            m_addPropertiesText.Position = new Vector2(m_addPropertiesTitleText.X,
                m_addPropertiesTitleText.Bounds.Bottom);
            m_unlockCostContainer.Position = new Vector2(m_blacksmithUI.X + 114f, 485f);
            m_equipmentTitleText.Position = new Vector2(m_blacksmithUI.X + 140f, m_textInfoTitleContainer.Y - 45f);
            m_textInfoTitleContainer.Visible = false;
            m_textInfoStatContainer.Visible = false;
            m_textInfoStatModContainer.Visible = false;
            m_addPropertiesTitleText.Visible = false;
            m_addPropertiesText.Visible = false;
            m_unlockCostContainer.Visible = false;
            m_equipmentTitleText.Visible = false;
        }

        private void DisplayCategory(int equipmentType)
        {
            var duration = 0.2f;
            var num = 0f;
            if (m_activeIconArray != null)
            {
                for (var i = 0; i < 15; i++)
                {
                    Tween.StopAllContaining(m_activeIconArray[i], false);
                    Tween.To(m_activeIconArray[i], duration, Back.EaseIn, "delay", num.ToString(), "ScaleX", "0",
                        "ScaleY", "0");
                }
            }
            m_activeIconArray = m_masterIconArray[equipmentType];
            num = 0.2f;
            for (var j = 0; j < 15; j++)
            {
                Tween.To(m_activeIconArray[j], duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY",
                    "1");
            }
            foreach (var current in m_newIconList)
            {
                Tween.StopAllContaining(current, false);
                current.Scale = Vector2.Zero;
                Tween.To(current, duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY", "1");
            }
            UpdateNewIcons();
            m_equippedIcon.Scale = Vector2.Zero;
            Tween.StopAllContaining(m_equippedIcon, false);
            Tween.To(m_equippedIcon, duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY", "1");
        }

        public void EaseInMenu()
        {
            var duration = 0.4f;
            Tween.To(m_blacksmithUI.GetChildAt(0), duration, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_selectionIcon, duration, Back.EaseOut, "delay", "0.25", "ScaleX", "1", "ScaleY", "1");
            var num = 0.2f;
            for (var i = 6; i < m_blacksmithUI.NumChildren - 3; i++)
            {
                num += 0.05f;
                Tween.To(m_blacksmithUI.GetChildAt(i), duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1",
                    "ScaleY", "1");
            }
            Tween.To(m_blacksmithUI.GetChildAt(m_blacksmithUI.NumChildren - 1), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_blacksmithUI.GetChildAt(m_blacksmithUI.NumChildren - 2), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_blacksmithUI.GetChildAt(m_blacksmithUI.NumChildren - 3), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.AddEndHandlerToLastTween(this, "EaseInComplete");
        }

        public void EaseInComplete()
        {
            m_lockControls = false;
        }

        private void EaseOutMenu()
        {
            foreach (var current in m_newIconList)
            {
                current.Visible = false;
            }
            m_equippedIcon.Visible = false;
            Tween.To(m_confirmText, 0.2f, Linear.EaseNone, "Opacity", "0");
            Tween.To(m_cancelText, 0.2f, Linear.EaseNone, "Opacity", "0");
            Tween.To(m_navigationText, 0.2f, Linear.EaseNone, "Opacity", "0");
            var num = 0.4f;
            var num2 = 0f;
            Tween.To(m_blacksmithUI.GetChildAt(m_blacksmithUI.NumChildren - 2), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            Tween.To(m_blacksmithUI.GetChildAt(m_blacksmithUI.NumChildren - 3), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            Tween.To(m_blacksmithUI.GetChildAt(m_blacksmithUI.NumChildren - 4), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            for (var i = 6; i < 11; i++)
            {
                if (m_currentCategoryIndex == i)
                {
                    Tween.To(m_selectionIcon, num, Back.EaseIn, "delay", num2.ToString(), "ScaleX", "0", "ScaleY", "0");
                }
                Tween.To(m_blacksmithUI.GetChildAt(i), num, Back.EaseIn, "delay", num2.ToString(), "ScaleX", "0",
                    "ScaleY", "0");
                num2 += 0.05f;
            }
            for (var j = 1; j < 6; j++)
            {
                m_blacksmithUI.GetChildAt(j).Scale = Vector2.Zero;
            }
            for (var k = 0; k < m_activeIconArray.Length; k++)
            {
                Tween.To(m_activeIconArray[k], num, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            }
            Tween.To(m_blacksmithUI.GetChildAt(0), num, Back.EaseIn, "delay", "0.3", "ScaleX", "0", "ScaleY", "0");
            Tween.RunFunction(num + 0.35f, ScreenManager, "HideCurrentScreen");
        }

        private void UpdateIconStates()
        {
            for (var i = 0; i < Game.PlayerStats.GetBlueprintArray.Count; i++)
            {
                for (var j = 0; j < Game.PlayerStats.GetBlueprintArray[i].Length; j++)
                {
                    var b = Game.PlayerStats.GetBlueprintArray[i][j];
                    if (b == 0)
                    {
                        m_masterIconArray[i][j].ChangeSprite("BlacksmithUI_QuestionMarkIcon_Character");
                    }
                    else
                    {
                        m_masterIconArray[i][j].ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(i) +
                                                             (j%5 + 1) + "Icon_Character");
                        for (var k = 1; k < m_masterIconArray[i][j].NumChildren; k++)
                        {
                            m_masterIconArray[i][j].GetChildAt(k).Opacity = 0.2f;
                        }
                    }
                    if (b > 2)
                    {
                        for (var l = 1; l < m_masterIconArray[i][j].NumChildren; l++)
                        {
                            m_masterIconArray[i][j].GetChildAt(l).Opacity = 1f;
                        }
                        var num = 1;
                        if (i == 0)
                        {
                            num = 2;
                        }
                        var equipmentData = Game.EquipmentSystem.GetEquipmentData(i, j);
                        m_masterIconArray[i][j].GetChildAt(num).TextureColor = equipmentData.FirstColour;
                        if (i != 4)
                        {
                            num++;
                            m_masterIconArray[i][j].GetChildAt(num).TextureColor = equipmentData.SecondColour;
                        }
                    }
                }
            }
        }

        private void UpdateNewIcons()
        {
            if (Player != null)
            {
                if (Player.CurrentMana > Player.MaxMana)
                {
                    Player.CurrentMana = Player.MaxMana;
                }
                if (Player.CurrentHealth > Player.MaxHealth)
                {
                    Player.CurrentHealth = Player.MaxHealth;
                }
            }
            UpdateMoneyText();
            m_newIconListIndex = 0;
            foreach (var current in m_newIconList)
            {
                current.Visible = false;
            }
            for (var i = 0; i < Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex].Length; i++)
            {
                var b = Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][i];
                if (b == 1)
                {
                    var arg_DE_0 = m_masterIconArray[CurrentCategoryIndex][i];
                    var spriteObj = m_newIconList[m_newIconListIndex];
                    spriteObj.Visible = true;
                    spriteObj.Position = m_masterIconArray[CurrentCategoryIndex][i].AbsPosition;
                    spriteObj.X -= 20f;
                    spriteObj.Y -= 30f;
                    m_newIconListIndex++;
                }
            }
            var b2 = Game.PlayerStats.GetEquippedArray[CurrentCategoryIndex];
            if (b2 > -1)
            {
                m_equippedIcon.Position = new Vector2(m_activeIconArray[b2].AbsPosition.X + 18f,
                    m_activeIconArray[b2].AbsPosition.Y + 18f);
                m_equippedIcon.Visible = true;
                return;
            }
            m_equippedIcon.Visible = false;
        }

        public override void OnEnter()
        {
            if (m_rainSound != null)
            {
                m_rainSound.Dispose();
            }
            if (DateTime.Now.Month != 12 && DateTime.Now.Month != 1)
            {
                m_rainSound = SoundManager.PlaySound("Rain1_Filtered");
            }
            else
            {
                m_rainSound = SoundManager.PlaySound("snowloop_filtered");
            }
            if (Game.PlayerStats.TotalBlueprintsFound >= 75)
            {
                GameUtil.UnlockAchievement("FEAR_OF_THROWING_STUFF_OUT");
            }
            m_lockControls = true;
            SoundManager.PlaySound("ShopMenuOpen");
            m_confirmText.Opacity = 0f;
            m_cancelText.Opacity = 0f;
            m_navigationText.Opacity = 0f;
            Tween.To(m_confirmText, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(m_cancelText, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(m_navigationText, 0.2f, Linear.EaseNone, "Opacity", "1");
            m_confirmText.Text = "[Input:" + 0 + "]  select/equip";
            m_cancelText.Text = "[Input:" + 2 + "]  cancel/close menu";
            if (!InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                m_navigationText.Text = "Arrow keys to navigate";
            }
            else
            {
                m_navigationText.Text = "[Button:LeftStick] to navigate";
            }
            m_currentEquipmentIndex = 0;
            m_inCategoryMenu = true;
            m_selectionIcon.Position = m_blacksmithUI.GetChildAt(6).AbsPosition;
            m_currentCategoryIndex = 6;
            UpdateIconStates();
            DisplayCategory(0);
            EaseInMenu();
            Tween.To(this, 0.2f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            UpdateIconSelectionText();
            base.OnEnter();
        }

        public override void OnExit()
        {
            if (m_rainSound != null)
            {
                m_rainSound.Stop(AudioStopOptions.Immediate);
            }
            for (var i = 0; i < m_blacksmithUI.NumChildren; i++)
            {
                m_blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
            }
            foreach (var current in m_masterIconArray)
            {
                for (var j = 0; j < current.Length; j++)
                {
                    current[j].Scale = Vector2.Zero;
                }
            }
            m_selectionIcon.Scale = Vector2.Zero;
            Player.CurrentHealth = Player.MaxHealth;
            Player.CurrentMana = Player.MaxMana;
            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
            var flag = true;
            var getEquippedArray = Game.PlayerStats.GetEquippedArray;
            for (var k = 0; k < getEquippedArray.Length; k++)
            {
                var b = getEquippedArray[k];
                if (b == -1)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                GameUtil.UnlockAchievement("FEAR_OF_NUDITY");
            }
            base.OnExit();
        }

        public override void HandleInput()
        {
            if (!m_lockControls)
            {
                if (m_inCategoryMenu)
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
            var currentCategoryIndex = m_currentCategoryIndex;
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                m_currentCategoryIndex--;
                if (m_currentCategoryIndex < 6)
                {
                    m_currentCategoryIndex = 10;
                }
            }
            else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
            {
                m_currentCategoryIndex++;
                if (m_currentCategoryIndex > 10)
                {
                    m_currentCategoryIndex = 6;
                }
            }
            if (currentCategoryIndex != m_currentCategoryIndex)
            {
                SoundManager.PlaySound("ShopBSMenuMove");
                m_selectionIcon.Position = m_blacksmithUI.GetChildAt(m_currentCategoryIndex).AbsPosition;
                for (var i = 1; i < 6; i++)
                {
                    if (i == 1)
                    {
                        m_blacksmithUI.GetChildAt(i).Scale = new Vector2(1f, 1f);
                    }
                    else
                    {
                        m_blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
                    }
                }
                if (m_currentCategoryIndex != 6)
                {
                    m_blacksmithUI.GetChildAt(m_currentCategoryIndex - 5).Scale = new Vector2(1f, 1f);
                }
                else
                {
                    m_blacksmithUI.GetChildAt(m_currentCategoryIndex - 5).Scale = Vector2.Zero;
                }
                DisplayCategory(m_currentCategoryIndex - 6);
            }
            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                m_lockControls = true;
                Tween.To(this, 0.2f, Linear.EaseNone, "delay", "0.5", "BackBufferOpacity", "0");
                EaseOutMenu();
                Tween.RunFunction(0.13f, typeof (SoundManager), "PlaySound", "ShopMenuClose");
            }
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                m_inCategoryMenu = false;
                m_currentEquipmentIndex = 0;
                m_selectionIcon.Position = m_activeIconArray[m_currentEquipmentIndex].AbsPosition;
                var b = Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][m_currentEquipmentIndex];
                if (b == 1)
                {
                    Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][m_currentEquipmentIndex] = 2;
                }
                UpdateNewIcons();
                UpdateIconSelectionText();
                SoundManager.PlaySound("ShopMenuConfirm");
            }
        }

        private void EquipmentSelectionInput()
        {
            var currentEquipmentIndex = m_currentEquipmentIndex;
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                m_currentEquipmentIndex -= 5;
                if (m_currentEquipmentIndex < 0)
                {
                    m_currentEquipmentIndex += 15;
                }
            }
            if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
            {
                m_currentEquipmentIndex += 5;
                if (m_currentEquipmentIndex > 14)
                {
                    m_currentEquipmentIndex -= 15;
                }
            }
            if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
            {
                m_currentEquipmentIndex--;
                if ((m_currentEquipmentIndex + 1)%5 == 0)
                {
                    m_currentEquipmentIndex += 5;
                }
            }
            if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
            {
                m_currentEquipmentIndex++;
                if (m_currentEquipmentIndex%5 == 0)
                {
                    m_currentEquipmentIndex -= 5;
                }
            }
            if (currentEquipmentIndex != m_currentEquipmentIndex)
            {
                var b = Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][m_currentEquipmentIndex];
                if (b == 1)
                {
                    Game.PlayerStats.GetBlueprintArray[CurrentCategoryIndex][m_currentEquipmentIndex] = 2;
                }
                UpdateNewIcons();
                UpdateIconSelectionText();
                m_selectionIcon.Position = m_activeIconArray[m_currentEquipmentIndex].AbsPosition;
                SoundManager.PlaySound("ShopBSMenuMove");
            }
            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                SoundManager.PlaySound("ShopMenuCancel");
                m_inCategoryMenu = true;
                m_selectionIcon.Position = m_blacksmithUI.GetChildAt(m_currentCategoryIndex).AbsPosition;
                UpdateIconSelectionText();
            }
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                var num = m_currentCategoryIndex - 6;
                int num2 = Game.PlayerStats.GetBlueprintArray[num][m_currentEquipmentIndex];
                int num3 = Game.PlayerStats.GetEquippedArray[num];
                if (num2 < 3 && num2 > 0)
                {
                    var equipmentData = Game.EquipmentSystem.GetEquipmentData(num, m_currentEquipmentIndex);
                    if (Game.PlayerStats.Gold >= equipmentData.Cost)
                    {
                        SoundManager.PlaySound("ShopMenuUnlock");
                        Game.PlayerStats.Gold -= equipmentData.Cost;
                        Game.PlayerStats.GetBlueprintArray[num][m_currentEquipmentIndex] = 3;
                        var objContainer = m_masterIconArray[num][m_currentEquipmentIndex];
                        objContainer.ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(num) +
                                                  (m_currentEquipmentIndex%5 + 1) + "Icon_Character");
                        for (var i = 1; i < objContainer.NumChildren; i++)
                        {
                            objContainer.GetChildAt(i).Opacity = 1f;
                        }
                        var num4 = 1;
                        if (num == 0)
                        {
                            num4 = 2;
                        }
                        objContainer.GetChildAt(num4).TextureColor = equipmentData.FirstColour;
                        if (num != 4)
                        {
                            num4++;
                            objContainer.GetChildAt(num4).TextureColor = equipmentData.SecondColour;
                        }
                        num2 = 3;
                        UpdateIconSelectionText();
                    }
                    else
                    {
                        SoundManager.PlaySound("ShopMenuUnlockFail");
                    }
                }
                if (num3 != m_currentEquipmentIndex && num2 == 3)
                {
                    var equipmentData2 = Game.EquipmentSystem.GetEquipmentData(num, m_currentEquipmentIndex);
                    int num5 = Game.PlayerStats.GetEquippedArray[num];
                    var num6 = 0;
                    if (num5 != -1)
                    {
                        num6 = Game.EquipmentSystem.GetEquipmentData(num, num5).Weight;
                    }
                    if (equipmentData2.Weight + Player.CurrentWeight - num6 <= Player.MaxWeight)
                    {
                        SoundManager.PlaySound("ShopBSEquip");
                        Game.PlayerStats.GetEquippedArray[num] = (sbyte) m_currentEquipmentIndex;
                        UpdateIconSelectionText();
                        var partIndices = PlayerPart.GetPartIndices(num);
                        if (partIndices.X != -1f)
                        {
                            Player.GetChildAt((int) partIndices.X).TextureColor = equipmentData2.FirstColour;
                        }
                        if (partIndices.Y != -1f)
                        {
                            Player.GetChildAt((int) partIndices.Y).TextureColor = equipmentData2.SecondColour;
                        }
                        if (partIndices.Z != -1f)
                        {
                            Player.GetChildAt((int) partIndices.Z).TextureColor = equipmentData2.SecondColour;
                        }
                        if (num == 2 && partIndices.X != -1f)
                        {
                            Player.GetChildAt(5).TextureColor = equipmentData2.FirstColour;
                        }
                        UpdateNewIcons();
                        return;
                    }
                    Console.WriteLine("cannot equip. too heavy. Weight:" +
                                      (equipmentData2.Weight + Player.CurrentWeight - num6));
                }
                else if (num3 == m_currentEquipmentIndex)
                {
                    Game.PlayerStats.GetEquippedArray[num] = -1;
                    Player.UpdateEquipmentColours();
                    UpdateIconSelectionText();
                    UpdateNewIcons();
                }
            }
        }

        private void UpdateIconSelectionText()
        {
            m_equipmentDescriptionText.Position = new Vector2(-1000f, -1000f);
            m_textInfoTitleContainer.Visible = false;
            m_textInfoStatContainer.Visible = false;
            m_textInfoStatModContainer.Visible = false;
            m_addPropertiesTitleText.Visible = false;
            m_addPropertiesText.Visible = false;
            m_unlockCostContainer.Visible = false;
            m_equipmentTitleText.Visible = false;
            if (m_inCategoryMenu)
            {
                m_equipmentDescriptionText.Text = "Select a category";
                return;
            }
            if (Game.PlayerStats.GetBlueprintArray[m_currentCategoryIndex - 6][m_currentEquipmentIndex] == 0)
            {
                m_equipmentDescriptionText.Position = new Vector2(230f, -20f);
                m_equipmentDescriptionText.Text = "Blueprint needed";
                return;
            }
            if (Game.PlayerStats.GetBlueprintArray[m_currentCategoryIndex - 6][m_currentEquipmentIndex] < 3)
            {
                m_equipmentDescriptionText.Text = "Purchase Info Here";
                (m_unlockCostContainer.GetChildAt(1) as TextObj).Text =
                    Game.EquipmentSystem.GetEquipmentData(m_currentCategoryIndex - 6, m_currentEquipmentIndex)
                        .Cost + " to unlock";
                m_unlockCostContainer.Visible = true;
                m_textInfoTitleContainer.Visible = true;
                m_textInfoStatContainer.Visible = true;
                m_textInfoStatModContainer.Visible = true;
                m_addPropertiesTitleText.Visible = true;
                m_addPropertiesText.Visible = true;
                m_equipmentTitleText.Visible = true;
                m_textInfoTitleContainer.Opacity = 0.5f;
                m_textInfoStatContainer.Opacity = 0.5f;
                m_textInfoStatModContainer.Opacity = 0.5f;
                m_addPropertiesTitleText.Opacity = 0.5f;
                m_addPropertiesText.Opacity = 0.5f;
                m_equipmentTitleText.Opacity = 0.5f;
                UpdateEquipmentDataText();
                return;
            }
            m_textInfoTitleContainer.Visible = true;
            m_textInfoStatContainer.Visible = true;
            m_textInfoStatModContainer.Visible = true;
            m_addPropertiesTitleText.Visible = true;
            m_addPropertiesText.Visible = true;
            m_equipmentTitleText.Visible = true;
            m_textInfoTitleContainer.Opacity = 1f;
            m_textInfoStatContainer.Opacity = 1f;
            m_textInfoStatModContainer.Opacity = 1f;
            m_addPropertiesTitleText.Opacity = 1f;
            m_addPropertiesText.Opacity = 1f;
            m_equipmentTitleText.Opacity = 1f;
            UpdateEquipmentDataText();
        }

        private void UpdateEquipmentDataText()
        {
            (m_textInfoStatContainer.GetChildAt(0) as TextObj).Text = Player.MaxHealth.ToString();
            (m_textInfoStatContainer.GetChildAt(1) as TextObj).Text = Player.MaxMana.ToString();
            (m_textInfoStatContainer.GetChildAt(2) as TextObj).Text = Player.Damage.ToString();
            (m_textInfoStatContainer.GetChildAt(3) as TextObj).Text = Player.TotalMagicDamage.ToString();
            (m_textInfoStatContainer.GetChildAt(4) as TextObj).Text = Player.TotalArmor.ToString();
            (m_textInfoStatContainer.GetChildAt(5) as TextObj).Text = Player.CurrentWeight + "/" +
                                                                      Player.MaxWeight;
            var num = m_currentCategoryIndex - 6;
            var equipmentData = Game.EquipmentSystem.GetEquipmentData(num, m_currentEquipmentIndex);
            int num2 = Game.PlayerStats.GetEquippedArray[num];
            var equipmentData2 = new EquipmentData();
            if (num2 > -1)
            {
                equipmentData2 = Game.EquipmentSystem.GetEquipmentData(num, num2);
            }
            var flag = Game.PlayerStats.GetEquippedArray[CurrentCategoryIndex] == m_currentEquipmentIndex;
            var num3 = equipmentData.BonusHealth - equipmentData2.BonusHealth;
            if (flag)
            {
                num3 = -equipmentData.BonusHealth;
            }
            var textObj = m_textInfoStatModContainer.GetChildAt(0) as TextObj;
            if (num3 > 0)
            {
                textObj.TextureColor = Color.Cyan;
                textObj.Text = "+" + num3;
            }
            else if (num3 < 0)
            {
                textObj.TextureColor = Color.Red;
                textObj.Text = num3.ToString();
            }
            else
            {
                textObj.Text = "";
            }
            var textObj2 = m_textInfoStatModContainer.GetChildAt(1) as TextObj;
            var num4 = equipmentData.BonusMana - equipmentData2.BonusMana;
            if (flag)
            {
                num4 = -equipmentData.BonusMana;
            }
            if (num4 > 0)
            {
                textObj2.TextureColor = Color.Cyan;
                textObj2.Text = "+" + num4;
            }
            else if (num4 < 0)
            {
                textObj2.TextureColor = Color.Red;
                textObj2.Text = num4.ToString();
            }
            else
            {
                textObj2.Text = "";
            }
            var textObj3 = m_textInfoStatModContainer.GetChildAt(2) as TextObj;
            var num5 = equipmentData.BonusDamage - equipmentData2.BonusDamage;
            if (flag)
            {
                num5 = -equipmentData.BonusDamage;
            }
            if (num5 > 0)
            {
                textObj3.TextureColor = Color.Cyan;
                textObj3.Text = "+" + num5;
            }
            else if (num5 < 0)
            {
                textObj3.TextureColor = Color.Red;
                textObj3.Text = num5.ToString();
            }
            else
            {
                textObj3.Text = "";
            }
            var textObj4 = m_textInfoStatModContainer.GetChildAt(3) as TextObj;
            var num6 = equipmentData.BonusMagic - equipmentData2.BonusMagic;
            if (flag)
            {
                num6 = -equipmentData.BonusMagic;
            }
            if (num6 > 0)
            {
                textObj4.TextureColor = Color.Cyan;
                textObj4.Text = "+" + num6;
            }
            else if (num6 < 0)
            {
                textObj4.TextureColor = Color.Red;
                textObj4.Text = num6.ToString();
            }
            else
            {
                textObj4.Text = "";
            }
            var textObj5 = m_textInfoStatModContainer.GetChildAt(4) as TextObj;
            var num7 = equipmentData.BonusArmor - equipmentData2.BonusArmor;
            if (flag)
            {
                num7 = -equipmentData.BonusArmor;
            }
            if (num7 > 0)
            {
                textObj5.TextureColor = Color.Cyan;
                textObj5.Text = "+" + num7;
            }
            else if (num7 < 0)
            {
                textObj5.TextureColor = Color.Red;
                textObj5.Text = num7.ToString();
            }
            else
            {
                textObj5.Text = "";
            }
            var textObj6 = m_textInfoStatModContainer.GetChildAt(5) as TextObj;
            var num8 = equipmentData.Weight - equipmentData2.Weight;
            if (flag)
            {
                num8 = -equipmentData.Weight;
            }
            if (num8 > 0)
            {
                textObj6.TextureColor = Color.Red;
                textObj6.Text = "+" + num8;
            }
            else if (num8 < 0)
            {
                textObj6.TextureColor = Color.Cyan;
                textObj6.Text = num8.ToString();
            }
            else
            {
                textObj6.Text = "";
            }
            var secondaryAttribute = equipmentData.SecondaryAttribute;
            m_addPropertiesText.Text = "";
            if (secondaryAttribute != null)
            {
                var array = secondaryAttribute;
                for (var i = 0; i < array.Length; i++)
                {
                    var vector = array[i];
                    if (vector.X != 0f)
                    {
                        if (vector.X < 7f)
                        {
                            var expr_4FE = m_addPropertiesText;
                            var text = expr_4FE.Text;
                            expr_4FE.Text = string.Concat(text, "+", (vector.Y*100f).ToString(), "% ",
                                EquipmentSecondaryDataType.ToString((int) vector.X), "\n");
                        }
                        else
                        {
                            var expr_56E = m_addPropertiesText;
                            var text2 = expr_56E.Text;
                            var array2 = new string[6];
                            array2[0] = text2;
                            array2[1] = "+";
                            var arg_5A0_0 = array2;
                            var arg_5A0_1 = 2;
                            var y = vector.Y;
                            arg_5A0_0[arg_5A0_1] = y.ToString();
                            array2[3] = " ";
                            array2[4] = EquipmentSecondaryDataType.ToString((int) vector.X);
                            array2[5] = "\n";
                            expr_56E.Text = string.Concat(array2);
                        }
                    }
                }
                if (secondaryAttribute.Length == 0)
                {
                    m_addPropertiesText.Text = "None";
                }
            }
            else
            {
                m_addPropertiesText.Text = "None";
            }
            m_equipmentTitleText.Text = EquipmentBaseType.ToString(m_currentEquipmentIndex) + " " +
                                        EquipmentCategoryType.ToString(num);
        }

        private void UpdateMoneyText()
        {
            m_playerMoney.Text = Game.PlayerStats.Gold.ToString();
            var levelScreen = Game.ScreenManager.GetLevelScreen();
            if (levelScreen != null)
            {
                levelScreen.UpdatePlayerHUD();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black*BackBufferOpacity);
            m_blacksmithUI.Draw(Camera);
            m_selectionIcon.Draw(Camera);
            m_textInfoTitleContainer.Draw(Camera);
            m_textInfoStatContainer.Draw(Camera);
            m_textInfoStatModContainer.Draw(Camera);
            m_addPropertiesTitleText.Draw(Camera);
            m_addPropertiesText.Draw(Camera);
            m_unlockCostContainer.Draw(Camera);
            m_equipmentTitleText.Draw(Camera);
            foreach (var current in m_masterIconArray)
            {
                for (var i = 0; i < current.Length; i++)
                {
                    current[i].Draw(Camera);
                }
            }
            m_navigationText.Draw(Camera);
            m_cancelText.Draw(Camera);
            m_confirmText.Draw(Camera);
            m_equippedIcon.Draw(Camera);
            foreach (var current2 in m_newIconList)
            {
                current2.Draw(Camera);
            }
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Blacksmith Screen");
                if (m_rainSound != null)
                {
                    m_rainSound.Dispose();
                }
                m_rainSound = null;
                m_blacksmithUI.Dispose();
                m_blacksmithUI = null;
                m_equipmentDescriptionText.Dispose();
                m_equipmentDescriptionText = null;
                m_selectionIcon.Dispose();
                m_selectionIcon = null;
                m_equipmentTitleText.Dispose();
                m_equipmentTitleText = null;
                m_activeIconArray = null;
                foreach (var current in m_masterIconArray)
                {
                    for (var i = 0; i < current.Length; i++)
                    {
                        current[i].Dispose();
                        current[i] = null;
                    }
                    Array.Clear(current, 0, current.Length);
                }
                m_masterIconArray.Clear();
                m_masterIconArray = null;
                m_textInfoStatContainer.Dispose();
                m_textInfoStatContainer = null;
                m_textInfoTitleContainer.Dispose();
                m_textInfoTitleContainer = null;
                m_textInfoStatModContainer.Dispose();
                m_textInfoStatModContainer = null;
                m_unlockCostContainer.Dispose();
                m_unlockCostContainer = null;
                m_addPropertiesText.Dispose();
                m_addPropertiesText = null;
                m_addPropertiesTitleText.Dispose();
                m_addPropertiesTitleText = null;
                m_equippedIcon.Dispose();
                m_equippedIcon = null;
                Player = null;
                m_confirmText.Dispose();
                m_confirmText = null;
                m_cancelText.Dispose();
                m_cancelText = null;
                m_navigationText.Dispose();
                m_navigationText = null;
                m_playerMoney = null;
                foreach (var current2 in m_newIconList)
                {
                    current2.Dispose();
                }
                m_newIconList.Clear();
                m_newIconList = null;
                base.Dispose();
            }
        }
    }
}
