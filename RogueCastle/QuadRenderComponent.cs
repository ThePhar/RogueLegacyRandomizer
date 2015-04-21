using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class QuadRenderComponent : DrawableGameComponent
	{
		private VertexPositionTexture[] verts;
		private short[] ib;
		public QuadRenderComponent(Game game) : base(game)
		{
		}
		protected override void LoadContent()
		{
			this.verts = new VertexPositionTexture[]
			{
				new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 1f)),
				new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(0f, 1f)),
				new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(0f, 0f)),
				new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 0f))
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
			base.LoadContent();
		}
		protected override void UnloadContent()
		{
			base.UnloadContent();
		}
		public void Render(Vector2 v1, Vector2 v2)
		{
			this.verts[0].Position.X = v2.X;
			this.verts[0].Position.Y = v1.Y;
			this.verts[1].Position.X = v1.X;
			this.verts[1].Position.Y = v1.Y;
			this.verts[2].Position.X = v1.X;
			this.verts[2].Position.Y = v2.Y;
			this.verts[3].Position.X = v2.X;
			this.verts[3].Position.Y = v2.Y;
			base.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, this.verts, 0, 4, this.ib, 0, 2);
		}
	}
}
