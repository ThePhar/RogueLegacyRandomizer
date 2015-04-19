using DS2DEngine;
using Microsoft.Xna.Framework;

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
			Name = "Bud";
			MaxHealth = 20;
			Damage = 20;
			XPValue = 25;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 2;
			MoneyDropChance = 0.4f;
			Speed = 125f;
			TurnSpeed = 10f;
			ProjectileSpeed = 900f;
			JumpHeight = 900f;
			CooldownTime = 2f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = false;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.Plant_Basic_Scale;
			ProjectileScale = EnemyEV.Plant_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.Plant_Basic_Tint;
			MeleeRadius = 325;
			ProjectileRadius = 690;
			EngageRadius = 850;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Plant_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Plantite";
				MaxHealth = 28;
				Damage = 23;
				XPValue = 50;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 150f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1000f;
				JumpHeight = 900f;
				CooldownTime = 1.75f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Plant_Advanced_Scale;
				ProjectileScale = EnemyEV.Plant_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Plant_Advanced_Tint;
				MeleeRadius = 325;
				EngageRadius = 850;
				ProjectileRadius = 690;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Plant_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Flowermon";
				MaxHealth = 53;
				Damage = 26;
				XPValue = 175;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 200f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1000f;
				JumpHeight = 900f;
				CooldownTime = 1.25f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Plant_Expert_Scale;
				ProjectileScale = EnemyEV.Plant_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Plant_Expert_Tint;
				MeleeRadius = 325;
				ProjectileRadius = 690;
				EngageRadius = 850;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Plant_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Stolas & Focalor";
				MaxHealth = 165;
				Damage = 28;
				XPValue = 500;
				MinMoneyDropAmount = 11;
				MaxMoneyDropAmount = 18;
				MoneyDropChance = 1f;
				Speed = 450f;
				TurnSpeed = 10f;
				ProjectileSpeed = 700f;
				JumpHeight = 900f;
				CooldownTime = 0.5f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Plant_Miniboss_Scale;
				ProjectileScale = EnemyEV.Plant_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Plant_Miniboss_Tint;
				MeleeRadius = 325;
				ProjectileRadius = 690;
				EngageRadius = 850;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Plant_Miniboss_KnockBack;
				break;
			}
			_objectList[1].TextureColor = new Color(201, 59, 136);
		}
		protected override void InitializeLogic()
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "PlantProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = true,
				RotationSpeed = 0f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = ProjectileScale
			};
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Squirm_01",
				"Enemy_Venus_Squirm_02",
				"Enemy_Venus_Squirm_03"
			}), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.Tag = 2;
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet2.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			logicSet2.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames, false), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet2.Tag = 2;
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-85f, -85f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-95f, -95f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-135f, -135f);
			logicSet3.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames, false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.Tag = 2;
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(1, TotalFrames - 1, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Enemy_Venus_Attack_01"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-60f, -60f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-87f, -87f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-93f, -93f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-75f, -75f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-105f, -105f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-120f, -120f);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(TotalFrames - 1, TotalFrames, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
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
			logicSet6.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
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
			logicSet7.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
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
			logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.25f, 0.45f, false), Types.Sequence.Serial);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet
			});
			m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet2
			});
			m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet3
			});
			m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet4
			});
			m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet5
			});
			m_generalCooldownExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet6,
				logicSet7,
				logicSet8
			});
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalMiniBossLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			logicBlocksToDispose.Add(m_generalCooldownExpertLB);
			SetCooldownLogicBlock(m_generalCooldownLB, new int[]
			{
				100
			});
			if (Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				LogicBlock arg_AA5_1 = m_generalCooldownExpertLB;
				int[] array = new int[3];
				array[0] = 50;
				array[1] = 50;
				SetCooldownLogicBlock(arg_AA5_1, array);
			}
			projectileData.Dispose();
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
				break;
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, new int[]
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
			switch (State)
			{
			case 0:
				break;
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalAdvancedLB, new int[]
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
			switch (State)
			{
			case 0:
				break;
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalExpertLB, new int[]
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
			switch (State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalMiniBossLB, new int[]
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
			Type = 22;
		}
	}
}
