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
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class ProfileCardScreen : Screen
	{
		private ObjContainer m_frontCard;
		private ObjContainer m_backCard;
		private PlayerHUDObj m_playerHUD;
		private TextObj m_playerName;
		private TextObj m_money;
		private TextObj m_levelClass;
		private SpriteObj m_playerBG;
		private TextObj m_frontTrait1;
		private TextObj m_frontTrait2;
		private TextObj m_author;
		private TextObj m_playerStats;
		private SpriteObj m_spellIcon;
		private TextObj m_classDescription;
		private List<TextObj> m_dataList1;
		private List<TextObj> m_dataList2;
		private TextObj m_equipmentTitle;
		private TextObj m_runesTitle;
		private List<TextObj> m_equipmentList;
		private List<TextObj> m_runeBackTitleList;
		private List<TextObj> m_runeBackDescriptionList;
		private ObjContainer m_playerSprite;
		private SpriteObj m_tombStoneSprite;
		private bool m_playerInAir;
		private Color m_skinColour1 = new Color(231, 175, 131, 255);
		private Color m_skinColour2 = new Color(199, 109, 112, 255);
		private Color m_lichColour1 = new Color(255, 255, 255, 255);
		private Color m_lichColour2 = new Color(198, 198, 198, 255);
		private KeyIconTextObj m_cancelText;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public ProfileCardScreen()
		{
			m_equipmentList = new List<TextObj>();
			m_runeBackTitleList = new List<TextObj>();
			m_runeBackDescriptionList = new List<TextObj>();
		}
		public override void LoadContent()
		{
			m_frontCard = new ObjContainer("CardFront_Character");
			m_frontCard.ForceDraw = true;
			m_frontCard.Position = new Vector2(145f, 30f);
			m_frontCard.GetChildAt(0).TextureColor = Color.Red;
			m_frontCard.GetChildAt(2).TextureColor = Color.Red;
			LoadFrontCard();
			m_backCard = new ObjContainer("CardBack_Character");
			m_backCard.ForceDraw = true;
			m_backCard.Position = new Vector2(m_frontCard.X + m_backCard.Width + 100f, m_frontCard.Y);
			m_backCard.AddChild(m_playerName.Clone() as GameObj);
			m_backCard.GetChildAt(0).TextureColor = Color.Red;
			m_backCard.GetChildAt(2).TextureColor = Color.Red;
			LoadBackCard();
			m_playerSprite = new ObjContainer("PlayerIdle_Character");
			m_playerSprite.ForceDraw = true;
			m_playerSprite.Scale = new Vector2(2f, 2f);
			m_playerSprite.OutlineWidth = 2;
			m_tombStoneSprite = new SpriteObj("Tombstone_Sprite");
			m_tombStoneSprite.ForceDraw = true;
			m_tombStoneSprite.Scale = new Vector2(3f, 3f);
			m_tombStoneSprite.OutlineWidth = 2;
			m_spellIcon = new SpriteObj(SpellType.Icon(12));
			m_spellIcon.Position = new Vector2(350f, 295f);
			m_spellIcon.OutlineWidth = 2;
			m_spellIcon.ForceDraw = true;
			m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
			m_cancelText.Text = "to exit options";
			m_cancelText.Align = Types.TextAlign.Right;
			m_cancelText.DropShadow = new Vector2(2f, 2f);
			m_cancelText.FontSize = 12f;
			m_cancelText.Position = new Vector2(1290f, 650f);
			m_cancelText.ForceDraw = true;
			base.LoadContent();
		}
		private void LoadFrontCard()
		{
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Text = "";
			textObj.FontSize = 10f;
			textObj.ForceDraw = true;
			textObj.TextureColor = Color.Black;
			m_playerName = (textObj.Clone() as TextObj);
			m_playerName.Text = "Sir Archibald the IV";
			m_playerName.Position = new Vector2(50f, 43f);
			m_frontCard.AddChild(m_playerName);
			m_money = (textObj.Clone() as TextObj);
			m_money.Position = new Vector2(m_frontCard.GetChildAt(3).X + 30f, m_playerName.Y);
			m_money.Text = "0";
			m_frontCard.AddChild(m_money);
			m_levelClass = (textObj.Clone() as TextObj);
			m_levelClass.Text = "Lvl 43 Knight";
			m_levelClass.Position = new Vector2(m_playerName.X, 370f);
			m_frontCard.AddChild(m_levelClass);
			m_playerBG = new SpriteObj("CardDungeonBG_Sprite");
			m_playerBG.Position = new Vector2(45f, 80f);
			m_frontCard.AddChildAt(1, m_playerBG);
			m_playerHUD = new PlayerHUDObj();
			m_playerHUD.ForceDraw = true;
			m_playerHUD.ShowBarsOnly = true;
			m_playerHUD.SetPosition(new Vector2(m_frontCard.X + 46f, m_frontCard.Y + 64f));
			m_frontCard.AddChild(m_playerHUD);
			m_frontTrait1 = new TextObj(Game.JunicodeFont);
			m_frontTrait1.FontSize = 7f;
			m_frontTrait1.TextureColor = Color.Black;
			m_frontTrait1.Position = new Vector2(50f, 550f);
			m_frontTrait1.Text = "Color Blind";
			m_frontCard.AddChild(m_frontTrait1);
			m_frontTrait2 = (m_frontTrait1.Clone() as TextObj);
			m_frontTrait2.Y -= 20f;
			m_frontTrait2.Text = "Myopic";
			m_frontCard.AddChild(m_frontTrait2);
			m_classDescription = new TextObj(Game.JunicodeFont);
			m_classDescription.FontSize = 8f;
			m_classDescription.TextureColor = Color.Black;
			m_classDescription.Text = "0";
			m_classDescription.Position = new Vector2(50f, 410f);
			m_frontCard.AddChild(m_classDescription);
			m_author = new TextObj(Game.JunicodeFont);
			m_author.FontSize = 8f;
			m_author.TextureColor = Color.White;
			m_author.Text = "Glauber Kotaki";
			m_author.X = m_playerName.X;
			m_author.Y = 590f;
			m_frontCard.AddChild(m_author);
			m_playerStats = (textObj.Clone() as TextObj);
			m_playerStats.Text = "10/10";
			m_playerStats.Align = Types.TextAlign.Centre;
			m_playerStats.Position = new Vector2(387f, 579f);
			m_frontCard.AddChild(m_playerStats);
		}
		private void LoadBackCard()
		{
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Text = "";
			textObj.FontSize = 9f;
			textObj.ForceDraw = true;
			textObj.TextureColor = Color.Black;
			m_dataList1 = new List<TextObj>();
			m_dataList2 = new List<TextObj>();
			string[] array = new[]
			{
				"Health",
				"Mana",
				"Armor",
				"Weight"
			};
			string[] array2 = new[]
			{
				"Strength",
				"Intelligence",
				"Crit. Chance",
				"Crit. Damage"
			};
			int num = 90;
			for (int i = 0; i < array.Length; i++)
			{
				TextObj textObj2 = textObj.Clone() as TextObj;
				textObj2.Align = Types.TextAlign.Right;
				textObj2.Text = array[i];
				textObj2.Position = new Vector2(120f, num);
				m_backCard.AddChild(textObj2);
				TextObj textObj3 = textObj.Clone() as TextObj;
				textObj3.Text = "0";
				textObj3.Position = new Vector2(textObj2.X + 20f, num);
				m_dataList1.Add(textObj3);
				m_backCard.AddChild(textObj3);
				TextObj textObj4 = textObj.Clone() as TextObj;
				textObj4.Align = Types.TextAlign.Right;
				textObj4.Text = array2[i];
				textObj4.Position = new Vector2(330f, num);
				m_backCard.AddChild(textObj4);
				TextObj textObj5 = textObj.Clone() as TextObj;
				textObj5.Text = "0";
				textObj5.Position = new Vector2(textObj4.X + 20f, num);
				m_dataList2.Add(textObj5);
				m_backCard.AddChild(textObj5);
				num += 20;
			}
			m_equipmentTitle = (textObj.Clone() as TextObj);
			m_equipmentTitle.FontSize = 12f;
			m_equipmentTitle.Text = "Equipment:";
			m_equipmentTitle.Position = new Vector2(50f, 180f);
			m_backCard.AddChild(m_equipmentTitle);
			m_runesTitle = (textObj.Clone() as TextObj);
			m_runesTitle.FontSize = 12f;
			m_runesTitle.Text = "Enchantments:";
			m_runesTitle.Position = new Vector2(m_equipmentTitle.X, 330f);
			m_backCard.AddChild(m_runesTitle);
			for (int j = 0; j < Game.PlayerStats.GetEquippedArray.Length; j++)
			{
				TextObj textObj6 = textObj.Clone() as TextObj;
				textObj6.Text = "test";
				textObj6.Position = new Vector2(80f, m_equipmentTitle.Y + 50f);
				m_equipmentList.Add(textObj6);
				m_backCard.AddChild(textObj6);
			}
			for (int k = 0; k < 10; k++)
			{
				TextObj textObj7 = textObj.Clone() as TextObj;
				textObj7.X = 60f;
				textObj7.Text = EquipmentAbilityType.ToString(k);
				textObj7.FontSize = 7f;
				m_runeBackTitleList.Add(textObj7);
				m_backCard.AddChild(textObj7);
				TextObj textObj8 = textObj.Clone() as TextObj;
				textObj8.X = textObj7.Bounds.Right + 10;
				textObj8.FontSize = 7f;
				m_runeBackDescriptionList.Add(textObj8);
				m_backCard.AddChild(textObj8);
			}
			TextObj textObj9 = textObj.Clone() as TextObj;
			textObj9.X = 60f;
			textObj9.Text = EquipmentAbilityType.ToString(20);
			textObj9.FontSize = 7f;
			m_runeBackTitleList.Add(textObj9);
			m_backCard.AddChild(textObj9);
			TextObj textObj10 = textObj.Clone() as TextObj;
			textObj10.X = textObj9.Bounds.Right + 10;
			textObj10.FontSize = 7f;
			m_runeBackDescriptionList.Add(textObj10);
			m_backCard.AddChild(textObj10);
			TextObj textObj11 = textObj.Clone() as TextObj;
			textObj11.X = 60f;
			textObj11.Text = EquipmentAbilityType.ToString(21);
			textObj11.FontSize = 7f;
			m_runeBackTitleList.Add(textObj11);
			m_backCard.AddChild(textObj11);
			TextObj textObj12 = textObj.Clone() as TextObj;
			textObj12.X = textObj11.Bounds.Right + 10;
			textObj12.FontSize = 7f;
			m_runeBackDescriptionList.Add(textObj12);
			m_backCard.AddChild(textObj12);
		}
		public override void OnEnter()
		{
			SoundManager.PlaySound("StatCard_In");
			LoadCardColour();
			m_spellIcon.ChangeSprite(SpellType.Icon(Game.PlayerStats.Spell));
			string[] array = new[]
			{
				"CardCastleBG_Sprite",
				"CardGardenBG_Sprite",
				"CardDungeonBG_Sprite",
				"CardTowerBG_Sprite"
			};
			m_playerBG.ChangeSprite(array[CDGMath.RandomInt(0, 3)]);
			m_frontCard.Y = 1500f;
			m_backCard.Y = 1500f;
			Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.7");
			Tween.To(m_frontCard, 0.4f, Back.EaseOut, "Y", "30");
			Tween.To(m_backCard, 0.4f, Back.EaseOut, "delay", "0.2", "Y", "30");
			PlayerObj player = (ScreenManager as RCScreenManager).Player;
			LoadFrontCardStats(player);
			LoadBackCardStats(player);
			ChangeParts(player);
			m_playerHUD.Update(player);
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
			}
			else
			{
				m_cancelText.ForcedScale = new Vector2(1f, 1f);
			}
			m_cancelText.Text = "[Input:" + 2 + "] to exit profile card";
			m_cancelText.Opacity = 0f;
			Tween.To(m_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
			string[] array2 = new[]
			{
				"Glauber Kotaki",
				"Kenny Lee",
				"Teddy Lee",
				"Gordon McGladdery",
				"Judson Cowan"
			};
			m_author.Text = array2[CDGMath.RandomInt(0, array2.Length - 1)];
			Array.Clear(array2, 0, array2.Length);
			base.OnEnter();
		}
		private void ChangeParts(PlayerObj player)
		{
			string[] array;
			if (Game.PlayerStats.Class == 16)
			{
				array = new[]
				{
					"Idle",
					"Walking",
					"LevelUp",
					"Dash",
					"FrontDash"
				};
			}
			else
			{
				array = new[]
				{
					"Idle",
					"Attacking3",
					"Walking",
					"LevelUp",
					"Dash",
					"FrontDash"
				};
			}
			string[] array2;
			if (Game.PlayerStats.Class == 16)
			{
				array2 = new[]
				{
					"Jumping",
					"Falling"
				};
			}
			else
			{
				array2 = new[]
				{
					"Jumping",
					"AirAttack",
					"Falling"
				};
			}
			if (CDGMath.RandomInt(0, 1) == 0)
			{
				m_playerInAir = true;
				SetPlayerStyle(array2[CDGMath.RandomInt(0, array2.Length - 1)]);
			}
			else
			{
				m_playerInAir = false;
				SetPlayerStyle(array[CDGMath.RandomInt(0, array.Length - 1)]);
			}
			for (int i = 0; i < player.NumChildren; i++)
			{
				SpriteObj spriteObj = player.GetChildAt(i) as SpriteObj;
				SpriteObj spriteObj2 = m_playerSprite.GetChildAt(i) as SpriteObj;
				spriteObj2.TextureColor = spriteObj.TextureColor;
			}
			string text = (m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
			int startIndex = text.IndexOf("_") - 1;
			text = text.Remove(startIndex, 1);
			if (Game.PlayerStats.Class == 16)
			{
				text = text.Replace("_", 6 + "_");
			}
			else if (Game.PlayerStats.Class == 17)
			{
				text = text.Replace("_", 7 + "_");
			}
			else
			{
				text = text.Replace("_", Game.PlayerStats.HeadPiece + "_");
			}
			m_playerSprite.GetChildAt(12).ChangeSprite(text);
			string text2 = (m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
			startIndex = text2.IndexOf("_") - 1;
			text2 = text2.Remove(startIndex, 1);
			text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
			m_playerSprite.GetChildAt(4).ChangeSprite(text2);
			string text3 = (m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
			startIndex = text3.IndexOf("_") - 1;
			text3 = text3.Remove(startIndex, 1);
			text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			m_playerSprite.GetChildAt(9).ChangeSprite(text3);
			string text4 = (m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
			startIndex = text4.IndexOf("_") - 1;
			text4 = text4.Remove(startIndex, 1);
			text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			m_playerSprite.GetChildAt(3).ChangeSprite(text4);
		}
		public void SetPlayerStyle(string animationType)
		{
			m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
			PlayerObj player = (ScreenManager as RCScreenManager).Player;
			for (int i = 0; i < m_playerSprite.NumChildren; i++)
			{
				m_playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
				m_playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
			}
			m_playerSprite.GetChildAt(16).Visible = false;
			m_playerSprite.Scale = player.Scale;
			if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
			{
				m_playerSprite.GetChildAt(7).Visible = false;
			}
			m_playerSprite.GetChildAt(14).Visible = false;
			if (Game.PlayerStats.SpecialItem == 8)
			{
				m_playerSprite.GetChildAt(14).Visible = true;
			}
			if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Shield_Sprite");
			}
			else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Lamp_Sprite");
			}
			else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Beard_Sprite");
			}
			else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Headband_Sprite");
			}
			else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Horns_Sprite");
			}
			else
			{
				m_playerSprite.GetChildAt(15).Visible = false;
			}
			m_playerSprite.GetChildAt(0).Visible = false;
			if (Game.PlayerStats.Class == 16)
			{
				m_playerSprite.GetChildAt(0).Visible = true;
				m_playerSprite.GetChildAt(12).ChangeSprite(string.Concat("Player", animationType, "Head", 6, "_Sprite"));
			}
			if (!Game.PlayerStats.IsFemale)
			{
				m_playerSprite.GetChildAt(5).Visible = false;
				m_playerSprite.GetChildAt(13).Visible = false;
			}
			else
			{
				m_playerSprite.GetChildAt(5).Visible = true;
				m_playerSprite.GetChildAt(13).Visible = true;
			}
			if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
			{
				m_playerSprite.Scale = new Vector2(3f, 3f);
			}
			if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
			{
				m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
			}
			if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
			{
				m_playerSprite.ScaleX *= 0.825f;
				m_playerSprite.ScaleY *= 1.25f;
			}
			if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
			{
				m_playerSprite.ScaleX *= 1.25f;
				m_playerSprite.ScaleY *= 1.175f;
			}
			if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
			{
				m_playerSprite.OutlineColour = Color.White;
			}
			else
			{
				m_playerSprite.OutlineColour = Color.Black;
			}
			m_playerSprite.CalculateBounds();
			m_playerSprite.Y = 435f - (m_playerSprite.Bounds.Bottom - m_playerSprite.Y);
		}
		public void ExitScreenTransition()
		{
			SoundManager.PlaySound("StatCard_Out");
			Tween.To(m_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
			m_frontCard.Y = 30f;
			m_backCard.Y = 30f;
			Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.3", "BackBufferOpacity", "0");
			Tween.To(m_frontCard, 0.4f, Back.EaseIn, "Y", "1500");
			Tween.To(m_backCard, 0.4f, Back.EaseIn, "delay", "0.2", "Y", "1500");
			Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
			OnExit();
		}
		private void LoadCardColour()
		{
			Color textureColor = Color.Red;
			Color textureColor2 = Color.Red;
			switch (Game.PlayerStats.Class)
			{
			case 0:
			case 8:
				textureColor2 = (textureColor = Color.White);
				break;
			case 1:
			case 9:
				textureColor2 = (textureColor = Color.Blue);
				break;
			case 2:
			case 10:
				textureColor2 = (textureColor = Color.Red);
				break;
			case 3:
			case 11:
				textureColor2 = (textureColor = Color.Green);
				break;
			case 4:
			case 12:
				textureColor2 = (textureColor = Color.Gray);
				break;
			case 5:
			case 13:
				textureColor2 = (textureColor = Color.Gold);
				break;
			case 6:
			case 14:
				textureColor = Color.Blue;
				textureColor2 = Color.Red;
				break;
			case 7:
			case 15:
				textureColor2 = (textureColor = Color.Black);
				break;
			case 16:
				textureColor = Color.White;
				textureColor2 = Color.Green;
				break;
			}
			m_frontCard.GetChildAt(0).TextureColor = textureColor;
			m_frontCard.GetChildAt(3).TextureColor = textureColor;
			m_backCard.GetChildAt(0).TextureColor = textureColor2;
			m_backCard.GetChildAt(2).TextureColor = textureColor2;
			m_frontCard.GetChildAt(2).TextureColor = new Color(235, 220, 185);
			m_backCard.GetChildAt(1).TextureColor = new Color(235, 220, 185);
		}
		private void LoadFrontCardStats(PlayerObj player)
		{
			m_frontTrait1.Visible = false;
			m_frontTrait2.Visible = false;
			byte b = (byte)Game.PlayerStats.Traits.X;
			if (b != 0)
			{
				m_frontTrait1.Text = TraitType.ToString(b) + ": " + TraitType.ProfileCardDescription(b);
				m_frontTrait1.Visible = true;
			}
			byte b2 = (byte)Game.PlayerStats.Traits.Y;
			if (b2 != 0)
			{
				m_frontTrait2.Y = m_frontTrait1.Y;
				if (b != 0)
				{
					m_frontTrait2.Y -= 20f;
				}
				m_frontTrait2.Text = TraitType.ToString(b2) + ": " + TraitType.ProfileCardDescription(b2);
				m_frontTrait2.Visible = true;
			}
			m_playerName.Text = Game.PlayerStats.PlayerName;
			m_playerStats.Text = (int)(player.Damage / 20f) + "/" + (int)(player.MaxHealth / 50f);
			m_levelClass.Text = string.Concat("Lv. ", Game.PlayerStats.CurrentLevel, " - ", ClassType.ToString(Game.PlayerStats.Class, Game.PlayerStats.IsFemale));
			m_money.Text = Game.PlayerStats.Gold.ToString();
			m_classDescription.Text = ClassType.ProfileCardDescription(Game.PlayerStats.Class);
		}
		private void LoadBackCardStats(PlayerObj player)
		{
			for (int i = 0; i < m_dataList1.Count; i++)
			{
				switch (i)
				{
				case 0:
					m_dataList1[i].Text = player.MaxHealth.ToString();
					m_dataList2[i].Text = player.Damage.ToString();
					break;
				case 1:
					m_dataList1[i].Text = player.MaxMana.ToString();
					m_dataList2[i].Text = player.TotalMagicDamage.ToString();
					break;
				case 2:
				{
					m_dataList1[i].Text = string.Concat(player.TotalArmor.ToString(), "(", (int)(player.TotalDamageReduc * 100f), "%)");
					float num = player.TotalCritChance * 100f;
					m_dataList2[i].Text = ((int)Math.Round(num, MidpointRounding.AwayFromZero)).ToString() + "%";
					break;
				}
				case 3:
					m_dataList1[i].Text = player.CurrentWeight + "/" + player.MaxWeight;
					m_dataList2[i].Text = ((int)(player.TotalCriticalDamage * 100f)).ToString() + "%";
					break;
				}
			}
			sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
			int num2 = (int)m_equipmentTitle.Y + 40;
			for (int j = 0; j < Game.PlayerStats.GetEquippedArray.Length; j++)
			{
				m_equipmentList[j].Visible = false;
				m_equipmentList[j].Y = num2;
				if (getEquippedArray[j] != -1)
				{
					m_equipmentList[j].Text = EquipmentBaseType.ToString(getEquippedArray[j]) + " " + EquipmentCategoryType.ToString2(j);
					m_equipmentList[j].Visible = true;
					num2 += 20;
				}
			}
			num2 = (int)m_runesTitle.Y + 40;
			for (int k = 0; k < m_runeBackTitleList.Count; k++)
			{
				m_runeBackTitleList[k].Y = num2;
				m_runeBackDescriptionList[k].Y = num2;
				m_runeBackTitleList[k].Visible = false;
				m_runeBackDescriptionList[k].Visible = false;
				float num3 = 0f;
				switch (k)
				{
				case 0:
					num3 = player.TotalDoubleJumps;
					break;
				case 1:
					num3 = player.TotalAirDashes;
					break;
				case 2:
					num3 = player.TotalVampBonus;
					break;
				case 3:
					num3 = player.TotalFlightTime;
					break;
				case 4:
					num3 = player.ManaGain;
					break;
				case 5:
					num3 = player.TotalDamageReturn * 100f;
					break;
				case 6:
					num3 = player.TotalGoldBonus * 100f;
					break;
				case 7:
					num3 = player.TotalMovementSpeedPercent * 100f - 100f;
					break;
				case 8:
					num3 = Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8;
					break;
				case 9:
					num3 = Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f;
					break;
				}
				if (num3 > 0f)
				{
					m_runeBackDescriptionList[k].Text = "(" + EquipmentAbilityType.ShortDescription(k, num3) + ")";
					m_runeBackTitleList[k].Visible = true;
					m_runeBackDescriptionList[k].Visible = true;
					num2 += 20;
				}
			}
			if (Game.PlayerStats.HasArchitectFee)
			{
				m_runeBackDescriptionList[m_runeBackDescriptionList.Count - 2].Text = "(" + EquipmentAbilityType.ShortDescription(20, 0f) + ")";
				m_runeBackDescriptionList[m_runeBackDescriptionList.Count - 2].Visible = true;
				m_runeBackTitleList[m_runeBackDescriptionList.Count - 2].Visible = true;
				num2 += 20;
			}
			if (Game.PlayerStats.TimesCastleBeaten > 0)
			{
				m_runeBackDescriptionList[m_runeBackDescriptionList.Count - 1].Text = "(" + EquipmentAbilityType.ShortDescription(21, 50 * Game.PlayerStats.TimesCastleBeaten) + ")";
				m_runeBackDescriptionList[m_runeBackDescriptionList.Count - 1].Visible = true;
				m_runeBackTitleList[m_runeBackDescriptionList.Count - 1].Visible = true;
				if (Game.PlayerStats.HasArchitectFee)
				{
					m_runeBackDescriptionList[m_runeBackDescriptionList.Count - 1].Y = num2;
					m_runeBackTitleList[m_runeBackDescriptionList.Count - 1].Y = num2;
				}
			}
			(m_backCard.GetChildAt(3) as TextObj).Text = Game.PlayerStats.PlayerName;
		}
		public override void HandleInput()
		{
			if (m_frontCard.Y == 30f && (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) || Game.GlobalInput.JustPressed(7)))
			{
				ExitScreenTransition();
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gametime)
		{
			m_playerHUD.SetPosition(new Vector2(m_frontCard.X + 46f, m_frontCard.Y + 64f));
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
			m_frontCard.Draw(Camera);
			m_backCard.Draw(Camera);
			m_cancelText.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			if (Game.PlayerStats.IsDead)
			{
				m_tombStoneSprite.Position = new Vector2(m_frontCard.X + 240f, m_frontCard.Y + 280f);
				m_tombStoneSprite.Draw(Camera);
			}
			else
			{
				if (m_playerInAir)
				{
					m_playerSprite.Position = new Vector2(m_frontCard.X + 180f, m_frontCard.Y + 202f);
				}
				else
				{
					m_playerSprite.Position = new Vector2(m_frontCard.X + 160f, m_frontCard.Y + 280f - (m_playerSprite.Bounds.Bottom - m_playerSprite.Y));
				}
				m_playerSprite.Draw(Camera);
				Game.ColourSwapShader.Parameters["desiredTint"].SetValue(m_playerSprite.GetChildAt(12).TextureColor.ToVector4());
				if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
				{
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_lichColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_lichColour2.ToVector4());
				}
				else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
				{
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
				}
				else
				{
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_skinColour2.ToVector4());
				}
				Camera.End();
				Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
				m_playerSprite.GetChildAt(12).Draw(Camera);
				Camera.End();
				Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
				if (Game.PlayerStats.IsFemale)
				{
					m_playerSprite.GetChildAt(13).Draw(Camera);
				}
				m_playerSprite.GetChildAt(15).Draw(Camera);
			}
			m_spellIcon.Position = new Vector2(m_frontCard.X + 380f, m_frontCard.Y + 320f);
			m_spellIcon.Draw(Camera);
			Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Profile Card Screen");
				m_frontCard.Dispose();
				m_frontCard = null;
				m_backCard.Dispose();
				m_backCard = null;
				m_playerName = null;
				m_money = null;
				m_levelClass = null;
				m_playerHUD = null;
				m_frontTrait1 = null;
				m_frontTrait2 = null;
				m_playerBG = null;
				m_classDescription = null;
				m_author = null;
				m_playerStats = null;
				m_equipmentTitle = null;
				m_runesTitle = null;
				m_equipmentList.Clear();
				m_equipmentList = null;
				m_runeBackTitleList.Clear();
				m_runeBackTitleList = null;
				m_runeBackDescriptionList.Clear();
				m_runeBackDescriptionList = null;
				m_playerSprite.Dispose();
				m_playerSprite = null;
				m_spellIcon.Dispose();
				m_spellIcon = null;
				m_tombStoneSprite.Dispose();
				m_tombStoneSprite = null;
				m_cancelText.Dispose();
				m_cancelText = null;
				m_dataList1.Clear();
				m_dataList1 = null;
				m_dataList2.Clear();
				m_dataList2 = null;
				base.Dispose();
			}
		}
	}
}
