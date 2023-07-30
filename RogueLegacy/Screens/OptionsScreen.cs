// RogueLegacyRandomizer - OptionsScreen.cs
// Last Modified 2023-07-30 9:28 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueLegacy.Options;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class OptionsScreen : Screen
    {
        private OptionsObj       _backToMenuObj;
        private ObjContainer     _bgSprite;
        private KeyIconTextObj   _cancelText;
        private SpriteObj        _changeControlsTitle;
        private bool             _changingControls;
        private KeyIconTextObj   _confirmText;
        private OptionsObj       _enableSteamCloudObj;
        private KeyIconTextObj   _navigationText;
        private List<OptionsObj> _optionsArray;
        private SpriteObj        _optionsBar;
        private SpriteObj        _optionsTitle;
        private OptionsObj       _quickDropObj;
        private OptionsObj       _toggleDeathLinkObj;
        private TextObj          _quickDropText;
        private OptionsObj       _reduceQualityObj;
        private OptionsObj       _retireCharacterOptionObj;
        private OptionsObj       _selectedOption;
        private int              _selectedOptionIndex;
        private bool             _titleScreenOptions;
        private bool             _transitioning;

        public OptionsScreen()
        {
            _optionsArray = new List<OptionsObj>();
            UpdateIfCovered = true;
            DrawIfCovered = true;
            _titleScreenOptions = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            _bgSprite = new ObjContainer("SkillUnlockPlate_Character");
            _bgSprite.ForceDraw = true;
            _optionsTitle = new SpriteObj("OptionsScreenTitle_Sprite");
            _bgSprite.AddChild(_optionsTitle);
            _optionsTitle.Position = new Vector2(0f, -(float) _bgSprite.Width / 2f + 60f);
            _changeControlsTitle = new SpriteObj("OptionsScreenChangeControls_Sprite");
            _bgSprite.AddChild(_changeControlsTitle);
            _changeControlsTitle.Position = new Vector2(1320f, _optionsTitle.Y);
            _optionsArray.Add(new ResolutionOptionsObj(this));
            _optionsArray.Add(new FullScreenOptionsObj(this));
            _reduceQualityObj = new ReduceQualityOptionsObj(this);
            _optionsArray.Add(_reduceQualityObj);
            _optionsArray.Add(new MusicVolOptionsObj(this));
            _optionsArray.Add(new SFXVolOptionsObj(this));
            _quickDropObj = new QuickDropOptionsObj(this);
            _optionsArray.Add(_quickDropObj);
            _optionsArray.Add(new DeadZoneOptionsObj(this));
            // _optionsArray.Add(new ChatOpacityOption(this));
            // _optionsArray.Add(new ChatOption(this));
            _optionsArray.Add(new ChangeControlsOptionsObj(this));
            _toggleDeathLinkObj = new DeathLinkOptionsObj(this);
            _retireCharacterOptionObj = new RetireOptionsObj(this);
            _optionsArray.Add(_toggleDeathLinkObj);
            _optionsArray.Add(_retireCharacterOptionObj);
            _optionsArray.Add(new ExitProgramOptionsObj(this));
            _backToMenuObj = new BackToMenuOptionsObj(this);
            _backToMenuObj.X = 420f;
            for (var i = 0; i < _optionsArray.Count; i++)
            {
                _optionsArray[i].X = 420f;
                _optionsArray[i].Y = 160 + i * 30;
            }

            _optionsBar = new SpriteObj("OptionsBar_Sprite");
            _optionsBar.ForceDraw = true;
            _optionsBar.Position = new Vector2(_optionsArray[0].X - 20f, _optionsArray[0].Y);
            _confirmText = new KeyIconTextObj(Game.JunicodeFont);
            _confirmText.Text = "to select option";
            _confirmText.DropShadow = new Vector2(2f, 2f);
            _confirmText.FontSize = 12f;
            _confirmText.Align = Types.TextAlign.Right;
            _confirmText.Position = new Vector2(1290f, 570f);
            _confirmText.ForceDraw = true;
            _cancelText = new KeyIconTextObj(Game.JunicodeFont);
            _cancelText.Text = "to exit options";
            _cancelText.Align = Types.TextAlign.Right;
            _cancelText.DropShadow = new Vector2(2f, 2f);
            _cancelText.FontSize = 12f;
            _cancelText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40f);
            _cancelText.ForceDraw = true;
            _navigationText = new KeyIconTextObj(Game.JunicodeFont);
            _navigationText.Text = "to navigate options";
            _navigationText.Align = Types.TextAlign.Right;
            _navigationText.DropShadow = new Vector2(2f, 2f);
            _navigationText.FontSize = 12f;
            _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 80f);
            _navigationText.ForceDraw = true;

            // Quick Drop Text
            _quickDropText = new TextObj(Game.JunicodeFont);
            _quickDropText.FontSize = 8f;
            _quickDropText.Text =
                "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
            _quickDropText.Position = new Vector2(420f, 530f);
            _quickDropText.ForceDraw = true;
            _quickDropText.DropShadow = new Vector2(2f, 2f);
            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            _titleScreenOptions = (bool) objList[0];
            base.PassInData(objList);
        }

        public override void OnEnter()
        {
            _quickDropText.Visible = false;
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

            // Death Link Object
            if (!_titleScreenOptions && !_optionsArray.Contains(_toggleDeathLinkObj))
            {
                _optionsArray.Insert(_optionsArray.Count - 1, _toggleDeathLinkObj);
            }

            if (_titleScreenOptions && _optionsArray.Contains(_toggleDeathLinkObj))
            {
                _optionsArray = _optionsArray.FindAll(options => options is not DeathLinkOptionsObj);
            }

            // Retire Object
            if (!_titleScreenOptions && !_optionsArray.Contains(_retireCharacterOptionObj))
            {
                _optionsArray.Insert(_optionsArray.Count - 1, _retireCharacterOptionObj);
            }

            if (_titleScreenOptions && _optionsArray.Contains(_retireCharacterOptionObj))
            {
                _optionsArray = _optionsArray.FindAll(options => options is not RetireOptionsObj);
            }

            // Return to menu.
            if (!_titleScreenOptions && !_optionsArray.Contains(_backToMenuObj))
            {
                _optionsArray.Insert(_optionsArray.Count - 1, _backToMenuObj);
            }

            _transitioning = true;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");
            _selectedOptionIndex = 0;
            _selectedOption = _optionsArray[_selectedOptionIndex];
            _selectedOption.IsActive = false;
            _bgSprite.Position = new Vector2(660f, 0f);
            _bgSprite.Opacity = 0f;
            Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
            Tween.AddEndHandlerToLastTween(this, "EndTransition");
            Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
            var num = 0;
            foreach (var current in _optionsArray)
            {
                current.Y = 160 + num * 30 - 360f;
                current.Opacity = 0f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", 360f.ToString());
                Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "1");
                current.Initialize();
                num++;
            }

            _optionsBar.Opacity = 0f;
            Tween.To(_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "1");
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
            Tween.To(_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "0");
            _bgSprite.Position = new Vector2(660f, 360f);
            _bgSprite.Opacity = 1f;
            Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
            Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");
            var num = 0;
            foreach (var current in _optionsArray)
            {
                current.Y = 160 + num * 30;
                current.Opacity = 1f;
                Tween.By(current, 0.5f, Quad.EaseOut, "Y", (-360f).ToString());
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
                }
                else
                {
                    if (!_selectedOption.IsActive)
                    {
                        var selectedOptionIndex = _selectedOptionIndex;
                        if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                        {
                            if (_selectedOptionIndex > 0)
                            {
                                SoundManager.PlaySound("frame_swap");
                            }

                            _selectedOptionIndex--;
                        }
                        else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
                        {
                            if (_selectedOptionIndex < _optionsArray.Count - 1)
                            {
                                SoundManager.PlaySound("frame_swap");
                            }

                            _selectedOptionIndex++;
                        }

                        if (_selectedOptionIndex < 0)
                        {
                            _selectedOptionIndex = _optionsArray.Count - 1;
                        }

                        if (_selectedOptionIndex > _optionsArray.Count - 1)
                        {
                            _selectedOptionIndex = 0;
                        }

                        if (selectedOptionIndex != _selectedOptionIndex)
                        {
                            if (_selectedOption != null)
                            {
                                _selectedOption.IsSelected = false;
                            }

                            _selectedOption = _optionsArray[_selectedOptionIndex];
                            _selectedOption.IsSelected = true;
                        }
                    }

                    if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                    {
                        SoundManager.PlaySound("Option_Menu_Select");
                        _selectedOption.IsActive = true;
                    }

                    if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) ||
                        Game.GlobalInput.JustPressed(4))
                    {
                        ExitTransition();
                    }
                }

                if (_selectedOption == _quickDropObj)
                {
                    _quickDropText.Visible = true;
                    _quickDropText.Text =
                        "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
                }
                else if (_selectedOption == _reduceQualityObj)
                {
                    _quickDropText.Visible = true;
                    _quickDropText.Text = "*The game must be restarted for this change to come into effect.";
                }
                else if (_selectedOption == _enableSteamCloudObj)
                {
                    _quickDropText.Visible = true;
                    _quickDropText.Text =
                        "*Cloud support must be enabled on the Steam platform as well for\nthis feature to work.";
                }
                else if (_selectedOption is ChatOption or ChatOpacityOption)
                {
                    _quickDropText.Visible = true;
                    _quickDropText.Text = "*Chat is only applicable for Archipelago rando games.";
                }
                else
                {
                    _quickDropText.Visible = false;
                }
            }
            else
            {
                _quickDropText.Visible = false;
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in _optionsArray)
            {
                current.Update(gameTime);
            }

            _optionsBar.Position = new Vector2(_selectedOption.X - 15f, _selectedOption.Y);
            base.Update(gameTime);
        }

        public void ToggleControlsConfig()
        {
            if (!_changingControls)
            {
                foreach (var current in _optionsArray)
                {
                    Tween.By(current, 0.3f, Quad.EaseInOut, "X", "-1320");
                }

                Tween.By(_optionsTitle, 0.3f, Quad.EaseInOut, "X", "-1320");
                Tween.By(_changeControlsTitle, 0.3f, Quad.EaseInOut, "X", "-1320");
                _changingControls = true;
                return;
            }

            foreach (var current2 in _optionsArray)
            {
                Tween.By(current2, 0.3f, Quad.EaseInOut, "X", "1320");
            }

            Tween.By(_optionsTitle, 0.3f, Quad.EaseInOut, "X", "1320");
            Tween.By(_changeControlsTitle, 0.3f, Quad.EaseInOut, "X", "1320");
            _changingControls = false;
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin();
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _bgSprite.Draw(Camera);
            foreach (var current in _optionsArray)
            {
                current.Draw(Camera);
            }

            _quickDropText.Draw(Camera);
            _confirmText.Draw(Camera);
            _cancelText.Draw(Camera);
            _navigationText.Draw(Camera);
            _optionsBar.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Options Screen");
            foreach (var current in _optionsArray)
            {
                current.Dispose();
            }

            _optionsArray.Clear();
            _optionsArray = null;
            _bgSprite.Dispose();
            _bgSprite = null;
            _optionsTitle = null;
            _changeControlsTitle = null;
            _backToMenuObj = null;
            _confirmText.Dispose();
            _confirmText = null;
            _cancelText.Dispose();
            _cancelText = null;
            _navigationText.Dispose();
            _navigationText = null;
            _optionsBar.Dispose();
            _optionsBar = null;
            _selectedOption = null;
            _quickDropText.Dispose();
            _quickDropText = null;
            _quickDropObj = null;
            _enableSteamCloudObj = null;
            _reduceQualityObj = null;
            base.Dispose();
        }
    }
}