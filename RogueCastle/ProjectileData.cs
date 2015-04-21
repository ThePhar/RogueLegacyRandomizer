/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;

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
            get { return m_isDisposed; }
        }

        public ProjectileData(GameObj source)
        {
            if (source == null)
            {
                throw new Exception("Cannot create a projectile without a source");
            }
            Source = source;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Source = null;
                Target = null;
                m_isDisposed = true;
            }
        }

        public ProjectileData Clone()
        {
            return new ProjectileData(Source)
            {
                IsWeighted = IsWeighted,
                SpriteName = SpriteName,
                Source = Source,
                Target = Target,
                SourceAnchor = SourceAnchor,
                AngleOffset = AngleOffset,
                Angle = Angle,
                RotationSpeed = RotationSpeed,
                Damage = Damage,
                Speed = Speed,
                Scale = Scale,
                CollidesWithTerrain = CollidesWithTerrain,
                Lifespan = Lifespan,
                ChaseTarget = ChaseTarget,
                TurnSpeed = TurnSpeed,
                FollowArc = FollowArc,
                StartingRotation = StartingRotation,
                ShowIcon = ShowIcon,
                DestroysWithTerrain = DestroysWithTerrain,
                IsCollidable = IsCollidable,
                DestroysWithEnemy = DestroysWithEnemy,
                LockPosition = LockPosition,
                CollidesWith1Ways = CollidesWith1Ways,
                DestroyOnRoomTransition = DestroyOnRoomTransition,
                CanBeFusRohDahed = CanBeFusRohDahed,
                IgnoreInvincibleCounter = IgnoreInvincibleCounter,
                WrapProjectile = WrapProjectile
            };
        }
    }
}