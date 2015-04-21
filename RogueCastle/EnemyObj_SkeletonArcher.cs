using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class EnemyObj_SkeletonArcher : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private float m_fireDelay = 0.5f;
		protected override void InitializeEV()
		{
			base.LockFlip = false;
			base.IsWeighted = true;
			base.IsCollidable = true;
			base.Name = "Archer";
			this.MaxHealth = 20;
			base.Damage = 22;
			base.XPValue = 75;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.4f;
			base.Speed = 125f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 1000f;
			base.JumpHeight = 0f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.SkeletonArcher_Basic_Scale;
			base.ProjectileScale = EnemyEV.SkeletonArcher_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.SkeletonArcher_Basic_Tint;
			this.MeleeRadius = 325;
			this.ProjectileRadius = 625;
			this.EngageRadius = 850;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.SkeletonArcher_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Ranger";
				this.MaxHealth = 35;
				base.Damage = 27;
				base.XPValue = 125;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 150f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1100f;
				base.JumpHeight = 0f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.SkeletonArcher_Advanced_Scale;
				base.ProjectileScale = EnemyEV.SkeletonArcher_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.SkeletonArcher_Advanced_Tint;
				this.MeleeRadius = 325;
				this.EngageRadius = 850;
				this.ProjectileRadius = 625;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.SkeletonArcher_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Sniper";
				this.MaxHealth = 61;
				base.Damage = 31;
				base.XPValue = 275;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 200f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1100f;
				base.JumpHeight = 0f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.SkeletonArcher_Expert_Scale;
				base.ProjectileScale = EnemyEV.SkeletonArcher_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.SkeletonArcher_Expert_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 625;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.SkeletonArcher_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Sharpshooter";
				this.MaxHealth = 600;
				base.Damage = 40;
				base.XPValue = 600;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 1f;
				base.Speed = 250f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1000f;
				base.JumpHeight = 0f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.SkeletonArcher_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.SkeletonArcher_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.SkeletonArcher_Miniboss_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 625;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.SkeletonArcher_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "ArrowProjectile_Sprite",
				SourceAnchor = new Vector2(10f, -20f),
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = true,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = base.ProjectileScale,
				FollowArc = true
			};
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-20f, -20f);
			logicSet2.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-20f, -20f);
			logicSet3.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-20f, -20f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-35f, -35f);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-35f, -35f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-15f, -15f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-55f, -55f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet5.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet7.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet7.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			logicSet7.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-70f, -70f);
			logicSet8.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet8.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet8.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-70f, -70f);
			logicSet8.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet8.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet8.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-55f, -55f);
			logicSet9.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet9.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet9.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet9.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-55f, -55f);
			logicSet9.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-85f, -85f);
			logicSet9.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet9.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet9.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet9.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet9.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet9.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "AngleArcher", new object[]
			{
				projectileData.Angle.X
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet10.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Load"
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack", false), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(this.m_fireDelay, false), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet10.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-85f, -85f);
			logicSet10.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-95f, -95f);
			logicSet10.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet10.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"SkeletonArcher_Attack_01",
				"SkeletonArcher_Attack_02",
				"SkeletonArcher_Attack_03"
			}), Types.Sequence.Serial);
			logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack", false), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new ChangePropertyLogicAction(base.GetChildAt(1), "Rotation", 0), Types.Sequence.Serial);
			logicSet10.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet8,
				logicSet5
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet3,
				logicSet9,
				logicSet6
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet4,
				logicSet10,
				logicSet7
			});
			projectileData.Dispose();
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this._objectList[1].TextureColor = this.TintablePart.TextureColor;
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 100;
				base.RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					20,
					15,
					15,
					50
				});
				return;
			case 2:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					20,
					15,
					50,
					15
				});
				return;
			case 3:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					20,
					50,
					15,
					15
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
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = this.m_generalAdvancedLB;
				int[] array = new int[4];
				array[0] = 100;
				base.RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				base.RunLogicBlock(false, this.m_generalAdvancedLB, new int[]
				{
					20,
					15,
					15,
					50
				});
				return;
			case 2:
				base.RunLogicBlock(false, this.m_generalAdvancedLB, new int[]
				{
					20,
					15,
					50,
					15
				});
				return;
			case 3:
				base.RunLogicBlock(false, this.m_generalAdvancedLB, new int[]
				{
					20,
					50,
					15,
					15
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
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = this.m_generalExpertLB;
				int[] array = new int[4];
				array[0] = 100;
				base.RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				base.RunLogicBlock(false, this.m_generalExpertLB, new int[]
				{
					20,
					15,
					15,
					50
				});
				return;
			case 2:
				base.RunLogicBlock(false, this.m_generalExpertLB, new int[]
				{
					20,
					50,
					15,
					15
				});
				return;
			case 3:
				base.RunLogicBlock(false, this.m_generalExpertLB, new int[]
				{
					20,
					15,
					50,
					15
				});
				return;
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 100;
				base.RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					20,
					15,
					15,
					50
				});
				return;
			case 2:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					20,
					15,
					50,
					15
				});
				return;
			case 3:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					20,
					50,
					15,
					15
				});
				return;
			default:
				return;
			}
		}
		public void AngleArcher(float angle)
		{
			if (this.Flip == SpriteEffects.None)
			{
				base.GetChildAt(1).Rotation = angle;
				return;
			}
			base.GetChildAt(1).Rotation = -angle;
		}
		public override void ResetState()
		{
			base.GetChildAt(1).Rotation = 0f;
			base.ResetState();
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
			base.HitEnemy(damage, position, isPlayer);
		}
		public EnemyObj_SkeletonArcher(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySkeletonArcherIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 25;
			this.TintablePart = this._objectList[0];
		}
	}
}
