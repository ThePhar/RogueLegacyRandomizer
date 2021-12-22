// 
// RogueLegacyArchipelago - RoomInfoPacket.cs
// Last Modified 2021-12-22
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using Newtonsoft.Json;
using RogueCastle.Archipelago.Network;

namespace RogueCastle.Archipelago.Packets
{
    internal class RoomInfoPacket : IDataPacket
    {
        [JsonProperty("cmd")]
        public string Command { get; set; }

        [JsonProperty("version")]
        public NetworkVersion Version { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("password")]
        public bool Password { get; set; }

        [JsonProperty("permissions")]
        public Dictionary<string, int> Permissions { get; set; }

        [JsonProperty("hint_cost")]
        public int HintCost { get; set; }

        [JsonProperty("location_check_points")]
        public int LocationCheckPoints { get; set; }

        [JsonProperty("players")]
        public List<NetworkPlayer> Players { get; set; }

        [JsonProperty("games")]
        public List<string> Games { get; set; }

        [JsonProperty("datapackage_version")]
        public int DataPackageVersion { get; set; }

        [JsonProperty("datapackage_versions")]
        public Dictionary<string, int> DataPackageVersions { get; set; }

        [JsonProperty("seed_name")]
        public string SeedName { get; set; }

        [JsonProperty("time")]
        public float Time { get; set; }
    }
}
