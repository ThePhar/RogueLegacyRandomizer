using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class ChestBonusRoomObj : BonusRoomObj
{
    private const int            TOTAL_UNLOCKED_CHESTS = 1;
    private       List<ChestObj> m_chestList;
    private       NpcObj         m_elf;
    private       PhysicsObj     m_gate;
    private       bool           m_paid;

    public ChestBonusRoomObj()
    {
        m_chestList = new List<ChestObj>();
        m_elf = new NpcObj("Elf_Character");
        m_elf.Scale = new Vector2(2f, 2f);
        m_gate = new PhysicsObj("CastleEntranceGate_Sprite");
        m_gate.IsWeighted = false;
        m_gate.IsCollidable = true;
        m_gate.CollisionTypeTag = 1;
        m_gate.Layer = -1f;
    }

    private int NumberOfChestsOpen
    {
        get
        {
            var num = 0;
            foreach (var current in m_chestList)
                if (current.IsOpen)
                    num++;

            return num;
        }
    }

    public override void Initialize()
    {
        var vector = Vector2.Zero;
        var zero = Vector2.Zero;
        foreach (var current in GameObjList)
            if (current is WaypointObj)
                zero.X = current.X;

        foreach (var current2 in TerrainObjList)
        {
            if (current2.Name == "GatePosition") vector = new Vector2(current2.X, current2.Bounds.Bottom);

            if (current2.Name == "Floor") zero.Y = current2.Y;
        }

        m_gate.Position = new Vector2(vector.X, vector.Y);
        if (!IsReversed) m_elf.Flip = SpriteEffects.FlipHorizontally;

        m_elf.Position = new Vector2(zero.X, zero.Y - (m_elf.Bounds.Bottom - m_elf.AnchorY) - 2f);
        GameObjList.Add(m_elf);
        GameObjList.Add(m_gate);
        base.Initialize();
    }

    public override void OnEnter()
    {
        ID = 1;
        foreach (var current in GameObjList)
        {
            var chestObj = current as ChestObj;
            if (chestObj != null)
            {
                chestObj.ChestType = ChestType.Silver;
                chestObj.IsEmpty = true;
                chestObj.IsLocked = true;
            }
        }

        (m_elf.GetChildAt(2) as SpriteObj).StopAnimation();
        base.OnEnter();
    }

    private void ShuffleChests(int goldPaid)
    {
        int[] array =
        {
            1,
            2,
            3
        };
        CDGMath.Shuffle(array);
        var num = 0;
        foreach (var current in GameObjList)
            if (current is ChestObj chestObj)
            {
                chestObj.ForcedItemType = ItemDropType.Coin;
                var num2 = array[num];
                if (num2 == 1)
                {
                    chestObj.IsEmpty = true;
                }
                else if (num2 == 2)
                {
                    chestObj.IsEmpty = true;
                }
                else
                {
                    chestObj.IsEmpty = false;
                    chestObj.ForcedAmount = goldPaid * 3f;
                }

                num++;
                m_chestList.Add(chestObj);
                chestObj.IsLocked = false;
                chestObj.TextureColor = Color.White;
                if (num2 == 3 && Game.PlayerStats.SpecialItem == 8) chestObj.TextureColor = Color.Gold;
            }
    }

    public override void Update(GameTime gameTime)
    {
        m_elf.Update(gameTime, Player);
        if (!RoomCompleted)
        {
            if (m_paid)
            {
                if (!IsReversed && Player.X < X + 50f)
                    Player.X = X + 50f;
                else if (IsReversed && Player.X > X + Width - 50f) Player.X = X + Width - 50f;
            }

            if (NumberOfChestsOpen >= 1)
            {
                var flag = false;
                foreach (var current in m_chestList)
                {
                    if (current.IsEmpty && current.IsOpen) flag = true;

                    current.IsLocked = true;
                }

                RoomCompleted = true;
                var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                if (!flag ||
                    !Program.Game.ArchipelagoManager.RandomizerData.CheckedLocations[LocationCode.CHEST_GAME_REWARD])
                {
                    rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-Won");

                    // Check location.
                    Program.Game.CollectItemFromLocation(LocationCode.CHEST_GAME_REWARD);
                    Game.ScreenManager.DisplayScreen(13, true);
                }
                else
                {
                    rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-Lost");
                    Game.ScreenManager.DisplayScreen(13, true);
                }
            }
        }

        HandleInput();
        base.Update(gameTime);
    }

    private void HandleInput()
    {
        if (m_elf.IsTouching && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
        {
            var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
            if (!RoomCompleted)
            {
                if (!m_paid)
                {
                    rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom" + ID + "-Start");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "PlayChestGame");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine",
                        "Canceling Selection");
                }
                else
                {
                    rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-Choose");
                }
            }
            else
            {
                rCScreenManager.DialogueScreen.SetDialogue("ChestBonusRoom1-End");
            }

            Game.ScreenManager.DisplayScreen(13, true);
        }
    }

    public void PlayChestGame()
    {
        if (Game.PlayerStats.Gold >= 4)
        {
            m_paid = true;
            float num;
            if (ID == 1)
                num = 0.25f;
            else if (ID == 2)
                num = 0.5f;
            else
                num = 0.75f;

            var num2 = (int) (Game.PlayerStats.Gold * num);
            Game.PlayerStats.Gold -= num2;
            ShuffleChests(num2);
            Player.AttachedLevel.TextManager.DisplayNumberStringText(-num2, "gold", Color.Yellow,
                new Vector2(Player.X, Player.Bounds.Top));
            Tween.By(m_gate, 1f, Quad.EaseInOut, "Y", (-m_gate.Height).ToString());
            return;
        }

        (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue(
            "ChestBonusRoom1-NoMoney");
        Tween.To(this, 0f, Linear.EaseNone);
        Tween.AddEndHandlerToLastTween(Player.AttachedLevel.ScreenManager, "DisplayScreen", 13, true,
            typeof(List<object>));
    }

    public override void Reset()
    {
        foreach (var current in m_chestList) current.ResetChest();
        if (m_paid)
        {
            m_gate.Y += m_gate.Height;
            m_paid = false;
        }

        base.Reset();
    }

    protected override GameObj CreateCloneInstance()
    {
        return new ChestBonusRoomObj();
    }

    protected override void FillCloneInstance(object obj)
    {
        base.FillCloneInstance(obj);
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            m_elf = null;
            m_gate = null;
            m_chestList.Clear();
            m_chestList = null;
            base.Dispose();
        }
    }
}
