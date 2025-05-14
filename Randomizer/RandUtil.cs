//  RogueLegacyRandomizer - RandUtil.cs
//  Last Modified 2023-10-26 2:07 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using Newtonsoft.Json.Linq;

namespace Randomizer;

public static class RandUtil
{
    public static void Console(string group, string message)
    {
        System.Console.WriteLine($"[{group}] {DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff")}: {message}");
    }

    // /// <summary>
    // /// Deep print all properties of a supplied object with given indentation level.
    // /// </summary>
    // /// <param name="obj">Object to print all properties from.</param>
    // public static void PrintProperties(object obj)
    // {
    //     if (obj == null)
    //     {
    //         return;
    //     }
    //
    //     var json = JObject.FromObject(obj);
    //     foreach (var property in json.Properties())
    //     {
    //         Console("Archipelago", $"{property.Name}: {property.Value}");
    //     }
    // }
}
