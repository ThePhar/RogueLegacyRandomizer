using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
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
				return this.CollectionCounter <= 0f;
			}
		}
		public ItemDropObj(string spriteName) : base(spriteName, null)
		{
			base.IsCollidable = true;
			base.IsWeighted = true;
			base.CollisionTypeTag = 3;
			base.StopAnimation();
			base.OutlineWidth = 2;
		}
		public void ConvertDrop(int dropType, float amount)
		{
			switch (dropType)
			{
			case 2:
				this.ChangeSprite("ChickenLeg_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			case 3:
				this.ChangeSprite("ManaPotion_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			case 4:
				this.ChangeSprite("Sword_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			case 6:
				this.ChangeSprite("Shield_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			case 7:
				this.ChangeSprite("Heart_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			case 8:
				this.ChangeSprite("Heart_Sprite");
				base.PlayAnimation(true);
				base.TextureColor = Color.Blue;
				goto IL_11A;
			case 9:
				this.ChangeSprite("Backpack_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			case 10:
				this.ChangeSprite("MoneyBag_Sprite");
				base.PlayAnimation(1, 1, false);
				goto IL_11A;
			case 11:
				this.ChangeSprite("Diamond_Sprite");
				base.PlayAnimation(true);
				goto IL_11A;
			}
			this.ChangeSprite("Coin_Sprite");
			base.PlayAnimation(true);
			IL_11A:
			this.DropType = dropType;
			this.m_amount = amount;
			base.ClearCollisionBoxes();
			base.AddCollisionBox(0, 0, this.Width, this.Height, 0);
		}
		public void GiveReward(PlayerObj player, TextManager textManager)
		{
			switch (this.DropType)
			{
			case 1:
			case 10:
			case 11:
			{
				player.AttachedLevel.ItemDropCollected(this.DropType);
				float num = 1f;
				if (Game.PlayerStats.HasArchitectFee)
				{
					num = 0.6f;
				}
				int num2 = (int)Math.Round((double)(this.m_amount * (1f + player.TotalGoldBonus) * num), MidpointRounding.AwayFromZero);
				Game.PlayerStats.Gold += num2;
				textManager.DisplayNumberStringText(num2, "gold", Color.Yellow, new Vector2(base.X, (float)this.Bounds.Top));
				if (this.DropType == 10)
				{
					base.PlayAnimation(1, 1, false);
					return;
				}
				break;
			}
			case 2:
			{
				int num3 = (int)((float)player.MaxHealth * (this.m_amount + SkillSystem.GetSkill(SkillType.Potion_Up).ModifierAmount));
				player.CurrentHealth += num3;
				textManager.DisplayNumberStringText(num3, "hp recovered", Color.LawnGreen, new Vector2(base.X, (float)this.Bounds.Top));
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Collect_Health");
				return;
			}
			case 3:
			{
				int num4 = (int)(player.MaxMana * (this.m_amount + SkillSystem.GetSkill(SkillType.Potion_Up).ModifierAmount));
				player.CurrentMana += (float)num4;
				textManager.DisplayNumberStringText(num4, "mp recovered", Color.LawnGreen, new Vector2(base.X, (float)this.Bounds.Top));
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Collect_Mana");
				return;
			}
			case 4:
				Game.PlayerStats.BonusStrength++;
				textManager.DisplayStringNumberText("Attack Up", 1, Color.LightSteelBlue, new Vector2(base.X, (float)this.Bounds.Top));
				return;
			case 5:
				Game.PlayerStats.BonusMagic++;
				textManager.DisplayStringNumberText("Magic Up", 1, Color.LightSteelBlue, new Vector2(base.X, (float)this.Bounds.Top));
				return;
			case 6:
				Game.PlayerStats.BonusDefense++;
				textManager.DisplayStringNumberText("Armor Up", 2, Color.LightSteelBlue, new Vector2(base.X, (float)this.Bounds.Top));
				return;
			case 7:
				Game.PlayerStats.BonusHealth++;
				textManager.DisplayStringNumberText("Max Health Up", 5, Color.LightSteelBlue, new Vector2(base.X, (float)this.Bounds.Top));
				player.CurrentHealth += 5;
				return;
			case 8:
				Game.PlayerStats.BonusMana++;
				textManager.DisplayStringNumberText("Max Mana Up", 5, Color.LightSteelBlue, new Vector2(base.X, (float)this.Bounds.Top));
				player.CurrentMana += 5f;
				return;
			case 9:
				Game.PlayerStats.BonusWeight++;
				textManager.DisplayStringNumberText("Max Weight Up", 5, Color.LightSteelBlue, new Vector2(base.X, (float)this.Bounds.Top));
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
				base.AccelerationX = 0f;
				base.Y = (float)((int)base.Y);
				if (this.DropType == 10 && base.CurrentFrame == 1 && CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect).Y < 0f)
				{
					base.PlayAnimation(2, base.TotalFrames, false);
				}
			}
		}
		public override void Draw(Camera2D camera)
		{
			if (this.CollectionCounter > 0f)
			{
				this.CollectionCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Draw(camera);
		}
	}
}
