// 
//  Rogue Legacy Randomizer - BossRoomObj.cs
//  Last Modified 2022-01-24
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
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;

using LogicSet = DS2DEngine.LogicSet;

namespace RogueCastle
{
    public abstract class BossRoomObj : RoomObj
    {
        protected       ChestObj  _bossChest;
        private         SpriteObj _bossDivider;
        private         TextObj   _bossTitle1;
        private         TextObj   _bossTitle2;
        protected       bool      _cutsceneRunning;
        private         float     _roomFloor;
        private         float     _sparkleTimer;
        private         bool      _teleportingOut;
        public abstract bool      BossKilled { get; }

        public override void Initialize()
        {
            _bossTitle1 = new TextObj(Game.JunicodeFont) { Text = "The Forsaken", OutlineWidth = 2, FontSize = 18f };
            _bossTitle2 = new TextObj(Game.JunicodeLargeFont) { Text = "Alexander", OutlineWidth = 2, FontSize = 40f };
            _bossDivider = new SpriteObj("Blank_Sprite") { OutlineWidth = 2 };
            foreach (var current in DoorList) _roomFloor = current.Bounds.Bottom;
            _bossChest = new ChestObj(null);
            _bossChest.Position = new Vector2(Bounds.Center.X - _bossChest.Width / 2f, Bounds.Center.Y);
            GameObjList.Add(_bossChest);
            base.Initialize();
        }

        public override void OnEnter()
        {
            Game.ScreenManager.GetLevelScreen().JukeboxEnabled = false;
            _bossChest.ChestType = ChestType.Boss;
            _bossChest.Visible = false;
            _bossChest.IsLocked = true;
            if (_bossChest.PhysicsMngr == null)
            {
                Player.PhysicsMngr.AddObject(_bossChest);
            }

            _teleportingOut = false;
            _bossTitle1.Opacity = 0f;
            _bossTitle2.Opacity = 0f;
            _bossDivider.ScaleX = 0f;
            _bossDivider.Opacity = 0f;
            base.OnEnter();
        }

        public void DisplayBossTitle(string bossTitle1, string bossTitle2, string endHandler)
        {
            SoundManager.PlaySound("Boss_Title");
            _bossTitle1.Text = bossTitle1;
            _bossTitle2.Text = bossTitle2;
            var camera = Player.AttachedLevel.Camera;
            if (Player.AttachedLevel.CurrentRoom is LastBossRoom)
            {
                _bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 100f);
            }
            else
            {
                _bossTitle1.Position = new Vector2(camera.X - 550f, camera.Y + 50f);
            }

            _bossTitle2.X = _bossTitle1.X - 0f;
            _bossTitle2.Y = _bossTitle1.Y + 50f;
            _bossDivider.Position = _bossTitle1.Position;
            _bossDivider.Y += _bossTitle1.Height - 5;
            _bossTitle1.X -= 1000f;
            _bossTitle2.X += 1500f;
            Tween.To(_bossDivider, 0.5f, Tween.EaseNone, "delay", "0.3", "Opacity", "1");
            Tween.To(_bossDivider, 1f, Quad.EaseInOut, "delay", "0", "ScaleX",
                ((float) (_bossTitle2.Width / 5)).ToString());
            Tween.To(_bossTitle1, 0.5f, Tween.EaseNone, "delay", "0.3", "Opacity", "1");
            Tween.To(_bossTitle2, 0.5f, Tween.EaseNone, "delay", "0.3", "Opacity", "1");
            Tween.By(_bossTitle1, 1f, Quad.EaseOut, "X", "1000");
            Tween.By(_bossTitle2, 1f, Quad.EaseOut, "X", "-1500");
            _bossTitle1.X += 1000f;
            _bossTitle2.X -= 1500f;
            Tween.By(_bossTitle1, 2f, Tween.EaseNone, "delay", "1", "X", "20");
            Tween.By(_bossTitle2, 2f, Tween.EaseNone, "delay", "1", "X", "-20");
            _bossTitle1.X -= 1000f;
            _bossTitle2.X += 1500f;
            Tween.AddEndHandlerToLastTween(this, endHandler);
            Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", "Boss_Title_Exit");
            _bossTitle1.X += 1020f;
            _bossTitle2.X -= 1520f;
            _bossTitle1.Opacity = 1f;
            _bossTitle2.Opacity = 1f;
            Tween.To(_bossTitle1, 0.5f, Tween.EaseNone, "delay", "3", "Opacity", "0");
            Tween.To(_bossTitle2, 0.5f, Tween.EaseNone, "delay", "3", "Opacity", "0");
            Tween.By(_bossTitle1, 0.6f, Quad.EaseIn, "delay", "3", "X", "1500");
            Tween.By(_bossTitle2, 0.6f, Quad.EaseIn, "delay", "3", "X", "-1000");
            _bossTitle1.Opacity = 0f;
            _bossTitle2.Opacity = 0f;
            _bossDivider.Opacity = 1f;
            Tween.To(_bossDivider, 0.5f, Tween.EaseNone, "delay", "2.8", "Opacity", "0");
            _bossDivider.Opacity = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_cutsceneRunning)
            {
                base.Update(gameTime);
            }

            if (BossKilled && !_bossChest.Visible)
            {
                BossCleanup();
                _bossChest.Visible = true;
                _bossChest.Opacity = 0f;
                SoundManager.PlayMusic("TitleScreenSong", true, 1f);
                Tween.To(_bossChest, 4f, Tween.EaseNone, "Opacity", "1");
                Tween.To(_bossChest, 4f, Quad.EaseOut, "Y", _roomFloor.ToString());
                Tween.AddEndHandlerToLastTween(this, "UnlockChest");
                _sparkleTimer = 0.5f;
            }

            if (_bossChest.Visible && !_bossChest.IsOpen && BossKilled)
            {
                if (!(_sparkleTimer > 0f))
                {
                    return;
                }

                _sparkleTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;

                if (!(_sparkleTimer <= 0f))
                {
                    return;
                }

                _sparkleTimer = 0.5f;
                Tween.RunFunction(0f, Player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect",
                    new Vector2(_bossChest.X, _bossChest.Y - _bossChest.Height / 2));
            }
            else if (_bossChest.Visible && _bossChest.IsOpen && BossKilled && !_teleportingOut)
            {
                _teleportingOut = true;
                if (LevelENV.RunDemoVersion)
                {
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(29, true);
                    return;
                }

                TeleportPlayer();
            }
        }

        public virtual void BossCleanup()
        {
            Player.StopAllSpells();
            Game.PlayerStats.NewBossBeaten = true;
            if (LinkedRoom != null)
            {
                Player.AttachedLevel.CloseBossDoor(LinkedRoom, Zone);
            }
        }

        public void TeleportPlayer()
        {
            Player.CurrentSpeed = 0f;
            var position = Player.Position;
            var scale = Player.Scale;
            Tween.To(Player, 0.05f, Linear.EaseNone, "delay", "1.2", "ScaleX", "0");
            Player.ScaleX = 0f;
            Tween.To(Player, 0.05f, Linear.EaseNone, "delay", "7", "ScaleX", scale.X.ToString());
            Player.ScaleX = scale.X;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", true));
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character", true, false));
            logicSet.AddAction(new DelayLogicAction(0.5f));
            logicSet.AddAction(new PlaySoundLogicAction("Teleport_Disappear"));
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool, "MegaTeleport",
                new Vector2(Player.X, Player.Bounds.Bottom), Player.Scale));
            logicSet.AddAction(new DelayLogicAction(0.3f));
            logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition"));
            logicSet.AddAction(new DelayLogicAction(0.2f));
            if (LinkedRoom != null)
            {
                Player.Position = new Vector2(Player.AttachedLevel.RoomList[1].Bounds.Center.X,
                    Player.AttachedLevel.RoomList[1].Bounds.Center.Y);
                Player.UpdateCollisionBoxes();
                logicSet.AddAction(new TeleportLogicAction(null, Player.Position));
                logicSet.AddAction(new DelayLogicAction(0.05f));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition"));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.RoomList[1], "RevealSymbol",
                    Zone, true));
                logicSet.AddAction(new DelayLogicAction(3.5f));
                logicSet.AddAction(
                    new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "StartWipeTransition"));
                logicSet.AddAction(new DelayLogicAction(0.2f));
                Player.Position = new Vector2(LinkedRoom.Bounds.Center.X,
                    LinkedRoom.Bounds.Bottom - 60 - (Player.Bounds.Bottom - Player.Y));
                Player.UpdateCollisionBoxes();
                logicSet.AddAction(new ChangePropertyLogicAction(Player.AttachedLevel, "DisableSongUpdating", false));
                logicSet.AddAction(new TeleportLogicAction(null, Player.Position));
                logicSet.AddAction(new DelayLogicAction(0.05f));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ScreenManager, "EndWipeTransition"));
                logicSet.AddAction(new DelayLogicAction(1f));
                logicSet.AddAction(new RunFunctionLogicAction(Player.AttachedLevel.ImpactEffectPool,
                    "MegaTeleportReverse", new Vector2(Player.X, LinkedRoom.Bounds.Bottom - 60), scale));
                logicSet.AddAction(new PlaySoundLogicAction("Teleport_Reappear"));
            }

            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "ForceInvincible", false));
            logicSet.AddAction(new RunFunctionLogicAction(Player, "UnlockControls"));
            Player.RunExternalLogicSet(logicSet);
            Player.Position = position;
            Player.UpdateCollisionBoxes();
        }

        public void UnlockChest()
        {
            _bossChest.IsLocked = false;
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            _bossDivider.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _bossTitle1.Draw(camera);
            _bossTitle2.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            _bossChest = null;
            _bossDivider.Dispose();
            _bossDivider = null;
            _bossTitle1.Dispose();
            _bossTitle1 = null;
            _bossTitle2.Dispose();
            _bossTitle2 = null;
            base.Dispose();
        }
    }
}
