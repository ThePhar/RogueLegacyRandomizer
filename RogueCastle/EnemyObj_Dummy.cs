/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class EnemyObj_Dummy : EnemyObj
	{
		protected override void InitializeEV()
		{
			Scale = new Vector2(2.2f, 2.2f);
			AnimationDelay = 0.0166666675f;
			Speed = 200f;
			MaxHealth = 999;
			EngageRadius = 30;
			ProjectileRadius = 20;
			MeleeRadius = 10;
			CooldownTime = 2f;
			KnockBack = new Vector2(1f, 2f);
			Damage = 25;
			JumpHeight = 20.5f;
			AlwaysFaceTarget = false;
			CanFallOffLedges = false;
			XPValue = 2;
			CanBeKnockedBack = false;
			Name = "Training Dummy";
			IsWeighted = false;
			LockFlip = true;
			switch (Difficulty)
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
			if (m_target != null && m_target.CurrentHealth > 0)
			{
				ChangeSprite("DummySad_Character");
				PlayAnimation(false);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"EnemyHit1",
					"EnemyHit2",
					"EnemyHit3",
					"EnemyHit4",
					"EnemyHit5",
					"EnemyHit6"
				});
				Blink(Color.Red, 0.1f);
				m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(collisionPt);
				m_levelScreen.ImpactEffectPool.WoodChipEffect(new Vector2(X, Bounds.Center.Y));
				if (isPlayer)
				{
					PlayerObj expr_D0 = m_target;
					expr_D0.NumSequentialAttacks += 1;
					if (m_target.IsAirAttacking)
					{
						m_target.IsAirAttacking = false;
						m_target.AccelerationY = -m_target.AirAttackKnockBack;
						m_target.NumAirBounces++;
					}
				}
				m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(X, Bounds.Top));
				m_levelScreen.SetLastEnemyHit(this);
				RandomizeName();
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
			Name = array[CDGMath.RandomInt(0, array.Length - 1)];
		}
		public EnemyObj_Dummy(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("Dummy_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			Type = 30;
		}
	}
}
