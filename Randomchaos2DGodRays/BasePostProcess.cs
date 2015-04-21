using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Randomchaos2DGodRays
{
	public class BasePostProcess
	{
		public Vector2 HalfPixel;
		public Texture2D BackBuffer;
		public Texture2D orgBuffer;
		public bool Enabled = true;
		protected Effect effect;
		protected Game Game;
		public RenderTarget2D newScene;
		private ScreenQuad sq;
		public bool UsesVertexShader;
		protected SpriteBatch spriteBatch
		{
			get
			{
				return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
			}
		}
		public BasePostProcess(Game game)
		{
			this.Game = game;
		}
		public virtual void Draw(GameTime gameTime)
		{
			if (this.Enabled)
			{
				if (this.sq == null)
				{
					this.sq = new ScreenQuad(this.Game);
					this.sq.Initialize();
				}
				this.effect.CurrentTechnique.Passes[0].Apply();
				this.sq.Draw();
			}
		}
	}
}
