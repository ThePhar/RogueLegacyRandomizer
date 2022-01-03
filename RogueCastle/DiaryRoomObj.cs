//
// RogueLegacyArchipelago - DiaryRoomObj.cs
// Last Modified 2021-12-27
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
//
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using Archipelago;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;

namespace RogueCastle
{
    public class DiaryRoomObj : BonusRoomObj
    {
        private SpriteObj m_diary;
        private int m_diaryIndex;
        private SpriteObj m_speechBubble;

        public override void Initialize()
        {
            m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
            m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
            GameObjList.Add(m_speechBubble);
            foreach (var current in GameObjList)
                if (current.Name == "diary")
                {
                    m_diary = current as SpriteObj;
                    break;
                }

            m_diary.OutlineWidth = 2;
            m_speechBubble.Position = new Vector2(m_diary.X, m_diary.Y - m_speechBubble.Height - 20f);
            base.Initialize();
        }

        public override void OnEnter()
        {
            if (!RoomCompleted)
            {
                m_speechBubble.Visible = true;
            }
            else
            {
                m_speechBubble.Visible = false;
            }

            if (!RoomCompleted)
            {
                m_diaryIndex = Game.PlayerStats.DiaryEntry;
            }

            if (m_diaryIndex >= 24)
            {
                m_speechBubble.Visible = false;
            }

            base.OnEnter();
        }

        public override void Update(GameTime gameTime)
        {
            m_speechBubble.Y = m_diary.Y - m_speechBubble.Height - 20f - 30f +
                               (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 2f;
            var bounds = m_diary.Bounds;
            bounds.X -= 50;
            bounds.Width += 100;
            if (!CollisionMath.Intersects(Player.Bounds, bounds) && m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
            {
                m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
            }

            if (!RoomCompleted || CollisionMath.Intersects(Player.Bounds, bounds))
            {
                m_speechBubble.Visible = true;
            }
            else if (RoomCompleted && !CollisionMath.Intersects(Player.Bounds, bounds))
            {
                m_speechBubble.Visible = false;
            }

            if (m_diaryIndex >= 24)
            {
                m_speechBubble.Visible = false;
            }

            if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
            {
                if (m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                {
                    m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");
                }

                if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                {
                    if (!RoomCompleted && Game.PlayerStats.DiaryEntry < 24)
                    {
                        var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                        rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + m_diaryIndex);
                        rCScreenManager.DisplayScreen(13, true);

                        // Check location.
                        var location = LocationDefinitions.GetLocation(Program.Game.ArchipelagoManager.Data, string.Format("Diary {0}", m_diaryIndex + 1)).Code;
                        var networkItem = Program.Game.ArchipelagoManager.LocationCache[location];
                        Program.Game.ArchipelagoManager.CheckLocations(location);

                        // If we're sending someone else something, let's show what we're sending.
                        if (networkItem.Player != Program.Game.ArchipelagoManager.Data.Slot)
                        {
                            var item = new List<object>
                            {
                                new Vector2(Game.ScreenManager.Player.X, Game.ScreenManager.Player.Y - Height / 2f),
                                GetItemType.GiveNetworkItem,
                                new Vector2(-1f, -1f),
                                new Vector2(-1f, -1f),
                                Program.Game.ArchipelagoManager.GetPlayerName(networkItem.Player),
                                networkItem.Item
                            };
                            Game.ScreenManager.DisplayScreen(ScreenType.GetItem, true, item);
                            Game.ScreenManager.Player.RunGetItemAnimation();
                        }

                        Game.PlayerStats.DiaryEntry += 1;
                        RoomCompleted = true;
                    }
                    else
                    {
                        RoomCompleted = true;
                        var rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                        rCScreenManager2.DisplayScreen(20, true);
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_diary = null;
                m_speechBubble = null;
                base.Dispose();
            }
        }
    }
}
