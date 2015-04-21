using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Zombie : EnemyObj
	{
		private LogicBlock m_basicWalkLS = new LogicBlock();
		private LogicBlock m_basicRiseLowerLS = new LogicBlock();
		public bool Risen
		{
			get;
			set;
		}
		public bool Lowered
		{
			get;
			set;
		}
		protected override void InitializeEV()
		{
			base.Name = "Zombie";
			this.MaxHealth = 24;
			base.Damage = 25;
			base.XPValue = 25;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.4f;
			base.Speed = 160f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 650f;
			base.JumpHeight = 900f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.0833333358f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Zombie_Basic_Scale;
			base.ProjectileScale = EnemyEV.Zombie_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Zombie_Basic_Tint;
			this.MeleeRadius = 100;
			this.ProjectileRadius = 150;
			this.EngageRadius = 435;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Zombie_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Zomboner";
				this.MaxHealth = 39;
				base.Damage = 29;
				base.XPValue = 75;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 260f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 900f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.0714285746f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Zombie_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Zombie_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Zombie_Advanced_Tint;
				this.MeleeRadius = 100;
				this.EngageRadius = 435;
				this.ProjectileRadius = 150;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Zombie_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Zombishnu";
				this.MaxHealth = 70;
				base.Damage = 33;
				base.XPValue = 200;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 350f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 900f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.0625f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Zombie_Expert_Scale;
				base.ProjectileScale = EnemyEV.Zombie_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Zombie_Expert_Tint;
				this.MeleeRadius = 100;
				this.ProjectileRadius = 150;
				this.EngageRadius = 435;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Zombie_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Zomg";
				this.MaxHealth = 800;
				base.Damage = 40;
				base.XPValue = 600;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 1f;
				base.Speed = 600f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 900f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.0714285746f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Zombie_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Zombie_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Zombie_Miniboss_Tint;
				this.MeleeRadius = 100;
				this.ProjectileRadius = 150;
				this.EngageRadius = 435;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Zombie_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyZombieWalk_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Zombie_Groan_01",
				"Zombie_Groan_02",
				"Zombie_Groan_03",
				"Blank",
				"Blank",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, 0f), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyZombieRise_Character", false, false), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Zombie_Rise"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(this, "Risen", true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(this, "Lowered", false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, false, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyZombieLower_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Zombie_Lower"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(this, "Risen", false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(this, "Lowered", true), Types.Sequence.Serial);
			this.m_basicWalkLS.AddLogicSet(new LogicSet[]
			{
				logicSet
			});
			this.m_basicRiseLowerLS.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet3
			});
			this.logicBlocksToDispose.Add(this.m_basicWalkLS);
			this.logicBlocksToDispose.Add(this.m_basicRiseLowerLS);
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
				if (!this.Lowered)
				{
					base.RunLogicBlock(false, this.m_basicRiseLowerLS, new int[]
					{
						0,
						100
					});
				}
				return;
			case 1:
			case 2:
			case 3:
				if (!this.Risen)
				{
					bool arg_3B_1 = false;
					LogicBlock arg_3B_2 = this.m_basicRiseLowerLS;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_basicWalkLS, new int[]
				{
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
				if (!this.Lowered)
				{
					base.RunLogicBlock(false, this.m_basicRiseLowerLS, new int[]
					{
						0,
						100
					});
				}
				return;
			case 1:
			case 2:
			case 3:
				if (!this.Risen)
				{
					bool arg_3B_1 = false;
					LogicBlock arg_3B_2 = this.m_basicRiseLowerLS;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_basicWalkLS, new int[]
				{
					100
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
				if (!this.Lowered)
				{
					base.RunLogicBlock(false, this.m_basicRiseLowerLS, new int[]
					{
						0,
						100
					});
				}
				return;
			case 1:
			case 2:
			case 3:
				if (!this.Risen)
				{
					bool arg_3B_1 = false;
					LogicBlock arg_3B_2 = this.m_basicRiseLowerLS;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_basicWalkLS, new int[]
				{
					100
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
				if (!this.Lowered)
				{
					base.RunLogicBlock(false, this.m_basicRiseLowerLS, new int[]
					{
						0,
						100
					});
				}
				return;
			case 1:
			case 2:
			case 3:
				if (!this.Risen)
				{
					bool arg_3B_1 = false;
					LogicBlock arg_3B_2 = this.m_basicRiseLowerLS;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_basicWalkLS, new int[]
				{
					100
				});
				return;
			default:
				return;
			}
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Zombie_Hit");
			base.HitEnemy(damage, position, isPlayer);
		}
		public override void Update(GameTime gameTime)
		{
			if ((this.m_currentActiveLB == null || !this.m_currentActiveLB.IsActive) && !this.Risen && base.IsAnimating)
			{
				this.ChangeSprite("EnemyZombieRise_Character");
				base.StopAnimation();
			}
			base.Update(gameTime);
		}
		public override void ResetState()
		{
			this.Lowered = true;
			this.Risen = false;
			base.ResetState();
			this.ChangeSprite("EnemyZombieLower_Character");
			base.GoToFrame(base.TotalFrames);
			base.StopAnimation();
		}
		public override void Reset()
		{
			this.ChangeSprite("EnemyZombieRise_Character");
			base.StopAnimation();
			this.Lowered = true;
			this.Risen = false;
			base.Reset();
		}
		public EnemyObj_Zombie(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyZombieLower_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.GoToFrame(base.TotalFrames);
			this.Lowered = true;
			base.ForceDraw = true;
			base.StopAnimation();
			this.Type = 20;
			base.PlayAnimationOnRestart = false;
		}
	}
}
