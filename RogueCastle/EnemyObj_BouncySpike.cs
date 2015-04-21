/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class EnemyObj_BouncySpike : EnemyObj
	{
		private float RotationSpeed = 250f;
		private float m_internalOrientation;
		public RoomObj SpawnRoom;
		private float m_selfDestructTimer = 0.7f;
		private int m_selfDestructCounter;
		private int m_selfDestructTotalBounces = 12;
		public Vector2 SavedStartingPos
		{
			get;
			set;
		}
		protected override void InitializeEV()
		{
			int num = CDGMath.RandomInt(0, 11);
			if (num >= 9)
			{
				Orientation = 0f;
			}
			else if (num >= 6)
			{
				Orientation = 180f;
			}
			else if (num >= 4)
			{
				Orientation = 90f;
			}
			else if (num >= 1)
			{
				Orientation = 270f;
			}
			else
			{
				Orientation = 45f;
			}
			m_internalOrientation = Orientation;
			HeadingX = (float)Math.Cos(MathHelper.ToRadians(Orientation));
			HeadingY = (float)Math.Sin(MathHelper.ToRadians(Orientation));
			Name = "Spiketor";
			MaxHealth = 5;
			Damage = 27;
			XPValue = 25;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 1;
			MoneyDropChance = 0.4f;
			Speed = 300f;
			TurnSpeed = 10f;
			ProjectileSpeed = 650f;
			JumpHeight = 300f;
			CooldownTime = 2f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = false;
			CanBeKnockedBack = false;
			IsWeighted = false;
			Scale = EnemyEV.BouncySpike_Basic_Scale;
			ProjectileScale = EnemyEV.BouncySpike_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.BouncySpike_Basic_Tint;
			MeleeRadius = 50;
			ProjectileRadius = 100;
			EngageRadius = 150;
			ProjectileDamage = Damage;
			KnockBack = new Vector2(1f, 2f);
			LockFlip = true;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
				break;
			case GameTypes.EnemyDifficulty.ADVANCED:
				Name = "Spiketex";
				MaxHealth = 5;
				Damage = 32;
				XPValue = 25;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 1;
				MoneyDropChance = 0.5f;
				Speed = 300f;
				TurnSpeed = 10f;
				ProjectileSpeed = 650f;
				JumpHeight = 300f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = EnemyEV.BouncySpike_Advanced_Scale;
				ProjectileScale = EnemyEV.BouncySpike_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.BouncySpike_Advanced_Tint;
				MeleeRadius = 50;
				EngageRadius = 150;
				ProjectileRadius = 100;
				ProjectileDamage = Damage;
				KnockBack = new Vector2(1f, 2f);
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				Name = "Spiketus";
				MaxHealth = 5;
				Damage = 38;
				XPValue = 25;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 1;
				MoneyDropChance = 1f;
				Speed = 300f;
				TurnSpeed = 10f;
				ProjectileSpeed = 650f;
				JumpHeight = 300f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = EnemyEV.BouncySpike_Expert_Scale;
				ProjectileScale = EnemyEV.BouncySpike_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.BouncySpike_Expert_Tint;
				MeleeRadius = 50;
				ProjectileRadius = 100;
				EngageRadius = 150;
				ProjectileDamage = Damage;
				KnockBack = new Vector2(1f, 2f);
				return;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Spiker";
				MaxHealth = 5;
				Damage = 40;
				XPValue = 25;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 1;
				MoneyDropChance = 1f;
				Speed = 300f;
				TurnSpeed = 10f;
				ProjectileSpeed = 650f;
				JumpHeight = 300f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = false;
				IsWeighted = false;
				Scale = EnemyEV.BouncySpike_Miniboss_Scale;
				ProjectileScale = EnemyEV.BouncySpike_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.BouncySpike_Miniboss_Tint;
				MeleeRadius = 50;
				ProjectileRadius = 100;
				EngageRadius = 150;
				ProjectileDamage = Damage;
				KnockBack = new Vector2(1f, 2f);
				return;
			default:
				return;
			}
		}
		protected override void RunBasicLogic()
		{
			switch (State)
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
			switch (State)
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
			switch (State)
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
			switch (State)
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
			if (!IsPaused)
			{
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				Vector2 value = Vector2.Zero;
				Rectangle bounds = m_levelScreen.CurrentRoom.Bounds;
				if (Y < bounds.Top + 10)
				{
					value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Left, bounds.Top, bounds.Width, 10));
				}
				else if (Y > bounds.Bottom - 10)
				{
					value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Left, bounds.Bottom - 10, bounds.Width, 10));
				}
				if (X > bounds.Right - 10)
				{
					value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Right - 10, bounds.Top, 10, bounds.Height));
				}
				else if (X < bounds.Left + 10)
				{
					value = CollisionMath.CalculateMTD(Bounds, new Rectangle(bounds.Left, bounds.Top, 10, bounds.Height));
				}
				if (value != Vector2.Zero)
				{
					Vector2 heading = Heading;
					Vector2 vector = new Vector2(value.Y, value.X * -1f);
					Vector2 heading2 = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) * vector - heading;
					Heading = heading2;
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, "GiantSpike_Bounce_01", "GiantSpike_Bounce_02", "GiantSpike_Bounce_03");
					m_selfDestructCounter++;
					m_selfDestructTimer = 1f;
				}
				if (m_selfDestructTimer > 0f)
				{
					m_selfDestructTimer -= num;
					if (m_selfDestructTimer <= 0f)
					{
						m_selfDestructCounter = 0;
					}
				}
				if (m_selfDestructCounter >= m_selfDestructTotalBounces)
				{
					Kill(false);
				}
				if (CurrentSpeed == 0f)
				{
					CurrentSpeed = Speed;
				}
				if (HeadingX > 0f)
				{
					Rotation += RotationSpeed * num;
				}
				else
				{
					Rotation -= RotationSpeed * num;
				}
			}
			base.Update(gameTime);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			TerrainObj terrainObj = otherBox.Parent as TerrainObj;
			if (terrainObj != null && !(terrainObj is DoorObj) && terrainObj.CollidesBottom && terrainObj.CollidesLeft && terrainObj.CollidesRight && terrainObj.CollidesTop)
			{
				Vector2 value = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, (int)thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, (int)otherBox.AbsRotation, Vector2.Zero);
				if (value != Vector2.Zero)
				{
					Vector2 heading = Heading;
					Vector2 vector = new Vector2(value.Y, value.X * -1f);
					Vector2 heading2 = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) * vector - heading;
					X += value.X;
					Y += value.Y;
					Heading = heading2;
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, "GiantSpike_Bounce_01", "GiantSpike_Bounce_02", "GiantSpike_Bounce_03");
					m_selfDestructCounter++;
					m_selfDestructTimer = 1f;
				}
			}
		}
		public override void Reset()
		{
			if (SpawnRoom != null)
			{
				m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
				Dispose();
				return;
			}
			Orientation = m_internalOrientation;
			base.Reset();
		}
		public EnemyObj_BouncySpike(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyBouncySpike_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			Type = 3;
			NonKillable = true;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				SpawnRoom = null;
				base.Dispose();
			}
		}
	}
}
