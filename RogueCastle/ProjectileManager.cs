using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
namespace RogueCastle
{
	public class ProjectileManager : IDisposable
	{
		private ProceduralLevelScreen m_levelScreen;
		private DS2DPool<ProjectileObj> m_projectilePool;
		private List<ProjectileObj> m_projectilesToRemoveList;
		private int m_poolSize;
		private bool m_isDisposed;
		public bool IsDisposed
		{
			get
			{
				return this.m_isDisposed;
			}
		}
		public List<ProjectileObj> ActiveProjectileList
		{
			get
			{
				return this.m_projectilePool.ActiveObjsList;
			}
		}
		public int ActiveProjectiles
		{
			get
			{
				return this.m_projectilePool.NumActiveObjs;
			}
		}
		public int TotalPoolSize
		{
			get
			{
				return this.m_projectilePool.TotalPoolSize;
			}
		}
		public int CurrentPoolSize
		{
			get
			{
				return this.TotalPoolSize - this.ActiveProjectiles;
			}
		}
		public ProjectileManager(ProceduralLevelScreen level, int poolSize)
		{
			this.m_projectilesToRemoveList = new List<ProjectileObj>();
			this.m_levelScreen = level;
			this.m_projectilePool = new DS2DPool<ProjectileObj>();
			this.m_poolSize = poolSize;
		}
		public void Initialize()
		{
			for (int i = 0; i < this.m_poolSize; i++)
			{
				ProjectileObj projectileObj = new ProjectileObj("BoneProjectile_Sprite");
				projectileObj.Visible = false;
				projectileObj.AnimationDelay = 0.05f;
				projectileObj.OutlineWidth = 2;
				this.m_projectilePool.AddToPool(projectileObj);
			}
		}
		public ProjectileObj FireProjectile(ProjectileData data)
		{
			if (data.Source == null)
			{
				throw new Exception("Cannot have a projectile with no source");
			}
			ProjectileObj projectileObj = this.m_projectilePool.CheckOut();
			projectileObj.Reset();
			projectileObj.LifeSpan = data.Lifespan;
			GameObj source = data.Source;
			projectileObj.ChaseTarget = data.ChaseTarget;
			projectileObj.Source = source;
			projectileObj.Target = data.Target;
			projectileObj.UpdateHeading();
			projectileObj.TurnSpeed = data.TurnSpeed;
			projectileObj.CollidesWithTerrain = data.CollidesWithTerrain;
			projectileObj.DestroysWithTerrain = data.DestroysWithTerrain;
			projectileObj.DestroysWithEnemy = data.DestroysWithEnemy;
			projectileObj.FollowArc = data.FollowArc;
			projectileObj.Orientation = MathHelper.ToRadians(data.StartingRotation);
			projectileObj.ShowIcon = data.ShowIcon;
			projectileObj.IsCollidable = data.IsCollidable;
			projectileObj.CollidesWith1Ways = data.CollidesWith1Ways;
			projectileObj.DestroyOnRoomTransition = data.DestroyOnRoomTransition;
			projectileObj.CanBeFusRohDahed = data.CanBeFusRohDahed;
			projectileObj.IgnoreInvincibleCounter = data.IgnoreInvincibleCounter;
			projectileObj.WrapProjectile = data.WrapProjectile;
			float num = 0f;
			if (data.Target != null)
			{
				float num2 = data.Target.X - source.X;
				float num3 = data.Target.Y - source.Y - data.SourceAnchor.Y;
				if (source.Flip == SpriteEffects.FlipHorizontally)
				{
					num = 180f - num;
					num2 += data.SourceAnchor.X;
					num = MathHelper.ToDegrees((float)Math.Atan2((double)num3, (double)num2));
					num -= data.AngleOffset;
				}
				else
				{
					num = MathHelper.ToDegrees((float)Math.Atan2((double)num3, (double)num2));
					num2 -= data.SourceAnchor.X;
					num += data.AngleOffset;
				}
			}
			else
			{
				num = data.Angle.X + data.AngleOffset;
				if (data.Angle.X != data.Angle.Y)
				{
					num = CDGMath.RandomFloat(data.Angle.X, data.Angle.Y) + data.AngleOffset;
				}
				if (source.Flip != SpriteEffects.None && source.Rotation != 0f)
				{
					num -= 180f;
				}
				else if (source.Flip != SpriteEffects.None && source.Rotation == 0f)
				{
					num = 180f - num;
				}
			}
			if (!data.LockPosition)
			{
				projectileObj.Rotation = num;
			}
			num = MathHelper.ToRadians(num);
			projectileObj.Damage = data.Damage;
			this.m_levelScreen.PhysicsManager.AddObject(projectileObj);
			projectileObj.ChangeSprite(data.SpriteName);
			projectileObj.RotationSpeed = data.RotationSpeed;
			projectileObj.Visible = true;
			if (source.Flip != SpriteEffects.None)
			{
				projectileObj.X = source.AbsX - data.SourceAnchor.X;
			}
			else
			{
				projectileObj.X = source.AbsX + data.SourceAnchor.X;
			}
			projectileObj.Y = source.AbsY + data.SourceAnchor.Y;
			projectileObj.IsWeighted = data.IsWeighted;
			Vector2 vector = new Vector2((float)Math.Cos((double)num), (float)Math.Sin((double)num));
			float num4 = data.Speed.X;
			if (data.Speed.X != data.Speed.Y)
			{
				num4 = CDGMath.RandomFloat(data.Speed.X, data.Speed.Y);
			}
			projectileObj.AccelerationX = vector.X * num4;
			projectileObj.AccelerationY = vector.Y * num4;
			projectileObj.CurrentSpeed = num4;
			if (source is PlayerObj)
			{
				if (projectileObj.LifeSpan == 0f)
				{
					projectileObj.LifeSpan = (source as PlayerObj).ProjectileLifeSpan;
				}
				projectileObj.CollisionTypeTag = 2;
				projectileObj.Scale = data.Scale;
			}
			else
			{
				if (projectileObj.LifeSpan == 0f)
				{
					projectileObj.LifeSpan = 15f;
				}
				projectileObj.CollisionTypeTag = 3;
				projectileObj.Scale = data.Scale;
			}
			if (data.Target != null && data.Source.Flip == SpriteEffects.FlipHorizontally && data.ChaseTarget)
			{
				projectileObj.Orientation = MathHelper.ToRadians(180f);
			}
			if (data.Source is PlayerObj && (Game.PlayerStats.Traits.X == 22f || Game.PlayerStats.Traits.Y == 22f))
			{
				projectileObj.AccelerationX *= -1f;
				if (!data.LockPosition)
				{
					if (data.Source.Flip == SpriteEffects.FlipHorizontally)
					{
						projectileObj.Flip = SpriteEffects.None;
					}
					else
					{
						projectileObj.Flip = SpriteEffects.FlipHorizontally;
					}
				}
			}
			projectileObj.PlayAnimation(true);
			return projectileObj;
		}
		public void DestroyProjectile(ProjectileObj projectile)
		{
			if (this.m_projectilePool.ActiveObjsList.Contains(projectile))
			{
				projectile.CollidesWithTerrain = true;
				projectile.DestroyOnRoomTransition = true;
				projectile.ChaseTarget = false;
				projectile.Target = null;
				projectile.Visible = false;
				projectile.Scale = new Vector2(1f, 1f);
				projectile.CurrentSpeed = 0f;
				projectile.Opacity = 1f;
				projectile.IsAlive = false;
				projectile.IsCollidable = true;
				this.m_levelScreen.PhysicsManager.RemoveObject(projectile);
				this.m_projectilePool.CheckIn(projectile);
			}
		}
		public void DestroyAllProjectiles(bool destroyRoomTransitionProjectiles)
		{
			ProjectileObj[] array = this.m_projectilePool.ActiveObjsList.ToArray();
			ProjectileObj[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				ProjectileObj projectileObj = array2[i];
				if (destroyRoomTransitionProjectiles || (!destroyRoomTransitionProjectiles && projectileObj.DestroyOnRoomTransition))
				{
					this.DestroyProjectile(projectileObj);
				}
			}
			this.PerformProjectileCleanup();
		}
		public void PauseAllProjectiles(bool pausePlayerProjectiles)
		{
			foreach (ProjectileObj current in this.m_projectilePool.ActiveObjsList)
			{
				if (current.CollisionTypeTag != 2 || pausePlayerProjectiles)
				{
					current.GamePaused = true;
					current.PauseAnimation();
					if (current.Spell != 11 || (current.Spell == 11 && current.CollisionTypeTag == 3))
					{
						current.AccelerationXEnabled = false;
						current.AccelerationYEnabled = false;
					}
				}
			}
			Tween.PauseAllContaining(typeof(ProjectileObj));
		}
		public void UnpauseAllProjectiles()
		{
			foreach (ProjectileObj current in this.m_projectilePool.ActiveObjsList)
			{
				if (current.GamePaused)
				{
					current.GamePaused = false;
					current.ResumeAnimation();
					if (current.Spell != 11 || (current.Spell == 11 && current.CollisionTypeTag == 3))
					{
						current.AccelerationXEnabled = true;
						current.AccelerationYEnabled = true;
					}
				}
			}
			Tween.ResumeAllContaining(typeof(ProjectileObj));
		}
		public void Update(GameTime gameTime)
		{
			RoomObj currentRoom = this.m_levelScreen.CurrentRoom;
			foreach (ProjectileObj current in this.m_projectilePool.ActiveObjsList)
			{
				if (current.WrapProjectile)
				{
					if (current.X < currentRoom.X)
					{
						current.X = currentRoom.X + (float)currentRoom.Width;
					}
					else if (current.X > currentRoom.X + (float)currentRoom.Width)
					{
						current.X = currentRoom.X;
					}
				}
				current.Update(gameTime);
			}
			if (currentRoom != null)
			{
				if (this.m_projectilesToRemoveList.Count > 0)
				{
					this.m_projectilesToRemoveList.Clear();
				}
				foreach (ProjectileObj current2 in this.m_projectilePool.ActiveObjsList)
				{
					if (current2.IsAlive && !current2.IsDying && !current2.IgnoreBoundsCheck)
					{
						if (current2.Bounds.Left < this.m_levelScreen.CurrentRoom.Bounds.Left - 200 || current2.Bounds.Right > this.m_levelScreen.CurrentRoom.Bounds.Right + 200)
						{
							this.m_projectilesToRemoveList.Add(current2);
						}
						else if (current2.Bounds.Bottom > this.m_levelScreen.CurrentRoom.Bounds.Bottom + 200)
						{
							this.m_projectilesToRemoveList.Add(current2);
						}
					}
					else if (!current2.IsAlive)
					{
						this.m_projectilesToRemoveList.Add(current2);
					}
				}
				foreach (ProjectileObj current3 in this.m_projectilesToRemoveList)
				{
					this.DestroyProjectile(current3);
				}
			}
		}
		public void PerformProjectileCleanup()
		{
			if (this.m_levelScreen.CurrentRoom != null)
			{
				if (this.m_projectilesToRemoveList.Count > 0)
				{
					this.m_projectilesToRemoveList.Clear();
				}
				foreach (ProjectileObj current in this.m_projectilePool.ActiveObjsList)
				{
					if (current.IsAlive && !current.IsDying && !current.IgnoreBoundsCheck)
					{
						if (current.Bounds.Left < this.m_levelScreen.CurrentRoom.Bounds.Left - 200 || current.Bounds.Right > this.m_levelScreen.CurrentRoom.Bounds.Right + 200)
						{
							this.m_projectilesToRemoveList.Add(current);
						}
						else if (current.Bounds.Bottom > this.m_levelScreen.CurrentRoom.Bounds.Bottom + 200)
						{
							this.m_projectilesToRemoveList.Add(current);
						}
					}
					else if (!current.IsAlive)
					{
						this.m_projectilesToRemoveList.Add(current);
					}
				}
				foreach (ProjectileObj current2 in this.m_projectilesToRemoveList)
				{
					this.DestroyProjectile(current2);
				}
			}
		}
		public void Draw(Camera2D camera)
		{
			foreach (ProjectileObj current in this.m_projectilePool.ActiveObjsList)
			{
				current.Draw(camera);
			}
		}
		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				Console.WriteLine("Disposing Projectile Manager");
				this.m_levelScreen = null;
				this.m_projectilePool.Dispose();
				this.m_projectilePool = null;
				this.m_projectilesToRemoveList.Clear();
				this.m_projectilesToRemoveList = null;
				this.m_isDisposed = true;
			}
		}
	}
}
