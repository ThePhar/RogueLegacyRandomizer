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
