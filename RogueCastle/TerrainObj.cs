using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class TerrainObj : BlankObj
	{
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
		public bool ShowTerrain = true;
		public Rectangle NonRotatedBounds
		{
			get
			{
				return new Rectangle((int)X, (int)Y, Width, Height);
			}
		}
		public TerrainObj(int width, int height) : base(width, height)
		{
			CollisionTypeTag = 1;
			IsCollidable = true;
			IsWeighted = false;
		}
		public override void Draw(Camera2D camera)
		{
			if ((ShowTerrain && CollisionMath.Intersects(Bounds, camera.Bounds)) || ForceDraw)
			{
				camera.Draw(Game.GenericTexture, Position, new Rectangle?(new Rectangle(0, 0, Width, Height)), TextureColor, MathHelper.ToRadians(Rotation), Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new TerrainObj(_width, _height);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			TerrainObj terrainObj = obj as TerrainObj;
			terrainObj.ShowTerrain = ShowTerrain;
			foreach (CollisionBox current in CollisionBoxes)
			{
				terrainObj.AddCollisionBox(current.X, current.Y, current.Width, current.Height, current.Type);
			}
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
	}
}
