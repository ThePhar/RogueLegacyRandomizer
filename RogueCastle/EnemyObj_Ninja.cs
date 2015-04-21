using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
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
			base.Name = "Ninjo";
			this.MaxHealth = 30;
			base.Damage = 20;
			base.XPValue = 150;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 250f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 550f;
			base.JumpHeight = 600f;
			this.CooldownTime = 1.5f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Ninja_Basic_Scale;
			base.ProjectileScale = EnemyEV.Ninja_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Ninja_Basic_Tint;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 700;
			this.EngageRadius = 1000;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Ninja_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.ChanceToTeleport = 0.5f;
				base.Name = "Ninpo";
				this.MaxHealth = 44;
				base.Damage = 25;
				base.XPValue = 250;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 325f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 625f;
				base.JumpHeight = 600f;
				this.CooldownTime = 1.5f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Ninja_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Ninja_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Ninja_Advanced_Tint;
				this.MeleeRadius = 225;
				this.EngageRadius = 1000;
				this.ProjectileRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Ninja_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.ChanceToTeleport = 0.65f;
				base.Name = "Ninopojo";
				this.MaxHealth = 62;
				base.Damage = 29;
				base.XPValue = 450;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 400f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 700f;
				base.JumpHeight = 600f;
				this.CooldownTime = 1.5f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Ninja_Expert_Scale;
				base.ProjectileScale = EnemyEV.Ninja_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Ninja_Expert_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 1000;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Ninja_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Master Ninja";
				this.MaxHealth = 900;
				base.Damage = 38;
				base.XPValue = 1250;
				this.MinMoneyDropAmount = 10;
				this.MaxMoneyDropAmount = 15;
				this.MoneyDropChance = 1f;
				base.Speed = 150f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 600f;
				base.JumpHeight = 600f;
				this.CooldownTime = 1.5f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Ninja_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Ninja_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Ninja_Miniboss_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 700;
				this.EngageRadius = 1000;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Ninja_Miniboss_KnockBack;
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
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyNinjaRun_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.25f, 0.85f, false), Types.Sequence.Serial);
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "ShurikenProjectile1_Sprite",
				SourceAnchor = new Vector2(15f, 0f),
				Target = this.m_target,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 20f,
				Damage = base.Damage,
				AngleOffset = 0f,
				CollidesWithTerrain = true,
				Scale = base.ProjectileScale
			};
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.PauseBeforeProjectile, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(this.PauseAfterProjectile, false), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.PauseBeforeProjectile, false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
			logicSet5.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			logicSet5.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -10f;
			logicSet5.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 10f;
			logicSet5.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(this.PauseAfterProjectile, false), Types.Sequence.Serial);
			logicSet5.Tag = 2;
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaIdle_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.PauseBeforeProjectile, false), Types.Sequence.Serial);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyNinjaThrow_Character", false, false), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction(1, 3, false), Types.Sequence.Serial);
			logicSet6.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			projectileData.AngleOffset = 0f;
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -5f;
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 5f;
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -25f;
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 25f;
			logicSet6.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet6.AddAction(new PlayAnimationLogicAction(4, 5, false), Types.Sequence.Serial);
			logicSet6.AddAction(new DelayLogicAction(this.PauseAfterProjectile, false), Types.Sequence.Serial);
			logicSet6.Tag = 2;
			LogicSet logicSet7 = new LogicSet(this);
			logicSet7.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet7.AddAction(new RunFunctionLogicAction(this, "CreateLog", null), Types.Sequence.Serial);
			logicSet7.AddAction(new DelayLogicAction(this.TeleportDelay, false), Types.Sequence.Serial);
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
			logicSet8.AddAction(new DelayLogicAction(this.TeleportDelay, false), Types.Sequence.Serial);
			logicSet8.AddAction(new RunFunctionLogicAction(this, "CreateSmoke", null), Types.Sequence.Serial);
			projectileData.Target = null;
			logicSet8.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"Ninja_ThrowStar_01",
				"Ninja_ThrowStar_02",
				"Ninja_ThrowStar_03"
			}), Types.Sequence.Serial);
			projectileData.AngleOffset = 45f;
			logicSet8.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = 135f;
			logicSet8.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -45f;
			logicSet8.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.AngleOffset = -135f;
			logicSet8.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new GroundCheckLogicAction(), Types.Sequence.Serial);
			logicSet8.AddAction(new DelayLogicAction(0.15f, false), Types.Sequence.Serial);
			logicSet8.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet8.Tag = 2;
			this.m_basicTeleportAttackLB.AddLogicSet(new LogicSet[]
			{
				logicSet7
			});
			this.m_expertTeleportAttackLB.AddLogicSet(new LogicSet[]
			{
				logicSet8
			});
			this.logicBlocksToDispose.Add(this.m_basicTeleportAttackLB);
			this.logicBlocksToDispose.Add(this.m_expertTeleportAttackLB);
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
				logicSet5
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet6
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
			LogicBlock arg_906_1 = this.m_generalCooldownLB;
			int[] array = new int[3];
			array[0] = 50;
			array[1] = 50;
			base.SetCooldownLogicBlock(arg_906_1, array);
			projectileData.Dispose();
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
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
			switch (base.State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = this.m_generalAdvancedLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalAdvancedLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
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
			switch (base.State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = this.m_generalExpertLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalExpertLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
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
			switch (base.State)
			{
			case 0:
			{
				bool arg_7D_1 = true;
				LogicBlock arg_7D_2 = this.m_generalBasicLB;
				int[] array = new int[4];
				array[0] = 50;
				array[1] = 50;
				base.RunLogicBlock(arg_7D_1, arg_7D_2, array);
				return;
			}
			case 1:
			{
				bool arg_5D_1 = true;
				LogicBlock arg_5D_2 = this.m_generalBasicLB;
				int[] array2 = new int[4];
				array2[0] = 65;
				array2[1] = 35;
				base.RunLogicBlock(arg_5D_1, arg_5D_2, array2);
				return;
			}
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
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
			this.Type = 13;
			this.m_smoke = new SpriteObj("NinjaSmoke_Sprite");
			this.m_smoke.AnimationDelay = 0.05f;
			this.m_log = new SpriteObj("Log_Sprite");
			this.m_smoke.Visible = false;
			this.m_smoke.Scale = new Vector2(5f, 5f);
			this.m_log.Visible = false;
			this.m_log.OutlineWidth = 2;
		}
		public override void Update(GameTime gameTime)
		{
			if (base.Y < this.m_levelScreen.CurrentRoom.Y)
			{
				base.Y = this.m_levelScreen.CurrentRoom.Y;
			}
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			if (this.m_target != null && this.m_target.CurrentHealth > 0 && this.m_currentActiveLB != this.m_basicTeleportAttackLB && this.m_currentActiveLB != this.m_expertTeleportAttackLB && CDGMath.RandomFloat(0f, 1f) <= this.ChanceToTeleport && this.m_closestCeiling != null)
			{
				this.m_closestCeiling = this.FindClosestCeiling();
				int num = this.TerrainBounds.Top - this.m_closestCeiling.Bounds.Bottom;
				if (this.m_closestCeiling != null && num > 150 && num < 700)
				{
					this.m_currentActiveLB.StopLogicBlock();
					if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
					{
						base.RunLogicBlock(false, this.m_expertTeleportAttackLB, new int[]
						{
							100
						});
					}
					else
					{
						base.RunLogicBlock(false, this.m_basicTeleportAttackLB, new int[]
						{
							100
						});
					}
					damage = (int)Math.Round((double)((float)damage * (1f - this.m_teleportDamageReduc)), MidpointRounding.AwayFromZero);
				}
			}
			base.HitEnemy(damage, position, isPlayer);
		}
		private TerrainObj FindClosestCeiling()
		{
			int num = 2147483647;
			TerrainObj result = null;
			RoomObj currentRoom = this.m_levelScreen.CurrentRoom;
			foreach (TerrainObj current in currentRoom.TerrainObjList)
			{
				Rectangle b = new Rectangle(this.Bounds.Left, this.Bounds.Top - 2000, this.Bounds.Width, this.Bounds.Height + 2000);
				if (current.CollidesBottom && CollisionMath.Intersects(current.Bounds, b))
				{
					float num2 = 3.40282347E+38f;
					if (current.Bounds.Bottom < this.TerrainBounds.Top)
					{
						num2 = (float)(this.TerrainBounds.Top - current.Bounds.Bottom);
					}
					if (num2 < (float)num)
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
			this.m_log.Position = base.Position;
			this.m_smoke.Position = base.Position;
			this.m_smoke.Visible = true;
			this.m_log.Visible = true;
			this.m_log.Opacity = 1f;
			this.m_smoke.PlayAnimation(false);
			Tween.By(this.m_log, 0.1f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Y",
				"10"
			});
			Tween.To(this.m_log, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"Opacity",
				"0"
			});
			SoundManager.Play3DSound(this, this.m_target, "Ninja_Teleport");
			base.Visible = false;
			base.IsCollidable = false;
			base.IsWeighted = false;
			this.m_storedRoom = this.m_levelScreen.CurrentRoom;
		}
		public void CreateSmoke()
		{
			if (this.m_levelScreen.CurrentRoom == this.m_storedRoom && this.m_closestCeiling != null)
			{
				base.UpdateCollisionBoxes();
				base.Y = (float)this.m_closestCeiling.Bounds.Bottom + (base.Y - (float)this.TerrainBounds.Top);
				base.X = this.m_target.X;
				this.ChangeSprite("EnemyNinjaAttack_Character");
				base.Visible = true;
				base.AccelerationX = 0f;
				base.AccelerationY = 0f;
				base.CurrentSpeed = 0f;
				base.IsCollidable = true;
				this.m_smoke.Position = base.Position;
				this.m_smoke.Visible = true;
				this.m_smoke.PlayAnimation(false);
				this.m_closestCeiling = null;
			}
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			this.m_log.Draw(camera);
			this.m_smoke.Draw(camera);
		}
		public override void Kill(bool giveXP = true)
		{
			this.m_smoke.Visible = false;
			this.m_log.Visible = false;
			base.Kill(giveXP);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_storedRoom = null;
				this.m_smoke.Dispose();
				this.m_smoke = null;
				this.m_log.Dispose();
				this.m_log = null;
				this.m_closestCeiling = null;
				base.Dispose();
			}
		}
	}
}
