using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class FireballBossRoom : BossRoomObj
	{
		private EnemyObj_Fireball m_boss;
		private List<SpriteObj> m_fireList;
		private float m_bossStartingScale;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled;
			}
		}
		public override void Initialize()
		{
			foreach (EnemyObj current in base.EnemyList)
			{
				if (current is EnemyObj_Fireball)
				{
					this.m_boss = (current as EnemyObj_Fireball);
				}
				current.Visible = false;
				current.PauseEnemy(true);
			}
			this.m_boss.ChangeSprite("EnemyGhostBossIdle_Character");
			this.m_bossStartingScale = this.m_boss.ScaleX;
			this.m_fireList = new List<SpriteObj>();
			float num = 0f;
			float num2 = 24f;
			for (int i = 0; i < 15; i++)
			{
				SpriteObj spriteObj = new SpriteObj("GhostBossProjectile_Sprite");
				spriteObj.PlayAnimation(true);
				spriteObj.OutlineWidth = 2;
				spriteObj.Position = CDGMath.GetCirclePosition(num, 300f, this.m_boss.Position);
				spriteObj.Scale = new Vector2(2f, 2f);
				num += num2;
				spriteObj.Opacity = 0f;
				this.m_fireList.Add(spriteObj);
				base.GameObjList.Add(spriteObj);
			}
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			this.Player.LockControls();
			this.m_boss.Scale = Vector2.Zero;
			this.m_boss.Visible = true;
			this.m_boss.PlayAnimation(true);
			this.Player.AttachedLevel.UpdateCamera();
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_boss.X.ToString(),
				"Y",
				this.m_boss.Y.ToString()
			});
			Tween.RunFunction(1.5f, this, "Intro2", new object[0]);
			this.Player.AttachedLevel.RunCinematicBorders(10f);
			base.OnEnter();
		}
		public void Intro2()
		{
			float num = 0f;
			for (int i = 0; i < this.m_fireList.Count; i++)
			{
				Tween.RunFunction(num, this, "DisplayOrb", new object[]
				{
					i
				});
				num += 0.1f;
			}
			Tween.RunFunction(num + 0.5f, this, "Intro3", new object[0]);
		}
		public void DisplayOrb(int index)
		{
			Tween.To(this.m_fireList[index], 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"Opacity",
				"1"
			});
			SoundManager.PlaySound("Boss_Fireball_Whoosh_01");
		}
		public void Intro3()
		{
			SoundManager.PlaySound("Boss_Fireball_Spawn");
			float num = 0f;
			for (int i = 0; i < this.m_fireList.Count; i++)
			{
				Tween.RunFunction(num, this, "AbsorbOrb", new object[]
				{
					i
				});
				num += 0.1f;
			}
			Tween.RunFunction(num + 0.5f, this, "DisplayBossTitle", new object[]
			{
				"The Sentinel",
				this.m_boss.Name,
				"Intro4"
			});
		}
		public void AbsorbOrb(int index)
		{
			SoundManager.PlaySound("Boss_Fireball_Puff_01");
			Tween.To(this.m_fireList[index], 0.2f, new Easing(Quad.EaseIn), new string[]
			{
				"X",
				this.m_boss.X.ToString(),
				"Y",
				this.m_boss.Y.ToString()
			});
			Tween.To(this.m_fireList[index], 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.1",
				"Opacity",
				"0"
			});
			this.m_boss.ScaleX += this.m_bossStartingScale / 15f;
			this.m_boss.ScaleY += this.m_bossStartingScale / 15f;
		}
		public void Intro4()
		{
			this.m_boss.Visible = true;
			this.m_boss.PlayAnimation(true);
			Tween.To(this.Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.5",
				"X",
				(this.Player.X + GlobalEV.Camera_XOffset).ToString(),
				"Y",
				((float)this.Bounds.Bottom - ((float)this.Player.AttachedLevel.Camera.Bounds.Bottom - this.Player.AttachedLevel.Camera.Y)).ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "BeginFight", new object[0]);
		}
		public void BeginFight()
		{
			SoundManager.PlayMusic("TowerBossIntroSong", false, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			foreach (EnemyObj current in base.EnemyList)
			{
				if (current is EnemyObj_BouncySpike)
				{
					this.Player.AttachedLevel.ImpactEffectPool.DisplaySpawnEffect(current.Position);
				}
				current.UnpauseEnemy(true);
				current.Visible = true;
			}
			this.m_cutsceneRunning = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_boss.CurrentHealth <= 0 && base.ActiveEnemies > 1)
			{
				foreach (EnemyObj current in base.EnemyList)
				{
					if (current is EnemyObj_BouncySpike)
					{
						current.Kill(false);
					}
				}
			}
			if (!this.m_cutsceneRunning && !SoundManager.IsMusicPlaying && !this.m_boss.BossVersionKilled)
			{
				SoundManager.PlayMusic("TowerBossSong", true, 0f);
			}
			base.Update(gameTime);
		}
		public override void BossCleanup()
		{
			foreach (EnemyObj current in base.EnemyList)
			{
				if (current is EnemyObj_BouncySpike)
				{
					current.Kill(false);
				}
			}
			base.BossCleanup();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_boss = null;
				this.m_fireList.Clear();
				this.m_fireList = null;
				base.Dispose();
			}
		}
	}
}
