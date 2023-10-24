//  RogueLegacyRandomizer - Util.cs
//  Last Modified 2023-10-24 6:29 PM
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using Newtonsoft.Json.Linq;

namespace Randomizer;

public static class Util
{
    /// <summary>
    /// Deep print all properties of a supplied object with given indentation level.
    /// </summary>
    /// <param name="obj">Object to print all properties from.</param>
    /// <param name="indentation">Indentation level. Uses tabs for indentation.</param>
    public static void PrintProperties(object obj, int indentation = 1)
    {
        if (obj == null)
        {
            return;
        }

        var indentString = new string('\t', indentation);
        var json = JObject.FromObject(obj);
        foreach (var property in json.Properties())
        {
            Console.WriteLine($"{property.Name}: {property.Value}");
        }

        // var objectType = obj.GetType();
        // var properties = objectType.GetProperties();
        // foreach (var property in properties)
        // {
        //     var value = property.GetValue(obj);
        //     if (value is IList elements)
        //     {
        //         foreach (var item in elements)
        //         {
        //             PrintProperties(item, indentation + 1);
        //         }
        //     }
        //     else
        //     {
        //         if (property.PropertyType.Assembly == objectType.Assembly)
        //         {
        //             Console.WriteLine("{0}{1}:", indentString, property.Name);
        //             PrintProperties(value, indentation + 1);
        //         }
        //         else
        //         {
        //             Console.WriteLine("{0}{1}: {2}", indentString, property.Name, value);
        //         }
        //     }
        // }

        // Final linebreak for readability.
        Console.WriteLine();
    }
}
