using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_Spark : EnemyObj
	{
		private bool m_hookedToGround;
		private byte m_collisionBoxSize = 10;
		private Rectangle TopRect
		{
			get
			{
				return new Rectangle(this.Bounds.Left + (int)this.m_collisionBoxSize, this.Bounds.Top, this.Width - (int)(this.m_collisionBoxSize * 2), (int)this.m_collisionBoxSize);
			}
		}
		private Rectangle BottomRect
		{
			get
			{
				return new Rectangle(this.Bounds.Left + (int)this.m_collisionBoxSize, this.Bounds.Bottom - (int)this.m_collisionBoxSize, this.Width - (int)(this.m_collisionBoxSize * 2), (int)this.m_collisionBoxSize);
			}
		}
		private Rectangle LeftRect
		{
			get
			{
				return new Rectangle(this.Bounds.Left, this.Bounds.Top + (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize, this.Height - (int)(this.m_collisionBoxSize * 2));
			}
		}
		private Rectangle RightRect
		{
			get
			{
				return new Rectangle(this.Bounds.Right - (int)this.m_collisionBoxSize, this.Bounds.Top + (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize, this.Height - (int)(this.m_collisionBoxSize * 2));
			}
		}
		private Rectangle TopLeftPoint
		{
			get
			{
				return new Rectangle(this.Bounds.Left, this.Bounds.Top, (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize);
			}
		}
		private Rectangle TopRightPoint
		{
			get
			{
				return new Rectangle(this.Bounds.Right - (int)this.m_collisionBoxSize, this.Bounds.Top, (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize);
			}
		}
		private Rectangle BottomLeftPoint
		{
			get
			{
				return new Rectangle(this.Bounds.Left, this.Bounds.Bottom - (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize);
			}
		}
		private Rectangle BottomRightPoint
		{
			get
			{
				return new Rectangle(this.Bounds.Right - (int)this.m_collisionBoxSize, this.Bounds.Bottom - (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize, (int)this.m_collisionBoxSize);
			}
		}
		protected override void InitializeEV()
		{
			base.LockFlip = true;
			base.IsWeighted = false;
			base.Name = "Sparky";
			this.MaxHealth = 20;
			base.Damage = 20;
			base.XPValue = 100;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 200f;
			this.TurnSpeed = 0.0275f;
			this.ProjectileSpeed = 525f;
			base.JumpHeight = 500f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = false;
			base.IsWeighted = false;
			this.Scale = EnemyEV.Spark_Basic_Scale;
			base.ProjectileScale = EnemyEV.Spark_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.Spark_Basic_Tint;
			this.MeleeRadius = 10;
			this.ProjectileRadius = 20;
			this.EngageRadius = 30;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.Spark_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				base.Name = "Mr. Spark";
				this.MaxHealth = 20;
				base.Damage = 28;
				base.XPValue = 175;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 300f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 525f;
				base.JumpHeight = 500f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Spark_Advanced_Scale;
				base.ProjectileScale = EnemyEV.Spark_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Spark_Advanced_Tint;
				this.MeleeRadius = 10;
				this.EngageRadius = 30;
				this.ProjectileRadius = 20;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Spark_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				base.Name = "Grandpa Spark";
				this.MaxHealth = 20;
				base.Damage = 32;
				base.XPValue = 350;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 3;
				this.MoneyDropChance = 1f;
				base.Speed = 375f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 525f;
				base.JumpHeight = 500f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Spark_Expert_Scale;
				base.ProjectileScale = EnemyEV.Spark_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Spark_Expert_Tint;
				this.MeleeRadius = 10;
				this.ProjectileRadius = 20;
				this.EngageRadius = 30;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Spark_Expert_KnockBack;
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Lord Spark, King of Sparkatonia";
				this.MaxHealth = 500;
				base.Damage = 45;
				base.XPValue = 800;
				this.MinMoneyDropAmount = 6;
				this.MaxMoneyDropAmount = 10;
				this.MoneyDropChance = 1f;
				base.Speed = 380f;
				this.TurnSpeed = 0.03f;
				this.ProjectileSpeed = 0f;
				base.JumpHeight = 500f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = false;
				base.IsWeighted = false;
				this.Scale = EnemyEV.Spark_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.Spark_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.Spark_Miniboss_Tint;
				this.MeleeRadius = 10;
				this.ProjectileRadius = 20;
				this.EngageRadius = 30;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.Spark_Miniboss_KnockBack;
				return;
			default:
				return;
			}
		}
		protected override void InitializeLogic()
		{
			base.CurrentSpeed = base.Speed;
			base.InitializeLogic();
		}
		public void HookToGround()
		{
			this.m_hookedToGround = true;
			float num = 1000f;
			TerrainObj terrainObj = null;
			foreach (TerrainObj current in this.m_levelScreen.CurrentRoom.TerrainObjList)
			{
				if (current.Y >= base.Y && current.Y - base.Y < num && CollisionMath.Intersects(current.Bounds, new Rectangle((int)base.X, (int)(base.Y + (current.Y - base.Y) + 5f), this.Width, this.Height / 2)))
				{
					num = current.Y - base.Y;
					terrainObj = current;
				}
			}
			if (terrainObj != null)
			{
				base.Y = terrainObj.Y - (float)(this.Height / 2) + 5f;
			}
		}
		protected override void RunBasicLogic()
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
		public override void Update(GameTime gameTime)
		{
			if (!this.m_hookedToGround)
			{
				this.HookToGround();
			}
			this.CollisionCheckRight();
			if (!base.IsPaused)
			{
				base.Position += base.Heading * (base.CurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
			}
			base.Update(gameTime);
		}
		private void CollisionCheckRight()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			float num = 0f;
			if (this.Bounds.Right >= this.m_levelScreen.CurrentRoom.Bounds.Right)
			{
				flag6 = true;
				flag4 = true;
				flag8 = true;
			}
			else if (this.Bounds.Left <= this.m_levelScreen.CurrentRoom.Bounds.Left)
			{
				flag5 = true;
				flag3 = true;
				flag7 = true;
			}
			if (this.Bounds.Top <= this.m_levelScreen.CurrentRoom.Bounds.Top)
			{
				flag6 = true;
				flag = true;
				flag5 = true;
			}
			else if (this.Bounds.Bottom >= this.m_levelScreen.CurrentRoom.Bounds.Bottom)
			{
				flag7 = true;
				flag2 = true;
				flag8 = true;
			}
			foreach (TerrainObj current in this.m_levelScreen.CurrentRoom.TerrainObjList)
			{
				Rectangle b = new Rectangle((int)current.X, (int)current.Y, current.Width, current.Height);
				if (CollisionMath.RotatedRectIntersects(this.TopLeftPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag5 = true;
				}
				if (CollisionMath.RotatedRectIntersects(this.TopRightPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag6 = true;
				}
				if (CollisionMath.RotatedRectIntersects(this.BottomRightPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag8 = true;
					if (current.Rotation != 0f)
					{
						Vector2 vector = CollisionMath.RotatedRectIntersectsMTD(this.BottomRightPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero);
						if (vector.X < 0f && vector.Y < 0f)
						{
							num = -45f;
						}
					}
				}
				if (CollisionMath.RotatedRectIntersects(this.BottomLeftPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag7 = true;
					if (current.Rotation != 0f)
					{
						Vector2 vector2 = CollisionMath.RotatedRectIntersectsMTD(this.BottomLeftPoint, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero);
						if (vector2.X > 0f && vector2.Y < 0f)
						{
							num = 45f;
						}
					}
				}
				if (CollisionMath.RotatedRectIntersects(this.TopRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag = true;
				}
				if (CollisionMath.RotatedRectIntersects(this.BottomRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag2 = true;
				}
				if (CollisionMath.RotatedRectIntersects(this.LeftRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag3 = true;
				}
				if (CollisionMath.RotatedRectIntersects(this.RightRect, 0f, Vector2.Zero, b, current.Rotation, Vector2.Zero))
				{
					flag4 = true;
				}
			}
			if (flag8 && !flag6 && !flag4)
			{
				base.Orientation = 0f;
			}
			if (flag6 && flag8 && !flag5)
			{
				base.Orientation = MathHelper.ToRadians(-90f);
			}
			if (flag6 && flag5 && !flag7)
			{
				base.Orientation = MathHelper.ToRadians(-180f);
			}
			if (flag5 && flag3 && !flag2)
			{
				base.Orientation = MathHelper.ToRadians(90f);
			}
			if (flag6 && !flag && !flag4)
			{
				base.Orientation = MathHelper.ToRadians(-90f);
			}
			if (flag5 && !flag && !flag3)
			{
				base.Orientation = MathHelper.ToRadians(-180f);
			}
			if (flag7 && !flag3 && !flag4 && !flag2)
			{
				base.Orientation = MathHelper.ToRadians(90f);
			}
			if (flag8 && !flag2 && !flag4)
			{
				base.Orientation = 0f;
			}
			if (num != 0f && ((num < 0f && flag8 && flag4) || (num > 0f && !flag8)))
			{
				base.Orientation = MathHelper.ToRadians(num);
			}
			base.HeadingX = (float)Math.Cos((double)base.Orientation);
			base.HeadingY = (float)Math.Sin((double)base.Orientation);
		}
		public EnemyObj_Spark(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySpark_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			base.IsWeighted = false;
			base.ForceDraw = true;
			this.Type = 24;
			base.NonKillable = true;
		}
	}
}
