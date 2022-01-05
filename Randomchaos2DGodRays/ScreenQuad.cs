using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randomchaos2DGodRays
{
    public class ScreenQuad
    {
        private readonly Game Game;
        private VertexPositionTexture[] corners;
        private short[] ib;
        private VertexBuffer vb;
        private VertexDeclaration vertDec;

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
            corners = new[]
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
            vb.SetData(corners);
        }

        public virtual void Draw()
        {
            Game.GraphicsDevice.SetVertexBuffer(vb);
            Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, corners, 0, 4, ib, 0, 2);
        }
    }
}