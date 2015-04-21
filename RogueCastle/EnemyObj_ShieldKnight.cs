using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class EnemyObj_ShieldKnight : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private Vector2 ShieldKnockback = new Vector2(900f, 1050f);
		private float m_blockDmgReduction = 0.6f;
		private FrameSoundObj m_walkSound;
		private FrameSoundObj m_walkSound2;
		protected override void InitializeEV()
		{
			base.LockFlip = true;
			base.Name = "Tall Guard";
			this.MaxHealth = 40;
			base.Damage = 31;
			base.XPValue = 150;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 100f;
			this.TurnSpeed = 0.0175f;
			this.ProjectileSpeed = 650f;
			base.JumpHeight = 950f;
			this.CooldownTime = 2.25f;
			base.AnimationDelay = 0.142857149f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.ShieldKnight_Basic_Scale;
			base.ProjectileScale = EnemyEV.ShieldKnight_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.ShieldKnight_Basic_Tint;
			this.MeleeRadius = 50;
			this.ProjectileRadius = 550;
			this.EngageRadius = 700;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.ShieldKnight_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.ShieldKnockback = new Vector2(1050f, 1150f);
				base.Name = "Hulk Guard";
				this.MaxHealth = 58;
				base.Damage = 38;
				base.XPValue = 250;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 175f;
				this.TurnSpeed = 0.0175f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 950f;
				this.CooldownTime = 2.25f;
				base.AnimationDelay = 0.142857149f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.ShieldKnight_Advanced_Scale;
				base.ProjectileScale = EnemyEV.ShieldKnight_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.ShieldKnight_Advanced_Tint;
				this.MeleeRadius = 50;
				this.EngageRadius = 700;
				this.ProjectileRadius = 550;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.ShieldKnight_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.ShieldKnockback = new Vector2(1550f, 1650f);
				base.Name = "Tower Guard";
				this.MaxHealth = 79;
				base.Damage = 43;
				base.XPValue = 450;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 250f;
				this.TurnSpeed = 0.0175f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 950f;
				this.CooldownTime = 2.25f;
				base.AnimationDelay = 0.09090909f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.ShieldKnight_Expert_Scale;
				base.ProjectileScale = EnemyEV.ShieldKnight_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.ShieldKnight_Expert_Tint;
				this.MeleeRadius = 50;
				this.ProjectileRadius = 550;
				this.EngageRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.ShieldKnight_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				this.ShieldKnockback = new Vector2(1200f, 1350f);
				base.Name = "Sentinel";
				this.MaxHealth = 1;
				base.Damage = 1;
				base.XPValue = 1250;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 250f;
				this.TurnSpeed = 0.0175f;
				this.ProjectileSpeed = 650f;
				base.JumpHeight = 950f;
				this.CooldownTime = 0f;
				base.AnimationDelay = 0.142857149f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.ShieldKnight_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.ShieldKnight_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.ShieldKnight_Miniboss_Tint;
				this.MeleeRadius = 50;
				this.ProjectileRadius = 550;
				this.EngageRadius = 700;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.ShieldKnight_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightIdle_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveDirectionLogicAction(0f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, 2f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightWalk_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.5f, 2f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveDirectionLogicAction(0f), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnIn_Character", false, false), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(1, 2, false), Types.Sequence.Serial);
			logicSet3.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"ShieldKnight_Turn"
			}), Types.Sequence.Serial);
			logicSet3.AddAction(new PlayAnimationLogicAction(3, base.TotalFrames, false), Types.Sequence.Serial);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnOut_Character", true, false), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveDirectionLogicAction(0f), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnIn_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.05f), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(1, 2, false), Types.Sequence.Serial);
			logicSet4.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"ShieldKnight_Turn"
			}), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction(3, base.TotalFrames, false), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(false, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet4.AddAction(new LockFaceDirectionLogicAction(true, 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnOut_Character", true, false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.09090909f), Types.Sequence.Serial);
			logicSet4.AddAction(new MoveDirectionLogicAction(-1f), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			this.m_generalExpertLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet4
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalExpertLB);
			base.SetCooldownLogicBlock(this.m_generalBasicLB, new int[]
			{
				100
			});
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_107_1 = true;
				LogicBlock arg_107_2 = this.m_generalBasicLB;
				int[] array = new int[3];
				array[0] = 100;
				base.RunLogicBlock(arg_107_1, arg_107_2, array);
				return;
			}
			case 1:
			case 2:
			case 3:
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_8E_1 = true;
				LogicBlock arg_8E_2 = this.m_generalBasicLB;
				int[] array2 = new int[3];
				array2[1] = 100;
				base.RunLogicBlock(arg_8E_1, arg_8E_2, array2);
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
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_107_1 = true;
				LogicBlock arg_107_2 = this.m_generalBasicLB;
				int[] array = new int[3];
				array[0] = 100;
				base.RunLogicBlock(arg_107_1, arg_107_2, array);
				return;
			}
			case 1:
			case 2:
			case 3:
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_8E_1 = true;
				LogicBlock arg_8E_2 = this.m_generalBasicLB;
				int[] array2 = new int[3];
				array2[1] = 100;
				base.RunLogicBlock(arg_8E_1, arg_8E_2, array2);
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
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_107_1 = true;
				LogicBlock arg_107_2 = this.m_generalExpertLB;
				int[] array = new int[3];
				array[0] = 100;
				base.RunLogicBlock(arg_107_1, arg_107_2, array);
				return;
			}
			case 1:
			case 2:
			case 3:
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					base.RunLogicBlock(true, this.m_generalExpertLB, new int[]
					{
						0,
						0,
						100
					});
					return;
				}
				bool arg_8E_1 = true;
				LogicBlock arg_8E_2 = this.m_generalExpertLB;
				int[] array2 = new int[3];
				array2[1] = 100;
				base.RunLogicBlock(arg_8E_1, arg_8E_2, array2);
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
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					bool arg_E9_1 = true;
					LogicBlock arg_E9_2 = this.m_generalBasicLB;
					int[] array = new int[3];
					array[0] = 100;
					base.RunLogicBlock(arg_E9_1, arg_E9_2, array);
					return;
				}
				bool arg_107_1 = true;
				LogicBlock arg_107_2 = this.m_generalBasicLB;
				int[] array2 = new int[3];
				array2[0] = 100;
				base.RunLogicBlock(arg_107_1, arg_107_2, array2);
				return;
			}
			case 1:
			case 2:
			case 3:
			{
				if ((this.m_target.X > base.X && base.HeadingX < 0f) || (this.m_target.X < base.X && base.HeadingX >= 0f))
				{
					bool arg_73_1 = true;
					LogicBlock arg_73_2 = this.m_generalBasicLB;
					int[] array3 = new int[3];
					array3[0] = 100;
					base.RunLogicBlock(arg_73_1, arg_73_2, array3);
					return;
				}
				bool arg_8E_1 = true;
				LogicBlock arg_8E_2 = this.m_generalBasicLB;
				int[] array4 = new int[3];
				array4[0] = 100;
				base.RunLogicBlock(arg_8E_1, arg_8E_2, array4);
				return;
			}
			default:
				return;
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
			if (collisionResponseType == 2 && ((playerObj != null && this.m_invincibleCounter <= 0f) || (projectileObj != null && this.m_invincibleCounterProjectile <= 0f)) && ((this.Flip == SpriteEffects.None && otherBox.AbsParent.AbsPosition.X > base.X) || (this.Flip == SpriteEffects.FlipHorizontally && otherBox.AbsParent.AbsPosition.X < base.X)) && playerObj != null && playerObj.SpriteName != "PlayerAirAttack_Character")
			{
				if (base.CanBeKnockedBack)
				{
					base.CurrentSpeed = 0f;
					this.m_currentActiveLB.StopLogicBlock();
					bool arg_DD_1 = true;
					LogicBlock arg_DD_2 = this.m_generalBasicLB;
					int[] array = new int[3];
					array[0] = 100;
					base.RunLogicBlock(arg_DD_1, arg_DD_2, array);
				}
				if (this.m_target.IsAirAttacking)
				{
					this.m_target.IsAirAttacking = false;
					this.m_target.AccelerationY = -this.m_target.AirAttackKnockBack;
					this.m_target.NumAirBounces++;
				}
				else
				{
					if ((float)(this.m_target.Bounds.Left + this.m_target.Bounds.Width / 2) < base.X)
					{
						this.m_target.AccelerationX = -this.ShieldKnockback.X;
					}
					else
					{
						this.m_target.AccelerationX = this.ShieldKnockback.X;
					}
					this.m_target.AccelerationY = -this.ShieldKnockback.Y;
				}
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
				Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
				Vector2 position = new Vector2((float)center.X, (float)center.Y);
				this.m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(position, new Vector2(2f, 2f));
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"ShieldKnight_Block01",
					"ShieldKnight_Block02",
					"ShieldKnight_Block03"
				});
				this.m_invincibleCounter = base.InvincibilityTime;
				this.m_levelScreen.SetLastEnemyHit(this);
				base.Blink(Color.LightBlue, 0.1f);
				ProjectileObj projectileObj2 = otherBox.AbsParent as ProjectileObj;
				if (projectileObj2 != null)
				{
					this.m_invincibleCounterProjectile = base.InvincibilityTime;
					this.m_levelScreen.ProjectileManager.DestroyProjectile(projectileObj2);
					return;
				}
			}
			else
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			if (this.m_target != null && this.m_target.CurrentHealth > 0)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"Knight_Hit01",
					"Knight_Hit02",
					"Knight_Hit03"
				});
				if (((this.Flip == SpriteEffects.None && this.m_target.X > base.X) || (this.Flip == SpriteEffects.FlipHorizontally && this.m_target.X < base.X)) && this.m_target.SpriteName != "PlayerAirAttack_Character")
				{
					damage = (int)((float)damage * (1f - this.m_blockDmgReduction));
				}
			}
			base.HitEnemy(damage, position, isPlayer);
		}
		public override void Update(GameTime gameTime)
		{
			if (base.SpriteName == "EnemyShieldKnightWalk_Character")
			{
				this.m_walkSound.Update();
				this.m_walkSound2.Update();
			}
			base.Update(gameTime);
		}
		public EnemyObj_ShieldKnight(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyShieldKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 14;
			this.m_walkSound = new FrameSoundObj(this, this.m_target, 1, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
			this.m_walkSound2 = new FrameSoundObj(this, this.m_target, 6, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_walkSound.Dispose();
				this.m_walkSound = null;
				this.m_walkSound2.Dispose();
				this.m_walkSound2 = null;
				base.Dispose();
			}
		}
	}
}
