using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
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
				return this.m_attachedProjectile;
			}
			set
			{
				this.m_attachedProjectile = value;
				if (value != null)
				{
					this.m_iconProjectile.ChangeSprite(value.SpriteName);
					this.m_iconProjectile.Scale = new Vector2(0.7f, 0.7f);
				}
			}
		}
		public ProjectileIconObj()
		{
			base.ForceDraw = true;
			this.m_iconBG = new SpriteObj("ProjectileIcon_Sprite");
			this.m_iconBG.ForceDraw = true;
			this.m_iconProjectile = new SpriteObj("Blank_Sprite");
			this.m_iconProjectile.ForceDraw = true;
		}
		public void Update(Camera2D camera)
		{
			if (this.AttachedProjectile.X <= (float)(camera.Bounds.Left + this.m_iconOffset))
			{
				base.X = (float)this.m_iconOffset;
			}
			else if (this.AttachedProjectile.X > (float)(camera.Bounds.Right - this.m_iconOffset))
			{
				base.X = (float)(1320 - this.m_iconOffset);
			}
			else
			{
				base.X = this.AttachedProjectile.X - camera.TopLeftCorner.X;
			}
			if (this.AttachedProjectile.Y <= (float)(camera.Bounds.Top + this.m_iconOffset))
			{
				base.Y = (float)this.m_iconOffset;
			}
			else if (this.AttachedProjectile.Y > (float)(camera.Bounds.Bottom - this.m_iconOffset))
			{
				base.Y = (float)(720 - this.m_iconOffset);
			}
			else
			{
				base.Y = this.AttachedProjectile.Y - camera.TopLeftCorner.Y;
			}
			base.Rotation = CDGMath.AngleBetweenPts(camera.TopLeftCorner + base.Position, this.AttachedProjectile.Position);
			this.m_iconBG.Position = base.Position;
			this.m_iconBG.Rotation = base.Rotation;
			this.m_iconProjectile.Position = base.Position;
			this.m_iconProjectile.Rotation = this.AttachedProjectile.Rotation;
			this.m_iconProjectile.GoToFrame(this.AttachedProjectile.CurrentFrame);
		}
		public override void Draw(Camera2D camera)
		{
			if (base.Visible)
			{
				this.m_iconBG.Draw(camera);
				this.m_iconProjectile.Draw(camera);
			}
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_iconBG.Dispose();
				this.m_iconBG = null;
				this.m_iconProjectile.Dispose();
				this.m_iconProjectile = null;
				this.AttachedProjectile = null;
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
			projectileIconObj.AttachedProjectile = this.AttachedProjectile;
		}
	}
}
