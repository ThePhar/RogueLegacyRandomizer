/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;

namespace RogueCastle
{
    public class DemoEndScreen : Screen
    {
        private TextObj m_text;
        private SpriteObj m_playerShrug;
        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_text = new TextObj(Game.JunicodeLargeFont);
            m_text.FontSize = 20f;
            m_text.Text = "Thanks for playing the Rogue Legacy Demo. You're pretty good at games.";
            m_text.ForceDraw = true;
            m_text.Position = new Vector2(660f - m_text.Width/2f, 360f - m_text.Height/2f - 30f);
            m_playerShrug = new SpriteObj("PlayerShrug_Sprite");
            m_playerShrug.ForceDraw = true;
            m_playerShrug.Position = new Vector2(660f, m_text.Bounds.Bottom + 100);
            m_playerShrug.Scale = new Vector2(3f, 3f);
            base.LoadContent();
        }

        public override void OnEnter()
        {
            BackBufferOpacity = 1f;
            Tween.RunFunction(8f, ScreenManager, "DisplayScreen", 3, true, typeof (List<object>));
            base.OnEnter();
        }

        public override void Draw(GameTime gametime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black*BackBufferOpacity);
            m_playerShrug.Draw(Camera);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            m_text.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_playerShrug.Dispose();
                m_playerShrug = null;
                m_text.Dispose();
                m_text = null;
                base.Dispose();
            }
        }
    }
}