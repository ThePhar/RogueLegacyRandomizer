using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Eagle : EnemyObj
	{
		private bool m_flyingLeft;
		private LogicBlock m_basicAttackLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		protected override void InitializeEV()
		{
			this.MaxHealth = 10;
			base.Damage = 10;
			base.XPValue = 5;
			this.HealthGainPerLevel = 2;
			this.DamageGainPerLevel = 2;
			this.XPBonusPerLevel = 1;
			base.IsWeighted = false;
			base.Speed = 6f;
			this.EngageRadius = 1900;
			this.ProjectileRadius = 1600;
			this.MeleeRadius = 650;
			this.CooldownTime = 2f;
			base.CanBeKnockedBack = false;
			base.JumpHeight = 20.5f;
			this.CanFallOffLedges = true;
			this.TurnSpeed = 0.0175f;
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
			logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			this.m_basicAttackLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet
			});
			this.logicBlocksToDispose.Add(this.m_basicAttackLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
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
				if (this.m_flyingLeft)
				{
					bool arg_3B_1 = false;
					LogicBlock arg_3B_2 = this.m_basicAttackLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3B_1, arg_3B_2, array);
				}
				else
				{
					base.RunLogicBlock(false, this.m_basicAttackLB, new int[]
					{
						0,
						100
					});
				}
				if (base.X > (float)this.m_levelScreen.CurrentRoom.Bounds.Right)
				{
					base.Y = this.m_levelScreen.CurrentRoom.Y + (float)CDGMath.RandomInt(100, this.m_levelScreen.CurrentRoom.Height - 100);
					this.m_flyingLeft = true;
					return;
				}
				if (base.X < (float)this.m_levelScreen.CurrentRoom.Bounds.Left)
				{
					base.Y = this.m_levelScreen.CurrentRoom.Y + (float)CDGMath.RandomInt(100, this.m_levelScreen.CurrentRoom.Height - 100);
					this.m_flyingLeft = false;
				}
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
			if (!base.IsAnimating)
			{
				base.PlayAnimation(true);
			}
			base.Update(gameTime);
		}
		public EnemyObj_Eagle(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("Dummy_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 4;
		}
	}
}
