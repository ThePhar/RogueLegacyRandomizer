// 
// RogueLegacyArchipelago - ChallengeBossRoomObj.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;
using LogicSet = DS2DEngine.LogicSet;

namespace RogueCastle
{
    public abstract class ChallengeBossRoomObj : RoomObj
    {
        protected ChestObj m_bossChest;
        private SpriteObj m_bossDivider;
        private TextObj m_bossTitle1;
        private TextObj m_bossTitle2;
        protected bool m_cutsceneRunning;
        private List<RaindropObj> m_rainFG;
        private float m_roomFloor;
        private float m_sparkleTimer;
        private PlayerStats m_storedPlayerStats;
        private Vector2 m_storedScale;
        private bool m_teleportingOut;

        public ChallengeBossRoomObj()
        {
            m_storedPlayerStats = new PlayerStats();
        }

        public abstract bool BossKilled { get; }
        public int StoredHP { get; private set; }
        public float StoredMP { get; private set; }

        public override void Initialize()
        {
            m_bossTitle1 = new TextObj(Game.JunicodeFont);
            m_bossTitle1.Text = "The Forsaken";
            m_bossTitle1.OutlineWidth = 2;
            m_bossTitle1.FontSize = 18f;
            m_bossTitle2 = new TextObj(Game.JunicodeLargeFont);
            m_bossTitle2.Text = "Alexander";
            m_bossTitle2.OutlineWidth = 2;
            m_bossTitle2.FontSize = 40f;
            m_bossDivider = new SpriteObj("Blank_Sprite");
            m_bossDivider.OutlineWidth = 2;
            foreach (var current in DoorList) m_roomFloor = current.Bounds.Bottom;
            m_bossChest = new ChestObj(null);
            m_bossChest.Position = new Vector2(Bounds.Center.X - m_bossChest.Width / 2f, Bounds.Center.Y);
            GameObjList.Add(m_bossChest);
            m_rainFG = new List<RaindropObj>();
            for (var i = 0; i < 50; i++)
            {
                var raindropObj =
                    new RaindropObj(new Vector2(CDGMath.RandomFloat(X - Width, X), CDGMath.RandomFloat(Y, Y + Height)));
                m_rainFG.Add(raindropObj);
                raindropObj.ChangeToParticle();
            }

            base.Initialize();
        }

        public void StorePlayerData()
        {
            m_storedPlayerStats = Game.PlayerStats;
            Game.PlayerStats = new PlayerStats();
            Game.PlayerStats.TutorialComplete = true;
            m_storedScale = Player.Scale;
            Player.Scale = new Vector2(2f, 2f);
            SkillSystem.ResetAllTraits();
            Player.OverrideInternalScale(Player.Scale);
            StoredHP = Player.CurrentHealth;
            StoredMP = Player.CurrentMana;
        }

        public void LoadPlayerData()
        {
            Game.PlayerStats = m_storedPlayerStats;
        }

        public override void OnEnter()
        {
            Player.CurrentHealth = Player.MaxHealth;
            Player.CurrentMana = Player.MaxMana;
            Player.ChangeSprite("PlayerIdle_Character");
            Player.AttachedLevel.UpdatePlayerHUD();
            Player.AttachedLevel.UpdatePlayerHUDAbilities();
            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.UpdateEquipmentColours();
            Player.AttachedLevel.RefreshPlayerHUDPos();
            Game.ScreenManager.GetLevelScreen().JukeboxEnabled = false;
            m_bossChest.ChestType = 5;
            m_bossChest.Visible = false;
            m_bossChest.IsLocked = true;
            m_bossChest.X = Player.X;
            if (m_bossChest.PhysicsMngr == null)
            {
                Player.PhysicsMngr.AddObject(m_bossChest);
            }

            m_teleportingOut = false;
            m_bossTitle1.Opacity = 0f;
            m_bossTitle2.Opacity = 0f;
            m_bossDivider.ScaleX = 0f;
            m_bossDivider.Opacity = 0f;
            if (LevelENV.WeakenBosses)
            {
                foreach (var current in EnemyList) current.CurrentHealth = 1;
            }

            base.OnEnter();
        }

        public override void OnExit()
        {
            LoadPlayerData();
            (Game.ScreenManager.Game as Game).SaveManager.LoadFiles(Player.AttachedLevel, SaveType.UpgradeData);
            Player.AttachedLevel.UpdatePlayerHUD();
            Player.AttachedLevel.UpdatePlayerHUDAbilities();
            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
            Player.AttachedLevel.UpdatePlayerSpellIcon();
            Player.UpdateEquipmentColours();
            Player.AttachedLevel.RefreshPlayerHUDPos();
            Player.CurrentHealth = StoredHP;
            Player.CurrentMana = StoredMP;
            if (BossKilled)
            {
                SaveCompletionData();
            }

            Game.PlayerStats.NewBossBeaten = true;
            if (LinkedRoom != null)
            {
                Player.AttachedLevel.CloseBossDoor(LinkedRoom, Zone);
            }

            (Game.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData);
            base.OnExit();
        }

        public void DisplayBossTitle(string bossTitle1, string bossTitle2, string endHandler)
        {
            SoundManager.PlaySound("Boss_Title");
            m_bossTitle1.Text = bossTitle1;
            m_bossTitle2.Text = bossTitle2;
            var camera = Player.AttachedLevel.Camera;
            if (Player.AttachedLevel.CurrentRoom is LastBossRoom)
            {
                m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 100f);
            }
            else
            {
                m_bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 50f);
            }

            m_bossTitle2.X = m_bossTitle1.X - 0f;
            m_bossTitle2.Y = m_bossTitle1.Y + 50f;
            m_bossDivider.Position = m_bossTitle1.Position;
            m_bossDivider.Y += m_bossTitle1.Height - 5;
            m_bossTitle1.X -= 1000f;
            m_bossTitle2.X += 1500f;
            Tween.To(m_bossDivider, 0.5f, Tween.EaseNone, "delay", "0.3", "Opacity", "1");
            Tween.To(m_bossDivider, 1f, Quad.EaseInOut, "delay", "0", "ScaleX",
                ((float) (m_bossTitle2.Width / 5)).ToString());
            Tween.To(m_bossTitle1, 0.5f, Tween.EaseNone, "delay", "0.3", "Opacity", "1");
            Tween.To(m_bossTitle2, 0.5f, Tween.EaseNone, "delay", "0.3", "Opacity", "1");
            Tween.By(m_bossTitle1, 1f, Quad.EaseOut, "X", "1000");
            Tween.By(m_bossTitle2, 1f, Quad.EaseOut, "X", "-1500");
            m_bossTitle1.X += 1000f;
            m_bossTitle2.X -= 1500f;
            Tween.By(m_bossTitle1, 2f, Tween.EaseNone, "delay", "1", "X", "20");
            Tween.By(m_bossTitle2, 2f, Tween.EaseNone, "delay", "1", "X", "-20");
            m_bossTitle1.X -= 1000f;
            m_bossTitle2.X += 1500f;
            Tween.AddEndHandlerToLastTween(this, endHandler);
            Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", "Boss_Title_Exit");
            m_bossTitle1.X += 1020f;
            m_bossTitle2.X -= 1520f;
            m_bossTitle1.Opacity = 1f;
            m_bossTitle2.Opacity = 1f;
            Tween.To(m_bossTitle1, 0.5f, Tween.EaseNone, "delay", "3", "Opacity", "0");
            Tween.To(m_bossTitle2, 0.5f, Tween.EaseNone, "delay", "3", "Opacity", "0");
            Tween.By(m_bossTitle1, 0.6f, Quad.EaseIn, "delay", "3", "X", "1500");
            Tween.By(m_bossTitle2, 0.6f, Quad.EaseIn, "delay", "3", "X", "-1000");
            m_bossTitle1.Opacity = 0f;
            m_bossTitle2.Opacity = 0f;
            m_bossDivider.Opacity = 1f;
            Tween.To(m_bossDivider, 0.5f, Tween.EaseNone, "delay", "2.8", "Opacity", "0");
            m_bossDivider.Opacity = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var current in m_rainFG) current.UpdateNoCollision(gameTime);
            if (!m_cutsceneRunning)
            {
                base.Update(gameTime);
            }

            if (BossKilled && !m_bossChest.Visible)
            {
                BossCleanup();
                m_bossChest.Visible = true;
                m_bossChest.Opacity = 0f;
                SoundManager.PlayMusic("TitleScreenSong", true, 1f);
                Tween.To(m_bossChest, 4f, Tween.EaseNone, "Opacity", "1");
                Tween.To(m_bossChest, 4f, Quad.EaseOut, "Y", m_roomFloor.ToString());
                Tween.AddEndHandlerToLastTween(this, "UnlockChest");
                m_sparkleTimer = 0.5f;
            }

            if (m_bossChest.Visible && !m_bossChest.IsOpen && BossKilled)
            {
                if (m_sparkleTimer > 0f)
                {
                    m_sparkleTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_sparkleTimer <= 0f)
                    {
                        m_sparkleTimer = 0.5f;
                        Tween.RunFunction(0f, Player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect",
                            new Vector2(m_bossChest.X, m_bossChest.Y - m_bossChest.Height / 2));
                    }
                }
            }
            else if (m_bossChest.Visible && m_bossChest.IsOpen && BossKilled && !m_teleportingOut)
            {
                m_teleportingOut = true;
                if (LevelENV.RunDemoVersion)
                {
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(29, true);
                    return;
                }

                TeleportPlayer();
            }
        }

        protected abstract void SaveCompletionData();

        public virtual void BossCleanup()
        {
            Player.StopAllSpells();
        }

        public void TeleportPlayer()
        {
            Player.CurrentSpeed = 0f;
            var position = Player.Position;
            var scale = Player.Scale;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", true));
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false));
            logicSet.AddAction(new DelayLogicAction(0.5f));
            logicSet.AddAction(new PlaySoundLogicAction("Teleport_Disappear"));
            logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleOut"));
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleport",
                new Vector2(Player.X, Player.Bounds.Bottom), Player.Scale));
            logicSet.AddAction(new DelayLogicAction(0.3f));
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition"));
            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new RunFunctionLogicAction(this, "LoadPlayerData"));
            if (LinkedRoom != null)
            {
                Player.Scale = m_storedScale;
                Player.OverrideInternalScale(m_storedScale);
                Player.UpdateCollisionBoxes();
                Player.Position = new Vector2(LinkedRoom.Bounds.Center.X,
                    LinkedRoom.Bounds.Bottom - 60 - (Player.TerrainBounds.Bottom - Player.Y));
                logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", false));
                logicSet.AddAction(new ChangePropertyLogicAction(Player, "ScaleY", m_storedScale.Y));
                logicSet.AddAction(new TeleportLogicAction(null, Player.Position));
                logicSet.AddAction(new DelayLogicAction(0.05f));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition"));
                logicSet.AddAction(new DelayLogicAction(1f));
                logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleIn"));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool,
                    "MegaTeleportReverse", new Vector2(Player.X, LinkedRoom.Bounds.Bottom - 60), m_storedScale));
                logicSet.AddAction(new PlaySoundLogicAction("Teleport_Reappear"));
            }

            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "ForceInvincible", false));
            logicSet.AddAction(new RunFunctionLogicAction(Player, "UnlockControls"));
            Player.RunExternalLogicSet(logicSet);
            Player.Position = position;
            Player.Scale = scale;
            Player.UpdateCollisionBoxes();
        }

        public void TeleportScaleOut()
        {
            Tween.To(Player, 0.05f, Tween.EaseNone, "ScaleX", "0");
        }

        public void TeleportScaleIn()
        {
            Tween.To(Player, 0.05f, Tween.EaseNone, "delay", "0.15", "ScaleX", m_storedScale.X.ToString());
        }

        public void KickPlayerOut()
        {
            Player.AttachedLevel.PauseScreen();
            SoundManager.StopMusic();
            Player.LockControls();
            Player.AttachedLevel.RunWhiteSlashEffect();
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
            Player.IsWeighted = false;
            Player.IsCollidable = false;
            Player.CurrentSpeed = 0f;
            Player.AccelerationX = 0f;
            Player.AccelerationY = 0f;
            if (this is BlobChallengeRoom)
            {
                Tween.To(Player.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, "Zoom", "1", "X", Player.X.ToString(), "Y",
                    Player.Y.ToString());
                Tween.AddEndHandlerToLastTween(this, "LockCamera");
            }

            Tween.RunFunction(1f, this, "KickPlayerOut2");
        }

        public void KickPlayerOut2()
        {
            Player.AttachedLevel.UnpauseScreen();
            Player.CurrentSpeed = 0f;
            var position = Player.Position;
            var scale = Player.Scale;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", true));
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
            logicSet.AddAction(new DelayLogicAction(1.3f));
            logicSet.AddAction(new PlaySoundLogicAction("Teleport_Disappear"));
            logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleOut"));
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleport",
                new Vector2(Player.X, Player.Bounds.Bottom), Player.Scale));
            logicSet.AddAction(new DelayLogicAction(0.3f));
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition"));
            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new RunFunctionLogicAction(this, "LoadPlayerData"));
            if (LinkedRoom != null)
            {
                Player.Scale = m_storedScale;
                Player.OverrideInternalScale(m_storedScale);
                Player.UpdateCollisionBoxes();
                Player.Position = new Vector2(LinkedRoom.Bounds.Center.X,
                    LinkedRoom.Bounds.Bottom - 60 - (Player.TerrainBounds.Bottom - Player.Y));
                logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", false));
                logicSet.AddAction(new ChangePropertyLogicAction(Player, "ScaleY", m_storedScale.Y));
                logicSet.AddAction(new TeleportLogicAction(null, Player.Position));
                logicSet.AddAction(new DelayLogicAction(0.05f));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition"));
                logicSet.AddAction(new DelayLogicAction(1f));
                logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportScaleIn"));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool,
                    "MegaTeleportReverse", new Vector2(Player.X, LinkedRoom.Bounds.Bottom - 60), m_storedScale));
                logicSet.AddAction(new PlaySoundLogicAction("Teleport_Reappear"));
            }

            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", true));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", true));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "ForceInvincible", false));
            logicSet.AddAction(new RunFunctionLogicAction(Player, "UnlockControls"));
            Player.RunExternalLogicSet(logicSet);
            Player.Position = position;
            Player.Scale = scale;
            Player.UpdateCollisionBoxes();
        }

        public void LockCamera()
        {
            Player.AttachedLevel.CameraLockedToPlayer = true;
        }

        public void UnlockChest()
        {
            m_bossChest.IsLocked = false;
        }

        public override void Draw(Camera2D camera)
        {
            foreach (var current in m_rainFG) current.Draw(camera);
            base.Draw(camera);
            m_bossDivider.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            m_bossTitle1.Draw(camera);
            m_bossTitle2.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_bossChest = null;
                m_bossDivider.Dispose();
                m_bossDivider = null;
                m_bossTitle1.Dispose();
                m_bossTitle1 = null;
                m_bossTitle2.Dispose();
                m_bossTitle2 = null;
                foreach (var current in m_rainFG) current.Dispose();
                m_rainFG.Clear();
                m_rainFG = null;
                base.Dispose();
            }
        }
    }
}