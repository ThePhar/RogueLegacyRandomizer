using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public abstract class CharacterObj : PhysicsObjContainer, IStateObj, IKillableObj
	{
		protected float CurrentAirSpeed;
		public float SlopeClimbRotation = 45f;
		protected int StepUp;
		protected bool m_isTouchingGround;
		protected bool m_isKilled;
		protected ProceduralLevelScreen m_levelScreen;
		public SpriteEffects InternalFlip;
		protected bool m_internalLockFlip;
		protected bool m_internalIsWeighted = true;
		protected float m_internalRotation;
		protected float m_internalAnimationDelay = 0.1f;
		protected Vector2 m_internalScale = new Vector2(1f, 1f);
		private Color m_blinkColour = Color.White;
		protected float m_blinkTimer;
		private int m_currentHealth;
		public float JumpHeight
		{
			get;
			set;
		}
		public virtual int MaxHealth
		{
			get;
			internal set;
		}
		public Vector2 KnockBack
		{
			get;
			internal set;
		}
		public float DoubleJumpHeight
		{
			get;
			internal set;
		}
		public bool CanBeKnockedBack
		{
			get;
			set;
		}
		public int State
		{
			get;
			set;
		}
		public int CurrentHealth
		{
			get
			{
				return this.m_currentHealth;
			}
			set
			{
				this.m_currentHealth = value;
				if (this.m_currentHealth > this.MaxHealth)
				{
					this.m_currentHealth = this.MaxHealth;
				}
				if (this.m_currentHealth < 0)
				{
					this.m_currentHealth = 0;
				}
			}
		}
		public bool IsKilled
		{
			get
			{
				return this.m_isKilled;
			}
		}
		public bool IsTouchingGround
		{
			get
			{
				return this.m_isTouchingGround;
			}
		}
		public Vector2 InternalScale
		{
			get
			{
				return this.m_internalScale;
			}
		}
		public CharacterObj(string spriteName, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo) : base(spriteName, physicsManager)
		{
			this.m_levelScreen = levelToAttachTo;
			this.CanBeKnockedBack = true;
		}
		protected abstract void InitializeEV();
		protected abstract void InitializeLogic();
		public virtual void HandleInput()
		{
		}
		public virtual void Update(GameTime gameTime)
		{
			if (this.m_blinkTimer > 0f)
			{
				this.m_blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				base.TextureColor = this.m_blinkColour;
				return;
			}
			if (base.TextureColor == this.m_blinkColour)
			{
				base.TextureColor = Color.White;
			}
		}
		public void Blink(Color blinkColour, float duration)
		{
			this.m_blinkColour = blinkColour;
			this.m_blinkTimer = duration;
		}
		public virtual void Kill(bool giveXP = true)
		{
			base.AccelerationX = 0f;
			base.AccelerationY = 0f;
			base.Opacity = 1f;
			base.CurrentSpeed = 0f;
			base.StopAnimation();
			base.Visible = false;
			this.m_isKilled = true;
			base.IsCollidable = false;
			base.IsWeighted = false;
			this.m_blinkTimer = 0f;
		}
		public virtual void Reset()
		{
			base.AccelerationX = 0f;
			base.AccelerationY = 0f;
			base.CurrentSpeed = 0f;
			this.CurrentHealth = this.MaxHealth;
			base.Opacity = 1f;
			base.IsCollidable = true;
			base.IsWeighted = this.m_internalIsWeighted;
			base.LockFlip = this.m_internalLockFlip;
			base.Rotation = this.m_internalRotation;
			base.AnimationDelay = this.m_internalAnimationDelay;
			this.Scale = this.m_internalScale;
			this.Flip = this.InternalFlip;
			this.m_isKilled = false;
			base.Visible = true;
			base.IsTriggered = false;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_levelScreen = null;
				base.Dispose();
			}
		}
	}
}
