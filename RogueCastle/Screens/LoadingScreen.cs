// 
//  Rogue Legacy Randomizer - LoadingScreen.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Globalization;
using System.Threading;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens
{
    public class LoadingScreen : Screen
    {
        private readonly byte             _screenTypeToLoad;
        private readonly bool             _wipeTransition;
        private          SpriteObj        _blackScreen;
        private          SpriteObj        _blackTransitionIn;
        private          SpriteObj        _blackTransitionOut;
        private          ImpactEffectPool _effectPool;
        private          bool             _gameCrashed;
        private          ObjContainer     _gateSprite;
        private          bool             _horizontalShake;
        private          bool             _isLoading;
        private          Screen           _levelToLoad;
        private          bool             _loadingComplete;
        private          TextObj          _loadingText;
        private          float            _screenShakeMagnitude;
        private          bool             _shakeScreen;
        private          bool             _verticalShake;

        public LoadingScreen(byte screenType, bool wipeTransition)
        {
            _screenTypeToLoad = screenType;
            _effectPool = new ImpactEffectPool(50);
            _wipeTransition = wipeTransition;
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            _loadingText = new TextObj();
            _loadingText.Font = Game.JunicodeLargeFont;
            _loadingText.Text = "Building";
            _loadingText.Align = Types.TextAlign.Centre;
            _loadingText.FontSize = 40f;
            _loadingText.OutlineWidth = 4;
            _loadingText.ForceDraw = true;
            _gateSprite = new ObjContainer("LoadingScreenGate_Character");
            _gateSprite.ForceDraw = true;
            _gateSprite.Scale = new Vector2(2f, 2f);
            _gateSprite.Y -= _gateSprite.Height;
            _effectPool.Initialize();
            _blackTransitionIn = new SpriteObj("Blank_Sprite");
            _blackTransitionIn.Rotation = 15f;
            _blackTransitionIn.Scale =
                new Vector2(1320 / _blackTransitionIn.Width, 2000 / _blackTransitionIn.Height);
            _blackTransitionIn.TextureColor = Color.Black;
            _blackTransitionIn.ForceDraw = true;
            _blackScreen = new SpriteObj("Blank_Sprite");
            _blackScreen.Scale = new Vector2(1320 / _blackScreen.Width, 720 / _blackScreen.Height);
            _blackScreen.TextureColor = Color.Black;
            _blackScreen.ForceDraw = true;
            _blackTransitionOut = new SpriteObj("Blank_Sprite");
            _blackTransitionOut.Rotation = 15f;
            _blackTransitionOut.Scale =
                new Vector2(1320 / _blackTransitionOut.Width, 2000 / _blackTransitionOut.Height);
            _blackTransitionOut.TextureColor = Color.Black;
            _blackTransitionOut.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            BackBufferOpacity = 0f;
            _gameCrashed = false;
            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
            {
                _loadingText.Text = "Jacking In";
            }
            else if (Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f)
            {
                _loadingText.Text = "Reminiscing";
            }
            else if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
            {
                _loadingText.Text = "Balding";
            }
            else
            {
                _loadingText.Text = "Building";
            }

            if (!_loadingComplete)
            {
                if (_screenTypeToLoad == 27)
                {
                    Tween.To(this, 0.05f, Tween.EaseNone, "BackBufferOpacity", "1");
                    Tween.RunFunction(1f, this, "BeginThreading");
                }
                else
                {
                    _blackTransitionIn.X = 0f;
                    _blackTransitionIn.X = 1320 - _blackTransitionIn.Bounds.Left;
                    _blackScreen.X = _blackTransitionIn.X;
                    _blackTransitionOut.X = _blackScreen.X + _blackScreen.Width;
                    if (!_wipeTransition)
                    {
                        SoundManager.PlaySound("GateDrop");
                        Tween.To(_gateSprite, 0.5f, Tween.EaseNone, "Y", "0");
                        Tween.RunFunction(0.3f, _effectPool, "LoadingGateSmokeEffect", 40);
                        Tween.RunFunction(0.3f, typeof(SoundManager), "PlaySound", "GateSlam");
                        Tween.RunFunction(0.55f, this, "ShakeScreen", 4, true, true);
                        Tween.RunFunction(0.65f, this, "StopScreenShake");
                        Tween.RunFunction(1.5f, this, "BeginThreading");
                    }
                    else
                    {
                        Tween.By(_blackTransitionIn, 0.15f, Quad.EaseIn, "X", (-_blackTransitionIn.X).ToString());
                        Tween.By(_blackScreen, 0.15f, Quad.EaseIn, "X", (-_blackTransitionIn.X).ToString());
                        Tween.By(_blackTransitionOut, 0.2f, Quad.EaseIn, "X", (-_blackTransitionIn.X).ToString());
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
            _isLoading = true;
            _loadingComplete = false;
            var screenTypeToLoad = _screenTypeToLoad;
            if (screenTypeToLoad <= 9)
            {
                switch (screenTypeToLoad)
                {
                    case 1:
                        _levelToLoad = new CDGSplashScreen();
                        lock (_levelToLoad)
                        {
                            _loadingComplete = true;
                            return;
                        }

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

                case 29:
                    break;

                default:
                    return;
            }

            IL_11E:
            _levelToLoad = new CreditsScreen();
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

            (_levelToLoad as CreditsScreen).IsEnding = isEnding;
            lock (_levelToLoad)
            {
                _loadingComplete = true;
                return;
            }

            IL_199:
            _levelToLoad = new TitleScreen();
            lock (_levelToLoad)
            {
                _loadingComplete = true;
                return;
            }

            IL_1CF:
            _levelToLoad = new LineageScreen();
            lock (_levelToLoad)
            {
                _loadingComplete = true;
                return;
            }

            IL_205:
            var rCScreenManager = ScreenManager as RCScreenManager;
            var area1List = Game.Area1List;
            _levelToLoad = null;
            if (_screenTypeToLoad == 15)
            {
                _levelToLoad = LevelBuilder2.CreateStartingRoom();
            }
            else if (_screenTypeToLoad == 23)
            {
                _levelToLoad = LevelBuilder2.CreateTutorialRoom();
            }
            else if (_screenTypeToLoad == 24)
            {
                _levelToLoad = LevelBuilder2.CreateEndingRoom();
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
                            _levelToLoad = (ScreenManager.Game as Game).SaveManager.LoadMap();
                        }
                        catch
                        {
                            _gameCrashed = true;
                        }

                        if (!_gameCrashed)
                        {
                            (ScreenManager.Game as Game).SaveManager.LoadFiles(_levelToLoad as ProceduralLevelScreen,
                                SaveType.MapData);
                            Game.PlayerStats.LockCastle = false;
                        }
                    }
                    else
                    {
                        _levelToLoad = LevelBuilder2.CreateLevel(levelScreen.RoomList[0], area1List);
                    }
                }
                else if (Game.PlayerStats.LoadStartingRoom)
                {
                    Console.WriteLine("This should only be used for debug purposes");
                    _levelToLoad = LevelBuilder2.CreateLevel(null, area1List);
                    (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.Map, SaveType.MapData);
                }
                else
                {
                    try
                    {
                        _levelToLoad = (ScreenManager.Game as Game).SaveManager.LoadMap();
                        (ScreenManager.Game as Game).SaveManager.LoadFiles(_levelToLoad as ProceduralLevelScreen,
                            SaveType.MapData);
                    }
                    catch
                    {
                        _gameCrashed = true;
                    }

                    if (!_gameCrashed)
                    {
                        Game.ScreenManager.Player.Position =
                            new Vector2((_levelToLoad as ProceduralLevelScreen).RoomList[1].X, 420f);
                    }
                }
            }

            if (!_gameCrashed)
            {
                lock (_levelToLoad)
                {
                    var proceduralLevelScreen = _levelToLoad as ProceduralLevelScreen;
                    proceduralLevelScreen.Player = rCScreenManager.Player;
                    rCScreenManager.Player.AttachLevel(proceduralLevelScreen);
                    for (var j = 0; j < proceduralLevelScreen.RoomList.Count; j++)
                    {
                        proceduralLevelScreen.RoomList[j].RoomNumber = j + 1;
                    }

                    rCScreenManager.AttachMap(proceduralLevelScreen);
                    if (!_wipeTransition)
                    {
                        Thread.Sleep(100);
                    }

                    _loadingComplete = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameCrashed)
            {
                (ScreenManager.Game as Game).SaveManager.ForceBackup();
            }

            if (_isLoading && _loadingComplete && !_gameCrashed)
            {
                EndLoading();
            }

            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _gateSprite.GetChildAt(1).Rotation += 120f * num;
            _gateSprite.GetChildAt(2).Rotation -= 120f * num;
            if (_shakeScreen)
            {
                UpdateShake();
            }

            base.Update(gameTime);
        }

        public void EndLoading()
        {
            _isLoading = false;
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
            _levelToLoad.DrawIfCovered = true;
            if (_screenTypeToLoad == 15)
            {
                if (Game.PlayerStats.IsDead)
                {
                    (_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = true;
                }

                ScreenManager.AddScreen(_levelToLoad, PlayerIndex.One);
                if (Game.PlayerStats.IsDead)
                {
                    ScreenManager.AddScreen((ScreenManager as RCScreenManager).SkillScreen, PlayerIndex.One);
                    (_levelToLoad as ProceduralLevelScreen).DisableRoomOnEnter = false;
                }

                _levelToLoad.UpdateIfCovered = false;
            }
            else
            {
                ScreenManager.AddScreen(_levelToLoad, PlayerIndex.One);
                _levelToLoad.UpdateIfCovered = true;
            }

            ScreenManager.AddScreen(this, PlayerIndex.One);
            AddFinalTransition();
        }

        public void AddFinalTransition()
        {
            if (_screenTypeToLoad == 27)
            {
                BackBufferOpacity = 1f;
                Tween.To(this, 2f, Tween.EaseNone, "BackBufferOpacity", "0");
                Tween.AddEndHandlerToLastTween(ScreenManager, "RemoveScreen", this, true);
                return;
            }

            if (!_wipeTransition)
            {
                SoundManager.PlaySound("GateRise");
                Tween.To(_gateSprite, 1f, Tween.EaseNone, "Y", (-_gateSprite.Height).ToString());
                Tween.AddEndHandlerToLastTween(ScreenManager, "RemoveScreen", this, true);
                return;
            }

            _blackTransitionOut.Y = -500f;
            Tween.By(_blackTransitionIn, 0.2f, Tween.EaseNone, "X", (-_blackTransitionIn.Bounds.Width).ToString());
            Tween.By(_blackScreen, 0.2f, Tween.EaseNone, "X", (-_blackTransitionIn.Bounds.Width).ToString());
            Tween.By(_blackTransitionOut, 0.2f, Tween.EaseNone, "X", (-_blackTransitionIn.Bounds.Width).ToString());
            Tween.AddEndHandlerToLastTween(ScreenManager, "RemoveScreen", this, true);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (_screenTypeToLoad != 27)
            {
                if (!_wipeTransition)
                {
                    _gateSprite.Draw(Camera);
                    _effectPool.Draw(Camera);
                    _loadingText.Position = new Vector2(_gateSprite.X + 995f, _gateSprite.Y + 540f);
                    _loadingText.Draw(Camera);
                }
                else
                {
                    _blackTransitionIn.Draw(Camera);
                    _blackTransitionOut.Draw(Camera);
                    _blackScreen.Draw(Camera);
                }
            }

            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.White * BackBufferOpacity);
            Camera.End();
            base.Draw(gameTime);
        }

        public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
        {
            _screenShakeMagnitude = magnitude;
            _horizontalShake = horizontalShake;
            _verticalShake = verticalShake;
            _shakeScreen = true;
        }

        public void UpdateShake()
        {
            if (_horizontalShake)
            {
                _gateSprite.X = CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
            }

            if (_verticalShake)
            {
                _gateSprite.Y = CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
            }
        }

        public void StopScreenShake()
        {
            _shakeScreen = false;
            _gateSprite.X = 0f;
            _gateSprite.Y = 0f;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Loading Screen");
            _loadingText.Dispose();
            _loadingText = null;
            _levelToLoad = null;
            _gateSprite.Dispose();
            _gateSprite = null;
            _effectPool.Dispose();
            _effectPool = null;
            _blackTransitionIn.Dispose();
            _blackTransitionIn = null;
            _blackScreen.Dispose();
            _blackScreen = null;
            _blackTransitionOut.Dispose();
            _blackTransitionOut = null;
            base.Dispose();
        }
    }
}
