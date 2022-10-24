// Rogue Legacy Randomizer - BlobChallengeRoom.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
{
    public class BlobChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Blob _boss;
        private EnemyObj_Blob _boss2;
        private Vector2       _startingCamPos;

        public BlobChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override bool BossKilled => NumActiveBlobs == 0;

        public int NumActiveBlobs =>
            EnemyList.Count(current => current.Type == 2 && !current.IsKilled) +
            TempEnemyList.Count(current2 => current2.Type == 2 && !current2.IsKilled);

        public override void Initialize()
        {
            _boss = EnemyList[0] as EnemyObj_Blob;
            _boss.SaveToFile = false;
            _boss2 = EnemyList[1] as EnemyObj_Blob;
            _boss2.SaveToFile = false;
            base.Initialize();
        }

        private void SetRoomData()
        {
            _boss.Name = "Astrodotus";
            _boss.GetChildAt(0).TextureColor = Color.Green;
            _boss.GetChildAt(2).TextureColor = Color.LightGreen;
            _boss.GetChildAt(2).Opacity = 0.8f;
            (_boss.GetChildAt(1) as SpriteObj).OutlineColour = Color.Red;
            _boss.GetChildAt(1).TextureColor = Color.Red;
            _boss2.GetChildAt(0).TextureColor = Color.Red;
            _boss2.GetChildAt(2).TextureColor = Color.LightPink;
            _boss2.GetChildAt(2).Opacity = 0.8f;
            (_boss2.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
            _boss2.GetChildAt(1).TextureColor = Color.DarkGray;
            _boss.MaxHealth = 64;
            _boss.Damage = 20;
            _boss.IsWeighted = false;
            _boss.TurnSpeed = 0.015f;
            _boss.Speed = 400f;
            _boss.IsNeo = true;
            _boss.ChangeNeoStats(0.8f, 1.06f, 6);
            _boss.Scale = new Vector2(2f, 2f);
            _boss2.Level = _boss.Level;
            _boss2.MaxHealth = _boss.MaxHealth;
            _boss2.Damage = _boss.Damage;
            _boss2.IsWeighted = _boss.IsWeighted;
            _boss2.TurnSpeed = 0.01f;
            _boss2.Speed = 625f;
            _boss2.IsNeo = _boss.IsNeo;
            _boss2.ChangeNeoStats(0.75f, 1.16f, 5);
            _boss2.Scale = _boss.Scale;

            // Soft-lock prevention.
            Player.CanBeKnockedBack = false;

            if (_boss != null)
            {
                _boss.CurrentHealth = _boss.MaxHealth;
            }

            if (_boss2 != null)
            {
                _boss2.CurrentHealth = _boss2.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            //Player.Flip = SpriteEffects.FlipHorizontally;
            Player.Flip = SpriteEffects.FlipHorizontally;
            SetRoomData();
            _cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            _boss.AnimationDelay = 0.1f;
            _boss.ChangeSprite("EnemyBlobBossAir_Character");
            _boss.PlayAnimation();
            _boss2.AnimationDelay = 0.1f;
            _boss2.ChangeSprite("EnemyBlobBossAir_Character");
            _boss2.PlayAnimation();
            Player.AttachedLevel.UpdateCamera();
            _startingCamPos = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", _boss.Y.ToString(), "X",
                _boss.X.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", "The Infinite and Beyond", _boss.Name,
                "Intro2");
            base.OnEnter();
            _bossChest.ForcedItemType = ItemDropType.TripStatDrop;
        }

        public void Intro2()
        {
            object arg_96_0 = Player.AttachedLevel.Camera;
            var arg_96_1 = 1f;
            Easing arg_96_2 = Quad.EaseInOut;
            var array = new string[8];
            array[0] = "delay";
            array[1] = "0.5";
            array[2] = "X";
            var arg_5D_0 = array;
            var arg_5D_1 = 3;
            var x = Bounds.Center.X;
            arg_5D_0[arg_5D_1] = x.ToString();
            array[4] = "Y";
            var arg_84_0 = array;
            var arg_84_1 = 5;
            var y = Bounds.Center.Y;
            arg_84_0[arg_84_1] = y.ToString();
            array[6] = "Zoom";
            array[7] = "0.5";
            Tween.To(arg_96_0, arg_96_1, arg_96_2, array);
            Tween.AddEndHandlerToLastTween(this, "EndCutscene");
        }

        public void EndCutscene()
        {
            _boss.Rotation = 0f;
            Player.IsWeighted = true;
            SoundManager.PlayMusic("DungeonBoss", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.UnlockControls();
            _cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            var bounds = Bounds;
            if (Player.Y > bounds.Bottom)
            {
                Player.Y = bounds.Top + 20;
            }
            else if (Player.Y < bounds.Top)
            {
                Player.Y = bounds.Bottom - 20;
            }

            if (Player.X > bounds.Right)
            {
                Player.X = bounds.Left + 20;
            }
            else if (Player.X < bounds.Left)
            {
                Player.X = bounds.Right - 20;
            }

            var list = Player.AttachedLevel.CurrentRoom.EnemyList;
            foreach (var current in list)
            {
                if (current.Y > bounds.Bottom - 10)
                {
                    current.Y = bounds.Top + 20;
                }
                else if (current.Y < bounds.Top + 10)
                {
                    current.Y = bounds.Bottom - 20;
                }

                if (current.X > bounds.Right - 10)
                {
                    current.X = bounds.Left + 20;
                }
                else if (current.X < bounds.Left + 10)
                {
                    current.X = bounds.Right - 20;
                }
            }

            list = Player.AttachedLevel.CurrentRoom.TempEnemyList;
            foreach (var current2 in list)
            {
                if (current2.Y > bounds.Bottom - 10)
                {
                    current2.Y = bounds.Top + 20;
                }
                else if (current2.Y < bounds.Top + 10)
                {
                    current2.Y = bounds.Bottom - 20;
                }

                if (current2.X > bounds.Right - 10)
                {
                    current2.X = bounds.Left + 20;
                }
                else if (current2.X < bounds.Left + 10)
                {
                    current2.X = bounds.Right - 20;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            var position = Player.Position;
            if (Player.X - Player.Width / 2f < X)
            {
                Player.Position = new Vector2(Player.X + Width, Player.Y);
                Player.Draw(camera);
            }
            else if (Player.X + Player.Width / 2f > X + Width)
            {
                Player.Position = new Vector2(Player.X - Width, Player.Y);
                Player.Draw(camera);
            }

            if (Player.Y - Player.Height / 2f < Y)
            {
                Player.Position = new Vector2(Player.X, Player.Y + Height);
                Player.Draw(camera);
            }
            else if (Player.Y + Player.Height / 2f > Y + Height)
            {
                Player.Position = new Vector2(Player.X, Player.Y - Height);
                Player.Draw(camera);
            }

            Player.Position = position;
            foreach (var current in EnemyList)
            {
                var position2 = current.Position;
                var pureBounds = current.PureBounds;
                if (current.X - current.Width / 2f < X)
                {
                    current.Position = new Vector2(current.X + Width, current.Y);
                    current.Draw(camera);
                }
                else if (current.X + current.Width / 2f > X + Width)
                {
                    current.Position = new Vector2(current.X - Width, current.Y);
                    current.Draw(camera);
                }

                if (pureBounds.Top < Y)
                {
                    current.Position = new Vector2(current.X, current.Y + Height);
                    current.Draw(camera);
                }
                else if (pureBounds.Bottom > Y + Height)
                {
                    current.Position = new Vector2(current.X, current.Y - Height);
                    current.Draw(camera);
                }

                current.Position = position2;
            }

            foreach (var current2 in TempEnemyList)
            {
                current2.ForceDraw = true;
                var position3 = current2.Position;
                var pureBounds2 = current2.PureBounds;
                if (current2.X - current2.Width / 2f < X)
                {
                    current2.Position = new Vector2(current2.X + Width, current2.Y);
                    current2.Draw(camera);
                }
                else if (current2.X + current2.Width / 2f > X + Width)
                {
                    current2.Position = new Vector2(current2.X - Width, current2.Y);
                    current2.Draw(camera);
                }

                if (pureBounds2.Top < Y)
                {
                    current2.Position = new Vector2(current2.X, current2.Y + Height);
                    current2.Draw(camera);
                }
                else if (pureBounds2.Bottom > Y + Height)
                {
                    current2.Position = new Vector2(current2.X, current2.Y - Height);
                    current2.Draw(camera);
                }

                current2.Position = position3;
            }
        }

        public override void OnExit()
        {
            if (!BossKilled)
            {
                foreach (var current in EnemyList) current.Reset();
            }

            foreach (var current2 in TempEnemyList)
            {
                current2.KillSilently();
                current2.Dispose();
            }

            TempEnemyList.Clear();
            Player.CanBeKnockedBack = true;
            base.OnExit();
        }

        protected override void SaveCompletionData()
        {
            Game.PlayerStats.BlobBossBeaten = true;
        }

        protected override GameObj CreateCloneInstance()
        {
            return new BlobChallengeRoom();
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
            _boss2 = null;
            base.Dispose();
        }
    }
}
