//  RogueLegacyRandomizer - StringExtensions.cs
//  Last Modified 2023-10-25 8:56 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Linq;

namespace RogueLegacy.Util;

public static class StringExtensions
{
    public static bool EqualsAny(this string value, params string[] comparedValues)
    {
        return comparedValues.Any(@string => @string == value);
    }
}
