// 
// RogueLegacyArchipelago - NetworkItem.cs
// Last Modified 2021-12-22
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using Newtonsoft.Json;

namespace Archipelago.Network
{
    internal class NetworkItem
    {
        [JsonProperty("item")]
        public int Item { get; set; }

        [JsonProperty("location")]
        public int Location { get; set; }

        [JsonProperty("player")]
        public int Player { get; set; }

        public override string ToString()
        {
            return string.Format("Item {0} in Location {1} for Player {2}", Item, Location, Player);
        }
    }
}
