// Rogue Legacy Randomizer - KeyIconTextObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLegacy
{
    public class KeyIconTextObj : TextObj
    {
        private List<KeyIconObj> m_iconList;
        private List<float> m_iconOffset;
        private float m_yOffset;

        public KeyIconTextObj(SpriteFont font = null) : base(font)
        {
            ForcedScale = Vector2.One;
            m_iconList = new List<KeyIconObj>();
            m_iconOffset = new List<float>();
        }

        public Vector2 ForcedScale { get; set; }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                foreach (var current in m_iconList) current.Dispose();
                m_iconList.Clear();
                m_iconOffset.Clear();
                var text = value;
                for (var num = text.IndexOf("["); num != -1; num = text.IndexOf("["))
                {
                    var num2 = text.IndexOf("]");
                    if (num2 == -1)
                    {
                        throw new Exception(
                            "ERROR in KeyIconTextObj: Found starting index for icon but could not find ending index. Make sure every '[' ends with a ']'.");
                    }

                    text = AddKeyIcon(text, num, num2);
                }

                base.Text = text;
            }
        }

        public string AddKeyIcon(string text, int startIndex, int endIndex)
        {
            var keyIconObj = new KeyIconObj();
            var arg_0B_0 = Vector2.Zero;
            var text2 = text.Substring(startIndex, endIndex - startIndex + 1);
            var oldValue = text2;
            if (text2.Contains("Input"))
            {
                var text3 = text2.Replace("[Input:", "");
                text3 = text3.Replace("]", "");
                var index = byte.Parse(text3);
                if (InputManager.GamePadIsConnected(PlayerIndex.One))
                {
                    var buttons = Game.GlobalInput.ButtonList[index];
                    text2 = text2.Replace(text3, buttons.ToString());
                    text2 = text2.Replace("Input", "Button");
                }
                else
                {
                    var keys = Game.GlobalInput.KeyList[index];
                    text2 = text2.Replace(text3, keys.ToString());
                    text2 = text2.Replace("Input", "Key");
                }
            }

            if (text2.Contains("Key"))
            {
                var text4 = text2.Replace("[Key:", "");
                text4 = text4.Replace("]", "");
                var upperCase = true;
                if (text4 == "Enter" || text4 == "Space")
                {
                    upperCase = false;
                }

                keyIconObj.SetKey((Keys) Enum.Parse(typeof(Keys), text4), upperCase);
                var y = (Font.MeasureString("0") * m_internalFontSizeScale * Scale).Y;
                keyIconObj.Scale = new Vector2(y / keyIconObj.Height, y / keyIconObj.Height) * ForcedScale;
                m_yOffset = y / 2f;
                var text5 = " ";
                while (Font.MeasureString(text5).X * m_internalFontSizeScale.X * Scale.X < keyIconObj.Width)
                    text5 += " ";
                var text6 = text.Substring(0, text.IndexOf("["));
                text = text.Replace(oldValue, text5);
                var num = Font.MeasureString(text5).X * m_internalFontSizeScale.X * Scale.X;
                num /= 2f;
                var num2 = Font.MeasureString(text6).X * m_internalFontSizeScale.X * Scale.X;
                var item = num2 + num;
                m_iconOffset.Add(item);
            }
            else
            {
                var text7 = text2.Replace("[Button:", "");
                text7 = text7.Replace("]", "");
                keyIconObj.SetButton((Buttons) Enum.Parse(typeof(Buttons), text7));
                var y2 = (Font.MeasureString("0") * m_internalFontSizeScale * Scale).Y;
                keyIconObj.Scale = new Vector2(y2 / keyIconObj.Height, y2 / keyIconObj.Height) * ForcedScale;
                m_yOffset = y2 / 2f;
                var text8 = " ";
                while (Font.MeasureString(text8).X * m_internalFontSizeScale.X * Scale.X < keyIconObj.Width)
                    text8 += " ";
                var text9 = text.Substring(0, text.IndexOf("["));
                text = text.Replace(oldValue, text8);
                var num3 = Font.MeasureString(text8).X * m_internalFontSizeScale.X * Scale.X;
                num3 /= 2f;
                var num4 = Font.MeasureString(text9).X * m_internalFontSizeScale.X * Scale.X;
                var item2 = num4 + num3;
                m_iconOffset.Add(item2);
            }

            m_iconList.Add(keyIconObj);
            return text;
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            var i = 0;
            while (i < m_iconList.Count)
            {
                var keyIconObj = m_iconList[i];
                switch (Align)
                {
                    case Types.TextAlign.Left:
                        goto IL_A4;

                    case Types.TextAlign.Centre:
                        keyIconObj.Position = new Vector2(AbsX + m_iconOffset[i] - Width / 2, AbsY + m_yOffset);
                        break;

                    case Types.TextAlign.Right:
                        keyIconObj.Position = new Vector2(AbsX + m_iconOffset[i] - Width, AbsY + m_yOffset);
                        break;

                    default:
                        goto IL_A4;
                }

                IL_CF:
                keyIconObj.ForceDraw = ForceDraw;
                keyIconObj.Opacity = Opacity;
                keyIconObj.Draw(camera);
                keyIconObj.Visible = Visible;
                i++;
                continue;
                IL_A4:
                keyIconObj.Position = new Vector2(AbsX + m_iconOffset[i], AbsY + m_yOffset);
                goto IL_CF;
            }
        }

        public override void WordWrap(int width)
        {
            var list = new List<KeyIconObj>();
            foreach (var current in m_iconList) list.Add(current.Clone() as KeyIconObj);
            var list2 = new List<float>();
            list2.AddRange(m_iconOffset);
            base.WordWrap(width);
            m_iconList = list;
            m_iconOffset = list2;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (var current in m_iconList) current.Dispose();
                m_iconList.Clear();
                m_iconList = null;
                m_iconOffset.Clear();
                m_iconOffset = null;
                base.Dispose();
            }
        }
    }
}
