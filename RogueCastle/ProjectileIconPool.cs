using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class ProjectileIconPool : IDisposable
	{
		private bool m_isDisposed;
		private DS2DPool<ProjectileIconObj> m_resourcePool;
		private int m_poolSize;
		private ProjectileManager m_projectileManager;
		private RCScreenManager m_screenManager;
		public bool IsDisposed
		{
			get
			{
				return this.m_isDisposed;
			}
		}
		public int ActiveTextObjs
		{
			get
			{
				return this.m_resourcePool.NumActiveObjs;
			}
		}
		public int TotalPoolSize
		{
			get
			{
				return this.m_resourcePool.TotalPoolSize;
			}
		}
		public int CurrentPoolSize
		{
			get
			{
				return this.TotalPoolSize - this.ActiveTextObjs;
			}
		}
		public ProjectileIconPool(int poolSize, ProjectileManager projectileManager, RCScreenManager screenManager)
		{
			this.m_poolSize = poolSize;
			this.m_resourcePool = new DS2DPool<ProjectileIconObj>();
			this.m_projectileManager = projectileManager;
			this.m_screenManager = screenManager;
		}
		public void Initialize()
		{
			for (int i = 0; i < this.m_poolSize; i++)
			{
				ProjectileIconObj projectileIconObj = new ProjectileIconObj();
				projectileIconObj.Visible = false;
				projectileIconObj.ForceDraw = true;
				projectileIconObj.TextureColor = Color.White;
				this.m_resourcePool.AddToPool(projectileIconObj);
			}
		}
		public void AddIcon(ProjectileObj projectile)
		{
			ProjectileIconObj projectileIconObj = this.m_resourcePool.CheckOut();
			projectileIconObj.Visible = true;
			projectileIconObj.ForceDraw = true;
			projectileIconObj.AttachedProjectile = projectile;
			projectile.AttachedIcon = projectileIconObj;
		}
		public void DestroyIcon(ProjectileObj projectile)
		{
			ProjectileIconObj attachedIcon = projectile.AttachedIcon;
			attachedIcon.Visible = false;
			attachedIcon.Rotation = 0f;
			attachedIcon.TextureColor = Color.White;
			attachedIcon.Opacity = 1f;
			attachedIcon.Flip = SpriteEffects.None;
			attachedIcon.Scale = new Vector2(1f, 1f);
			this.m_resourcePool.CheckIn(attachedIcon);
			attachedIcon.AttachedProjectile = null;
			projectile.AttachedIcon = null;
		}
		public void DestroyAllIcons()
		{
			foreach (ProjectileObj current in this.m_projectileManager.ActiveProjectileList)
			{
				if (current.AttachedIcon != null)
				{
					this.DestroyIcon(current);
				}
			}
		}
		public void Update(Camera2D camera)
		{
			PlayerObj player = this.m_screenManager.Player;
			foreach (ProjectileObj current in this.m_projectileManager.ActiveProjectileList)
			{
				if (current.ShowIcon)
				{
					if (current.AttachedIcon == null)
					{
						if (!CollisionMath.Intersects(current.Bounds, camera.Bounds) && ((current.AccelerationX > 1f && current.X < player.X && current.Y > (float)camera.Bounds.Top && current.Y < (float)camera.Bounds.Bottom) || (current.AccelerationX < -1f && current.X > player.X && current.Y > (float)camera.Bounds.Top && current.Y < (float)camera.Bounds.Bottom) || (current.AccelerationY > 1f && current.Y < player.Y && current.X > (float)camera.Bounds.Left && current.X < (float)camera.Bounds.Right) || (current.AccelerationY < -1f && current.Y > player.Y && current.X > (float)camera.Bounds.Left && current.X < (float)camera.Bounds.Right)))
						{
							this.AddIcon(current);
						}
					}
					else if (CollisionMath.Intersects(current.Bounds, camera.Bounds))
					{
						this.DestroyIcon(current);
					}
				}
			}
			for (int i = 0; i < this.m_resourcePool.ActiveObjsList.Count; i++)
			{
				if (!this.m_resourcePool.ActiveObjsList[i].AttachedProjectile.IsAlive)
				{
					this.DestroyIcon(this.m_resourcePool.ActiveObjsList[i].AttachedProjectile);
					i--;
				}
			}
			foreach (ProjectileIconObj current2 in this.m_resourcePool.ActiveObjsList)
			{
				current2.Update(camera);
			}
		}
		public void Draw(Camera2D camera)
		{
			if (Game.PlayerStats.Traits.X != 21f && Game.PlayerStats.Traits.Y != 21f)
			{
				foreach (ProjectileIconObj current in this.m_resourcePool.ActiveObjsList)
				{
					current.Draw(camera);
				}
			}
		}
		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				Console.WriteLine("Disposing Projectile Icon Pool");
				this.m_resourcePool.Dispose();
				this.m_resourcePool = null;
				this.m_isDisposed = true;
				this.m_projectileManager = null;
				this.m_screenManager = null;
			}
		}
	}
}
