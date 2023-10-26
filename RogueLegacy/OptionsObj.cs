//  RogueLegacyRandomizer - OptionsObj.cs
//  Last Modified 2023-10-25 8:36 PM
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Screens;

namespace RogueLegacy
{
    public abstract class OptionsObj : ObjContainer
    {
        protected bool m_isActive;
        protected bool m_isSelected;
        protected TextObj _nameText;
        protected int m_optionsTextOffset = 250;
        protected OptionsScreen _parentScreen;

        public OptionsObj(OptionsScreen parentScreen, string name)
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
            get { return m_isActive; }
            set
            {
                if (value)
                {
                    IsSelected = false;
                }
                else
                {
                    IsSelected = true;
                }

                m_isActive = value;
                if (!value)
                {
                    (_parentScreen.ScreenManager.Game as Game).SaveConfig();
                }
            }
        }

        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                m_isSelected = value;
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
            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                SoundManager.PlaySound("Options_Menu_Deselect");
            }
        }

        public virtual void Update(GameTime gameTime) { }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _parentScreen = null;
                _nameText = null;
                base.Dispose();
            }
        }
    }
}
