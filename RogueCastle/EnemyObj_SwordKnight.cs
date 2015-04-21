using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_SwordKnight : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private float SlashDelay;
		private float SlashTripleDelay = 1.25f;
		private float TripleAttackSpeed = 500f;
		private FrameSoundObj m_walkSound;
		private FrameSoundObj m_walkSound2;
		protected override void InitializeEV()
		{
			this.SlashDelay = 0.25f;
			base.Name = "Gray Knight";
			this.MaxHealth = 40;
			base.Damage = 36;
			base.XPValue = 100;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 75f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 850f;
			base.JumpHeight = 950f;
			this.CooldownTime = 0.5f;
			base.AnimationDelay = 0.0833333358f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.SwordKnight_Basic_Scale;
			base.ProjectileScale = EnemyEV.SwordKnight_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.SwordKnight_Basic_Tint;
			this.MeleeRadius = 210;
			this.ProjectileRadius = 500;
			this.EngageRadius = 800;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.SwordKnight_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.SlashDelay = 0.25f;
				base.Name = "Gray Baron";
				this.MaxHealth = 58;
				base.Damage = 51;
				base.XPValue = 175;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 75f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 850f;
				base.JumpHeight = 950f;
				this.CooldownTime = 1.25f;
				base.AnimationDelay = 0.0833333358f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.SwordKnight_Advanced_Scale;
				base.ProjectileScale = EnemyEV.SwordKnight_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.SwordKnight_Advanced_Tint;
				this.MeleeRadius = 300;
				this.EngageRadius = 800;
				this.ProjectileRadius = 500;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.SwordKnight_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.SlashDelay = 0.25f;
				this.TripleAttackSpeed = 500f;
				base.Name = "Graydiator";
				this.MaxHealth = 72;
				base.Damage = 57;
				base.XPValue = 350;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 85f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1350f;
				base.JumpHeight = 950f;
				this.CooldownTime = 1.25f;
				base.AnimationDelay = 0.0833333358f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.SwordKnight_Expert_Scale;
				base.ProjectileScale = EnemyEV.SwordKnight_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.SwordKnight_Expert_Tint;
				this.MeleeRadius = 375;
				this.ProjectileRadius = 500;
				this.EngageRadius = 800;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.SwordKnight_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.ForceDraw = true;
				this.SlashDelay = 1.05f;
				base.Name = "Graypion";
				this.MaxHealth = 750;
				base.Damage = 80;
				base.XPValue = 800;
				this.MinMoneyDropAmount = 10;
				this.MaxMoneyDropAmount = 15;
				this.MoneyDropChance = 1f;
				base.Speed = 85f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 850f;
				base.JumpHeight = 950f;
				this.CooldownTime = 8f;
				base.AnimationDelay = 0.125f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.SwordKnight_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.SwordKnight_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.SwordKnight_Miniboss_Tint;
				this.MeleeRadius = 300;
				this.ProjectileRadius = 500;
				this.EngageRadius = 800;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.SwordKnight_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightWalk_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightWalk_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.SlashDelay, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SwordKnight_Attack_v02"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.SlashTripleDelay, false), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveDirectionLogicAction(this.TripleAttackSpeed), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0166666675f), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SwordKnight_Attack_v02"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SwordKnight_Attack_v02"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0833333358f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			ProjectileData data = new ProjectileData(this)
			{
				SpriteName = "EnemySpearKnightWave_Sprite",
				SourceAnchor = new Vector2(60f, 0f),
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = true,
				Scale = base.ProjectileScale
			};
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.SlashDelay, false), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SpearKnightAttack1"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(this.SlashTripleDelay, false), Types.Sequence.Serial);
			logicSet7.AddAction(new MoveDirectionLogicAction(this.TripleAttackSpeed), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.008333334f), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SwordKnight_Attack_v02"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SwordKnight_Attack_v02"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SwordKnight_Attack_v02"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet7.AddAction(new MoveLogicAction(null, true, 0f), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0833333358f), Types.Sequence.Serial);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SpearKnightAttack1"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet7.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet7.Tag = 2;
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false), Types.Sequence.Serial);
			logicSet8.AddAction(new PlayAnimationLogicAction("Start", "Windup", false), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(this.SlashDelay, false), Types.Sequence.Serial);
			logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"SpearKnightAttack1"
			}), Types.Sequence.Serial);
			logicSet8.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, data), Types.Sequence.Serial);
			logicSet8.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet8.Tag = 2;
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet7
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
				{
					14,
					11,
					75
				});
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
			case GameTypes.EnemyDifficulty.EXPERT:
				base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
				{
					40,
					30,
					30
				});
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
			{
				LogicBlock arg_9C4_1 = this.m_generalCooldownLB;
				int[] array = new int[3];
				array[0] = 100;
				base.SetCooldownLogicBlock(arg_9C4_1, array);
				break;
			}
			}
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_73_1 = true;
				LogicBlock arg_73_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[2] = 100;
				base.RunLogicBlock(arg_73_1, arg_73_2, array);
				return;
			}
			case 1:
			case 2:
			{
				bool arg_58_1 = true;
				LogicBlock arg_58_2 = this.m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 15;
				array2[1] = 15;
				array2[2] = 70;
				base.RunLogicBlock(arg_58_1, arg_58_2, array2);
				return;
			}
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					0,
					0,
					100
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
				bool arg_78_1 = true;
				LogicBlock arg_78_2 = this.m_generalAdvancedLB;
				int[] array = new int[5];
				array[2] = 100;
				base.RunLogicBlock(arg_78_1, arg_78_2, array);
				return;
			}
			case 1:
			case 2:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalAdvancedLB;
				int[] array2 = new int[5];
				array2[0] = 60;
				array2[1] = 20;
				array2[2] = 20;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 3:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					0,
					0,
					0,
					65,
					35
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
				bool arg_78_1 = true;
				LogicBlock arg_78_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[2] = 100;
				base.RunLogicBlock(arg_78_1, arg_78_2, array);
				return;
			}
			case 1:
			case 2:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 60;
				array2[1] = 20;
				array2[2] = 20;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 3:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					0,
					0,
					0,
					62,
					38
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
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (base.SpriteName == "EnemySwordKnightWalk_Character")
			{
				this.m_walkSound.Update();
				this.m_walkSound2.Update();
			}
			base.Update(gameTime);
		}
		public EnemyObj_SwordKnight(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySwordKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 16;
			this.m_walkSound = new FrameSoundObj(this, this.m_target, 1, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
			this.m_walkSound2 = new FrameSoundObj(this, this.m_target, 6, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
			{
				"Knight_Hit01",
				"Knight_Hit02",
				"Knight_Hit03"
			});
			base.HitEnemy(damage, position, isPlayer);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_walkSound.Dispose();
				this.m_walkSound = null;
				this.m_walkSound2.Dispose();
				this.m_walkSound2 = null;
				base.Dispose();
			}
		}
	}
}
