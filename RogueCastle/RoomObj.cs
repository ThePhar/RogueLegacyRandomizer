/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;

namespace RogueCastle
{
    public class RoomObj : GameObj
    {
        public string BlueEnemyType;
        public string GreenEnemyType;
        private Vector2 m_debugRoomPosition;
        protected float m_doorSparkleDelay = 0.2f;
        private TextObj m_fairyChestText;
        private TextObj m_indexText;
        private SpriteObj m_pauseBG;
        private float m_roomActivityCounter;
        protected float m_roomActivityDelay = 0.2f;
        private TextObj m_roomInfoText;
        private int m_roomNumber = -1;
        public PlayerObj Player;
        public int PoolIndex = -1;
        public string RedEnemyType;

        public RoomObj()
        {
            GameObjList = new List<GameObj>();
            TerrainObjList = new List<TerrainObj>();
            DoorList = new List<DoorObj>();
            EnemyList = new List<EnemyObj>();
            BorderList = new List<BorderObj>();
            TempEnemyList = new List<EnemyObj>();
            LevelType = GameTypes.LevelType.NONE;
            m_indexText = new TextObj(Game.PixelArtFontBold);
            m_indexText.FontSize = 150f;
            m_indexText.Align = Types.TextAlign.Centre;
            m_roomInfoText = new TextObj(Game.PixelArtFontBold);
            m_roomInfoText.FontSize = 30f;
            m_roomInfoText.Align = Types.TextAlign.Centre;
            m_fairyChestText = new TextObj(Game.JunicodeFont);
            m_fairyChestText.FontSize = 26f;
            m_fairyChestText.Position = new Vector2(300f, 20f);
            m_fairyChestText.DropShadow = new Vector2(2f, 2f);
            m_fairyChestText.Text = "";
            m_pauseBG = new SpriteObj("Blank_Sprite");
            m_pauseBG.TextureColor = Color.Black;
            m_pauseBG.Opacity = 0f;
            IsReversed = false;
        }

        public bool AddToCastlePool { get; set; }
        public bool AddToGardenPool { get; set; }
        public bool AddToTowerPool { get; set; }
        public bool AddToDungeonPool { get; set; }
        public bool IsDLCMap { get; set; }
        public GameTypes.LevelType LevelType { get; set; }
        public int Level { get; set; }
        public List<DoorObj> DoorList { get; internal set; }
        public List<EnemyObj> EnemyList { get; internal set; }
        public List<TerrainObj> TerrainObjList { get; internal set; }
        public List<GameObj> GameObjList { get; internal set; }
        public List<BorderObj> BorderList { get; internal set; }
        public List<EnemyObj> TempEnemyList { get; internal set; }
        public bool IsReversed { get; internal set; }
        public RoomObj LinkedRoom { get; set; }

        public int RoomNumber
        {
            get { return m_roomNumber; }
            set
            {
                m_roomNumber = value;
                if (Name == "Linker")
                {
                    m_indexText.Text = "Linker " + m_roomNumber;
                    return;
                }
                if (Name == "EntranceBoss")
                {
                    m_indexText.Text = "Boss\nEnt. " + m_roomNumber;
                    return;
                }
                if (Name == "Boss")
                {
                    m_indexText.Text = "Boss " + m_roomNumber;
                    return;
                }
                if (Name == "Secret")
                {
                    m_indexText.Text = "Secret " + m_roomNumber;
                    return;
                }
                if (Name == "Bonus")
                {
                    m_indexText.Text = "Bonus " + m_roomNumber;
                    return;
                }
                if (Name == "Start")
                {
                    m_indexText.Text = "Starting Room";
                    return;
                }
                m_indexText.Text = m_roomNumber.ToString();
            }
        }

        public Vector2 DebugRoomPosition
        {
            get { return m_debugRoomPosition; }
            set
            {
                m_debugRoomPosition = value;
                m_roomInfoText.Text = string.Concat("Level Editor Pos: ", m_debugRoomPosition.ToString(), "\nReversed: ",
                    IsReversed);
            }
        }

        public RenderTarget2D BGRender { get; private set; }

        public bool HasFairyChest
        {
            get
            {
                foreach (var current in GameObjList)
                {
                    if (current is FairyChestObj)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int ActiveEnemies
        {
            get
            {
                var num = 0;
                foreach (var current in EnemyList)
                {
                    if (!current.NonKillable && !current.IsKilled)
                    {
                        num++;
                    }
                }
                foreach (var current2 in TempEnemyList)
                {
                    if (!current2.NonKillable && !current2.IsKilled)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual void LoadContent(GraphicsDevice graphics)
        {
        }

        public virtual void OnEnter()
        {
            m_roomActivityCounter = m_roomActivityDelay;
            foreach (var current in GameObjList)
            {
                var fairyChestObj = current as FairyChestObj;
                if (fairyChestObj != null && fairyChestObj.State == 2 && fairyChestObj.ConditionType != 10 &&
                    fairyChestObj.ConditionType != 0)
                {
                    Player.AttachedLevel.ObjectiveFailed();
                }
            }
        }

        public virtual void OnExit()
        {
            Player.AttachedLevel.ResetObjectivePlate(false);
            for (var i = 0; i < TempEnemyList.Count; i++)
            {
                if (TempEnemyList[i].IsDemented)
                {
                    Player.AttachedLevel.RemoveEnemyFromRoom(TempEnemyList[i], this);
                    i--;
                }
            }
        }

        public virtual void InitializeRenderTarget(RenderTarget2D bgRenderTarget)
        {
            BGRender = bgRenderTarget;
        }

        public void SetWidth(int width)
        {
            _width = width;
            m_pauseBG.Scale = Vector2.One;
            m_pauseBG.Scale = new Vector2((Width + 20)/m_pauseBG.Width, (Height + 20)/m_pauseBG.Height);
        }

        public void SetHeight(int height)
        {
            _height = height;
            m_pauseBG.Scale = Vector2.One;
            m_pauseBG.Scale = new Vector2((Width + 20)/m_pauseBG.Width, (Height + 20)/m_pauseBG.Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Name == "EntranceBoss" && LinkedRoom is ChallengeBossRoomObj)
            {
                if (m_doorSparkleDelay <= 0f)
                {
                    m_doorSparkleDelay = 0.1f;
                    Player.AttachedLevel.ImpactEffectPool.DoorSparkleEffect(new Rectangle((int) X + 590, (int) Y + 140,
                        190, 150));
                }
                else
                {
                    m_doorSparkleDelay -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (m_roomActivityCounter <= 0f)
            {
                foreach (var current in EnemyList)
                {
                    if (!current.IsKilled)
                    {
                        current.Update(gameTime);
                    }
                }
                using (var enumerator2 = TempEnemyList.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        var current2 = enumerator2.Current;
                        if (!current2.IsKilled)
                        {
                            current2.Update(gameTime);
                        }
                    }
                    return;
                }
            }
            m_roomActivityCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void DrawRenderTargets(Camera2D camera)
        {
            camera.GraphicsDevice.SetRenderTarget(BGRender);
            camera.GraphicsDevice.Clear(Color.White);
            foreach (var current in TerrainObjList)
            {
                if (current.Visible && current.Height > 40 && current.ShowTerrain)
                {
                    current.ForceDraw = true;
                    current.TextureColor = Color.Black;
                    current.Draw(camera);
                    current.ForceDraw = false;
                    current.TextureColor = Color.White;
                }
            }
        }

        public void DrawBGObjs(Camera2D camera)
        {
            foreach (var current in GameObjList)
            {
                if (current.Layer == -1f)
                {
                    current.Draw(camera);
                }
            }
        }

        public override void Draw(Camera2D camera)
        {
            if (camera.Zoom != 1f)
            {
                foreach (var current in TerrainObjList)
                {
                    current.Draw(camera);
                }
            }
            foreach (var current2 in BorderList)
            {
                current2.Draw(camera);
            }
            foreach (var current3 in BorderList)
            {
                if (current3.Rotation == 0f)
                {
                    current3.DrawCorners(camera);
                }
            }
            foreach (var current4 in GameObjList)
            {
                if (current4.Layer == 0f)
                {
                    current4.Draw(camera);
                }
            }
            foreach (var current5 in GameObjList)
            {
                if (current5.Layer == 1f)
                {
                    current5.Draw(camera);
                }
            }
            m_pauseBG.Position = Position;
            m_pauseBG.Draw(camera);
            foreach (var current6 in EnemyList)
            {
                current6.Draw(camera);
            }
            foreach (var current7 in TempEnemyList)
            {
                current7.Draw(camera);
            }
            foreach (var current8 in DoorList)
            {
                current8.Draw(camera);
            }
            if (LevelEV.SHOW_DEBUG_TEXT)
            {
                m_indexText.Position = new Vector2(Position.X + Width/2, Position.Y + Height/2 - m_indexText.Height/2);
                m_indexText.Draw(camera);
                m_roomInfoText.Position = new Vector2(Position.X + Width/2, Position.Y);
                m_roomInfoText.Draw(camera);
            }
            m_fairyChestText.Draw(camera);
        }

        public virtual void Reset()
        {
            var count = EnemyList.Count;
            for (var i = 0; i < count; i++)
            {
                EnemyList[i].Reset();
                if (count != EnemyList.Count)
                {
                    i--;
                    count = EnemyList.Count;
                }
            }
            for (var j = 0; j < TempEnemyList.Count; j++)
            {
                TempEnemyList[j].Reset();
                j--;
            }
        }

        public void Reverse()
        {
            IsReversed = true;
            var num = X + Width/2;
            foreach (var current in TerrainObjList)
            {
                if (current.Name == "Left")
                {
                    current.Name = "Right";
                }
                else if (current.Name == "Right")
                {
                    current.Name = "Left";
                }
                else if (current.Name == "!Left")
                {
                    current.Name = "!Right";
                }
                else if (current.Name == "!Right")
                {
                    current.Name = "!Left";
                }
                else if (current.Name == "!RightTop")
                {
                    current.Name = "!LeftTop";
                }
                else if (current.Name == "!RightBottom")
                {
                    current.Name = "!LeftBottom";
                }
                else if (current.Name == "!LeftTop")
                {
                    current.Name = "!RightTop";
                }
                else if (current.Name == "!LeftBottom")
                {
                    current.Name = "!RightBottom";
                }
                else if (current.Name == "RightTop")
                {
                    current.Name = "LeftTop";
                }
                else if (current.Name == "RightBottom")
                {
                    current.Name = "LeftBottom";
                }
                else if (current.Name == "LeftTop")
                {
                    current.Name = "RightTop";
                }
                else if (current.Name == "LeftBottom")
                {
                    current.Name = "RightBottom";
                }
                else if (current.Name == "!BottomLeft")
                {
                    current.Name = "!BottomRight";
                }
                else if (current.Name == "!BottomRight")
                {
                    current.Name = "!BottomLeft";
                }
                else if (current.Name == "!TopLeft")
                {
                    current.Name = "!TopRight";
                }
                else if (current.Name == "!TopRight")
                {
                    current.Name = "!TopLeft";
                }
                else if (current.Name == "BottomLeft")
                {
                    current.Name = "BottomRight";
                }
                else if (current.Name == "BottomRight")
                {
                    current.Name = "BottomLeft";
                }
                else if (current.Name == "TopLeft")
                {
                    current.Name = "TopRight";
                }
                else if (current.Name == "TopRight")
                {
                    current.Name = "TopLeft";
                }
                var num2 = current.X - num;
                if (num2 >= 0f)
                {
                    if (current.Rotation == 0f)
                    {
                        current.X -= num2*2f + current.Width;
                    }
                    else
                    {
                        var position =
                            CollisionMath.UpperRightCorner(
                                new Rectangle((int) current.X, (int) current.Y, current.Width, current.Height),
                                current.Rotation, Vector2.Zero);
                        position.X -= (position.X - num)*2f;
                        current.Position = position;
                        current.Rotation = -current.Rotation;
                    }
                }
                else if (current.Rotation == 0f)
                {
                    current.X = num - num2 - current.Width;
                }
                else
                {
                    var position2 =
                        CollisionMath.UpperRightCorner(
                            new Rectangle((int) current.X, (int) current.Y, current.Width, current.Height),
                            current.Rotation, Vector2.Zero);
                    position2.X += (num - position2.X)*2f;
                    current.Position = position2;
                    current.Rotation = -current.Rotation;
                }
            }
            foreach (var current2 in GameObjList)
            {
                ReverseObjNames(current2);
                if (!(current2 is HazardObj) && !(current2 is ChestObj))
                {
                    if (current2.Flip == SpriteEffects.None)
                    {
                        current2.Flip = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        current2.Flip = SpriteEffects.None;
                    }
                    var num3 = current2.X - X;
                    current2.X = Bounds.Right - num3;
                    if (current2.Rotation != 0f)
                    {
                        current2.Rotation = -current2.Rotation;
                    }
                }
                else
                {
                    if (current2 is ChestObj)
                    {
                        if (current2.Flip == SpriteEffects.None)
                        {
                            current2.Flip = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            current2.Flip = SpriteEffects.None;
                        }
                    }
                    var num4 = current2.X - num;
                    if (num4 >= 0f)
                    {
                        if (current2.Rotation == 0f)
                        {
                            current2.X -= num4*2f + current2.Width;
                        }
                        else
                        {
                            var position3 =
                                CollisionMath.UpperRightCorner(
                                    new Rectangle((int) current2.X, (int) current2.Y, current2.Width, current2.Height),
                                    current2.Rotation, Vector2.Zero);
                            position3.X -= (position3.X - num)*2f;
                            current2.Position = position3;
                            current2.Rotation = -current2.Rotation;
                        }
                    }
                    else if (current2.Rotation == 0f)
                    {
                        current2.X = num - num4 - current2.Width;
                    }
                    else
                    {
                        var position4 =
                            CollisionMath.UpperRightCorner(
                                new Rectangle((int) current2.X, (int) current2.Y, current2.Width, current2.Height),
                                current2.Rotation, Vector2.Zero);
                        position4.X += (num - position4.X)*2f;
                        current2.Position = position4;
                        current2.Rotation = -current2.Rotation;
                    }
                }
            }
            foreach (var current3 in EnemyList)
            {
                if (current3.Flip == SpriteEffects.None)
                {
                    current3.Flip = SpriteEffects.FlipHorizontally;
                    current3.InternalFlip = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    current3.Flip = SpriteEffects.None;
                    current3.InternalFlip = SpriteEffects.None;
                }
                ReverseObjNames(current3);
                var num5 = current3.X - num;
                if (num5 >= 0f)
                {
                    current3.X -= num5*2f;
                    current3.Rotation = -current3.Rotation;
                }
                else
                {
                    current3.X = num - num5;
                    current3.Rotation = -current3.Rotation;
                }
            }
            foreach (var current4 in BorderList)
            {
                ReverseObjNames(current4);
                var num6 = current4.X - num;
                if (num6 >= 0f)
                {
                    if (current4.Rotation == 0f)
                    {
                        current4.X -= num6*2f + current4.Width;
                    }
                    else
                    {
                        var position5 =
                            CollisionMath.UpperRightCorner(
                                new Rectangle((int) current4.X, (int) current4.Y, current4.Width, current4.Height),
                                current4.Rotation, Vector2.Zero);
                        position5.X -= (position5.X - num)*2f;
                        current4.Position = position5;
                        current4.Rotation = -current4.Rotation;
                    }
                }
                else if (current4.Rotation == 0f)
                {
                    current4.X = num - num6 - current4.Width;
                }
                else
                {
                    var position6 =
                        CollisionMath.UpperRightCorner(
                            new Rectangle((int) current4.X, (int) current4.Y, current4.Width, current4.Height),
                            current4.Rotation, Vector2.Zero);
                    position6.X += (num - position6.X)*2f;
                    current4.Position = position6;
                    current4.Rotation = -current4.Rotation;
                }
                if (current4.BorderRight)
                {
                    if (!current4.BorderLeft)
                    {
                        current4.BorderRight = false;
                    }
                    current4.BorderLeft = true;
                }
                else if (current4.BorderLeft)
                {
                    if (!current4.BorderRight)
                    {
                        current4.BorderLeft = false;
                    }
                    current4.BorderRight = true;
                }
            }
            foreach (var current5 in DoorList)
            {
                if (current5.DoorPosition == "Left")
                {
                    current5.X = X + Width - current5.Width;
                    current5.DoorPosition = "Right";
                }
                else if (current5.DoorPosition == "Right")
                {
                    current5.X = X;
                    current5.DoorPosition = "Left";
                }
                else if (current5.DoorPosition == "Top")
                {
                    var num7 = current5.X - num;
                    if (num7 >= 0f)
                    {
                        current5.X -= num7*2f + current5.Width;
                    }
                    else
                    {
                        current5.X = num - num7 - current5.Width;
                    }
                }
                else if (current5.DoorPosition == "Bottom")
                {
                    var num8 = current5.X - num;
                    if (num8 >= 0f)
                    {
                        current5.X -= num8*2f + current5.Width;
                    }
                    else
                    {
                        current5.X = num - num8 - current5.Width;
                    }
                }
            }
        }

        private void ReverseObjNames(GameObj obj)
        {
            if (obj.Name == "Left")
            {
                obj.Name = "Right";
                return;
            }
            if (obj.Name == "Right")
            {
                obj.Name = "Left";
                return;
            }
            if (obj.Name == "!Left")
            {
                obj.Name = "!Right";
                return;
            }
            if (obj.Name == "!Right")
            {
                obj.Name = "!Left";
                return;
            }
            if (obj.Name == "!RightTop")
            {
                obj.Name = "!LeftTop";
                return;
            }
            if (obj.Name == "!RightBottom")
            {
                obj.Name = "!LeftBottom";
                return;
            }
            if (obj.Name == "!LeftTop")
            {
                obj.Name = "!RightTop";
                return;
            }
            if (obj.Name == "!LeftBottom")
            {
                obj.Name = "!RightBottom";
                return;
            }
            if (obj.Name == "RightTop")
            {
                obj.Name = "LeftTop";
                return;
            }
            if (obj.Name == "RightBottom")
            {
                obj.Name = "LeftBottom";
                return;
            }
            if (obj.Name == "LeftTop")
            {
                obj.Name = "RightTop";
                return;
            }
            if (obj.Name == "LeftBottom")
            {
                obj.Name = "RightBottom";
                return;
            }
            if (obj.Name == "!BottomLeft")
            {
                obj.Name = "!BottomRight";
                return;
            }
            if (obj.Name == "!BottomRight")
            {
                obj.Name = "!BottomLeft";
                return;
            }
            if (obj.Name == "!TopLeft")
            {
                obj.Name = "!TopRight";
                return;
            }
            if (obj.Name == "!TopRight")
            {
                obj.Name = "!TopLeft";
                return;
            }
            if (obj.Name == "BottomLeft")
            {
                obj.Name = "BottomRight";
                return;
            }
            if (obj.Name == "BottomRight")
            {
                obj.Name = "BottomLeft";
                return;
            }
            if (obj.Name == "TopLeft")
            {
                obj.Name = "TopRight";
                return;
            }
            if (obj.Name == "TopRight")
            {
                obj.Name = "TopLeft";
            }
        }

        protected override GameObj CreateCloneInstance()
        {
            return new RoomObj();
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var roomObj = obj as RoomObj;
            roomObj.SetWidth(_width);
            roomObj.SetHeight(_height);
            foreach (var current in TerrainObjList)
            {
                roomObj.TerrainObjList.Add(current.Clone() as TerrainObj);
            }
            foreach (var current2 in GameObjList)
            {
                roomObj.GameObjList.Add(current2.Clone() as GameObj);
            }
            foreach (var current3 in DoorList)
            {
                var doorObj = current3.Clone() as DoorObj;
                doorObj.Room = roomObj;
                roomObj.DoorList.Add(doorObj);
            }
            foreach (var current4 in EnemyList)
            {
                roomObj.EnemyList.Add(current4.Clone() as EnemyObj);
            }
            foreach (var current5 in BorderList)
            {
                roomObj.BorderList.Add(current5.Clone() as BorderObj);
            }
            roomObj.AddToCastlePool = AddToCastlePool;
            roomObj.AddToGardenPool = AddToGardenPool;
            roomObj.AddToDungeonPool = AddToDungeonPool;
            roomObj.AddToTowerPool = AddToTowerPool;
            roomObj.PoolIndex = PoolIndex;
            roomObj.RedEnemyType = RedEnemyType;
            roomObj.BlueEnemyType = BlueEnemyType;
            roomObj.GreenEnemyType = GreenEnemyType;
            roomObj.LinkedRoom = LinkedRoom;
            roomObj.IsReversed = IsReversed;
            roomObj.DebugRoomPosition = DebugRoomPosition;
            roomObj.LevelType = LevelType;
            roomObj.Level = Level;
            roomObj.IsDLCMap = IsDLCMap;
        }

        public void DisplayFairyChestInfo()
        {
            FairyChestObj fairyChestObj = null;
            foreach (var current in GameObjList)
            {
                if (current is FairyChestObj)
                {
                    fairyChestObj = (current as FairyChestObj);
                }
                var arg_2D_0 = current as WaypointObj;
            }
            if (fairyChestObj != null && !fairyChestObj.IsOpen && fairyChestObj.ConditionType != 0 &&
                fairyChestObj.ConditionType != 10 && fairyChestObj.State != 1)
            {
                Player.AttachedLevel.DisplayObjective("Fairy Chest Objective",
                    DialogueManager.GetText("Chest_Locked " + fairyChestObj.ConditionType).Speakers[0],
                    DialogueManager.GetText("Chest_Locked " + fairyChestObj.ConditionType).Dialogue[0], true);
                return;
            }
            m_fairyChestText.Text = "";
        }

        public void DarkenRoom()
        {
            Tween.To(m_pauseBG, 0.1f, Tween.EaseNone, "Opacity", "0.7");
        }

        public virtual void PauseRoom()
        {
            foreach (var current in GameObjList)
            {
                var animateableObj = current as IAnimateableObj;
                if (animateableObj != null)
                {
                    animateableObj.PauseAnimation();
                }
                var textObj = current as TextObj;
                if (textObj != null && textObj.IsTypewriting)
                {
                    textObj.PauseTypewriting();
                }
            }
            foreach (var current2 in EnemyList)
            {
                current2.PauseAnimation();
            }
            foreach (var current3 in TempEnemyList)
            {
                current3.PauseAnimation();
            }
        }

        public virtual void UnpauseRoom()
        {
            Tween.To(m_pauseBG, 0.1f, Tween.EaseNone, "Opacity", "0");
            foreach (var current in GameObjList)
            {
                var animateableObj = current as IAnimateableObj;
                if (animateableObj != null)
                {
                    animateableObj.ResumeAnimation();
                }
                var textObj = current as TextObj;
                if (textObj != null && textObj.IsTypewriting)
                {
                    textObj.ResumeTypewriting();
                }
            }
            foreach (var current2 in EnemyList)
            {
                current2.ResumeAnimation();
            }
            foreach (var current3 in TempEnemyList)
            {
                current3.ResumeAnimation();
            }
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (var current in DoorList)
                {
                    current.Dispose();
                }
                DoorList.Clear();
                DoorList = null;
                foreach (var current2 in TerrainObjList)
                {
                    current2.Dispose();
                }
                TerrainObjList.Clear();
                TerrainObjList = null;
                foreach (var current3 in GameObjList)
                {
                    current3.Dispose();
                }
                GameObjList.Clear();
                GameObjList = null;
                foreach (var current4 in EnemyList)
                {
                    current4.Dispose();
                }
                EnemyList.Clear();
                EnemyList = null;
                foreach (var current5 in BorderList)
                {
                    current5.Dispose();
                }
                BorderList.Clear();
                BorderList = null;
                BGRender = null;
                LinkedRoom = null;
                foreach (var current6 in TempEnemyList)
                {
                    current6.Dispose();
                }
                TempEnemyList.Clear();
                TempEnemyList = null;
                Player = null;
                m_fairyChestText.Dispose();
                m_fairyChestText = null;
                m_pauseBG.Dispose();
                m_pauseBG = null;
                m_indexText.Dispose();
                m_indexText = null;
                m_roomInfoText.Dispose();
                m_roomInfoText = null;
                base.Dispose();
            }
        }

        public void CopyRoomProperties(RoomObj room)
        {
            Name = room.Name;
            SetWidth(room.Width);
            SetHeight(room.Height);
            Position = room.Position;
            AddToCastlePool = room.AddToCastlePool;
            AddToGardenPool = room.AddToGardenPool;
            AddToDungeonPool = room.AddToDungeonPool;
            AddToTowerPool = room.AddToTowerPool;
            PoolIndex = room.PoolIndex;
            RedEnemyType = room.RedEnemyType;
            BlueEnemyType = room.BlueEnemyType;
            GreenEnemyType = room.GreenEnemyType;
            LinkedRoom = room.LinkedRoom;
            IsReversed = room.IsReversed;
            DebugRoomPosition = room.DebugRoomPosition;
            Tag = room.Tag;
            LevelType = room.LevelType;
            IsDLCMap = room.IsDLCMap;
            TextureColor = room.TextureColor;
        }

        public void CopyRoomObjects(RoomObj room)
        {
            foreach (var current in room.TerrainObjList)
            {
                TerrainObjList.Add(current.Clone() as TerrainObj);
            }
            foreach (var current2 in room.GameObjList)
            {
                GameObjList.Add(current2.Clone() as GameObj);
            }
            foreach (var current3 in room.DoorList)
            {
                var doorObj = current3.Clone() as DoorObj;
                doorObj.Room = this;
                DoorList.Add(doorObj);
            }
            foreach (var current4 in room.EnemyList)
            {
                EnemyList.Add(current4.Clone() as EnemyObj);
            }
            foreach (var current5 in room.BorderList)
            {
                BorderList.Add(current5.Clone() as BorderObj);
            }
        }

        public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
        {
            base.PopulateFromXMLReader(reader, ci);
            SetWidth(_width);
            SetHeight(_height);
            if (reader.MoveToAttribute("CastlePool"))
            {
                AddToCastlePool = bool.Parse(reader.Value);
            }
            if (reader.MoveToAttribute("GardenPool"))
            {
                AddToGardenPool = bool.Parse(reader.Value);
            }
            if (reader.MoveToAttribute("TowerPool"))
            {
                AddToTowerPool = bool.Parse(reader.Value);
            }
            if (reader.MoveToAttribute("DungeonPool"))
            {
                AddToDungeonPool = bool.Parse(reader.Value);
            }
            DebugRoomPosition = Position;
        }
    }
}