using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace RogueCastle
{
	public class KeyIconTextObj : TextObj
	{
		private List<KeyIconObj> m_iconList;
		private List<float> m_iconOffset;
		private float m_yOffset;
		public Vector2 ForcedScale
		{
			get;
			set;
		}
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				foreach (KeyIconObj current in this.m_iconList)
				{
					current.Dispose();
				}
				this.m_iconList.Clear();
				this.m_iconOffset.Clear();
				string text = value;
				for (int num = text.IndexOf("["); num != -1; num = text.IndexOf("["))
				{
					int num2 = text.IndexOf("]");
					if (num2 == -1)
					{
						throw new Exception("ERROR in KeyIconTextObj: Found starting index for icon but could not find ending index. Make sure every '[' ends with a ']'.");
					}
					text = this.AddKeyIcon(text, num, num2);
				}
				base.Text = text;
			}
		}
		public KeyIconTextObj(SpriteFont font = null) : base(font)
		{
			this.ForcedScale = Vector2.One;
			this.m_iconList = new List<KeyIconObj>();
			this.m_iconOffset = new List<float>();
		}
		public string AddKeyIcon(string text, int startIndex, int endIndex)
		{
			KeyIconObj keyIconObj = new KeyIconObj();
			Vector2 arg_0B_0 = Vector2.Zero;
			string text2 = text.Substring(startIndex, endIndex - startIndex + 1);
			string oldValue = text2;
			if (text2.Contains("Input"))
			{
				string text3 = text2.Replace("[Input:", "");
				text3 = text3.Replace("]", "");
				byte index = byte.Parse(text3);
				if (InputManager.GamePadIsConnected(PlayerIndex.One))
				{
					Buttons buttons = Game.GlobalInput.ButtonList[(int)index];
					text2 = text2.Replace(text3, buttons.ToString());
					text2 = text2.Replace("Input", "Button");
				}
				else
				{
					Keys keys = Game.GlobalInput.KeyList[(int)index];
					text2 = text2.Replace(text3, keys.ToString());
					text2 = text2.Replace("Input", "Key");
				}
			}
			if (text2.Contains("Key"))
			{
				string text4 = text2.Replace("[Key:", "");
				text4 = text4.Replace("]", "");
				bool upperCase = true;
				if (text4 == "Enter" || text4 == "Space")
				{
					upperCase = false;
				}
				keyIconObj.SetKey(new Keys?((Keys)Enum.Parse(typeof(Keys), text4)), upperCase);
				float y = (base.Font.MeasureString("0") * this.m_internalFontSizeScale * this.Scale).Y;
				keyIconObj.Scale = new Vector2(y / (float)keyIconObj.Height, y / (float)keyIconObj.Height) * this.ForcedScale;
				this.m_yOffset = y / 2f;
				string text5 = " ";
				while (base.Font.MeasureString(text5).X * this.m_internalFontSizeScale.X * this.Scale.X < (float)keyIconObj.Width)
				{
					text5 += " ";
				}
				string text6 = text.Substring(0, text.IndexOf("["));
				text = text.Replace(oldValue, text5);
				float num = base.Font.MeasureString(text5).X * this.m_internalFontSizeScale.X * this.Scale.X;
				num /= 2f;
				float num2 = base.Font.MeasureString(text6).X * this.m_internalFontSizeScale.X * this.Scale.X;
				float item = num2 + num;
				this.m_iconOffset.Add(item);
			}
			else
			{
				string text7 = text2.Replace("[Button:", "");
				text7 = text7.Replace("]", "");
				keyIconObj.SetButton((Buttons)Enum.Parse(typeof(Buttons), text7));
				float y2 = (base.Font.MeasureString("0") * this.m_internalFontSizeScale * this.Scale).Y;
				keyIconObj.Scale = new Vector2(y2 / (float)keyIconObj.Height, y2 / (float)keyIconObj.Height) * this.ForcedScale;
				this.m_yOffset = y2 / 2f;
				string text8 = " ";
				while (base.Font.MeasureString(text8).X * this.m_internalFontSizeScale.X * this.Scale.X < (float)keyIconObj.Width)
				{
					text8 += " ";
				}
				string text9 = text.Substring(0, text.IndexOf("["));
				text = text.Replace(oldValue, text8);
				float num3 = base.Font.MeasureString(text8).X * this.m_internalFontSizeScale.X * this.Scale.X;
				num3 /= 2f;
				float num4 = base.Font.MeasureString(text9).X * this.m_internalFontSizeScale.X * this.Scale.X;
				float item2 = num4 + num3;
				this.m_iconOffset.Add(item2);
			}
			this.m_iconList.Add(keyIconObj);
			return text;
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			int i = 0;
			while (i < this.m_iconList.Count)
			{
				KeyIconObj keyIconObj = this.m_iconList[i];
				switch (this.Align)
				{
				case Types.TextAlign.Left:
					goto IL_A4;
				case Types.TextAlign.Centre:
					keyIconObj.Position = new Vector2(base.AbsX + this.m_iconOffset[i] - (float)(this.Width / 2), base.AbsY + this.m_yOffset);
					break;
				case Types.TextAlign.Right:
					keyIconObj.Position = new Vector2(base.AbsX + this.m_iconOffset[i] - (float)this.Width, base.AbsY + this.m_yOffset);
					break;
				default:
					goto IL_A4;
				}
				IL_CF:
				keyIconObj.ForceDraw = base.ForceDraw;
				keyIconObj.Opacity = base.Opacity;
				keyIconObj.Draw(camera);
				keyIconObj.Visible = base.Visible;
				i++;
				continue;
				IL_A4:
				keyIconObj.Position = new Vector2(base.AbsX + this.m_iconOffset[i], base.AbsY + this.m_yOffset);
				goto IL_CF;
			}
		}
		public override void WordWrap(int width)
		{
			List<KeyIconObj> list = new List<KeyIconObj>();
			foreach (KeyIconObj current in this.m_iconList)
			{
				list.Add(current.Clone() as KeyIconObj);
			}
			List<float> list2 = new List<float>();
			list2.AddRange(this.m_iconOffset);
			base.WordWrap(width);
			this.m_iconList = list;
			this.m_iconOffset = list2;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				foreach (KeyIconObj current in this.m_iconList)
				{
					current.Dispose();
				}
				this.m_iconList.Clear();
				this.m_iconList = null;
				this.m_iconOffset.Clear();
				this.m_iconOffset = null;
				base.Dispose();
			}
		}
	}
}
