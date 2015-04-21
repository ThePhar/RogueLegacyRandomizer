using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class FairyBossRoom : BossRoomObj
	{
		private EnemyObj_Fairy m_boss;
		private ObjContainer m_bossShadow;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled;
			}
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_Fairy);
			this.m_boss.PauseEnemy(true);
			this.m_bossShadow = new ObjContainer("EnemyFairyGhostBossMove_Character");
			this.m_boss.ChangeSprite("EnemyFairyGhostBossIdle_Character");
			this.m_bossShadow.TextureColor = Color.Black;
			this.m_bossShadow.Scale = this.m_boss.Scale;
			this.m_bossShadow.PlayAnimation(true);
			base.GameObjList.Add(this.m_bossShadow);
			base.Initialize();
		}
		public override void OnEnter()
		{
			SoundManager.StopMusic(0.5f);
			this.Player.LockControls();
			this.m_boss.Opacity = 0f;
			this.Player.AttachedLevel.UpdateCamera();
			this.m_bossShadow.Position = new Vector2((float)(this.Bounds.Left + 100), (float)(this.Bounds.Top + 400));
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.Player.AttachedLevel.RunCinematicBorders(11f);
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(this.Bounds.Left + 900).ToString(),
				"Y",
				(this.Bounds.Top + 400).ToString()
			});
			Tween.By(this.m_bossShadow, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"X",
				"3000"
			});
			Tween.RunFunction(1.8f, this, "Intro2", new object[0]);
			Tween.RunFunction(0.5f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Flameskull_Whoosh_01"
			});
			base.OnEnter();
		}
		public void Intro2()
		{
			this.m_bossShadow.Position = new Vector2((float)(this.Bounds.Right - 100), (float)(this.Bounds.Bottom - 400));
			this.m_bossShadow.Flip = SpriteEffects.FlipHorizontally;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				(this.Bounds.Right - 900).ToString(),
				"Y",
				(this.Bounds.Bottom - 400).ToString()
			});
			Tween.By(this.m_bossShadow, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1",
				"X",
				"-3000"
			});
			Tween.RunFunction(1.8f, this, "Intro3", new object[0]);
			Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Flameskull_Whoosh_02"
			});
		}
		public void Intro3()
		{
			this.m_bossShadow.Position = this.m_boss.Position;
			this.m_bossShadow.X -= 1500f;
			this.m_bossShadow.Flip = SpriteEffects.None;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_boss.X.ToString(),
				"Y",
				this.m_boss.Y.ToString()
			});
			Tween.By(this.m_bossShadow, 1f, new Easing(Quad.EaseOut), new string[]
			{
				"delay",
				"1",
				"X",
				"1500"
			});
			Tween.RunFunction(1.8f, this, "Intro4", new object[0]);
			Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Boss_Flameskull_Spawn"
			});
		}
		public void Intro4()
		{
			this.m_boss.PlayAnimation(true);
			this.m_bossShadow.ChangeSprite("EnemyFairyGhostBossIdle_Character");
			this.m_bossShadow.PlayAnimation(true);
			Tween.To(this.m_boss, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"1"
			});
			Tween.To(this.m_bossShadow, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.5",
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(this, "DisplayBossTitle", new object[]
			{
				"The Forgotten",
				this.m_boss.Name,
				"Intro5"
			});
		}
		public void Intro5()
		{
			Tween.To(this.Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.3",
				"X",
				(this.Player.X + GlobalEV.Camera_XOffset).ToString(),
				"Y",
				((float)this.Bounds.Bottom - ((float)this.Player.AttachedLevel.Camera.Bounds.Bottom - this.Player.AttachedLevel.Camera.Y)).ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "BeginFight", new object[0]);
		}
		public void BeginFight()
		{
			SoundManager.PlayMusic("GardenBossSong", true, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			this.m_boss.UnpauseEnemy(true);
		}
		public override void Draw(Camera2D camera)
		{
			if (this.m_boss.IsKilled && Game.PlayerStats.Traits.X != 1f && Game.PlayerStats.Traits.Y != 1f)
			{
				camera.End();
				this.m_boss.StopAnimation();
				Game.HSVEffect.Parameters["Saturation"].SetValue(0);
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, Game.HSVEffect, camera.GetTransformation());
				this.m_boss.Visible = true;
				this.m_boss.Draw(camera);
				this.m_boss.Visible = false;
				camera.End();
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			}
			base.Draw(camera);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_boss = null;
				this.m_bossShadow = null;
				base.Dispose();
			}
		}
	}
}
