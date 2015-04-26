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
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class OptionsScreen : Screen
    {
        private OptionsObj m_backToMenuObj;
        private ObjContainer m_bgSprite;
        private KeyIconTextObj m_cancelText;
        private SpriteObj m_changeControlsTitle;
        private bool m_changingControls;
        private KeyIconTextObj m_confirmText;
        private OptionsObj m_enableSteamCloudObj;
        private KeyIconTextObj m_navigationText;
        private List<OptionsObj> m_optionsArray;
        private SpriteObj m_optionsBar;
        private SpriteObj m_optionsTitle;
        private OptionsObj m_quickDropObj;
        private TextObj m_quickDropText;
        private OptionsObj m_reduceQualityObj;
        private OptionsObj m_selectedOption;
        private int m_selectedOptionIndex;
        private bool m_titleScreenOptions;
        private bool m_transitioning;

        public OptionsScreen()
        {
            m_optionsArray = new List<OptionsObj>();
            UpdateIfCovered = true;
            DrawIfCovered = true;
            m_titleScreenOptions = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_bgSprite = new ObjContainer("SkillUnlockPlate_Character");
            m_bgSprite.ForceDraw = true;
            m_optionsTitle = new SpriteObj("OptionsScreenTitle_Sprite");
            m_bgSprite.AddChild(m_optionsTitle);
            m_optionsTitle.Position = new Vector2(0f, -(float) m_bgSprite.Width/2f + 60f);
            m_changeControlsTitle = new SpriteObj("OptionsScreenChangeControls_Sprite");
            m_bgSprite.AddChild(m_changeControlsTitle);
            m_changeControlsTitle.Position = new Vector2(1320f, m_optionsTitle.Y);
            m_optionsArray.Add(new ResolutionOptionsObj(this));
            m_optionsArray.Add(new FullScreenOptionsObj(this));
            m_reduceQualityObj = new ReduceQualityOptionsObj(this);
            m_optionsArray.Add(m_reduceQualityObj);
            m_optionsArray.Add(new MusicVolOptionsObj(this));
            m_optionsArray.Add(new SFXVolOptionsObj(this));
            m_quickDropObj = new QuickDropOptionsObj(this);
            m_optionsArray.Add(m_quickDropObj);
            m_optionsArray.Add(new DeadZoneOptionsObj(this));
            m_optionsArray.Add(new ToggleDirectInputOptionsObj(this));
            m_optionsArray.Add(new ChangeControlsOptionsObj(this));
            m_optionsArray.Add(new ExitProgramOptionsObj(this));
            m_backToMenuObj = new BackToMenuOptionsObj(this);
            m_backToMenuObj.X = 420f;
            for (var i = 0; i < m_optionsArray.Count; i++)
            {
                m_optionsArray[i].X = 420f;
                m_optionsArray[i].Y = 160 + i*30;
            }
            m_optionsBar = new SpriteObj("OptionsBar_Sprite");
            m_optionsBar.ForceDraw = true;
            m_optionsBar.Position = new Vector2(m_optionsArray[0].X - 20f, m_optionsArray[0].Y);
            m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
            m_confirmText.Text = "to select option";
            m_confirmText.DropShadow = new Vector2(2f, 2f);
            m_confirmText.FontSize = 12f;
            m_confirmText.Align = Types.TextAlign.Right;
            m_confirmText.Position = new Vector2(1290f, 570f);
            m_confirmText.ForceDraw = true;
            m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
            m_cancelText.Text = "to exit options";
            m_cancelText.Align = Types.TextAlign.Right;
            m_cancelText.DropShadow = new Vector2(2f, 2f);
            m_cancelText.FontSize = 12f;
            m_cancelText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 40f);
            m_cancelText.ForceDraw = true;
            m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
            m_navigationText.Text = "to navigate options";
            m_navigationText.Align = Types.TextAlign.Right;
            m_navigationText.DropShadow = new Vector2(2f, 2f);
            m_navigationText.FontSize = 12f;
            m_navigationText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 80f);
            m_navigationText.ForceDraw = true;
            m_quickDropText = new TextObj(Game.JunicodeFont);
            m_quickDropText.FontSize = 8f;
            m_quickDropText.Text =
                "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
            m_quickDropText.Position = new Vector2(420f, 530f);
            m_quickDropText.ForceDraw = true;
            m_quickDropText.DropShadow = new Vector2(2f, 2f);
            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            m_titleScreenOptions = (bool) objList[0];
            base.PassInData(objList);
        }

        public override void OnEnter()
        {
            m_quickDropText.Visible = false;
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                m_confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
                m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
                m_navigationText.Text = "[Button:LeftStick] to navigate options";
            }
            else
            {
                m_confirmText.ForcedScale = new Vector2(1f, 1f);
                m_cancelText.ForcedScale = new Vector2(1f, 1f);
                m_navigationText.Text = "Arrow keys to navigate options";
            }
            m_confirmText.Text = "[Input:" + 0 + "] to select option";
            m_cancelText.Text = "[Input:" + 2 + "] to exit options";
            m_confirmText.Opacity = 0f;
            m_cancelText.Opacity = 0f;
            m_navigationText.Opacity = 0f;
            Tween.To(m_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(m_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(m_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.RunFunction(0.1f, typeof (SoundManager), "PlaySound", "DialogueMenuOpen");
            if (!m_optionsArray.Contains(m_backToMenuObj))
            {
                m_optionsArray.Insert(m_optionsArray.Count - 1, m_backToMenuObj);
            }
            if (m_titleScreenOptions)
            {
                m_optionsArray.RemoveAt(m_optionsArray.Count - 2);
            }
            m_transitioning = true;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");
            m_selectedOptionIndex = 0;
            m_selectedOption = m_optionsArray[m_selectedOptionIndex];
            m_selectedOption.IsActive = false;
            m_bgSprite.Position = new Vector2(660f, 0f);
            m_bgSprite.Opacity = 0f;
            Tween.To(m_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
            Tween.AddEndHandlerToLastTween(this, "EndTransition");
            Tween.To(m_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            foreach (var current in m_optionsArray)
            {
                current.Y = 160 + num*30 - 360f;
                current.Opacity = 0f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "1");
                current.Initialize();
                num++;
            }
            m_optionsBar.Opacity = 0f;
            Tween.To(m_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "1");
            base.OnEnter();
        }

        public void EndTransition()
        {
            m_transitioning = false;
        }

        private void ExitTransition()
        {
            SoundManager.PlaySound("DialogMenuClose");
            m_transitioning = true;
            Tween.To(m_confirmText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(m_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(m_navigationText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0");
            Tween.To(m_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "0");
            m_bgSprite.Position = new Vector2(660f, 360f);
            m_bgSprite.Opacity = 1f;
            Tween.To(m_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
            Tween.To(m_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");
            var num = 0;
            foreach (var current in m_optionsArray)
            {
                current.Y = 160 + num*30;
                current.Opacity = 1f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", (-360f).ToString());
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "0");
                num++;
            }
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
        }

        public override void OnExit()
        {
            m_selectedOption.IsActive = false;
            m_selectedOption.IsSelected = false;
            m_selectedOption = null;
            (ScreenManager.Game as Game).SaveConfig();
            (ScreenManager as RCScreenManager).UpdatePauseScreenIcons();
            base.OnExit();
        }

        public override void HandleInput()
        {
            if (!m_transitioning)
            {
                if (m_selectedOption.IsActive)
                {
                    m_selectedOption.HandleInput();
                }
                else
                {
                    if (!m_selectedOption.IsActive)
                    {
                        var selectedOptionIndex = m_selectedOptionIndex;
                        if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                        {
                            if (m_selectedOptionIndex > 0)
                            {
                                SoundManager.PlaySound("frame_swap");
                            }
                            m_selectedOptionIndex--;
                        }
                        else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
                        {
                            if (m_selectedOptionIndex < m_optionsArray.Count - 1)
                            {
                                SoundManager.PlaySound("frame_swap");
                            }
                            m_selectedOptionIndex++;
                        }
                        if (m_selectedOptionIndex < 0)
                        {
                            m_selectedOptionIndex = m_optionsArray.Count - 1;
                        }
                        if (m_selectedOptionIndex > m_optionsArray.Count - 1)
                        {
                            m_selectedOptionIndex = 0;
                        }
                        if (selectedOptionIndex != m_selectedOptionIndex)
                        {
                            if (m_selectedOption != null)
                            {
                                m_selectedOption.IsSelected = false;
                            }
                            m_selectedOption = m_optionsArray[m_selectedOptionIndex];
                            m_selectedOption.IsSelected = true;
                        }
                    }
                    if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                    {
                        SoundManager.PlaySound("Option_Menu_Select");
                        m_selectedOption.IsActive = true;
                    }
                    if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) ||
                        Game.GlobalInput.JustPressed(4))
                    {
                        ExitTransition();
                    }
                }
                if (m_selectedOption == m_quickDropObj)
                {
                    m_quickDropText.Visible = true;
                    m_quickDropText.Text =
                        "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
                }
                else if (m_selectedOption == m_reduceQualityObj)
                {
                    m_quickDropText.Visible = true;
                    m_quickDropText.Text = "*The game must be restarted for this change to come into effect.";
                }
                else if (m_selectedOption == m_enableSteamCloudObj)
                {
                    m_quickDropText.Visible = true;
                    m_quickDropText.Text =
                        "*Cloud support must be enabled on the Steam platform as well for\nthis feature to work.";
                }
                else
                {
                    m_quickDropText.Visible = false;
                }
            }
            else
            {
                m_quickDropText.Visible = false;
            }
            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in m_optionsArray)
            {
                current.Update(gameTime);
            }
            m_optionsBar.Position = new Vector2(m_selectedOption.X - 15f, m_selectedOption.Y);
            base.Update(gameTime);
        }

        public void ToggleControlsConfig()
        {
            if (!m_changingControls)
            {
                foreach (var current in m_optionsArray)
                {
                    Tween.By(current, 0.3f, Quad.EaseInOut, "X", "-1320");
                }
                Tween.By(m_optionsTitle, 0.3f, Quad.EaseInOut, "X", "-1320");
                Tween.By(m_changeControlsTitle, 0.3f, Quad.EaseInOut, "X", "-1320");
                m_changingControls = true;
                return;
            }
            foreach (var current2 in m_optionsArray)
            {
                Tween.By(current2, 0.3f, Quad.EaseInOut, "X", "1320");
            }
            Tween.By(m_optionsTitle, 0.3f, Quad.EaseInOut, "X", "1320");
            Tween.By(m_changeControlsTitle, 0.3f, Quad.EaseInOut, "X", "1320");
            m_changingControls = false;
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black*BackBufferOpacity);
            m_bgSprite.Draw(Camera);
            foreach (var current in m_optionsArray)
            {
                current.Draw(Camera);
            }
            m_quickDropText.Draw(Camera);
            m_confirmText.Draw(Camera);
            m_cancelText.Draw(Camera);
            m_navigationText.Draw(Camera);
            m_optionsBar.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Options Screen");
                foreach (var current in m_optionsArray)
                {
                    current.Dispose();
                }
                m_optionsArray.Clear();
                m_optionsArray = null;
                m_bgSprite.Dispose();
                m_bgSprite = null;
                m_optionsTitle = null;
                m_changeControlsTitle = null;
                m_backToMenuObj = null;
                m_confirmText.Dispose();
                m_confirmText = null;
                m_cancelText.Dispose();
                m_cancelText = null;
                m_navigationText.Dispose();
                m_navigationText = null;
                m_optionsBar.Dispose();
                m_optionsBar = null;
                m_selectedOption = null;
                m_quickDropText.Dispose();
                m_quickDropText = null;
                m_quickDropObj = null;
                m_enableSteamCloudObj = null;
                m_reduceQualityObj = null;
                base.Dispose();
            }
        }
    }
}