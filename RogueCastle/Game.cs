// 
// RogueLegacyArchipelago - Game.cs
// Last Modified 2021-12-23
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Archipelago.MultiClient.Net;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteSystem;
using Tweener;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RogueCastle
{
    public class Game : Microsoft.Xna.Framework.Game
    {
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
        public static SettingStruct GameConfig;
        public static List<string> NameArray;
        public static List<string> FemaleNameArray;
        public static float TotalGameTime;
        private static float TotalGameTimeHours;
        private readonly float m_frameLimit = 0.025f;
        private WeakReference gcTracker = new WeakReference(new object());
        public GraphicsDeviceManager graphics;
        private string m_commandLineFilePath = "";
        private bool m_contentLoaded;
        private GameTime m_forcedGameTime1;
        private GameTime m_forcedGameTime2;
        private bool m_frameLimitSwap;
        private bool m_gameLoaded;

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
            m_commandLineFilePath = filePath;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            EngineEV.ScreenWidth = 1320;
            EngineEV.ScreenHeight = 720;
            Window.Title = "Rogue Legacy Archipelago";
            ScreenManager = new RCScreenManager(this);
            SaveManager = new SaveGameManager(this);
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = !LevelEV.SHOW_FPS;
            Window.AllowUserResizing = false;
            if (!LevelEV.ENABLE_OFFSCREEN_CONTROL)
            {
                InactiveSleepTime = default(TimeSpan);
            }
            PhysicsManager = new PhysicsManager();
            EquipmentSystem = new EquipmentSystem();
            EquipmentSystem.InitializeEquipmentData();
            EquipmentSystem.InitializeAbilityCosts();
            GameConfig = default(SettingStruct);
            var form = Control.FromHandle(Window.Handle) as Form;
            if (form != null)
            {
                form.FormClosing += FormClosing;
            }
            GraphicsDeviceManager.PreparingDeviceSettings += ChangeGraphicsSettings;
            SleepUtil.DisableScreensaver();
        }

        public ArchipelagoSession ArchSession;

        public static RCScreenManager ScreenManager { get; internal set; }
        public static float PlaySessionLength { get; set; }
        public PhysicsManager PhysicsManager { get; private set; }

        public ContentManager ContentManager
        {
            get { return Content; }
        }

        public SaveGameManager SaveManager { get; private set; }

        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return graphics; }
        }

        protected void ChangeGraphicsSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.None;
        }

        protected override void Initialize()
        {
            Tween.Initialize(7000);
            InputManager.Initialize();
            InputManager.InitializeDXManager(Services, Window);
            Buttons[] buttonList =
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
                DialogueManager.LoadLanguageDocument(Content, "Languages\\Text_En");
                DialogueManager.LoadLanguageDocument(Content, "Languages\\Diary_En");
            }
            else
            {
                DialogueManager.LoadLanguageBinFile("Content\\Languages\\Text_En.bin");
                DialogueManager.LoadLanguageBinFile("Content\\Languages\\Diary_En.bin");
            }
            DialogueManager.SetLanguage("English");
            SaveManager.Initialize();
            PhysicsManager.Initialize(ScreenManager.Camera);
            PhysicsManager.TerminalVelocity = 2000;
            InitializeNameArray();
            InitializeFemaleNameArray();
            ScreenManager.Initialize();
            InitializeGlobalInput();
            LoadConfig();
            InitializeScreenConfig();
            if (LevelEV.SHOW_FPS)
            {
                var frameRateCounter = new FrameRateCounter(this);
                Components.Add(frameRateCounter);
                frameRateCounter.Initialize();
            }
            m_forcedGameTime1 = new GameTime(default(TimeSpan), new TimeSpan(0, 0, 0, 0, (int) (m_frameLimit*1000f)));
            m_forcedGameTime2 = new GameTime(default(TimeSpan), new TimeSpan(0, 0, 0, 0, (int) (m_frameLimit*1050f)));
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
            if (GlobalInput != null)
            {
                GlobalInput.ClearAll();
            }
            else
            {
                GlobalInput = new InputMap(PlayerIndex.One, true);
            }
            GlobalInput.AddInput(0, Keys.Enter);
            GlobalInput.AddInput(2, Keys.Escape);
            GlobalInput.AddInput(6, Keys.LeftControl);
            GlobalInput.AddInput(4, Keys.Tab);
            GlobalInput.AddInput(7, Keys.LeftShift);
            GlobalInput.AddInput(5, Keys.Back);
            GlobalInput.AddInput(8, Keys.Escape);
            GlobalInput.AddInput(9, Keys.Tab);
            GlobalInput.AddInput(10, Keys.S);
            GlobalInput.AddInput(11, Keys.Space);
            GlobalInput.AddInput(24, Keys.W);
            GlobalInput.AddInput(12, Keys.D);
            GlobalInput.AddInput(13, Keys.A);
            GlobalInput.AddInput(14, Keys.Q);
            GlobalInput.AddInput(15, Keys.E);
            GlobalInput.AddInput(16, Keys.I);
            GlobalInput.AddInput(17, Keys.Up);
            GlobalInput.AddInput(18, Keys.K);
            GlobalInput.AddInput(19, Keys.Down);
            GlobalInput.AddInput(20, Keys.J);
            GlobalInput.AddInput(21, Keys.Left);
            GlobalInput.AddInput(22, Keys.L);
            GlobalInput.AddInput(23, Keys.Right);
            GlobalInput.AddInput(0, Buttons.A);
            GlobalInput.AddInput(1, Buttons.Start);
            GlobalInput.AddInput(2, Buttons.B);
            GlobalInput.AddInput(3, Buttons.Back);
            GlobalInput.AddInput(6, Buttons.RightTrigger);
            GlobalInput.AddInput(4, Buttons.Y);
            GlobalInput.AddInput(7, Buttons.X);
            GlobalInput.AddInput(5, Buttons.Back);
            GlobalInput.AddInput(8, Buttons.Start);
            GlobalInput.AddInput(9, Buttons.Back);
            GlobalInput.AddInput(10, Buttons.A);
            GlobalInput.AddInput(12, Buttons.X);
            GlobalInput.AddInput(13, Buttons.Y);
            GlobalInput.AddInput(14, Buttons.LeftTrigger);
            GlobalInput.AddInput(15, Buttons.RightTrigger);
            GlobalInput.AddInput(16, Buttons.DPadUp);
            GlobalInput.AddInput(17, ThumbStick.LeftStick, -90f, 30f);
            GlobalInput.AddInput(18, Buttons.DPadDown);
            GlobalInput.AddInput(19, ThumbStick.LeftStick, 90f, 37f);
            GlobalInput.AddInput(20, Buttons.DPadLeft);
            GlobalInput.AddInput(21, Buttons.LeftThumbstickLeft);
            GlobalInput.AddInput(22, Buttons.DPadRight);
            GlobalInput.AddInput(23, Buttons.LeftThumbstickRight);
            GlobalInput.AddInput(24, Buttons.B);
            GlobalInput.AddInput(25, Keys.Escape);
            GlobalInput.AddInput(25, Buttons.Back);
            GlobalInput.AddInput(26, Keys.Back);
            GlobalInput.AddInput(26, Buttons.Y);
            GlobalInput.KeyList[1] = GlobalInput.KeyList[12];
            GlobalInput.KeyList[3] = GlobalInput.KeyList[10];
        }

        private void InitializeDefaultConfig()
        {
            GameConfig.FullScreen = false;
            GameConfig.ScreenWidth = 1360;
            GameConfig.ScreenHeight = 768;
            GameConfig.MusicVolume = 1f;
            GameConfig.SFXVolume = 0.8f;
            GameConfig.EnableDirectInput = true;
            InputManager.Deadzone = 10f;
            GameConfig.ProfileSlot = "_no-seed-2";
            GameConfig.EnableSteamCloud = false;
            GameConfig.ReduceQuality = false;
            InitializeGlobalInput();
        }

        protected override void LoadContent()
        {
            if (!m_contentLoaded)
            {
                m_contentLoaded = true;
                LoadAllSpriteFonts();
                LoadAllEffects();
                LoadAllSpritesheets();
                SoundManager.Initialize("Content\\Audio\\RogueCastleXACTProj.xgs");
                SoundManager.LoadWaveBank("Content\\Audio\\SFXWaveBank.xwb");
                SoundManager.LoadWaveBank("Content\\Audio\\MusicWaveBank.xwb", true);
                SoundManager.LoadSoundBank("Content\\Audio\\SFXSoundBank.xsb");
                SoundManager.LoadSoundBank("Content\\Audio\\MusicSoundBank.xsb", true);
                SoundManager.GlobalMusicVolume = GameConfig.MusicVolume;
                SoundManager.GlobalSFXVolume = GameConfig.SFXVolume;
                if (InputManager.GamePadIsConnected(PlayerIndex.One))
                {
                    InputManager.SetPadType(PlayerIndex.One, PadTypes.GamePad);
                }
                InputManager.UseDirectInput = GameConfig.EnableDirectInput;
                GenericTexture = new Texture2D(GraphicsDevice, 1, 1);
                GenericTexture.SetData(new[]
                {
                    Color.White
                });
                if (!LevelEV.LOAD_SPLASH_SCREEN)
                {
                    LevelBuilder2.Initialize();
                    LevelParser.ParseRooms("Map_1x1", Content);
                    LevelParser.ParseRooms("Map_1x2", Content);
                    LevelParser.ParseRooms("Map_1x3", Content);
                    LevelParser.ParseRooms("Map_2x1", Content);
                    LevelParser.ParseRooms("Map_2x2", Content);
                    LevelParser.ParseRooms("Map_2x3", Content);
                    LevelParser.ParseRooms("Map_3x1", Content);
                    LevelParser.ParseRooms("Map_3x2", Content);
                    LevelParser.ParseRooms("Map_Special", Content);
                    LevelParser.ParseRooms("Map_DLC1", Content, true);
                    LevelBuilder2.IndexRoomList();
                }
                SkillSystem.Initialize();
                var areaStruct = new AreaStruct
                {
                    Name = "The Grand Entrance",
                    LevelType = GameTypes.LevelType.CASTLE,
                    TotalRooms = new Vector2(24f, 28f),
                    BossInArea = true,
                    SecretRooms = new Vector2(1f, 3f),
                    BonusRooms = new Vector2(2f, 3f),
                    Color = Color.White
                };
                var areaStruct2 = new AreaStruct
                {
                    LevelType = GameTypes.LevelType.GARDEN,
                    TotalRooms = new Vector2(23f, 27f),
                    BossInArea = true,
                    SecretRooms = new Vector2(1f, 3f),
                    BonusRooms = new Vector2(2f, 3f),
                    Color = Color.Green
                };
                var areaStruct3 = new AreaStruct
                {
                    LevelType = GameTypes.LevelType.TOWER,
                    TotalRooms = new Vector2(23f, 27f),
                    BossInArea = true,
                    SecretRooms = new Vector2(1f, 3f),
                    BonusRooms = new Vector2(2f, 3f),
                    Color = Color.DarkBlue
                };
                var areaStruct4 = new AreaStruct
                {
                    LevelType = GameTypes.LevelType.DUNGEON,
                    TotalRooms = new Vector2(23f, 27f),
                    BossInArea = true,
                    SecretRooms = new Vector2(1f, 3f),
                    BonusRooms = new Vector2(2f, 3f),
                    Color = Color.Red
                };
                var areaStruct5 = new AreaStruct
                {
                    Name = "The Grand Entrance",
                    LevelType = GameTypes.LevelType.CASTLE,
                    TotalRooms = new Vector2(24f, 27f),
                    BossInArea = true,
                    SecretRooms = new Vector2(2f, 3f),
                    BonusRooms = new Vector2(2f, 3f),
                    Color = Color.White
                };
                var areaStruct6 = default(AreaStruct);
                areaStruct6.Name = "The Grand Entrance";
                areaStruct6.LevelType = GameTypes.LevelType.GARDEN;
                areaStruct6.TotalRooms = new Vector2(12f, 14f);
                areaStruct6.BossInArea = true;
                areaStruct6.SecretRooms = new Vector2(2f, 3f);
                areaStruct6.BonusRooms = new Vector2(1f, 2f);
                areaStruct6.Color = Color.Green;
                var areaStruct7 = default(AreaStruct);
                areaStruct7.Name = "The Grand Entrance";
                areaStruct7.LevelType = GameTypes.LevelType.DUNGEON;
                areaStruct7.TotalRooms = new Vector2(12f, 14f);
                areaStruct7.BossInArea = true;
                areaStruct7.SecretRooms = new Vector2(2f, 3f);
                areaStruct7.BonusRooms = new Vector2(1f, 2f);
                areaStruct7.Color = Color.Red;
                var areaStruct8 = default(AreaStruct);
                areaStruct8.Name = "The Grand Entrance";
                areaStruct8.LevelType = GameTypes.LevelType.TOWER;
                areaStruct8.TotalRooms = new Vector2(12f, 14f);
                areaStruct8.BossInArea = true;
                areaStruct8.SecretRooms = new Vector2(2f, 3f);
                areaStruct8.BonusRooms = new Vector2(1f, 2f);
                areaStruct8.Color = Color.DarkBlue;
                Area1List = new[]
                {
                    areaStruct,
                    areaStruct2,
                    areaStruct3,
                    areaStruct4
                };
                if (LevelEV.RUN_DEMO_VERSION)
                {
                    Area1List = new[]
                    {
                        areaStruct5
                    };
                }
            }
        }

        public void LoadAllSpriteFonts()
        {
            SpriteFontArray.SpriteFontList.Clear();
            PixelArtFont = Content.Load<SpriteFont>("Fonts\\Arial12");
            SpriteFontArray.SpriteFontList.Add(PixelArtFont);
            PixelArtFontBold = Content.Load<SpriteFont>("Fonts\\PixelArtFontBold");
            SpriteFontArray.SpriteFontList.Add(PixelArtFontBold);
            EnemyLevelFont = Content.Load<SpriteFont>("Fonts\\EnemyLevelFont");
            SpriteFontArray.SpriteFontList.Add(EnemyLevelFont);
            EnemyLevelFont.Spacing = -5f;
            PlayerLevelFont = Content.Load<SpriteFont>("Fonts\\PlayerLevelFont");
            SpriteFontArray.SpriteFontList.Add(PlayerLevelFont);
            PlayerLevelFont.Spacing = -7f;
            GoldFont = Content.Load<SpriteFont>("Fonts\\GoldFont");
            SpriteFontArray.SpriteFontList.Add(GoldFont);
            GoldFont.Spacing = -5f;
            JunicodeFont = Content.Load<SpriteFont>("Fonts\\Junicode");
            SpriteFontArray.SpriteFontList.Add(JunicodeFont);
            JunicodeLargeFont = Content.Load<SpriteFont>("Fonts\\JunicodeLarge");
            SpriteFontArray.SpriteFontList.Add(JunicodeLargeFont);
            JunicodeLargeFont.Spacing = -1f;
            HerzogFont = Content.Load<SpriteFont>("Fonts\\HerzogVonGraf24");
            SpriteFontArray.SpriteFontList.Add(HerzogFont);
            CinzelFont = Content.Load<SpriteFont>("Fonts\\CinzelFont");
            SpriteFontArray.SpriteFontList.Add(CinzelFont);
            BitFont = Content.Load<SpriteFont>("Fonts\\BitFont");
            SpriteFontArray.SpriteFontList.Add(BitFont);
        }

        public void LoadAllSpritesheets()
        {
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\blacksmithUISpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\enemyFinal2Spritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\enemyFinalSpritesheetBig", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\miscSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\traitsCastleSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\castleTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\playerSpritesheetBig", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\titleScreen3Spritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\mapSpritesheetBig", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\startingRoomSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\towerTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\dungeonTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\profileCardSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\portraitSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\gardenTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\parallaxBGSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\getItemScreenSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\neoTerrainSpritesheet", false);
        }

        public void LoadAllEffects()
        {
            MaskEffect = Content.Load<Effect>("Shaders\\AlphaMaskShader");
            ShadowEffect = Content.Load<Effect>("Shaders\\ShadowFX");
            ParallaxEffect = Content.Load<Effect>("Shaders\\ParallaxFX");
            HSVEffect = Content.Load<Effect>("Shaders\\HSVShader");
            InvertShader = Content.Load<Effect>("Shaders\\InvertShader");
            ColourSwapShader = Content.Load<Effect>("Shaders\\ColourSwapShader");
            RippleEffect = Content.Load<Effect>("Shaders\\Shockwave");
            RippleEffect.Parameters["mag"].SetValue(2);
            GaussianBlur = new GaussianBlur(this, 1320, 720);
            GaussianBlur.Amount = 2f;
            GaussianBlur.Radius = 7;
            GaussianBlur.ComputeKernel();
            GaussianBlur.ComputeOffsets();
            GaussianBlur.InvertMask = true;
            BWMaskEffect = Content.Load<Effect>("Shaders\\BWMaskShader");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!m_gameLoaded)
            {
                m_gameLoaded = true;
                if (LevelEV.DELETE_SAVEFILE)
                {
                    SaveManager.ClearAllFileTypes(true);
                    SaveManager.ClearAllFileTypes(false);
                }
                if (LevelEV.LOAD_SPLASH_SCREEN)
                {
                    if (LevelEV.RUN_DEMO_VERSION)
                    {
                        ScreenManager.DisplayScreen(28, true);
                    }
                    else
                    {
                        ScreenManager.DisplayScreen(1, true);
                    }
                }
                else if (!LevelEV.LOAD_TITLE_SCREEN)
                {
                    if (LevelEV.RUN_TESTROOM)
                    {
                        ScreenManager.DisplayScreen(5, true);
                    }
                    else if (LevelEV.RUN_TUTORIAL)
                    {
                        ScreenManager.DisplayScreen(23, true);
                    }
                    else
                    {
                        ScreenManager.DisplayScreen(15, true);
                    }
                }
                else
                {
                    ScreenManager.DisplayScreen(3, true);
                }
            }
            TotalGameTime = (float) gameTime.TotalGameTime.TotalSeconds;
            TotalGameTimeHours = (float) gameTime.TotalGameTime.TotalHours;
            var gameTime2 = gameTime;
            if (gameTime.ElapsedGameTime.TotalSeconds > m_frameLimit)
            {
                if (!m_frameLimitSwap)
                {
                    m_frameLimitSwap = true;
                    gameTime2 = m_forcedGameTime1;
                }
                else
                {
                    m_frameLimitSwap = false;
                    gameTime2 = m_forcedGameTime2;
                }
            }
            SoundManager.Update(gameTime2);
            if (IsActive || (!IsActive && LevelEV.ENABLE_OFFSCREEN_CONTROL))
            {
                InputManager.Update(gameTime2);
            }
            Tween.Update(gameTime2);
            ScreenManager.Update(gameTime2);
            SoundManager.Update3DSounds();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            ScreenManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void InitializeNameArray()
        {
            NameArray = new List<string>();
            using (var streamReader = new StreamReader("Content\\HeroNames.txt"))
            {
                var spriteFont = Content.Load<SpriteFont>("Fonts\\Junicode");
                SpriteFontArray.SpriteFontList.Add(spriteFont);
                var textObj = new TextObj(spriteFont);
                while (!streamReader.EndOfStream)
                {
                    var text = streamReader.ReadLine();
                    var flag = false;
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
                        NameArray.Add(text);
                    }
                }
                if (NameArray.Count < 1)
                {
                    NameArray.Add("Lee");
                    NameArray.Add("Charles");
                    NameArray.Add("Lancelot");
                }
                textObj.Dispose();
                SpriteFontArray.SpriteFontList.Clear();
            }
        }

        private void InitializeFemaleNameArray()
        {
            FemaleNameArray = new List<string>();
            using (var streamReader = new StreamReader("Content\\HeroineNames.txt"))
            {
                var spriteFont = Content.Load<SpriteFont>("Fonts\\Junicode");
                SpriteFontArray.SpriteFontList.Add(spriteFont);
                var textObj = new TextObj(spriteFont);
                while (!streamReader.EndOfStream)
                {
                    var text = streamReader.ReadLine();
                    var flag = false;
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
                        FemaleNameArray.Add(text);
                    }
                }
                if (FemaleNameArray.Count < 1)
                {
                    FemaleNameArray.Add("Jenny");
                    FemaleNameArray.Add("Shanoa");
                    FemaleNameArray.Add("Chun Li");
                }
                textObj.Dispose();
                SpriteFontArray.SpriteFontList.Clear();
            }
        }

        public void SaveOnExit()
        {
            if (!(ScreenManager.CurrentScreen is CDGSplashScreen) && !(ScreenManager.CurrentScreen is DemoStartScreen))
            {
                UpdatePlaySessionLength();
                var levelScreen = ScreenManager.GetLevelScreen();
                if (levelScreen != null &&
                    (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom ||
                     levelScreen.CurrentRoom is CarnivalShoot2BonusRoom))
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
                    var challengeBossRoomObj = levelScreen.CurrentRoom as ChallengeBossRoomObj;
                    if (challengeBossRoomObj != null)
                    {
                        challengeBossRoomObj.LoadPlayerData();
                        SaveManager.LoadFiles(levelScreen, SaveType.UpgradeData);
                        levelScreen.Player.CurrentHealth = challengeBossRoomObj.StoredHP;
                        levelScreen.Player.CurrentMana = challengeBossRoomObj.StoredMP;
                    }
                }
                if (ScreenManager.CurrentScreen is GameOverScreen)
                {
                    PlayerStats.Traits = Vector2.Zero;
                }
                if (SaveManager.FileExists(SaveType.PlayerData))
                {
                    SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
                    if (PlayerStats.TutorialComplete && levelScreen != null && levelScreen.CurrentRoom.Name != "Start" &&
                        levelScreen.CurrentRoom.Name != "Ending" && levelScreen.CurrentRoom.Name != "Tutorial")
                    {
                        SaveManager.SaveFiles(SaveType.MapData);
                    }
                }
            }
        }

        public void FormClosing(object sender, FormClosingEventArgs args)
        {
            if (args.CloseReason == CloseReason.UserClosing)
            {
                SaveOnExit();
            }
        }

        public void UpdatePlaySessionLength()
        {
            PlaySessionLength = TotalGameTimeHours;
        }

        public List<Vector2> GetSupportedResolutions()
        {
            var list = new List<Vector2>();
            foreach (var current in GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                if (current.Width < 2000 && current.Height < 2000)
                {
                    var item = new Vector2(current.Width, current.Height);
                    if (!list.Contains(item))
                    {
                        list.Add(new Vector2(current.Width, current.Height));
                    }
                }
            }
            return list;
        }

        public void SaveConfig()
        {
            Console.WriteLine("Saving Config file");
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(folderPath, "Rogue Legacy Archipelago");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var path2 = Path.Combine(folderPath, "Rogue Legacy Archipelago", "GameConfig.ini");
            using (var streamWriter = new StreamWriter(path2, false))
            {
                streamWriter.WriteLine("[Screen Resolution]");
                streamWriter.WriteLine("ScreenWidth=" + GameConfig.ScreenWidth);
                streamWriter.WriteLine("ScreenHeight=" + GameConfig.ScreenHeight);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Fullscreen]");
                streamWriter.WriteLine("Fullscreen=" + GameConfig.FullScreen);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[QuickDrop]");
                streamWriter.WriteLine("QuickDrop=" + GameConfig.QuickDrop);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Game Volume]");
                streamWriter.WriteLine("MusicVol=" + string.Format("{0:F2}", GameConfig.MusicVolume));
                streamWriter.WriteLine("SFXVol=" + string.Format("{0:F2}", GameConfig.SFXVolume));
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Joystick Dead Zone]");
                streamWriter.WriteLine("DeadZone=" + InputManager.Deadzone);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Enable DirectInput Gamepads]");
                streamWriter.WriteLine("EnableDirectInput=" + GameConfig.EnableDirectInput);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Reduce Shader Quality]");
                streamWriter.WriteLine("ReduceQuality=" + GameConfig.ReduceQuality);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Profile]");
                streamWriter.WriteLine("Slot=" + GameConfig.ProfileSlot);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Keyboard Config]");
                streamWriter.WriteLine("KeyUP=" + GlobalInput.KeyList[16]);
                streamWriter.WriteLine("KeyDOWN=" + GlobalInput.KeyList[18]);
                streamWriter.WriteLine("KeyLEFT=" + GlobalInput.KeyList[20]);
                streamWriter.WriteLine("KeyRIGHT=" + GlobalInput.KeyList[22]);
                streamWriter.WriteLine("KeyATTACK=" + GlobalInput.KeyList[12]);
                streamWriter.WriteLine("KeyJUMP=" + GlobalInput.KeyList[10]);
                streamWriter.WriteLine("KeySPECIAL=" + GlobalInput.KeyList[13]);
                streamWriter.WriteLine("KeyDASHLEFT=" + GlobalInput.KeyList[14]);
                streamWriter.WriteLine("KeyDASHRIGHT=" + GlobalInput.KeyList[15]);
                streamWriter.WriteLine("KeySPELL1=" + GlobalInput.KeyList[24]);
                streamWriter.WriteLine();
                streamWriter.WriteLine("[Gamepad Config]");
                streamWriter.WriteLine("ButtonUP=" + GlobalInput.ButtonList[16]);
                streamWriter.WriteLine("ButtonDOWN=" + GlobalInput.ButtonList[18]);
                streamWriter.WriteLine("ButtonLEFT=" + GlobalInput.ButtonList[20]);
                streamWriter.WriteLine("ButtonRIGHT=" + GlobalInput.ButtonList[22]);
                streamWriter.WriteLine("ButtonATTACK=" + GlobalInput.ButtonList[12]);
                streamWriter.WriteLine("ButtonJUMP=" + GlobalInput.ButtonList[10]);
                streamWriter.WriteLine("ButtonSPECIAL=" + GlobalInput.ButtonList[13]);
                streamWriter.WriteLine("ButtonDASHLEFT=" + GlobalInput.ButtonList[14]);
                streamWriter.WriteLine("ButtonDASHRIGHT=" + GlobalInput.ButtonList[15]);
                streamWriter.WriteLine("ButtonSPELL1=" + GlobalInput.ButtonList[24]);
                streamWriter.Close();
            }
        }

        public void LoadConfig()
        {
            Console.WriteLine("Loading Config file");
            InitializeDefaultConfig();
            try
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Path.Combine(folderPath, "Rogue Legacy Archipelago", "GameConfig.ini");
                using (var streamReader = new StreamReader(path))
                {
                    var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
                    cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
                    string text;
                    while ((text = streamReader.ReadLine()) != null)
                    {
                        var num = text.IndexOf("=");
                        if (num != -1)
                        {
                            var text2 = text.Substring(0, num);
                            var text3 = text.Substring(num + 1);
                            string key;
                            switch (key = text2)
                            {
                                case "ScreenWidth":
                                    GameConfig.ScreenWidth = int.Parse(text3, NumberStyles.Any, cultureInfo);
                                    break;
                                case "ScreenHeight":
                                    GameConfig.ScreenHeight = int.Parse(text3, NumberStyles.Any, cultureInfo);
                                    break;
                                case "Fullscreen":
                                    GameConfig.FullScreen = bool.Parse(text3);
                                    break;
                                case "QuickDrop":
                                    GameConfig.QuickDrop = bool.Parse(text3);
                                    break;
                                case "MusicVol":
                                    GameConfig.MusicVolume = float.Parse(text3);
                                    break;
                                case "SFXVol":
                                    GameConfig.SFXVolume = float.Parse(text3);
                                    break;
                                case "DeadZone":
                                    InputManager.Deadzone = int.Parse(text3, NumberStyles.Any, cultureInfo);
                                    break;
                                case "EnableDirectInput":
                                    GameConfig.EnableDirectInput = bool.Parse(text3);
                                    break;
                                case "ReduceQuality":
                                    GameConfig.ReduceQuality = bool.Parse(text3);
                                    LevelEV.SAVE_FRAMES = GameConfig.ReduceQuality;
                                    break;
                                case "EnableSteamCloud":
                                    GameConfig.EnableSteamCloud = bool.Parse(text3);
                                    break;
                                case "Slot":
                                    GameConfig.ProfileSlot = text3;
                                    break;
                                case "KeyUP":
                                    GlobalInput.KeyList[16] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyDOWN":
                                    GlobalInput.KeyList[18] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyLEFT":
                                    GlobalInput.KeyList[20] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyRIGHT":
                                    GlobalInput.KeyList[22] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyATTACK":
                                    GlobalInput.KeyList[12] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyJUMP":
                                    GlobalInput.KeyList[10] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeySPECIAL":
                                    GlobalInput.KeyList[13] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyDASHLEFT":
                                    GlobalInput.KeyList[14] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeyDASHRIGHT":
                                    GlobalInput.KeyList[15] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "KeySPELL1":
                                    GlobalInput.KeyList[24] = (Keys) Enum.Parse(typeof (Keys), text3);
                                    break;
                                case "ButtonUP":
                                    GlobalInput.ButtonList[16] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonDOWN":
                                    GlobalInput.ButtonList[18] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonLEFT":
                                    GlobalInput.ButtonList[20] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonRIGHT":
                                    GlobalInput.ButtonList[22] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonATTACK":
                                    GlobalInput.ButtonList[12] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonJUMP":
                                    GlobalInput.ButtonList[10] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonSPECIAL":
                                    GlobalInput.ButtonList[13] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonDASHLEFT":
                                    GlobalInput.ButtonList[14] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonDASHRIGHT":
                                    GlobalInput.ButtonList[15] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                                case "ButtonSPELL1":
                                    GlobalInput.ButtonList[24] = (Buttons) Enum.Parse(typeof (Buttons), text3);
                                    break;
                            }
                        }
                    }
                    GlobalInput.KeyList[1] = GlobalInput.KeyList[12];
                    GlobalInput.KeyList[3] = GlobalInput.KeyList[10];
                    streamReader.Close();
                    if (GameConfig.ScreenHeight <= 0 || GameConfig.ScreenWidth <= 0)
                    {
                        throw new Exception("Blank Config File");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Config File Not Found. Creating Default Config File.");
                InitializeDefaultConfig();
                SaveConfig();
            }
        }

        public void InitializeScreenConfig()
        {
            graphics.PreferredBackBufferWidth = GameConfig.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameConfig.ScreenHeight;
            if ((graphics.IsFullScreen && !GameConfig.FullScreen) || (!graphics.IsFullScreen && GameConfig.FullScreen))
            {
                graphics.ToggleFullScreen();
            }
            else
            {
                graphics.ApplyChanges();
            }
            ScreenManager.ForceResolutionChangeCheck();
        }

        public static float GetTotalGameTimeHours()
        {
            return TotalGameTimeHours;
        }

        public struct SettingStruct
        {
            public bool EnableDirectInput;
            public bool EnableSteamCloud;
            public bool FullScreen;
            public float MusicVolume;
            public string ProfileSlot;
            public bool QuickDrop;
            public bool ReduceQuality;
            public int ScreenHeight;
            public int ScreenWidth;
            public float SFXVolume;
        }
    }
}
