using DS2DEngine;
using System;
namespace RogueCastle
{
	public class CompassRoomObj : RoomObj
	{
		public override void OnEnter()
		{
			if (Game.PlayerStats.SpecialItem == 6)
			{
				Game.PlayerStats.SpecialItem = 0;
				this.Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
			}
			SoundManager.StopMusic(2f);
			this.Player.UnlockControls();
			foreach (GameObj current in base.GameObjList)
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
			this.Player.AttachedLevel.RemoveCompassDoor();
			this.Player.UnlockControls();
			this.Player.KickInHitInvincibility();
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
