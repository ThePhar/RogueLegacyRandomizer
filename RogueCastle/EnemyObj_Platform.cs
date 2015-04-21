using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using Tweener;
namespace RogueCastle
{
	public class EnemyObj_Platform : EnemyObj
	{
		private bool m_isExtended;
		private float m_retractCounter;
		private bool m_blinkedWarning;
		private float RetractDelay;
		protected override void InitializeEV()
		{
			this.Scale = new Vector2(2f, 2f);
			base.AnimationDelay = 0.0333333351f;
			base.Speed = 0f;
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
			base.LockFlip = true;
			base.IsWeighted = false;
			this.RetractDelay = 3f;
			base.Name = "Platform";
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
			case GameTypes.EnemyDifficulty.ADVANCED:
			case GameTypes.EnemyDifficulty.EXPERT:
			case GameTypes.EnemyDifficulty.MINIBOSS:
				IL_DE:
				if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
				{
					this.m_isExtended = true;
					base.PlayAnimation("EndRetract", "EndRetract", false);
				}
				return;
			}
			goto IL_DE;
		}
		public override void Update(GameTime gameTime)
		{
			bool flag = false;
			if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
			{
				flag = true;
			}
			if (!flag)
			{
				if (this.m_retractCounter > 0f)
				{
					this.m_retractCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (this.m_retractCounter <= 1.5f && !this.m_blinkedWarning)
					{
						this.m_blinkedWarning = true;
						float num = 0f;
						for (int i = 0; i < 10; i++)
						{
							Tween.RunFunction(num, this, "Blink", new object[]
							{
								Color.Red,
								0.05f
							});
							num += 0.1f;
						}
					}
					if (this.m_retractCounter <= 0f)
					{
						this.m_isExtended = false;
						base.PlayAnimation("StartExtract", "EndExtract", false);
						SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
						{
							"Platform_Activate_03",
							"Platform_Activate_04"
						});
					}
				}
			}
			else if (!this.m_isExtended)
			{
				this.m_isExtended = true;
				base.PlayAnimation("EndRetract", "EndRetract", false);
			}
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer = true)
		{
			if (this.m_target.IsAirAttacking && !this.m_isExtended && (base.CurrentFrame == 1 || base.CurrentFrame == base.TotalFrames))
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
				base.Blink(Color.Red, 0.1f);
				this.m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(position);
				this.m_isExtended = true;
				this.m_blinkedWarning = false;
				base.PlayAnimation("StartRetract", "EndRetract", false);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"Platform_Activate_01",
					"Platform_Activate_02"
				});
				this.m_retractCounter = this.RetractDelay;
				if (this.m_target.IsAirAttacking)
				{
					this.m_target.IsAirAttacking = false;
					this.m_target.AccelerationY = -this.m_target.AirAttackKnockBack;
					this.m_target.NumAirBounces++;
				}
			}
		}
		public override void ResetState()
		{
			if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
			{
				return;
			}
			base.PlayAnimation(1, 1, false);
			this.m_isExtended = false;
			this.m_blinkedWarning = false;
			this.m_retractCounter = 0f;
			base.ResetState();
		}
		public override void Reset()
		{
			if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
			{
				return;
			}
			base.PlayAnimation(1, 1, false);
			this.m_isExtended = false;
			this.m_blinkedWarning = false;
			this.m_retractCounter = 0f;
			base.Reset();
			base.StopAnimation();
		}
		public EnemyObj_Platform(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyPlatform_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.CollisionTypeTag = 1;
			this.Type = 27;
			base.CollidesBottom = false;
			base.CollidesLeft = false;
			base.CollidesRight = false;
			base.StopAnimation();
			base.PlayAnimationOnRestart = false;
			base.NonKillable = true;
			base.DisableCollisionBoxRotations = false;
		}
	}
}
