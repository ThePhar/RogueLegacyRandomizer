// Rogue Legacy Randomizer - RandomizerScreen.cs
// Last Modified 2022-10-28
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Archipelago;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Options;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class RandomizerScreen : Screen
    {
        private List<RandomizerOption> _multiRandomizerOptions;
        private List<RandomizerOption> _soloRandomizerOptions;
        private RandoModeOption        _randoMode;
        private SpriteObj              _randomizerBar;
        private SpriteObj              _randomizerTitle;
        private SpriteObj              _upArrow;
        private SpriteObj              _downArrow;
        private ObjContainer           _bgSprite;
        private KeyIconTextObj         _cancelText;
        private KeyIconTextObj         _confirmText;
        private RandomizerOption       _selectedOption;
        private int                    _selectedOptionIndex;
        private TextBoxOption          _slot;
        private bool                   _transitioning;

        // Solo
        private TextBoxOption _startingName;

        // MultiWorld
        private TextBoxOption  _hostname;
        private KeyIconTextObj _navigationText;
        private TextBoxOption  _password;
        private TextBoxOption  _port;

        public RandomizerScreen()
        {
            _multiRandomizerOptions = new List<RandomizerOption>();
            _soloRandomizerOptions = new List<RandomizerOption>();
            UpdateIfCovered = true;
            DrawIfCovered = true;
        }

        private List<RandomizerOption> RandomizerOptions
        {
            get
            {
                var list = new List<RandomizerOption> { _randoMode };
                return list.Concat(Game.RandomizerOptions.IsArchipelago ? _multiRandomizerOptions : _soloRandomizerOptions).ToList();
            }
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

            // Randomizer Menu Title
            _randomizerTitle = new SpriteObj("OptionsScreenTitle_Sprite");
            _bgSprite.AddChild(_randomizerTitle);
            _randomizerTitle.Position = new Vector2(0f, -(float) _bgSprite.Width / 2f + 60f);

            // Arrows
            _upArrow = new SpriteObj("ContinueTextIcon_Sprite");
            _downArrow = new SpriteObj("ContinueTextIcon_Sprite");
            _bgSprite.AddChild(_upArrow);
            _bgSprite.AddChild(_downArrow);
            _upArrow.Visible = false;

            _upArrow.Position = new Vector2(0 - _upArrow.Width / 2, -150);
            _upArrow.Rotation = 180;
            _downArrow.Position = new Vector2(0 - _downArrow.Width / 2, 190);

            // Randomizer Mode Selector
            _randoMode = new RandoModeOption(this);

            // Solo Randomizer Options
            _startingName = new TextBoxOption(this, "Starting Name", "Lee");

            // _soloRandomizerOptions.Add(_startingName);
            _soloRandomizerOptions.Add(new TextBoxOption(this, "Currently Not Working", ""));

            // MultiWorld Randomizer Options
            _hostname = new TextBoxOption(this, "Hostname", !LevelENV.RunConsole ? "archipelago.gg" : "localhost");
            _port = new TextBoxOption(this, "Port", "38281");
            _slot = new TextBoxOption(this, "Slot Name", !LevelENV.RunConsole ? "Sir Lee" : "Phar");
            _password = new TextBoxOption(this, "Password", "");

            _multiRandomizerOptions.Add(_hostname);
            _multiRandomizerOptions.Add(_port);
            _multiRandomizerOptions.Add(_slot);
            _multiRandomizerOptions.Add(_password);
            _multiRandomizerOptions.Add(new ConnectArchipelagoOptionObj(this));
            _multiRandomizerOptions.Add(new BackToMenuArchipelagoObj(this));

            _randoMode.X = 420f;
            _randoMode.Y = 160f;

            for (var i = 0; i < _multiRandomizerOptions.Count; i++)
            {
                _multiRandomizerOptions[i].X = 420f;
                _multiRandomizerOptions[i].Y = 180 + (i + 1) * 30;
            }

            for (var i = 0; i < _soloRandomizerOptions.Count; i++)
            {
                if (i < 11)
                {
                    _soloRandomizerOptions[i].X = 420f;
                    _soloRandomizerOptions[i].Y = 180 + (i + 1) * 30;
                }
                else
                {
                    _soloRandomizerOptions[i].Visible = false;
                }
            }

            // Scrollbar
            _randomizerBar = new SpriteObj("OptionsBar_Sprite")
            {
                ForceDraw = true,
                Position = new Vector2(RandomizerOptions[0].X - 20f, RandomizerOptions[0].Y)
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
            _selectedOption = RandomizerOptions[_selectedOptionIndex];
            _selectedOption.IsActive = false;
            _bgSprite.Position = new Vector2(660f, 0f);
            _bgSprite.Opacity = 0f;
            Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
            Tween.AddEndHandlerToLastTween(this, "EndTransition");
            Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            foreach (var current in RandomizerOptions)
            {
                current.Y = 180 + num * 30 - 360f;

                if (current is RandoModeOption)
                    current.Y = -200f;

                if (num < 12)
                {
                    current.Visible = true;
                }
                else
                {
                    current.Visible = false;
                }

                current.Opacity = 0f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "1");
                current.Initialize();
                num++;
            }

            _randomizerBar.Opacity = 0f;
            Tween.To(_randomizerBar, 0.2f, Tween.EaseNone, "Opacity", "1");
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
            Tween.To(_randomizerBar, 0.2f, Tween.EaseNone, "Opacity", "0");
            _bgSprite.Position = new Vector2(660f, 360f);
            _bgSprite.Opacity = 1f;
            Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
            Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");
            foreach (var current in RandomizerOptions)
            {
                current.Opacity = 1f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", (-360f).ToString(CultureInfo.InvariantCulture));
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "0");
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
                    if (_selectedOptionIndex < RandomizerOptions.Count - 1)
                    {
                        SoundManager.PlaySound("frame_swap");
                    }

                    _selectedOptionIndex++;
                }

                if (_selectedOptionIndex < 0)
                {
                    _selectedOptionIndex = RandomizerOptions.Count - 1;
                }

                if (_selectedOptionIndex > RandomizerOptions.Count - 1)
                {
                    _selectedOptionIndex = 0;
                }

                // If moved, readjust the order of options.
                if ((InputTypeHelper.PressedUp || InputTypeHelper.PressedDown) && !Game.RandomizerOptions.IsArchipelago)
                {
                    if (_selectedOptionIndex == 0)
                    {
                        for (var i = 0; i < _soloRandomizerOptions.Count; i++)
                        {
                            _soloRandomizerOptions[i].X = 420;

                            if (i < 11)
                            {
                                _soloRandomizerOptions[i].Visible = true;
                                _soloRandomizerOptions[i].Y = 180 + (i + 1) * 30;
                            }
                            else
                            {
                                _soloRandomizerOptions[i].Visible = false;
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < _soloRandomizerOptions.Count; i++)
                        {
                            var trueIndex = _selectedOptionIndex - 1;
                            _soloRandomizerOptions[i].X = 420;

                            // Check if in first 6 values.
                            if (trueIndex < 6)
                            {
                                if (i < 11)
                                {
                                    _soloRandomizerOptions[i].Visible = true;
                                    _soloRandomizerOptions[i].Y = 180 + (i + 1) * 30;
                                }
                                else
                                {
                                    _soloRandomizerOptions[i].Visible = false;
                                }
                            }
                            // Check if in last 6 values.
                            else if (trueIndex > _soloRandomizerOptions.Count - 7)
                            {
                                if (i > _soloRandomizerOptions.Count - 12)
                                {
                                    _soloRandomizerOptions[i].Visible = true;
                                    _soloRandomizerOptions[i].Y = 180 + (i - (_soloRandomizerOptions.Count - 12)) * 30;
                                }
                                else
                                {
                                    _soloRandomizerOptions[i].Visible = false;
                                }
                            }
                            // Reorganize everything.
                            else
                            {
                                if (i < trueIndex - 5 || i > trueIndex + 5)
                                {
                                    _soloRandomizerOptions[i].Visible = false;
                                }
                                else
                                {
                                    _soloRandomizerOptions[i].Visible = true;
                                    _soloRandomizerOptions[i].Y = 180 + (i + (6 - trueIndex)) * 30;
                                }
                            }
                        }
                    }
                }

                if (selectedOptionIndex != _selectedOptionIndex)
                {
                    if (_selectedOption != null)
                    {
                        _selectedOption.IsSelected = false;
                    }

                    _selectedOption = RandomizerOptions[_selectedOptionIndex];
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

            // Hide arrows if on Archipelago settings.
            if (Game.RandomizerOptions.IsArchipelago)
            {
                _upArrow.Visible = false;
                _downArrow.Visible = false;
            }
            else
            {
                if (_selectedOptionIndex <= 6)
                {
                    _upArrow.Visible = false;
                    _downArrow.Visible = true;
                }
                else if (_selectedOptionIndex >= RandomizerOptions.Count - 6)
                {
                    _upArrow.Visible = true;
                    _downArrow.Visible = false;
                }
                else
                {
                    _upArrow.Visible = true;
                    _downArrow.Visible = true;
                }
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in RandomizerOptions)
            {
                current.Update(gameTime);
            }

            _randomizerBar.Position = new Vector2(_selectedOption.X - 15f, _selectedOption.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _bgSprite.Draw(Camera);
            foreach (var current in RandomizerOptions)
            {
                current.Draw(Camera);
            }

            _confirmText.Draw(Camera);
            _cancelText.Draw(Camera);
            _navigationText.Draw(Camera);
            _randomizerBar.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed) return;

            Console.WriteLine("Disposing Randomizer Screen");
            foreach (var current in _multiRandomizerOptions)
            {
                current.Dispose();
            }

            foreach (var current in _soloRandomizerOptions)
            {
                current.Dispose();
            }

            _multiRandomizerOptions.Clear();
            _multiRandomizerOptions = null;
            _soloRandomizerOptions.Clear();
            _soloRandomizerOptions = null;
            _bgSprite.Dispose();
            _bgSprite = null;
            _randomizerTitle = null;
            _confirmText.Dispose();
            _confirmText = null;
            _cancelText.Dispose();
            _cancelText = null;
            _navigationText.Dispose();
            _navigationText = null;
            _randomizerBar.Dispose();
            _randomizerBar = null;
            _selectedOption = null;
            base.Dispose();
        }
    }
}
