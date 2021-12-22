// 
// RogueLegacyArchipelago - NetworkVersion.cs
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
    internal class NetworkVersion
    {
        [JsonProperty("major")]
        public int Major { get; set; }

        [JsonProperty("minor")]
        public int Minor { get; set; }

        [JsonProperty("build")]
        public int Build { get; set; }

        // What's great about this property is it isn't listed as required in the docs. :')
        [JsonProperty("class")]
        public string Class { get; set; }

        public NetworkVersion()
        {
            Class = "Version";
        }

        public override string ToString()
        {
            return Major + "." + Minor + "." + Build;
        }
    }
}
