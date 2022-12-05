// Rogue Legacy Randomizer - EyeballChallengeRoom.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
{
    public class EyeballChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Eyeball _boss;

        public EyeballChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled => _boss.IsKilled;

        public override void Initialize()
        {
            _boss = EnemyList[0] as EnemyObj_Eyeball;
            _boss.SaveToFile = false;
            base.Initialize();
        }

        private void SetRoomData()
        {
            _boss.GetChildAt(0).TextureColor = Color.HotPink;
            _boss.MaxHealth = 800;
            _boss.Damage = 25;
            _boss.IsNeo = true;
            _boss.Name = "Neo Khidr";

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
            _boss.ChangeSprite("EnemyEyeballBossEye_Character");
            _boss.ChangeToBossPupil();
            _boss.PlayAnimation();
            Player.AttachedLevel.Camera.X = (int) (Bounds.Left + Player.AttachedLevel.Camera.Width * 0.5f);
            Player.AttachedLevel.Camera.Y = Player.Y;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", $"{_boss.Y}");
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", "The Keymaster", _boss.Name, "Intro2");
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
            SoundManager.PlayMusic("CastleBossIntroSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            _cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_cutsceneRunning && !_boss.BossVersionKilled && Player.CurrentHealth > 0 && !SoundManager.IsMusicPlaying)
            {
                SoundManager.PlayMusic("CastleBossSong", true);
            }

            base.Update(gameTime);
        }

        protected override void SaveCompletionData()
        {
            Game.PlayerStats.EyeballBossBeaten = true;
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
