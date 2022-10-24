// Rogue Legacy Randomizer - CDGSplashScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Threading;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class CDGSplashScreen : Screen
    {
        private bool      _fadingOut;
        private bool      _levelDataLoaded;
        private TextObj   _loadingText;
        private SpriteObj _logo;
        private float     _totalElapsedTime;

        public override void LoadContent()
        {
            _logo = new SpriteObj("CDGLogo_Sprite");
            _logo.Position = new Vector2(660f, 360f);
            _logo.Rotation = 90f;
            _logo.ForceDraw = true;
            _loadingText = new TextObj(Game.JunicodeFont);
            _loadingText.FontSize = 18f;
            _loadingText.Align = Types.TextAlign.Right;
            _loadingText.Text = "...Loading";
            _loadingText.TextureColor = new Color(100, 100, 100);
            _loadingText.Position = new Vector2(1280f, 630f);
            _loadingText.ForceDraw = true;
            _loadingText.Opacity = 0f;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            _levelDataLoaded = false;
            _fadingOut = false;
            var thread = new Thread(LoadLevelData);
            thread.Start();
            _logo.Opacity = 0f;
            Tween.To(_logo, 1f, Linear.EaseNone, "delay", "0.5", "Opacity", "1");
            Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", "CDGSplashCreak");
            base.OnEnter();
        }

        private void LoadLevelData()
        {
            lock (this)
            {
                LevelBuilder2.Initialize();
                LevelParser.ParseRooms("Map_1x1", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_1x2", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_1x3", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_2x1", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_2x2", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_2x3", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_3x1", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_3x2", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_Special", ScreenManager.Game.Content);
                LevelParser.ParseRooms("Map_DLC1", ScreenManager.Game.Content, true);
                LevelBuilder2.IndexRoomList();
                _levelDataLoaded = true;
            }
        }

        public void LoadNextScreen()
        {
            if ((ScreenManager.Game as Game).SaveManager.FileExists(SaveType.PlayerData))
            {
                (ScreenManager.Game as Game).SaveManager.LoadFiles(null, SaveType.PlayerData);
                if (Game.PlayerStats.ShoulderPiece < 1 || Game.PlayerStats.HeadPiece < 1 ||
                    Game.PlayerStats.ChestPiece < 1)
                {
                    Game.PlayerStats.TutorialComplete = false;
                    return;
                }

                if (!Game.PlayerStats.TutorialComplete)
                {
                    (ScreenManager as RCScreenManager).DisplayScreen(23, true);
                    return;
                }

                (ScreenManager as RCScreenManager).DisplayScreen(3, true);
            }
            else
            {
                if (!Game.PlayerStats.TutorialComplete)
                {
                    (ScreenManager as RCScreenManager).DisplayScreen(23, true);
                    return;
                }

                (ScreenManager as RCScreenManager).DisplayScreen(3, true);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!_levelDataLoaded && _logo.Opacity == 1f)
            {
                var opacity = (float) Math.Abs(Math.Sin(_totalElapsedTime));
                _totalElapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
                _loadingText.Opacity = opacity;
            }

            if (_levelDataLoaded && !_fadingOut)
            {
                _fadingOut = true;
                var opacity2 = _logo.Opacity;
                _logo.Opacity = 1f;
                Tween.To(_logo, 1f, Linear.EaseNone, "delay", "1.5", "Opacity", "0");
                Tween.AddEndHandlerToLastTween(this, "LoadNextScreen");
                Tween.To(_loadingText, 0.5f, Tween.EaseNone, "Opacity", "0");
                _logo.Opacity = opacity2;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin();
            _logo.Draw(Camera);
            _loadingText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing CDG Splash Screen");
            _logo.Dispose();
            _logo = null;
            _loadingText.Dispose();
            _loadingText = null;
            base.Dispose();
        }
    }
}
