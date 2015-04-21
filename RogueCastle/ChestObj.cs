using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
				return this.m_chestType;
			}
			set
			{
				this.m_chestType = value;
				bool isOpen = this.IsOpen;
				if (this.m_chestType == 5)
				{
					this.ForcedItemType = 14;
					this.ChangeSprite("BossChest_Sprite");
				}
				else if (this.m_chestType == 4)
				{
					this.ChangeSprite("Chest4_Sprite");
				}
				else if (this.m_chestType == 3)
				{
					this.ChangeSprite("Chest3_Sprite");
				}
				else if (this.m_chestType == 2)
				{
					this.ChangeSprite("Chest2_Sprite");
				}
				else
				{
					this.ChangeSprite("Chest1_Sprite");
				}
				if (isOpen)
				{
					base.GoToFrame(2);
				}
			}
		}
		public bool IsOpen
		{
			get
			{
				return base.CurrentFrame == 2;
			}
		}
		public ChestObj(PhysicsManager physicsManager) : base("Chest1_Sprite", physicsManager)
		{
			base.DisableHitboxUpdating = true;
			base.IsWeighted = false;
			base.Layer = 1f;
			base.OutlineWidth = 2;
			this.IsProcedural = true;
			this.m_arrowIcon = new SpriteObj("UpArrowSquare_Sprite");
			this.m_arrowIcon.OutlineWidth = 2;
			this.m_arrowIcon.Visible = false;
		}
		public virtual void OpenChest(ItemDropManager itemDropManager, PlayerObj player)
		{
			if (!this.IsOpen && !this.IsLocked)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
				base.GoToFrame(2);
				if (this.IsEmpty)
				{
					return;
				}
				if (this.ChestType == 3)
				{
					GameUtil.UnlockAchievement("LOVE_OF_GOLD");
				}
				if (this.ForcedItemType == 0)
				{
					int num = CDGMath.RandomInt(1, 100);
					int num2 = 0;
					int[] array;
					if (this.ChestType == 1)
					{
						array = GameEV.BRONZECHEST_ITEMDROP_CHANCE;
					}
					else if (this.ChestType == 2)
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
						this.GiveGold(itemDropManager, 0);
					}
					else if (num2 == 1)
					{
						this.GiveStatDrop(itemDropManager, player, 1, 0);
					}
					else
					{
						this.GivePrint(itemDropManager, player);
					}
				}
				else
				{
					switch (this.ForcedItemType)
					{
					case 1:
					case 10:
					case 11:
						this.GiveGold(itemDropManager, (int)this.ForcedAmount);
						break;
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
						this.GiveStatDrop(itemDropManager, player, 1, this.ForcedItemType);
						break;
					case 12:
					case 13:
						this.GivePrint(itemDropManager, player);
						break;
					case 14:
						this.GiveStatDrop(itemDropManager, player, 3, 0);
						break;
					case 15:
					case 16:
					case 17:
					case 18:
					case 19:
						this.GiveStatDrop(itemDropManager, player, 1, this.ForcedItemType);
						break;
					}
				}
				player.AttachedLevel.RefreshMapChestIcons();
			}
		}
		public void GiveGold(ItemDropManager itemDropManager, int amount = 0)
		{
			int num;
			if (this.ChestType == 1)
			{
				num = CDGMath.RandomInt((int)this.BronzeChestGoldRange.X, (int)this.BronzeChestGoldRange.Y) * 10;
			}
			else if (this.ChestType == 2 || this.ChestType == 4)
			{
				num = CDGMath.RandomInt((int)this.SilverChestGoldRange.X, (int)this.SilverChestGoldRange.Y) * 10;
			}
			else
			{
				num = CDGMath.RandomInt((int)this.GoldChestGoldRange.X, (int)this.GoldChestGoldRange.Y) * 10;
			}
			num += (int)Math.Floor((double)(this.GoldIncreasePerLevel * (float)this.Level * 10f));
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
				Tween.To(this, num5, new Easing(Linear.EaseNone), new string[0]);
				Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem", new object[]
				{
					new Vector2(base.Position.X, base.Position.Y - (float)(this.Height / 2)),
					11,
					500
				});
				num5 += 0.1f;
			}
			num5 = 0f;
			for (int j = 0; j < num3; j++)
			{
				Tween.To(this, num5, new Easing(Linear.EaseNone), new string[0]);
				Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem", new object[]
				{
					new Vector2(base.Position.X, base.Position.Y - (float)(this.Height / 2)),
					10,
					100
				});
				num5 += 0.1f;
			}
			num5 = 0f;
			for (int k = 0; k < num4; k++)
			{
				Tween.To(this, num5, new Easing(Linear.EaseNone), new string[0]);
				Tween.AddEndHandlerToLastTween(itemDropManager, "DropItem", new object[]
				{
					new Vector2(base.Position.X, base.Position.Y - (float)(this.Height / 2)),
					1,
					10
				});
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
			list.Add(new Vector2(base.X, base.Y - (float)this.Height / 2f));
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
			list.Add(new Vector2((float)array[0], 0f));
			if (numDrops > 1)
			{
				list.Add(new Vector2((float)array[1], (float)array[2]));
			}
			player.AttachedLevel.UpdatePlayerHUD();
			(player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list);
			player.RunGetItemAnimation();
		}
		public void GivePrint(ItemDropManager manager, PlayerObj player)
		{
			if (Game.PlayerStats.TotalBlueprintsFound >= 75)
			{
				if (this.ChestType == 3)
				{
					this.GiveStatDrop(manager, player, 1, 0);
					return;
				}
				this.GiveGold(manager, 0);
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
							if (this.Level >= (int)equipmentData.LevelRequirement && this.ChestType >= equipmentData.ChestColourRequirement)
							{
								list.Add(new Vector2((float)num, (float)num2));
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
					list2.Add(new Vector2(base.X, base.Y - (float)this.Height / 2f));
					list2.Add(1);
					list2.Add(new Vector2(vector.X, vector.Y));
					(player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list2);
					player.RunGetItemAnimation();
					Console.WriteLine(string.Concat(new object[]
					{
						"Unlocked item index ",
						vector.X,
						" of type ",
						vector.Y
					}));
					return;
				}
				this.GiveGold(manager, 0);
				return;
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
			if (!this.IsLocked && !this.IsOpen && playerObj != null && playerObj.IsTouchingGround)
			{
				this.m_arrowIcon.Visible = true;
			}
			base.CollisionResponse(thisBox, otherBox, collisionResponseType);
		}
		public override void Draw(Camera2D camera)
		{
			if (this.m_arrowIcon.Visible)
			{
				this.m_arrowIcon.Position = new Vector2((float)this.Bounds.Center.X, (float)(this.Bounds.Top - 50) + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 3f);
				this.m_arrowIcon.Draw(camera);
				this.m_arrowIcon.Visible = false;
			}
			base.Draw(camera);
		}
		public virtual void ForceOpen()
		{
			base.GoToFrame(2);
		}
		public virtual void ResetChest()
		{
			base.GoToFrame(1);
		}
		protected override GameObj CreateCloneInstance()
		{
			return new ChestObj(base.PhysicsMngr);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			ChestObj chestObj = obj as ChestObj;
			chestObj.IsProcedural = this.IsProcedural;
			chestObj.ChestType = this.ChestType;
			chestObj.Level = this.Level;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_arrowIcon.Dispose();
				this.m_arrowIcon = null;
				base.Dispose();
			}
		}
	}
}
