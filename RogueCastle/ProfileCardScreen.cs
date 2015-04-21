using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
			this.m_equipmentList = new List<TextObj>();
			this.m_runeBackTitleList = new List<TextObj>();
			this.m_runeBackDescriptionList = new List<TextObj>();
		}
		public override void LoadContent()
		{
			this.m_frontCard = new ObjContainer("CardFront_Character");
			this.m_frontCard.ForceDraw = true;
			this.m_frontCard.Position = new Vector2(145f, 30f);
			this.m_frontCard.GetChildAt(0).TextureColor = Color.Red;
			this.m_frontCard.GetChildAt(2).TextureColor = Color.Red;
			this.LoadFrontCard();
			this.m_backCard = new ObjContainer("CardBack_Character");
			this.m_backCard.ForceDraw = true;
			this.m_backCard.Position = new Vector2(this.m_frontCard.X + (float)this.m_backCard.Width + 100f, this.m_frontCard.Y);
			this.m_backCard.AddChild(this.m_playerName.Clone() as GameObj);
			this.m_backCard.GetChildAt(0).TextureColor = Color.Red;
			this.m_backCard.GetChildAt(2).TextureColor = Color.Red;
			this.LoadBackCard();
			this.m_playerSprite = new ObjContainer("PlayerIdle_Character");
			this.m_playerSprite.ForceDraw = true;
			this.m_playerSprite.Scale = new Vector2(2f, 2f);
			this.m_playerSprite.OutlineWidth = 2;
			this.m_tombStoneSprite = new SpriteObj("Tombstone_Sprite");
			this.m_tombStoneSprite.ForceDraw = true;
			this.m_tombStoneSprite.Scale = new Vector2(3f, 3f);
			this.m_tombStoneSprite.OutlineWidth = 2;
			this.m_spellIcon = new SpriteObj(SpellType.Icon(12));
			this.m_spellIcon.Position = new Vector2(350f, 295f);
			this.m_spellIcon.OutlineWidth = 2;
			this.m_spellIcon.ForceDraw = true;
			this.m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_cancelText.Text = "to exit options";
			this.m_cancelText.Align = Types.TextAlign.Right;
			this.m_cancelText.DropShadow = new Vector2(2f, 2f);
			this.m_cancelText.FontSize = 12f;
			this.m_cancelText.Position = new Vector2(1290f, 650f);
			this.m_cancelText.ForceDraw = true;
			base.LoadContent();
		}
		private void LoadFrontCard()
		{
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Text = "";
			textObj.FontSize = 10f;
			textObj.ForceDraw = true;
			textObj.TextureColor = Color.Black;
			this.m_playerName = (textObj.Clone() as TextObj);
			this.m_playerName.Text = "Sir Archibald the IV";
			this.m_playerName.Position = new Vector2(50f, 43f);
			this.m_frontCard.AddChild(this.m_playerName);
			this.m_money = (textObj.Clone() as TextObj);
			this.m_money.Position = new Vector2(this.m_frontCard.GetChildAt(3).X + 30f, this.m_playerName.Y);
			this.m_money.Text = "0";
			this.m_frontCard.AddChild(this.m_money);
			this.m_levelClass = (textObj.Clone() as TextObj);
			this.m_levelClass.Text = "Lvl 43 Knight";
			this.m_levelClass.Position = new Vector2(this.m_playerName.X, 370f);
			this.m_frontCard.AddChild(this.m_levelClass);
			this.m_playerBG = new SpriteObj("CardDungeonBG_Sprite");
			this.m_playerBG.Position = new Vector2(45f, 80f);
			this.m_frontCard.AddChildAt(1, this.m_playerBG);
			this.m_playerHUD = new PlayerHUDObj();
			this.m_playerHUD.ForceDraw = true;
			this.m_playerHUD.ShowBarsOnly = true;
			this.m_playerHUD.SetPosition(new Vector2(this.m_frontCard.X + 46f, this.m_frontCard.Y + 64f));
			this.m_frontCard.AddChild(this.m_playerHUD);
			this.m_frontTrait1 = new TextObj(Game.JunicodeFont);
			this.m_frontTrait1.FontSize = 7f;
			this.m_frontTrait1.TextureColor = Color.Black;
			this.m_frontTrait1.Position = new Vector2(50f, 550f);
			this.m_frontTrait1.Text = "Color Blind";
			this.m_frontCard.AddChild(this.m_frontTrait1);
			this.m_frontTrait2 = (this.m_frontTrait1.Clone() as TextObj);
			this.m_frontTrait2.Y -= 20f;
			this.m_frontTrait2.Text = "Myopic";
			this.m_frontCard.AddChild(this.m_frontTrait2);
			this.m_classDescription = new TextObj(Game.JunicodeFont);
			this.m_classDescription.FontSize = 8f;
			this.m_classDescription.TextureColor = Color.Black;
			this.m_classDescription.Text = "0";
			this.m_classDescription.Position = new Vector2(50f, 410f);
			this.m_frontCard.AddChild(this.m_classDescription);
			this.m_author = new TextObj(Game.JunicodeFont);
			this.m_author.FontSize = 8f;
			this.m_author.TextureColor = Color.White;
			this.m_author.Text = "Glauber Kotaki";
			this.m_author.X = this.m_playerName.X;
			this.m_author.Y = 590f;
			this.m_frontCard.AddChild(this.m_author);
			this.m_playerStats = (textObj.Clone() as TextObj);
			this.m_playerStats.Text = "10/10";
			this.m_playerStats.Align = Types.TextAlign.Centre;
			this.m_playerStats.Position = new Vector2(387f, 579f);
			this.m_frontCard.AddChild(this.m_playerStats);
		}
		private void LoadBackCard()
		{
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Text = "";
			textObj.FontSize = 9f;
			textObj.ForceDraw = true;
			textObj.TextureColor = Color.Black;
			this.m_dataList1 = new List<TextObj>();
			this.m_dataList2 = new List<TextObj>();
			string[] array = new string[]
			{
				"Health",
				"Mana",
				"Armor",
				"Weight"
			};
			string[] array2 = new string[]
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
				textObj2.Position = new Vector2(120f, (float)num);
				this.m_backCard.AddChild(textObj2);
				TextObj textObj3 = textObj.Clone() as TextObj;
				textObj3.Text = "0";
				textObj3.Position = new Vector2(textObj2.X + 20f, (float)num);
				this.m_dataList1.Add(textObj3);
				this.m_backCard.AddChild(textObj3);
				TextObj textObj4 = textObj.Clone() as TextObj;
				textObj4.Align = Types.TextAlign.Right;
				textObj4.Text = array2[i];
				textObj4.Position = new Vector2(330f, (float)num);
				this.m_backCard.AddChild(textObj4);
				TextObj textObj5 = textObj.Clone() as TextObj;
				textObj5.Text = "0";
				textObj5.Position = new Vector2(textObj4.X + 20f, (float)num);
				this.m_dataList2.Add(textObj5);
				this.m_backCard.AddChild(textObj5);
				num += 20;
			}
			this.m_equipmentTitle = (textObj.Clone() as TextObj);
			this.m_equipmentTitle.FontSize = 12f;
			this.m_equipmentTitle.Text = "Equipment:";
			this.m_equipmentTitle.Position = new Vector2(50f, 180f);
			this.m_backCard.AddChild(this.m_equipmentTitle);
			this.m_runesTitle = (textObj.Clone() as TextObj);
			this.m_runesTitle.FontSize = 12f;
			this.m_runesTitle.Text = "Enchantments:";
			this.m_runesTitle.Position = new Vector2(this.m_equipmentTitle.X, 330f);
			this.m_backCard.AddChild(this.m_runesTitle);
			for (int j = 0; j < Game.PlayerStats.GetEquippedArray.Length; j++)
			{
				TextObj textObj6 = textObj.Clone() as TextObj;
				textObj6.Text = "test";
				textObj6.Position = new Vector2(80f, this.m_equipmentTitle.Y + 50f);
				this.m_equipmentList.Add(textObj6);
				this.m_backCard.AddChild(textObj6);
			}
			for (int k = 0; k < 10; k++)
			{
				TextObj textObj7 = textObj.Clone() as TextObj;
				textObj7.X = 60f;
				textObj7.Text = EquipmentAbilityType.ToString(k);
				textObj7.FontSize = 7f;
				this.m_runeBackTitleList.Add(textObj7);
				this.m_backCard.AddChild(textObj7);
				TextObj textObj8 = textObj.Clone() as TextObj;
				textObj8.X = (float)(textObj7.Bounds.Right + 10);
				textObj8.FontSize = 7f;
				this.m_runeBackDescriptionList.Add(textObj8);
				this.m_backCard.AddChild(textObj8);
			}
			TextObj textObj9 = textObj.Clone() as TextObj;
			textObj9.X = 60f;
			textObj9.Text = EquipmentAbilityType.ToString(20);
			textObj9.FontSize = 7f;
			this.m_runeBackTitleList.Add(textObj9);
			this.m_backCard.AddChild(textObj9);
			TextObj textObj10 = textObj.Clone() as TextObj;
			textObj10.X = (float)(textObj9.Bounds.Right + 10);
			textObj10.FontSize = 7f;
			this.m_runeBackDescriptionList.Add(textObj10);
			this.m_backCard.AddChild(textObj10);
			TextObj textObj11 = textObj.Clone() as TextObj;
			textObj11.X = 60f;
			textObj11.Text = EquipmentAbilityType.ToString(21);
			textObj11.FontSize = 7f;
			this.m_runeBackTitleList.Add(textObj11);
			this.m_backCard.AddChild(textObj11);
			TextObj textObj12 = textObj.Clone() as TextObj;
			textObj12.X = (float)(textObj11.Bounds.Right + 10);
			textObj12.FontSize = 7f;
			this.m_runeBackDescriptionList.Add(textObj12);
			this.m_backCard.AddChild(textObj12);
		}
		public override void OnEnter()
		{
			SoundManager.PlaySound("StatCard_In");
			this.LoadCardColour();
			this.m_spellIcon.ChangeSprite(SpellType.Icon(Game.PlayerStats.Spell));
			string[] array = new string[]
			{
				"CardCastleBG_Sprite",
				"CardGardenBG_Sprite",
				"CardDungeonBG_Sprite",
				"CardTowerBG_Sprite"
			};
			this.m_playerBG.ChangeSprite(array[CDGMath.RandomInt(0, 3)]);
			this.m_frontCard.Y = 1500f;
			this.m_backCard.Y = 1500f;
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.7"
			});
			Tween.To(this.m_frontCard, 0.4f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"30"
			});
			Tween.To(this.m_backCard, 0.4f, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.2",
				"Y",
				"30"
			});
			PlayerObj player = (base.ScreenManager as RCScreenManager).Player;
			this.LoadFrontCardStats(player);
			this.LoadBackCardStats(player);
			this.ChangeParts(player);
			this.m_playerHUD.Update(player);
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
			}
			else
			{
				this.m_cancelText.ForcedScale = new Vector2(1f, 1f);
			}
			this.m_cancelText.Text = "[Input:" + 2 + "] to exit profile card";
			this.m_cancelText.Opacity = 0f;
			Tween.To(this.m_cancelText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			string[] array2 = new string[]
			{
				"Glauber Kotaki",
				"Kenny Lee",
				"Teddy Lee",
				"Gordon McGladdery",
				"Judson Cowan"
			};
			this.m_author.Text = array2[CDGMath.RandomInt(0, array2.Length - 1)];
			Array.Clear(array2, 0, array2.Length);
			base.OnEnter();
		}
		private void ChangeParts(PlayerObj player)
		{
			string[] array;
			if (Game.PlayerStats.Class == 16)
			{
				array = new string[]
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
				array = new string[]
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
				array2 = new string[]
				{
					"Jumping",
					"Falling"
				};
			}
			else
			{
				array2 = new string[]
				{
					"Jumping",
					"AirAttack",
					"Falling"
				};
			}
			if (CDGMath.RandomInt(0, 1) == 0)
			{
				this.m_playerInAir = true;
				this.SetPlayerStyle(array2[CDGMath.RandomInt(0, array2.Length - 1)]);
			}
			else
			{
				this.m_playerInAir = false;
				this.SetPlayerStyle(array[CDGMath.RandomInt(0, array.Length - 1)]);
			}
			for (int i = 0; i < player.NumChildren; i++)
			{
				SpriteObj spriteObj = player.GetChildAt(i) as SpriteObj;
				SpriteObj spriteObj2 = this.m_playerSprite.GetChildAt(i) as SpriteObj;
				spriteObj2.TextureColor = spriteObj.TextureColor;
			}
			string text = (this.m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
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
			this.m_playerSprite.GetChildAt(12).ChangeSprite(text);
			string text2 = (this.m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
			startIndex = text2.IndexOf("_") - 1;
			text2 = text2.Remove(startIndex, 1);
			text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
			this.m_playerSprite.GetChildAt(4).ChangeSprite(text2);
			string text3 = (this.m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
			startIndex = text3.IndexOf("_") - 1;
			text3 = text3.Remove(startIndex, 1);
			text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			this.m_playerSprite.GetChildAt(9).ChangeSprite(text3);
			string text4 = (this.m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
			startIndex = text4.IndexOf("_") - 1;
			text4 = text4.Remove(startIndex, 1);
			text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			this.m_playerSprite.GetChildAt(3).ChangeSprite(text4);
		}
		public void SetPlayerStyle(string animationType)
		{
			this.m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
			PlayerObj player = (base.ScreenManager as RCScreenManager).Player;
			for (int i = 0; i < this.m_playerSprite.NumChildren; i++)
			{
				this.m_playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
				this.m_playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
			}
			this.m_playerSprite.GetChildAt(16).Visible = false;
			this.m_playerSprite.Scale = player.Scale;
			if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
			{
				this.m_playerSprite.GetChildAt(7).Visible = false;
			}
			this.m_playerSprite.GetChildAt(14).Visible = false;
			if (Game.PlayerStats.SpecialItem == 8)
			{
				this.m_playerSprite.GetChildAt(14).Visible = true;
			}
			if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Shield_Sprite");
			}
			else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Lamp_Sprite");
			}
			else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Beard_Sprite");
			}
			else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Headband_Sprite");
			}
			else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Horns_Sprite");
			}
			else
			{
				this.m_playerSprite.GetChildAt(15).Visible = false;
			}
			this.m_playerSprite.GetChildAt(0).Visible = false;
			if (Game.PlayerStats.Class == 16)
			{
				this.m_playerSprite.GetChildAt(0).Visible = true;
				this.m_playerSprite.GetChildAt(12).ChangeSprite(string.Concat(new object[]
				{
					"Player",
					animationType,
					"Head",
					6,
					"_Sprite"
				}));
			}
			if (!Game.PlayerStats.IsFemale)
			{
				this.m_playerSprite.GetChildAt(5).Visible = false;
				this.m_playerSprite.GetChildAt(13).Visible = false;
			}
			else
			{
				this.m_playerSprite.GetChildAt(5).Visible = true;
				this.m_playerSprite.GetChildAt(13).Visible = true;
			}
			if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
			{
				this.m_playerSprite.Scale = new Vector2(3f, 3f);
			}
			if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
			{
				this.m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
			}
			if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
			{
				this.m_playerSprite.ScaleX *= 0.825f;
				this.m_playerSprite.ScaleY *= 1.25f;
			}
			if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
			{
				this.m_playerSprite.ScaleX *= 1.25f;
				this.m_playerSprite.ScaleY *= 1.175f;
			}
			if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
			{
				this.m_playerSprite.OutlineColour = Color.White;
			}
			else
			{
				this.m_playerSprite.OutlineColour = Color.Black;
			}
			this.m_playerSprite.CalculateBounds();
			this.m_playerSprite.Y = 435f - ((float)this.m_playerSprite.Bounds.Bottom - this.m_playerSprite.Y);
		}
		public void ExitScreenTransition()
		{
			SoundManager.PlaySound("StatCard_Out");
			Tween.To(this.m_cancelText, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			this.m_frontCard.Y = 30f;
			this.m_backCard.Y = 30f;
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.3",
				"BackBufferOpacity",
				"0"
			});
			Tween.To(this.m_frontCard, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"Y",
				"1500"
			});
			Tween.To(this.m_backCard, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.2",
				"Y",
				"1500"
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
			base.OnExit();
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
			this.m_frontCard.GetChildAt(0).TextureColor = textureColor;
			this.m_frontCard.GetChildAt(3).TextureColor = textureColor;
			this.m_backCard.GetChildAt(0).TextureColor = textureColor2;
			this.m_backCard.GetChildAt(2).TextureColor = textureColor2;
			this.m_frontCard.GetChildAt(2).TextureColor = new Color(235, 220, 185);
			this.m_backCard.GetChildAt(1).TextureColor = new Color(235, 220, 185);
		}
		private void LoadFrontCardStats(PlayerObj player)
		{
			this.m_frontTrait1.Visible = false;
			this.m_frontTrait2.Visible = false;
			byte b = (byte)Game.PlayerStats.Traits.X;
			if (b != 0)
			{
				this.m_frontTrait1.Text = TraitType.ToString(b) + ": " + TraitType.ProfileCardDescription(b);
				this.m_frontTrait1.Visible = true;
			}
			byte b2 = (byte)Game.PlayerStats.Traits.Y;
			if (b2 != 0)
			{
				this.m_frontTrait2.Y = this.m_frontTrait1.Y;
				if (b != 0)
				{
					this.m_frontTrait2.Y -= 20f;
				}
				this.m_frontTrait2.Text = TraitType.ToString(b2) + ": " + TraitType.ProfileCardDescription(b2);
				this.m_frontTrait2.Visible = true;
			}
			this.m_playerName.Text = Game.PlayerStats.PlayerName;
			this.m_playerStats.Text = (int)((float)player.Damage / 20f) + "/" + (int)((float)player.MaxHealth / 50f);
			this.m_levelClass.Text = string.Concat(new object[]
			{
				"Lv. ",
				Game.PlayerStats.CurrentLevel,
				" - ",
				ClassType.ToString(Game.PlayerStats.Class, Game.PlayerStats.IsFemale)
			});
			this.m_money.Text = Game.PlayerStats.Gold.ToString();
			this.m_classDescription.Text = ClassType.ProfileCardDescription(Game.PlayerStats.Class);
		}
		private void LoadBackCardStats(PlayerObj player)
		{
			for (int i = 0; i < this.m_dataList1.Count; i++)
			{
				switch (i)
				{
				case 0:
					this.m_dataList1[i].Text = player.MaxHealth.ToString();
					this.m_dataList2[i].Text = player.Damage.ToString();
					break;
				case 1:
					this.m_dataList1[i].Text = player.MaxMana.ToString();
					this.m_dataList2[i].Text = player.TotalMagicDamage.ToString();
					break;
				case 2:
				{
					this.m_dataList1[i].Text = string.Concat(new object[]
					{
						player.TotalArmor.ToString(),
						"(",
						(int)(player.TotalDamageReduc * 100f),
						"%)"
					});
					float num = player.TotalCritChance * 100f;
					this.m_dataList2[i].Text = ((int)Math.Round((double)num, MidpointRounding.AwayFromZero)).ToString() + "%";
					break;
				}
				case 3:
					this.m_dataList1[i].Text = player.CurrentWeight + "/" + player.MaxWeight;
					this.m_dataList2[i].Text = ((int)(player.TotalCriticalDamage * 100f)).ToString() + "%";
					break;
				}
			}
			sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
			int num2 = (int)this.m_equipmentTitle.Y + 40;
			for (int j = 0; j < Game.PlayerStats.GetEquippedArray.Length; j++)
			{
				this.m_equipmentList[j].Visible = false;
				this.m_equipmentList[j].Y = (float)num2;
				if (getEquippedArray[j] != -1)
				{
					this.m_equipmentList[j].Text = EquipmentBaseType.ToString((int)getEquippedArray[j]) + " " + EquipmentCategoryType.ToString2(j);
					this.m_equipmentList[j].Visible = true;
					num2 += 20;
				}
			}
			num2 = (int)this.m_runesTitle.Y + 40;
			for (int k = 0; k < this.m_runeBackTitleList.Count; k++)
			{
				this.m_runeBackTitleList[k].Y = (float)num2;
				this.m_runeBackDescriptionList[k].Y = (float)num2;
				this.m_runeBackTitleList[k].Visible = false;
				this.m_runeBackDescriptionList[k].Visible = false;
				float num3 = 0f;
				switch (k)
				{
				case 0:
					num3 = (float)player.TotalDoubleJumps;
					break;
				case 1:
					num3 = (float)player.TotalAirDashes;
					break;
				case 2:
					num3 = (float)player.TotalVampBonus;
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
					num3 = (float)(Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8);
					break;
				case 9:
					num3 = (float)Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f;
					break;
				}
				if (num3 > 0f)
				{
					this.m_runeBackDescriptionList[k].Text = "(" + EquipmentAbilityType.ShortDescription(k, num3) + ")";
					this.m_runeBackTitleList[k].Visible = true;
					this.m_runeBackDescriptionList[k].Visible = true;
					num2 += 20;
				}
			}
			if (Game.PlayerStats.HasArchitectFee)
			{
				this.m_runeBackDescriptionList[this.m_runeBackDescriptionList.Count - 2].Text = "(" + EquipmentAbilityType.ShortDescription(20, 0f) + ")";
				this.m_runeBackDescriptionList[this.m_runeBackDescriptionList.Count - 2].Visible = true;
				this.m_runeBackTitleList[this.m_runeBackDescriptionList.Count - 2].Visible = true;
				num2 += 20;
			}
			if (Game.PlayerStats.TimesCastleBeaten > 0)
			{
				this.m_runeBackDescriptionList[this.m_runeBackDescriptionList.Count - 1].Text = "(" + EquipmentAbilityType.ShortDescription(21, (float)(50 * Game.PlayerStats.TimesCastleBeaten)) + ")";
				this.m_runeBackDescriptionList[this.m_runeBackDescriptionList.Count - 1].Visible = true;
				this.m_runeBackTitleList[this.m_runeBackDescriptionList.Count - 1].Visible = true;
				if (Game.PlayerStats.HasArchitectFee)
				{
					this.m_runeBackDescriptionList[this.m_runeBackDescriptionList.Count - 1].Y = (float)num2;
					this.m_runeBackTitleList[this.m_runeBackDescriptionList.Count - 1].Y = (float)num2;
				}
			}
			(this.m_backCard.GetChildAt(3) as TextObj).Text = Game.PlayerStats.PlayerName;
		}
		public override void HandleInput()
		{
			if (this.m_frontCard.Y == 30f && (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3) || Game.GlobalInput.JustPressed(7)))
			{
				this.ExitScreenTransition();
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gametime)
		{
			this.m_playerHUD.SetPosition(new Vector2(this.m_frontCard.X + 46f, this.m_frontCard.Y + 64f));
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_frontCard.Draw(base.Camera);
			this.m_backCard.Draw(base.Camera);
			this.m_cancelText.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			if (Game.PlayerStats.IsDead)
			{
				this.m_tombStoneSprite.Position = new Vector2(this.m_frontCard.X + 240f, this.m_frontCard.Y + 280f);
				this.m_tombStoneSprite.Draw(base.Camera);
			}
			else
			{
				if (this.m_playerInAir)
				{
					this.m_playerSprite.Position = new Vector2(this.m_frontCard.X + 180f, this.m_frontCard.Y + 202f);
				}
				else
				{
					this.m_playerSprite.Position = new Vector2(this.m_frontCard.X + 160f, this.m_frontCard.Y + 280f - ((float)this.m_playerSprite.Bounds.Bottom - this.m_playerSprite.Y));
				}
				this.m_playerSprite.Draw(base.Camera);
				Game.ColourSwapShader.Parameters["desiredTint"].SetValue(this.m_playerSprite.GetChildAt(12).TextureColor.ToVector4());
				if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
				{
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_lichColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_lichColour2.ToVector4());
				}
				else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
				{
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
				}
				else
				{
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_skinColour2.ToVector4());
				}
				base.Camera.End();
				base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
				this.m_playerSprite.GetChildAt(12).Draw(base.Camera);
				base.Camera.End();
				base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
				if (Game.PlayerStats.IsFemale)
				{
					this.m_playerSprite.GetChildAt(13).Draw(base.Camera);
				}
				this.m_playerSprite.GetChildAt(15).Draw(base.Camera);
			}
			this.m_spellIcon.Position = new Vector2(this.m_frontCard.X + 380f, this.m_frontCard.Y + 320f);
			this.m_spellIcon.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Profile Card Screen");
				this.m_frontCard.Dispose();
				this.m_frontCard = null;
				this.m_backCard.Dispose();
				this.m_backCard = null;
				this.m_playerName = null;
				this.m_money = null;
				this.m_levelClass = null;
				this.m_playerHUD = null;
				this.m_frontTrait1 = null;
				this.m_frontTrait2 = null;
				this.m_playerBG = null;
				this.m_classDescription = null;
				this.m_author = null;
				this.m_playerStats = null;
				this.m_equipmentTitle = null;
				this.m_runesTitle = null;
				this.m_equipmentList.Clear();
				this.m_equipmentList = null;
				this.m_runeBackTitleList.Clear();
				this.m_runeBackTitleList = null;
				this.m_runeBackDescriptionList.Clear();
				this.m_runeBackDescriptionList = null;
				this.m_playerSprite.Dispose();
				this.m_playerSprite = null;
				this.m_spellIcon.Dispose();
				this.m_spellIcon = null;
				this.m_tombStoneSprite.Dispose();
				this.m_tombStoneSprite = null;
				this.m_cancelText.Dispose();
				this.m_cancelText = null;
				this.m_dataList1.Clear();
				this.m_dataList1 = null;
				this.m_dataList2.Clear();
				this.m_dataList2 = null;
				base.Dispose();
			}
		}
	}
}
