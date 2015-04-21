using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class TeleporterObj : PhysicsObj
	{
		private SpriteObj m_arrowIcon;
		public bool Activated
		{
			get;
			set;
		}
		public TeleporterObj() : base("TeleporterBase_Sprite", null)
		{
			base.CollisionTypeTag = 1;
			this.SetCollision(false);
			base.IsWeighted = false;
			this.Activated = false;
			base.OutlineWidth = 2;
			this.m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
			this.m_arrowIcon.OutlineWidth = 2;
			this.m_arrowIcon.Visible = false;
		}
		public void SetCollision(bool collides)
		{
			base.CollidesTop = collides;
			base.CollidesBottom = collides;
			base.CollidesLeft = collides;
			base.CollidesRight = collides;
		}
		public override void Draw(Camera2D camera)
		{
			if (this.m_arrowIcon.Visible)
			{
				this.m_arrowIcon.Position = new Vector2((float)this.Bounds.Center.X, (float)(this.Bounds.Top - 50) + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f);
				this.m_arrowIcon.Draw(camera);
				this.m_arrowIcon.Visible = false;
			}
			base.Draw(camera);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (!Game.ScreenManager.Player.ControlsLocked && playerObj != null && playerObj.IsTouchingGround)
			{
				this.m_arrowIcon.Visible = true;
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new TeleporterObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			TeleporterObj teleporterObj = obj as TeleporterObj;
			teleporterObj.Activated = this.Activated;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_arrowIcon.Dispose();
				this.m_arrowIcon = null;
				base.Dispose();
			}
		}
	}
}
