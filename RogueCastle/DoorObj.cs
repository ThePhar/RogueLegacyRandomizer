using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Xml;
namespace RogueCastle
{
	public class DoorObj : TerrainObj
	{
		private GameTypes.DoorType m_doorType = GameTypes.DoorType.OPEN;
		public string DoorPosition = "NONE";
		public bool Attached;
		public bool IsBossDoor;
		public bool Locked;
		private SpriteObj m_arrowIcon;
		public RoomObj Room
		{
			get;
			set;
		}
		public GameTypes.DoorType DoorType
		{
			get
			{
				return this.m_doorType;
			}
		}
		public DoorObj(RoomObj roomRef, int width, int height, GameTypes.DoorType doorType) : base(width, height)
		{
			this.m_doorType = doorType;
			this.Room = roomRef;
			base.CollisionTypeTag = 0;
			base.DisableHitboxUpdating = true;
			this.m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
			this.m_arrowIcon.OutlineWidth = 2;
			this.m_arrowIcon.Visible = false;
		}
		public override void Draw(Camera2D camera)
		{
			if (this.m_arrowIcon.Visible)
			{
				this.m_arrowIcon.Position = new Vector2((float)this.Bounds.Center.X, (float)(this.Bounds.Top - 10) + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 3f);
				this.m_arrowIcon.Draw(camera);
				this.m_arrowIcon.Visible = false;
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (!this.Locked && playerObj != null && playerObj.IsTouchingGround && this.DoorPosition == "None")
			{
				this.m_arrowIcon.Visible = true;
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new DoorObj(this.Room, this._width, this._height, this.m_doorType);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			DoorObj doorObj = obj as DoorObj;
			doorObj.Attached = this.Attached;
			doorObj.IsBossDoor = this.IsBossDoor;
			doorObj.Locked = this.Locked;
			doorObj.DoorPosition = this.DoorPosition;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.Room = null;
				this.m_arrowIcon.Dispose();
				this.m_arrowIcon = null;
				base.Dispose();
			}
		}
		public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
		{
			base.PopulateFromXMLReader(reader, ci);
			if (reader.MoveToAttribute("BossDoor"))
			{
				this.IsBossDoor = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("DoorPos"))
			{
				this.DoorPosition = reader.Value;
			}
		}
	}
}
