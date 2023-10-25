//  RogueLegacyRandomizer - ReceivedItemElement.cs
//  Last Modified 2023-10-25 6:21 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;

namespace RogueLegacy.GameObjects.HUD;

public sealed class ReceivedItemElement : ObjContainer
{
    private readonly ReceivedItemsHUD _receivedItemsHUD;
    private          SpriteObj        _icon;
    private          TextObj          _item;
    private          TextObj          _receivedBy;

    public ReceivedItemElement(
        ReceivedItemsHUD parent,
        ItemCode.ItemType type,
        long item,
        string receivedFrom,
        Tuple<float, float, float, float> stats)
    {
        ForceDraw = true;
        var manager = Program.Game.ArchipelagoManager;
        var self = manager.GetPlayerName(manager.Slot) == receivedFrom;

        Console.WriteLine("WE GOT AN ITEM");

        _receivedItemsHUD = parent;
        X = parent.X;
        Y = parent.Y;
        Opacity = parent.Opacity;

        _item = new(Game.BitFont)
        {
            Text = manager.GetItemName(item),
            TextureColor = Color.Yellow,
            OutlineWidth = 1,
            OutlineColour = Color.Black,
            FontSize = 10f,
            Align = Types.TextAlign.Right,
        };
        _receivedBy = new(Game.JunicodeFont)
        {
            Text = self ? "Received from yourself!" : $"Received from {receivedFrom}.",
            OutlineWidth = 1,
            OutlineColour = Color.Black,
            FontSize = 7f,
            Align = Types.TextAlign.Right,
        };

        switch (type)
        {
            case ItemCode.ItemType.Blueprint:
                _icon = new("BlueprintIcon_Sprite");
                break;
            case ItemCode.ItemType.Rune:
                _icon = new("RuneIcon_Sprite");
                break;
            case ItemCode.ItemType.Stats:
                _icon = new("ManaCrystal_Sprite");
                _item.Text = $"{GetStatText(stats.Item1)}, {GetStatText(stats.Item2)}, {GetStatText(stats.Item3)}";
                break;
            case ItemCode.ItemType.Gold:
                _icon = new("MoneyBag_Sprite");
                _icon.StopAnimation();
                break;
            case ItemCode.ItemType.Skill:
                _icon = new(GetItemScreen.GetSkillPlateIcon(item, out _));
                break;
            case ItemCode.ItemType.Fountain:
                var randomSprite = new[]
                {
                    "TeleportRock1_Sprite",
                    "TeleportRock2_Sprite",
                    "TeleportRock3_Sprite",
                    "TeleportRock4_Sprite",
                    "TeleportRock5_Sprite",
                };
                _icon = new(randomSprite[CDGMath.RandomInt(0, 4)]);
                break;
            default:
                _icon = new("BlueprintIcon_Sprite");
                break;
        }

        _icon.X = parent.X + 200 + (60f - _icon.Width) / _icon.Width;
        _icon.Y = parent.Y;
        _icon.AnchorX = _icon.Width;
        _icon.AnchorY = 0;
        _icon.Scale = new(32f / _icon.Height, 32f / _icon.Height);
        _icon.ForceDraw = true;
        _item.X = parent.X + 200 - _icon.Width - 8;
        _item.Y = parent.Y;
        _item.AnchorX = _item.Width;
        _item.ForceDraw = true;
        _receivedBy.X = parent.X + 200 - _icon.Width - 8;
        _receivedBy.AnchorX = _receivedBy.Width;
        _receivedBy.ForceDraw = true;
    }

    public DateTime? FirstDrawnTime { get; set; }
    public bool      FadingOut      { get; private set; }

    public void FadeOut()
    {
        Tween.To(_icon, 3f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_item, 3f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_receivedBy, 3f, Tween.EaseNone, "Opacity", "0");
        Tween.RunFunction(3f, this, "Dispose");
        FadingOut = true;
    }

    public void UpdateY(float y)
    {
        _icon.Y = _receivedItemsHUD.Y + y;
        _item.Y = _receivedItemsHUD.Y + y;
        _receivedBy.Y = _receivedItemsHUD.Y + y + 12f;
    }

    public override void Draw(Camera2D camera)
    {
        FirstDrawnTime ??= DateTime.Now;

        _item.Draw(camera);
        _receivedBy.Draw(camera);
        _icon.Draw(camera);

        base.Draw(camera);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _receivedItemsHUD.Elements.Remove(this);
        _icon.Dispose();
        _icon = null;
        _item.Dispose();
        _item = null;
        _receivedBy.Dispose();
        _receivedBy = null;
        base.Dispose();
    }

    private static string GetStatText(float type)
    {
        return (ItemDropType) type switch
        {
            ItemDropType.StatStrength  => "+2 ATK",
            ItemDropType.StatMagic     => "+2 MAG",
            ItemDropType.StatDefense   => "+4 DEF",
            ItemDropType.StatMaxHealth => "+10 HP",
            ItemDropType.StatMaxMana   => "+10 MP",
            ItemDropType.StatWeight    => "+10 WGT",
            _                          => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}
