// Rogue Legacy Randomizer - DeathDefiedScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class DeathDefiedScreen : Screen
    {
        private Vector2   _cameraPos;
        private PlayerObj _player;
        private string    _songName;
        private SpriteObj _spotlight;
        private float     _storedMusicVolume;
        private SpriteObj _title;
        private SpriteObj _titlePlate;

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            new Vector2(2f, 2f);
            new Color(255, 254, 128);
            _spotlight = new SpriteObj("GameOverSpotlight_Sprite");
            _spotlight.Rotation = 90f;
            _spotlight.ForceDraw = true;
            _spotlight.Position = new Vector2(660f, 40 + _spotlight.Height);
            _titlePlate = new SpriteObj("SkillUnlockTitlePlate_Sprite");
            _titlePlate.Position = new Vector2(660f, 160f);
            _titlePlate.ForceDraw = true;
            _title = new SpriteObj("DeathDefyText_Sprite");
            _title.Position = _titlePlate.Position;
            _title.Y -= 40f;
            _title.ForceDraw = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            _cameraPos = new Vector2(Camera.X, Camera.Y);
            if (_player == null)
            {
                _player = (ScreenManager as RCScreenManager).Player;
            }

            _titlePlate.Scale = Vector2.Zero;
            _title.Scale = Vector2.Zero;
            _title.Opacity = 1f;
            _titlePlate.Opacity = 1f;
            _storedMusicVolume = SoundManager.GlobalMusicVolume;
            _songName = SoundManager.GetCurrentMusicName();
            Tween.To(typeof(SoundManager), 1f, Tween.EaseNone, "GlobalMusicVolume", (_storedMusicVolume * 0.1f).ToString());
            SoundManager.PlaySound("Player_Death_FadeToBlack");
            _player.Visible = true;
            _player.Opacity = 1f;
            _spotlight.Opacity = 0f;
            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "1");
            Tween.To(_spotlight, 0.1f, Linear.EaseNone, "delay", "1", "Opacity", "1");
            Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", "Player_Death_Spotlight");
            Tween.To(Camera, 1f, Quad.EaseInOut, "X", _player.AbsX.ToString(), "Y", (_player.Bounds.Bottom - 10).ToString(), "Zoom", "1");
            Tween.RunFunction(2f, this, "PlayerLevelUpAnimation");
            base.OnEnter();
        }

        public void PlayerLevelUpAnimation()
        {
            _player.ChangeSprite("PlayerLevelUp_Character");
            _player.PlayAnimation(false);
            Tween.To(_titlePlate, 0.5f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_title, 0.5f, Back.EaseOut, "delay", "0.1", "ScaleX", "0.8", "ScaleY", "0.8");
            Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "GetItemStinger3");
            Tween.RunFunction(2f, this, "ExitTransition");
        }

        public void ExitTransition()
        {
            Tween.To(typeof(SoundManager), 1f, Tween.EaseNone, "GlobalMusicVolume", _storedMusicVolume.ToString());
            Tween.To(Camera, 1f, Quad.EaseInOut, "X", _cameraPos.X.ToString(), "Y", _cameraPos.Y.ToString());
            Tween.To(_spotlight, 0.5f, Tween.EaseNone, "Opacity", "0");
            Tween.To(_titlePlate, 0.5f, Tween.EaseNone, "Opacity", "0");
            Tween.To(_title, 0.5f, Tween.EaseNone, "Opacity", "0");
            Tween.To(this, 0.2f, Tween.EaseNone, "delay", "1", "BackBufferOpacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.GetTransformation());
            Camera.Draw(Game.GenericTexture, new Rectangle((int) Camera.TopLeftCorner.X - 10, (int) Camera.TopLeftCorner.Y - 10, 1340, 740), Color.Black * BackBufferOpacity);
            _player.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            _spotlight.Draw(Camera);
            _titlePlate.Draw(Camera);
            _title.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Death Defied Screen");
            _player = null;
            _spotlight.Dispose();
            _spotlight = null;
            _title.Dispose();
            _title = null;
            _titlePlate.Dispose();
            _titlePlate = null;
            base.Dispose();
        }
    }
}
