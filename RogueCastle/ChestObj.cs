// 
// RogueLegacyArchipelago - ChestObj.cs
// Last Modified 2021-12-25
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.TypeDefinitions;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class ChestObj : PhysicsObj
    {
        private readonly float GoldIncreasePerLevel = 1.425f;
        private Vector2 BronzeChestGoldRange = new Vector2(9f, 14f);
        private Vector2 GoldChestGoldRange = new Vector2(47f, 57f);
        public int Level;
        private SpriteObj m_arrowIcon;
        private byte m_chestType;
        private Vector2 SilverChestGoldRange = new Vector2(20f, 28f);

        public ChestObj(PhysicsManager physicsManager) : base("Chest1_Sprite", physicsManager)
        {
            DisableHitboxUpdating = true;
            IsWeighted = false;
            Layer = 1f;
            OutlineWidth = 2;
            IsProcedural = true;
            m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
            m_arrowIcon.OutlineWidth = 2;
            m_arrowIcon.Visible = false;
        }

        public bool IsEmpty { get; set; }
        public bool IsLocked { get; set; }
        public int ForcedItemType { get; set; }
        public float ForcedAmount { get; set; }
        public bool IsProcedural { get; set; }

        public byte ChestType
        {
            get { return m_chestType; }
            set
            {
                m_chestType = value;
                var isOpen = IsOpen;
                switch (m_chestType)
                {
                    case TypeDefinitions.ChestType.Boss:
                        ForcedItemType = 14;
                        ChangeSprite("BossChest_Sprite");
                        break;
                    case TypeDefinitions.ChestType.Fairy:
                        ChangeSprite("Chest4_Sprite");
                        break;
                    case TypeDefinitions.ChestType.Gold:
                        ChangeSprite("Chest3_Sprite");
                        break;
                    case TypeDefinitions.ChestType.Silver:
                        ChangeSprite("Chest2_Sprite");
                        break;
                    default:
                        ChangeSprite("Chest1_Sprite");
                        break;
                }

                if (isOpen)
                    GoToFrame(2);
            }
        }

        public bool IsOpen
        {
            get { return CurrentFrame == 2; }
        }

        public virtual void OpenChest(ItemDropManager itemDropManager, PlayerObj player)
        {
            // Do not open chests that have been opened or are locked.
            if (IsOpen || IsLocked)
                return;

            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
            GoToFrame(2);

            // If chest is empty, that's sad.
            if (IsEmpty)
                return;

            // Unlock achievement!
            if (ChestType == TypeDefinitions.ChestType.Gold)
                GameUtil.UnlockAchievement("LOVE_OF_GOLD");

            GiveNetworkItem(itemDropManager, player);
            // if (ForcedItemType == 0)
            // {
            //     var num = CDGMath.RandomInt(1, 100);
            //     var num2 = 0;
            //     int[] array;
            //     if (ChestType == TypeDefinitions.ChestType.Brown)
            //     {
            //         array = GameEV.BRONZECHEST_ITEMDROP_CHANCE;
            //     }
            //     else if (ChestType == TypeDefinitions.ChestType.Silver)
            //     {
            //         array = GameEV.SILVERCHEST_ITEMDROP_CHANCE;
            //     }
            //     else
            //     {
            //         array = GameEV.GOLDCHEST_ITEMDROP_CHANCE;
            //     }
            //     var num3 = 0;
            //     for (var i = 0; i < array.Length; i++)
            //     {
            //         num3 += array[i];
            //         if (num <= num3)
            //         {
            //             num2 = i;
            //             break;
            //         }
            //     }
            //     if (num2 == 0)
            //     {
            //         GiveGold(itemDropManager);
            //     }
            //     else if (num2 == 1)
            //     {
            //         GiveStatDrop(itemDropManager, player, 1, 0);
            //     }
            //     else
            //     {
            //         GivePrint(itemDropManager, player);
            //     }
            // }
            // else
            // {
            //     switch (ForcedItemType)
            //     {
            //         case 1:
            //         case 10:
            //         case 11:
            //             GiveGold(itemDropManager, (int) ForcedAmount);
            //             break;
            //         case 4:
            //         case 5:
            //         case 6:
            //         case 7:
            //         case 8:
            //         case 9:
            //             GiveStatDrop(itemDropManager, player, 1, ForcedItemType);
            //             break;
            //         case 12:
            //         case 13:
            //             GivePrint(itemDropManager, player);
            //             break;
            //         case 14:
            //             GiveStatDrop(itemDropManager, player, 3, 0);
            //             break;
            //         case 15:
            //         case 16:
            //         case 17:
            //         case 18:
            //         case 19:
            //             GiveStatDrop(itemDropManager, player, 1, ForcedItemType);
            //             break;
            //     }
            // }

            player.AttachedLevel.RefreshMapChestIcons();
        }

        public void GiveGold(ItemDropManager itemDropManager, int amount = 0)
        {
            int num;
            if (ChestType == 1)
            {
                num = CDGMath.RandomInt((int) BronzeChestGoldRange.X, (int) BronzeChestGoldRange.Y)*10;
            }
            else if (ChestType == 2 || ChestType == 4)
            {
                num = CDGMath.RandomInt((int) SilverChestGoldRange.X, (int) SilverChestGoldRange.Y)*10;
            }
            else
            {
                num = CDGMath.RandomInt((int) GoldChestGoldRange.X, (int) GoldChestGoldRange.Y)*10;
            }
            num += (int) Math.Floor(GoldIncreasePerLevel*Level*10f);
            if (amount != 0)
            {
                num = amount;
            }
            var num2 = num/500;
            num -= num2*500;
            var num3 = num/100;
            num -= num3*100;
            var num4 = num/10;
            var num5 = 0f;
            for (var i = 0; i < num2; i++)
            {
                Tween.To(this, num5, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                    new Vector2(Position.X, Position.Y - Height/2), 11, 500);
                num5 += 0.1f;
            }
            num5 = 0f;
            for (var j = 0; j < num3; j++)
            {
                Tween.To(this, num5, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                    new Vector2(Position.X, Position.Y - Height/2), 10, 100);
                num5 += 0.1f;
            }
            num5 = 0f;
            for (var k = 0; k < num4; k++)
            {
                Tween.To(this, num5, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                    new Vector2(Position.X, Position.Y - Height/2), 1, 10);
                num5 += 0.1f;
            }
        }

        public void GiveStatDrop(ItemDropManager manager, PlayerObj player, int numDrops, int statDropType)
        {
            var array = new int[numDrops];
            for (var i = 0; i < numDrops; i++)
            {
                if (statDropType == 0)
                {
                    var num = CDGMath.RandomInt(1, 100);
                    var num2 = 0;
                    var j = 0;
                    while (j < GameEV.STATDROP_CHANCE.Length)
                    {
                        num2 += GameEV.STATDROP_CHANCE[j];
                        if (num <= num2)
                        {
                            if (j == 0)
                            {
                                array[i] = 4;
                                Game.PlayerStats.BonusStrength++;
                                break;
                            }
                            if (j == 1)
                            {
                                array[i] = 5;
                                Game.PlayerStats.BonusMagic++;
                                break;
                            }
                            if (j == 2)
                            {
                                array[i] = 6;
                                Game.PlayerStats.BonusDefense++;
                                break;
                            }
                            if (j == 3)
                            {
                                array[i] = 7;
                                Game.PlayerStats.BonusHealth++;
                                break;
                            }
                            if (j == 4)
                            {
                                array[i] = 8;
                                Game.PlayerStats.BonusMana++;
                                break;
                            }
                            array[i] = 9;
                            Game.PlayerStats.BonusWeight++;
                            break;
                        }
                        j++;
                    }
                }
                else
                {
                    switch (statDropType)
                    {
                        case 4:
                            Game.PlayerStats.BonusStrength++;
                            break;
                        case 5:
                            Game.PlayerStats.BonusMagic++;
                            break;
                        case 6:
                            Game.PlayerStats.BonusDefense++;
                            break;
                        case 7:
                            Game.PlayerStats.BonusHealth++;
                            break;
                        case 8:
                            Game.PlayerStats.BonusMana++;
                            break;
                        case 9:
                            Game.PlayerStats.BonusWeight++;
                            break;
                    }
                    array[i] = statDropType;
                }
            }
            var list = new List<object>();
            list.Add(new Vector2(X, Y - Height/2f));
            if (statDropType >= 15 && statDropType <= 19)
            {
                list.Add(7);
            }
            else if (numDrops <= 1)
            {
                list.Add(3);
            }
            else
            {
                list.Add(6);
            }
            list.Add(new Vector2(array[0], 0f));
            if (numDrops > 1)
            {
                list.Add(new Vector2(array[1], array[2]));
            }
            player.AttachedLevel.UpdatePlayerHUD();
            (player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list);
            player.RunGetItemAnimation();
        }

        public void GivePrint(ItemDropManager manager, PlayerObj player)
        {
            if (Game.PlayerStats.TotalBlueprintsFound >= 75)
            {
                if (ChestType == 3)
                {
                    GiveStatDrop(manager, player, 1, 0);
                    return;
                }
                GiveGold(manager);
            }
            else
            {
                var getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
                var list = new List<Vector2>();
                var num = 0;
                foreach (var current in getBlueprintArray)
                {
                    var num2 = 0;
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        if (array[i] == 0)
                        {
                            var equipmentData = Game.EquipmentSystem.GetEquipmentData(num, num2);
                            if (Level >= equipmentData.LevelRequirement &&
                                ChestType >= equipmentData.ChestColourRequirement)
                            {
                                list.Add(new Vector2(num, num2));
                            }
                        }
                        num2++;
                    }
                    num++;
                }
                if (list.Count > 0)
                {
                    var vector = list[CDGMath.RandomInt(0, list.Count - 1)];
                    Game.PlayerStats.GetBlueprintArray[(int) vector.X][(int) vector.Y] = 1;
                    var list2 = new List<object>();
                    list2.Add(new Vector2(X, Y - Height/2f));
                    list2.Add(1);
                    list2.Add(new Vector2(vector.X, vector.Y));
                    (player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list2);
                    player.RunGetItemAnimation();
                    Console.WriteLine(string.Concat("Unlocked item index ", vector.X, " of type ", vector.Y));
                    return;
                }
                GiveGold(manager);
            }
        }

        public void GiveNetworkItem(ItemDropManager manager, PlayerObj player)
        {
            var pName = Program.Game.ArchClient.Session.Players.GetPlayerAlias(
                Program.Game.ArchClient.LocationCache[44200 + Program.Game.ArchClient.TestNumber].Player);
            var pItem = Program.Game.ArchClient.Session.Items.GetItemName(
                Program.Game.ArchClient.LocationCache[44200 + Program.Game.ArchClient.TestNumber].Item);
            var networkItem = new List<object>
            {
                new Vector2(X, Y - Height / 2f),
                GetItemType.NetworkItem,
                new Vector2(0, 0),
                pName,
                pItem
            };

            Program.Game.ArchClient.Session.Locations.CompleteLocationChecks(44200 + Program.Game.ArchClient.TestNumber);
            Program.Game.ArchClient.TestNumber++;

            Game.ScreenManager.DisplayScreen(12, true, networkItem);
            player.RunGetItemAnimation();
        }


        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            var playerObj = otherBox.AbsParent as PlayerObj;
            if (!IsLocked && !IsOpen && playerObj != null && playerObj.IsTouchingGround)
            {
                m_arrowIcon.Visible = true;
            }
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void Draw(Camera2D camera)
        {
            if (m_arrowIcon.Visible)
            {
                m_arrowIcon.Position = new Vector2(Bounds.Center.X,
                    Bounds.Top - 50 + (float) Math.Sin(Game.TotalGameTime*20f)*3f);
                m_arrowIcon.Draw(camera);
                m_arrowIcon.Visible = false;
            }
            base.Draw(camera);
        }

        public virtual void ForceOpen()
        {
            GoToFrame(2);
        }

        public virtual void ResetChest()
        {
            GoToFrame(1);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new ChestObj(PhysicsMngr);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var chestObj = obj as ChestObj;
            chestObj.IsProcedural = IsProcedural;
            chestObj.ChestType = ChestType;
            chestObj.Level = Level;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_arrowIcon.Dispose();
                m_arrowIcon = null;
                base.Dispose();
            }
        }
    }
}
