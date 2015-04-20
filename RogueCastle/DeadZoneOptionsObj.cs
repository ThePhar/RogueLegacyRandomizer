/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;

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
					m_deadZoneBar.TextureColor = Color.Yellow;
					return;
				}
				m_deadZoneBar.TextureColor = Color.White;
			}
		}
		public DeadZoneOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Joystick Dead Zone")
		{
			m_deadZoneBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite");
			m_deadZoneBarBG.X = m_optionsTextOffset;
			m_deadZoneBarBG.Y = m_deadZoneBarBG.Height / 2f - 2f;
			AddChild(m_deadZoneBarBG);
			m_deadZoneBar = new SpriteObj("OptionsScreenVolumeBar_Sprite");
			m_deadZoneBar.X = m_deadZoneBarBG.X + 6f;
			m_deadZoneBar.Y = m_deadZoneBarBG.Y + 5f;
			AddChild(m_deadZoneBar);
		}
		public override void Initialize()
		{
			m_deadZoneBar.ScaleX = InputManager.Deadzone / 95f;
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
			{
				if (InputManager.Deadzone - 1f >= 0f)
				{
					InputManager.Deadzone -= 1f;
					UpdateDeadZoneBar();
				}
			}
			else if ((Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) && InputManager.Deadzone + 1f <= 95f)
			{
				InputManager.Deadzone += 1f;
				UpdateDeadZoneBar();
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				IsActive = false;
			}
			base.HandleInput();
		}
		public void UpdateDeadZoneBar()
		{
			m_deadZoneBar.ScaleX = InputManager.Deadzone / 95f;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_deadZoneBar = null;
				m_deadZoneBarBG = null;
				base.Dispose();
			}
		}
	}
}
