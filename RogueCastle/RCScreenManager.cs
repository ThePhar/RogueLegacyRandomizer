using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;
using Screen = DS2DEngine.Screen;

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
				return m_inventoryVisible;
			}
		}
		public RenderTarget2D RenderTarget
		{
			get
			{
				return m_virtualScreen.RenderTarget;
			}
		}
		public DialogueScreen DialogueScreen
		{
			get
			{
				return m_dialogueScreen;
			}
		}
		public SkillScreen SkillScreen
		{
			get
			{
				return m_traitScreen;
			}
		}
		public PlayerObj Player
		{
			get
			{
				return m_player;
			}
		}
		public bool IsTransitioning
		{
			get
			{
				return m_isTransitioning;
			}
		}
		public RCScreenManager(Game Game) : base(Game)
		{
		}
		public override void Initialize()
		{
			InitializeScreens();
			base.Initialize();
			m_virtualScreen = new VirtualScreen(1320, 720, Camera.GraphicsDevice);
			Game.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
			Game.Deactivated += new EventHandler<EventArgs>(PauseGame);
			Form form = Control.FromHandle(Game.Window.Handle) as Form;
			if (form != null)
			{
				form.MouseCaptureChanged += new EventHandler(PauseGame);
			}
			Camera.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(ReinitializeContent);
		}
		public void PauseGame(object sender, EventArgs e)
		{
			ProceduralLevelScreen proceduralLevelScreen = CurrentScreen as ProceduralLevelScreen;
			if (proceduralLevelScreen != null && !(proceduralLevelScreen.CurrentRoom is EndingRoomObj))
			{
				DisplayScreen(16, true, null);
			}
		}
		public void ReinitializeContent(object sender, EventArgs e)
		{
			m_virtualScreen.ReinitializeRTs(Game.GraphicsDevice);
			foreach (Screen current in m_screenArray)
			{
				current.DisposeRTs();
			}
			foreach (Screen current2 in m_screenArray)
			{
				current2.ReinitializeRTs();
			}
		}
		public void ReinitializeCamera(GraphicsDevice graphicsDevice)
		{
			m_camera.Dispose();
			m_camera = new Camera2D(graphicsDevice, EngineEV.ScreenWidth, EngineEV.ScreenHeight);
		}
		public void InitializeScreens()
		{
			m_gameOverScreen = new GameOverScreen();
			m_traitScreen = new SkillScreen();
			m_blacksmithScreen = new BlacksmithScreen();
			m_getItemScreen = new GetItemScreen();
			m_enchantressScreen = new EnchantressScreen();
			m_dialogueScreen = new DialogueScreen();
			m_pauseScreen = new PauseScreen();
			m_optionsScreen = new OptionsScreen();
			m_profileCardScreen = new ProfileCardScreen();
			m_creditsScreen = new CreditsScreen();
			m_skillUnlockScreen = new SkillUnlockScreen();
			m_diaryEntryScreen = new DiaryEntryScreen();
			m_deathDefyScreen = new DeathDefiedScreen();
			m_textScreen = new TextScreen();
			m_flashbackScreen = new DiaryFlashbackScreen();
			m_gameOverBossScreen = new GameOverBossScreen();
			m_profileSelectScreen = new ProfileSelectScreen();
		}
		public override void LoadContent()
		{
			m_gameOverScreen.LoadContent();
			m_traitScreen.LoadContent();
			m_blacksmithScreen.LoadContent();
			m_getItemScreen.LoadContent();
			m_enchantressScreen.LoadContent();
			m_dialogueScreen.LoadContent();
			m_pauseScreen.LoadContent();
			m_optionsScreen.LoadContent();
			m_profileCardScreen.LoadContent();
			m_creditsScreen.LoadContent();
			m_skillUnlockScreen.LoadContent();
			m_diaryEntryScreen.LoadContent();
			m_deathDefyScreen.LoadContent();
			m_textScreen.LoadContent();
			m_flashbackScreen.LoadContent();
			m_gameOverBossScreen.LoadContent();
			m_profileSelectScreen.LoadContent();
			m_blackTransitionIn = new SpriteObj("Blank_Sprite");
			m_blackTransitionIn.Rotation = 15f;
			m_blackTransitionIn.Scale = new Vector2(1320 / m_blackTransitionIn.Width, 2000 / m_blackTransitionIn.Height);
			m_blackTransitionIn.TextureColor = Color.Black;
			m_blackTransitionIn.ForceDraw = true;
			m_blackScreen = new SpriteObj("Blank_Sprite");
			m_blackScreen.Scale = new Vector2(1320 / m_blackScreen.Width, 720 / m_blackScreen.Height);
			m_blackScreen.TextureColor = Color.Black;
			m_blackScreen.ForceDraw = true;
			m_blackTransitionOut = new SpriteObj("Blank_Sprite");
			m_blackTransitionOut.Rotation = 15f;
			m_blackTransitionOut.Scale = new Vector2(1320 / m_blackTransitionOut.Width, 2000 / m_blackTransitionOut.Height);
			m_blackTransitionOut.TextureColor = Color.Black;
			m_blackTransitionOut.ForceDraw = true;
			m_blackTransitionIn.X = 0f;
			m_blackTransitionIn.X = 1320 - m_blackTransitionIn.Bounds.Left;
			m_blackScreen.X = m_blackTransitionIn.X;
			m_blackTransitionOut.X = m_blackScreen.X + m_blackScreen.Width;
			m_blackTransitionIn.Visible = false;
			m_blackScreen.Visible = false;
			m_blackTransitionOut.Visible = false;
			LoadPlayer();
			base.LoadContent();
		}
		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			m_virtualScreen.PhysicalResolutionChanged();
			EngineEV.RefreshEngine(Camera.GraphicsDevice);
			Console.WriteLine("resolution changed");
		}
		private void LoadPlayer()
		{
			if (m_player == null)
			{
				m_player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (Game as Game).PhysicsManager, null, Game as Game);
				m_player.Position = new Vector2(200f, 200f);
				m_player.Initialize();
			}
		}
		public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null)
		{
			LoadPlayer();
			if (pauseOtherScreens)
			{
				Screen[] screens = GetScreens();
				for (int i = 0; i < screens.Length; i++)
				{
					Screen screen = screens[i];
					if (screen == CurrentScreen)
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
				LoadScreen((byte)screenType, true);
				break;
			case 4:
				m_optionsScreen.PassInData(objList);
				AddScreen(m_optionsScreen, null);
				break;
			case 5:
				if (RogueCastle.Game.PlayerStats.LockCastle || !(CurrentScreen is ProceduralLevelScreen))
				{
					LoadScreen((byte)screenType, true);
				}
				else
				{
					LoadScreen((byte)screenType, false);
				}
				break;
			case 6:
				AddScreen(m_traitScreen, null);
				break;
			case 7:
				m_gameOverScreen.PassInData(objList);
				AddScreen(m_gameOverScreen, null);
				break;
			case 10:
				AddScreen(m_blacksmithScreen, null);
				m_blacksmithScreen.Player = m_player;
				break;
			case 11:
				AddScreen(m_enchantressScreen, null);
				m_enchantressScreen.Player = m_player;
				break;
			case 12:
				m_getItemScreen.PassInData(objList);
				AddScreen(m_getItemScreen, null);
				break;
			case 13:
				AddScreen(m_dialogueScreen, null);
				break;
			case 14:
				m_mapScreen.SetPlayer(m_player);
				AddScreen(m_mapScreen, null);
				break;
			case 16:
				GetLevelScreen().CurrentRoom.DarkenRoom();
				AddScreen(m_pauseScreen, null);
				break;
			case 17:
				AddScreen(m_profileCardScreen, null);
				break;
			case 18:
				LoadScreen(18, true);
				break;
			case 19:
				m_skillUnlockScreen.PassInData(objList);
				AddScreen(m_skillUnlockScreen, null);
				break;
			case 20:
				AddScreen(m_diaryEntryScreen, null);
				break;
			case 21:
				AddScreen(m_deathDefyScreen, null);
				break;
			case 22:
				m_textScreen.PassInData(objList);
				AddScreen(m_textScreen, null);
				break;
			case 23:
				LoadScreen(23, true);
				break;
			case 24:
				GetLevelScreen().CameraLockedToPlayer = false;
				GetLevelScreen().DisableRoomTransitioning = true;
				Player.Position = new Vector2(100f, 100f);
				LoadScreen(24, true);
				break;
			case 25:
				AddScreen(m_flashbackScreen, null);
				break;
			case 26:
				m_gameOverBossScreen.PassInData(objList);
				AddScreen(m_gameOverBossScreen, null);
				break;
			case 30:
				AddScreen(m_profileSelectScreen, null);
				break;
			}
			if (m_isWipeTransitioning)
			{
				EndWipeTransition();
			}
		}
		public void AddRoomsToMap(List<RoomObj> roomList)
		{
			m_mapScreen.AddRooms(roomList);
		}
		public void AddIconsToMap(List<RoomObj> roomList)
		{
			m_mapScreen.AddAllIcons(roomList);
		}
		public void RefreshMapScreenChestIcons(RoomObj room)
		{
			m_mapScreen.RefreshMapChestIcons(room);
		}
		public void ActivateMapScreenTeleporter()
		{
			m_mapScreen.IsTeleporter = true;
		}
		public void HideCurrentScreen()
		{
			RemoveScreen(CurrentScreen, false);
			ProceduralLevelScreen proceduralLevelScreen = CurrentScreen as ProceduralLevelScreen;
			if (proceduralLevelScreen != null)
			{
				proceduralLevelScreen.UnpauseScreen();
			}
			if (m_isWipeTransitioning)
			{
				EndWipeTransition();
			}
		}
		public void ForceResolutionChangeCheck()
		{
			m_virtualScreen.PhysicalResolutionChanged();
			EngineEV.RefreshEngine(Game.GraphicsDevice);
		}
		private void LoadScreen(byte screenType, bool wipeTransition)
		{
			foreach (Screen current in m_screenArray)
			{
				current.DrawIfCovered = true;
				ProceduralLevelScreen proceduralLevelScreen = current as ProceduralLevelScreen;
				if (proceduralLevelScreen != null)
				{
					m_player.AttachLevel(proceduralLevelScreen);
					proceduralLevelScreen.Player = m_player;
					AttachMap(proceduralLevelScreen);
				}
			}
			if (m_gameOverScreen != null)
			{
				InitializeScreens();
				LoadContent();
			}
			LoadingScreen screen = new LoadingScreen(screenType, wipeTransition);
			m_isTransitioning = true;
			AddScreen(screen, new PlayerIndex?(PlayerIndex.One));
			GC.Collect();
		}
		public override void RemoveScreen(Screen screen, bool disposeScreen)
		{
			if (screen is LoadingScreen)
			{
				m_isTransitioning = false;
			}
			base.RemoveScreen(screen, disposeScreen);
		}
		public void AttachMap(ProceduralLevelScreen level)
		{
			if (m_mapScreen != null)
			{
				m_mapScreen.Dispose();
			}
			m_mapScreen = new MapScreen(level);
		}
		public override void Update(GameTime gameTime)
		{
			m_virtualScreen.Update();
			if (!m_isTransitioning)
			{
				base.Update(gameTime);
				return;
			}
			Camera.GameTime = gameTime;
			if (CurrentScreen != null)
			{
				CurrentScreen.Update(gameTime);
				CurrentScreen.HandleInput();
			}
		}
		public override void Draw(GameTime gameTime)
		{
			m_virtualScreen.BeginCapture();
			Camera.GraphicsDevice.Clear(Color.Black);
			if (Camera.GameTime == null)
			{
				Camera.GameTime = gameTime;
			}
			base.Draw(gameTime);
			m_virtualScreen.EndCapture();
			Camera.GraphicsDevice.Clear(Color.Black);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			m_virtualScreen.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			m_blackTransitionIn.Draw(Camera);
			m_blackTransitionOut.Draw(Camera);
			m_blackScreen.Draw(Camera);
			Camera.End();
		}
		public void StartWipeTransition()
		{
			m_isWipeTransitioning = true;
			m_blackTransitionIn.Visible = true;
			m_blackScreen.Visible = true;
			m_blackTransitionOut.Visible = true;
			m_blackTransitionIn.X = 0f;
			m_blackTransitionOut.Y = -500f;
			m_blackTransitionIn.X = 1320 - m_blackTransitionIn.Bounds.Left;
			m_blackScreen.X = m_blackTransitionIn.X;
			m_blackTransitionOut.X = m_blackScreen.X + m_blackScreen.Width;
			Tween.By(m_blackTransitionIn, 0.15f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(-m_blackTransitionIn.X).ToString()
			});
			Tween.By(m_blackScreen, 0.15f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(-m_blackTransitionIn.X).ToString()
			});
			Tween.By(m_blackTransitionOut, 0.15f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(-m_blackTransitionIn.X).ToString()
			});
		}
		public void EndWipeTransition()
		{
			m_isWipeTransitioning = false;
			m_blackTransitionOut.Y = -500f;
			Tween.By(m_blackTransitionIn, 0.25f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"-3000"
			});
			Tween.By(m_blackScreen, 0.25f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"-3000"
			});
			Tween.By(m_blackTransitionOut, 0.25f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				"-3000"
			});
		}
		public void UpdatePauseScreenIcons()
		{
			m_pauseScreen.UpdatePauseScreenIcons();
		}
		public ProceduralLevelScreen GetLevelScreen()
		{
			Screen[] screens = GetScreens();
			for (int i = 0; i < screens.Length; i++)
			{
				Screen screen = screens[i];
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
