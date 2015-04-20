/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
				return m_bossVersionKilled;
			}
		}
		public bool IsNeo
		{
			get
			{
				return m_isNeo;
			}
			set
			{
				if (value)
				{
					HealthGainPerLevel = 0;
					DamageGainPerLevel = 0;
					MoneyDropChance = 0f;
					ItemDropChance = 0f;
					m_saveToEnemiesKilledList = false;
				}
				m_isNeo = value;
			}
		}
		protected override void InitializeEV()
		{
			Name = "Scout";
			MaxHealth = 12;
			Damage = 15;
			XPValue = 50;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 1;
			MoneyDropChance = 0.4f;
			Speed = 435f;
			TurnSpeed = 10f;
			ProjectileSpeed = 435f;
			JumpHeight = 950f;
			CooldownTime = 0f;
			AnimationDelay = 0.05f;
			AlwaysFaceTarget = false;
			CanFallOffLedges = false;
			CanBeKnockedBack = false;
			IsWeighted = false;
			Scale = EnemyEV.Eyeball_Basic_Scale;
			ProjectileScale = EnemyEV.Eyeball_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.Eyeball_Basic_Tint;
			MeleeRadius = 325;
			ProjectileRadius = 690;
			EngageRadius = 850;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Eyeball_Basic_KnockBack;
			PupilOffset = 4;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Pupil";
				MaxHealth = 25;
				Damage = 18;
				XPValue = 75;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 435f;
				TurnSpeed = 10f;
				ProjectileSpeed = 435f;
				JumpHeight = 950f;
				CooldownTime = 0f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = false;
				CanFallOffLedges = false;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = EnemyEV.Eyeball_Advanced_Scale;
				ProjectileScale = EnemyEV.Eyeball_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Eyeball_Advanced_Tint;
				MeleeRadius = 325;
				EngageRadius = 850;
				ProjectileRadius = 690;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Eyeball_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Visionary";
				MaxHealth = 57;
				Damage = 21;
				XPValue = 125;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 3;
				MoneyDropChance = 1f;
				Speed = 435f;
				TurnSpeed = 10f;
				ProjectileSpeed = 435f;
				JumpHeight = 950f;
				CooldownTime = 0f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = false;
				CanFallOffLedges = false;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = EnemyEV.Eyeball_Expert_Scale;
				ProjectileScale = EnemyEV.Eyeball_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Eyeball_Expert_Tint;
				MeleeRadius = 325;
				ProjectileRadius = 690;
				EngageRadius = 850;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Eyeball_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Khidr";
				MaxHealth = 580;
				Damage = 23;
				XPValue = 1100;
				MinMoneyDropAmount = 15;
				MaxMoneyDropAmount = 20;
				MoneyDropChance = 1f;
				Speed = 435f;
				TurnSpeed = 10f;
				ProjectileSpeed = 370f;
				JumpHeight = 1350f;
				CooldownTime = 1.9f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = false;
				CanFallOffLedges = false;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = EnemyEV.Eyeball_Miniboss_Scale;
				ProjectileScale = EnemyEV.Eyeball_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Eyeball_Miniboss_Tint;
				MeleeRadius = 325;
				ProjectileRadius = 690;
				EngageRadius = 850;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Eyeball_Miniboss_KnockBack;
				PupilOffset = 0;
				if (LevelEV.WEAKEN_BOSSES)
				{
					MaxHealth = 1;
				}
				break;
			}
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				m_resetSpriteName = "EnemyEyeballBossEye_Character";
			}
		}
		protected override void InitializeLogic()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = m_target,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = ProjectileScale
			};
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
			{
				"Eyeball_ProjectileAttack"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, 3f, false), Types.Sequence.Serial);
			logicSet.Tag = 2;
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.75f, 2f, false), Types.Sequence.Serial);
			logicSet2.Tag = 2;
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
			{
				"EyeballFire1"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			ThrowThreeProjectiles(logicSet3);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, 3f, false), Types.Sequence.Serial);
			logicSet3.Tag = 2;
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
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
			logicSet4.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
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
			logicSet5.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
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
			logicSet6.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "LockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupilFire_Sprite"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(FireballDelay, false), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
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
			logicSet7.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossPupil_Sprite"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball", new object[0]), Types.Sequence.Serial);
			logicSet7.Tag = 2;
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new DelayLogicAction(0.2f, 0.5f, false), Types.Sequence.Serial);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet8
			});
			m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet8
			});
			m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet3,
				logicSet8
			});
			m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet4,
				logicSet6,
				logicSet7,
				logicSet8
			});
			m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet8
			});
			m_generalNeoLB.AddLogicSet(new LogicSet[]
			{
				logicSet5,
				logicSet6,
				logicSet7,
				logicSet8
			});
			logicBlocksToDispose.Add(m_generalNeoLB);
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalMiniBossLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			SetCooldownLogicBlock(m_generalCooldownLB, new int[]
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
				Target = m_target,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = ProjectileScale,
				Angle = new Vector2(0f, 0f)
			};
			for (int i = 0; i <= 3; i++)
			{
				projectileData.AngleOffset = 0f;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = 45f;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = -45f;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
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
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				Scale = ProjectileScale,
				CollidesWithTerrain = false,
				Angle = new Vector2(0f, 0f)
			};
			int num = CDGMath.RandomPlusMinus();
			for (int i = 0; i <= 170; i += 10)
			{
				projectileData.AngleOffset = i * num;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = 90 + i * num;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = 180 + i * num;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
				projectileData.AngleOffset = 270 + i * num;
				ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
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
					Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
					IsWeighted = false,
					RotationSpeed = 0f,
					Damage = Damage,
					AngleOffset = 0f,
					Scale = ProjectileScale,
					CollidesWithTerrain = false,
					Angle = new Vector2(0f, 0f)
				};
				if (randomizeFlipper)
				{
					flipper = CDGMath.RandomPlusMinus();
				}
				projectileData.AngleOffset = -10 + startProjIndex * 10 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = 80 + startProjIndex * 10 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = 170 + startProjIndex * 10 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = 260 + startProjIndex * 10 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
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
					Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
					IsWeighted = false,
					RotationSpeed = 0f,
					Damage = Damage,
					AngleOffset = 0f,
					Scale = ProjectileScale,
					CollidesWithTerrain = false,
					Angle = new Vector2(0f, 0f)
				};
				if (randomizeFlipper)
				{
					flipper = CDGMath.RandomPlusMinus();
				}
				projectileData.AngleOffset = -10 + startProjIndex * 17 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = 80 + startProjIndex * 17 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = 170 + startProjIndex * 17 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileData.AngleOffset = 260 + startProjIndex * 17 * flipper;
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
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
			m_pupil.ChangeSprite("EnemyEyeballBossPupil_Sprite");
			m_pupil.Scale = new Vector2(0.9f, 0.9f);
		}
		public void ThrowSprayProjectiles(bool firstShot)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "EyeballProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = ProjectileScale
			};
			int num = 30;
			for (int i = 0; i <= 360; i += num)
			{
				if (firstShot)
				{
					projectileData.AngleOffset = 10 + i;
				}
				else
				{
					projectileData.AngleOffset = 20 + i;
				}
				m_levelScreen.ProjectileManager.FireProjectile(projectileData);
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
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = ProjectileScale
			};
			projectileData.Angle = new Vector2(0f, 44f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(45f, 89f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(90f, 134f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(135f, 179f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(180f, 224f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(225f, 269f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(270f, 314f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(315f, 359f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
				RunLogicBlock(true, m_generalBasicLB, new int[]
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
				LogicBlock arg_33_2 = m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (State)
			{
			case 0:
				RunLogicBlock(true, m_generalAdvancedLB, new int[]
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
				LogicBlock arg_33_2 = m_generalAdvancedLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (State)
			{
			case 0:
				RunLogicBlock(true, m_generalExpertLB, new int[]
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
				LogicBlock arg_33_2 = m_generalExpertLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			switch (State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			{
				if (!IsNeo)
				{
					bool arg_45_1 = true;
					LogicBlock arg_45_2 = m_generalMiniBossLB;
					int[] array = new int[4];
					array[0] = 40;
					array[1] = 20;
					array[2] = 40;
					RunLogicBlock(arg_45_1, arg_45_2, array);
					return;
				}
				bool arg_6A_1 = false;
				LogicBlock arg_6A_2 = m_generalNeoLB;
				int[] array2 = new int[4];
				array2[0] = 53;
				array2[1] = 12;
				array2[2] = 35;
				RunLogicBlock(arg_6A_1, arg_6A_2, array2);
				return;
			}
			default:
				return;
			}
		}
		public EnemyObj_Eyeball(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyEyeballIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			m_pupil = new SpriteObj("EnemyEyeballPupil_Sprite");
			AddChild(m_pupil);
			m_squishSound = new FrameSoundObj(this, m_target, 2, new string[]
			{
				"Eyeball_Prefire"
			});
			Type = 6;
			DisableCollisionBoxRotations = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (m_playDeathLoop && (m_deathLoop == null || !m_deathLoop.IsPlaying))
			{
				m_deathLoop = SoundManager.PlaySound("Boss_Eyeball_Death_Loop");
			}
			float num = m_target.Y - Y;
			float num2 = m_target.X - X;
			float num3 = (float)Math.Atan2(num, num2);
			m_pupil.X = (float)Math.Cos(num3) * PupilOffset;
			m_pupil.Y = (float)Math.Sin(num3) * PupilOffset;
			if (m_shake && m_shakeTimer > 0f)
			{
				m_shakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (m_shakeTimer <= 0f)
				{
					m_shakeTimer = m_shakeDuration;
					if (m_shookLeft)
					{
						m_shookLeft = false;
						X += 5f;
					}
					else
					{
						X -= 5f;
						m_shookLeft = true;
					}
				}
			}
			m_squishSound.Update();
			base.Update(gameTime);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !m_bossVersionKilled)
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
			if (!m_bossVersionKilled)
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
			if (Difficulty != GameTypes.EnemyDifficulty.MINIBOSS)
			{
				base.Kill(giveXP);
				return;
			}
			if (m_target.CurrentHealth > 0)
			{
				Game.PlayerStats.EyeballBossBeaten = true;
				Tween.StopAllContaining(this, false);
				if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
				{
					m_currentActiveLB.StopLogicBlock();
				}
				SoundManager.StopMusic(0f);
				m_bossVersionKilled = true;
				m_target.LockControls();
				m_levelScreen.PauseScreen();
				m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
				m_levelScreen.RunWhiteSlashEffect();
				SoundManager.PlaySound("Boss_Flash");
				SoundManager.PlaySound("Boss_Eyeball_Freeze");
				Tween.RunFunction(1f, this, "Part2", new object[0]);
				GameUtil.UnlockAchievement("FEAR_OF_EYES");
			}
		}
		public void Part2()
		{
			m_levelScreen.UnpauseScreen();
			m_target.UnlockControls();
			if (m_currentActiveLB != null)
			{
				m_currentActiveLB.StopLogicBlock();
			}
			LockEyeball();
			PauseEnemy(true);
			ChangeSprite("EnemyEyeballBossFire_Character");
			PlayAnimation(true);
			m_target.CurrentSpeed = 0f;
			m_target.ForceInvincible = true;
			if (IsNeo)
			{
				m_target.InvincibleToSpikes = true;
			}
			object arg_106_0 = m_levelScreen.Camera;
			float arg_106_1 = 0.5f;
			Easing arg_106_2 = new Easing(Quad.EaseInOut);
			string[] array = new string[4];
			array[0] = "X";
			string[] arg_CF_0 = array;
			int arg_CF_1 = 1;
			int x = m_levelScreen.CurrentRoom.Bounds.Center.X;
			arg_CF_0[arg_CF_1] = x.ToString();
			array[2] = "Y";
			string[] arg_103_0 = array;
			int arg_103_1 = 3;
			int y = m_levelScreen.CurrentRoom.Bounds.Center.Y;
			arg_103_0[arg_103_1] = y.ToString();
			Tween.To(arg_106_0, arg_106_1, arg_106_2, array);
			m_shake = true;
			m_shakeTimer = m_shakeDuration;
			m_playDeathLoop = true;
			for (int i = 0; i < 40; i++)
			{
				Vector2 vector = new Vector2(CDGMath.RandomInt(Bounds.Left, Bounds.Right), CDGMath.RandomInt(Bounds.Top, Bounds.Bottom));
				Tween.RunFunction(i * 0.1f, typeof(SoundManager), "Play3DSound", new object[]
				{
					this,
					m_target,
					new string[]
					{
						"Boss_Explo_01",
						"Boss_Explo_02",
						"Boss_Explo_03"
					}
				});
				Tween.RunFunction(i * 0.1f, m_levelScreen.ImpactEffectPool, "DisplayExplosionEffect", new object[]
				{
					vector
				});
			}
			Tween.AddEndHandlerToLastTween(this, "Part3", new object[0]);
			if (!IsNeo)
			{
				List<int> list = new List<int>();
				for (int j = 0; j < m_bossCoins; j++)
				{
					list.Add(0);
				}
				for (int k = 0; k < m_bossMoneyBags; k++)
				{
					list.Add(1);
				}
				for (int l = 0; l < m_bossDiamonds; l++)
				{
					list.Add(2);
				}
				CDGMath.Shuffle<int>(list);
				float num = 2.5f / list.Count;
				for (int m = 0; m < list.Count; m++)
				{
					Vector2 vector2 = new Vector2(CDGMath.RandomInt(m_pupil.AbsBounds.Left, m_pupil.AbsBounds.Right), CDGMath.RandomInt(m_pupil.AbsBounds.Top, m_pupil.AbsBounds.Bottom));
					if (list[m] == 0)
					{
						Tween.RunFunction(m * num, m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							vector2,
							1,
							10
						});
					}
					else if (list[m] == 1)
					{
						Tween.RunFunction(m * num, m_levelScreen.ItemDropManager, "DropItem", new object[]
						{
							vector2,
							10,
							100
						});
					}
					else
					{
						Tween.RunFunction(m * num, m_levelScreen.ItemDropManager, "DropItem", new object[]
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
			m_playDeathLoop = false;
			if (m_deathLoop != null && m_deathLoop.IsPlaying)
			{
				m_deathLoop.Stop(AudioStopOptions.Immediate);
			}
			GoToFrame(1);
			StopAnimation();
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
			if (!IsDisposed)
			{
				m_pupil.Dispose();
				m_pupil = null;
				m_squishSound.Dispose();
				m_squishSound = null;
				if (m_deathLoop != null && !m_deathLoop.IsDisposed)
				{
					m_deathLoop.Dispose();
				}
				m_deathLoop = null;
				base.Dispose();
			}
		}
	}
}
