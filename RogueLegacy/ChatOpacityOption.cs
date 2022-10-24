// Rogue Legacy Randomizer - ChatOpacityOption.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy
{
    public class ChatOpacityOption : OptionsObj
    {
        private SpriteObj _opacityBar;
        private SpriteObj _opacityBarBG;

        public ChatOpacityOption(OptionsScreen parentScreen) : base(parentScreen, "Chat Transparency")
        {
            _opacityBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite");
            _opacityBarBG.X = m_optionsTextOffset;
            _opacityBarBG.Y = _opacityBarBG.Height / 2f - 2f;
            AddChild(_opacityBarBG);
            _opacityBar = new SpriteObj("OptionsScreenVolumeBar_Sprite");
            _opacityBar.X = _opacityBarBG.X + 6f;
            _opacityBar.Y = _opacityBarBG.Y + 5f;
            AddChild(_opacityBar);
        }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (value)
                {
                    _opacityBar.TextureColor = Color.Yellow;
                    return;
                }

                _opacityBar.TextureColor = Color.White;
            }
        }

        public override void Initialize()
        {
            _opacityBar.ScaleX = Game.GameConfig.ChatOpacity;
            base.Initialize();
        }

        public override void HandleInput()
        {
            if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
            {
                Game.GameConfig.ChatOpacity -= 0.01f;
                SetOpacityLevel();
            }
            else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
            {
                Game.GameConfig.ChatOpacity += 0.01f;
                SetOpacityLevel();
            }

            if (InputTypeHelper.PressedConfirm || InputTypeHelper.PressedCancel)
            {
                IsActive = false;
            }

            base.HandleInput();
        }

        public void SetOpacityLevel()
        {
            if (Game.GameConfig.ChatOpacity > 1)
            {
                Game.GameConfig.ChatOpacity = 1;
            }
            else if (Game.GameConfig.ChatOpacity < 0)
            {
                Game.GameConfig.ChatOpacity = 0;
            }

            _opacityBar.ScaleX = Game.GameConfig.ChatOpacity;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _opacityBar = null;
                _opacityBarBG = null;
                base.Dispose();
            }
        }
    }
}
