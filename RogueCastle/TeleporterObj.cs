/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;

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
			CollisionTypeTag = 1;
			SetCollision(false);
			IsWeighted = false;
			Activated = false;
			OutlineWidth = 2;
			m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
			m_arrowIcon.OutlineWidth = 2;
			m_arrowIcon.Visible = false;
		}
		public void SetCollision(bool collides)
		{
			CollidesTop = collides;
			CollidesBottom = collides;
			CollidesLeft = collides;
			CollidesRight = collides;
		}
		public override void Draw(Camera2D camera)
		{
			if (m_arrowIcon.Visible)
			{
				m_arrowIcon.Position = new Vector2(Bounds.Center.X, Bounds.Top - 50 + (float)Math.Sin(Game.TotalGameTime * 20f) * 2f);
				m_arrowIcon.Draw(camera);
				m_arrowIcon.Visible = false;
			}
			base.Draw(camera);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (!Game.ScreenManager.Player.ControlsLocked && playerObj != null && playerObj.IsTouchingGround)
			{
				m_arrowIcon.Visible = true;
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
			teleporterObj.Activated = Activated;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_arrowIcon.Dispose();
				m_arrowIcon = null;
				base.Dispose();
			}
		}
	}
}
