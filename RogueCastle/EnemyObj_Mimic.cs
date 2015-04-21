using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Mimic : EnemyObj
	{
		private bool m_isAttacking;
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private FrameSoundObj m_closeSound;
		protected override void InitializeEV()
		{
			base.Name = "Mimic";
			this.MaxHealth = 35;
			base.Damage = 20;
			base.XPValue = 75;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.4f;
			base.Speed = 400f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 775f;
			base.JumpHeight = 550f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.05f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = true;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Mimic_Basic_Scale;
			base.ProjectileScale = EnemyEV.Mimic_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Mimic_Basic_Tint;
			this.MeleeRadius = 10;
			this.ProjectileRadius = 20;
			this.EngageRadius = 975;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Mimic_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Mimicant";
				this.MaxHealth = 40;
				base.Damage = 23;
				base.XPValue = 125;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 600f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1100f;
				base.JumpHeight = 650f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Mimic_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Mimic_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Mimic_Advanced_Tint;
				this.MeleeRadius = 10;
				this.EngageRadius = 975;
				this.ProjectileRadius = 20;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Mimic_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Mimicrunch";
				this.MaxHealth = 70;
				base.Damage = 25;
				base.XPValue = 225;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 750f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 925f;
				base.JumpHeight = 550f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Mimic_Expert_Scale;
				base.ProjectileScale = EnemyEV.Mimic_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Mimic_Expert_Tint;
				this.MeleeRadius = 10;
				this.ProjectileRadius = 20;
				this.EngageRadius = 975;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Mimic_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Chesticles";
				this.MaxHealth = 100;
				base.Damage = 32;
				base.XPValue = 750;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 0f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 900f;
				base.JumpHeight = 750f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = true;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Mimic_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Mimic_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Mimic_Miniboss_Tint;
				this.MeleeRadius = 10;
				this.ProjectileRadius = 20;
				this.EngageRadius = 975;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Mimic_Miniboss_KnockBack;
				break;
			}
			base.LockFlip = true;
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
			logicSet2.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyMimicAttack_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Chest_Open_Large"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new JumpLogicAction(0f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.3f, false), Types.Sequence.Serial);
			logicSet2.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			new LogicSet(this);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				IL_1D:
				if (!this.m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			goto IL_1D;
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				IL_1D:
				if (!this.m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			goto IL_1D;
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				IL_1D:
				if (!this.m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			goto IL_1D;
		}
		protected override void RunMinibossLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				IL_1D:
				if (!this.m_isAttacking)
				{
					bool arg_3A_1 = false;
					LogicBlock arg_3A_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3A_1, arg_3A_2, array);
					return;
				}
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			}
			goto IL_1D;
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			if (!this.m_isAttacking)
			{
				this.m_currentActiveLB.StopLogicBlock();
				this.m_isAttacking = true;
				base.LockFlip = false;
			}
			base.HitEnemy(damage, collisionPt, isPlayer);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (otherBox.AbsParent is PlayerObj && !this.m_isAttacking)
			{
				this.m_currentActiveLB.StopLogicBlock();
				this.m_isAttacking = true;
				base.LockFlip = false;
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public EnemyObj_Mimic(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyMimicIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 33;
			base.OutlineWidth = 0;
			this.m_closeSound = new FrameSoundObj(this, this.m_target, 1, new string[]
			{
				"Chest_Snap"
			});
		}
		public override void Update(GameTime gameTime)
		{
			if (base.SpriteName == "EnemyMimicAttack_Character")
			{
				this.m_closeSound.Update();
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_closeSound.Dispose();
				this.m_closeSound = null;
				base.Dispose();
			}
		}
	}
}
