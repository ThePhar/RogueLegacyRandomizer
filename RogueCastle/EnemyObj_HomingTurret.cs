using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
			base.LockFlip = false;
			this.FireDelay = 2f;
			base.Name = "GuardBox";
			this.MaxHealth = 18;
			base.Damage = 20;
			base.XPValue = 75;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.4f;
			base.Speed = 0f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 775f;
			base.JumpHeight = 1035f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.HomingTurret_Basic_Scale;
			base.ProjectileScale = EnemyEV.HomingTurret_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.HomingTurret_Basic_Tint;
			this.MeleeRadius = 10;
			this.ProjectileRadius = 20;
			this.EngageRadius = 975;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.HomingTurret_Basic_KnockBack;
			this.InitialLogicDelay = 1f;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.FireDelay = 1.5f;
				base.Name = "GuardBox XL";
				this.MaxHealth = 25;
				base.Damage = 26;
				base.XPValue = 125;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 0f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1100f;
				base.JumpHeight = 1035f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.HomingTurret_Advanced_Scale;
				base.ProjectileScale = EnemyEV.HomingTurret_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.HomingTurret_Advanced_Tint;
				this.MeleeRadius = 10;
				this.EngageRadius = 975;
				this.ProjectileRadius = 20;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.HomingTurret_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.FireDelay = 2.25f;
				base.Name = "GuardBox 2000";
				this.MaxHealth = 42;
				base.Damage = 30;
				base.XPValue = 225;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 0f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 925f;
				base.JumpHeight = 1035f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.HomingTurret_Expert_Scale;
				base.ProjectileScale = EnemyEV.HomingTurret_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.HomingTurret_Expert_Tint;
				this.MeleeRadius = 10;
				this.ProjectileRadius = 20;
				this.EngageRadius = 975;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.HomingTurret_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "GuardBox Gigasaur";
				this.MaxHealth = 500;
				base.Damage = 40;
				base.XPValue = 750;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 0f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 900f;
				base.JumpHeight = 1035f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.HomingTurret_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.HomingTurret_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.HomingTurret_Miniboss_Tint;
				this.MeleeRadius = 10;
				this.ProjectileRadius = 20;
				this.EngageRadius = 975;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.HomingTurret_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			float arg_06_0 = base.Rotation;
			float num = base.ParseTagToFloat("delay");
			float num2 = base.ParseTagToFloat("speed");
			if (num == 0f)
			{
				Console.WriteLine("ERROR: Turret set with delay of 0. Shoots too fast.");
				num = this.FireDelay;
			}
			if (num2 == 0f)
			{
				num2 = this.ProjectileSpeed;
			}
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "HomingProjectile_Sprite",
				SourceAnchor = new Vector2(35f, 0f),
				Speed = new Vector2(num2, num2),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = base.ProjectileScale,
				FollowArc = false,
				ChaseTarget = false,
				TurnSpeed = 0f,
				StartingRotation = 0f,
				Lifespan = 10f
			};
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect", new object[0]), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Turret_Attack01",
				"Turret_Attack02",
				"Turret_Attack03"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Turret_Attack01",
				"Turret_Attack02",
				"Turret_Attack03"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Turret_Attack01",
				"Turret_Attack02",
				"Turret_Attack03"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.1f, false), Types.Sequence.Serial);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Turret_Attack01",
				"Turret_Attack02",
				"Turret_Attack03"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect", new object[0]), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
			projectileData.ChaseTarget = true;
			projectileData.Target = this.m_target;
			projectileData.TurnSpeed = 0.02f;
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, this.m_target, new string[]
			{
				"Turret_Attack01",
				"Turret_Attack02",
				"Turret_Attack03"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet3,
				logicSet
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet4,
				logicSet
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			projectileData.Dispose();
			base.InitializeLogic();
		}
		public void FireProjectileEffect()
		{
			Vector2 position = base.Position;
			if (this.Flip == SpriteEffects.None)
			{
				position.X += 30f;
			}
			else
			{
				position.X -= 30f;
			}
			this.m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
			this.m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
			this.m_levelScreen.ImpactEffectPool.TurretFireEffect(position, new Vector2(0.5f, 0.5f));
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = this.m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			bool arg_4F_1 = false;
			LogicBlock arg_4F_2 = this.m_generalBasicLB;
			int[] array2 = new int[2];
			array2[0] = 100;
			base.RunLogicBlock(arg_4F_1, arg_4F_2, array2);
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = this.m_generalAdvancedLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			bool arg_4F_1 = false;
			LogicBlock arg_4F_2 = this.m_generalAdvancedLB;
			int[] array2 = new int[2];
			array2[0] = 100;
			base.RunLogicBlock(arg_4F_1, arg_4F_2, array2);
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = this.m_generalExpertLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			base.RunLogicBlock(false, this.m_generalExpertLB, new int[]
			{
				0,
				100
			});
		}
		protected override void RunMinibossLogic()
		{
			switch (base.State)
			{
			case 1:
			case 2:
			case 3:
			{
				bool arg_34_1 = false;
				LogicBlock arg_34_2 = this.m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_34_1, arg_34_2, array);
				return;
			}
			}
			bool arg_4F_1 = false;
			LogicBlock arg_4F_2 = this.m_generalBasicLB;
			int[] array2 = new int[2];
			array2[0] = 100;
			base.RunLogicBlock(arg_4F_1, arg_4F_2, array2);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		public EnemyObj_HomingTurret(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyHomingTurret_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.StopAnimation();
			base.ForceDraw = true;
			this.Type = 28;
			base.PlayAnimationOnRestart = false;
		}
	}
}
