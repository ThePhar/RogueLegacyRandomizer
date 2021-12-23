// 
// RogueLegacyArchipelago - ReceivedItemsPacket.cs
// Last Modified 2021-12-22
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using Archipelago.Network;
using Newtonsoft.Json;

namespace Archipelago.Packets
{
    internal class ReceivedItemsPacket : IDataPacket
    {
        [JsonProperty("cmd")]
        public string Command { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("items")]
        public List<NetworkItem> Items { get; set; }
    }
}
