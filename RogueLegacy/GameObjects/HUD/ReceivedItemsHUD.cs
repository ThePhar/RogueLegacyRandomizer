//  RogueLegacyRandomizer - ReceivedItemsHUD.cs
//  Last Modified 2023-10-25 2:51 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;

namespace RogueLegacy.GameObjects.HUD;

public class ReceivedItemsHUD : ObjContainer
{
    private const int MAXIMUM_ELEMENTS     = 5;
    private const int ITEM_TIMEOUT_SECONDS = 5;

    public List<ReceivedItemElement> Elements { get; private set; } = new();

    private List<ReceivedItemElement> ViewableElements
        => Elements.GetRange(0, Math.Min(Elements.Count, MAXIMUM_ELEMENTS));

    public void Update()
    {
        var elementY = Y;
        foreach (var element in ViewableElements)
        {
            element.FirstDrawnTime ??= DateTime.Now;

            if (element.FirstDrawnTime?.AddSeconds(ITEM_TIMEOUT_SECONDS) < DateTime.Now && !element.FadingOut)
            {
                element.FadeOut();
            }

            element.UpdateY(elementY);
            elementY += 48f;
        }
    }

    public override void Draw(Camera2D camera)
    {
        foreach (var element in ViewableElements)
        {
            element.Draw(camera);
        }

        base.Draw(camera);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        Elements.Clear();
        Elements = null;
        base.Dispose();
    }
}
