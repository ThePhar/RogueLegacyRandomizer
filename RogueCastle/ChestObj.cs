using System;
using System.Collections.Generic;
using Archipelago;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using Tweener;
using Tweener.Ease;

using Screen = RogueCastle.Enums.Screen;

namespace RogueCastle
{
    public class ChestObj : PhysicsObj
    {
        private readonly float GoldIncreasePerLevel = 1.425f;
        private readonly Vector2 BronzeChestGoldRange = new Vector2(9f, 14f);
        private readonly Vector2 GoldChestGoldRange = new Vector2(47f, 57f);
        public int Level;
        private SpriteObj m_arrowIcon;
        private byte m_chestType;
        private readonly Vector2 SilverChestGoldRange = new Vector2(20f, 28f);

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
                    case (byte) Chest.Boss:
                        ForcedItemType = 14;
                        ChangeSprite("BossChest_Sprite");
                        break;

                    case (byte) Chest.Fairy:
                        ChangeSprite("Chest4_Sprite");
                        break;

                    case (byte) Chest.Gold:
                        ChangeSprite("Chest3_Sprite");
                        break;

                    case (byte) Chest.Silver:
                        ChangeSprite("Chest2_Sprite");
                        break;

                    default:
                        ChangeSprite("Chest1_Sprite");
                        break;
                }

                if (isOpen)
                {
                    GoToFrame(2);
                }
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
            {
                return;
            }

            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
            GoToFrame(2);

            // If chest is empty, that's sad.
            if (IsEmpty)
            {
                return;
            }

            var randomInt = CDGMath.RandomInt(1, 100);
            var dropType = 0;
            int[] chances;

            switch (ChestType)
            {
                case (byte) Chest.Brown:
                    chances = GameEV.BRONZECHEST_ITEMDROP_CHANCE;
                    break;

                case (byte) Chest.Silver:
                    chances = GameEV.SILVERCHEST_ITEMDROP_CHANCE;
                    break;

                default:
                    chances = GameEV.GOLDCHEST_ITEMDROP_CHANCE;
                    break;
            }

            var threshold = 0;
            for (var i = 0; i < chances.Length; i++)
            {
                threshold += chances[i];
                if (randomInt <= threshold)
                {
                    dropType = i;
                    break;
                }
            }

            if (ChestType ==(byte)  Chest.Boss)
            {
                GiveStatDrop(itemDropManager, player, 3, 0);
                return;
            }

            GiveNetworkItem(itemDropManager, player);
            player.AttachedLevel.RefreshMapChestIcons();
        }

        public void GiveGold(ItemDropManager itemDropManager, int amount = 0)
        {
            int num;
            if (ChestType == 1)
            {
                num = CDGMath.RandomInt((int) BronzeChestGoldRange.X, (int) BronzeChestGoldRange.Y) * 10;
            }
            else if (ChestType == 2 || ChestType == 4)
            {
                num = CDGMath.RandomInt((int) SilverChestGoldRange.X, (int) SilverChestGoldRange.Y) * 10;
            }
            else
            {
                num = CDGMath.RandomInt((int) GoldChestGoldRange.X, (int) GoldChestGoldRange.Y) * 10;
            }

            num += (int) Math.Floor(GoldIncreasePerLevel * Level * 10f);
            if (amount != 0)
            {
                num = amount;
            }

            var num2 = num / 500;
            num -= num2 * 500;
            var num3 = num / 100;
            num -= num3 * 100;
            var num4 = num / 10;
            var num5 = 0f;
            for (var i = 0; i < num2; i++)
            {
                Tween.To(this, num5, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                    new Vector2(Position.X, Position.Y - Height / 2), 11, 500);
                num5 += 0.1f;
            }

            num5 = 0f;
            for (var j = 0; j < num3; j++)
            {
                Tween.To(this, num5, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                    new Vector2(Position.X, Position.Y - Height / 2), 10, 100);
                num5 += 0.1f;
            }

            num5 = 0f;
            for (var k = 0; k < num4; k++)
            {
                Tween.To(this, num5, Linear.EaseNone);
                Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem",
                    new Vector2(Position.X, Position.Y - Height / 2), 1, 10);
                num5 += 0.1f;
            }
        }

        public void GiveStatDrop(ItemDropManager manager, PlayerObj player, int numDrops, int statDropType)
        {
            var array = new int[numDrops];
            for (var i = 0; i < numDrops; i++)
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

            var list = new List<object>();
            list.Add(new Vector2(X, Y - Height / 2f));
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

        protected void GiveNetworkItem(ItemDropManager manager, PlayerObj player, bool isFairy = false)
        {
            var arch = Program.Game.ArchipelagoManager;
            var room = Game.ScreenManager.GetLevelScreen().CurrentRoom;
            var location = "";

            if (ForcedItemType == 1)
            {
                GiveGold(manager);
                return;
            }

            if (isFairy)
            {
                var total = 0;

                switch (room.Zone)
                {
                    case Zone.None:
                    case Zone.Castle:
                        total = ++Game.PlayerStats.OpenedChests.CastleFairyChests;
                        location = string.Format("Castle Hamson - Fairy Chest {0}", total);
                        break;

                    case Zone.Garden:
                        total = ++Game.PlayerStats.OpenedChests.GardenFairyChests;
                        location = string.Format("Forest Abkhazia - Fairy Chest {0}", total);
                        break;

                    case Zone.Dungeon:
                        total = ++Game.PlayerStats.OpenedChests.DungeonFairyChests;
                        location = string.Format("The Land of Darkness - Fairy Chest {0}", total);
                        break;

                    case Zone.Tower:
                        total = ++Game.PlayerStats.OpenedChests.TowerFairyChests;
                        location = string.Format("The Maya - Fairy Chest {0}", total);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (total > arch.Data.FairyChestsPerZone)
                {
                    GiveStatDrop(manager, player, 1, 0);
                    return;
                }
            }
            else
            {
                var total = 0;

                switch (room.Zone)
                {
                    case Zone.None:
                    case Zone.Castle:
                        total = ++Game.PlayerStats.OpenedChests.CastleChests;
                        location = string.Format("Castle Hamson - Chest {0}", total);
                        break;

                    case Zone.Garden:
                        total = ++Game.PlayerStats.OpenedChests.GardenChests;
                        location = string.Format("Forest Abkhazia - Chest {0}", total);
                        break;

                    case Zone.Dungeon:
                        total = ++Game.PlayerStats.OpenedChests.DungeonChests;
                        location = string.Format("The Land of Darkness - Chest {0}", total);
                        break;

                    case Zone.Tower:
                        total = ++Game.PlayerStats.OpenedChests.TowerChests;
                        location = string.Format("The Maya - Chest {0}", total);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                GiveGold(manager);

                if (total > arch.Data.ChestsPerZone)
                {
                    return;
                }
            }

            var code = LocationDefinitions.GetLocation(Program.Game.ArchipelagoManager.Data, location).Code;
            var name = arch.GetPlayerName(arch.LocationCache[code].Player);
            var item = arch.LocationCache[code].Item;

            var networkItem = new List<object>
            {
                new Vector2(X, Y - Height / 2f),
                ItemCategory.GiveNetworkItem,
                new Vector2(-1f, -1f),
                new Vector2(-1f, -1f),
                name,
                item
            };

            Program.Game.ArchipelagoManager.CheckLocations(code);

            // If we're sending someone else something, let's show what we're sending.
            if (arch.LocationCache[code].Player != arch.Data.Slot)
            {
                Game.ScreenManager.DisplayScreen((int) Screen.GetItem, true, networkItem);
                player.RunGetItemAnimation();
            }
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
                    Bounds.Top - 50 + (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 3f);
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
