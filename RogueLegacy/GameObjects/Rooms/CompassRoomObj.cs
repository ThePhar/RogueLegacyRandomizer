// RogueLegacyRandomizer - CompassRoomObj.cs
// Last Modified 2023-07-30 12:57 PM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Randomizer.Definitions;
using RogueLegacy.Enums;

namespace RogueLegacy
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
            var leftPlaced = false;
            foreach (var current in GameObjList)
            {
                var chestObj = current as ChestObj;
                if (chestObj != null)
                {
                    chestObj.ResetChest();
                    chestObj.ChestType = ChestType.Gold;
                    if (!leftPlaced)
                    {
                        chestObj.ForcedLocation = LocationCode.SECRET_ROOM_LEFT;
                        leftPlaced = true;
                    }
                    else
                    {
                        chestObj.ForcedLocation = LocationCode.SECRET_ROOM_RIGHT;
                    }
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