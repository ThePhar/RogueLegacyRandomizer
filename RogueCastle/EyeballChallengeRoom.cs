// 
//  Rogue Legacy Randomizer - EyeballChallengeRoom.cs
//  Last Modified 2022-01-23
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class EyeballChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Eyeball m_boss;

        public EyeballChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled
        {
            get { return m_boss.IsKilled; }
        }

        public override void Initialize()
        {
            m_boss = EnemyList[0] as EnemyObj_Eyeball;
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
            if (m_boss != null)
            {
                m_boss.CurrentHealth = m_boss.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            SetRoomData();
            m_cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            m_boss.ChangeSprite("EnemyEyeballBossEye_Character");
            m_boss.ChangeToBossPupil();
            m_boss.PlayAnimation();
            Player.AttachedLevel.Camera.X = (int) (Bounds.Left + Player.AttachedLevel.Camera.Width * 0.5f);
            Player.AttachedLevel.Camera.Y = Player.Y;
            var arg_BC_0 = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", m_boss.Y.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", "The Keymaster", m_boss.Name,
                "Intro2");
            base.OnEnter();
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
            SoundManager.PlayMusic("CastleBossIntroSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            m_cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_cutsceneRunning && !m_boss.BossVersionKilled && Player.CurrentHealth > 0 &&
                !SoundManager.IsMusicPlaying)
            {
                SoundManager.PlayMusic("CastleBossSong", true);
            }

            base.Update(gameTime);
        }

        protected override void SaveCompletionData()
        {
            Game.PlayerStats.EyeballBossBeaten = true;
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
