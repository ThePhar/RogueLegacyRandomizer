// 
// RogueLegacyArchipelago - LoadingScreen.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Globalization;
using System.Threading;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class LoadingScreen : Screen
    {
        private readonly byte m_screenTypeToLoad;
        private readonly bool m_wipeTransition;
        private SpriteObj m_blackScreen;
        private SpriteObj m_blackTransitionIn;
        private SpriteObj m_blackTransitionOut;
        private ImpactEffectPool m_effectPool;
        private bool m_gameCrashed;
        private ObjContainer m_gateSprite;
        private bool m_horizontalShake;
        private bool m_isLoading;
        private Screen m_levelToLoad;
        private bool m_loadingComplete;
        private TextObj m_loadingText;
        private float m_screenShakeMagnitude;
        private bool m_shakeScreen;
        private bool m_verticalShake;

        public LoadingScreen(byte screenType, bool wipeTransition)
        {
            m_screenTypeToLoad = screenType;
            m_effectPool = new ImpactEffectPool(50);
            m_wipeTransition = wipeTransition;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_loadingText = new TextObj();
            m_loadingText.Font = Game.JunicodeLargeFont;
            m_loadingText.Text = "Building";
            m_loadingText.Align = Types.TextAlign.Centre;
            m_loadingText.FontSize = 40f;
            m_loadingText.OutlineWidth = 4;
            m_loadingText.ForceDraw = true;
            m_gateSprite = new ObjContainer("LoadingScreenGate_Character");
            m_gateSprite.ForceDraw = true;
            m_gateSprite.Scale = new Vector2(2f, 2f);
            m_gateSprite.Y -= m_gateSprite.Height;
            m_effectPool.Initialize();
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
            base.LoadContent();
        }

        public override void OnEnter()
        {
            BackBufferOpacity = 0f;
            m_gameCrashed = false;
            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
            {
                m_loadingText.Text = "Jacking In";
            }
            else if (Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f)
            {
                m_loadingText.Text = "Reminiscing";
            }
            else if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
            {
                m_loadingText.Text = "Balding";
            }
            else
            {
                m_loadingText.Text = "Building";
            }
            if (!m_loadingComplete)
            {
                if (m_screenTypeToLoad == 27)
                {
                    Tween.To(this, 0.05f, Tween.EaseNone, "BackBufferOpacity", "1");
                    Tween.RunFunction(1f, this, "BeginThreading");
                }
                else
                {
                    m_blackTransitionIn.X = 0f;
                    m_blackTransitionIn.X = 1320 - m_blackTransitionIn.Bounds.Left;
                    m_blackScreen.X = m_blackTransitionIn.X;
                    m_blackTransitionOut.X = m_blackScreen.X + m_blackScreen.Width;
                    if (!m_wipeTransition)
                    {
                        SoundManager.PlaySound("GateDrop");
                        Tween.To(m_gateSprite, 0.5f, Tween.EaseNone, "Y", "0");
                        Tween.RunFunction(0.3f, m_effectPool, "LoadingGateSmokeEffect", 40);
                        Tween.RunFunction(0.3f, typeof (SoundManager), "PlaySound", "GateSlam");
                        Tween.RunFunction(0.55f, this, "ShakeScreen", 4, true, true);
                        Tween.RunFunction(0.65f, this, "StopScreenShake");
                        Tween.RunFunction(1.5f, this, "BeginThreading");
                    }
                    else
                    {
                        Tween.By(m_blackTransitionIn, 0.15f, Quad.EaseIn, "X", (-m_blackTransitionIn.X).ToString());
                        Tween.By(m_blackScreen, 0.15f, Quad.EaseIn, "X", (-m_blackTransitionIn.X).ToString());
                        Tween.By(m_blackTransitionOut, 0.2f, Quad.EaseIn, "X", (-m_blackTransitionIn.X).ToString());
                        Tween.AddEndHandlerToLastTween(this, "BeginThreading");
                    }
                }
                base.OnEnter();
            }
        }

        public void BeginThreading()
        {
            Tween.StopAll(false);
            var thread = new Thread(BeginLoading);
            if (thread.CurrentCulture.Name != "en-US")
            {
                thread.CurrentCulture = new CultureInfo("en-US", false);
                thread.CurrentUICulture = new CultureInfo("en-US", false);
            }
            thread.Start();
        }

        private void BeginLoading()
        {
            m_isLoading = true;
            m_loadingComplete = false;
            var screenTypeToLoad = m_screenTypeToLoad;
            if (screenTypeToLoad <= 9)
            {
                switch (screenTypeToLoad)
                {
                    case 1:
                        m_levelToLoad = new CDGSplashScreen();
                        lock (m_levelToLoad)
                        {
                            m_loadingComplete = true;
                            return;
                        }
                        break;
                    case 2:
                    case 4:
                        return;
                    case 3:
                        goto IL_199;
                    case 5:
                        goto IL_205;
                    default:
                        if (screenTypeToLoad != 9)
                        {
                            return;
                        }
                        goto IL_1CF;
                }
            }
            if (screenTypeToLoad == 15)
            {
                goto IL_205;
            }
            if (screenTypeToLoad == 18)
            {
                goto IL_11E;
            }
            switch (screenTypeToLoad)
            {
                case 23:
                case 24:
                    goto IL_205;
                case 25:
                case 26:
                    return;
                case 27:
                    goto IL_199;
                case 28:
                    goto IL_E8;
                case 29:
                    break;
                default:
                    return;
            }
            m_levelToLoad = new DemoEndScreen();
            lock (m_levelToLoad)
            {
                m_loadingComplete = true;
                return;
            }
            IL_E8:
            m_levelToLoad = new DemoStartScreen();
            lock (m_levelToLoad)
            {
                m_loadingComplete = true;
                return;
            }
            IL_11E:
            m_levelToLoad = new CreditsScreen();
            var isEnding = true;
            var screens = ScreenManager.GetScreens();
            for (var i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                if (screen is TitleScreen)
                {
                    isEnding = false;
                    break;
                }
            }
            (m_levelToLoad as CreditsScreen).IsEnding = isEnding;
            lock (m_levelToLoad)
            {
                m_loadingComplete = true;
                return;
            }
            IL_199:
            m_levelToLoad = new TitleScreen();
            lock (m_levelToLoad)
            {
                m_loadingComplete = true;
                return;
            }
            IL_1CF:
            m_levelToLoad = new LineageScreen();
            lock (m_levelToLoad)
            {
                m_loadingComplete = true;
                return;
            }
            IL_205:
            var rCScreenManager = ScreenManager as RCScreenManager;
            var area1List = Game.Area1List;
            m_levelToLoad = null;
            if (m_screenTypeToLoad == 15)
            {
                m_levelToLoad = LevelBuilder2.CreateStartingRoom();
            }
            else if (m_screenTypeToLoad == 23)
            {
                m_levelToLoad = LevelBuilder2.CreateTutorialRoom();
            }
            else if (m_screenTypeToLoad == 24)
            {
                m_levelToLoad = LevelBuilder2.CreateEndingRoom();
            }
            else
            {
                var levelScreen = (ScreenManager as RCScreenManager).GetLevelScreen();
                if (levelScreen != null)
                {
                    if (Game.PlayerStats.LockCastle)
                    {
                        try
                        {
                            m_levelToLoad = (ScreenManager.Game as Game).SaveManager.LoadMap();
                        }
                        catch
                        {
                            m_gameCrashed = true;
                        }
                        if (!m_gameCrashed)
                        {
                            (ScreenManager.Game as Game).SaveManager.LoadFiles(m_levelToLoad as ProceduralLevelScreen,
                                SaveType.MapData);
                            Game.PlayerStats.LockCastle = false;
                        }
                    }
                    else
                    {
                        m_levelToLoad = LevelBuilder2.CreateLevel(levelScreen.RoomList[0], area1List);
                    }
                }
                else if (Game.PlayerStats.LoadStartingRoom)
                {
                    Console.WriteLine("This should only be used for debug purposes");
                    m_levelToLoad = LevelBuilder2.CreateLevel(null, area1List);
                    (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.Map, SaveType.MapData);
                }
                else
                {
                    try
                    {
                        m_levelToLoad = (ScreenManager.Game as Game).SaveManager.LoadMap();
                        (ScreenManager.Game as Game).SaveManager.LoadFiles(m_levelToLoad as ProceduralLevelScreen,
                            SaveType.MapData);
                    }
                    catch
                    {
                        m_gameCrashed = true;
                    }
                    if (!m_gameCrashed)
                    {
                        Game.ScreenManager.Player.Position =
                            new Vector2((m_levelToLoad as ProceduralLevelScreen).RoomList[1].X, 420f);
                    }
                }
            }
            if (!m_gameCrashed)
            {
                lock (m_levelToLoad)
                {
                    var proceduralLevelScreen = m_levelToLoad as ProceduralLevelScreen;
                    proceduralLevelScreen.Player = rCScreenManager.Player;
                    rCScreenManager.Player.AttachLevel(proceduralLevelScreen);
                    for (var j = 0; j < proceduralLevelScreen.RoomList.Count; j++)
                    {
                        proceduralLevelScreen.RoomList[j].RoomNumber = j + 1;
                    }
                    rCScreenManager.AttachMap(proceduralLevelScreen);
                    if (!m_wipeTransition)
                    {
                        Thread.Sleep(100);
                    }
                    m_loadingComplete = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (m_gameCrashed)
            {
                (ScreenManager.Game as Game).SaveManager.ForceBackup();
            }
            if (m_isLoading && m_loadingComplete && !m_gameCrashed)
            {
                EndLoading();
            }
            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            m_gateSprite.GetChildAt(1).Rotation += 120f*num;
            m_gateSprite.GetChildAt(2).Rotation -= 120f*num;
            if (m_shakeScreen)
            {
                UpdateShake();
            }
            base.Update(gameTime);
        }

        public void EndLoading()
        {
            m_isLoading = false;
            var screenManager = ScreenManager;
            var screens = ScreenManager.GetScreens();
            for (var i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                if (screen != this)
                {
                    screenManager.RemoveScreen(screen, true);
                }
                else
                {
                    screenManager.RemoveScreen(screen, false);
                }
            }
            ScreenManager = screenManager;
            m_levelToLoad.DrawIfCovered = true;
            if (m_screenTypeToLoad == 15)
            {
                if (Game.PlayerStats.IsDead)
                {
                    (m_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = true;
                }
                ScreenManager.AddScreen(m_levelToLoad, PlayerIndex.One);
                if (Game.PlayerStats.IsDead)
                {
                    ScreenManager.AddScreen((ScreenManager as RCScreenManager).SkillScreen, PlayerIndex.One);
                    (m_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = false;
                }
                m_levelToLoad.UpdateIfCovered = false;
            }
            else
            {
                ScreenManager.AddScreen(m_levelToLoad, PlayerIndex.One);
                m_levelToLoad.UpdateIfCovered = true;
            }
            ScreenManager.AddScreen(this, PlayerIndex.One);
            AddFinalTransition();
        }

        public void AddFinalTransition()
        {
            if (m_screenTypeToLoad == 27)
            {
                BackBufferOpacity = 1f;
                Tween.To(this, 2f, Tween.EaseNone, "BackBufferOpacity", "0");
                Tween.AddEndHandlerToLastTween(ScreenManager, "RemoveScreen", this, true);
                return;
            }
            if (!m_wipeTransition)
            {
                SoundManager.PlaySound("GateRise");
                Tween.To(m_gateSprite, 1f, Tween.EaseNone, "Y", (-m_gateSprite.Height).ToString());
                Tween.AddEndHandlerToLastTween(ScreenManager, "RemoveScreen", this, true);
                return;
            }
            m_blackTransitionOut.Y = -500f;
            Tween.By(m_blackTransitionIn, 0.2f, Tween.EaseNone, "X", (-m_blackTransitionIn.Bounds.Width).ToString());
            Tween.By(m_blackScreen, 0.2f, Tween.EaseNone, "X", (-m_blackTransitionIn.Bounds.Width).ToString());
            Tween.By(m_blackTransitionOut, 0.2f, Tween.EaseNone, "X", (-m_blackTransitionIn.Bounds.Width).ToString());
            Tween.AddEndHandlerToLastTween(ScreenManager, "RemoveScreen", this, true);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (m_screenTypeToLoad != 27)
            {
                if (!m_wipeTransition)
                {
                    m_gateSprite.Draw(Camera);
                    m_effectPool.Draw(Camera);
                    m_loadingText.Position = new Vector2(m_gateSprite.X + 995f, m_gateSprite.Y + 540f);
                    m_loadingText.Draw(Camera);
                }
                else
                {
                    m_blackTransitionIn.Draw(Camera);
                    m_blackTransitionOut.Draw(Camera);
                    m_blackScreen.Draw(Camera);
                }
            }
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.White*BackBufferOpacity);
            Camera.End();
            base.Draw(gameTime);
        }

        public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
        {
            m_screenShakeMagnitude = magnitude;
            m_horizontalShake = horizontalShake;
            m_verticalShake = verticalShake;
            m_shakeScreen = true;
        }

        public void UpdateShake()
        {
            if (m_horizontalShake)
            {
                m_gateSprite.X = CDGMath.RandomPlusMinus()*(CDGMath.RandomFloat(0f, 1f)*m_screenShakeMagnitude);
            }
            if (m_verticalShake)
            {
                m_gateSprite.Y = CDGMath.RandomPlusMinus()*(CDGMath.RandomFloat(0f, 1f)*m_screenShakeMagnitude);
            }
        }

        public void StopScreenShake()
        {
            m_shakeScreen = false;
            m_gateSprite.X = 0f;
            m_gateSprite.Y = 0f;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Loading Screen");
                m_loadingText.Dispose();
                m_loadingText = null;
                m_levelToLoad = null;
                m_gateSprite.Dispose();
                m_gateSprite = null;
                m_effectPool.Dispose();
                m_effectPool = null;
                m_blackTransitionIn.Dispose();
                m_blackTransitionIn = null;
                m_blackScreen.Dispose();
                m_blackScreen = null;
                m_blackTransitionOut.Dispose();
                m_blackTransitionOut = null;
                base.Dispose();
            }
        }
    }
}
