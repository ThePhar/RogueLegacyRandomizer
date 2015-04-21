using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class CDGSplashScreen : Screen
	{
		private SpriteObj m_logo;
		private TextObj m_loadingText;
		private bool m_levelDataLoaded;
		private bool m_fadingOut;
		private float m_totalElapsedTime;
		public override void LoadContent()
		{
			this.m_logo = new SpriteObj("CDGLogo_Sprite");
			this.m_logo.Position = new Vector2(660f, 360f);
			this.m_logo.Rotation = 90f;
			this.m_logo.ForceDraw = true;
			this.m_loadingText = new TextObj(Game.JunicodeFont);
			this.m_loadingText.FontSize = 18f;
			this.m_loadingText.Align = Types.TextAlign.Right;
			this.m_loadingText.Text = "...Loading";
			this.m_loadingText.TextureColor = new Color(100, 100, 100);
			this.m_loadingText.Position = new Vector2(1280f, 630f);
			this.m_loadingText.ForceDraw = true;
			this.m_loadingText.Opacity = 0f;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.m_levelDataLoaded = false;
			this.m_fadingOut = false;
			Thread thread = new Thread(new ThreadStart(this.LoadLevelData));
			thread.Start();
			this.m_logo.Opacity = 0f;
			Tween.To(this.m_logo, 1f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"1"
			});
			Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", new object[]
			{
				"CDGSplashCreak"
			});
			base.OnEnter();
		}
		private void LoadLevelData()
		{
			lock (this)
			{
				LevelBuilder2.Initialize();
				LevelParser.ParseRooms("Map_1x1", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_1x2", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_1x3", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_2x1", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_2x2", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_2x3", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_3x1", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_3x2", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_Special", base.ScreenManager.Game.Content, false);
				LevelParser.ParseRooms("Map_DLC1", base.ScreenManager.Game.Content, true);
				LevelBuilder2.IndexRoomList();
				this.m_levelDataLoaded = true;
			}
		}
		public void LoadNextScreen()
		{
			if ((base.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.PlayerData))
			{
				(base.ScreenManager.Game as Game).SaveManager.LoadFiles(null, new SaveType[]
				{
					SaveType.PlayerData
				});
				if (Game.PlayerStats.ShoulderPiece < 1 || Game.PlayerStats.HeadPiece < 1 || Game.PlayerStats.ChestPiece < 1)
				{
					Game.PlayerStats.TutorialComplete = false;
					return;
				}
				if (!Game.PlayerStats.TutorialComplete)
				{
					(base.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
					return;
				}
				(base.ScreenManager as RCScreenManager).DisplayScreen(3, true, null);
				return;
			}
			else
			{
				if (!Game.PlayerStats.TutorialComplete)
				{
					(base.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
					return;
				}
				(base.ScreenManager as RCScreenManager).DisplayScreen(3, true, null);
				return;
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (!this.m_levelDataLoaded && this.m_logo.Opacity == 1f)
			{
				float opacity = (float)Math.Abs(Math.Sin((double)this.m_totalElapsedTime));
				this.m_totalElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
				this.m_loadingText.Opacity = opacity;
			}
			if (this.m_levelDataLoaded && !this.m_fadingOut)
			{
				this.m_fadingOut = true;
				float opacity2 = this.m_logo.Opacity;
				this.m_logo.Opacity = 1f;
				Tween.To(this.m_logo, 1f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"1.5",
					"Opacity",
					"0"
				});
				Tween.AddEndHandlerToLastTween(this, "LoadNextScreen", new object[0]);
				Tween.To(this.m_loadingText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				this.m_logo.Opacity = opacity2;
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Begin();
			this.m_logo.Draw(base.Camera);
			this.m_loadingText.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing CDG Splash Screen");
				this.m_logo.Dispose();
				this.m_logo = null;
				this.m_loadingText.Dispose();
				this.m_loadingText = null;
				base.Dispose();
			}
		}
	}
}
