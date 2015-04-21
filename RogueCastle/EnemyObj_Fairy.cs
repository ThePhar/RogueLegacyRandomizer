using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EnemyObj_Fairy : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private LogicBlock m_generalNeoLB = new LogicBlock();
		private bool m_isNeo;
		private float AttackDelay = 0.5f;
		public RoomObj SpawnRoom;
		public int NumHits = 1;
		private bool m_shake;
		private bool m_shookLeft;
		private float m_shakeTimer;
		private float m_shakeDuration = 0.03f;
		private int m_bossCoins = 30;
		private int m_bossMoneyBags = 12;
		private int m_bossDiamonds = 3;
		private Cue m_deathLoop;
		private bool m_playDeathLoop;
		private int m_numSummons = 22;
		private float m_summonTimer = 6f;
		private float m_summonCounter = 6f;
		private float m_summonTimerNeo = 3f;
		public bool MainFairy
		{
			get;
			set;
		}
		public Vector2 SavedStartingPos
		{
			get;
			set;
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
			base.Name = "Fury";
			this.MaxHealth = 27;
			base.Damage = 18;
			base.XPValue = 125;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 250f;
			this.TurnSpeed = 0.0325f;
			this.ProjectileSpeed = 475f;
			base.JumpHeight = 300f;
			this.CooldownTime = 1.75f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = true;
			base.CanBeKnockedBack = true;
			base.IsWeighted = false;
			this.Scale = EnemyEV.Fairy_Basic_Scale;
			base.ProjectileScale = EnemyEV.Fairy_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Fairy_Basic_Tint;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 700;
			this.EngageRadius = 925;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Fairy_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Rage";
				this.MaxHealth = 37;
				base.Damage = 22;
				base.XPValue = 200;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 265f;
				this.TurnSpeed = 0.0325f;
				this.ProjectileSpeed = 475f;
				base.JumpHeight = 300f;
				this.CooldownTime = 1.75f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Fairy_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Fairy_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Fairy_Advanced_Tint;
				this.MeleeRadius = 225;
				this.EngageRadius = 925;
				this.ProjectileRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Fairy_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Wrath";
				this.MaxHealth = 72;
				base.Damage = 24;
				base.XPValue = 350;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 280f;
				this.TurnSpeed = 0.0325f;
				this.ProjectileSpeed = 475f;
				base.JumpHeight = 300f;
				this.CooldownTime = 2.5f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Fairy_Expert_Scale;
				base.ProjectileScale = EnemyEV.Fairy_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Fairy_Expert_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 925;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Fairy_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Alexander";
				this.MaxHealth = 635;
				base.Damage = 30;
				base.XPValue = 1000;
				this.MinMoneyDropAmount = 15;
				this.MaxMoneyDropAmount = 20;
				this.MoneyDropChance = 1f;
				base.Speed = 220f;
				this.TurnSpeed = 0.0325f;
				this.ProjectileSpeed = 545f;
				base.JumpHeight = 300f;
				this.CooldownTime = 3f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = new Vector2(2.5f, 2.5f);
				base.ProjectileScale = new Vector2(2f, 2f);
				this.MeleeRadius = 225;
				this.ProjectileRadius = 775;
				this.EngageRadius = 925;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Fairy_Miniboss_KnockBack;
				this.NumHits = 1;
				if (LevelEV.WEAKEN_BOSSES)
				{
					this.MaxHealth = 1;
				}
				break;
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				this.m_resetSpriteName = "EnemyFairyGhostBossIdle_Character";
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostMove_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyMove1",
				"FairyMove2",
				"FairyMove3"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new ChaseLogicAction(this.m_target, true, 1f, -1f), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChaseLogicAction(this.m_target, false, 0.5f, -1f), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, 0.75f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.AttackDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				false
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.AttackDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				false
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				false
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				false
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.AttackDelay, false), Types.Sequence.Serial);
			this.ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			this.ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			this.ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			this.ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character", true, true), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyMove1",
				"FairyMove2",
				"FairyMove3"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new ChaseLogicAction(this.m_target, true, 1f, -1f), Types.Sequence.Serial);
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet8.AddAction(new ChaseLogicAction(this.m_target, false, 0.5f, -1f), Types.Sequence.Serial);
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.5f, 0.75f, false), Types.Sequence.Serial);
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet10.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false), Types.Sequence.Serial);
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(this.AttackDelay, false), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false), Types.Sequence.Serial);
			logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character", true, true), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet10.Tag = 2;
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet5
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet6
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet7,
				logicSet8,
				logicSet9,
				logicSet10
			});
			this.m_generalNeoLB.AddLogicSet(new LogicSet[]
			{
				logicSet7,
				logicSet8,
				logicSet9,
				logicSet10
			});
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
				{
					logicSet7,
					logicSet8,
					logicSet9
				});
			}
			else
			{
				this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
				{
					logicSet,
					logicSet2,
					logicSet3
				});
			}
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			this.logicBlocksToDispose.Add(this.m_generalNeoLB);
			LogicBlock arg_975_1 = this.m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 70;
			array[1] = 30;
			base.SetCooldownLogicBlock(arg_975_1, array);
			base.InitializeLogic();
		}
		public void ThrowFourProjectiles(bool useBossProjectile)
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
				projectileData.SpriteName = "GhostProjectileBoss_Sprite";
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"Boss_Flameskull_Roar_01",
					"Boss_Flameskull_Roar_02",
					"Boss_Flameskull_Roar_03"
				});
			}
			else
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyAttack1");
			}
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(90f, 90f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(180f, 180f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(-90f, -90f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Dispose();
		}
		private void ThrowEightProjectiles(LogicSet ls)
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
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(90f, 90f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(180f, 180f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			ls.AddAction(new DelayLogicAction(0.125f, false), Types.Sequence.Serial);
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(135f, 135f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(45f, 45f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-135f, -135f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[2] = 100;
				base.RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = this.m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 100;
				base.RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					50,
					10,
					0,
					40
				});
				return;
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
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = this.m_generalAdvancedLB;
				int[] array = new int[4];
				array[2] = 100;
				base.RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = this.m_generalAdvancedLB;
				int[] array2 = new int[4];
				array2[0] = 100;
				base.RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					50,
					10,
					0,
					40
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
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = this.m_generalExpertLB;
				int[] array = new int[4];
				array[2] = 100;
				base.RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = this.m_generalExpertLB;
				int[] array2 = new int[4];
				array2[0] = 100;
				base.RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					50,
					10,
					0,
					40
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
					bool arg_80_1 = true;
					LogicBlock arg_80_2 = this.m_generalMiniBossLB;
					int[] array = new int[4];
					array[0] = 80;
					array[1] = 20;
					base.RunLogicBlock(arg_80_1, arg_80_2, array);
					return;
				}
				case 1:
				{
					bool arg_60_1 = true;
					LogicBlock arg_60_2 = this.m_generalMiniBossLB;
					int[] array2 = new int[4];
					array2[0] = 100;
					base.RunLogicBlock(arg_60_1, arg_60_2, array2);
					return;
				}
				case 2:
				case 3:
					base.RunLogicBlock(true, this.m_generalMiniBossLB, new int[]
					{
						50,
						10,
						0,
						40
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
					bool arg_10C_1 = true;
					LogicBlock arg_10C_2 = this.m_generalNeoLB;
					int[] array3 = new int[4];
					array3[0] = 80;
					array3[1] = 20;
					base.RunLogicBlock(arg_10C_1, arg_10C_2, array3);
					return;
				}
				case 1:
				{
					bool arg_E8_1 = true;
					LogicBlock arg_E8_2 = this.m_generalNeoLB;
					int[] array4 = new int[4];
					array4[0] = 100;
					base.RunLogicBlock(arg_E8_1, arg_E8_2, array4);
					return;
				}
				case 2:
				case 3:
					base.RunLogicBlock(true, this.m_generalNeoLB, new int[]
					{
						50,
						10,
						0,
						40
					});
					return;
				default:
					return;
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !base.IsPaused)
			{
				if (this.m_summonCounter > 0f)
				{
					this.m_summonCounter -= num;
					if (this.m_summonCounter <= 0f)
					{
						if (this.IsNeo)
						{
							this.m_summonTimer = this.m_summonTimerNeo;
						}
						this.m_summonCounter = this.m_summonTimer;
						this.NumHits--;
						if (!base.IsKilled && this.NumHits <= 0 && this.m_levelScreen.CurrentRoom.ActiveEnemies <= this.m_numSummons + 1)
						{
							if (Game.PlayerStats.TimesCastleBeaten <= 0 || this.IsNeo)
							{
								this.CreateFairy(GameTypes.EnemyDifficulty.BASIC);
								this.CreateFairy(GameTypes.EnemyDifficulty.BASIC);
							}
							else
							{
								this.CreateFairy(GameTypes.EnemyDifficulty.ADVANCED);
								this.CreateFairy(GameTypes.EnemyDifficulty.ADVANCED);
							}
							this.NumHits = 1;
						}
					}
				}
				RoomObj currentRoom = this.m_levelScreen.CurrentRoom;
				Rectangle bounds = this.Bounds;
				Rectangle bounds2 = currentRoom.Bounds;
				int num2 = bounds.Right - bounds2.Right;
				int num3 = bounds.Left - bounds2.Left;
				int num4 = bounds.Bottom - bounds2.Bottom;
				if (num2 > 0)
				{
					base.X -= (float)num2;
				}
				if (num3 < 0)
				{
					base.X -= (float)num3;
				}
				if (num4 > 0)
				{
					base.Y -= (float)num4;
				}
			}
			if (this.m_shake && this.m_shakeTimer > 0f)
			{
				this.m_shakeTimer -= num;
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
			if (this.m_playDeathLoop && this.m_deathLoop != null && !this.m_deathLoop.IsPlaying)
			{
				this.m_deathLoop = SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death_Loop");
			}
			base.Update(gameTime);
		}
		public EnemyObj_Fairy(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyFairyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.PlayAnimation(true);
			this.MainFairy = true;
			this.TintablePart = this._objectList[0];
			this.Type = 7;
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
		private void CreateFairy(GameTypes.EnemyDifficulty difficulty)
		{
			EnemyObj_Fairy enemyObj_Fairy = new EnemyObj_Fairy(null, null, null, difficulty);
			enemyObj_Fairy.Position = base.Position;
			enemyObj_Fairy.DropsItem = false;
			if (this.m_target.X < enemyObj_Fairy.X)
			{
				enemyObj_Fairy.Orientation = MathHelper.ToRadians(0f);
			}
			else
			{
				enemyObj_Fairy.Orientation = MathHelper.ToRadians(180f);
			}
			enemyObj_Fairy.Level = base.Level - 7 - 1;
			this.m_levelScreen.AddEnemyToCurrentRoom(enemyObj_Fairy);
			enemyObj_Fairy.PlayAnimation(true);
			enemyObj_Fairy.MainFairy = false;
			enemyObj_Fairy.SavedStartingPos = enemyObj_Fairy.Position;
			enemyObj_Fairy.SaveToFile = false;
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				enemyObj_Fairy.InitializeDebugRadii();
			}
			enemyObj_Fairy.SpawnRoom = this.m_levelScreen.CurrentRoom;
			enemyObj_Fairy.GivesLichHealth = false;
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			if (!this.m_bossVersionKilled)
			{
				base.HitEnemy(damage, position, isPlayer);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
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
				Game.PlayerStats.FairyBossBeaten = true;
				SoundManager.StopMusic(0f);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "PressStart");
				this.m_bossVersionKilled = true;
				this.m_target.LockControls();
				this.m_levelScreen.PauseScreen();
				this.m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
				this.m_levelScreen.RunWhiteSlashEffect();
				Tween.RunFunction(1f, this, "Part2", new object[0]);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Freeze");
				GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");
			}
		}
		public void Part2()
		{
			this.m_playDeathLoop = true;
			foreach (EnemyObj current in this.m_levelScreen.CurrentRoom.TempEnemyList)
			{
				if (!current.IsKilled)
				{
					current.Kill(true);
				}
			}
			this.m_levelScreen.UnpauseScreen();
			this.m_target.UnlockControls();
			if (this.m_currentActiveLB != null)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			base.PauseEnemy(true);
			this.ChangeSprite("EnemyFairyGhostBossShoot_Character");
			base.PlayAnimation(true);
			this.m_target.CurrentSpeed = 0f;
			this.m_target.ForceInvincible = true;
			if (this.IsNeo)
			{
				this.m_target.InvincibleToSpikes = true;
			}
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
			this.m_playDeathLoop = false;
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death");
			if (this.m_deathLoop != null && this.m_deathLoop.IsPlaying)
			{
				this.m_deathLoop.Stop(AudioStopOptions.Immediate);
			}
			this.m_levelScreen.RunWhiteSlash2();
			base.Kill(true);
		}
		public override void Reset()
		{
			if (!this.MainFairy)
			{
				this.m_levelScreen.RemoveEnemyFromRoom(this, this.SpawnRoom, this.SavedStartingPos);
				this.Dispose();
				return;
			}
			this.NumHits = 1;
			base.Reset();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.SpawnRoom = null;
				if (this.m_deathLoop != null && !this.m_deathLoop.IsDisposed)
				{
					this.m_deathLoop.Dispose();
				}
				this.m_deathLoop = null;
				base.Dispose();
			}
		}
	}
}
