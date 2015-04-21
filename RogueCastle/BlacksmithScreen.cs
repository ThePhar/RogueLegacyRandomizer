using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class BlacksmithScreen : Screen
	{
		private const int m_startingCategoryIndex = 6;
		private ObjContainer m_blacksmithUI;
		private SpriteObj m_selectionIcon;
		private int m_currentCategoryIndex;
		private int m_currentEquipmentIndex;
		private List<ObjContainer[]> m_masterIconArray;
		private ObjContainer[] m_activeIconArray;
		private List<SpriteObj> m_newIconList;
		private int m_newIconListIndex;
		private TextObj m_playerMoney;
		private SpriteObj m_equippedIcon;
		private TextObj m_equipmentDescriptionText;
		private ObjContainer m_textInfoTitleContainer;
		private ObjContainer m_textInfoStatContainer;
		private ObjContainer m_textInfoStatModContainer;
		private ObjContainer m_unlockCostContainer;
		private TextObj m_addPropertiesTitleText;
		private TextObj m_addPropertiesText;
		private TextObj m_equipmentTitleText;
		private bool m_inCategoryMenu = true;
		private KeyIconTextObj m_confirmText;
		private KeyIconTextObj m_cancelText;
		private KeyIconTextObj m_navigationText;
		private bool m_lockControls;
		private Cue m_rainSound;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public PlayerObj Player
		{
			get;
			set;
		}
		private int CurrentCategoryIndex
		{
			get
			{
				return this.m_currentCategoryIndex - 6;
			}
		}
		public BlacksmithScreen()
		{
			this.m_currentCategoryIndex = 6;
			this.m_masterIconArray = new List<ObjContainer[]>();
			for (int i = 0; i < 5; i++)
			{
				this.m_masterIconArray.Add(new ObjContainer[15]);
			}
		}
		public override void LoadContent()
		{
			this.m_blacksmithUI = new ObjContainer("BlacksmithUI_Character");
			this.m_blacksmithUI.Position = new Vector2(660f, 360f);
			this.m_playerMoney = new TextObj(Game.GoldFont);
			this.m_playerMoney.Align = Types.TextAlign.Left;
			this.m_playerMoney.Text = "1000";
			this.m_playerMoney.FontSize = 30f;
			this.m_playerMoney.OverrideParentScale = true;
			this.m_playerMoney.Position = new Vector2(210f, -225f);
			this.m_playerMoney.AnchorY = 10f;
			this.m_blacksmithUI.AddChild(this.m_playerMoney);
			for (int i = 0; i < this.m_blacksmithUI.NumChildren; i++)
			{
				this.m_blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
			}
			this.m_selectionIcon = new SpriteObj("BlacksmithUI_SelectionIcon_Sprite");
			this.m_selectionIcon.PlayAnimation(true);
			this.m_selectionIcon.Scale = Vector2.Zero;
			this.m_selectionIcon.AnimationDelay = 0.1f;
			this.m_selectionIcon.ForceDraw = true;
			this.m_equipmentDescriptionText = new TextObj(Game.JunicodeFont);
			this.m_equipmentDescriptionText.Align = Types.TextAlign.Centre;
			this.m_equipmentDescriptionText.FontSize = 12f;
			this.m_equipmentDescriptionText.Position = new Vector2(230f, -20f);
			this.m_equipmentDescriptionText.Text = "Select a category";
			this.m_equipmentDescriptionText.WordWrap(190);
			this.m_equipmentDescriptionText.Scale = Vector2.Zero;
			this.m_blacksmithUI.AddChild(this.m_equipmentDescriptionText);
			foreach (ObjContainer[] current in this.m_masterIconArray)
			{
				Vector2 absPosition = this.m_blacksmithUI.GetChildAt(6).AbsPosition;
				absPosition.X += 85f;
				float x = absPosition.X;
				float num = 70f;
				float num2 = 80f;
				for (int j = 0; j < current.Length; j++)
				{
					current[j] = new ObjContainer("BlacksmithUI_QuestionMarkIcon_Character");
					current[j].Position = absPosition;
					current[j].Scale = Vector2.Zero;
					current[j].ForceDraw = true;
					absPosition.X += num;
					if (absPosition.X > x + num * 4f)
					{
						absPosition.X = x;
						absPosition.Y += num2;
					}
				}
			}
			this.InitializeTextObjs();
			this.m_equippedIcon = new SpriteObj("BlacksmithUI_EquippedIcon_Sprite");
			this.m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_confirmText.Text = "to close map";
			this.m_confirmText.FontSize = 12f;
			this.m_confirmText.Position = new Vector2(50f, 550f);
			this.m_confirmText.ForceDraw = true;
			this.m_cancelText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_cancelText.Text = "to re-center on player";
			this.m_cancelText.FontSize = 12f;
			this.m_cancelText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 40f);
			this.m_cancelText.ForceDraw = true;
			this.m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_navigationText.Text = "to move map";
			this.m_navigationText.FontSize = 12f;
			this.m_navigationText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 80f);
			this.m_navigationText.ForceDraw = true;
			this.m_newIconList = new List<SpriteObj>();
			for (int k = 0; k < 25; k++)
			{
				SpriteObj spriteObj = new SpriteObj("BlacksmithUI_NewIcon_Sprite");
				spriteObj.Visible = false;
				spriteObj.Scale = new Vector2(1.1f, 1.1f);
				this.m_newIconList.Add(spriteObj);
			}
			base.LoadContent();
		}
		private void InitializeTextObjs()
		{
			this.m_textInfoTitleContainer = new ObjContainer();
			this.m_textInfoStatContainer = new ObjContainer();
			this.m_textInfoStatModContainer = new ObjContainer();
			string[] array = new string[]
			{
				"Health",
				"Mana",
				"Damage",
				"Magic",
				"Armor",
				"Weight"
			};
			Vector2 zero = Vector2.Zero;
			TextObj textObj = new TextObj(null);
			textObj.Font = Game.JunicodeFont;
			textObj.FontSize = 10f;
			textObj.Text = "0";
			textObj.ForceDraw = true;
			for (int i = 0; i < array.Length; i++)
			{
				textObj.Position = zero;
				this.m_textInfoTitleContainer.AddChild(textObj.Clone() as TextObj);
				this.m_textInfoStatContainer.AddChild(textObj.Clone() as TextObj);
				this.m_textInfoStatModContainer.AddChild(textObj.Clone() as TextObj);
				(this.m_textInfoTitleContainer.GetChildAt(i) as TextObj).Align = Types.TextAlign.Right;
				(this.m_textInfoTitleContainer.GetChildAt(i) as TextObj).Text = array[i];
				zero.Y += (float)(this.m_textInfoTitleContainer.GetChildAt(i).Height - 5);
			}
			this.m_addPropertiesTitleText = new TextObj(null);
			this.m_addPropertiesTitleText.Font = Game.JunicodeFont;
			this.m_addPropertiesTitleText.FontSize = 8f;
			this.m_addPropertiesTitleText.TextureColor = new Color(237, 202, 138);
			this.m_addPropertiesTitleText.Text = "Additional Properties:";
			this.m_addPropertiesText = new TextObj(null);
			this.m_addPropertiesText.Font = Game.JunicodeFont;
			this.m_addPropertiesText.FontSize = 8f;
			this.m_unlockCostContainer = new ObjContainer();
			TextObj textObj2 = new TextObj(null);
			textObj2.Font = Game.JunicodeFont;
			textObj2.FontSize = 10f;
			textObj2.TextureColor = Color.Yellow;
			textObj2.Position = new Vector2(50f, 9f);
			this.m_unlockCostContainer.AddChild(new SpriteObj("BlacksmithUI_CoinBG_Sprite"));
			this.m_unlockCostContainer.AddChild(textObj2);
			this.m_equipmentTitleText = new TextObj(Game.JunicodeFont);
			this.m_equipmentTitleText.ForceDraw = true;
			this.m_equipmentTitleText.FontSize = 12f;
			this.m_equipmentTitleText.DropShadow = new Vector2(2f, 2f);
			this.m_equipmentTitleText.TextureColor = new Color(237, 202, 138);
			this.m_textInfoTitleContainer.Position = new Vector2(this.m_blacksmithUI.X + 205f, this.m_blacksmithUI.Y - (float)(this.m_blacksmithUI.Height / 2) + 45f);
			this.m_textInfoStatContainer.Position = new Vector2(this.m_textInfoTitleContainer.X + 15f, this.m_textInfoTitleContainer.Y);
			this.m_textInfoStatModContainer.Position = new Vector2(this.m_textInfoStatContainer.X + 75f, this.m_textInfoStatContainer.Y);
			this.m_addPropertiesTitleText.Position = new Vector2(this.m_blacksmithUI.X + 140f, (float)(this.m_textInfoStatModContainer.Bounds.Bottom + 5));
			this.m_addPropertiesText.Position = new Vector2(this.m_addPropertiesTitleText.X, (float)this.m_addPropertiesTitleText.Bounds.Bottom);
			this.m_unlockCostContainer.Position = new Vector2(this.m_blacksmithUI.X + 114f, 485f);
			this.m_equipmentTitleText.Position = new Vector2(this.m_blacksmithUI.X + 140f, this.m_textInfoTitleContainer.Y - 45f);
			this.m_textInfoTitleContainer.Visible = false;
			this.m_textInfoStatContainer.Visible = false;
			this.m_textInfoStatModContainer.Visible = false;
			this.m_addPropertiesTitleText.Visible = false;
			this.m_addPropertiesText.Visible = false;
			this.m_unlockCostContainer.Visible = false;
			this.m_equipmentTitleText.Visible = false;
		}
		private void DisplayCategory(int equipmentType)
		{
			float duration = 0.2f;
			float num = 0f;
			if (this.m_activeIconArray != null)
			{
				for (int i = 0; i < 15; i++)
				{
					Tween.StopAllContaining(this.m_activeIconArray[i], false);
					Tween.To(this.m_activeIconArray[i], duration, new Easing(Back.EaseIn), new string[]
					{
						"delay",
						num.ToString(),
						"ScaleX",
						"0",
						"ScaleY",
						"0"
					});
				}
			}
			this.m_activeIconArray = this.m_masterIconArray[equipmentType];
			num = 0.2f;
			for (int j = 0; j < 15; j++)
			{
				Tween.To(this.m_activeIconArray[j], duration, new Easing(Back.EaseOut), new string[]
				{
					"delay",
					num.ToString(),
					"ScaleX",
					"1",
					"ScaleY",
					"1"
				});
			}
			foreach (SpriteObj current in this.m_newIconList)
			{
				Tween.StopAllContaining(current, false);
				current.Scale = Vector2.Zero;
				Tween.To(current, duration, new Easing(Back.EaseOut), new string[]
				{
					"delay",
					num.ToString(),
					"ScaleX",
					"1",
					"ScaleY",
					"1"
				});
			}
			this.UpdateNewIcons();
			this.m_equippedIcon.Scale = Vector2.Zero;
			Tween.StopAllContaining(this.m_equippedIcon, false);
			Tween.To(this.m_equippedIcon, duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				num.ToString(),
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
		}
		public void EaseInMenu()
		{
			float duration = 0.4f;
			Tween.To(this.m_blacksmithUI.GetChildAt(0), duration, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_selectionIcon, duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.25",
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			float num = 0.2f;
			for (int i = 6; i < this.m_blacksmithUI.NumChildren - 3; i++)
			{
				num += 0.05f;
				Tween.To(this.m_blacksmithUI.GetChildAt(i), duration, new Easing(Back.EaseOut), new string[]
				{
					"delay",
					num.ToString(),
					"ScaleX",
					"1",
					"ScaleY",
					"1"
				});
			}
			Tween.To(this.m_blacksmithUI.GetChildAt(this.m_blacksmithUI.NumChildren - 1), duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				num.ToString(),
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_blacksmithUI.GetChildAt(this.m_blacksmithUI.NumChildren - 2), duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				num.ToString(),
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_blacksmithUI.GetChildAt(this.m_blacksmithUI.NumChildren - 3), duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				num.ToString(),
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.AddEndHandlerToLastTween(this, "EaseInComplete", new object[0]);
		}
		public void EaseInComplete()
		{
			this.m_lockControls = false;
		}
		private void EaseOutMenu()
		{
			foreach (SpriteObj current in this.m_newIconList)
			{
				current.Visible = false;
			}
			this.m_equippedIcon.Visible = false;
			Tween.To(this.m_confirmText, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_cancelText, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.To(this.m_navigationText, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			float num = 0.4f;
			float num2 = 0f;
			Tween.To(this.m_blacksmithUI.GetChildAt(this.m_blacksmithUI.NumChildren - 2), num, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_blacksmithUI.GetChildAt(this.m_blacksmithUI.NumChildren - 3), num, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_blacksmithUI.GetChildAt(this.m_blacksmithUI.NumChildren - 4), num, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			for (int i = 6; i < 11; i++)
			{
				if (this.m_currentCategoryIndex == i)
				{
					Tween.To(this.m_selectionIcon, num, new Easing(Back.EaseIn), new string[]
					{
						"delay",
						num2.ToString(),
						"ScaleX",
						"0",
						"ScaleY",
						"0"
					});
				}
				Tween.To(this.m_blacksmithUI.GetChildAt(i), num, new Easing(Back.EaseIn), new string[]
				{
					"delay",
					num2.ToString(),
					"ScaleX",
					"0",
					"ScaleY",
					"0"
				});
				num2 += 0.05f;
			}
			for (int j = 1; j < 6; j++)
			{
				this.m_blacksmithUI.GetChildAt(j).Scale = Vector2.Zero;
			}
			for (int k = 0; k < this.m_activeIconArray.Length; k++)
			{
				Tween.To(this.m_activeIconArray[k], num, new Easing(Back.EaseIn), new string[]
				{
					"ScaleX",
					"0",
					"ScaleY",
					"0"
				});
			}
			Tween.To(this.m_blacksmithUI.GetChildAt(0), num, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.3",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.RunFunction(num + 0.35f, base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		private void UpdateIconStates()
		{
			for (int i = 0; i < Game.PlayerStats.GetBlueprintArray.Count; i++)
			{
				for (int j = 0; j < Game.PlayerStats.GetBlueprintArray[i].Length; j++)
				{
					byte b = Game.PlayerStats.GetBlueprintArray[i][j];
					if (b == 0)
					{
						this.m_masterIconArray[i][j].ChangeSprite("BlacksmithUI_QuestionMarkIcon_Character");
					}
					else
					{
						this.m_masterIconArray[i][j].ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(i) + (j % 5 + 1).ToString() + "Icon_Character");
						for (int k = 1; k < this.m_masterIconArray[i][j].NumChildren; k++)
						{
							this.m_masterIconArray[i][j].GetChildAt(k).Opacity = 0.2f;
						}
					}
					if (b > 2)
					{
						for (int l = 1; l < this.m_masterIconArray[i][j].NumChildren; l++)
						{
							this.m_masterIconArray[i][j].GetChildAt(l).Opacity = 1f;
						}
						int num = 1;
						if (i == 0)
						{
							num = 2;
						}
						EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(i, j);
						this.m_masterIconArray[i][j].GetChildAt(num).TextureColor = equipmentData.FirstColour;
						if (i != 4)
						{
							num++;
							this.m_masterIconArray[i][j].GetChildAt(num).TextureColor = equipmentData.SecondColour;
						}
					}
				}
			}
		}
		private void UpdateNewIcons()
		{
			if (this.Player != null)
			{
				if (this.Player.CurrentMana > this.Player.MaxMana)
				{
					this.Player.CurrentMana = this.Player.MaxMana;
				}
				if (this.Player.CurrentHealth > this.Player.MaxHealth)
				{
					this.Player.CurrentHealth = this.Player.MaxHealth;
				}
			}
			this.UpdateMoneyText();
			this.m_newIconListIndex = 0;
			foreach (SpriteObj current in this.m_newIconList)
			{
				current.Visible = false;
			}
			for (int i = 0; i < Game.PlayerStats.GetBlueprintArray[this.CurrentCategoryIndex].Length; i++)
			{
				byte b = Game.PlayerStats.GetBlueprintArray[this.CurrentCategoryIndex][i];
				if (b == 1)
				{
					ObjContainer arg_DE_0 = this.m_masterIconArray[this.CurrentCategoryIndex][i];
					SpriteObj spriteObj = this.m_newIconList[this.m_newIconListIndex];
					spriteObj.Visible = true;
					spriteObj.Position = this.m_masterIconArray[this.CurrentCategoryIndex][i].AbsPosition;
					spriteObj.X -= 20f;
					spriteObj.Y -= 30f;
					this.m_newIconListIndex++;
				}
			}
			sbyte b2 = Game.PlayerStats.GetEquippedArray[this.CurrentCategoryIndex];
			if (b2 > -1)
			{
				this.m_equippedIcon.Position = new Vector2(this.m_activeIconArray[(int)b2].AbsPosition.X + 18f, this.m_activeIconArray[(int)b2].AbsPosition.Y + 18f);
				this.m_equippedIcon.Visible = true;
				return;
			}
			this.m_equippedIcon.Visible = false;
		}
		public override void OnEnter()
		{
			if (this.m_rainSound != null)
			{
				this.m_rainSound.Dispose();
			}
			if (DateTime.Now.Month != 12 && DateTime.Now.Month != 1)
			{
				this.m_rainSound = SoundManager.PlaySound("Rain1_Filtered");
			}
			else
			{
				this.m_rainSound = SoundManager.PlaySound("snowloop_filtered");
			}
			if (Game.PlayerStats.TotalBlueprintsFound >= 75)
			{
				GameUtil.UnlockAchievement("FEAR_OF_THROWING_STUFF_OUT");
			}
			this.m_lockControls = true;
			SoundManager.PlaySound("ShopMenuOpen");
			this.m_confirmText.Opacity = 0f;
			this.m_cancelText.Opacity = 0f;
			this.m_navigationText.Opacity = 0f;
			Tween.To(this.m_confirmText, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(this.m_cancelText, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(this.m_navigationText, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			this.m_confirmText.Text = "[Input:" + 0 + "]  select/equip";
			this.m_cancelText.Text = "[Input:" + 2 + "]  cancel/close menu";
			if (!InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_navigationText.Text = "Arrow keys to navigate";
			}
			else
			{
				this.m_navigationText.Text = "[Button:LeftStick] to navigate";
			}
			this.m_currentEquipmentIndex = 0;
			this.m_inCategoryMenu = true;
			this.m_selectionIcon.Position = this.m_blacksmithUI.GetChildAt(6).AbsPosition;
			this.m_currentCategoryIndex = 6;
			this.UpdateIconStates();
			this.DisplayCategory(0);
			this.EaseInMenu();
			Tween.To(this, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.5"
			});
			this.UpdateIconSelectionText();
			base.OnEnter();
		}
		public override void OnExit()
		{
			if (this.m_rainSound != null)
			{
				this.m_rainSound.Stop(AudioStopOptions.Immediate);
			}
			for (int i = 0; i < this.m_blacksmithUI.NumChildren; i++)
			{
				this.m_blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
			}
			foreach (ObjContainer[] current in this.m_masterIconArray)
			{
				for (int j = 0; j < current.Length; j++)
				{
					current[j].Scale = Vector2.Zero;
				}
			}
			this.m_selectionIcon.Scale = Vector2.Zero;
			this.Player.CurrentHealth = this.Player.MaxHealth;
			this.Player.CurrentMana = this.Player.MaxMana;
			(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.UpgradeData
			});
			bool flag = true;
			sbyte[] getEquippedArray = Game.PlayerStats.GetEquippedArray;
			for (int k = 0; k < getEquippedArray.Length; k++)
			{
				sbyte b = getEquippedArray[k];
				if (b == -1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				GameUtil.UnlockAchievement("FEAR_OF_NUDITY");
			}
			base.OnExit();
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls)
			{
				if (this.m_inCategoryMenu)
				{
					this.CategorySelectionInput();
				}
				else
				{
					this.EquipmentSelectionInput();
				}
			}
			base.HandleInput();
		}
		private void CategorySelectionInput()
		{
			int currentCategoryIndex = this.m_currentCategoryIndex;
			if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
			{
				this.m_currentCategoryIndex--;
				if (this.m_currentCategoryIndex < 6)
				{
					this.m_currentCategoryIndex = 10;
				}
			}
			else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
			{
				this.m_currentCategoryIndex++;
				if (this.m_currentCategoryIndex > 10)
				{
					this.m_currentCategoryIndex = 6;
				}
			}
			if (currentCategoryIndex != this.m_currentCategoryIndex)
			{
				SoundManager.PlaySound("ShopBSMenuMove");
				this.m_selectionIcon.Position = this.m_blacksmithUI.GetChildAt(this.m_currentCategoryIndex).AbsPosition;
				for (int i = 1; i < 6; i++)
				{
					if (i == 1)
					{
						this.m_blacksmithUI.GetChildAt(i).Scale = new Vector2(1f, 1f);
					}
					else
					{
						this.m_blacksmithUI.GetChildAt(i).Scale = Vector2.Zero;
					}
				}
				if (this.m_currentCategoryIndex != 6)
				{
					this.m_blacksmithUI.GetChildAt(this.m_currentCategoryIndex - 5).Scale = new Vector2(1f, 1f);
				}
				else
				{
					this.m_blacksmithUI.GetChildAt(this.m_currentCategoryIndex - 5).Scale = Vector2.Zero;
				}
				this.DisplayCategory(this.m_currentCategoryIndex - 6);
			}
			if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				this.m_lockControls = true;
				Tween.To(this, 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					"0.5",
					"BackBufferOpacity",
					"0"
				});
				this.EaseOutMenu();
				Tween.RunFunction(0.13f, typeof(SoundManager), "PlaySound", new object[]
				{
					"ShopMenuClose"
				});
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				this.m_inCategoryMenu = false;
				this.m_currentEquipmentIndex = 0;
				this.m_selectionIcon.Position = this.m_activeIconArray[this.m_currentEquipmentIndex].AbsPosition;
				byte b = Game.PlayerStats.GetBlueprintArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex];
				if (b == 1)
				{
					Game.PlayerStats.GetBlueprintArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex] = 2;
				}
				this.UpdateNewIcons();
				this.UpdateIconSelectionText();
				SoundManager.PlaySound("ShopMenuConfirm");
			}
		}
		private void EquipmentSelectionInput()
		{
			int currentEquipmentIndex = this.m_currentEquipmentIndex;
			if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
			{
				this.m_currentEquipmentIndex -= 5;
				if (this.m_currentEquipmentIndex < 0)
				{
					this.m_currentEquipmentIndex += 15;
				}
			}
			if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
			{
				this.m_currentEquipmentIndex += 5;
				if (this.m_currentEquipmentIndex > 14)
				{
					this.m_currentEquipmentIndex -= 15;
				}
			}
			if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
			{
				this.m_currentEquipmentIndex--;
				if ((this.m_currentEquipmentIndex + 1) % 5 == 0)
				{
					this.m_currentEquipmentIndex += 5;
				}
			}
			if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
			{
				this.m_currentEquipmentIndex++;
				if (this.m_currentEquipmentIndex % 5 == 0)
				{
					this.m_currentEquipmentIndex -= 5;
				}
			}
			if (currentEquipmentIndex != this.m_currentEquipmentIndex)
			{
				byte b = Game.PlayerStats.GetBlueprintArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex];
				if (b == 1)
				{
					Game.PlayerStats.GetBlueprintArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex] = 2;
				}
				this.UpdateNewIcons();
				this.UpdateIconSelectionText();
				this.m_selectionIcon.Position = this.m_activeIconArray[this.m_currentEquipmentIndex].AbsPosition;
				SoundManager.PlaySound("ShopBSMenuMove");
			}
			if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				SoundManager.PlaySound("ShopMenuCancel");
				this.m_inCategoryMenu = true;
				this.m_selectionIcon.Position = this.m_blacksmithUI.GetChildAt(this.m_currentCategoryIndex).AbsPosition;
				this.UpdateIconSelectionText();
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				int num = this.m_currentCategoryIndex - 6;
				int num2 = (int)Game.PlayerStats.GetBlueprintArray[num][this.m_currentEquipmentIndex];
				int num3 = (int)Game.PlayerStats.GetEquippedArray[num];
				if (num2 < 3 && num2 > 0)
				{
					EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(num, this.m_currentEquipmentIndex);
					if (Game.PlayerStats.Gold >= equipmentData.Cost)
					{
						SoundManager.PlaySound("ShopMenuUnlock");
						Game.PlayerStats.Gold -= equipmentData.Cost;
						Game.PlayerStats.GetBlueprintArray[num][this.m_currentEquipmentIndex] = 3;
						ObjContainer objContainer = this.m_masterIconArray[num][this.m_currentEquipmentIndex];
						objContainer.ChangeSprite("BlacksmithUI_" + EquipmentCategoryType.ToString(num) + (this.m_currentEquipmentIndex % 5 + 1).ToString() + "Icon_Character");
						for (int i = 1; i < objContainer.NumChildren; i++)
						{
							objContainer.GetChildAt(i).Opacity = 1f;
						}
						int num4 = 1;
						if (num == 0)
						{
							num4 = 2;
						}
						objContainer.GetChildAt(num4).TextureColor = equipmentData.FirstColour;
						if (num != 4)
						{
							num4++;
							objContainer.GetChildAt(num4).TextureColor = equipmentData.SecondColour;
						}
						num2 = 3;
						this.UpdateIconSelectionText();
					}
					else
					{
						SoundManager.PlaySound("ShopMenuUnlockFail");
					}
				}
				if (num3 != this.m_currentEquipmentIndex && num2 == 3)
				{
					EquipmentData equipmentData2 = Game.EquipmentSystem.GetEquipmentData(num, this.m_currentEquipmentIndex);
					int num5 = (int)Game.PlayerStats.GetEquippedArray[num];
					int num6 = 0;
					if (num5 != -1)
					{
						num6 = Game.EquipmentSystem.GetEquipmentData(num, num5).Weight;
					}
					if (equipmentData2.Weight + this.Player.CurrentWeight - num6 <= this.Player.MaxWeight)
					{
						SoundManager.PlaySound("ShopBSEquip");
						Game.PlayerStats.GetEquippedArray[num] = (sbyte)this.m_currentEquipmentIndex;
						this.UpdateIconSelectionText();
						Vector3 partIndices = PlayerPart.GetPartIndices(num);
						if (partIndices.X != -1f)
						{
							this.Player.GetChildAt((int)partIndices.X).TextureColor = equipmentData2.FirstColour;
						}
						if (partIndices.Y != -1f)
						{
							this.Player.GetChildAt((int)partIndices.Y).TextureColor = equipmentData2.SecondColour;
						}
						if (partIndices.Z != -1f)
						{
							this.Player.GetChildAt((int)partIndices.Z).TextureColor = equipmentData2.SecondColour;
						}
						if (num == 2 && partIndices.X != -1f)
						{
							this.Player.GetChildAt(5).TextureColor = equipmentData2.FirstColour;
						}
						this.UpdateNewIcons();
						return;
					}
					Console.WriteLine("cannot equip. too heavy. Weight:" + (equipmentData2.Weight + this.Player.CurrentWeight - num6));
					return;
				}
				else if (num3 == this.m_currentEquipmentIndex)
				{
					Game.PlayerStats.GetEquippedArray[num] = -1;
					this.Player.UpdateEquipmentColours();
					this.UpdateIconSelectionText();
					this.UpdateNewIcons();
				}
			}
		}
		private void UpdateIconSelectionText()
		{
			this.m_equipmentDescriptionText.Position = new Vector2(-1000f, -1000f);
			this.m_textInfoTitleContainer.Visible = false;
			this.m_textInfoStatContainer.Visible = false;
			this.m_textInfoStatModContainer.Visible = false;
			this.m_addPropertiesTitleText.Visible = false;
			this.m_addPropertiesText.Visible = false;
			this.m_unlockCostContainer.Visible = false;
			this.m_equipmentTitleText.Visible = false;
			if (this.m_inCategoryMenu)
			{
				this.m_equipmentDescriptionText.Text = "Select a category";
				return;
			}
			if (Game.PlayerStats.GetBlueprintArray[this.m_currentCategoryIndex - 6][this.m_currentEquipmentIndex] == 0)
			{
				this.m_equipmentDescriptionText.Position = new Vector2(230f, -20f);
				this.m_equipmentDescriptionText.Text = "Blueprint needed";
				return;
			}
			if (Game.PlayerStats.GetBlueprintArray[this.m_currentCategoryIndex - 6][this.m_currentEquipmentIndex] < 3)
			{
				this.m_equipmentDescriptionText.Text = "Purchase Info Here";
				(this.m_unlockCostContainer.GetChildAt(1) as TextObj).Text = Game.EquipmentSystem.GetEquipmentData(this.m_currentCategoryIndex - 6, this.m_currentEquipmentIndex).Cost.ToString() + " to unlock";
				this.m_unlockCostContainer.Visible = true;
				this.m_textInfoTitleContainer.Visible = true;
				this.m_textInfoStatContainer.Visible = true;
				this.m_textInfoStatModContainer.Visible = true;
				this.m_addPropertiesTitleText.Visible = true;
				this.m_addPropertiesText.Visible = true;
				this.m_equipmentTitleText.Visible = true;
				this.m_textInfoTitleContainer.Opacity = 0.5f;
				this.m_textInfoStatContainer.Opacity = 0.5f;
				this.m_textInfoStatModContainer.Opacity = 0.5f;
				this.m_addPropertiesTitleText.Opacity = 0.5f;
				this.m_addPropertiesText.Opacity = 0.5f;
				this.m_equipmentTitleText.Opacity = 0.5f;
				this.UpdateEquipmentDataText();
				return;
			}
			this.m_textInfoTitleContainer.Visible = true;
			this.m_textInfoStatContainer.Visible = true;
			this.m_textInfoStatModContainer.Visible = true;
			this.m_addPropertiesTitleText.Visible = true;
			this.m_addPropertiesText.Visible = true;
			this.m_equipmentTitleText.Visible = true;
			this.m_textInfoTitleContainer.Opacity = 1f;
			this.m_textInfoStatContainer.Opacity = 1f;
			this.m_textInfoStatModContainer.Opacity = 1f;
			this.m_addPropertiesTitleText.Opacity = 1f;
			this.m_addPropertiesText.Opacity = 1f;
			this.m_equipmentTitleText.Opacity = 1f;
			this.UpdateEquipmentDataText();
		}
		private void UpdateEquipmentDataText()
		{
			(this.m_textInfoStatContainer.GetChildAt(0) as TextObj).Text = this.Player.MaxHealth.ToString();
			(this.m_textInfoStatContainer.GetChildAt(1) as TextObj).Text = this.Player.MaxMana.ToString();
			(this.m_textInfoStatContainer.GetChildAt(2) as TextObj).Text = this.Player.Damage.ToString();
			(this.m_textInfoStatContainer.GetChildAt(3) as TextObj).Text = this.Player.TotalMagicDamage.ToString();
			(this.m_textInfoStatContainer.GetChildAt(4) as TextObj).Text = this.Player.TotalArmor.ToString();
			(this.m_textInfoStatContainer.GetChildAt(5) as TextObj).Text = this.Player.CurrentWeight.ToString() + "/" + this.Player.MaxWeight.ToString();
			int num = this.m_currentCategoryIndex - 6;
			EquipmentData equipmentData = Game.EquipmentSystem.GetEquipmentData(num, this.m_currentEquipmentIndex);
			int num2 = (int)Game.PlayerStats.GetEquippedArray[num];
			EquipmentData equipmentData2 = new EquipmentData();
			if (num2 > -1)
			{
				equipmentData2 = Game.EquipmentSystem.GetEquipmentData(num, num2);
			}
			bool flag = (int)Game.PlayerStats.GetEquippedArray[this.CurrentCategoryIndex] == this.m_currentEquipmentIndex;
			int num3 = equipmentData.BonusHealth - equipmentData2.BonusHealth;
			if (flag)
			{
				num3 = -equipmentData.BonusHealth;
			}
			TextObj textObj = this.m_textInfoStatModContainer.GetChildAt(0) as TextObj;
			if (num3 > 0)
			{
				textObj.TextureColor = Color.Cyan;
				textObj.Text = "+" + num3.ToString();
			}
			else if (num3 < 0)
			{
				textObj.TextureColor = Color.Red;
				textObj.Text = num3.ToString();
			}
			else
			{
				textObj.Text = "";
			}
			TextObj textObj2 = this.m_textInfoStatModContainer.GetChildAt(1) as TextObj;
			int num4 = equipmentData.BonusMana - equipmentData2.BonusMana;
			if (flag)
			{
				num4 = -equipmentData.BonusMana;
			}
			if (num4 > 0)
			{
				textObj2.TextureColor = Color.Cyan;
				textObj2.Text = "+" + num4.ToString();
			}
			else if (num4 < 0)
			{
				textObj2.TextureColor = Color.Red;
				textObj2.Text = num4.ToString();
			}
			else
			{
				textObj2.Text = "";
			}
			TextObj textObj3 = this.m_textInfoStatModContainer.GetChildAt(2) as TextObj;
			int num5 = equipmentData.BonusDamage - equipmentData2.BonusDamage;
			if (flag)
			{
				num5 = -equipmentData.BonusDamage;
			}
			if (num5 > 0)
			{
				textObj3.TextureColor = Color.Cyan;
				textObj3.Text = "+" + num5.ToString();
			}
			else if (num5 < 0)
			{
				textObj3.TextureColor = Color.Red;
				textObj3.Text = num5.ToString();
			}
			else
			{
				textObj3.Text = "";
			}
			TextObj textObj4 = this.m_textInfoStatModContainer.GetChildAt(3) as TextObj;
			int num6 = equipmentData.BonusMagic - equipmentData2.BonusMagic;
			if (flag)
			{
				num6 = -equipmentData.BonusMagic;
			}
			if (num6 > 0)
			{
				textObj4.TextureColor = Color.Cyan;
				textObj4.Text = "+" + num6.ToString();
			}
			else if (num6 < 0)
			{
				textObj4.TextureColor = Color.Red;
				textObj4.Text = num6.ToString();
			}
			else
			{
				textObj4.Text = "";
			}
			TextObj textObj5 = this.m_textInfoStatModContainer.GetChildAt(4) as TextObj;
			int num7 = equipmentData.BonusArmor - equipmentData2.BonusArmor;
			if (flag)
			{
				num7 = -equipmentData.BonusArmor;
			}
			if (num7 > 0)
			{
				textObj5.TextureColor = Color.Cyan;
				textObj5.Text = "+" + num7.ToString();
			}
			else if (num7 < 0)
			{
				textObj5.TextureColor = Color.Red;
				textObj5.Text = num7.ToString();
			}
			else
			{
				textObj5.Text = "";
			}
			TextObj textObj6 = this.m_textInfoStatModContainer.GetChildAt(5) as TextObj;
			int num8 = equipmentData.Weight - equipmentData2.Weight;
			if (flag)
			{
				num8 = -equipmentData.Weight;
			}
			if (num8 > 0)
			{
				textObj6.TextureColor = Color.Red;
				textObj6.Text = "+" + num8.ToString();
			}
			else if (num8 < 0)
			{
				textObj6.TextureColor = Color.Cyan;
				textObj6.Text = num8.ToString();
			}
			else
			{
				textObj6.Text = "";
			}
			Vector2[] secondaryAttribute = equipmentData.SecondaryAttribute;
			this.m_addPropertiesText.Text = "";
			if (secondaryAttribute != null)
			{
				Vector2[] array = secondaryAttribute;
				for (int i = 0; i < array.Length; i++)
				{
					Vector2 vector = array[i];
					if (vector.X != 0f)
					{
						if (vector.X < 7f)
						{
							TextObj expr_4FE = this.m_addPropertiesText;
							string text = expr_4FE.Text;
							expr_4FE.Text = string.Concat(new string[]
							{
								text,
								"+",
								(vector.Y * 100f).ToString(),
								"% ",
								EquipmentSecondaryDataType.ToString((int)vector.X),
								"\n"
							});
						}
						else
						{
							TextObj expr_56E = this.m_addPropertiesText;
							string text2 = expr_56E.Text;
							string[] array2 = new string[6];
							array2[0] = text2;
							array2[1] = "+";
							string[] arg_5A0_0 = array2;
							int arg_5A0_1 = 2;
							float y = vector.Y;
							arg_5A0_0[arg_5A0_1] = y.ToString();
							array2[3] = " ";
							array2[4] = EquipmentSecondaryDataType.ToString((int)vector.X);
							array2[5] = "\n";
							expr_56E.Text = string.Concat(array2);
						}
					}
				}
				if (secondaryAttribute.Length == 0)
				{
					this.m_addPropertiesText.Text = "None";
				}
			}
			else
			{
				this.m_addPropertiesText.Text = "None";
			}
			this.m_equipmentTitleText.Text = EquipmentBaseType.ToString(this.m_currentEquipmentIndex) + " " + EquipmentCategoryType.ToString(num);
		}
		private void UpdateMoneyText()
		{
			this.m_playerMoney.Text = Game.PlayerStats.Gold.ToString();
			ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
			if (levelScreen != null)
			{
				levelScreen.UpdatePlayerHUD();
			}
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin();
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_blacksmithUI.Draw(base.Camera);
			this.m_selectionIcon.Draw(base.Camera);
			this.m_textInfoTitleContainer.Draw(base.Camera);
			this.m_textInfoStatContainer.Draw(base.Camera);
			this.m_textInfoStatModContainer.Draw(base.Camera);
			this.m_addPropertiesTitleText.Draw(base.Camera);
			this.m_addPropertiesText.Draw(base.Camera);
			this.m_unlockCostContainer.Draw(base.Camera);
			this.m_equipmentTitleText.Draw(base.Camera);
			foreach (ObjContainer[] current in this.m_masterIconArray)
			{
				for (int i = 0; i < current.Length; i++)
				{
					current[i].Draw(base.Camera);
				}
			}
			this.m_navigationText.Draw(base.Camera);
			this.m_cancelText.Draw(base.Camera);
			this.m_confirmText.Draw(base.Camera);
			this.m_equippedIcon.Draw(base.Camera);
			foreach (SpriteObj current2 in this.m_newIconList)
			{
				current2.Draw(base.Camera);
			}
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Blacksmith Screen");
				if (this.m_rainSound != null)
				{
					this.m_rainSound.Dispose();
				}
				this.m_rainSound = null;
				this.m_blacksmithUI.Dispose();
				this.m_blacksmithUI = null;
				this.m_equipmentDescriptionText.Dispose();
				this.m_equipmentDescriptionText = null;
				this.m_selectionIcon.Dispose();
				this.m_selectionIcon = null;
				this.m_equipmentTitleText.Dispose();
				this.m_equipmentTitleText = null;
				this.m_activeIconArray = null;
				foreach (ObjContainer[] current in this.m_masterIconArray)
				{
					for (int i = 0; i < current.Length; i++)
					{
						current[i].Dispose();
						current[i] = null;
					}
					Array.Clear(current, 0, current.Length);
				}
				this.m_masterIconArray.Clear();
				this.m_masterIconArray = null;
				this.m_textInfoStatContainer.Dispose();
				this.m_textInfoStatContainer = null;
				this.m_textInfoTitleContainer.Dispose();
				this.m_textInfoTitleContainer = null;
				this.m_textInfoStatModContainer.Dispose();
				this.m_textInfoStatModContainer = null;
				this.m_unlockCostContainer.Dispose();
				this.m_unlockCostContainer = null;
				this.m_addPropertiesText.Dispose();
				this.m_addPropertiesText = null;
				this.m_addPropertiesTitleText.Dispose();
				this.m_addPropertiesTitleText = null;
				this.m_equippedIcon.Dispose();
				this.m_equippedIcon = null;
				this.Player = null;
				this.m_confirmText.Dispose();
				this.m_confirmText = null;
				this.m_cancelText.Dispose();
				this.m_cancelText = null;
				this.m_navigationText.Dispose();
				this.m_navigationText = null;
				this.m_playerMoney = null;
				foreach (SpriteObj current2 in this.m_newIconList)
				{
					current2.Dispose();
				}
				this.m_newIconList.Clear();
				this.m_newIconList = null;
				base.Dispose();
			}
		}
	}
}
