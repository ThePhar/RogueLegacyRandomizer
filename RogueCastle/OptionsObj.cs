using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
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
			get
			{
				return this.m_isActive;
			}
			set
			{
				if (value)
				{
					this.IsSelected = false;
				}
				else
				{
					this.IsSelected = true;
				}
				this.m_isActive = value;
				if (!value)
				{
					(this.m_parentScreen.ScreenManager.Game as Game).SaveConfig();
				}
			}
		}
		public bool IsSelected
		{
			get
			{
				return this.m_isSelected;
			}
			set
			{
				this.m_isSelected = value;
				if (value)
				{
					this.m_nameText.TextureColor = Color.Yellow;
					return;
				}
				this.m_nameText.TextureColor = Color.White;
			}
		}
		public OptionsObj(OptionsScreen parentScreen, string name)
		{
			this.m_parentScreen = parentScreen;
			this.m_nameText = new TextObj(Game.JunicodeFont);
			this.m_nameText.FontSize = 12f;
			this.m_nameText.Text = name;
			this.m_nameText.DropShadow = new Vector2(2f, 2f);
			this.AddChild(this.m_nameText);
			base.ForceDraw = true;
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
			if (!base.IsDisposed)
			{
				this.m_parentScreen = null;
				this.m_nameText = null;
				base.Dispose();
			}
		}
	}
}
