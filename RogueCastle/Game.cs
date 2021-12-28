// 
// RogueLegacyArchipelago - Game.cs
// Last Modified 2021-12-27
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
using Archipelago;
using Archipelago.Legacy;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.Screens;
using RogueCastle.Structs;
using SpriteSystem;
using Tweener;

using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RogueCastle
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static AreaStruct[]  Area1List;
        public static SettingStruct GameConfig;

        public static Cue             LineageSongCue       { get; set; }
        public static RCScreenManager ScreenManager        { get; set; }
        public static Effect          BWMaskEffect         { get; set; }
        public static Effect          ColourSwapShader     { get; set; }
        public static Effect          HSVEffect            { get; set; }
        public static Effect          InvertShader         { get; set; }
        public static Effect          MaskEffect           { get; set; }
        public static Effect          ParallaxEffect       { get; set; }
        public static Effect          RippleEffect         { get; set; }
        public static Effect          ShadowEffect         { get; set; }
        public static EquipmentSystem EquipmentSystem      { get; set; }
        public static GaussianBlur    GaussianBlur         { get; set; }
        public static PlayerStats     PlayerStats          { get; set; }
        public static InputMap        GlobalInput          { get; set; }
        public static List<string>    NameArray            { get; set; }
        public static List<string>    FemaleNameArray      { get; set; }
        public static SpriteFont      PixelArtFont         { get; set; }
        public static SpriteFont      PixelArtFontBold     { get; set; }
        public static SpriteFont      JunicodeFont         { get; set; }
        public static SpriteFont      EnemyLevelFont       { get; set; }
        public static SpriteFont      PlayerLevelFont      { get; set; }
        public static SpriteFont      GoldFont             { get; set; }
        public static SpriteFont      HerzogFont           { get; set; }
        public static SpriteFont      JunicodeLargeFont    { get; set; }
        public static SpriteFont      CinzelFont           { get; set; }
        public static SpriteFont      BitFont              { get; set; }
        public static Texture2D       GenericTexture       { get; set; }
        public static float           PlaySessionLength    { get; set; }
        public static float           TotalGameTimeSeconds { get; set; }
        public static float           TotalGameTimeHours   { get; set; }
        public static string          ProfileName          { get; set; }

        public ArchipelagoClient     ArchipelagoManager    { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public PhysicsManager        PhysicsManager        { get; private set; }
        public SaveGameManager       SaveManager           { get; private set; }

        private       GameTime m_forcedGameTime2;
        private       GameTime m_forcedGameTime1;
        private       bool     m_contentLoaded;
        private       bool     m_frameLimitSwap;
        private       bool     m_gameLoaded;

        public Game()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
            }

            ArchipelagoManager = new ArchipelagoClient();
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            ScreenManager = new RCScreenManager(this);
            SaveManager = new SaveGameManager();
            PhysicsManager = new PhysicsManager();
            EquipmentSystem = new EquipmentSystem();

            PlayerStats = new PlayerStats();
            Content.RootDirectory = "Content";
            EngineEV.ScreenWidth = 1320;
            EngineEV.ScreenHeight = 720;
            Window.Title = "Rogue Legacy Archipelago";
            IsFixedTimeStep = false;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = !LevelENV.ShowFps;
            Window.AllowUserResizing = false;

            if (!LevelENV.EnableOffscreenControl)
            {
                InactiveSleepTime = default(TimeSpan);
            }

            EquipmentSystem.InitializeEquipmentData();
            EquipmentSystem.InitializeAbilityCosts();
            GameConfig = default(SettingStruct);

            var form = Control.FromHandle(Window.Handle) as Form;
            if (form != null)
                form.FormClosing += FormClosing;

            GraphicsDeviceManager.PreparingDeviceSettings += ChangeGraphicsSettings;
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
            DialogueManager.LoadLanguageBinFile("Content\\Languages\\Text_En.bin");
            DialogueManager.LoadLanguageBinFile("Content\\Languages\\Diary_En.bin");
            DialogueManager.SetLanguage("English");

            ProfileName = "DEFAULT";
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

            m_forcedGameTime1 = new GameTime(default(TimeSpan), new TimeSpan(0, 0, 0, 0, (int) (LevelENV.FrameLimit * 1000f)));
            m_forcedGameTime2 = new GameTime(default(TimeSpan), new TimeSpan(0, 0, 0, 0, (int) (LevelENV.FrameLimit * 1050f)));
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
                GlobalInput.ClearAll();
            else
                GlobalInput = new InputMap(PlayerIndex.One, true);

            GlobalInput.AddInput(InputMapType.MenuConfirm1, Keys.Enter);
            GlobalInput.AddInput(InputMapType.MenuCancel1, Keys.Escape);
            GlobalInput.AddInput(InputMapType.MenuCredits, Keys.LeftControl);
            GlobalInput.AddInput(InputMapType.MenuOptions, Keys.Tab);
            GlobalInput.AddInput(InputMapType.MenuProfileCard, Keys.LeftShift);
            GlobalInput.AddInput(InputMapType.MenuRogueMode, Keys.Back);
            GlobalInput.AddInput(InputMapType.MenuPause, Keys.Escape);
            GlobalInput.AddInput(InputMapType.MenuMap, Keys.Tab);
            GlobalInput.AddInput(InputMapType.PlayerJump1, Keys.S);
            GlobalInput.AddInput(InputMapType.PlayerJump2, Keys.Space);
            GlobalInput.AddInput(InputMapType.PlayerSpell1, Keys.W);
            GlobalInput.AddInput(InputMapType.PlayerAttack, Keys.D);
            GlobalInput.AddInput(InputMapType.PlayerBlock, Keys.A);
            GlobalInput.AddInput(InputMapType.PlayerDashLeft, Keys.Q);
            GlobalInput.AddInput(InputMapType.PlayerDashRight, Keys.E);
            GlobalInput.AddInput(InputMapType.PlayerUp1, Keys.I);
            GlobalInput.AddInput(InputMapType.PlayerUp2, Keys.Up);
            GlobalInput.AddInput(InputMapType.PlayerDown1, Keys.K);
            GlobalInput.AddInput(InputMapType.PlayerDown2, Keys.Down);
            GlobalInput.AddInput(InputMapType.PlayerLeft1, Keys.J);
            GlobalInput.AddInput(InputMapType.PlayerLeft2, Keys.Left);
            GlobalInput.AddInput(InputMapType.PlayerRight1, Keys.L);
            GlobalInput.AddInput(InputMapType.PlayerRight2, Keys.Right);
            GlobalInput.AddInput(InputMapType.MenuConfirm1, Buttons.A);
            GlobalInput.AddInput(InputMapType.MenuConfirm2, Buttons.Start);
            GlobalInput.AddInput(InputMapType.MenuCancel1, Buttons.B);
            GlobalInput.AddInput(InputMapType.MenuCancel2, Buttons.Back);
            GlobalInput.AddInput(InputMapType.MenuCredits, Buttons.RightTrigger);
            GlobalInput.AddInput(InputMapType.MenuOptions, Buttons.Y);
            GlobalInput.AddInput(InputMapType.MenuProfileCard, Buttons.X);
            GlobalInput.AddInput(InputMapType.MenuRogueMode, Buttons.Back);
            GlobalInput.AddInput(InputMapType.MenuPause, Buttons.Start);
            GlobalInput.AddInput(InputMapType.MenuMap, Buttons.Back);
            GlobalInput.AddInput(InputMapType.PlayerJump1, Buttons.A);
            GlobalInput.AddInput(InputMapType.PlayerAttack, Buttons.X);
            GlobalInput.AddInput(InputMapType.PlayerBlock, Buttons.Y);
            GlobalInput.AddInput(InputMapType.PlayerDashLeft, Buttons.LeftTrigger);
            GlobalInput.AddInput(InputMapType.PlayerDashRight, Buttons.RightTrigger);
            GlobalInput.AddInput(InputMapType.PlayerUp1, Buttons.DPadUp);
            GlobalInput.AddInput(InputMapType.PlayerUp2, ThumbStick.LeftStick, -90f, 30f);
            GlobalInput.AddInput(InputMapType.PlayerDown1, Buttons.DPadDown);
            GlobalInput.AddInput(InputMapType.PlayerDown2, ThumbStick.LeftStick, 90f, 37f);
            GlobalInput.AddInput(InputMapType.PlayerLeft1, Buttons.DPadLeft);
            GlobalInput.AddInput(InputMapType.PlayerLeft2, Buttons.LeftThumbstickLeft);
            GlobalInput.AddInput(InputMapType.PlayerRight1, Buttons.DPadRight);
            GlobalInput.AddInput(InputMapType.PlayerRight2, Buttons.LeftThumbstickRight);
            GlobalInput.AddInput(InputMapType.PlayerSpell1, Buttons.B);
            GlobalInput.AddInput(InputMapType.MenuProfileSelect, Keys.Escape);
            GlobalInput.AddInput(InputMapType.MenuProfileSelect, Buttons.Back);
            GlobalInput.AddInput(InputMapType.MenuDeleteProfile, Keys.Back);
            GlobalInput.AddInput(InputMapType.MenuDeleteProfile, Buttons.Y);

            GlobalInput.KeyList[1] = GlobalInput.KeyList[12];
            GlobalInput.KeyList[3] = GlobalInput.KeyList[10];
        }

        public static void ChangeProfile(string seed, int slot)
        {
            ProfileName = string.Format("AP_{0}-{1}", seed, slot);
        }

        protected override void LoadContent()
        {
            if (m_contentLoaded)
                return;

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
                LevelType = LevelType.Castle,
                TotalRooms = new Vector2(24f, 28f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.White,
            };
            var gardenArea = new AreaStruct
            {
                LevelType = LevelType.Garden,
                TotalRooms = new Vector2(23f, 27f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.Green,
            };
            var towerArea = new AreaStruct
            {
                LevelType = LevelType.Tower,
                TotalRooms = new Vector2(23f, 27f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.DarkBlue,
            };
            var dungeonArea = new AreaStruct
            {
                LevelType = LevelType.Dungeon,
                TotalRooms = new Vector2(23f, 27f),
                BossInArea = true,
                SecretRooms = new Vector2(1f, 3f),
                BonusRooms = new Vector2(2f, 3f),
                Color = Color.Red,
            };

            Area1List = new[]
            {
                castleArea,
                gardenArea,
                towerArea,
                dungeonArea,
            };
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
            GaussianBlur = new GaussianBlur(this, 1320, 720)
            {
                Amount = 2f,
                Radius = 7,
            };
            GaussianBlur.ComputeKernel();
            GaussianBlur.ComputeOffsets();
            GaussianBlur.InvertMask = true;
            BWMaskEffect = Content.Load<Effect>("Shaders\\BWMaskShader");
        }

        protected override void Update(GameTime gameTime)
        {
            if (!m_gameLoaded)
            {
                m_gameLoaded = true;
                if (LevelENV.DeleteSaveFile)
                {
                    SaveManager.ClearAllFileTypes(true);
                    SaveManager.ClearAllFileTypes(false);
                }

                ScreenManager.DisplayScreen(LevelENV.LoadSplashScreen
                    ? ScreenType.CDGSplash
                    : ScreenType.Title, true);
            }

            TotalGameTimeSeconds = (float) gameTime.TotalGameTime.TotalSeconds;
            TotalGameTimeHours = (float) gameTime.TotalGameTime.TotalHours;

            if (gameTime.ElapsedGameTime.TotalSeconds > LevelENV.FrameLimit)
            {
                if (!m_frameLimitSwap)
                {
                    m_frameLimitSwap = true;
                    gameTime = m_forcedGameTime1;
                }
                else
                {
                    m_frameLimitSwap = false;
                    gameTime = m_forcedGameTime2;
                }
            }

            SoundManager.Update(gameTime);
            if (IsActive || (!IsActive && LevelENV.EnableOffscreenControl))
            {
                InputManager.Update(gameTime);
            }

            Tween.Update(gameTime);
            ScreenManager.Update(gameTime);
            SoundManager.Update3DSounds();

            // Wait for Arch to say its ready.
            switch (ArchipelagoManager.Status)
            {
                // We're ready!
                case ArchipelagoStatus.Connected:
                {
                    if (!(ScreenManager.CurrentScreen is ArchipelagoScreen))
                        break;

                    // Initialize Save Data
                    ProfileName = string.Format("{0}-{1}", ArchipelagoManager.Data.Seed, ArchipelagoManager.Data.Slot);
                    SaveManager.CreateSaveDirectory();

                    // Load save file.
                    SaveManager.LoadAllFileTypes(null);

                    SoundManager.PlaySound("Game_Start");
                    var newGame = !PlayerStats.CharacterFound;
                    var heroIsDead = PlayerStats.IsDead;
                    var startingRoom = PlayerStats.LoadStartingRoom;

                    if (PlayerStats.IsFemale && !FemaleNameArray.Contains(ArchipelagoManager.Data.Name))
                        FemaleNameArray.Add(ArchipelagoManager.Data.Name);
                    else if (!PlayerStats.IsFemale && !NameArray.Contains(ArchipelagoManager.Data.Name))
                        NameArray.Add(ArchipelagoManager.Data.Name);

                    if (newGame)
                    {
                        PlayerStats.CharacterFound = true;
                        PlayerStats.Gold = 0;
                        PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, 5);
                        PlayerStats.EnemiesKilledInRun.Clear();

                        // Set AP Settings
                        PlayerStats.TimesCastleBeaten = ArchipelagoManager.Data.Difficulty;

                        // Rename Sir Lee to the player's name and initial gender.
                        PlayerStats.IsFemale = ArchipelagoManager.Data.StartingGender == StartingGender.Lady;
                        PlayerStats.PlayerName = string.Format("{1} {0}", ArchipelagoManager.Data.Name, PlayerStats.IsFemale ? "Lady" : "Sir");

                        Program.Game.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
                        ScreenManager.DisplayScreen(ScreenType.StartingRoom, true);
                    }
                    else
                    {
                        if (heroIsDead)
                            ScreenManager.DisplayScreen(ScreenType.Lineage, true);
                        else
                            ScreenManager.DisplayScreen(startingRoom ? ScreenType.StartingRoom : ScreenType.Level, true);
                    }

                    ArchipelagoManager.StartPlaying();
                    SoundManager.StopMusic(0.2f);
                    break;
                }
            }

            // Check for received items and send to player.
            if (ArchipelagoManager.ItemQueue.Count > 0)
            {
                // Check to ensure we're in a safe place to receive.
                if (ScreenManager.Player != null && !ScreenManager.Player.ControlsLocked && ScreenManager.CurrentScreen is ProceduralLevelScreen)
                {
                    var item = ArchipelagoManager.ItemQueue.Dequeue();
                    var data = new List<object>
                    {
                        new Vector2(ScreenManager.Player.X, ScreenManager.Player.Y),
                        GetItemType.ReceiveNetworkItem,
                        new Vector2(CDGMath.RandomInt(4, 9), CDGMath.RandomInt(5, 10) * 1000),
                        new Vector2(CDGMath.RandomInt(4, 9), CDGMath.RandomInt(4, 9)),
                        ArchipelagoManager.GetPlayerName(item.Player),
                        item.Item,
                    };

                    // TODO: Give item here
                    ScreenManager.DisplayScreen(ScreenType.GetItem, true, data);
                }
            }

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
                    var error = false;
                    try
                    {
                        textObj.Text = text;
                    }
                    catch
                    {
                        error = true;
                    }

                    if (!text.Contains("//") && !error)
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
                    var error = false;
                    try
                    {
                        textObj.Text = text;
                    }
                    catch
                    {
                        error = true;
                    }

                    if (!text.Contains("//") && !error)
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
            // No point in saving if we're on the Splash or Demo screens.
            if (ScreenManager.CurrentScreen is CDGSplashScreen || ScreenManager.CurrentScreen is DemoStartScreen)
                return;

            UpdatePlaySessionLength();
            var screen = ScreenManager.GetLevelScreen();
            if (screen != null)
            {
                var currentRoom = screen.CurrentRoom;
                if (currentRoom != null && (currentRoom is CarnivalShoot1BonusRoom || currentRoom is CarnivalShoot2BonusRoom))
                {
                    var bonusRoom1 = currentRoom as CarnivalShoot1BonusRoom;
                    if (bonusRoom1 != null)
                        bonusRoom1.UnequipPlayer();

                    var bonusRoom2 = currentRoom as CarnivalShoot2BonusRoom;
                    if (bonusRoom2 != null)
                        bonusRoom2.UnequipPlayer();
                }

                var challengeBossRoom = currentRoom as ChallengeBossRoomObj;
                if (challengeBossRoom != null)
                {
                    challengeBossRoom.LoadPlayerData();
                    SaveManager.LoadFiles(ScreenManager.GetLevelScreen(), SaveType.UpgradeData);
                    currentRoom.Player.CurrentHealth = challengeBossRoom.StoredHP;
                    currentRoom.Player.CurrentMana = challengeBossRoom.StoredMP;
                }
            }

            if (ScreenManager.CurrentScreen is GameOverScreen)
                PlayerStats.Traits = Vector2.Zero;

            if (SaveManager.FileExists(SaveType.PlayerData))
            {
                SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
                if (screen.CurrentRoom == null)
                    return;

                if (screen.CurrentRoom.Name != "Start" && screen.CurrentRoom.Name != "Ending" && screen.CurrentRoom.Name != "Tutorial")
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
            var dirPath = Path.Combine(folderPath, LevelENV.GameName);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var iniPath = Path.Combine(folderPath, LevelENV.GameName, "GameConfig.ini");
            using (var streamWriter = new StreamWriter(iniPath, false))
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
        }

        public void LoadConfig()
        {
            Console.WriteLine("Loading Config file");
            InitializeDefaultConfig();
            try
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Path.Combine(folderPath, LevelENV.GameName, "GameConfig.ini");
                using (var streamReader = new StreamReader(path))
                {
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
            if ((GraphicsDeviceManager.IsFullScreen && !GameConfig.FullScreen)
             || (!GraphicsDeviceManager.IsFullScreen && GameConfig.FullScreen))
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
            public bool   EnableDirectInput;
            public bool   EnableSteamCloud;
            public bool   FullScreen;
            public float  MusicVolume;
            public bool   QuickDrop;
            public bool   ReduceQuality;
            public int    ScreenHeight;
            public int    ScreenWidth;
            public float  SFXVolume;
        }
    }
}
