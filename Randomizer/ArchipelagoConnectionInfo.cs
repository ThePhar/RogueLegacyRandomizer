//  RogueLegacyRandomizer - ArchipelagoConnectionInfo.cs
//  Last Modified 2023-10-23 11:47 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

namespace Randomizer;

public record ArchipelagoConnectionInfo
{
    public string Url      { get; init; } = "wss://archipelago.gg:38281";
    public string SlotName { get; init; } = "Sir Lee";
    public string Password { get; init; } = "";

    /// <summary>
    /// Returns the default connection information.
    /// </summary>
    public static readonly ArchipelagoConnectionInfo Default = new();
}
