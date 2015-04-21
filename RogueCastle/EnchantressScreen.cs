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
	public class EnchantressScreen : Screen
	{
		private const int m_startingCategoryIndex = 6;
		private ObjContainer m_enchantressUI;
		private SpriteObj m_selectionIcon;
		private int m_currentCategoryIndex;
		private int m_currentEquipmentIndex;
		private List<SpriteObj[]> m_masterIconArray;
		private SpriteObj[] m_activeIconArray;
		private List<SpriteObj> m_newIconList;
		private int m_newIconListIndex;
		private TextObj m_playerMoney;
		private SpriteObj m_equippedIcon;
		private TextObj m_equipmentDescriptionText;
		private TextObj m_descriptionText;
		private ObjContainer m_unlockCostContainer;
		private TextObj m_instructionsTitleText;
		private KeyIconTextObj m_instructionsText;
		private TextObj m_equipmentTitleText;
		private bool m_inCategoryMenu = true;
		private bool m_lockControls;
		private KeyIconTextObj m_confirmText;
		private KeyIconTextObj m_cancelText;
		private KeyIconTextObj m_navigationText;
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
		public EnchantressScreen()
		{
			this.m_currentCategoryIndex = 6;
			this.m_masterIconArray = new List<SpriteObj[]>();
			for (int i = 0; i < 5; i++)
			{
				this.m_masterIconArray.Add(new SpriteObj[11]);
			}
		}
		public override void LoadContent()
		{
			this.m_enchantressUI = new ObjContainer("BlacksmithUI_Character");
			this.m_enchantressUI.Position = new Vector2(660f, 360f);
			this.m_playerMoney = new TextObj(Game.GoldFont);
			this.m_playerMoney.Align = Types.TextAlign.Left;
			this.m_playerMoney.Text = "1000";
			this.m_playerMoney.FontSize = 30f;
			this.m_playerMoney.OverrideParentScale = true;
			this.m_playerMoney.Position = new Vector2(210f, -225f);
			this.m_playerMoney.AnchorY = 10f;
			this.m_enchantressUI.AddChild(this.m_playerMoney);
			this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 3).ChangeSprite("EnchantressUI_Title_Sprite");
			for (int i = 0; i < this.m_enchantressUI.NumChildren; i++)
			{
				this.m_enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
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
			this.m_enchantressUI.AddChild(this.m_equipmentDescriptionText);
			foreach (SpriteObj[] current in this.m_masterIconArray)
			{
				Vector2 absPosition = this.m_enchantressUI.GetChildAt(6).AbsPosition;
				absPosition.X += 85f;
				float x = absPosition.X;
				float num = 70f;
				float num2 = 80f;
				for (int j = 0; j < current.Length; j++)
				{
					current[j] = new SpriteObj("BlacksmithUI_QuestionMarkIcon_Sprite");
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
			for (int k = 0; k < 55; k++)
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
			this.m_descriptionText = new TextObj(Game.JunicodeFont);
			this.m_descriptionText.FontSize = 9f;
			this.m_instructionsTitleText = new TextObj(null);
			this.m_instructionsTitleText.Font = Game.JunicodeFont;
			this.m_instructionsTitleText.FontSize = 10f;
			this.m_instructionsTitleText.TextureColor = new Color(237, 202, 138);
			this.m_instructionsTitleText.Text = "Instructions:";
			this.m_instructionsText = new KeyIconTextObj(null);
			this.m_instructionsText.Font = Game.JunicodeFont;
			this.m_instructionsText.FontSize = 10f;
			this.m_unlockCostContainer = new ObjContainer();
			TextObj textObj = new TextObj(null);
			textObj.Font = Game.JunicodeFont;
			textObj.FontSize = 10f;
			textObj.TextureColor = Color.Yellow;
			textObj.Position = new Vector2(50f, 9f);
			this.m_unlockCostContainer.AddChild(new SpriteObj("BlacksmithUI_CoinBG_Sprite"));
			this.m_unlockCostContainer.AddChild(textObj);
			this.m_descriptionText.Position = new Vector2(this.m_enchantressUI.X + 140f, this.m_enchantressUI.Y - (float)(this.m_enchantressUI.Height / 2) + 20f + 40f);
			this.m_instructionsTitleText.Position = new Vector2(this.m_enchantressUI.X + 140f, (float)(this.m_descriptionText.Bounds.Bottom + 20));
			this.m_instructionsText.Position = new Vector2(this.m_instructionsTitleText.X, (float)this.m_instructionsTitleText.Bounds.Bottom);
			this.m_unlockCostContainer.Position = new Vector2(this.m_enchantressUI.X + 114f, 485f);
			this.m_equipmentTitleText = new TextObj(Game.JunicodeFont);
			this.m_equipmentTitleText.ForceDraw = true;
			this.m_equipmentTitleText.FontSize = 10f;
			this.m_equipmentTitleText.DropShadow = new Vector2(2f, 2f);
			this.m_equipmentTitleText.TextureColor = new Color(237, 202, 138);
			this.m_equipmentTitleText.Position = new Vector2(this.m_enchantressUI.X + 140f, this.m_descriptionText.Y - 50f);
			this.m_descriptionText.Visible = false;
			this.m_instructionsTitleText.Visible = false;
			this.m_instructionsText.Visible = false;
			this.m_unlockCostContainer.Visible = false;
		}
		private void DisplayCategory(int equipmentType)
		{
			float duration = 0.2f;
			float num = 0f;
			if (this.m_activeIconArray != null)
			{
				for (int i = 0; i < 11; i++)
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
			num = 0.25f;
			for (int j = 0; j < 11; j++)
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
			Tween.To(this.m_enchantressUI.GetChildAt(0), duration, new Easing(Back.EaseOut), new string[]
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
			for (int i = 6; i < this.m_enchantressUI.NumChildren - 3; i++)
			{
				num += 0.05f;
				Tween.To(this.m_enchantressUI.GetChildAt(i), duration, new Easing(Back.EaseOut), new string[]
				{
					"delay",
					num.ToString(),
					"ScaleX",
					"1",
					"ScaleY",
					"1"
				});
			}
			Tween.To(this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 1), duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				num.ToString(),
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 2), duration, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				num.ToString(),
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 3), duration, new Easing(Back.EaseOut), new string[]
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
			Tween.To(this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 2), num, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 3), num, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_enchantressUI.GetChildAt(this.m_enchantressUI.NumChildren - 4), num, new Easing(Back.EaseIn), new string[]
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
				Tween.To(this.m_enchantressUI.GetChildAt(i), num, new Easing(Back.EaseIn), new string[]
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
				this.m_enchantressUI.GetChildAt(j).Scale = Vector2.Zero;
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
			Tween.To(this.m_enchantressUI.GetChildAt(0), num, new Easing(Back.EaseIn), new string[]
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
			for (int i = 0; i < Game.PlayerStats.GetRuneArray.Count; i++)
			{
				for (int j = 0; j < Game.PlayerStats.GetRuneArray[i].Length; j++)
				{
					byte b = Game.PlayerStats.GetRuneArray[i][j];
					if (b == 0)
					{
						this.m_masterIconArray[i][j].ChangeSprite("BlacksmithUI_QuestionMarkIcon_Sprite");
					}
					else
					{
						this.m_masterIconArray[i][j].ChangeSprite(EquipmentAbilityType.Icon(j));
						this.m_masterIconArray[i][j].Opacity = 0.2f;
					}
					if (b >= 3)
					{
						this.m_masterIconArray[i][j].Opacity = 1f;
					}
				}
			}
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
			if (Game.PlayerStats.TotalRunesFound >= 55)
			{
				GameUtil.UnlockAchievement("LOVE_OF_KNOWLEDGE");
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
			this.m_selectionIcon.Position = this.m_enchantressUI.GetChildAt(6).AbsPosition;
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
			for (int i = 0; i < this.m_enchantressUI.NumChildren; i++)
			{
				this.m_enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
			}
			foreach (SpriteObj[] current in this.m_masterIconArray)
			{
				for (int j = 0; j < current.Length; j++)
				{
					current[j].Scale = Vector2.Zero;
				}
			}
			this.m_selectionIcon.Scale = Vector2.Zero;
			(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.UpgradeData
			});
			bool flag = true;
			sbyte[] getEquippedRuneArray = Game.PlayerStats.GetEquippedRuneArray;
			for (int k = 0; k < getEquippedRuneArray.Length; k++)
			{
				sbyte b = getEquippedRuneArray[k];
				if (b == -1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				GameUtil.UnlockAchievement("LOVE_OF_CHANGE");
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
				this.m_selectionIcon.Position = this.m_enchantressUI.GetChildAt(this.m_currentCategoryIndex).AbsPosition;
				for (int i = 1; i < 6; i++)
				{
					if (i == 1)
					{
						this.m_enchantressUI.GetChildAt(i).Scale = new Vector2(1f, 1f);
					}
					else
					{
						this.m_enchantressUI.GetChildAt(i).Scale = Vector2.Zero;
					}
				}
				if (this.m_currentCategoryIndex != 6)
				{
					this.m_enchantressUI.GetChildAt(this.m_currentCategoryIndex - 5).Scale = new Vector2(1f, 1f);
				}
				else
				{
					this.m_enchantressUI.GetChildAt(this.m_currentCategoryIndex - 5).Scale = Vector2.Zero;
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
				byte b = Game.PlayerStats.GetRuneArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex];
				if (b == 1)
				{
					Game.PlayerStats.GetRuneArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex] = 2;
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
				if (this.m_currentEquipmentIndex > 4)
				{
					this.m_currentEquipmentIndex -= 5;
				}
				if (this.m_currentEquipmentIndex < 0)
				{
					this.m_currentEquipmentIndex = 0;
				}
			}
			if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
			{
				if (this.m_currentEquipmentIndex < 6)
				{
					this.m_currentEquipmentIndex += 5;
				}
				if (this.m_currentEquipmentIndex > 10)
				{
					this.m_currentEquipmentIndex -= 5;
				}
			}
			if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
			{
				this.m_currentEquipmentIndex--;
				if ((this.m_currentEquipmentIndex + 1) % 5 == 0)
				{
					this.m_currentEquipmentIndex++;
				}
			}
			if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
			{
				this.m_currentEquipmentIndex++;
				if (this.m_currentEquipmentIndex % 5 == 0 || this.m_currentEquipmentIndex > 10)
				{
					this.m_currentEquipmentIndex--;
				}
			}
			if (currentEquipmentIndex != this.m_currentEquipmentIndex)
			{
				byte b = Game.PlayerStats.GetRuneArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex];
				if (b == 1)
				{
					Game.PlayerStats.GetRuneArray[this.CurrentCategoryIndex][this.m_currentEquipmentIndex] = 2;
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
				this.m_selectionIcon.Position = this.m_enchantressUI.GetChildAt(this.m_currentCategoryIndex).AbsPosition;
				this.UpdateIconSelectionText();
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				int num = this.m_currentCategoryIndex - 6;
				int num2 = (int)Game.PlayerStats.GetRuneArray[num][this.m_currentEquipmentIndex];
				int num3 = (int)Game.PlayerStats.GetEquippedRuneArray[num];
				if (num2 < 3 && num2 > 0)
				{
					int abilityCost = Game.EquipmentSystem.GetAbilityCost(num, this.m_currentEquipmentIndex);
					if (Game.PlayerStats.Gold >= abilityCost)
					{
						SoundManager.PlaySound("ShopMenuUnlock");
						Game.PlayerStats.Gold -= abilityCost;
						Game.PlayerStats.GetRuneArray[num][this.m_currentEquipmentIndex] = 3;
						Game.PlayerStats.GetEquippedRuneArray[num] = (sbyte)this.m_currentEquipmentIndex;
						this.Player.AttachedLevel.UpdatePlayerHUDAbilities();
						SpriteObj spriteObj = this.m_masterIconArray[num][this.m_currentEquipmentIndex];
						spriteObj.Opacity = 1f;
						num2 = 3;
						this.UpdateIconSelectionText();
					}
				}
				if (num3 != this.m_currentEquipmentIndex && num2 == 3)
				{
					this.m_equippedIcon.Scale = new Vector2(1f, 1f);
					this.m_equippedIcon.Position = this.m_activeIconArray[this.m_currentEquipmentIndex].AbsPosition;
					this.m_equippedIcon.Position += new Vector2(18f, 18f);
					Game.PlayerStats.GetEquippedRuneArray[num] = (sbyte)this.m_currentEquipmentIndex;
					this.Player.AttachedLevel.UpdatePlayerHUDAbilities();
					SoundManager.PlaySound("ShopBSEquip");
					this.UpdateIconSelectionText();
					this.UpdateNewIcons();
					return;
				}
				if (num3 == this.m_currentEquipmentIndex)
				{
					this.m_equippedIcon.Scale = Vector2.Zero;
					Game.PlayerStats.GetEquippedRuneArray[num] = -1;
					this.UpdateNewIcons();
				}
			}
		}
		private void UpdateIconSelectionText()
		{
			this.m_equipmentDescriptionText.Position = new Vector2(-1000f, -1000f);
			this.m_descriptionText.Visible = false;
			this.m_instructionsTitleText.Visible = false;
			this.m_instructionsText.Visible = false;
			this.m_unlockCostContainer.Visible = false;
			this.m_equipmentTitleText.Visible = false;
			if (this.m_inCategoryMenu)
			{
				this.m_equipmentDescriptionText.Text = "Select a category";
				return;
			}
			if (Game.PlayerStats.GetRuneArray[this.m_currentCategoryIndex - 6][this.m_currentEquipmentIndex] == 0)
			{
				this.m_equipmentDescriptionText.Position = new Vector2(230f, -20f);
				this.m_equipmentDescriptionText.Text = "Rune needed";
				return;
			}
			if (Game.PlayerStats.GetRuneArray[this.m_currentCategoryIndex - 6][this.m_currentEquipmentIndex] < 3)
			{
				this.m_equipmentDescriptionText.Text = "Purchase Info Here";
				(this.m_unlockCostContainer.GetChildAt(1) as TextObj).Text = Game.EquipmentSystem.GetAbilityCost(this.m_currentCategoryIndex - 6, this.m_currentEquipmentIndex).ToString() + " to unlock";
				this.m_unlockCostContainer.Visible = true;
				this.m_descriptionText.Visible = true;
				this.m_instructionsTitleText.Visible = true;
				this.m_instructionsText.Visible = true;
				this.m_equipmentTitleText.Visible = true;
				this.m_descriptionText.Opacity = 0.5f;
				this.m_instructionsTitleText.Opacity = 0.5f;
				this.m_instructionsText.Opacity = 0.5f;
				this.m_equipmentTitleText.Opacity = 0.5f;
				this.UpdateEquipmentDataText();
				return;
			}
			this.m_descriptionText.Visible = true;
			this.m_instructionsTitleText.Visible = true;
			this.m_instructionsText.Visible = true;
			this.m_equipmentTitleText.Visible = true;
			this.m_descriptionText.Opacity = 1f;
			this.m_instructionsTitleText.Opacity = 1f;
			this.m_instructionsText.Opacity = 1f;
			this.m_equipmentTitleText.Opacity = 1f;
			this.UpdateEquipmentDataText();
		}
		private void UpdateEquipmentDataText()
		{
			this.m_equipmentTitleText.Text = EquipmentAbilityType.ToString(this.m_currentEquipmentIndex) + " Rune\n(" + EquipmentCategoryType.ToString2(this.m_currentCategoryIndex - 6) + ")";
			this.m_descriptionText.Text = EquipmentAbilityType.Description(this.m_currentEquipmentIndex);
			this.m_descriptionText.WordWrap(195);
			this.m_descriptionText.Y = this.m_equipmentTitleText.Y + 60f;
			this.m_instructionsTitleText.Position = new Vector2(this.m_enchantressUI.X + 140f, (float)(this.m_descriptionText.Bounds.Bottom + 20));
			this.m_instructionsText.Text = EquipmentAbilityType.Instructions(this.m_currentEquipmentIndex);
			this.m_instructionsText.WordWrap(200);
			this.m_instructionsText.Position = new Vector2(this.m_instructionsTitleText.X, (float)this.m_instructionsTitleText.Bounds.Bottom);
		}
		private void UpdateNewIcons()
		{
			this.UpdateMoneyText();
			this.m_newIconListIndex = 0;
			foreach (SpriteObj current in this.m_newIconList)
			{
				current.Visible = false;
			}
			for (int i = 0; i < Game.PlayerStats.GetRuneArray[this.CurrentCategoryIndex].Length; i++)
			{
				byte b = Game.PlayerStats.GetRuneArray[this.CurrentCategoryIndex][i];
				if (b == 1)
				{
					SpriteObj arg_7A_0 = this.m_masterIconArray[this.CurrentCategoryIndex][i];
					SpriteObj spriteObj = this.m_newIconList[this.m_newIconListIndex];
					spriteObj.Visible = true;
					spriteObj.Position = this.m_masterIconArray[this.CurrentCategoryIndex][i].AbsPosition;
					spriteObj.X -= 20f;
					spriteObj.Y -= 30f;
					this.m_newIconListIndex++;
				}
			}
			sbyte b2 = Game.PlayerStats.GetEquippedRuneArray[this.CurrentCategoryIndex];
			if (b2 > -1)
			{
				this.m_equippedIcon.Position = new Vector2(this.m_activeIconArray[(int)b2].AbsPosition.X + 18f, this.m_activeIconArray[(int)b2].AbsPosition.Y + 18f);
				this.m_equippedIcon.Visible = true;
				return;
			}
			this.m_equippedIcon.Visible = false;
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
			this.m_enchantressUI.Draw(base.Camera);
			this.m_selectionIcon.Draw(base.Camera);
			this.m_descriptionText.Draw(base.Camera);
			this.m_instructionsTitleText.Draw(base.Camera);
			this.m_instructionsText.Draw(base.Camera);
			this.m_unlockCostContainer.Draw(base.Camera);
			this.m_equipmentTitleText.Draw(base.Camera);
			foreach (SpriteObj[] current in this.m_masterIconArray)
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
				Console.WriteLine("Disposing Enchantress Screen");
				if (this.m_rainSound != null)
				{
					this.m_rainSound.Dispose();
				}
				this.m_rainSound = null;
				this.m_enchantressUI.Dispose();
				this.m_enchantressUI = null;
				this.m_equipmentDescriptionText.Dispose();
				this.m_equipmentDescriptionText = null;
				this.m_selectionIcon.Dispose();
				this.m_selectionIcon = null;
				this.m_equipmentTitleText.Dispose();
				this.m_equipmentTitleText = null;
				this.m_activeIconArray = null;
				foreach (SpriteObj[] current in this.m_masterIconArray)
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
				this.m_descriptionText.Dispose();
				this.m_descriptionText = null;
				this.m_unlockCostContainer.Dispose();
				this.m_unlockCostContainer = null;
				this.m_instructionsText.Dispose();
				this.m_instructionsText = null;
				this.m_instructionsTitleText.Dispose();
				this.m_instructionsTitleText = null;
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
