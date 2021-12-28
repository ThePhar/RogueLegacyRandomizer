// 
// RogueLegacyArchipelago - ConnectionInfo.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;

namespace Archipelago
{
    public class ConnectionInfo
    {
        public string Hostname { get; set; }
        public int    Port     { get; set; }
        public string Name     { get; set; }
        public string Password { get; set; }
    }
}
