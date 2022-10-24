// Rogue Legacy Randomizer - ChatOption.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy
{
    public class ChatOption : OptionsObj
    {
        private TextObj        _toggleText;
        private ChatOptionType _chatType;

        public ChatOption(OptionsScreen parentScreen) : base(parentScreen, "Chat Filtering")
        {
            _toggleText = m_nameText.Clone() as TextObj;
            _toggleText.X = m_optionsTextOffset;
            _toggleText.Text = ChatTypeString;
            AddChild(_toggleText);
        }

        public override bool IsActive
        {
            get => base.IsActive;
            set
            {
                base.IsActive = value;
                if (value)
                {
                    _toggleText.TextureColor = Color.Yellow;
                    return;
                }

                _toggleText.TextureColor = Color.White;
            }
        }

        public string ChatTypeString => _chatType switch
        {
            ChatOptionType.Chat              => "Chat Only",
            ChatOptionType.ChatHints         => "Hints",
            ChatOptionType.ChatHintsOwnItems => "Hints & Own Items",
            ChatOptionType.ChatHintsItems    => "All Events",
            _                                => throw new ArgumentOutOfRangeException()
        };

        public override void Initialize()
        {
            _chatType = (ChatOptionType) Game.GameConfig.ChatOption;
            _toggleText.Text = ChatTypeString;

            base.Initialize();
        }

        public override void HandleInput()
        {
            if (InputTypeHelper.PressedLeft)
            {
                var option = (int) _chatType - 1;
                if (option < 0)
                {
                    _chatType = ChatOptionType.ChatHintsItems;
                }
                else
                {
                    _chatType = (ChatOptionType) option;
                }

                _toggleText.Text = ChatTypeString;
            }
            else if (InputTypeHelper.PressedRight)
            {
                var option = (int) _chatType + 1;
                if (option > 3)
                {
                    _chatType = ChatOptionType.Chat;
                }
                else
                {
                    _chatType = (ChatOptionType) option;
                }

                _toggleText.Text = ChatTypeString;
            }
            else if (InputTypeHelper.PressedConfirm)
            {
                IsActive = false;
                Game.GameConfig.ChatOption = (int) _chatType;
            }
            else if (InputTypeHelper.PressedCancel)
            {
                IsActive = false;
                _chatType = (ChatOptionType) Game.GameConfig.ChatOption;
                _toggleText.Text = ChatTypeString;
            }

            base.HandleInput();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _toggleText = null;
                base.Dispose();
            }
        }
    }

    public enum ChatOptionType
    {
        Chat,
        ChatHints,
        ChatHintsOwnItems,
        ChatHintsItems,
    }
}
