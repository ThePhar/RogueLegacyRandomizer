using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Portrait : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		public bool Shake
		{
			get;
			set;
		}
		public bool Chasing
		{
			get;
			set;
		}
		protected override void InitializeEV()
		{
			base.LockFlip = true;
			base.Name = "Doomvas";
			this.MaxHealth = 35;
			base.Damage = 21;
			base.XPValue = 50;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 500f;
			this.TurnSpeed = 0.03f;
			this.ProjectileSpeed = 1020f;
			base.JumpHeight = 600f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.0166666675f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			base.IsWeighted = false;
			this.Scale = EnemyEV.Portrait_Basic_Scale;
			base.ProjectileScale = EnemyEV.Portrait_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Portrait_Basic_Tint;
			this.MeleeRadius = 25;
			this.ProjectileRadius = 100;
			this.EngageRadius = 300;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Portrait_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Doomtrait";
				this.MaxHealth = 43;
				base.Damage = 25;
				base.XPValue = 75;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 550f;
				this.TurnSpeed = 0.0425f;
				this.ProjectileSpeed = 400f;
				base.JumpHeight = 600f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.0166666675f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Portrait_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Portrait_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Portrait_Advanced_Tint;
				this.MeleeRadius = 25;
				this.EngageRadius = 300;
				this.ProjectileRadius = 100;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Portrait_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Doomscape";
				this.MaxHealth = 61;
				base.Damage = 27;
				base.XPValue = 100;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 1f;
				base.Speed = 600f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 400f;
				base.JumpHeight = 600f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.0166666675f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Portrait_Expert_Scale;
				base.ProjectileScale = EnemyEV.Portrait_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Portrait_Expert_Tint;
				this.MeleeRadius = 25;
				this.ProjectileRadius = 100;
				this.EngageRadius = 300;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Portrait_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Sallos";
				this.MaxHealth = 215;
				base.Damage = 28;
				base.XPValue = 1000;
				this.MinMoneyDropAmount = 11;
				this.MaxMoneyDropAmount = 15;
				this.MoneyDropChance = 1f;
				base.Speed = 515f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 250f;
				base.JumpHeight = 600f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.0166666675f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Portrait_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Portrait_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Portrait_Miniboss_Tint;
				this.MeleeRadius = 25;
				this.ProjectileRadius = 100;
				this.EngageRadius = 300;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Portrait_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "Shake", true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this, "Shake", false), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChaseLogicAction(this.m_target, Vector2.Zero, Vector2.Zero, true, 1f, -1f), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChaseLogicAction(this.m_target, Vector2.Zero, Vector2.Zero, true, 1.75f, -1f), Types.Sequence.Serial);
			this.ThrowAdvancedProjectiles(logicSet3, true);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChaseLogicAction(this.m_target, Vector2.Zero, Vector2.Zero, true, 1.75f, -1f), Types.Sequence.Serial);
			this.ThrowExpertProjectiles(logicSet4, true);
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChaseLogicAction(this.m_target, Vector2.Zero, Vector2.Zero, true, 1.25f, -1f), Types.Sequence.Serial);
			this.ThrowProjectiles(logicSet5, true);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2
			});
			this.m_generalAdvancedLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet3
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet4
			});
			this.m_generalMiniBossLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet5
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalAdvancedLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			base.InitializeLogic();
			base.CollisionBoxes.Clear();
			base.CollisionBoxes.Add(new CollisionBox((int)(-18f * this.ScaleX), (int)(-24f * this.ScaleY), (int)(36f * this.ScaleX), (int)(48f * this.ScaleY), 2, this));
			base.CollisionBoxes.Add(new CollisionBox((int)(-15f * this.ScaleX), (int)(-21f * this.ScaleY), (int)(31f * this.ScaleX), (int)(44f * this.ScaleY), 1, this));
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				(base.GetChildAt(0) as SpriteObj).ChangeSprite("GiantPortrait_Sprite");
				this.Scale = new Vector2(2f, 2f);
				this.AddChild(new SpriteObj("Portrait" + CDGMath.RandomInt(0, 7) + "_Sprite")
				{
					OverrideParentScale = true
				});
				base.CollisionBoxes.Clear();
				base.CollisionBoxes.Add(new CollisionBox(-124, -176, 250, 354, 2, this));
				base.CollisionBoxes.Add(new CollisionBox(-124, -176, 250, 354, 1, this));
			}
		}
		private void ThrowProjectiles(LogicSet ls, bool useBossProjectile = false)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale
			};
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(135f, 135f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(45f, 45f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-45f, -45f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-135f, -135f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Dispose();
		}
		private void ThrowExpertProjectiles(LogicSet ls, bool useBossProjectile = false)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale
			};
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(0f, 0f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(90f, 90f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(180f, 180f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Dispose();
		}
		private void ThrowAdvancedProjectiles(LogicSet ls, bool useBossProjectile = false)
		{
			ProjectileData projectileData = new ProjectileData(this)
			{
				SpriteName = "GhostProjectile_Sprite",
				SourceAnchor = Vector2.Zero,
				Target = null,
				Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
				IsWeighted = false,
				RotationSpeed = 0f,
				Damage = base.Damage,
				AngleOffset = 0f,
				Angle = new Vector2(0f, 0f),
				CollidesWithTerrain = false,
				Scale = base.ProjectileScale
			};
			ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"FairyAttack1"
			}), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(90f, 90f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Angle = new Vector2(-90f, -90f);
			ls.AddAction(new FireProjectileLogicAction(this.m_levelScreen.ProjectileManager, projectileData), Types.Sequence.Serial);
			projectileData.Dispose();
		}
		protected override void RunBasicLogic()
		{
			if (!this.Chasing)
			{
				switch (base.State)
				{
				case 0:
					break;
				case 1:
				{
					bool arg_42_1 = true;
					LogicBlock arg_42_2 = this.m_generalBasicLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_42_1, arg_42_2, array);
					return;
				}
				case 2:
				case 3:
					this.ChasePlayer();
					return;
				default:
					return;
				}
			}
			else
			{
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					100
				});
			}
		}
		protected override void RunAdvancedLogic()
		{
			if (!this.Chasing)
			{
				switch (base.State)
				{
				case 0:
					break;
				case 1:
				{
					bool arg_42_1 = true;
					LogicBlock arg_42_2 = this.m_generalAdvancedLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_42_1, arg_42_2, array);
					return;
				}
				case 2:
				case 3:
					this.ChasePlayer();
					return;
				default:
					return;
				}
			}
			else
			{
				base.RunLogicBlock(true, this.m_generalAdvancedLB, new int[]
				{
					0,
					100
				});
			}
		}
		protected override void RunExpertLogic()
		{
			if (!this.Chasing)
			{
				switch (base.State)
				{
				case 0:
					break;
				case 1:
				{
					bool arg_42_1 = true;
					LogicBlock arg_42_2 = this.m_generalExpertLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_42_1, arg_42_2, array);
					return;
				}
				case 2:
				case 3:
					this.ChasePlayer();
					return;
				default:
					return;
				}
			}
			else
			{
				base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
				{
					0,
					100
				});
			}
		}
		protected override void RunMinibossLogic()
		{
			if (!this.Chasing)
			{
				switch (base.State)
				{
				case 0:
					break;
				case 1:
				{
					bool arg_43_1 = true;
					LogicBlock arg_43_2 = this.m_generalMiniBossLB;
					int[] array = new int[2];
					array[0] = 100;
					base.RunLogicBlock(arg_43_1, arg_43_2, array);
					return;
				}
				case 2:
				case 3:
					this.Chasing = true;
					return;
				default:
					return;
				}
			}
			else
			{
				base.RunLogicBlock(true, this.m_generalMiniBossLB, new int[]
				{
					0,
					100
				});
			}
		}
		public override void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!this.Chasing)
			{
				if (base.Difficulty != GameTypes.EnemyDifficulty.MINIBOSS)
				{
					if (this.Shake)
					{
						base.Rotation = (float)Math.Sin((double)(Game.TotalGameTime * 15f)) * 2f;
					}
					else
					{
						base.Rotation = 0f;
					}
				}
			}
			else
			{
				if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
				{
					base.Rotation += 420f * num;
				}
				else
				{
					base.Rotation += 600f * num;
				}
				SpriteObj spriteObj = base.GetChildAt(0) as SpriteObj;
				if (spriteObj.SpriteName != "EnemyPortrait" + (int)base.Difficulty + "_Sprite")
				{
					this.ChangePortrait();
				}
			}
			base.Update(gameTime);
		}
		public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			this.ChasePlayer();
			base.HitEnemy(damage, collisionPt, isPlayer);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (otherBox.AbsParent is PlayerObj)
			{
				this.ChasePlayer();
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public void ChasePlayer()
		{
			if (!this.Chasing)
			{
				if (this.m_currentActiveLB != null)
				{
					this.m_currentActiveLB.StopLogicBlock();
				}
				this.Chasing = true;
				if (this.m_target.X < base.X)
				{
					base.Orientation = 0f;
					return;
				}
				base.Orientation = MathHelper.ToRadians(180f);
			}
		}
		public override void Reset()
		{
			this.Chasing = false;
			base.Reset();
		}
		public EnemyObj_Portrait(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyPortrait_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 32;
			string spriteName = "FramePicture" + CDGMath.RandomInt(1, 16) + "_Sprite";
			base.GetChildAt(0).ChangeSprite(spriteName);
			base.GetChildAt(0);
			base.DisableCollisionBoxRotations = false;
		}
		public void ChangePortrait()
		{
			SoundManager.PlaySound("FinalBoss_St2_BlockLaugh");
			SpriteObj spriteObj = base.GetChildAt(0) as SpriteObj;
			spriteObj.ChangeSprite("EnemyPortrait" + (int)base.Difficulty + "_Sprite");
			if (base.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
			{
				base.GetChildAt(1).Visible = false;
			}
		}
	}
}
