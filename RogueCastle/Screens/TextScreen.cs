// 
//  Rogue Legacy Randomizer - TextScreen.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;

namespace RogueCastle.Screens
{
    public class TextScreen : Screen
    {
        private float     _backBufferOpacity;
        private float     _fadeInSpeed;
        private bool      _loadEndingAfterward;
        private SpriteObj _smoke1;
        private SpriteObj _smoke2;
        private SpriteObj _smoke3;
        private TextObj   _text;
        private float     _textDuration;
        private bool      _typewriteText;

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            var textureColor = new Color(200, 150, 55);
            _smoke1 = new SpriteObj("TextSmoke_Sprite");
            _smoke1.ForceDraw = true;
            _smoke1.Scale = new Vector2(2f, 2f);
            _smoke1.Opacity = 0.3f;
            _smoke1.TextureColor = textureColor;
            _smoke2 = _smoke1.Clone() as SpriteObj;
            _smoke2.Flip = SpriteEffects.FlipHorizontally;
            _smoke2.Opacity = 0.2f;
            _smoke3 = _smoke1.Clone() as SpriteObj;
            _smoke3.Scale = new Vector2(2.5f, 3f);
            _smoke3.Opacity = 0.15f;
            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            _backBufferOpacity = (float) objList[0];
            _fadeInSpeed = (float) objList[1];
            _textDuration = (float) objList[2];
            _typewriteText = (bool) objList[3];
            var textObj = objList[4] as TextObj;
            if (_text != null)
            {
                _text.Dispose();
                _text = null;
            }

            _text = textObj.Clone() as TextObj;
            _loadEndingAfterward = (bool) objList[5];
        }

        public override void OnEnter()
        {
            _smoke1.Position = new Vector2(CDGMath.RandomInt(300, 1000),
                _text.Y + _text.Height / 2f - 30f + CDGMath.RandomInt(-100, 100));
            _smoke2.Position = new Vector2(CDGMath.RandomInt(200, 700),
                _text.Y + _text.Height / 2f - 30f + CDGMath.RandomInt(-50, 50));
            _smoke3.Position = new Vector2(CDGMath.RandomInt(300, 800),
                _text.Y + _text.Height / 2f - 30f + CDGMath.RandomInt(-100, 100));
            _smoke1.Opacity = _smoke2.Opacity = _smoke3.Opacity = 0f;
            Tween.To(_smoke1, _fadeInSpeed, Tween.EaseNone, "Opacity", "0.3");
            Tween.To(_smoke2, _fadeInSpeed, Tween.EaseNone, "Opacity", "0.2");
            Tween.To(_smoke3, _fadeInSpeed, Tween.EaseNone, "Opacity", "0.15");
            BackBufferOpacity = 0f;
            _text.Opacity = 0f;
            Tween.To(this, _fadeInSpeed, Tween.EaseNone, "BackBufferOpacity", _backBufferOpacity.ToString());
            Tween.To(_text, _fadeInSpeed, Tween.EaseNone, "Opacity", "1");
            if (_typewriteText)
            {
                _text.Visible = false;
                Tween.RunFunction(_fadeInSpeed, _text, "BeginTypeWriting", _text.Text.Length * 0.05f, "");
            }
            else
            {
                _text.Visible = true;
            }

            base.OnEnter();
        }

        private void ExitTransition()
        {
            if (!_loadEndingAfterward)
            {
                Tween.To(_smoke1, _fadeInSpeed, Tween.EaseNone, "Opacity", "0");
                Tween.To(_smoke2, _fadeInSpeed, Tween.EaseNone, "Opacity", "0");
                Tween.To(_smoke3, _fadeInSpeed, Tween.EaseNone, "Opacity", "0");
                Tween.To(this, _fadeInSpeed, Tween.EaseNone, "BackBufferOpacity", "0");
                Tween.To(_text, _fadeInSpeed, Tween.EaseNone, "Opacity", "0");
                Tween.AddEndHandlerToLastTween(Game.ScreenManager, "HideCurrentScreen");
                return;
            }

            Game.ScreenManager.DisplayScreen(24, true);
        }

        public override void Update(GameTime gameTime)
        {
            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _smoke1.X += 5f * num;
            _smoke2.X += 15f * num;
            _smoke3.X += 10f * num;
            if (!_text.Visible && _text.IsTypewriting)
            {
                _text.Visible = true;
            }

            if (_textDuration > 0f)
            {
                _textDuration -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (_textDuration <= 0f)
                {
                    ExitTransition();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _smoke1.Draw(Camera);
            _smoke2.Draw(Camera);
            _smoke3.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _text.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Text Screen");
            _text.Dispose();
            _text = null;
            _smoke1.Dispose();
            _smoke1 = null;
            _smoke2.Dispose();
            _smoke2 = null;
            _smoke3.Dispose();
            _smoke3 = null;
            base.Dispose();
        }
    }
}
