using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class LastBossChallengeRoom : ChallengeBossRoomObj
	{
		private EnemyObj_LastBoss m_boss;
		private EnemyObj_LastBoss m_boss2;
		public override bool BossKilled
		{
			get
			{
				return this.m_boss.IsKilled && this.m_boss2.IsKilled;
			}
		}
		public LastBossChallengeRoom()
		{
			this.m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			this.m_boss = (base.EnemyList[0] as EnemyObj_LastBoss);
			this.m_boss.SaveToFile = false;
			this.m_boss2 = (base.EnemyList[1] as EnemyObj_LastBoss);
			this.m_boss2.SaveToFile = false;
			base.Initialize();
		}
		private void SetRoomData()
		{
			this.m_boss.GetChildAt(1).TextureColor = Color.DarkRed;
			this.m_boss2.GetChildAt(1).TextureColor = Color.MediumPurple;
			this.m_boss.GetChildAt(4).TextureColor = Color.DarkRed;
			this.m_boss2.GetChildAt(4).TextureColor = Color.MediumPurple;
			this.m_boss.GetChildAt(12).TextureColor = Color.MediumPurple;
			this.m_boss2.GetChildAt(12).TextureColor = Color.DarkRed;
			this.m_boss.GetChildAt(7).TextureColor = Color.DarkRed;
			this.m_boss2.GetChildAt(7).TextureColor = Color.MediumPurple;
			this.m_boss.GetChildAt(2).TextureColor = Color.MediumPurple;
			this.m_boss2.GetChildAt(2).TextureColor = Color.DarkRed;
			this.m_boss.GetChildAt(6).TextureColor = Color.MediumPurple;
			this.m_boss2.GetChildAt(6).TextureColor = Color.DarkRed;
			this.m_boss.GetChildAt(9).TextureColor = Color.MediumPurple;
			this.m_boss.GetChildAt(3).TextureColor = Color.MediumPurple;
			this.m_boss2.GetChildAt(9).TextureColor = Color.DarkRed;
			this.m_boss2.GetChildAt(3).TextureColor = Color.DarkRed;
			this.m_boss.GetChildAt(10).TextureColor = Color.White;
			this.m_boss.GetChildAt(11).TextureColor = Color.DarkRed;
			this.m_boss2.GetChildAt(10).TextureColor = Color.White;
			this.m_boss2.GetChildAt(11).TextureColor = Color.DarkRed;
			this.m_boss.IsNeo = true;
			this.m_boss2.IsNeo = true;
			this.m_boss2.Flip = SpriteEffects.FlipHorizontally;
			this.m_boss.Flip = SpriteEffects.None;
			this.m_boss.Name = "The Brohannes";
			this.m_boss2.Name = this.m_boss.Name;
			this.m_boss.Level = 100;
			this.m_boss2.Level = this.m_boss.Level;
			this.m_boss.MaxHealth = 5000;
			this.m_boss2.MaxHealth = this.m_boss.MaxHealth;
			this.m_boss.Damage = 100;
			this.m_boss2.Damage = this.m_boss.Damage;
			this.m_boss.Speed = 345f;
			this.m_boss2.Speed = this.m_boss.Speed;
			Game.PlayerStats.PlayerName = "Johannes the Traitor";
			Game.PlayerStats.Class = 17;
			Game.PlayerStats.IsFemale = false;
			Game.PlayerStats.BonusHealth = 180;
			Game.PlayerStats.BonusMana = 66;
			Game.PlayerStats.BonusStrength = 125;
			Game.PlayerStats.BonusMagic = 150;
			Game.PlayerStats.BonusDefense = 0;
			Game.PlayerStats.Traits = new Vector2(16f, 0f);
			Game.PlayerStats.Spell = 14;
			Game.PlayerStats.GetEquippedRuneArray[1] = 0;
			Game.PlayerStats.GetEquippedRuneArray[2] = 0;
			Game.PlayerStats.GetEquippedRuneArray[4] = 0;
			Game.PlayerStats.GetEquippedRuneArray[3] = 1;
			Game.PlayerStats.GetEquippedRuneArray[0] = 1;
			if (this.m_boss != null)
			{
				this.m_boss.CurrentHealth = this.m_boss.MaxHealth;
				this.m_boss2.CurrentHealth = this.m_boss.MaxHealth;
			}
		}
		public override void OnEnter()
		{
			base.StorePlayerData();
			this.SetRoomData();
			this.m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			this.m_boss.PlayAnimation(true);
			this.m_boss2.PlayAnimation(true);
			this.Player.AttachedLevel.Camera.X = this.Player.X;
			this.Player.AttachedLevel.Camera.Y = this.Player.Y;
			Vector2 arg_8A_0 = this.Player.AttachedLevel.Camera.Position;
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
			this.m_bossChest.ForcedItemType = 19;
		}
		public override void OnExit()
		{
			if (!this.BossKilled)
			{
				foreach (EnemyObj current in base.EnemyList)
				{
					current.Reset();
				}
			}
			base.OnExit();
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
			SoundManager.PlayMusic("LastBossSong", false, 1f);
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.Player.UnlockControls();
			this.m_cutsceneRunning = false;
		}
		protected override void SaveCompletionData()
		{
			Game.PlayerStats.ChallengeLastBossBeaten = true;
			GameUtil.UnlockAchievement("FEAR_OF_RELATIVES");
		}
		protected override GameObj CreateCloneInstance()
		{
			return new LastBossChallengeRoom();
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
				this.m_boss2 = null;
				base.Dispose();
			}
		}
	}
}
