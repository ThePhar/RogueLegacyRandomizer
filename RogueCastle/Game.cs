using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Archipelago;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.GameObjects;
using RogueCastle.Screens;
using RogueCastle.Enums;
using SpriteSystem;
using Tweener;

using Button = RogueCastle.Enums.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Screen = RogueCastle.Enums.Screen;

namespace RogueCastle
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static AreaStruct[] Area1List;
        public static SettingStruct GameConfig;
        private bool _contentLoaded;
        private GameTime _forcedGameTime1;
        private GameTime _forcedGameTime2;
        private bool _frameLimitSwap;
        private bool _gameLoaded;

        public Game()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
            }

            ArchipelagoManager = new Client();
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            ScreenManager = new RCScreenManager(this);
            SaveManager = new SaveGameManager();
            PhysicsManager = new PhysicsManager();
            EquipmentSystem = new EquipmentSystem();

            PlayerStats = new PlayerStats();
            Content.RootDirectory = "Content";
            EngineEV.ScreenWidth = 1320;
            EngineEV.ScreenHeight = 720;
            Window.Title = "Rogue Legacy Randomizer";
            IsFixedTimeStep = false;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = !LevelENV.ShowFps;
            Window.AllowUserResizing = true;

            if (!LevelENV.EnableOffscreenControl)
            {
                InactiveSleepTime = default;
            }

            EquipmentSystem.InitializeEquipmentData();
            EquipmentSystem.InitializeAbilityCosts();
            GameConfig = default;

            if (Control.FromHandle(Window.Handle) is Form form)
            {
                form.FormClosing += FormClosing;
            }

            GraphicsDeviceManager.PreparingDeviceSettings += ChangeGraphicsSettings;
            SleepUtil.DisableScreensaver();
        }

        public static Cue LineageSongCue { get; set; }
        public static RCScreenManager ScreenManager { get; set; }
        public static Effect BWMaskEffect { get; set; }
        public static Effect ColourSwapShader { get; set; }
        public static Effect HSVEffect { get; set; }
        public static Effect InvertShader { get; set; }
        public static Effect MaskEffect { get; set; }
        public static Effect ParallaxEffect { get; set; }
        public static Effect RippleEffect { get; set; }
        public static Effect ShadowEffect { get; set; }
        public static EquipmentSystem EquipmentSystem { get; set; }
        public static GaussianBlur GaussianBlur { get; set; }
        public static PlayerStats PlayerStats { get; set; }
        public static InputMap GlobalInput { get; set; }
        public static List<string> NameArray { get; set; }
        public static List<string> FemaleNameArray { get; set; }
        public static SpriteFont PixelArtFont { get; set; }
        public static SpriteFont PixelArtFontBold { get; set; }
        public static SpriteFont JunicodeFont { get; set; }
        public static SpriteFont EnemyLevelFont { get; set; }
        public static SpriteFont PlayerLevelFont { get; set; }
        public static SpriteFont GoldFont { get; set; }
        public static SpriteFont HerzogFont { get; set; }
        public static SpriteFont JunicodeLargeFont { get; set; }
        public static SpriteFont CinzelFont { get; set; }
        public static SpriteFont BitFont { get; set; }
        public static Texture2D GenericTexture { get; set; }
        public static float PlaySessionLength { get; set; }
        public static float TotalGameTimeSeconds { get; set; }
        public static float TotalGameTimeHours { get; set; }
        public static string ProfileName { get; set; }

        public Client ArchipelagoManager { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public PhysicsManager PhysicsManager { get; private set; }
        public SaveGameManager SaveManager { get; private set; }

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
            DialogueManager.LoadLanguageBinFile(@"Content\Languages\Text_En.bin");
            DialogueManager.LoadLanguageBinFile(@"Content\Languages\Diary_En.bin");
            DialogueManager.SetLanguage("English");

            ProfileName = "DEFAULT-0";
            SaveManager.Initialize();
            PhysicsManager.Initialize(ScreenManager.Camera);
            PhysicsManager.TerminalVelocity = 2000;
            ScreenManager.Initialize();
            InitializeNameArray();
            InitializeFemaleNameArray();
            InitializeGlobalInput();
            LoadConfig();

            InitializeScreenConfig();
            if (LevelENV.ShowFps)
            {
                var frameRateCounter = new FrameRateCounter(this);
                Components.Add(frameRateCounter);
                frameRateCounter.Initialize();
            }

            if (LevelENV.ShowArchipelagoStatus)
            {
                var statusIndicator = new ArchipelagoStatusIndicator(this);
                Components.Add(statusIndicator);
                statusIndicator.Initialize();
            }

            _forcedGameTime1 = new GameTime(default, new TimeSpan(0, 0, 0, 0, (int) (LevelENV.FrameLimit * 1000f)));
            _forcedGameTime2 = new GameTime(default, new TimeSpan(0, 0, 0, 0, (int) (LevelENV.FrameLimit * 1050f)));
            base.Initialize();
        }

        private static void InitializeDefaultConfig()
        {
            GameConfig.FullScreen = false;
            GameConfig.ScreenWidth = 1360;
            GameConfig.ScreenHeight = 768;
            GameConfig.MusicVolume = 1f;
            GameConfig.SFXVolume = 0.8f;
            GameConfig.EnableDirectInput = true;
            InputManager.Deadzone = 10f;
            GameConfig.EnableSteamCloud = false;
            GameConfig.ReduceQuality = false;

            InitializeGlobalInput();
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

            GlobalInput.AddInput((int) Button.MenuConfirm1, Keys.Enter);
            GlobalInput.AddInput((int) Button.MenuCancel1, Keys.Escape);
            GlobalInput.AddInput((int) Button.MenuCredits, Keys.LeftControl);
            GlobalInput.AddInput((int) Button.MenuOptions, Keys.Tab);
            GlobalInput.AddInput((int) Button.MenuProfileCard, Keys.LeftShift);
            GlobalInput.AddInput((int) Button.MenuRogueMode, Keys.Back);
            GlobalInput.AddInput((int) Button.MenuPause, Keys.Escape);
            GlobalInput.AddInput((int) Button.MenuMap, Keys.Tab);
            GlobalInput.AddInput((int) Button.PlayerJump1, Keys.S);
            GlobalInput.AddInput((int) Button.PlayerJump2, Keys.Space);
            GlobalInput.AddInput((int) Button.PlayerSpell, Keys.W);
            GlobalInput.AddInput((int) Button.PlayerAttack, Keys.D);
            GlobalInput.AddInput((int) Button.PlayerBlock, Keys.A);
            GlobalInput.AddInput((int) Button.PlayerDashLeft, Keys.Q);
            GlobalInput.AddInput((int) Button.PlayerDashRight, Keys.E);
            GlobalInput.AddInput((int) Button.PlayerUp1, Keys.I);
            GlobalInput.AddInput((int) Button.PlayerUp2, Keys.Up);
            GlobalInput.AddInput((int) Button.PlayerDown1, Keys.K);
            GlobalInput.AddInput((int) Button.PlayerDown2, Keys.Down);
            GlobalInput.AddInput((int) Button.PlayerLeft1, Keys.J);
            GlobalInput.AddInput((int) Button.PlayerLeft2, Keys.Left);
            GlobalInput.AddInput((int) Button.PlayerRight1, Keys.L);
            GlobalInput.AddInput((int) Button.PlayerRight2, Keys.Right);
            GlobalInput.AddInput((int) Button.MenuConfirm1, Buttons.A);
            GlobalInput.AddInput((int) Button.MenuConfirm2, Buttons.Start);
            GlobalInput.AddInput((int) Button.MenuCancel1, Buttons.B);
            GlobalInput.AddInput((int) Button.MenuCancel2, Buttons.Back);
            GlobalInput.AddInput((int) Button.MenuCredits, Buttons.RightTrigger);
            GlobalInput.AddInput((int) Button.MenuOptions, Buttons.Y);
            GlobalInput.AddInput((int) Button.MenuProfileCard, Buttons.X);
            GlobalInput.AddInput((int) Button.MenuRogueMode, Buttons.Back);
            GlobalInput.AddInput((int) Button.MenuPause, Buttons.Start);
            GlobalInput.AddInput((int) Button.MenuMap, Buttons.Back);
            GlobalInput.AddInput((int) Button.PlayerJump1, Buttons.A);
            GlobalInput.AddInput((int) Button.PlayerAttack, Buttons.X);
            GlobalInput.AddInput((int) Button.PlayerBlock, Buttons.Y);
            GlobalInput.AddInput((int) Button.PlayerDashLeft, Buttons.LeftTrigger);
            GlobalInput.AddInput((int) Button.PlayerDashRight, Buttons.RightTrigger);
            GlobalInput.AddInput((int) Button.PlayerUp1, Buttons.DPadUp);
            GlobalInput.AddInput((int) Button.PlayerUp2, ThumbStick.LeftStick, -90f, 30f);
            GlobalInput.AddInput((int) Button.PlayerDown1, Buttons.DPadDown);
            GlobalInput.AddInput((int) Button.PlayerDown2, ThumbStick.LeftStick, 90f, 37f);
            GlobalInput.AddInput((int) Button.PlayerLeft1, Buttons.DPadLeft);
            GlobalInput.AddInput((int) Button.PlayerLeft2, Buttons.LeftThumbstickLeft);
            GlobalInput.AddInput((int) Button.PlayerRight1, Buttons.DPadRight);
            GlobalInput.AddInput((int) Button.PlayerRight2, Buttons.LeftThumbstickRight);
            GlobalInput.AddInput((int) Button.PlayerSpell, Buttons.B);
            GlobalInput.AddInput((int) Button.MenuProfileSelect, Keys.Escape);
            GlobalInput.AddInput((int) Button.MenuProfileSelect, Buttons.Back);
            GlobalInput.AddInput((int) Button.MenuDeleteProfile, Keys.Back);
            GlobalInput.AddInput((int) Button.MenuDeleteProfile, Buttons.Y);

            GlobalInput.KeyList[1] = GlobalInput.KeyList[12];
            GlobalInput.KeyList[3] = GlobalInput.KeyList[10];
        }

        public void ChangeProfile(string seed, int slot)
        {
            // Load Default Save Profile
            ProfileName = "DEFAULT";
            SaveManager.CreateSaveDirectory();
            SaveManager.LoadAllFileTypes(null);

            // Change Profile Name
            ProfileName = $"AP_{seed}-{slot}";

            // Load AP Save Files
            SaveManager.CreateSaveDirectory();
            SaveManager.LoadAllFileTypes(null);
        }

        protected override void LoadContent()
        {
            if (_contentLoaded)
            {
                return;
            }

            _contentLoaded = true;
            LoadAllSpriteFonts();
            LoadAllEffects();
            LoadAllSpritesheets();
            SoundManager.Initialize(@"Content\Audio\RogueCastleXACTProj.xgs");
            SoundManager.LoadWaveBank(@"Content\Audio\SFXWaveBank.xwb");
            SoundManager.LoadWaveBank(@"Content\Audio\MusicWaveBank.xwb", true);
            SoundManager.LoadSoundBank(@"Content\Audio\SFXSoundBank.xsb");
            SoundManager.LoadSoundBank(@"Content\Audio\MusicSoundBank.xsb", true);
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

            if (!LevelENV.LoadSplashScreen)
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

            var castleArea = new AreaStruct
            {
                Name = "The Grand Entrance",
                Zone = Zone.Castle,
                TotalRooms = new Vector2(24f, 28f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.White
            };
            var gardenArea = new AreaStruct
            {
                Zone = Zone.Garden,
                TotalRooms = new Vector2(23f, 27f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.Green
            };
            var towerArea = new AreaStruct
            {
                Zone = Zone.Tower,
                TotalRooms = new Vector2(23f, 27f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.DarkBlue
            };
            var dungeonArea = new AreaStruct
            {
                Zone = Zone.Dungeon,
                TotalRooms = new Vector2(23f, 27f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.Red
            };

            Area1List = new[]
            {
                castleArea,
                gardenArea,
                towerArea,
                dungeonArea
            };
        }

        public void LoadAllSpriteFonts()
        {
            SpriteFontArray.SpriteFontList.Clear();

            PixelArtFont = Content.Load<SpriteFont>(@"Fonts\Arial12");
            PixelArtFontBold = Content.Load<SpriteFont>(@"Fonts\PixelArtFontBold");
            EnemyLevelFont = Content.Load<SpriteFont>(@"Fonts\EnemyLevelFont");
            PlayerLevelFont = Content.Load<SpriteFont>(@"Fonts\PlayerLevelFont");
            GoldFont = Content.Load<SpriteFont>(@"Fonts\GoldFont");
            JunicodeFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
            JunicodeLargeFont = Content.Load<SpriteFont>(@"Fonts\JunicodeLarge");
            HerzogFont = Content.Load<SpriteFont>(@"Fonts\HerzogVonGraf24");
            CinzelFont = Content.Load<SpriteFont>(@"Fonts\CinzelFont");
            BitFont = Content.Load<SpriteFont>(@"Fonts\BitFont");

            SpriteFontArray.SpriteFontList.Add(PixelArtFont);
            SpriteFontArray.SpriteFontList.Add(PixelArtFontBold);
            SpriteFontArray.SpriteFontList.Add(EnemyLevelFont);
            SpriteFontArray.SpriteFontList.Add(PlayerLevelFont);
            SpriteFontArray.SpriteFontList.Add(GoldFont);
            SpriteFontArray.SpriteFontList.Add(JunicodeFont);
            SpriteFontArray.SpriteFontList.Add(JunicodeLargeFont);
            SpriteFontArray.SpriteFontList.Add(HerzogFont);
            SpriteFontArray.SpriteFontList.Add(CinzelFont);
            SpriteFontArray.SpriteFontList.Add(BitFont);

            EnemyLevelFont.Spacing = -5f;
            PlayerLevelFont.Spacing = -7f;
            GoldFont.Spacing = -5f;
            JunicodeLargeFont.Spacing = -1f;
        }

        public void LoadAllSpritesheets()
        {
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\blacksmithUISpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\enemyFinal2Spritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\enemyFinalSpritesheetBig", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\miscSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\traitsCastleSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\castleTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\playerSpritesheetBig", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\titleScreen3Spritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\mapSpritesheetBig", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\startingRoomSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\towerTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\dungeonTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\profileCardSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\portraitSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\gardenTerrainSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\parallaxBGSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\getItemScreenSpritesheet", false);
            SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\neoTerrainSpritesheet", false);
        }

        public void LoadAllEffects()
        {
            MaskEffect = Content.Load<Effect>(@"Shaders\AlphaMaskShader");
            ShadowEffect = Content.Load<Effect>(@"Shaders\ShadowFX");
            ParallaxEffect = Content.Load<Effect>(@"Shaders\ParallaxFX");
            HSVEffect = Content.Load<Effect>(@"Shaders\HSVShader");
            InvertShader = Content.Load<Effect>(@"Shaders\InvertShader");
            ColourSwapShader = Content.Load<Effect>(@"Shaders\ColourSwapShader");
            RippleEffect = Content.Load<Effect>(@"Shaders\Shockwave");
            RippleEffect.Parameters["mag"].SetValue(2);
            GaussianBlur = new GaussianBlur(this, 1320, 720)
            {
                Amount = 2f,
                Radius = 7
            };

            GaussianBlur.ComputeKernel();
            GaussianBlur.ComputeOffsets();
            GaussianBlur.InvertMask = true;
            BWMaskEffect = Content.Load<Effect>(@"Shaders\BWMaskShader");
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_gameLoaded)
            {
                _gameLoaded = true;
                if (LevelENV.DeleteSaveFile)
                {
                    SaveManager.ClearAllFileTypes(true);
                    SaveManager.ClearAllFileTypes(false);
                }

                ScreenManager.DisplayScreen(LevelENV.LoadSplashScreen ? Screen.CDGSplash : Screen.Title, true);
            }

            TotalGameTimeSeconds = (float) gameTime.TotalGameTime.TotalSeconds;
            TotalGameTimeHours = (float) gameTime.TotalGameTime.TotalHours;

            if (gameTime.ElapsedGameTime.TotalSeconds > LevelENV.FrameLimit)
            {
                if (!_frameLimitSwap)
                {
                    _frameLimitSwap = true;
                    gameTime = _forcedGameTime1;
                }
                else
                {
                    _frameLimitSwap = false;
                    gameTime = _forcedGameTime2;
                }
            }

            SoundManager.Update(gameTime);
            if (IsActive || !IsActive && LevelENV.EnableOffscreenControl)
            {
                InputManager.Update(gameTime);
            }

            Tween.Update(gameTime);
            ScreenManager.Update(gameTime);
            SoundManager.Update3DSounds();

            // Wait for Arch to say its ready.
            switch (ArchipelagoManager.ConnectionStatus)
            {
                // We're ready!
                case ConnectionStatus.Connected:
                {
                    // Do not attempt to start the game if we connect off the Arch screen. Most likely means we lost connection in gameplay and reconnected.
                    if (ScreenManager.CurrentScreen is not ArchipelagoScreen)
                    {
                        break;
                    }

                    // Initialize Save Data
                    ChangeProfile(ArchipelagoManager.Data.Seed, ArchipelagoManager.Data.Slot);

                    SoundManager.PlaySound("Game_Start");
                    var newGame = !PlayerStats.CharacterFound;
                    var heroIsDead = PlayerStats.IsDead;
                    var startingRoom = PlayerStats.LoadStartingRoom;

                    switch (ArchipelagoManager.Data.IsFemale)
                    {
                        case true when !FemaleNameArray.Contains(ArchipelagoManager.Data.Name):
                            FemaleNameArray.Add(ArchipelagoManager.Data.Name);
                            break;

                        case false when !NameArray.Contains(ArchipelagoManager.Data.Name):
                            NameArray.Add(ArchipelagoManager.Data.Name);
                            break;
                    }

                    if (newGame)
                    {
                        PlayerStats.CharacterFound = true;
                        PlayerStats.Gold = 0;
                        PlayerStats.Class = ArchipelagoManager.Data.StartingClass;

                        PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, 5);
                        PlayerStats.EnemiesKilledInRun.Clear();

                        // Set AP Settings
                        PlayerStats.TimesCastleBeaten = ArchipelagoManager.Data.Difficulty;

                        // Rename Sir Lee to the player's name and initial gender.
                        PlayerStats.IsFemale = ArchipelagoManager.Data.IsFemale;
                        PlayerStats.PlayerName = $"{(PlayerStats.IsFemale ? "Lady" : "Sir")} {ArchipelagoManager.Data.Name}";

                        SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
                        ScreenManager.DisplayScreen(Screen.StartingRoom, true);
                    }
                    else
                    {
                        if (heroIsDead)
                        {
                            ScreenManager.DisplayScreen(Screen.Lineage, true);
                        }
                        else
                        {
                            ScreenManager.DisplayScreen(startingRoom ? Screen.StartingRoom : Screen.Level, true);
                        }
                    }

                    SoundManager.StopMusic(0.2f);
                    break;
                }
            }

            // This function will ensure our locations are synced. Only really used in Co-Op scenarios.
            if (ArchipelagoManager.CheckedLocationsUpdated)
            {
                foreach (var location in ArchipelagoManager.CheckedLocations)
                {
                    // Manor
                    switch (location)
                    {
                        case 91000: // Manor Renovation - Ground Road
                            SkillSystem.GetSkill(Skill.ManorGroundRoad).CurrentLevel = 1;
                            break;
                        case 91001: // Manor Renovation - Main Base
                            SkillSystem.GetSkill(Skill.ManorMainBase).CurrentLevel = 1;
                            break;
                        case 91002: // Manor Renovation - Main Bottom Window
                            SkillSystem.GetSkill(Skill.ManorMainWindowBottom).CurrentLevel = 1;
                            break;
                        case 91003: // Manor Renovation - Main Top Window
                            SkillSystem.GetSkill(Skill.ManorMainWindowTop).CurrentLevel = 1;
                            break;
                        case 91004: // Manor Renovation - Main Rooftop
                            SkillSystem.GetSkill(Skill.ManorMainRoof).CurrentLevel = 1;
                            break;
                        case 91005: // Manor Renovation - Left Wing Base
                            SkillSystem.GetSkill(Skill.ManorLeftWingBase).CurrentLevel = 1;
                            break;
                        case 91006: // Manor Renovation - Left Wing Window
                            SkillSystem.GetSkill(Skill.ManorLeftWingWindow).CurrentLevel = 1;
                            break;
                        case 91007: // Manor Renovation - Left Wing Rooftop
                            SkillSystem.GetSkill(Skill.ManorLeftWingRoof).CurrentLevel = 1;
                            break;
                        case 91008: // Manor Renovation - Left Big Base
                            SkillSystem.GetSkill(Skill.ManorLeftBigBase).CurrentLevel = 1;
                            break;
                        case 91009: // Manor Renovation - Left Big Upper 1
                            SkillSystem.GetSkill(Skill.ManorLeftBigUpper1).CurrentLevel = 1;
                            break;
                        case 91010: // Manor Renovation - Left Big Upper 2
                            SkillSystem.GetSkill(Skill.ManorLeftBigUpper2).CurrentLevel = 1;
                            break;
                        case 91011: // Manor Renovation - Left Big Windows
                            SkillSystem.GetSkill(Skill.ManorLeftBigWindows).CurrentLevel = 1;
                            break;
                        case 91012: // Manor Renovation - Left Big Rooftop
                            SkillSystem.GetSkill(Skill.ManorLeftBigRoof).CurrentLevel = 1;
                            break;
                        case 91013: // Manor Renovation - Left Far Base
                            SkillSystem.GetSkill(Skill.ManorLeftFarBase).CurrentLevel = 1;
                            break;
                        case 91014: // Manor Renovation - Left Far Roof
                            SkillSystem.GetSkill(Skill.ManorLeftFarRoof).CurrentLevel = 1;
                            break;
                        case 91015: // Manor Renovation - Left Extension
                            SkillSystem.GetSkill(Skill.ManorLeftExtension).CurrentLevel = 1;
                            break;
                        case 91016: // Manor Renovation - Left Tree 1
                            SkillSystem.GetSkill(Skill.ManorLeftTree1).CurrentLevel = 1;
                            break;
                        case 91017: // Manor Renovation - Left Tree 2
                            SkillSystem.GetSkill(Skill.ManorLeftTree2).CurrentLevel = 1;
                            break;
                        case 91018: // Manor Renovation - Right Wing Base
                            SkillSystem.GetSkill(Skill.ManorRightWingBase).CurrentLevel = 1;
                            break;
                        case 91019: // Manor Renovation - Right Wing Window
                            SkillSystem.GetSkill(Skill.ManorRightWingWindow).CurrentLevel = 1;
                            break;
                        case 91020: // Manor Renovation - Right Wing Rooftop
                            SkillSystem.GetSkill(Skill.ManorRightWingRoof).CurrentLevel = 1;
                            break;
                        case 91021: // Manor Renovation - Right Big Base
                            SkillSystem.GetSkill(Skill.ManorRightBigBase).CurrentLevel = 1;
                            break;
                        case 91022: // Manor Renovation - Right Big Upper
                            SkillSystem.GetSkill(Skill.ManorRightBigUpper).CurrentLevel = 1;
                            break;
                        case 91023: // Manor Renovation - Right Big Rooftop
                            SkillSystem.GetSkill(Skill.ManorRightBigRoof).CurrentLevel = 1;
                            break;
                        case 91024: // Manor Renovation - Right High Base
                            SkillSystem.GetSkill(Skill.ManorRightHighBase).CurrentLevel = 1;
                            break;
                        case 91025: // Manor Renovation - Right High Upper
                            SkillSystem.GetSkill(Skill.ManorRightHighUpper).CurrentLevel = 1;
                            break;
                        case 91026: // Manor Renovation - Right High Tower
                            SkillSystem.GetSkill(Skill.ManorRightHighTower).CurrentLevel = 1;
                            break;
                        case 91027: // Manor Renovation - Right Extension
                            SkillSystem.GetSkill(Skill.ManorRightExtension).CurrentLevel = 1;
                            break;
                        case 91028: // Manor Renovation - Right Tree
                            SkillSystem.GetSkill(Skill.ManorRightTree).CurrentLevel = 1;
                            break;
                        case 91029: // Manor Renovation - Observatory Base
                            SkillSystem.GetSkill(Skill.ManorObservatoryBase).CurrentLevel = 1;
                            break;
                        case 91030: // Manor Renovation - Observatory Telescope
                            SkillSystem.GetSkill(Skill.ManorObservatoryTelescope).CurrentLevel = 1;
                            break;
                    }
                }

                // Mark that we finished updating our locations.
                ArchipelagoManager.CheckedLocationsUpdated = false;
            }

            // Check for received items and send to player.
            if (ArchipelagoManager.ItemQueue.Count > 0)
            {
                if (ScreenManager.Player is { ControlsLocked: false } && ScreenManager.CurrentScreen is ProceduralLevelScreen)
                {
                    var item = ArchipelagoManager.ItemQueue.Dequeue();

                    // Only give item if we haven't received it before!
                    if (PlayerStats.CheckReceived(item))
                    {
                        var randomGold = 0;
                        if (item.Item == ItemDefinitions.Gold1000.Code)
                        {
                            randomGold = 1000;
                        }
                        else if (item.Item == ItemDefinitions.Gold3000.Code)
                        {
                            randomGold = 3000;
                        }
                        else if (item.Item == ItemDefinitions.Gold5000.Code)
                        {
                            randomGold = 5000;
                        }

                        var stat1 = CDGMath.RandomInt(4, 9);
                        var stat2 = CDGMath.RandomInt(4, 9);
                        var stat3 = CDGMath.RandomInt(4, 9);

                        var data = new List<object>
                        {
                            new Vector2(ScreenManager.Player.X, ScreenManager.Player.Y),
                            ItemCategory.ReceiveNetworkItem,
                            new Vector2(stat1, randomGold),
                            new Vector2(stat2, stat3),
                            ArchipelagoManager.GetPlayerName(item.Player),
                            item.Item
                        };

                        DisgustingGetItemLogic(item, randomGold, stat1, stat2, stat3);
                        ScreenManager.DisplayScreen((int) Screen.GetItem, true, data);
                        ScreenManager.Player.RunGetItemAnimation();
                    }
                }
            }

            // Death Link handling logic.
            if (ArchipelagoManager.ItemQueue.Count == 0 && ArchipelagoManager.DeathLink != null)
            {
                if (ScreenManager.Player != null && !ScreenManager.Player.ControlsLocked
                                                 && ScreenManager.CurrentScreen is ProceduralLevelScreen &&
                                                 !PlayerStats.IsDead)
                {
                    ScreenManager.Player.AttachedLevel.SetObjectKilledPlayer(
                        new DeathLinkObj(ArchipelagoManager.DeathLink.Source));
                    ScreenManager.Player.Kill();
                    ArchipelagoManager.ClearDeathLink();
                }
            }

            base.Update(gameTime);
        }

        private void DisgustingGetItemLogic(NetworkItem item, int randomGold, params int[] stats)
        {
            SkillObj skill;
            if (item.Item == ItemDefinitions.Blacksmith.Code)
            {
                skill = SkillSystem.GetSkill(Skill.Smithy);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.Enchantress.Code)
            {
                skill = SkillSystem.GetSkill(Skill.Enchanter);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.Architect.Code)
            {
                skill = SkillSystem.GetSkill(Skill.Architect);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.Paladin.Code)
            {
                skill = SkillSystem.GetSkill(Skill.KnightUp);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.Archmage.Code)
            {
                skill = SkillSystem.GetSkill(Skill.MageUp);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.BarbarianKing.Code)
            {
                skill = SkillSystem.GetSkill(Skill.BarbarianUp);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.Assassin.Code)
            {
                skill = SkillSystem.GetSkill(Skill.AssassinUp);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.ProgressiveShinobi.Code)
            {
                skill = SkillSystem.GetSkill(Skill.NinjaUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(Skill.NinjaUp);
                    skill.CanPurchase = true;
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
                else
                {
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
            }
            else if (item.Item == ItemDefinitions.ProgressiveMiner.Code)
            {
                skill = SkillSystem.GetSkill(Skill.BankerUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(Skill.BankerUp);
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
                else
                {
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
            }
            else if (item.Item == ItemDefinitions.ProgressiveLich.Code)
            {
                skill = SkillSystem.GetSkill(Skill.LichUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(Skill.LichUp);
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
                else
                {
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
            }
            else if (item.Item == ItemDefinitions.ProgressiveSpellthief.Code)
            {
                skill = SkillSystem.GetSkill(Skill.SpellswordUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(Skill.SpellSwordUp);
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
                else
                {
                    SkillSystem.LevelUpTrait(skill, false, false);
                }
            }
            else if (item.Item == ItemDefinitions.Dragon.Code)
            {
                skill = SkillSystem.GetSkill(Skill.SuperSecret);
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.Traitors.Code)
            {
                skill = SkillSystem.GetSkill(Skill.Traitorous);
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.HealthUp.Code)
            {
                var maxHp = ScreenManager.Player.MaxHealth;
                skill = SkillSystem.GetSkill(Skill.HealthUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                ScreenManager.Player.CurrentHealth += ScreenManager.Player.MaxHealth - maxHp;
            }
            else if (item.Item == ItemDefinitions.ManaUp.Code)
            {
                var maxMp = ScreenManager.Player.MaxMana;
                skill = SkillSystem.GetSkill(Skill.ManaUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                ScreenManager.Player.CurrentMana += ScreenManager.Player.MaxMana - maxMp;
            }
            else if (item.Item == ItemDefinitions.AttackUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.AttackUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.MagicDamageUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.MagicDamageUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.ArmorUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.ArmorUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.EquipUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.EquipUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.CritChanceUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.CritChanceUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.CritDamageUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.CritDamageUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.DownStrikeUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.DownStrikeUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.GoldGainUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.GoldGainUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.PotionEfficiencyUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.PotionUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.InvulnTimeUp.Code)
            {
                skill = SkillSystem.GetSkill(Skill.InvulnerabilityTimeUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.ManaCostDown.Code)
            {
                skill = SkillSystem.GetSkill(Skill.ManaCostDown);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.DeathDefiance.Code)
            {
                skill = SkillSystem.GetSkill(Skill.DeathDodge);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.Haggling.Code)
            {
                skill = SkillSystem.GetSkill(Skill.PricesDown);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
            }
            else if (item.Item == ItemDefinitions.RandomizeChildren.Code)
            {
                skill = SkillSystem.GetSkill(Skill.RandomizeChildren);
                SkillSystem.LevelUpTrait(skill, false, false);
            }
            else if (item.Item == ItemDefinitions.VaultRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Vault] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Vault] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Vault] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Vault] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Vault] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Vault] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Vault] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Vault] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Vault] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Vault] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SprintRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Sprint] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Sprint] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Sprint] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Sprint] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Sprint] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Sprint] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Sprint] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Sprint] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Sprint] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Sprint] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.VampireRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Vampire] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Vampire] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Vampire] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Vampire] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Vampire] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Vampire] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Vampire] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Vampire] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Vampire] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Vampire] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SkyRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Sky] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Sky] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Sky] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Sky] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Sky] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Sky] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Sky] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Sky] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Sky] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Sky] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SiphonRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Siphon] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Siphon] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Siphon] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Siphon] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Siphon] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Siphon] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Siphon] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Siphon] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Siphon] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Siphon] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.RetaliationRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Retaliation] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Retaliation] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Retaliation] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Retaliation] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Retaliation] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Retaliation] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Retaliation] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Retaliation] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Retaliation] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][ (int) EquipmentAbility.Retaliation] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.BountyRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Bounty] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Bounty] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Bounty] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Bounty] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Bounty] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Bounty] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][ (int) EquipmentAbility.Bounty] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Bounty] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Bounty] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Bounty] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.HasteRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Haste] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Haste] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Haste] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Haste] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Haste] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Haste] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Haste] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Haste] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Haste] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Haste] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.CurseRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Curse] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Curse] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Curse] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Curse] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Curse] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Curse] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Curse] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Curse] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Curse] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Curse] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.GraceRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Grace] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Grace] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Grace] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Grace] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Grace] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Grace] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Grace] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Grace] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Grace] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Grace] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.BalanceRunes.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Balance] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Balance] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Balance] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Balance] = 1;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Balance] = 1;
                }
                else
                {
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Balance] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Balance] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Balance] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Balance] = 3;
                    PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Balance] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SquireArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Squire] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Squire] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Squire] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Squire] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Squire] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Squire] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Squire] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Squire] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Squire] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Squire] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SilverArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Silver] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Silver] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Silver] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Silver] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Silver] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Silver] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Silver] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Silver] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Silver] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Silver] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.GuardianArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Guardian] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Guardian] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Guardian] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Guardian] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Guardian] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Guardian] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Guardian] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Guardian] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Guardian] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Guardian] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.ImperialArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Imperial] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Imperial] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Imperial] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Imperial] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Imperial] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Imperial] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Imperial] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Imperial] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Imperial] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Imperial] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.RoyalArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Royal] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Royal] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Royal] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Royal] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Royal] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Royal] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Royal] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Royal] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Royal] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Royal] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.KnightArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Knight] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Knight] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Knight] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Knight] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Knight] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Knight] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Knight] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Knight] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Knight] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Knight] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.RangerArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Ranger] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Ranger] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Ranger] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Ranger] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Ranger] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Ranger] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Ranger] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Ranger] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Ranger] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Ranger] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SkyArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Sky] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Sky] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Sky] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Sky] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Sky] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Sky] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Sky] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Sky] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Sky] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Sky] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.DragonArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Dragon] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Dragon] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Dragon] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Dragon] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Dragon] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Dragon] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Dragon] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Dragon] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Dragon] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Dragon] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SlayerArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Slayer] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Slayer] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Slayer] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Slayer] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Slayer] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Slayer] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Slayer] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Slayer] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Slayer] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Slayer] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.BloodArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Blood] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Blood] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Blood] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Blood] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Blood] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Blood] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Blood] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Blood] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Blood] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Blood] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.SageArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Sage] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Sage] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Sage] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Sage] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Sage] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Sage] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Sage] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Sage] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Sage] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Sage] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.RetributionArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Retribution] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Retribution] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Retribution] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Retribution] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Retribution] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Retribution] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Retribution] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Retribution] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Retribution] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Retribution] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.HolyArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Holy] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Holy] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Holy] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Holy] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Holy] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Holy] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Holy] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Holy] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Holy] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Holy] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.DarkArmor.Code)
            {
                if (ArchipelagoManager.Data.RequirePurchasing)
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Dark] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Dark] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Dark] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Dark] = 1;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Dark] = 1;
                }
                else
                {
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Dark] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Dark] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Dark] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Dark] = 3;
                    PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Dark] = 3;
                }
            }
            else if (item.Item == ItemDefinitions.TripStatIncrease.Code)
            {
                var iterations = 3;
                for (var i = 0; i < iterations; i++)
                {
                    if (stats[i] == 4)
                    {
                        PlayerStats.BonusStrength++;
                        continue;
                    }

                    if (stats[i] == 5)
                    {
                        PlayerStats.BonusMagic++;
                        continue;
                    }

                    if (stats[i] == 6)
                    {
                        PlayerStats.BonusDefense++;
                        continue;
                    }

                    if (stats[i] == 7)
                    {
                        PlayerStats.BonusHealth++;
                        continue;
                    }

                    if (stats[i] == 8)
                    {
                        PlayerStats.BonusMana++;
                        continue;
                    }

                    // stats[i] == 9
                    PlayerStats.BonusWeight++;
                }
            }
            else if (item.Item == ItemDefinitions.Gold1000.Code || item.Item == ItemDefinitions.Gold3000.Code ||
                     item.Item == ItemDefinitions.Gold5000.Code)
            {
                PlayerStats.Gold += randomGold;
            }
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
            using var streamReader = new StreamReader(@"Content\HeroNames.txt");
            var spriteFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
            SpriteFontArray.SpriteFontList.Add(spriteFont);
            var textObj = new TextObj(spriteFont);

            while (!streamReader.EndOfStream)
            {
                var text = streamReader.ReadLine();
                var error = false;
                try
                {
                    textObj.Text = text;
                }
                catch
                {
                    error = true;
                }

                if (text != null && !text.Contains("//") && !error)
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

        private void InitializeFemaleNameArray()
        {
            FemaleNameArray = new List<string>();
            using var streamReader = new StreamReader(@"Content\HeroineNames.txt");
            var spriteFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
            SpriteFontArray.SpriteFontList.Add(spriteFont);
            var textObj = new TextObj(spriteFont);

            while (!streamReader.EndOfStream)
            {
                var text = streamReader.ReadLine();
                var error = false;
                try
                {
                    textObj.Text = text;
                }
                catch
                {
                    error = true;
                }

                if (text != null && !text.Contains("//") && !error)
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

        public void SaveOnExit()
        {
            // No point in saving if we're on the Splash or Demo screens.
            if (ScreenManager.CurrentScreen is CDGSplashScreen or DemoStartScreen)
            {
                return;
            }

            UpdatePlaySessionLength();
            var screen = ScreenManager.GetLevelScreen();
            if (screen != null)
            {
                var currentRoom = screen.CurrentRoom;
                if (currentRoom is CarnivalShoot1BonusRoom or CarnivalShoot2BonusRoom)
                {
                    if (currentRoom is CarnivalShoot1BonusRoom bonusRoom1)
                    {
                        bonusRoom1.UnequipPlayer();
                    }

                    if (currentRoom is CarnivalShoot2BonusRoom bonusRoom2)
                    {
                        bonusRoom2.UnequipPlayer();
                    }
                }

                if (currentRoom is ChallengeBossRoomObj challengeBossRoom)
                {
                    challengeBossRoom.LoadPlayerData();
                    SaveManager.LoadFiles(ScreenManager.GetLevelScreen(), SaveType.UpgradeData);
                    currentRoom.Player.CurrentHealth = challengeBossRoom.StoredHP;
                    currentRoom.Player.CurrentMana = challengeBossRoom.StoredMP;
                }
            }

            if (ScreenManager.CurrentScreen is GameOverScreen)
            {
                PlayerStats.Traits = Vector2.Zero;
            }

            if (SaveManager.FileExists(SaveType.PlayerData))
            {
                SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
                if (screen is { CurrentRoom: null })
                {
                    return;
                }

                if (screen != null && screen.CurrentRoom.Name != "Start" && screen.CurrentRoom.Name != "Ending" && screen.CurrentRoom.Name != "Tutorial")
                {
                    SaveManager.SaveFiles(SaveType.MapData);
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
                if (current.Width >= 2000 || current.Height >= 2000)
                {
                    continue;
                }

                var item = new Vector2(current.Width, current.Height);
                if (!list.Contains(item))
                {
                    list.Add(new Vector2(current.Width, current.Height));
                }
            }

            return list;
        }

        public void SaveConfig()
        {
            Console.WriteLine("Saving Config file");
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dirPath = Path.Combine(folderPath, LevelENV.GameName);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var iniPath = Path.Combine(folderPath, LevelENV.GameName, "GameConfig.ini");
            using var streamWriter = new StreamWriter(iniPath, false);

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
            streamWriter.WriteLine("MusicVol=" + $"{GameConfig.MusicVolume:F2}");
            streamWriter.WriteLine("SFXVol=" + $"{GameConfig.SFXVolume:F2}");
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
            streamWriter.WriteLine("Profile=DEFAULT");
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

        public void LoadConfig()
        {
            Console.WriteLine("Loading Config file");
            InitializeDefaultConfig();
            try
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Path.Combine(folderPath, LevelENV.GameName, "GameConfig.ini");
                using var streamReader = new StreamReader(path);
                var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
                cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
                string text;

                while ((text = streamReader.ReadLine()) != null)
                {
                    var separatorIndex = text.IndexOf("=", StringComparison.Ordinal);
                    if (separatorIndex != -1)
                    {
                        var option = text.Substring(0, separatorIndex);
                        var setting = text.Substring(separatorIndex + 1);
                        switch (option)
                        {
                            case "ScreenWidth":
                                GameConfig.ScreenWidth = int.Parse(setting, NumberStyles.Any, cultureInfo);
                                break;

                            case "ScreenHeight":
                                GameConfig.ScreenHeight = int.Parse(setting, NumberStyles.Any, cultureInfo);
                                break;

                            case "Fullscreen":
                                GameConfig.FullScreen = bool.Parse(setting);
                                break;

                            case "QuickDrop":
                                GameConfig.QuickDrop = bool.Parse(setting);
                                break;

                            case "MusicVol":
                                GameConfig.MusicVolume = float.Parse(setting);
                                break;

                            case "SFXVol":
                                GameConfig.SFXVolume = float.Parse(setting);
                                break;

                            case "DeadZone":
                                InputManager.Deadzone = int.Parse(setting, NumberStyles.Any, cultureInfo);
                                break;

                            case "EnableDirectInput":
                                GameConfig.EnableDirectInput = bool.Parse(setting);
                                break;

                            case "ReduceQuality":
                                GameConfig.ReduceQuality = bool.Parse(setting);
                                LevelENV.SaveFrames = GameConfig.ReduceQuality;
                                break;

                            case "EnableSteamCloud":
                                GameConfig.EnableSteamCloud = bool.Parse(setting);
                                break;

                            case "KeyUP":
                                GlobalInput.KeyList[16] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyDOWN":
                                GlobalInput.KeyList[18] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyLEFT":
                                GlobalInput.KeyList[20] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyRIGHT":
                                GlobalInput.KeyList[22] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyATTACK":
                                GlobalInput.KeyList[12] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyJUMP":
                                GlobalInput.KeyList[10] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeySPECIAL":
                                GlobalInput.KeyList[13] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyDASHLEFT":
                                GlobalInput.KeyList[14] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeyDASHRIGHT":
                                GlobalInput.KeyList[15] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "KeySPELL1":
                                GlobalInput.KeyList[24] = (Keys) Enum.Parse(typeof(Keys), setting);
                                break;

                            case "ButtonUP":
                                GlobalInput.ButtonList[16] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonDOWN":
                                GlobalInput.ButtonList[18] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonLEFT":
                                GlobalInput.ButtonList[20] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonRIGHT":
                                GlobalInput.ButtonList[22] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonATTACK":
                                GlobalInput.ButtonList[12] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonJUMP":
                                GlobalInput.ButtonList[10] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonSPECIAL":
                                GlobalInput.ButtonList[13] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonDASHLEFT":
                                GlobalInput.ButtonList[14] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonDASHRIGHT":
                                GlobalInput.ButtonList[15] = (Buttons) Enum.Parse(typeof(Buttons), setting);
                                break;

                            case "ButtonSPELL1":
                                GlobalInput.ButtonList[24] = (Buttons) Enum.Parse(typeof(Buttons), setting);
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
            catch
            {
                Console.WriteLine("Config File Not Found. Creating Default Config File.");
                InitializeDefaultConfig();
                SaveConfig();
            }
        }

        public void InitializeScreenConfig()
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = GameConfig.ScreenWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = GameConfig.ScreenHeight;
            if (GraphicsDeviceManager.IsFullScreen && !GameConfig.FullScreen || !GraphicsDeviceManager.IsFullScreen && GameConfig.FullScreen)
            {
                GraphicsDeviceManager.ToggleFullScreen();
            }
            else
            {
                GraphicsDeviceManager.ApplyChanges();
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
            public bool QuickDrop;
            public bool ReduceQuality;
            public int ScreenHeight;
            public int ScreenWidth;
            public float SFXVolume;
        }
    }
}