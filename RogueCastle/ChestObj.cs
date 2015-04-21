/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class ChestObj : PhysicsObj
	{
		private byte m_chestType;
		private float GoldIncreasePerLevel = 1.425f;
		private Vector2 BronzeChestGoldRange = new Vector2(9f, 14f);
		private Vector2 SilverChestGoldRange = new Vector2(20f, 28f);
		private Vector2 GoldChestGoldRange = new Vector2(47f, 57f);
		public int Level;
		private SpriteObj m_arrowIcon;
		public bool IsEmpty
		{
			get;
			set;
		}
		public bool IsLocked
		{
			get;
			set;
		}
		public int ForcedItemType
		{
			get;
			set;
		}
		public float ForcedAmount
		{
			get;
			set;
		}
		public bool IsProcedural
		{
			get;
			set;
		}
		public byte ChestType
		{
			get
			{
				return m_chestType;
			}
			set
			{
				m_chestType = value;
				bool isOpen = IsOpen;
				if (m_chestType == 5)
				{
					ForcedItemType = 14;
					ChangeSprite("BossChest_Sprite");
				}
				else if (m_chestType == 4)
				{
					ChangeSprite("Chest4_Sprite");
				}
				else if (m_chestType == 3)
				{
					ChangeSprite("Chest3_Sprite");
				}
				else if (m_chestType == 2)
				{
					ChangeSprite("Chest2_Sprite");
				}
				else
				{
					ChangeSprite("Chest1_Sprite");
				}
				if (isOpen)
				{
					GoToFrame(2);
				}
			}
		}
		public bool IsOpen
		{
			get
			{
				return CurrentFrame == 2;
			}
		}
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
		public virtual void OpenChest(ItemDropManager itemDropManager, PlayerObj player)
		{
			if (!IsOpen && !IsLocked)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
				GoToFrame(2);
				if (IsEmpty)
				{
					return;
				}
				if (ChestType == 3)
				{
					GameUtil.UnlockAchievement("LOVE_OF_GOLD");
				}
				if (ForcedItemType == 0)
				{
					int num = CDGMath.RandomInt(1, 100);
					int num2 = 0;
					int[] array;
					if (ChestType == 1)
					{
						array = GameEV.BRONZECHEST_ITEMDROP_CHANCE;
					}
					else if (ChestType == 2)
					{
						array = GameEV.SILVERCHEST_ITEMDROP_CHANCE;
					}
					else
					{
						array = GameEV.GOLDCHEST_ITEMDROP_CHANCE;
					}
					int num3 = 0;
					for (int i = 0; i < array.Length; i++)
					{
						num3 += array[i];
						if (num <= num3)
						{
							num2 = i;
							break;
						}
					}
					if (num2 == 0)
					{
						GiveGold(itemDropManager);
					}
					else if (num2 == 1)
					{
						GiveStatDrop(itemDropManager, player, 1, 0);
					}
					else
					{
						GivePrint(itemDropManager, player);
					}
				}
				else
				{
					switch (ForcedItemType)
					{
					case 1:
					case 10:
					case 11:
						GiveGold(itemDropManager, (int)ForcedAmount);
						break;
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
						GiveStatDrop(itemDropManager, player, 1, ForcedItemType);
						break;
					case 12:
					case 13:
						GivePrint(itemDropManager, player);
						break;
					case 14:
						GiveStatDrop(itemDropManager, player, 3, 0);
						break;
					case 15:
					case 16:
					case 17:
					case 18:
					case 19:
						GiveStatDrop(itemDropManager, player, 1, ForcedItemType);
						break;
					}
				}
				player.AttachedLevel.RefreshMapChestIcons();
			}
		}
		public void GiveGold(ItemDropManager itemDropManager, int amount = 0)
		{
			int num;
			if (ChestType == 1)
			{
				num = CDGMath.RandomInt((int)BronzeChestGoldRange.X, (int)BronzeChestGoldRange.Y) * 10;
			}
			else if (ChestType == 2 || ChestType == 4)
			{
				num = CDGMath.RandomInt((int)SilverChestGoldRange.X, (int)SilverChestGoldRange.Y) * 10;
			}
			else
			{
				num = CDGMath.RandomInt((int)GoldChestGoldRange.X, (int)GoldChestGoldRange.Y) * 10;
			}
			num += (int)Math.Floor(GoldIncreasePerLevel * Level * 10f);
			if (amount != 0)
			{
				num = amount;
			}
			int num2 = num / 500;
			num -= num2 * 500;
			int num3 = num / 100;
			num -= num3 * 100;
			int num4 = num / 10;
			float num5 = 0f;
			for (int i = 0; i < num2; i++)
			{
				Tween.To(this, num5, Linear.EaseNone);
				Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem", new Vector2(Position.X, Position.Y - Height / 2), 11, 500);
				num5 += 0.1f;
			}
			num5 = 0f;
			for (int j = 0; j < num3; j++)
			{
				Tween.To(this, num5, Linear.EaseNone);
				Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem", new Vector2(Position.X, Position.Y - Height / 2), 10, 100);
				num5 += 0.1f;
			}
			num5 = 0f;
			for (int k = 0; k < num4; k++)
			{
				Tween.To(this, num5, Linear.EaseNone);
				Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem", new Vector2(Position.X, Position.Y - Height / 2), 1, 10);
				num5 += 0.1f;
			}
		}
		public void GiveStatDrop(ItemDropManager manager, PlayerObj player, int numDrops, int statDropType)
		{
			int[] array = new int[numDrops];
			for (int i = 0; i < numDrops; i++)
			{
				if (statDropType == 0)
				{
					int num = CDGMath.RandomInt(1, 100);
					int num2 = 0;
					int j = 0;
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
						else
						{
							j++;
						}
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
			List<object> list = new List<object>();
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
				return;
			}
			else
			{
				List<byte[]> getBlueprintArray = Game.PlayerStats.GetBlueprintArray;
				List<Vector2> list = new List<Vector2>();
				int num = 0;
				foreach (byte[] current in getBlueprintArray)
				{
					int num2 = 0;
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] == 0)
						{
							EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(num, num2);
							if (Level >= equipmentData.LevelRequirement && ChestType >= equipmentData.ChestColourRequirement)
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
					Vector2 vector = list[CDGMath.RandomInt(0, list.Count - 1)];
					Game.PlayerStats.GetBlueprintArray[(int)vector.X][(int)vector.Y] = 1;
					List<object> list2 = new List<object>();
					list2.Add(new Vector2(X, Y - Height / 2f));
					list2.Add(1);
					list2.Add(new Vector2(vector.X, vector.Y));
					(player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list2);
					player.RunGetItemAnimation();
					Console.WriteLine(string.Concat("Unlocked item index ", vector.X, " of type ", vector.Y));
					return;
				}
				GiveGold(manager);
				return;
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
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
				m_arrowIcon.Position = new Vector2(Bounds.Center.X, Bounds.Top - 50 + (float)Math.Sin(Game.TotalGameTime * 20f) * 3f);
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
			ChestObj chestObj = obj as ChestObj;
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
