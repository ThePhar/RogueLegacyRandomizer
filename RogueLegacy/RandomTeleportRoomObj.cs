// Rogue Legacy Randomizer - RandomTeleportRoomObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public class RandomTeleportRoomObj : BonusRoomObj
    {
        private TeleporterObj m_teleporter;

        public override void Initialize()
        {
            m_teleporter = new TeleporterObj();
            SpriteObj item = null;
            foreach (var current in GameObjList)
                if (current.Name == "teleporter")
                {
                    m_teleporter.Position = current.Position;
                    item = current as SpriteObj;
                    break;
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
            var levelScreen = Game.ScreenManager.GetLevelScreen();
            var player = Game.ScreenManager.Player;
            player.UpdateCollisionBoxes();
            if (levelScreen != null)
            {
                var position = default(Vector2);
                var num = levelScreen.RoomList.Count - 1;
                if (num < 1)
                {
                    num = 1;
                }

                var index = CDGMath.RandomInt(0, num);
                var roomObj = levelScreen.RoomList[index];
                while (roomObj.Name == "Boss" || roomObj.Name == "Start" || roomObj.Name == "Ending" ||
                       roomObj.Name == "Compass" || roomObj.Name == "ChallengeBoss" || roomObj.Name == "Throne" ||
                       roomObj.Name == "Tutorial" || roomObj.Name == "EntranceBoss" || roomObj.Name == "Bonus" ||
                       roomObj.Name == "Linker" || roomObj.Name == "Secret" || roomObj.Name == "CastleEntrance" ||
                       roomObj.DoorList.Count < 2)
                {
                    index = CDGMath.RandomInt(0, num);
                    roomObj = levelScreen.RoomList[index];
                }

                foreach (var current in roomObj.DoorList)
                {
                    var flag = false;
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
