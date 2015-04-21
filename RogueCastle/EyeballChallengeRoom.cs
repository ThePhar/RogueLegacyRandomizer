using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class EyeballChallengeRoom : ChallengeBossRoomObj
	{
		private EnemyObj_Eyeball m_boss;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled;
			}
		}
		public EyeballChallengeRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_Eyeball);
			this.m_boss.SaveToFile = false;
			base.Initialize();
		}
		private void SetRoomData()
		{
			this.m_boss.GetChildAt(0).TextureColor = Color.HotPink;
			this.m_boss.Level = 100;
			this.m_boss.MaxHealth = 17000;
			this.m_boss.Damage = 57;
			this.m_boss.IsNeo = true;
			this.m_boss.Name = "Neo Khidr";
			Game.PlayerStats.PlayerName = "Lady McSwordy";
			Game.PlayerStats.Class = 14;
			Game.PlayerStats.Spell = 10;
			Game.PlayerStats.IsFemale = true;
			Game.PlayerStats.BonusHealth = 39;
			Game.PlayerStats.BonusMana = 0;
			Game.PlayerStats.BonusStrength = 5;
			Game.PlayerStats.BonusMagic = 190;
			Game.PlayerStats.Traits = new Vector2(20f, 14f);
			Game.PlayerStats.SpecialItem = 8;
			Game.PlayerStats.GetEquippedArray[1] = 1;
			Game.PlayerStats.GetEquippedArray[2] = 1;
			Game.PlayerStats.GetEquippedArray[4] = 1;
			Game.PlayerStats.GetEquippedArray[3] = 1;
			Game.PlayerStats.GetEquippedArray[0] = 1;
			Game.PlayerStats.GetEquippedRuneArray[1] = 0;
			Game.PlayerStats.GetEquippedRuneArray[2] = 0;
			Game.PlayerStats.GetEquippedRuneArray[4] = 1;
			Game.PlayerStats.GetEquippedRuneArray[3] = 1;
			Game.PlayerStats.GetEquippedRuneArray[0] = 7;
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
			this.m_boss.ChangeSprite("EnemyEyeballBossEye_Character");
			this.m_boss.ChangeToBossPupil();
			this.m_boss.PlayAnimation(true);
			this.Player.AttachedLevel.Camera.X = (float)((int)((float)this.Bounds.Left + (float)this.Player.AttachedLevel.Camera.Width * 0.5f));
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Vector2 arg_BC_0 = this.Player.AttachedLevel.Camera.Position;
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
			this.m_bossChest.ForcedItemType = 15;
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
			if (!this.m_cutsceneRunning && !this.m_boss.BossVersionKilled && this.Player.CurrentHealth > 0 && !SoundManager.IsMusicPlaying)
			{
				SoundManager.PlayMusic("CastleBossSong", true, 0f);
			}
			base.Update(gameTime);
		}
		protected override void SaveCompletionData()
		{
			Game.PlayerStats.ChallengeEyeballBeaten = true;
			GameUtil.UnlockAchievement("FEAR_OF_BLINDNESS");
		}
		public override void OnExit()
		{
			this.Player.InvincibleToSpikes = false;
			base.OnExit();
		}
		protected override GameObj CreateCloneInstance()
		{
			return new EyeballChallengeRoom();
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
