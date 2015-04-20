/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	internal class LightArea
	{
		private GraphicsDevice graphicsDevice;
		public RenderTarget2D RenderTarget
		{
			get;
			private set;
		}
		public Vector2 LightPosition
		{
			get;
			set;
		}
		public Vector2 LightAreaSize
		{
			get;
			set;
		}
		public LightArea(GraphicsDevice graphicsDevice, ShadowmapSize size)
		{
			int num = 2 << (int)size;
			LightAreaSize = new Vector2(num);
			RenderTarget = new RenderTarget2D(graphicsDevice, num, num);
			this.graphicsDevice = graphicsDevice;
		}
		public Vector2 ToRelativePosition(Vector2 worldPosition)
		{
			return worldPosition - (LightPosition - LightAreaSize * 0.5f);
		}
		public void BeginDrawingShadowCasters()
		{
			graphicsDevice.SetRenderTarget(RenderTarget);
			graphicsDevice.Clear(Color.Transparent);
		}
		public void EndDrawingShadowCasters()
		{
			graphicsDevice.SetRenderTarget(null);
		}
	}
}
