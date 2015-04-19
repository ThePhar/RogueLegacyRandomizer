using DS2DEngine;

namespace RogueCastle
{
	public class CompassRoomObj : RoomObj
	{
		public override void OnEnter()
		{
			if (Game.PlayerStats.SpecialItem == 6)
			{
				Game.PlayerStats.SpecialItem = 0;
				Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
			}
			SoundManager.StopMusic(2f);
			Player.UnlockControls();
			foreach (GameObj current in GameObjList)
			{
				ChestObj chestObj = current as ChestObj;
				if (chestObj != null)
				{
					chestObj.ResetChest();
					chestObj.ChestType = 3;
				}
			}
			base.OnEnter();
		}
		public override void OnExit()
		{
			Player.AttachedLevel.RemoveCompassDoor();
			Player.UnlockControls();
			Player.KickInHitInvincibility();
			base.OnExit();
		}
		protected override GameObj CreateCloneInstance()
		{
			return new CompassRoomObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
	}
}
