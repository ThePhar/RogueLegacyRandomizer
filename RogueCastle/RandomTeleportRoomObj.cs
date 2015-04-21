/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public class RandomTeleportRoomObj : BonusRoomObj
    {
        private TeleporterObj m_teleporter;

        public override void Initialize()
        {
            m_teleporter = new TeleporterObj();
            SpriteObj item = null;
            foreach (GameObj current in GameObjList)
            {
                if (current.Name == "teleporter")
                {
                    m_teleporter.Position = current.Position;
                    item = (current as SpriteObj);
                    break;
                }
            }
            GameObjList.Remove(item);
            GameObjList.Add(m_teleporter);
            m_teleporter.OutlineWidth = 2;
            m_teleporter.TextureColor = Color.PaleVioletRed;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (CollisionMath.Intersects(Player.Bounds, m_teleporter.Bounds) && Player.IsTouchingGround &&
                (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
            {
                TeleportPlayer();
            }
        }

        private void TeleportPlayer()
        {
            ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
            PlayerObj player = Game.ScreenManager.Player;
            player.UpdateCollisionBoxes();
            if (levelScreen != null)
            {
                Vector2 position = default(Vector2);
                int num = levelScreen.RoomList.Count - 1;
                if (num < 1)
                {
                    num = 1;
                }
                int index = CDGMath.RandomInt(0, num);
                RoomObj roomObj = levelScreen.RoomList[index];
                while (roomObj.Name == "Boss" || roomObj.Name == "Start" || roomObj.Name == "Ending" ||
                       roomObj.Name == "Compass" || roomObj.Name == "ChallengeBoss" || roomObj.Name == "Throne" ||
                       roomObj.Name == "Tutorial" || roomObj.Name == "EntranceBoss" || roomObj.Name == "Bonus" ||
                       roomObj.Name == "Linker" || roomObj.Name == "Secret" || roomObj.Name == "CastleEntrance" ||
                       roomObj.DoorList.Count < 2)
                {
                    index = CDGMath.RandomInt(0, num);
                    roomObj = levelScreen.RoomList[index];
                }
                foreach (DoorObj current in roomObj.DoorList)
                {
                    bool flag = false;
                    position.X = current.Bounds.Center.X;
                    string doorPosition;
                    if ((doorPosition = current.DoorPosition) != null)
                    {
                        if (!(doorPosition == "Left") && !(doorPosition == "Right"))
                        {
                            if (doorPosition == "Bottom")
                            {
                                position.Y = current.Bounds.Top - (player.Bounds.Bottom - player.Y);
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = true;
                            position.Y = current.Bounds.Bottom - (player.Bounds.Bottom - player.Y);
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                player.TeleportPlayer(position, m_teleporter);
            }
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_teleporter = null;
                base.Dispose();
            }
        }
    }
}