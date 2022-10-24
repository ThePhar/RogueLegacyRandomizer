// Rogue Legacy Randomizer - TerrainObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLegacy
{
    public class TerrainObj : BlankObj
    {
        public bool ShowTerrain = true;

        public TerrainObj(int width, int height) : base(width, height)
        {
            CollisionTypeTag = 1;
            IsCollidable = true;
            IsWeighted = false;
        }

        public Rectangle NonRotatedBounds
        {
            get { return new Rectangle((int) X, (int) Y, Width, Height); }
        }

        public override void Draw(Camera2D camera)
        {
            if (ShowTerrain && CollisionMath.Intersects(Bounds, camera.Bounds) || ForceDraw)
            {
                camera.Draw(Game.GenericTexture, Position, new Rectangle(0, 0, Width, Height), TextureColor,
                    MathHelper.ToRadians(Rotation), Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        protected override GameObj CreateCloneInstance()
        {
            return new TerrainObj(_width, _height);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var terrainObj = obj as TerrainObj;
            terrainObj.ShowTerrain = ShowTerrain;
            foreach (var current in CollisionBoxes)
                terrainObj.AddCollisionBox(current.X, current.Y, current.Width, current.Height, current.Type);
        }

        public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
        {
            base.PopulateFromXMLReader(reader, ci);
            SetWidth(_width);
            SetHeight(_height);
            AddCollisionBox(0, 0, _width, _height, 0);
            AddCollisionBox(0, 0, _width, _height, 2);
            if (CollidesTop && !CollidesBottom && !CollidesLeft && !CollidesRight)
            {
                SetHeight(Height / 2);
            }
        }

        private struct CornerPoint
        {
            public Vector2 Position;
            public float Rotation;

            public CornerPoint(Vector2 position, float rotation)
            {
                Position = position;
                Rotation = MathHelper.ToRadians(rotation);
            }
        }
    }
}
