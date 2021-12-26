// 
// RogueLegacyArchipelago - ArchipelagoScreen.cs
// Last Modified 2021-12-26
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.TypeDefinitions;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class ArchipelagoScreen : Screen
    {
        private ObjContainer m_bgSprite;
        private KeyIconTextObj m_cancelText;
        private KeyIconTextObj m_confirmText;
        private KeyIconTextObj m_navigationText;
        private List<ArchipelagoOptionsObj> m_archipelagoArray;
        private SpriteObj m_archipelagoBar;
        private SpriteObj m_archipelagoTitle;
        private ArchipelagoOptionsObj m_selectedOption;
        private int m_selectedOptionIndex;
        private bool m_transitioning;

        private TextBoxOptionsObj m_hostname;
        private TextBoxOptionsObj m_port;
        private TextBoxOptionsObj m_slot;
        private TextBoxOptionsObj m_password;

        public ArchipelagoScreen()
        {
            m_archipelagoArray = new List<ArchipelagoOptionsObj>();
            UpdateIfCovered = true;
            DrawIfCovered = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            // Background Image
            m_bgSprite = new ObjContainer("SkillUnlockPlate_Character")
            {
                ForceDraw = true
            };

            // Archipelago Menu Title
            m_archipelagoTitle = new SpriteObj("OptionsScreenTitle_Sprite");
            m_bgSprite.AddChild(m_archipelagoTitle);
            m_archipelagoTitle.Position = new Vector2(0f, -(float) m_bgSprite.Width / 2f + 60f);

            // Archipelago Options
            m_hostname = new TextBoxOptionsObj(this, "Hostname", "localhost");
            m_port = new TextBoxOptionsObj(this, "Port", "38281");
            m_slot = new TextBoxOptionsObj(this, "Slot Name", "Phar");
            m_password = new TextBoxOptionsObj(this, "Password", "");

            m_archipelagoArray.Add(m_hostname);
            m_archipelagoArray.Add(m_port);
            m_archipelagoArray.Add(m_slot);
            m_archipelagoArray.Add(m_password);
            m_archipelagoArray.Add(new ConnectArchipelagoOptionObj(this));
            m_archipelagoArray.Add(new BackToMenuArchipelagoObj(this));
            for (var i = 0; i < m_archipelagoArray.Count; i++)
            {
                m_archipelagoArray[i].X = 420f;
                m_archipelagoArray[i].Y = 160 + (i * 30);
            }

            // Scrollbar
            m_archipelagoBar = new SpriteObj("OptionsBar_Sprite")
            {
                ForceDraw = true,
                Position = new Vector2(m_archipelagoArray[0].X - 20f, m_archipelagoArray[0].Y)
            };

            // Menu Help-text
            m_confirmText = new KeyIconTextObj(Game.JunicodeFont)
            {
                Text = "to select option",
                DropShadow = new Vector2(2f, 2f),
                FontSize = 12f,
                Align = Types.TextAlign.Right,
                Position = new Vector2(1290f, 570f),
                ForceDraw = true
            };
            m_cancelText = new KeyIconTextObj(Game.JunicodeFont)
            {
                Text = "to exit options",
                Align = Types.TextAlign.Right,
                DropShadow = new Vector2(2f, 2f),
                FontSize = 12f,
                Position = new Vector2(m_confirmText.X, m_confirmText.Y + 40f),
                ForceDraw = true
            };
            m_navigationText = new KeyIconTextObj(Game.JunicodeFont)
            {
                Text = "to navigate options",
                Align = Types.TextAlign.Right,
                DropShadow = new Vector2(2f, 2f),
                FontSize = 12f,
                Position = new Vector2(m_confirmText.X, m_confirmText.Y + 80f),
                ForceDraw = true
            };

            base.LoadContent();
        }

        public void Connect()
        {
            try
            {
                // Parse port and connect.
                var port = int.Parse(m_port.GetValue);
                Program.Game.ArchClient.Connect(m_hostname.GetValue, port, m_slot.GetValue, m_password.GetValue == "" ? null : m_password.GetValue);
            }
            catch (FormatException ex)
            {
                // TODO: Make this into a standardized message handler?
                var screenManager = Game.ScreenManager;
                var errorUuid = Guid.NewGuid().ToString();

                // Print exception message.
                Console.WriteLine(ex);
                DialogueManager.AddText(errorUuid, new[] {"Invalid Port"}, new[] {ex.Message});
                screenManager.DialogueScreen.SetDialogue(errorUuid);
                screenManager.DisplayScreen(ScreenType.Dialogue, true);
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

        public override void OnEnter()
        {
            // Show correct icons based on input device.
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
            Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "DialogueMenuOpen");
            m_transitioning = true;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");
            m_selectedOptionIndex = 0;
            m_selectedOption = m_archipelagoArray[m_selectedOptionIndex];
            m_selectedOption.IsActive = false;
            m_bgSprite.Position = new Vector2(660f, 0f);
            m_bgSprite.Opacity = 0f;
            Tween.To(m_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
            Tween.AddEndHandlerToLastTween(this, "EndTransition");
            Tween.To(m_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            foreach (var current in m_archipelagoArray)
            {
                current.Y = 160 + num * 30 - 360f;
                current.Opacity = 0f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "1");
                current.Initialize();
                num++;
            }

            m_archipelagoBar.Opacity = 0f;
            Tween.To(m_archipelagoBar, 0.2f, Tween.EaseNone, "Opacity", "1");
            base.OnEnter();
        }

        public void EndTransition()
        {
            m_transitioning = false;
        }

        public void ExitTransition()
        {
            SoundManager.PlaySound("DialogMenuClose");
            m_transitioning = true;
            Tween.To(m_confirmText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(m_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(m_navigationText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0");
            Tween.To(m_archipelagoBar, 0.2f, Tween.EaseNone, "Opacity", "0");
            m_bgSprite.Position = new Vector2(660f, 360f);
            m_bgSprite.Opacity = 1f;
            Tween.To(m_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
            Tween.To(m_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");
            var num = 0;
            foreach (var current in m_archipelagoArray)
            {
                current.Y = 160 + num * 30;
                current.Opacity = 1f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", (-360f).ToString(CultureInfo.InvariantCulture));
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
                            if (m_selectedOptionIndex < m_archipelagoArray.Count - 1)
                            {
                                SoundManager.PlaySound("frame_swap");
                            }

                            m_selectedOptionIndex++;
                        }

                        if (m_selectedOptionIndex < 0)
                        {
                            m_selectedOptionIndex = m_archipelagoArray.Count - 1;
                        }

                        if (m_selectedOptionIndex > m_archipelagoArray.Count - 1)
                        {
                            m_selectedOptionIndex = 0;
                        }

                        if (selectedOptionIndex != m_selectedOptionIndex)
                        {
                            if (m_selectedOption != null)
                            {
                                m_selectedOption.IsSelected = false;
                            }

                            m_selectedOption = m_archipelagoArray[m_selectedOptionIndex];
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
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in m_archipelagoArray)
            {
                current.Update(gameTime);
            }

            m_archipelagoBar.Position = new Vector2(m_selectedOption.X - 15f, m_selectedOption.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            m_bgSprite.Draw(Camera);
            foreach (var current in m_archipelagoArray)
            {
                current.Draw(Camera);
            }

            m_confirmText.Draw(Camera);
            m_cancelText.Draw(Camera);
            m_navigationText.Draw(Camera);
            m_archipelagoBar.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Archipelago Screen");
                foreach (var current in m_archipelagoArray)
                {
                    current.Dispose();
                }

                m_archipelagoArray.Clear();
                m_archipelagoArray = null;
                m_bgSprite.Dispose();
                m_bgSprite = null;
                m_archipelagoTitle = null;
                m_confirmText.Dispose();
                m_confirmText = null;
                m_cancelText.Dispose();
                m_cancelText = null;
                m_navigationText.Dispose();
                m_navigationText = null;
                m_archipelagoBar.Dispose();
                m_archipelagoBar = null;
                m_selectedOption = null;
                base.Dispose();
            }
        }
    }
}
