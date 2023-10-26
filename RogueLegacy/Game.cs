//  RogueLegacyRandomizer - Game.cs
//  Last Modified 2023-10-26 11:30 AM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Randomizer;
using RogueLegacy.Enums;
using RogueLegacy.GameObjects;
using RogueLegacy.GameObjects.Rooms;
using RogueLegacy.Screens;
using RogueLegacy.Util;
using SpriteSystem;
using Tweener;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RogueLegacy;

public class Game : Microsoft.Xna.Framework.Game
{
    public static AreaStruct[]  Area1List;
    public static SettingStruct GameConfig;
    private       bool          _contentLoaded;
    private       GameTime      _forcedGameTime1;
    private       GameTime      _forcedGameTime2;
    private       bool          _frameLimitSwap;
    private       bool          _gameLoaded;

    public Game()
    {
        if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
        }

        FinalWords = GameEV.FinalWords[CDGMath.RandomInt(0, GameEV.FinalWords.Length - 1)];

        GraphicsDeviceManager = new GraphicsDeviceManager(this);
        ScreenManager = new RCScreenManager(this);
        SaveManager = new SaveGameManager();
        PhysicsManager = new PhysicsManager();
        EquipmentSystem = new EquipmentSystem();
        NextChildItemQueue = new Queue<NetworkItem>();

        PlayerStats = new PlayerStats();
        ItemHandler = new ItemHandler();
        Content.RootDirectory = "Content";
        EngineEV.ScreenWidth = 1320;
        EngineEV.ScreenHeight = 720;
        Window.Title = "Rogue Legacy Randomizer";
        IsFixedTimeStep = false;
        GraphicsDeviceManager.SynchronizeWithVerticalRetrace = !LevelENV.ShowFps;
        Window.AllowUserResizing = true;

        if (!LevelENV.EnableOffscreenControl) InactiveSleepTime = default;

        EquipmentSystem.InitializeEquipmentData();
        EquipmentSystem.InitializeAbilityCosts();
        GameConfig = default;

        if (Control.FromHandle(Window.Handle) is Form form) form.FormClosing += FormClosing;

        GraphicsDeviceManager.PreparingDeviceSettings += ChangeGraphicsSettings;
        SleepUtil.DisableScreensaver();
    }

    public static Cue                   LineageSongCue       { get; set; }
    public static RCScreenManager       ScreenManager        { get; set; }
    public static Effect                BWMaskEffect         { get; set; }
    public static Effect                ColourSwapShader     { get; set; }
    public static Effect                HSVEffect            { get; set; }
    public static Effect                InvertShader         { get; set; }
    public static Effect                MaskEffect           { get; set; }
    public static Effect                ParallaxEffect       { get; set; }
    public static Effect                RippleEffect         { get; set; }
    public static Effect                ShadowEffect         { get; set; }
    public static EquipmentSystem       EquipmentSystem      { get; set; }
    public static GaussianBlur          GaussianBlur         { get; set; }
    public static PlayerStats           PlayerStats          { get; set; }
    public static bool                  Retired              { get; set; }
    public static InputMap              GlobalInput          { get; set; }
    public static List<string>          NameArray            { get; set; }
    public static List<string>          FemaleNameArray      { get; set; }
    public static SpriteFont            PixelArtFont         { get; set; }
    public static SpriteFont            PixelArtFontBold     { get; set; }
    public static SpriteFont            JunicodeFont         { get; set; }
    public static SpriteFont            EnemyLevelFont       { get; set; }
    public static SpriteFont            PlayerLevelFont      { get; set; }
    public static SpriteFont            GoldFont             { get; set; }
    public static SpriteFont            FountainFont         { get; set; }
    public static SpriteFont            HerzogFont           { get; set; }
    public static SpriteFont            JunicodeLargeFont    { get; set; }
    public static SpriteFont            CinzelFont           { get; set; }
    public static SpriteFont            BitFont              { get; set; }
    public static Texture2D             GenericTexture       { get; set; }
    public static float                 PlaySessionLength    { get; set; }
    public static float                 TotalGameTimeSeconds { get; set; }
    public static float                 TotalGameTimeHours   { get; set; }
    public static string                ProfileName          { get; set; }
    public static ItemHandler           ItemHandler          { get; set; }
    public        Queue<NetworkItem>    NextChildItemQueue   { get; set; }
    public static Tuple<string, string> FinalWords           { get; private set; }

    public GraphicsDeviceManager GraphicsDeviceManager { get; }
    public PhysicsManager        PhysicsManager        { get; }
    public SaveGameManager       SaveManager           { get; }
    public ArchipelagoManager    ArchipelagoManager    { get; set; }

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
            GlobalInput.ClearAll();
        else
            GlobalInput = new InputMap(PlayerIndex.One, true);

        GlobalInput.AddInput((int) InputType.MenuConfirm1, Keys.Enter);
        GlobalInput.AddInput((int) InputType.MenuCancel1, Keys.Escape);
        GlobalInput.AddInput((int) InputType.MenuCredits, Keys.LeftControl);
        GlobalInput.AddInput((int) InputType.MenuOptions, Keys.Tab);
        GlobalInput.AddInput((int) InputType.MenuProfileCard, Keys.LeftShift);
        GlobalInput.AddInput((int) InputType.MenuRogueMode, Keys.Back);
        GlobalInput.AddInput((int) InputType.MenuPause, Keys.Escape);
        GlobalInput.AddInput((int) InputType.MenuMap, Keys.Tab);
        GlobalInput.AddInput((int) InputType.PlayerJump1, Keys.S);
        GlobalInput.AddInput((int) InputType.PlayerJump2, Keys.Space);
        GlobalInput.AddInput((int) InputType.PlayerSpell, Keys.W);
        GlobalInput.AddInput((int) InputType.PlayerAttack, Keys.D);
        GlobalInput.AddInput((int) InputType.PlayerBlock, Keys.A);
        GlobalInput.AddInput((int) InputType.PlayerDashLeft, Keys.Q);
        GlobalInput.AddInput((int) InputType.PlayerDashRight, Keys.E);
        GlobalInput.AddInput((int) InputType.PlayerUp1, Keys.I);
        GlobalInput.AddInput((int) InputType.PlayerUp2, Keys.Up);
        GlobalInput.AddInput((int) InputType.PlayerDown1, Keys.K);
        GlobalInput.AddInput((int) InputType.PlayerDown2, Keys.Down);
        GlobalInput.AddInput((int) InputType.PlayerLeft1, Keys.J);
        GlobalInput.AddInput((int) InputType.PlayerLeft2, Keys.Left);
        GlobalInput.AddInput((int) InputType.PlayerRight1, Keys.L);
        GlobalInput.AddInput((int) InputType.PlayerRight2, Keys.Right);
        GlobalInput.AddInput((int) InputType.MenuConfirm1, Buttons.A);
        GlobalInput.AddInput((int) InputType.MenuConfirm2, Buttons.Start);
        GlobalInput.AddInput((int) InputType.MenuCancel1, Buttons.B);
        GlobalInput.AddInput((int) InputType.MenuCancel2, Buttons.Back);
        GlobalInput.AddInput((int) InputType.MenuCredits, Buttons.RightTrigger);
        GlobalInput.AddInput((int) InputType.MenuOptions, Buttons.Y);
        GlobalInput.AddInput((int) InputType.MenuProfileCard, Buttons.X);
        GlobalInput.AddInput((int) InputType.MenuRogueMode, Buttons.Back);
        GlobalInput.AddInput((int) InputType.MenuPause, Buttons.Start);
        GlobalInput.AddInput((int) InputType.MenuMap, Buttons.Back);
        GlobalInput.AddInput((int) InputType.PlayerJump1, Buttons.A);
        GlobalInput.AddInput((int) InputType.PlayerAttack, Buttons.X);
        GlobalInput.AddInput((int) InputType.PlayerBlock, Buttons.Y);
        GlobalInput.AddInput((int) InputType.PlayerDashLeft, Buttons.LeftTrigger);
        GlobalInput.AddInput((int) InputType.PlayerDashRight, Buttons.RightTrigger);
        GlobalInput.AddInput((int) InputType.PlayerUp1, Buttons.DPadUp);
        GlobalInput.AddInput((int) InputType.PlayerUp2, ThumbStick.LeftStick, -90f, 30f);
        GlobalInput.AddInput((int) InputType.PlayerDown1, Buttons.DPadDown);
        GlobalInput.AddInput((int) InputType.PlayerDown2, ThumbStick.LeftStick, 90f, 37f);
        GlobalInput.AddInput((int) InputType.PlayerLeft1, Buttons.DPadLeft);
        GlobalInput.AddInput((int) InputType.PlayerLeft2, Buttons.LeftThumbstickLeft);
        GlobalInput.AddInput((int) InputType.PlayerRight1, Buttons.DPadRight);
        GlobalInput.AddInput((int) InputType.PlayerRight2, Buttons.LeftThumbstickRight);
        GlobalInput.AddInput((int) InputType.PlayerSpell, Buttons.B);
        GlobalInput.AddInput((int) InputType.MenuProfileSelect, Keys.Escape);
        GlobalInput.AddInput((int) InputType.MenuProfileSelect, Buttons.Back);
        GlobalInput.AddInput((int) InputType.MenuDeleteProfile, Keys.Back);
        GlobalInput.AddInput((int) InputType.MenuDeleteProfile, Buttons.Y);

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
        if (_contentLoaded) return;

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
            InputManager.SetPadType(PlayerIndex.One, PadTypes.GamePad);

        // InputManager.UseDirectInput = GameConfig.EnableDirectInput;
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

        AreaStruct castleArea;
        AreaStruct gardenArea;
        AreaStruct towerArea;
        AreaStruct dungeonArea;

        // Safe defaults.
        castleArea = new AreaStruct
        {
            Zone = Zone.Castle,
            TotalRooms = new Vector2(30f, 35f),
            BossInArea = true,
            SecretRooms = new Vector2(1f, 3f),
            BonusRooms = new Vector2(4f, 5f),
            Color = Color.White
        };
        gardenArea = new AreaStruct
        {
            Zone = Zone.Garden,
            TotalRooms = new Vector2(28f, 30f),
            BossInArea = true,
            SecretRooms = new Vector2(1f, 3f),
            BonusRooms = new Vector2(4f, 5f),
            Color = Color.Green
        };
        towerArea = new AreaStruct
        {
            Zone = Zone.Tower,
            TotalRooms = new Vector2(28f, 30f),
            BossInArea = true,
            SecretRooms = new Vector2(1f, 3f),
            BonusRooms = new Vector2(4f, 5f),
            Color = Color.DarkBlue
        };
        dungeonArea = new AreaStruct
        {
            Zone = Zone.Dungeon,
            TotalRooms = new Vector2(28f, 30f),
            BossInArea = true,
            SecretRooms = new Vector2(1f, 3f),
            BonusRooms = new Vector2(4f, 5f),
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
        FountainFont = Content.Load<SpriteFont>(@"Fonts\FountainFont");
        JunicodeFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
        JunicodeLargeFont = Content.Load<SpriteFont>(@"Fonts\JunicodeLarge");
        HerzogFont = Content.Load<SpriteFont>(@"Fonts\HerzogVonGraf24");
        CinzelFont = Content.Load<SpriteFont>(@"Fonts\CinzelFont");
        BitFont = Content.Load<SpriteFont>(@"Fonts\BitFont");

        PixelArtFont.DefaultCharacter = '?';
        PixelArtFontBold.DefaultCharacter = '?';
        EnemyLevelFont.DefaultCharacter = '?';
        PlayerLevelFont.DefaultCharacter = '?';
        GoldFont.DefaultCharacter = '?';
        FountainFont.DefaultCharacter = '?';
        JunicodeFont.DefaultCharacter = '?';
        JunicodeLargeFont.DefaultCharacter = '?';
        HerzogFont.DefaultCharacter = '?';
        CinzelFont.DefaultCharacter = '?';
        BitFont.DefaultCharacter = '?';

        SpriteFontArray.SpriteFontList.Add(PixelArtFont);
        SpriteFontArray.SpriteFontList.Add(PixelArtFontBold);
        SpriteFontArray.SpriteFontList.Add(EnemyLevelFont);
        SpriteFontArray.SpriteFontList.Add(PlayerLevelFont);
        SpriteFontArray.SpriteFontList.Add(GoldFont);
        SpriteFontArray.SpriteFontList.Add(FountainFont);
        SpriteFontArray.SpriteFontList.Add(JunicodeFont);
        SpriteFontArray.SpriteFontList.Add(JunicodeLargeFont);
        SpriteFontArray.SpriteFontList.Add(HerzogFont);
        SpriteFontArray.SpriteFontList.Add(CinzelFont);
        SpriteFontArray.SpriteFontList.Add(BitFont);

        EnemyLevelFont.Spacing = -5f;
        PlayerLevelFont.Spacing = -7f;
        GoldFont.Spacing = -5f;
        FountainFont.Spacing = -5f;
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

            ScreenManager.DisplayScreen(LevelENV.LoadSplashScreen ? (int) ScreenType.CDGSplash : (int) ScreenType.Title,
                true);
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
        if (IsActive || (!IsActive && LevelENV.EnableOffscreenControl)) InputManager.Update(gameTime);

        Tween.Update(gameTime);
        ScreenManager.Update(gameTime);
        SoundManager.Update3DSounds();

        // Do not proceed if ArchipelagoManager is not ready.
        if (ArchipelagoManager == null)
        {
            base.Update(gameTime);
            return;
        }

        // Wait for Arch to say its ready.
        if (ArchipelagoManager.Ready && ScreenManager.CurrentScreen is RandomizerScreen)
        {
            // Initialize Save Data
            ChangeProfile(ArchipelagoManager.Seed, ArchipelagoManager.Slot);

            // Load Area Struct Data
            Area1List = RandomizerData.AreaStructs;

            SoundManager.PlaySound("Game_Start");
            var newGame = !PlayerStats.CharacterFound;
            var heroIsDead = PlayerStats.IsDead;
            var startingRoom = PlayerStats.LoadStartingRoom;

            if (newGame)
            {
                PlayerStats.CharacterFound = true;
                PlayerStats.Gold = 0;
                PlayerStats.Class = RandomizerData.StartingClass;
                PlayerStats.Spell = (byte) ((ClassType) PlayerStats.Class).SpellList()[0];

                // Unlock the player's starting class.
                var skill = (ClassType) RandomizerData.StartingClass switch
                {
                    ClassType.Knight        => SkillSystem.GetSkill(SkillType.KnightUnlock),
                    ClassType.Mage          => SkillSystem.GetSkill(SkillType.MageUnlock),
                    ClassType.Barbarian     => SkillSystem.GetSkill(SkillType.BarbarianUnlock),
                    ClassType.Knave         => SkillSystem.GetSkill(SkillType.AssassinUnlock),
                    ClassType.Miner         => SkillSystem.GetSkill(SkillType.BankerUnlock),
                    ClassType.Shinobi       => SkillSystem.GetSkill(SkillType.NinjaUnlock),
                    ClassType.Lich          => SkillSystem.GetSkill(SkillType.LichUnlock),
                    ClassType.Spellthief    => SkillSystem.GetSkill(SkillType.SpellswordUnlock),
                    ClassType.Dragon        => SkillSystem.GetSkill(SkillType.SuperSecret),
                    ClassType.Traitor       => SkillSystem.GetSkill(SkillType.Traitorous),
                    _                       => throw new ArgumentException("Unsupported Starting Class"),
                };

                skill.MaxLevel = 1;
                SkillSystem.LevelUpTrait(skill, false, false);

                PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, 5);
                PlayerStats.EnemiesKilledInRun.Clear();

                // Set AP Settings
                PlayerStats.TimesCastleBeaten = RandomizerData.NewGamePlus;

                // Set the player's initial gender.
                PlayerStats.IsFemale = RandomizerData.StartingGender == Gender.Lady;

                SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
                ScreenManager.DisplayScreen((int) ScreenType.StartingRoom, true);
            }
            else
            {
                if (heroIsDead)
                {
                    ScreenManager.DisplayScreen((int) ScreenType.Lineage, true);
                }
                else
                {
                    ScreenManager.DisplayScreen(
                        startingRoom ? (int) ScreenType.StartingRoom : (int) ScreenType.Level,
                        true);
                }
            }

            SoundManager.StopMusic(0.2f);
        }

        // Check for received items and send to player.
        if (ScreenManager.Player is { ControlsLocked: false } && ScreenManager.CurrentScreen is ProceduralLevelScreen)
        {
            ItemHandler.CheckReceivedItemQueue();
        }

        // Death Link handling logic.
        if (ArchipelagoManager.ItemQueue.Count == 0 && ArchipelagoManager.DeathLinkData != null)
        {
            if (ScreenManager.Player is { ControlsLocked: false } &&
                ScreenManager.CurrentScreen is ProceduralLevelScreen screen && !PlayerStats.IsDead &&
                ArchipelagoManager.IsDeathLinkSafe)
            {
                if (PlayerStats.SpecialItem == 3)
                {
                    ScreenManager.Player.CurrentHealth = (int) (ScreenManager.Player.MaxHealth * 0.25f);
                    PlayerStats.SpecialItem = 0;
                    screen.UpdatePlayerHUDSpecialItem();
                    ScreenManager.DisplayScreen(21, true);
                }
                else
                {
                    var num6 = CDGMath.RandomInt(1, 100);
                    if (num6 <= SkillSystem.GetSkill(SkillType.DeathDodge).ModifierAmount * 100f)
                    {
                        ScreenManager.Player.CurrentHealth = (int) (ScreenManager.Player.MaxHealth * 0.25f);
                        PlayerStats.SpecialItem = 0;
                        (ScreenManager.CurrentScreen as ProceduralLevelScreen)!.UpdatePlayerHUDSpecialItem();
                        ScreenManager.DisplayScreen(21, true);
                    }
                    else
                    {
                        ScreenManager.Player.AttachedLevel.SetObjectKilledPlayer(
                            new DeathLinkObj(ArchipelagoManager.DeathLinkData.Source));
                        ScreenManager.Player.Kill();
                    }
                }

                ArchipelagoManager.ClearDeathLink();
            }
        }

        // Retire handling Logic.
        if (Retired
            && ScreenManager.Player is { ControlsLocked: false }
            && ScreenManager.CurrentScreen is ProceduralLevelScreen
            && !PlayerStats.IsDead)
        {
            ScreenManager.Player.AttachedLevel.SetObjectKilledPlayer(new RetireObj());
            ScreenManager.Player.Kill();
            Retired = false;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        ScreenManager.Draw(gameTime);
        base.Draw(gameTime);
    }

    private void InitializeNameArray(bool allowDefaults = true, IEnumerable<string> extraNames = null)
    {
        NameArray = new List<string>();
        using var streamReader = new StreamReader(@"Content\HeroNames.txt");
        var spriteFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
        var temp = SpriteFontArray.SpriteFontList;
        SpriteFontArray.SpriteFontList = new List<SpriteFont> { spriteFont };
        var textObj = new TextObj(spriteFont);

        while (!streamReader.EndOfStream && allowDefaults)
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

            if (text != null && !text.Contains("//") && !error) NameArray.Add(text);
        }

        if (extraNames != null) NameArray = NameArray.Concat(extraNames).ToList();

        if (NameArray.Count < 1)
        {
            NameArray.Add("Lee");
            NameArray.Add("Charles");
            NameArray.Add("Lancelot");
            NameArray.Add("Zachary");
            NameArray.Add("Travis");
        }

        textObj.Dispose();
        SpriteFontArray.SpriteFontList = temp;
    }

    private void InitializeFemaleNameArray(bool allowDefaults = true, IEnumerable<string> extraNames = null)
    {
        FemaleNameArray = new List<string>();
        using var streamReader = new StreamReader(@"Content\HeroineNames.txt");
        var spriteFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
        var temp = SpriteFontArray.SpriteFontList;
        SpriteFontArray.SpriteFontList = new List<SpriteFont> { spriteFont };
        var textObj = new TextObj(spriteFont);

        while (!streamReader.EndOfStream && allowDefaults)
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

            if (text != null && !text.Contains("//") && !error) FemaleNameArray.Add(text);
        }

        if (extraNames != null) FemaleNameArray = FemaleNameArray.Concat(extraNames).ToList();

        if (FemaleNameArray.Count < 1)
        {
            FemaleNameArray.Add("Jenny");
            FemaleNameArray.Add("Shanoa");
            FemaleNameArray.Add("Chun Li");
            FemaleNameArray.Add("Sasha");
            FemaleNameArray.Add("Mika-mi");
        }

        textObj.Dispose();
        SpriteFontArray.SpriteFontList = temp;
    }

    public void SaveAndReset()
    {
        if (ScreenManager.CurrentScreen is CDGSplashScreen)
        {
            return;
        }

        if (ArchipelagoManager.Ready)
        {
            Program.Game.ArchipelagoManager.Disconnect();
        }

        UpdatePlaySessionLength();
        var screen = ScreenManager.GetLevelScreen();
        if (screen != null)
        {
            var currentRoom = screen.CurrentRoom;
            switch (currentRoom)
            {
                case CarnivalShoot1BonusRoom carnival:
                    carnival.UnequipPlayer();
                    break;
                case CarnivalShoot2BonusRoom carnival:
                    carnival.UnequipPlayer();
                    break;
                case ChallengeBossRoomObj challenge:
                    SaveManager.LoadFiles(ScreenManager.GetLevelScreen(), SaveType.UpgradeData);
                    currentRoom.Player.CurrentHealth = challenge.StoredHP;
                    currentRoom.Player.CurrentMana = challenge.StoredMP;
                    break;
            }
        }

        if (ScreenManager.CurrentScreen is GameOverScreen)
        {
            PlayerStats.Traits = Vector2.Zero;
        }

        if (SaveManager.FileExists(SaveType.PlayerData))
        {
            SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
        }

        if (screen is { CurrentRoom: not null } && !screen.CurrentRoom.Name.EqualsAny("Start", "Ending", "Tutorial"))
        {
            SaveManager.SaveFiles(SaveType.MapData);
        }

        SkillSystem.ResetAllTraits();
        SkillSystem.PreInitialize();
        SkillSystem.Initialize();
        PlayerStats.Dispose();
        PlayerStats = new();
        ItemHandler.ReceivedItems = new();
        ScreenManager.Player.Reset();
        ScreenManager.Player.CurrentHealth = PlayerStats.CurrentHealth;
        ScreenManager.Player.CurrentMana = PlayerStats.CurrentMana;
    }

    public void FormClosing(object sender, FormClosingEventArgs args)
    {
        if (args.CloseReason == CloseReason.UserClosing)
        {
            SaveAndReset();
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
            if (current.Width >= 2000 || current.Height >= 2000) continue;

            var item = new Vector2(current.Width, current.Height);
            if (!list.Contains(item)) list.Add(new Vector2(current.Width, current.Height));
        }

        return list;
    }

    public void SaveConfig()
    {
        Console.WriteLine("Saving Config file");
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dirPath = Path.Combine(folderPath, LevelENV.GameName);
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

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
        streamWriter.WriteLine("[ChatOption]");
        streamWriter.WriteLine("ChatOption=" + GameConfig.ChatOption);
        streamWriter.WriteLine("ChatOpacity=" + GameConfig.ChatOpacity);
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

                        case "ChatOption":
                            GameConfig.ChatOption = int.Parse(setting);
                            break;

                        case "ChatOpacity":
                            GameConfig.ChatOpacity = float.Parse(setting);
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
            if (GameConfig.ScreenHeight <= 0 || GameConfig.ScreenWidth <= 0) throw new Exception("Blank Config File");
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
        if ((GraphicsDeviceManager.IsFullScreen && !GameConfig.FullScreen) ||
            (!GraphicsDeviceManager.IsFullScreen && GameConfig.FullScreen))
            GraphicsDeviceManager.ToggleFullScreen();
        else
            GraphicsDeviceManager.ApplyChanges();

        ScreenManager.ForceResolutionChangeCheck();
    }

    public static float GetTotalGameTimeHours()
    {
        return TotalGameTimeHours;
    }

    public struct SettingStruct
    {
        public bool  EnableDirectInput;
        public bool  EnableSteamCloud;
        public bool  FullScreen;
        public float MusicVolume;
        public bool  QuickDrop;
        public int   ChatOption;
        public float ChatOpacity;
        public bool  ReduceQuality;
        public int   ScreenHeight;
        public int   ScreenWidth;
        public float SFXVolume;
    }

    public void CollectItemFromLocation(long location)
    {
        // Ignore checking the location if it was already checked.
        if (ArchipelagoManager.IsLocationChecked(location)) return;

        var item = ArchipelagoManager.LocationDictionary[location];
        var self = item.Player == ArchipelagoManager.Slot;
        var stats = new Tuple<float, float, float, float>(-1, -1, -1, 0);

        var data = new List<object>
        {
            new Vector2(ScreenManager.Player.X, ScreenManager.Player.Y),
            self ? ItemCategory.ReceiveNetworkItem : ItemCategory.GiveNetworkItem,
            new Vector2(stats.Item1, stats.Item4),
            new Vector2(stats.Item2, stats.Item3),
            ArchipelagoManager.GetPlayerName(item.Player),
            item.Item
        };

        ScreenManager.DisplayScreen((int) ScreenType.GetItem, true, data);
        ScreenManager.Player.RunGetItemAnimation();
        ArchipelagoManager.CheckLocations(location);
    }
}
