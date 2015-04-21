using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class FrameRateCounter : DrawableGameComponent
	{
		private ContentManager content;
		private SpriteBatch spriteBatch;
		private SpriteFont spriteFont;
		private int frameRate;
		private int frameCounter;
		private TimeSpan elapsedTime = TimeSpan.Zero;
		public FrameRateCounter(Game game) : base(game)
		{
			this.content = base.Game.Content;
		}
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			this.spriteFont = this.content.Load<SpriteFont>("Fonts\\FpsFont");
		}
		protected override void UnloadContent()
		{
			this.content.Unload();
		}
		public override void Update(GameTime gameTime)
		{
			this.elapsedTime += gameTime.ElapsedGameTime;
			if (this.elapsedTime > TimeSpan.FromSeconds(1.0))
			{
				this.elapsedTime -= TimeSpan.FromSeconds(1.0);
				this.frameRate = this.frameCounter;
				this.frameCounter = 0;
			}
		}
		public override void Draw(GameTime gameTime)
		{
			this.frameCounter++;
			string text = string.Format("fps: {0}", this.frameRate);
			this.spriteBatch.Begin();
			this.spriteBatch.DrawString(this.spriteFont, text, new Vector2(1220f, 33f), Color.Black);
			this.spriteBatch.DrawString(this.spriteFont, text, new Vector2(1221f, 32f), Color.White);
			this.spriteBatch.End();
		}
	}
}
