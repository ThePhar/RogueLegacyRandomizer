using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EnemyObj_Fireball : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalNeoLB = new LogicBlock();
		private Color FlameColour = Color.OrangeRed;
		private float DashDelay;
		private float DashSpeed;
		private float DashDuration;
		private float m_minibossFireTimeCounter = 0.6f;
		private float m_minibossFireTime = 0.6f;
		private float m_MinibossProjectileLifspan = 11f;
		private float m_MinibossProjectileLifspanNeo = 20f;
		private bool m_shake;
		private bool m_shookLeft;
		private float m_shakeTimer;
		private float m_shakeDuration = 0.03f;
		private int m_bossCoins = 30;
		private int m_bossMoneyBags = 17;
		private int m_bossDiamonds = 5;
		private bool m_isNeo;
		public bool BossVersionKilled
		{
			get
			{
				return this.m_bossVersionKilled;
			}
		}
		public bool IsNeo
		{
			get
			{
				return this.m_isNeo;
			}
			set
			{
				this.m_isNeo = value;
				if (value)
				{
					this.HealthGainPerLevel = 0;
					this.DamageGainPerLevel = 0;
					this.MoneyDropChance = 0f;
					this.ItemDropChance = 0f;
					this.m_saveToEnemiesKilledList = false;
				}
			}
		}
		protected override void InitializeEV()
		{
			this.DashDelay = 0.75f;
			this.DashDuration = 0.5f;
			this.DashSpeed = 900f;
			base.Name = "Charite";
			this.MaxHealth = 35;
			base.Damage = 23;
			base.XPValue = 100;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 400f;
			this.TurnSpeed = 0.0275f;
			this.ProjectileSpeed = 525f;
			base.JumpHeight = 950f;
			this.CooldownTime = 4f;
			base.AnimationDelay = 0.05f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = false;
			this.Scale = EnemyEV.Fireball_Basic_Scale;
			base.ProjectileScale = EnemyEV.Fireball_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Fireball_Basic_Tint;
			this.MeleeRadius = 500;
			this.ProjectileRadius = 700;
			this.EngageRadius = 1350;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Fireball_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Pyrite";
				this.MaxHealth = 45;
				base.Damage = 25;
				base.XPValue = 175;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 420f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 525f;
				base.JumpHeight = 950f;
				this.CooldownTime = 4f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Fireball_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Fireball_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Fireball_Advanced_Tint;
				this.MeleeRadius = 500;
				this.EngageRadius = 1350;
				this.ProjectileRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Fireball_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Infernite";
				this.MaxHealth = 63;
				base.Damage = 27;
				base.XPValue = 350;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 440f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 525f;
				base.JumpHeight = 950f;
				this.CooldownTime = 4f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Fireball_Expert_Scale;
				base.ProjectileScale = EnemyEV.Fireball_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Fireball_Expert_Tint;
				this.MeleeRadius = 500;
				this.ProjectileRadius = 700;
				this.EngageRadius = 1350;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Fireball_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Ponce de Leon";
				this.MaxHealth = 505;
				base.Damage = 29;
				base.XPValue = 800;
				this.MinMoneyDropAmount = 15;
				this.MaxMoneyDropAmount = 20;
				this.MoneyDropChance = 1f;
				base.Speed = 380f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 950f;
				this.CooldownTime = 4f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Fireball_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Fireball_Miniboss_ProjectileScale;
				this.MeleeRadius = 500;
				this.ProjectileRadius = 775;
				this.EngageRadius = 1350;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Fireball_Miniboss_KnockBack;
				if (LevelEV.WEAKEN_BOSSES)
				{
					this.MaxHealth = 1;
				}
				break;
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				this.m_resetSpriteName = "EnemyGhostBossIdle_Character";
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyGhostChase_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new ChaseLogicAction(this.m_target, true, 2f, -1f), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyGhostChase_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChaseLogicAction(this.m_target, false, 1f, -1f), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, 2f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostDashPrep_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.DashDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostDash_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, this.DashSpeed), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.DashDuration, false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostDashPrep_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.DashDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostDash_Character", true, true), Types.Sequence.Serial);
			this.ThrowProjectiles(logicSet6, true);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, this.DashSpeed), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.DashDuration, false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet7.AddAction(new ChaseLogicAction(this.m_target, true, 2f, -1f), Types.Sequence.Serial);
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet8.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(1f, 2f, false), Types.Sequence.Serial);
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet9.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet9.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossDashPrep_Character", true, true), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(this.DashDelay, false), Types.Sequence.Serial);
			logicSet9.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null), Types.Sequence.Serial);
			logicSet9.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet9.AddAction(new RunFunctionLogicAction(this, "ChangeFlameDirection", new object[0]), Types.Sequence.Serial);
			logicSet9.AddAction(new MoveLogicAction(this.m_target, true, this.DashSpeed), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(this.DashDuration, false), Types.Sequence.Serial);
			logicSet9.AddAction(new ChangePropertyLogicAction(this._objectList[0], "Rotation", 0), Types.Sequence.Serial);
			logicSet9.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet9.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet6
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet7,
				logicSet2,
				logicSet8,
				logicSet4,
				logicSet9
			});
			this.m_generalNeoLB.AddLogicSet(new LogicSet[]
			{
				logicSet7,
				logicSet2,
				logicSet8,
				logicSet4,
				logicSet9
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalNeoLB);
			LogicBlock arg_600_1 = this.m_generalBasicLB;
			int[] array = new int[5];
			array[0] = 100;
			base.SetCooldownLogicBlock(arg_600_1, array);
			base.InitializeLogic();
		}
		public void ChangeFlameDirection()
		{
			if (this.m_target.X < base.X)
			{
				this._objectList[0].Rotation = 90f;
				return;
			}
			this._objectList[0].Rotation = -90f;
		}
		private void ThrowProjectiles(LogicSet ls, bool useBossProjectile = false)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale
			};
			if (useBossProjectile)
			{
				projectileData.SpriteName = "GhostProjectile_Sprite";
			}
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(60f, 60f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(30f, 30f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(120f, 120f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(150f, 150f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-30f, -30f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-150f, -150f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Dispose();
		}
		private void ThrowStandingProjectile(bool useBossProjectile = false)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale,
				Lifespan = this.m_MinibossProjectileLifspan
			};
			if (this.IsNeo)
			{
				projectileData.Lifespan = this.m_MinibossProjectileLifspanNeo;
			}
			if (useBossProjectile)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
			}
			ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileObj.Rotation = 0f;
			if (this.IsNeo)
			{
				projectileObj.TextureColor = Color.MediumSpringGreen;
			}
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_4E_1 = true;
				LogicBlock arg_4E_2 = this.m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_4E_1, arg_4E_2, array);
				return;
			}
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array2);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = this.m_generalAdvancedLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			case 2:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalAdvancedLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 3:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					40,
					0,
					0,
					0,
					60
				});
				return;
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = this.m_generalExpertLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			case 2:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = this.m_generalExpertLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				base.RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 3:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					40,
					0,
					0,
					0,
					60
				});
				return;
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			if (!this.IsNeo)
			{
				switch (base.State)
				{
				case 0:
				{
					bool arg_76_1 = true;
					LogicBlock arg_76_2 = this.m_generalMiniBossLB;
					int[] array = new int[5];
					array[0] = 100;
					base.RunLogicBlock(arg_76_1, arg_76_2, array);
					return;
				}
				case 1:
				case 2:
				{
					bool arg_5B_1 = true;
					LogicBlock arg_5B_2 = this.m_generalMiniBossLB;
					int[] array2 = new int[5];
					array2[0] = 100;
					base.RunLogicBlock(arg_5B_1, arg_5B_2, array2);
					return;
				}
				case 3:
					base.RunLogicBlock(true, this.m_generalMiniBossLB, new int[]
					{
						52,
						0,
						0,
						0,
						48
					});
					return;
				default:
					return;
				}
			}
			else
			{
				switch (base.State)
				{
				case 0:
				{
					bool arg_F6_1 = true;
					LogicBlock arg_F6_2 = this.m_generalNeoLB;
					int[] array3 = new int[5];
					array3[0] = 100;
					base.RunLogicBlock(arg_F6_1, arg_F6_2, array3);
					return;
				}
				case 1:
				case 2:
				{
					bool arg_D8_1 = true;
					LogicBlock arg_D8_2 = this.m_generalNeoLB;
					int[] array4 = new int[5];
					array4[0] = 100;
					base.RunLogicBlock(arg_D8_1, arg_D8_2, array4);
					return;
				}
				case 3:
					base.RunLogicBlock(true, this.m_generalNeoLB, new int[]
					{
						45,
						0,
						0,
						0,
						55
					});
					return;
				default:
					return;
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !base.IsPaused && this.m_minibossFireTimeCounter > 0f && !this.m_bossVersionKilled)
			{
				this.m_minibossFireTimeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_minibossFireTimeCounter <= 0f)
				{
					this.ThrowStandingProjectile(true);
					this.m_minibossFireTimeCounter = this.m_minibossFireTime;
				}
			}
			if (this.m_shake && this.m_shakeTimer > 0f)
			{
				this.m_shakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_shakeTimer <= 0f)
				{
					this.m_shakeTimer = this.m_shakeDuration;
					if (this.m_shookLeft)
					{
						this.m_shookLeft = false;
						base.X += 5f;
					}
					else
					{
						base.X -= 5f;
						this.m_shookLeft = true;
					}
				}
			}
			base.Update(gameTime);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !this.m_bossVersionKilled)
			{
				PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
				if (playerObj != null && otherBox.Type == 1 && !playerObj.IsInvincible && playerObj.State == 8)
				{
					playerObj.HitPlayer(this);
				}
			}
			if (collisionResponseType != 1)
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
		}
		public void TurnToPlayer()
		{
			float turnSpeed = this.TurnSpeed;
			this.TurnSpeed = 2f;
			CDGMath.TurnToFace(this, this.m_target.Position);
			this.TurnSpeed = turnSpeed;
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			if (!this.m_bossVersionKilled)
			{
				base.HitEnemy(damage, collisionPt, isPlayer);
			}
		}
		public override void Kill(bool giveXP = true)
		{
			if (base.Difficulty != GameTypes.EnemyDifficulty.MINIBOSS)
			{
				base.Kill(giveXP);
				return;
			}
			if (this.m_target.CurrentHealth > 0)
			{
				Game.PlayerStats.FireballBossBeaten = true;
				SoundManager.StopMusic(0f);
				SoundManager.PlaySound("PressStart");
				this.m_bossVersionKilled = true;
				this.m_target.LockControls();
				this.m_levelScreen.PauseScreen();
				this.m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
				this.m_levelScreen.RunWhiteSlashEffect();
				Tween.RunFunction(1f, this, "Part2", new object[0]);
				SoundManager.PlaySound("Boss_Flash");
				SoundManager.PlaySound("Boss_Fireball_Freeze");
				GameUtil.UnlockAchievement("FEAR_OF_FIRE");
			}
		}
		public void Part2()
		{
			this.m_levelScreen.UnpauseScreen();
			this.m_target.UnlockControls();
			if (this.m_currentActiveLB != null)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			base.PauseEnemy(true);
			this.ChangeSprite("EnemyGhostBossIdle_Character");
			base.PlayAnimation(true);
			this.m_target.CurrentSpeed = 0f;
			this.m_target.ForceInvincible = true;
			Tween.To(this.m_levelScreen.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				base.X.ToString(),
				"Y",
				base.Y.ToString()
			});
			this.m_shake = true;
			this.m_shakeTimer = this.m_shakeDuration;
			for (int i = 0; i < 40; i++)
			{
				Vector2 vector = new Vector2((float)CDGMath.RandomInt(this.Bounds.Left, this.Bounds.Right), (float)CDGMath.RandomInt(this.Bounds.Top, this.Bounds.Bottom));
				Tween.RunFunction((float)i * 0.1f, typeof(SoundManager), "Play3DSound", new object[]
				{
					this,
					this.m_target,
					new string[]
					{
						"Boss_Explo_01",
						"Boss_Explo_02",
						"Boss_Explo_03"
					}
				});
				Tween.RunFunction((float)i * 0.1f, this.m_levelScreen.ImpactEffectPool, "DisplayExplosionEffect", new object[]
				{
					vector
				});
			}
			Tween.AddEndHandlerToLastTween(this, "Part3", new object[0]);
			if (!this.IsNeo)
			{
				List<int> list = new List<int>();
				for (int j = 0; j < this.m_bossCoins; j++)
				{
					list.Add(0);
				}
				for (int k = 0; k < this.m_bossMoneyBags; k++)
				{
					list.Add(1);
				}
				for (int l = 0; l < this.m_bossDiamonds; l++)
				{
					list.Add(2);
				}
				CDGMath.Shuffle<int>(list);
				float num = 2.5f / (float)list.Count;
				for (int m = 0; m < list.Count; m++)
				{
					Vector2 position = base.Position;
					if (list[m] == 0)
					{
						Tween.RunFunction((float)m * num, this.m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							position,
							1,
							10
						});
					}
					else if (list[m] == 1)
					{
						Tween.RunFunction((float)m * num, this.m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							position,
							10,
							100
						});
					}
					else
					{
						Tween.RunFunction((float)m * num, this.m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							position,
							11,
							500
						});
					}
				}
			}
		}
		public void Part3()
		{
			SoundManager.PlaySound("Boss_Fireball_Death");
			this.m_levelScreen.ImpactEffectPool.DestroyFireballBoss(base.Position);
			base.Kill(true);
		}
		public EnemyObj_Fireball(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 8;
			this.TintablePart = this._objectList[0];
		}
	}
}
