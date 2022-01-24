// 
//  Rogue Legacy Randomizer - ArchipelagoScreen.cs
//  Last Modified 2022-01-24
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using Archipelago;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using RogueCastle.Options;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens
{
    public class ArchipelagoScreen : Screen
    {
        private List<ArchipelagoOptionsObj> _archipelagoArray;
        private SpriteObj                   _archipelagoBar;
        private SpriteObj                   _archipelagoTitle;
        private ObjContainer                _bgSprite;
        private KeyIconTextObj              _cancelText;
        private KeyIconTextObj              _confirmText;
        private TextBoxOptionsObj           _hostname;
        private KeyIconTextObj              _navigationText;
        private TextBoxOptionsObj           _password;
        private TextBoxOptionsObj           _port;
        private ArchipelagoOptionsObj       _selectedOption;
        private int                         _selectedOptionIndex;
        private TextBoxOptionsObj           _slot;
        private bool                        _transitioning;

        public ArchipelagoScreen()
        {
            _archipelagoArray = new List<ArchipelagoOptionsObj>();
            UpdateIfCovered = true;
            DrawIfCovered = true;
        }

        public float BackBufferOpacity { get; set; }
        public bool  LockControls      { get; set; }

        public override void LoadContent()
        {
            // Background Image
            _bgSprite = new ObjContainer("SkillUnlockPlate_Character")
            {
                ForceDraw = true
            };

            // Archipelago Menu Title
            _archipelagoTitle = new SpriteObj("OptionsScreenTitle_Sprite");
            _bgSprite.AddChild(_archipelagoTitle);
            _archipelagoTitle.Position = new Vector2(0f, -(float) _bgSprite.Width / 2f + 60f);

            // Archipelago Options
            _hostname = new TextBoxOptionsObj(this, "Hostname", "archipelago.gg");
            _port = new TextBoxOptionsObj(this, "Port", "38281");
            _slot = new TextBoxOptionsObj(this, "Slot Name", "Lee");
            _password = new TextBoxOptionsObj(this, "Password", "");

            _archipelagoArray.Add(_hostname);
            _archipelagoArray.Add(_port);
            _archipelagoArray.Add(_slot);
            _archipelagoArray.Add(_password);
            _archipelagoArray.Add(new ConnectArchipelagoOptionObj(this));
            _archipelagoArray.Add(new BackToMenuArchipelagoObj(this));
            for (var i = 0; i < _archipelagoArray.Count; i++)
            {
                _archipelagoArray[i].X = 420f;
                _archipelagoArray[i].Y = 160 + i * 30;
            }

            // Scrollbar
            _archipelagoBar = new SpriteObj("OptionsBar_Sprite")
            {
                ForceDraw = true,
                Position = new Vector2(_archipelagoArray[0].X - 20f, _archipelagoArray[0].Y)
            };

            // Menu Help-text
            _confirmText = new KeyIconTextObj(Game.JunicodeFont)
            {
                Text = "to select option",
                DropShadow = new Vector2(2f, 2f),
                FontSize = 12f,
                Align = Types.TextAlign.Right,
                Position = new Vector2(1290f, 570f),
                ForceDraw = true
            };
            _cancelText = new KeyIconTextObj(Game.JunicodeFont)
            {
                Text = "to exit options",
                Align = Types.TextAlign.Right,
                DropShadow = new Vector2(2f, 2f),
                FontSize = 12f,
                Position = new Vector2(_confirmText.X, _confirmText.Y + 40f),
                ForceDraw = true
            };
            _navigationText = new KeyIconTextObj(Game.JunicodeFont)
            {
                Text = "to navigate options",
                Align = Types.TextAlign.Right,
                DropShadow = new Vector2(2f, 2f),
                FontSize = 12f,
                Position = new Vector2(_confirmText.X, _confirmText.Y + 80f),
                ForceDraw = true
            };

            base.LoadContent();
        }

        public void Connect()
        {
            LockControls = true;

            try
            {
                // Parse port and connect.
                var port = int.Parse(_port.GetValue);
                Program.Game.ArchipelagoManager.Connect(new ConnectionInfo
                {
                    Hostname = _hostname.GetValue,
                    Port = port,
                    Name = _slot.GetValue,
                    Password = _password.GetValue
                });
            }
            catch (FormatException ex)
            {
                // TODO: Make this into a standardized message handler?
                var screenManager = Game.ScreenManager;
                var errorUuid = Guid.NewGuid().ToString();

                // Print exception message.
                Console.WriteLine(ex);
                DialogueManager.AddText(errorUuid, new[] { "Invalid Port" }, new[] { ex.Message });
                screenManager.DialogueScreen.SetDialogue(errorUuid);
                screenManager.DisplayScreen((int) ScreenType.Dialogue, true);
            }
            catch (Exception ex)
            {
                var screenManager = Game.ScreenManager;
                var errorUuid = Guid.NewGuid().ToString();

                // Print exception message.
                Console.WriteLine(ex);
                DialogueManager.AddText(errorUuid, new[] { "An Exception Occurred" }, new[] { ex.Message });
                screenManager.DialogueScreen.SetDialogue(errorUuid);
                screenManager.DisplayScreen((int) ScreenType.Dialogue, true);
            }
            finally
            {
                LockControls = false;
            }
        }

        public override void OnEnter()
        {
            // Show correct icons based on input device.
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                _confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
                _cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
                _navigationText.Text = "[Button:LeftStick] to navigate options";
            }
            else
            {
                _confirmText.ForcedScale = new Vector2(1f, 1f);
                _cancelText.ForcedScale = new Vector2(1f, 1f);
                _navigationText.Text = "Arrow keys to navigate options";
            }

            _confirmText.Text = "[Input:" + 0 + "] to select option";
            _cancelText.Text = "[Input:" + 2 + "] to exit options";
            _confirmText.Opacity = 0f;
            _cancelText.Opacity = 0f;
            _navigationText.Opacity = 0f;
            Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "DialogueMenuOpen");
            _transitioning = true;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");
            _selectedOptionIndex = 0;
            _selectedOption = _archipelagoArray[_selectedOptionIndex];
            _selectedOption.IsActive = false;
            _bgSprite.Position = new Vector2(660f, 0f);
            _bgSprite.Opacity = 0f;
            Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
            Tween.AddEndHandlerToLastTween(this, "EndTransition");
            Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            foreach (var current in _archipelagoArray)
            {
                current.Y = 160 + num * 30 - 360f;
                current.Opacity = 0f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "1");
                current.Initialize();
                num++;
            }

            _archipelagoBar.Opacity = 0f;
            Tween.To(_archipelagoBar, 0.2f, Tween.EaseNone, "Opacity", "1");
            base.OnEnter();
        }

        public void EndTransition()
        {
            _transitioning = false;
        }

        public void ExitTransition()
        {
            SoundManager.PlaySound("DialogMenuClose");
            _transitioning = true;
            Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0");
            Tween.To(_archipelagoBar, 0.2f, Tween.EaseNone, "Opacity", "0");
            _bgSprite.Position = new Vector2(660f, 360f);
            _bgSprite.Opacity = 1f;
            Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
            Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");
            var num = 0;
            foreach (var current in _archipelagoArray)
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
            _selectedOption.IsActive = false;
            _selectedOption.IsSelected = false;
            _selectedOption = null;
            (ScreenManager.Game as Game).SaveConfig();
            (ScreenManager as RCScreenManager).UpdatePauseScreenIcons();
            base.OnExit();
        }

        public override void HandleInput()
        {
            if (!_transitioning)
            {
                if (_selectedOption.IsActive)
                {
                    _selectedOption.HandleInput();
                    return;
                }

                var selectedOptionIndex = _selectedOptionIndex;
                if (InputTypeHelper.PressedUp)
                {
                    if (_selectedOptionIndex > 0)
                    {
                        SoundManager.PlaySound("frame_swap");
                    }

                    _selectedOptionIndex--;
                }
                else if (InputTypeHelper.PressedDown)
                {
                    if (_selectedOptionIndex < _archipelagoArray.Count - 1)
                    {
                        SoundManager.PlaySound("frame_swap");
                    }

                    _selectedOptionIndex++;
                }

                if (_selectedOptionIndex < 0)
                {
                    _selectedOptionIndex = _archipelagoArray.Count - 1;
                }

                if (_selectedOptionIndex > _archipelagoArray.Count - 1)
                {
                    _selectedOptionIndex = 0;
                }

                if (selectedOptionIndex != _selectedOptionIndex)
                {
                    if (_selectedOption != null)
                    {
                        _selectedOption.IsSelected = false;
                    }

                    _selectedOption = _archipelagoArray[_selectedOptionIndex];
                    _selectedOption.IsSelected = true;
                }

                if (InputTypeHelper.PressedConfirm)
                {
                    SoundManager.PlaySound("Option_Menu_Select");
                    LockControls = true;
                    _selectedOption.IsActive = true;
                }

                if (InputTypeHelper.PressedAny(InputType.MenuCancel1, InputType.MenuCancel1, InputType.MenuOptions))
                {
                    ExitTransition();
                }
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in _archipelagoArray)
            {
                current.Update(gameTime);
            }

            _archipelagoBar.Position = new Vector2(_selectedOption.X - 15f, _selectedOption.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _bgSprite.Draw(Camera);
            foreach (var current in _archipelagoArray)
            {
                current.Draw(Camera);
            }

            _confirmText.Draw(Camera);
            _cancelText.Draw(Camera);
            _navigationText.Draw(Camera);
            _archipelagoBar.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Archipelago Screen");
            foreach (var current in _archipelagoArray)
            {
                current.Dispose();
            }

            _archipelagoArray.Clear();
            _archipelagoArray = null;
            _bgSprite.Dispose();
            _bgSprite = null;
            _archipelagoTitle = null;
            _confirmText.Dispose();
            _confirmText = null;
            _cancelText.Dispose();
            _cancelText = null;
            _navigationText.Dispose();
            _navigationText = null;
            _archipelagoBar.Dispose();
            _archipelagoBar = null;
            _selectedOption = null;
            base.Dispose();
        }
    }
}
