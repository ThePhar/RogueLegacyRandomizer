// 
// RogueLegacyArchipelago - CastleEntranceRoomObj.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class CastleEntranceRoomObj : RoomObj
    {
        private bool m_allFilesSaved;
        private DoorObj m_bossDoor;
        private bool m_bossDoorOpening;
        private ObjContainer m_bossDoorSprite;
        private PhysicsObj m_castleGate;
        private SpriteObj m_diary;
        private bool m_gateClosed;
        private KeyIconObj m_mapIcon;
        private TextObj m_mapText;
        private SpriteObj m_speechBubble;
        private TeleporterObj m_teleporter;

        public CastleEntranceRoomObj()
        {
            m_castleGate = new PhysicsObj("CastleEntranceGate_Sprite");
            m_castleGate.IsWeighted = false;
            m_castleGate.IsCollidable = true;
            m_castleGate.CollisionTypeTag = 1;
            m_castleGate.Layer = -1f;
            m_castleGate.OutlineWidth = 2;
            GameObjList.Add(m_castleGate);
            m_teleporter = new TeleporterObj();
            GameObjList.Add(m_teleporter);
        }

        public override void Initialize()
        {
            m_speechBubble = new SpriteObj("ExclamationSquare_Sprite");
            m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
            m_speechBubble.Scale = new Vector2(1.2f, 1.2f);
            GameObjList.Add(m_speechBubble);
            m_mapText = new KeyIconTextObj(Game.JunicodeFont);
            m_mapText.Text = "view map any time";
            m_mapText.Align = Types.TextAlign.Centre;
            m_mapText.FontSize = 12f;
            m_mapText.OutlineWidth = 2;
            GameObjList.Add(m_mapText);
            m_mapIcon = new KeyIconObj();
            m_mapIcon.Scale = new Vector2(0.5f, 0.5f);
            GameObjList.Add(m_mapIcon);
            foreach (var current in GameObjList)
            {
                if (current.Name == "diary")
                {
                    m_diary = (current as SpriteObj);
                }
                if (current.Name == "map")
                {
                    (current as SpriteObj).OutlineWidth = 2;
                    m_mapText.Position = new Vector2(current.X, current.Bounds.Top - 50);
                    m_mapIcon.Position = new Vector2(m_mapText.X, m_mapText.Y - 20f);
                }
            }
            m_diary.OutlineWidth = 2;
            m_speechBubble.Position = new Vector2(m_diary.X, m_diary.Y - m_speechBubble.Height - 20f);
            DoorObj doorObj = null;
            foreach (var current2 in GameObjList)
            {
                if (current2.Name == "LastDoor")
                {
                    m_bossDoorSprite = (current2 as ObjContainer);
                    break;
                }
            }
            foreach (var current3 in DoorList)
            {
                if (current3.DoorPosition == "Left")
                {
                    doorObj = current3;
                }
                if (current3.IsBossDoor)
                {
                    m_bossDoor = current3;
                    m_bossDoor.Locked = true;
                }
            }
            for (var i = 1; i < m_bossDoorSprite.NumChildren; i++)
            {
                m_bossDoorSprite.GetChildAt(i).Opacity = 0f;
            }
            m_bossDoorSprite.AnimationDelay = 0.1f;
            m_castleGate.Position = new Vector2(doorObj.Bounds.Right - m_castleGate.Width,
                doorObj.Y - m_castleGate.Height);
            m_teleporter.Position = new Vector2(X + Width/2f - 600f, Y + 720f - 120f);
            base.Initialize();
        }

        public void RevealSymbol(LevelType levelType, bool tween)
        {
            var flag = false;
            int index;
            switch (levelType)
            {
                case LevelType.Castle:
                    index = 1;
                    if (Game.PlayerStats.ChallengeEyeballBeaten)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Garden:
                    index = 3;
                    if (Game.PlayerStats.ChallengeSkullBeaten)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Dungeon:
                    index = 4;
                    if (Game.PlayerStats.ChallengeBlobBeaten)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Tower:
                    index = 2;
                    if (Game.PlayerStats.ChallengeFireballBeaten)
                    {
                        flag = true;
                    }
                    break;
                default:
                    index = 5;
                    if (Game.PlayerStats.ChallengeLastBossBeaten)
                    {
                        flag = true;
                    }
                    break;
            }
            if (flag)
            {
                m_bossDoorSprite.GetChildAt(index).TextureColor = Color.Yellow;
            }
            else
            {
                m_bossDoorSprite.GetChildAt(index).TextureColor = Color.White;
            }
            if (tween)
            {
                m_bossDoorSprite.GetChildAt(index).Opacity = 0f;
                Tween.To(m_bossDoorSprite.GetChildAt(index), 0.5f, Quad.EaseInOut, "delay", "1.5", "Opacity", "1");
                return;
            }
            m_bossDoorSprite.GetChildAt(index).Opacity = 1f;
        }

        public override void OnEnter()
        {
            m_bossDoorOpening = false;
            if (Game.PlayerStats.ReadLastDiary && LinkedRoom.LinkedRoom != null)
            {
                LinkedRoom = LinkedRoom.LinkedRoom;
            }
            Game.PlayerStats.LoadStartingRoom = false;
            if (Game.PlayerStats.DiaryEntry < 1)
            {
                m_speechBubble.Visible = true;
            }
            else
            {
                m_speechBubble.Visible = false;
            }
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                m_mapIcon.SetButton(Game.GlobalInput.ButtonList[9]);
                m_mapIcon.Scale = new Vector2(1f, 1f);
            }
            else
            {
                m_mapIcon.SetKey(Game.GlobalInput.KeyList[9]);
                m_mapIcon.Scale = new Vector2(0.5f, 0.5f);
            }
            if (!m_allFilesSaved)
            {
                Player.Game.SaveManager.SaveAllFileTypes(false);
                m_allFilesSaved = true;
            }
            if (Game.PlayerStats.EyeballBossBeaten)
            {
                RevealSymbol(LevelType.Castle, false);
            }
            if (Game.PlayerStats.FairyBossBeaten)
            {
                RevealSymbol(LevelType.Garden, false);
            }
            if (Game.PlayerStats.BlobBossBeaten)
            {
                RevealSymbol(LevelType.Dungeon, false);
            }
            if (Game.PlayerStats.FireballBossBeaten)
            {
                RevealSymbol(LevelType.Tower, false);
            }
            if (Game.PlayerStats.EyeballBossBeaten && Game.PlayerStats.FairyBossBeaten &&
                Game.PlayerStats.BlobBossBeaten && Game.PlayerStats.FireballBossBeaten &&
                !Game.PlayerStats.FinalDoorOpened && Player.ScaleX > 0.1f)
            {
                PlayBossDoorAnimation();
            }
            else if (Game.PlayerStats.FinalDoorOpened)
            {
                m_bossDoor.Locked = false;
                m_bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
                m_bossDoorSprite.GoToFrame(m_bossDoorSprite.TotalFrames);
            }
            if (!m_gateClosed)
            {
                CloseGate(true);
            }
            if (Game.PlayerStats.EyeballBossBeaten && Game.PlayerStats.FairyBossBeaten &&
                Game.PlayerStats.BlobBossBeaten && Game.PlayerStats.FireballBossBeaten &&
                !Game.PlayerStats.FinalDoorOpened && Player.ScaleX > 0.1f)
            {
                Game.PlayerStats.FinalDoorOpened = true;
                Player.AttachedLevel.RunCinematicBorders(6f);
            }
            base.OnEnter();
        }

        public void PlayBossDoorAnimation()
        {
            Player.StopDash();
            m_bossDoorOpening = true;
            m_bossDoor.Locked = false;
            Player.AttachedLevel.UpdateCamera();
            RevealSymbol(LevelType.None, true);
            Player.CurrentSpeed = 0f;
            Player.LockControls();
            Player.AttachedLevel.CameraLockedToPlayer = false;
            var x = Player.AttachedLevel.Camera.X;
            object arg_C7_0 = Player.AttachedLevel.Camera;
            var arg_C7_1 = 1f;
            Easing arg_C7_2 = Quad.EaseInOut;
            var array = new string[2];
            array[0] = "X";
            var arg_C5_0 = array;
            var arg_C5_1 = 1;
            var x2 = Bounds.Center.X;
            arg_C5_0[arg_C5_1] = x2.ToString();
            Tween.To(arg_C7_0, arg_C7_1, arg_C7_2, array);
            Tween.RunFunction(2.2f, this, "PlayBossDoorAnimation2", x);
        }

        public void PlayBossDoorAnimation2(float storedX)
        {
            m_bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
            m_bossDoorSprite.PlayAnimation(false);
            SoundManager.PlaySound("LastDoor_Open");
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "2", "X", storedX.ToString());
            Tween.RunFunction(3.1f, this, "BossDoorAnimationComplete");
        }

        public void BossDoorAnimationComplete()
        {
            m_bossDoorOpening = false;
            Player.UnlockControls();
            Player.AttachedLevel.CameraLockedToPlayer = true;
        }

        public void ForceGateClosed()
        {
            m_castleGate.Y += m_castleGate.Height;
            m_gateClosed = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_bossDoorOpening && !Player.ControlsLocked)
            {
                Player.LockControls();
            }
            if (!SoundManager.IsMusicPlaying)
            {
                SoundManager.PlayMusic("CastleSong", true);
            }
            if (Player.X < m_castleGate.Bounds.Right)
            {
                Player.X = m_castleGate.Bounds.Right + 20;
                Player.AttachedLevel.UpdateCamera();
            }
            var bounds = m_diary.Bounds;
            bounds.X -= 50;
            bounds.Width += 100;
            m_speechBubble.Y = m_diary.Y - m_speechBubble.Height - 20f - 30f +
                               (float) Math.Sin(Game.TotalGameTime*20f)*2f;
            if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
            {
                if (m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                {
                    m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");
                }
            }
            else if (m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
            {
                m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
            }
            if (Game.PlayerStats.DiaryEntry < 1 || CollisionMath.Intersects(Player.Bounds, bounds))
            {
                m_speechBubble.Visible = true;
            }
            else if (Game.PlayerStats.DiaryEntry >= 1 && !CollisionMath.Intersects(Player.Bounds, bounds))
            {
                m_speechBubble.Visible = false;
            }
            if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround &&
                (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
            {
                if (Game.PlayerStats.DiaryEntry < 1)
                {
                    var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                    rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry0");
                    rCScreenManager.DisplayScreen(13, true);
                    var expr_24E = Game.PlayerStats;
                    expr_24E.DiaryEntry += 1;
                }
                else
                {
                    var rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                    rCScreenManager2.DisplayScreen(20, true);
                }
            }
            base.Update(gameTime);
        }

        public void CloseGate(bool animate)
        {
            if (animate)
            {
                Player.Y = 381f;
                Player.X += 10f;
                Player.State = 1;
                var logicSet = new LogicSet(Player);
                logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
                logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
                logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
                logicSet.AddAction(new PlayAnimationLogicAction());
                logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 200));
                logicSet.AddAction(new DelayLogicAction(0.2f));
                logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
                Player.RunExternalLogicSet(logicSet);
                Tween.By(m_castleGate, 1.5f, Quad.EaseOut, "Y", m_castleGate.Height.ToString());
                Tween.AddEndHandlerToLastTween(Player, "UnlockControls");
                Player.AttachedLevel.RunCinematicBorders(1.5f);
            }
            else
            {
                m_castleGate.Y += m_castleGate.Height;
            }
            m_gateClosed = true;
        }

        public override void Reset()
        {
            if (m_gateClosed)
            {
                m_castleGate.Y -= m_castleGate.Height;
                m_gateClosed = false;
            }
            base.Reset();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_castleGate = null;
                m_teleporter = null;
                m_bossDoor = null;
                m_bossDoorSprite = null;
                m_diary = null;
                m_speechBubble = null;
                m_mapText = null;
                m_mapIcon = null;
                base.Dispose();
            }
        }

        protected override GameObj CreateCloneInstance()
        {
            return new CastleEntranceRoomObj();
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
        }
    }
}
