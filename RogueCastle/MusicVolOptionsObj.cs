using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class MusicVolOptionsObj : OptionsObj
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
					this.m_volumeBar.TextureColor = Color.Yellow;
					return;
				}
				this.m_volumeBar.TextureColor = Color.White;
			}
		}
		public MusicVolOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Music Volume")
		{
			this.m_volumeBarBG = new SpriteObj("OptionsScreenVolumeBG_Sprite");
			this.m_volumeBarBG.X = (float)this.m_optionsTextOffset;
			this.m_volumeBarBG.Y = (float)this.m_volumeBarBG.Height / 2f - 2f;
			this.AddChild(this.m_volumeBarBG);
			this.m_volumeBar = new SpriteObj("OptionsScreenVolumeBar_Sprite");
			this.m_volumeBar.X = this.m_volumeBarBG.X + 6f;
			this.m_volumeBar.Y = this.m_volumeBarBG.Y + 5f;
			this.AddChild(this.m_volumeBar);
		}
		public override void Initialize()
		{
			this.m_volumeBar.ScaleX = SoundManager.GlobalMusicVolume;
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
			{
				SoundManager.GlobalMusicVolume -= 0.01f;
				this.SetVolumeLevel();
			}
			else if (Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23))
			{
				SoundManager.GlobalMusicVolume += 0.01f;
				this.SetVolumeLevel();
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				this.IsActive = false;
			}
			base.HandleInput();
		}
		public void SetVolumeLevel()
		{
			this.m_volumeBar.ScaleX = SoundManager.GlobalMusicVolume;
			Game.GameConfig.MusicVolume = SoundManager.GlobalMusicVolume;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_volumeBar = null;
				this.m_volumeBarBG = null;
				base.Dispose();
			}
		}
	}
}
