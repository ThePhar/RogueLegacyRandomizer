using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Starburst : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private float FireballDelay = 0.5f;
		protected override void InitializeEV()
		{
			base.Name = "Plinky";
			this.MaxHealth = 18;
			base.Damage = 15;
			base.XPValue = 50;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.4f;
			base.Speed = 435f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 435f;
			base.JumpHeight = 950f;
			this.CooldownTime = 0f;
			base.AnimationDelay = 0.05f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			base.IsWeighted = false;
			this.Scale = EnemyEV.Starburst_Basic_Scale;
			base.ProjectileScale = EnemyEV.Starburst_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Starburst_Basic_Tint;
			this.MeleeRadius = 325;
			this.ProjectileRadius = 690;
			this.EngageRadius = 850;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Starburst_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Planky";
				this.MaxHealth = 25;
				base.Damage = 18;
				base.XPValue = 75;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 435f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 435f;
				base.JumpHeight = 950f;
				this.CooldownTime = 0f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Starburst_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Starburst_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Starburst_Advanced_Tint;
				this.MeleeRadius = 325;
				this.EngageRadius = 850;
				this.ProjectileRadius = 690;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Starburst_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Plonky";
				this.MaxHealth = 42;
				base.Damage = 21;
				base.XPValue = 125;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 435f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 435f;
				base.JumpHeight = 950f;
				this.CooldownTime = 0f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Starburst_Expert_Scale;
				base.ProjectileScale = EnemyEV.Starburst_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Starburst_Expert_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 690;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Starburst_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Ploo";
				this.MaxHealth = 750;
				base.Damage = 30;
				base.XPValue = 1100;
				this.MinMoneyDropAmount = 8;
				this.MaxMoneyDropAmount = 16;
				this.MoneyDropChance = 1f;
				base.Speed = 435f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 370f;
				base.JumpHeight = 1350f;
				this.CooldownTime = 0f;
				base.AnimationDelay = 0.05f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Starburst_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Starburst_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Starburst_Miniboss_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 690;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Starburst_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "TurretProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = base.ProjectileScale
			};
			LogicSet logicSet = new LogicSet(this);
			projectileData.Angle = new Vector2(0f, 0f);
			logicSet.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Eyeball_ProjectileAttack"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(90f, 90f);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(180f, 180f);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, 1f, false), Types.Sequence.Serial);
			logicSet.Tag = 2;
			LogicSet logicSet2 = new LogicSet(this);
			projectileData.Angle = new Vector2(45f, 45f);
			logicSet2.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
			logicSet2.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Eyeball_ProjectileAttack"
			}), Types.Sequence.Serial);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(135f, 135f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-135f, -135f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(90f, 90f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(180f, 180f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(0f, 0f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1f, 1f, false), Types.Sequence.Serial);
			logicSet2.Tag = 2;
			LogicSet logicSet3 = new LogicSet(this);
			projectileData.Angle = new Vector2(45f, 45f);
			projectileData.CollidesWithTerrain = false;
			projectileData.SpriteName = "GhostProjectile_Sprite";
			logicSet3.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
			logicSet3.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(this.FireballDelay, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Eyeball_ProjectileAttack"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(135f, 135f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-135f, -135f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(90f, 90f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(180f, 180f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(0f, 0f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, 1f, false), Types.Sequence.Serial);
			logicSet3.AddAction(new RunFunctionLogicAction(this, "FireAnimation", new object[0]), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Eyeball_ProjectileAttack"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(25f, 25f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-25f, -25f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(115f, 115f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-115f, -115f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-70f, -70f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(70f, 70f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(160f, 160f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-160f, -160f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(this._objectList[1], "Rotation", 45), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1.25f, 1.25f, false), Types.Sequence.Serial);
			logicSet3.Tag = 2;
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new DelayLogicAction(0.5f, 0.5f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet4
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet4
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet3,
				logicSet4
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet2,
				logicSet4
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			projectileData.Dispose();
			base.InitializeLogic();
		}
		public void FireAnimation()
		{
			this.ChangeSprite("EnemyStarburstAttack_Character");
			(this._objectList[0] as IAnimateableObj).PlayAnimation(true);
			(this._objectList[1] as IAnimateableObj).PlayAnimation(false);
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalBasicLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalAdvancedLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
			{
				bool arg_33_1 = true;
				LogicBlock arg_33_2 = this.m_generalExpertLB;
				int[] array = new int[2];
				array[0] = 100;
				base.RunLogicBlock(arg_33_1, arg_33_2, array);
				return;
			}
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
				bool arg_38_1 = true;
				LogicBlock arg_38_2 = this.m_generalMiniBossLB;
				int[] array = new int[3];
				array[0] = 60;
				array[1] = 40;
				base.RunLogicBlock(arg_38_1, arg_38_2, array);
				return;
			}
			default:
				return;
			}
		}
		public EnemyObj_Starburst(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyStarburstIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 31;
		}
	}
}
