/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			Game = game;
			corners = new VertexPositionTexture[4];
			corners[0].Position = new Vector3(0f, 0f, 0f);
			corners[0].TextureCoordinate = Vector2.Zero;
		}
		public virtual void Initialize()
		{
			vertDec = VertexPositionTexture.VertexDeclaration;
			corners = new VertexPositionTexture[]
			{
				new VertexPositionTexture(new Vector3(1f, -1f, 0f), new Vector2(1f, 1f)),
				new VertexPositionTexture(new Vector3(-1f, -1f, 0f), new Vector2(0f, 1f)),
				new VertexPositionTexture(new Vector3(-1f, 1f, 0f), new Vector2(0f, 0f)),
				new VertexPositionTexture(new Vector3(1f, 1f, 0f), new Vector2(1f, 0f))
			};
			ib = new short[]
			{
				0,
				1,
				2,
				2,
				3,
				0
			};
			vb = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionTexture), corners.Length, BufferUsage.None);
			vb.SetData<VertexPositionTexture>(corners);
		}
		public virtual void Draw()
		{
			Game.GraphicsDevice.SetVertexBuffer(vb);
			Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, corners, 0, 4, ib, 0, 2);
		}
	}
}
