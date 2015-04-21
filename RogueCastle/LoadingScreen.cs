using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Threading;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class LoadingScreen : Screen
	{
		private TextObj m_loadingText;
		private byte m_screenTypeToLoad;
		private bool m_isLoading;
		private bool m_loadingComplete;
		private Screen m_levelToLoad;
		private ImpactEffectPool m_effectPool;
		private ObjContainer m_gateSprite;
		private SpriteObj m_blackTransitionIn;
		private SpriteObj m_blackScreen;
		private SpriteObj m_blackTransitionOut;
		private bool m_wipeTransition;
		private bool m_gameCrashed;
		private bool m_horizontalShake;
		private bool m_verticalShake;
		private bool m_shakeScreen;
		private float m_screenShakeMagnitude;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public LoadingScreen(byte screenType, bool wipeTransition)
		{
			this.m_screenTypeToLoad = screenType;
			this.m_effectPool = new ImpactEffectPool(50);
			this.m_wipeTransition = wipeTransition;
		}
		public override void LoadContent()
		{
			this.m_loadingText = new TextObj(null);
			this.m_loadingText.Font = Game.JunicodeLargeFont;
			this.m_loadingText.Text = "Building";
			this.m_loadingText.Align = Types.TextAlign.Centre;
			this.m_loadingText.FontSize = 40f;
			this.m_loadingText.OutlineWidth = 4;
			this.m_loadingText.ForceDraw = true;
			this.m_gateSprite = new ObjContainer("LoadingScreenGate_Character");
			this.m_gateSprite.ForceDraw = true;
			this.m_gateSprite.Scale = new Vector2(2f, 2f);
			this.m_gateSprite.Y -= (float)this.m_gateSprite.Height;
			this.m_effectPool.Initialize();
			this.m_blackTransitionIn = new SpriteObj("Blank_Sprite");
			this.m_blackTransitionIn.Rotation = 15f;
			this.m_blackTransitionIn.Scale = new Vector2((float)(1320 / this.m_blackTransitionIn.Width), (float)(2000 / this.m_blackTransitionIn.Height));
			this.m_blackTransitionIn.TextureColor = Color.Black;
			this.m_blackTransitionIn.ForceDraw = true;
			this.m_blackScreen = new SpriteObj("Blank_Sprite");
			this.m_blackScreen.Scale = new Vector2((float)(1320 / this.m_blackScreen.Width), (float)(720 / this.m_blackScreen.Height));
			this.m_blackScreen.TextureColor = Color.Black;
			this.m_blackScreen.ForceDraw = true;
			this.m_blackTransitionOut = new SpriteObj("Blank_Sprite");
			this.m_blackTransitionOut.Rotation = 15f;
			this.m_blackTransitionOut.Scale = new Vector2((float)(1320 / this.m_blackTransitionOut.Width), (float)(2000 / this.m_blackTransitionOut.Height));
			this.m_blackTransitionOut.TextureColor = Color.Black;
			this.m_blackTransitionOut.ForceDraw = true;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.BackBufferOpacity = 0f;
			this.m_gameCrashed = false;
			if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
			{
				this.m_loadingText.Text = "Jacking In";
			}
			else if (Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f)
			{
				this.m_loadingText.Text = "Reminiscing";
			}
			else if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
			{
				this.m_loadingText.Text = "Balding";
			}
			else
			{
				this.m_loadingText.Text = "Building";
			}
			if (!this.m_loadingComplete)
			{
				if (this.m_screenTypeToLoad == 27)
				{
					Tween.To(this, 0.05f, new Easing(Tween.EaseNone), new string[]
					{
						"BackBufferOpacity",
						"1"
					});
					Tween.RunFunction(1f, this, "BeginThreading", new object[0]);
				}
				else
				{
					this.m_blackTransitionIn.X = 0f;
					this.m_blackTransitionIn.X = (float)(1320 - this.m_blackTransitionIn.Bounds.Left);
					this.m_blackScreen.X = this.m_blackTransitionIn.X;
					this.m_blackTransitionOut.X = this.m_blackScreen.X + (float)this.m_blackScreen.Width;
					if (!this.m_wipeTransition)
					{
						SoundManager.PlaySound("GateDrop");
						Tween.To(this.m_gateSprite, 0.5f, new Easing(Tween.EaseNone), new string[]
						{
							"Y",
							"0"
						});
						Tween.RunFunction(0.3f, this.m_effectPool, "LoadingGateSmokeEffect", new object[]
						{
							40
						});
						Tween.RunFunction(0.3f, typeof(SoundManager), "PlaySound", new object[]
						{
							"GateSlam"
						});
						Tween.RunFunction(0.55f, this, "ShakeScreen", new object[]
						{
							4,
							true,
							true
						});
						Tween.RunFunction(0.65f, this, "StopScreenShake", new object[0]);
						Tween.RunFunction(1.5f, this, "BeginThreading", new object[0]);
					}
					else
					{
						Tween.By(this.m_blackTransitionIn, 0.15f, new Easing(Quad.EaseIn), new string[]
						{
							"X",
							(-this.m_blackTransitionIn.X).ToString()
						});
						Tween.By(this.m_blackScreen, 0.15f, new Easing(Quad.EaseIn), new string[]
						{
							"X",
							(-this.m_blackTransitionIn.X).ToString()
						});
						Tween.By(this.m_blackTransitionOut, 0.2f, new Easing(Quad.EaseIn), new string[]
						{
							"X",
							(-this.m_blackTransitionIn.X).ToString()
						});
						Tween.AddEndHandlerToLastTween(this, "BeginThreading", new object[0]);
					}
				}
				base.OnEnter();
			}
		}
		public void BeginThreading()
		{
			Tween.StopAll(false);
			Thread thread = new Thread(new ThreadStart(this.BeginLoading));
			if (thread.CurrentCulture.Name != "en-US")
			{
				thread.CurrentCulture = new CultureInfo("en-US", false);
				thread.CurrentUICulture = new CultureInfo("en-US", false);
			}
			thread.Start();
		}
		private void BeginLoading()
		{
			this.m_isLoading = true;
			this.m_loadingComplete = false;
			byte screenTypeToLoad = this.m_screenTypeToLoad;
			if (screenTypeToLoad <= 9)
			{
				switch (screenTypeToLoad)
				{
				case 1:
					this.m_levelToLoad = new CDGSplashScreen();
					lock (this.m_levelToLoad)
					{
						this.m_loadingComplete = true;
						return;
					}
					break;
				case 2:
				case 4:
					return;
				case 3:
					goto IL_199;
				case 5:
					goto IL_205;
				default:
					if (screenTypeToLoad != 9)
					{
						return;
					}
					goto IL_1CF;
				}
			}
			else
			{
				if (screenTypeToLoad == 15)
				{
					goto IL_205;
				}
				if (screenTypeToLoad == 18)
				{
					goto IL_11E;
				}
				switch (screenTypeToLoad)
				{
				case 23:
				case 24:
					goto IL_205;
				case 25:
				case 26:
					return;
				case 27:
					goto IL_199;
				case 28:
					goto IL_E8;
				case 29:
					break;
				default:
					return;
				}
			}
			this.m_levelToLoad = new DemoEndScreen();
			lock (this.m_levelToLoad)
			{
				this.m_loadingComplete = true;
				return;
			}
			IL_E8:
			this.m_levelToLoad = new DemoStartScreen();
			lock (this.m_levelToLoad)
			{
				this.m_loadingComplete = true;
				return;
			}
			IL_11E:
			this.m_levelToLoad = new CreditsScreen();
			bool isEnding = true;
			Screen[] screens = base.ScreenManager.GetScreens();
			for (int i = 0; i < screens.Length; i++)
			{
				Screen screen = screens[i];
				if (screen is TitleScreen)
				{
					isEnding = false;
					break;
				}
			}
			(this.m_levelToLoad as CreditsScreen).IsEnding = isEnding;
			lock (this.m_levelToLoad)
			{
				this.m_loadingComplete = true;
				return;
			}
			IL_199:
			this.m_levelToLoad = new TitleScreen();
			lock (this.m_levelToLoad)
			{
				this.m_loadingComplete = true;
				return;
			}
			IL_1CF:
			this.m_levelToLoad = new LineageScreen();
			lock (this.m_levelToLoad)
			{
				this.m_loadingComplete = true;
				return;
			}
			IL_205:
			RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
			AreaStruct[] area1List = Game.Area1List;
			this.m_levelToLoad = null;
			if (this.m_screenTypeToLoad == 15)
			{
				this.m_levelToLoad = LevelBuilder2.CreateStartingRoom();
			}
			else if (this.m_screenTypeToLoad == 23)
			{
				this.m_levelToLoad = LevelBuilder2.CreateTutorialRoom();
			}
			else if (this.m_screenTypeToLoad == 24)
			{
				this.m_levelToLoad = LevelBuilder2.CreateEndingRoom();
			}
			else
			{
				ProceduralLevelScreen levelScreen = (base.ScreenManager as RCScreenManager).GetLevelScreen();
				if (levelScreen != null)
				{
					if (Game.PlayerStats.LockCastle)
					{
						try
						{
							this.m_levelToLoad = (base.ScreenManager.Game as Game).SaveManager.LoadMap();
						}
						catch
						{
							this.m_gameCrashed = true;
						}
						if (!this.m_gameCrashed)
						{
							(base.ScreenManager.Game as Game).SaveManager.LoadFiles(this.m_levelToLoad as ProceduralLevelScreen, new SaveType[]
							{
								SaveType.MapData
							});
							Game.PlayerStats.LockCastle = false;
						}
					}
					else
					{
						this.m_levelToLoad = LevelBuilder2.CreateLevel(levelScreen.RoomList[0], area1List);
					}
				}
				else if (Game.PlayerStats.LoadStartingRoom)
				{
					Console.WriteLine("This should only be used for debug purposes");
					this.m_levelToLoad = LevelBuilder2.CreateLevel(null, area1List);
					(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
					{
						SaveType.Map,
						SaveType.MapData
					});
				}
				else
				{
					try
					{
						this.m_levelToLoad = (base.ScreenManager.Game as Game).SaveManager.LoadMap();
						(base.ScreenManager.Game as Game).SaveManager.LoadFiles(this.m_levelToLoad as ProceduralLevelScreen, new SaveType[]
						{
							SaveType.MapData
						});
					}
					catch
					{
						this.m_gameCrashed = true;
					}
					if (!this.m_gameCrashed)
					{
						Game.ScreenManager.Player.Position = new Vector2((this.m_levelToLoad as ProceduralLevelScreen).RoomList[1].X, 420f);
					}
				}
			}
			if (!this.m_gameCrashed)
			{
				lock (this.m_levelToLoad)
				{
					ProceduralLevelScreen proceduralLevelScreen = this.m_levelToLoad as ProceduralLevelScreen;
					proceduralLevelScreen.Player = rCScreenManager.Player;
					rCScreenManager.Player.AttachLevel(proceduralLevelScreen);
					for (int j = 0; j < proceduralLevelScreen.RoomList.Count; j++)
					{
						proceduralLevelScreen.RoomList[j].RoomNumber = j + 1;
					}
					rCScreenManager.AttachMap(proceduralLevelScreen);
					if (!this.m_wipeTransition)
					{
						Thread.Sleep(100);
					}
					this.m_loadingComplete = true;
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_gameCrashed)
			{
				(base.ScreenManager.Game as Game).SaveManager.ForceBackup();
			}
			if (this.m_isLoading && this.m_loadingComplete && !this.m_gameCrashed)
			{
				this.EndLoading();
			}
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			this.m_gateSprite.GetChildAt(1).Rotation += 120f * num;
			this.m_gateSprite.GetChildAt(2).Rotation -= 120f * num;
			if (this.m_shakeScreen)
			{
				this.UpdateShake();
			}
			base.Update(gameTime);
		}
		public void EndLoading()
		{
			this.m_isLoading = false;
			ScreenManager screenManager = base.ScreenManager;
			Screen[] screens = base.ScreenManager.GetScreens();
			for (int i = 0; i < screens.Length; i++)
			{
				Screen screen = screens[i];
				if (screen != this)
				{
					screenManager.RemoveScreen(screen, true);
				}
				else
				{
					screenManager.RemoveScreen(screen, false);
				}
			}
			base.ScreenManager = screenManager;
			this.m_levelToLoad.DrawIfCovered = true;
			if (this.m_screenTypeToLoad == 15)
			{
				if (Game.PlayerStats.IsDead)
				{
					(this.m_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = true;
				}
				base.ScreenManager.AddScreen(this.m_levelToLoad, new PlayerIndex?(PlayerIndex.One));
				if (Game.PlayerStats.IsDead)
				{
					base.ScreenManager.AddScreen((base.ScreenManager as RCScreenManager).SkillScreen, new PlayerIndex?(PlayerIndex.One));
					(this.m_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = false;
				}
				this.m_levelToLoad.UpdateIfCovered = false;
			}
			else
			{
				base.ScreenManager.AddScreen(this.m_levelToLoad, new PlayerIndex?(PlayerIndex.One));
				this.m_levelToLoad.UpdateIfCovered = true;
			}
			base.ScreenManager.AddScreen(this, new PlayerIndex?(PlayerIndex.One));
			this.AddFinalTransition();
		}
		public void AddFinalTransition()
		{
			if (this.m_screenTypeToLoad == 27)
			{
				this.BackBufferOpacity = 1f;
				Tween.To(this, 2f, new Easing(Tween.EaseNone), new string[]
				{
					"BackBufferOpacity",
					"0"
				});
				Tween.AddEndHandlerToLastTween(base.ScreenManager, "RemoveScreen", new object[]
				{
					this,
					true
				});
				return;
			}
			if (!this.m_wipeTransition)
			{
				SoundManager.PlaySound("GateRise");
				Tween.To(this.m_gateSprite, 1f, new Easing(Tween.EaseNone), new string[]
				{
					"Y",
					(-this.m_gateSprite.Height).ToString()
				});
				Tween.AddEndHandlerToLastTween(base.ScreenManager, "RemoveScreen", new object[]
				{
					this,
					true
				});
				return;
			}
			this.m_blackTransitionOut.Y = -500f;
			Tween.By(this.m_blackTransitionIn, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				(-this.m_blackTransitionIn.Bounds.Width).ToString()
			});
			Tween.By(this.m_blackScreen, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				(-this.m_blackTransitionIn.Bounds.Width).ToString()
			});
			Tween.By(this.m_blackTransitionOut, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				(-this.m_blackTransitionIn.Bounds.Width).ToString()
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "RemoveScreen", new object[]
			{
				this,
				true
			});
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (this.m_screenTypeToLoad != 27)
			{
				if (!this.m_wipeTransition)
				{
					this.m_gateSprite.Draw(base.Camera);
					this.m_effectPool.Draw(base.Camera);
					this.m_loadingText.Position = new Vector2(this.m_gateSprite.X + 995f, this.m_gateSprite.Y + 540f);
					this.m_loadingText.Draw(base.Camera);
				}
				else
				{
					this.m_blackTransitionIn.Draw(base.Camera);
					this.m_blackTransitionOut.Draw(base.Camera);
					this.m_blackScreen.Draw(base.Camera);
				}
			}
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.White * this.BackBufferOpacity);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
		{
			this.m_screenShakeMagnitude = magnitude;
			this.m_horizontalShake = horizontalShake;
			this.m_verticalShake = verticalShake;
			this.m_shakeScreen = true;
		}
		public void UpdateShake()
		{
			if (this.m_horizontalShake)
			{
				this.m_gateSprite.X = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
			}
			if (this.m_verticalShake)
			{
				this.m_gateSprite.Y = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
			}
		}
		public void StopScreenShake()
		{
			this.m_shakeScreen = false;
			this.m_gateSprite.X = 0f;
			this.m_gateSprite.Y = 0f;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Loading Screen");
				this.m_loadingText.Dispose();
				this.m_loadingText = null;
				this.m_levelToLoad = null;
				this.m_gateSprite.Dispose();
				this.m_gateSprite = null;
				this.m_effectPool.Dispose();
				this.m_effectPool = null;
				this.m_blackTransitionIn.Dispose();
				this.m_blackTransitionIn = null;
				this.m_blackScreen.Dispose();
				this.m_blackScreen = null;
				this.m_blackTransitionOut.Dispose();
				this.m_blackTransitionOut = null;
				base.Dispose();
			}
		}
	}
}
