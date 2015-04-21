/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class EnemyObj_HomingTurret : EnemyObj
	{
		private float FireDelay = 5f;
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		protected override void InitializeEV()
		{
			LockFlip = false;
			FireDelay = 2f;
			Name = "GuardBox";
			MaxHealth = 18;
			Damage = 20;
			XPValue = 75;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 1;
			MoneyDropChance = 0.4f;
			Speed = 0f;
			TurnSpeed = 10f;
			ProjectileSpeed = 775f;
			JumpHeight = 1035f;
			CooldownTime = 2f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = false;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.HomingTurret_Basic_Scale;
			ProjectileScale = EnemyEV.HomingTurret_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.HomingTurret_Basic_Tint;
			MeleeRadius = 10;
			ProjectileRadius = 20;
			EngageRadius = 975;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.HomingTurret_Basic_KnockBack;
			InitialLogicDelay = 1f;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				FireDelay = 1.5f;
				Name = "GuardBox XL";
				MaxHealth = 25;
				Damage = 26;
				XPValue = 125;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 0f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1100f;
				JumpHeight = 1035f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.HomingTurret_Advanced_Scale;
				ProjectileScale = EnemyEV.HomingTurret_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.HomingTurret_Advanced_Tint;
				MeleeRadius = 10;
				EngageRadius = 975;
				ProjectileRadius = 20;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.HomingTurret_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				FireDelay = 2.25f;
				Name = "GuardBox 2000";
				MaxHealth = 42;
				Damage = 30;
				XPValue = 225;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 3;
				MoneyDropChance = 1f;
				Speed = 0f;
				TurnSpeed = 10f;
				ProjectileSpeed = 925f;
				JumpHeight = 1035f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.HomingTurret_Expert_Scale;
				ProjectileScale = EnemyEV.HomingTurret_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.HomingTurret_Expert_Tint;
				MeleeRadius = 10;
				ProjectileRadius = 20;
				EngageRadius = 975;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.HomingTurret_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "GuardBox Gigasaur";
				MaxHealth = 500;
				Damage = 40;
				XPValue = 750;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 0f;
				TurnSpeed = 10f;
				ProjectileSpeed = 900f;
				JumpHeight = 1035f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.HomingTurret_Miniboss_Scale;
				ProjectileScale = EnemyEV.HomingTurret_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.HomingTurret_Miniboss_Tint;
				MeleeRadius = 10;
				ProjectileRadius = 20;
				EngageRadius = 975;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.HomingTurret_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			float arg_06_0 = Rotation;
			float num = ParseTagToFloat("delay");
			float num2 = ParseTagToFloat("speed");
			if (num == 0f)
			{
				Console.WriteLine("ERROR: Turret set with delay of 0. Shoots too fast.");
				num = FireDelay;
			}
			if (num2 == 0f)
			{
				num2 = ProjectileSpeed;
			}
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "HomingProjectile_Sprite",
				SourceAnchor = new Vector2(35f, 0f),
				Speed = new Vector2(num2, num2),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = ProjectileScale,
				FollowArc = false,
				ChaseTarget = false,
				TurnSpeed = 0f,
				StartingRotation = 0f,
				Lifespan = 10f
			};
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new DelayLogicAction(0.5f));
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet2.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect"));
			logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
			logicSet2.AddAction(new DelayLogicAction(num));
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
			logicSet3.AddAction(new DelayLogicAction(0.1f));
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
			logicSet3.AddAction(new DelayLogicAction(0.1f));
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet3.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
			logicSet3.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect"));
			logicSet3.AddAction(new DelayLogicAction(num));
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
			projectileData.ChaseTarget = true;
			projectileData.Target = m_target;
			projectileData.TurnSpeed = 0.02f;
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData));
			logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
			logicSet4.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect"));
			logicSet4.AddAction(new DelayLogicAction(num));
			m_generalBasicLB.AddLogicSet(logicSet2, logicSet);
			m_generalAdvancedLB.AddLogicSet(logicSet3, logicSet);
			m_generalExpertLB.AddLogicSet(logicSet4, logicSet);
			m_generalMiniBossLB.AddLogicSet(logicSet2, logicSet);
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalMiniBossLB);
			projectileData.Dispose();
			base.InitializeLogic();
		}
		public void FireProjectileEffect()
		{
			Vector2 position = Position;
			if (Flip == SpriteEffects.None)
			{
				position.X += 30f;
			}
			else
			{
				position.X -= 30f;
			}
			m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
			m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
			m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			bool arg_4F_1 = false;
			LogicBlock arg_4F_2 = m_generalBasicLB;
			int[] array2 = new int[2];
			array2[0] = 100;
			RunLogicBlock(arg_4F_1, arg_4F_2, array2);
		}
		protected override void RunAdvancedLogic()
		{
			switch (State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = m_generalAdvancedLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			bool arg_4F_1 = false;
			LogicBlock arg_4F_2 = m_generalAdvancedLB;
			int[] array2 = new int[2];
			array2[0] = 100;
			RunLogicBlock(arg_4F_1, arg_4F_2, array2);
		}
		protected override void RunExpertLogic()
		{
			switch (State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = m_generalExpertLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			RunLogicBlock(false, m_generalExpertLB, 0, 100);
		}
		protected override void RunMinibossLogic()
		{
			switch (State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			bool arg_4F_1 = false;
			LogicBlock arg_4F_2 = m_generalBasicLB;
			int[] array2 = new int[2];
			array2[0] = 100;
			RunLogicBlock(arg_4F_1, arg_4F_2, array2);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		public EnemyObj_HomingTurret(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyHomingTurret_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			StopAnimation();
			ForceDraw = true;
			Type = 28;
			PlayAnimationOnRestart = false;
		}
	}
}
