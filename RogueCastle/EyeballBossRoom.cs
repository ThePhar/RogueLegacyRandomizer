/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;
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
				return m_boss.IsKilled;
			}
		}
		public EyeballBossRoom()
		{
			m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			m_boss = (EnemyList[0] as EnemyObj_Eyeball);
			base.Initialize();
		}
		public override void OnEnter()
		{
			m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			m_boss.ChangeSprite("EnemyEyeballBossFire_Character");
			m_boss.ChangeToBossPupil();
			Vector2 scale = m_boss.Scale;
			m_boss.Scale = new Vector2(0.3f, 0.3f);
			Player.AttachedLevel.Camera.X = (int)(Bounds.Left + Player.AttachedLevel.Camera.Width * 0.5f);
			Player.AttachedLevel.Camera.Y = Player.Y;
			Vector2 arg_CA_0 = Player.AttachedLevel.Camera.Position;
			m_boss.AnimationDelay = 0.1f;
			Player.LockControls();
			Player.AttachedLevel.RunCinematicBorders(8f);
			Player.AttachedLevel.CameraLockedToPlayer = false;
			Player.AttachedLevel.Camera.Y = Player.Y;
			Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				m_boss.Y.ToString()
			});
			Tween.RunFunction(1.1f, m_boss, "PlayAnimation", new object[]
			{
				true
			});
			Tween.To(m_boss, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"2.5",
				"AnimationDelay",
				0.0166666675f.ToString()
			});
			Tween.To(m_boss, 3f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"1",
				"Rotation",
				"1800"
			});
			Tween.AddEndHandlerToLastTween(m_boss, "ChangeSprite", new object[]
			{
				"EnemyEyeballBossEye_Character"
			});
			Tween.To(m_boss, 2f, new Easing(Bounce.EaseOut), new string[]
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
				m_boss.Name,
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
			Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.5",
				"Y",
				((int)(Bounds.Bottom - Player.AttachedLevel.Camera.Height * 0.5f)).ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "EndCutscene", new object[0]);
		}
		public void EndCutscene()
		{
			m_boss.Rotation = 0f;
			SoundManager.PlayMusic("CastleBossIntroSong", false, 1f);
			Player.AttachedLevel.CameraLockedToPlayer = true;
			Player.UnlockControls();
			m_cutsceneRunning = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (!m_cutsceneRunning && !m_boss.BossVersionKilled && !SoundManager.IsMusicPlaying)
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
			if (!IsDisposed)
			{
				m_boss = null;
				base.Dispose();
			}
		}
	}
}
