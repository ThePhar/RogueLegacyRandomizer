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
	public class EyeballChallengeRoom : ChallengeBossRoomObj
	{
		private EnemyObj_Eyeball m_boss;
		public override bool BossKilled
		{
			get
			{
				return m_boss.IsKilled;
			}
		}
		public EyeballChallengeRoom()
		{
			m_roomActivityDelay = 0.5f;
		}
		public override void Initialize()
		{
			m_boss = (EnemyList[0] as EnemyObj_Eyeball);
			m_boss.SaveToFile = false;
			base.Initialize();
		}
		private void SetRoomData()
		{
			m_boss.GetChildAt(0).TextureColor = Color.HotPink;
			m_boss.Level = 100;
			m_boss.MaxHealth = 17000;
			m_boss.Damage = 57;
			m_boss.IsNeo = true;
			m_boss.Name = "Neo Khidr";
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
			m_boss.ChangeSprite("EnemyEyeballBossEye_Character");
			m_boss.ChangeToBossPupil();
			m_boss.PlayAnimation(true);
			Player.AttachedLevel.Camera.X = (int)(Bounds.Left + Player.AttachedLevel.Camera.Width * 0.5f);
			Player.AttachedLevel.Camera.Y = Player.Y;
			Vector2 arg_BC_0 = Player.AttachedLevel.Camera.Position;
			Player.LockControls();
			Player.AttachedLevel.RunCinematicBorders(6f);
			Player.AttachedLevel.CameraLockedToPlayer = false;
			Player.AttachedLevel.Camera.Y = Player.Y;
			Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				m_boss.Y.ToString()
			});
			Tween.RunFunction(1.2f, this, "DisplayBossTitle", new object[]
			{
				Game.PlayerStats.PlayerName + " VS",
				m_boss.Name,
				"Intro2"
			});
			base.OnEnter();
			m_bossChest.ForcedItemType = 15;
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
			if (!m_cutsceneRunning && !m_boss.BossVersionKilled && Player.CurrentHealth > 0 && !SoundManager.IsMusicPlaying)
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
			Player.InvincibleToSpikes = false;
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
			if (!IsDisposed)
			{
				m_boss = null;
				base.Dispose();
			}
		}
	}
}
