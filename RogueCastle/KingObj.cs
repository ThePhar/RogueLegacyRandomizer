using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class KingObj : PhysicsObj
	{
		private bool m_wasHit;
		public bool WasHit
		{
			get
			{
				return this.m_wasHit;
			}
		}
		public KingObj(string spriteName) : base(spriteName, null)
		{
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
			if (collisionResponseType == 2 && physicsObj.CollisionTypeTag == 2 && !this.m_wasHit)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"EnemyHit1",
					"EnemyHit2",
					"EnemyHit3",
					"EnemyHit4",
					"EnemyHit5",
					"EnemyHit6"
				});
				SoundManager.PlaySound("Boss_Title_Exit");
				SoundManager.PlaySound("Player_Death_Grunt");
				Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
				if (thisBox.AbsRotation != 0f || otherBox.AbsRotation != 0f)
				{
					center = Rectangle.Intersect(thisBox.AbsParent.Bounds, otherBox.AbsParent.Bounds).Center;
				}
				Vector2 position = new Vector2((float)center.X, (float)center.Y);
				(otherBox.AbsParent as PlayerObj).AttachedLevel.ImpactEffectPool.DisplayEnemyImpactEffect(position);
				this.m_wasHit = true;
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new KingObj(base.SpriteName);
		}
	}
}
