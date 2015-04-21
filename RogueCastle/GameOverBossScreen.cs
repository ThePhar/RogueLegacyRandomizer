/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class GameOverBossScreen : Screen
    {
        private EnemyObj_LastBoss m_lastBoss;
        private ObjContainer m_dialoguePlate;
        private KeyIconTextObj m_continueText;
        private SpriteObj m_playerGhost;
        private SpriteObj m_king;
        private SpriteObj m_spotlight;
        private LineageObj m_playerFrame;
        private FrameSoundObj m_bossFallSound;
        private FrameSoundObj m_bossKneesSound;
        private Vector2 m_initialCameraPos;
        private bool m_lockControls;
        public float BackBufferOpacity { get; set; }

        public GameOverBossScreen()
        {
            DrawIfCovered = true;
        }

        public override void PassInData(List<object> objList)
        {
            m_lastBoss = (objList[0] as EnemyObj_LastBoss);
            m_bossKneesSound = new FrameSoundObj(m_lastBoss, 3, "FinalBoss_St2_Deathsceen_Knees");
            m_bossFallSound = new FrameSoundObj(m_lastBoss, 13, "FinalBoss_St2_Deathsceen_Fall");
            base.PassInData(objList);
        }

        public override void LoadContent()
        {
            m_continueText = new KeyIconTextObj(Game.JunicodeFont);
            m_continueText.FontSize = 14f;
            m_continueText.Align = Types.TextAlign.Right;
            m_continueText.Opacity = 0f;
            m_continueText.Position = new Vector2(1270f, 30f);
            m_continueText.ForceDraw = true;
            Vector2 dropShadow = new Vector2(2f, 2f);
            Color textureColor = new Color(255, 254, 128);
            m_dialoguePlate = new ObjContainer("DialogBox_Character");
            m_dialoguePlate.Position = new Vector2(660f, 610f);
            m_dialoguePlate.ForceDraw = true;
            TextObj textObj = new TextObj(Game.JunicodeFont);
            textObj.Align = Types.TextAlign.Centre;
            textObj.Text = "Your valor shown in battle shall never be forgotten.";
            textObj.FontSize = 18f;
            textObj.DropShadow = dropShadow;
            textObj.Position = new Vector2(0f, -(float) m_dialoguePlate.Height/2 + 25);
            m_dialoguePlate.AddChild(textObj);
            KeyIconTextObj keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
            keyIconTextObj.FontSize = 12f;
            keyIconTextObj.Align = Types.TextAlign.Centre;
            keyIconTextObj.Text = "\"Arrrrggghhhh\"";
            keyIconTextObj.DropShadow = dropShadow;
            keyIconTextObj.Y = 10f;
            keyIconTextObj.TextureColor = textureColor;
            m_dialoguePlate.AddChild(keyIconTextObj);
            TextObj textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.FontSize = 8f;
            textObj2.Text = "-Player X's parting words";
            textObj2.Y = keyIconTextObj.Y;
            textObj2.Y += 40f;
            textObj2.X += 20f;
            textObj2.DropShadow = dropShadow;
            m_dialoguePlate.AddChild(textObj2);
            m_playerGhost = new SpriteObj("PlayerGhost_Sprite");
            m_playerGhost.AnimationDelay = 0.1f;
            m_spotlight = new SpriteObj("GameOverSpotlight_Sprite");
            m_spotlight.Rotation = 90f;
            m_spotlight.ForceDraw = true;
            m_spotlight.Position = new Vector2(660f, 40 + m_spotlight.Height);
            m_playerFrame = new LineageObj(null, true);
            m_playerFrame.DisablePlaque = true;
            m_king = new SpriteObj("King_Sprite");
            m_king.OutlineWidth = 2;
            m_king.AnimationDelay = 0.1f;
            m_king.PlayAnimation();
            m_king.Scale = new Vector2(2f, 2f);
            base.LoadContent();
        }

        public override void OnEnter()
        {
            m_initialCameraPos = Camera.Position;
            SetObjectKilledPlayerText();
            m_playerFrame.Opacity = 0f;
            m_playerFrame.Position = m_lastBoss.Position;
            m_playerFrame.SetTraits(Vector2.Zero);
            m_playerFrame.IsFemale = false;
            m_playerFrame.Class = 0;
            m_playerFrame.Y -= 120f;
            m_playerFrame.SetPortrait(8, 1, 1);
            m_playerFrame.UpdateData();
            Tween.To(m_playerFrame, 1f, Tween.EaseNone, "delay", "4", "Opacity", "1");
            SoundManager.StopMusic(0.5f);
            m_lockControls = false;
            SoundManager.PlaySound("Player_Death_FadeToBlack");
            m_continueText.Text = "Press [Input:" + 0 + "] to move on";
            m_lastBoss.Visible = true;
            m_lastBoss.Opacity = 1f;
            m_continueText.Opacity = 0f;
            m_dialoguePlate.Opacity = 0f;
            m_playerGhost.Opacity = 0f;
            m_spotlight.Opacity = 0f;
            m_playerGhost.Position = new Vector2(m_lastBoss.X - m_playerGhost.Width/2, m_lastBoss.Bounds.Top - 20);
            Tween.RunFunction(3f, typeof (SoundManager), "PlaySound", "Player_Ghost");
            Tween.To(m_playerGhost, 0.5f, Linear.EaseNone, "delay", "3", "Opacity", "0.4");
            Tween.By(m_playerGhost, 2f, Linear.EaseNone, "delay", "3", "Y", "-150");
            m_playerGhost.Opacity = 0.4f;
            Tween.To(m_playerGhost, 0.5f, Linear.EaseNone, "delay", "4", "Opacity", "0");
            m_playerGhost.Opacity = 0f;
            m_playerGhost.PlayAnimation();
            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "1");
            Tween.To(m_spotlight, 0.1f, Linear.EaseNone, "delay", "1", "Opacity", "1");
            Tween.AddEndHandlerToLastTween(typeof (SoundManager), "PlaySound", "Player_Death_Spotlight");
            Tween.RunFunction(2f, typeof (SoundManager), "PlaySound", "FinalBoss_St1_DeathGrunt");
            Tween.RunFunction(1.2f, typeof (SoundManager), "PlayMusic", "GameOverBossStinger", false, 0.5f);
            Tween.To(Camera, 1f, Quad.EaseInOut, "X", m_lastBoss.AbsX.ToString(), "Y",
                (m_lastBoss.Bounds.Bottom - 10).ToString());
            Tween.RunFunction(2f, m_lastBoss, "PlayAnimation", false);
            (m_dialoguePlate.GetChildAt(2) as TextObj).Text = "The sun... I had forgotten how it feels...";
            (m_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + m_lastBoss.Name + "'s Parting Words";
            Tween.To(m_dialoguePlate, 0.5f, Tween.EaseNone, "delay", "2", "Opacity", "1");
            Tween.RunFunction(4f, this, "DropStats");
            Tween.To(m_continueText, 0.4f, Linear.EaseNone, "delay", "4", "Opacity", "1");
            base.OnEnter();
        }

        public void DropStats()
        {
            Vector2 arg_05_0 = Vector2.Zero;
            float num = 0f;
            Vector2 topLeftCorner = Camera.TopLeftCorner;
            topLeftCorner.X += 200f;
            topLeftCorner.Y += 450f;
            m_king.Position = topLeftCorner;
            m_king.Visible = true;
            m_king.StopAnimation();
            m_king.Scale /= 2f;
            m_king.Opacity = 0f;
            num += 0.05f;
            Tween.To(m_king, 0f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "1");
        }

        private void SetObjectKilledPlayerText()
        {
            TextObj textObj = m_dialoguePlate.GetChildAt(1) as TextObj;
            textObj.Text = m_lastBoss.Name + " has been slain by " + Game.PlayerStats.PlayerName;
        }

        public override void HandleInput()
        {
            if (!m_lockControls &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)) && m_continueText.Opacity == 1f)
            {
                m_lockControls = true;
                ExitTransition();
            }
            base.HandleInput();
        }

        private void ExitTransition()
        {
            Tween.StopAll(false);
            SoundManager.StopMusic(1f);
            Tween.To(this, 0.5f, Quad.EaseIn, "BackBufferOpacity", "0");
            Tween.To(m_lastBoss, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(m_dialoguePlate, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(m_continueText, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(m_playerGhost, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(m_king, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(m_spotlight, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(m_playerFrame, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(Camera, 0.5f, Quad.EaseInOut, "X", m_initialCameraPos.X.ToString(), "Y",
                m_initialCameraPos.Y.ToString());
            Tween.RunFunction(0.5f, ScreenManager, "HideCurrentScreen");
        }

        public override void OnExit()
        {
            BackBufferOpacity = 0f;
            Game.ScreenManager.Player.UnlockControls();
            Game.ScreenManager.Player.AttachedLevel.CameraLockedToPlayer = true;
            (Game.ScreenManager.GetLevelScreen().CurrentRoom as LastBossRoom).BossCleanup();
            base.OnExit();
        }

        public override void Update(GameTime gameTime)
        {
            if (m_lastBoss.SpriteName == "EnemyLastBossDeath_Character")
            {
                m_bossKneesSound.Update();
                m_bossFallSound.Update();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.GetTransformation());
            Camera.Draw(Game.GenericTexture,
                new Rectangle((int) Camera.TopLeftCorner.X - 10, (int) Camera.TopLeftCorner.Y - 10, 1420, 820),
                Color.Black*BackBufferOpacity);
            m_king.Draw(Camera);
            m_playerFrame.Draw(Camera);
            m_lastBoss.Draw(Camera);
            if (m_playerGhost.Opacity > 0f)
            {
                m_playerGhost.X += (float) Math.Sin(Game.TotalGameTime*5f);
            }
            m_playerGhost.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            m_spotlight.Draw(Camera);
            m_dialoguePlate.Draw(Camera);
            m_continueText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Game Over Boss Screen");
                m_lastBoss = null;
                m_dialoguePlate.Dispose();
                m_dialoguePlate = null;
                m_continueText.Dispose();
                m_continueText = null;
                m_playerGhost.Dispose();
                m_playerGhost = null;
                m_spotlight.Dispose();
                m_spotlight = null;
                m_bossFallSound.Dispose();
                m_bossFallSound = null;
                m_bossKneesSound.Dispose();
                m_bossKneesSound = null;
                m_playerFrame.Dispose();
                m_playerFrame = null;
                m_king.Dispose();
                m_king = null;
                base.Dispose();
            }
        }
    }
}