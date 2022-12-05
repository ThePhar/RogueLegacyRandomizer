// Rogue Legacy Randomizer - BonusRoomObj.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

namespace RogueLegacy;

public class BonusRoomObj : RoomObj
{
    protected BonusRoomObj()
    {
        ID = -1;
    }

    public bool RoomCompleted { get; set; }

    public override void Reset()
    {
        RoomCompleted = false;
        base.Reset();
    }
}
