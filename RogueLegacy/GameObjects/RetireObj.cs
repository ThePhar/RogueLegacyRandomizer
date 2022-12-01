// Rogue Legacy Randomizer - RetireObj.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;

namespace RogueLegacy.GameObjects;

public class RetireObj : GameObj
{
    protected override GameObj CreateCloneInstance()
    {
        return new RetireObj();
    }
}
