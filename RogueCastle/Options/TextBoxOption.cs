// 
//  Rogue Legacy Randomizer - TextBoxOption.cs
//  Last Modified 2022-04-03
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Windows.Forms;
using DS2DEngine;
using Gma.System.MouseKeyHook;
using Microsoft.Xna.Framework;
using RogueCastle.Screens;

namespace RogueCastle.Options
{
    public class TextBoxOption : RandomizerOption
    {
        private static IKeyboardMouseEvents _inputHook = Hook.AppEvents();

        private string  _placeholder;
        private string  _currentValue = string.Empty;
        private TextObj _toggleText;
        private bool    _ready;
        public  int     _cursorIndex;

        public TextBoxOption(RandomizerScreen parentScreen, string name, string placeholder) : base(parentScreen, name)
        {
            _placeholder = placeholder;

            // Toggle Text Highlighting.
            _toggleText = _nameText.Clone() as TextObj;
            _toggleText.X = OPTIONS_TEXT_OFFSET;
            _toggleText.Text = _placeholder;
            _toggleText.TextureColor = Color.Gray;

            base.AddChild(_toggleText);
        }

        public string GetValue =>
            _currentValue != string.Empty
                ? _currentValue
                : _placeholder;

        public override bool IsActive
        {
            get => base.IsActive;
            set
            {
                base.IsActive = value;
                if (value)
                {
                    _toggleText.TextureColor = Color.Yellow;
                    _inputHook.KeyPress += HandleInput;
                    _inputHook.KeyDown += CheckCursor;
                    _cursorIndex = _currentValue.Length;
                    return;
                }

                _inputHook.KeyPress -= HandleInput;
                _inputHook.KeyDown -= CheckCursor;
                _ready = false;
                _toggleText.TextureColor = _currentValue.Length == 0 ? Color.Gray : Color.White;
            }
        }

        public override void Initialize()
        {
            _currentValue = string.Empty;
            base.Initialize();
        }

        public void CheckCursor(object sender, KeyEventArgs e)
        {
            if (!IsActive) return;

            // Move cursor to the left and right.
            if (e.KeyCode == Keys.Left)
                _cursorIndex = _cursorIndex - 1 < 0 ? 0 : _cursorIndex - 1;
            if (e.KeyCode == Keys.Right)
                _cursorIndex = _cursorIndex + 1 > _currentValue.Length ? _currentValue.Length : _cursorIndex + 1;

            // Check for delete key stuff.
            if (e.KeyCode == Keys.Delete)
            {
                if (_cursorIndex != _currentValue.Length)
                {
                    _currentValue = _currentValue.Remove(_cursorIndex, 1);
                }
            }

            // Check for pasting.
            if (e.KeyCode == Keys.V && e.Control)
            {
                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    _currentValue = Clipboard.GetText(TextDataFormat.Text)
                        .Substring(0, Math.Min(128, Clipboard.GetText().Length))
                        .Trim();
                    _cursorIndex = _currentValue.Length;
                }
            }

            _toggleText.Text = _currentValue.Insert(_cursorIndex, "_");
        }

        public void HandleInput(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine($"KEY: '{e.KeyChar}' [\\u{(int) e.KeyChar:x4}]");

            if (!_ready)
            {
                _ready = true;
                _toggleText.Text = _currentValue.Insert(_cursorIndex, "_");
                return;
            }

            switch (e.KeyChar)
            {
                case '\u0008': // BS
                    if (_cursorIndex != 0)
                    {
                        _currentValue = _currentValue.Remove(_cursorIndex - 1, 1);
                        _cursorIndex -= 1;
                    }

                    break;

                case '\u000a': // LF
                case '\u000d': // CR
                case '\u001b': // ESC
                    IsActive = false;
                    break;

                // Ignore command and Non-ASCII characters.
                case >= '\u0000' and <= '\u001f' or > '\u007f':
                    break;

                default:
                    _currentValue = _currentValue.Insert(_cursorIndex, e.KeyChar.ToString());
                    _cursorIndex += 1;
                    break;
            }

            // Update colors.
            if (IsActive)
            {
                _toggleText.TextureColor = Color.Yellow;
                _toggleText.Text = _currentValue.Insert(_cursorIndex, "_");;
            }
            else
            {
                // Remove any trailing whitespace.
                _currentValue = _currentValue.Trim();

                if (_currentValue.Length == 0)
                {
                    _toggleText.Text = _placeholder;
                    _toggleText.TextureColor = Color.Gray;
                }
                else
                {
                    _toggleText.Text = _currentValue;
                    _toggleText.TextureColor = Color.White;
                }
            }

            base.HandleInput();
        }

        public override void Dispose()
        {
            if (IsDisposed) return;

            _toggleText = null;
            base.Dispose();
        }
    }
}
