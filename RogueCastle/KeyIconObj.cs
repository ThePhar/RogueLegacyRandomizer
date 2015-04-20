/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RogueCastle
{
	public class KeyIconObj : ObjContainer
	{
		private TextObj m_buttonText;
		private Keys m_storedKey;
		private Buttons m_storedButton;
		public bool IsGamepadButton
		{
			get;
			internal set;
		}
		public Keys Key
		{
			get
			{
				return m_storedKey;
			}
		}
		public Buttons Button
		{
			get
			{
				return m_storedButton;
			}
		}
		public KeyIconObj()
		{
			SpriteObj obj = new SpriteObj("KeyboardButton_Sprite");
			AddChild(obj);
			m_buttonText = new TextObj(Game.PixelArtFont);
			m_buttonText.FontSize = 16f;
			m_buttonText.DropShadow = new Vector2(1f, 1f);
			m_buttonText.Align = Types.TextAlign.Centre;
			m_buttonText.Y = -(float)(m_buttonText.Height / 2);
			AddChild(m_buttonText);
			m_buttonText.Visible = false;
		}
		public void SetKey(Keys? key, bool upperCase = true)
		{
			m_storedKey = key.Value;
			m_buttonText.FontSize = 20f;
			bool flag = true;
			Keys valueOrDefault = key.GetValueOrDefault();
			if (key.HasValue)
			{
				if (valueOrDefault <= Keys.Escape)
				{
					switch (valueOrDefault)
					{
					case Keys.Back:
					case Keys.Tab:
						break;
					default:
						if (valueOrDefault != Keys.Enter && valueOrDefault != Keys.Escape)
						{
							goto IL_186;
						}
						break;
					}
				}
				else
				{
					switch (valueOrDefault)
					{
					case Keys.Space:
						GetChildAt(0).ChangeSprite("KeyboardSpacebar_Sprite");
						goto IL_1A7;
					case Keys.PageUp:
					case Keys.PageDown:
					case Keys.End:
					case Keys.Home:
						goto IL_186;
					case Keys.Left:
						GetChildAt(0).ChangeSprite("KeyboardArrowLeft_Sprite");
						flag = false;
						goto IL_1A7;
					case Keys.Up:
						GetChildAt(0).ChangeSprite("KeyboardArrowUp_Sprite");
						flag = false;
						goto IL_1A7;
					case Keys.Right:
						GetChildAt(0).ChangeSprite("KeyboardArrowRight_Sprite");
						flag = false;
						goto IL_1A7;
					case Keys.Down:
						GetChildAt(0).ChangeSprite("KeyboardArrowDown_Sprite");
						flag = false;
						goto IL_1A7;
					default:
						switch (valueOrDefault)
						{
						case Keys.NumPad0:
						case Keys.NumPad1:
						case Keys.NumPad2:
						case Keys.NumPad3:
						case Keys.NumPad4:
						case Keys.NumPad5:
						case Keys.NumPad6:
						case Keys.NumPad7:
						case Keys.NumPad8:
						case Keys.NumPad9:
							GetChildAt(0).ChangeSprite("KeyboardButtonLong_Sprite");
							goto IL_1A7;
						default:
							switch (valueOrDefault)
							{
							case Keys.LeftShift:
							case Keys.RightShift:
							case Keys.LeftControl:
							case Keys.RightControl:
							case Keys.LeftAlt:
							case Keys.RightAlt:
								break;
							default:
								goto IL_186;
							}
							break;
						}
						break;
					}
				}
				GetChildAt(0).ChangeSprite("KeyboardButtonLong_Sprite");
				m_buttonText.FontSize = 16f;
				goto IL_1A7;
			}
			IL_186:
			GetChildAt(0).ChangeSprite("KeyboardButton_Sprite");
			m_buttonText.FontSize = 24f;
			IL_1A7:
			if (!key.HasValue)
			{
				GetChildAt(0).ChangeSprite("KeyboardButtonLong_Sprite");
			}
			if (flag)
			{
				if (upperCase)
				{
					m_buttonText.Text = InputReader.GetInputString(key, false, false, false).ToUpper();
				}
				else
				{
					m_buttonText.Text = InputReader.GetInputString(key, false, false, false);
				}
			}
			else
			{
				m_buttonText.Text = "";
			}
			m_buttonText.Y = -(float)(m_buttonText.Height / 2);
			m_buttonText.Visible = true;
			IsGamepadButton = false;
			CalculateBounds();
		}
		public void SetButton(Buttons button)
		{
			m_storedButton = button;
			IsGamepadButton = true;
			m_buttonText.Visible = false;
			if (button <= Buttons.LeftShoulder)
			{
				if (button <= Buttons.Start)
				{
					switch (button)
					{
					case Buttons.DPadUp:
						GetChildAt(0).ChangeSprite("GamepadUpArrow_Sprite");
						break;
					case Buttons.DPadDown:
						GetChildAt(0).ChangeSprite("GamepadDownArrow_Sprite");
						break;
					case Buttons.DPadUp | Buttons.DPadDown:
						break;
					case Buttons.DPadLeft:
						GetChildAt(0).ChangeSprite("GamepadLeftArrow_Sprite");
						break;
					default:
						if (button != Buttons.DPadRight)
						{
							if (button == Buttons.Start)
							{
								GetChildAt(0).ChangeSprite("GamepadStartButton_Sprite");
							}
						}
						else
						{
							GetChildAt(0).ChangeSprite("GamepadRightArrow_Sprite");
						}
						break;
					}
				}
				else if (button <= Buttons.LeftStick)
				{
					if (button != Buttons.Back)
					{
						if (button == Buttons.LeftStick)
						{
							GetChildAt(0).ChangeSprite("GamepadLeftStick_Sprite");
						}
					}
					else
					{
						GetChildAt(0).ChangeSprite("GamepadBackButton_Sprite");
					}
				}
				else if (button != Buttons.RightStick)
				{
					if (button == Buttons.LeftShoulder)
					{
						GetChildAt(0).ChangeSprite("GamepadLeftButton_Sprite");
					}
				}
				else
				{
					GetChildAt(0).ChangeSprite("GamepadRightStick_Sprite");
				}
			}
			else if (button <= Buttons.B)
			{
				if (button != Buttons.RightShoulder)
				{
					if (button != Buttons.A)
					{
						if (button == Buttons.B)
						{
							GetChildAt(0).ChangeSprite("GamepadBButton_Sprite");
						}
					}
					else
					{
						GetChildAt(0).ChangeSprite("GamepadAButton_Sprite");
					}
				}
				else
				{
					GetChildAt(0).ChangeSprite("GamepadRightButton_Sprite");
				}
			}
			else if (button <= Buttons.Y)
			{
				if (button != Buttons.X)
				{
					if (button == Buttons.Y)
					{
						GetChildAt(0).ChangeSprite("GamepadYButton_Sprite");
					}
				}
				else
				{
					GetChildAt(0).ChangeSprite("GamepadXButton_Sprite");
				}
			}
			else if (button != Buttons.RightTrigger)
			{
				if (button == Buttons.LeftTrigger)
				{
					GetChildAt(0).ChangeSprite("GamepadLeftTrigger_Sprite");
				}
			}
			else
			{
				GetChildAt(0).ChangeSprite("GamepadRightTrigger_Sprite");
			}
			CalculateBounds();
		}
		protected override GameObj CreateCloneInstance()
		{
			return new KeyIconObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			KeyIconObj keyIconObj = obj as KeyIconObj;
			if (IsGamepadButton)
			{
				keyIconObj.SetButton(Button);
				return;
			}
			keyIconObj.SetKey(new Keys?(Key), true);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_buttonText = null;
				base.Dispose();
			}
		}
	}
}
