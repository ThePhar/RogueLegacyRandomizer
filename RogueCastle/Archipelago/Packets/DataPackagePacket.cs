// 
// RogueLegacyArchipelago - DataPackagePacket.cs
// Last Modified 2021-12-22
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using Newtonsoft.Json;
using RogueCastle.Archipelago.Network;

namespace RogueCastle.Archipelago.Packets
{
    internal class DataPackagePacket : IDataPacket
    {
        [JsonProperty("cmd")]
        public string Command { get; set; }

        [JsonProperty("data")]
        public DataPackage Data { get; set; }
    }
}
