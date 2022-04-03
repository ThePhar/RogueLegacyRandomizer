// 
//  Rogue Legacy Randomizer - RandoModeOption.cs
//  Last Modified 2022-04-03
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using RogueCastle.Screens;

namespace RogueCastle.Options
{
    public class RandoModeOption : RandomizerOption
    {
        private TextObj _toggleText;

        public RandoModeOption(RandomizerScreen parentScreen) : base(parentScreen, "Rando Mode")
        {
            _toggleText = _nameText.Clone() as TextObj;
            _toggleText.X = OPTIONS_TEXT_OFFSET;
            _toggleText.Text = "No";
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

        public override void Initialize()
        {
            _toggleText.Text = Game.RandomizerOptions.IsArchipelago ? "Archipelago" : "Solo";
            base.Initialize();
        }

        public override void HandleInput()
        {
            if (InputTypeHelper.PressedLeft || InputTypeHelper.PressedRight)
            {
                SoundManager.PlaySound("frame_swap");
                _toggleText.Text = _toggleText.Text == "Solo" ? "Archipelago" : "Solo";
            }

            if (InputTypeHelper.PressedConfirm)
            {
                SoundManager.PlaySound("Option_Menu_Select");
                Game.RandomizerOptions.IsArchipelago = _toggleText.Text == "Archipelago";
                IsActive = false;
            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                _toggleText.Text = Game.RandomizerOptions.IsArchipelago ? "Archipelago" : "Solo";
                IsActive = false;
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
