// 
// RogueLegacyArchipelago - TitleScreen.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Randomchaos2DGodRays;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class TitleScreen : Screen
    {
        private SpriteObj m_bg;
        private SpriteObj m_castle;
        private TextObj m_copyrightText;
        private SpriteObj m_creditsIcon;
        private KeyIconTextObj m_creditsKey;
        private SpriteObj m_dlcIcon;
        private CrepuscularRays m_godRay;
        private RenderTarget2D m_godRayTexture;
        private float m_hardCoreModeOpacity;
        private SpriteObj m_largeCloud1;
        private SpriteObj m_largeCloud2;
        private SpriteObj m_largeCloud3;
        private SpriteObj m_largeCloud4;
        private SpriteObj m_logo;
        private bool m_optionsEntered;
        private SpriteObj m_optionsIcon;
        private KeyIconTextObj m_optionsKey;
        private PostProcessingManager m_ppm;
        private KeyIconTextObj m_pressStartText;
        private TextObj m_pressStartText2;
        private SpriteObj m_profileCard;
        private KeyIconTextObj m_profileCardKey;
        private float m_randomSeagullSfx;
        private Cue m_seagullCue;
        private SpriteObj m_smallCloud1;
        private SpriteObj m_smallCloud2;
        private SpriteObj m_smallCloud3;
        private SpriteObj m_smallCloud4;
        private SpriteObj m_smallCloud5;
        private bool m_startPressed;
        private TextObj m_titleText;
        private TextObj m_versionNumber;

        public override void LoadContent()
        {
            // Post-Processing + Fancy God-Rays
            m_ppm = new PostProcessingManager(ScreenManager.Game, ScreenManager.Camera);
            m_godRay = new CrepuscularRays(ScreenManager.Game, Vector2.One * 0.5f, "GameSpritesheets/flare3", 2f, 0.97f,
                0.97f, 0.5f, 1.25f);
            m_ppm.AddEffect(m_godRay);
            m_godRay.lightSource = new Vector2(0.495f, 0.3f);
            m_godRayTexture = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color,
                DepthFormat.None);

            // Background
            m_bg = new SpriteObj("TitleBG_Sprite");
            m_bg.Scale = new Vector2(1320f / m_bg.Width, 720f / m_bg.Height);
            m_bg.TextureColor = Color.Red;

            // Hardcore Mode?
            m_hardCoreModeOpacity = 0f;

            // Logo
            m_logo = new SpriteObj("TitleLogo_Sprite")
            {
                Position = new Vector2(660f, 360f),
                DropShadow = new Vector2(0f, 5f)
            };

            // Castle
            m_castle = new SpriteObj("TitleCastle_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            };
            m_castle.Position = new Vector2(630f, 720 - m_castle.Height / 2);

            // Clouds
            m_smallCloud1 = new SpriteObj("TitleSmallCloud1_Sprite")
            {
                Position = new Vector2(660f, 0f)
            };
            m_smallCloud2 = new SpriteObj("TitleSmallCloud2_Sprite")
            {
                Position = m_smallCloud1.Position
            };
            m_smallCloud3 = new SpriteObj("TitleSmallCloud3_Sprite")
            {
                Position = m_smallCloud1.Position
            };
            m_smallCloud4 = new SpriteObj("TitleSmallCloud4_Sprite")
            {
                Position = m_smallCloud1.Position
            };
            m_smallCloud5 = new SpriteObj("TitleSmallCloud5_Sprite")
            {
                Position = m_smallCloud1.Position
            };

            m_largeCloud1 = new SpriteObj("TitleLargeCloud1_Sprite");
            m_largeCloud1.Position = new Vector2(0f, 720 - m_largeCloud1.Height);
            m_largeCloud2 = new SpriteObj("TitleLargeCloud2_Sprite");
            m_largeCloud2.Position = new Vector2(440f, 720 - m_largeCloud2.Height + 130);
            m_largeCloud3 = new SpriteObj("TitleLargeCloud1_Sprite");
            m_largeCloud3.Position = new Vector2(880f, 720 - m_largeCloud3.Height + 50);
            m_largeCloud3.Flip = SpriteEffects.FlipHorizontally;
            m_largeCloud4 = new SpriteObj("TitleLargeCloud2_Sprite");
            m_largeCloud4.Position = new Vector2(1320f, 720 - m_largeCloud4.Height);
            m_largeCloud4.Flip = SpriteEffects.FlipHorizontally;

            // Title
            m_titleText = new TextObj
            {
                Font = Game.JunicodeFont,
                FontSize = 45f,
                Text = "ROGUE CASTLE",
                Position = new Vector2(660f, 60f),
                Align = Types.TextAlign.Centre
            };

            // Copyright
            m_copyrightText = new TextObj(Game.JunicodeFont)
            {
                FontSize = 8f,
                Text =
                    "Rogue Legacy for Archipelago Client\nCopyright(C) 2011-2015, Cellar Door Games Inc. Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.",
                Align = Types.TextAlign.Centre,
                DropShadow = new Vector2(1f, 2f)
            };
            m_copyrightText.Position = new Vector2(660f, 720 - m_copyrightText.Height - 10);

            // Version Number
            m_versionNumber = m_copyrightText.Clone() as TextObj;
            m_versionNumber.Align = Types.TextAlign.Left;
            m_versionNumber.FontSize = 8f;
            m_versionNumber.Position = new Vector2(14f, 5f);
            m_versionNumber.Text = string.Format("Randomizer v{0}", LevelENV.GameVersion);

            // Press Start Text
            m_pressStartText = new KeyIconTextObj(Game.JunicodeFont)
            {
                FontSize = 20f,
                Text = "Press Enter to begin",
                Align = Types.TextAlign.Centre,
                Position = new Vector2(660f, 560f),
                DropShadow = new Vector2(2f, 2f)
            };
            m_pressStartText2 = new TextObj(Game.JunicodeFont)
            {
                FontSize = 20f,
                Text = "Press Enter to begin",
                Align = Types.TextAlign.Centre,
                Position = m_pressStartText.Position,
                DropShadow = new Vector2(2f, 2f)
            };
            m_pressStartText2.Y -= m_pressStartText.Height - 5;

            // Profile Card Icon - HIDDEN
            m_profileCard = new SpriteObj("TitleProfileCard_Sprite")
            {
                OutlineWidth = 2,
                Scale = new Vector2(2f, 2f)
            };
            m_profileCard.Position = new Vector2(m_profileCard.Width, 720 - m_profileCard.Height);

            // Options Icon
            m_optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite")
            {
                Scale = new Vector2(2f, 2f),
                OutlineWidth = m_profileCard.OutlineWidth,
                ForceDraw = true
            };
            m_optionsIcon.Position = new Vector2(1320 - m_optionsIcon.Width * 2, m_profileCard.Y);

            // Credits Icon
            m_creditsIcon = new SpriteObj("TitleCreditsIcon_Sprite")
            {
                Scale = new Vector2(2f, 2f),
                OutlineWidth = m_profileCard.OutlineWidth,
                Position = new Vector2(m_optionsIcon.X + 120f, m_profileCard.Y),
                ForceDraw = true
            };

            // Profile Card Key
            m_profileCardKey = new KeyIconTextObj(Game.JunicodeFont)
            {
                Align = Types.TextAlign.Centre,
                FontSize = 12f,
                Text = "[Input:" + InputType.MenuProfileCard + "]"
            };
            m_profileCardKey.Position =
                new Vector2(m_profileCard.X, m_profileCard.Bounds.Top - m_profileCardKey.Height - 10);

            // Options Key
            m_optionsKey = new KeyIconTextObj(Game.JunicodeFont)
            {
                Align = Types.TextAlign.Centre,
                FontSize = 12f,
                Text = "[Input:" + InputType.MenuOptions + "]",
                ForceDraw = true
            };
            m_optionsKey.Position = new Vector2(m_optionsIcon.X, m_optionsIcon.Bounds.Top - m_optionsKey.Height - 10);

            // Credits Key
            m_creditsKey = new KeyIconTextObj(Game.JunicodeFont)
            {
                Align = Types.TextAlign.Centre,
                FontSize = 12f,
                Text = "[Input:" + InputType.MenuCredits + "]",
                ForceDraw = true
            };
            m_creditsKey.Position = new Vector2(m_creditsIcon.X, m_creditsIcon.Bounds.Top - m_creditsKey.Height - 10);

            // DLC?
            m_dlcIcon = new SpriteObj("MedallionPiece5_Sprite")
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
            m_optionsEntered = false;
            m_bg.TextureColor = Color.Red;
            m_randomSeagullSfx = CDGMath.RandomInt(1, 5);
            m_startPressed = false;
            Tween.By(m_godRay, 5f, Quad.EaseInOut, "Y", "-0.23");
            m_logo.Opacity = 0f;
            m_logo.Position = new Vector2(660f, 310f);
            Tween.To(m_logo, 2f, Linear.EaseNone, "Opacity", "1");
            Tween.To(m_logo, 3f, Quad.EaseInOut, "Y", "300");
            m_dlcIcon.Opacity = 0f;
            m_dlcIcon.Visible = Game.PlayerStats.ChallengeLastBossBeaten;
            m_dlcIcon.Position = new Vector2(898f, 267f);
            Tween.To(m_dlcIcon, 2f, Linear.EaseNone, "Opacity", "1");
            Tween.By(m_dlcIcon, 3f, Quad.EaseInOut, "Y", "50");
            Camera.Position = new Vector2(660f, 360f);
            m_pressStartText.Text = "[Input:" + InputType.MenuConfirm1 + "]";

            // Reload the defaults.
            Game.ProfileName = "DEFAULT";
            Program.Game.ArchipelagoManager.Disconnect();
            Program.Game.SaveManager.LoadAllFileTypes(null);

            InitializeStartingText();

            base.OnEnter();
        }

        public override void OnExit()
        {
            if (m_seagullCue != null && m_seagullCue.IsPlaying)
            {
                m_seagullCue.Stop(AudioStopOptions.Immediate);
                m_seagullCue.Dispose();
            }

            base.OnExit();
        }

        public void InitializeStartingText()
        {
            m_pressStartText2.Text = "Start Your Randomized Legacy";
        }

        public override void Update(GameTime gameTime)
        {
            if (m_randomSeagullSfx > 0f)
            {
                m_randomSeagullSfx -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_randomSeagullSfx <= 0f)
                {
                    if (m_seagullCue != null && m_seagullCue.IsPlaying)
                    {
                        m_seagullCue.Stop(AudioStopOptions.Immediate);
                        m_seagullCue.Dispose();
                    }

                    m_seagullCue = SoundManager.PlaySound("Wind1");
                    m_randomSeagullSfx = CDGMath.RandomInt(10, 15);
                }
            }

            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            m_smallCloud1.Rotation += 1.8f * num;
            m_smallCloud2.Rotation += 1.2f * num;
            m_smallCloud3.Rotation += 3f * num;
            m_smallCloud4.Rotation -= 0.6f * num;
            m_smallCloud5.Rotation -= 1.8f * num;
            m_largeCloud2.X += 2.4f * num;
            if (m_largeCloud2.Bounds.Left > 1320)
            {
                m_largeCloud2.X = -(m_largeCloud2.Width / 2f);
            }

            m_largeCloud3.X -= 3f * num;
            if (m_largeCloud3.Bounds.Right < 0)
            {
                m_largeCloud3.X = 1320 + m_largeCloud3.Width / 2;
            }

            if (!m_startPressed)
            {
                m_pressStartText.Opacity = (float) Math.Abs(Math.Sin(Game.TotalGameTimeSeconds * 1f));
            }

            m_godRay.LightSourceSize = 1f + (float) Math.Abs(Math.Sin(Game.TotalGameTimeSeconds * 0.5f)) * 0.5f;
            if (m_optionsEntered && Game.ScreenManager.CurrentScreen == this)
            {
                m_optionsEntered = false;
                m_optionsKey.Text = "[Input:" + InputType.MenuOptions + "]";
                m_creditsKey.Text = "[Input:" + InputType.MenuCredits + "]";
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (Game.GlobalInput.JustPressed(InputType.MenuConfirm1) ||
                Game.GlobalInput.JustPressed(InputType.MenuConfirm2))
            {
                var list = new List<object> { this };
                (ScreenManager as RCScreenManager).DisplayScreen(80, false, list);
            }

            if (Game.GlobalInput.JustPressed(InputType.MenuOptions))
            {
                m_optionsEntered = true;
                var list = new List<object> { true };
                (ScreenManager as RCScreenManager).DisplayScreen(4, false, list);
            }

            if (Game.GlobalInput.JustPressed(InputType.MenuCredits))
            {
                (ScreenManager as RCScreenManager).DisplayScreen(18, false);
            }

            base.HandleInput();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.GraphicsDevice.SetRenderTarget(m_godRayTexture);
            Camera.GraphicsDevice.Clear(Color.White);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null);
            m_smallCloud1.DrawOutline(Camera);
            m_smallCloud3.DrawOutline(Camera);
            m_smallCloud4.DrawOutline(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            m_castle.DrawOutline(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_smallCloud2.DrawOutline(Camera);
            m_smallCloud5.DrawOutline(Camera);
            m_logo.DrawOutline(Camera);
            m_dlcIcon.DrawOutline(Camera);
            Camera.End();
            m_ppm.Draw(gameTime, m_godRayTexture);
            Camera.GraphicsDevice.SetRenderTarget(m_godRayTexture);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            m_bg.Draw(Camera);
            m_smallCloud1.Draw(Camera);
            m_smallCloud3.Draw(Camera);
            m_smallCloud4.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            m_castle.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_smallCloud2.Draw(Camera);
            m_smallCloud5.Draw(Camera);
            m_largeCloud1.Draw(Camera);
            m_largeCloud2.Draw(Camera);
            m_largeCloud3.Draw(Camera);
            m_largeCloud4.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            Camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.Black * m_hardCoreModeOpacity);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_logo.Draw(Camera);
            m_copyrightText.Draw(Camera);
            m_versionNumber.Draw(Camera);
            m_pressStartText2.Opacity = m_pressStartText.Opacity;
            m_pressStartText.Draw(Camera);
            m_pressStartText2.Draw(Camera);
            m_creditsKey.Draw(Camera);
            m_optionsKey.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            m_dlcIcon.Draw(Camera);
            m_optionsIcon.Draw(Camera);
            m_creditsIcon.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Camera.Draw(m_ppm.Scene,
                new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height),
                Color.White);
            Camera.Draw(m_godRayTexture,
                new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height),
                Color.White);
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
            m_godRayTexture.Dispose();
            m_godRayTexture = null;
            m_bg.Dispose();
            m_bg = null;
            m_logo.Dispose();
            m_logo = null;
            m_castle.Dispose();
            m_castle = null;
            m_smallCloud1.Dispose();
            m_smallCloud2.Dispose();
            m_smallCloud3.Dispose();
            m_smallCloud4.Dispose();
            m_smallCloud5.Dispose();
            m_smallCloud1 = null;
            m_smallCloud2 = null;
            m_smallCloud3 = null;
            m_smallCloud4 = null;
            m_smallCloud5 = null;
            m_largeCloud1.Dispose();
            m_largeCloud1 = null;
            m_largeCloud2.Dispose();
            m_largeCloud2 = null;
            m_largeCloud3.Dispose();
            m_largeCloud3 = null;
            m_largeCloud4.Dispose();
            m_largeCloud4 = null;
            m_pressStartText.Dispose();
            m_pressStartText = null;
            m_pressStartText2.Dispose();
            m_pressStartText2 = null;
            m_copyrightText.Dispose();
            m_copyrightText = null;
            m_versionNumber.Dispose();
            m_versionNumber = null;
            m_titleText.Dispose();
            m_titleText = null;
            m_profileCard.Dispose();
            m_profileCard = null;
            m_optionsIcon.Dispose();
            m_optionsIcon = null;
            m_creditsIcon.Dispose();
            m_creditsIcon = null;
            m_profileCardKey.Dispose();
            m_profileCardKey = null;
            m_optionsKey.Dispose();
            m_optionsKey = null;
            m_creditsKey.Dispose();
            m_creditsKey = null;
            m_dlcIcon.Dispose();
            m_dlcIcon = null;
            m_seagullCue = null;

            base.Dispose();
        }
    }
}