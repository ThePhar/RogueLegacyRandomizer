using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
namespace Randomchaos2DGodRays
{
	public class PostProcessingManager
	{
		protected Game Game;
		public Texture2D Scene;
		public RenderTarget2D newScene;
		protected List<BasePostProcessingEffect> postProcessingEffects = new List<BasePostProcessingEffect>();
		public Vector2 HalfPixel;
		private SpriteBatch m_spriteBatch;
		public SpriteBatch spriteBatch
		{
			get
			{
				return this.m_spriteBatch;
			}
		}
		public PostProcessingManager(Game game, SpriteBatch spriteBatch)
		{
			this.Game = game;
			this.m_spriteBatch = spriteBatch;
		}
		public void AddEffect(BasePostProcessingEffect ppEfect)
		{
			this.postProcessingEffects.Add(ppEfect);
		}
		public virtual void Draw(GameTime gameTime, Texture2D scene)
		{
			this.HalfPixel = -new Vector2(0.5f / (float)this.Game.GraphicsDevice.Viewport.Width, 0.5f / (float)this.Game.GraphicsDevice.Viewport.Height);
			int count = this.postProcessingEffects.Count;
			this.Scene = scene;
			for (int i = 0; i < count; i++)
			{
				if (this.postProcessingEffects[i].Enabled)
				{
					this.postProcessingEffects[i].HalfPixel = this.HalfPixel;
					this.postProcessingEffects[i].orgScene = scene;
					this.postProcessingEffects[i].Draw(gameTime, this.Scene);
					this.Scene = this.postProcessingEffects[i].lastScene;
				}
			}
			this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
			this.spriteBatch.Draw(this.Scene, new Rectangle(0, 0, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height), Color.White);
			this.spriteBatch.End();
		}
		protected void SaveTexture(Texture2D texture, string name)
		{
			FileStream fileStream = new FileStream(name, FileMode.Create);
			texture.SaveAsJpeg(fileStream, texture.Width, texture.Height);
			fileStream.Close();
		}
	}
}
