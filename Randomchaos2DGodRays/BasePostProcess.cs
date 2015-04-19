using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
				return (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
			}
		}
		public BasePostProcess(Game game)
		{
			Game = game;
		}
		public virtual void Draw(GameTime gameTime)
		{
			if (Enabled)
			{
				if (sq == null)
				{
					sq = new ScreenQuad(Game);
					sq.Initialize();
				}
				effect.CurrentTechnique.Passes[0].Apply();
				sq.Draw();
			}
		}
	}
}
