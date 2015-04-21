using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Energon : EnemyObj
	{
		private const byte TYPE_SWORD = 0;
		private const byte TYPE_SHIELD = 1;
		private const byte TYPE_DOWNSWORD = 2;
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalAdvancedLB = new LogicBlock();
		private LogicBlock m_generalExpertLB = new LogicBlock();
		private LogicBlock m_generalMiniBossLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private SpriteObj m_shield;
		private DS2DPool<EnergonProjectileObj> m_projectilePool;
		private byte m_poolSize = 10;
		private byte m_currentAttackType;
		protected override void InitializeEV()
		{
			base.Name = "Energon";
			this.MaxHealth = 25;
			base.Damage = 25;
			base.XPValue = 150;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 75f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 300f;
			base.JumpHeight = 950f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.Energon_Basic_Scale;
			base.ProjectileScale = EnemyEV.Energon_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Energon_Basic_Tint;
			this.MeleeRadius = 150;
			this.ProjectileRadius = 750;
			this.EngageRadius = 850;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Energon_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Mastertron";
				this.MaxHealth = 38;
				base.Damage = 29;
				base.XPValue = 250;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 100f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 300f;
				base.JumpHeight = 950f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Energon_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Energon_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Energon_Advanced_Tint;
				this.MeleeRadius = 150;
				this.EngageRadius = 850;
				this.ProjectileRadius = 750;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Energon_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Voltron";
				this.MaxHealth = 61;
				base.Damage = 33;
				base.XPValue = 375;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 125f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 300f;
				base.JumpHeight = 950f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Energon_Expert_Scale;
				base.ProjectileScale = EnemyEV.Energon_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Energon_Expert_Tint;
				this.MeleeRadius = 150;
				this.ProjectileRadius = 750;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Energon_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Prime";
				this.MaxHealth = 300;
				base.Damage = 39;
				base.XPValue = 1500;
				this.MinMoneyDropAmount = 8;
				this.MaxMoneyDropAmount = 16;
				this.MoneyDropChance = 1f;
				base.Speed = 200f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 300f;
				base.JumpHeight = 950f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.Energon_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Energon_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Energon_Miniboss_Tint;
				this.MeleeRadius = 150;
				this.ProjectileRadius = 750;
				this.EngageRadius = 850;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Energon_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new ChangeSpriteLogicAction("EnemyEnergonWalk_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyEnergonWalk_Character", true, true), Types.Sequence.Serial);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character", true, true), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(0.2f, 0.75f, false), Types.Sequence.Serial);
			LogicSet logicSet4 = new LogicSet(this);
			logicSet4.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEnergonAttack_Character", false, false), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack", false), Types.Sequence.Serial);
			logicSet4.AddAction(new RunFunctionLogicAction(this, "FireCurrentTypeProjectile", new object[0]), Types.Sequence.Serial);
			logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character", true, true), Types.Sequence.Serial);
			logicSet4.Tag = 2;
			LogicSet logicSet5 = new LogicSet(this);
			logicSet5.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEnergonAttack_Character", false, false), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack", false), Types.Sequence.Serial);
			logicSet5.AddAction(new RunFunctionLogicAction(this, "SwitchRandomType", new object[0]), Types.Sequence.Serial);
			logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End", false), Types.Sequence.Serial);
			logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character", true, true), Types.Sequence.Serial);
			logicSet5.AddAction(new DelayLogicAction(2f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3,
				logicSet4,
				logicSet5
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
			this.logicBlocksToDispose.Add(this.m_generalMiniBossLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
			{
				0,
				0,
				100
			});
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
				break;
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					0,
					30,
					60,
					10
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
			case 1:
			case 2:
			case 3:
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
				return;
			}
		}
		public void FireCurrentTypeProjectile()
		{
			this.FireProjectile(this.m_currentAttackType);
		}
		public void FireProjectile(byte type)
		{
			EnergonProjectileObj energonProjectileObj = this.m_projectilePool.CheckOut();
			energonProjectileObj.SetType(type);
			base.PhysicsMngr.AddObject(energonProjectileObj);
			energonProjectileObj.Target = this.m_target;
			energonProjectileObj.Visible = true;
			energonProjectileObj.Position = base.Position;
			energonProjectileObj.CurrentSpeed = this.ProjectileSpeed;
			energonProjectileObj.Flip = this.Flip;
			energonProjectileObj.Scale = base.ProjectileScale;
			energonProjectileObj.Opacity = 0.8f;
			energonProjectileObj.Damage = base.Damage;
			energonProjectileObj.PlayAnimation(true);
		}
		public void DestroyProjectile(EnergonProjectileObj projectile)
		{
			if (this.m_projectilePool.ActiveObjsList.Contains(projectile))
			{
				projectile.Visible = false;
				projectile.Scale = new Vector2(1f, 1f);
				projectile.CollisionTypeTag = 3;
				base.PhysicsMngr.RemoveObject(projectile);
				this.m_projectilePool.CheckIn(projectile);
			}
		}
		public void DestroyAllProjectiles()
		{
			ProjectileObj[] array = this.m_projectilePool.ActiveObjsList.ToArray();
			ProjectileObj[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				EnergonProjectileObj projectile = (EnergonProjectileObj)array2[i];
				this.DestroyProjectile(projectile);
			}
		}
		public void SwitchRandomType()
		{
			byte b;
			for (b = this.m_currentAttackType; b == this.m_currentAttackType; b = (byte)CDGMath.RandomInt(0, 2))
			{
			}
			this.SwitchType(b);
		}
		public void SwitchType(byte type)
		{
			this.m_currentAttackType = type;
			switch (type)
			{
			case 0:
				this.m_shield.ChangeSprite("EnergonSwordShield_Sprite");
				break;
			case 1:
				this.m_shield.ChangeSprite("EnergonShieldShield_Sprite");
				break;
			case 2:
				this.m_shield.ChangeSprite("EnergonDownSwordShield_Sprite");
				break;
			}
			this.m_shield.PlayAnimation(true);
		}
		public override void Update(GameTime gameTime)
		{
			foreach (EnergonProjectileObj current in this.m_projectilePool.ActiveObjsList)
			{
				current.Update(gameTime);
			}
			base.Update(gameTime);
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			this.m_shield.Position = base.Position;
			this.m_shield.Flip = this.Flip;
			this.m_shield.Draw(camera);
			foreach (ProjectileObj current in this.m_projectilePool.ActiveObjsList)
			{
				current.Draw(camera);
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (collisionResponseType == 2 && this.m_invincibleCounter <= 0f && otherBox.AbsParent is PlayerObj)
			{
				if ((float)(this.m_target.Bounds.Left + this.m_target.Bounds.Width / 2) < base.X)
				{
					this.m_target.AccelerationX = -this.m_target.EnemyKnockBack.X;
				}
				else
				{
					this.m_target.AccelerationX = this.m_target.EnemyKnockBack.X;
				}
				this.m_target.AccelerationY = -this.m_target.EnemyKnockBack.Y;
				Point center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
				Vector2 position = new Vector2((float)center.X, (float)center.Y);
				this.m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(position, Vector2.One);
				this.m_levelScreen.SetLastEnemyHit(this);
				this.m_invincibleCounter = base.InvincibilityTime;
				base.Blink(Color.LightBlue, 0.1f);
				ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
				if (projectileObj != null)
				{
					this.m_levelScreen.ProjectileManager.DestroyProjectile(projectileObj);
					return;
				}
			}
			else if (otherBox.AbsParent is EnergonProjectileObj)
			{
				EnergonProjectileObj energonProjectileObj = otherBox.AbsParent as EnergonProjectileObj;
				if (energonProjectileObj != null)
				{
					Point center2 = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
					Vector2 vector = new Vector2((float)center2.X, (float)center2.Y);
					this.DestroyProjectile(energonProjectileObj);
					if (energonProjectileObj.AttackType == this.m_currentAttackType)
					{
						this.HitEnemy(energonProjectileObj.Damage, vector, true);
						return;
					}
					this.m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(vector, Vector2.One);
					return;
				}
			}
			else
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
		}
		public override void Kill(bool giveXP = true)
		{
			this.m_shield.Visible = false;
			this.DestroyAllProjectiles();
			base.Kill(giveXP);
		}
		public override void Reset()
		{
			this.m_shield.Visible = true;
			base.Reset();
		}
		public EnemyObj_Energon(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyEnergonIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 23;
			this.m_shield = new SpriteObj("EnergonSwordShield_Sprite");
			this.m_shield.AnimationDelay = 0.1f;
			this.m_shield.PlayAnimation(true);
			this.m_shield.Opacity = 0.5f;
			this.m_shield.Scale = new Vector2(1.2f, 1.2f);
			this.m_projectilePool = new DS2DPool<EnergonProjectileObj>();
			for (int i = 0; i < (int)this.m_poolSize; i++)
			{
				EnergonProjectileObj energonProjectileObj = new EnergonProjectileObj("EnergonSwordProjectile_Sprite", this);
				energonProjectileObj.Visible = false;
				energonProjectileObj.CollidesWithTerrain = false;
				energonProjectileObj.PlayAnimation(true);
				energonProjectileObj.AnimationDelay = 0.05f;
				this.m_projectilePool.AddToPool(energonProjectileObj);
			}
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_projectilePool.Dispose();
				this.m_projectilePool = null;
				this.m_shield.Dispose();
				this.m_shield = null;
				base.Dispose();
			}
		}
	}
}
