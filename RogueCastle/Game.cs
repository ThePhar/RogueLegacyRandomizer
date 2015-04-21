using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Tweener;
namespace RogueCastle
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		public struct SettingStruct
		{
			public int ScreenWidth;
			public int ScreenHeight;
			public bool FullScreen;
			public float MusicVolume;
			public float SFXVolume;
			public bool QuickDrop;
			public bool EnableDirectInput;
			public byte ProfileSlot;
			public bool ReduceQuality;
			public bool EnableSteamCloud;
		}
		public static Texture2D GenericTexture;
		public static Effect MaskEffect;
		public static Effect BWMaskEffect;
		public static Effect ShadowEffect;
		public static Effect ParallaxEffect;
		public static Effect RippleEffect;
		public static GaussianBlur GaussianBlur;
		public static Effect HSVEffect;
		public static Effect InvertShader;
		public static Effect ColourSwapShader;
		public static AreaStruct[] Area1List;
		public GraphicsDeviceManager graphics;
		private SaveGameManager m_saveGameManager;
		private PhysicsManager m_physicsManager;
		public static EquipmentSystem EquipmentSystem;
		public static PlayerStats PlayerStats = new PlayerStats();
		public static SpriteFont PixelArtFont;
		public static SpriteFont PixelArtFontBold;
		public static SpriteFont JunicodeFont;
		public static SpriteFont EnemyLevelFont;
		public static SpriteFont PlayerLevelFont;
		public static SpriteFont GoldFont;
		public static SpriteFont HerzogFont;
		public static SpriteFont JunicodeLargeFont;
		public static SpriteFont CinzelFont;
		public static SpriteFont BitFont;
		public static Cue LineageSongCue;
		public static InputMap GlobalInput;
		public static Game.SettingStruct GameConfig;
		public static List<string> NameArray;
		public static List<string> FemaleNameArray;
		private string m_commandLineFilePath = "";
		private GameTime m_forcedGameTime1;
		private GameTime m_forcedGameTime2;
		private float m_frameLimit = 0.025f;
		private bool m_frameLimitSwap;
		public static float TotalGameTime = 0f;
		private static float TotalGameTimeHours = 0f;
		private bool m_contentLoaded;
		private bool m_gameLoaded;
		private WeakReference gcTracker = new WeakReference(new object());
		public static RCScreenManager ScreenManager
		{
			get;
			internal set;
		}
		public static float PlaySessionLength
		{
			get;
			set;
		}
		public PhysicsManager PhysicsManager
		{
			get
			{
				return this.m_physicsManager;
			}
		}
		public ContentManager ContentManager
		{
			get
			{
				return base.Content;
			}
		}
		public SaveGameManager SaveManager
		{
			get
			{
				return this.m_saveGameManager;
			}
		}
		public GraphicsDeviceManager GraphicsDeviceManager
		{
			get
			{
				return this.graphics;
			}
		}
		public Game(string filePath = "")
		{
			if (filePath.Contains("-t"))
			{
				LevelEV.TESTROOM_LEVELTYPE = GameTypes.LevelType.TOWER;
				filePath = filePath.Replace("-t", "");
			}
			else if (filePath.Contains("-d"))
			{
				LevelEV.TESTROOM_LEVELTYPE = GameTypes.LevelType.DUNGEON;
				filePath = filePath.Replace("-d", "");
			}
			else if (filePath.Contains("-g"))
			{
				LevelEV.TESTROOM_LEVELTYPE = GameTypes.LevelType.GARDEN;
				filePath = filePath.Replace("-g", "");
			}
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
			}
			this.m_commandLineFilePath = filePath;
			this.graphics = new GraphicsDeviceManager(this);
			base.Content.RootDirectory = "Content";
			EngineEV.ScreenWidth = 1320;
			EngineEV.ScreenHeight = 720;
			base.Window.Title = "Rogue Legacy";
			Game.ScreenManager = new RCScreenManager(this);
			this.m_saveGameManager = new SaveGameManager(this);
			base.IsFixedTimeStep = false;
			this.graphics.SynchronizeWithVerticalRetrace = !LevelEV.SHOW_FPS;
			base.Window.AllowUserResizing = false;
			if (!LevelEV.ENABLE_OFFSCREEN_CONTROL)
			{
				base.InactiveSleepTime = default(TimeSpan);
			}
			this.m_physicsManager = new PhysicsManager();
			Game.EquipmentSystem = new EquipmentSystem();
			Game.EquipmentSystem.InitializeEquipmentData();
			Game.EquipmentSystem.InitializeAbilityCosts();
			Game.GameConfig = default(Game.SettingStruct);
			Form form = Control.FromHandle(base.Window.Handle) as Form;
			if (form != null)
			{
				form.FormClosing += new FormClosingEventHandler(this.FormClosing);
			}
			this.GraphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(this.ChangeGraphicsSettings);
			SleepUtil.DisableScreensaver();
		}
		protected void ChangeGraphicsSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
			e.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.None;
		}
		protected override void Initialize()
		{
			Tween.Initialize(7000);
			InputManager.Initialize();
			InputManager.InitializeDXManager(base.Services, base.Window);
			Buttons[] buttonList = new Buttons[]
			{
				Buttons.X,
				Buttons.A,
				Buttons.B,
				Buttons.Y,
				Buttons.LeftShoulder,
				Buttons.RightShoulder,
				Buttons.LeftTrigger,
				Buttons.RightTrigger,
				Buttons.Back,
				Buttons.Start,
				Buttons.LeftStick,
				Buttons.RightStick
			};
			InputManager.RemapDXPad(buttonList);
			SpriteLibrary.Init();
			DialogueManager.Initialize();
			if (!LevelEV.CREATE_RETAIL_VERSION)
			{
				DialogueManager.LoadLanguageDocument(base.Content, "Languages\\Text_En");
				DialogueManager.LoadLanguageDocument(base.Content, "Languages\\Diary_En");
			}
			else
			{
				DialogueManager.LoadLanguageBinFile("Content\\Languages\\Text_En.bin");
				DialogueManager.LoadLanguageBinFile("Content\\Languages\\Diary_En.bin");
			}
			DialogueManager.SetLanguage("English");
			this.m_saveGameManager.Initialize();
			this.m_physicsManager.Initialize(Game.ScreenManager.Camera);
			this.m_physicsManager.TerminalVelocity = 2000;
			this.InitializeNameArray();
			this.InitializeFemaleNameArray();
			Game.ScreenManager.Initialize();
			Game.InitializeGlobalInput();
			this.LoadConfig();
			this.InitializeScreenConfig();
			if (LevelEV.SHOW_FPS)
			{
				FrameRateCounter frameRateCounter = new FrameRateCounter(this);
				base.Components.Add(frameRateCounter);
				frameRateCounter.Initialize();
			}
			this.m_forcedGameTime1 = new GameTime(default(TimeSpan), new TimeSpan(0, 0, 0, 0, (int)(this.m_frameLimit * 1000f)));
			this.m_forcedGameTime2 = new GameTime(default(TimeSpan), new TimeSpan(0, 0, 0, 0, (int)(this.m_frameLimit * 1050f)));
			base.Initialize();
			if (!LevelEV.CREATE_RETAIL_VERSION)
			{
				XMLCompiler.CompileEnemies(new List<EnemyEditorData>
				{
					new EnemyEditorData(15),
					new EnemyEditorData(12),
					new EnemyEditorData(8),
					new EnemyEditorData(7),
					new EnemyEditorData(17),
					new EnemyEditorData(13),
					new EnemyEditorData(10),
					new EnemyEditorData(20),
					new EnemyEditorData(19),
					new EnemyEditorData(1),
					new EnemyEditorData(6),
					new EnemyEditorData(2),
					new EnemyEditorData(16),
					new EnemyEditorData(4),
					new EnemyEditorData(14),
					new EnemyEditorData(9),
					new EnemyEditorData(11),
					new EnemyEditorData(5),
					new EnemyEditorData(3),
					new EnemyEditorData(21),
					new EnemyEditorData(22),
					new EnemyEditorData(23),
					new EnemyEditorData(24),
					new EnemyEditorData(25),
					new EnemyEditorData(26),
					new EnemyEditorData(27),
					new EnemyEditorData(28),
					new EnemyEditorData(29),
					new EnemyEditorData(30),
					new EnemyEditorData(31),
					new EnemyEditorData(32),
					new EnemyEditorData(33)
				}, Directory.GetCurrentDirectory());
			}
		}
		public static void InitializeGlobalInput()
		{
			if (Game.GlobalInput != null)
			{
				Game.GlobalInput.ClearAll();
			}
			else
			{
				Game.GlobalInput = new InputMap(PlayerIndex.One, true);
			}
			Game.GlobalInput.AddInput(0, Microsoft.Xna.Framework.Input.Keys.Enter);
			Game.GlobalInput.AddInput(2, Microsoft.Xna.Framework.Input.Keys.Escape);
			Game.GlobalInput.AddInput(6, Microsoft.Xna.Framework.Input.Keys.LeftControl);
			Game.GlobalInput.AddInput(4, Microsoft.Xna.Framework.Input.Keys.Tab);
			Game.GlobalInput.AddInput(7, Microsoft.Xna.Framework.Input.Keys.LeftShift);
			Game.GlobalInput.AddInput(5, Microsoft.Xna.Framework.Input.Keys.Back);
			Game.GlobalInput.AddInput(8, Microsoft.Xna.Framework.Input.Keys.Escape);
			Game.GlobalInput.AddInput(9, Microsoft.Xna.Framework.Input.Keys.Tab);
			Game.GlobalInput.AddInput(10, Microsoft.Xna.Framework.Input.Keys.S);
			Game.GlobalInput.AddInput(11, Microsoft.Xna.Framework.Input.Keys.Space);
			Game.GlobalInput.AddInput(24, Microsoft.Xna.Framework.Input.Keys.W);
			Game.GlobalInput.AddInput(12, Microsoft.Xna.Framework.Input.Keys.D);
			Game.GlobalInput.AddInput(13, Microsoft.Xna.Framework.Input.Keys.A);
			Game.GlobalInput.AddInput(14, Microsoft.Xna.Framework.Input.Keys.Q);
			Game.GlobalInput.AddInput(15, Microsoft.Xna.Framework.Input.Keys.E);
			Game.GlobalInput.AddInput(16, Microsoft.Xna.Framework.Input.Keys.I);
			Game.GlobalInput.AddInput(17, Microsoft.Xna.Framework.Input.Keys.Up);
			Game.GlobalInput.AddInput(18, Microsoft.Xna.Framework.Input.Keys.K);
			Game.GlobalInput.AddInput(19, Microsoft.Xna.Framework.Input.Keys.Down);
			Game.GlobalInput.AddInput(20, Microsoft.Xna.Framework.Input.Keys.J);
			Game.GlobalInput.AddInput(21, Microsoft.Xna.Framework.Input.Keys.Left);
			Game.GlobalInput.AddInput(22, Microsoft.Xna.Framework.Input.Keys.L);
			Game.GlobalInput.AddInput(23, Microsoft.Xna.Framework.Input.Keys.Right);
			Game.GlobalInput.AddInput(0, Buttons.A);
			Game.GlobalInput.AddInput(1, Buttons.Start);
			Game.GlobalInput.AddInput(2, Buttons.B);
			Game.GlobalInput.AddInput(3, Buttons.Back);
			Game.GlobalInput.AddInput(6, Buttons.RightTrigger);
			Game.GlobalInput.AddInput(4, Buttons.Y);
			Game.GlobalInput.AddInput(7, Buttons.X);
			Game.GlobalInput.AddInput(5, Buttons.Back);
			Game.GlobalInput.AddInput(8, Buttons.Start);
			Game.GlobalInput.AddInput(9, Buttons.Back);
			Game.GlobalInput.AddInput(10, Buttons.A);
			Game.GlobalInput.AddInput(12, Buttons.X);
			Game.GlobalInput.AddInput(13, Buttons.Y);
			Game.GlobalInput.AddInput(14, Buttons.LeftTrigger);
			Game.GlobalInput.AddInput(15, Buttons.RightTrigger);
			Game.GlobalInput.AddInput(16, Buttons.DPadUp);
			Game.GlobalInput.AddInput(17, ThumbStick.LeftStick, -90f, 30f);
			Game.GlobalInput.AddInput(18, Buttons.DPadDown);
			Game.GlobalInput.AddInput(19, ThumbStick.LeftStick, 90f, 37f);
			Game.GlobalInput.AddInput(20, Buttons.DPadLeft);
			Game.GlobalInput.AddInput(21, Buttons.LeftThumbstickLeft);
			Game.GlobalInput.AddInput(22, Buttons.DPadRight);
			Game.GlobalInput.AddInput(23, Buttons.LeftThumbstickRight);
			Game.GlobalInput.AddInput(24, Buttons.B);
			Game.GlobalInput.AddInput(25, Microsoft.Xna.Framework.Input.Keys.Escape);
			Game.GlobalInput.AddInput(25, Buttons.Back);
			Game.GlobalInput.AddInput(26, Microsoft.Xna.Framework.Input.Keys.Back);
			Game.GlobalInput.AddInput(26, Buttons.Y);
			Game.GlobalInput.KeyList[1] = Game.GlobalInput.KeyList[12];
			Game.GlobalInput.KeyList[3] = Game.GlobalInput.KeyList[10];
		}
		private void InitializeDefaultConfig()
		{
			Game.GameConfig.FullScreen = false;
			Game.GameConfig.ScreenWidth = 1360;
			Game.GameConfig.ScreenHeight = 768;
			Game.GameConfig.MusicVolume = 1f;
			Game.GameConfig.SFXVolume = 0.8f;
			Game.GameConfig.EnableDirectInput = true;
			InputManager.Deadzone = 10f;
			Game.GameConfig.ProfileSlot = 1;
			Game.GameConfig.EnableSteamCloud = false;
			Game.GameConfig.ReduceQuality = false;
			Game.InitializeGlobalInput();
		}
		protected override void LoadContent()
		{
			if (!this.m_contentLoaded)
			{
				this.m_contentLoaded = true;
				this.LoadAllSpriteFonts();
				this.LoadAllEffects();
				this.LoadAllSpritesheets();
				SoundManager.Initialize("Content\\Audio\\RogueCastleXACTProj.xgs");
				SoundManager.LoadWaveBank("Content\\Audio\\SFXWaveBank.xwb", false);
				SoundManager.LoadWaveBank("Content\\Audio\\MusicWaveBank.xwb", true);
				SoundManager.LoadSoundBank("Content\\Audio\\SFXSoundBank.xsb", false);
				SoundManager.LoadSoundBank("Content\\Audio\\MusicSoundBank.xsb", true);
				SoundManager.GlobalMusicVolume = Game.GameConfig.MusicVolume;
				SoundManager.GlobalSFXVolume = Game.GameConfig.SFXVolume;
				if (InputManager.GamePadIsConnected(PlayerIndex.One))
				{
					InputManager.SetPadType(PlayerIndex.One, PadTypes.GamePad);
				}
				InputManager.UseDirectInput = Game.GameConfig.EnableDirectInput;
				Game.GenericTexture = new Texture2D(base.GraphicsDevice, 1, 1);
				Game.GenericTexture.SetData<Color>(new Color[]
				{
					Color.White
				});
				if (!LevelEV.LOAD_SPLASH_SCREEN)
				{
					LevelBuilder2.Initialize();
					LevelParser.ParseRooms("Map_1x1", base.Content, false);
					LevelParser.ParseRooms("Map_1x2", base.Content, false);
					LevelParser.ParseRooms("Map_1x3", base.Content, false);
					LevelParser.ParseRooms("Map_2x1", base.Content, false);
					LevelParser.ParseRooms("Map_2x2", base.Content, false);
					LevelParser.ParseRooms("Map_2x3", base.Content, false);
					LevelParser.ParseRooms("Map_3x1", base.Content, false);
					LevelParser.ParseRooms("Map_3x2", base.Content, false);
					LevelParser.ParseRooms("Map_Special", base.Content, false);
					LevelParser.ParseRooms("Map_DLC1", base.Content, true);
					LevelBuilder2.IndexRoomList();
				}
				SkillSystem.Initialize();
				AreaStruct areaStruct = new AreaStruct
				{
					Name = "The Grand Entrance",
					LevelType = GameTypes.LevelType.CASTLE,
					TotalRooms = new Vector2(24f, 28f),
					BossInArea = true,
					SecretRooms = new Vector2(1f, 3f),
					BonusRooms = new Vector2(2f, 3f),
					Color = Color.White
				};
				AreaStruct areaStruct2 = new AreaStruct
				{
					LevelType = GameTypes.LevelType.GARDEN,
					TotalRooms = new Vector2(23f, 27f),
					BossInArea = true,
					SecretRooms = new Vector2(1f, 3f),
					BonusRooms = new Vector2(2f, 3f),
					Color = Color.Green
				};
				AreaStruct areaStruct3 = new AreaStruct
				{
					LevelType = GameTypes.LevelType.TOWER,
					TotalRooms = new Vector2(23f, 27f),
					BossInArea = true,
					SecretRooms = new Vector2(1f, 3f),
					BonusRooms = new Vector2(2f, 3f),
					Color = Color.DarkBlue
				};
				AreaStruct areaStruct4 = new AreaStruct
				{
					LevelType = GameTypes.LevelType.DUNGEON,
					TotalRooms = new Vector2(23f, 27f),
					BossInArea = true,
					SecretRooms = new Vector2(1f, 3f),
					BonusRooms = new Vector2(2f, 3f),
					Color = Color.Red
				};
				AreaStruct areaStruct5 = new AreaStruct
				{
					Name = "The Grand Entrance",
					LevelType = GameTypes.LevelType.CASTLE,
					TotalRooms = new Vector2(24f, 27f),
					BossInArea = true,
					SecretRooms = new Vector2(2f, 3f),
					BonusRooms = new Vector2(2f, 3f),
					Color = Color.White
				};
				AreaStruct areaStruct6 = default(AreaStruct);
				areaStruct6.Name = "The Grand Entrance";
				areaStruct6.LevelType = GameTypes.LevelType.GARDEN;
				areaStruct6.TotalRooms = new Vector2(12f, 14f);
				areaStruct6.BossInArea = true;
				areaStruct6.SecretRooms = new Vector2(2f, 3f);
				areaStruct6.BonusRooms = new Vector2(1f, 2f);
				areaStruct6.Color = Color.Green;
				AreaStruct areaStruct7 = default(AreaStruct);
				areaStruct7.Name = "The Grand Entrance";
				areaStruct7.LevelType = GameTypes.LevelType.DUNGEON;
				areaStruct7.TotalRooms = new Vector2(12f, 14f);
				areaStruct7.BossInArea = true;
				areaStruct7.SecretRooms = new Vector2(2f, 3f);
				areaStruct7.BonusRooms = new Vector2(1f, 2f);
				areaStruct7.Color = Color.Red;
				AreaStruct areaStruct8 = default(AreaStruct);
				areaStruct8.Name = "The Grand Entrance";
				areaStruct8.LevelType = GameTypes.LevelType.TOWER;
				areaStruct8.TotalRooms = new Vector2(12f, 14f);
				areaStruct8.BossInArea = true;
				areaStruct8.SecretRooms = new Vector2(2f, 3f);
				areaStruct8.BonusRooms = new Vector2(1f, 2f);
				areaStruct8.Color = Color.DarkBlue;
				Game.Area1List = new AreaStruct[]
				{
					areaStruct,
					areaStruct2,
					areaStruct3,
					areaStruct4
				};
				if (LevelEV.RUN_DEMO_VERSION)
				{
					Game.Area1List = new AreaStruct[]
					{
						areaStruct5
					};
				}
			}
		}
		public void LoadAllSpriteFonts()
		{
			SpriteFontArray.SpriteFontList.Clear();
			Game.PixelArtFont = base.Content.Load<SpriteFont>("Fonts\\Arial12");
			SpriteFontArray.SpriteFontList.Add(Game.PixelArtFont);
			Game.PixelArtFontBold = base.Content.Load<SpriteFont>("Fonts\\PixelArtFontBold");
			SpriteFontArray.SpriteFontList.Add(Game.PixelArtFontBold);
			Game.EnemyLevelFont = base.Content.Load<SpriteFont>("Fonts\\EnemyLevelFont");
			SpriteFontArray.SpriteFontList.Add(Game.EnemyLevelFont);
			Game.EnemyLevelFont.Spacing = -5f;
			Game.PlayerLevelFont = base.Content.Load<SpriteFont>("Fonts\\PlayerLevelFont");
			SpriteFontArray.SpriteFontList.Add(Game.PlayerLevelFont);
			Game.PlayerLevelFont.Spacing = -7f;
			Game.GoldFont = base.Content.Load<SpriteFont>("Fonts\\GoldFont");
			SpriteFontArray.SpriteFontList.Add(Game.GoldFont);
			Game.GoldFont.Spacing = -5f;
			Game.JunicodeFont = base.Content.Load<SpriteFont>("Fonts\\Junicode");
			SpriteFontArray.SpriteFontList.Add(Game.JunicodeFont);
			Game.JunicodeLargeFont = base.Content.Load<SpriteFont>("Fonts\\JunicodeLarge");
			SpriteFontArray.SpriteFontList.Add(Game.JunicodeLargeFont);
			Game.JunicodeLargeFont.Spacing = -1f;
			Game.HerzogFont = base.Content.Load<SpriteFont>("Fonts\\HerzogVonGraf24");
			SpriteFontArray.SpriteFontList.Add(Game.HerzogFont);
			Game.CinzelFont = base.Content.Load<SpriteFont>("Fonts\\CinzelFont");
			SpriteFontArray.SpriteFontList.Add(Game.CinzelFont);
			Game.BitFont = base.Content.Load<SpriteFont>("Fonts\\BitFont");
			SpriteFontArray.SpriteFontList.Add(Game.BitFont);
		}
		public void LoadAllSpritesheets()
		{
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\blacksmithUISpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\enemyFinal2Spritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\enemyFinalSpritesheetBig", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\miscSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\traitsCastleSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\castleTerrainSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\playerSpritesheetBig", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\titleScreen3Spritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\mapSpritesheetBig", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\startingRoomSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\towerTerrainSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\dungeonTerrainSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\profileCardSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\portraitSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\gardenTerrainSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\parallaxBGSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\getItemScreenSpritesheet", false);
			SpriteLibrary.LoadSpritesheet(base.Content, "GameSpritesheets\\neoTerrainSpritesheet", false);
		}
		public void LoadAllEffects()
		{
			Game.MaskEffect = base.Content.Load<Effect>("Shaders\\AlphaMaskShader");
			Game.ShadowEffect = base.Content.Load<Effect>("Shaders\\ShadowFX");
			Game.ParallaxEffect = base.Content.Load<Effect>("Shaders\\ParallaxFX");
			Game.HSVEffect = base.Content.Load<Effect>("Shaders\\HSVShader");
			Game.InvertShader = base.Content.Load<Effect>("Shaders\\InvertShader");
			Game.ColourSwapShader = base.Content.Load<Effect>("Shaders\\ColourSwapShader");
			Game.RippleEffect = base.Content.Load<Effect>("Shaders\\Shockwave");
			Game.RippleEffect.Parameters["mag"].SetValue(2);
			Game.GaussianBlur = new GaussianBlur(this, 1320, 720);
			Game.GaussianBlur.Amount = 2f;
			Game.GaussianBlur.Radius = 7;
			Game.GaussianBlur.ComputeKernel();
			Game.GaussianBlur.ComputeOffsets();
			Game.GaussianBlur.InvertMask = true;
			Game.BWMaskEffect = base.Content.Load<Effect>("Shaders\\BWMaskShader");
		}
		protected override void UnloadContent()
		{
		}
		protected override void Update(GameTime gameTime)
		{
			if (!this.m_gameLoaded)
			{
				this.m_gameLoaded = true;
				if (LevelEV.DELETE_SAVEFILE)
				{
					this.SaveManager.ClearAllFileTypes(true);
					this.SaveManager.ClearAllFileTypes(false);
				}
				if (LevelEV.LOAD_SPLASH_SCREEN)
				{
					if (LevelEV.RUN_DEMO_VERSION)
					{
						Game.ScreenManager.DisplayScreen(28, true, null);
					}
					else
					{
						Game.ScreenManager.DisplayScreen(1, true, null);
					}
				}
				else if (!LevelEV.LOAD_TITLE_SCREEN)
				{
					if (LevelEV.RUN_TESTROOM)
					{
						Game.ScreenManager.DisplayScreen(5, true, null);
					}
					else if (LevelEV.RUN_TUTORIAL)
					{
						Game.ScreenManager.DisplayScreen(23, true, null);
					}
					else
					{
						Game.ScreenManager.DisplayScreen(15, true, null);
					}
				}
				else
				{
					Game.ScreenManager.DisplayScreen(3, true, null);
				}
			}
			Game.TotalGameTime = (float)gameTime.TotalGameTime.TotalSeconds;
			Game.TotalGameTimeHours = (float)gameTime.TotalGameTime.TotalHours;
			GameTime gameTime2 = gameTime;
			if (gameTime.ElapsedGameTime.TotalSeconds > (double)this.m_frameLimit)
			{
				if (!this.m_frameLimitSwap)
				{
					this.m_frameLimitSwap = true;
					gameTime2 = this.m_forcedGameTime1;
				}
				else
				{
					this.m_frameLimitSwap = false;
					gameTime2 = this.m_forcedGameTime2;
				}
			}
			SoundManager.Update(gameTime2);
			if (base.IsActive || (!base.IsActive && LevelEV.ENABLE_OFFSCREEN_CONTROL))
			{
				InputManager.Update(gameTime2);
			}
			Tween.Update(gameTime2);
			Game.ScreenManager.Update(gameTime2);
			SoundManager.Update3DSounds();
			base.Update(gameTime);
		}
		protected override void Draw(GameTime gameTime)
		{
			base.GraphicsDevice.Clear(Color.Black);
			Game.ScreenManager.Draw(gameTime);
			base.Draw(gameTime);
		}
		private void InitializeNameArray()
		{
			Game.NameArray = new List<string>();
			using (StreamReader streamReader = new StreamReader("Content\\HeroNames.txt"))
			{
				SpriteFont spriteFont = base.Content.Load<SpriteFont>("Fonts\\Junicode");
				SpriteFontArray.SpriteFontList.Add(spriteFont);
				TextObj textObj = new TextObj(spriteFont);
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					bool flag = false;
					try
					{
						textObj.Text = text;
					}
					catch
					{
						flag = true;
					}
					if (!text.Contains("//") && !flag)
					{
						Game.NameArray.Add(text);
					}
				}
				if (Game.NameArray.Count < 1)
				{
					Game.NameArray.Add("Lee");
					Game.NameArray.Add("Charles");
					Game.NameArray.Add("Lancelot");
				}
				textObj.Dispose();
				SpriteFontArray.SpriteFontList.Clear();
			}
		}
		private void InitializeFemaleNameArray()
		{
			Game.FemaleNameArray = new List<string>();
			using (StreamReader streamReader = new StreamReader("Content\\HeroineNames.txt"))
			{
				SpriteFont spriteFont = base.Content.Load<SpriteFont>("Fonts\\Junicode");
				SpriteFontArray.SpriteFontList.Add(spriteFont);
				TextObj textObj = new TextObj(spriteFont);
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					bool flag = false;
					try
					{
						textObj.Text = text;
					}
					catch
					{
						flag = true;
					}
					if (!text.Contains("//") && !flag)
					{
						Game.FemaleNameArray.Add(text);
					}
				}
				if (Game.FemaleNameArray.Count < 1)
				{
					Game.FemaleNameArray.Add("Jenny");
					Game.FemaleNameArray.Add("Shanoa");
					Game.FemaleNameArray.Add("Chun Li");
				}
				textObj.Dispose();
				SpriteFontArray.SpriteFontList.Clear();
			}
		}
		public void SaveOnExit()
		{
			if (!(Game.ScreenManager.CurrentScreen is CDGSplashScreen) && !(Game.ScreenManager.CurrentScreen is DemoStartScreen))
			{
				this.UpdatePlaySessionLength();
				ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
				if (levelScreen != null && (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom || levelScreen.CurrentRoom is CarnivalShoot2BonusRoom))
				{
					if (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom)
					{
						(levelScreen.CurrentRoom as CarnivalShoot1BonusRoom).UnequipPlayer();
					}
					if (levelScreen.CurrentRoom is CarnivalShoot2BonusRoom)
					{
						(levelScreen.CurrentRoom as CarnivalShoot2BonusRoom).UnequipPlayer();
					}
				}
				if (levelScreen != null)
				{
					ChallengeBossRoomObj challengeBossRoomObj = levelScreen.CurrentRoom as ChallengeBossRoomObj;
					if (challengeBossRoomObj != null)
					{
						challengeBossRoomObj.LoadPlayerData();
						this.SaveManager.LoadFiles(levelScreen, new SaveType[]
						{
							SaveType.UpgradeData
						});
						levelScreen.Player.CurrentHealth = challengeBossRoomObj.StoredHP;
						levelScreen.Player.CurrentMana = challengeBossRoomObj.StoredMP;
					}
				}
				if (Game.ScreenManager.CurrentScreen is GameOverScreen)
				{
					Game.PlayerStats.Traits = Vector2.Zero;
				}
				if (this.SaveManager.FileExists(SaveType.PlayerData))
				{
					this.SaveManager.SaveFiles(new SaveType[]
					{
						SaveType.PlayerData,
						SaveType.UpgradeData
					});
					if (Game.PlayerStats.TutorialComplete && levelScreen != null && levelScreen.CurrentRoom.Name != "Start" && levelScreen.CurrentRoom.Name != "Ending" && levelScreen.CurrentRoom.Name != "Tutorial")
					{
						this.SaveManager.SaveFiles(new SaveType[]
						{
							SaveType.MapData
						});
					}
				}
			}
		}
		public void FormClosing(object sender, FormClosingEventArgs args)
		{
			if (args.CloseReason == CloseReason.UserClosing)
			{
				this.SaveOnExit();
			}
		}
		public void UpdatePlaySessionLength()
		{
			Game.PlaySessionLength = Game.TotalGameTimeHours;
		}
		public List<Vector2> GetSupportedResolutions()
		{
			List<Vector2> list = new List<Vector2>();
			foreach (DisplayMode current in base.GraphicsDevice.Adapter.SupportedDisplayModes)
			{
				if (current.Width < 2000 && current.Height < 2000)
				{
					Vector2 item = new Vector2((float)current.Width, (float)current.Height);
					if (!list.Contains(item))
					{
						list.Add(new Vector2((float)current.Width, (float)current.Height));
					}
				}
			}
			return list;
		}
		public void SaveConfig()
		{
			Console.WriteLine("Saving Config file");
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string path = Path.Combine(folderPath, "Rogue Legacy");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string path2 = Path.Combine(folderPath, "Rogue Legacy", "GameConfig.ini");
			using (StreamWriter streamWriter = new StreamWriter(path2, false))
			{
				streamWriter.WriteLine("[Screen Resolution]");
				streamWriter.WriteLine("ScreenWidth=" + Game.GameConfig.ScreenWidth);
				streamWriter.WriteLine("ScreenHeight=" + Game.GameConfig.ScreenHeight);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Fullscreen]");
				streamWriter.WriteLine("Fullscreen=" + Game.GameConfig.FullScreen);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[QuickDrop]");
				streamWriter.WriteLine("QuickDrop=" + Game.GameConfig.QuickDrop);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Game Volume]");
				streamWriter.WriteLine("MusicVol=" + string.Format("{0:F2}", Game.GameConfig.MusicVolume));
				streamWriter.WriteLine("SFXVol=" + string.Format("{0:F2}", Game.GameConfig.SFXVolume));
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Joystick Dead Zone]");
				streamWriter.WriteLine("DeadZone=" + InputManager.Deadzone);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Enable DirectInput Gamepads]");
				streamWriter.WriteLine("EnableDirectInput=" + Game.GameConfig.EnableDirectInput);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Reduce Shader Quality]");
				streamWriter.WriteLine("ReduceQuality=" + Game.GameConfig.ReduceQuality);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Profile]");
				streamWriter.WriteLine("Slot=" + Game.GameConfig.ProfileSlot);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Keyboard Config]");
				streamWriter.WriteLine("KeyUP=" + Game.GlobalInput.KeyList[16]);
				streamWriter.WriteLine("KeyDOWN=" + Game.GlobalInput.KeyList[18]);
				streamWriter.WriteLine("KeyLEFT=" + Game.GlobalInput.KeyList[20]);
				streamWriter.WriteLine("KeyRIGHT=" + Game.GlobalInput.KeyList[22]);
				streamWriter.WriteLine("KeyATTACK=" + Game.GlobalInput.KeyList[12]);
				streamWriter.WriteLine("KeyJUMP=" + Game.GlobalInput.KeyList[10]);
				streamWriter.WriteLine("KeySPECIAL=" + Game.GlobalInput.KeyList[13]);
				streamWriter.WriteLine("KeyDASHLEFT=" + Game.GlobalInput.KeyList[14]);
				streamWriter.WriteLine("KeyDASHRIGHT=" + Game.GlobalInput.KeyList[15]);
				streamWriter.WriteLine("KeySPELL1=" + Game.GlobalInput.KeyList[24]);
				streamWriter.WriteLine();
				streamWriter.WriteLine("[Gamepad Config]");
				streamWriter.WriteLine("ButtonUP=" + Game.GlobalInput.ButtonList[16]);
				streamWriter.WriteLine("ButtonDOWN=" + Game.GlobalInput.ButtonList[18]);
				streamWriter.WriteLine("ButtonLEFT=" + Game.GlobalInput.ButtonList[20]);
				streamWriter.WriteLine("ButtonRIGHT=" + Game.GlobalInput.ButtonList[22]);
				streamWriter.WriteLine("ButtonATTACK=" + Game.GlobalInput.ButtonList[12]);
				streamWriter.WriteLine("ButtonJUMP=" + Game.GlobalInput.ButtonList[10]);
				streamWriter.WriteLine("ButtonSPECIAL=" + Game.GlobalInput.ButtonList[13]);
				streamWriter.WriteLine("ButtonDASHLEFT=" + Game.GlobalInput.ButtonList[14]);
				streamWriter.WriteLine("ButtonDASHRIGHT=" + Game.GlobalInput.ButtonList[15]);
				streamWriter.WriteLine("ButtonSPELL1=" + Game.GlobalInput.ButtonList[24]);
				streamWriter.Close();
			}
		}
		public void LoadConfig()
		{
			Console.WriteLine("Loading Config file");
			this.InitializeDefaultConfig();
			try
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string path = Path.Combine(folderPath, "Rogue Legacy", "GameConfig.ini");
				using (StreamReader streamReader = new StreamReader(path))
				{
					CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
					cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						int num = text.IndexOf("=");
						if (num != -1)
						{
							string text2 = text.Substring(0, num);
							string text3 = text.Substring(num + 1);
							string key;
							switch (key = text2)
							{
							case "ScreenWidth":
								Game.GameConfig.ScreenWidth = int.Parse(text3, NumberStyles.Any, cultureInfo);
								break;
							case "ScreenHeight":
								Game.GameConfig.ScreenHeight = int.Parse(text3, NumberStyles.Any, cultureInfo);
								break;
							case "Fullscreen":
								Game.GameConfig.FullScreen = bool.Parse(text3);
								break;
							case "QuickDrop":
								Game.GameConfig.QuickDrop = bool.Parse(text3);
								break;
							case "MusicVol":
								Game.GameConfig.MusicVolume = float.Parse(text3);
								break;
							case "SFXVol":
								Game.GameConfig.SFXVolume = float.Parse(text3);
								break;
							case "DeadZone":
								InputManager.Deadzone = (float)int.Parse(text3, NumberStyles.Any, cultureInfo);
								break;
							case "EnableDirectInput":
								Game.GameConfig.EnableDirectInput = bool.Parse(text3);
								break;
							case "ReduceQuality":
								Game.GameConfig.ReduceQuality = bool.Parse(text3);
								LevelEV.SAVE_FRAMES = Game.GameConfig.ReduceQuality;
								break;
							case "EnableSteamCloud":
								Game.GameConfig.EnableSteamCloud = bool.Parse(text3);
								break;
							case "Slot":
								Game.GameConfig.ProfileSlot = byte.Parse(text3, NumberStyles.Any, cultureInfo);
								break;
							case "KeyUP":
								Game.GlobalInput.KeyList[16] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyDOWN":
								Game.GlobalInput.KeyList[18] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyLEFT":
								Game.GlobalInput.KeyList[20] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyRIGHT":
								Game.GlobalInput.KeyList[22] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyATTACK":
								Game.GlobalInput.KeyList[12] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyJUMP":
								Game.GlobalInput.KeyList[10] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeySPECIAL":
								Game.GlobalInput.KeyList[13] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyDASHLEFT":
								Game.GlobalInput.KeyList[14] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeyDASHRIGHT":
								Game.GlobalInput.KeyList[15] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "KeySPELL1":
								Game.GlobalInput.KeyList[24] = (Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), text3);
								break;
							case "ButtonUP":
								Game.GlobalInput.ButtonList[16] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonDOWN":
								Game.GlobalInput.ButtonList[18] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonLEFT":
								Game.GlobalInput.ButtonList[20] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonRIGHT":
								Game.GlobalInput.ButtonList[22] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonATTACK":
								Game.GlobalInput.ButtonList[12] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonJUMP":
								Game.GlobalInput.ButtonList[10] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonSPECIAL":
								Game.GlobalInput.ButtonList[13] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonDASHLEFT":
								Game.GlobalInput.ButtonList[14] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonDASHRIGHT":
								Game.GlobalInput.ButtonList[15] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							case "ButtonSPELL1":
								Game.GlobalInput.ButtonList[24] = (Buttons)Enum.Parse(typeof(Buttons), text3);
								break;
							}
						}
					}
					Game.GlobalInput.KeyList[1] = Game.GlobalInput.KeyList[12];
					Game.GlobalInput.KeyList[3] = Game.GlobalInput.KeyList[10];
					streamReader.Close();
					if (Game.GameConfig.ScreenHeight <= 0 || Game.GameConfig.ScreenWidth <= 0)
					{
						throw new Exception("Blank Config File");
					}
				}
			}
			catch
			{
				Console.WriteLine("Config File Not Found. Creating Default Config File.");
				this.InitializeDefaultConfig();
				this.SaveConfig();
			}
		}
		public void InitializeScreenConfig()
		{
			this.graphics.PreferredBackBufferWidth = Game.GameConfig.ScreenWidth;
			this.graphics.PreferredBackBufferHeight = Game.GameConfig.ScreenHeight;
			if ((this.graphics.IsFullScreen && !Game.GameConfig.FullScreen) || (!this.graphics.IsFullScreen && Game.GameConfig.FullScreen))
			{
				this.graphics.ToggleFullScreen();
			}
			else
			{
				this.graphics.ApplyChanges();
			}
			Game.ScreenManager.ForceResolutionChangeCheck();
		}
		public static float GetTotalGameTimeHours()
		{
			return Game.TotalGameTimeHours;
		}
	}
}
