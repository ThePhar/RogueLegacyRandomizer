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
		private byte m_currentAttackType;
		private bool m_canHitEnemy;
		private EnemyObj_Energon m_parent;
		public byte AttackType
		{
			get
			{
				return m_currentAttackType;
			}
		}
		public EnergonProjectileObj(string spriteName, EnemyObj_Energon parent) : base(spriteName)
		{
			TurnSpeed = 1f;
			IsWeighted = false;
			m_parent = parent;
			ChaseTarget = true;
		}
		public void SetType(byte type)
		{
			m_currentAttackType = type;
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
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (playerObj != null && !m_canHitEnemy)
			{
				if ((AttackType == 0 && otherBox.Type == 1 && !playerObj.IsAirAttacking) || (AttackType == 1 && otherBox.Type == 2 && playerObj.State == 6) || (AttackType == 2 && otherBox.Type == 1 && playerObj.IsAirAttacking))
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
					return;
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
			float num = facePosition.X - Position.X;
			float num2 = facePosition.Y - Position.Y;
			float num3 = (float)Math.Atan2(num2, num);
			float num4 = MathHelper.WrapAngle(num3 - Orientation);
			num4 = MathHelper.Clamp(num4, -turnSpeed, turnSpeed);
			Orientation = MathHelper.WrapAngle(Orientation + num4);
		}
	}
}
