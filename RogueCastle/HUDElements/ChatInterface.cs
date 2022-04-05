// 
//  Rogue Legacy Randomizer - ChatInterface.cs
//  Last Modified 2022-04-04
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Archipelago;
using DS2DEngine;
using Gma.System.MouseKeyHook;
using Microsoft.Xna.Framework;
using RogueCastle.Screens;
using Tweener;

namespace RogueCastle.HUDElements
{
    public class ChatInterface : ObjContainer
    {
        private static IKeyboardMouseEvents _inputHook = Hook.AppEvents();

        public const int MAXIMUM_WIDTH    = 512;
        public const int MAXIMUM_HEIGHT   = 800;
        public const int MAXIMUM_ELEMENTS = 12;
        public const int TIMEOUT_SECONDS  = 30;

        private ChatElement _inputText;
        private string      _inputEntry  = string.Empty;
        private int         _inputCursor = 0;

        private bool _isInputActive = false;
        private bool IsInputActive
        {
            get => _isInputActive;
            set
            {
                var player = Game.ScreenManager.Player;

                if (value)
                {
                    _isInputActive = true;
                    _inputText.Visible = true;
                    _inputHook.KeyDown += CheckCursor;

                    player.LockControls();
                }
                else
                {
                    _isInputActive = false;
                    _inputText.Visible = false;
                    _inputEntry = string.Empty;
                    _inputCursor = 0;
                    _inputHook.KeyDown -= CheckCursor;

                    player.UnlockControls();
                }
            }
        }

        public  List<ChatElement> Elements { get; private set; } = new();

        public ChatInterface()
        {
            _inputText = new ChatElement(this);
            _inputHook.KeyPress += HandleInput;
        }

        public void CheckCursor(object sender, KeyEventArgs e)
        {
            if (!IsInputActive) return;

            // Move cursor to the left and right.
            if (e.KeyCode == Keys.Left)
                _inputCursor = _inputCursor - 1 < 0 ? 0 : _inputCursor - 1;
            if (e.KeyCode == Keys.Right)
                _inputCursor = _inputCursor + 1 > _inputEntry.Length ? _inputEntry.Length : _inputCursor + 1;

            // Check for delete key stuff.
            if (e.KeyCode == Keys.Delete)
            {
                if (_inputCursor != _inputEntry.Length)
                {
                    _inputEntry = _inputEntry.Remove(_inputCursor, 1);
                }
            }

            // Check for pasting.
            if (e.KeyCode == Keys.V && e.Control)
            {
                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    _inputEntry = Clipboard.GetText(TextDataFormat.Text)
                        .Substring(0, Math.Min(1024, Clipboard.GetText().Length))
                        .Trim();
                    _inputCursor = _inputEntry.Length;
                }
            }
        }

        public void HandleInput(object sender, KeyPressEventArgs e)
        {
            // Ignore input if player is not on screen or we aren't in a level.
            if (Game.ScreenManager.Player == null || Game.ScreenManager.CurrentScreen is not ProceduralLevelScreen)
            {
                return;
            }

            if (!IsInputActive)
            {
                if (e.KeyChar is '\u000d' or '\u000a')
                {
                    IsInputActive = true;
                    return;
                }
            }

            switch (e.KeyChar)
            {
                case '\u0008': // BS
                    if (_inputCursor != 0)
                    {
                        _inputEntry = _inputEntry.Remove(_inputCursor - 1, 1);
                        _inputCursor -= 1;
                    }

                    break;

                case '\u001b': // ESC
                    IsInputActive = false;
                    break;

                case '\u000a': // LF
                case '\u000d': // CR
                    var input = _inputEntry.Trim();
                    if (input != "")
                        Program.Game.ArchipelagoManager.Chat(_inputEntry.Trim());

                    IsInputActive = false;
                    break;

                // Ignore command characters.
                case >= '\u0000' and <= '\u001f':
                    break;

                default:
                    _inputEntry = _inputEntry.Insert(_inputCursor, e.KeyChar.ToString());
                    _inputCursor += 1;
                    break;
            }
        }

        public void Update()
        {
            // Check if queue has new elements.
            while (Program.Game.ArchipelagoManager.IncomingChatQueue.Count > 0)
            {
                var chat = Program.Game.ArchipelagoManager.IncomingChatQueue.Dequeue();
                var element = new ChatElement(this, chat.Item1);

                // Change color based on type of message.
                switch (chat.Item2)
                {
                    case ChatType.Item:
                        element.TextureColor = Color.Cyan;
                        break;

                    case ChatType.Hint:
                        element.TextureColor = Color.Yellow;
                        break;
                }

                Elements.Add(element);
            }

            // Clear any older chats if we have more than our maximum.
            while (Elements.Count > MAXIMUM_ELEMENTS)
            {
                Elements.RemoveAt(0);
            }

            float y = Y;

            // Draw text input field.
            if (IsInputActive)
            {
                _inputText.Y = y;
                _inputText.X = X;
                _inputText.UpdateText("> " + _inputEntry.Insert(_inputCursor, "_"));
                y += _inputText.Height + 16; // OFFSET!
            }

            foreach (var element in Elements)
            {
                // Check if text element is too old, then remove it.
                if (element.CreatedTime.AddSeconds(TIMEOUT_SECONDS) < DateTime.Now && !element.FadingOut)
                {
                    element.FadeOut();
                }

                // Set the positions of each element.
                element.Y = y;
                y += element.Height;
            }
        }

        public override void Draw(Camera2D camera)
        {
            // Draw text fields.
            foreach (var element in Elements)
            {
                element.Draw(camera);
            }

            // Draw input field.
            _inputText.Draw(camera);
        }

        public override void Dispose()
        {
            if (IsDisposed) return;

            _inputHook.KeyPress -= HandleInput;
            _inputHook.KeyDown -= CheckCursor;

            _inputText.Dispose();
            _inputText = null;
            Elements.Clear();
            Elements = null;
            base.Dispose();
        }

        public sealed class ChatElement : TextObj
        {
            private readonly ChatInterface _chatInterface;

            public DateTime CreatedTime { get; private set; } = DateTime.Now;
            public bool     FadingOut   { get; private set; } = false;

            public ChatElement(ChatInterface @interface, string text = "") : base(Game.BitFont)
            {
                ForceDraw = true;
                FontSize = 8f;
                OutlineColour = Color.Black;
                OutlineWidth = 1;
                Text = text;
                WordWrap(MAXIMUM_WIDTH);

                _chatInterface = @interface;
                X = @interface.X;
                Y = @interface.Y;
            }

            public ChatElement UpdateText(string newText)
            {
                Text = newText;
                WordWrap(MAXIMUM_WIDTH);
                return this;
            }

            public void FadeOut()
            {
                Tween.To(this, 1f, Tween.EaseNone, "Opacity", "0");
                Tween.RunFunction(1f, this, "Dispose");
                FadingOut = true;
            }

            public override void Dispose()
            {
                if (IsDisposed) return;

                _chatInterface.Elements.Remove(this);
                base.Dispose();
            }
        }
    }
}
