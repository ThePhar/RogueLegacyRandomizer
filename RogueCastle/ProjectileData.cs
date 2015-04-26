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
        public Vector2 Angle;
        public float AngleOffset;
        public bool CanBeFusRohDahed = true;
        public bool ChaseTarget;
        public bool CollidesWith1Ways;
        public bool CollidesWithTerrain = true;
        public int Damage;
        public bool DestroyOnRoomTransition = true;
        public bool DestroysWithEnemy = true;
        public bool DestroysWithTerrain = true;
        public bool FollowArc;
        public bool IgnoreInvincibleCounter;
        public bool IsCollidable = true;
        public bool IsWeighted;
        public float Lifespan = 10f;
        public bool LockPosition;
        public float RotationSpeed;
        public Vector2 Scale = new Vector2(1f, 1f);
        public bool ShowIcon = true;
        public GameObj Source;
        public Vector2 SourceAnchor;
        public Vector2 Speed;
        public string SpriteName;
        public float StartingRotation;
        public GameObj Target;
        public float TurnSpeed = 10f;
        public bool WrapProjectile;

        public ProjectileData(GameObj source)
        {
            if (source == null)
            {
                throw new Exception("Cannot create a projectile without a source");
            }
            Source = source;
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Source = null;
                Target = null;
                IsDisposed = true;
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