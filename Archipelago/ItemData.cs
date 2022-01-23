//
//  Rogue Legacy Randomizer - ItemData.cs
//  Last Modified 2022-01-23
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using Archipelago.Definitions;

namespace Archipelago
{
    public class ItemData
    {
        public ItemData(int code, string name, ItemType type)
        {
            Code = code;
            Name = name;
            Type = type;
        }

        public ItemType Type { get; }
        public int      Code { get; }
        public string   Name { get; }
    }
}
