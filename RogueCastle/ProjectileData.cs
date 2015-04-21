using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class ProjectileData : IDisposable
	{
		private bool m_isDisposed;
		public bool IsWeighted;
		public string SpriteName;
		public GameObj Source;
		public GameObj Target;
		public Vector2 SourceAnchor;
		public float AngleOffset;
		public Vector2 Angle;
		public float RotationSpeed;
		public int Damage;
		public Vector2 Speed;
		public Vector2 Scale = new Vector2(1f, 1f);
		public bool CollidesWithTerrain = true;
		public bool DestroysWithTerrain = true;
		public float Lifespan = 10f;
		public float TurnSpeed = 10f;
		public bool ChaseTarget;
		public bool FollowArc;
		public float StartingRotation;
		public bool ShowIcon = true;
		public bool IsCollidable = true;
		public bool DestroysWithEnemy = true;
		public bool LockPosition;
		public bool CollidesWith1Ways;
		public bool DestroyOnRoomTransition = true;
		public bool CanBeFusRohDahed = true;
		public bool IgnoreInvincibleCounter;
		public bool WrapProjectile;
		public bool IsDisposed
		{
			get
			{
				return this.m_isDisposed;
			}
		}
		public ProjectileData(GameObj source)
		{
			if (source == null)
			{
				throw new Exception("Cannot create a projectile without a source");
			}
			this.Source = source;
		}
		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				this.Source = null;
				this.Target = null;
				this.m_isDisposed = true;
			}
		}
		public ProjectileData Clone()
		{
			return new ProjectileData(this.Source)
			{
				IsWeighted = this.IsWeighted,
				SpriteName = this.SpriteName,
				Source = this.Source,
				Target = this.Target,
				SourceAnchor = this.SourceAnchor,
				AngleOffset = this.AngleOffset,
				Angle = this.Angle,
				RotationSpeed = this.RotationSpeed,
				Damage = this.Damage,
				Speed = this.Speed,
				Scale = this.Scale,
				CollidesWithTerrain = this.CollidesWithTerrain,
				Lifespan = this.Lifespan,
				ChaseTarget = this.ChaseTarget,
				TurnSpeed = this.TurnSpeed,
				FollowArc = this.FollowArc,
				StartingRotation = this.StartingRotation,
				ShowIcon = this.ShowIcon,
				DestroysWithTerrain = this.DestroysWithTerrain,
				IsCollidable = this.IsCollidable,
				DestroysWithEnemy = this.DestroysWithEnemy,
				LockPosition = this.LockPosition,
				CollidesWith1Ways = this.CollidesWith1Ways,
				DestroyOnRoomTransition = this.DestroyOnRoomTransition,
				CanBeFusRohDahed = this.CanBeFusRohDahed,
				IgnoreInvincibleCounter = this.IgnoreInvincibleCounter,
				WrapProjectile = this.WrapProjectile
			};
		}
	}
}
