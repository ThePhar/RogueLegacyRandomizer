using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EyeballBossRoom : BossRoomObj
	{
		private EnemyObj_Eyeball m_boss;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled;
			}
		}
		public EyeballBossRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_Eyeball);
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			this.m_boss.ChangeSprite("EnemyEyeballBossFire_Character");
			this.m_boss.ChangeToBossPupil();
			Vector2 scale = this.m_boss.Scale;
			this.m_boss.Scale = new Vector2(0.3f, 0.3f);
			this.Player.AttachedLevel.Camera.X = (float)((int)((float)this.Bounds.Left + (float)this.Player.AttachedLevel.Camera.Width * 0.5f));
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Vector2 arg_CA_0 = this.Player.AttachedLevel.Camera.Position;
			this.m_boss.AnimationDelay = 0.1f;
			this.Player.LockControls();
			this.Player.AttachedLevel.RunCinematicBorders(8f);
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				this.m_boss.Y.ToString()
			});
			Tween.RunFunction(1.1f, this.m_boss, "PlayAnimation", new object[]
			{
				true
			});
			Tween.To(this.m_boss, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"2.5",
				"AnimationDelay",
				0.0166666675f.ToString()
			});
			Tween.To(this.m_boss, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"1",
				"Rotation",
				"1800"
			});
			Tween.AddEndHandlerToLastTween(this.m_boss, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossEye_Character"
			});
			Tween.To(this.m_boss, 2f, new Easing(Bounce.EaseOut), new string[]
			{
				"delay",
				"2",
				"ScaleX",
				scale.X.ToString(),
				"ScaleY",
				scale.Y.ToString()
			});
			Tween.RunFunction(3.2f, this, "DisplayBossTitle", new object[]
			{
				"The Gatekeeper",
				this.m_boss.Name,
				"Intro2"
			});
			Tween.RunFunction(0.8f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Eyeball_Build"
			});
			base.OnEnter();
		}
		public void Intro2()
		{
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.5",
				"Y",
				((int)((float)this.Bounds.Bottom - (float)this.Player.AttachedLevel.Camera.Height * 0.5f)).ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "EndCutscene", new object[0]);
		}
		public void EndCutscene()
		{
			this.m_boss.Rotation = 0f;
			SoundManager.PlayMusic("CastleBossIntroSong", false, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			this.m_cutsceneRunning = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (!this.m_cutsceneRunning && !this.m_boss.BossVersionKilled && !SoundManager.IsMusicPlaying)
			{
				SoundManager.PlayMusic("CastleBossSong", true, 0f);
			}
			base.Update(gameTime);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new EyeballBossRoom();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_boss = null;
				base.Dispose();
			}
		}
	}
}
