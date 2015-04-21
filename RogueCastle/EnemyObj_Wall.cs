using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Wall : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		protected override void InitializeEV()
		{
			base.XPValue = 5000;
			this.MaxHealth = 10000;
			base.Damage = 900;
			this.ProjectileDamage = 50;
			base.IsWeighted = false;
			this.Scale = new Vector2(1f, 3f);
			base.ProjectileScale = new Vector2(3f, 3f);
			base.Speed = 1.75f;
			this.EngageRadius = 1900;
			this.ProjectileRadius = 1600;
			this.MeleeRadius = 650;
			this.CooldownTime = 2f;
			base.KnockBack = new Vector2(5f, 6f);
			base.JumpHeight = 20.5f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
			case GameTypes.EnemyDifficulty.ADVANCED:
			case GameTypes.EnemyDifficulty.EXPERT:
			case GameTypes.EnemyDifficulty.MINIBOSS:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.5f, 1.25f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.5f, 1.25f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ResizeProjectile", new object[]
			{
				true
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ResizeProjectile", new object[]
			{
				false
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(3f, false), Types.Sequence.Serial);
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "FireRandomProjectile", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "FireRandomProjectile", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "FireRandomProjectile", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(1.3f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5
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
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
			{
				40,
				40,
				20
			});
			base.InitializeLogic();
		}
		public void FireRandomProjectile()
		{
		}
		public void ResizeProjectile(bool resize)
		{
			if (resize)
			{
				base.ProjectileScale = new Vector2(2.5f, 2.5f);
				return;
			}
			base.ProjectileScale = new Vector2(2.5f, 2.5f);
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(false, this.m_generalBasicLB, new int[]
				{
					0,
					0,
					0,
					20,
					80
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
			case 1:
			case 2:
			case 3:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
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
				return;
			}
		}
		public override void Update(GameTime gameTime)
		{
			base.HeadingY = 0f;
			if (base.HeadingX > 0f)
			{
				base.HeadingX = 1f;
			}
			else if (base.HeadingX < 0f)
			{
				base.HeadingX = -1f;
			}
			base.Update(gameTime);
			base.Y -= base.HeadingY;
		}
		public EnemyObj_Wall(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.CollisionTypeTag = 4;
			this.Type = 18;
		}
	}
}
