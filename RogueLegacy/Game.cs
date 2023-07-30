// RogueLegacyRandomizer - Game.cs
// Last Modified 2023-07-30 1:05 PM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

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
using Randomizer.Definitions;
using RogueLegacy.Enums;
using RogueLegacy.GameObjects;
using RogueLegacy.GameObjects.HUD;
using RogueLegacy.Screens;
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

        GraphicsDeviceManager = new GraphicsDeviceManager(this);
        ScreenManager = new RCScreenManager(this);
        SaveManager = new SaveGameManager();
        PhysicsManager = new PhysicsManager();
        EquipmentSystem = new EquipmentSystem();
        NextChildItemQueue = new Queue<NetworkItem>();

        PlayerStats = new PlayerStats();
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

    public static Cue                LineageSongCue       { get; set; }
    public static RCScreenManager    ScreenManager        { get; set; }
    public static Effect             BWMaskEffect         { get; set; }
    public static Effect             ColourSwapShader     { get; set; }
    public static Effect             HSVEffect            { get; set; }
    public static Effect             InvertShader         { get; set; }
    public static Effect             MaskEffect           { get; set; }
    public static Effect             ParallaxEffect       { get; set; }
    public static Effect             RippleEffect         { get; set; }
    public static Effect             ShadowEffect         { get; set; }
    public static EquipmentSystem    EquipmentSystem      { get; set; }
    public static GaussianBlur       GaussianBlur         { get; set; }
    public static PlayerStats        PlayerStats          { get; set; }
    public static bool               Retired              { get; set; }
    public static InputMap           GlobalInput          { get; set; }
    public static List<string>       NameArray            { get; set; }
    public static List<string>       FemaleNameArray      { get; set; }
    public static SpriteFont         PixelArtFont         { get; set; }
    public static SpriteFont         PixelArtFontBold     { get; set; }
    public static SpriteFont         JunicodeFont         { get; set; }
    public static SpriteFont         EnemyLevelFont       { get; set; }
    public static SpriteFont         PlayerLevelFont      { get; set; }
    public static SpriteFont         GoldFont             { get; set; }
    public static SpriteFont         HerzogFont           { get; set; }
    public static SpriteFont         JunicodeLargeFont    { get; set; }
    public static SpriteFont         CinzelFont           { get; set; }
    public static SpriteFont         BitFont              { get; set; }
    public static Texture2D          GenericTexture       { get; set; }
    public static float              PlaySessionLength    { get; set; }
    public static float              TotalGameTimeSeconds { get; set; }
    public static float              TotalGameTimeHours   { get; set; }
    public static string             ProfileName          { get; set; }
    public        Queue<NetworkItem> NextChildItemQueue   { get; set; }

    public GraphicsDeviceManager GraphicsDeviceManager { get; }
    public PhysicsManager        PhysicsManager        { get; }
    public SaveGameManager       SaveManager           { get; }

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

        // Wait for Arch to say its ready.
        switch (ArchipelagoManager.Status)
        {
            // We're ready!
            case ConnectionStatus.Authenticated:
            {
                // Do not attempt to start the game if we connect off the Arch screen. Most likely means we lost
                // connection in gameplay and reconnected.
                if (ScreenManager.CurrentScreen is not RandomizerScreen)
                    break;

                // Initialize Save Data
                ChangeProfile(ArchipelagoManager.RandomizerData.Seed, ArchipelagoManager.RandomizerData.Slot);

                // Load Area Struct Data
                Area1List = ArchipelagoManager.RandomizerData.AreaStructs;

                SoundManager.PlaySound("Game_Start");
                var newGame = !PlayerStats.CharacterFound;
                var heroIsDead = PlayerStats.IsDead;
                var startingRoom = PlayerStats.LoadStartingRoom;

                if (newGame)
                {
                    PlayerStats.CharacterFound = true;
                    PlayerStats.Gold = 0;
                    PlayerStats.Class = ArchipelagoManager.RandomizerData.StartingClass;
                    PlayerStats.Spell = (byte) ClassExtensions.SpellList((ClassType) PlayerStats.Class)[0];

                    // Unlock the player's starting class.
                    var skill = (ClassType) ArchipelagoManager.RandomizerData.StartingClass switch
                    {
                        ClassType.Knight     => SkillSystem.GetSkill(SkillType.KnightUnlock),
                        ClassType.Mage       => SkillSystem.GetSkill(SkillType.MageUnlock),
                        ClassType.Barbarian  => SkillSystem.GetSkill(SkillType.BarbarianUnlock),
                        ClassType.Knave      => SkillSystem.GetSkill(SkillType.AssassinUnlock),
                        ClassType.Miner      => SkillSystem.GetSkill(SkillType.BankerUnlock),
                        ClassType.Shinobi    => SkillSystem.GetSkill(SkillType.NinjaUnlock),
                        ClassType.Lich       => SkillSystem.GetSkill(SkillType.LichUnlock),
                        ClassType.Spellthief => SkillSystem.GetSkill(SkillType.SpellswordUnlock),
                        ClassType.Dragon     => SkillSystem.GetSkill(SkillType.SuperSecret),
                        ClassType.Traitor    => SkillSystem.GetSkill(SkillType.Traitorous),
                        _                    => throw new ArgumentException("Unsupported Starting Class")
                    };

                    SkillSystem.LevelUpTrait(skill, false);

                    PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, 5);
                    PlayerStats.EnemiesKilledInRun.Clear();

                    // Set AP Settings
                    PlayerStats.TimesCastleBeaten = ArchipelagoManager.RandomizerData.NewGamePlus;

                    // Set the player's initial gender.
                    PlayerStats.IsFemale = ArchipelagoManager.RandomizerData.StartingGender == Gender.Lady;

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
                break;
            }
        }

        // Check for received items and send to player.
        if (ArchipelagoManager.ReceiveItemQueue.Count > 0)
        {
            if (ScreenManager.Player is { ControlsLocked: false } && ScreenManager.CurrentScreen is ProceduralLevelScreen)
            {
                var (index, item) = ArchipelagoManager.ReceiveItemQueue.Dequeue();

                // Only give item if we haven't received it before!
                if (PlayerStats.HasNotReceivedItem(index, item))
                {
                    PlayerStats.ReceivedItems.Add(item);
                    var stats = DisgustingGetItemLogic(item);

                    (ScreenManager.CurrentScreen as ProceduralLevelScreen).AddReceivedItem(
                        item.Item.GetItemType(),
                        item.Item,
                        ArchipelagoManager.GetPlayerName(item.Player),
                        stats
                    );
                }
            }
        }

        // Death Link handling logic.
        if (ArchipelagoManager.ReceiveItemQueue.Count == 0 && ArchipelagoManager.DeathLink != null)
        {
            if (ScreenManager.Player is { ControlsLocked: false } &&
                ScreenManager.CurrentScreen is ProceduralLevelScreen && !PlayerStats.IsDead &&
                ArchipelagoManager.DeathLinkSafe)
            {
                if (PlayerStats.SpecialItem == 3)
                {
                    ScreenManager.Player.CurrentHealth = (int) (ScreenManager.Player.MaxHealth * 0.25f);
                    PlayerStats.SpecialItem = 0;
                    (ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerHUDSpecialItem();
                    ScreenManager.DisplayScreen(21, true);
                }
                else
                {
                    var num6 = CDGMath.RandomInt(1, 100);
                    if (num6 <= SkillSystem.GetSkill(SkillType.DeathDodge).ModifierAmount * 100f)
                    {
                        ScreenManager.Player.CurrentHealth = (int) (ScreenManager.Player.MaxHealth * 0.25f);
                        PlayerStats.SpecialItem = 0;
                        (ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerHUDSpecialItem();
                        ScreenManager.DisplayScreen(21, true);
                    }
                    else
                    {
                        ScreenManager.Player.AttachedLevel.SetObjectKilledPlayer(
                            new DeathLinkObj(ArchipelagoManager.DeathLink.Source));
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

    public Tuple<float, float, float, float> DisgustingGetItemLogic(NetworkItem item, bool ignoreDefer = false)
    {
        SkillObj skill;
        byte equipStatus = 0;
        var player = ScreenManager.Player;

        switch (item.Item)
        {
            case ItemCode.BLACKSMITH:
                skill = SkillSystem.GetSkill(SkillType.Smithy);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.ENCHANTRESS:
                skill = SkillSystem.GetSkill(SkillType.Enchanter);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.ARCHITECT:
                skill = SkillSystem.GetSkill(SkillType.Architect);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.PROGRESSIVE_KNIGHT:
                skill = SkillSystem.GetSkill(SkillType.KnightUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.KnightUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_MAGE:
                skill = SkillSystem.GetSkill(SkillType.MageUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.MageUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_BARBARIAN:
                skill = SkillSystem.GetSkill(SkillType.BarbarianUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.BarbarianUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_KNAVE:
                skill = SkillSystem.GetSkill(SkillType.AssassinUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.AssassinUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_SHINOBI:
                skill = SkillSystem.GetSkill(SkillType.NinjaUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.NinjaUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_MINER:
                skill = SkillSystem.GetSkill(SkillType.BankerUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.BankerUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_LICH:
                skill = SkillSystem.GetSkill(SkillType.LichUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.LichUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.PROGRESSIVE_SPELLTHIEF:
                skill = SkillSystem.GetSkill(SkillType.SpellswordUnlock);
                if (skill.CurrentLevel > 0)
                {
                    skill = SkillSystem.GetSkill(SkillType.SpellSwordUp);
                    SkillSystem.LevelUpTrait(skill, false);
                }
                else
                    SkillSystem.LevelUpTrait(skill, false);

                skill.CanPurchase = true;
                break;

            case ItemCode.DRAGON:
                skill = SkillSystem.GetSkill(SkillType.SuperSecret);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.TRAITOR:
                skill = SkillSystem.GetSkill(SkillType.Traitorous);
                skill.CanPurchase = true;
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.HEALTH:
                var preMaxHealth = player.MaxHealth;
                skill = SkillSystem.GetSkill(SkillType.HealthUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                player.CurrentHealth += player.MaxHealth - preMaxHealth;
                break;

            case ItemCode.MANA:
                var preMaxMana = player.MaxMana;
                skill = SkillSystem.GetSkill(SkillType.ManaUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                player.CurrentMana += player.MaxMana - preMaxMana;
                break;

            case ItemCode.ATTACK:
                skill = SkillSystem.GetSkill(SkillType.AttackUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.MAGIC_DAMAGE:
                skill = SkillSystem.GetSkill(SkillType.MagicDamageUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.ARMOR:
                skill = SkillSystem.GetSkill(SkillType.ArmorUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.EQUIP:
                skill = SkillSystem.GetSkill(SkillType.EquipUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.CRIT_CHANCE:
                skill = SkillSystem.GetSkill(SkillType.CritChanceUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.CRIT_DAMAGE:
                skill = SkillSystem.GetSkill(SkillType.CritDamageUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.DOWN_STRIKE:
                skill = SkillSystem.GetSkill(SkillType.DownStrikeUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.GOLD_GAIN:
                skill = SkillSystem.GetSkill(SkillType.GoldGainUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.POTION_EFFICIENCY:
                skill = SkillSystem.GetSkill(SkillType.PotionUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.INVULN_TIME:
                skill = SkillSystem.GetSkill(SkillType.InvulnerabilityTimeUp);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.MANA_COST_DOWN:
                skill = SkillSystem.GetSkill(SkillType.ManaCostDown);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.DEATH_DEFIANCE:
                skill = SkillSystem.GetSkill(SkillType.DeathDodge);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.HAGGLING:
                skill = SkillSystem.GetSkill(SkillType.PricesDown);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.RANDOMIZE_CHILDREN:
                skill = SkillSystem.GetSkill(SkillType.RandomizeChildren);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.BLACKSMITH_SWORD:
                skill = SkillSystem.GetSkill(SkillType.BlacksmithSword);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.BLACKSMITH_HELM:
                skill = SkillSystem.GetSkill(SkillType.BlacksmithHelm);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.BLACKSMITH_CHEST:
                skill = SkillSystem.GetSkill(SkillType.BlacksmithChest);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.BLACKSMITH_LIMBS:
                skill = SkillSystem.GetSkill(SkillType.BlacksmithLimbs);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.BLACKSMITH_CAPE:
                skill = SkillSystem.GetSkill(SkillType.BlacksmithCape);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.ENCHANTRESS_SWORD:
                skill = SkillSystem.GetSkill(SkillType.EnchantressSword);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.ENCHANTRESS_HELM:
                skill = SkillSystem.GetSkill(SkillType.EnchantressHelm);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.ENCHANTRESS_CHEST:
                skill = SkillSystem.GetSkill(SkillType.EnchantressChest);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.ENCHANTRESS_LIMBS:
                skill = SkillSystem.GetSkill(SkillType.EnchantressLimbs);
                SkillSystem.LevelUpTrait(skill, false);
                break;
            case ItemCode.ENCHANTRESS_CAPE:
                skill = SkillSystem.GetSkill(SkillType.EnchantressCape);
                SkillSystem.LevelUpTrait(skill, false);
                break;

            case ItemCode.RUNE_VAULT:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Vault] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Vault] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Vault] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Vault] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Vault] = equipStatus;
                break;

            case ItemCode.RUNE_SPRINT:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Sprint] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Sprint] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Sprint] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Sprint] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Sprint] = equipStatus;
                break;

            case ItemCode.RUNE_VAMPIRE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Vampire] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Vampire] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Vampire] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Vampire] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Vampire] = equipStatus;
                break;

            case ItemCode.RUNE_SKY:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Sky] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Sky] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Sky] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Sky] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Sky] = equipStatus;
                break;

            case ItemCode.RUNE_SIPHON:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Siphon] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Siphon] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Siphon] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Siphon] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Siphon] = equipStatus;
                break;

            case ItemCode.RUNE_RETALIATION:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Retaliation] =
                    equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Retaliation] =
                    equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Retaliation] =
                    equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Retaliation] =
                    equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Retaliation] =
                    equipStatus;
                break;

            case ItemCode.RUNE_BOUNTY:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Bounty] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Bounty] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Bounty] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Bounty] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Bounty] = equipStatus;
                break;

            case ItemCode.RUNE_HASTE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Haste] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Haste] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Haste] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Haste] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Haste] = equipStatus;
                break;

            case ItemCode.RUNE_CURSE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Curse] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Curse] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Curse] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Curse] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Curse] = equipStatus;
                break;

            case ItemCode.RUNE_GRACE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Grace] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Grace] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Grace] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Grace] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Grace] = equipStatus;
                break;

            case ItemCode.RUNE_BALANCE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Sword][(int) EquipmentAbility.Balance] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Helm][(int) EquipmentAbility.Balance] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Chest][(int) EquipmentAbility.Balance] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Limbs][(int) EquipmentAbility.Balance] = equipStatus;
                PlayerStats.GetRuneArray[(int) EquipmentCategory.Cape][(int) EquipmentAbility.Balance] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_PROGRESSIVE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                var index = 0;
                var progressiveArmorOrder = new[]
                {
                    (int) EquipmentBase.Squire,
                    (int) EquipmentBase.Knight,
                    (int) EquipmentBase.Blood,
                    (int) EquipmentBase.Silver,
                    (int) EquipmentBase.Ranger,
                    (int) EquipmentBase.Sage,
                    (int) EquipmentBase.Guardian,
                    (int) EquipmentBase.Sky,
                    (int) EquipmentBase.Retribution,
                    (int) EquipmentBase.Imperial,
                    (int) EquipmentBase.Dragon,
                    (int) EquipmentBase.Holy,
                    (int) EquipmentBase.Royal,
                    (int) EquipmentBase.Slayer,
                    (int) EquipmentBase.Dark
                };

                while (index <= 15 && PlayerStats.GetBlueprintArray[0][progressiveArmorOrder[index]] > 0)
                    index++;

                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][progressiveArmorOrder[index]] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][progressiveArmorOrder[index]] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][progressiveArmorOrder[index]] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][progressiveArmorOrder[index]] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][progressiveArmorOrder[index]] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_SQUIRE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Squire] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Squire] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Squire] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Squire] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Squire] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_SILVER:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Silver] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Silver] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Silver] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Silver] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Silver] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_GUARDIAN:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Guardian] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Guardian] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Guardian] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Guardian] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Guardian] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_IMPERIAL:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Imperial] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Imperial] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Imperial] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Imperial] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Imperial] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_ROYAL:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Royal] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Royal] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Royal] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Royal] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Royal] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_KNIGHT:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Knight] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Knight] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Knight] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Knight] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Knight] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_RANGER:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Ranger] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Ranger] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Ranger] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Ranger] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Ranger] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_SKY:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Sky] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Sky] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Sky] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Sky] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Sky] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_DRAGON:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Dragon] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Dragon] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Dragon] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Dragon] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Dragon] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_SLAYER:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Slayer] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Slayer] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Slayer] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Slayer] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Slayer] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_BLOOD:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Blood] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Blood] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Blood] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Blood] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Blood] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_SAGE:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Sage] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Sage] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Sage] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Sage] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Sage] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_RETRIBUTION:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Retribution] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Retribution] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Retribution] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Retribution] =
                    equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Retribution] =
                    equipStatus;
                break;

            case ItemCode.EQUIPMENT_HOLY:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Holy] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Holy] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Holy] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Holy] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Holy] = equipStatus;
                break;

            case ItemCode.EQUIPMENT_DARK:
                equipStatus = (byte) (ArchipelagoManager.RandomizerData.RequireVendorPurchasing ? 1 : 3);
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Sword][(int) EquipmentBase.Dark] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Helm][(int) EquipmentBase.Dark] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Chest][(int) EquipmentBase.Dark] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Limbs][(int) EquipmentBase.Dark] = equipStatus;
                PlayerStats.GetBlueprintArray[(int) EquipmentCategory.Cape][(int) EquipmentBase.Dark] = equipStatus;
                break;

            case ItemCode.FOUNTAIN_PIECE:
                PlayerStats.FountainPieces += 1;
                break;

            case ItemCode.TRAP_HEDGEHOG:
                PlayerStats.SpecialItem = (byte) SpecialItemType.HedgehogsCurse;
                break;

            case ItemCode.TRAP_VERTIGO:
                PlayerStats.HasVertigo = !PlayerStats.HasVertigo;
                break;

            case ItemCode.TRAP_GENETIC_LOTTERY:
                var @class = PlayerStats.Class;
                PlayerStats.Class = (byte) ClassExtensions.RandomClass();

                var spells = ClassExtensions.SpellList((ClassType) PlayerStats.Class);
                PlayerStats.Spell = (byte) spells[CDGMath.RandomInt(0, spells.Length - 1)];

                var traits = TraitHelper.ReturnRandomTraits();
                PlayerStats.Traits = new Vector2((float) traits[0], (float) traits[1]);

                var currentlyFemale = PlayerStats.IsFemale;
                PlayerStats.IsFemale = CDGMath.RandomInt(0, 1) == 0;
                if (currentlyFemale != PlayerStats.IsFemale)
                {
                    PlayerStats.PlayerName = PlayerStats.IsFemale
                        ? PlayerStats.PlayerName.Replace("Sir ", "Lady ")
                        : PlayerStats.PlayerName.Replace("Lady ", "Sir ");
                }
                break;

            case ItemCode.GOLD_1000:
                PlayerStats.Gold += 1000;
                return new Tuple<float, float, float, float>(-1, -1, -1, 1000);

            case ItemCode.GOLD_3000:
                PlayerStats.Gold += 3000;
                return new Tuple<float, float, float, float>(-1, -1, -1, 3000);

            case ItemCode.GOLD_5000:
                PlayerStats.Gold += 5000;
                return new Tuple<float, float, float, float>(-1, -1, -1, 5000);

            case ItemCode.TRIP_STAT_INCREASE:
                var stats = new float[3];
                for (var i = 0; i < 3; i++)
                {
                    var stat = CDGMath.RandomInt(4, 9);
                    stats[i] = stat;
                    switch (stat)
                    {
                        case 4:
                            PlayerStats.BonusStrength++;
                            continue;
                        case 5:
                            PlayerStats.BonusMagic++;
                            continue;
                        case 6:
                            PlayerStats.BonusDefense++;
                            continue;
                        case 7:
                            PlayerStats.BonusHealth++;
                            continue;
                        case 8:
                            PlayerStats.BonusMana++;
                            continue;
                        default:
                            // stats[i] == 9
                            PlayerStats.BonusWeight++;
                            continue;
                    }
                }

                return new Tuple<float, float, float, float>(stats[0], stats[1], stats[2], 0);
        }

        return new Tuple<float, float, float, float>(-1, -1, -1, 0);
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

    public void SaveOnExit()
    {
        if (ScreenManager.CurrentScreen is CDGSplashScreen) return;

        UpdatePlaySessionLength();
        var screen = ScreenManager.GetLevelScreen();
        if (screen != null)
        {
            var currentRoom = screen.CurrentRoom;
            if (currentRoom is CarnivalShoot1BonusRoom or CarnivalShoot2BonusRoom)
            {
                if (currentRoom is CarnivalShoot1BonusRoom bonusRoom1) bonusRoom1.UnequipPlayer();

                if (currentRoom is CarnivalShoot2BonusRoom bonusRoom2) bonusRoom2.UnequipPlayer();
            }

            if (currentRoom is ChallengeBossRoomObj challengeBossRoom)
            {
                SaveManager.LoadFiles(ScreenManager.GetLevelScreen(), SaveType.UpgradeData);
                currentRoom.Player.CurrentHealth = challengeBossRoom.StoredHP;
                currentRoom.Player.CurrentMana = challengeBossRoom.StoredMP;
            }
        }

        if (ScreenManager.CurrentScreen is GameOverScreen) PlayerStats.Traits = Vector2.Zero;

        if (SaveManager.FileExists(SaveType.PlayerData))
        {
            SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);
            if (screen is { CurrentRoom: null }) return;

            if (screen != null && screen.CurrentRoom.Name != "Start" && screen.CurrentRoom.Name != "Ending" &&
                screen.CurrentRoom.Name != "Tutorial")
                SaveManager.SaveFiles(SaveType.MapData);
        }
    }

    public void FormClosing(object sender, FormClosingEventArgs args)
    {
        if (args.CloseReason == CloseReason.UserClosing) SaveOnExit();
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

        var item = ArchipelagoManager.AllLocations[location];
        var self = item.Player == ArchipelagoManager.RandomizerData.Slot;
        var stats = new Tuple<float, float, float, float>(-1, -1, -1, 0);
        if (self)
        {
            // Ignore any items we've already received.
            if (!PlayerStats.HasNotReceivedItem(-1, item))
                return;

            stats = DisgustingGetItemLogic(item);
            PlayerStats.ReceivedItems.Add(item);
        }

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