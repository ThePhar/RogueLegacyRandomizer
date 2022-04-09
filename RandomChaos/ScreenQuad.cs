// 
//  Rogue Legacy Randomizer - ScreenQuad.cs
//  Last Modified 2022-04-08
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RandomChaos
{
    public sealed class ScreenQuad
    {
        private readonly Game _game;

        private VertexPositionTexture[] _corners;
        private short[]                 _ib;
        private VertexBuffer            _vb;
        private VertexDeclaration       _vertDec;

        public ScreenQuad(Game game)
        {
            _game = game;
            _corners = new VertexPositionTexture[4];
            _corners[0].Position = new Vector3(0f, 0f, 0f);
            _corners[0].TextureCoordinate = Vector2.Zero;
        }

        public void Initialize()
        {
            _vertDec = VertexPositionTexture.VertexDeclaration;
            _corners = new[]
            {
                new VertexPositionTexture(new Vector3(1f, -1f, 0f), new Vector2(1f, 1f)),
                new VertexPositionTexture(new Vector3(-1f, -1f, 0f), new Vector2(0f, 1f)),
                new VertexPositionTexture(new Vector3(-1f, 1f, 0f), new Vector2(0f, 0f)),
                new VertexPositionTexture(new Vector3(1f, 1f, 0f), new Vector2(1f, 0f))
            };
            _ib = new short[]
            {
                0,
                1,
                2,
                2,
                3,
                0
            };
            _vb = new VertexBuffer(_game.GraphicsDevice, typeof(VertexPositionTexture), _corners.Length,
                BufferUsage.None);
            _vb.SetData(_corners);
        }

        public void Draw()
        {
            _game.GraphicsDevice.SetVertexBuffer(_vb);
            _game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _corners, 0, 4, _ib, 0, 2);
        }
    }
}
