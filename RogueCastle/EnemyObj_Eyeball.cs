using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EnemyObj_Eyeball : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private LogicBlock m_generalNeoLB = new LogicBlock();
		private SpriteObj m_pupil;
		private FrameSoundObj m_squishSound;
		private bool m_shake;
		private bool m_shookLeft;
		private float FireballDelay = 0.5f;
		private float m_shakeTimer;
		private float m_shakeDuration = 0.03f;
		private int m_bossCoins = 30;
		private int m_bossMoneyBags = 7;
		private int m_bossDiamonds = 1;
		private Cue m_deathLoop;
		private bool m_playDeathLoop;
		private bool m_isNeo;
		public int PupilOffset
		{
			get;
			set;
		}
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
				if (value)
				{
					this.HealthGainPerLevel = 0;
					this.DamageGainPerLevel = 0;
					this.MoneyDropChance = 0f;
					this.ItemDropChance = 0f;
					this.m_saveToEnemiesKilledList = false;
				}
				this.m_isNeo = value;
			}
		}
		protected override void InitializeEV()
		{
			base.Name = "Scout";
			this.MaxHealth = 12;
			base.Damage = 15;
			base.XPValue = 50;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.4f;
			base.Speed = 435f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 435f;
			base.JumpHeight = 950f;
			this.CooldownTime = 0f;
			base.AnimationDelay = 0.05f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			base.IsWeighted = false;
			this.Scale = EnemyEV.Eyeball_Basic_Scale;
			base.ProjectileScale = EnemyEV.Eyeball_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Eyeball_Basic_Tint;
			this.MeleeRadius = 325;
			this.ProjectileRadius = 690;
			this.EngageRadius = 850;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Eyeball_Basic_KnockBack;
			this.PupilOffset = 4;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Pupil";
				this.MaxHealth = 25;
				base.Damage = 18;
				base.XPValue = 75;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 435f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 435f;
				base.JumpHeight = 950f;
				this.CooldownTime = 0f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Eyeball_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Eyeball_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Eyeball_Advanced_Tint;
				this.MeleeRadius = 325;
				this.EngageRadius = 850;
				this.ProjectileRadius = 690;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Eyeball_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Visionary";
				this.MaxHealth = 57;
				base.Damage = 21;
				base.XPValue = 125;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 435f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 435f;
				base.JumpHeight = 950f;
				this.CooldownTime = 0f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Eyeball_Expert_Scale;
				base.ProjectileScale = EnemyEV.Eyeball_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Eyeball_Expert_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 690;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Eyeball_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Khidr";
				this.MaxHealth = 580;
				base.Damage = 23;
				base.XPValue = 1100;
				this.MinMoneyDropAmount = 15;
				this.MaxMoneyDropAmount = 20;
				this.MoneyDropChance = 1f;
				base.Speed = 435f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 370f;
				base.JumpHeight = 1350f;
				this.CooldownTime = 1.9f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Eyeball_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Eyeball_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Eyeball_Miniboss_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 690;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Eyeball_Miniboss_KnockBack;
				this.PupilOffset = 0;
				if (LevelEV.WEAKEN_BOSSES)
				{
					this.MaxHealth = 1;
				}
				break;
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				this.m_resetSpriteName = "EnemyEyeballBossEye_Character";
			}
		}
		protected override void InitializeLogic()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = this.m_target,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale
			};
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Eyeball_ProjectileAttack"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, 3f, false), Types.Sequence.Serial);
			logicSet.Tag = 2;
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.75f, 2f, false), Types.Sequence.Serial);
			logicSet2.Tag = 2;
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			this.ThrowThreeProjectiles(logicSet3);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, 3f, false), Types.Sequence.Serial);
			logicSet3.Tag = 2;
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ThrowCardinalProjectiles", new object[]
			{
				0,
				true,
				0
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(3.15f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowCardinalProjectilesNeo", new object[]
			{
				0,
				true,
				0
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(3f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ThrowSprayProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(1.6f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ThrowSprayProjectiles", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(1.6f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.575f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.575f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.575f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.575f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.575f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this.m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet7.Tag = 2;
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new DelayLogicAction(0.2f, 0.5f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet8
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet8
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet3,
				logicSet8
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet4,
				logicSet6,
				logicSet7,
				logicSet8
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet8
			});
			this.m_generalNeoLB.AddLogicSet(new LogicSet[]
			{
				logicSet5,
				logicSet6,
				logicSet7,
				logicSet8
			});
			this.logicBlocksToDispose.Add(this.m_generalNeoLB);
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
			{
				100
			});
			projectileData.Dispose();
			base.InitializeLogic();
		}
		private void ThrowThreeProjectiles(LogicSet ls)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = this.m_target,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale,
				Angle = new Vector2(0f, 0f)
			};
			for (int i = 0; i <= 3; i++)
			{
				projectileData.AngleOffset = 0f;
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = 45f;
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = -45f;
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				ls.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			}
			projectileData.Dispose();
		}
		private void ThrowCardinalProjectiles(LogicSet ls)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Scale = base.ProjectileScale,
				CollidesWithTerrain = false,
				Angle = new Vector2(0f, 0f)
			};
			int num = CDGMath.RandomPlusMinus();
			for (int i = 0; i <= 170; i += 10)
			{
				projectileData.AngleOffset = (float)(i * num);
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = (float)(90 + i * num);
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = (float)(180 + i * num);
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = (float)(270 + i * num);
				ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				ls.AddAction(new DelayLogicAction(0.175f, false), Types.Sequence.Serial);
			}
			projectileData.Dispose();
		}
		public void ThrowCardinalProjectiles(int startProjIndex, bool randomizeFlipper, int flipper)
		{
			if (startProjIndex < 17)
			{
				ProjectileData projectileData = new ProjectileData(this)
				{
					SpriteName = "EyeballProjectile_Sprite",
					SourceAnchor = Vector2.Zero,
					Target = null,
					Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
					IsWeighted = false,
					RotationSpeed = 0f,
					Damage = base.Damage,
					AngleOffset = 0f,
					Scale = base.ProjectileScale,
					CollidesWithTerrain = false,
					Angle = new Vector2(0f, 0f)
				};
				if (randomizeFlipper)
				{
					flipper = CDGMath.RandomPlusMinus();
				}
				projectileData.AngleOffset = (float)(-10 + startProjIndex * 10 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = (float)(80 + startProjIndex * 10 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = (float)(170 + startProjIndex * 10 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = (float)(260 + startProjIndex * 10 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.Dispose();
				startProjIndex++;
				Tween.RunFunction(0.12f, this, "ThrowCardinalProjectiles", new object[]
				{
					startProjIndex,
					false,
					flipper
				});
			}
		}
		public void ThrowCardinalProjectilesNeo(int startProjIndex, bool randomizeFlipper, int flipper)
		{
			if (startProjIndex < 13)
			{
				ProjectileData projectileData = new ProjectileData(this)
				{
					SpriteName = "EyeballProjectile_Sprite",
					SourceAnchor = Vector2.Zero,
					Target = null,
					Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
					IsWeighted = false,
					RotationSpeed = 0f,
					Damage = base.Damage,
					AngleOffset = 0f,
					Scale = base.ProjectileScale,
					CollidesWithTerrain = false,
					Angle = new Vector2(0f, 0f)
				};
				if (randomizeFlipper)
				{
					flipper = CDGMath.RandomPlusMinus();
				}
				projectileData.AngleOffset = (float)(-10 + startProjIndex * 17 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = (float)(80 + startProjIndex * 17 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = (float)(170 + startProjIndex * 17 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = (float)(260 + startProjIndex * 17 * flipper);
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.Dispose();
				startProjIndex++;
				Tween.RunFunction(0.12f, this, "ThrowCardinalProjectilesNeo", new object[]
				{
					startProjIndex,
					false,
					flipper
				});
			}
		}
		public void LockEyeball()
		{
			Tween.To(this, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"PupilOffset",
				"0"
			});
		}
		public void UnlockEyeball()
		{
			Tween.To(this, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"PupilOffset",
				"30"
			});
		}
		public void ChangeToBossPupil()
		{
			this.m_pupil.ChangeSprite("EnemyEyeballBossPupil_Sprite");
			this.m_pupil.Scale = new Vector2(0.9f, 0.9f);
		}
		public void ThrowSprayProjectiles(bool firstShot)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
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
			int num = 30;
			for (int i = 0; i <= 360; i += num)
			{
				if (firstShot)
				{
					projectileData.AngleOffset = (float)(10 + i);
				}
				else
				{
					projectileData.AngleOffset = (float)(20 + i);
				}
				this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			}
			if (firstShot)
			{
				Tween.RunFunction(0.8f, this, "ThrowSprayProjectiles", new object[]
				{
					false
				});
			}
			projectileData.Dispose();
		}
		public void ThrowRandomProjectiles()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
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
			projectileData.Angle = new Vector2(0f, 44f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(45f, 89f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(90f, 134f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(135f, 179f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(180f, 224f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(225f, 269f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(270f, 314f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(315f, 359f);
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
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
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalAdvancedLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalExpertLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			{
				if (!this.IsNeo)
				{
					bool arg_45_1 = true;
					LogicBlock arg_45_2 = this.m_generalMiniBossLB;
					int[] array = new int[4];
					array[0] = 40;
					array[1] = 20;
					array[2] = 40;
					base.RunLogicBlock(arg_45_1, arg_45_2, array);
					return;
				}
				bool arg_6A_1 = false;
				LogicBlock arg_6A_2 = this.m_generalNeoLB;
				int[] array2 = new int[4];
				array2[0] = 53;
				array2[1] = 12;
				array2[2] = 35;
				base.RunLogicBlock(arg_6A_1, arg_6A_2, array2);
				return;
			}
			default:
				return;
			}
		}
		public EnemyObj_Eyeball(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyEyeballIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.m_pupil = new SpriteObj("EnemyEyeballPupil_Sprite");
			this.AddChild(this.m_pupil);
			this.m_squishSound = new FrameSoundObj(this, this.m_target, 2, new string[]
			{
				"Eyeball_Prefire"
			});
			this.Type = 6;
			base.DisableCollisionBoxRotations = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_playDeathLoop && (this.m_deathLoop == null || !this.m_deathLoop.IsPlaying))
			{
				this.m_deathLoop = SoundManager.PlaySound("Boss_Eyeball_Death_Loop");
			}
			float num = this.m_target.Y - base.Y;
			float num2 = this.m_target.X - base.X;
			float num3 = (float)Math.Atan2((double)num, (double)num2);
			this.m_pupil.X = (float)Math.Cos((double)num3) * (float)this.PupilOffset;
			this.m_pupil.Y = (float)Math.Sin((double)num3) * (float)this.PupilOffset;
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
			this.m_squishSound.Update();
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
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			if (!this.m_bossVersionKilled)
			{
				SoundManager.PlaySound(new string[]
				{
					"EyeballSquish1",
					"EyeballSquish2",
					"EyeballSquish3"
				});
				base.HitEnemy(damage, position, isPlayer);
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
				Game.PlayerStats.EyeballBossBeaten = true;
				Tween.StopAllContaining(this, false);
				if (this.m_currentActiveLB != null && this.m_currentActiveLB.IsActive)
				{
					this.m_currentActiveLB.StopLogicBlock();
				}
				SoundManager.StopMusic(0f);
				this.m_bossVersionKilled = true;
				this.m_target.LockControls();
				this.m_levelScreen.PauseScreen();
				this.m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
				this.m_levelScreen.RunWhiteSlashEffect();
				SoundManager.PlaySound("Boss_Flash");
				SoundManager.PlaySound("Boss_Eyeball_Freeze");
				Tween.RunFunction(1f, this, "Part2", new object[0]);
				GameUtil.UnlockAchievement("FEAR_OF_EYES");
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
			this.LockEyeball();
			base.PauseEnemy(true);
			this.ChangeSprite("EnemyEyeballBossFire_Character");
			base.PlayAnimation(true);
			this.m_target.CurrentSpeed = 0f;
			this.m_target.ForceInvincible = true;
			if (this.IsNeo)
			{
				this.m_target.InvincibleToSpikes = true;
			}
			object arg_106_0 = this.m_levelScreen.Camera;
			float arg_106_1 = 0.5f;
			Easing arg_106_2 = new Easing(Quad.EaseInOut);
			string[] array = new string[4];
			array[0] = "X";
			string[] arg_CF_0 = array;
			int arg_CF_1 = 1;
			int x = this.m_levelScreen.CurrentRoom.Bounds.Center.X;
			arg_CF_0[arg_CF_1] = x.ToString();
			array[2] = "Y";
			string[] arg_103_0 = array;
			int arg_103_1 = 3;
			int y = this.m_levelScreen.CurrentRoom.Bounds.Center.Y;
			arg_103_0[arg_103_1] = y.ToString();
			Tween.To(arg_106_0, arg_106_1, arg_106_2, array);
			this.m_shake = true;
			this.m_shakeTimer = this.m_shakeDuration;
			this.m_playDeathLoop = true;
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
					Vector2 vector2 = new Vector2((float)CDGMath.RandomInt(this.m_pupil.AbsBounds.Left, this.m_pupil.AbsBounds.Right), (float)CDGMath.RandomInt(this.m_pupil.AbsBounds.Top, this.m_pupil.AbsBounds.Bottom));
					if (list[m] == 0)
					{
						Tween.RunFunction((float)m * num, this.m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							vector2,
							1,
							10
						});
					}
					else if (list[m] == 1)
					{
						Tween.RunFunction((float)m * num, this.m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							vector2,
							10,
							100
						});
					}
					else
					{
						Tween.RunFunction((float)m * num, this.m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							vector2,
							11,
							500
						});
					}
				}
			}
		}
		public void Part3()
		{
			SoundManager.PlaySound("Boss_Eyeball_Death");
			this.m_playDeathLoop = false;
			if (this.m_deathLoop != null && this.m_deathLoop.IsPlaying)
			{
				this.m_deathLoop.Stop(AudioStopOptions.Immediate);
			}
			base.GoToFrame(1);
			base.StopAnimation();
			Tween.To(this, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"Rotation",
				"-1080"
			});
			Tween.To(this, 2f, new Easing(Quad.EaseInOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
			Tween.AddEndHandlerToLastTween(this, "DeathComplete", new object[0]);
		}
		public void DeathComplete()
		{
			SoundManager.PlaySound(new string[]
			{
				"Boss_Explo_01",
				"Boss_Explo_02",
				"Boss_Explo_03"
			});
			base.Kill(true);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_pupil.Dispose();
				this.m_pupil = null;
				this.m_squishSound.Dispose();
				this.m_squishSound = null;
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
