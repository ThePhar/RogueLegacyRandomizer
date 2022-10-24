// Rogue Legacy Randomizer - TitleScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RandomChaos;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class TitleScreen : Screen
    {
        private SpriteObj             _bg;
        private SpriteObj             _castle;
        private TextObj               _copyrightText;
        private SpriteObj             _creditsIcon;
        private KeyIconTextObj        _creditsKey;
        private SpriteObj             _dlcIcon;
        private CrepuscularRays       _godRay;
        private RenderTarget2D        _godRayTexture;
        private float                 _hardCoreModeOpacity;
        private SpriteObj             _largeCloud1;
        private SpriteObj             _largeCloud2;
        private SpriteObj             _largeCloud3;
        private SpriteObj             _largeCloud4;
        private SpriteObj             _logo;
        private bool                  _optionsEntered;
        private SpriteObj             _optionsIcon;
        private KeyIconTextObj        _optionsKey;
        private PostProcessingManager _ppm;
        private KeyIconTextObj        _pressStartText;
        private TextObj               _pressStartText2;
        private SpriteObj             _profileCard;
        private KeyIconTextObj        _profileCardKey;
        private float                 _randomSeagullSfx;
        private Cue                   _seagullCue;
        private SpriteObj             _smallCloud1;
        private SpriteObj             _smallCloud2;
        private SpriteObj             _smallCloud3;
        private SpriteObj             _smallCloud4;
        private SpriteObj             _smallCloud5;
        private bool                  _startPressed;
        private TextObj               _titleText;
        private TextObj               _versionNumber;

        public override void LoadContent()
        {
            // Post-Processing + Fancy God-Rays
            _ppm = new PostProcessingManager(ScreenManager.Game, ScreenManager.Camera);
            _godRay = new CrepuscularRays(ScreenManager.Game, Vector2.One * 0.5f, "GameSpritesheets/flare3", 2f, 0.97f, 0.97f, 0.5f, 1.25f);
            _ppm.AddEffect(_godRay);
            _godRay.LightSource = new Vector2(0.495f, 0.3f);
            _godRayTexture = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color, DepthFormat.None);

            // Background
            _bg = new SpriteObj("TitleBG_Sprite");
            _bg.Scale = new Vector2(1320f / _bg.Width, 720f / _bg.Height);
            _bg.TextureColor = Color.Red;

            // Hardcore Mode?
            _hardCoreModeOpacity = 0f;

            // Logo
            _logo = new SpriteObj("TitleLogo_Sprite")
            {
                Position = new Vector2(660f, 360f),
                DropShadow = new Vector2(0f, 5f)
            };

            // Castle
            _castle = new SpriteObj("TitleCastle_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            };
            _castle.Position = new Vector2(630f, 720 - _castle.Height / 2);

            // Clouds
            _smallCloud1 = new SpriteObj("TitleSmallCloud1_Sprite")
            {
                Position = new Vector2(660f, 0f)
            };
            _smallCloud2 = new SpriteObj("TitleSmallCloud2_Sprite")
            {
                Position = _smallCloud1.Position
            };
            _smallCloud3 = new SpriteObj("TitleSmallCloud3_Sprite")
            {
                Position = _smallCloud1.Position
            };
            _smallCloud4 = new SpriteObj("TitleSmallCloud4_Sprite")
            {
                Position = _smallCloud1.Position
            };
            _smallCloud5 = new SpriteObj("TitleSmallCloud5_Sprite")
            {
                Position = _smallCloud1.Position
            };

            _largeCloud1 = new SpriteObj("TitleLargeCloud1_Sprite");
            _largeCloud1.Position = new Vector2(0f, 720 - _largeCloud1.Height);
            _largeCloud2 = new SpriteObj("TitleLargeCloud2_Sprite");
            _largeCloud2.Position = new Vector2(440f, 720 - _largeCloud2.Height + 130);
            _largeCloud3 = new SpriteObj("TitleLargeCloud1_Sprite");
            _largeCloud3.Position = new Vector2(880f, 720 - _largeCloud3.Height + 50);
            _largeCloud3.Flip = SpriteEffects.FlipHorizontally;
            _largeCloud4 = new SpriteObj("TitleLargeCloud2_Sprite");
            _largeCloud4.Position = new Vector2(1320f, 720 - _largeCloud4.Height);
            _largeCloud4.Flip = SpriteEffects.FlipHorizontally;

            // Title
            _titleText = new TextObj
            {
                Font = Game.JunicodeFont,
                FontSize = 45f,
                Text = "ROGUE CASTLE",
                Position = new Vector2(660f, 60f),
                Align = Types.TextAlign.Centre
            };

            // Copyright
            _copyrightText = new TextObj(Game.JunicodeFont)
            {
                FontSize = 8f,
                Text = "Copyright(C) 2011-2015, Cellar Door Games Inc. Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.",
                Align = Types.TextAlign.Centre,
                DropShadow = new Vector2(1f, 2f)
            };
            _copyrightText.Position = new Vector2(660f, 720 - _copyrightText.Height - 10);

            // Version Number
            _versionNumber = (TextObj) _copyrightText.Clone();
            _versionNumber.Align = Types.TextAlign.Left;
            _versionNumber.FontSize = 8f;
            _versionNumber.Position = new Vector2(14f, 5f);
            _versionNumber.Text = $"https://github.com/thephar/RogueLegacyRandomizer\nRogue Legacy Randomizer {LevelENV.FullVersion}";

            // Press Start Text
            _pressStartText = new KeyIconTextObj(Game.JunicodeFont)
            {
                FontSize = 20f,
                Text = "Press Enter to begin",
                Align = Types.TextAlign.Centre,
                Position = new Vector2(660f, 560f),
                DropShadow = new Vector2(2f, 2f)
            };
            _pressStartText2 = new TextObj(Game.JunicodeFont)
            {
                FontSize = 20f,
                Text = "Press Enter to begin",
                Align = Types.TextAlign.Centre,
                Position = _pressStartText.Position,
                DropShadow = new Vector2(2f, 2f)
            };
            _pressStartText2.Y -= _pressStartText.Height - 5;

            // Profile Card Icon - HIDDEN
            _profileCard = new SpriteObj("TitleProfileCard_Sprite")
            {
                OutlineWidth = 2,
                Scale = new Vector2(2f, 2f)
            };
            _profileCard.Position = new Vector2(_profileCard.Width, 720 - _profileCard.Height);

            // Options Icon
            _optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite")
            {
                Scale = new Vector2(2f, 2f),
                OutlineWidth = _profileCard.OutlineWidth,
                ForceDraw = true
            };
            _optionsIcon.Position = new Vector2(1320 - _optionsIcon.Width * 2, _profileCard.Y);

            // Credits Icon
            _creditsIcon = new SpriteObj("TitleCreditsIcon_Sprite")
            {
                Scale = new Vector2(2f, 2f),
                OutlineWidth = _profileCard.OutlineWidth,
                Position = new Vector2(_optionsIcon.X + 120f, _profileCard.Y),
                ForceDraw = true
            };

            // Profile Card Key
            _profileCardKey = new KeyIconTextObj(Game.JunicodeFont)
            {
                Align = Types.TextAlign.Centre,
                FontSize = 12f,
                Text = InputType.MenuProfileCard.Input()
            };
            _profileCardKey.Position = new Vector2(_profileCard.X, _profileCard.Bounds.Top - _profileCardKey.Height - 10);

            // Options Key
            _optionsKey = new KeyIconTextObj(Game.JunicodeFont)
            {
                Align = Types.TextAlign.Centre,
                FontSize = 12f,
                Text = InputType.MenuOptions.Input(),
                ForceDraw = true
            };
            _optionsKey.Position = new Vector2(_optionsIcon.X, _optionsIcon.Bounds.Top - _optionsKey.Height - 10);

            // Credits Key
            _creditsKey = new KeyIconTextObj(Game.JunicodeFont)
            {
                Align = Types.TextAlign.Centre,
                FontSize = 12f,
                Text = InputType.MenuCredits.Input(),
                ForceDraw = true
            };
            _creditsKey.Position = new Vector2(_creditsIcon.X, _creditsIcon.Bounds.Top - _creditsKey.Height - 10);

            // DLC?
            _dlcIcon = new SpriteObj("MedallionPiece5_Sprite")
            {
                Position = new Vector2(950f, 310f),
                ForceDraw = true,
                TextureColor = Color.Yellow
            };

            base.LoadContent();
        }

        public override void OnEnter()
        {
            Camera.Zoom = 1f;
            SoundManager.PlayMusic("TitleScreenSong", true, 1f);
            Game.ScreenManager.Player.ForceInvincible = false;
            _optionsEntered = false;
            _bg.TextureColor = Color.Red;
            _randomSeagullSfx = CDGMath.RandomInt(1, 5);
            _startPressed = false;
            Tween.By(_godRay, 5f, Quad.EaseInOut, "Y", "-0.23");
            _logo.Opacity = 0f;
            _logo.Position = new Vector2(660f, 310f);
            Tween.To(_logo, 2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(_logo, 3f, Quad.EaseInOut, "Y", "300");
            _dlcIcon.Opacity = 0f;
            _dlcIcon.Visible = Game.PlayerStats.ChallengeLastBossBeaten;
            _dlcIcon.Position = new Vector2(898f, 267f);
            Tween.To(_dlcIcon, 2f, Linear.EaseNone, "Opacity", "1");
            Tween.By(_dlcIcon, 3f, Quad.EaseInOut, "Y", "50");
            Camera.Position = new Vector2(660f, 360f);
            _pressStartText.Text = "[Input:" + (int) InputType.MenuConfirm1 + "]";

            // Load the default profile.
            Console.WriteLine("Loading default profile...");
            Program.Game.ArchipelagoManager.Disconnect();
            Program.Game.ChangeProfile("DEFAULT", 0);
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();
            Game.ScreenManager.Player.Reset();
            Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;

            InitializeStartingText();

            base.OnEnter();
        }

        public override void OnExit()
        {
            if (_seagullCue is { IsPlaying: true })
            {
                _seagullCue.Stop(AudioStopOptions.Immediate);
                _seagullCue.Dispose();
            }

            base.OnExit();
        }

        public void InitializeStartingText()
        {
            _pressStartText2.Text = "Start Your Randomized Legacy";
        }

        public override void Update(GameTime gameTime)
        {
            if (_randomSeagullSfx > 0f)
            {
                _randomSeagullSfx -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (_randomSeagullSfx <= 0f)
                {
                    if (_seagullCue is { IsPlaying: true })
                    {
                        _seagullCue.Stop(AudioStopOptions.Immediate);
                        _seagullCue.Dispose();
                    }

                    _seagullCue = SoundManager.PlaySound("Wind1");
                    _randomSeagullSfx = CDGMath.RandomInt(10, 15);
                }
            }

            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _smallCloud1.Rotation += 1.8f * num;
            _smallCloud2.Rotation += 1.2f * num;
            _smallCloud3.Rotation += 3f * num;
            _smallCloud4.Rotation -= 0.6f * num;
            _smallCloud5.Rotation -= 1.8f * num;

            _largeCloud2.X += 2.4f * num;
            if (_largeCloud2.Bounds.Left > 1320)
            {
                _largeCloud2.X = -(_largeCloud2.Width / 2f);
            }

            _largeCloud3.X -= 3f * num;
            if (_largeCloud3.Bounds.Right < 0)
            {
                _largeCloud3.X = 1320 + _largeCloud3.Width / 2;
            }

            if (!_startPressed)
            {
                _pressStartText.Opacity = (float) Math.Abs(Math.Sin(Game.TotalGameTimeSeconds * 1f));
            }

            _godRay.LightSourceSize = 1f + (float) Math.Abs(Math.Sin(Game.TotalGameTimeSeconds * 0.5f)) * 0.5f;
            if (_optionsEntered && Game.ScreenManager.CurrentScreen == this)
            {
                _optionsEntered = false;
                _optionsKey.Text = InputType.MenuOptions.Input();
                _creditsKey.Text = InputType.MenuCredits.Input();
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (InputTypeHelper.PressedConfirm)
            {
                var list = new List<object> { this };
                Game.ScreenManager.DisplayScreen((int) ScreenType.Archipelago, false, list);
            }

            if (Game.GlobalInput.JustPressed((int) InputType.MenuOptions))
            {
                _optionsEntered = true;
                var list = new List<object> { true };
                Game.ScreenManager.DisplayScreen((int) ScreenType.Options, false, list);
            }

            if (Game.GlobalInput.JustPressed((int) InputType.MenuCredits))
            {
                Game.ScreenManager.DisplayScreen((int) ScreenType.Credits, false);
            }

            base.HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.GraphicsDevice.SetRenderTarget(_godRayTexture);
            Camera.GraphicsDevice.Clear(Color.White);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null);
            _smallCloud1.DrawOutline(Camera);
            _smallCloud3.DrawOutline(Camera);
            _smallCloud4.DrawOutline(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _castle.DrawOutline(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _smallCloud2.DrawOutline(Camera);
            _smallCloud5.DrawOutline(Camera);
            _logo.DrawOutline(Camera);
            _dlcIcon.DrawOutline(Camera);
            Camera.End();
            _ppm.Draw(gameTime, _godRayTexture);
            Camera.GraphicsDevice.SetRenderTarget(_godRayTexture);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            _bg.Draw(Camera);
            _smallCloud1.Draw(Camera);
            _smallCloud3.Draw(Camera);
            _smallCloud4.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _castle.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _smallCloud2.Draw(Camera);
            _smallCloud5.Draw(Camera);
            _largeCloud1.Draw(Camera);
            _largeCloud2.Draw(Camera);
            _largeCloud3.Draw(Camera);
            _largeCloud4.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            Camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.Black * _hardCoreModeOpacity);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _logo.Draw(Camera);
            _copyrightText.Draw(Camera);
            _versionNumber.Draw(Camera);
            _pressStartText2.Opacity = _pressStartText.Opacity;
            _pressStartText.Draw(Camera);
            _pressStartText2.Draw(Camera);
            _creditsKey.Draw(Camera);
            _optionsKey.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            _dlcIcon.Draw(Camera);
            _optionsIcon.Draw(Camera);
            _creditsIcon.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Camera.Draw(_ppm.Scene, new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height), Color.White);
            Camera.Draw(_godRayTexture, new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height), Color.White);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Title Screen");
            _godRayTexture.Dispose();
            _godRayTexture = null;
            _bg.Dispose();
            _bg = null;
            _logo.Dispose();
            _logo = null;
            _castle.Dispose();
            _castle = null;
            _smallCloud1.Dispose();
            _smallCloud2.Dispose();
            _smallCloud3.Dispose();
            _smallCloud4.Dispose();
            _smallCloud5.Dispose();
            _smallCloud1 = null;
            _smallCloud2 = null;
            _smallCloud3 = null;
            _smallCloud4 = null;
            _smallCloud5 = null;
            _largeCloud1.Dispose();
            _largeCloud1 = null;
            _largeCloud2.Dispose();
            _largeCloud2 = null;
            _largeCloud3.Dispose();
            _largeCloud3 = null;
            _largeCloud4.Dispose();
            _largeCloud4 = null;
            _pressStartText.Dispose();
            _pressStartText = null;
            _pressStartText2.Dispose();
            _pressStartText2 = null;
            _copyrightText.Dispose();
            _copyrightText = null;
            _versionNumber.Dispose();
            _versionNumber = null;
            _titleText.Dispose();
            _titleText = null;
            _profileCard.Dispose();
            _profileCard = null;
            _optionsIcon.Dispose();
            _optionsIcon = null;
            _creditsIcon.Dispose();
            _creditsIcon = null;
            _profileCardKey.Dispose();
            _profileCardKey = null;
            _optionsKey.Dispose();
            _optionsKey = null;
            _creditsKey.Dispose();
            _creditsKey = null;
            _dlcIcon.Dispose();
            _dlcIcon = null;
            _seagullCue = null;

            base.Dispose();
        }
    }
}
