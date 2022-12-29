using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
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
            Zone = Zone.Tower;
            base.Initialize();
        }

        private void SetRoomData()
        {
            _boss.GetChildAt(0).TextureColor = Color.MediumSpringGreen;
            _boss.Name = "Ponce de Freon";
            _boss.MaxHealth = 1000;
            _boss.Damage = 35;
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
