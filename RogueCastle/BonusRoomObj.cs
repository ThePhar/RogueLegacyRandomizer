/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

namespace RogueCastle
{
	public class BonusRoomObj : RoomObj
	{
		public bool RoomCompleted
		{
			get;
			set;
		}
		public BonusRoomObj()
		{
			ID = -1;
		}
		public override void Reset()
		{
			RoomCompleted = false;
			base.Reset();
		}
	}
}
