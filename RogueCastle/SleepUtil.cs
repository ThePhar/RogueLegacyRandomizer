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
			if (SetThreadExecutionState((EXECUTION_STATE)2147483715u) == 0u)
			{
				SetThreadExecutionState((EXECUTION_STATE)2147483651u);
			}
		}
		public static void EnableScreensaver()
		{
			SetThreadExecutionState((EXECUTION_STATE)2147483648u);
		}
	}
}
