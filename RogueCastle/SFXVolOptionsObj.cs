/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class SFXVolOptionsObj : OptionsObj
	{
		private SpriteObj m_volumeBarBG;
		private SpriteObj m_volumeBar;
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
					m_volumeBar.TextureColor = Color.Yellow;
					return;
				}
				m_volumeBar.TextureColor = Color.White;
			}
		}
		public SFXVolOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "SFX Volume")
		{
			m_volumeBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite");
			m_volumeBarBG.X = m_optionsTextOffset;
			m_volumeBarBG.Y = m_volumeBarBG.Height / 2f - 2f;
			AddChild(m_volumeBarBG);
			m_volumeBar = new SpriteObj("OptionsScreenVolumeBar_Sprite");
			m_volumeBar.X = m_volumeBarBG.X + 6f;
			m_volumeBar.Y = m_volumeBarBG.Y + 5f;
			AddChild(m_volumeBar);
		}
		public override void Initialize()
		{
			m_volumeBar.ScaleX = SoundManager.GlobalSFXVolume;
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
			{
				SoundManager.GlobalSFXVolume -= 0.01f;
				SetVolumeLevel();
			}
			else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
			{
				SoundManager.GlobalSFXVolume += 0.01f;
				SetVolumeLevel();
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				IsActive = false;
			}
			base.HandleInput();
		}
		public void SetVolumeLevel()
		{
			m_volumeBar.ScaleX = SoundManager.GlobalSFXVolume;
			Game.GameConfig.SFXVolume = SoundManager.GlobalSFXVolume;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_volumeBar = null;
				m_volumeBarBG = null;
				base.Dispose();
			}
		}
	}
}
