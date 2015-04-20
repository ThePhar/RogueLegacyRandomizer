/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class ItemDropObj : PhysicsObj
	{
		public int DropType;
		private float m_amount;
		public float CollectionCounter
		{
			get;
			set;
		}
		public bool IsCollectable
		{
			get
			{
				return CollectionCounter <= 0f;
			}
		}
		public ItemDropObj(string spriteName) : base(spriteName, null)
		{
			IsCollidable = true;
			IsWeighted = true;
			CollisionTypeTag = 3;
			StopAnimation();
			OutlineWidth = 2;
		}
		public void ConvertDrop(int dropType, float amount)
		{
			switch (dropType)
			{
			case 2:
				ChangeSprite("ChickenLeg_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			case 3:
				ChangeSprite("ManaPotion_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			case 4:
				ChangeSprite("Sword_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			case 6:
				ChangeSprite("Shield_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			case 7:
				ChangeSprite("Heart_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			case 8:
				ChangeSprite("Heart_Sprite");
				PlayAnimation(true);
				TextureColor = Color.Blue;
				goto IL_11A;
			case 9:
				ChangeSprite("Backpack_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			case 10:
				ChangeSprite("MoneyBag_Sprite");
				PlayAnimation(1, 1, false);
				goto IL_11A;
			case 11:
				ChangeSprite("Diamond_Sprite");
				PlayAnimation(true);
				goto IL_11A;
			}
			ChangeSprite("Coin_Sprite");
			PlayAnimation(true);
			IL_11A:
			DropType = dropType;
			m_amount = amount;
			ClearCollisionBoxes();
			AddCollisionBox(0, 0, Width, Height, 0);
		}
		public void GiveReward(PlayerObj player, TextManager textManager)
		{
			switch (DropType)
			{
			case 1:
			case 10:
			case 11:
			{
				player.AttachedLevel.ItemDropCollected(DropType);
				float num = 1f;
				if (Game.PlayerStats.HasArchitectFee)
				{
					num = 0.6f;
				}
				int num2 = (int)Math.Round(m_amount * (1f + player.TotalGoldBonus) * num, MidpointRounding.AwayFromZero);
				Game.PlayerStats.Gold += num2;
				textManager.DisplayNumberStringText(num2, "gold", Color.Yellow, new Vector2(X, Bounds.Top));
				if (DropType == 10)
				{
					PlayAnimation(1, 1, false);
					return;
				}
				break;
			}
			case 2:
			{
				int num3 = (int)(player.MaxHealth * (m_amount + SkillSystem.GetSkill(SkillType.Potion_Up).ModifierAmount));
				player.CurrentHealth += num3;
				textManager.DisplayNumberStringText(num3, "hp recovered", Color.LawnGreen, new Vector2(X, Bounds.Top));
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Collect_Health");
				return;
			}
			case 3:
			{
				int num4 = (int)(player.MaxMana * (m_amount + SkillSystem.GetSkill(SkillType.Potion_Up).ModifierAmount));
				player.CurrentMana += num4;
				textManager.DisplayNumberStringText(num4, "mp recovered", Color.LawnGreen, new Vector2(X, Bounds.Top));
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Collect_Mana");
				return;
			}
			case 4:
				Game.PlayerStats.BonusStrength++;
				textManager.DisplayStringNumberText("Attack Up", 1, Color.LightSteelBlue, new Vector2(X, Bounds.Top));
				return;
			case 5:
				Game.PlayerStats.BonusMagic++;
				textManager.DisplayStringNumberText("Magic Up", 1, Color.LightSteelBlue, new Vector2(X, Bounds.Top));
				return;
			case 6:
				Game.PlayerStats.BonusDefense++;
				textManager.DisplayStringNumberText("Armor Up", 2, Color.LightSteelBlue, new Vector2(X, Bounds.Top));
				return;
			case 7:
				Game.PlayerStats.BonusHealth++;
				textManager.DisplayStringNumberText("Max Health Up", 5, Color.LightSteelBlue, new Vector2(X, Bounds.Top));
				player.CurrentHealth += 5;
				return;
			case 8:
				Game.PlayerStats.BonusMana++;
				textManager.DisplayStringNumberText("Max Mana Up", 5, Color.LightSteelBlue, new Vector2(X, Bounds.Top));
				player.CurrentMana += 5f;
				return;
			case 9:
				Game.PlayerStats.BonusWeight++;
				textManager.DisplayStringNumberText("Max Weight Up", 5, Color.LightSteelBlue, new Vector2(X, Bounds.Top));
				break;
			default:
				return;
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (collisionResponseType == 1 && (otherBox.AbsParent is TerrainObj || otherBox.AbsParent is HazardObj) && !(otherBox.AbsParent is DoorObj))
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
				AccelerationX = 0f;
				Y = (int)Y;
				if (DropType == 10 && CurrentFrame == 1 && CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect).Y < 0f)
				{
					PlayAnimation(2, TotalFrames, false);
				}
			}
		}
		public override void Draw(Camera2D camera)
		{
			if (CollectionCounter > 0f)
			{
				CollectionCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Draw(camera);
		}
	}
}
