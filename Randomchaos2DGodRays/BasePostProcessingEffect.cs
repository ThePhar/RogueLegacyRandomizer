using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace Randomchaos2DGodRays
{
	public class BasePostProcessingEffect
	{
		public Vector2 HalfPixel;
		public Texture2D lastScene;
		public Texture2D orgScene;
		protected List<BasePostProcess> postProcesses = new List<BasePostProcess>();
		protected Game Game;
		public bool Enabled = true;
		public BasePostProcessingEffect(Game game)
		{
			this.Game = game;
		}
		public void AddPostProcess(BasePostProcess postProcess)
		{
			this.postProcesses.Add(postProcess);
		}
		public virtual void Draw(GameTime gameTime, Texture2D scene)
		{
			if (!this.Enabled)
			{
				return;
			}
			this.orgScene = scene;
			int count = this.postProcesses.Count;
			this.lastScene = null;
			for (int i = 0; i < count; i++)
			{
				if (this.postProcesses[i].Enabled)
				{
					this.postProcesses[i].HalfPixel = this.HalfPixel;
					this.postProcesses[i].orgBuffer = this.orgScene;
					if (this.postProcesses[i].newScene == null)
					{
						this.postProcesses[i].newScene = new RenderTarget2D(this.Game.GraphicsDevice, this.Game.GraphicsDevice.Viewport.Width / 2, this.Game.GraphicsDevice.Viewport.Height / 2, false, SurfaceFormat.Color, DepthFormat.None);
					}
					this.Game.GraphicsDevice.SetRenderTarget(this.postProcesses[i].newScene);
					if (this.lastScene == null)
					{
						this.lastScene = this.orgScene;
					}
					this.postProcesses[i].BackBuffer = this.lastScene;
					this.Game.GraphicsDevice.Textures[0] = this.postProcesses[i].BackBuffer;
					this.postProcesses[i].Draw(gameTime);
					this.Game.GraphicsDevice.SetRenderTarget(null);
					this.lastScene = this.postProcesses[i].newScene;
				}
			}
			if (this.lastScene == null)
			{
				this.lastScene = scene;
			}
		}
	}
}
