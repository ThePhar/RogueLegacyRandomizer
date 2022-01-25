// 
//  Rogue Legacy Randomizer - FireballChallengeRoom.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class FireballChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Fireball _boss;

        public FireballChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled => _boss.IsKilled;

        public override void Initialize()
        {
            _boss = EnemyList[0] as EnemyObj_Fireball;
            _boss.SaveToFile = false;
            _boss.IsNeo = true;
            base.Initialize();
        }

        private void SetRoomData()
        {
            _boss.GetChildAt(0).TextureColor = Color.MediumSpringGreen;
            _boss.Name = "Ponce de Freon";
            _boss.Level = 100;
            _boss.MaxHealth = 12000;
            _boss.Damage = 380;
            _boss.Speed = 430f;
            _boss.IsNeo = true;
            if (_boss != null)
            {
                _boss.CurrentHealth = _boss.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            SetRoomData();
            _cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            _boss.ChangeSprite("EnemyGhostBossIdle_Character");
            _boss.PlayAnimation();
            Player.AttachedLevel.Camera.X = Player.X;
            Player.AttachedLevel.Camera.Y = Player.Y;
            var arg_8E_0 = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", _boss.Y.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", "The Adversarial", _boss.Name,
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
            _boss.Rotation = 0f;
            SoundManager.PlayMusic("TowerBossIntroSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            _cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (_boss.CurrentHealth <= 0 && ActiveEnemies > 1)
            {
                foreach (var current in EnemyList)
                {
                    if (current is EnemyObj_BouncySpike)
                    {
                        current.Kill(false);
                    }
                }
            }

            if (!_cutsceneRunning && !SoundManager.IsMusicPlaying && !_boss.BossVersionKilled &&
                Player.CurrentHealth > 0)
            {
                SoundManager.PlayMusic("TowerBossSong", true);
            }

            base.Update(gameTime);
        }

        protected override void SaveCompletionData()
        {
            Game.PlayerStats.FireballBossBeaten = true;
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
            if (IsDisposed)
            {
                return;
            }

            _boss = null;
            base.Dispose();
        }
    }
}
