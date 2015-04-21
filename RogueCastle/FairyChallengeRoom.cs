using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class FairyChallengeRoom : ChallengeBossRoomObj
	{
		private EnemyObj_Fairy m_boss;
		private Vector2 m_startingCamPos;
		private bool m_teleportingOut;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled;
			}
		}
		public FairyChallengeRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_Fairy);
			this.m_boss.SaveToFile = false;
			this.m_boss.Flip = SpriteEffects.FlipHorizontally;
			base.Initialize();
		}
		private void SetRoomData()
		{
			this.m_boss.GetChildAt(0).TextureColor = Color.Yellow;
			this.m_boss.Name = "Alexander the IV";
			this.m_boss.Level = 100;
			this.m_boss.MaxHealth = 15000;
			this.m_boss.Damage = 200;
			this.m_boss.Speed = 400f;
			this.m_boss.IsNeo = true;
			Game.PlayerStats.PlayerName = "Sir Wagner";
			Game.PlayerStats.Class = 12;
			Game.PlayerStats.Spell = 6;
			Game.PlayerStats.IsFemale = false;
			Game.PlayerStats.BonusHealth = 30;
			Game.PlayerStats.BonusMana = 10;
			Game.PlayerStats.BonusStrength = 150;
			Game.PlayerStats.BonusMagic = 40;
			Game.PlayerStats.BonusDefense = 230;
			Game.PlayerStats.Traits = new Vector2(13f, 15f);
			Game.PlayerStats.GetEquippedArray[1] = 14;
			Game.PlayerStats.GetEquippedArray[2] = 12;
			Game.PlayerStats.GetEquippedArray[4] = 7;
			Game.PlayerStats.GetEquippedArray[3] = 12;
			Game.PlayerStats.GetEquippedArray[0] = 14;
			Game.PlayerStats.GetEquippedRuneArray[2] = 0;
			Game.PlayerStats.GetEquippedRuneArray[4] = 0;
			if (this.m_boss != null)
			{
				this.m_boss.CurrentHealth = this.m_boss.MaxHealth;
			}
		}
		public override void OnEnter()
		{
			this.m_teleportingOut = false;
			base.StorePlayerData();
			this.Player.Flip = SpriteEffects.None;
			this.SetRoomData();
			this.m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			this.m_boss.ChangeSprite("EnemyFairyGhostBossIdle_Character");
			this.m_boss.PlayAnimation(true);
			this.Player.AttachedLevel.UpdateCamera();
			this.m_startingCamPos = this.Player.AttachedLevel.Camera.Position;
			this.Player.LockControls();
			this.Player.AttachedLevel.RunCinematicBorders(6f);
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				this.m_boss.Y.ToString(),
				"X",
				this.m_boss.X.ToString()
			});
			Tween.RunFunction(1.2f, this, "DisplayBossTitle", new object[]
			{
				Game.PlayerStats.PlayerName + " VS",
				this.m_boss.Name,
				"Intro2"
			});
			base.OnEnter();
			this.Player.GetChildAt(10).TextureColor = Color.White;
			this.m_bossChest.ForcedItemType = 16;
		}
		public void Intro2()
		{
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"0.5",
				"Y",
				this.m_startingCamPos.Y.ToString(),
				"X",
				this.m_startingCamPos.X.ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "EndCutscene", new object[0]);
		}
		public void EndCutscene()
		{
			this.m_boss.Rotation = 0f;
			SoundManager.PlayMusic("GardenBossSong", false, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			this.m_cutsceneRunning = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_boss.IsKilled && !this.m_teleportingOut)
			{
				this.Player.CurrentMana = this.Player.MaxMana;
			}
			base.Update(gameTime);
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
		public override void OnExit()
		{
			foreach (EnemyObj current in base.TempEnemyList)
			{
				current.KillSilently();
				current.Dispose();
			}
			base.TempEnemyList.Clear();
			this.Player.InvincibleToSpikes = false;
			this.m_teleportingOut = true;
			base.OnExit();
		}
		protected override void SaveCompletionData()
		{
			Game.PlayerStats.ChallengeSkullBeaten = true;
			GameUtil.UnlockAchievement("FEAR_OF_BONES");
		}
		protected override GameObj CreateCloneInstance()
		{
			return new FairyChallengeRoom();
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
