//
//  Rogue Legacy Randomizer - FireballBossRoom.cs
//  Last Modified 2022-01-23
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class FireballBossRoom : BossRoomObj
    {
        private EnemyObj_Fireball m_boss;
        private float m_bossStartingScale;
        private List<SpriteObj> m_fireList;

        public override bool BossKilled
        {
            get { return m_boss.IsKilled; }
        }

        public override void Initialize()
        {
            foreach (var current in EnemyList)
            {
                if (current is EnemyObj_Fireball)
                {
                    m_boss = current as EnemyObj_Fireball;
                }

                current.Visible = false;
                current.PauseEnemy(true);
            }

            m_boss.ChangeSprite("EnemyGhostBossIdle_Character");
            m_bossStartingScale = m_boss.ScaleX;
            m_fireList = new List<SpriteObj>();
            var num = 0f;
            var num2 = 24f;
            for (var i = 0; i < 15; i++)
            {
                var spriteObj = new SpriteObj("GhostBossProjectile_Sprite");
                spriteObj.PlayAnimation();
                spriteObj.OutlineWidth = 2;
                spriteObj.Position = CDGMath.GetCirclePosition(num, 300f, m_boss.Position);
                spriteObj.Scale = new Vector2(2f, 2f);
                num += num2;
                spriteObj.Opacity = 0f;
                m_fireList.Add(spriteObj);
                GameObjList.Add(spriteObj);
            }

            base.Initialize();
        }

        public override void OnEnter()
        {
            _cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            Player.LockControls();
            m_boss.Scale = Vector2.Zero;
            m_boss.Visible = true;
            m_boss.PlayAnimation();
            Player.AttachedLevel.UpdateCamera();
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", m_boss.X.ToString(), "Y",
                m_boss.Y.ToString());
            Tween.RunFunction(1.5f, this, "Intro2");
            Player.AttachedLevel.RunCinematicBorders(10f);
            base.OnEnter();
        }

        public void Intro2()
        {
            var num = 0f;
            for (var i = 0; i < m_fireList.Count; i++)
            {
                Tween.RunFunction(num, this, "DisplayOrb", i);
                num += 0.1f;
            }

            Tween.RunFunction(num + 0.5f, this, "Intro3");
        }

        public void DisplayOrb(int index)
        {
            Tween.To(m_fireList[index], 0.2f, Quad.EaseOut, "Opacity", "1");
            SoundManager.PlaySound("Boss_Fireball_Whoosh_01");
        }

        public void Intro3()
        {
            SoundManager.PlaySound("Boss_Fireball_Spawn");
            var num = 0f;
            for (var i = 0; i < m_fireList.Count; i++)
            {
                Tween.RunFunction(num, this, "AbsorbOrb", i);
                num += 0.1f;
            }

            Tween.RunFunction(num + 0.5f, this, "DisplayBossTitle", "The Sentinel", m_boss.Name, "Intro4");
        }

        public void AbsorbOrb(int index)
        {
            SoundManager.PlaySound("Boss_Fireball_Puff_01");
            Tween.To(m_fireList[index], 0.2f, Quad.EaseIn, "X", m_boss.X.ToString(), "Y", m_boss.Y.ToString());
            Tween.To(m_fireList[index], 0.1f, Tween.EaseNone, "delay", "0.1", "Opacity", "0");
            m_boss.ScaleX += m_bossStartingScale / 15f;
            m_boss.ScaleY += m_bossStartingScale / 15f;
        }

        public void Intro4()
        {
            m_boss.Visible = true;
            m_boss.PlayAnimation();
            Tween.To(Player.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, "delay", "0.5", "X",
                (Player.X + GlobalEV.Camera_XOffset).ToString(), "Y",
                (Bounds.Bottom - (Player.AttachedLevel.Camera.Bounds.Bottom - Player.AttachedLevel.Camera.Y))
                .ToString());
            Tween.AddEndHandlerToLastTween(this, "BeginFight");
        }

        public void BeginFight()
        {
            SoundManager.PlayMusic("TowerBossIntroSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            foreach (var current in EnemyList)
            {
                if (current is EnemyObj_BouncySpike)
                {
                    Player.AttachedLevel.ImpactEffectPool.DisplaySpawnEffect(current.Position);
                }

                current.UnpauseEnemy(true);
                current.Visible = true;
            }

            _cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_boss.CurrentHealth <= 0 && ActiveEnemies > 1)
            {
                foreach (var current in EnemyList)
                    if (current is EnemyObj_BouncySpike)
                    {
                        current.Kill(false);
                    }
            }

            if (!_cutsceneRunning && !SoundManager.IsMusicPlaying && !m_boss.BossVersionKilled)
            {
                SoundManager.PlayMusic("TowerBossSong", true);
            }

            base.Update(gameTime);
        }

        public override void BossCleanup()
        {
            foreach (var current in EnemyList)
                if (current is EnemyObj_BouncySpike)
                {
                    current.Kill(false);
                }

            base.BossCleanup();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_boss = null;
                m_fireList.Clear();
                m_fireList = null;
                base.Dispose();
            }
        }
    }
}
