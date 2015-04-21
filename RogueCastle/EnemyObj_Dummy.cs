using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Dummy : EnemyObj
	{
		protected override void InitializeEV()
		{
			this.Scale = new Vector2(2.2f, 2.2f);
			base.AnimationDelay = 0.0166666675f;
			base.Speed = 200f;
			this.MaxHealth = 999;
			this.EngageRadius = 30;
			this.ProjectileRadius = 20;
			this.MeleeRadius = 10;
			this.CooldownTime = 2f;
			base.KnockBack = new Vector2(1f, 2f);
			base.Damage = 25;
			base.JumpHeight = 20.5f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.XPValue = 2;
			base.CanBeKnockedBack = false;
			base.Name = "Training Dummy";
			base.IsWeighted = false;
			base.LockFlip = true;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
			case GameTypes.EnemyDifficulty.ADVANCED:
			case GameTypes.EnemyDifficulty.EXPERT:
			case GameTypes.EnemyDifficulty.MINIBOSS:
				return;
			}
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			if (this.m_target != null && this.m_target.CurrentHealth > 0)
			{
				this.ChangeSprite("DummySad_Character");
				base.PlayAnimation(false);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"EnemyHit1",
					"EnemyHit2",
					"EnemyHit3",
					"EnemyHit4",
					"EnemyHit5",
					"EnemyHit6"
				});
				base.Blink(Color.Red, 0.1f);
				this.m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(collisionPt);
				this.m_levelScreen.ImpactEffectPool.WoodChipEffect(new Vector2(base.X, (float)this.Bounds.Center.Y));
				if (isPlayer)
				{
					PlayerObj expr_D0 = this.m_target;
					expr_D0.NumSequentialAttacks += 1;
					if (this.m_target.IsAirAttacking)
					{
						this.m_target.IsAirAttacking = false;
						this.m_target.AccelerationY = -this.m_target.AirAttackKnockBack;
						this.m_target.NumAirBounces++;
					}
				}
				this.m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(base.X, (float)this.Bounds.Top));
				this.m_levelScreen.SetLastEnemyHit(this);
				this.RandomizeName();
			}
		}
		private void RandomizeName()
		{
			string[] array = new string[]
			{
				"Oh god!",
				"The pain!",
				"It hurts so much",
				"STOP IT!!",
				"Training Dummy",
				"What'd I do to you?",
				"WHY???",
				"Stop hitting me",
				"Why you do this?",
				"IT HUURRRTS",
				"Oof",
				"Ouch",
				"BLARGH",
				"I give up!",
				"Do you even lift?",
				"Ok, that one hurt",
				"That tickled",
				"That all you got?",
				"Dost thou even hoist?",
				"Nice try",
				"Weak",
				"Try again"
			};
			base.Name = array[CDGMath.RandomInt(0, array.Length - 1)];
		}
		public EnemyObj_Dummy(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("Dummy_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 30;
		}
	}
}
