//
//  Rogue Legacy Randomizer - TextBoxOptionsObj.cs
//  Last Modified 2022-01-03
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Linq;
using System.Windows.Forms;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RogueCastle.Screens;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RogueCastle.Options
{
    public class TextBoxOptionsObj : ArchipelagoOptionsObj
    {
        private readonly string m_placeholder;
        private string m_currentValue = "";
        private KeyboardState m_keyboardState;
        private TextObj m_toggleText;

        public TextBoxOptionsObj(ArchipelagoScreen parentScreen, string name, string placeholder) :
            base(parentScreen, name)
        {
            m_placeholder = placeholder;

            // Toggle Text Highlighting.
            m_toggleText = m_nameText.Clone() as TextObj;
            m_toggleText.X = m_optionsTextOffset;
            m_toggleText.Text = m_placeholder;
            m_toggleText.TextureColor = Color.Gray;

            base.AddChild(m_toggleText);
        }

        public string GetValue
        {
            get
            {
                return m_currentValue != string.Empty
                    ? m_currentValue
                    : m_placeholder;
            }
        }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (value)
                {
                    m_toggleText.TextureColor = Color.Yellow;
                    return;
                }

                m_toggleText.TextureColor = m_currentValue.Length == 0 ? Color.Gray : Color.White;
            }
        }

        public override void Initialize()
        {
            m_currentValue = string.Empty;
            m_keyboardState = Keyboard.GetState();
            base.Initialize();
        }

        public override void HandleInput()
        {
            var currentKeyboardState = Keyboard.GetState();
            var pressedKeys = currentKeyboardState.GetPressedKeys();
            var holdingControl = currentKeyboardState.IsKeyDown(Keys.LeftControl) ||
                                 currentKeyboardState.IsKeyDown(Keys.RightControl);

            foreach (var key in pressedKeys)
            {
                if (m_keyboardState.IsKeyUp(key))
                {
                    SoundManager.PlaySound("Option_Menu_Select");

                    switch (key)
                    {
                        // Backspace key, obviously.
                        case Keys.Back:
                            if (m_currentValue.Length != 0)
                            {
                                m_currentValue = m_currentValue.Remove(m_currentValue.Length - 1, 1);
                            }

                            break;

                        // Add a space.
                        case Keys.Space:
                            m_currentValue += " ";
                            break;

                        // Exit option.
                        case Keys.Enter:
                            IsActive = false;
                            m_parentScreen.LockControls = false;
                            break;

                        // We handle V differently so I can paste. ;)
                        case Keys.V:
                            if (holdingControl)
                            {
                                if (Clipboard.ContainsText())
                                {
                                    m_currentValue = Clipboard.GetText(TextDataFormat.Text)
                                        .Substring(0, Math.Min(128, Clipboard.GetText().Length)).Trim();
                                }

                                break;
                            }

                            goto normalKey;

                        case Keys.C:
                            if (holdingControl)
                            {
                                try
                                {
                                    Clipboard.SetText(m_currentValue);
                                    m_currentValue = "";
                                }
                                catch
                                {
                                    Clipboard.Clear();
                                }

                                break;
                            }

                            goto normalKey;

                        case Keys.X:
                            if (holdingControl)
                            {
                                try
                                {
                                    Clipboard.SetText(m_currentValue);
                                    m_currentValue = "";
                                }
                                catch
                                {
                                    Clipboard.Clear();
                                }

                                break;
                            }

                            goto normalKey;

                        case Keys.A:
                        case Keys.B:
                        case Keys.D:
                        case Keys.E:
                        case Keys.F:
                        case Keys.G:
                        case Keys.H:
                        case Keys.I:
                        case Keys.J:
                        case Keys.K:
                        case Keys.L:
                        case Keys.M:
                        case Keys.N:
                        case Keys.O:
                        case Keys.P:
                        case Keys.Q:
                        case Keys.R:
                        case Keys.S:
                        case Keys.T:
                        case Keys.U:
                        case Keys.W:
                        case Keys.Y:
                        case Keys.Z:
                            normalKey:
                            m_currentValue += HandleShift(pressedKeys, key.ToString(), key.ToString().ToLower());
                            break;

                        case Keys.Decimal:
                        case Keys.OemPeriod:
                            m_currentValue += HandleShift(pressedKeys, ">", ".");
                            break;

                        case Keys.OemSemicolon:
                            m_currentValue += HandleShift(pressedKeys, ":", ";");
                            break;

                        case Keys.OemComma:
                            m_currentValue += HandleShift(pressedKeys, "<", ",");
                            break;

                        case Keys.OemQuotes:
                            m_currentValue += HandleShift(pressedKeys, "\"", "'");
                            break;

                        case Keys.OemOpenBrackets:
                            m_currentValue += HandleShift(pressedKeys, "{", "[");
                            break;

                        case Keys.OemCloseBrackets:
                            m_currentValue += HandleShift(pressedKeys, "}", "]");
                            break;

                        case Keys.OemBackslash:
                            m_currentValue += HandleShift(pressedKeys, "|", "\\");
                            break;

                        case Keys.OemQuestion:
                            m_currentValue += HandleShift(pressedKeys, "?", "/");
                            break;

                        case Keys.Subtract:
                        case Keys.OemMinus:
                            m_currentValue += HandleShift(pressedKeys, "_", "-");
                            break;

                        case Keys.Add:
                        case Keys.OemPlus:
                            m_currentValue += HandleShift(pressedKeys, "+", "=");
                            break;

                        case Keys.OemTilde:
                            m_currentValue += HandleShift(pressedKeys, "~", "`");
                            break;

                        case Keys.NumPad0:
                        case Keys.D0:
                            m_currentValue += HandleShift(pressedKeys, ")", "0");
                            break;

                        case Keys.NumPad1:
                        case Keys.D1:
                            m_currentValue += HandleShift(pressedKeys, "!", "1");
                            break;

                        case Keys.NumPad2:
                        case Keys.D2:
                            m_currentValue += HandleShift(pressedKeys, "@", "2");
                            break;

                        case Keys.NumPad3:
                        case Keys.D3:
                            m_currentValue += HandleShift(pressedKeys, "#", "3");
                            break;

                        case Keys.NumPad4:
                        case Keys.D4:
                            m_currentValue += HandleShift(pressedKeys, "$", "4");
                            break;

                        case Keys.NumPad5:
                        case Keys.D5:
                            m_currentValue += HandleShift(pressedKeys, "%", "5");
                            break;

                        case Keys.NumPad6:
                        case Keys.D6:
                            m_currentValue += HandleShift(pressedKeys, "^", "6");
                            break;

                        case Keys.NumPad7:
                        case Keys.D7:
                            m_currentValue += HandleShift(pressedKeys, "&", "7");
                            break;

                        case Keys.NumPad8:
                        case Keys.D8:
                            m_currentValue += HandleShift(pressedKeys, "*", "8");
                            break;

                        case Keys.NumPad9:
                        case Keys.D9:
                            m_currentValue += HandleShift(pressedKeys, "(", "9");
                            break;
                    }
                }
            }

            // Update keyboard state.
            m_keyboardState = currentKeyboardState;

            // Update colors.
            if (IsActive)
            {
                m_toggleText.TextureColor = Color.Yellow;
                m_toggleText.Text = m_currentValue + "_";
            }
            else
            {
                // Remove any trailing whitespace.
                m_currentValue = m_currentValue.Trim();

                if (m_currentValue.Length == 0)
                {
                    m_toggleText.Text = m_placeholder;
                    m_toggleText.TextureColor = Color.Gray;
                }
                else
                {
                    m_toggleText.Text = m_currentValue;
                    m_toggleText.TextureColor = Color.White;
                }
            }

            base.HandleInput();
        }

        private string HandleShift(Keys[] keys, string uppercase, string lowercase)
        {
            if (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift))
            {
                return uppercase;
            }

            return lowercase;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_toggleText = null;
                base.Dispose();
            }
        }
    }
}
