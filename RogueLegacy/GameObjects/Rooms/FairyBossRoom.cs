//  RogueLegacyRandomizer - FairyBossRoom.cs
//  Last Modified 2023-10-26 11:30 AM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.GameObjects.Rooms;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
{
    public class FairyBossRoom : BossRoomObj
    {
        private EnemyObj_Fairy m_boss;
        private ObjContainer m_bossShadow;

        protected override bool BossKilled
        {
            get { return m_boss.IsKilled; }
        }

        public override void Initialize()
        {
            m_boss = EnemyList[0] as EnemyObj_Fairy;
            m_boss.PauseEnemy(true);
            m_bossShadow = new ObjContainer("EnemyFairyGhostBossMove_Character");
            m_boss.ChangeSprite("EnemyFairyGhostBossIdle_Character");
            m_bossShadow.TextureColor = Color.Black;
            m_bossShadow.Scale = m_boss.Scale;
            m_bossShadow.PlayAnimation();
            GameObjList.Add(m_bossShadow);
            base.Initialize();
        }

        public override void OnEnter()
        {
            SoundManager.StopMusic(0.5f);
            Player.LockControls();
            m_boss.Opacity = 0f;
            Player.AttachedLevel.UpdateCamera();
            m_bossShadow.Position = new Vector2(Bounds.Left + 100, Bounds.Top + 400);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.RunCinematicBorders(11f);
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", (Bounds.Left + 900).ToString(), "Y",
                (Bounds.Top + 400).ToString());
            Tween.By(m_bossShadow, 0.5f, Tween.EaseNone, "delay", "1", "X", "3000");
            Tween.RunFunction(1.8f, this, "Intro2");
            Tween.RunFunction(0.5f, typeof(SoundManager), "PlaySound", "Boss_Flameskull_Whoosh_01");
            base.OnEnter();
        }

        public void Intro2()
        {
            m_bossShadow.Position = new Vector2(Bounds.Right - 100, Bounds.Bottom - 400);
            m_bossShadow.Flip = SpriteEffects.FlipHorizontally;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", (Bounds.Right - 900).ToString(), "Y",
                (Bounds.Bottom - 400).ToString());
            Tween.By(m_bossShadow, 0.5f, Tween.EaseNone, "delay", "1", "X", "-3000");
            Tween.RunFunction(1.8f, this, "Intro3");
            Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", "Boss_Flameskull_Whoosh_02");
        }

        public void Intro3()
        {
            m_bossShadow.Position = m_boss.Position;
            m_bossShadow.X -= 1500f;
            m_bossShadow.Flip = SpriteEffects.None;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", m_boss.X.ToString(), "Y",
                m_boss.Y.ToString());
            Tween.By(m_bossShadow, 1f, Quad.EaseOut, "delay", "1", "X", "1500");
            Tween.RunFunction(1.8f, this, "Intro4");
            Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", "Boss_Flameskull_Spawn");
        }

        public void Intro4()
        {
            m_boss.PlayAnimation();
            m_bossShadow.ChangeSprite("EnemyFairyGhostBossIdle_Character");
            m_bossShadow.PlayAnimation();
            Tween.To(m_boss, 0.5f, Tween.EaseNone, "delay", "0.5", "Opacity", "1");
            Tween.To(m_bossShadow, 0.5f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(this, "DisplayBossTitle", "The Forgotten", m_boss.Name, "Intro5");
        }

        public void Intro5()
        {
            Tween.To(Player.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, "delay", "0.3", "X",
                (Player.X + GlobalEV.Camera_XOffset).ToString(), "Y",
                (Bounds.Bottom - (Player.AttachedLevel.Camera.Bounds.Bottom - Player.AttachedLevel.Camera.Y))
                .ToString());
            Tween.AddEndHandlerToLastTween(this, "BeginFight");
        }

        public void BeginFight()
        {
            SoundManager.PlayMusic("GardenBossSong", true, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            m_boss.UnpauseEnemy(true);
        }

        public override void Draw(Camera2D camera)
        {
            if (m_boss.IsKilled && Game.PlayerStats.Traits.X != 1f && Game.PlayerStats.Traits.Y != 1f)
            {
                camera.End();
                m_boss.StopAnimation();
                Game.HSVEffect.Parameters["Saturation"].SetValue(0);
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null,
                    Game.HSVEffect, camera.GetTransformation());
                m_boss.Visible = true;
                m_boss.Draw(camera);
                m_boss.Visible = false;
                camera.End();
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                    camera.GetTransformation());
            }

            base.Draw(camera);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_boss = null;
                m_bossShadow = null;
                base.Dispose();
            }
        }
    }
}
