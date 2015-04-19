using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class EnemyObj_Ninja : EnemyObj
	{
		private LogicBlock m_basicTeleportAttackLB = new LogicBlock();
		private LogicBlock m_expertTeleportAttackLB = new LogicBlock();
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private RoomObj m_storedRoom;
		private SpriteObj m_smoke;
		private SpriteObj m_log;
		private float TeleportDelay = 0.35f;
		private float ChanceToTeleport = 0.35f;
		private float PauseBeforeProjectile = 0.45f;
		private float PauseAfterProjectile = 0.45f;
		private float m_teleportDamageReduc = 0.6f;
		private TerrainObj m_closestCeiling;
		protected override void InitializeEV()
		{
			Name = "Ninjo";
			MaxHealth = 30;
			Damage = 20;
			XPValue = 150;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 2;
			MoneyDropChance = 0.4f;
			Speed = 250f;
			TurnSpeed = 10f;
			ProjectileSpeed = 550f;
			JumpHeight = 600f;
			CooldownTime = 1.5f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = false;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.Ninja_Basic_Scale;
			ProjectileScale = EnemyEV.Ninja_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.Ninja_Basic_Tint;
			MeleeRadius = 225;
			ProjectileRadius = 700;
			EngageRadius = 1000;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.Ninja_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				ChanceToTeleport = 0.5f;
				Name = "Ninpo";
				MaxHealth = 44;
				Damage = 25;
				XPValue = 250;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 325f;
				TurnSpeed = 10f;
				ProjectileSpeed = 625f;
				JumpHeight = 600f;
				CooldownTime = 1.5f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Ninja_Advanced_Scale;
				ProjectileScale = EnemyEV.Ninja_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Ninja_Advanced_Tint;
				MeleeRadius = 225;
				EngageRadius = 1000;
				ProjectileRadius = 700;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Ninja_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				ChanceToTeleport = 0.65f;
				Name = "Ninopojo";
				MaxHealth = 62;
				Damage = 29;
				XPValue = 450;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 400f;
				TurnSpeed = 10f;
				ProjectileSpeed = 700f;
				JumpHeight = 600f;
				CooldownTime = 1.5f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Ninja_Expert_Scale;
				ProjectileScale = EnemyEV.Ninja_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Ninja_Expert_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 700;
				EngageRadius = 1000;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Ninja_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Master Ninja";
				MaxHealth = 900;
				Damage = 38;
				XPValue = 1250;
				MinMoneyDropAmount = 10;
				MaxMoneyDropAmount = 15;
				MoneyDropChance = 1f;
				Speed = 150f;
				TurnSpeed = 10f;
				ProjectileSpeed = 600f;
				JumpHeight = 600f;
				CooldownTime = 1.5f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.Ninja_Miniboss_Scale;
				ProjectileScale = EnemyEV.Ninja_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.Ninja_Miniboss_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 700;
				EngageRadius = 1000;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.Ninja_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "ShurikenProjectile1_Sprite",
				SourceAnchor = new Vector2(15f, 0f),
				Target = m_target,
				Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 20f,
				Damage = Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = ProjectileScale
			};
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(PauseBeforeProjectile, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(PauseAfterProjectile, false), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(PauseBeforeProjectile, false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -10f;
			logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 10f;
			logicSet5.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(PauseAfterProjectile, false), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(PauseBeforeProjectile, false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			projectileData.AngleOffset = 0f;
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -5f;
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 5f;
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -25f;
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 25f;
			logicSet6.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(PauseAfterProjectile, false), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateLog", null), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(TeleportDelay, false), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet7.Tag = 2;
			LogicSet logicSet8 = new LogicSet(this);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateLog", null), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(TeleportDelay, false), Types.Sequence.Serial);
			logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null), Types.Sequence.Serial);
			projectileData.Target = null;
			logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			projectileData.AngleOffset = 45f;
			logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 135f;
			logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -45f;
			logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -135f;
			logicSet8.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet8.Tag = 2;
			m_basicTeleportAttackLB.AddLogicSet(new LogicSet[]
			{
				logicSet7
			});
			m_expertTeleportAttackLB.AddLogicSet(new LogicSet[]
			{
				logicSet8
			});
			logicBlocksToDispose.Add(m_basicTeleportAttackLB);
			logicBlocksToDispose.Add(m_expertTeleportAttackLB);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4
			});
			m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet5
			});
			m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet6
			});
			m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalAdvancedLB);
			logicBlocksToDispose.Add(m_generalExpertLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			LogicBlock arg_906_1 = m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 50;
			array[1] = 50;
			SetCooldownLogicBlock(arg_906_1, array);
			projectileData.Dispose();
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, new int[]
				{
					40,
					30,
					0,
					30
				});
				return;
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = m_generalAdvancedLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = m_generalAdvancedLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalAdvancedLB, new int[]
				{
					40,
					30,
					0,
					30
				});
				return;
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = m_generalExpertLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = m_generalExpertLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalExpertLB, new int[]
				{
					40,
					30,
					0,
					30
				});
				return;
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			switch (State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, new int[]
				{
					40,
					30,
					0,
					30
				});
				return;
			default:
				return;
			}
		}
		public EnemyObj_Ninja(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyNinjaIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			Type = 13;
			m_smoke = new SpriteObj("NinjaSmoke_Sprite");
			m_smoke.AnimationDelay = 0.05f;
			m_log = new SpriteObj("Log_Sprite");
			m_smoke.Visible = false;
			m_smoke.Scale = new Vector2(5f, 5f);
			m_log.Visible = false;
			m_log.OutlineWidth = 2;
		}
		public override void Update(GameTime gameTime)
		{
			if (Y < m_levelScreen.CurrentRoom.Y)
			{
				Y = m_levelScreen.CurrentRoom.Y;
			}
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			if (m_target != null && m_target.CurrentHealth > 0 && m_currentActiveLB != m_basicTeleportAttackLB && m_currentActiveLB != m_expertTeleportAttackLB && CDGMath.RandomFloat(0f, 1f) <= ChanceToTeleport && m_closestCeiling != null)
			{
				m_closestCeiling = FindClosestCeiling();
				int num = TerrainBounds.Top - m_closestCeiling.Bounds.Bottom;
				if (m_closestCeiling != null && num > 150 && num < 700)
				{
					m_currentActiveLB.StopLogicBlock();
					if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
					{
						RunLogicBlock(false, m_expertTeleportAttackLB, new int[]
						{
							100
						});
					}
					else
					{
						RunLogicBlock(false, m_basicTeleportAttackLB, new int[]
						{
							100
						});
					}
					damage = (int)Math.Round(damage * (1f - m_teleportDamageReduc), MidpointRounding.AwayFromZero);
				}
			}
			base.HitEnemy(damage, position, isPlayer);
		}
		private TerrainObj FindClosestCeiling()
		{
			int num = 2147483647;
			TerrainObj result = null;
			RoomObj currentRoom = m_levelScreen.CurrentRoom;
			foreach (TerrainObj current in currentRoom.TerrainObjList)
			{
				Rectangle b = new Rectangle(Bounds.Left, Bounds.Top - 2000, Bounds.Width, Bounds.Height + 2000);
				if (current.CollidesBottom && CollisionMath.Intersects(current.Bounds, b))
				{
					float num2 = 3.40282347E+38f;
					if (current.Bounds.Bottom < TerrainBounds.Top)
					{
						num2 = TerrainBounds.Top - current.Bounds.Bottom;
					}
					if (num2 < num)
					{
						num = (int)num2;
						result = current;
					}
				}
			}
			return result;
		}
		public void CreateLog()
		{
			m_log.Position = Position;
			m_smoke.Position = Position;
			m_smoke.Visible = true;
			m_log.Visible = true;
			m_log.Opacity = 1f;
			m_smoke.PlayAnimation(false);
			Tween.By(m_log, 0.1f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Y",
				"10"
			});
			Tween.To(m_log, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"0"
			});
			SoundManager.Play3DSound(this, m_target, "Ninja_Teleport");
			Visible = false;
			IsCollidable = false;
			IsWeighted = false;
			m_storedRoom = m_levelScreen.CurrentRoom;
		}
		public void CreateSmoke()
		{
			if (m_levelScreen.CurrentRoom == m_storedRoom && m_closestCeiling != null)
			{
				UpdateCollisionBoxes();
				Y = m_closestCeiling.Bounds.Bottom + (Y - TerrainBounds.Top);
				X = m_target.X;
				ChangeSprite("EnemyNinjaAttack_Character");
				Visible = true;
				AccelerationX = 0f;
				AccelerationY = 0f;
				CurrentSpeed = 0f;
				IsCollidable = true;
				m_smoke.Position = Position;
				m_smoke.Visible = true;
				m_smoke.PlayAnimation(false);
				m_closestCeiling = null;
			}
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			m_log.Draw(camera);
			m_smoke.Draw(camera);
		}
		public override void Kill(bool giveXP = true)
		{
			m_smoke.Visible = false;
			m_log.Visible = false;
			base.Kill(giveXP);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_storedRoom = null;
				m_smoke.Dispose();
				m_smoke = null;
				m_log.Dispose();
				m_log = null;
				m_closestCeiling = null;
				base.Dispose();
			}
		}
	}
}
