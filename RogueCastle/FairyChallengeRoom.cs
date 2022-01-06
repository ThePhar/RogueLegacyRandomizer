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
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class FairyChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Fairy m_boss;
        private Vector2 m_startingCamPos;
        private bool m_teleportingOut;

        public FairyChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled
        {
            get { return m_boss.IsKilled; }
        }

        public override void Initialize()
        {
            m_boss = EnemyList[0] as EnemyObj_Fairy;
            m_boss.SaveToFile = false;
            m_boss.Flip = SpriteEffects.FlipHorizontally;
            base.Initialize();
        }

        private void SetRoomData()
        {
            m_boss.GetChildAt(0).TextureColor = Color.Yellow;
            m_boss.Name = "Alexander the IV";
            m_boss.Level = 100;
            m_boss.MaxHealth = 15000;
            m_boss.Damage = 200;
            m_boss.Speed = 400f;
            m_boss.IsNeo = true;
            Game.PlayerStats.PlayerName = "Sir Wagner";
            Game.PlayerStats.Class = 12;
            Game.PlayerStats.Spell = 6;
            Game.PlayerStats.IsFemale = false;
            Game.PlayerStats.BonusHealth = 30;
            Game.PlayerStats.BonusMana = 10;
            Game.PlayerStats.BonusStrength = 150;
            Game.PlayerStats.BonusMagic = 40;
            Game.PlayerStats.BonusDefense = 230;
            Game.PlayerStats.Traits = new Vector2(13f, 15f);
            Game.PlayerStats.GetEquippedArray[1] = 14;
            Game.PlayerStats.GetEquippedArray[2] = 12;
            Game.PlayerStats.GetEquippedArray[4] = 7;
            Game.PlayerStats.GetEquippedArray[3] = 12;
            Game.PlayerStats.GetEquippedArray[0] = 14;
            Game.PlayerStats.GetEquippedRuneArray[2] = 0;
            Game.PlayerStats.GetEquippedRuneArray[4] = 0;
            if (m_boss != null)
            {
                m_boss.CurrentHealth = m_boss.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            m_teleportingOut = false;
            //Player.Flip = SpriteEffects.None;
            StorePlayerData();
            Player.Flip = SpriteEffects.None;
            SetRoomData();
            m_cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            m_boss.ChangeSprite("EnemyFairyGhostBossIdle_Character");
            m_boss.PlayAnimation();
            Player.AttachedLevel.UpdateCamera();
            m_startingCamPos = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", m_boss.Y.ToString(), "X",
                m_boss.X.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", Game.PlayerStats.PlayerName + " VS", m_boss.Name,
                "Intro2");
            base.OnEnter();
            Player.GetChildAt(10).TextureColor = Color.White;
            m_bossChest.ForcedItemType = ItemDrop.FountainPiece2;
        }

        public void Intro2()
        {
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "0.5", "Y",
                m_startingCamPos.Y.ToString(),
                "X", m_startingCamPos.X.ToString());
            Tween.AddEndHandlerToLastTween(this, "EndCutscene");
        }

        public void EndCutscene()
        {
            m_boss.Rotation = 0f;
            SoundManager.PlayMusic("GardenBossSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            m_cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_boss.IsKilled && !m_teleportingOut)
            {
                Player.CurrentMana = Player.MaxMana;
            }

            base.Update(gameTime);
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

        public override void OnExit()
        {
            foreach (var current in TempEnemyList)
            {
                current.KillSilently();
                current.Dispose();
            }

            TempEnemyList.Clear();
            Player.InvincibleToSpikes = false;
            m_teleportingOut = true;
            base.OnExit();
        }

        protected override void SaveCompletionData()
        {
            Game.PlayerStats.ChallengeSkullBeaten = true;
            GameUtil.UnlockAchievement("FEAR_OF_BONES");
        }

        protected override GameObj CreateCloneInstance()
        {
            return new FairyChallengeRoom();
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