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
