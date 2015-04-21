using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class RCScreenManager : ScreenManager
	{
		private GameOverScreen m_gameOverScreen;
		private SkillScreen m_traitScreen;
		private EnchantressScreen m_enchantressScreen;
		private BlacksmithScreen m_blacksmithScreen;
		private GetItemScreen m_getItemScreen;
		private DialogueScreen m_dialogueScreen;
		private MapScreen m_mapScreen;
		private PauseScreen m_pauseScreen;
		private OptionsScreen m_optionsScreen;
		private ProfileCardScreen m_profileCardScreen;
		private CreditsScreen m_creditsScreen;
		private SkillUnlockScreen m_skillUnlockScreen;
		private DiaryEntryScreen m_diaryEntryScreen;
		private DeathDefiedScreen m_deathDefyScreen;
		private DiaryFlashbackScreen m_flashbackScreen;
		private TextScreen m_textScreen;
		private GameOverBossScreen m_gameOverBossScreen;
		private ProfileSelectScreen m_profileSelectScreen;
		private VirtualScreen m_virtualScreen;
		private bool m_isTransitioning;
		private bool m_inventoryVisible;
		private PlayerObj m_player;
		private SpriteObj m_blackTransitionIn;
		private SpriteObj m_blackScreen;
		private SpriteObj m_blackTransitionOut;
		private bool m_isWipeTransitioning;
		public bool InventoryVisible
		{
			get
			{
				return this.m_inventoryVisible;
			}
		}
		public RenderTarget2D RenderTarget
		{
			get
			{
				return this.m_virtualScreen.RenderTarget;
			}
		}
		public DialogueScreen DialogueScreen
		{
			get
			{
				return this.m_dialogueScreen;
			}
		}
		public SkillScreen SkillScreen
		{
			get
			{
				return this.m_traitScreen;
			}
		}
		public PlayerObj Player
		{
			get
			{
				return this.m_player;
			}
		}
		public bool IsTransitioning
		{
			get
			{
				return this.m_isTransitioning;
			}
		}
		public RCScreenManager(Game Game) : base(Game)
		{
		}
		public override void Initialize()
		{
			this.InitializeScreens();
			base.Initialize();
			this.m_virtualScreen = new VirtualScreen(1320, 720, base.Camera.GraphicsDevice);
			base.Game.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
			base.Game.Deactivated += new EventHandler<EventArgs>(this.PauseGame);
			Form form = Control.FromHandle(base.Game.Window.Handle) as Form;
			if (form != null)
			{
				form.MouseCaptureChanged += new EventHandler(this.PauseGame);
			}
			base.Camera.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(this.ReinitializeContent);
		}
		public void PauseGame(object sender, EventArgs e)
		{
			ProceduralLevelScreen proceduralLevelScreen = base.CurrentScreen as ProceduralLevelScreen;
			if (proceduralLevelScreen != null && !(proceduralLevelScreen.CurrentRoom is EndingRoomObj))
			{
				this.DisplayScreen(16, true, null);
			}
		}
		public void ReinitializeContent(object sender, EventArgs e)
		{
			this.m_virtualScreen.ReinitializeRTs(base.Game.GraphicsDevice);
			foreach (DS2DEngine.Screen current in this.m_screenArray)
			{
				current.DisposeRTs();
			}
			foreach (DS2DEngine.Screen current2 in this.m_screenArray)
			{
				current2.ReinitializeRTs();
			}
		}
		public void ReinitializeCamera(GraphicsDevice graphicsDevice)
		{
			this.m_camera.Dispose();
			this.m_camera = new Camera2D(graphicsDevice, EngineEV.ScreenWidth, EngineEV.ScreenHeight);
		}
		public void InitializeScreens()
		{
			this.m_gameOverScreen = new GameOverScreen();
			this.m_traitScreen = new SkillScreen();
			this.m_blacksmithScreen = new BlacksmithScreen();
			this.m_getItemScreen = new GetItemScreen();
			this.m_enchantressScreen = new EnchantressScreen();
			this.m_dialogueScreen = new DialogueScreen();
			this.m_pauseScreen = new PauseScreen();
			this.m_optionsScreen = new OptionsScreen();
			this.m_profileCardScreen = new ProfileCardScreen();
			this.m_creditsScreen = new CreditsScreen();
			this.m_skillUnlockScreen = new SkillUnlockScreen();
			this.m_diaryEntryScreen = new DiaryEntryScreen();
			this.m_deathDefyScreen = new DeathDefiedScreen();
			this.m_textScreen = new TextScreen();
			this.m_flashbackScreen = new DiaryFlashbackScreen();
			this.m_gameOverBossScreen = new GameOverBossScreen();
			this.m_profileSelectScreen = new ProfileSelectScreen();
		}
		public override void LoadContent()
		{
			this.m_gameOverScreen.LoadContent();
			this.m_traitScreen.LoadContent();
			this.m_blacksmithScreen.LoadContent();
			this.m_getItemScreen.LoadContent();
			this.m_enchantressScreen.LoadContent();
			this.m_dialogueScreen.LoadContent();
			this.m_pauseScreen.LoadContent();
			this.m_optionsScreen.LoadContent();
			this.m_profileCardScreen.LoadContent();
			this.m_creditsScreen.LoadContent();
			this.m_skillUnlockScreen.LoadContent();
			this.m_diaryEntryScreen.LoadContent();
			this.m_deathDefyScreen.LoadContent();
			this.m_textScreen.LoadContent();
			this.m_flashbackScreen.LoadContent();
			this.m_gameOverBossScreen.LoadContent();
			this.m_profileSelectScreen.LoadContent();
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
			this.m_blackTransitionIn.X = 0f;
			this.m_blackTransitionIn.X = (float)(1320 - this.m_blackTransitionIn.Bounds.Left);
			this.m_blackScreen.X = this.m_blackTransitionIn.X;
			this.m_blackTransitionOut.X = this.m_blackScreen.X + (float)this.m_blackScreen.Width;
			this.m_blackTransitionIn.Visible = false;
			this.m_blackScreen.Visible = false;
			this.m_blackTransitionOut.Visible = false;
			this.LoadPlayer();
			base.LoadContent();
		}
		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			this.m_virtualScreen.PhysicalResolutionChanged();
			EngineEV.RefreshEngine(base.Camera.GraphicsDevice);
			Console.WriteLine("resolution changed");
		}
		private void LoadPlayer()
		{
			if (this.m_player == null)
			{
				this.m_player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (base.Game as Game).PhysicsManager, null, base.Game as Game);
				this.m_player.Position = new Vector2(200f, 200f);
				this.m_player.Initialize();
			}
		}
		public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null)
		{
			this.LoadPlayer();
			if (pauseOtherScreens)
			{
				DS2DEngine.Screen[] screens = base.GetScreens();
				for (int i = 0; i < screens.Length; i++)
				{
					DS2DEngine.Screen screen = screens[i];
					if (screen == base.CurrentScreen)
					{
						screen.PauseScreen();
						break;
					}
				}
			}
			switch (screenType)
			{
			case 1:
			case 3:
			case 9:
			case 15:
			case 27:
			case 28:
			case 29:
				this.LoadScreen((byte)screenType, true);
				break;
			case 4:
				this.m_optionsScreen.PassInData(objList);
				this.AddScreen(this.m_optionsScreen, null);
				break;
			case 5:
				if (RogueCastle.Game.PlayerStats.LockCastle || !(base.CurrentScreen is ProceduralLevelScreen))
				{
					this.LoadScreen((byte)screenType, true);
				}
				else
				{
					this.LoadScreen((byte)screenType, false);
				}
				break;
			case 6:
				this.AddScreen(this.m_traitScreen, null);
				break;
			case 7:
				this.m_gameOverScreen.PassInData(objList);
				this.AddScreen(this.m_gameOverScreen, null);
				break;
			case 10:
				this.AddScreen(this.m_blacksmithScreen, null);
				this.m_blacksmithScreen.Player = this.m_player;
				break;
			case 11:
				this.AddScreen(this.m_enchantressScreen, null);
				this.m_enchantressScreen.Player = this.m_player;
				break;
			case 12:
				this.m_getItemScreen.PassInData(objList);
				this.AddScreen(this.m_getItemScreen, null);
				break;
			case 13:
				this.AddScreen(this.m_dialogueScreen, null);
				break;
			case 14:
				this.m_mapScreen.SetPlayer(this.m_player);
				this.AddScreen(this.m_mapScreen, null);
				break;
			case 16:
				this.GetLevelScreen().CurrentRoom.DarkenRoom();
				this.AddScreen(this.m_pauseScreen, null);
				break;
			case 17:
				this.AddScreen(this.m_profileCardScreen, null);
				break;
			case 18:
				this.LoadScreen(18, true);
				break;
			case 19:
				this.m_skillUnlockScreen.PassInData(objList);
				this.AddScreen(this.m_skillUnlockScreen, null);
				break;
			case 20:
				this.AddScreen(this.m_diaryEntryScreen, null);
				break;
			case 21:
				this.AddScreen(this.m_deathDefyScreen, null);
				break;
			case 22:
				this.m_textScreen.PassInData(objList);
				this.AddScreen(this.m_textScreen, null);
				break;
			case 23:
				this.LoadScreen(23, true);
				break;
			case 24:
				this.GetLevelScreen().CameraLockedToPlayer = false;
				this.GetLevelScreen().DisableRoomTransitioning = true;
				this.Player.Position = new Vector2(100f, 100f);
				this.LoadScreen(24, true);
				break;
			case 25:
				this.AddScreen(this.m_flashbackScreen, null);
				break;
			case 26:
				this.m_gameOverBossScreen.PassInData(objList);
				this.AddScreen(this.m_gameOverBossScreen, null);
				break;
			case 30:
				this.AddScreen(this.m_profileSelectScreen, null);
				break;
			}
			if (this.m_isWipeTransitioning)
			{
				this.EndWipeTransition();
			}
		}
		public void AddRoomsToMap(List<RoomObj> roomList)
		{
			this.m_mapScreen.AddRooms(roomList);
		}
		public void AddIconsToMap(List<RoomObj> roomList)
		{
			this.m_mapScreen.AddAllIcons(roomList);
		}
		public void RefreshMapScreenChestIcons(RoomObj room)
		{
			this.m_mapScreen.RefreshMapChestIcons(room);
		}
		public void ActivateMapScreenTeleporter()
		{
			this.m_mapScreen.IsTeleporter = true;
		}
		public void HideCurrentScreen()
		{
			this.RemoveScreen(base.CurrentScreen, false);
			ProceduralLevelScreen proceduralLevelScreen = base.CurrentScreen as ProceduralLevelScreen;
			if (proceduralLevelScreen != null)
			{
				proceduralLevelScreen.UnpauseScreen();
			}
			if (this.m_isWipeTransitioning)
			{
				this.EndWipeTransition();
			}
		}
		public void ForceResolutionChangeCheck()
		{
			this.m_virtualScreen.PhysicalResolutionChanged();
			EngineEV.RefreshEngine(base.Game.GraphicsDevice);
		}
		private void LoadScreen(byte screenType, bool wipeTransition)
		{
			foreach (DS2DEngine.Screen current in this.m_screenArray)
			{
				current.DrawIfCovered = true;
				ProceduralLevelScreen proceduralLevelScreen = current as ProceduralLevelScreen;
				if (proceduralLevelScreen != null)
				{
					this.m_player.AttachLevel(proceduralLevelScreen);
					proceduralLevelScreen.Player = this.m_player;
					this.AttachMap(proceduralLevelScreen);
				}
			}
			if (this.m_gameOverScreen != null)
			{
				this.InitializeScreens();
				this.LoadContent();
			}
			LoadingScreen screen = new LoadingScreen(screenType, wipeTransition);
			this.m_isTransitioning = true;
			this.AddScreen(screen, new PlayerIndex?(PlayerIndex.One));
			GC.Collect();
		}
		public override void RemoveScreen(DS2DEngine.Screen screen, bool disposeScreen)
		{
			if (screen is LoadingScreen)
			{
				this.m_isTransitioning = false;
			}
			base.RemoveScreen(screen, disposeScreen);
		}
		public void AttachMap(ProceduralLevelScreen level)
		{
			if (this.m_mapScreen != null)
			{
				this.m_mapScreen.Dispose();
			}
			this.m_mapScreen = new MapScreen(level);
		}
		public override void Update(GameTime gameTime)
		{
			this.m_virtualScreen.Update();
			if (!this.m_isTransitioning)
			{
				base.Update(gameTime);
				return;
			}
			base.Camera.GameTime = gameTime;
			if (base.CurrentScreen != null)
			{
				base.CurrentScreen.Update(gameTime);
				base.CurrentScreen.HandleInput();
			}
		}
		public override void Draw(GameTime gameTime)
		{
			this.m_virtualScreen.BeginCapture();
			base.Camera.GraphicsDevice.Clear(Color.Black);
			if (base.Camera.GameTime == null)
			{
				base.Camera.GameTime = gameTime;
			}
			base.Draw(gameTime);
			this.m_virtualScreen.EndCapture();
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			this.m_virtualScreen.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_blackTransitionIn.Draw(base.Camera);
			this.m_blackTransitionOut.Draw(base.Camera);
			this.m_blackScreen.Draw(base.Camera);
			base.Camera.End();
		}
		public void StartWipeTransition()
		{
			this.m_isWipeTransitioning = true;
			this.m_blackTransitionIn.Visible = true;
			this.m_blackScreen.Visible = true;
			this.m_blackTransitionOut.Visible = true;
			this.m_blackTransitionIn.X = 0f;
			this.m_blackTransitionOut.Y = -500f;
			this.m_blackTransitionIn.X = (float)(1320 - this.m_blackTransitionIn.Bounds.Left);
			this.m_blackScreen.X = this.m_blackTransitionIn.X;
			this.m_blackTransitionOut.X = this.m_blackScreen.X + (float)this.m_blackScreen.Width;
			Tween.By(this.m_blackTransitionIn, 0.15f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(-this.m_blackTransitionIn.X).ToString()
			});
			Tween.By(this.m_blackScreen, 0.15f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(-this.m_blackTransitionIn.X).ToString()
			});
			Tween.By(this.m_blackTransitionOut, 0.15f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(-this.m_blackTransitionIn.X).ToString()
			});
		}
		public void EndWipeTransition()
		{
			this.m_isWipeTransitioning = false;
			this.m_blackTransitionOut.Y = -500f;
			Tween.By(this.m_blackTransitionIn, 0.25f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"-3000"
			});
			Tween.By(this.m_blackScreen, 0.25f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"-3000"
			});
			Tween.By(this.m_blackTransitionOut, 0.25f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"-3000"
			});
		}
		public void UpdatePauseScreenIcons()
		{
			this.m_pauseScreen.UpdatePauseScreenIcons();
		}
		public ProceduralLevelScreen GetLevelScreen()
		{
			DS2DEngine.Screen[] screens = base.GetScreens();
			for (int i = 0; i < screens.Length; i++)
			{
				DS2DEngine.Screen screen = screens[i];
				ProceduralLevelScreen proceduralLevelScreen = screen as ProceduralLevelScreen;
				if (proceduralLevelScreen != null)
				{
					return proceduralLevelScreen;
				}
			}
			return null;
		}
	}
}
