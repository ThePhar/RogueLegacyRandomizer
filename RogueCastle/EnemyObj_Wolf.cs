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
	public class EnemyObj_Wolf : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private LogicBlock m_wolfHitLB = new LogicBlock();
		private float PounceDelay = 0.3f;
		private float PounceLandDelay = 0.5f;
		private Color FurColour = Color.White;
		private float m_startDelay = 1f;
		private float m_startDelayCounter;
		private FrameSoundObj m_runFrameSound;
		public bool Chasing
		{
			get;
			set;
		}
		protected override void InitializeEV()
		{
			Name = "Warg";
			MaxHealth = 18;
			Damage = 25;
			XPValue = 75;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 2;
			MoneyDropChance = 0.4f;
			Speed = 600f;
			TurnSpeed = 10f;
			ProjectileSpeed = 650f;
			JumpHeight = 1035f;
			CooldownTime = 2f;
			AnimationDelay = 0.0333333351f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = true;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.Wolf_Basic_Scale;
			ProjectileScale = EnemyEV.Wolf_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.Wolf_Basic_Tint;
			MeleeRadius = 50;
			ProjectileRadius = 400;
			EngageRadius = 550;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Wolf_Basic_KnockBack;
			InitialLogicDelay = 1f;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Wargen";
				MaxHealth = 25;
				Damage = 28;
				XPValue = 125;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 700f;
				TurnSpeed = 10f;
				ProjectileSpeed = 650f;
				JumpHeight = 1035f;
				CooldownTime = 2f;
				AnimationDelay = 0.0333333351f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Wolf_Advanced_Scale;
				ProjectileScale = EnemyEV.Wolf_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Wolf_Advanced_Tint;
				MeleeRadius = 50;
				EngageRadius = 575;
				ProjectileRadius = 400;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Wolf_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Wargenflorgen";
				MaxHealth = 52;
				Damage = 32;
				XPValue = 225;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 3;
				MoneyDropChance = 1f;
				Speed = 850f;
				TurnSpeed = 10f;
				ProjectileSpeed = 650f;
				JumpHeight = 1035f;
				CooldownTime = 2f;
				AnimationDelay = 0.0333333351f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Wolf_Expert_Scale;
				ProjectileScale = EnemyEV.Wolf_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Wolf_Expert_Tint;
				MeleeRadius = 50;
				ProjectileRadius = 400;
				EngageRadius = 625;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Wolf_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Zorg Warg";
				MaxHealth = 500;
				Damage = 35;
				XPValue = 750;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 1f;
				Speed = 925f;
				TurnSpeed = 10f;
				ProjectileSpeed = 650f;
				JumpHeight = 1035f;
				CooldownTime = 2f;
				AnimationDelay = 0.0333333351f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = true;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Wolf_Miniboss_Scale;
				ProjectileScale = EnemyEV.Wolf_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Wolf_Miniboss_Tint;
				MeleeRadius = 50;
				ProjectileRadius = 400;
				EngageRadius = 700;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Wolf_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyWargRun_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "Chasing", true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(this, "Chasing", false), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Wolf_Attack"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargPounce_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(PounceDelay, false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargJump_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
			logicSet3.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet3.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(PounceLandDelay, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyWargHit_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			m_wolfHitLB.AddLogicSet(new LogicSet[]
			{
				logicSet4
			});
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			logicBlocksToDispose.Add(m_wolfHitLB);
			SetCooldownLogicBlock(m_generalCooldownLB, new int[]
			{
				40,
				40,
				20
			});
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			if (m_startDelayCounter <= 0f)
			{
				switch (State)
				{
				case 0:
					if (Chasing)
					{
						bool arg_C7_1 = false;
						LogicBlock arg_C7_2 = m_generalBasicLB;
						int[] array = new int[3];
						array[1] = 100;
						RunLogicBlock(arg_C7_1, arg_C7_2, array);
					}
					break;
				case 1:
					if (!Chasing)
					{
						bool arg_A1_1 = false;
						LogicBlock arg_A1_2 = m_generalBasicLB;
						int[] array2 = new int[3];
						array2[0] = 100;
						RunLogicBlock(arg_A1_1, arg_A1_2, array2);
						return;
					}
					break;
				case 2:
				case 3:
				{
					if (m_target.Y < Y - m_target.Height)
					{
						RunLogicBlock(false, m_generalBasicLB, new int[]
						{
							0,
							0,
							100
						});
						return;
					}
					bool arg_7E_1 = false;
					LogicBlock arg_7E_2 = m_generalBasicLB;
					int[] array3 = new int[3];
					array3[0] = 100;
					RunLogicBlock(arg_7E_1, arg_7E_2, array3);
					return;
				}
				default:
					return;
				}
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (State)
			{
			case 0:
				if (Chasing)
				{
					bool arg_B7_1 = false;
					LogicBlock arg_B7_2 = m_generalBasicLB;
					int[] array = new int[3];
					array[1] = 100;
					RunLogicBlock(arg_B7_1, arg_B7_2, array);
				}
				break;
			case 1:
				if (!Chasing)
				{
					bool arg_91_1 = false;
					LogicBlock arg_91_2 = m_generalBasicLB;
					int[] array2 = new int[3];
					array2[0] = 100;
					RunLogicBlock(arg_91_1, arg_91_2, array2);
					return;
				}
				break;
			case 2:
			case 3:
			{
				if (m_target.Y < Y - m_target.Height)
				{
					RunLogicBlock(false, m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_6E_1 = false;
				LogicBlock arg_6E_2 = m_generalBasicLB;
				int[] array3 = new int[3];
				array3[0] = 100;
				RunLogicBlock(arg_6E_1, arg_6E_2, array3);
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
				if (Chasing)
				{
					bool arg_B7_1 = false;
					LogicBlock arg_B7_2 = m_generalBasicLB;
					int[] array = new int[3];
					array[1] = 100;
					RunLogicBlock(arg_B7_1, arg_B7_2, array);
				}
				break;
			case 1:
				if (!Chasing)
				{
					bool arg_91_1 = false;
					LogicBlock arg_91_2 = m_generalBasicLB;
					int[] array2 = new int[3];
					array2[0] = 100;
					RunLogicBlock(arg_91_1, arg_91_2, array2);
					return;
				}
				break;
			case 2:
			case 3:
			{
				if (m_target.Y < Y - m_target.Height)
				{
					RunLogicBlock(false, m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_6E_1 = false;
				LogicBlock arg_6E_2 = m_generalBasicLB;
				int[] array3 = new int[3];
				array3[0] = 100;
				RunLogicBlock(arg_6E_1, arg_6E_2, array3);
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
				if (Chasing)
				{
					bool arg_B7_1 = false;
					LogicBlock arg_B7_2 = m_generalBasicLB;
					int[] array = new int[3];
					array[1] = 100;
					RunLogicBlock(arg_B7_1, arg_B7_2, array);
				}
				break;
			case 1:
				if (!Chasing)
				{
					bool arg_91_1 = false;
					LogicBlock arg_91_2 = m_generalBasicLB;
					int[] array2 = new int[3];
					array2[0] = 100;
					RunLogicBlock(arg_91_1, arg_91_2, array2);
					return;
				}
				break;
			case 2:
			case 3:
			{
				if (m_target.Y < Y - m_target.Height)
				{
					RunLogicBlock(false, m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_6E_1 = false;
				LogicBlock arg_6E_2 = m_generalBasicLB;
				int[] array3 = new int[3];
				array3[0] = 100;
				RunLogicBlock(arg_6E_1, arg_6E_2, array3);
				return;
			}
			default:
				return;
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (m_startDelayCounter > 0f)
			{
				m_startDelayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (!m_isTouchingGround && IsWeighted && CurrentSpeed == 0f && SpriteName == "EnemyWargJump_Character")
			{
				CurrentSpeed = Speed;
			}
			base.Update(gameTime);
			if (m_isTouchingGround && CurrentSpeed == 0f && !IsAnimating)
			{
				ChangeSprite("EnemyWargIdle_Character");
				PlayAnimation(true);
			}
			if (SpriteName == "EnemyWargRun_Character")
			{
				m_runFrameSound.Update();
			}
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
			{
				"Wolf_Hit_01",
				"Wolf_Hit_02",
				"Wolf_Hit_03"
			});
			if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
			{
				m_currentActiveLB.StopLogicBlock();
			}
			m_currentActiveLB = m_wolfHitLB;
			m_currentActiveLB.RunLogicBlock(new int[]
			{
				100
			});
			base.HitEnemy(damage, position, isPlayer);
		}
		public override void ResetState()
		{
			m_startDelayCounter = m_startDelay;
			base.ResetState();
		}
		public override void Reset()
		{
			m_startDelayCounter = m_startDelay;
			base.Reset();
		}
		public EnemyObj_Wolf(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyWargIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			Type = 19;
			m_startDelayCounter = m_startDelay;
			m_runFrameSound = new FrameSoundObj(this, 1, new string[]
			{
				"Wolf_Move01",
				"Wolf_Move02",
				"Wolf_Move03"
			});
		}
	}
}
