// RogueLegacyRandomizer - DiaryRoomObj.cs
// Last Modified 2023-07-30 8:32 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using Randomizer.Definitions;

namespace RogueLegacy;

public class DiaryRoomObj : BonusRoomObj
{
    private SpriteObj m_diary;
    private int       m_diaryIndex;
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
            m_speechBubble.Visible = true;
        else
            m_speechBubble.Visible = false;

        if (!RoomCompleted) m_diaryIndex = Game.PlayerStats.DiaryEntry;

        if (m_diaryIndex >= 24) m_speechBubble.Visible = false;

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
            m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");

        if (!RoomCompleted || CollisionMath.Intersects(Player.Bounds, bounds))
            m_speechBubble.Visible = true;
        else if (RoomCompleted && !CollisionMath.Intersects(Player.Bounds, bounds)) m_speechBubble.Visible = false;

        if (m_diaryIndex >= 25) m_speechBubble.Visible = false;

        if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
        {
            if (m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");

            if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
            {
                if (!RoomCompleted && Game.PlayerStats.DiaryEntry < 25)
                {
                    // We're going to set diaries based on lower ones completed.
                    Game.PlayerStats.DiaryEntry = 0;

                    for (var diary = 0; diary < LocationCode.MAX_DIARIES; diary++)
                    {
                        var location = LocationCode.STARTING_DIARY + diary;

                        // Check if we already checked this location and try to get the next item in the sequence if so.
                        if (ArchipelagoManager.IsLocationChecked(location)) continue;

                        Game.PlayerStats.DiaryEntry = (byte) (diary + 1);
                        Program.Game.CollectItemFromLocation(location);
                        RoomCompleted = true;
                        break;
                    }
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