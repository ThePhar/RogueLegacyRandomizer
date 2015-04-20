/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class EnemyObj_Mimic : EnemyObj
	{
		private bool m_isAttacking;
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private FrameSoundObj m_closeSound;
		protected override void InitializeEV()
		{
			Name = "Mimic";
			MaxHealth = 35;
			Damage = 20;
			XPValue = 75;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 1;
			MoneyDropChance = 0.4f;
			Speed = 400f;
			TurnSpeed = 10f;
			ProjectileSpeed = 775f;
			JumpHeight = 550f;
			CooldownTime = 2f;
			AnimationDelay = 0.05f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = true;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.Mimic_Basic_Scale;
			ProjectileScale = EnemyEV.Mimic_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.Mimic_Basic_Tint;
			MeleeRadius = 10;
			ProjectileRadius = 20;
			EngageRadius = 975;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Mimic_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Mimicant";
				MaxHealth = 40;
				Damage = 23;
				XPValue = 125;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 600f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1100f;
				JumpHeight = 650f;
				CooldownTime = 2f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Mimic_Advanced_Scale;
				ProjectileScale = EnemyEV.Mimic_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Mimic_Advanced_Tint;
				MeleeRadius = 10;
				EngageRadius = 975;
				ProjectileRadius = 20;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Mimic_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Mimicrunch";
				MaxHealth = 70;
				Damage = 25;
				XPValue = 225;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 3;
				MoneyDropChance = 1f;
				Speed = 750f;
				TurnSpeed = 10f;
				ProjectileSpeed = 925f;
				JumpHeight = 550f;
				CooldownTime = 2f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Mimic_Expert_Scale;
				ProjectileScale = EnemyEV.Mimic_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Mimic_Expert_Tint;
				MeleeRadius = 10;
				ProjectileRadius = 20;
				EngageRadius = 975;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Mimic_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Chesticles";
				MaxHealth = 100;
				Damage = 32;
				XPValue = 750;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 0f;
				TurnSpeed = 10f;
				ProjectileSpeed = 900f;
				JumpHeight = 750f;
				CooldownTime = 2f;
				AnimationDelay = 0.05f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Mimic_Miniboss_Scale;
				ProjectileScale = EnemyEV.Mimic_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Mimic_Miniboss_Tint;
				MeleeRadius = 10;
				ProjectileRadius = 20;
				EngageRadius = 975;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Mimic_Miniboss_KnockBack;
				break;
			}
			LockFlip = true;
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyMimicShake_Character", false, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyMimicIdle_Character", true, false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(3f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyMimicAttack_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, m_target, new string[]
			{
				"Chest_Open_Large"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			new LogicSet(this);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2
			});
			logicBlocksToDispose.Add(m_generalBasicLB);
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				//IL_1D:
				if (!m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				RunLogicBlock(false, m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			//goto IL_1D;
		}
		protected override void RunAdvancedLogic()
		{
			switch (State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				//IL_1D:
				if (!m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				RunLogicBlock(false, m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			//goto IL_1D;
		}
		protected override void RunExpertLogic()
		{
			switch (State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				//IL_1D:
				if (!m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				RunLogicBlock(false, m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			//goto IL_1D;
		}
		protected override void RunMinibossLogic()
		{
			switch (State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				//IL_1D:
				if (!m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				RunLogicBlock(false, m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			//goto IL_1D;
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			if (!m_isAttacking)
			{
				m_currentActiveLB.StopLogicBlock();
				m_isAttacking = true;
				LockFlip = false;
			}
			base.HitEnemy(damage, collisionPt, isPlayer);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (otherBox.AbsParent is PlayerObj && !m_isAttacking)
			{
				m_currentActiveLB.StopLogicBlock();
				m_isAttacking = true;
				LockFlip = false;
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public EnemyObj_Mimic(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyMimicIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			Type = 33;
			OutlineWidth = 0;
			m_closeSound = new FrameSoundObj(this, m_target, 1, new string[]
			{
				"Chest_Snap"
			});
		}
		public override void Update(GameTime gameTime)
		{
			if (SpriteName == "EnemyMimicAttack_Character")
			{
				m_closeSound.Update();
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_closeSound.Dispose();
				m_closeSound = null;
				base.Dispose();
			}
		}
	}
}
