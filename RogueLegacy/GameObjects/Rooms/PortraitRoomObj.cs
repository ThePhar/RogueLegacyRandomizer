using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer.Definitions;

namespace RogueLegacy;

public class PortraitRoomObj : BonusRoomObj
{
    private SpriteObj m_portrait;
    private SpriteObj m_portraitFrame;
    private int       m_portraitIndex;

    public override void Initialize()
    {
        foreach (var current in GameObjList)
            if (current.Name == "portrait")
            {
                m_portraitFrame = current as SpriteObj;
                break;
            }

        m_portraitFrame.ChangeSprite("GiantPortrait_Sprite");
        m_portraitFrame.Scale = new Vector2(2f, 2f);
        m_portrait = new SpriteObj("Blank_Sprite");
        m_portrait.Position = m_portraitFrame.Position;
        m_portrait.Scale = new Vector2(0.95f, 0.95f);
        GameObjList.Add(m_portrait);
        base.Initialize();
    }

    public override void OnEnter()
    {
        if (!RoomCompleted && ID == -1)
        {
            m_portraitIndex = CDGMath.RandomInt(0, 7);
            m_portrait.ChangeSprite("Portrait" + m_portraitIndex + "_Sprite");
            ID = m_portraitIndex;
            base.OnEnter();
            return;
        }

        if (ID != -1)
        {
            m_portraitIndex = ID;
            m_portrait.ChangeSprite("Portrait" + m_portraitIndex + "_Sprite");
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
        {
            var b = new Rectangle(Bounds.Center.X - 100, Bounds.Bottom - 300, 200, 200);
            if (CollisionMath.Intersects(Player.Bounds, b) && Player.IsTouchingGround && ID > -1)
            {
                var screenManager = Game.ScreenManager;
                screenManager.DialogueScreen.SetDialogue("PortraitRoomText" + ID);
                screenManager.DisplayScreen(13, true);

                // Check location.
                Program.Game.CollectItemFromLocation(LocationCode.PORTRAIT_REWARD);
            }
        }

        base.Update(gameTime);
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            m_portraitFrame = null;
            m_portrait = null;
            base.Dispose();
        }
    }
}
