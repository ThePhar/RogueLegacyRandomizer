// 
// RogueLegacyArchipelago - DataPacketTypes.cs
// Last Modified 2021-12-22
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace Archipelago.Packets
{
    internal static class DataPacketTypes
    {
        // Server Packets
        public const string RoomInfo          = "RoomInfo";
        public const string ConnectionRefused = "ConnectionRefused";
        public const string Connected         = "Connected";
        public const string ReceivedItems     = "ReceivedItems";
        public const string LocationInfo      = "LocationInfo";
        public const string RoomUpdate        = "RoomUpdate";
        public const string Print             = "Print";
        public const string PrintJson         = "PrintJSON";
        public const string DataPackage       = "DataPackage";
        public const string Bounced           = "Bounced";
        public const string InvalidPacket     = "InvalidPacket";

        // Client Packets
        public const string Connect           = "Connect";
        public const string Sync              = "Sync";
        public const string LocationChecks    = "LocationChecks";
        public const string LocationScouts    = "LocationScouts";
        public const string StatusUpdate      = "StatusUpdate";
        public const string Say               = "Say";
        public const string GetDataPackage    = "GetDataPackage";
        public const string Bounce            = "Bounce";
    }
}
