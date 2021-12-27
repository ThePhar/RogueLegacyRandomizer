// 
// RogueLegacyArchipelago - CompassRoomObj.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

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

            // TODO: FIX
            // Program.Game.ArchClient.Session.Locations.CompleteLocationChecks(4445042);
            // Program.Game.ArchClient.CheckedLocations[4445042] = true;

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
