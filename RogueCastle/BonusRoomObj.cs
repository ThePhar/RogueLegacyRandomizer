using System;
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
			base.ID = -1;
		}
		public override void Reset()
		{
			this.RoomCompleted = false;
			base.Reset();
		}
	}
}
