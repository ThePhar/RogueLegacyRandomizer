// Rogue Legacy Randomizer - SleepUtil.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace RogueLegacy
{
    public static class SleepUtil
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public static void DisableScreensaver()
        {
            if (SetThreadExecutionState((EXECUTION_STATE) 2147483715u) == 0u)
            {
                SetThreadExecutionState((EXECUTION_STATE) 2147483651u);
            }
        }

        public static void EnableScreensaver()
        {
            SetThreadExecutionState((EXECUTION_STATE) 2147483648u);
        }

        [Flags]
        private enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 64u,
            ES_CONTINUOUS = 2147483648u,
            ES_DISPLAY_REQUIRED = 2u,
            ES_SYSTEM_REQUIRED = 1u
        }
    }
}
