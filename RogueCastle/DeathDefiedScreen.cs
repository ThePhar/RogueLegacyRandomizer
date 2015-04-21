using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class DeathDefiedScreen : Screen
	{
		private PlayerObj m_player;
		private SpriteObj m_spotlight;
		private SpriteObj m_titlePlate;
		private SpriteObj m_title;
		private Vector2 m_cameraPos;
		private string m_songName;
		private float m_storedMusicVolume;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public override void LoadContent()
		{
			new Vector2(2f, 2f);
			new Color(255, 254, 128);
			this.m_spotlight = new SpriteObj("GameOverSpotlight_Sprite");
			this.m_spotlight.Rotation = 90f;
			this.m_spotlight.ForceDraw = true;
			this.m_spotlight.Position = new Vector2(660f, (float)(40 + this.m_spotlight.Height));
			this.m_titlePlate = new SpriteObj("SkillUnlockTitlePlate_Sprite");
			this.m_titlePlate.Position = new Vector2(660f, 160f);
			this.m_titlePlate.ForceDraw = true;
			this.m_title = new SpriteObj("DeathDefyText_Sprite");
			this.m_title.Position = this.m_titlePlate.Position;
			this.m_title.Y -= 40f;
			this.m_title.ForceDraw = true;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.m_cameraPos = new Vector2(base.Camera.X, base.Camera.Y);
			if (this.m_player == null)
			{
				this.m_player = (base.ScreenManager as RCScreenManager).Player;
			}
			this.m_titlePlate.Scale = Vector2.Zero;
			this.m_title.Scale = Vector2.Zero;
			this.m_title.Opacity = 1f;
			this.m_titlePlate.Opacity = 1f;
			this.m_storedMusicVolume = SoundManager.GlobalMusicVolume;
			this.m_songName = SoundManager.GetCurrentMusicName();
			Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new string[]
			{
				"GlobalMusicVolume",
				(this.m_storedMusicVolume * 0.1f).ToString()
			});
			SoundManager.PlaySound("Player_Death_FadeToBlack");
			this.m_player.Visible = true;
			this.m_player.Opacity = 1f;
			this.m_spotlight.Opacity = 0f;
			Tween.To(this, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"1"
			});
			Tween.To(this.m_spotlight, 0.1f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"1",
				"Opacity",
				"1"
			});
			Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_Spotlight"
			});
			Tween.To(base.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_player.AbsX.ToString(),
				"Y",
				(this.m_player.Bounds.Bottom - 10).ToString(),
				"Zoom",
				"1"
			});
			Tween.RunFunction(2f, this, "PlayerLevelUpAnimation", new object[0]);
			base.OnEnter();
		}
		public void PlayerLevelUpAnimation()
		{
			this.m_player.ChangeSprite("PlayerLevelUp_Character");
			this.m_player.PlayAnimation(false);
			Tween.To(this.m_titlePlate, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_title, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0.8",
				"ScaleY",
				"0.8"
			});
			Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"GetItemStinger3"
			});
			Tween.RunFunction(2f, this, "ExitTransition", new object[0]);
		}
		public void ExitTransition()
		{
			Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new string[]
			{
				"GlobalMusicVolume",
				this.m_storedMusicVolume.ToString()
			});
			Tween.To(base.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_cameraPos.X.ToString(),
				"Y",
				this.m_cameraPos.Y.ToString()
			});
			Tween.To(this.m_spotlight, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_titlePlate, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_title, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"BackBufferOpacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, base.Camera.GetTransformation());
			base.Camera.Draw(Game.GenericTexture, new Rectangle((int)base.Camera.TopLeftCorner.X - 10, (int)base.Camera.TopLeftCorner.Y - 10, 1340, 740), Color.Black * this.BackBufferOpacity);
			this.m_player.Draw(base.Camera);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
			this.m_spotlight.Draw(base.Camera);
			this.m_titlePlate.Draw(base.Camera);
			this.m_title.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Death Defied Screen");
				this.m_player = null;
				this.m_spotlight.Dispose();
				this.m_spotlight = null;
				this.m_title.Dispose();
				this.m_title = null;
				this.m_titlePlate.Dispose();
				this.m_titlePlate = null;
				base.Dispose();
			}
		}
	}
}
