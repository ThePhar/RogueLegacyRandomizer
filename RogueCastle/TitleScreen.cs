/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Randomchaos2DGodRays;
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
			m_ppm = new PostProcessingManager(ScreenManager.Game, ScreenManager.Camera);
			m_godRay = new CrepuscularRays(ScreenManager.Game, Vector2.One * 0.5f, "GameSpritesheets/flare3", 2f, 0.97f, 0.97f, 0.5f, 1.25f);
			m_ppm.AddEffect(m_godRay);
			m_godRayTexture = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color, DepthFormat.None);
			m_godRay.lightSource = new Vector2(0.495f, 0.3f);
			m_bg = new SpriteObj("TitleBG_Sprite");
			m_bg.Scale = new Vector2(1320f / m_bg.Width, 720f / m_bg.Height);
			m_bg.TextureColor = Color.Red;
			m_hardCoreModeOpacity = 0f;
			m_logo = new SpriteObj("TitleLogo_Sprite");
			m_logo.Position = new Vector2(660f, 360f);
			m_logo.DropShadow = new Vector2(0f, 5f);
			m_castle = new SpriteObj("TitleCastle_Sprite");
			m_castle.Scale = new Vector2(2f, 2f);
			m_castle.Position = new Vector2(630f, 720 - m_castle.Height / 2);
			m_smallCloud1 = new SpriteObj("TitleSmallCloud1_Sprite");
			m_smallCloud1.Position = new Vector2(660f, 0f);
			m_smallCloud2 = new SpriteObj("TitleSmallCloud2_Sprite");
			m_smallCloud2.Position = m_smallCloud1.Position;
			m_smallCloud3 = new SpriteObj("TitleSmallCloud3_Sprite");
			m_smallCloud3.Position = m_smallCloud1.Position;
			m_smallCloud4 = new SpriteObj("TitleSmallCloud4_Sprite");
			m_smallCloud4.Position = m_smallCloud1.Position;
			m_smallCloud5 = new SpriteObj("TitleSmallCloud5_Sprite");
			m_smallCloud5.Position = m_smallCloud1.Position;
			m_largeCloud1 = new SpriteObj("TitleLargeCloud1_Sprite");
			m_largeCloud1.Position = new Vector2(0f, 720 - m_largeCloud1.Height);
			m_largeCloud2 = new SpriteObj("TitleLargeCloud2_Sprite");
			m_largeCloud2.Position = new Vector2(440f, 720 - m_largeCloud2.Height + 130);
			m_largeCloud3 = new SpriteObj("TitleLargeCloud1_Sprite");
			m_largeCloud3.Position = new Vector2(880f, 720 - m_largeCloud3.Height + 50);
			m_largeCloud3.Flip = SpriteEffects.FlipHorizontally;
			m_largeCloud4 = new SpriteObj("TitleLargeCloud2_Sprite");
			m_largeCloud4.Position = new Vector2(1320f, 720 - m_largeCloud4.Height);
			m_largeCloud4.Flip = SpriteEffects.FlipHorizontally;
			m_titleText = new TextObj(null);
			m_titleText.Font = Game.JunicodeFont;
			m_titleText.FontSize = 45f;
			m_titleText.Text = "ROGUE CASTLE";
			m_titleText.Position = new Vector2(660f, 60f);
			m_titleText.Align = Types.TextAlign.Centre;
			m_copyrightText = new TextObj(Game.JunicodeFont);
			m_copyrightText.FontSize = 8f;
			m_copyrightText.Text = "Copyright(C) 2011-2015, Cellar Door Games Inc. Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.";
			m_copyrightText.Align = Types.TextAlign.Centre;
			m_copyrightText.Position = new Vector2(660f, 720 - m_copyrightText.Height - 10);
			m_copyrightText.DropShadow = new Vector2(1f, 2f);
			m_versionNumber = (m_copyrightText.Clone() as TextObj);
			m_versionNumber.Align = Types.TextAlign.Right;
			m_versionNumber.FontSize = 8f;
			m_versionNumber.Position = new Vector2(1305f, 5f);
			m_versionNumber.Text = "v1.2.0c";
			m_pressStartText = new KeyIconTextObj(Game.JunicodeFont);
			m_pressStartText.FontSize = 20f;
			m_pressStartText.Text = "Press Enter to begin";
			m_pressStartText.Align = Types.TextAlign.Centre;
			m_pressStartText.Position = new Vector2(660f, 560f);
			m_pressStartText.DropShadow = new Vector2(2f, 2f);
			m_pressStartText2 = new TextObj(Game.JunicodeFont);
			m_pressStartText2.FontSize = 20f;
			m_pressStartText2.Text = "Press Enter to begin";
			m_pressStartText2.Align = Types.TextAlign.Centre;
			m_pressStartText2.Position = m_pressStartText.Position;
			m_pressStartText2.Y -= m_pressStartText.Height - 5;
			m_pressStartText2.DropShadow = new Vector2(2f, 2f);
			m_profileCard = new SpriteObj("TitleProfileCard_Sprite");
			m_profileCard.OutlineWidth = 2;
			m_profileCard.Scale = new Vector2(2f, 2f);
			m_profileCard.Position = new Vector2(m_profileCard.Width, 720 - m_profileCard.Height);
			m_profileCard.ForceDraw = true;
			m_optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
			m_optionsIcon.Scale = new Vector2(2f, 2f);
			m_optionsIcon.OutlineWidth = m_profileCard.OutlineWidth;
			m_optionsIcon.Position = new Vector2(1320 - m_optionsIcon.Width * 2, m_profileCard.Y);
			m_optionsIcon.ForceDraw = true;
			m_creditsIcon = new SpriteObj("TitleCreditsIcon_Sprite");
			m_creditsIcon.Scale = new Vector2(2f, 2f);
			m_creditsIcon.OutlineWidth = m_profileCard.OutlineWidth;
			m_creditsIcon.Position = new Vector2(m_optionsIcon.X + 120f, m_profileCard.Y);
			m_creditsIcon.ForceDraw = true;
			m_profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
			m_profileCardKey.Align = Types.TextAlign.Centre;
			m_profileCardKey.FontSize = 12f;
			m_profileCardKey.Text = "[Input:" + 7 + "]";
			m_profileCardKey.Position = new Vector2(m_profileCard.X, m_profileCard.Bounds.Top - m_profileCardKey.Height - 10);
			m_profileCardKey.ForceDraw = true;
			m_optionsKey = new KeyIconTextObj(Game.JunicodeFont);
			m_optionsKey.Align = Types.TextAlign.Centre;
			m_optionsKey.FontSize = 12f;
			m_optionsKey.Text = "[Input:" + 4 + "]";
			m_optionsKey.Position = new Vector2(m_optionsIcon.X, m_optionsIcon.Bounds.Top - m_optionsKey.Height - 10);
			m_optionsKey.ForceDraw = true;
			m_creditsKey = new KeyIconTextObj(Game.JunicodeFont);
			m_creditsKey.Align = Types.TextAlign.Centre;
			m_creditsKey.FontSize = 12f;
			m_creditsKey.Text = "[Input:" + 6 + "]";
			m_creditsKey.Position = new Vector2(m_creditsIcon.X, m_creditsIcon.Bounds.Top - m_creditsKey.Height - 10);
			m_creditsKey.ForceDraw = true;
			m_profileSelectKey = new KeyIconTextObj(Game.JunicodeFont);
			m_profileSelectKey.Align = Types.TextAlign.Left;
			m_profileSelectKey.FontSize = 10f;
			m_profileSelectKey.Text = string.Concat(new object[]
			{
				"[Input:",
				25,
				"] to Change Profile (",
				Game.GameConfig.ProfileSlot,
				")"
			});
			m_profileSelectKey.Position = new Vector2(30f, 15f);
			m_profileSelectKey.ForceDraw = true;
			m_profileSelectKey.DropShadow = new Vector2(2f, 2f);
			m_crown = new SpriteObj("Crown_Sprite");
			m_crown.ForceDraw = true;
			m_crown.Scale = new Vector2(0.7f, 0.7f);
			m_crown.Rotation = -30f;
			m_crown.OutlineWidth = 2;
			m_dlcIcon = new SpriteObj("MedallionPiece5_Sprite");
			m_dlcIcon.Position = new Vector2(950f, 310f);
			m_dlcIcon.ForceDraw = true;
			m_dlcIcon.TextureColor = Color.Yellow;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			Camera.Zoom = 1f;
			m_profileSelectKey.Text = string.Concat(new object[]
			{
				"[Input:",
				25,
				"] to Change Profile (",
				Game.GameConfig.ProfileSlot,
				")"
			});
			SoundManager.PlayMusic("TitleScreenSong", true, 1f);
			Game.ScreenManager.Player.ForceInvincible = false;
			m_optionsEntered = false;
			m_startNewLegacy = false;
			m_heroIsDead = false;
			m_startNewGamePlus = false;
			m_loadStartingRoom = false;
			m_bg.TextureColor = Color.Red;
			m_crown.Visible = false;
			m_randomSeagullSFX = CDGMath.RandomInt(1, 5);
			m_startPressed = false;
			Tween.By(m_godRay, 5f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"-0.23"
			});
			m_logo.Opacity = 0f;
			m_logo.Position = new Vector2(660f, 310f);
			Tween.To(m_logo, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(m_logo, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"360"
			});
			m_crown.Opacity = 0f;
			m_crown.Position = new Vector2(390f, 200f);
			Tween.To(m_crown, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(m_crown, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"50"
			});
			m_dlcIcon.Opacity = 0f;
			m_dlcIcon.Visible = false;
			if (Game.PlayerStats.ChallengeLastBossBeaten)
			{
				m_dlcIcon.Visible = true;
			}
			m_dlcIcon.Position = new Vector2(898f, 267f);
			Tween.To(m_dlcIcon, 2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.By(m_dlcIcon, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				"50"
			});
			Camera.Position = new Vector2(660f, 360f);
			m_pressStartText.Text = "[Input:" + 0 + "]";
			LoadSaveData();
			Game.PlayerStats.TutorialComplete = true;
			m_startNewLegacy = !Game.PlayerStats.CharacterFound;
			m_heroIsDead = Game.PlayerStats.IsDead;
			m_startNewGamePlus = Game.PlayerStats.LastbossBeaten;
			m_loadStartingRoom = Game.PlayerStats.LoadStartingRoom;
			if (Game.PlayerStats.TimesCastleBeaten > 0)
			{
				m_crown.Visible = true;
				m_bg.TextureColor = Color.White;
			}
			InitializeStartingText();
			base.OnEnter();
		}
		public override void OnExit()
		{
			if (m_seagullCue != null && m_seagullCue.IsPlaying)
			{
				m_seagullCue.Stop(AudioStopOptions.Immediate);
				m_seagullCue.Dispose();
			}
			base.OnExit();
		}
		public void LoadSaveData()
		{
			SkillSystem.ResetAllTraits();
			Game.PlayerStats.Dispose();
			Game.PlayerStats = new PlayerStats();
			(ScreenManager as RCScreenManager).Player.Reset();
			(ScreenManager.Game as Game).SaveManager.LoadFiles(null, new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.Lineage,
				SaveType.UpgradeData
			});
			Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
			Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
		}
		public void InitializeStartingText()
		{
			if (!m_startNewLegacy)
			{
				if (!m_heroIsDead)
				{
					if (Game.PlayerStats.TimesCastleBeaten == 1)
					{
						m_pressStartText2.Text = "Continue Your Quest +";
						return;
					}
					if (Game.PlayerStats.TimesCastleBeaten > 1)
					{
						m_pressStartText2.Text = "Continue Your Quest +" + Game.PlayerStats.TimesCastleBeaten;
						return;
					}
					m_pressStartText2.Text = "Continue Your Quest";
					return;
				}
				else
				{
					if (Game.PlayerStats.TimesCastleBeaten == 1)
					{
						m_pressStartText2.Text = "Choose Your Heir +";
						return;
					}
					if (Game.PlayerStats.TimesCastleBeaten > 1)
					{
						m_pressStartText2.Text = "Choose Your Heir +" + Game.PlayerStats.TimesCastleBeaten;
						return;
					}
					m_pressStartText2.Text = "Choose Your Heir";
					return;
				}
			}
			else
			{
				if (!m_startNewGamePlus)
				{
					m_pressStartText2.Text = "Start Your Legacy";
					return;
				}
				if (Game.PlayerStats.TimesCastleBeaten == 1)
				{
					m_pressStartText2.Text = "Start Your Legacy +";
					return;
				}
				m_pressStartText2.Text = "Start Your Legacy +" + Game.PlayerStats.TimesCastleBeaten;
				return;
			}
		}
		public void StartPressed()
		{
			SoundManager.PlaySound("Game_Start");
			if (!m_startNewLegacy)
			{
				if (!m_heroIsDead)
				{
					if (m_loadStartingRoom)
					{
						(ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
					}
					else
					{
						(ScreenManager as RCScreenManager).DisplayScreen(5, true, null);
					}
				}
				else
				{
					(ScreenManager as RCScreenManager).DisplayScreen(9, true, null);
				}
			}
			else
			{
				Game.PlayerStats.CharacterFound = true;
				if (m_startNewGamePlus)
				{
					Game.PlayerStats.LastbossBeaten = false;
					Game.PlayerStats.BlobBossBeaten = false;
					Game.PlayerStats.EyeballBossBeaten = false;
					Game.PlayerStats.FairyBossBeaten = false;
					Game.PlayerStats.FireballBossBeaten = false;
					Game.PlayerStats.FinalDoorOpened = false;
					if ((ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map))
					{
						(ScreenManager.Game as Game).SaveManager.ClearFiles(new SaveType[]
						{
							SaveType.Map,
							SaveType.MapData
						});
						(ScreenManager.Game as Game).SaveManager.ClearBackupFiles(new SaveType[]
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
				(ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.PlayerData,
					SaveType.Lineage,
					SaveType.UpgradeData
				});
				(ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
			}
			SoundManager.StopMusic(0.2f);
		}
		public override void Update(GameTime gameTime)
		{
			if (m_randomSeagullSFX > 0f)
			{
				m_randomSeagullSFX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (m_randomSeagullSFX <= 0f)
				{
					if (m_seagullCue != null && m_seagullCue.IsPlaying)
					{
						m_seagullCue.Stop(AudioStopOptions.Immediate);
						m_seagullCue.Dispose();
					}
					m_seagullCue = SoundManager.PlaySound("Wind1");
					m_randomSeagullSFX = CDGMath.RandomInt(10, 15);
				}
			}
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			m_smallCloud1.Rotation += 1.8f * num;
			m_smallCloud2.Rotation += 1.2f * num;
			m_smallCloud3.Rotation += 3f * num;
			m_smallCloud4.Rotation -= 0.6f * num;
			m_smallCloud5.Rotation -= 1.8f * num;
			m_largeCloud2.X += 2.4f * num;
			if (m_largeCloud2.Bounds.Left > 1320)
			{
				m_largeCloud2.X = -(float)(m_largeCloud2.Width / 2);
			}
			m_largeCloud3.X -= 3f * num;
			if (m_largeCloud3.Bounds.Right < 0)
			{
				m_largeCloud3.X = 1320 + m_largeCloud3.Width / 2;
			}
			if (!m_startPressed)
			{
				m_pressStartText.Opacity = (float)Math.Abs(Math.Sin(Game.TotalGameTime * 1f));
			}
			m_godRay.LightSourceSize = 1f + (float)Math.Abs(Math.Sin(Game.TotalGameTime * 0.5f)) * 0.5f;
			if (m_optionsEntered && Game.ScreenManager.CurrentScreen == this)
			{
				m_optionsEntered = false;
				m_optionsKey.Text = "[Input:" + 4 + "]";
				m_profileCardKey.Text = "[Input:" + 7 + "]";
				m_creditsKey.Text = "[Input:" + 6 + "]";
				m_profileSelectKey.Text = string.Concat(new object[]
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
				StartPressed();
			}
			if (!m_startNewLegacy && Game.GlobalInput.JustPressed(7))
			{
				(ScreenManager as RCScreenManager).DisplayScreen(17, false, null);
			}
			if (Game.GlobalInput.JustPressed(4))
			{
				m_optionsEntered = true;
				List<object> list = new List<object>();
				list.Add(true);
				(ScreenManager as RCScreenManager).DisplayScreen(4, false, list);
			}
			if (Game.GlobalInput.JustPressed(6))
			{
				(ScreenManager as RCScreenManager).DisplayScreen(18, false, null);
			}
			if (Game.GlobalInput.JustPressed(25))
			{
				(ScreenManager as RCScreenManager).DisplayScreen(30, false, null);
			}
			base.HandleInput();
		}
		public void ChangeRay()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Up))
			{
				m_godRay.lightSource = new Vector2(m_godRay.lightSource.X, m_godRay.lightSource.Y - 0.01f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Down))
			{
				m_godRay.lightSource = new Vector2(m_godRay.lightSource.X, m_godRay.lightSource.Y + 0.01f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Left))
			{
				m_godRay.lightSource = new Vector2(m_godRay.lightSource.X - 0.01f, m_godRay.lightSource.Y);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Right))
			{
				m_godRay.lightSource = new Vector2(m_godRay.lightSource.X + 0.01f, m_godRay.lightSource.Y);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Y))
			{
				m_godRay.Exposure += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.H))
			{
				m_godRay.Exposure -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.U))
			{
				m_godRay.LightSourceSize += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.J))
			{
				m_godRay.LightSourceSize -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.I))
			{
				m_godRay.Density += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.K))
			{
				m_godRay.Density -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.O))
			{
				m_godRay.Decay += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.L))
			{
				m_godRay.Decay -= 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.P))
			{
				m_godRay.Weight += 0.01f;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
			{
				m_godRay.Weight -= 0.01f;
			}
		}
		public override void Draw(GameTime gameTime)
		{
			Camera.GraphicsDevice.SetRenderTarget(m_godRayTexture);
			Camera.GraphicsDevice.Clear(Color.White);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null);
			m_smallCloud1.DrawOutline(Camera);
			m_smallCloud3.DrawOutline(Camera);
			m_smallCloud4.DrawOutline(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			m_castle.DrawOutline(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_smallCloud2.DrawOutline(Camera);
			m_smallCloud5.DrawOutline(Camera);
			m_logo.DrawOutline(Camera);
			m_dlcIcon.DrawOutline(Camera);
			m_crown.DrawOutline(Camera);
			Camera.End();
			m_ppm.Draw(gameTime, m_godRayTexture);
			Camera.GraphicsDevice.SetRenderTarget(m_godRayTexture);
			Camera.GraphicsDevice.Clear(Color.Black);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			m_bg.Draw(Camera);
			m_smallCloud1.Draw(Camera);
			m_smallCloud3.Draw(Camera);
			m_smallCloud4.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			m_castle.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_smallCloud2.Draw(Camera);
			m_smallCloud5.Draw(Camera);
			m_largeCloud1.Draw(Camera);
			m_largeCloud2.Draw(Camera);
			m_largeCloud3.Draw(Camera);
			m_largeCloud4.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			Camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.Black * m_hardCoreModeOpacity);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_logo.Draw(Camera);
			m_crown.Draw(Camera);
			m_copyrightText.Draw(Camera);
			m_versionNumber.Draw(Camera);
			m_pressStartText2.Opacity = m_pressStartText.Opacity;
			m_pressStartText.Draw(Camera);
			m_pressStartText2.Draw(Camera);
			if (!m_startNewLegacy)
			{
				m_profileCardKey.Draw(Camera);
			}
			m_creditsKey.Draw(Camera);
			m_optionsKey.Draw(Camera);
			m_profileSelectKey.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			if (!m_startNewLegacy)
			{
				m_profileCard.Draw(Camera);
			}
			m_dlcIcon.Draw(Camera);
			m_optionsIcon.Draw(Camera);
			m_creditsIcon.Draw(Camera);
			Camera.End();
			Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
			Camera.GraphicsDevice.Clear(Color.Black);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.Additive);
			Camera.Draw(m_ppm.Scene, new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height), Color.White);
			Camera.Draw(m_godRayTexture, new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height), Color.White);
			Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Title Screen");
				m_godRayTexture.Dispose();
				m_godRayTexture = null;
				m_bg.Dispose();
				m_bg = null;
				m_logo.Dispose();
				m_logo = null;
				m_castle.Dispose();
				m_castle = null;
				m_smallCloud1.Dispose();
				m_smallCloud2.Dispose();
				m_smallCloud3.Dispose();
				m_smallCloud4.Dispose();
				m_smallCloud5.Dispose();
				m_smallCloud1 = null;
				m_smallCloud2 = null;
				m_smallCloud3 = null;
				m_smallCloud4 = null;
				m_smallCloud5 = null;
				m_largeCloud1.Dispose();
				m_largeCloud1 = null;
				m_largeCloud2.Dispose();
				m_largeCloud2 = null;
				m_largeCloud3.Dispose();
				m_largeCloud3 = null;
				m_largeCloud4.Dispose();
				m_largeCloud4 = null;
				m_pressStartText.Dispose();
				m_pressStartText = null;
				m_pressStartText2.Dispose();
				m_pressStartText2 = null;
				m_copyrightText.Dispose();
				m_copyrightText = null;
				m_versionNumber.Dispose();
				m_versionNumber = null;
				m_titleText.Dispose();
				m_titleText = null;
				m_profileCard.Dispose();
				m_profileCard = null;
				m_optionsIcon.Dispose();
				m_optionsIcon = null;
				m_creditsIcon.Dispose();
				m_creditsIcon = null;
				m_profileCardKey.Dispose();
				m_profileCardKey = null;
				m_optionsKey.Dispose();
				m_optionsKey = null;
				m_creditsKey.Dispose();
				m_creditsKey = null;
				m_crown.Dispose();
				m_crown = null;
				m_profileSelectKey.Dispose();
				m_profileSelectKey = null;
				m_dlcIcon.Dispose();
				m_dlcIcon = null;
				m_seagullCue = null;
				base.Dispose();
			}
		}
	}
}
