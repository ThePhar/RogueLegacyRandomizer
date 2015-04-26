/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
    public class QuadRenderComponent : DrawableGameComponent
    {
        private short[] ib;
        private VertexPositionTexture[] verts;

        public QuadRenderComponent(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            verts = new[]
            {
                new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 1f)),
                new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(0f, 1f)),
                new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(0f, 0f)),
                new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(1f, 0f))
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
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public void Render(Vector2 v1, Vector2 v2)
        {
            verts[0].Position.X = v2.X;
            verts[0].Position.Y = v1.Y;
            verts[1].Position.X = v1.X;
            verts[1].Position.Y = v1.Y;
            verts[2].Position.X = v1.X;
            verts[2].Position.Y = v2.Y;
            verts[3].Position.X = v2.X;
            verts[3].Position.Y = v2.Y;
            GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
        }
    }
}