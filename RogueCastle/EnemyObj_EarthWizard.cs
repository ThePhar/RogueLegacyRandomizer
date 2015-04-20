/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class EnemyObj_EarthWizard : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private float SpellDelay = 0.3f;
		private float SpellDuration = 0.75f;
		private int SpellIceProjectileCount = 24;
		private float SpellFireDelay = 1.5f;
		private float SpellFireInterval = 0.2f;
		private Vector2 MiniBossFireballSize = new Vector2(2f, 2f);
		private Vector2 MiniBossIceSize = new Vector2(1.5f, 1.5f);
		private ProjectileObj m_fireballSummon;
		private ProjectileObj m_iceballSummon;
		private SpriteObj m_earthSummonInSprite;
		private SpriteObj m_earthSummonOutSprite;
		private ProjectileObj m_earthProjectileObj;
		public RoomObj SpawnRoom;
		private Vector2 m_spellOffset = new Vector2(40f, -80f);
		private float TeleportDelay = 0.5f;
		private float TeleportDuration = 1f;
		private float MoveDuration = 1f;
		private float m_earthParticleEffectCounter = 0.5f;
		private int m_effectCycle;
		public Vector2 SavedStartingPos
		{
			get;
			set;
		}
		public SpriteObj EarthProjectile
		{
			get
			{
				return m_earthProjectileObj;
			}
		}
		protected override void InitializeEV()
		{
			Name = "Earthsor";
			MaxHealth = 32;
			Damage = 20;
			XPValue = 175;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 2;
			MoneyDropChance = 0.4f;
			Speed = 270f;
			TurnSpeed = 0.04f;
			ProjectileSpeed = 650f;
			JumpHeight = 300f;
			CooldownTime = 1.25f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = true;
			CanBeKnockedBack = true;
			IsWeighted = false;
			Scale = EnemyEV.EarthWizard_Basic_Scale;
			ProjectileScale = EnemyEV.EarthWizard_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.EarthWizard_Basic_Tint;
			MeleeRadius = 225;
			ProjectileRadius = 700;
			EngageRadius = 900;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.EarthWizard_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				SpellDelay = 0.5f;
				SpellDuration = 1f;
				Name = "Gravisor";
				MaxHealth = 45;
				Damage = 24;
				XPValue = 200;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 270f;
				TurnSpeed = 0.04f;
				ProjectileSpeed = 650f;
				JumpHeight = 300f;
				CooldownTime = 1.25f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = false;
				Scale = EnemyEV.EarthWizard_Advanced_Scale;
				ProjectileScale = EnemyEV.EarthWizard_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.EarthWizard_Advanced_Tint;
				MeleeRadius = 225;
				EngageRadius = 900;
				ProjectileRadius = 700;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.EarthWizard_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				SpellDelay = 0.7f;
				SpellDuration = 3.5f;
				Name = "Terrasor";
				MaxHealth = 72;
				Damage = 27;
				XPValue = 400;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 3;
				MoneyDropChance = 1f;
				Speed = 300f;
				TurnSpeed = 0.04f;
				ProjectileSpeed = 650f;
				JumpHeight = 300f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = false;
				Scale = EnemyEV.EarthWizard_Expert_Scale;
				ProjectileScale = EnemyEV.EarthWizard_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.EarthWizard_Expert_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 700;
				EngageRadius = 900;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.EarthWizard_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				SpellDelay = 0.85f;
				SpellDuration = 2f;
				m_spellOffset = new Vector2(40f, -140f);
				Name = "Barbatos  & Amon";
				MaxHealth = 225;
				Damage = 30;
				XPValue = 1000;
				MinMoneyDropAmount = 18;
				MaxMoneyDropAmount = 25;
				MoneyDropChance = 1f;
				Speed = 225f;
				TurnSpeed = 0.04f;
				ProjectileSpeed = 650f;
				JumpHeight = 300f;
				CooldownTime = 0.75f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = false;
				Scale = EnemyEV.EarthWizard_Miniboss_Scale;
				ProjectileScale = EnemyEV.EarthWizard_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.EarthWizard_Miniboss_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 700;
				EngageRadius = 900;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.EarthWizard_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		public void PublicInitializeEV()
		{
			InitializeEV();
		}
		protected override void InitializeLogic()
		{
			InitializeProjectiles();
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new ChaseLogicAction(m_target, new Vector2(-255f, -175f), new Vector2(255f, -75f), true, MoveDuration, -1f), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChaseLogicAction(m_target, false, 1f, -1f), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CancelEarthSpell", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpellIn", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpellOut", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(SpellDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("CastSpell", "End", false), Types.Sequence.Parallel);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CastEarthSpell", new object[]
			{
				SpellDuration
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "CancelEarthSpellIn", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(SpellFireInterval, false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "CastFireball", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell", false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(SpellDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ShatterIceball", new object[]
			{
				SpellIceProjectileCount
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("CastSpell", "End", false), Types.Sequence.Parallel);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(TeleportDelay, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("TeleportStart", "End", false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(TeleportDuration, false), Types.Sequence.Serial);
			logicSet7.AddAction(new TeleportLogicAction(m_target, new Vector2(-400f, -400f), new Vector2(400f, 400f)), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5,
				logicSet6,
				logicSet7
			});
			m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalMiniBossLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			LogicBlock arg_971_1 = m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 100;
			SetCooldownLogicBlock(arg_971_1, array);
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = m_generalBasicLB;
				int[] array3 = new int[5];
				array3[0] = 40;
				array3[3] = 60;
				RunLogicBlock(arg_38_1, arg_38_2, array3);
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
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = m_generalBasicLB;
				int[] array3 = new int[5];
				array3[0] = 40;
				array3[3] = 60;
				RunLogicBlock(arg_38_1, arg_38_2, array3);
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
			{
				bool arg_6E_1 = true;
				LogicBlock arg_6E_2 = m_generalBasicLB;
				int[] array = new int[5];
				array[2] = 100;
				RunLogicBlock(arg_6E_1, arg_6E_2, array);
				return;
			}
			case 1:
			{
				bool arg_53_1 = true;
				LogicBlock arg_53_2 = m_generalBasicLB;
				int[] array2 = new int[5];
				array2[0] = 100;
				RunLogicBlock(arg_53_1, arg_53_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = m_generalBasicLB;
				int[] array3 = new int[5];
				array3[0] = 40;
				array3[3] = 60;
				RunLogicBlock(arg_38_1, arg_38_2, array3);
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
			{
				bool arg_82_1 = true;
				LogicBlock arg_82_2 = m_generalMiniBossLB;
				int[] array = new int[7];
				array[0] = 60;
				array[1] = 10;
				array[2] = 30;
				RunLogicBlock(arg_82_1, arg_82_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = m_generalMiniBossLB;
				int[] array2 = new int[7];
				array2[0] = 100;
				RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
			{
				bool arg_42_1 = true;
				LogicBlock arg_42_2 = m_generalMiniBossLB;
				int[] array3 = new int[7];
				array3[0] = 34;
				array3[3] = 22;
				array3[4] = 22;
				array3[5] = 22;
				RunLogicBlock(arg_42_1, arg_42_2, array3);
				return;
			}
			default:
				return;
			}
		}
		public EnemyObj_EarthWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			PlayAnimation(true);
			TintablePart = _objectList[0];
			Type = 5;
		}
		private void InitializeProjectiles()
		{
			m_earthSummonInSprite = new ProjectileObj("WizardEarthSpellCast_Sprite");
			m_earthSummonInSprite.AnimationDelay = 0.1f;
			m_earthSummonInSprite.PlayAnimation(true);
			m_earthSummonInSprite.Scale = Vector2.Zero;
			m_earthSummonOutSprite = (m_earthSummonInSprite.Clone() as SpriteObj);
			m_earthSummonOutSprite.PlayAnimation(true);
			m_earthProjectileObj = new ProjectileObj("WizardEarthSpell_Sprite");
			m_earthProjectileObj.IsWeighted = false;
			m_earthProjectileObj.CollidesWithTerrain = false;
			m_earthProjectileObj.DestroysWithEnemy = false;
			m_earthProjectileObj.Damage = Damage;
			m_earthProjectileObj.Scale = ProjectileScale;
			m_earthProjectileObj.AnimationDelay = 0.05f;
			m_earthProjectileObj.Rotation = 0f;
			m_earthProjectileObj.CanBeFusRohDahed = false;
		}
		public void CancelEarthSpell()
		{
			Tween.StopAllContaining(m_earthSummonOutSprite, false);
			Tween.StopAllContaining(this, false);
			Tween.To(m_earthSummonOutSprite, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			if (m_earthProjectileObj.CurrentFrame != 1 && m_earthProjectileObj.CurrentFrame != m_earthProjectileObj.TotalFrames)
			{
				SoundManager.Play3DSound(this, m_target, "Earth_Wizard_Fall");
				m_earthProjectileObj.PlayAnimation("Grown", "End", false);
			}
			m_levelScreen.PhysicsManager.RemoveObject(m_earthProjectileObj);
		}
		public void CancelEarthSpellIn()
		{
			Tween.StopAllContaining(m_earthSummonInSprite, false);
			Tween.To(m_earthSummonInSprite, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
		}
		public void CastEarthSpellIn()
		{
			SoundManager.Play3DSound(this, m_target, "Earth_Wizard_Form");
			m_earthSummonInSprite.Scale = Vector2.Zero;
			Tween.To(m_earthSummonInSprite, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
		}
		public void CastEarthSpellOut()
		{
			m_earthSummonOutSprite.Scale = Vector2.Zero;
			m_earthSummonOutSprite.X = m_target.X;
			int num = 2147483647;
			TerrainObj terrainObj = null;
			foreach (TerrainObj current in m_levelScreen.CurrentRoom.TerrainObjList)
			{
				if (CollisionMath.Intersects(new Rectangle((int)m_target.X, (int)m_target.Y, 2, 720), current.Bounds))
				{
					int num2 = current.Bounds.Top - m_target.TerrainBounds.Bottom;
					if (num2 < num)
					{
						num = num2;
						terrainObj = current;
					}
				}
			}
			if (terrainObj != null)
			{
				if (terrainObj.Rotation == 0f)
				{
					m_earthSummonOutSprite.Y = terrainObj.Bounds.Top;
				}
				else
				{
					Vector2 vector;
					Vector2 vector2;
					if (terrainObj.Width > terrainObj.Height)
					{
						vector = CollisionMath.UpperLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
						vector2 = CollisionMath.UpperRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
					}
					else if (terrainObj.Rotation > 0f)
					{
						vector = CollisionMath.LowerLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
						vector2 = CollisionMath.UpperLeftCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
					}
					else
					{
						vector = CollisionMath.UpperRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
						vector2 = CollisionMath.LowerRightCorner(terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero);
					}
					float num3 = vector2.X - vector.X;
					float num4 = vector2.Y - vector.Y;
					float x = vector.X;
					float y = vector.Y;
					float x2 = m_earthSummonOutSprite.X;
					float num5 = y + (x2 - x) * (num4 / num3);
					num5 -= m_earthSummonOutSprite.Bounds.Bottom - m_earthSummonOutSprite.Y;
					m_earthSummonOutSprite.Y = (float)Math.Round(num5, MidpointRounding.ToEven);
				}
			}
			object arg_2A0_0 = m_earthSummonOutSprite;
			float arg_2A0_1 = 0.5f;
			Easing arg_2A0_2 = new Easing(Back.EaseOut);
			string[] array = new string[6];
			array[0] = "Opacity";
			array[1] = "1";
			array[2] = "ScaleX";
			string[] arg_28B_0 = array;
			int arg_28B_1 = 3;
			float x3 = ProjectileScale.X;
			arg_28B_0[arg_28B_1] = x3.ToString();
			array[4] = "ScaleY";
			array[5] = "1";
			Tween.To(arg_2A0_0, arg_2A0_1, arg_2A0_2, array);
		}
		public void CastEarthSpell(float duration)
		{
			m_levelScreen.PhysicsManager.AddObject(m_earthProjectileObj);
			m_earthProjectileObj.Scale = ProjectileScale;
			m_earthProjectileObj.StopAnimation();
			m_earthProjectileObj.Position = m_earthSummonOutSprite.Position;
			m_earthProjectileObj.PlayAnimation("Start", "Grown", false);
			SoundManager.Play3DSound(this, m_target, "Earth_Wizard_Attack");
			Tween.RunFunction(duration, this, "CancelEarthSpell", new object[0]);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Flip == SpriteEffects.None)
			{
				m_earthSummonInSprite.Position = new Vector2(X + m_spellOffset.X, Y + m_spellOffset.Y);
			}
			else
			{
				m_earthSummonInSprite.Position = new Vector2(X - m_spellOffset.X, Y + m_spellOffset.Y);
			}
			if (m_fireballSummon != null)
			{
				if (Flip == SpriteEffects.None)
				{
					m_fireballSummon.Position = new Vector2(X + m_spellOffset.X, Y + m_spellOffset.Y);
				}
				else
				{
					m_fireballSummon.Position = new Vector2(X - m_spellOffset.X, Y + m_spellOffset.Y);
				}
			}
			if (m_iceballSummon != null)
			{
				if (Flip == SpriteEffects.None)
				{
					m_iceballSummon.Position = new Vector2(X + m_spellOffset.X, Y + m_spellOffset.Y);
				}
				else
				{
					m_iceballSummon.Position = new Vector2(X - m_spellOffset.X, Y + m_spellOffset.Y);
				}
			}
			if (m_earthParticleEffectCounter > 0f)
			{
				m_earthParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (m_earthParticleEffectCounter <= 0f)
				{
					if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
					{
						if (m_effectCycle == 0)
						{
							m_levelScreen.ImpactEffectPool.DisplayEarthParticleEffect(this);
						}
						else if (m_effectCycle == 1)
						{
							m_levelScreen.ImpactEffectPool.DisplayFireParticleEffect(this);
						}
						else
						{
							m_levelScreen.ImpactEffectPool.DisplayIceParticleEffect(this);
						}
						m_effectCycle++;
						if (m_effectCycle > 2)
						{
							m_effectCycle = 0;
						}
					}
					else
					{
						m_levelScreen.ImpactEffectPool.DisplayEarthParticleEffect(this);
					}
					m_earthParticleEffectCounter = 0.15f;
				}
			}
		}
		public void CastFireball()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardFireballProjectile_Sprite",
				SourceAnchor = m_spellOffset,
				Target = m_target,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = MiniBossFireballSize
			};
			if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				projectileData.AngleOffset = CDGMath.RandomInt(-25, 25);
			}
			if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
			}
			SoundManager.Play3DSound(this, m_target, new string[]
			{
				"FireWizard_Attack_01",
				"FireWizard_Attack_02",
				"FireWizard_Attack_03",
				"FireWizard_Attack_04"
			});
			ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileObj.Rotation = 0f;
			Tween.RunFunction(0.15f, this, "ChangeFireballState", new object[]
			{
				projectileObj
			});
		}
		public void ChangeFireballState(ProjectileObj fireball)
		{
			fireball.CollidesWithTerrain = true;
		}
		public void SummonFireball()
		{
			ResetFireball();
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardFireballProjectile_Sprite",
				SourceAnchor = m_spellOffset,
				Target = m_target,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = MiniBossFireballSize
			};
			if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				projectileData.SpriteName = "GhostBossProjectile_Sprite";
			}
			SoundManager.Play3DSound(this, m_target, "Fire_Wizard_Form");
			m_fireballSummon = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			m_fireballSummon.Opacity = 0f;
			m_fireballSummon.Scale = Vector2.Zero;
			m_fireballSummon.AnimationDelay = 0.1f;
			m_fireballSummon.PlayAnimation(true);
			m_fireballSummon.Rotation = 0f;
			Tween.To(m_fireballSummon, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Opacity",
				"1",
				"ScaleX",
				MiniBossFireballSize.X.ToString(),
				"ScaleY",
				MiniBossFireballSize.Y.ToString()
			});
			projectileData.Dispose();
		}
		public void ResetFireball()
		{
			if (m_fireballSummon != null)
			{
				m_levelScreen.ProjectileManager.DestroyProjectile(m_fireballSummon);
				m_fireballSummon = null;
			}
		}
		public void SummonIceball()
		{
			ResetIceball();
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardIceSpell_Sprite",
				SourceAnchor = m_spellOffset,
				Target = null,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = MiniBossIceSize
			};
			SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Form");
			m_iceballSummon = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			m_iceballSummon.PlayAnimation("Start", "Grown", false);
			projectileData.Dispose();
		}
		public void ShatterIceball(int numIceballs)
		{
			SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Attack_Glass");
			if (m_iceballSummon.SpriteName == "WizardIceSpell_Sprite")
			{
				m_iceballSummon.PlayAnimation("Grown", "End", false);
			}
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "WizardIceProjectile_Sprite",
				SourceAnchor = m_spellOffset,
				Target = null,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = MiniBossIceSize
			};
			float num = 0f;
			float num2 = 360 / numIceballs;
			for (int i = 0; i < numIceballs; i++)
			{
				projectileData.Angle = new Vector2(num, num);
				ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				Tween.RunFunction(0.15f, this, "ChangeIceballState", new object[]
				{
					projectileObj
				});
				num += num2;
			}
			projectileData.Dispose();
		}
		public void ChangeIceballState(ProjectileObj iceball)
		{
			iceball.CollidesWithTerrain = true;
		}
		public void ResetIceball()
		{
			if (m_iceballSummon != null)
			{
				m_levelScreen.ProjectileManager.DestroyProjectile(m_iceballSummon);
				m_iceballSummon = null;
			}
		}
		public override void Kill(bool giveXP = true)
		{
			if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
			{
				CancelEarthSpell();
				CancelEarthSpellIn();
				m_currentActiveLB.StopLogicBlock();
			}
			base.Kill(giveXP);
		}
		public override void Draw(Camera2D camera)
		{
			m_earthSummonInSprite.Draw(camera);
			m_earthSummonOutSprite.Draw(camera);
			m_earthProjectileObj.Draw(camera);
			base.Draw(camera);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (otherBox.AbsParent is PlayerObj)
			{
				CurrentSpeed = 0f;
			}
			if (collisionResponseType != 1)
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
				return;
			}
			if (!(otherBox.AbsParent is PlayerObj))
			{
				IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
				if (physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.CollidesLeft && physicsObj.CollidesRight)
				{
					Position += CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
				}
			}
		}
		public override void ResetState()
		{
			Tween.StopAllContaining(this, false);
			Tween.StopAllContaining(m_earthSummonOutSprite, false);
			Tween.StopAllContaining(m_earthSummonInSprite, false);
			m_earthSummonInSprite.Scale = Vector2.Zero;
			m_earthSummonOutSprite.Scale = Vector2.Zero;
			m_earthProjectileObj.StopAnimation();
			m_earthProjectileObj.GoToFrame(m_earthProjectileObj.TotalFrames);
			ResetFireball();
			ResetIceball();
			base.ResetState();
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_fireballSummon = null;
				m_iceballSummon = null;
				if (m_earthSummonInSprite != null)
				{
					m_earthSummonInSprite.Dispose();
					m_earthSummonInSprite = null;
				}
				if (m_earthSummonOutSprite != null)
				{
					m_earthSummonOutSprite.Dispose();
					m_earthSummonOutSprite = null;
				}
				if (m_earthProjectileObj != null)
				{
					m_earthProjectileObj.Dispose();
					m_earthProjectileObj = null;
				}
				SpawnRoom = null;
				base.Dispose();
			}
		}
	}
}
