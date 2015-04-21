using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class EnemyObj_SpikeTrap : EnemyObj
	{
		private LogicSet m_extractLS;
		private float ExtractDelay;
		private Rectangle DetectionRect;
		private Rectangle AbsDetectionRect
		{
			get
			{
				return new Rectangle((int)(base.X - (float)this.DetectionRect.Width / 2f), (int)(base.Y - (float)this.DetectionRect.Height), this.DetectionRect.Width, this.DetectionRect.Height);
			}
		}
		protected override void InitializeEV()
		{
			this.Scale = new Vector2(2f, 2f);
			base.AnimationDelay = 0.1f;
			base.Speed = 0f;
			this.MaxHealth = 10;
			this.EngageRadius = 2100;
			this.ProjectileRadius = 2200;
			this.MeleeRadius = 650;
			this.CooldownTime = 2f;
			base.KnockBack = new Vector2(1f, 2f);
			base.Damage = 25;
			base.JumpHeight = 20.5f;
			this.AlwaysFaceTarget = false;
			this.CanFallOffLedges = false;
			base.XPValue = 2;
			base.CanBeKnockedBack = false;
			base.LockFlip = true;
			base.IsWeighted = false;
			this.ExtractDelay = 0.1f;
			this.DetectionRect = new Rectangle(0, 0, 120, 30);
			base.Name = "Spike Trap";
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.BASIC:
			case GameTypes.EnemyDifficulty.ADVANCED:
			case GameTypes.EnemyDifficulty.EXPERT:
			case GameTypes.EnemyDifficulty.MINIBOSS:
				IL_F5:
				base.IsCollidable = false;
				return;
			}
			goto IL_F5;
		}
		protected override void InitializeLogic()
		{
			this.m_extractLS = new LogicSet(this);
			this.m_extractLS.AddAction(new PlayAnimationLogicAction(1, 2, false), Types.Sequence.Serial);
			this.m_extractLS.AddAction(new DelayLogicAction(this.ExtractDelay, false), Types.Sequence.Serial);
			this.m_extractLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, new string[]
			{
				"TrapSpike_01",
				"TrapSpike_02",
				"TrapSpike_03"
			}), Types.Sequence.Serial);
			this.m_extractLS.AddAction(new PlayAnimationLogicAction(2, 4, false), Types.Sequence.Serial);
			base.InitializeLogic();
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
			if (!base.IsPaused)
			{
				if (Game.PlayerStats.Traits.X != 23f && Game.PlayerStats.Traits.Y != 23f)
				{
					if (CollisionMath.Intersects(this.AbsDetectionRect, this.m_target.Bounds))
					{
						if (base.CurrentFrame == 1 || base.CurrentFrame == base.TotalFrames)
						{
							base.IsCollidable = true;
							this.m_extractLS.Execute();
						}
					}
					else if (base.CurrentFrame == 5 && !this.m_extractLS.IsActive)
					{
						base.IsCollidable = false;
						base.PlayAnimation("StartRetract", "RetractComplete", false);
					}
				}
				if (this.m_extractLS.IsActive)
				{
					this.m_extractLS.Update(gameTime);
				}
			}
			base.Update(gameTime);
		}
		public EnemyObj_SpikeTrap(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemySpikeTrap_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.Type = 21;
			base.StopAnimation();
			base.PlayAnimationOnRestart = false;
			base.NonKillable = true;
		}
		public override void Reset()
		{
			base.PlayAnimation(1, 1, false);
			base.Reset();
		}
		public override void ResetState()
		{
			base.PlayAnimation(1, 1, false);
			base.ResetState();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_extractLS.Dispose();
				this.m_extractLS = null;
				base.Dispose();
			}
		}
	}
}
