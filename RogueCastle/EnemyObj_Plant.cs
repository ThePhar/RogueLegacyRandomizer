using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Plant : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private LogicBlock m_generalCooldownExpertLB = new LogicBlock();
		protected override void InitializeEV()
		{
			base.Name = "Bud";
			this.MaxHealth = 20;
			base.Damage = 20;
			base.XPValue = 25;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 125f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 900f;
			base.JumpHeight = 900f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Plant_Basic_Scale;
			base.ProjectileScale = EnemyEV.Plant_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Plant_Basic_Tint;
			this.MeleeRadius = 325;
			this.ProjectileRadius = 690;
			this.EngageRadius = 850;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Plant_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Plantite";
				this.MaxHealth = 28;
				base.Damage = 23;
				base.XPValue = 50;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 150f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1000f;
				base.JumpHeight = 900f;
				this.CooldownTime = 1.75f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Plant_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Plant_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Plant_Advanced_Tint;
				this.MeleeRadius = 325;
				this.EngageRadius = 850;
				this.ProjectileRadius = 690;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Plant_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Flowermon";
				this.MaxHealth = 53;
				base.Damage = 26;
				base.XPValue = 175;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 200f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1000f;
				base.JumpHeight = 900f;
				this.CooldownTime = 1.25f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Plant_Expert_Scale;
				base.ProjectileScale = EnemyEV.Plant_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Plant_Expert_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 690;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Plant_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Stolas & Focalor";
				this.MaxHealth = 165;
				base.Damage = 28;
				base.XPValue = 500;
				this.MinMoneyDropAmount = 11;
				this.MaxMoneyDropAmount = 18;
				this.MoneyDropChance = 1f;
				base.Speed = 450f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 700f;
				base.JumpHeight = 900f;
				this.CooldownTime = 0.5f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Plant_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Plant_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Plant_Miniboss_Tint;
				this.MeleeRadius = 325;
				this.ProjectileRadius = 690;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Plant_Miniboss_KnockBack;
				break;
			}
			this._objectList[1].TextureColor = new Color(201, 59, 136);
		}
		protected override void InitializeLogic()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "PlantProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = true,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = base.ProjectileScale
			};
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Squirm_01",
				"Enemy_Venus_Squirm_02",
				"Enemy_Venus_Squirm_03"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(1, base.TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(base.TotalFrames - 1, base.TotalFrames, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.Tag = 2;
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(1, base.TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			logicSet2.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(base.TotalFrames - 1, base.TotalFrames, false), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.Tag = 2;
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(1, base.TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-85f, -85f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-95f, -95f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-135f, -135f);
			logicSet3.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(base.TotalFrames - 1, base.TotalFrames, false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.Tag = 2;
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(1, base.TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-87f, -87f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-93f, -93f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(base.TotalFrames - 1, base.TotalFrames, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Squirm_01",
				"Enemy_Venus_Squirm_02",
				"Enemy_Venus_Squirm_03",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(0.25f, 0.45f, false), Types.Sequence.Serial);
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Squirm_01",
				"Enemy_Venus_Squirm_02",
				"Enemy_Venus_Squirm_03",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet7.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.25f, 0.45f, false), Types.Sequence.Serial);
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Squirm_01",
				"Enemy_Venus_Squirm_02",
				"Enemy_Venus_Squirm_03",
				"Blank",
				"Blank",
				"Blank"
			}), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet8.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.25f, 0.45f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet2
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet3
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet4
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet5
			});
			this.m_generalCooldownExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet6,
				logicSet7,
				logicSet8
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownExpertLB);
			base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
			{
				100
			});
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				LogicBlock arg_AA5_1 = this.m_generalCooldownExpertLB;
				int[] array = new int[3];
				array[0] = 50;
				array[1] = 50;
				base.SetCooldownLogicBlock(arg_AA5_1, array);
			}
			projectileData.Dispose();
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
				break;
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					100
				});
				break;
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
				break;
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					100
				});
				break;
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
				break;
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					100
				});
				break;
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
				base.RunLogicBlock(true, this.m_generalMiniBossLB, new int[]
				{
					100
				});
				return;
			default:
				return;
			}
		}
		public EnemyObj_Plant(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyPlantIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 22;
		}
	}
}
