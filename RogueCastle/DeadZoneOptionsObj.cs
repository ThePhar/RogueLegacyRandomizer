using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class DeadZoneOptionsObj : OptionsObj
	{
		private SpriteObj m_deadZoneBarBG;
		private SpriteObj m_deadZoneBar;
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
					this.m_deadZoneBar.TextureColor = Color.Yellow;
					return;
				}
				this.m_deadZoneBar.TextureColor = Color.White;
			}
		}
		public DeadZoneOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Joystick Dead Zone")
		{
			this.m_deadZoneBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite");
			this.m_deadZoneBarBG.X = (float)this.m_optionsTextOffset;
			this.m_deadZoneBarBG.Y = (float)this.m_deadZoneBarBG.Height / 2f - 2f;
			this.AddChild(this.m_deadZoneBarBG);
			this.m_deadZoneBar = new SpriteObj("OptionsScreenVolumeBar_Sprite");
			this.m_deadZoneBar.X = this.m_deadZoneBarBG.X + 6f;
			this.m_deadZoneBar.Y = this.m_deadZoneBarBG.Y + 5f;
			this.AddChild(this.m_deadZoneBar);
		}
		public override void Initialize()
		{
			this.m_deadZoneBar.ScaleX = InputManager.Deadzone / 95f;
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
			{
				if (InputManager.Deadzone - 1f >= 0f)
				{
					InputManager.Deadzone -= 1f;
					this.UpdateDeadZoneBar();
				}
			}
			else if ((Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) && InputManager.Deadzone + 1f <= 95f)
			{
				InputManager.Deadzone += 1f;
				this.UpdateDeadZoneBar();
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				this.IsActive = false;
			}
			base.HandleInput();
		}
		public void UpdateDeadZoneBar()
		{
			this.m_deadZoneBar.ScaleX = InputManager.Deadzone / 95f;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_deadZoneBar = null;
				this.m_deadZoneBarBG = null;
				base.Dispose();
			}
		}
	}
}
