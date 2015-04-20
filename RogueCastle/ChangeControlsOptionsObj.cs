/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
	public class ChangeControlsOptionsObj : OptionsObj
	{
		private List<TextObj> m_buttonTitle;
		private List<KeyIconTextObj> m_keyboardControls;
		private List<KeyIconTextObj> m_gamepadControls;
		private int m_selectedEntryIndex;
		private TextObj m_selectedEntry;
		private KeyIconTextObj m_selectedButton;
		private ObjContainer m_setKeyPlate;
		private bool m_settingKey;
		private int[] m_controlKeys;
		private bool m_lockControls;
		private int m_startingY = -200;
		private SpriteObj m_selectionBar;
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				if (value)
				{
					OnEnter();
				}
				else
				{
					OnExit();
				}
				if (value != m_isActive)
				{
					m_parentScreen.ToggleControlsConfig();
				}
				base.IsActive = value;
			}
		}
		public ChangeControlsOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Change Controls")
		{
			m_buttonTitle = new List<TextObj>();
			m_keyboardControls = new List<KeyIconTextObj>();
			m_gamepadControls = new List<KeyIconTextObj>();
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 12f;
			textObj.DropShadow = new Vector2(2f, 2f);
			string[] array = new string[]
			{
				"Up",
				"Down",
				"Left",
				"Right",
				"Attack",
				"Jump",
				"Special",
				"Dash Left",
				"Dash Right",
				"Cast Spell",
				"Reset Controls"
			};
			m_controlKeys = new int[]
			{
				16,
				18,
				20,
				22,
				12,
				10,
				13,
				14,
				15,
				24,
				-1
			};
			for (int i = 0; i < array.Length; i++)
			{
				TextObj textObj2 = textObj.Clone() as TextObj;
				textObj2.Text = array[i];
				textObj2.X = 1320f;
				textObj2.Y = m_startingY + i * 30;
				AddChild(textObj2);
				m_buttonTitle.Add(textObj2);
				KeyIconTextObj keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
				keyIconTextObj.FontSize = 9f;
				keyIconTextObj.X = textObj2.X + 200f;
				keyIconTextObj.Y = textObj2.Y + 5f;
				AddChild(keyIconTextObj);
				m_keyboardControls.Add(keyIconTextObj);
				KeyIconTextObj keyIconTextObj2 = new KeyIconTextObj(Game.JunicodeFont);
				keyIconTextObj2.FontSize = 9f;
				keyIconTextObj2.X = keyIconTextObj.X + 200f;
				keyIconTextObj2.Y = keyIconTextObj.Y;
				AddChild(keyIconTextObj2);
				m_gamepadControls.Add(keyIconTextObj2);
			}
			UpdateKeyBindings();
			m_setKeyPlate = new ObjContainer("GameOverStatPlate_Character");
			m_setKeyPlate.ForceDraw = true;
			m_setKeyPlate.Scale = Vector2.Zero;
			TextObj textObj3 = new TextObj(Game.JunicodeFont);
			textObj3.FontSize = 12f;
			textObj3.Align = Types.TextAlign.Centre;
			textObj3.DropShadow = new Vector2(2f, 2f);
			textObj3.ForceDraw = true;
			textObj3.Text = "Press Any Key";
			textObj3.Y -= textObj3.Height / 2f;
			m_setKeyPlate.AddChild(textObj3);
			m_selectionBar = new SpriteObj("OptionsBar_Sprite");
		}
		private void OnEnter()
		{
			m_selectedEntryIndex = 0;
			m_selectedEntry = m_buttonTitle[m_selectedEntryIndex];
			m_selectedEntry.TextureColor = Color.Yellow;
			m_selectedButton = null;
			m_settingKey = false;
			m_lockControls = false;
			m_setKeyPlate.Scale = Vector2.Zero;
			m_setKeyPlate.Position = new Vector2(660f, 360f);
		}
		private void OnExit()
		{
			if (m_selectedEntry != null)
			{
				m_selectedEntry.TextureColor = Color.White;
			}
			if (m_selectedButton != null)
			{
				m_selectedButton.TextureColor = Color.White;
			}
		}
		public override void HandleInput()
		{
			if (!m_lockControls)
			{
				if (!m_settingKey)
				{
					int selectedEntryIndex = m_selectedEntryIndex;
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						SoundManager.PlaySound("frame_swap");
						m_selectedEntryIndex--;
					}
					else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
					{
						SoundManager.PlaySound("frame_swap");
						m_selectedEntryIndex++;
					}
					if (m_selectedEntryIndex > m_buttonTitle.Count - 1)
					{
						m_selectedEntryIndex = 0;
					}
					if (m_selectedEntryIndex < 0)
					{
						m_selectedEntryIndex = m_buttonTitle.Count - 1;
					}
					if (selectedEntryIndex != m_selectedEntryIndex)
					{
						m_selectedEntry.TextureColor = Color.White;
						m_selectedEntry = m_buttonTitle[m_selectedEntryIndex];
						m_selectedEntry.TextureColor = Color.Yellow;
					}
					if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
					{
						SoundManager.PlaySound("Option_Menu_Select");
						m_lockControls = true;
						if (m_selectedEntryIndex == m_controlKeys.Length - 1)
						{
							RCScreenManager screenManager = Game.ScreenManager;
							screenManager.DialogueScreen.SetDialogue("RestoreDefaultControlsWarning");
							screenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
							screenManager.DialogueScreen.SetConfirmEndHandler(this, "RestoreControls", new object[0]);
							screenManager.DialogueScreen.SetCancelEndHandler(this, "CancelRestoreControls", new object[0]);
							screenManager.DisplayScreen(13, true, null);
						}
						else
						{
							Tween.To(m_setKeyPlate, 0.3f, new Easing(Back.EaseOut), new string[]
							{
								"ScaleX",
								"1",
								"ScaleY",
								"1"
							});
							Tween.AddEndHandlerToLastTween(this, "SetKeyTrue", new object[0]);
						}
					}
					else if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
					{
						IsActive = false;
					}
					base.HandleInput();
					return;
				}
				if (InputManager.AnyButtonPressed(PlayerIndex.One) || InputManager.AnyKeyPressed())
				{
					ChangeKey();
				}
			}
		}
		public void SetKeyTrue()
		{
			m_settingKey = true;
			m_lockControls = false;
		}
		private void ChangeKey()
		{
			if (InputManager.AnyKeyPressed())
			{
				Keys keys = InputManager.KeysPressedArray[0];
				if (InputReader.GetInputString(new Keys?(keys), false, false, false).ToUpper() == "")
				{
					return;
				}
				bool flag = false;
				Keys[] array = new Keys[]
				{
					Keys.Tab,
					Keys.CapsLock,
					Keys.LeftShift,
					Keys.LeftControl,
					Keys.LeftAlt,
					Keys.RightAlt,
					Keys.RightControl,
					Keys.RightShift,
					Keys.Enter,
					Keys.Back,
					Keys.Space,
					Keys.Left,
					Keys.Right,
					Keys.Up,
					Keys.Down
				};
				Keys[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Keys keys2 = array2[i];
					if (keys == keys2)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return;
				}
				if (keys == Keys.Escape)
				{
					Tween.To(m_setKeyPlate, 0.3f, new Easing(Back.EaseIn), new string[]
					{
						"ScaleX",
						"0",
						"ScaleY",
						"0"
					});
					m_settingKey = false;
					return;
				}
			}
			else if (InputManager.AnyButtonPressed(PlayerIndex.One))
			{
				Buttons buttons = InputManager.ButtonsPressedArray(PlayerIndex.One)[0];
				if (buttons == Buttons.Start || buttons == Buttons.Back)
				{
					return;
				}
			}
			SoundManager.PlaySound("Gen_Menu_Toggle");
			Tween.To(m_setKeyPlate, 0.3f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			m_settingKey = false;
			if (InputManager.AnyButtonPressed(PlayerIndex.One))
			{
				int[] array3 = new int[]
				{
					0,
					1,
					2,
					3,
					6,
					4,
					8,
					7,
					5
				};
				Buttons buttons2 = InputManager.ButtonsPressedArray(PlayerIndex.One)[0];
				for (int j = 0; j < Game.GlobalInput.ButtonList.Count; j++)
				{
					bool flag2 = true;
					int[] array4 = array3;
					for (int k = 0; k < array4.Length; k++)
					{
						int num = array4[k];
						if (j == num)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2 && Game.GlobalInput.ButtonList[j] == buttons2)
					{
						Game.GlobalInput.ButtonList[j] = Game.GlobalInput.ButtonList[m_controlKeys[m_selectedEntryIndex]];
					}
				}
				Game.GlobalInput.ButtonList[m_controlKeys[m_selectedEntryIndex]] = buttons2;
			}
			else if (InputManager.AnyKeyPressed())
			{
				Keys keys3 = InputManager.KeysPressedArray[0];
				int[] array5 = new int[]
				{
					0,
					1,
					2,
					3,
					6,
					4,
					8,
					7,
					5
				};
				for (int l = 0; l < Game.GlobalInput.KeyList.Count; l++)
				{
					bool flag3 = true;
					int[] array6 = array5;
					for (int m = 0; m < array6.Length; m++)
					{
						int num2 = array6[m];
						if (l == num2)
						{
							flag3 = false;
							break;
						}
					}
					if (flag3 && Game.GlobalInput.KeyList[l] == keys3)
					{
						Game.GlobalInput.KeyList[l] = Game.GlobalInput.KeyList[m_controlKeys[m_selectedEntryIndex]];
					}
				}
				Game.GlobalInput.KeyList[m_controlKeys[m_selectedEntryIndex]] = keys3;
			}
			UpdateKeyBindings();
		}
		private void UpdateKeyBindings()
		{
			for (int i = 0; i < m_keyboardControls.Count; i++)
			{
				if (m_controlKeys[i] == -1)
				{
					m_keyboardControls[i].Text = "";
					m_gamepadControls[i].Text = "";
				}
				else
				{
					int num = m_controlKeys[i];
					if (num != 10)
					{
						switch (num)
						{
						case 16:
							m_keyboardControls[i].Text = string.Concat(new object[]
							{
								"[Key:",
								Game.GlobalInput.KeyList[m_controlKeys[i]],
								"], [Key:",
								Game.GlobalInput.KeyList[17],
								"]"
							});
							goto IL_30B;
						case 18:
							m_keyboardControls[i].Text = string.Concat(new object[]
							{
								"[Key:",
								Game.GlobalInput.KeyList[m_controlKeys[i]],
								"], [Key:",
								Game.GlobalInput.KeyList[19],
								"]"
							});
							goto IL_30B;
						case 20:
							m_keyboardControls[i].Text = string.Concat(new object[]
							{
								"[Key:",
								Game.GlobalInput.KeyList[m_controlKeys[i]],
								"], [Key:",
								Game.GlobalInput.KeyList[21],
								"]"
							});
							goto IL_30B;
						case 22:
							m_keyboardControls[i].Text = string.Concat(new object[]
							{
								"[Key:",
								Game.GlobalInput.KeyList[m_controlKeys[i]],
								"], [Key:",
								Game.GlobalInput.KeyList[23],
								"]"
							});
							goto IL_30B;
						}
						m_keyboardControls[i].Text = "[Key:" + Game.GlobalInput.KeyList[m_controlKeys[i]] + "]";
					}
					else
					{
						m_keyboardControls[i].Text = string.Concat(new object[]
						{
							"[Key:",
							Game.GlobalInput.KeyList[m_controlKeys[i]],
							"], [Key:",
							Game.GlobalInput.KeyList[11],
							"]"
						});
					}
					IL_30B:
					m_gamepadControls[i].Text = "[Button:" + Game.GlobalInput.ButtonList[m_controlKeys[i]] + "]";
					if (Game.GlobalInput.ButtonList[m_controlKeys[i]] == 0)
					{
						m_gamepadControls[i].Text = "";
					}
					if (Game.GlobalInput.KeyList[m_controlKeys[i]] == Keys.None)
					{
						m_keyboardControls[i].Text = "";
					}
				}
			}
			Game.GlobalInput.KeyList[1] = Game.GlobalInput.KeyList[12];
			Game.GlobalInput.KeyList[3] = Game.GlobalInput.KeyList[10];
		}
		public void RestoreControls()
		{
			Game.InitializeGlobalInput();
			UpdateKeyBindings();
			m_lockControls = false;
		}
		public void CancelRestoreControls()
		{
			m_lockControls = false;
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			m_setKeyPlate.Draw(camera);
			if (m_selectedEntry != null)
			{
				m_selectionBar.Position = new Vector2(m_selectedEntry.AbsX - 15f, m_selectedEntry.AbsY);
				m_selectionBar.Draw(camera);
			}
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_buttonTitle.Clear();
				m_buttonTitle = null;
				m_keyboardControls.Clear();
				m_keyboardControls = null;
				m_gamepadControls.Clear();
				m_gamepadControls = null;
				m_selectedEntry = null;
				m_selectedButton = null;
				m_setKeyPlate.Dispose();
				m_setKeyPlate = null;
				Array.Clear(m_controlKeys, 0, m_controlKeys.Length);
				m_controlKeys = null;
				m_selectionBar.Dispose();
				m_selectionBar = null;
				base.Dispose();
			}
		}
	}
}
