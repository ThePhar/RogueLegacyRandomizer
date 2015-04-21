using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
			this.m_smoke1 = new SpriteObj("TextSmoke_Sprite");
			this.m_smoke1.ForceDraw = true;
			this.m_smoke1.Scale = new Vector2(2f, 2f);
			this.m_smoke1.Opacity = 0.3f;
			this.m_smoke1.TextureColor = textureColor;
			this.m_smoke2 = (this.m_smoke1.Clone() as SpriteObj);
			this.m_smoke2.Flip = SpriteEffects.FlipHorizontally;
			this.m_smoke2.Opacity = 0.2f;
			this.m_smoke3 = (this.m_smoke1.Clone() as SpriteObj);
			this.m_smoke3.Scale = new Vector2(2.5f, 3f);
			this.m_smoke3.Opacity = 0.15f;
			base.LoadContent();
		}
		public override void PassInData(List<object> objList)
		{
			this.m_backBufferOpacity = (float)objList[0];
			this.m_fadeInSpeed = (float)objList[1];
			this.m_textDuration = (float)objList[2];
			this.m_typewriteText = (bool)objList[3];
			TextObj textObj = objList[4] as TextObj;
			if (this.m_text != null)
			{
				this.m_text.Dispose();
				this.m_text = null;
			}
			this.m_text = (textObj.Clone() as TextObj);
			this.m_loadEndingAfterward = (bool)objList[5];
		}
		public override void OnEnter()
		{
			this.m_smoke1.Position = new Vector2((float)CDGMath.RandomInt(300, 1000), this.m_text.Y + (float)this.m_text.Height / 2f - 30f + (float)CDGMath.RandomInt(-100, 100));
			this.m_smoke2.Position = new Vector2((float)CDGMath.RandomInt(200, 700), this.m_text.Y + (float)this.m_text.Height / 2f - 30f + (float)CDGMath.RandomInt(-50, 50));
			this.m_smoke3.Position = new Vector2((float)CDGMath.RandomInt(300, 800), this.m_text.Y + (float)this.m_text.Height / 2f - 30f + (float)CDGMath.RandomInt(-100, 100));
			this.m_smoke1.Opacity = (this.m_smoke2.Opacity = (this.m_smoke3.Opacity = 0f));
			Tween.To(this.m_smoke1, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0.3"
			});
			Tween.To(this.m_smoke2, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0.2"
			});
			Tween.To(this.m_smoke3, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0.15"
			});
			this.BackBufferOpacity = 0f;
			this.m_text.Opacity = 0f;
			Tween.To(this, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				this.m_backBufferOpacity.ToString()
			});
			Tween.To(this.m_text, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			if (this.m_typewriteText)
			{
				this.m_text.Visible = false;
				Tween.RunFunction(this.m_fadeInSpeed, this.m_text, "BeginTypeWriting", new object[]
				{
					(float)this.m_text.Text.Length * 0.05f,
					""
				});
			}
			else
			{
				this.m_text.Visible = true;
			}
			base.OnEnter();
		}
		private void ExitTransition()
		{
			if (!this.m_loadEndingAfterward)
			{
				Tween.To(this.m_smoke1, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.To(this.m_smoke2, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.To(this.m_smoke3, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
				Tween.To(this, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
				{
					"BackBufferOpacity",
					"0"
				});
				Tween.To(this.m_text, this.m_fadeInSpeed, new Easing(Tween.EaseNone), new string[]
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
			this.m_smoke1.X += 5f * num;
			this.m_smoke2.X += 15f * num;
			this.m_smoke3.X += 10f * num;
			if (!this.m_text.Visible && this.m_text.IsTypewriting)
			{
				this.m_text.Visible = true;
			}
			if (this.m_textDuration > 0f)
			{
				this.m_textDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_textDuration <= 0f)
				{
					this.ExitTransition();
				}
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_smoke1.Draw(base.Camera);
			this.m_smoke2.Draw(base.Camera);
			this.m_smoke3.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_text.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Text Screen");
				this.m_text.Dispose();
				this.m_text = null;
				this.m_smoke1.Dispose();
				this.m_smoke1 = null;
				this.m_smoke2.Dispose();
				this.m_smoke2 = null;
				this.m_smoke3.Dispose();
				this.m_smoke3 = null;
				base.Dispose();
			}
		}
	}
}
