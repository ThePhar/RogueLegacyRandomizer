using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
namespace RogueCastle
{
	public class DemoStartScreen : Screen
	{
		private TextObj m_text;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public override void LoadContent()
		{
			this.m_text = new TextObj(Game.JunicodeLargeFont);
			this.m_text.FontSize = 20f;
			this.m_text.Text = "This is a demo of Rogue Legacy.\nThere may be bugs, and some assets are missing, but we hope you enjoy it.";
			this.m_text.ForceDraw = true;
			this.m_text.Position = new Vector2(660f - (float)this.m_text.Width / 2f, 360f - (float)this.m_text.Height / 2f - 30f);
			base.LoadContent();
		}
		public override void OnEnter()
		{
			this.BackBufferOpacity = 1f;
			Tween.RunFunction(7f, base.ScreenManager, "DisplayScreen", new object[]
			{
				1,
				true,
				typeof(List<object>)
			});
			base.OnEnter();
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_text.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_text.Dispose();
				this.m_text = null;
				base.Dispose();
			}
		}
	}
}
