// 
// RogueLegacyArchipelago - RCScreenManager.cs
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
        private ArchipelagoScreen m_archipelagoScreen;
        private PauseScreen m_pauseScreen;
        private ProfileCardScreen m_profileCardScreen;
        private ProfileSelectScreen m_profileSelectScreen;
        private SkillUnlockScreen m_skillUnlockScreen;
        private TextScreen m_textScreen;
        private VirtualScreen m_virtualScreen;

        public RCScreenManager(Game Game) : base(Game)
        {
        }

        public bool InventoryVisible { get; private set; }

        public RenderTarget2D RenderTarget
        {
            get { return m_virtualScreen.RenderTarget; }
        }

        public DialogueScreen DialogueScreen { get; private set; }
        public SkillScreen SkillScreen { get; private set; }
        public PlayerObj Player { get; private set; }
        public bool IsTransitioning { get; private set; }

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
                DisplayScreen(16, true);
            }
        }

        public void ReinitializeContent(object sender, EventArgs e)
        {
            m_virtualScreen.ReinitializeRTs(Game.GraphicsDevice);
            foreach (var current in m_screenArray)
            {
                current.DisposeRTs();
            }
            foreach (var current2 in m_screenArray)
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
            SkillScreen = new SkillScreen();
            m_blacksmithScreen = new BlacksmithScreen();
            m_getItemScreen = new GetItemScreen();
            m_enchantressScreen = new EnchantressScreen();
            DialogueScreen = new DialogueScreen();
            m_pauseScreen = new PauseScreen();
            m_optionsScreen = new OptionsScreen();
            m_archipelagoScreen = new ArchipelagoScreen();
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
            SkillScreen.LoadContent();
            m_blacksmithScreen.LoadContent();
            m_getItemScreen.LoadContent();
            m_enchantressScreen.LoadContent();
            DialogueScreen.LoadContent();
            m_pauseScreen.LoadContent();
            m_optionsScreen.LoadContent();
            m_archipelagoScreen.LoadContent();
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
            m_blackTransitionIn.Scale = new Vector2(1320/m_blackTransitionIn.Width, 2000/m_blackTransitionIn.Height);
            m_blackTransitionIn.TextureColor = Color.Black;
            m_blackTransitionIn.ForceDraw = true;
            m_blackScreen = new SpriteObj("Blank_Sprite");
            m_blackScreen.Scale = new Vector2(1320/m_blackScreen.Width, 720/m_blackScreen.Height);
            m_blackScreen.TextureColor = Color.Black;
            m_blackScreen.ForceDraw = true;
            m_blackTransitionOut = new SpriteObj("Blank_Sprite");
            m_blackTransitionOut.Rotation = 15f;
            m_blackTransitionOut.Scale = new Vector2(1320/m_blackTransitionOut.Width, 2000/m_blackTransitionOut.Height);
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
            if (Player == null)
            {
                Player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (Game as Game).PhysicsManager, null,
                    Game as Game);
                Player.Position = new Vector2(200f, 200f);
                Player.Initialize();
            }
        }

        public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null)
        {
            LoadPlayer();
            if (pauseOtherScreens)
            {
                var screens = GetScreens();
                for (var i = 0; i < screens.Length; i++)
                {
                    var screen = screens[i];
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
                    LoadScreen((byte) screenType, true);
                    break;
                case 4:
                    m_optionsScreen.PassInData(objList);
                    AddScreen(m_optionsScreen, null);
                    break;
                case 5:
                    if (RogueCastle.Game.PlayerStats.LockCastle || !(CurrentScreen is ProceduralLevelScreen))
                    {
                        LoadScreen((byte) screenType, true);
                    }
                    else
                    {
                        LoadScreen((byte) screenType, false);
                    }
                    break;
                case 6:
                    AddScreen(SkillScreen, null);
                    break;
                case 7:
                    m_gameOverScreen.PassInData(objList);
                    AddScreen(m_gameOverScreen, null);
                    break;
                case 10:
                    AddScreen(m_blacksmithScreen, null);
                    m_blacksmithScreen.Player = Player;
                    break;
                case 11:
                    AddScreen(m_enchantressScreen, null);
                    m_enchantressScreen.Player = Player;
                    break;
                case 12:
                    m_getItemScreen.PassInData(objList);
                    AddScreen(m_getItemScreen, null);
                    break;
                case 13:
                    AddScreen(DialogueScreen, null);
                    break;
                case 14:
                    m_mapScreen.SetPlayer(Player);
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
                case 80:
                    m_archipelagoScreen.PassInData(objList);
                    AddScreen(m_archipelagoScreen, null);
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
            if (m_mapScreen != null)
            {
                m_mapScreen.Dispose();
            }
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
            for (var i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                var proceduralLevelScreen = screen as ProceduralLevelScreen;
                if (proceduralLevelScreen != null)
                {
                    return proceduralLevelScreen;
                }
            }
            return null;
        }
    }
}
