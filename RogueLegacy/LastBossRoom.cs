// Rogue Legacy Randomizer - LastBossRoom.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
{
    public class LastBossRoom : BossRoomObj
    {
        private readonly int m_bossCoins = 40;
        private readonly int m_bossDiamonds = 12;
        private readonly int m_bossMoneyBags = 16;
        private readonly float m_shakeDuration = 0.03f;
        private EnemyObj_LastBoss m_boss;
        private DoorObj m_bossDoor;
        private SpriteObj m_bossDoorSprite;
        private ObjContainer m_fountain;
        private float m_playerX;
        private bool m_shake;
        private float m_shakeTimer = 0.03f;
        private bool m_shookLeft;

        public LastBossRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public float BackBufferOpacity { get; set; }

        public override bool BossKilled
        {
            get { return m_boss.IsKilled && m_boss.IsSecondForm; }
        }

        public override void Initialize()
        {
            m_boss = EnemyList[0] as EnemyObj_LastBoss;
            foreach (var current in GameObjList)
            {
                if (current.Name == "fountain")
                {
                    m_fountain = current as ObjContainer;
                }

                if (current.Name == "stainglass")
                {
                    current.Opacity = 0.5f;
                }

                if (current.Name == "door")
                {
                    m_bossDoorSprite = current as SpriteObj;
                }
            }

            foreach (var current2 in DoorList)
                if (current2.Name == "FinalBossDoor")
                {
                    m_bossDoor = current2;
                    m_bossDoor.Locked = true;
                    break;
                }

            base.Initialize();
        }

        public override void OnEnter()
        {
            Player.AttachedLevel.RemoveCompassDoor();
            m_boss.Level += 8;
            m_boss.CurrentHealth = m_boss.MaxHealth;
            BackBufferOpacity = 0f;
            SoundManager.StopMusic(0.5f);
            StartCutscene();
            base.OnEnter();
        }

        public void StartCutscene()
        {
            _cutsceneRunning = true;
            Player.LockControls();
            Player.AccelerationY = 0f;
            Player.AttachedLevel.RunCinematicBorders(8f);
            Player.Flip = SpriteEffects.None;
            Player.State = 1;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", false));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", false));
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
            logicSet.AddAction(new PlayAnimationLogicAction());
            logicSet.AddAction(new DelayLogicAction(1.5f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", true));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", true));
            Player.RunExternalLogicSet(logicSet);
            Tween.RunFunction(1.6f, this, "Cutscene2");
        }

        public void Cutscene2()
        {
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Tween.By(Player.AttachedLevel.Camera, 1.5f, Quad.EaseInOut, "X", "300");
            Tween.AddEndHandlerToLastTween(this, "Cutscene3");
        }

        public void Cutscene3()
        {
            Tween.RunFunction(0.5f, this, "Cutscene4");
            var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
            if (Game.PlayerStats.Class == 17)
            {
                rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk01_Special");
                GameUtil.UnlockAchievement("LOVE_OF_LAUGHING_AT_OTHERS");
            }
            else
            {
                rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk01");
            }

            rCScreenManager.DisplayScreen(13, true);
        }

        public void Cutscene4()
        {
            Tween.RunFunction(0.5f, this, "DisplayBossTitle", "The Traitor", m_boss.Name, "Cutscene5");
        }

        public void Cutscene5()
        {
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", Player.X.ToString());
            Tween.AddEndHandlerToLastTween(this, "BeginBattle");
        }

        public void BeginBattle()
        {
            SoundManager.PlayMusic("TitleScreenSong", true, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            _cutsceneRunning = false;
        }

        public void RunFountainCutscene()
        {
            Player.AttachedLevel.CameraLockedToPlayer = false;
            var camera = Player.AttachedLevel.Camera;
            m_playerX = camera.X;
            SoundManager.PlaySound("Cutsc_CameraMove");
            Tween.To(camera, 1f, Quad.EaseInOut, "X", (m_fountain.Bounds.Center.X - 400).ToString());
            Tween.RunFunction(2f, this, "RunFountainCutscene2");
        }

        public void RunFountainCutscene2()
        {
            StartShake();
            SoundManager.PlaySound("Cutsc_StatueCrumble");
            m_fountain.ChangeSprite("FountainOfYouthShatter_Character");
            m_fountain.PlayAnimation(false);
            Player.AttachedLevel.ImpactEffectPool.DisplayFountainShatterSmoke(m_fountain);
            Tween.RunFunction(2f, this, "DisplaySecondBoss");
            Tween.RunFunction(2f, this, "RunFountainCutscene3");
        }

        public void DisplaySecondBoss()
        {
            m_boss.SecondFormComplete();
            m_boss.UpdateCollisionBoxes();
            m_boss.Position = new Vector2(m_fountain.X, m_fountain.Y - (m_boss.Bounds.Bottom - m_boss.Y));
        }

        public void RunFountainCutscene3()
        {
            SoundManager.PlaySound("FinalBoss_St2_BlockLaugh");
            SoundManager.PlayMusic("LastBossSong", true, 1f);
            m_fountain.Visible = false;
            StopShake();
            Tween.RunFunction(2f, this, "DisplayBossTitle", "Johannes", "The Fountain", "RunFountainCutscene4");
        }

        public void StartShake()
        {
            m_shake = true;
        }

        public void StopShake()
        {
            m_shake = false;
        }

        public void DisableFountain()
        {
            m_fountain.Visible = false;
        }

        public void RunFountainCutscene4()
        {
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", m_playerX.ToString());
            Tween.AddEndHandlerToLastTween(m_boss, "SecondFormActive");
        }

        public override void Update(GameTime gameTime)
        {
            if (m_shake && m_shakeTimer > 0f)
            {
                m_shakeTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_shakeTimer <= 0f)
                {
                    var camera = Player.AttachedLevel.Camera;
                    m_shakeTimer = m_shakeDuration;
                    if (m_shookLeft)
                    {
                        m_shookLeft = false;
                        camera.X += 5f;
                    }
                    else
                    {
                        camera.X -= 5f;
                        m_shookLeft = true;
                    }
                }
            }

            if (!_cutsceneRunning)
            {
                foreach (var current in EnemyList)
                    if (!current.IsKilled)
                    {
                        current.Update(gameTime);
                    }

                foreach (var current2 in TempEnemyList)
                    if (!current2.IsKilled)
                    {
                        current2.Update(gameTime);
                    }
            }
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            camera.Draw(Game.GenericTexture, new Rectangle((int) X, (int) Y, Width, Height),
                Color.White * BackBufferOpacity);
        }

        public void ChangeWindowOpacity()
        {
            foreach (var current in GameObjList)
                if (current.Name == "stainglass")
                {
                    Tween.To(current, 2f, Tween.EaseNone, "Opacity", "0.2");
                }
        }

        public override void BossCleanup()
        {
            Player.StopAllSpells();
            Game.PlayerStats.NewBossBeaten = true;
            m_bossDoorSprite.ChangeSprite("CastleDoorOpen_Sprite");
            m_bossDoor.Locked = false;
            SoundManager.PlaySound("FinalBoss_St2_WeatherChange_b");
            DropGold();
            AddEnemyKilled();
        }

        private void AddEnemyKilled()
        {
            Game.PlayerStats.NumEnemiesBeaten++;
            var value = Game.PlayerStats.EnemiesKilledList[m_boss.Type];
            value.X += 1f;
            value.Y += 1f;
            Game.PlayerStats.EnemiesKilledList[m_boss.Type] = value;
        }

        private void DropGold()
        {
            var list = new List<int>();
            for (var i = 0; i < m_bossCoins; i++) list.Add(0);
            for (var j = 0; j < m_bossMoneyBags; j++) list.Add(1);
            for (var k = 0; k < m_bossDiamonds; k++) list.Add(2);
            CDGMath.Shuffle(list);
            var num = 0f;
            SoundManager.PlaySound("Boss_Flash");
            for (var l = 0; l < list.Count; l++)
            {
                var position = m_boss.Position;
                if (list[l] == 0)
                {
                    Tween.RunFunction(l * num, Player.AttachedLevel.ItemDropManager, "DropItemWide", position, 1, 10);
                }
                else if (list[l] == 1)
                {
                    Tween.RunFunction(l * num, Player.AttachedLevel.ItemDropManager, "DropItemWide", position, 10, 100);
                }
                else
                {
                    Tween.RunFunction(l * num, Player.AttachedLevel.ItemDropManager, "DropItemWide", position, 11, 500);
                }
            }
        }

        protected override GameObj CreateCloneInstance()
        {
            return new LastBossRoom();
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_fountain = null;
                m_boss = null;
                base.Dispose();
            }
        }
    }
}
