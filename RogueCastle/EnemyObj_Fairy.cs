/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
				return m_isNeo;
			}
			set
			{
				m_isNeo = value;
				if (value)
				{
					HealthGainPerLevel = 0;
					DamageGainPerLevel = 0;
					MoneyDropChance = 0f;
					ItemDropChance = 0f;
					m_saveToEnemiesKilledList = false;
				}
			}
		}
		protected override void InitializeEV()
		{
			Name = "Fury";
			MaxHealth = 27;
			Damage = 18;
			XPValue = 125;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 2;
			MoneyDropChance = 0.4f;
			Speed = 250f;
			TurnSpeed = 0.0325f;
			ProjectileSpeed = 475f;
			JumpHeight = 300f;
			CooldownTime = 1.75f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = true;
			CanBeKnockedBack = true;
			IsWeighted = false;
			Scale = EnemyEV.Fairy_Basic_Scale;
			ProjectileScale = EnemyEV.Fairy_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.Fairy_Basic_Tint;
			MeleeRadius = 225;
			ProjectileRadius = 700;
			EngageRadius = 925;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Fairy_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Rage";
				MaxHealth = 37;
				Damage = 22;
				XPValue = 200;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 265f;
				TurnSpeed = 0.0325f;
				ProjectileSpeed = 475f;
				JumpHeight = 300f;
				CooldownTime = 1.75f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = false;
				Scale = EnemyEV.Fairy_Advanced_Scale;
				ProjectileScale = EnemyEV.Fairy_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Fairy_Advanced_Tint;
				MeleeRadius = 225;
				EngageRadius = 925;
				ProjectileRadius = 700;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Fairy_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Wrath";
				MaxHealth = 72;
				Damage = 24;
				XPValue = 350;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 280f;
				TurnSpeed = 0.0325f;
				ProjectileSpeed = 475f;
				JumpHeight = 300f;
				CooldownTime = 2.5f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = false;
				Scale = EnemyEV.Fairy_Expert_Scale;
				ProjectileScale = EnemyEV.Fairy_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Fairy_Expert_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 700;
				EngageRadius = 925;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Fairy_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Alexander";
				MaxHealth = 635;
				Damage = 30;
				XPValue = 1000;
				MinMoneyDropAmount = 15;
				MaxMoneyDropAmount = 20;
				MoneyDropChance = 1f;
				Speed = 220f;
				TurnSpeed = 0.0325f;
				ProjectileSpeed = 545f;
				JumpHeight = 300f;
				CooldownTime = 3f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = new Vector2(2.5f, 2.5f);
				ProjectileScale = new Vector2(2f, 2f);
				MeleeRadius = 225;
				ProjectileRadius = 775;
				EngageRadius = 925;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Fairy_Miniboss_KnockBack;
				NumHits = 1;
				if (LevelEV.WEAKEN_BOSSES)
				{
					MaxHealth = 1;
				}
				break;
			}
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				m_resetSpriteName = "EnemyFairyGhostBossIdle_Character";
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostMove_Character"));
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyMove1", "FairyMove2", "FairyMove3"));
			logicSet.AddAction(new ChaseLogicAction(m_target, true, 1f));
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet2.AddAction(new ChaseLogicAction(m_target, false, 0.5f));
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet3.AddAction(new DelayLogicAction(0.5f, 0.75f));
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false));
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
			logicSet4.AddAction(new DelayLogicAction(AttackDelay));
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
			logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End"));
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet4.AddAction(new DelayLogicAction(0.25f));
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false));
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
			logicSet5.AddAction(new DelayLogicAction(AttackDelay));
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
			logicSet5.AddAction(new DelayLogicAction(0.15f));
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
			logicSet5.AddAction(new DelayLogicAction(0.15f));
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End"));
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet5.AddAction(new DelayLogicAction(0.25f));
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false));
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
			logicSet6.AddAction(new DelayLogicAction(AttackDelay));
			ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new DelayLogicAction(0.25f));
			ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new DelayLogicAction(0.25f));
			ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new DelayLogicAction(0.25f));
			ThrowEightProjectiles(logicSet6);
			logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End"));
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
			logicSet6.AddAction(new DelayLogicAction(0.25f));
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character"));
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyMove1", "FairyMove2", "FairyMove3"));
			logicSet7.AddAction(new ChaseLogicAction(m_target, true, 1f));
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
			logicSet8.AddAction(new ChaseLogicAction(m_target, false, 0.5f));
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet9.AddAction(new DelayLogicAction(0.5f, 0.75f));
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
			logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false));
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
			logicSet10.AddAction(new DelayLogicAction(AttackDelay));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false));
			logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "End"));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character"));
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
			logicSet10.AddAction(new DelayLogicAction(0.25f));
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
			logicSet10.AddAction(new DelayLogicAction(0.25f));
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
			logicSet10.AddAction(new DelayLogicAction(0.25f));
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
			logicSet10.AddAction(new DelayLogicAction(0.25f));
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
			logicSet10.AddAction(new DelayLogicAction(0.25f));
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
			logicSet10.AddAction(new DelayLogicAction(0.25f));
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
			logicSet10.Tag = 2;
			m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4);
			m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5);
			m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet6);
			m_generalMiniBossLB.AddLogicSet(logicSet7, logicSet8, logicSet9, logicSet10);
			m_generalNeoLB.AddLogicSet(logicSet7, logicSet8, logicSet9, logicSet10);
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				m_generalCooldownLB.AddLogicSet(logicSet7, logicSet8, logicSet9);
			}
			else
			{
				m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);
			}
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalMiniBossLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			logicBlocksToDispose.Add(m_generalNeoLB);
			LogicBlock arg_975_1 = m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 70;
			array[1] = 30;
			SetCooldownLogicBlock(arg_975_1, array);
			base.InitializeLogic();
		}
		public void ThrowFourProjectiles(bool useBossProjectile)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
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
			if (useBossProjectile)
			{
				projectileData.SpriteName = "GhostProjectileBoss_Sprite";
			}
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Roar_01", "Boss_Flameskull_Roar_02", "Boss_Flameskull_Roar_03");
			}
			else
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyAttack1");
			}
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(90f, 90f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(180f, 180f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Angle = new Vector2(-90f, -90f);
			m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Dispose();
		}
		private void ThrowEightProjectiles(LogicSet ls)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
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
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(90f, 90f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(180f, 180f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-90f, -90f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			ls.AddAction(new DelayLogicAction(0.125f));
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
			projectileData.Angle = new Vector2(135f, 135f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(45f, 45f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-45f, -45f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-135f, -135f);
			ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = m_generalBasicLB;
				int[] array = new int[4];
				array[2] = 100;
				RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 100;
				RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, 50, 10, 0, 40);
				return;
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = m_generalAdvancedLB;
				int[] array = new int[4];
				array[2] = 100;
				RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = m_generalAdvancedLB;
				int[] array2 = new int[4];
				array2[0] = 100;
				RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalAdvancedLB, 50, 10, 0, 40);
				return;
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = m_generalExpertLB;
				int[] array = new int[4];
				array[2] = 100;
				RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = m_generalExpertLB;
				int[] array2 = new int[4];
				array2[0] = 100;
				RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalExpertLB, 50, 10, 0, 40);
				return;
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			if (!IsNeo)
			{
				switch (State)
				{
				case 0:
				{
					bool arg_80_1 = true;
					LogicBlock arg_80_2 = m_generalMiniBossLB;
					int[] array = new int[4];
					array[0] = 80;
					array[1] = 20;
					RunLogicBlock(arg_80_1, arg_80_2, array);
					return;
				}
				case 1:
				{
					bool arg_60_1 = true;
					LogicBlock arg_60_2 = m_generalMiniBossLB;
					int[] array2 = new int[4];
					array2[0] = 100;
					RunLogicBlock(arg_60_1, arg_60_2, array2);
					return;
				}
				case 2:
				case 3:
					RunLogicBlock(true, m_generalMiniBossLB, 50, 10, 0, 40);
					return;
				default:
					return;
				}
			}
			else
			{
				switch (State)
				{
				case 0:
				{
					bool arg_10C_1 = true;
					LogicBlock arg_10C_2 = m_generalNeoLB;
					int[] array3 = new int[4];
					array3[0] = 80;
					array3[1] = 20;
					RunLogicBlock(arg_10C_1, arg_10C_2, array3);
					return;
				}
				case 1:
				{
					bool arg_E8_1 = true;
					LogicBlock arg_E8_2 = m_generalNeoLB;
					int[] array4 = new int[4];
					array4[0] = 100;
					RunLogicBlock(arg_E8_1, arg_E8_2, array4);
					return;
				}
				case 2:
				case 3:
					RunLogicBlock(true, m_generalNeoLB, 50, 10, 0, 40);
					return;
				default:
					return;
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS && !IsPaused)
			{
				if (m_summonCounter > 0f)
				{
					m_summonCounter -= num;
					if (m_summonCounter <= 0f)
					{
						if (IsNeo)
						{
							m_summonTimer = m_summonTimerNeo;
						}
						m_summonCounter = m_summonTimer;
						NumHits--;
						if (!IsKilled && NumHits <= 0 && m_levelScreen.CurrentRoom.ActiveEnemies <= m_numSummons + 1)
						{
							if (Game.PlayerStats.TimesCastleBeaten <= 0 || IsNeo)
							{
								CreateFairy(GameTypes.EnemyDifficulty.BASIC);
								CreateFairy(GameTypes.EnemyDifficulty.BASIC);
							}
							else
							{
								CreateFairy(GameTypes.EnemyDifficulty.ADVANCED);
								CreateFairy(GameTypes.EnemyDifficulty.ADVANCED);
							}
							NumHits = 1;
						}
					}
				}
				RoomObj currentRoom = m_levelScreen.CurrentRoom;
				Rectangle bounds = Bounds;
				Rectangle bounds2 = currentRoom.Bounds;
				int num2 = bounds.Right - bounds2.Right;
				int num3 = bounds.Left - bounds2.Left;
				int num4 = bounds.Bottom - bounds2.Bottom;
				if (num2 > 0)
				{
					X -= num2;
				}
				if (num3 < 0)
				{
					X -= num3;
				}
				if (num4 > 0)
				{
					Y -= num4;
				}
			}
			if (m_shake && m_shakeTimer > 0f)
			{
				m_shakeTimer -= num;
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
			if (m_playDeathLoop && m_deathLoop != null && !m_deathLoop.IsPlaying)
			{
				m_deathLoop = SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death_Loop");
			}
			base.Update(gameTime);
		}
		public EnemyObj_Fairy(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyFairyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			PlayAnimation();
			MainFairy = true;
			TintablePart = _objectList[0];
			Type = 7;
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
			if (collisionResponseType != 1)
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
		}
		private void CreateFairy(GameTypes.EnemyDifficulty difficulty)
		{
			EnemyObj_Fairy enemyObj_Fairy = new EnemyObj_Fairy(null, null, null, difficulty);
			enemyObj_Fairy.Position = Position;
			enemyObj_Fairy.DropsItem = false;
			if (m_target.X < enemyObj_Fairy.X)
			{
				enemyObj_Fairy.Orientation = MathHelper.ToRadians(0f);
			}
			else
			{
				enemyObj_Fairy.Orientation = MathHelper.ToRadians(180f);
			}
			enemyObj_Fairy.Level = Level - 7 - 1;
			m_levelScreen.AddEnemyToCurrentRoom(enemyObj_Fairy);
			enemyObj_Fairy.PlayAnimation();
			enemyObj_Fairy.MainFairy = false;
			enemyObj_Fairy.SavedStartingPos = enemyObj_Fairy.Position;
			enemyObj_Fairy.SaveToFile = false;
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				enemyObj_Fairy.InitializeDebugRadii();
			}
			enemyObj_Fairy.SpawnRoom = m_levelScreen.CurrentRoom;
			enemyObj_Fairy.GivesLichHealth = false;
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			if (!m_bossVersionKilled)
			{
				base.HitEnemy(damage, position, isPlayer);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
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
				Game.PlayerStats.FairyBossBeaten = true;
				SoundManager.StopMusic();
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "PressStart");
				m_bossVersionKilled = true;
				m_target.LockControls();
				m_levelScreen.PauseScreen();
				m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
				m_levelScreen.RunWhiteSlashEffect();
				Tween.RunFunction(1f, this, "Part2");
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Freeze");
				GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");
			}
		}
		public void Part2()
		{
			m_playDeathLoop = true;
			foreach (EnemyObj current in m_levelScreen.CurrentRoom.TempEnemyList)
			{
				if (!current.IsKilled)
				{
					current.Kill();
				}
			}
			m_levelScreen.UnpauseScreen();
			m_target.UnlockControls();
			if (m_currentActiveLB != null)
			{
				m_currentActiveLB.StopLogicBlock();
			}
			PauseEnemy(true);
			ChangeSprite("EnemyFairyGhostBossShoot_Character");
			PlayAnimation();
			m_target.CurrentSpeed = 0f;
			m_target.ForceInvincible = true;
			if (IsNeo)
			{
				m_target.InvincibleToSpikes = true;
			}
			Tween.To(m_levelScreen.Camera, 0.5f, Quad.EaseInOut, "X", X.ToString(), "Y", Y.ToString());
			m_shake = true;
			m_shakeTimer = m_shakeDuration;
			for (int i = 0; i < 40; i++)
			{
				Vector2 vector = new Vector2(CDGMath.RandomInt(Bounds.Left, Bounds.Right), CDGMath.RandomInt(Bounds.Top, Bounds.Bottom));
				Tween.RunFunction(i * 0.1f, typeof(SoundManager), "Play3DSound", this, m_target, new[]
				{
				    "Boss_Explo_01",
				    "Boss_Explo_02",
				    "Boss_Explo_03"
				});
				Tween.RunFunction(i * 0.1f, m_levelScreen.ImpactEffectPool, "DisplayExplosionEffect", vector);
			}
			Tween.AddEndHandlerToLastTween(this, "Part3");
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
				CDGMath.Shuffle(list);
				float num = 2.5f / list.Count;
				for (int m = 0; m < list.Count; m++)
				{
					Vector2 position = Position;
					if (list[m] == 0)
					{
						Tween.RunFunction(m * num, m_levelScreen.ItemDropManager, "DropItem", position, 1, 10);
					}
					else if (list[m] == 1)
					{
						Tween.RunFunction(m * num, m_levelScreen.ItemDropManager, "DropItem", position, 10, 100);
					}
					else
					{
						Tween.RunFunction(m * num, m_levelScreen.ItemDropManager, "DropItem", position, 11, 500);
					}
				}
			}
		}
		public void Part3()
		{
			m_playDeathLoop = false;
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death");
			if (m_deathLoop != null && m_deathLoop.IsPlaying)
			{
				m_deathLoop.Stop(AudioStopOptions.Immediate);
			}
			m_levelScreen.RunWhiteSlash2();
			base.Kill();
		}
		public override void Reset()
		{
			if (!MainFairy)
			{
				m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
				Dispose();
				return;
			}
			NumHits = 1;
			base.Reset();
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				SpawnRoom = null;
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
