using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using Tweener;
namespace RogueCastle
{
	public abstract class EnemyObj : CharacterObj, IDealsDamageObj
	{
		protected const int STATE_WANDER = 0;
		protected const int STATE_ENGAGE = 1;
		protected const int STATE_PROJECTILE_ENGAGE = 2;
		protected const int STATE_MELEE_ENGAGE = 3;
		protected float DistanceToPlayer;
		protected GameObj TintablePart;
		protected int ProjectileRadius;
		protected int MeleeRadius;
		protected int EngageRadius;
		protected float CooldownTime;
		protected int m_damage;
		protected bool CanFallOffLedges = true;
		protected bool AlwaysFaceTarget;
		protected int ProjectileDamage = 5;
		protected float ProjectileSpeed = 100f;
		protected int m_xpValue;
		private int m_level;
		protected int DamageGainPerLevel;
		protected int XPBonusPerLevel;
		protected int HealthGainPerLevel;
		protected float MinMoneyGainPerLevel;
		protected float MaxMoneyGainPerLevel;
		protected float ItemDropChance;
		protected int MinMoneyDropAmount;
		protected int MaxMoneyDropAmount;
		protected float MoneyDropChance;
		protected float m_invincibilityTime = 0.4f;
		protected float m_invincibleCounter;
		protected float m_invincibleCounterProjectile;
		protected float StatLevelHPMod;
		protected float StatLevelDMGMod;
		protected float StatLevelXPMod;
		public float InitialLogicDelay;
		protected float m_initialDelayCounter;
		protected PlayerObj m_target;
		protected string m_resetSpriteName;
		private int m_numTouchingGrounds;
		private LogicBlock m_walkingLB;
		protected LogicBlock m_currentActiveLB;
		private LogicBlock m_cooldownLB;
		private bool m_runCooldown;
		private float m_cooldownTimer;
		private int[] m_cooldownParams;
		private Texture2D m_engageRadiusTexture;
		private Texture2D m_projectileRadiusTexture;
		private Texture2D m_meleeRadiusTexture;
		public bool SaveToFile = true;
		public byte Type;
		private bool m_isPaused;
		protected bool m_bossVersionKilled;
		protected List<LogicBlock> logicBlocksToDispose;
		protected TweenObject m_flipTween;
		protected bool m_saveToEnemiesKilledList = true;
		public Vector2 ProjectileScale
		{
			get;
			internal set;
		}
		public bool Procedural
		{
			get;
			set;
		}
		public bool NonKillable
		{
			get;
			set;
		}
		public bool GivesLichHealth
		{
			get;
			set;
		}
		public bool IsDemented
		{
			get;
			set;
		}
		public int Level
		{
			get
			{
				return this.m_level;
			}
			set
			{
				this.m_level = value;
				if (this.m_level < 0)
				{
					this.m_level = 0;
				}
			}
		}
		protected float InvincibilityTime
		{
			get
			{
				return this.m_invincibilityTime;
			}
		}
		public GameTypes.EnemyDifficulty Difficulty
		{
			get;
			internal set;
		}
		public bool IsProcedural
		{
			get;
			set;
		}
		public bool PlayAnimationOnRestart
		{
			get;
			set;
		}
		public bool DropsItem
		{
			get;
			set;
		}
		private Rectangle GroundCollisionRect
		{
			get
			{
				return new Rectangle(this.Bounds.X - 10, this.Bounds.Y, this.Width + 20, this.Height + 10);
			}
		}
		private Rectangle RotatedGroundCollisionRect
		{
			get
			{
				return new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height + 40);
			}
		}
		public override Rectangle Bounds
		{
			get
			{
				if (base.IsWeighted)
				{
					return this.TerrainBounds;
				}
				return base.Bounds;
			}
		}
		public override int MaxHealth
		{
			get
			{
				return base.MaxHealth + this.HealthGainPerLevel * (this.Level - 1);
			}
			internal set
			{
				base.MaxHealth = value;
			}
		}
		public int Damage
		{
			get
			{
				return this.m_damage + this.DamageGainPerLevel * (this.Level - 1);
			}
			internal set
			{
				this.m_damage = value;
			}
		}
		public int XPValue
		{
			get
			{
				return this.m_xpValue + this.XPBonusPerLevel * (this.Level - 1);
			}
			internal set
			{
				this.m_xpValue = value;
			}
		}
		public string ResetSpriteName
		{
			get
			{
				return this.m_resetSpriteName;
			}
		}
		public new bool IsPaused
		{
			get
			{
				return this.m_isPaused;
			}
		}
		public override SpriteEffects Flip
		{
			get
			{
				return base.Flip;
			}
			set
			{
				if ((Game.PlayerStats.Traits.X == 18f || Game.PlayerStats.Traits.Y == 18f) && this.Flip != value && this.m_levelScreen != null)
				{
					if (this.m_flipTween != null && this.m_flipTween.TweenedObject == this && this.m_flipTween.Active)
					{
						this.m_flipTween.StopTween(false);
					}
					float scaleY = this.ScaleY;
					this.ScaleX = 0f;
					this.m_flipTween = Tween.To(this, 0.15f, new Easing(Tween.EaseNone), new string[]
					{
						"ScaleX",
						scaleY.ToString()
					});
				}
				base.Flip = value;
			}
		}
		private void InitializeBaseEV()
		{
			base.Speed = 1f;
			this.MaxHealth = 10;
			this.EngageRadius = 400;
			this.ProjectileRadius = 200;
			this.MeleeRadius = 50;
			base.KnockBack = Vector2.Zero;
			this.Damage = 5;
			this.ProjectileScale = new Vector2(1f, 1f);
			this.XPValue = 0;
			this.ProjectileDamage = 5;
			this.ItemDropChance = 0f;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 1;
			this.MoneyDropChance = 0.5f;
			this.StatLevelHPMod = 0.16f;
			this.StatLevelDMGMod = 0.091f;
			this.StatLevelXPMod = 0.025f;
			this.MinMoneyGainPerLevel = 0.23f;
			this.MaxMoneyGainPerLevel = 0.29f;
			base.ForceDraw = true;
		}
		protected override void InitializeEV()
		{
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, false), Types.Sequence.Serial);
			this.m_walkingLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
		}
		public EnemyObj(string spriteName, PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base(spriteName, physicsManager, levelToAttachTo)
		{
			base.DisableCollisionBoxRotations = true;
			this.Type = 0;
			base.CollisionTypeTag = 3;
			this.m_target = target;
			this.m_walkingLB = new LogicBlock();
			this.m_currentActiveLB = new LogicBlock();
			this.m_cooldownLB = new LogicBlock();
			this.logicBlocksToDispose = new List<LogicBlock>();
			this.m_resetSpriteName = spriteName;
			this.Difficulty = difficulty;
			this.ProjectileScale = new Vector2(1f, 1f);
			base.PlayAnimation(true);
			this.PlayAnimationOnRestart = true;
			base.OutlineWidth = 2;
			this.GivesLichHealth = true;
			this.DropsItem = true;
		}
		public void SetDifficulty(GameTypes.EnemyDifficulty difficulty, bool reinitialize)
		{
			this.Difficulty = difficulty;
			if (reinitialize)
			{
				this.Initialize();
			}
		}
		public void Initialize()
		{
			if (this.TintablePart == null)
			{
				this.TintablePart = base.GetChildAt(0);
			}
			this.InitializeBaseEV();
			this.InitializeEV();
			this.HealthGainPerLevel = (int)((float)base.MaxHealth * this.StatLevelHPMod);
			this.DamageGainPerLevel = (int)((float)this.m_damage * this.StatLevelDMGMod);
			this.XPBonusPerLevel = (int)((float)this.m_xpValue * this.StatLevelXPMod);
			this.m_internalLockFlip = base.LockFlip;
			this.m_internalIsWeighted = base.IsWeighted;
			this.m_internalRotation = base.Rotation;
			this.m_internalAnimationDelay = base.AnimationDelay;
			this.m_internalScale = this.Scale;
			this.InternalFlip = this.Flip;
			foreach (LogicBlock current in this.logicBlocksToDispose)
			{
				current.ClearAllLogicSets();
			}
			if (this.m_levelScreen != null)
			{
				this.InitializeLogic();
			}
			this.m_initialDelayCounter = this.InitialLogicDelay;
			base.CurrentHealth = this.MaxHealth;
		}
		public void InitializeDebugRadii()
		{
			if (this.m_engageRadiusTexture == null)
			{
				int num = this.EngageRadius;
				int num2 = this.ProjectileRadius;
				int num3 = this.MeleeRadius;
				if (num > 1000)
				{
					num = 1000;
				}
				if (num2 > 1000)
				{
					num2 = 1000;
				}
				if (num3 > 1000)
				{
					num3 = 1000;
				}
				this.m_engageRadiusTexture = DebugHelper.CreateCircleTexture(num, this.m_levelScreen.Camera.GraphicsDevice);
				this.m_projectileRadiusTexture = DebugHelper.CreateCircleTexture(num2, this.m_levelScreen.Camera.GraphicsDevice);
				this.m_meleeRadiusTexture = DebugHelper.CreateCircleTexture(num3, this.m_levelScreen.Camera.GraphicsDevice);
			}
		}
		public void SetPlayerTarget(PlayerObj target)
		{
			this.m_target = target;
		}
		public void SetLevelScreen(ProceduralLevelScreen levelScreen)
		{
			this.m_levelScreen = levelScreen;
		}
		public override void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (this.m_initialDelayCounter > 0f)
			{
				this.m_initialDelayCounter -= num;
			}
			else
			{
				if (this.m_invincibleCounter > 0f)
				{
					this.m_invincibleCounter -= num;
				}
				if (this.m_invincibleCounterProjectile > 0f)
				{
					this.m_invincibleCounterProjectile -= num;
				}
				if (this.m_invincibleCounter <= 0f && this.m_invincibleCounterProjectile <= 0f && !base.IsWeighted)
				{
					if (base.AccelerationY < 0f)
					{
						base.AccelerationY += 15f;
					}
					else if (base.AccelerationY > 0f)
					{
						base.AccelerationY -= 15f;
					}
					if (base.AccelerationX < 0f)
					{
						base.AccelerationX += 15f;
					}
					else if (base.AccelerationX > 0f)
					{
						base.AccelerationX -= 15f;
					}
					if (base.AccelerationY < 3.6f && base.AccelerationY > -3.6f)
					{
						base.AccelerationY = 0f;
					}
					if (base.AccelerationX < 3.6f && base.AccelerationX > -3.6f)
					{
						base.AccelerationX = 0f;
					}
				}
				if (!base.IsKilled && !this.IsPaused)
				{
					this.DistanceToPlayer = CDGMath.DistanceBetweenPts(base.Position, this.m_target.Position);
					if (this.DistanceToPlayer > (float)this.EngageRadius)
					{
						base.State = 0;
					}
					else if (this.DistanceToPlayer < (float)this.EngageRadius && this.DistanceToPlayer >= (float)this.ProjectileRadius)
					{
						base.State = 1;
					}
					else if (this.DistanceToPlayer < (float)this.ProjectileRadius && this.DistanceToPlayer >= (float)this.MeleeRadius)
					{
						base.State = 2;
					}
					else
					{
						base.State = 3;
					}
					if (this.m_cooldownTimer > 0f && this.m_currentActiveLB == this.m_cooldownLB)
					{
						this.m_cooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
					}
					if (this.m_cooldownTimer <= 0f && this.m_runCooldown)
					{
						this.m_runCooldown = false;
					}
					if (!base.LockFlip)
					{
						if (!this.AlwaysFaceTarget)
						{
							if (base.Heading.X < 0f)
							{
								this.Flip = SpriteEffects.FlipHorizontally;
							}
							else
							{
								this.Flip = SpriteEffects.None;
							}
						}
						else if (base.X > this.m_target.X)
						{
							this.Flip = SpriteEffects.FlipHorizontally;
						}
						else
						{
							this.Flip = SpriteEffects.None;
						}
					}
					if (!this.m_currentActiveLB.IsActive && !this.m_runCooldown)
					{
						switch (this.Difficulty)
						{
						case GameTypes.EnemyDifficulty.BASIC:
							this.RunBasicLogic();
							break;
						case GameTypes.EnemyDifficulty.ADVANCED:
							this.RunAdvancedLogic();
							break;
						case GameTypes.EnemyDifficulty.EXPERT:
							this.RunExpertLogic();
							break;
						case GameTypes.EnemyDifficulty.MINIBOSS:
							this.RunMinibossLogic();
							break;
						}
						if (this.m_runCooldown && this.m_currentActiveLB.ActiveLS.Tag == 2)
						{
							this.m_cooldownTimer = this.CooldownTime;
						}
					}
					if (!this.m_currentActiveLB.IsActive && this.m_runCooldown && this.m_cooldownTimer > 0f && !this.m_cooldownLB.IsActive)
					{
						this.m_currentActiveLB = this.m_cooldownLB;
						this.m_currentActiveLB.RunLogicBlock(this.m_cooldownParams);
					}
					if (base.IsWeighted && this.m_invincibleCounter <= 0f && this.m_invincibleCounterProjectile <= 0f)
					{
						if (base.HeadingX > 0f)
						{
							base.HeadingX = 1f;
						}
						else if (base.HeadingX < 0f)
						{
							base.HeadingX = -1f;
						}
						base.X += base.HeadingX * (base.CurrentSpeed * num);
					}
					else if (this.m_isTouchingGround || !base.IsWeighted)
					{
						base.Position += base.Heading * (base.CurrentSpeed * num);
					}
					if (base.X < (float)this.m_levelScreen.CurrentRoom.Bounds.Left)
					{
						base.X = (float)this.m_levelScreen.CurrentRoom.Bounds.Left;
					}
					else if (base.X > (float)this.m_levelScreen.CurrentRoom.Bounds.Right)
					{
						base.X = (float)this.m_levelScreen.CurrentRoom.Bounds.Right;
					}
					if (base.Y < (float)this.m_levelScreen.CurrentRoom.Bounds.Top)
					{
						base.Y = (float)this.m_levelScreen.CurrentRoom.Bounds.Top;
					}
					else if (base.Y > (float)this.m_levelScreen.CurrentRoom.Bounds.Bottom)
					{
						base.Y = (float)this.m_levelScreen.CurrentRoom.Bounds.Bottom;
					}
					if (this.m_currentActiveLB == this.m_cooldownLB)
					{
						this.m_currentActiveLB.Update(gameTime);
					}
					else
					{
						this.m_currentActiveLB.Update(gameTime);
						this.m_cooldownLB.Update(gameTime);
					}
				}
			}
			if (base.IsWeighted)
			{
				this.CheckGroundCollision();
			}
			if (base.CurrentHealth <= 0 && !base.IsKilled && !this.m_bossVersionKilled)
			{
				this.Kill(true);
			}
			base.Update(gameTime);
		}
		public void CheckGroundCollisionOld()
		{
			this.m_numTouchingGrounds = 0;
			float num = 2.14748365E+09f;
			int num2 = 10;
			bool flag = true;
			foreach (IPhysicsObj current in this.m_levelScreen.PhysicsManager.ObjectList)
			{
				if (current != this && current.CollidesTop && (current.CollisionTypeTag == 1 || current.CollisionTypeTag == 5 || current.CollisionTypeTag == 4 || current.CollisionTypeTag == 10) && Math.Abs(current.Bounds.Top - this.Bounds.Bottom) < num2)
				{
					foreach (CollisionBox current2 in current.CollisionBoxes)
					{
						if (current2.Type == 0)
						{
							Rectangle a = this.GroundCollisionRect;
							if (current2.AbsRotation != 0f)
							{
								a = this.RotatedGroundCollisionRect;
							}
							if (CollisionMath.RotatedRectIntersects(a, 0f, Vector2.Zero, current2.AbsRect, current2.AbsRotation, Vector2.Zero))
							{
								this.m_numTouchingGrounds++;
								if (current2.AbsParent.Rotation == 0f)
								{
									flag = false;
								}
								Vector2 vector = CollisionMath.RotatedRectIntersectsMTD(this.GroundCollisionRect, 0f, Vector2.Zero, current2.AbsRect, current2.AbsRotation, Vector2.Zero);
								if (flag)
								{
									flag = !CollisionMath.RotatedRectIntersects(this.Bounds, 0f, Vector2.Zero, current2.AbsRect, current2.AbsRotation, Vector2.Zero);
								}
								float y = vector.Y;
								if (num > y)
								{
									num = y;
								}
							}
						}
					}
				}
			}
			if (num <= 2f && base.AccelerationY >= 0f)
			{
				this.m_isTouchingGround = true;
			}
		}
		private void CheckGroundCollision()
		{
			this.m_isTouchingGround = false;
			this.m_numTouchingGrounds = 0;
			if (base.AccelerationY >= 0f)
			{
				IPhysicsObj physicsObj = null;
				float num = 3.40282347E+38f;
				IPhysicsObj physicsObj2 = null;
				float num2 = 3.40282347E+38f;
				Rectangle terrainBounds = this.TerrainBounds;
				terrainBounds.Height += 10;
				foreach (TerrainObj current in this.m_levelScreen.CurrentRoom.TerrainObjList)
				{
					if (current.Visible && current.IsCollidable && current.CollidesTop && current.HasTerrainHitBox && (current.CollisionTypeTag == 1 || current.CollisionTypeTag == 10 || current.CollisionTypeTag == 6 || current.CollisionTypeTag == 4))
					{
						if (current.Rotation == 0f)
						{
							Rectangle left = terrainBounds;
							left.X -= 30;
							left.Width += 60;
							Vector2 value = CollisionMath.CalculateMTD(left, current.Bounds);
							if (value != Vector2.Zero)
							{
								this.m_numTouchingGrounds++;
							}
							if (CollisionMath.CalculateMTD(terrainBounds, current.Bounds).Y < 0f)
							{
								int num3 = current.Bounds.Top - this.Bounds.Bottom;
								if ((float)num3 < num)
								{
									physicsObj = current;
									num = (float)num3;
								}
							}
						}
						else
						{
							Vector2 value2 = CollisionMath.RotatedRectIntersectsMTD(terrainBounds, base.Rotation, Vector2.Zero, current.TerrainBounds, current.Rotation, Vector2.Zero);
							if (value2 != Vector2.Zero)
							{
								this.m_numTouchingGrounds++;
							}
							if (value2.Y < 0f)
							{
								float y = value2.Y;
								if (y < num2 && value2.Y < 0f)
								{
									physicsObj2 = current;
									num2 = y;
								}
							}
							Rectangle terrainBounds2 = this.TerrainBounds;
							terrainBounds2.Height += 50;
							int num4 = 15;
							Vector2 pt = CollisionMath.RotatedRectIntersectsMTD(terrainBounds2, base.Rotation, Vector2.Zero, current.TerrainBounds, current.Rotation, Vector2.Zero);
							if (pt.Y < 0f)
							{
								float num5 = CDGMath.DistanceBetweenPts(pt, Vector2.Zero);
								float num6 = (float)(50.0 - Math.Sqrt((double)(num5 * num5 * 2f)));
								if (num6 > 0f && num6 < (float)num4)
								{
									base.Y += num6;
								}
								float y2 = value2.Y;
								if (y2 < num2)
								{
									physicsObj2 = current;
									num2 = y2;
								}
							}
						}
					}
					if (physicsObj != null)
					{
						this.m_isTouchingGround = true;
					}
					if (physicsObj2 != null)
					{
						this.m_isTouchingGround = true;
					}
				}
			}
		}
		private void HookToSlope(IPhysicsObj collisionObj)
		{
			base.UpdateCollisionBoxes();
			Rectangle terrainBounds = this.TerrainBounds;
			terrainBounds.Height += 100;
			float num = base.X;
			if (CollisionMath.RotatedRectIntersectsMTD(terrainBounds, base.Rotation, Vector2.Zero, collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero).Y < 0f)
			{
				bool flag = false;
				Vector2 vector;
				Vector2 vector2;
				if (collisionObj.Width > collisionObj.Height)
				{
					vector = CollisionMath.UpperLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
					vector2 = CollisionMath.UpperRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
					if (collisionObj.Rotation > 0f)
					{
						num = (float)this.TerrainBounds.Left;
					}
					else
					{
						num = (float)this.TerrainBounds.Right;
					}
					if (num > vector.X && num < vector2.X)
					{
						flag = true;
					}
				}
				else if (collisionObj.Rotation > 0f)
				{
					vector = CollisionMath.LowerLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
					vector2 = CollisionMath.UpperLeftCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
					num = (float)this.TerrainBounds.Right;
					if (num > vector.X && num < vector2.X)
					{
						flag = true;
					}
				}
				else
				{
					vector = CollisionMath.UpperRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
					vector2 = CollisionMath.LowerRightCorner(collisionObj.TerrainBounds, collisionObj.Rotation, Vector2.Zero);
					num = (float)this.TerrainBounds.Left;
					if (num > vector.X && num < vector2.X)
					{
						flag = true;
					}
				}
				if (flag)
				{
					float num2 = vector2.X - vector.X;
					float num3 = vector2.Y - vector.Y;
					float x = vector.X;
					float y = vector.Y;
					float num4 = y + (num - x) * (num3 / num2);
					num4 -= (float)this.TerrainBounds.Bottom - base.Y - 2f;
					base.Y = (float)((int)num4);
				}
			}
		}
		protected void SetCooldownLogicBlock(LogicBlock cooldownLB, params int[] percentage)
		{
			this.m_cooldownLB = cooldownLB;
			this.m_cooldownParams = percentage;
		}
		protected void RunLogicBlock(bool runCDLogicAfterward, LogicBlock block, params int[] percentage)
		{
			this.m_runCooldown = runCDLogicAfterward;
			this.m_currentActiveLB = block;
			this.m_currentActiveLB.RunLogicBlock(percentage);
		}
		protected virtual void RunBasicLogic()
		{
		}
		protected virtual void RunAdvancedLogic()
		{
			this.RunBasicLogic();
		}
		protected virtual void RunExpertLogic()
		{
			this.RunBasicLogic();
		}
		protected virtual void RunMinibossLogic()
		{
			this.RunBasicLogic();
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			IPhysicsObj physicsObj = otherBox.AbsParent as IPhysicsObj;
			Vector2 vector = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
			if (collisionResponseType == 2 && (physicsObj.CollisionTypeTag == 2 || physicsObj.CollisionTypeTag == 10 || (physicsObj.CollisionTypeTag == 10 && base.IsWeighted)) && ((!(otherBox.AbsParent is ProjectileObj) && this.m_invincibleCounter <= 0f) || (otherBox.AbsParent is ProjectileObj && (this.m_invincibleCounterProjectile <= 0f || (otherBox.AbsParent as ProjectileObj).IgnoreInvincibleCounter))))
			{
				if (this.IsDemented)
				{
					this.m_invincibleCounter = this.InvincibilityTime;
					this.m_invincibleCounterProjectile = this.InvincibilityTime;
					this.m_levelScreen.ImpactEffectPool.DisplayQuestionMark(new Vector2(base.X, (float)this.Bounds.Top));
					return;
				}
				int num = (physicsObj as IDealsDamageObj).Damage;
				bool isPlayer = false;
				if (physicsObj == this.m_target)
				{
					if (CDGMath.RandomFloat(0f, 1f) <= this.m_target.TotalCritChance && !this.NonKillable && physicsObj == this.m_target)
					{
						this.m_levelScreen.ImpactEffectPool.DisplayCriticalText(new Vector2(base.X, (float)this.Bounds.Top));
						num = (int)((float)num * this.m_target.TotalCriticalDamage);
					}
					isPlayer = true;
				}
				ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
				if (projectileObj != null)
				{
					this.m_invincibleCounterProjectile = this.InvincibilityTime;
					if (projectileObj.DestroysWithEnemy && !this.NonKillable)
					{
						projectileObj.RunDestroyAnimation(false);
					}
				}
				Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
				if (thisBox.AbsRotation != 0f || otherBox.AbsRotation != 0f)
				{
					center = Rectangle.Intersect(thisBox.AbsParent.Bounds, otherBox.AbsParent.Bounds).Center;
				}
				Vector2 collisionPt = new Vector2((float)center.X, (float)center.Y);
				if (projectileObj == null || (projectileObj != null && projectileObj.Spell != 20))
				{
					if (projectileObj != null || physicsObj.CollisionTypeTag != 10 || (physicsObj.CollisionTypeTag == 10 && base.IsWeighted))
					{
						this.HitEnemy(num, collisionPt, isPlayer);
					}
				}
				else if (projectileObj != null && projectileObj.Spell == 20 && base.CanBeKnockedBack && !this.IsPaused)
				{
					base.CurrentSpeed = 0f;
					float num2 = 3f;
					if (base.KnockBack == Vector2.Zero)
					{
						if (base.X < this.m_target.X)
						{
							base.AccelerationX = -this.m_target.EnemyKnockBack.X * num2;
						}
						else
						{
							base.AccelerationX = this.m_target.EnemyKnockBack.X * num2;
						}
						base.AccelerationY = -this.m_target.EnemyKnockBack.Y * num2;
					}
					else
					{
						if (base.X < this.m_target.X)
						{
							base.AccelerationX = -base.KnockBack.X * num2;
						}
						else
						{
							base.AccelerationX = base.KnockBack.X * num2;
						}
						base.AccelerationY = -base.KnockBack.Y * num2;
					}
				}
				if (physicsObj == this.m_target)
				{
					this.m_invincibleCounter = this.InvincibilityTime;
				}
			}
			if (collisionResponseType == 1 && (physicsObj.CollisionTypeTag == 1 || physicsObj.CollisionTypeTag == 6 || physicsObj.CollisionTypeTag == 10) && base.CollisionTypeTag != 4)
			{
				if (base.CurrentSpeed != 0f && vector.X != 0f && Math.Abs(vector.X) > 10f && ((vector.X > 0f && physicsObj.CollidesRight) || (vector.X < 0f && physicsObj.CollidesLeft)))
				{
					base.CurrentSpeed = 0f;
				}
				if (this.m_numTouchingGrounds <= 1 && base.CurrentSpeed != 0f && vector.Y < 0f && !this.CanFallOffLedges)
				{
					if (this.Bounds.Left < physicsObj.Bounds.Left && base.HeadingX < 0f)
					{
						base.X = (float)physicsObj.Bounds.Left + (base.AbsX - (float)this.Bounds.Left);
						base.CurrentSpeed = 0f;
					}
					else if (this.Bounds.Right > physicsObj.Bounds.Right && base.HeadingX > 0f)
					{
						base.X = (float)physicsObj.Bounds.Right - ((float)this.Bounds.Right - base.AbsX);
						base.CurrentSpeed = 0f;
					}
					this.m_isTouchingGround = true;
				}
				if (base.AccelerationX != 0f && this.m_isTouchingGround)
				{
					base.AccelerationX = 0f;
				}
				bool flag = false;
				if (Math.Abs(vector.X) < 10f && vector.X != 0f && Math.Abs(vector.Y) < 10f && vector.Y != 0f)
				{
					flag = true;
				}
				if (this.m_isTouchingGround && !physicsObj.CollidesBottom && physicsObj.CollidesTop && physicsObj.TerrainBounds.Top < this.TerrainBounds.Bottom - 30)
				{
					flag = true;
				}
				if (!physicsObj.CollidesRight && !physicsObj.CollidesLeft && physicsObj.CollidesTop && physicsObj.CollidesBottom)
				{
					flag = true;
				}
				Vector2 vector2 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
				if (!flag)
				{
					base.CollisionResponse(thisBox, otherBox, collisionResponseType);
				}
				if (vector2.Y < 0f && otherBox.AbsRotation != 0f && base.IsWeighted)
				{
					base.X -= vector2.X;
				}
			}
		}
		public virtual void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
		{
			if (this.m_target != null && this.m_target.CurrentHealth > 0)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
				{
					"EnemyHit1",
					"EnemyHit2",
					"EnemyHit3",
					"EnemyHit4",
					"EnemyHit5",
					"EnemyHit6"
				});
				base.Blink(Color.Red, 0.1f);
				this.m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(collisionPt);
				if (isPlayer && (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14))
				{
					base.CurrentHealth -= damage;
					this.m_target.CurrentMana += (float)((int)((float)damage * 0.3f));
					this.m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(base.X, (float)this.Bounds.Top));
					this.m_levelScreen.TextManager.DisplayNumberStringText((int)((float)damage * 0.3f), "mp", Color.RoyalBlue, new Vector2(this.m_target.X, (float)(this.m_target.Bounds.Top - 30)));
				}
				else
				{
					base.CurrentHealth -= damage;
					this.m_levelScreen.TextManager.DisplayNumberText(damage, Color.Red, new Vector2(base.X, (float)this.Bounds.Top));
				}
				if (isPlayer)
				{
					PlayerObj expr_198 = this.m_target;
					expr_198.NumSequentialAttacks += 1;
					if (this.m_target.IsAirAttacking)
					{
						this.m_target.IsAirAttacking = false;
						this.m_target.AccelerationY = -this.m_target.AirAttackKnockBack;
						this.m_target.NumAirBounces++;
					}
				}
				if (base.CanBeKnockedBack && !this.IsPaused && Game.PlayerStats.Traits.X != 17f && Game.PlayerStats.Traits.Y != 17f)
				{
					base.CurrentSpeed = 0f;
					float num = 1f;
					if (Game.PlayerStats.Traits.X == 16f || Game.PlayerStats.Traits.Y == 16f)
					{
						num = 2f;
					}
					if (base.KnockBack == Vector2.Zero)
					{
						if (base.X < this.m_target.X)
						{
							base.AccelerationX = -this.m_target.EnemyKnockBack.X * num;
						}
						else
						{
							base.AccelerationX = this.m_target.EnemyKnockBack.X * num;
						}
						base.AccelerationY = -this.m_target.EnemyKnockBack.Y * num;
					}
					else
					{
						if (base.X < this.m_target.X)
						{
							base.AccelerationX = -base.KnockBack.X * num;
						}
						else
						{
							base.AccelerationX = base.KnockBack.X * num;
						}
						base.AccelerationY = -base.KnockBack.Y * num;
					}
				}
				this.m_levelScreen.SetLastEnemyHit(this);
			}
		}
		public void KillSilently()
		{
			base.Kill(false);
		}
		public override void Kill(bool giveXP = true)
		{
			int totalVampBonus = this.m_target.TotalVampBonus;
			if (totalVampBonus > 0)
			{
				this.m_target.CurrentHealth += totalVampBonus;
				this.m_levelScreen.TextManager.DisplayNumberStringText(totalVampBonus, "hp", Color.LightGreen, new Vector2(this.m_target.X, (float)(this.m_target.Bounds.Top - 60)));
			}
			if (this.m_target.ManaGain > 0f)
			{
				this.m_target.CurrentMana += this.m_target.ManaGain;
				this.m_levelScreen.TextManager.DisplayNumberStringText((int)this.m_target.ManaGain, "mp", Color.RoyalBlue, new Vector2(this.m_target.X, (float)(this.m_target.Bounds.Top - 90)));
			}
			if (Game.PlayerStats.SpecialItem == 5)
			{
				this.m_levelScreen.ItemDropManager.DropItem(base.Position, 1, 10f);
				this.m_levelScreen.ItemDropManager.DropItem(base.Position, 1, 10f);
			}
			this.m_levelScreen.KillEnemy(this);
			SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Enemy_Death");
			if (this.DropsItem)
			{
				if (this.Type == 26)
				{
					this.m_levelScreen.ItemDropManager.DropItem(base.Position, 2, 0.1f);
				}
				else if (CDGMath.RandomInt(1, 100) <= 2)
				{
					if (CDGMath.RandomPlusMinus() < 0)
					{
						this.m_levelScreen.ItemDropManager.DropItem(base.Position, 2, 0.1f);
					}
					else
					{
						this.m_levelScreen.ItemDropManager.DropItem(base.Position, 3, 0.1f);
					}
				}
				if (CDGMath.RandomFloat(0f, 1f) <= this.MoneyDropChance)
				{
					int num = CDGMath.RandomInt(this.MinMoneyDropAmount, this.MaxMoneyDropAmount) * 10 + (int)(CDGMath.RandomFloat(this.MinMoneyGainPerLevel, this.MaxMoneyGainPerLevel) * (float)this.Level * 10f);
					int num2 = num / 500;
					num -= num2 * 500;
					int num3 = num / 100;
					num -= num3 * 100;
					int num4 = num / 10;
					for (int i = 0; i < num2; i++)
					{
						this.m_levelScreen.ItemDropManager.DropItem(base.Position, 11, 500f);
					}
					for (int j = 0; j < num3; j++)
					{
						this.m_levelScreen.ItemDropManager.DropItem(base.Position, 10, 100f);
					}
					for (int k = 0; k < num4; k++)
					{
						this.m_levelScreen.ItemDropManager.DropItem(base.Position, 1, 10f);
					}
				}
			}
			if (this.m_currentActiveLB.IsActive)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			this.m_levelScreen.ImpactEffectPool.DisplayDeathEffect(base.Position);
			if ((Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15) && this.GivesLichHealth)
			{
				int num5 = 0;
				int currentLevel = Game.PlayerStats.CurrentLevel;
				int num6 = (int)((float)this.Level * 2.75f);
				if (currentLevel < num6)
				{
					num5 = 4;
				}
				else if (currentLevel >= num6)
				{
					num5 = 4;
				}
				int num7 = (int)Math.Round((double)(((float)(this.m_target.BaseHealth + this.m_target.GetEquipmentHealth() + Game.PlayerStats.BonusHealth * 5) + SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount + SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount) * 1f), MidpointRounding.AwayFromZero);
				if (this.m_target.MaxHealth + num5 < num7)
				{
					Game.PlayerStats.LichHealth += num5;
					this.m_target.CurrentHealth += num5;
					this.m_levelScreen.TextManager.DisplayNumberStringText(num5, "max hp", Color.LightGreen, new Vector2(this.m_target.X, (float)(this.m_target.Bounds.Top - 30)));
				}
			}
			Game.PlayerStats.NumEnemiesBeaten++;
			if (this.m_saveToEnemiesKilledList)
			{
				Vector4 value = Game.PlayerStats.EnemiesKilledList[(int)this.Type];
				switch (this.Difficulty)
				{
				case GameTypes.EnemyDifficulty.BASIC:
					value.X += 1f;
					break;
				case GameTypes.EnemyDifficulty.ADVANCED:
					value.Y += 1f;
					break;
				case GameTypes.EnemyDifficulty.EXPERT:
					value.Z += 1f;
					break;
				case GameTypes.EnemyDifficulty.MINIBOSS:
					value.W += 1f;
					break;
				}
				Game.PlayerStats.EnemiesKilledList[(int)this.Type] = value;
			}
			if (giveXP && this.Type == 26)
			{
				GameUtil.UnlockAchievement("FEAR_OF_CHICKENS");
			}
			base.Kill(true);
		}
		public void PauseEnemy(bool forcePause = false)
		{
			if ((!this.IsPaused && !base.IsKilled && !this.m_bossVersionKilled) || forcePause)
			{
				this.m_isPaused = true;
				base.DisableAllWeight = true;
				base.PauseAnimation();
			}
		}
		public void UnpauseEnemy(bool forceUnpause = false)
		{
			if ((this.IsPaused && !base.IsKilled && !this.m_bossVersionKilled) || forceUnpause)
			{
				this.m_isPaused = false;
				base.DisableAllWeight = false;
				base.ResumeAnimation();
			}
		}
		public void DrawDetectionRadii(Camera2D camera)
		{
			camera.Draw(this.m_engageRadiusTexture, new Vector2(base.Position.X - (float)this.EngageRadius, base.Position.Y - (float)this.EngageRadius), Color.Red * 0.5f);
			camera.Draw(this.m_projectileRadiusTexture, new Vector2(base.Position.X - (float)this.ProjectileRadius, base.Position.Y - (float)this.ProjectileRadius), Color.Blue * 0.5f);
			camera.Draw(this.m_meleeRadiusTexture, new Vector2(base.Position.X - (float)this.MeleeRadius, base.Position.Y - (float)this.MeleeRadius), Color.Green * 0.5f);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				if (this.m_currentActiveLB.IsActive)
				{
					this.m_currentActiveLB.StopLogicBlock();
				}
				this.m_currentActiveLB = null;
				foreach (LogicBlock current in this.logicBlocksToDispose)
				{
					current.Dispose();
				}
				for (int i = 0; i < this.logicBlocksToDispose.Count; i++)
				{
					this.logicBlocksToDispose[i] = null;
				}
				this.logicBlocksToDispose.Clear();
				this.logicBlocksToDispose = null;
				this.m_target = null;
				this.m_walkingLB.Dispose();
				this.m_walkingLB = null;
				if (this.m_cooldownLB.IsActive)
				{
					this.m_cooldownLB.StopLogicBlock();
				}
				this.m_cooldownLB.Dispose();
				this.m_cooldownLB = null;
				if (this.m_engageRadiusTexture != null)
				{
					this.m_engageRadiusTexture.Dispose();
				}
				this.m_engageRadiusTexture = null;
				if (this.m_engageRadiusTexture != null)
				{
					this.m_projectileRadiusTexture.Dispose();
				}
				this.m_projectileRadiusTexture = null;
				if (this.m_engageRadiusTexture != null)
				{
					this.m_meleeRadiusTexture.Dispose();
				}
				this.m_meleeRadiusTexture = null;
				if (this.m_cooldownParams != null)
				{
					Array.Clear(this.m_cooldownParams, 0, this.m_cooldownParams.Length);
				}
				this.m_cooldownParams = null;
				this.TintablePart = null;
				this.m_flipTween = null;
				base.Dispose();
			}
		}
		public override void Reset()
		{
			if (this.m_currentActiveLB.IsActive)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			if (this.m_cooldownLB.IsActive)
			{
				this.m_cooldownLB.StopLogicBlock();
			}
			this.m_invincibleCounter = 0f;
			this.m_invincibleCounterProjectile = 0f;
			base.State = 0;
			this.ChangeSprite(this.m_resetSpriteName);
			if (this.PlayAnimationOnRestart)
			{
				base.PlayAnimation(true);
			}
			this.m_initialDelayCounter = this.InitialLogicDelay;
			this.UnpauseEnemy(true);
			this.m_bossVersionKilled = false;
			this.m_blinkTimer = 0f;
			base.Reset();
		}
		public virtual void ResetState()
		{
			if (this.m_currentActiveLB.IsActive)
			{
				this.m_currentActiveLB.StopLogicBlock();
			}
			if (this.m_cooldownLB.IsActive)
			{
				this.m_cooldownLB.StopLogicBlock();
			}
			this.m_invincibleCounter = 0f;
			this.m_invincibleCounterProjectile = 0f;
			base.State = 0;
			if (this.Type != 32)
			{
				this.ChangeSprite(this.m_resetSpriteName);
			}
			if (this.PlayAnimationOnRestart)
			{
				base.PlayAnimation(true);
			}
			this.m_initialDelayCounter = this.InitialLogicDelay;
			base.LockFlip = this.m_internalLockFlip;
			this.Flip = this.InternalFlip;
			base.AnimationDelay = this.m_internalAnimationDelay;
			this.UnpauseEnemy(true);
			base.CurrentHealth = this.MaxHealth;
			this.m_blinkTimer = 0f;
		}
		protected float ParseTagToFloat(string key)
		{
			if (this.Tag != "")
			{
				int num = this.Tag.IndexOf(key + ":") + key.Length + 1;
				int num2 = this.Tag.IndexOf(",", num);
				if (num2 == -1)
				{
					num2 = this.Tag.Length;
				}
				try
				{
					CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
					cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
					float result = float.Parse(this.Tag.Substring(num, num2 - num), NumberStyles.Any, cultureInfo);
					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine(string.Concat(new string[]
					{
						"Could not parse key:",
						key,
						" with string:",
						this.Tag,
						".  Original Error: ",
						ex.Message
					}));
					float result = 0f;
					return result;
				}
			}
			return 0f;
		}
		protected string ParseTagToString(string key)
		{
			int num = this.Tag.IndexOf(key + ":") + key.Length + 1;
			int num2 = this.Tag.IndexOf(",", num);
			if (num2 == -1)
			{
				num2 = this.Tag.Length;
			}
			return this.Tag.Substring(num, num2 - num);
		}
		protected override GameObj CreateCloneInstance()
		{
			return EnemyBuilder.BuildEnemy((int)this.Type, this.m_target, null, this.m_levelScreen, this.Difficulty, false);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			EnemyObj enemyObj = obj as EnemyObj;
			enemyObj.IsProcedural = this.IsProcedural;
			enemyObj.InitialLogicDelay = this.InitialLogicDelay;
			enemyObj.NonKillable = this.NonKillable;
			enemyObj.GivesLichHealth = this.GivesLichHealth;
			enemyObj.DropsItem = this.DropsItem;
			enemyObj.IsDemented = this.IsDemented;
		}
	}
}
