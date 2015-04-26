/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

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
            foreach (var current in GameObjList)
            {
                var chestObj = current as ChestObj;
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