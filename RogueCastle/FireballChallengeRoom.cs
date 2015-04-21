using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class FireballChallengeRoom : ChallengeBossRoomObj
	{
		private EnemyObj_Fireball m_boss;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled;
			}
		}
		public FireballChallengeRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_Fireball);
			this.m_boss.SaveToFile = false;
			this.m_boss.IsNeo = true;
			base.Initialize();
		}
		private void SetRoomData()
		{
			this.m_boss.GetChildAt(0).TextureColor = Color.MediumSpringGreen;
			this.m_boss.Name = "Ponce de Freon";
			this.m_boss.Level = 100;
			this.m_boss.MaxHealth = 12000;
			this.m_boss.Damage = 380;
			this.m_boss.Speed = 430f;
			this.m_boss.IsNeo = true;
			Game.PlayerStats.PlayerName = "Sir Dovahkiin";
			Game.PlayerStats.Class = 10;
			Game.PlayerStats.Spell = 0;
			Game.PlayerStats.IsFemale = false;
			Game.PlayerStats.BonusHealth = 28;
			Game.PlayerStats.BonusMana = -14;
			Game.PlayerStats.BonusStrength = 150;
			Game.PlayerStats.BonusMagic = -10;
			Game.PlayerStats.BonusDefense = 230;
			Game.PlayerStats.Traits = new Vector2(26f, 15f);
			Game.PlayerStats.GetEquippedArray[0] = 4;
			Game.PlayerStats.GetEquippedArray[1] = 4;
			Game.PlayerStats.GetEquippedArray[4] = 7;
			Game.PlayerStats.GetEquippedArray[3] = 4;
			Game.PlayerStats.GetEquippedArray[2] = 4;
			Game.PlayerStats.GetEquippedRuneArray[1] = 0;
			Game.PlayerStats.GetEquippedRuneArray[2] = 0;
			Game.PlayerStats.GetEquippedRuneArray[4] = 0;
			Game.PlayerStats.GetEquippedRuneArray[3] = 0;
			Game.PlayerStats.GetEquippedRuneArray[0] = 1;
			if (this.m_boss != null)
			{
				this.m_boss.CurrentHealth = this.m_boss.MaxHealth;
			}
		}
		public override void OnEnter()
		{
			base.StorePlayerData();
			this.SetRoomData();
			this.m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			this.m_boss.ChangeSprite("EnemyGhostBossIdle_Character");
			this.m_boss.PlayAnimation(true);
			this.Player.AttachedLevel.Camera.X = this.Player.X;
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Vector2 arg_8E_0 = this.Player.AttachedLevel.Camera.Position;
			this.Player.LockControls();
			this.Player.AttachedLevel.RunCinematicBorders(6f);
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				this.m_boss.Y.ToString()
			});
			Tween.RunFunction(1.2f, this, "DisplayBossTitle", new object[]
			{
				Game.PlayerStats.PlayerName + " VS",
				this.m_boss.Name,
				"Intro2"
			});
			base.OnEnter();
			this.m_bossChest.ForcedItemType = 17;
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
			SoundManager.PlayMusic("TowerBossIntroSong", false, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
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
			if (!this.m_cutsceneRunning && !SoundManager.IsMusicPlaying && !this.m_boss.BossVersionKilled && this.Player.CurrentHealth > 0)
			{
				SoundManager.PlayMusic("TowerBossSong", true, 0f);
			}
			base.Update(gameTime);
		}
		protected override void SaveCompletionData()
		{
			Game.PlayerStats.ChallengeFireballBeaten = true;
			GameUtil.UnlockAchievement("FEAR_OF_CHEMICALS");
		}
		protected override GameObj CreateCloneInstance()
		{
			return new FireballChallengeRoom();
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
