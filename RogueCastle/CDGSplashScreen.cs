// 
// RogueLegacyArchipelago - CDGSplashScreen.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Threading;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class CDGSplashScreen : Screen
    {
        private bool m_fadingOut;
        private bool m_levelDataLoaded;
        private TextObj m_loadingText;
        private SpriteObj m_logo;
        private float m_totalElapsedTime;

        public override void LoadContent()
        {
            m_logo = new SpriteObj("CDGLogo_Sprite");
            m_logo.Position = new Vector2(660f, 360f);
            m_logo.Rotation = 90f;
            m_logo.ForceDraw = true;
            m_loadingText = new TextObj(Game.JunicodeFont);
            m_loadingText.FontSize = 18f;
            m_loadingText.Align = Types.TextAlign.Right;
            m_loadingText.Text = "...Loading";
            m_loadingText.TextureColor = new Color(100, 100, 100);
            m_loadingText.Position = new Vector2(1280f, 630f);
            m_loadingText.ForceDraw = true;
            m_loadingText.Opacity = 0f;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            m_levelDataLoaded = false;
            m_fadingOut = false;
            var thread = new Thread(LoadLevelData);
            thread.Start();
            m_logo.Opacity = 0f;
            Tween.To(m_logo, 1f, Linear.EaseNone, "delay", "0.5", "Opacity", "1");
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
                m_levelDataLoaded = true;
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
            if (!m_levelDataLoaded && m_logo.Opacity == 1f)
            {
                var opacity = (float) Math.Abs(Math.Sin(m_totalElapsedTime));
                m_totalElapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
                m_loadingText.Opacity = opacity;
            }

            if (m_levelDataLoaded && !m_fadingOut)
            {
                m_fadingOut = true;
                var opacity2 = m_logo.Opacity;
                m_logo.Opacity = 1f;
                Tween.To(m_logo, 1f, Linear.EaseNone, "delay", "1.5", "Opacity", "0");
                Tween.AddEndHandlerToLastTween(this, "LoadNextScreen");
                Tween.To(m_loadingText, 0.5f, Tween.EaseNone, "Opacity", "0");
                m_logo.Opacity = opacity2;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin();
            m_logo.Draw(Camera);
            m_loadingText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing CDG Splash Screen");
                m_logo.Dispose();
                m_logo = null;
                m_loadingText.Dispose();
                m_loadingText = null;
                base.Dispose();
            }
        }
    }
}