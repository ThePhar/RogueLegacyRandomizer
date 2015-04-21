/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class BlobChallengeRoom : ChallengeBossRoomObj
    {
        private EnemyObj_Blob m_boss;
        private EnemyObj_Blob m_boss2;
        private Vector2 m_startingCamPos;

        public override bool BossKilled
        {
            get { return NumActiveBlobs == 0; }
        }

        public int NumActiveBlobs
        {
            get
            {
                int num = 0;
                foreach (EnemyObj current in EnemyList)
                {
                    if (current.Type == 2 && !current.IsKilled)
                    {
                        num++;
                    }
                }
                foreach (EnemyObj current2 in TempEnemyList)
                {
                    if (current2.Type == 2 && !current2.IsKilled)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public BlobChallengeRoom()
        {
            m_roomActivityDelay = 0.5f;
        }

        public override void Initialize()
        {
            m_boss = (EnemyList[0] as EnemyObj_Blob);
            m_boss.SaveToFile = false;
            m_boss2 = (EnemyList[1] as EnemyObj_Blob);
            m_boss2.SaveToFile = false;
            base.Initialize();
        }

        private void SetRoomData()
        {
            m_boss.Name = "Astrodotus";
            m_boss.GetChildAt(0).TextureColor = Color.Green;
            m_boss.GetChildAt(2).TextureColor = Color.LightGreen;
            m_boss.GetChildAt(2).Opacity = 0.8f;
            (m_boss.GetChildAt(1) as SpriteObj).OutlineColour = Color.Red;
            m_boss.GetChildAt(1).TextureColor = Color.Red;
            m_boss2.GetChildAt(0).TextureColor = Color.Red;
            m_boss2.GetChildAt(2).TextureColor = Color.LightPink;
            m_boss2.GetChildAt(2).Opacity = 0.8f;
            (m_boss2.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
            m_boss2.GetChildAt(1).TextureColor = Color.DarkGray;
            m_boss.Level = 100;
            m_boss.MaxHealth = 100;
            m_boss.Damage = 370;
            m_boss.IsWeighted = false;
            m_boss.TurnSpeed = 0.015f;
            m_boss.Speed = 400f;
            m_boss.IsNeo = true;
            m_boss.ChangeNeoStats(0.8f, 1.06f, 6);
            m_boss.Scale = new Vector2(2f, 2f);
            m_boss2.Level = m_boss.Level;
            m_boss2.MaxHealth = m_boss.MaxHealth;
            m_boss2.Damage = m_boss.Damage;
            m_boss2.IsWeighted = m_boss.IsWeighted;
            m_boss2.TurnSpeed = 0.01f;
            m_boss2.Speed = 625f;
            m_boss2.IsNeo = m_boss.IsNeo;
            m_boss2.ChangeNeoStats(0.75f, 1.16f, 5);
            m_boss2.Scale = m_boss.Scale;
            Game.PlayerStats.PlayerName = "Lady Echidna";
            Game.PlayerStats.Class = 16;
            Game.PlayerStats.Spell = 15;
            Game.PlayerStats.IsFemale = true;
            Game.PlayerStats.BonusHealth = 90;
            Game.PlayerStats.BonusMana = 8;
            Game.PlayerStats.BonusStrength = 50;
            Game.PlayerStats.BonusMagic = 33;
            Game.PlayerStats.BonusDefense = 230;
            Game.PlayerStats.Traits = new Vector2(2f, 17f);
            Player.CanBeKnockedBack = false;
            Game.PlayerStats.GetEquippedArray[0] = 8;
            Game.PlayerStats.GetEquippedArray[1] = 8;
            Game.PlayerStats.GetEquippedArray[3] = 8;
            Game.PlayerStats.GetEquippedArray[2] = 8;
            Game.PlayerStats.GetEquippedRuneArray[1] = 7;
            Game.PlayerStats.GetEquippedRuneArray[2] = 7;
            Player.IsWeighted = false;
            if (m_boss != null)
            {
                m_boss.CurrentHealth = m_boss.MaxHealth;
            }
            if (m_boss2 != null)
            {
                m_boss2.CurrentHealth = m_boss2.MaxHealth;
            }
        }

        public override void OnEnter()
        {
            //Player.Flip = SpriteEffects.FlipHorizontally;
            StorePlayerData();
            Player.Flip = SpriteEffects.FlipHorizontally;
            SetRoomData();
            m_cutsceneRunning = true;
            SoundManager.StopMusic(0.5f);
            m_boss.AnimationDelay = 0.1f;
            m_boss.ChangeSprite("EnemyBlobBossAir_Character");
            m_boss.PlayAnimation();
            m_boss2.AnimationDelay = 0.1f;
            m_boss2.ChangeSprite("EnemyBlobBossAir_Character");
            m_boss2.PlayAnimation();
            Player.AttachedLevel.UpdateCamera();
            m_startingCamPos = Player.AttachedLevel.Camera.Position;
            Player.LockControls();
            Player.AttachedLevel.RunCinematicBorders(6f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.AttachedLevel.Camera.Y = Player.Y;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", m_boss.Y.ToString(), "X", m_boss.X.ToString());
            Tween.RunFunction(1.2f, this, "DisplayBossTitle", Game.PlayerStats.PlayerName + " VS", m_boss.Name, "Intro2");
            base.OnEnter();
            m_bossChest.ForcedItemType = 18;
        }

        public void Intro2()
        {
            object arg_96_0 = Player.AttachedLevel.Camera;
            float arg_96_1 = 1f;
            Easing arg_96_2 = Quad.EaseInOut;
            string[] array = new string[8];
            array[0] = "delay";
            array[1] = "0.5";
            array[2] = "X";
            string[] arg_5D_0 = array;
            int arg_5D_1 = 3;
            int x = Bounds.Center.X;
            arg_5D_0[arg_5D_1] = x.ToString();
            array[4] = "Y";
            string[] arg_84_0 = array;
            int arg_84_1 = 5;
            int y = Bounds.Center.Y;
            arg_84_0[arg_84_1] = y.ToString();
            array[6] = "Zoom";
            array[7] = "0.5";
            Tween.To(arg_96_0, arg_96_1, arg_96_2, array);
            Tween.AddEndHandlerToLastTween(this, "EndCutscene");
        }

        public void EndCutscene()
        {
            m_boss.Rotation = 0f;
            Player.IsWeighted = true;
            SoundManager.PlayMusic("DungeonBoss", false, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Player.UnlockControls();
            m_cutsceneRunning = false;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle bounds = Bounds;
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
            List<EnemyObj> list = Player.AttachedLevel.CurrentRoom.EnemyList;
            foreach (EnemyObj current in list)
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
            foreach (EnemyObj current2 in list)
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
            Vector2 position = Player.Position;
            if (Player.X - Player.Width/2f < X)
            {
                Player.Position = new Vector2(Player.X + Width, Player.Y);
                Player.Draw(camera);
            }
            else if (Player.X + Player.Width/2f > X + Width)
            {
                Player.Position = new Vector2(Player.X - Width, Player.Y);
                Player.Draw(camera);
            }
            if (Player.Y - Player.Height/2f < Y)
            {
                Player.Position = new Vector2(Player.X, Player.Y + Height);
                Player.Draw(camera);
            }
            else if (Player.Y + Player.Height/2f > Y + Height)
            {
                Player.Position = new Vector2(Player.X, Player.Y - Height);
                Player.Draw(camera);
            }
            Player.Position = position;
            foreach (EnemyObj current in EnemyList)
            {
                Vector2 position2 = current.Position;
                Rectangle pureBounds = current.PureBounds;
                if (current.X - current.Width/2f < X)
                {
                    current.Position = new Vector2(current.X + Width, current.Y);
                    current.Draw(camera);
                }
                else if (current.X + current.Width/2f > X + Width)
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
            foreach (EnemyObj current2 in TempEnemyList)
            {
                current2.ForceDraw = true;
                Vector2 position3 = current2.Position;
                Rectangle pureBounds2 = current2.PureBounds;
                if (current2.X - current2.Width/2f < X)
                {
                    current2.Position = new Vector2(current2.X + Width, current2.Y);
                    current2.Draw(camera);
                }
                else if (current2.X + current2.Width/2f > X + Width)
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
                foreach (EnemyObj current in EnemyList)
                {
                    current.Reset();
                }
            }
            foreach (EnemyObj current2 in TempEnemyList)
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
            Game.PlayerStats.ChallengeBlobBeaten = true;
            GameUtil.UnlockAchievement("FEAR_OF_SPACE");
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
            if (!IsDisposed)
            {
                m_boss = null;
                m_boss2 = null;
                base.Dispose();
            }
        }
    }
}