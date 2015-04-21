using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
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
				return this.m_storedKey;
			}
		}
		public Buttons Button
		{
			get
			{
				return this.m_storedButton;
			}
		}
		public KeyIconObj()
		{
			SpriteObj obj = new SpriteObj("KeyboardButton_Sprite");
			this.AddChild(obj);
			this.m_buttonText = new TextObj(Game.PixelArtFont);
			this.m_buttonText.FontSize = 16f;
			this.m_buttonText.DropShadow = new Vector2(1f, 1f);
			this.m_buttonText.Align = Types.TextAlign.Centre;
			this.m_buttonText.Y = (float)(-(float)(this.m_buttonText.Height / 2));
			this.AddChild(this.m_buttonText);
			this.m_buttonText.Visible = false;
		}
		public void SetKey(Keys? key, bool upperCase = true)
		{
			this.m_storedKey = key.Value;
			this.m_buttonText.FontSize = 20f;
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
						base.GetChildAt(0).ChangeSprite("KeyboardSpacebar_Sprite");
						goto IL_1A7;
					case Keys.PageUp:
					case Keys.PageDown:
					case Keys.End:
					case Keys.Home:
						goto IL_186;
					case Keys.Left:
						base.GetChildAt(0).ChangeSprite("KeyboardArrowLeft_Sprite");
						flag = false;
						goto IL_1A7;
					case Keys.Up:
						base.GetChildAt(0).ChangeSprite("KeyboardArrowUp_Sprite");
						flag = false;
						goto IL_1A7;
					case Keys.Right:
						base.GetChildAt(0).ChangeSprite("KeyboardArrowRight_Sprite");
						flag = false;
						goto IL_1A7;
					case Keys.Down:
						base.GetChildAt(0).ChangeSprite("KeyboardArrowDown_Sprite");
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
							base.GetChildAt(0).ChangeSprite("KeyboardButtonLong_Sprite");
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
				base.GetChildAt(0).ChangeSprite("KeyboardButtonLong_Sprite");
				this.m_buttonText.FontSize = 16f;
				goto IL_1A7;
			}
			IL_186:
			base.GetChildAt(0).ChangeSprite("KeyboardButton_Sprite");
			this.m_buttonText.FontSize = 24f;
			IL_1A7:
			if (!key.HasValue)
			{
				base.GetChildAt(0).ChangeSprite("KeyboardButtonLong_Sprite");
			}
			if (flag)
			{
				if (upperCase)
				{
					this.m_buttonText.Text = InputReader.GetInputString(key, false, false, false).ToUpper();
				}
				else
				{
					this.m_buttonText.Text = InputReader.GetInputString(key, false, false, false);
				}
			}
			else
			{
				this.m_buttonText.Text = "";
			}
			this.m_buttonText.Y = (float)(-(float)(this.m_buttonText.Height / 2));
			this.m_buttonText.Visible = true;
			this.IsGamepadButton = false;
			base.CalculateBounds();
		}
		public void SetButton(Buttons button)
		{
			this.m_storedButton = button;
			this.IsGamepadButton = true;
			this.m_buttonText.Visible = false;
			if (button <= Buttons.LeftShoulder)
			{
				if (button <= Buttons.Start)
				{
					switch (button)
					{
					case Buttons.DPadUp:
						base.GetChildAt(0).ChangeSprite("GamepadUpArrow_Sprite");
						break;
					case Buttons.DPadDown:
						base.GetChildAt(0).ChangeSprite("GamepadDownArrow_Sprite");
						break;
					case Buttons.DPadUp | Buttons.DPadDown:
						break;
					case Buttons.DPadLeft:
						base.GetChildAt(0).ChangeSprite("GamepadLeftArrow_Sprite");
						break;
					default:
						if (button != Buttons.DPadRight)
						{
							if (button == Buttons.Start)
							{
								base.GetChildAt(0).ChangeSprite("GamepadStartButton_Sprite");
							}
						}
						else
						{
							base.GetChildAt(0).ChangeSprite("GamepadRightArrow_Sprite");
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
							base.GetChildAt(0).ChangeSprite("GamepadLeftStick_Sprite");
						}
					}
					else
					{
						base.GetChildAt(0).ChangeSprite("GamepadBackButton_Sprite");
					}
				}
				else if (button != Buttons.RightStick)
				{
					if (button == Buttons.LeftShoulder)
					{
						base.GetChildAt(0).ChangeSprite("GamepadLeftButton_Sprite");
					}
				}
				else
				{
					base.GetChildAt(0).ChangeSprite("GamepadRightStick_Sprite");
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
							base.GetChildAt(0).ChangeSprite("GamepadBButton_Sprite");
						}
					}
					else
					{
						base.GetChildAt(0).ChangeSprite("GamepadAButton_Sprite");
					}
				}
				else
				{
					base.GetChildAt(0).ChangeSprite("GamepadRightButton_Sprite");
				}
			}
			else if (button <= Buttons.Y)
			{
				if (button != Buttons.X)
				{
					if (button == Buttons.Y)
					{
						base.GetChildAt(0).ChangeSprite("GamepadYButton_Sprite");
					}
				}
				else
				{
					base.GetChildAt(0).ChangeSprite("GamepadXButton_Sprite");
				}
			}
			else if (button != Buttons.RightTrigger)
			{
				if (button == Buttons.LeftTrigger)
				{
					base.GetChildAt(0).ChangeSprite("GamepadLeftTrigger_Sprite");
				}
			}
			else
			{
				base.GetChildAt(0).ChangeSprite("GamepadRightTrigger_Sprite");
			}
			base.CalculateBounds();
		}
		protected override GameObj CreateCloneInstance()
		{
			return new KeyIconObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			KeyIconObj keyIconObj = obj as KeyIconObj;
			if (this.IsGamepadButton)
			{
				keyIconObj.SetButton(this.Button);
				return;
			}
			keyIconObj.SetKey(new Keys?(this.Key), true);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_buttonText = null;
				base.Dispose();
			}
		}
	}
}
