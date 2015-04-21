using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EnemyObj_LastBoss : EnemyObj
	{
		private FrameSoundObj m_walkUpSoundFinalBoss;
		private FrameSoundObj m_walkDownSoundFinalBoss;
		private Vector2 AxeSpellScale = new Vector2(3f, 3f);
		private float AxeProjectileSpeed = 1100f;
		private Vector2 DaggerSpellScale = new Vector2(3.5f, 3.5f);
		private float DaggerProjectileSpeed = 900f;
		private float m_Spell_Close_Lifespan = 6f;
		private float m_Spell_Close_Scale = 3.5f;
		private int MegaFlyingDaggerProjectileSpeed = 2350;
		private int MegaFlyingSwordAmount = 29;
		private int MegaUpwardSwordProjectileSpeed = 2450;
		private int MegaUpwardSwordProjectileAmount = 8;
		private int m_Mega_Shield_Distance = 525;
		private float m_Mega_Shield_Scale = 4f;
		private float m_Mega_Shield_Speed = 1f;
		private int m_numSpears = 26;
		private float m_spearDuration = 1.75f;
		private bool m_isHurt;
		private bool m_isDashing;
		private bool m_inSecondForm;
		private float m_smokeCounter = 0.05f;
		private float m_castDelay = 0.25f;
		private int m_orbsEasy = 1;
		private int m_orbsNormal = 2;
		private int m_orbsHard = 3;
		private float m_lastBossAttackDelay = 0.35f;
		private bool m_shake;
		private bool m_shookLeft;
		private float m_shakeTimer;
		private float m_shakeDuration = 0.03f;
		private List<ProjectileObj> m_damageShieldProjectiles;
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_damageShieldLB = new LogicBlock();
		private LogicBlock m_cooldownLB = new LogicBlock();
		private LogicBlock m_secondFormCooldownLB = new LogicBlock();
		private LogicBlock m_firstFormDashAwayLB = new LogicBlock();
		private LogicBlock m_generalBasicNeoLB = new LogicBlock();
		private bool m_firstFormDying;
		private float m_teleportDuration;
		private BlankObj m_delayObj;
		private bool m_isNeo;
		private bool m_neoDying;
		private ProjectileData m_daggerProjData;
		private ProjectileData m_axeProjData;
		public bool IsSecondForm
		{
			get
			{
				return this.m_inSecondForm;
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
					this.ItemDropChance = 0f;
					this.MoneyDropChance = 0f;
					this.m_saveToEnemiesKilledList = false;
					this.CanFallOffLedges = true;
				}
			}
		}
		protected override void InitializeEV()
		{
			base.Name = "Johannes";
			this.MaxHealth = 300;
			base.Damage = 22;
			base.XPValue = 1000;
			this.MinMoneyDropAmount = 20;
			this.MaxMoneyDropAmount = 30;
			this.MoneyDropChance = 1f;
			base.Speed = 500f;
			this.TurnSpeed = 2f;
			this.ProjectileSpeed = 1100f;
			base.JumpHeight = 1050f;
			this.CooldownTime = 1f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.LastBoss_Basic_Scale;
			base.ProjectileScale = EnemyEV.LastBoss_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.LastBoss_Basic_Tint;
			this.MeleeRadius = 300;
			this.ProjectileRadius = 650;
			this.EngageRadius = 900;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.LastBoss_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "The Fountain";
				this.MaxHealth = 530;
				base.Damage = 22;
				base.XPValue = 5000;
				this.MinMoneyDropAmount = 20;
				this.MaxMoneyDropAmount = 30;
				this.MoneyDropChance = 1f;
				base.Speed = 325f;
				this.TurnSpeed = 2f;
				this.ProjectileSpeed = 1000f;
				base.JumpHeight = 1050f;
				this.CooldownTime = 1f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.LastBoss_Advanced_Scale;
				base.ProjectileScale = EnemyEV.LastBoss_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.LastBoss_Advanced_Tint;
				this.MeleeRadius = 300;
				this.EngageRadius = 925;
				this.ProjectileRadius = 675;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.LastBoss_Advanced_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Johannes";
				this.MaxHealth = 100;
				base.Damage = 30;
				base.XPValue = 1000;
				this.MinMoneyDropAmount = 20;
				this.MaxMoneyDropAmount = 30;
				this.MoneyDropChance = 1f;
				base.Speed = 550f;
				this.TurnSpeed = 2f;
				this.ProjectileSpeed = 1100f;
				base.JumpHeight = 1050f;
				this.CooldownTime = 1f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.LastBoss_Expert_Scale;
				base.ProjectileScale = EnemyEV.LastBoss_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.LastBoss_Expert_Tint;
				this.MeleeRadius = 300;
				this.ProjectileRadius = 650;
				this.EngageRadius = 900;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.LastBoss_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Fountain of Youth";
				this.MaxHealth = 100;
				base.Damage = 38;
				base.XPValue = 5000;
				this.MinMoneyDropAmount = 20;
				this.MaxMoneyDropAmount = 30;
				this.MoneyDropChance = 1f;
				base.Speed = 125f;
				this.TurnSpeed = 2f;
				this.ProjectileSpeed = 1000f;
				base.JumpHeight = 1050f;
				this.CooldownTime = 1f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.LastBoss_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.LastBoss_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.LastBoss_Miniboss_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 675;
				this.EngageRadius = 925;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.LastBoss_Miniboss_KnockBack;
				return;
			}
			base.AnimationDelay = 0.1f;
			if (LevelEV.WEAKEN_BOSSES)
			{
				this.MaxHealth = 1;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new DebugTraceLogicAction("WalkTowardSLS"), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.3f, 0.75f, false), Types.Sequence.Serial);
			logicSet.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new DebugTraceLogicAction("WalkAway"), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet2.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new DebugTraceLogicAction("walkStop"), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet3.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.25f, 0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new DebugTraceLogicAction("attack"), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.05f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(2, 4, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Player_Attack01",
				"Player_Attack02"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new DebugTraceLogicAction("moveattack"), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.05f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction(2, 4, false), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Player_Attack01",
				"Player_Attack02"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("AttackStart", "End", false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new DebugTraceLogicAction("Throwing Daggers"), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "CastCloseShield", new object[0]), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new DebugTraceLogicAction("Throwing Daggers"), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet7.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, true), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowDaggerProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet7.Tag = 2;
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new DebugTraceLogicAction("Throwing Daggers"), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet8.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", false), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, true), Types.Sequence.Serial);
			logicSet8.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
			logicSet8.AddAction(new RunFunctionLogicAction(this, "ThrowDaggerProjectilesNeo", new object[0]), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f), Types.Sequence.Serial);
			logicSet8.Tag = 2;
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new DebugTraceLogicAction("jumpLS"), Types.Sequence.Serial);
			logicSet9.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet9.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet9.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet9.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet9.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Player_Jump"
			}), Types.Sequence.Serial);
			logicSet9.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet9.AddAction(new RunFunctionLogicAction(this, "ThrowAxeProjectiles", new object[0]), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
			logicSet9.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet9.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new DebugTraceLogicAction("jumpLS"), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true), Types.Sequence.Serial);
			logicSet10.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet10.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet10.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Player_Jump"
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowAxeProjectilesNeo", new object[0]), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.75f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet10.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			LogicSet logicSet11 = new LogicSet(this);
			logicSet11.AddAction(new DebugTraceLogicAction("dashLS"), Types.Sequence.Serial);
			logicSet11.AddAction(new RunFunctionLogicAction(this, "CastCloseShield", new object[0]), Types.Sequence.Serial);
			logicSet11.AddAction(new RunFunctionLogicAction(this, "Dash", new object[]
			{
				0
			}), Types.Sequence.Serial);
			logicSet11.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet11.AddAction(new RunFunctionLogicAction(this, "DashComplete", new object[0]), Types.Sequence.Serial);
			LogicSet logicSet12 = new LogicSet(this);
			logicSet12.AddAction(new DebugTraceLogicAction("dashAwayRightLS"), Types.Sequence.Serial);
			logicSet12.AddAction(new RunFunctionLogicAction(this, "Dash", new object[]
			{
				1
			}), Types.Sequence.Serial);
			logicSet12.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet12.AddAction(new RunFunctionLogicAction(this, "DashComplete", new object[0]), Types.Sequence.Serial);
			LogicSet logicSet13 = new LogicSet(this);
			logicSet13.AddAction(new DebugTraceLogicAction("dashAwayLeftLS"), Types.Sequence.Serial);
			logicSet13.AddAction(new RunFunctionLogicAction(this, "Dash", new object[]
			{
				-1
			}), Types.Sequence.Serial);
			logicSet13.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet13.AddAction(new RunFunctionLogicAction(this, "DashComplete", new object[0]), Types.Sequence.Serial);
			LogicSet logicSet14 = new LogicSet(this);
			logicSet14.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet14.AddAction(new ChangeSpriteLogicAction("EnemyLastBossRun_Character", true, true), Types.Sequence.Serial);
			logicSet14.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet14.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet14.AddAction(new DelayLogicAction(0.35f, 1.15f, false), Types.Sequence.Serial);
			logicSet14.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet15 = new LogicSet(this);
			logicSet15.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet15.AddAction(new ChangeSpriteLogicAction("EnemyLastBossRun_Character", true, true), Types.Sequence.Serial);
			logicSet15.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet15.AddAction(new DelayLogicAction(0.2f, 1f, false), Types.Sequence.Serial);
			logicSet15.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet16 = new LogicSet(this);
			logicSet16.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet16.AddAction(new ChangeSpriteLogicAction("EnemyLastBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet16.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet16.AddAction(new DelayLogicAction(0.2f, 0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet17 = new LogicSet(this);
			logicSet17.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet17.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet17.AddAction(new ChangeSpriteLogicAction("EnemyLastBossAttack_Character", false, false), Types.Sequence.Serial);
			logicSet17.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack", false), Types.Sequence.Serial);
			logicSet17.AddAction(new DelayLogicAction(this.m_lastBossAttackDelay, false), Types.Sequence.Serial);
			logicSet17.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSwing"
			}), Types.Sequence.Serial);
			logicSet17.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_Effort_01",
				"FinalBoss_St2_Effort_02",
				"FinalBoss_St2_Effort_03",
				"FinalBoss_St2_Effort_04",
				"FinalBoss_St2_Effort_05"
			}), Types.Sequence.Serial);
			logicSet17.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet17.AddAction(new ChangeSpriteLogicAction("EnemyLastBossIdle_Character", true, true), Types.Sequence.Serial);
			logicSet17.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet18 = new LogicSet(this);
			this.RunTeleportLS(logicSet18, "Centre");
			logicSet18.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false), Types.Sequence.Serial);
			logicSet18.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSlam_Prime"
			}), Types.Sequence.Serial);
			logicSet18.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast", false), Types.Sequence.Serial);
			logicSet18.AddAction(new DelayLogicAction(this.m_castDelay, false), Types.Sequence.Serial);
			logicSet18.AddAction(new RunFunctionLogicAction(this, "CastSpears", new object[]
			{
				this.m_numSpears,
				this.m_spearDuration
			}), Types.Sequence.Serial);
			logicSet18.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSlam"
			}), Types.Sequence.Serial);
			logicSet18.AddAction(new PlayAnimationLogicAction("BeforeCast", "End", false), Types.Sequence.Serial);
			logicSet18.AddAction(new DelayLogicAction(this.m_spearDuration + 1f, false), Types.Sequence.Serial);
			LogicSet logicSet19 = new LogicSet(this);
			logicSet19.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0), Types.Sequence.Serial);
			logicSet19.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell2_Character", false, false), Types.Sequence.Serial);
			logicSet19.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSummon_a"
			}), Types.Sequence.Serial);
			logicSet19.AddAction(new PlayAnimationLogicAction("Start", "Cast", false), Types.Sequence.Serial);
			logicSet19.AddAction(new DelayLogicAction(this.m_castDelay, false), Types.Sequence.Serial);
			logicSet19.AddAction(new RunFunctionLogicAction(this, "CastSwordsRandom", new object[0]), Types.Sequence.Serial);
			logicSet19.AddAction(new PlayAnimationLogicAction("Cast", "End", false), Types.Sequence.Serial);
			logicSet19.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet20 = new LogicSet(this);
			logicSet20.AddAction(new LockFaceDirectionLogicAction(true, 1), Types.Sequence.Serial);
			this.RunTeleportLS(logicSet20, "Left");
			logicSet20.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false), Types.Sequence.Serial);
			logicSet20.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSlam_Prime"
			}), Types.Sequence.Serial);
			logicSet20.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast", false), Types.Sequence.Serial);
			logicSet20.AddAction(new DelayLogicAction(this.m_castDelay, false), Types.Sequence.Serial);
			logicSet20.AddAction(new RunFunctionLogicAction(this, "CastSwords", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet20.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSlam"
			}), Types.Sequence.Serial);
			logicSet20.AddAction(new PlayAnimationLogicAction("BeforeCast", "End", false), Types.Sequence.Serial);
			logicSet20.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			logicSet20.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet21 = new LogicSet(this);
			logicSet21.AddAction(new LockFaceDirectionLogicAction(true, -1), Types.Sequence.Serial);
			this.RunTeleportLS(logicSet21, "Right");
			logicSet21.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false), Types.Sequence.Serial);
			logicSet21.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSlam_Prime"
			}), Types.Sequence.Serial);
			logicSet21.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast", false), Types.Sequence.Serial);
			logicSet21.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_BlockLaugh"
			}), Types.Sequence.Serial);
			logicSet21.AddAction(new DelayLogicAction(this.m_castDelay, false), Types.Sequence.Serial);
			logicSet21.AddAction(new RunFunctionLogicAction(this, "CastSwords", new object[]
			{
				false
			}), Types.Sequence.Serial);
			logicSet21.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_SwordSlam"
			}), Types.Sequence.Serial);
			logicSet21.AddAction(new PlayAnimationLogicAction("BeforeCast", "End", false), Types.Sequence.Serial);
			logicSet21.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			logicSet21.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet22 = new LogicSet(this);
			logicSet22.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", new object[]
			{
				this.m_orbsEasy
			}), Types.Sequence.Serial);
			logicSet22.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet23 = new LogicSet(this);
			logicSet23.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", new object[]
			{
				this.m_orbsNormal
			}), Types.Sequence.Serial);
			logicSet23.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet24 = new LogicSet(this);
			logicSet24.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", new object[]
			{
				this.m_orbsHard
			}), Types.Sequence.Serial);
			logicSet24.AddAction(new DelayLogicAction(0f, false), Types.Sequence.Serial);
			logicSet24.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet9,
				logicSet5,
				logicSet6,
				logicSet7,
				logicSet11
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet14,
				logicSet15,
				logicSet16,
				logicSet17,
				logicSet18,
				logicSet19,
				logicSet20,
				logicSet21
			});
			this.m_damageShieldLB.AddLogicSet(new LogicSet[]
			{
				logicSet22,
				logicSet23,
				logicSet24
			});
			this.m_cooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet9,
				logicSet5,
				logicSet6,
				logicSet7,
				logicSet11
			});
			this.m_secondFormCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet14,
				logicSet15,
				logicSet16
			});
			this.m_generalBasicNeoLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet10,
				logicSet5,
				logicSet6,
				logicSet8,
				logicSet11
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_damageShieldLB);
			this.logicBlocksToDispose.Add(this.m_cooldownLB);
			this.logicBlocksToDispose.Add(this.m_secondFormCooldownLB);
			this.logicBlocksToDispose.Add(this.m_generalBasicNeoLB);
			this.m_firstFormDashAwayLB.AddLogicSet(new LogicSet[]
			{
				logicSet13,
				logicSet12
			});
			this.logicBlocksToDispose.Add(this.m_firstFormDashAwayLB);
			LogicBlock arg_139D_1 = this.m_cooldownLB;
			int[] array = new int[8];
			array[0] = 70;
			array[2] = 30;
			base.SetCooldownLogicBlock(arg_139D_1, array);
			base.InitializeLogic();
		}
		private void RunTeleportLS(LogicSet logicSet, string roomPosition)
		{
			logicSet.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "Opacity", 0.5f), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyLastBossTeleport_Character", false, false), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"FinalBoss_St2_BlockAction"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportTo", new object[]
			{
				roomPosition
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayObjLogicAction(this.m_delayObj), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "Opacity", 1), Types.Sequence.Serial);
		}
		public void ThrowAxeProjectiles()
		{
			if (this.m_axeProjData != null)
			{
				this.m_axeProjData.Dispose();
				this.m_axeProjData = null;
			}
			this.m_axeProjData = new ProjectileData(this)
			{
				SpriteName = "SpellAxe_Sprite",
				SourceAnchor = new Vector2(20f, -20f),
				Target = null,
				Speed = new Vector2(this.AxeProjectileSpeed, this.AxeProjectileSpeed),
				IsWeighted = true,
				RotationSpeed = 10f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(-90f, -90f),
				CollidesWithTerrain = false,
				Scale = this.AxeSpellScale
			};
			Tween.RunFunction(0f, this, "CastAxe", new object[]
			{
				false
			});
			Tween.RunFunction(0.15f, this, "CastAxe", new object[]
			{
				true
			});
			Tween.RunFunction(0.3f, this, "CastAxe", new object[]
			{
				true
			});
			Tween.RunFunction(0.45f, this, "CastAxe", new object[]
			{
				true
			});
			Tween.RunFunction(0.6f, this, "CastAxe", new object[]
			{
				true
			});
		}
		public void ThrowAxeProjectilesNeo()
		{
			if (this.m_axeProjData != null)
			{
				this.m_axeProjData.Dispose();
				this.m_axeProjData = null;
			}
			this.m_axeProjData = new ProjectileData(this)
			{
				SpriteName = "SpellAxe_Sprite",
				SourceAnchor = new Vector2(20f, -20f),
				Target = null,
				Speed = new Vector2(this.AxeProjectileSpeed, this.AxeProjectileSpeed),
				IsWeighted = true,
				RotationSpeed = 10f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(-90f, -90f),
				CollidesWithTerrain = false,
				Scale = this.AxeSpellScale
			};
			Tween.RunFunction(0.3f, this, "CastAxe", new object[]
			{
				true
			});
			Tween.RunFunction(0.3f, this, "CastAxe", new object[]
			{
				true
			});
			Tween.RunFunction(0.3f, this, "CastAxe", new object[]
			{
				true
			});
		}
		public void CastAxe(bool randomize)
		{
			if (randomize)
			{
				this.m_axeProjData.AngleOffset = (float)CDGMath.RandomInt(-70, 70);
			}
			this.m_levelScreen.ProjectileManager.FireProjectile(this.m_axeProjData);
			SoundManager.Play3DSound(this, this.m_target, "Cast_Axe");
			this.m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 45f, true);
		}
		public void ThrowDaggerProjectiles()
		{
			if (this.m_daggerProjData != null)
			{
				this.m_daggerProjData.Dispose();
				this.m_daggerProjData = null;
			}
			this.m_daggerProjData = new ProjectileData(this)
			{
				SpriteName = "SpellDagger_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = this.m_target,
				Speed = new Vector2(this.DaggerProjectileSpeed, this.DaggerProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = this.DaggerSpellScale
			};
			Tween.RunFunction(0f, this, "CastDaggers", new object[]
			{
				false
			});
			Tween.RunFunction(0.05f, this, "CastDaggers", new object[]
			{
				true
			});
			Tween.RunFunction(0.1f, this, "CastDaggers", new object[]
			{
				true
			});
			Tween.RunFunction(0.15f, this, "CastDaggers", new object[]
			{
				true
			});
			Tween.RunFunction(0.2f, this, "CastDaggers", new object[]
			{
				true
			});
		}
		public void ThrowDaggerProjectilesNeo()
		{
			if (this.m_daggerProjData != null)
			{
				this.m_daggerProjData.Dispose();
				this.m_daggerProjData = null;
			}
			this.m_daggerProjData = new ProjectileData(this)
			{
				SpriteName = "SpellDagger_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = this.m_target,
				Speed = new Vector2(this.DaggerProjectileSpeed - 160f, this.DaggerProjectileSpeed - 160f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = this.DaggerSpellScale
			};
			Tween.RunFunction(0f, this, "CastDaggers", new object[]
			{
				false
			});
			Tween.RunFunction(0.05f, this, "CastDaggers", new object[]
			{
				true
			});
			Tween.RunFunction(0.1f, this, "CastDaggers", new object[]
			{
				true
			});
		}
		public void CastDaggers(bool randomize)
		{
			if (randomize)
			{
				this.m_daggerProjData.AngleOffset = (float)CDGMath.RandomInt(-8, 8);
			}
			this.m_levelScreen.ProjectileManager.FireProjectile(this.m_daggerProjData);
			SoundManager.Play3DSound(this, this.m_target, "Cast_Dagger");
			this.m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 0f, true);
		}
		public void CastCloseShield()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "SpellClose_Sprite",
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				DestroysWithEnemy = false,
				DestroysWithTerrain = false,
				CollidesWithTerrain = false,
				Scale = new Vector2(this.m_Spell_Close_Scale, this.m_Spell_Close_Scale),
				Damage = base.Damage,
				Lifespan = this.m_Spell_Close_Lifespan,
				LockPosition = true
			};
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			SoundManager.Play3DSound(this, this.m_target, "Cast_GiantSword");
			this.m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 90f, true);
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			if (base.CurrentHealth > 0)
			{
				if (!this.m_inSecondForm)
				{
					if (!this.m_isHurt)
					{
						switch (base.State)
						{
						case 0:
						{
							if (!this.IsNeo)
							{
								bool arg_182_1 = true;
								LogicBlock arg_182_2 = this.m_generalBasicLB;
								int[] array = new int[8];
								array[0] = 50;
								array[6] = 50;
								base.RunLogicBlock(arg_182_1, arg_182_2, array);
								return;
							}
							bool arg_1B2_1 = true;
							LogicBlock arg_1B2_2 = this.m_generalBasicNeoLB;
							int[] array2 = new int[8];
							array2[0] = 50;
							array2[2] = 10;
							array2[3] = 10;
							array2[6] = 30;
							base.RunLogicBlock(arg_1B2_1, arg_1B2_2, array2);
							return;
						}
						case 1:
						{
							if (!this.IsNeo)
							{
								bool arg_126_1 = true;
								LogicBlock arg_126_2 = this.m_generalBasicLB;
								int[] array3 = new int[8];
								array3[0] = 40;
								array3[3] = 20;
								array3[6] = 40;
								base.RunLogicBlock(arg_126_1, arg_126_2, array3);
								return;
							}
							bool arg_156_1 = true;
							LogicBlock arg_156_2 = this.m_generalBasicNeoLB;
							int[] array4 = new int[8];
							array4[0] = 40;
							array4[2] = 20;
							array4[3] = 20;
							array4[6] = 20;
							base.RunLogicBlock(arg_156_1, arg_156_2, array4);
							return;
						}
						case 2:
							if (!this.IsNeo)
							{
								base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
								{
									35,
									0,
									0,
									25,
									0,
									0,
									20,
									20
								});
								return;
							}
							base.RunLogicBlock(true, this.m_generalBasicNeoLB, new int[]
							{
								25,
								0,
								20,
								15,
								0,
								0,
								15,
								25
							});
							return;
						case 3:
							if (!this.IsNeo)
							{
								base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
								{
									0,
									0,
									0,
									35,
									35,
									0,
									0,
									30
								});
								return;
							}
							base.RunLogicBlock(true, this.m_generalBasicNeoLB, new int[]
							{
								0,
								0,
								0,
								50,
								20,
								0,
								0,
								30
							});
							return;
						default:
							return;
						}
					}
				}
				else
				{
					this.RunAdvancedLogic();
				}
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					63,
					0,
					0,
					0,
					15,
					12,
					5,
					5
				});
				return;
			case 1:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					68,
					0,
					0,
					0,
					10,
					12,
					5,
					5
				});
				return;
			case 2:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					52,
					12,
					0,
					0,
					11,
					15,
					5,
					5
				});
				return;
			case 3:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					31,
					15,
					0,
					26,
					3,
					13,
					6,
					6
				});
				return;
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			if (base.CurrentHealth > 0)
			{
				if (!this.m_inSecondForm)
				{
					if (!this.m_isHurt)
					{
						switch (base.State)
						{
						case 0:
						{
							if (!this.m_target.IsJumping)
							{
								bool arg_1C2_1 = true;
								LogicBlock arg_1C2_2 = this.m_generalBasicLB;
								int[] array = new int[8];
								array[0] = 50;
								array[2] = 10;
								array[3] = 20;
								array[6] = 20;
								base.RunLogicBlock(arg_1C2_1, arg_1C2_2, array);
								return;
							}
							bool arg_1F2_1 = true;
							LogicBlock arg_1F2_2 = this.m_generalBasicLB;
							int[] array2 = new int[8];
							array2[0] = 50;
							array2[2] = 10;
							array2[3] = 20;
							array2[6] = 20;
							base.RunLogicBlock(arg_1F2_1, arg_1F2_2, array2);
							return;
						}
						case 1:
							if (!this.m_target.IsJumping)
							{
								base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
								{
									30,
									0,
									15,
									20,
									0,
									25,
									0,
									10
								});
								return;
							}
							base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
							{
								50,
								0,
								15,
								0,
								0,
								25,
								0,
								10
							});
							return;
						case 2:
							if (!this.m_target.IsJumping)
							{
								base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
								{
									20,
									0,
									10,
									10,
									0,
									15,
									20,
									10
								});
								return;
							}
							base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
							{
								40,
								0,
								15,
								0,
								0,
								15,
								20,
								10
							});
							return;
						case 3:
							if (this.m_isTouchingGround)
							{
								base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
								{
									0,
									10,
									0,
									20,
									35,
									10,
									0,
									25
								});
								return;
							}
							base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
							{
								0,
								10,
								0,
								0,
								55,
								10,
								0,
								25
							});
							return;
						default:
							return;
						}
					}
				}
				else
				{
					this.RunAdvancedLogic();
				}
			}
		}
		protected override void RunMinibossLogic()
		{
			bool arg_15_1 = true;
			LogicBlock arg_15_2 = this.m_generalAdvancedLB;
			int[] array = new int[8];
			array[4] = 100;
			base.RunLogicBlock(arg_15_1, arg_15_2, array);
		}
		public void TeleportTo(string roomPosition)
		{
			Vector2 zero = Vector2.Zero;
			float x = 0f;
			if (roomPosition != null)
			{
				if (!(roomPosition == "Left"))
				{
					if (!(roomPosition == "Right"))
					{
						if (roomPosition == "Centre")
						{
							x = (float)this.m_levelScreen.CurrentRoom.Bounds.Center.X;
						}
					}
					else
					{
						x = (float)(this.m_levelScreen.CurrentRoom.Bounds.Right - 200);
					}
				}
				else
				{
					x = (float)(this.m_levelScreen.CurrentRoom.Bounds.Left + 200);
				}
			}
			zero = new Vector2(x, base.Y);
			float num = Math.Abs(CDGMath.DistanceBetweenPts(base.Position, zero));
			this.m_teleportDuration = num * 0.001f;
			this.m_delayObj.X = this.m_teleportDuration;
			Tween.To(this, this.m_teleportDuration, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				zero.X.ToString()
			});
			SoundManager.Play3DSound(this, this.m_target, "FinalBoss_St2_BlockMove");
		}
		public void CastSwords(bool castLeft)
		{
			ProjectileData data = new ProjectileData(this)
			{
				SpriteName = "LastBossSwordProjectile_Sprite",
				Target = null,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				StartingRotation = 0f,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				DestroysWithEnemy = false
			};
			float num = 1f;
			int num2 = this.MegaFlyingDaggerProjectileSpeed;
			if (!castLeft)
			{
				num2 = this.MegaFlyingDaggerProjectileSpeed * -1;
			}
			SoundManager.Play3DSound(this, this.m_target, "FinalBoss_St2_SwordSummon_b");
			for (int i = 0; i < this.MegaFlyingSwordAmount; i++)
			{
				Vector2 vector = new Vector2(base.X, base.Y + (float)CDGMath.RandomInt(-1320, 100));
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(data);
				projectileObj.Position = vector;
				Tween.By(projectileObj, 2.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					num.ToString(),
					"X",
					num2.ToString()
				});
				Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile", new object[0]);
				Tween.RunFunction(num, typeof(SoundManager), "Play3DSound", new object[]
				{
					this,
					this.m_target,
					new string[]
					{
						"FinalBoss_St2_SwordSummon_c_01",
						"FinalBoss_St2_SwordSummon_c_02",
						"FinalBoss_St2_SwordSummon_c_03",
						"FinalBoss_St2_SwordSummon_c_04",
						"FinalBoss_St2_SwordSummon_c_05",
						"FinalBoss_St2_SwordSummon_c_06",
						"FinalBoss_St2_SwordSummon_c_07",
						"FinalBoss_St2_SwordSummon_c_08"
					}
				});
				this.m_levelScreen.ImpactEffectPool.SpellCastEffect(vector, 0f, false);
				num += 0.075f;
			}
		}
		public void CastSpears(int numSpears, float duration)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "LastBossSpearProjectile_Sprite",
				Target = null,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				StartingRotation = 0f,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				DestroysWithEnemy = false,
				ShowIcon = false,
				LockPosition = true,
				CanBeFusRohDahed = false
			};
			int num = 0;
			int num2 = 0;
			float num3 = 0.5f;
			base.UpdateCollisionBoxes();
			Vector2 vector = new Vector2((float)this.m_levelScreen.CurrentRoom.Bounds.Center.X, base.Y);
			for (int i = 0; i < numSpears; i++)
			{
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileObj.Scale = new Vector2(2f, 2f);
				projectileObj.X = vector.X + 50f + (float)num;
				projectileObj.Y = base.Y + ((float)this.Bounds.Bottom - base.Y);
				projectileObj.StopAnimation();
				num += projectileObj.Width;
				Tween.RunFunction(num3, typeof(SoundManager), "Play3DSound", new object[]
				{
					this,
					this.m_target,
					new string[]
					{
						"FinalBoss_St2_Lance_01",
						"FinalBoss_St2_Lance_02",
						"FinalBoss_St2_Lance_03",
						"FinalBoss_St2_Lance_04",
						"FinalBoss_St2_Lance_05",
						"FinalBoss_St2_Lance_06",
						"FinalBoss_St2_Lance_07",
						"FinalBoss_St2_Lance_08"
					}
				});
				Tween.RunFunction(num3, projectileObj, "PlayAnimation", new object[]
				{
					"Before",
					"End",
					false
				});
				Tween.RunFunction(num3 + duration, projectileObj, "PlayAnimation", new object[]
				{
					"Retract",
					"RetractComplete",
					false
				});
				Tween.RunFunction(num3 + duration, typeof(SoundManager), "Play3DSound", new object[]
				{
					this,
					this.m_target,
					new string[]
					{
						"FinalBoss_St2_Lance_Retract_01",
						"FinalBoss_St2_Lance_Retract_02",
						"FinalBoss_St2_Lance_Retract_03",
						"FinalBoss_St2_Lance_Retract_04",
						"FinalBoss_St2_Lance_Retract_05",
						"FinalBoss_St2_Lance_Retract_06"
					}
				});
				Tween.RunFunction(num3 + duration + 1f, projectileObj, "KillProjectile", new object[0]);
				ProjectileObj projectileObj2 = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileObj2.Scale = new Vector2(2f, 2f);
				projectileObj2.X = vector.X - 50f + (float)num2;
				projectileObj2.Y = base.Y + ((float)this.Bounds.Bottom - base.Y);
				projectileObj2.StopAnimation();
				num2 -= projectileObj2.Width;
				Tween.RunFunction(num3, projectileObj2, "PlayAnimation", new object[]
				{
					"Before",
					"End",
					false
				});
				Tween.RunFunction(num3 + duration, projectileObj2, "PlayAnimation", new object[]
				{
					"Retract",
					"RetractComplete",
					false
				});
				Tween.RunFunction(num3 + duration + 1f, projectileObj2, "KillProjectile", new object[0]);
				num3 += 0.05f;
			}
			projectileData.Dispose();
		}
		public void CastSwordsRandom()
		{
			Vector2 vector = new Vector2((float)this.m_levelScreen.CurrentRoom.Bounds.Center.X, base.Y);
			base.UpdateCollisionBoxes();
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "LastBossSwordVerticalProjectile_Sprite",
				Target = null,
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				StartingRotation = 0f,
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				DestroysWithEnemy = false,
				LockPosition = true
			};
			int megaUpwardSwordProjectileSpeed = this.MegaUpwardSwordProjectileSpeed;
			int num = 0;
			int num2 = 0;
			float num3 = 1f;
			for (int i = 0; i < this.MegaUpwardSwordProjectileAmount; i++)
			{
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileObj.Scale = new Vector2(1.5f, 1.5f);
				projectileObj.X = vector.X + 50f + (float)num;
				projectileObj.Y = vector.Y + ((float)this.Bounds.Bottom - base.Y) + 120f;
				projectileObj.Opacity = 0f;
				Tween.To(projectileObj, 0.25f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(projectileObj, 2.5f, new Easing(Quad.EaseIn), new string[]
				{
					"delay",
					num3.ToString(),
					"Y",
					(-megaUpwardSwordProjectileSpeed).ToString()
				});
				Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile", new object[0]);
				num = CDGMath.RandomInt(50, 1000);
				ProjectileObj projectileObj2 = this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
				projectileObj2.Scale = new Vector2(2f, 2f);
				projectileObj2.X = vector.X - 50f + (float)num2;
				projectileObj2.Y = vector.Y + ((float)this.Bounds.Bottom - base.Y) + 120f;
				projectileObj2.Opacity = 0f;
				Tween.To(projectileObj2, 0.25f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.By(projectileObj2, 2.5f, new Easing(Quad.EaseIn), new string[]
				{
					"delay",
					num3.ToString(),
					"Y",
					(-megaUpwardSwordProjectileSpeed).ToString()
				});
				Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile", new object[0]);
				num2 = -CDGMath.RandomInt(50, 1000);
				num3 += 0.25f;
			}
			projectileData.Dispose();
		}
		public void ChangeProjectileSpeed(ProjectileObj proj, float speed, Vector2 heading)
		{
			proj.AccelerationX = heading.X * speed;
			proj.AccelerationY = -heading.Y * speed;
		}
		public void CastDamageShield(int numOrbs)
		{
			foreach (ProjectileObj current in this.m_damageShieldProjectiles)
			{
				current.KillProjectile();
			}
			this.m_damageShieldProjectiles.Clear();
			ProjectileData data = new ProjectileData(this)
			{
				SpriteName = "LastBossOrbProjectile_Sprite",
				Angle = new Vector2(-65f, -65f),
				Speed = new Vector2(this.m_Mega_Shield_Speed, this.m_Mega_Shield_Speed),
				Target = this,
				IsWeighted = false,
				RotationSpeed = 0f,
				CollidesWithTerrain = false,
				DestroysWithTerrain = false,
				DestroysWithEnemy = false,
				CanBeFusRohDahed = false,
				ShowIcon = false,
				Lifespan = 9999f,
				Damage = base.Damage / 2
			};
			SoundManager.Play3DSound(this, this.m_target, "FinalBoss_St2_SwordSummon_b");
			int mega_Shield_Distance = this.m_Mega_Shield_Distance;
			for (int i = 0; i < numOrbs; i++)
			{
				float num = 360f / (float)numOrbs * (float)i;
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(data);
				projectileObj.AltX = num;
				projectileObj.AltY = (float)mega_Shield_Distance;
				projectileObj.Spell = 11;
				projectileObj.AccelerationXEnabled = false;
				projectileObj.AccelerationYEnabled = false;
				projectileObj.IgnoreBoundsCheck = true;
				projectileObj.Scale = new Vector2(this.m_Mega_Shield_Scale, this.m_Mega_Shield_Scale);
				projectileObj.Position = CDGMath.GetCirclePosition(num, (float)mega_Shield_Distance, base.Position);
				this.m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, projectileObj.Rotation, false);
				this.m_damageShieldProjectiles.Add(projectileObj);
			}
		}
		public void Dash(int heading)
		{
			base.HeadingY = 0f;
			if (this.m_target.Position.X < base.X)
			{
				if (heading == 0)
				{
					base.HeadingX = 1f;
				}
				if (this.Flip == SpriteEffects.None)
				{
					this.ChangeSprite("PlayerFrontDash_Character");
				}
				else
				{
					this.ChangeSprite("PlayerDash_Character");
				}
				this.m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(base.X, (float)this.TerrainBounds.Bottom), false);
			}
			else
			{
				if (heading == 0)
				{
					base.HeadingX = -1f;
				}
				if (this.Flip == SpriteEffects.None)
				{
					this.ChangeSprite("PlayerDash_Character");
				}
				else
				{
					this.ChangeSprite("PlayerFrontDash_Character");
				}
				this.m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(base.X, (float)this.TerrainBounds.Bottom), true);
			}
			if (heading != 0)
			{
				base.HeadingX = (float)heading;
			}
			SoundManager.Play3DSound(this, this.m_target, "Player_Dash");
			base.LockFlip = true;
			base.AccelerationX = 0f;
			base.AccelerationY = 0f;
			base.PlayAnimation(false);
			base.CurrentSpeed = 900f;
			base.AccelerationYEnabled = false;
			this.m_isDashing = true;
		}
		public void DashComplete()
		{
			base.LockFlip = false;
			base.CurrentSpeed = 500f;
			base.AccelerationYEnabled = true;
			this.m_isDashing = false;
			base.AnimationDelay = 0.1f;
		}
		public override void Update(GameTime gameTime)
		{
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
			if (this.m_smokeCounter > 0f && !this.m_inSecondForm)
			{
				this.m_smokeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_smokeCounter <= 0f)
				{
					this.m_smokeCounter = 0.25f;
					if (base.CurrentSpeed > 0f)
					{
						this.m_smokeCounter = 0.05f;
					}
					this.m_levelScreen.ImpactEffectPool.BlackSmokeEffect(this);
				}
			}
			if (!this.m_inSecondForm)
			{
				if (!this.m_isTouchingGround && this.m_currentActiveLB != null && base.SpriteName != "PlayerAttacking3_Character" && !this.m_isDashing && base.SpriteName != "PlayerLevelUp_Character")
				{
					if (base.AccelerationY < 0f && base.SpriteName != "PlayerJumping_Character")
					{
						this.ChangeSprite("PlayerJumping_Character");
						base.PlayAnimation(true);
					}
					else if (base.AccelerationY > 0f && base.SpriteName != "PlayerFalling_Character")
					{
						this.ChangeSprite("PlayerFalling_Character");
						base.PlayAnimation(true);
					}
				}
				else if (this.m_isTouchingGround && this.m_currentActiveLB != null && base.SpriteName == "PlayerAttacking3_Character" && base.CurrentSpeed != 0f)
				{
					SpriteObj spriteObj = base.GetChildAt(2) as SpriteObj;
					if (spriteObj.SpriteName != "PlayerWalkingLegs_Sprite")
					{
						spriteObj.ChangeSprite("PlayerWalkingLegs_Sprite");
						spriteObj.PlayAnimation(base.CurrentFrame, base.TotalFrames, false);
						spriteObj.Y += 4f;
						spriteObj.OverrideParentAnimationDelay = true;
						spriteObj.AnimationDelay = 0.1f;
					}
				}
			}
			else if (base.SpriteName == "EnemyLastBossRun_Character")
			{
				this.m_walkUpSoundFinalBoss.Update();
				this.m_walkDownSoundFinalBoss.Update();
			}
			if (!this.m_inSecondForm && base.CurrentHealth <= 0 && this.m_target.CurrentHealth > 0 && !this.IsNeo)
			{
				if (base.IsTouchingGround && !this.m_firstFormDying)
				{
					this.m_firstFormDying = true;
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 2, 0.1f);
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 2, 0.1f);
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 2, 0.1f);
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 2, 0.1f);
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 3, 0.1f);
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 3, 0.1f);
					this.m_levelScreen.ItemDropManager.DropItemWide(base.Position, 3, 0.1f);
					base.IsWeighted = false;
					base.IsCollidable = false;
					base.AnimationDelay = 0.1f;
					base.CurrentSpeed = 0f;
					base.AccelerationX = 0f;
					base.AccelerationY = 0f;
					base.TextureColor = Color.White;
					this.ChangeSprite("PlayerDeath_Character");
					SoundManager.PlaySound("Boss_Flash");
					SoundManager.StopMusic(1f);
					this.m_target.StopAllSpells();
					this.m_target.ForceInvincible = true;
					if (this.m_target.X < base.X)
					{
						this.Flip = SpriteEffects.FlipHorizontally;
					}
					else
					{
						this.Flip = SpriteEffects.None;
					}
					if (this.m_currentActiveLB != null && this.m_currentActiveLB.IsActive)
					{
						this.m_currentActiveLB.StopLogicBlock();
					}
				}
				if (this.m_target.IsTouchingGround && !this.m_inSecondForm && base.SpriteName == "PlayerDeath_Character")
				{
					this.MovePlayerTo();
				}
			}
			if ((!this.m_firstFormDying && !this.m_inSecondForm) || (this.m_firstFormDying && this.m_inSecondForm) || (this.IsNeo && base.CurrentHealth > 0))
			{
				base.Update(gameTime);
			}
			if (!this.m_inSecondForm && base.CurrentHealth <= 0 && this.m_target.CurrentHealth > 0 && this.IsNeo && base.IsTouchingGround && !this.m_firstFormDying)
			{
				this.KillPlayerNeo();
				this.m_firstFormDying = true;
			}
		}
		public void MovePlayerTo()
		{
			this.m_target.StopAllSpells();
			this.m_levelScreen.ProjectileManager.DestroyAllProjectiles(true);
			this.m_inSecondForm = true;
			this.m_isKilled = true;
			this.m_levelScreen.RunCinematicBorders(16f);
			this.m_currentActiveLB.StopLogicBlock();
			int num = 250;
			Vector2 zero = Vector2.Zero;
			if ((this.m_target.X < base.X && base.X > this.m_levelScreen.CurrentRoom.X + 500f) || base.X > (float)(this.m_levelScreen.CurrentRoom.Bounds.Right - 500))
			{
				zero = new Vector2(base.X - (float)num, base.Y);
				if (zero.X > (float)(this.m_levelScreen.CurrentRoom.Bounds.Right - 500))
				{
					zero.X = (float)(this.m_levelScreen.CurrentRoom.Bounds.Right - 500);
				}
			}
			else
			{
				zero = new Vector2(base.X + (float)num, base.Y);
			}
			this.m_target.Flip = SpriteEffects.None;
			if (zero.X < this.m_target.X)
			{
				this.m_target.Flip = SpriteEffects.FlipHorizontally;
			}
			float num2 = CDGMath.DistanceBetweenPts(this.m_target.Position, zero) / this.m_target.Speed;
			this.m_target.UpdateCollisionBoxes();
			this.m_target.State = 1;
			this.m_target.IsWeighted = false;
			this.m_target.AccelerationY = 0f;
			this.m_target.AccelerationX = 0f;
			this.m_target.IsCollidable = false;
			this.m_target.Y = (float)(this.m_levelScreen.CurrentRoom.Bounds.Bottom - 180) - ((float)this.m_target.Bounds.Bottom - this.m_target.Y);
			this.m_target.CurrentSpeed = 0f;
			this.m_target.LockControls();
			this.m_target.ChangeSprite("PlayerWalking_Character");
			LogicSet logicSet = new LogicSet(this.m_target);
			logicSet.AddAction(new DelayLogicAction(num2, false), Types.Sequence.Serial);
			this.m_target.RunExternalLogicSet(logicSet);
			this.m_target.PlayAnimation(true);
			Tween.To(this.m_target, num2, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				zero.X.ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "SecondFormDeath", new object[0]);
		}
		public void SecondFormDeath()
		{
			if (this.m_target.X < base.X)
			{
				this.m_target.Flip = SpriteEffects.None;
			}
			else
			{
				this.m_target.Flip = SpriteEffects.FlipHorizontally;
			}
			base.PlayAnimation(false);
			SoundManager.PlaySound("FinalBoss_St1_DeathGrunt");
			Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_SwordTwirl"
			});
			Tween.RunFunction(0.7f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_SwordLand"
			});
			Tween.RunFunction(1.2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_BodyFall"
			});
			float num = 2f;
			Tween.RunFunction(2f, this, "PlayBlackSmokeSounds", new object[0]);
			for (int i = 0; i < 30; i++)
			{
				Tween.RunFunction(num, this.m_levelScreen.ImpactEffectPool, "BlackSmokeEffect", new object[]
				{
					base.Position,
					new Vector2(1f + num * 1f, 1f + num * 1f)
				});
				num += 0.05f;
			}
			Tween.RunFunction(3f, this, "HideEnemy", new object[0]);
			Tween.RunFunction(6f, this, "SecondFormDialogue", new object[0]);
		}
		public void PlayBlackSmokeSounds()
		{
			SoundManager.PlaySound("Cutsc_Smoke");
		}
		public void HideEnemy()
		{
			base.Visible = false;
		}
		public void SecondFormDialogue()
		{
			RCScreenManager rCScreenManager = this.m_levelScreen.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk02");
			rCScreenManager.DialogueScreen.SetConfirmEndHandler(this.m_levelScreen.CurrentRoom, "RunFountainCutscene", new object[0]);
			rCScreenManager.DisplayScreen(13, true, null);
		}
		public void SecondFormComplete()
		{
			this.m_target.ForceInvincible = false;
			base.Level += 10;
			this.Flip = SpriteEffects.FlipHorizontally;
			base.Visible = true;
			this.MaxHealth = 530;
			base.Damage = 22;
			base.CurrentHealth = this.MaxHealth;
			base.Name = "The Fountain";
			if (LevelEV.WEAKEN_BOSSES)
			{
				base.CurrentHealth = 1;
			}
			this.MinMoneyDropAmount = 20;
			this.MaxMoneyDropAmount = 30;
			this.MoneyDropChance = 1f;
			base.Speed = 325f;
			this.TurnSpeed = 2f;
			this.ProjectileSpeed = 1000f;
			base.JumpHeight = 1050f;
			this.CooldownTime = 1f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			base.ProjectileScale = EnemyEV.LastBoss_Advanced_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.LastBoss_Advanced_Tint;
			this.MeleeRadius = 300;
			this.EngageRadius = 925;
			this.ProjectileRadius = 675;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.LastBoss_Advanced_KnockBack;
			this.ChangeSprite("EnemyLastBossIdle_Character");
			base.SetCooldownLogicBlock(this.m_secondFormCooldownLB, new int[]
			{
				40,
				20,
				40
			});
			base.PlayAnimation(true);
			base.Name = "The Fountain";
			base.IsWeighted = true;
			base.IsCollidable = true;
		}
		public void SecondFormActive()
		{
			if (base.IsPaused)
			{
				base.UnpauseEnemy(true);
			}
			this.m_levelScreen.CameraLockedToPlayer = true;
			this.m_target.UnlockControls();
			this.m_target.IsWeighted = true;
			this.m_target.IsCollidable = true;
			this.m_isKilled = false;
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			if (!this.m_inSecondForm)
			{
				if (!this.m_isHurt && !this.m_isDashing)
				{
					SoundManager.Play3DSound(this, this.m_target, new string[]
					{
						"FinalBoss_St1_Dmg_01",
						"FinalBoss_St1_Dmg_02",
						"FinalBoss_St1_Dmg_03",
						"FinalBoss_St1_Dmg_04"
					});
					base.HitEnemy(damage, collisionPt, isPlayer);
					return;
				}
			}
			else
			{
				SoundManager.Play3DSound(this, this.m_target, new string[]
				{
					"FinalBoss_St2_Hit_01",
					"FinalBoss_St2_Hit_03",
					"FinalBoss_St2_Hit_04"
				});
				SoundManager.Play3DSound(this, this.m_target, new string[]
				{
					"FinalBoss_St2_DmgVox_01",
					"FinalBoss_St2_DmgVox_02",
					"FinalBoss_St2_DmgVox_03",
					"FinalBoss_St2_DmgVox_04",
					"FinalBoss_St2_DmgVox_05",
					"FinalBoss_St2_DmgVox_06",
					"FinalBoss_St2_DmgVox_07",
					"FinalBoss_St2_DmgVox_08",
					"FinalBoss_St2_DmgVox_09"
				});
				base.HitEnemy(damage, collisionPt, isPlayer);
			}
		}
		public override void Kill(bool giveXP = true)
		{
			if (this.m_target.CurrentHealth > 0)
			{
				if (this.m_inSecondForm && !this.m_bossVersionKilled)
				{
					this.m_bossVersionKilled = true;
					this.SetPlayerData();
					this.m_levelScreen.PauseScreen();
					this.m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
					this.m_target.StopAllSpells();
					this.m_levelScreen.RunWhiteSlashEffect();
					this.ChangeSprite("EnemyLastBossDeath_Character");
					if (this.m_target.X < base.X)
					{
						this.Flip = SpriteEffects.FlipHorizontally;
					}
					else
					{
						this.Flip = SpriteEffects.None;
					}
					Tween.RunFunction(1f, this, "Part2", new object[0]);
					SoundManager.PlaySound("Boss_Flash");
					SoundManager.PlaySound("Boss_Eyeball_Freeze");
					SoundManager.StopMusic(0f);
					this.m_target.LockControls();
					GameUtil.UnlockAchievement("FEAR_OF_FATHERS");
					if (Game.PlayerStats.TimesCastleBeaten > 1)
					{
						GameUtil.UnlockAchievement("FEAR_OF_TWINS");
					}
				}
				if (this.IsNeo && !this.m_neoDying)
				{
					this.m_neoDying = true;
					this.m_levelScreen.PauseScreen();
					SoundManager.PauseMusic();
					this.m_levelScreen.RunWhiteSlashEffect();
					SoundManager.PlaySound("Boss_Flash");
					SoundManager.PlaySound("Boss_Eyeball_Freeze");
					Tween.RunFunction(1f, this.m_levelScreen, "UnpauseScreen", new object[0]);
					Tween.RunFunction(1f, typeof(SoundManager), "ResumeMusic", new object[0]);
				}
			}
		}
		public void KillPlayerNeo()
		{
			this.m_isKilled = true;
			if (this.m_currentActiveLB != null && this.m_currentActiveLB.IsActive)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			base.IsWeighted = false;
			base.IsCollidable = false;
			base.AnimationDelay = 0.1f;
			base.CurrentSpeed = 0f;
			base.AccelerationX = 0f;
			base.AccelerationY = 0f;
			this.ChangeSprite("PlayerDeath_Character");
			base.PlayAnimation(false);
			SoundManager.PlaySound("FinalBoss_St1_DeathGrunt");
			Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_SwordTwirl"
			});
			Tween.RunFunction(0.7f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_SwordLand"
			});
			Tween.RunFunction(1.2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Player_Death_BodyFall"
			});
		}
		public void SetPlayerData()
		{
			FamilyTreeNode item = new FamilyTreeNode
			{
				Name = Game.PlayerStats.PlayerName,
				Age = Game.PlayerStats.Age,
				ChildAge = Game.PlayerStats.ChildAge,
				Class = Game.PlayerStats.Class,
				HeadPiece = Game.PlayerStats.HeadPiece,
				ChestPiece = Game.PlayerStats.ChestPiece,
				ShoulderPiece = Game.PlayerStats.ShoulderPiece,
				NumEnemiesBeaten = Game.PlayerStats.NumEnemiesBeaten,
				BeatenABoss = true,
				Traits = Game.PlayerStats.Traits,
				IsFemale = Game.PlayerStats.IsFemale
			};
			Game.PlayerStats.FamilyTreeArray.Add(item);
			Game.PlayerStats.NewBossBeaten = false;
			Game.PlayerStats.RerolledChildren = false;
			Game.PlayerStats.NumEnemiesBeaten = 0;
			Game.PlayerStats.LichHealth = 0;
			Game.PlayerStats.LichMana = 0;
			Game.PlayerStats.LichHealthMod = 1f;
			Game.PlayerStats.LoadStartingRoom = true;
			Game.PlayerStats.LastbossBeaten = true;
			Game.PlayerStats.CharacterFound = false;
			Game.PlayerStats.TimesCastleBeaten++;
			(this.m_target.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			Vector2 vector = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (playerObj != null && otherBox.Type == 0 && !playerObj.IsInvincible && playerObj.State != 3)
			{
				playerObj.HitPlayer(this);
			}
			if (this.m_isTouchingGround && this.m_isHurt)
			{
				this.m_isHurt = false;
				if (!this.m_inSecondForm)
				{
					this.ChangeSprite("PlayerIdle_Character");
				}
			}
			if (!(otherBox.AbsParent is EnemyObj_Platform))
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
			TerrainObj terrainObj = otherBox.AbsParent as TerrainObj;
			if (terrainObj != null && !this.m_isTouchingGround && !(terrainObj is DoorObj) && !this.m_inSecondForm)
			{
				if (this.m_currentActiveLB != null && this.m_currentActiveLB.IsActive)
				{
					this.m_currentActiveLB.StopLogicBlock();
				}
				if (vector.X > 0f)
				{
					base.RunLogicBlock(true, this.m_firstFormDashAwayLB, new int[]
					{
						0,
						100
					});
					return;
				}
				if (vector.X < 0f)
				{
					bool arg_11D_1 = true;
					LogicBlock arg_11D_2 = this.m_firstFormDashAwayLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_11D_1, arg_11D_2, array);
				}
			}
		}
		public void Part2()
		{
			SoundManager.PlaySound("FinalBoss_St2_WeatherChange_a");
			this.m_levelScreen.UnpauseScreen();
			if (this.m_currentActiveLB != null)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			base.PauseEnemy(true);
			this.m_target.CurrentSpeed = 0f;
			this.m_target.ForceInvincible = true;
			Tween.RunFunction(1f, this.m_levelScreen, "RevealMorning", new object[0]);
			Tween.RunFunction(1f, this.m_levelScreen.CurrentRoom, "ChangeWindowOpacity", new object[0]);
			Tween.RunFunction(5f, this, "Part3", new object[0]);
		}
		public void Part3()
		{
			RCScreenManager rCScreenManager = this.m_levelScreen.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk03");
			rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "Part4", new object[0]);
			rCScreenManager.DisplayScreen(13, true, null);
		}
		public void Part4()
		{
			List<object> list = new List<object>();
			list.Add(this);
			(this.m_levelScreen.ScreenManager as RCScreenManager).DisplayScreen(26, true, list);
		}
		public EnemyObj_LastBoss(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("PlayerIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			foreach (GameObj current in this._objectList)
			{
				current.TextureColor = new Color(100, 100, 100);
			}
			this.Type = 29;
			this.m_damageShieldProjectiles = new List<ProjectileObj>();
			this._objectList[5].Visible = false;
			this._objectList[15].Visible = false;
			this._objectList[16].Visible = false;
			this._objectList[14].Visible = false;
			this._objectList[13].Visible = false;
			this._objectList[0].Visible = false;
			string text = (this._objectList[12] as IAnimateableObj).SpriteName;
			int startIndex = text.IndexOf("_") - 1;
			text = text.Remove(startIndex, 1);
			text = text.Replace("_", 7 + "_");
			this._objectList[12].ChangeSprite(text);
			base.PlayAnimation(true);
			this.m_delayObj = new BlankObj(0, 0);
			this.m_walkDownSoundFinalBoss = new FrameSoundObj(this, 3, new string[]
			{
				"FinalBoss_St2_Foot_01",
				"FinalBoss_St2_Foot_02",
				"FinalBoss_St2_Foot_03"
			});
			this.m_walkUpSoundFinalBoss = new FrameSoundObj(this, 6, new string[]
			{
				"FinalBoss_St2_Foot_04",
				"FinalBoss_St2_Foot_05"
			});
		}
		public override void ChangeSprite(string spriteName)
		{
			base.ChangeSprite(spriteName);
			if (!this.m_inSecondForm)
			{
				string text = (this._objectList[12] as IAnimateableObj).SpriteName;
				int startIndex = text.IndexOf("_") - 1;
				text = text.Remove(startIndex, 1);
				text = text.Replace("_", 7 + "_");
				this._objectList[12].ChangeSprite(text);
				this._objectList[5].Visible = false;
				this._objectList[15].Visible = false;
				this._objectList[16].Visible = false;
				this._objectList[14].Visible = false;
				this._objectList[13].Visible = false;
				this._objectList[0].Visible = false;
			}
		}
		public override void Draw(Camera2D camera)
		{
			if (base.IsKilled && base.TextureColor != Color.White)
			{
				this.m_blinkTimer = 0f;
				base.TextureColor = Color.White;
			}
			base.Draw(camera);
		}
		public override void Reset()
		{
			this.m_neoDying = false;
			this.m_inSecondForm = false;
			this.m_firstFormDying = false;
			base.CanBeKnockedBack = true;
			base.Reset();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_damageShieldProjectiles.Clear();
				this.m_damageShieldProjectiles = null;
				this.m_delayObj.Dispose();
				this.m_delayObj = null;
				if (this.m_daggerProjData != null)
				{
					this.m_daggerProjData.Dispose();
					this.m_daggerProjData = null;
				}
				if (this.m_axeProjData != null)
				{
					this.m_axeProjData.Dispose();
					this.m_axeProjData = null;
				}
				base.Dispose();
			}
		}
		public void ForceSecondForm(bool value)
		{
			this.m_inSecondForm = value;
		}
	}
}
