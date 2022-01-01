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
    public class EnergonProjectileObj : ProjectileObj
    {
        private const byte TYPE_SWORD = 0;
        private const byte TYPE_SHIELD = 1;
        private const byte TYPE_DOWNSWORD = 2;
        private bool m_canHitEnemy;
        private EnemyObj_Energon m_parent;

        public EnergonProjectileObj(string spriteName, EnemyObj_Energon parent) : base(spriteName)
        {
            TurnSpeed = 1f;
            IsWeighted = false;
            m_parent = parent;
            ChaseTarget = true;
        }

        public byte AttackType { get; private set; }

        public void SetType(byte type)
        {
            AttackType = type;
            switch (type)
            {
                case 0:
                    ChangeSprite("EnergonSwordProjectile_Sprite");
                    return;

                case 1:
                    ChangeSprite("EnergonShieldProjectile_Sprite");
                    return;

                case 2:
                    ChangeSprite("EnergonDownSwordProjectile_Sprite");
                    return;

                default:
                    return;
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            var playerObj = otherBox.AbsParent as PlayerObj;
            if (playerObj != null && !m_canHitEnemy)
            {
                if (AttackType == 0 && otherBox.Type == 1 && !playerObj.IsAirAttacking ||
                    AttackType == 1 && otherBox.Type == 2 && playerObj.State == 6 ||
                    AttackType == 2 && otherBox.Type == 1 && playerObj.IsAirAttacking)
                {
                    Target = m_parent;
                    CollisionTypeTag = 2;
                    CurrentSpeed *= 2f;
                    playerObj.AttachedLevel.ImpactEffectPool.DisplayEnemyImpactEffect(Position);
                    return;
                }

                if (otherBox.Type == 2)
                {
                    m_parent.DestroyProjectile(this);
                }
            }
            else
            {
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
            }
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_parent = null;
                base.Dispose();
            }
        }

        private void TurnToFace(Vector2 facePosition, float turnSpeed)
        {
            var num = facePosition.X - Position.X;
            var num2 = facePosition.Y - Position.Y;
            var num3 = (float) Math.Atan2(num2, num);
            var num4 = MathHelper.WrapAngle(num3 - Orientation);
            num4 = MathHelper.Clamp(num4, -turnSpeed, turnSpeed);
            Orientation = MathHelper.WrapAngle(Orientation + num4);
        }
    }
}