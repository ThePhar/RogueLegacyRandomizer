// RogueLegacyRandomizer - SpecialItemRoomObj.cs
// Last Modified 2023-07-30 10:28 AM by
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;

namespace RogueLegacy
{
    public class SpecialItemRoomObj : BonusRoomObj
    {
        private SpriteObj m_icon;
        private float m_iconYPos;
        private SpriteObj m_pedestal;
        private SpriteObj m_speechBubble;
        public byte ItemType { get; set; }

        public override void Initialize()
        {
            m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
            m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
            m_icon = new SpriteObj("Blank_Sprite");
            m_icon.Scale = new Vector2(2f, 2f);
            foreach (var current in GameObjList)
                if (current.Name == "pedestal")
                {
                    m_pedestal = current as SpriteObj;
                    break;
                }

            m_pedestal.OutlineWidth = 2;
            m_icon.X = m_pedestal.X;
            m_icon.Y = m_pedestal.Y - (m_pedestal.Y - m_pedestal.Bounds.Top) - m_icon.Height / 2f - 10f;
            m_icon.OutlineWidth = 2;
            m_iconYPos = m_icon.Y;
            GameObjList.Add(m_icon);
            m_speechBubble.Y = m_icon.Y - 30f;
            m_speechBubble.X = m_icon.X;
            m_speechBubble.Visible = false;
            GameObjList.Add(m_speechBubble);
            base.Initialize();
        }

        private void RandomizeItem()
        {
            if (Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f || Game.PlayerStats.Traits.X == 4f ||
                Game.PlayerStats.Traits.Y == 4f || Game.PlayerStats.HasVertigo ||
                Game.PlayerStats.Traits.X == 1f || Game.PlayerStats.Traits.Y == 1f ||
                Game.PlayerStats.Traits.X == 29f ||
                Game.PlayerStats.Traits.Y == 29f)
            {
                if (CDGMath.RandomInt(1, 100) <= 40)
                {
                    ItemType = 8;
                }
                else
                {
                    ItemType = GetRandomItem();
                }
            }
            else
            {
                ItemType = GetRandomItem();
            }

            m_icon.ChangeSprite(((SpecialItemType) ItemType).SpriteName());
            ID = ItemType;
        }

        private byte GetRandomItem()
        {
            var list = new List<byte>();
            for (var i = 1; i < 7; i++) list.Add((byte) i);

            return list[CDGMath.RandomInt(0, list.Count - 1)];
        }

        public override void OnEnter()
        {
            m_icon.Visible = false;
            if (ID == -1 && !RoomCompleted)
            {
                do
                {
                    RandomizeItem();
                } while (ItemType == Game.PlayerStats.SpecialItem);
            }
            else if (ID != -1)
            {
                ItemType = (byte) ID;
                m_icon.ChangeSprite(((SpecialItemType) ItemType).SpriteName());
                if (RoomCompleted)
                {
                    m_icon.Visible = false;
                    m_speechBubble.Visible = false;
                }
            }

            if (RoomCompleted)
            {
                m_pedestal.TextureColor = new Color(100, 100, 100);
            }
            else
            {
                m_pedestal.TextureColor = Color.White;
            }

            base.OnEnter();
        }

        public override void Update(GameTime gameTime)
        {
            m_icon.Y = m_iconYPos + (float) Math.Sin(Game.TotalGameTimeSeconds * 2f) * 5f;
            m_speechBubble.Y = m_iconYPos - 30f + (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 2f;
            if (!RoomCompleted)
            {
                if (CollisionMath.Intersects(Player.Bounds, m_pedestal.Bounds))
                {
                    m_speechBubble.Visible = true;
                    if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                    {
                        var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                        rCScreenManager.DialogueScreen.SetDialogue("Special Item Prayer");
                        rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                        rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "TakeItem");
                        rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine",
                            "Canceling Selection");
                        rCScreenManager.DisplayScreen(13, true);
                    }
                }
                else
                {
                    m_speechBubble.Visible = false;
                }
            }
            else
            {
                m_icon.Visible = false;
            }

            base.Update(gameTime);
        }

        public void TakeItem()
        {
            RoomCompleted = true;
            Game.PlayerStats.SpecialItem = ItemType;
            m_pedestal.TextureColor = new Color(100, 100, 100);
            if (ItemType == 8)
            {
                Player.GetChildAt(10).Visible = true;
                Player.PlayAnimation();
            }
            else
            {
                Player.GetChildAt(10).Visible = false;
            }

            ItemType = 0;
            m_speechBubble.Visible = false;
            m_icon.Visible = false;
            (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerHUDSpecialItem();
            (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerSpellIcon();
            var list = new List<object>();
            list.Add(new Vector2(m_pedestal.X, m_pedestal.Y - m_pedestal.Height / 2f));
            list.Add(5);
            list.Add(new Vector2(Game.PlayerStats.SpecialItem, 0f));
            (Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData,
                SaveType.MapData);
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list);
            Tween.RunFunction(0f, Player, "RunGetItemAnimation");
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_pedestal = null;
                m_icon = null;
                m_speechBubble = null;
                base.Dispose();
            }
        }
    }
}