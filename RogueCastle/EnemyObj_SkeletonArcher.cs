/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			LockFlip = false;
			IsWeighted = true;
			IsCollidable = true;
			Name = "Archer";
			MaxHealth = 20;
			Damage = 22;
			XPValue = 75;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 1;
			MoneyDropChance = 0.4f;
			Speed = 125f;
			TurnSpeed = 10f;
			ProjectileSpeed = 1000f;
			JumpHeight = 0f;
			CooldownTime = 2f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = false;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.SkeletonArcher_Basic_Scale;
			ProjectileScale = EnemyEV.SkeletonArcher_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.SkeletonArcher_Basic_Tint;
			MeleeRadius = 325;
			ProjectileRadius = 625;
			EngageRadius = 850;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.SkeletonArcher_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Ranger";
				MaxHealth = 35;
				Damage = 27;
				XPValue = 125;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 150f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1100f;
				JumpHeight = 0f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.SkeletonArcher_Advanced_Scale;
				ProjectileScale = EnemyEV.SkeletonArcher_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.SkeletonArcher_Advanced_Tint;
				MeleeRadius = 325;
				EngageRadius = 850;
				ProjectileRadius = 625;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.SkeletonArcher_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Sniper";
				MaxHealth = 61;
				Damage = 31;
				XPValue = 275;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 3;
				MoneyDropChance = 1f;
				Speed = 200f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1100f;
				JumpHeight = 0f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.SkeletonArcher_Expert_Scale;
				ProjectileScale = EnemyEV.SkeletonArcher_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.SkeletonArcher_Expert_Tint;
				MeleeRadius = 325;
				ProjectileRadius = 625;
				EngageRadius = 850;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.SkeletonArcher_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Sharpshooter";
				MaxHealth = 600;
				Damage = 40;
				XPValue = 600;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 1f;
				Speed = 250f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1000f;
				JumpHeight = 0f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.SkeletonArcher_Miniboss_Scale;
				ProjectileScale = EnemyEV.SkeletonArcher_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.SkeletonArcher_Miniboss_Tint;
				MeleeRadius = 325;
				ProjectileRadius = 625;
				EngageRadius = 850;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.SkeletonArcher_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherIdle_Character"));
			logicSet.AddAction(new MoveLogicAction(m_target, true, 0f));
			logicSet.AddAction(new DelayLogicAction(0.2f, 0.75f));
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "ArrowProjectile_Sprite",
				SourceAnchor = new Vector2(10f, -20f),
				Target = null,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = true,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = ProjectileScale,
				FollowArc = true
			};
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-20f, -20f);
			logicSet2.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet2.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet2.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet2.AddAction(new DelayLogicAction(m_fireDelay));
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet2.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet2.AddAction(new DelayLogicAction(0.5f));
			logicSet2.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet2.AddAction(new DelayLogicAction(0.5f));
			logicSet2.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-20f, -20f);
			logicSet3.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet3.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet3.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet3.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-20f, -20f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet3.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet3.AddAction(new DelayLogicAction(0.5f));
			logicSet3.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet3.AddAction(new DelayLogicAction(0.5f));
			logicSet3.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-35f, -35f);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet4.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-35f, -35f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-15f, -15f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-55f, -55f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet4.AddAction(new DelayLogicAction(0.5f));
			logicSet4.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet4.AddAction(new DelayLogicAction(0.5f));
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet5.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet5.AddAction(new DelayLogicAction(0.5f));
			logicSet5.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet5.AddAction(new DelayLogicAction(0.5f));
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet6.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet6.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-50f, -50f);
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet6.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet6.AddAction(new DelayLogicAction(0.5f));
			logicSet6.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet6.AddAction(new DelayLogicAction(0.5f));
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet7.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet7.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-45f, -45f);
			logicSet7.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet7.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet7.AddAction(new DelayLogicAction(0.5f));
			logicSet7.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet7.AddAction(new DelayLogicAction(0.5f));
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-70f, -70f);
			logicSet8.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet8.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet8.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet8.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet8.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-70f, -70f);
			logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet8.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet8.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet8.AddAction(new DelayLogicAction(0.5f));
			logicSet8.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet8.AddAction(new DelayLogicAction(0.5f));
			logicSet8.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet9 = new LogicSet(this);
			logicSet9.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-55f, -55f);
			logicSet9.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet9.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet9.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet9.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet9.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-55f, -55f);
			logicSet9.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-85f, -85f);
			logicSet9.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet9.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet9.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet9.AddAction(new DelayLogicAction(0.5f));
			logicSet9.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet9.AddAction(new DelayLogicAction(0.5f));
			logicSet9.AddAction(new LockFaceDirectionLogicAction(false));
			LogicSet logicSet10 = new LogicSet(this);
			logicSet10.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet10.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projectileData.Angle.X));
			logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
			logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Load"));
			logicSet10.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
			logicSet10.AddAction(new DelayLogicAction(m_fireDelay));
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet10.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-85f, -85f);
			logicSet10.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			projectileData.Angle = new Vector2(-95f, -95f);
			logicSet10.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, "SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
			logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
			logicSet10.AddAction(new DelayLogicAction(0.5f));
			logicSet10.AddAction(new ChangePropertyLogicAction(GetChildAt(1), "Rotation", 0));
			logicSet10.AddAction(new DelayLogicAction(0.5f));
			logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
			m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet8, logicSet5);
			m_generalAdvancedLB.AddLogicSet(logicSet, logicSet3, logicSet9, logicSet6);
			m_generalExpertLB.AddLogicSet(logicSet, logicSet4, logicSet10, logicSet7);
			projectileData.Dispose();
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			_objectList[1].TextureColor = TintablePart.TextureColor;
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 100;
				RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				RunLogicBlock(false, m_generalBasicLB, 20, 15, 15, 50);
				return;
			case 2:
				RunLogicBlock(false, m_generalBasicLB, 20, 15, 50, 15);
				return;
			case 3:
				RunLogicBlock(false, m_generalBasicLB, 20, 50, 15, 15);
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
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = m_generalAdvancedLB;
				int[] array = new int[4];
				array[0] = 100;
				RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				RunLogicBlock(false, m_generalAdvancedLB, 20, 15, 15, 50);
				return;
			case 2:
				RunLogicBlock(false, m_generalAdvancedLB, 20, 15, 50, 15);
				return;
			case 3:
				RunLogicBlock(false, m_generalAdvancedLB, 20, 50, 15, 15);
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
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = m_generalExpertLB;
				int[] array = new int[4];
				array[0] = 100;
				RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				RunLogicBlock(false, m_generalExpertLB, 20, 15, 15, 50);
				return;
			case 2:
				RunLogicBlock(false, m_generalExpertLB, 20, 50, 15, 15);
				return;
			case 3:
				RunLogicBlock(false, m_generalExpertLB, 20, 15, 50, 15);
				return;
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
				bool arg_B4_1 = false;
				LogicBlock arg_B4_2 = m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 100;
				RunLogicBlock(arg_B4_1, arg_B4_2, array);
				return;
			}
			case 1:
				RunLogicBlock(false, m_generalBasicLB, 20, 15, 15, 50);
				return;
			case 2:
				RunLogicBlock(false, m_generalBasicLB, 20, 15, 50, 15);
				return;
			case 3:
				RunLogicBlock(false, m_generalBasicLB, 20, 50, 15, 15);
				return;
			default:
				return;
			}
		}
		public void AngleArcher(float angle)
		{
			if (Flip == SpriteEffects.None)
			{
				GetChildAt(1).Rotation = angle;
				return;
			}
			GetChildAt(1).Rotation = -angle;
		}
		public override void ResetState()
		{
			GetChildAt(1).Rotation = 0f;
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
			Type = 25;
			TintablePart = _objectList[0];
		}
	}
}
