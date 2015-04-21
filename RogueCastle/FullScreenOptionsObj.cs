using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class FullScreenOptionsObj : OptionsObj
	{
		private TextObj m_toggleText;
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				base.IsActive = value;
				if (value)
				{
					this.m_toggleText.TextureColor = Color.Yellow;
					return;
				}
				this.m_toggleText.TextureColor = Color.White;
			}
		}
		public FullScreenOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Fullscreen")
		{
			this.m_toggleText = (this.m_nameText.Clone() as TextObj);
			this.m_toggleText.X = (float)this.m_optionsTextOffset;
			this.m_toggleText.Text = "No";
			this.AddChild(this.m_toggleText);
		}
		public override void Initialize()
		{
			if ((this.m_parentScreen.ScreenManager.Game as Game).graphics.IsFullScreen)
			{
				this.m_toggleText.Text = "Yes";
			}
			else
			{
				this.m_toggleText.Text = "No";
			}
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21) || Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
			{
				SoundManager.PlaySound("frame_swap");
				if (this.m_toggleText.Text == "No")
				{
					this.m_toggleText.Text = "Yes";
				}
				else
				{
					this.m_toggleText.Text = "No";
				}
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				SoundManager.PlaySound("Option_Menu_Select");
				GraphicsDeviceManager graphics = (this.m_parentScreen.ScreenManager.Game as Game).graphics;
				if ((this.m_toggleText.Text == "No" && graphics.IsFullScreen) || (this.m_toggleText.Text == "Yes" && !graphics.IsFullScreen))
				{
					this.ToggleFullscreen(graphics);
					this.IsActive = false;
				}
			}
			if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				if ((this.m_parentScreen.ScreenManager.Game as Game).graphics.IsFullScreen)
				{
					this.m_toggleText.Text = "Yes";
				}
				else
				{
					this.m_toggleText.Text = "No";
				}
				this.IsActive = false;
			}
			base.HandleInput();
		}
		public void ToggleFullscreen(GraphicsDeviceManager graphics)
		{
			graphics.ToggleFullScreen();
			Game.GameConfig.FullScreen = graphics.IsFullScreen;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_toggleText = null;
				base.Dispose();
			}
		}
	}
}
