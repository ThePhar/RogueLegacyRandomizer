using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class ChestObj : PhysicsObj
{
    private readonly Vector2   _bronzeChestGoldRange = new(9f, 14f);
    private readonly Vector2   _goldChestGoldRange   = new(47f, 57f);
    private readonly float     _goldIncreasePerLevel = 1.425f;
    private readonly Vector2   _silverChestGoldRange = new(20f, 28f);
    private          SpriteObj _arrowIcon;
    private          ChestType _chestTypeType;
    public           int       Level;

    public ChestObj(PhysicsManager physicsManager) : base("Chest1_Sprite", physicsManager)
    {
        DisableHitboxUpdating = true;
        IsWeighted = false;
        Layer = 1f;
        OutlineWidth = 2;
        IsProcedural = true;
        _arrowIcon = new SpriteObj("UpArrowSquare_Sprite") { OutlineWidth = 2, Visible = false };
    }

    public bool         IsEmpty        { get; set; }
    public bool         IsLocked       { get; set; }
    public ItemDropType ForcedItemType { get; set; }
    public float        ForcedAmount   { get; set; }
    public bool         IsProcedural   { get; set; }

    public ChestType ChestType
    {
        get => _chestTypeType;
        set
        {
            _chestTypeType = value;
            var isOpen = IsOpen;
            switch (_chestTypeType)
            {
                case ChestType.Boss:
                    ForcedItemType = ItemDropType.TripStatDrop;
                    ChangeSprite("BossChest_Sprite");
                    break;

                case ChestType.Fairy:
                    ChangeSprite("Chest4_Sprite");
                    break;

                case ChestType.Gold:
                    ChangeSprite("Chest3_Sprite");
                    break;

                case ChestType.Silver:
                    ChangeSprite("Chest2_Sprite");
                    break;

                default:
                    ChangeSprite("Chest1_Sprite");
                    break;
            }

            if (isOpen) GoToFrame(2);
        }
    }

    public bool IsOpen => CurrentFrame == 2;

    public virtual void OpenChest(ItemDropManager itemDropManager, PlayerObj player)
    {
        // Do not open chests that have been opened or are locked.
        if (IsOpen || IsLocked) return;

        SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
        GoToFrame(2);

        // If chest is empty, that's sad.
        if (IsEmpty) return;

        var randomInt = CDGMath.RandomInt(1, 100);
        var dropType = 0;
        var chances = ChestType switch
        {
            ChestType.Brown  => GameEV.BRONZECHEST_ITEMDROP_CHANCE,
            ChestType.Silver => GameEV.SILVERCHEST_ITEMDROP_CHANCE,
            _                => GameEV.GOLDCHEST_ITEMDROP_CHANCE
        };

        var threshold = 0;
        for (var i = 0; i < chances.Length; i++)
        {
            threshold += chances[i];

            // Skip if we rolled higher.
            if (randomInt > threshold) continue;

            dropType = i;
            break;
        }

        // Extra boss stuff!
        if (ChestType == ChestType.Boss)
        {
            GiveStatDrop(itemDropManager, player, 3, 0);
            return;
        }

        GiveNetworkItem(itemDropManager, player);
        player.AttachedLevel.RefreshMapChestIcons();
    }

    public void GiveGold(ItemDropManager itemDropManager, int amount = 0)
    {
        var num = ChestType switch
        {
            ChestType.Brown => CDGMath.RandomInt((int) _bronzeChestGoldRange.X, (int) _bronzeChestGoldRange.Y) * 10,
            ChestType.Silver or ChestType.Fairy => CDGMath.RandomInt((int) _silverChestGoldRange.X,
                (int) _silverChestGoldRange.Y) * 10,
            _ => CDGMath.RandomInt((int) _goldChestGoldRange.X, (int) _goldChestGoldRange.Y) * 10
        };

        num += (int) Math.Floor(_goldIncreasePerLevel * Level * 10f);
        if (amount != 0) num = amount;

        var num2 = num / 500;
        num -= num2 * 500;
        var num3 = num / 100;
        num -= num3 * 100;
        var num4 = num / 10;
        var num5 = 0f;
        for (var i = 0; i < num2; i++)
        {
            Tween.To(this, num5, Linear.EaseNone);
            Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                new Vector2(Position.X, Position.Y - Height / 2), 11, 500);
            num5 += 0.1f;
        }

        num5 = 0f;
        for (var j = 0; j < num3; j++)
        {
            Tween.To(this, num5, Linear.EaseNone);
            Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                new Vector2(Position.X, Position.Y - Height / 2), 10, 100);
            num5 += 0.1f;
        }

        num5 = 0f;
        for (var k = 0; k < num4; k++)
        {
            Tween.To(this, num5, Linear.EaseNone);
            Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                new Vector2(Position.X, Position.Y - Height / 2), 1, 10);
            num5 += 0.1f;
        }
    }

    public void GiveStatDrop(ItemDropManager manager, PlayerObj player, int numDrops, int statDropType)
    {
        var array = new int[numDrops];
        for (var i = 0; i < numDrops; i++)
            if (statDropType == 0)
            {
                var num = CDGMath.RandomInt(1, 100);
                var num2 = 0;
                var j = 0;
                while (j < GameEV.STATDROP_CHANCE.Length)
                {
                    num2 += GameEV.STATDROP_CHANCE[j];
                    if (num <= num2)
                    {
                        if (j == 0)
                        {
                            array[i] = 4;
                            Game.PlayerStats.BonusStrength++;
                            break;
                        }

                        if (j == 1)
                        {
                            array[i] = 5;
                            Game.PlayerStats.BonusMagic++;
                            break;
                        }

                        if (j == 2)
                        {
                            array[i] = 6;
                            Game.PlayerStats.BonusDefense++;
                            break;
                        }

                        if (j == 3)
                        {
                            array[i] = 7;
                            Game.PlayerStats.BonusHealth++;
                            break;
                        }

                        if (j == 4)
                        {
                            array[i] = 8;
                            Game.PlayerStats.BonusMana++;
                            break;
                        }

                        array[i] = 9;
                        Game.PlayerStats.BonusWeight++;
                        break;
                    }

                    j++;
                }
            }
            else
            {
                switch (statDropType)
                {
                    case 4:
                        Game.PlayerStats.BonusStrength++;
                        break;

                    case 5:
                        Game.PlayerStats.BonusMagic++;
                        break;

                    case 6:
                        Game.PlayerStats.BonusDefense++;
                        break;

                    case 7:
                        Game.PlayerStats.BonusHealth++;
                        break;

                    case 8:
                        Game.PlayerStats.BonusMana++;
                        break;

                    case 9:
                        Game.PlayerStats.BonusWeight++;
                        break;
                }

                array[i] = statDropType;
            }

        var list = new List<object>();
        list.Add(new Vector2(X, Y - Height / 2f));
        if (statDropType >= 15 && statDropType <= 19)
            list.Add(7);
        else if (numDrops <= 1)
            list.Add(3);
        else
            list.Add(6);

        list.Add(new Vector2(array[0], 0f));
        if (numDrops > 1) list.Add(new Vector2(array[1], array[2]));

        player.AttachedLevel.UpdatePlayerHUD();
        (player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list);
        player.RunGetItemAnimation();
    }

    protected void GiveNetworkItem(ItemDropManager manager, PlayerObj player, bool isFairy = false)
    {
        var room = Game.ScreenManager.GetLevelScreen().CurrentRoom;

        if (ForcedItemType == ItemDropType.Coin)
        {
            GiveGold(manager, (int) ForcedAmount);
            return;
        }

        var randomizerData = Program.Game.ArchipelagoManager.RandomizerData;
        if (isFairy)
        {
            for (var chest = 0; chest <= LocationCode.MAX_FAIRY_CHESTS; chest++)
            {
                // Ignore if chest doesn't exist.
                if (!LocationCode.TryGetFairyChestLocation(randomizerData, chest, room.Zone, out var location))
                    break;

                // Ignore checked locations.
                if (randomizerData.CheckedLocations[location])
                    continue;

                Program.Game.CollectItemFromLocation(location);
                return;
            }

            GiveStatDrop(manager, player, 3, 0);
        }
        else
        {
            for (var chest = 0; chest <= LocationCode.MAX_CHESTS; chest++)
            {
                // Ignore if chest doesn't exist.
                if (!LocationCode.TryGetChestLocation(randomizerData, chest, room.Zone, out var location))
                    break;

                // Ignore checked locations.
                if (randomizerData.CheckedLocations[location])
                    continue;

                Program.Game.CollectItemFromLocation(location);
                break;
            }

            GiveGold(manager);
        }
    }

    public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
    {
        if (!IsLocked && !IsOpen && otherBox.AbsParent is PlayerObj { IsTouchingGround: true })
            _arrowIcon.Visible = true;

        base.CollisionResponse(thisBox, otherBox, collisionResponseType);
    }

    public override void Draw(Camera2D camera)
    {
        if (_arrowIcon.Visible)
        {
            _arrowIcon.Position = new Vector2(Bounds.Center.X,
                Bounds.Top - 50 + (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 3f);
            _arrowIcon.Draw(camera);
            _arrowIcon.Visible = false;
        }

        base.Draw(camera);
    }

    public virtual void ForceOpen()
    {
        GoToFrame(2);
    }

    public virtual void ResetChest()
    {
        GoToFrame(1);
    }

    protected override GameObj CreateCloneInstance()
    {
        return new ChestObj(PhysicsMngr);
    }

    protected override void FillCloneInstance(object obj)
    {
        base.FillCloneInstance(obj);
        var chestObj = obj as ChestObj;
        chestObj.IsProcedural = IsProcedural;
        chestObj.ChestType = ChestType;
        chestObj.Level = Level;
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        _arrowIcon.Dispose();
        _arrowIcon = null;
        base.Dispose();
    }
}
