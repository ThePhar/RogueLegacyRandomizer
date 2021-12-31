/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class LastBossChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_LastBoss m_boss;
        private EnemyObj_LastBoss m_boss2;

        public LastBossChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled
        {
            get { return m_boss.IsKilled && m_boss2.IsKilled; }
        }

        public override void Initialize()
        {
            m_boss = EnemyList[0] as EnemyObj_LastBoss;
            m_boss.SaveToFile = false;
            m_boss2 = EnemyList[1] as EnemyObj_LastBoss;
            m_boss2.SaveToFile = false;
            base.Initialize();
        }

        private void SetRoomData()
        {
            m_boss.GetChildAt(1).TextureColor = Color.DarkRed;
            m_boss2.GetChildAt(1).TextureColor = Color.MediumPurple;
            m_boss.GetChildAt(4).TextureColor = Color.DarkRed;
            m_boss2.GetChildAt(4).TextureColor = Color.MediumPurple;
            m_boss.GetChildAt(12).TextureColor = Color.MediumPurple;
            m_boss2.GetChildAt(12).TextureColor = Color.DarkRed;
            m_boss.GetChildAt(7).TextureColor = Color.DarkRed;
            m_boss2.GetChildAt(7).TextureColor = Color.MediumPurple;
            m_boss.GetChildAt(2).TextureColor = Color.MediumPurple;
            m_boss2.GetChildAt(2).TextureColor = Color.DarkRed;
            m_boss.GetChildAt(6).TextureColor = Color.MediumPurple;
            m_boss2.GetChildAt(6).TextureColor = Color.DarkRed;
            m_boss.GetChildAt(9).TextureColor = Color.MediumPurple;
            m_boss.GetChildAt(3).TextureColor = Color.MediumPurple;
            m_boss2.GetChildAt(9).TextureColor = Color.DarkRed;
            m_boss2.GetChildAt(3).TextureColor = Color.DarkRed;
            m_boss.GetChildAt(10).TextureColor = Color.White;
            m_boss.GetChildAt(11).TextureColor = Color.DarkRed;
            m_boss2.GetChildAt(10).TextureColor = Color.White;
            m_boss2.GetChildAt(11).TextureColor = Color.DarkRed;
            m_boss.IsNeo = true;
            m_boss2.IsNeo = true;
            m_boss2.Flip = SpriteEffects.FlipHorizontally;
            m_boss.Flip = SpriteEffects.None;
            m_boss.Name = "The Brohannes";
            m_boss2.Name = m_boss.Name;
            m_boss.Level = 100;
            m_boss2.Level = m_boss.Level;
            m_boss.MaxHealth = 5000;
            m_boss2.MaxHealth = m_boss.MaxHealth;
            m_boss.Damage = 100;
            m_boss2.Damage = m_boss.Damage;
            m_boss.Speed = 345f;
            m_boss2.Speed = m_boss.Speed;
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
            if (m_boss != null)
            {
                m_boss.CurrentHealth = m_boss.MaxHealth;
                m_boss2.CurrentHealth = m_boss.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            StorePlayerData();
            SetRoomData();
            m_cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            m_boss.PlayAnimation();
            m_boss2.PlayAnimation();
            Player.AttachedLevel.Camera.X = Player.X;
            Player.AttachedLevel.Camera.Y = Player.Y;
            var arg_8A_0 = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", m_boss.Y.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", Game.PlayerStats.PlayerName + " VS", m_boss.Name,
                "Intro2");
            base.OnEnter();
            m_bossChest.ForcedItemType = 19;
        }

        public override void OnExit()
        {
            if (!BossKilled)
            {
                foreach (var current in EnemyList) current.Reset();
            }

            base.OnExit();
        }

        public void Intro2()
        {
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "0.5", "Y",
                ((int) (Bounds.Bottom - Player.AttachedLevel.Camera.Height * 0.5f)).ToString());
            Tween.AddEndHandlerToLastTween(this, "EndCutscene");
        }

        public void EndCutscene()
        {
            m_boss.Rotation = 0f;
            SoundManager.PlayMusic("LastBossSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            m_cutsceneRunning = false;
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
            if (!IsDisposed)
            {
                m_boss = null;
                m_boss2 = null;
                base.Dispose();
            }
        }
    }
}