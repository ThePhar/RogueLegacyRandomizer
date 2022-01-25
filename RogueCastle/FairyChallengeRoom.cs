// 
//  Rogue Legacy Randomizer - FairyChallengeRoom.cs
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
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class FairyChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Fairy _boss;
        private Vector2        _startingCamPos;
        private bool           _teleportingOut;

        public FairyChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled => _boss.IsKilled;

        public override void Initialize()
        {
            _boss = EnemyList[0] as EnemyObj_Fairy;
            _boss.SaveToFile = false;
            _boss.Flip = SpriteEffects.FlipHorizontally;
            base.Initialize();
        }

        private void SetRoomData()
        {
            _boss.GetChildAt(0).TextureColor = Color.Yellow;
            _boss.Name = "Alexander the IV";
            _boss.MaxHealth = 1000;
            _boss.Damage = 35;
            _boss.Speed = 350f;
            _boss.IsNeo = true;

            if (_boss != null)
            {
                _boss.CurrentHealth = _boss.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            _teleportingOut = false;
            //Player.Flip = SpriteEffects.None;
            Player.Flip = SpriteEffects.None;
            SetRoomData();
            _cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            _boss.ChangeSprite("EnemyFairyGhostBossIdle_Character");
            _boss.PlayAnimation();
            Player.AttachedLevel.UpdateCamera();
            _startingCamPos = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", _boss.Y.ToString(), "X",
                _boss.X.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", "The Lost", _boss.Name,
                "Intro2");
            base.OnEnter();
            Player.GetChildAt(10).TextureColor = Color.White;
        }

        public void Intro2()
        {
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "0.5", "Y",
                _startingCamPos.Y.ToString(),
                "X", _startingCamPos.X.ToString());
            Tween.AddEndHandlerToLastTween(this, "EndCutscene");
        }

        public void EndCutscene()
        {
            _boss.Rotation = 0f;
            SoundManager.PlayMusic("GardenBossSong", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            _cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (_boss.IsKilled && !_teleportingOut)
            {
                Player.CurrentMana = Player.MaxMana;
            }

            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera)
        {
            if (_boss.IsKilled && Game.PlayerStats.Traits.X != 1f && Game.PlayerStats.Traits.Y != 1f)
            {
                camera.End();
                _boss.StopAnimation();
                Game.HSVEffect.Parameters["Saturation"].SetValue(0);
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null,
                    Game.HSVEffect, camera.GetTransformation());
                _boss.Visible = true;
                _boss.Draw(camera);
                _boss.Visible = false;
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
            _teleportingOut = true;
            base.OnExit();
        }

        protected override void SaveCompletionData()
        {
            Game.PlayerStats.FairyBossBeaten = true;
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
                _boss = null;
                base.Dispose();
            }
        }
    }
}
