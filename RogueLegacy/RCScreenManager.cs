//  RogueLegacyRandomizer - RCScreenManager.cs
//  Last Modified 2023-10-25 8:36 PM
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;
using Tweener.Ease;
using Screen = DS2DEngine.Screen;

namespace RogueLegacy
{
    public class RCScreenManager : ScreenManager
    {
        private RandomizerScreen _randomizerScreen;
        private SpriteObj m_blackScreen;
        private BlacksmithScreen m_blacksmithScreen;
        private SpriteObj m_blackTransitionIn;
        private SpriteObj m_blackTransitionOut;
        private CreditsScreen m_creditsScreen;
        private DeathDefiedScreen m_deathDefyScreen;
        private DiaryEntryScreen m_diaryEntryScreen;
        private EnchantressScreen m_enchantressScreen;
        private DiaryFlashbackScreen m_flashbackScreen;
        private GameOverBossScreen m_gameOverBossScreen;
        private GameOverScreen m_gameOverScreen;
        private GetItemScreen m_getItemScreen;
        private bool m_isWipeTransitioning;
        private MapScreen m_mapScreen;
        private OptionsScreen m_optionsScreen;
        private PauseScreen m_pauseScreen;
        private ProfileCardScreen m_profileCardScreen;
        private SkillUnlockScreen m_skillUnlockScreen;
        private TextScreen m_textScreen;
        private VirtualScreen m_virtualScreen;

        public RCScreenManager(Game game) : base(game) { }

        public DialogueScreen DialogueScreen { get; private set; }
        public SkillScreen SkillScreen { get; private set; }
        public PlayerObj Player { get; private set; }
        public bool IsTransitioning { get; private set; }

        public RenderTarget2D RenderTarget
        {
            get { return m_virtualScreen.RenderTarget; }
        }

        public override void Initialize()
        {
            InitializeScreens();
            base.Initialize();
            m_virtualScreen = new VirtualScreen(1320, 720, Camera.GraphicsDevice);
            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            Game.Deactivated += PauseGame;
            var form = Control.FromHandle(Game.Window.Handle) as Form;
            if (form != null)
            {
                form.MouseCaptureChanged += PauseGame;
            }

            Camera.GraphicsDevice.DeviceReset += ReinitializeContent;
        }

        public void PauseGame(object sender, EventArgs e)
        {
            var proceduralLevelScreen = CurrentScreen as ProceduralLevelScreen;
            if (proceduralLevelScreen != null && !(proceduralLevelScreen.CurrentRoom is EndingRoomObj))
            {
                DisplayScreen((int) ScreenType.Pause, true);
            }
        }

        public void ReinitializeContent(object sender, EventArgs e)
        {
            m_virtualScreen.ReinitializeRTs(Game.GraphicsDevice);
            foreach (var current in m_screenArray) current.DisposeRTs();
            foreach (var current2 in m_screenArray) current2.ReinitializeRTs();
        }

        public void ReinitializeCamera(GraphicsDevice graphicsDevice)
        {
            m_camera.Dispose();
            m_camera = new Camera2D(graphicsDevice, EngineEV.ScreenWidth, EngineEV.ScreenHeight);
        }

        public void InitializeScreens()
        {
            m_gameOverScreen = new GameOverScreen();
            SkillScreen = new SkillScreen();
            m_blacksmithScreen = new BlacksmithScreen();
            m_getItemScreen = new GetItemScreen();
            m_enchantressScreen = new EnchantressScreen();
            DialogueScreen = new DialogueScreen();
            m_pauseScreen = new PauseScreen();
            m_optionsScreen = new OptionsScreen();
            _randomizerScreen = new RandomizerScreen();
            m_profileCardScreen = new ProfileCardScreen();
            m_creditsScreen = new CreditsScreen();
            m_skillUnlockScreen = new SkillUnlockScreen();
            m_diaryEntryScreen = new DiaryEntryScreen();
            m_deathDefyScreen = new DeathDefiedScreen();
            m_textScreen = new TextScreen();
            m_flashbackScreen = new DiaryFlashbackScreen();
            m_gameOverBossScreen = new GameOverBossScreen();
        }

        public override void LoadContent()
        {
            m_gameOverScreen.LoadContent();
            SkillScreen.LoadContent();
            m_blacksmithScreen.LoadContent();
            m_getItemScreen.LoadContent();
            m_enchantressScreen.LoadContent();
            DialogueScreen.LoadContent();
            m_pauseScreen.LoadContent();
            m_optionsScreen.LoadContent();
            _randomizerScreen.LoadContent();
            m_profileCardScreen.LoadContent();
            m_creditsScreen.LoadContent();
            m_skillUnlockScreen.LoadContent();
            m_diaryEntryScreen.LoadContent();
            m_deathDefyScreen.LoadContent();
            m_textScreen.LoadContent();
            m_flashbackScreen.LoadContent();
            m_gameOverBossScreen.LoadContent();
            m_blackTransitionIn = new SpriteObj("Blank_Sprite");
            m_blackTransitionIn.Rotation = 15f;
            m_blackTransitionIn.Scale =
                new Vector2(1320 / m_blackTransitionIn.Width, 2000 / m_blackTransitionIn.Height);
            m_blackTransitionIn.TextureColor = Color.Black;
            m_blackTransitionIn.ForceDraw = true;
            m_blackScreen = new SpriteObj("Blank_Sprite");
            m_blackScreen.Scale = new Vector2(1320 / m_blackScreen.Width, 720 / m_blackScreen.Height);
            m_blackScreen.TextureColor = Color.Black;
            m_blackScreen.ForceDraw = true;
            m_blackTransitionOut = new SpriteObj("Blank_Sprite");
            m_blackTransitionOut.Rotation = 15f;
            m_blackTransitionOut.Scale =
                new Vector2(1320 / m_blackTransitionOut.Width, 2000 / m_blackTransitionOut.Height);
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
            if (Player != null)
            {
                return;
            }

            Player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, Program.Game.PhysicsManager, null,
                Program.Game)
            {
                Position = new Vector2(200f, 200f)
            };

            Player.Initialize();
        }

        public void DisplayScreen(ScreenType screenType, bool pauseOtherScreens, List<object> objList = null)
        {
            DisplayScreen((int) screenType, pauseOtherScreens, objList);
        }

        public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null)
        {
            LoadPlayer();
            if (pauseOtherScreens)
            {
                var screens = GetScreens();
                foreach (var screen in screens)
                {
                    if (screen != CurrentScreen) continue;

                    screen.PauseScreen();
                    break;
                }
            }

            switch (screenType)
            {
                case (int) ScreenType.CDGSplash:
                case (int) ScreenType.Title:
                case (int) ScreenType.Lineage:
                case (int) ScreenType.StartingRoom:
                case (int) ScreenType.TitleWhite:
                case (int) ScreenType.DemoStart:
                case (int) ScreenType.DemoEnd:
                    LoadScreen((byte) screenType, true);
                    break;

                case (int) ScreenType.Options:
                    m_optionsScreen.PassInData(objList);
                    AddScreen(m_optionsScreen, null);
                    break;

                case (int) ScreenType.Level:
                    if (RogueLegacy.Game.PlayerStats.LockCastle || !(CurrentScreen is ProceduralLevelScreen))
                        LoadScreen((byte) screenType, true);
                    else
                        LoadScreen((byte) screenType, false);

                    break;

                case (int) ScreenType.Skill:
                    AddScreen(SkillScreen, null);
                    break;

                case (int) ScreenType.GameOver:
                    m_gameOverScreen.PassInData(objList);
                    AddScreen(m_gameOverScreen, null);
                    break;

                case (int) ScreenType.Blacksmith:
                    AddScreen(m_blacksmithScreen, null);
                    m_blacksmithScreen.Player = Player;
                    break;

                case (int) ScreenType.Enchantress:
                    AddScreen(m_enchantressScreen, null);
                    m_enchantressScreen.Player = Player;
                    break;

                case (int) ScreenType.GetItem:
                    m_getItemScreen.PassInData(objList);
                    AddScreen(m_getItemScreen, null);
                    break;

                case (int) ScreenType.Dialogue:
                    AddScreen(DialogueScreen, null);
                    break;

                case (int) ScreenType.Map:
                    m_mapScreen.SetPlayer(Player);
                    AddScreen(m_mapScreen, null);
                    break;

                case (int) ScreenType.Pause:
                    GetLevelScreen().CurrentRoom.DarkenRoom();
                    AddScreen(m_pauseScreen, null);
                    break;

                case (int) ScreenType.ProfileCard:
                    AddScreen(m_profileCardScreen, null);
                    break;

                case (int) ScreenType.Credits:
                    LoadScreen((int) ScreenType.Credits, true);
                    break;

                case (int) ScreenType.SkillUnlock:
                    m_skillUnlockScreen.PassInData(objList);
                    AddScreen(m_skillUnlockScreen, null);
                    break;

                case (int) ScreenType.DiaryEntry:
                    AddScreen(m_diaryEntryScreen, null);
                    break;

                case (int) ScreenType.DeathDefy:
                    AddScreen(m_deathDefyScreen, null);
                    break;

                case (int) ScreenType.Text:
                    m_textScreen.PassInData(objList);
                    AddScreen(m_textScreen, null);
                    break;

                case (int) ScreenType.TutorialRoom:
                    LoadScreen((int) ScreenType.TutorialRoom, true);
                    break;

                case (int) ScreenType.Ending:
                    GetLevelScreen().CameraLockedToPlayer = false;
                    GetLevelScreen().DisableRoomTransitioning = true;
                    Player.Position = new Vector2(100f, 100f);
                    LoadScreen((int) ScreenType.Ending, true);
                    break;

                case (int) ScreenType.DiaryFlashback:
                    AddScreen(m_flashbackScreen, null);
                    break;

                case (int) ScreenType.GameOverBoss:
                    m_gameOverBossScreen.PassInData(objList);
                    AddScreen(m_gameOverBossScreen, null);
                    break;

                case (int) ScreenType.Archipelago:
                    _randomizerScreen.PassInData(objList);
                    AddScreen(_randomizerScreen, null);
                    break;
            }

            if (m_isWipeTransitioning) EndWipeTransition();
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
            var proceduralLevelScreen = CurrentScreen as ProceduralLevelScreen;
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
            foreach (var current in m_screenArray)
            {
                current.DrawIfCovered = true;
                var proceduralLevelScreen = current as ProceduralLevelScreen;
                if (proceduralLevelScreen != null)
                {
                    Player.AttachLevel(proceduralLevelScreen);
                    proceduralLevelScreen.Player = Player;
                    AttachMap(proceduralLevelScreen);
                }
            }

            if (m_gameOverScreen != null)
            {
                InitializeScreens();
                LoadContent();
            }

            var screen = new LoadingScreen(screenType, wipeTransition);
            IsTransitioning = true;
            AddScreen(screen, PlayerIndex.One);
            GC.Collect();
        }

        public override void RemoveScreen(Screen screen, bool disposeScreen)
        {
            if (screen is LoadingScreen)
            {
                IsTransitioning = false;
            }

            base.RemoveScreen(screen, disposeScreen);
        }

        public void AttachMap(ProceduralLevelScreen level)
        {
            m_mapScreen?.Dispose();
            m_mapScreen = new MapScreen(level);
        }

        public override void Update(GameTime gameTime)
        {
            m_virtualScreen.Update();
            if (!IsTransitioning)
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
            Tween.By(m_blackTransitionIn, 0.15f, Quad.EaseInOut, "X", (-m_blackTransitionIn.X).ToString());
            Tween.By(m_blackScreen, 0.15f, Quad.EaseInOut, "X", (-m_blackTransitionIn.X).ToString());
            Tween.By(m_blackTransitionOut, 0.15f, Quad.EaseInOut, "X", (-m_blackTransitionIn.X).ToString());
        }

        public void EndWipeTransition()
        {
            m_isWipeTransitioning = false;
            m_blackTransitionOut.Y = -500f;
            Tween.By(m_blackTransitionIn, 0.25f, Quad.EaseInOut, "X", "-3000");
            Tween.By(m_blackScreen, 0.25f, Quad.EaseInOut, "X", "-3000");
            Tween.By(m_blackTransitionOut, 0.25f, Quad.EaseInOut, "X", "-3000");
        }

        public void UpdatePauseScreenIcons()
        {
            m_pauseScreen.UpdatePauseScreenIcons();
        }

        public ProceduralLevelScreen GetLevelScreen()
        {
            var screens = GetScreens();
            return screens.OfType<ProceduralLevelScreen>().FirstOrDefault();
        }
    }
}
