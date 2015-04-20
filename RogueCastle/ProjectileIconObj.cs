/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class ProjectileIconObj : GameObj
	{
		private SpriteObj m_iconBG;
		private SpriteObj m_iconProjectile;
		private ProjectileObj m_attachedProjectile;
		private int m_iconOffset = 60;
		public ProjectileObj AttachedProjectile
		{
			get
			{
				return m_attachedProjectile;
			}
			set
			{
				m_attachedProjectile = value;
				if (value != null)
				{
					m_iconProjectile.ChangeSprite(value.SpriteName);
					m_iconProjectile.Scale = new Vector2(0.7f, 0.7f);
				}
			}
		}
		public ProjectileIconObj()
		{
			ForceDraw = true;
			m_iconBG = new SpriteObj("ProjectileIcon_Sprite");
			m_iconBG.ForceDraw = true;
			m_iconProjectile = new SpriteObj("Blank_Sprite");
			m_iconProjectile.ForceDraw = true;
		}
		public void Update(Camera2D camera)
		{
			if (AttachedProjectile.X <= camera.Bounds.Left + m_iconOffset)
			{
				X = m_iconOffset;
			}
			else if (AttachedProjectile.X > camera.Bounds.Right - m_iconOffset)
			{
				X = 1320 - m_iconOffset;
			}
			else
			{
				X = AttachedProjectile.X - camera.TopLeftCorner.X;
			}
			if (AttachedProjectile.Y <= camera.Bounds.Top + m_iconOffset)
			{
				Y = m_iconOffset;
			}
			else if (AttachedProjectile.Y > camera.Bounds.Bottom - m_iconOffset)
			{
				Y = 720 - m_iconOffset;
			}
			else
			{
				Y = AttachedProjectile.Y - camera.TopLeftCorner.Y;
			}
			Rotation = CDGMath.AngleBetweenPts(camera.TopLeftCorner + Position, AttachedProjectile.Position);
			m_iconBG.Position = Position;
			m_iconBG.Rotation = Rotation;
			m_iconProjectile.Position = Position;
			m_iconProjectile.Rotation = AttachedProjectile.Rotation;
			m_iconProjectile.GoToFrame(AttachedProjectile.CurrentFrame);
		}
		public override void Draw(Camera2D camera)
		{
			if (Visible)
			{
				m_iconBG.Draw(camera);
				m_iconProjectile.Draw(camera);
			}
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_iconBG.Dispose();
				m_iconBG = null;
				m_iconProjectile.Dispose();
				m_iconProjectile = null;
				AttachedProjectile = null;
				base.Dispose();
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new ProjectileIconObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			ProjectileIconObj projectileIconObj = obj as ProjectileIconObj;
			projectileIconObj.AttachedProjectile = AttachedProjectile;
		}
	}
}
