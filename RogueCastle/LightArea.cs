using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
			this.LightAreaSize = new Vector2((float)num);
			this.RenderTarget = new RenderTarget2D(graphicsDevice, num, num);
			this.graphicsDevice = graphicsDevice;
		}
		public Vector2 ToRelativePosition(Vector2 worldPosition)
		{
			return worldPosition - (this.LightPosition - this.LightAreaSize * 0.5f);
		}
		public void BeginDrawingShadowCasters()
		{
			this.graphicsDevice.SetRenderTarget(this.RenderTarget);
			this.graphicsDevice.Clear(Color.Transparent);
		}
		public void EndDrawingShadowCasters()
		{
			this.graphicsDevice.SetRenderTarget(null);
		}
	}
}
