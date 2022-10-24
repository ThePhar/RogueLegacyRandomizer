// Rogue Legacy Randomizer - RandomizerOption.cs
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

namespace RogueLegacy.Options
{
    public abstract class RandomizerOption : ObjContainer
    {
        protected const int OPTIONS_TEXT_OFFSET = 250;

        protected bool             _isActive;
        protected bool             _isSelected;
        protected TextObj          _nameText;
        protected RandomizerScreen _parentScreen;

        public RandomizerOption(RandomizerScreen parentScreen, string name)
        {
            _parentScreen = parentScreen;
            _nameText = new TextObj(Game.JunicodeFont);
            _nameText.FontSize = 12f;
            _nameText.Text = name;
            _nameText.DropShadow = new Vector2(2f, 2f);
            AddChild(_nameText);
            ForceDraw = true;
        }

        public virtual bool IsActive
        {
            get => _isActive;
            set
            {
                IsSelected = !value;

                _isActive = value;
                if (!value)
                {
                    Program.Game.SaveConfig();
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (value)
                {
                    _nameText.TextureColor = Color.Yellow;
                    return;
                }

                _nameText.TextureColor = Color.White;
            }
        }

        public virtual void Initialize() { }

        public virtual void HandleInput()
        {
            if (InputTypeHelper.PressedCancel)
            {
                SoundManager.PlaySound("Options_Menu_Deselect");
            }
        }

        public virtual void Update(GameTime gameTime) { }

        public override void Dispose()
        {
            if (IsDisposed) return;

            _parentScreen = null;
            _nameText = null;
            base.Dispose();
        }
    }
}
