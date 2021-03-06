//
//  Rogue Legacy Randomizer - UniChests.cs
//  Last Modified 2022-04-03
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;

namespace RogueCastle.Enums
{
    [Flags]
    public enum UniChests
    {
        NoUniversal  = 0b00,
        NormalChests = 0b01,
        FairyChests  = 0b10,
        AllChests    = 0b11,
    }
}
