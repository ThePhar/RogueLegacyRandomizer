/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			Game = game;
		}
		public void AddPostProcess(BasePostProcess postProcess)
		{
			postProcesses.Add(postProcess);
		}
		public virtual void Draw(GameTime gameTime, Texture2D scene)
		{
			if (!Enabled)
			{
				return;
			}
			orgScene = scene;
			int count = postProcesses.Count;
			lastScene = null;
			for (int i = 0; i < count; i++)
			{
				if (postProcesses[i].Enabled)
				{
					postProcesses[i].HalfPixel = HalfPixel;
					postProcesses[i].orgBuffer = orgScene;
					if (postProcesses[i].newScene == null)
					{
						postProcesses[i].newScene = new RenderTarget2D(Game.GraphicsDevice, Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2, false, SurfaceFormat.Color, DepthFormat.None);
					}
					Game.GraphicsDevice.SetRenderTarget(postProcesses[i].newScene);
					if (lastScene == null)
					{
						lastScene = orgScene;
					}
					postProcesses[i].BackBuffer = lastScene;
					Game.GraphicsDevice.Textures[0] = postProcesses[i].BackBuffer;
					postProcesses[i].Draw(gameTime);
					Game.GraphicsDevice.SetRenderTarget(null);
					lastScene = postProcesses[i].newScene;
				}
			}
			if (lastScene == null)
			{
				lastScene = scene;
			}
		}
	}
}
