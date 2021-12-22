// 
// RogueLegacyArchipelago - NetworkPlayer.cs
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
    internal class NetworkPlayer
    {
        [JsonProperty("team")]
        public int Team { get; set; }

        [JsonProperty("slot")]
        public int Slot { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - Player {1} (aka {2}) - Team {3}", Slot, Name, Alias, Team);
        }
    }
}
