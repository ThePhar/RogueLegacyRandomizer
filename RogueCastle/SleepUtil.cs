/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Runtime.InteropServices;

namespace RogueCastle
{
    public static class SleepUtil
    {
        [Flags]
        private enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 64u,
            ES_CONTINUOUS = 2147483648u,
            ES_DISPLAY_REQUIRED = 2u,
            ES_SYSTEM_REQUIRED = 1u
        }

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
    }
}