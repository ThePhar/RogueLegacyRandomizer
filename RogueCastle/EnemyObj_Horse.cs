using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace RogueCastle
{
	public class EnemyObj_Horse : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private LogicBlock m_turnLB = new LogicBlock();
		private int m_wallDistanceCheck = 430;
		private float m_collisionCheckTimer = 0.5f;
		private float m_fireDropTimer = 0.5f;
		private float m_fireDropInterval = 0.075f;
		private float m_fireDropLifespan = 0.75f;
		private int m_numFireShieldObjs = 2;
		private float m_fireDistance = 110f;
		private float m_fireRotationSpeed = 1.5f;
		private float m_fireShieldScale = 2.5f;
		private List<ProjectileObj> m_fireShieldList;
		private FrameSoundObj m_gallopSound;
		private bool m_turning;
		private Rectangle WallCollisionPoint
		{
			get
			{
				if (base.HeadingX < 0f)
				{
					return new Rectangle((int)base.X - this.m_wallDistanceCheck, (int)base.Y, 2, 2);
				}
				return new Rectangle((int)base.X + this.m_wallDistanceCheck, (int)base.Y, 2, 2);
			}
		}
		private Rectangle GroundCollisionPoint
		{
			get
			{
				if (base.HeadingX < 0f)
				{
					return new Rectangle((int)(base.X - (float)this.m_wallDistanceCheck * this.ScaleX), (int)(base.Y + 60f * this.ScaleY), 2, 2);
				}
				return new Rectangle((int)(base.X + (float)this.m_wallDistanceCheck * this.ScaleX), (int)(base.Y + 60f * this.ScaleY), 2, 2);
			}
		}
		protected override void InitializeEV()
		{
			base.LockFlip = true;
			base.Name = "Headless Horse";
			this.MaxHealth = 30;
			base.Damage = 23;
			base.XPValue = 25;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 425f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 0f;
			base.JumpHeight = 900f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.06666667f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Horse_Basic_Scale;
			base.ProjectileScale = EnemyEV.Horse_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Horse_Basic_Tint;
			this.MeleeRadius = 700;
			this.ProjectileRadius = 1800;
			this.EngageRadius = 2100;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Horse_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Dark Stallion";
				this.MaxHealth = 37;
				base.Damage = 27;
				base.XPValue = 75;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 500f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 900f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.06666667f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Horse_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Horse_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Horse_Advanced_Tint;
				this.MeleeRadius = 700;
				this.EngageRadius = 2100;
				this.ProjectileRadius = 1800;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Horse_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Night Mare";
				this.MaxHealth = 60;
				base.Damage = 30;
				base.XPValue = 200;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 550f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 900f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.06666667f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Horse_Expert_Scale;
				base.ProjectileScale = EnemyEV.Horse_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Horse_Expert_Tint;
				this.MeleeRadius = 700;
				this.ProjectileRadius = 1800;
				this.EngageRadius = 2100;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Horse_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "My Little Pony";
				this.MaxHealth = 800;
				base.Damage = 40;
				base.XPValue = 600;
				this.MinMoneyDropAmount = 10;
				this.MaxMoneyDropAmount = 15;
				this.MoneyDropChance = 1f;
				base.Speed = 900f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 900f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.06666667f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Horse_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Horse_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Horse_Miniboss_Tint;
				this.MeleeRadius = 700;
				this.ProjectileRadius = 1800;
				this.EngageRadius = 2100;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Horse_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyHorseTurn_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.None), Types.Sequence.Serial);
			logicSet3.AddAction(new RunFunctionLogicAction(this, "ResetTurn", new object[0]), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyHorseTurn_Character", true, true), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
			logicSet4.AddAction(new DelayLogicAction(0.25f, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.FlipHorizontally), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "ResetTurn", new object[0]), Types.Sequence.Serial);
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new MoveDirectionLogicAction(new Vector2(-1f, 0f), -1f), Types.Sequence.Serial);
			this.ThrowStandingProjectiles(logicSet5, true);
			LogicSet logicSet6 = new LogicSet(this);
			logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true), Types.Sequence.Serial);
			logicSet6.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
			this.ThrowStandingProjectiles(logicSet6, true);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2
			});
			this.m_turnLB.AddLogicSet(new LogicSet[]
			{
				logicSet4,
				logicSet3
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_turnLB);
			this.m_gallopSound = new FrameSoundObj(this, this.m_target, 2, new string[]
			{
				"Enemy_Horse_Gallop_01",
				"Enemy_Horse_Gallop_02",
				"Enemy_Horse_Gallop_03"
			});
			base.InitializeLogic();
		}
		private void ThrowStandingProjectiles(LogicSet ls, bool useBossProjectile = false)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "SpellDamageShield_Sprite",
				SourceAnchor = new Vector2(0f, 60f),
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale,
				Lifespan = 0.75f
			};
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			ls.AddAction(new DelayLogicAction(0.075f, false), Types.Sequence.Serial);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			ls.AddAction(new DelayLogicAction(0.075f, false), Types.Sequence.Serial);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				if (this.Flip == SpriteEffects.FlipHorizontally)
				{
					bool arg_3C_1 = true;
					LogicBlock arg_3C_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
					return;
				}
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
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
			case 1:
			case 2:
			case 3:
				if (this.Flip == SpriteEffects.FlipHorizontally)
				{
					bool arg_3C_1 = true;
					LogicBlock arg_3C_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
					return;
				}
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
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
			case 1:
			case 2:
			case 3:
				if (this.Flip == SpriteEffects.FlipHorizontally)
				{
					bool arg_3C_1 = true;
					LogicBlock arg_3C_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
					return;
				}
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
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
			case 1:
			case 2:
			case 3:
				if (this.Flip == SpriteEffects.FlipHorizontally)
				{
					bool arg_3C_1 = true;
					LogicBlock arg_3C_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_3C_1, arg_3C_2, array);
					return;
				}
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
				return;
			default:
				return;
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_target.AttachedLevel.CurrentRoom.Name != "Ending")
			{
				this.m_gallopSound.Update();
			}
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (base.Difficulty >= GameTypes.EnemyDifficulty.ADVANCED && this.m_fireDropTimer > 0f)
			{
				this.m_fireDropTimer -= num;
				if (this.m_fireDropTimer <= 0f)
				{
					this.DropFireProjectile();
					this.m_fireDropTimer = this.m_fireDropInterval;
				}
			}
			if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT && !base.IsPaused && this.m_fireShieldList.Count < 1)
			{
				this.CastFireShield(this.m_numFireShieldObjs);
			}
			if ((this.Bounds.Left < this.m_levelScreen.CurrentRoom.Bounds.Left || this.Bounds.Right > this.m_levelScreen.CurrentRoom.Bounds.Right) && this.m_collisionCheckTimer <= 0f)
			{
				this.TurnHorse();
			}
			Rectangle b = default(Rectangle);
			Rectangle b2 = default(Rectangle);
			if (this.Flip == SpriteEffects.FlipHorizontally)
			{
				b = new Rectangle(this.Bounds.Left - 10, this.Bounds.Bottom + 20, 5, 5);
				b2 = new Rectangle(this.Bounds.Right + 50, this.Bounds.Bottom - 20, 5, 5);
			}
			else
			{
				b = new Rectangle(this.Bounds.Right + 10, this.Bounds.Bottom + 20, 5, 5);
				b2 = new Rectangle(this.Bounds.Left - 50, this.Bounds.Bottom - 20, 5, 5);
			}
			bool flag = true;
			foreach (TerrainObj current in this.m_levelScreen.CurrentRoom.TerrainObjList)
			{
				if (CollisionMath.Intersects(current.Bounds, b) || CollisionMath.Intersects(current.Bounds, b2))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.TurnHorse();
			}
			if (this.m_collisionCheckTimer > 0f)
			{
				this.m_collisionCheckTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Update(gameTime);
		}
		public void ResetTurn()
		{
			this.m_turning = false;
		}
		private void DropFireProjectile()
		{
			base.UpdateCollisionBoxes();
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "SpellDamageShield_Sprite",
				SourceAnchor = new Vector2(0f, (float)this.Bounds.Bottom - base.Y - 10f),
				Speed = new Vector2(0f, 0f),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				Angle = new Vector2(0f, 0f),
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale,
				Lifespan = this.m_fireDropLifespan,
				LockPosition = true
			};
			this.m_levelScreen.ProjectileManager.FireProjectile(projectileData);
			projectileData.Dispose();
		}
		private void CastFireShield(int numFires)
		{
			ProjectileData data = new ProjectileData(this)
			{
				SpriteName = "SpellDamageShield_Sprite",
				SourceAnchor = new Vector2(0f, (float)this.Bounds.Bottom - base.Y - 10f),
				Speed = new Vector2(this.m_fireRotationSpeed, this.m_fireRotationSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Target = this,
				Damage = base.Damage,
				Angle = new Vector2(0f, 0f),
				AngleOffset = 0f,
				CollidesWithTerrain = false,
				Scale = new Vector2(this.m_fireShieldScale, this.m_fireShieldScale),
				Lifespan = 999999f,
				DestroysWithEnemy = false,
				LockPosition = true
			};
			SoundManager.PlaySound("Cast_FireShield");
			float fireDistance = this.m_fireDistance;
			for (int i = 0; i < numFires; i++)
			{
				float altX = 360f / (float)numFires * (float)i;
				ProjectileObj projectileObj = this.m_levelScreen.ProjectileManager.FireProjectile(data);
				projectileObj.AltX = altX;
				projectileObj.AltY = fireDistance;
				projectileObj.Spell = 11;
				projectileObj.CanBeFusRohDahed = false;
				projectileObj.AccelerationXEnabled = false;
				projectileObj.AccelerationYEnabled = false;
				projectileObj.IgnoreBoundsCheck = true;
				this.m_fireShieldList.Add(projectileObj);
			}
		}
		private void TurnHorse()
		{
			if (!this.m_turning)
			{
				this.m_turning = true;
				if (base.HeadingX < 0f)
				{
					this.m_currentActiveLB.StopLogicBlock();
					base.RunLogicBlock(false, this.m_turnLB, new int[]
					{
						0,
						100
					});
				}
				else
				{
					this.m_currentActiveLB.StopLogicBlock();
					bool arg_63_1 = false;
					LogicBlock arg_63_2 = this.m_turnLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_63_1, arg_63_2, array);
				}
				this.m_collisionCheckTimer = 0.5f;
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			TerrainObj terrainObj = otherBox.AbsParent as TerrainObj;
			if (otherBox.AbsParent.Bounds.Top < this.TerrainBounds.Bottom - 20 && terrainObj != null && terrainObj.CollidesLeft && terrainObj.CollidesRight && terrainObj.CollidesBottom && collisionResponseType == 1 && otherBox.AbsRotation == 0f && this.m_collisionCheckTimer <= 0f && CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect).X != 0f)
			{
				this.TurnHorse();
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			SoundManager.Play3DSound(this, this.m_target, new string[]
			{
				"Enemy_Horse_Hit_01",
				"Enemy_Horse_Hit_02",
				"Enemy_Horse_Hit_03"
			});
			base.HitEnemy(damage, collisionPt, isPlayer);
		}
		public override void Kill(bool giveXP = true)
		{
			foreach (ProjectileObj current in this.m_fireShieldList)
			{
				current.RunDestroyAnimation(false);
			}
			this.m_fireShieldList.Clear();
			SoundManager.Play3DSound(this, this.m_target, "Enemy_Horse_Dead");
			base.Kill(giveXP);
		}
		public override void ResetState()
		{
			this.m_fireShieldList.Clear();
			base.ResetState();
		}
		public EnemyObj_Horse(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyHorseRun_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 10;
			this.m_fireShieldList = new List<ProjectileObj>();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				if (this.m_gallopSound != null)
				{
					this.m_gallopSound.Dispose();
				}
				this.m_gallopSound = null;
				base.Dispose();
			}
		}
	}
}
