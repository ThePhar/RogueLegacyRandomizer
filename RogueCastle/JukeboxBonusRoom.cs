using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class JukeboxBonusRoom : BonusRoomObj
	{
		private GameObj m_jukeBox;
		private string[] m_songList;
		private string[] m_songTitleList;
		private int m_songIndex;
		private bool m_rotatedLeft;
		private TextObj m_nowPlayingText;
		private TextObj m_songTitle;
		private SpriteObj m_speechBubble;
		public JukeboxBonusRoom()
		{
			this.m_songList = new string[]
			{
				"CastleBossSong",
				"GardenSong",
				"GardenBossSong",
				"TowerSong",
				"TowerBossSong",
				"DungeonSong",
				"DungeonBoss",
				"CastleSong",
				"PooyanSong",
				"LegacySong",
				"SkillTreeSong",
				"TitleScreenSong",
				"CreditsSong",
				"LastBossSong",
				"EndSong",
				"EndSongDrums"
			};
			this.m_songTitleList = new string[]
			{
				"Pistol Shrimp",
				"The Grim Outdoors",
				"Skin Off My Teeth",
				"Narwhal",
				"Lamprey",
				"Broadside of the Broadsword",
				"Mincemeat",
				"Trilobyte",
				"Poot-yan",
				"SeaSawHorse (Legacy)",
				"SeaSawHorse (Manor)",
				"Rogue Legacy",
				"The Fish and the Whale",
				"Rotten Legacy",
				"Whale. Shark.",
				"Whale. Shark. (Drums)"
			};
		}
		public override void Initialize()
		{
			this.m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			this.m_speechBubble.Visible = false;
			base.GameObjList.Add(this.m_speechBubble);
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "Jukebox")
				{
					this.m_jukeBox = current;
					break;
				}
			}
			(this.m_jukeBox as SpriteObj).OutlineWidth = 2;
			this.m_jukeBox.Y -= 2f;
			this.m_speechBubble.Position = new Vector2(this.m_jukeBox.X, this.m_jukeBox.Y - (float)this.m_speechBubble.Height - 20f);
			base.Initialize();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			this.m_songTitle = new TextObj(null);
			this.m_songTitle.Font = Game.JunicodeLargeFont;
			this.m_songTitle.Align = Types.TextAlign.Right;
			this.m_songTitle.Text = "Song name here";
			this.m_songTitle.Opacity = 0f;
			this.m_songTitle.FontSize = 40f;
			this.m_songTitle.Position = new Vector2(1270f, 570f);
			this.m_songTitle.OutlineWidth = 2;
			this.m_nowPlayingText = (this.m_songTitle.Clone() as TextObj);
			this.m_nowPlayingText.Text = "Now Playing";
			this.m_nowPlayingText.FontSize = 24f;
			this.m_nowPlayingText.Y -= 50f;
			base.LoadContent(graphics);
		}
		public override void OnEnter()
		{
			this.m_jukeBox.Scale = new Vector2(3f, 3f);
			this.m_jukeBox.Rotation = 0f;
			base.OnEnter();
		}
		public override void Update(GameTime gameTime)
		{
			if (CollisionMath.Intersects(this.Player.Bounds, this.m_jukeBox.Bounds))
			{
				this.m_speechBubble.Visible = true;
				this.m_speechBubble.Y = this.m_jukeBox.Y - (float)this.m_speechBubble.Height - 110f + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
				if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					Tween.StopAllContaining(this.m_jukeBox, false);
					this.m_jukeBox.Scale = new Vector2(3f, 3f);
					this.m_jukeBox.Rotation = 0f;
					Tween.StopAllContaining(this.m_nowPlayingText, false);
					Tween.StopAllContaining(this.m_songTitle, false);
					this.m_songTitle.Opacity = 0f;
					this.m_nowPlayingText.Opacity = 0f;
					Tween.To(this.m_songTitle, 0.5f, new Easing(Linear.EaseNone), new string[]
					{
						"delay",
						"0.2",
						"Opacity",
						"1"
					});
					this.m_songTitle.Opacity = 1f;
					Tween.To(this.m_songTitle, 0.5f, new Easing(Linear.EaseNone), new string[]
					{
						"delay",
						"2.2",
						"Opacity",
						"0"
					});
					this.m_songTitle.Opacity = 0f;
					Tween.To(this.m_nowPlayingText, 0.5f, new Easing(Linear.EaseNone), new string[]
					{
						"Opacity",
						"1"
					});
					this.m_nowPlayingText.Opacity = 1f;
					Tween.To(this.m_nowPlayingText, 0.5f, new Easing(Linear.EaseNone), new string[]
					{
						"delay",
						"2",
						"Opacity",
						"0"
					});
					this.m_nowPlayingText.Opacity = 0f;
					SoundManager.PlayMusic(this.m_songList[this.m_songIndex], true, 1f);
					this.m_songTitle.Text = this.m_songTitleList[this.m_songIndex];
					this.m_songIndex++;
					if (this.m_songIndex > this.m_songList.Length - 1)
					{
						this.m_songIndex = 0;
					}
					this.AnimateJukebox();
					this.CheckForSongRepeat();
				}
			}
			else
			{
				this.m_speechBubble.Visible = false;
			}
			base.Update(gameTime);
		}
		private void CheckForSongRepeat()
		{
			Game.ScreenManager.GetLevelScreen().JukeboxEnabled = true;
		}
		public void AnimateJukebox()
		{
			Tween.To(this.m_jukeBox, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleY",
				"2.9",
				"ScaleX",
				"3.1",
				"Rotation",
				"0"
			});
			Tween.AddEndHandlerToLastTween(this, "AnimateJukebox2", new object[0]);
			this.Player.AttachedLevel.ImpactEffectPool.DisplayMusicNote(new Vector2((float)(this.m_jukeBox.Bounds.Center.X + CDGMath.RandomInt(-20, 20)), (float)(this.m_jukeBox.Bounds.Top + CDGMath.RandomInt(0, 20))));
		}
		public void AnimateJukebox2()
		{
			if (!this.m_rotatedLeft)
			{
				Tween.To(this.m_jukeBox, 0.2f, new Easing(Tween.EaseNone), new string[]
				{
					"Rotation",
					"-2"
				});
				this.m_rotatedLeft = true;
			}
			else
			{
				Tween.To(this.m_jukeBox, 0.2f, new Easing(Tween.EaseNone), new string[]
				{
					"Rotation",
					"2"
				});
				this.m_rotatedLeft = false;
			}
			Tween.To(this.m_jukeBox, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleY",
				"3.1",
				"ScaleX",
				"2.9"
			});
			Tween.AddEndHandlerToLastTween(this, "AnimateJukebox", new object[0]);
		}
		public override void Draw(Camera2D camera)
		{
			this.m_songTitle.Position = new Vector2(base.X + 1320f - 50f, base.Y + 720f - 150f);
			this.m_nowPlayingText.Position = this.m_songTitle.Position;
			this.m_nowPlayingText.Y -= 50f;
			base.Draw(camera);
			SamplerState value = camera.GraphicsDevice.SamplerStates[0];
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_songTitle.Draw(camera);
			this.m_nowPlayingText.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = value;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_songTitle.Dispose();
				this.m_songTitle = null;
				this.m_nowPlayingText.Dispose();
				this.m_nowPlayingText = null;
				this.m_jukeBox = null;
				Array.Clear(this.m_songList, 0, this.m_songList.Length);
				Array.Clear(this.m_songTitleList, 0, this.m_songTitleList.Length);
				this.m_songTitleList = null;
				this.m_songList = null;
				this.m_speechBubble = null;
				base.Dispose();
			}
		}
	}
}
