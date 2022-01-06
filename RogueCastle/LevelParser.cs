// 
// RogueLegacyArchipelago - LevelParser.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Enums;

namespace RogueCastle
{
    internal class LevelParser
    {
        private const int MAX_ROOM_SIZE = 4;
        private const int ROOM_HEIGHT = 720;
        private const int ROOM_WIDTH = 1320;

        public static void ParseRooms(string filePath, ContentManager contentManager = null, bool isDLCMap = false)
        {
            var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreWhitespace = true;
            XmlReader xmlReader;
            if (contentManager == null)
            {
                xmlReader = XmlReader.Create(filePath, xmlReaderSettings);
            }
            else
            {
                xmlReader = XmlReader.Create(contentManager.RootDirectory + "\\Levels\\" + filePath + ".xml",
                    xmlReaderSettings);
            }

            RoomObj roomObj = null;
            RoomObj roomObj2 = null;
            RoomObj roomObj3 = null;
            RoomObj roomObj4 = null;
            RoomObj roomObj5 = null;
            while (xmlReader.Read())
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "RoomObject")
                    {
                        roomObj = new RoomObj();
                        ParseGenericXML(xmlReader, roomObj);
                        if (isDLCMap)
                        {
                            roomObj.IsDLCMap = true;
                        }

                        roomObj2 = roomObj.Clone() as RoomObj;
                        roomObj3 = roomObj.Clone() as RoomObj;
                        roomObj4 = roomObj.Clone() as RoomObj;
                        roomObj5 = roomObj.Clone() as RoomObj;
                    }

                    if (xmlReader.Name == "GameObject")
                    {
                        xmlReader.MoveToAttribute("Type");
                        var value = xmlReader.Value;
                        GameObj gameObj = null;
                        string key;
                        switch (key = value)
                        {
                            case "CollHullObj":
                                gameObj = new TerrainObj(0, 0);
                                break;

                            case "DoorObj":
                                gameObj = new DoorObj(roomObj, 0, 0, Door.Open);
                                break;

                            case "ChestObj":
                                if (xmlReader.MoveToAttribute("Fairy"))
                                {
                                    if (bool.Parse(xmlReader.Value))
                                    {
                                        gameObj = new FairyChestObj(null);
                                        (gameObj as ChestObj).ChestType = 4;
                                    }
                                    else
                                    {
                                        gameObj = new ChestObj(null);
                                    }
                                }
                                else
                                {
                                    gameObj = new ChestObj(null);
                                }

                                break;

                            case "HazardObj":
                                gameObj = new HazardObj(0, 0);
                                break;

                            case "BorderObj":
                                gameObj = new BorderObj();
                                break;

                            case "EnemyObj":
                                xmlReader.MoveToAttribute("Procedural");
                                if (!bool.Parse(xmlReader.Value))
                                {
                                    xmlReader.MoveToAttribute("EnemyType");
                                    var enemyType = byte.Parse(xmlReader.Value, NumberStyles.Any, cultureInfo);
                                    xmlReader.MoveToAttribute("Difficulty");
                                    var difficulty =
                                        (EnemyDifficulty)
                                        Enum.Parse(typeof(EnemyDifficulty), xmlReader.Value, true);
                                    gameObj = EnemyBuilder.BuildEnemy(enemyType, null, null, null, difficulty);
                                    if (xmlReader.MoveToAttribute("Flip") && bool.Parse(xmlReader.Value))
                                    {
                                        gameObj.Flip = SpriteEffects.FlipHorizontally;
                                    }

                                    if (xmlReader.MoveToAttribute("InitialDelay"))
                                    {
                                        (gameObj as EnemyObj).InitialLogicDelay = float.Parse(xmlReader.Value,
                                            NumberStyles.Any, cultureInfo);
                                    }
                                }
                                else
                                {
                                    xmlReader.MoveToAttribute("EnemyType");
                                    var value2 = xmlReader.Value;
                                    gameObj = new EnemyTagObj();
                                    (gameObj as EnemyTagObj).EnemyType = value2;
                                }

                                break;

                            case "EnemyOrbObj":
                            {
                                xmlReader.MoveToAttribute("OrbType");
                                var orbType = int.Parse(xmlReader.Value, NumberStyles.Any, cultureInfo);
                                var flag = false;
                                if (xmlReader.MoveToAttribute("IsWaypoint"))
                                {
                                    flag = bool.Parse(xmlReader.Value);
                                }

                                if (flag)
                                {
                                    gameObj = new WaypointObj();
                                    (gameObj as WaypointObj).OrbType = orbType;
                                }
                                else
                                {
                                    gameObj = new EnemyOrbObj();
                                    (gameObj as EnemyOrbObj).OrbType = orbType;
                                    if (xmlReader.MoveToAttribute("ForceFlying"))
                                    {
                                        (gameObj as EnemyOrbObj).ForceFlying = bool.Parse(xmlReader.Value);
                                    }
                                }

                                break;
                            }

                            case "SpriteObj":
                                xmlReader.MoveToAttribute("SpriteName");
                                if (xmlReader.Value == "LightSource_Sprite")
                                {
                                    gameObj = new LightSourceObj();
                                }
                                else
                                {
                                    gameObj = new SpriteObj(xmlReader.Value);
                                }

                                break;

                            case "PhysicsObj":
                            {
                                xmlReader.MoveToAttribute("SpriteName");
                                gameObj = new PhysicsObj(xmlReader.Value);
                                var physicsObj = gameObj as PhysicsObj;
                                physicsObj.CollisionTypeTag = 5;
                                physicsObj.CollidesBottom = false;
                                physicsObj.CollidesLeft = false;
                                physicsObj.CollidesRight = false;
                                break;
                            }

                            case "PhysicsObjContainer":
                            {
                                var flag2 = false;
                                if (xmlReader.MoveToAttribute("Breakable"))
                                {
                                    flag2 = bool.Parse(xmlReader.Value);
                                }

                                xmlReader.MoveToAttribute("SpriteName");
                                if (flag2)
                                {
                                    gameObj = new BreakableObj(xmlReader.Value);
                                }
                                else
                                {
                                    gameObj = new PhysicsObjContainer(xmlReader.Value);
                                }

                                break;
                            }

                            case "ObjContainer":
                                xmlReader.MoveToAttribute("SpriteName");
                                gameObj = new ObjContainer(xmlReader.Value);
                                break;

                            case "PlayerStartObj":
                                gameObj = new PlayerStartObj();
                                break;
                        }

                        ParseGenericXML(xmlReader, gameObj);
                        var levelType = Zone.None;
                        if (xmlReader.MoveToAttribute("LevelType"))
                        {
                            levelType = (Zone) int.Parse(xmlReader.Value, NumberStyles.Any, cultureInfo);
                        }

                        if (levelType == Zone.Castle)
                        {
                            StoreObj(gameObj, roomObj2);
                            StoreSwappedObj(gameObj, Zone.Dungeon, roomObj3);
                            StoreSwappedObj(gameObj, Zone.Tower, roomObj5);
                            StoreSwappedObj(gameObj, Zone.Garden, roomObj4);
                            var spriteObj = gameObj as SpriteObj;
                            if (spriteObj != null && spriteObj.SpriteName == "CastleAssetFrame_Sprite")
                            {
                                spriteObj.ChangeSprite("FramePicture" + CDGMath.RandomInt(1, 16) + "_Sprite");
                            }
                        }
                        else if (levelType == Zone.Dungeon)
                        {
                            StoreObj(gameObj, roomObj3);
                        }
                        else if (levelType == Zone.Tower)
                        {
                            StoreObj(gameObj, roomObj5);
                        }
                        else if (levelType == Zone.Garden)
                        {
                            StoreObj(gameObj, roomObj4);
                        }
                        else
                        {
                            StoreObj(gameObj, roomObj2);
                            StoreObj(gameObj, roomObj3);
                            StoreObj(gameObj, roomObj5);
                            StoreObj(gameObj, roomObj4);
                            StoreObj(gameObj, roomObj);
                        }

                        if (LevelENV.RunTestRoom &&
                            (levelType == LevelENV.TestRoomZone || levelType == Zone.Castle))
                        {
                            if (levelType == LevelENV.TestRoomZone)
                            {
                                StoreObj(gameObj, roomObj);
                            }
                            else if (levelType == Zone.Castle)
                            {
                                StoreSwappedObj(gameObj, LevelENV.TestRoomZone, roomObj);
                            }
                        }

                        if (gameObj is PlayerStartObj)
                        {
                            var expr_65E = roomObj;
                            expr_65E.Name += "DEBUG_ROOM";
                        }
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "RoomObject")
                {
                    if (roomObj.X < 10000f && roomObj.Name != "Boss" && roomObj.Name != "ChallengeBoss")
                    {
                        if (!roomObj.Name.Contains("DEBUG_ROOM"))
                        {
                            if (roomObj.AddToCastlePool)
                            {
                                LevelBuilder2.StoreRoom(roomObj2, Zone.Castle);
                                LevelBuilder2.StoreSpecialRoom(roomObj2, Zone.Castle);
                            }

                            if (roomObj.AddToDungeonPool)
                            {
                                LevelBuilder2.StoreRoom(roomObj3, Zone.Dungeon);
                                LevelBuilder2.StoreSpecialRoom(roomObj3, Zone.Dungeon);
                            }

                            if (roomObj.AddToGardenPool)
                            {
                                LevelBuilder2.StoreRoom(roomObj4, Zone.Garden);
                                LevelBuilder2.StoreSpecialRoom(roomObj4, Zone.Garden);
                            }

                            if (roomObj.AddToTowerPool)
                            {
                                LevelBuilder2.StoreRoom(roomObj5, Zone.Tower);
                                LevelBuilder2.StoreSpecialRoom(roomObj5, Zone.Tower);
                            }
                        }

                        if (roomObj.Name.Contains("DEBUG_ROOM"))
                        {
                            roomObj.Name = roomObj.Name.Replace("DEBUG_ROOM", "");
                            if (LevelENV.TestRoomZone != Zone.Castle)
                            {
                                LevelBuilder2.StoreSpecialRoom(roomObj, Zone.Castle, true);
                            }

                            LevelBuilder2.StoreSpecialRoom(roomObj, LevelENV.TestRoomZone, true);
                        }
                    }

                    if (roomObj.X < 10000f && (roomObj.Name == "Boss" || roomObj.Name == "ChallengeBoss"))
                    {
                        LevelBuilder2.StoreSpecialRoom(roomObj, Zone.Castle);
                    }
                }
        }

        public static void ParseGenericXML(XmlReader reader, GameObj obj)
        {
            var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            obj.PopulateFromXMLReader(reader, cultureInfo);
            var flag = false;
            if (reader.MoveToAttribute("BGLayer"))
            {
                flag = bool.Parse(reader.Value);
            }

            if (flag)
            {
                obj.Layer = -1f;
            }

            var breakableObj = obj as BreakableObj;
            if (breakableObj != null)
            {
                breakableObj.IsCollidable = true;
            }
        }

        public static void StoreSwappedObj(GameObj obj, Zone zone, RoomObj currentRoom)
        {
            string[] array;
            switch (zone)
            {
                case Zone.Garden:
                    array = LevelENV.GardenAssetSwapList;
                    break;

                case Zone.Dungeon:
                    array = LevelENV.DungeonAssetSwapList;
                    break;

                case Zone.Tower:
                    array = LevelENV.TowerAssetSwapList;
                    break;

                default:
                    throw new Exception("Cannot find asset swaplist for leveltype " + zone);
            }

            var breakableObj = obj as BreakableObj;
            if (breakableObj != null && breakableObj.SpriteName.Contains("CastleAssetUrn"))
            {
                breakableObj.CollidesTop = false;
            }

            var flag = false;
            var animateableObj = obj.Clone() as IAnimateableObj;
            if (animateableObj != null)
            {
                var i = 0;
                while (i < LevelENV.CastleAssetSwapList.Length)
                {
                    if (animateableObj.SpriteName == LevelENV.CastleAssetSwapList[i])
                    {
                        var text = array[i];
                        if (text.Contains("RANDOM"))
                        {
                            var max = int.Parse(Convert.ToString(text[text.IndexOf("RANDOM") + 6]));
                            var num = CDGMath.RandomInt(1, max);
                            text = text.Replace("RANDOM" + max, num.ToString());
                            if (text.Contains("TowerHole"))
                            {
                                (animateableObj as GameObj).X += CDGMath.RandomInt(-50, 50);
                                (animateableObj as GameObj).Y += CDGMath.RandomInt(-50, 50);
                                if (CDGMath.RandomInt(1, 100) > 70)
                                {
                                    (animateableObj as GameObj).Visible = false;
                                }
                            }

                            if (text.Contains("GardenFloatingRock"))
                            {
                                animateableObj = new HoverObj(text)
                                {
                                    Position = (animateableObj as GameObj).Position,
                                    Amplitude = CDGMath.RandomFloat(-50f, 50f),
                                    HoverSpeed = CDGMath.RandomFloat(-2f, 2f),
                                    Scale = (animateableObj as GameObj).Scale,
                                    Layer = (animateableObj as GameObj).Layer
                                };
                            }
                        }

                        if (text == "CastleAssetFrame_Sprite")
                        {
                            text = "FramePicture" + CDGMath.RandomInt(1, 16) + "_Sprite";
                        }

                        if (!(text != ""))
                        {
                            break;
                        }

                        animateableObj.ChangeSprite(text);
                        flag = true;
                        if (text.Contains("GardenFairy"))
                        {
                            (animateableObj as GameObj).X += CDGMath.RandomInt(-25, 25);
                            (animateableObj as GameObj).Y += CDGMath.RandomInt(-25, 25);
                            (animateableObj as GameObj).Opacity = 0.8f;
                        }

                        break;
                    }

                    i++;
                }
            }

            if (flag)
            {
                StoreObj(animateableObj as GameObj, currentRoom);
            }
        }

        public static void StoreObj(GameObj obj, RoomObj currentRoom)
        {
            if (obj is EnemyObj)
            {
                currentRoom.EnemyList.Add(obj as EnemyObj);
                return;
            }

            if (obj is DoorObj)
            {
                currentRoom.DoorList.Add(obj as DoorObj);
                return;
            }

            if (obj is TerrainObj)
            {
                currentRoom.TerrainObjList.Add(obj as TerrainObj);
                return;
            }

            if (obj is BorderObj)
            {
                currentRoom.BorderList.Add(obj as BorderObj);
                return;
            }

            currentRoom.GameObjList.Add(obj);
        }
    }
}