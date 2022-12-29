using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;

namespace RogueLegacy.GameObjects.HUD;

public class ReceivedItemsHUD : ObjContainer
{
    public const int MAXIMUM_ELEMENTS     = 5;
    public const int ITEM_TIMEOUT_SECONDS = 5;

    public List<ReceivedItemElement> Elements { get; private set; } = new();

    public List<ReceivedItemElement> FirstMaxElements =>
        Elements.GetRange(0, Math.Min(Elements.Count, MAXIMUM_ELEMENTS));

    public void Update()
    {
        var y = Y;
        foreach (var element in FirstMaxElements)
        {
            element.FirstDrawnTime ??= DateTime.Now;

            if (element.FirstDrawnTime?.AddSeconds(ITEM_TIMEOUT_SECONDS) < DateTime.Now && !element.FadingOut)
                element.FadeOut();

            element.UpdateY(y);
            y += 48f;
        }
    }

    public override void Draw(Camera2D camera)
    {
        foreach (var element in FirstMaxElements)
            element.Draw(camera);

        base.Draw(camera);
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;

        Elements.Clear();
        Elements = null;
        base.Dispose();
    }
}

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
        var randomizerData = Program.Game.ArchipelagoManager.RandomizerData;
        var self = Program.Game.ArchipelagoManager.GetPlayerName(randomizerData.Slot) == receivedFrom;

        _receivedItemsHUD = parent;
        X = parent.X;
        Y = parent.Y;
        Opacity = parent.Opacity;

        _item = new TextObj
        {
            Text = Program.Game.ArchipelagoManager.GetItemName(item),
            Font = Game.BitFont,
            TextureColor = Color.Yellow,
            OutlineWidth = 1,
            OutlineColour = Color.Black,
            FontSize = 10f,
            Y = parent.Y,
            X = parent.X + 42f,
            Anchor = Vector2.Zero
        };
        _receivedBy = new TextObj
        {
            Text = self ? "Received from yourself." : $"Received from {receivedFrom}.",
            Font = Game.JunicodeFont,
            OutlineWidth = 1,
            OutlineColour = Color.Black,
            FontSize = 6f,
            Y = parent.Y + 12f,
            X = parent.X + 42f
        };

        switch (type)
        {
            case ItemCode.ItemType.Blueprint:
                _icon = new SpriteObj("BlueprintIcon_Sprite");
                break;
            case ItemCode.ItemType.Rune:
                _icon = new SpriteObj("RuneIcon_Sprite");
                break;
            case ItemCode.ItemType.Stats:
                _icon = new SpriteObj("ManaCrystal_Sprite");
                _item.Text = $"{GetStatText(stats.Item1)}, {GetStatText(stats.Item2)}, {GetStatText(stats.Item3)}";
                break;
            case ItemCode.ItemType.Gold:
                _icon = new SpriteObj("MoneyBag_Sprite");
                _icon.StopAnimation();
                break;
            case ItemCode.ItemType.Skill:
                _icon = new SpriteObj(GetItemScreen.GetSkillPlateIcon(item, out _));
                break;
            case ItemCode.ItemType.Fountain:
                var randomSprite = new[]
                {
                    "TeleportRock1_Sprite",
                    "TeleportRock2_Sprite",
                    "TeleportRock3_Sprite",
                    "TeleportRock4_Sprite",
                    "TeleportRock5_Sprite"
                };
                _icon = new SpriteObj(randomSprite[CDGMath.RandomInt(0, 4)]);
                break;
            default:
                _icon = new SpriteObj("BlueprintIcon_Sprite");
                break;
        }

        Console.WriteLine($"{_icon.Width}, {_icon.Sprite.Name}");
        _icon.X = parent.X + (60f - _icon.Width) / _icon.Width;
        _icon.Y = parent.Y;
        _icon.Anchor = Vector2.Zero;
        _icon.Scale = new Vector2(32f / _icon.Height, 32f / _icon.Height);
        _icon.ForceDraw = true;
        _item.ForceDraw = true;
        _receivedBy.ForceDraw = true;
    }

    public DateTime? FirstDrawnTime { get; set; }
    public bool      FadingOut      { get; private set; }

    private string GetStatText(float type)
    {
        return (ItemDropType) type switch
        {
            ItemDropType.StatStrength  => "+1 ATK",
            ItemDropType.StatMagic     => "+1 MAG",
            ItemDropType.StatDefense   => "+1 DEF",
            ItemDropType.StatMaxHealth => "+5 HP",
            ItemDropType.StatMaxMana   => "+5 MP",
            ItemDropType.StatWeight    => "+5 WGT",
            _                          => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

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
            return;

        _receivedItemsHUD.Elements.Remove(this);
        _icon.Dispose();
        _icon = null;
        _item.Dispose();
        _item = null;
        _receivedBy.Dispose();
        _receivedBy = null;
        base.Dispose();
    }
}
