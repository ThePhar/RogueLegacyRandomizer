//  RogueLegacyRandomizer - EyeballBossRoom.cs
//  Last Modified 2023-10-26 11:30 AM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.GameObjects.Rooms;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
{
    public class EyeballBossRoom : BossRoomObj
    {
        private EnemyObj_Eyeball m_boss;

        public EyeballBossRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        protected override bool BossKilled
        {
            get { return m_boss.IsKilled; }
        }

        public override void Initialize()
        {
            m_boss = EnemyList[0] as EnemyObj_Eyeball;
            base.Initialize();
        }

        public override void OnEnter()
        {
            CutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            m_boss.ChangeSprite("EnemyEyeballBossFire_Character");
            m_boss.ChangeToBossPupil();
            var scale = m_boss.Scale;
            m_boss.Scale = new Vector2(0.3f, 0.3f);
            Player.AttachedLevel.Camera.X = (int) (Bounds.Left + Player.AttachedLevel.Camera.Width * 0.5f);
            Player.AttachedLevel.Camera.Y = Player.Y;
            var arg_CA_0 = Player.AttachedLevel.Camera.Position;
            m_boss.AnimationDelay = 0.1f;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(8f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", m_boss.Y.ToString());
            Tween.RunFunction(1.1f, m_boss, "PlayAnimation", true);
            Tween.To(m_boss, 0.5f, Linear.EaseNone, "delay", "2.5", "AnimationDelay", 0.0166666675f.ToString());
            Tween.To(m_boss, 3f, Quad.EaseInOut, "delay", "1", "Rotation", "1800");
            Tween.AddEndHandlerToLastTween(m_boss, "ChangeSprite", "EnemyEyeballBossEye_Character");
            Tween.To(m_boss, 2f, Bounce.EaseOut, "delay", "2", "ScaleX", scale.X.ToString(), "ScaleY",
                scale.Y.ToString());
            Tween.RunFunction(3.2f, this, "DisplayBossTitle", "The Gatekeeper", m_boss.Name, "Intro2");
            Tween.RunFunction(0.8f, typeof(SoundManager), "PlaySound", "Boss_Eyeball_Build");
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
            CutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!CutsceneRunning && !m_boss.BossVersionKilled && !SoundManager.IsMusicPlaying)
            {
                SoundManager.PlayMusic("CastleBossSong", true);
            }

            base.Update(gameTime);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new EyeballBossRoom();
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
