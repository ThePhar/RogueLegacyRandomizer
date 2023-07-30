// RogueLegacyRandomizer - GameOverBossScreen.cs
// Last Modified 2023-07-30 1:29 PM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class GameOverBossScreen : Screen
    {
        private FrameSoundObj     _bossFallSound;
        private FrameSoundObj     _bossKneesSound;
        private KeyIconTextObj    _continueText;
        private ObjContainer      _dialoguePlate;
        private Vector2           _initialCameraPos;
        private SpriteObj         _king;
        private EnemyObj_LastBoss _lastBoss;
        private bool              _lockControls;
        private LineageObj        _playerFrame;
        private SpriteObj         _playerGhost;
        private SpriteObj         _spotlight;

        public GameOverBossScreen()
        {
            DrawIfCovered = true;
        }

        public float BackBufferOpacity { get; set; }

        public override void PassInData(List<object> objList)
        {
            _lastBoss = objList[0] as EnemyObj_LastBoss;
            _bossKneesSound = new FrameSoundObj(_lastBoss, 3, "FinalBoss_St2_Deathsceen_Knees");
            _bossFallSound = new FrameSoundObj(_lastBoss, 13, "FinalBoss_St2_Deathsceen_Fall");
            base.PassInData(objList);
        }

        public override void LoadContent()
        {
            _continueText = new KeyIconTextObj(Game.JunicodeFont);
            _continueText.FontSize = 14f;
            _continueText.Align = Types.TextAlign.Right;
            _continueText.Opacity = 0f;
            _continueText.Position = new Vector2(1270f, 30f);
            _continueText.ForceDraw = true;
            var dropShadow = new Vector2(2f, 2f);
            var textureColor = new Color(255, 254, 128);
            _dialoguePlate = new ObjContainer("DialogBox_Character");
            _dialoguePlate.Position = new Vector2(660f, 610f);
            _dialoguePlate.ForceDraw = true;
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Align = Types.TextAlign.Centre;
            textObj.Text = "Your valor shown in battle shall never be forgotten.";
            textObj.FontSize = 18f;
            textObj.DropShadow = dropShadow;
            textObj.Position = new Vector2(0f, -(float) _dialoguePlate.Height / 2 + 25);
            _dialoguePlate.AddChild(textObj);
            var keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
            keyIconTextObj.FontSize = 12f;
            keyIconTextObj.Align = Types.TextAlign.Centre;
            keyIconTextObj.Text = "\"Arrrrggghhhh\"";
            keyIconTextObj.DropShadow = dropShadow;
            keyIconTextObj.Y = 10f;
            keyIconTextObj.TextureColor = textureColor;
            _dialoguePlate.AddChild(keyIconTextObj);
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.FontSize = 8f;
            textObj2.Text = "-Player X's parting words";
            textObj2.Y = keyIconTextObj.Y;
            textObj2.Y += 40f;
            textObj2.X += 20f;
            textObj2.DropShadow = dropShadow;
            _dialoguePlate.AddChild(textObj2);
            _playerGhost = new SpriteObj("PlayerGhost_Sprite");
            _playerGhost.AnimationDelay = 0.1f;
            _spotlight = new SpriteObj("GameOverSpotlight_Sprite");
            _spotlight.Rotation = 90f;
            _spotlight.ForceDraw = true;
            _spotlight.Position = new Vector2(660f, 40 + _spotlight.Height);
            _playerFrame = new LineageObj(null, true);
            _playerFrame.DisablePlaque = true;
            _king = new SpriteObj("King_Sprite");
            _king.OutlineWidth = 2;
            _king.AnimationDelay = 0.1f;
            _king.PlayAnimation();
            _king.Scale = new Vector2(2f, 2f);
            base.LoadContent();
        }

        public override void OnEnter()
        {
            _initialCameraPos = Camera.Position;
            SetObjectKilledPlayerText();
            _playerFrame.Opacity = 0f;
            _playerFrame.Position = _lastBoss.Position;
            _playerFrame.SetTraits(Vector2.Zero);
            _playerFrame.IsFemale = false;
            _playerFrame.Class = 0;
            _playerFrame.Y -= 120f;
            _playerFrame.SetPortrait(8, 1, 1);
            _playerFrame.UpdateData();
            Tween.To(_playerFrame, 1f, Tween.EaseNone, "delay", "4", "Opacity", "1");
            SoundManager.StopMusic(0.5f);
            _lockControls = false;
            SoundManager.PlaySound("Player_Death_FadeToBlack");
            _continueText.Text = "Press [Input:" + 0 + "] to move on";
            _lastBoss.Visible = true;
            _lastBoss.Opacity = 1f;
            _continueText.Opacity = 0f;
            _dialoguePlate.Opacity = 0f;
            _playerGhost.Opacity = 0f;
            _spotlight.Opacity = 0f;
            _playerGhost.Position = new Vector2(_lastBoss.X - _playerGhost.Width / 2, _lastBoss.Bounds.Top - 20);
            Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", "Player_Ghost");
            Tween.To(_playerGhost, 0.5f, Linear.EaseNone, "delay", "3", "Opacity", "0.4");
            Tween.By(_playerGhost, 2f, Linear.EaseNone, "delay", "3", "Y", "-150");
            _playerGhost.Opacity = 0.4f;
            Tween.To(_playerGhost, 0.5f, Linear.EaseNone, "delay", "4", "Opacity", "0");
            _playerGhost.Opacity = 0f;
            _playerGhost.PlayAnimation();
            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "1");
            Tween.To(_spotlight, 0.1f, Linear.EaseNone, "delay", "1", "Opacity", "1");
            Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", "Player_Death_Spotlight");
            Tween.RunFunction(2f, typeof(SoundManager), "PlaySound", "FinalBoss_St1_DeathGrunt");
            Tween.RunFunction(1.2f, typeof(SoundManager), "PlayMusic", "GameOverBossStinger", false, 0.5f);
            Tween.To(Camera, 1f, Quad.EaseInOut, "X", _lastBoss.AbsX.ToString(), "Y",
                (_lastBoss.Bounds.Bottom - 10).ToString());
            Tween.RunFunction(2f, _lastBoss, "PlayAnimation", false);
            (_dialoguePlate.GetChildAt(2) as TextObj).Text = Game.FinalWords;
            (_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + _lastBoss.Name + "'s Parting Words";
            Tween.To(_dialoguePlate, 0.5f, Tween.EaseNone, "delay", "2", "Opacity", "1");
            Tween.RunFunction(4f, this, "DropStats");
            Tween.To(_continueText, 0.4f, Linear.EaseNone, "delay", "4", "Opacity", "1");
            base.OnEnter();
        }

        public void DropStats()
        {
            var arg_05_0 = Vector2.Zero;
            var num = 0f;
            var topLeftCorner = Camera.TopLeftCorner;
            topLeftCorner.X += 200f;
            topLeftCorner.Y += 450f;
            _king.Position = topLeftCorner;
            _king.Visible = true;
            _king.StopAnimation();
            _king.Scale /= 2f;
            _king.Opacity = 0f;
            num += 0.05f;
            Tween.To(_king, 0f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "1");
        }

        private void SetObjectKilledPlayerText()
        {
            var textObj = _dialoguePlate.GetChildAt(1) as TextObj;
            textObj.Text = _lastBoss.Name + " has been slain by " + Game.PlayerStats.PlayerName;
        }

        public override void HandleInput()
        {
            if (!_lockControls &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                 Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)) && _continueText.Opacity == 1f)
            {
                _lockControls = true;
                ExitTransition();
            }

            base.HandleInput();
        }

        private void ExitTransition()
        {
            Tween.StopAll(false);
            SoundManager.StopMusic(1f);
            Tween.To(this, 0.5f, Quad.EaseIn, "BackBufferOpacity", "0");
            Tween.To(_lastBoss, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(_dialoguePlate, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(_continueText, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(_playerGhost, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(_king, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(_spotlight, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(_playerFrame, 0.5f, Quad.EaseIn, "Opacity", "0");
            Tween.To(Camera, 0.5f, Quad.EaseInOut, "X", _initialCameraPos.X.ToString(), "Y",
                _initialCameraPos.Y.ToString());
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
            if (_lastBoss.SpriteName == "EnemyLastBossDeath_Character")
            {
                _bossKneesSound.Update();
                _bossFallSound.Update();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.GetTransformation());
            Camera.Draw(Game.GenericTexture,
                new Rectangle((int) Camera.TopLeftCorner.X - 10, (int) Camera.TopLeftCorner.Y - 10, 1420, 820),
                Color.Black * BackBufferOpacity);
            _king.Draw(Camera);
            _playerFrame.Draw(Camera);
            _lastBoss.Draw(Camera);
            if (_playerGhost.Opacity > 0f)
            {
                _playerGhost.X += (float) Math.Sin(Game.TotalGameTimeSeconds * 5f);
            }

            _playerGhost.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            _spotlight.Draw(Camera);
            _dialoguePlate.Draw(Camera);
            _continueText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Game Over Boss Screen");
            _lastBoss = null;
            _dialoguePlate.Dispose();
            _dialoguePlate = null;
            _continueText.Dispose();
            _continueText = null;
            _playerGhost.Dispose();
            _playerGhost = null;
            _spotlight.Dispose();
            _spotlight = null;
            _bossFallSound.Dispose();
            _bossFallSound = null;
            _bossKneesSound.Dispose();
            _bossKneesSound = null;
            _playerFrame.Dispose();
            _playerFrame = null;
            _king.Dispose();
            _king = null;
            base.Dispose();
        }
    }
}