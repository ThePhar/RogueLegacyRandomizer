/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class EnchantressScreen : Screen
    {
        private const int m_startingCategoryIndex = 6;
        private ObjContainer m_enchantressUI;
        private SpriteObj m_selectionIcon;
        private int m_currentCategoryIndex;
        private int m_currentEquipmentIndex;
        private List<SpriteObj[]> m_masterIconArray;
        private SpriteObj[] m_activeIconArray;
        private List<SpriteObj> m_newIconList;
        private int m_newIconListIndex;
        private TextObj m_playerMoney;
        private SpriteObj m_equippedIcon;
        private TextObj m_equipmentDescriptionText;
        private TextObj m_descriptionText;
        private ObjContainer m_unlockCostContainer;
        private TextObj m_instructionsTitleText;
        private KeyIconTextObj m_instructionsText;
        private TextObj m_equipmentTitleText;
        private bool m_inCategoryMenu = true;
        private bool m_lockControls;
        private KeyIconTextObj m_confirmText;
        private KeyIconTextObj m_cancelText;
        private KeyIconTextObj m_navigationText;
        private Cue m_rainSound;
        public float BackBufferOpacity { get; set; }
        public PlayerObj Player { get; set; }

        private int CurrentCategoryIndex
        {
            get { return m_currentCategoryIndex - 6; }
        }

        public EnchantressScreen()
        {
            m_currentCategoryIndex = 6;
            m_masterIconArray = new List<SpriteObj[]>();
            for (int i = 0; i < 5; i++)
            {
                m_masterIconArray.Add(new SpriteObj[11]);
            }
        }

        public override void LoadContent()
        {
            m_enchantressUI = new ObjContainer("BlacksmithUI_Character");
            m_enchantressUI.Position = new Vector2(660f, 360f);
            m_playerMoney = new TextObj(Game.GoldFont);
            m_playerMoney.Align = Types.TextAlign.Left;
            m_playerMoney.Text = "1000";
            m_playerMoney.FontSize = 30f;
            m_playerMoney.OverrideParentScale = true;
            m_playerMoney.Position = new Vector2(210f, -225f);
            m_playerMoney.AnchorY = 10f;
            m_enchantressUI.AddChild(m_playerMoney);
            m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 3).ChangeSprite("EnchantressUI_Title_Sprite");
            for (int i = 0; i < m_enchantressUI.NumChildren; i++)
            {
                m_enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
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
            m_enchantressUI.AddChild(m_equipmentDescriptionText);
            foreach (SpriteObj[] current in m_masterIconArray)
            {
                Vector2 absPosition = m_enchantressUI.GetChildAt(6).AbsPosition;
                absPosition.X += 85f;
                float x = absPosition.X;
                float num = 70f;
                float num2 = 80f;
                for (int j = 0; j < current.Length; j++)
                {
                    current[j] = new SpriteObj("BlacksmithUI_QuestionMarkIcon_Sprite");
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
            for (int k = 0; k < 55; k++)
            {
                SpriteObj spriteObj = new SpriteObj("BlacksmithUI_NewIcon_Sprite");
                spriteObj.Visible = false;
                spriteObj.Scale = new Vector2(1.1f, 1.1f);
                m_newIconList.Add(spriteObj);
            }
            base.LoadContent();
        }

        private void InitializeTextObjs()
        {
            m_descriptionText = new TextObj(Game.JunicodeFont);
            m_descriptionText.FontSize = 9f;
            m_instructionsTitleText = new TextObj();
            m_instructionsTitleText.Font = Game.JunicodeFont;
            m_instructionsTitleText.FontSize = 10f;
            m_instructionsTitleText.TextureColor = new Color(237, 202, 138);
            m_instructionsTitleText.Text = "Instructions:";
            m_instructionsText = new KeyIconTextObj();
            m_instructionsText.Font = Game.JunicodeFont;
            m_instructionsText.FontSize = 10f;
            m_unlockCostContainer = new ObjContainer();
            TextObj textObj = new TextObj();
            textObj.Font = Game.JunicodeFont;
            textObj.FontSize = 10f;
            textObj.TextureColor = Color.Yellow;
            textObj.Position = new Vector2(50f, 9f);
            m_unlockCostContainer.AddChild(new SpriteObj("BlacksmithUI_CoinBG_Sprite"));
            m_unlockCostContainer.AddChild(textObj);
            m_descriptionText.Position = new Vector2(m_enchantressUI.X + 140f,
                m_enchantressUI.Y - m_enchantressUI.Height/2 + 20f + 40f);
            m_instructionsTitleText.Position = new Vector2(m_enchantressUI.X + 140f,
                m_descriptionText.Bounds.Bottom + 20);
            m_instructionsText.Position = new Vector2(m_instructionsTitleText.X, m_instructionsTitleText.Bounds.Bottom);
            m_unlockCostContainer.Position = new Vector2(m_enchantressUI.X + 114f, 485f);
            m_equipmentTitleText = new TextObj(Game.JunicodeFont);
            m_equipmentTitleText.ForceDraw = true;
            m_equipmentTitleText.FontSize = 10f;
            m_equipmentTitleText.DropShadow = new Vector2(2f, 2f);
            m_equipmentTitleText.TextureColor = new Color(237, 202, 138);
            m_equipmentTitleText.Position = new Vector2(m_enchantressUI.X + 140f, m_descriptionText.Y - 50f);
            m_descriptionText.Visible = false;
            m_instructionsTitleText.Visible = false;
            m_instructionsText.Visible = false;
            m_unlockCostContainer.Visible = false;
        }

        private void DisplayCategory(int equipmentType)
        {
            float duration = 0.2f;
            float num = 0f;
            if (m_activeIconArray != null)
            {
                for (int i = 0; i < 11; i++)
                {
                    Tween.StopAllContaining(m_activeIconArray[i], false);
                    Tween.To(m_activeIconArray[i], duration, Back.EaseIn, "delay", num.ToString(), "ScaleX", "0",
                        "ScaleY", "0");
                }
            }
            m_activeIconArray = m_masterIconArray[equipmentType];
            num = 0.25f;
            for (int j = 0; j < 11; j++)
            {
                Tween.To(m_activeIconArray[j], duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1", "ScaleY",
                    "1");
            }
            foreach (SpriteObj current in m_newIconList)
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
            float duration = 0.4f;
            Tween.To(m_enchantressUI.GetChildAt(0), duration, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_selectionIcon, duration, Back.EaseOut, "delay", "0.25", "ScaleX", "1", "ScaleY", "1");
            float num = 0.2f;
            for (int i = 6; i < m_enchantressUI.NumChildren - 3; i++)
            {
                num += 0.05f;
                Tween.To(m_enchantressUI.GetChildAt(i), duration, Back.EaseOut, "delay", num.ToString(), "ScaleX", "1",
                    "ScaleY", "1");
            }
            Tween.To(m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 1), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 2), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 3), duration, Back.EaseOut, "delay",
                num.ToString(), "ScaleX", "1", "ScaleY", "1");
            Tween.AddEndHandlerToLastTween(this, "EaseInComplete");
        }

        public void EaseInComplete()
        {
            m_lockControls = false;
        }

        private void EaseOutMenu()
        {
            foreach (SpriteObj current in m_newIconList)
            {
                current.Visible = false;
            }
            m_equippedIcon.Visible = false;
            Tween.To(m_confirmText, 0.2f, Linear.EaseNone, "Opacity", "0");
            Tween.To(m_cancelText, 0.2f, Linear.EaseNone, "Opacity", "0");
            Tween.To(m_navigationText, 0.2f, Linear.EaseNone, "Opacity", "0");
            float num = 0.4f;
            float num2 = 0f;
            Tween.To(m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 2), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            Tween.To(m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 3), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            Tween.To(m_enchantressUI.GetChildAt(m_enchantressUI.NumChildren - 4), num, Back.EaseIn, "ScaleX", "0",
                "ScaleY", "0");
            for (int i = 6; i < 11; i++)
            {
                if (m_currentCategoryIndex == i)
                {
                    Tween.To(m_selectionIcon, num, Back.EaseIn, "delay", num2.ToString(), "ScaleX", "0", "ScaleY", "0");
                }
                Tween.To(m_enchantressUI.GetChildAt(i), num, Back.EaseIn, "delay", num2.ToString(), "ScaleX", "0",
                    "ScaleY", "0");
                num2 += 0.05f;
            }
            for (int j = 1; j < 6; j++)
            {
                m_enchantressUI.GetChildAt(j).Scale = Vector2.Zero;
            }
            for (int k = 0; k < m_activeIconArray.Length; k++)
            {
                Tween.To(m_activeIconArray[k], num, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            }
            Tween.To(m_enchantressUI.GetChildAt(0), num, Back.EaseIn, "delay", "0.3", "ScaleX", "0", "ScaleY", "0");
            Tween.RunFunction(num + 0.35f, ScreenManager, "HideCurrentScreen");
        }

        private void UpdateIconStates()
        {
            for (int i = 0; i < Game.PlayerStats.GetRuneArray.Count; i++)
            {
                for (int j = 0; j < Game.PlayerStats.GetRuneArray[i].Length; j++)
                {
                    byte b = Game.PlayerStats.GetRuneArray[i][j];
                    if (b == 0)
                    {
                        m_masterIconArray[i][j].ChangeSprite("BlacksmithUI_QuestionMarkIcon_Sprite");
                    }
                    else
                    {
                        m_masterIconArray[i][j].ChangeSprite(EquipmentAbilityType.Icon(j));
                        m_masterIconArray[i][j].Opacity = 0.2f;
                    }
                    if (b >= 3)
                    {
                        m_masterIconArray[i][j].Opacity = 1f;
                    }
                }
            }
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
            if (Game.PlayerStats.TotalRunesFound >= 55)
            {
                GameUtil.UnlockAchievement("LOVE_OF_KNOWLEDGE");
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
            m_selectionIcon.Position = m_enchantressUI.GetChildAt(6).AbsPosition;
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
            for (int i = 0; i < m_enchantressUI.NumChildren; i++)
            {
                m_enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
            }
            foreach (SpriteObj[] current in m_masterIconArray)
            {
                for (int j = 0; j < current.Length; j++)
                {
                    current[j].Scale = Vector2.Zero;
                }
            }
            m_selectionIcon.Scale = Vector2.Zero;
            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
            bool flag = true;
            sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
            for (int k = 0; k < getEquippedRuneArray.Length; k++)
            {
                sbyte b = getEquippedRuneArray[k];
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
            int currentCategoryIndex = m_currentCategoryIndex;
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
                m_selectionIcon.Position = m_enchantressUI.GetChildAt(m_currentCategoryIndex).AbsPosition;
                for (int i = 1; i < 6; i++)
                {
                    if (i == 1)
                    {
                        m_enchantressUI.GetChildAt(i).Scale = new Vector2(1f, 1f);
                    }
                    else
                    {
                        m_enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
                    }
                }
                if (m_currentCategoryIndex != 6)
                {
                    m_enchantressUI.GetChildAt(m_currentCategoryIndex - 5).Scale = new Vector2(1f, 1f);
                }
                else
                {
                    m_enchantressUI.GetChildAt(m_currentCategoryIndex - 5).Scale = Vector2.Zero;
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
                byte b = Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][m_currentEquipmentIndex];
                if (b == 1)
                {
                    Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][m_currentEquipmentIndex] = 2;
                }
                UpdateNewIcons();
                UpdateIconSelectionText();
                SoundManager.PlaySound("ShopMenuConfirm");
            }
        }

        private void EquipmentSelectionInput()
        {
            int currentEquipmentIndex = m_currentEquipmentIndex;
            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                if (m_currentEquipmentIndex > 4)
                {
                    m_currentEquipmentIndex -= 5;
                }
                if (m_currentEquipmentIndex < 0)
                {
                    m_currentEquipmentIndex = 0;
                }
            }
            if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
            {
                if (m_currentEquipmentIndex < 6)
                {
                    m_currentEquipmentIndex += 5;
                }
                if (m_currentEquipmentIndex > 10)
                {
                    m_currentEquipmentIndex -= 5;
                }
            }
            if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
            {
                m_currentEquipmentIndex--;
                if ((m_currentEquipmentIndex + 1)%5 == 0)
                {
                    m_currentEquipmentIndex++;
                }
            }
            if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
            {
                m_currentEquipmentIndex++;
                if (m_currentEquipmentIndex%5 == 0 || m_currentEquipmentIndex > 10)
                {
                    m_currentEquipmentIndex--;
                }
            }
            if (currentEquipmentIndex != m_currentEquipmentIndex)
            {
                byte b = Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][m_currentEquipmentIndex];
                if (b == 1)
                {
                    Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][m_currentEquipmentIndex] = 2;
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
                m_selectionIcon.Position = m_enchantressUI.GetChildAt(m_currentCategoryIndex).AbsPosition;
                UpdateIconSelectionText();
            }
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                int num = m_currentCategoryIndex - 6;
                int num2 = Game.PlayerStats.GetRuneArray[num][m_currentEquipmentIndex];
                int num3 = Game.PlayerStats.GetEquippedRuneArray[num];
                if (num2 < 3 && num2 > 0)
                {
                    int abilityCost = Game.EquipmentSystem.GetAbilityCost(num, m_currentEquipmentIndex);
                    if (Game.PlayerStats.Gold >= abilityCost)
                    {
                        SoundManager.PlaySound("ShopMenuUnlock");
                        Game.PlayerStats.Gold -= abilityCost;
                        Game.PlayerStats.GetRuneArray[num][m_currentEquipmentIndex] = 3;
                        Game.PlayerStats.GetEquippedRuneArray[num] = (sbyte) m_currentEquipmentIndex;
                        Player.AttachedLevel.UpdatePlayerHUDAbilities();
                        SpriteObj spriteObj = m_masterIconArray[num][m_currentEquipmentIndex];
                        spriteObj.Opacity = 1f;
                        num2 = 3;
                        UpdateIconSelectionText();
                    }
                }
                if (num3 != m_currentEquipmentIndex && num2 == 3)
                {
                    m_equippedIcon.Scale = new Vector2(1f, 1f);
                    m_equippedIcon.Position = m_activeIconArray[m_currentEquipmentIndex].AbsPosition;
                    m_equippedIcon.Position += new Vector2(18f, 18f);
                    Game.PlayerStats.GetEquippedRuneArray[num] = (sbyte) m_currentEquipmentIndex;
                    Player.AttachedLevel.UpdatePlayerHUDAbilities();
                    SoundManager.PlaySound("ShopBSEquip");
                    UpdateIconSelectionText();
                    UpdateNewIcons();
                    return;
                }
                if (num3 == m_currentEquipmentIndex)
                {
                    m_equippedIcon.Scale = Vector2.Zero;
                    Game.PlayerStats.GetEquippedRuneArray[num] = -1;
                    UpdateNewIcons();
                }
            }
        }

        private void UpdateIconSelectionText()
        {
            m_equipmentDescriptionText.Position = new Vector2(-1000f, -1000f);
            m_descriptionText.Visible = false;
            m_instructionsTitleText.Visible = false;
            m_instructionsText.Visible = false;
            m_unlockCostContainer.Visible = false;
            m_equipmentTitleText.Visible = false;
            if (m_inCategoryMenu)
            {
                m_equipmentDescriptionText.Text = "Select a category";
                return;
            }
            if (Game.PlayerStats.GetRuneArray[m_currentCategoryIndex - 6][m_currentEquipmentIndex] == 0)
            {
                m_equipmentDescriptionText.Position = new Vector2(230f, -20f);
                m_equipmentDescriptionText.Text = "Rune needed";
                return;
            }
            if (Game.PlayerStats.GetRuneArray[m_currentCategoryIndex - 6][m_currentEquipmentIndex] < 3)
            {
                m_equipmentDescriptionText.Text = "Purchase Info Here";
                (m_unlockCostContainer.GetChildAt(1) as TextObj).Text =
                    Game.EquipmentSystem.GetAbilityCost(m_currentCategoryIndex - 6, m_currentEquipmentIndex).ToString() +
                    " to unlock";
                m_unlockCostContainer.Visible = true;
                m_descriptionText.Visible = true;
                m_instructionsTitleText.Visible = true;
                m_instructionsText.Visible = true;
                m_equipmentTitleText.Visible = true;
                m_descriptionText.Opacity = 0.5f;
                m_instructionsTitleText.Opacity = 0.5f;
                m_instructionsText.Opacity = 0.5f;
                m_equipmentTitleText.Opacity = 0.5f;
                UpdateEquipmentDataText();
                return;
            }
            m_descriptionText.Visible = true;
            m_instructionsTitleText.Visible = true;
            m_instructionsText.Visible = true;
            m_equipmentTitleText.Visible = true;
            m_descriptionText.Opacity = 1f;
            m_instructionsTitleText.Opacity = 1f;
            m_instructionsText.Opacity = 1f;
            m_equipmentTitleText.Opacity = 1f;
            UpdateEquipmentDataText();
        }

        private void UpdateEquipmentDataText()
        {
            m_equipmentTitleText.Text = EquipmentAbilityType.ToString(m_currentEquipmentIndex) + " Rune\n(" +
                                        EquipmentCategoryType.ToString2(m_currentCategoryIndex - 6) + ")";
            m_descriptionText.Text = EquipmentAbilityType.Description(m_currentEquipmentIndex);
            m_descriptionText.WordWrap(195);
            m_descriptionText.Y = m_equipmentTitleText.Y + 60f;
            m_instructionsTitleText.Position = new Vector2(m_enchantressUI.X + 140f,
                m_descriptionText.Bounds.Bottom + 20);
            m_instructionsText.Text = EquipmentAbilityType.Instructions(m_currentEquipmentIndex);
            m_instructionsText.WordWrap(200);
            m_instructionsText.Position = new Vector2(m_instructionsTitleText.X, m_instructionsTitleText.Bounds.Bottom);
        }

        private void UpdateNewIcons()
        {
            UpdateMoneyText();
            m_newIconListIndex = 0;
            foreach (SpriteObj current in m_newIconList)
            {
                current.Visible = false;
            }
            for (int i = 0; i < Game.PlayerStats.GetRuneArray[CurrentCategoryIndex].Length; i++)
            {
                byte b = Game.PlayerStats.GetRuneArray[CurrentCategoryIndex][i];
                if (b == 1)
                {
                    SpriteObj arg_7A_0 = m_masterIconArray[CurrentCategoryIndex][i];
                    SpriteObj spriteObj = m_newIconList[m_newIconListIndex];
                    spriteObj.Visible = true;
                    spriteObj.Position = m_masterIconArray[CurrentCategoryIndex][i].AbsPosition;
                    spriteObj.X -= 20f;
                    spriteObj.Y -= 30f;
                    m_newIconListIndex++;
                }
            }
            sbyte b2 = Game.PlayerStats.GetEquippedRuneArray[CurrentCategoryIndex];
            if (b2 > -1)
            {
                m_equippedIcon.Position = new Vector2(m_activeIconArray[b2].AbsPosition.X + 18f,
                    m_activeIconArray[b2].AbsPosition.Y + 18f);
                m_equippedIcon.Visible = true;
                return;
            }
            m_equippedIcon.Visible = false;
        }

        private void UpdateMoneyText()
        {
            m_playerMoney.Text = Game.PlayerStats.Gold.ToString();
            ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
            if (levelScreen != null)
            {
                levelScreen.UpdatePlayerHUD();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black*BackBufferOpacity);
            m_enchantressUI.Draw(Camera);
            m_selectionIcon.Draw(Camera);
            m_descriptionText.Draw(Camera);
            m_instructionsTitleText.Draw(Camera);
            m_instructionsText.Draw(Camera);
            m_unlockCostContainer.Draw(Camera);
            m_equipmentTitleText.Draw(Camera);
            foreach (SpriteObj[] current in m_masterIconArray)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    current[i].Draw(Camera);
                }
            }
            m_navigationText.Draw(Camera);
            m_cancelText.Draw(Camera);
            m_confirmText.Draw(Camera);
            m_equippedIcon.Draw(Camera);
            foreach (SpriteObj current2 in m_newIconList)
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
                Console.WriteLine("Disposing Enchantress Screen");
                if (m_rainSound != null)
                {
                    m_rainSound.Dispose();
                }
                m_rainSound = null;
                m_enchantressUI.Dispose();
                m_enchantressUI = null;
                m_equipmentDescriptionText.Dispose();
                m_equipmentDescriptionText = null;
                m_selectionIcon.Dispose();
                m_selectionIcon = null;
                m_equipmentTitleText.Dispose();
                m_equipmentTitleText = null;
                m_activeIconArray = null;
                foreach (SpriteObj[] current in m_masterIconArray)
                {
                    for (int i = 0; i < current.Length; i++)
                    {
                        current[i].Dispose();
                        current[i] = null;
                    }
                    Array.Clear(current, 0, current.Length);
                }
                m_masterIconArray.Clear();
                m_masterIconArray = null;
                m_descriptionText.Dispose();
                m_descriptionText = null;
                m_unlockCostContainer.Dispose();
                m_unlockCostContainer = null;
                m_instructionsText.Dispose();
                m_instructionsText = null;
                m_instructionsTitleText.Dispose();
                m_instructionsTitleText = null;
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
                foreach (SpriteObj current2 in m_newIconList)
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