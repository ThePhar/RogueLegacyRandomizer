using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Randomchaos2DGodRays;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class TitleScreen : Screen
	{
		private TextObj m_titleText;
		private SpriteObj m_bg;
		private SpriteObj m_logo;
		private SpriteObj m_castle;
		private SpriteObj m_smallCloud1;
		private SpriteObj m_smallCloud2;
		private SpriteObj m_smallCloud3;
		private SpriteObj m_smallCloud4;
		private SpriteObj m_smallCloud5;
		private SpriteObj m_largeCloud1;
		private SpriteObj m_largeCloud2;
		private SpriteObj m_largeCloud3;
		private SpriteObj m_largeCloud4;
		private KeyIconTextObj m_pressStartText;
		private TextObj m_pressStartText2;
		private TextObj m_copyrightText;
		private bool m_startPressed;
		private RenderTarget2D m_godRayTexture;
		private CrepuscularRays m_godRay;
		private PostProcessingManager m_ppm;
		private float m_randomSeagullSFX;
		private Cue m_seagullCue;
		private SpriteObj m_profileCard;
		private SpriteObj m_optionsIcon;
		private SpriteObj m_creditsIcon;
		private KeyIconTextObj m_profileCardKey;
		private KeyIconTextObj m_optionsKey;
		private KeyIconTextObj m_creditsKey;
		private KeyIconTextObj m_profileSelectKey;
		private SpriteObj m_crown;
		private TextObj m_versionNumber;
		private float m_hardCoreModeOpacity;
		private bool m_optionsEntered;
		private bool m_startNewLegacy;
		private bool m_heroIsDead;
		private bool m_startNewGamePlus;
		private bool m_loadStartingRoom;
		private SpriteObj m_dlcIcon;
		public override void LoadContent()
		{
			this.m_ppm = new PostProcessingManager(base.ScreenManager.Game, base.ScreenManager.Camera);
			this.m_godRay = new CrepuscularRays(base.ScreenManager.Game, Vector2.One * 0.5f, "GameSpritesheets/flare3", 2f, 0.97f, 0.97f, 0.5f, 1.25f);
			this.m_ppm.AddEffect(this.m_godRay);
			this.m_godRayTexture = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color, DepthFormat.None);
			this.m_godRay.lightSource = new Vector2(0.495f, 0.3f);
			this.m_bg = new SpriteObj("TitleBG_Sprite");
			this.m_bg.Scale = new Vector2(1320f / (float)this.m_bg.Width, 720f / (float)this.m_bg.Height);
			this.m_bg.TextureColor = Color.Red;
			this.m_hardCoreModeOpacity = 0f;
			this.m_logo = new SpriteObj("TitleLogo_Sprite");
			this.m_logo.Position = new Vector2(660f, 360f);
			this.m_logo.DropShadow = new Vector2(0f, 5f);
			this.m_castle = new SpriteObj("TitleCastle_Sprite");
			this.m_castle.Scale = new Vector2(2f, 2f);
			this.m_castle.Position = new Vector2(630f, (float)(720 - this.m_castle.Height / 2));
			this.m_smallCloud1 = new SpriteObj("TitleSmallCloud1_Sprite");
			this.m_smallCloud1.Position = new Vector2(660f, 0f);
			this.m_smallCloud2 = new SpriteObj("TitleSmallCloud2_Sprite");
			this.m_smallCloud2.Position = this.m_smallCloud1.Position;
			this.m_smallCloud3 = new SpriteObj("TitleSmallCloud3_Sprite");
			this.m_smallCloud3.Position = this.m_smallCloud1.Position;
			this.m_smallCloud4 = new SpriteObj("TitleSmallCloud4_Sprite");
			this.m_smallCloud4.Position = this.m_smallCloud1.Position;
			this.m_smallCloud5 = new SpriteObj("TitleSmallCloud5_Sprite");
			this.m_smallCloud5.Position = this.m_smallCloud1.Position;
			this.m_largeCloud1 = new SpriteObj("TitleLargeCloud1_Sprite");
			this.m_largeCloud1.Position = new Vector2(0f, (float)(720 - this.m_largeCloud1.Height));
			this.m_largeCloud2 = new SpriteObj("TitleLargeCloud2_Sprite");
			this.m_largeCloud2.Position = new Vector2(440f, (float)(720 - this.m_largeCloud2.Height + 130));
			this.m_largeCloud3 = new SpriteObj("TitleLargeCloud1_Sprite");
			this.m_largeCloud3.Position = new Vector2(880f, (float)(720 - this.m_largeCloud3.Height + 50));
			this.m_largeCloud3.Flip = SpriteEffects.FlipHorizontally;
			this.m_largeCloud4 = new SpriteObj("TitleLargeCloud2_Sprite");
			this.m_largeCloud4.Position = new Vector2(1320f, (float)(720 - this.m_largeCloud4.Height));
			this.m_largeCloud4.Flip = SpriteEffects.FlipHorizontally;
			this.m_titleText = new TextObj(null);
			this.m_titleText.Font = Game.JunicodeFont;
			this.m_titleText.FontSize = 45f;
			this.m_titleText.Text = "ROGUE CASTLE";
			this.m_titleText.Position = new Vector2(660f, 60f);
			this.m_titleText.Align = Types.TextAlign.Centre;
			this.m_copyrightText = new TextObj(Game.JunicodeFont);
			this.m_copyrightText.FontSize = 8f;
			this.m_copyrightText.Text = " Copyright(C) 2011-2013, Cellar Door Games Inc. Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.";
			this.m_copyrightText.Align = Types.TextAlign.Centre;
			this.m_copyrightText.Position = new Vector2(660f, (float)(720 - this.m_copyrightText.Height - 10));
			this.m_copyrightText.DropShadow = new Vector2(1f, 2f);
			this.m_versionNumber = (this.m_copyrightText.Clone() as TextObj);
			this.m_versionNumber.Align = Types.TextAlign.Right;
			this.m_versionNumber.FontSize = 8f;
			this.m_versionNumber.Position = new Vector2(1305f, 5f);
			this.m_versionNumber.Text = "v1.2.0b";
			this.m_pressStartText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_pressStartText.FontSize = 20f;
			this.m_pressStartText.Text = "Press Enter to begin";
			this.m_pressStartText.Align = Types.TextAlign.Centre;
			this.m_pressStartText.Position = new Vector2(660f, 560f);
			this.m_pressStartText.DropShadow = new Vector2(2f, 2f);
			this.m_pressStartText2 = new TextObj(Game.JunicodeFont);
			this.m_pressStartText2.FontSize = 20f;
			this.m_pressStartText2.Text = "Press Enter to begin";
			this.m_pressStartText2.Align = Types.TextAlign.Centre;
			this.m_pressStartText2.Position = this.m_pressStartText.Position;
			this.m_pressStartText2.Y -= (float)(this.m_pressStartText.Height - 5);
			this.m_pressStartText2.DropShadow = new Vector2(2f, 2f);
			this.m_profileCard = new SpriteObj("TitleProfileCard_Sprite");
			this.m_profileCard.OutlineWidth = 2;
			this.m_profileCard.Scale = new Vector2(2f, 2f);
			this.m_profileCard.Position = new Vector2((float)this.m_profileCard.Width, (float)(720 - this.m_profileCard.Height));
			this.m_profileCard.ForceDraw = true;
			this.m_optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
			this.m_optionsIcon.Scale = new Vector2(2f, 2f);
			this.m_optionsIcon.OutlineWidth = this.m_profileCard.OutlineWidth;
			this.m_optionsIcon.Position = new Vector2((float)(1320 - this.m_optionsIcon.Width * 2), this.m_profileCard.Y);
			this.m_optionsIcon.ForceDraw = true;
			this.m_creditsIcon = new SpriteObj("TitleCreditsIcon_Sprite");
			this.m_creditsIcon.Scale = new Vector2(2f, 2f);
			this.m_creditsIcon.OutlineWidth = this.m_profileCard.OutlineWidth;
			this.m_creditsIcon.Position = new Vector2(this.m_optionsIcon.X + 120f, this.m_profileCard.Y);
			this.m_creditsIcon.ForceDraw = true;
			this.m_profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
			this.m_profileCardKey.Align = Types.TextAlign.Centre;
			this.m_profileCardKey.FontSize = 12f;
			this.m_profileCardKey.Text = "[Input:" + 7 + "]";
			this.m_profileCardKey.Position = new Vector2(this.m_profileCard.X, (float)(this.m_profileCard.Bounds.Top - this.m_profileCardKey.Height - 10));
			this.m_profileCardKey.ForceDraw = true;
			this.m_optionsKey = new KeyIconTextObj(Game.JunicodeFont);
			this.m_optionsKey.Align = Types.TextAlign.Centre;
			this.m_optionsKey.FontSize = 12f;
			this.m_optionsKey.Text = "[Input:" + 4 + "]";
			this.m_optionsKey.Position = new Vector2(this.m_optionsIcon.X, (float)(this.m_optionsIcon.Bounds.Top - this.m_optionsKey.Height - 10));
			this.m_optionsKey.ForceDraw = true;
			this.m_creditsKey = new KeyIconTextObj(Game.JunicodeFont);
			this.m_creditsKey.Align = Types.TextAlign.Centre;
			this.m_creditsKey.FontSize = 12f;
			this.m_creditsKey.Text = "[Input:" + 6 + "]";
			this.m_creditsKey.Position = new Vector2(this.m_creditsIcon.X, (float)(this.m_creditsIcon.Bounds.Top - this.m_creditsKey.Height - 10));
			this.m_creditsKey.ForceDraw = true;
			this.m_profileSelectKey = new KeyIconTextObj(Game.JunicodeFont);
			this.m_profileSelectKey.Align = Types.TextAlign.Left;
			this.m_profileSelectKey.FontSize = 10f;
			this.m_profileSelectKey.Text = string.Concat(new object[]
			{
				"[Input:",
				25,
				"] to Change Profile (",
				Game.GameConfig.ProfileSlot,
				")"
			});
			this.m_profileSelectKey.Position = new Vector2(30f, 15f);
			this.m_profileSelectKey.ForceDraw = true;
			this.m_profileSelectKey.DropShadow = new Vector2(2f, 2f);
			this.m_crown = new SpriteObj("Crown_Sprite");
			this.m_crown.ForceDraw = true;
			this.m_crown.Scale = new Vector2(0.7f, 0.7f);
			this.m_crown.Rotation = -30f;
			this.m_crown.OutlineWidth = 2;
			this.m_dlcIcon = new SpriteObj("MedallionPiece5_Sprite");
			this.m_dlcIcon.Position = new Vector2(950f, 310f);
			this.m_dlcIcon.ForceDraw = true;
			this.m_dlcIcon.TextureColor = Color.Yellow;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			base.Camera.Zoom = 1f;
			this.m_profileSelectKey.Text = string.Concat(new object[]
			{
				"[Input:",
				25,
				"] to Change Profile (",
				Game.GameConfig.ProfileSlot,
				")"
			});
			SoundManager.PlayMusic("TitleScreenSong", true, 1f);
			Game.ScreenManager.Player.ForceInvincible = false;
			this.m_optionsEntered = false;
			this.m_startNewLegacy = false;
			this.m_heroIsDead = false;
			this.m_startNewGamePlus = false;
			this.m_loadStartingRoom = false;
			this.m_bg.TextureColor = Color.Red;
			this.m_crown.Visible = false;
			this.m_randomSeagullSFX = (float)CDGMath.RandomInt(1, 5);
			this.m_startPressed = false;
			Tween.By(this.m_godRay, 5f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"-0.23"
			});
			this.m_logo.Opacity = 0f;
			this.m_logo.Position = new Vector2(660f, 310f);
			Tween.To(this.m_logo, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(this.m_logo, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"360"
			});
			this.m_crown.Opacity = 0f;
			this.m_crown.Position = new Vector2(390f, 200f);
			Tween.To(this.m_crown, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(this.m_crown, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"50"
			});
			this.m_dlcIcon.Opacity = 0f;
			this.m_dlcIcon.Visible = false;
			if (Game.PlayerStats.ChallengeLastBossBeaten)
			{
				this.m_dlcIcon.Visible = true;
			}
			this.m_dlcIcon.Position = new Vector2(898f, 267f);
			Tween.To(this.m_dlcIcon, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(this.m_dlcIcon, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"50"
			});
			base.Camera.Position = new Vector2(660f, 360f);
			this.m_pressStartText.Text = "[Input:" + 0 + "]";
			this.LoadSaveData();
			Game.PlayerStats.TutorialComplete = true;
			this.m_startNewLegacy = !Game.PlayerStats.CharacterFound;
			this.m_heroIsDead = Game.PlayerStats.IsDead;
			this.m_startNewGamePlus = Game.PlayerStats.LastbossBeaten;
			this.m_loadStartingRoom = Game.PlayerStats.LoadStartingRoom;
			if (Game.PlayerStats.TimesCastleBeaten > 0)
			{
				this.m_crown.Visible = true;
				this.m_bg.TextureColor = Color.White;
			}
			this.InitializeStartingText();
			base.OnEnter();
		}
		public override void OnExit()
		{
			if (this.m_seagullCue != null && this.m_seagullCue.IsPlaying)
			{
				this.m_seagullCue.Stop(AudioStopOptions.Immediate);
				this.m_seagullCue.Dispose();
			}
			base.OnExit();
		}
		public void LoadSaveData()
		{
			SkillSystem.ResetAllTraits();
			Game.PlayerStats.Dispose();
			Game.PlayerStats = new PlayerStats();
			(base.ScreenManager as RCScreenManager).Player.Reset();
			(base.ScreenManager.Game as Game).SaveManager.LoadFiles(null, new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.Lineage,
				SaveType.UpgradeData
			});
			Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
			Game.ScreenManager.Player.CurrentMana = (float)Game.PlayerStats.CurrentMana;
		}
		public void InitializeStartingText()
		{
			if (!this.m_startNewLegacy)
			{
				if (!this.m_heroIsDead)
				{
					if (Game.PlayerStats.TimesCastleBeaten == 1)
					{
						this.m_pressStartText2.Text = "Continue Your Quest +";
						return;
					}
					if (Game.PlayerStats.TimesCastleBeaten > 1)
					{
						this.m_pressStartText2.Text = "Continue Your Quest +" + Game.PlayerStats.TimesCastleBeaten;
						return;
					}
					this.m_pressStartText2.Text = "Continue Your Quest";
					return;
				}
				else
				{
					if (Game.PlayerStats.TimesCastleBeaten == 1)
					{
						this.m_pressStartText2.Text = "Choose Your Heir +";
						return;
					}
					if (Game.PlayerStats.TimesCastleBeaten > 1)
					{
						this.m_pressStartText2.Text = "Choose Your Heir +" + Game.PlayerStats.TimesCastleBeaten;
						return;
					}
					this.m_pressStartText2.Text = "Choose Your Heir";
					return;
				}
			}
			else
			{
				if (!this.m_startNewGamePlus)
				{
					this.m_pressStartText2.Text = "Start Your Legacy";
					return;
				}
				if (Game.PlayerStats.TimesCastleBeaten == 1)
				{
					this.m_pressStartText2.Text = "Start Your Legacy +";
					return;
				}
				this.m_pressStartText2.Text = "Start Your Legacy +" + Game.PlayerStats.TimesCastleBeaten;
				return;
			}
		}
		public void StartPressed()
		{
			SoundManager.PlaySound("Game_Start");
			if (!this.m_startNewLegacy)
			{
				if (!this.m_heroIsDead)
				{
					if (this.m_loadStartingRoom)
					{
						(base.ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
					}
					else
					{
						(base.ScreenManager as RCScreenManager).DisplayScreen(5, true, null);
					}
				}
				else
				{
					(base.ScreenManager as RCScreenManager).DisplayScreen(9, true, null);
				}
			}
			else
			{
				Game.PlayerStats.CharacterFound = true;
				if (this.m_startNewGamePlus)
				{
					Game.PlayerStats.LastbossBeaten = false;
					Game.PlayerStats.BlobBossBeaten = false;
					Game.PlayerStats.EyeballBossBeaten = false;
					Game.PlayerStats.FairyBossBeaten = false;
					Game.PlayerStats.FireballBossBeaten = false;
					Game.PlayerStats.FinalDoorOpened = false;
					if ((base.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map))
					{
						(base.ScreenManager.Game as Game).SaveManager.ClearFiles(new SaveType[]
						{
							SaveType.Map,
							SaveType.MapData
						});
						(base.ScreenManager.Game as Game).SaveManager.ClearBackupFiles(new SaveType[]
						{
							SaveType.Map,
							SaveType.MapData
						});
					}
				}
				else
				{
					Game.PlayerStats.Gold = 0;
				}
				Game.PlayerStats.HeadPiece = (byte)CDGMath.RandomInt(1, 5);
				Game.PlayerStats.EnemiesKilledInRun.Clear();
				(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.PlayerData,
					SaveType.Lineage,
					SaveType.UpgradeData
				});
				(base.ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
			}
			SoundManager.StopMusic(0.2f);
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_randomSeagullSFX > 0f)
			{
				this.m_randomSeagullSFX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_randomSeagullSFX <= 0f)
				{
					if (this.m_seagullCue != null && this.m_seagullCue.IsPlaying)
					{
						this.m_seagullCue.Stop(AudioStopOptions.Immediate);
						this.m_seagullCue.Dispose();
					}
					this.m_seagullCue = SoundManager.PlaySound("Wind1");
					this.m_randomSeagullSFX = (float)CDGMath.RandomInt(10, 15);
				}
			}
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			this.m_smallCloud1.Rotation += 1.8f * num;
			this.m_smallCloud2.Rotation += 1.2f * num;
			this.m_smallCloud3.Rotation += 3f * num;
			this.m_smallCloud4.Rotation -= 0.6f * num;
			this.m_smallCloud5.Rotation -= 1.8f * num;
			this.m_largeCloud2.X += 2.4f * num;
			if (this.m_largeCloud2.Bounds.Left > 1320)
			{
				this.m_largeCloud2.X = (float)(-(float)(this.m_largeCloud2.Width / 2));
			}
			this.m_largeCloud3.X -= 3f * num;
			if (this.m_largeCloud3.Bounds.Right < 0)
			{
				this.m_largeCloud3.X = (float)(1320 + this.m_largeCloud3.Width / 2);
			}
			if (!this.m_startPressed)
			{
				this.m_pressStartText.Opacity = (float)Math.Abs(Math.Sin((double)(Game.TotalGameTime * 1f)));
			}
			this.m_godRay.LightSourceSize = 1f + (float)Math.Abs(Math.Sin((double)(Game.TotalGameTime * 0.5f))) * 0.5f;
			if (this.m_optionsEntered && Game.ScreenManager.CurrentScreen == this)
			{
				this.m_optionsEntered = false;
				this.m_optionsKey.Text = "[Input:" + 4 + "]";
				this.m_profileCardKey.Text = "[Input:" + 7 + "]";
				this.m_creditsKey.Text = "[Input:" + 6 + "]";
				this.m_profileSelectKey.Text = string.Concat(new object[]
				{
					"[Input:",
					25,
					"] to Change Profile (",
					Game.GameConfig.ProfileSlot,
					")"
				});
			}
			base.Update(gameTime);
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				this.StartPressed();
			}
			if (!this.m_startNewLegacy && Game.GlobalInput.JustPressed(7))
			{
				(base.ScreenManager as RCScreenManager).DisplayScreen(17, false, null);
			}
			if (Game.GlobalInput.JustPressed(4))
			{
				this.m_optionsEntered = true;
				List<object> list = new List<object>();
				list.Add(true);
				(base.ScreenManager as RCScreenManager).DisplayScreen(4, false, list);
			}
			if (Game.GlobalInput.JustPressed(6))
			{
				(base.ScreenManager as RCScreenManager).DisplayScreen(18, false, null);
			}
			if (Game.GlobalInput.JustPressed(25))
			{
				(base.ScreenManager as RCScreenManager).DisplayScreen(30, false, null);
			}
			base.HandleInput();
		}
		public void ChangeRay()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Up))
			{
				this.m_godRay.lightSource = new Vector2(this.m_godRay.lightSource.X, this.m_godRay.lightSource.Y - 0.01f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Down))
			{
				this.m_godRay.lightSource = new Vector2(this.m_godRay.lightSource.X, this.m_godRay.lightSource.Y + 0.01f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Left))
			{
				this.m_godRay.lightSource = new Vector2(this.m_godRay.lightSource.X - 0.01f, this.m_godRay.lightSource.Y);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Right))
			{
				this.m_godRay.lightSource = new Vector2(this.m_godRay.lightSource.X + 0.01f, this.m_godRay.lightSource.Y);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Y))
			{
				this.m_godRay.Exposure += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.H))
			{
				this.m_godRay.Exposure -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.U))
			{
				this.m_godRay.LightSourceSize += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.J))
			{
				this.m_godRay.LightSourceSize -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.I))
			{
				this.m_godRay.Density += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.K))
			{
				this.m_godRay.Density -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.O))
			{
				this.m_godRay.Decay += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.L))
			{
				this.m_godRay.Decay -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.P))
			{
				this.m_godRay.Weight += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
			{
				this.m_godRay.Weight -= 0.01f;
			}
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_godRayTexture);
			base.Camera.GraphicsDevice.Clear(Color.White);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null);
			this.m_smallCloud1.DrawOutline(base.Camera);
			this.m_smallCloud3.DrawOutline(base.Camera);
			this.m_smallCloud4.DrawOutline(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_castle.DrawOutline(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_smallCloud2.DrawOutline(base.Camera);
			this.m_smallCloud5.DrawOutline(base.Camera);
			this.m_logo.DrawOutline(base.Camera);
			this.m_dlcIcon.DrawOutline(base.Camera);
			this.m_crown.DrawOutline(base.Camera);
			base.Camera.End();
			this.m_ppm.Draw(gameTime, this.m_godRayTexture);
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_godRayTexture);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			this.m_bg.Draw(base.Camera);
			this.m_smallCloud1.Draw(base.Camera);
			this.m_smallCloud3.Draw(base.Camera);
			this.m_smallCloud4.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_castle.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_smallCloud2.Draw(base.Camera);
			this.m_smallCloud5.Draw(base.Camera);
			this.m_largeCloud1.Draw(base.Camera);
			this.m_largeCloud2.Draw(base.Camera);
			this.m_largeCloud3.Draw(base.Camera);
			this.m_largeCloud4.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			base.Camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.Black * this.m_hardCoreModeOpacity);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_logo.Draw(base.Camera);
			this.m_crown.Draw(base.Camera);
			this.m_copyrightText.Draw(base.Camera);
			this.m_versionNumber.Draw(base.Camera);
			this.m_pressStartText2.Opacity = this.m_pressStartText.Opacity;
			this.m_pressStartText.Draw(base.Camera);
			this.m_pressStartText2.Draw(base.Camera);
			if (!this.m_startNewLegacy)
			{
				this.m_profileCardKey.Draw(base.Camera);
			}
			this.m_creditsKey.Draw(base.Camera);
			this.m_optionsKey.Draw(base.Camera);
			this.m_profileSelectKey.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			if (!this.m_startNewLegacy)
			{
				this.m_profileCard.Draw(base.Camera);
			}
			this.m_dlcIcon.Draw(base.Camera);
			this.m_optionsIcon.Draw(base.Camera);
			this.m_creditsIcon.Draw(base.Camera);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.Additive);
			base.Camera.Draw(this.m_ppm.Scene, new Rectangle(0, 0, base.Camera.GraphicsDevice.Viewport.Width, base.Camera.GraphicsDevice.Viewport.Height), Color.White);
			base.Camera.Draw(this.m_godRayTexture, new Rectangle(0, 0, base.Camera.GraphicsDevice.Viewport.Width, base.Camera.GraphicsDevice.Viewport.Height), Color.White);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Title Screen");
				this.m_godRayTexture.Dispose();
				this.m_godRayTexture = null;
				this.m_bg.Dispose();
				this.m_bg = null;
				this.m_logo.Dispose();
				this.m_logo = null;
				this.m_castle.Dispose();
				this.m_castle = null;
				this.m_smallCloud1.Dispose();
				this.m_smallCloud2.Dispose();
				this.m_smallCloud3.Dispose();
				this.m_smallCloud4.Dispose();
				this.m_smallCloud5.Dispose();
				this.m_smallCloud1 = null;
				this.m_smallCloud2 = null;
				this.m_smallCloud3 = null;
				this.m_smallCloud4 = null;
				this.m_smallCloud5 = null;
				this.m_largeCloud1.Dispose();
				this.m_largeCloud1 = null;
				this.m_largeCloud2.Dispose();
				this.m_largeCloud2 = null;
				this.m_largeCloud3.Dispose();
				this.m_largeCloud3 = null;
				this.m_largeCloud4.Dispose();
				this.m_largeCloud4 = null;
				this.m_pressStartText.Dispose();
				this.m_pressStartText = null;
				this.m_pressStartText2.Dispose();
				this.m_pressStartText2 = null;
				this.m_copyrightText.Dispose();
				this.m_copyrightText = null;
				this.m_versionNumber.Dispose();
				this.m_versionNumber = null;
				this.m_titleText.Dispose();
				this.m_titleText = null;
				this.m_profileCard.Dispose();
				this.m_profileCard = null;
				this.m_optionsIcon.Dispose();
				this.m_optionsIcon = null;
				this.m_creditsIcon.Dispose();
				this.m_creditsIcon = null;
				this.m_profileCardKey.Dispose();
				this.m_profileCardKey = null;
				this.m_optionsKey.Dispose();
				this.m_optionsKey = null;
				this.m_creditsKey.Dispose();
				this.m_creditsKey = null;
				this.m_crown.Dispose();
				this.m_crown = null;
				this.m_profileSelectKey.Dispose();
				this.m_profileSelectKey = null;
				this.m_dlcIcon.Dispose();
				this.m_dlcIcon = null;
				this.m_seagullCue = null;
				base.Dispose();
			}
		}
	}
}
