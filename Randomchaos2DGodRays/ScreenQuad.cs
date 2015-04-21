using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Randomchaos2DGodRays
{
	public class ScreenQuad
	{
		private VertexPositionTexture[] corners;
		private VertexBuffer vb;
		private short[] ib;
		private VertexDeclaration vertDec;
		private Game Game;
		public ScreenQuad(Game game)
		{
			this.Game = game;
			this.corners = new VertexPositionTexture[4];
			this.corners[0].Position = new Vector3(0f, 0f, 0f);
			this.corners[0].TextureCoordinate = Vector2.Zero;
		}
		public virtual void Initialize()
		{
			this.vertDec = VertexPositionTexture.VertexDeclaration;
			this.corners = new VertexPositionTexture[]
			{
				new VertexPositionTexture(new Vector3(1f, -1f, 0f), new Vector2(1f, 1f)),
				new VertexPositionTexture(new Vector3(-1f, -1f, 0f), new Vector2(0f, 1f)),
				new VertexPositionTexture(new Vector3(-1f, 1f, 0f), new Vector2(0f, 0f)),
				new VertexPositionTexture(new Vector3(1f, 1f, 0f), new Vector2(1f, 0f))
			};
			this.ib = new short[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			};
			this.vb = new VertexBuffer(this.Game.GraphicsDevice, typeof(VertexPositionTexture), this.corners.Length, BufferUsage.None);
			this.vb.SetData<VertexPositionTexture>(this.corners);
		}
		public virtual void Draw()
		{
			this.Game.GraphicsDevice.SetVertexBuffer(this.vb);
			this.Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, this.corners, 0, 4, this.ib, 0, 2);
		}
	}
}
