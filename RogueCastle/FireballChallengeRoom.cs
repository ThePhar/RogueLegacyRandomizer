/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;
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
				return m_boss.IsKilled;
			}
		}
		public FireballChallengeRoom()
		{
			m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			m_boss = (EnemyList[0] as EnemyObj_Fireball);
			m_boss.SaveToFile = false;
			m_boss.IsNeo = true;
			base.Initialize();
		}
		private void SetRoomData()
		{
			m_boss.GetChildAt(0).TextureColor = Color.MediumSpringGreen;
			m_boss.Name = "Ponce de Freon";
			m_boss.Level = 100;
			m_boss.MaxHealth = 12000;
			m_boss.Damage = 380;
			m_boss.Speed = 430f;
			m_boss.IsNeo = true;
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
			if (m_boss != null)
			{
				m_boss.CurrentHealth = m_boss.MaxHealth;
			}
		}
		public override void OnEnter()
		{
			StorePlayerData();
			SetRoomData();
			m_cutsceneRunning = true;
			SoundManager.StopMusic(0.5f);
			m_boss.ChangeSprite("EnemyGhostBossIdle_Character");
			m_boss.PlayAnimation();
			Player.AttachedLevel.Camera.X = Player.X;
			Player.AttachedLevel.Camera.Y = Player.Y;
			Vector2 arg_8E_0 = Player.AttachedLevel.Camera.Position;
			Player.LockControls();
			Player.AttachedLevel.RunCinematicBorders(6f);
			Player.AttachedLevel.CameraLockedToPlayer = false;
			Player.AttachedLevel.Camera.Y = Player.Y;
			Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", m_boss.Y.ToString());
			Tween.RunFunction(1.2f, this, "DisplayBossTitle", Game.PlayerStats.PlayerName + " VS", m_boss.Name, "Intro2");
			base.OnEnter();
			m_bossChest.ForcedItemType = 17;
		}
		public void Intro2()
		{
			Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "0.5", "Y", ((int)(Bounds.Bottom - Player.AttachedLevel.Camera.Height * 0.5f)).ToString());
			Tween.AddEndHandlerToLastTween(this, "EndCutscene");
		}
		public void EndCutscene()
		{
			m_boss.Rotation = 0f;
			SoundManager.PlayMusic("TowerBossIntroSong", false, 1f);
			Player.AttachedLevel.CameraLockedToPlayer = true;
			Player.UnlockControls();
			m_cutsceneRunning = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (m_boss.CurrentHealth <= 0 && ActiveEnemies > 1)
			{
				foreach (EnemyObj current in EnemyList)
				{
					if (current is EnemyObj_BouncySpike)
					{
						current.Kill(false);
					}
				}
			}
			if (!m_cutsceneRunning && !SoundManager.IsMusicPlaying && !m_boss.BossVersionKilled && Player.CurrentHealth > 0)
			{
				SoundManager.PlayMusic("TowerBossSong", true);
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
			if (!IsDisposed)
			{
				m_boss = null;
				base.Dispose();
			}
		}
	}
}
