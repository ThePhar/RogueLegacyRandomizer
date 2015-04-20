/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;

namespace RogueCastle
{
	public class TextScreen : Screen
	{
		private TextObj m_text;
		private float m_fadeInSpeed;
		private float m_backBufferOpacity;
		private bool m_typewriteText;
		private float m_textDuration;
		private bool m_loadEndingAfterward;
		private SpriteObj m_smoke1;
		private SpriteObj m_smoke2;
		private SpriteObj m_smoke3;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public override void LoadContent()
		{
			Color textureColor = new Color(200, 150, 55);
			m_smoke1 = new SpriteObj("TextSmoke_Sprite");
			m_smoke1.ForceDraw = true;
			m_smoke1.Scale = new Vector2(2f, 2f);
			m_smoke1.Opacity = 0.3f;
			m_smoke1.TextureColor = textureColor;
			m_smoke2 = (m_smoke1.Clone() as SpriteObj);
			m_smoke2.Flip = SpriteEffects.FlipHorizontally;
			m_smoke2.Opacity = 0.2f;
			m_smoke3 = (m_smoke1.Clone() as SpriteObj);
			m_smoke3.Scale = new Vector2(2.5f, 3f);
			m_smoke3.Opacity = 0.15f;
			base.LoadContent();
		}
		public override void PassInData(List<object> objList)
		{
			m_backBufferOpacity = (float)objList[0];
			m_fadeInSpeed = (float)objList[1];
			m_textDuration = (float)objList[2];
			m_typewriteText = (bool)objList[3];
			TextObj textObj = objList[4] as TextObj;
			if (m_text != null)
			{
				m_text.Dispose();
				m_text = null;
			}
			m_text = (textObj.Clone() as TextObj);
			m_loadEndingAfterward = (bool)objList[5];
		}
		public override void OnEnter()
		{
			m_smoke1.Position = new Vector2(CDGMath.RandomInt(300, 1000), m_text.Y + m_text.Height / 2f - 30f + CDGMath.RandomInt(-100, 100));
			m_smoke2.Position = new Vector2(CDGMath.RandomInt(200, 700), m_text.Y + m_text.Height / 2f - 30f + CDGMath.RandomInt(-50, 50));
			m_smoke3.Position = new Vector2(CDGMath.RandomInt(300, 800), m_text.Y + m_text.Height / 2f - 30f + CDGMath.RandomInt(-100, 100));
			m_smoke1.Opacity = (m_smoke2.Opacity = (m_smoke3.Opacity = 0f));
			Tween.To(m_smoke1, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0.3"
			});
			Tween.To(m_smoke2, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0.2"
			});
			Tween.To(m_smoke3, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0.15"
			});
			BackBufferOpacity = 0f;
			m_text.Opacity = 0f;
			Tween.To(this, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				m_backBufferOpacity.ToString()
			});
			Tween.To(m_text, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			if (m_typewriteText)
			{
				m_text.Visible = false;
				Tween.RunFunction(m_fadeInSpeed, m_text, "BeginTypeWriting", new object[]
				{
					m_text.Text.Length * 0.05f,
					""
				});
			}
			else
			{
				m_text.Visible = true;
			}
			base.OnEnter();
		}
		private void ExitTransition()
		{
			if (!m_loadEndingAfterward)
			{
				Tween.To(m_smoke1, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.To(m_smoke2, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.To(m_smoke3, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.To(this, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"BackBufferOpacity",
					"0"
				});
				Tween.To(m_text, m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.AddEndHandlerToLastTween(Game.ScreenManager, "HideCurrentScreen", new object[0]);
				return;
			}
			Game.ScreenManager.DisplayScreen(24, true, null);
		}
		public override void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			m_smoke1.X += 5f * num;
			m_smoke2.X += 15f * num;
			m_smoke3.X += 10f * num;
			if (!m_text.Visible && m_text.IsTypewriting)
			{
				m_text.Visible = true;
			}
			if (m_textDuration > 0f)
			{
				m_textDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (m_textDuration <= 0f)
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
			m_smoke1.Draw(Camera);
			m_smoke2.Draw(Camera);
			m_smoke3.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_text.Draw(Camera);
			Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Text Screen");
				m_text.Dispose();
				m_text = null;
				m_smoke1.Dispose();
				m_smoke1 = null;
				m_smoke2.Dispose();
				m_smoke2 = null;
				m_smoke3.Dispose();
				m_smoke3 = null;
				base.Dispose();
			}
		}
	}
}
