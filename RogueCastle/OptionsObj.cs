/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public abstract class OptionsObj : ObjContainer
    {
        protected bool m_isSelected;
        protected bool m_isActive;
        protected TextObj m_nameText;
        protected OptionsScreen m_parentScreen;
        protected int m_optionsTextOffset = 250;

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
                    (m_parentScreen.ScreenManager.Game as Game).SaveConfig();
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
                    m_nameText.TextureColor = Color.Yellow;
                    return;
                }
                m_nameText.TextureColor = Color.White;
            }
        }

        public OptionsObj(OptionsScreen parentScreen, string name)
        {
            m_parentScreen = parentScreen;
            m_nameText = new TextObj(Game.JunicodeFont);
            m_nameText.FontSize = 12f;
            m_nameText.Text = name;
            m_nameText.DropShadow = new Vector2(2f, 2f);
            AddChild(m_nameText);
            ForceDraw = true;
        }

        public virtual void Initialize()
        {
        }

        public virtual void HandleInput()
        {
            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                SoundManager.PlaySound("Options_Menu_Deselect");
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_parentScreen = null;
                m_nameText = null;
                base.Dispose();
            }
        }
    }
}