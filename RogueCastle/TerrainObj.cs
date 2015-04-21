using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Xml;
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
				this.Position = position;
				this.Rotation = MathHelper.ToRadians(rotation);
			}
		}
		public bool ShowTerrain = true;
		public Rectangle NonRotatedBounds
		{
			get
			{
				return new Rectangle((int)base.X, (int)base.Y, this.Width, this.Height);
			}
		}
		public TerrainObj(int width, int height) : base(width, height)
		{
			base.CollisionTypeTag = 1;
			base.IsCollidable = true;
			base.IsWeighted = false;
		}
		public override void Draw(Camera2D camera)
		{
			if ((this.ShowTerrain && CollisionMath.Intersects(this.Bounds, camera.Bounds)) || base.ForceDraw)
			{
				camera.Draw(Game.GenericTexture, base.Position, new Rectangle?(new Rectangle(0, 0, this.Width, this.Height)), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new TerrainObj(this._width, this._height);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			TerrainObj terrainObj = obj as TerrainObj;
			terrainObj.ShowTerrain = this.ShowTerrain;
			foreach (CollisionBox current in base.CollisionBoxes)
			{
				terrainObj.AddCollisionBox(current.X, current.Y, current.Width, current.Height, current.Type);
			}
		}
		public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
		{
			base.PopulateFromXMLReader(reader, ci);
			base.SetWidth(this._width);
			base.SetHeight(this._height);
			base.AddCollisionBox(0, 0, this._width, this._height, 0);
			base.AddCollisionBox(0, 0, this._width, this._height, 2);
			if (base.CollidesTop && !base.CollidesBottom && !base.CollidesLeft && !base.CollidesRight)
			{
				base.SetHeight(this.Height / 2);
			}
		}
	}
}
