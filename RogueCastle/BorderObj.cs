/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class BorderObj : GameObj
	{
		public bool BorderTop;
		public bool BorderBottom;
		public bool BorderLeft;
		public bool BorderRight;
		public GameTypes.LevelType LevelType = GameTypes.LevelType.CASTLE;
		public Texture2D BorderTexture
		{
			get;
			internal set;
		}
		public SpriteObj CornerTexture
		{
			get;
			internal set;
		}
		public SpriteObj CornerLTexture
		{
			get;
			internal set;
		}
		public Texture2D NeoTexture
		{
			get;
			set;
		}
		public Vector2 TextureScale
		{
			get;
			set;
		}
		public Vector2 TextureOffset
		{
			get;
			set;
		}
		public BorderObj()
		{
			TextureScale = new Vector2(1f, 1f);
			CornerTexture = new SpriteObj("Blank_Sprite");
			CornerTexture.Scale = new Vector2(2f, 2f);
			CornerLTexture = new SpriteObj("Blank_Sprite");
			CornerLTexture.Scale = new Vector2(2f, 2f);
		}
		public void SetBorderTextures(Texture2D borderTexture, string cornerTextureString, string cornerLTextureString)
		{
			BorderTexture = borderTexture;
			CornerTexture.ChangeSprite(cornerTextureString);
			CornerLTexture.ChangeSprite(cornerLTextureString);
		}
		public void SetWidth(int width)
		{
			_width = width;
		}
		public void SetHeight(int height)
		{
			_height = height;
			if (height < 60)
			{
				BorderBottom = false;
				BorderLeft = false;
				BorderRight = false;
				TextureColor = new Color(150, 150, 150);
			}
		}
		public override void Draw(Camera2D camera)
		{
			Texture2D texture2D = BorderTexture;
			if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
			{
				TextureOffset = Vector2.Zero;
				texture2D = NeoTexture;
			}
			if (BorderBottom)
			{
				camera.Draw(texture2D, new Vector2(Bounds.Right - CornerTexture.Width + TextureOffset.X, Bounds.Bottom - TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)(Width / TextureScale.X) - CornerTexture.Width * 2, texture2D.Height)), TextureColor, MathHelper.ToRadians(180f), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
			}
			if (BorderLeft)
			{
				camera.Draw(texture2D, new Vector2(X + TextureOffset.Y, Bounds.Bottom - CornerTexture.Width - TextureOffset.X), new Rectangle?(new Rectangle(0, 0, (int)(Height / TextureScale.Y) - CornerTexture.Width * 2, texture2D.Height)), TextureColor, MathHelper.ToRadians(-90f), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
			}
			if (BorderRight)
			{
				camera.Draw(texture2D, new Vector2(Bounds.Right - TextureOffset.Y, Y + CornerTexture.Width + TextureOffset.X), new Rectangle?(new Rectangle(0, 0, (int)(Height / TextureScale.Y) - CornerTexture.Width * 2, texture2D.Height)), TextureColor, MathHelper.ToRadians(90f), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
			}
			if (BorderTop)
			{
				if (Rotation == 0f)
				{
					camera.Draw(texture2D, new Vector2(X + CornerTexture.Width + TextureOffset.X, Y + TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)(Width / TextureScale.X) - CornerTexture.Width * 2, texture2D.Height)), TextureColor, MathHelper.ToRadians(Rotation), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
				}
				else
				{
					Vector2 position = CollisionMath.UpperLeftCorner(new Rectangle((int)X, (int)Y, _width, _height), Rotation, Vector2.Zero);
					Vector2 position2 = CollisionMath.UpperRightCorner(new Rectangle((int)X, (int)Y, _width, _height), Rotation, Vector2.Zero);
					if (Rotation > 0f && Rotation < 80f)
					{
						CornerTexture.Flip = SpriteEffects.FlipHorizontally;
						CornerTexture.Position = position;
						CornerTexture.Rotation = 0f;
						CornerTexture.Draw(camera);
						CornerTexture.Flip = SpriteEffects.None;
						CornerTexture.Position = new Vector2(position2.X - CornerTexture.Width / 2f, position2.Y);
						CornerTexture.Rotation = 0f;
						CornerTexture.Draw(camera);
					}
					if (Rotation < 0f && Rotation > -80f)
					{
						CornerTexture.Flip = SpriteEffects.FlipHorizontally;
						CornerTexture.Position = position;
						CornerTexture.X += CornerTexture.Width / 2f;
						CornerTexture.Rotation = 0f;
						CornerTexture.Draw(camera);
						CornerTexture.Flip = SpriteEffects.None;
						CornerTexture.Position = position2;
						CornerTexture.Rotation = 0f;
						CornerTexture.Draw(camera);
					}
					camera.Draw(texture2D, new Vector2(X + TextureOffset.X - (float)Math.Sin(MathHelper.ToRadians(Rotation)) * TextureOffset.Y, Y + (float)Math.Cos(MathHelper.ToRadians(Rotation)) * TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)(Width / TextureScale.X), texture2D.Height)), TextureColor, MathHelper.ToRadians(Rotation), Vector2.Zero, TextureScale, SpriteEffects.None, 0f);
				}
			}
			base.Draw(camera);
		}
		public void DrawCorners(Camera2D camera)
		{
			CornerTexture.TextureColor = TextureColor;
			CornerLTexture.TextureColor = TextureColor;
			CornerLTexture.Flip = SpriteEffects.None;
			CornerTexture.Flip = SpriteEffects.None;
			CornerLTexture.Rotation = 0f;
			if (BorderTop)
			{
				if (BorderRight)
				{
					CornerLTexture.Position = new Vector2(Bounds.Right - CornerLTexture.Width, Bounds.Top);
					CornerLTexture.Draw(camera);
				}
				else
				{
					CornerTexture.Position = new Vector2(Bounds.Right - CornerTexture.Width, Bounds.Top);
					CornerTexture.Draw(camera);
				}
				CornerLTexture.Flip = SpriteEffects.FlipHorizontally;
				CornerTexture.Flip = SpriteEffects.FlipHorizontally;
				if (BorderLeft)
				{
					CornerLTexture.Position = new Vector2(Bounds.Left + CornerLTexture.Width, Bounds.Top);
					CornerLTexture.Draw(camera);
				}
				else
				{
					CornerTexture.Position = new Vector2(Bounds.Left + CornerTexture.Width, Bounds.Top);
					CornerTexture.Draw(camera);
				}
			}
			if (BorderBottom)
			{
				CornerTexture.Flip = SpriteEffects.FlipVertically;
				CornerLTexture.Flip = SpriteEffects.FlipVertically;
				if (BorderRight)
				{
					CornerLTexture.Position = new Vector2(Bounds.Right - CornerLTexture.Width, Bounds.Bottom - CornerLTexture.Height);
					CornerLTexture.Draw(camera);
				}
				else
				{
					CornerTexture.Flip = SpriteEffects.FlipVertically;
					CornerTexture.Position = new Vector2(Bounds.Right - CornerTexture.Width, Bounds.Bottom - CornerTexture.Height);
					CornerTexture.Draw(camera);
				}
				if (BorderLeft)
				{
					CornerLTexture.Position = new Vector2(Bounds.Left + CornerLTexture.Width, Bounds.Bottom - CornerLTexture.Height);
					CornerLTexture.Rotation = 90f;
					CornerLTexture.Draw(camera);
					CornerLTexture.Rotation = 0f;
				}
				else
				{
					CornerTexture.Flip = SpriteEffects.None;
					CornerTexture.Position = new Vector2(Bounds.Left + CornerTexture.Width, Bounds.Bottom);
					CornerTexture.Rotation = 180f;
					CornerTexture.Draw(camera);
					CornerTexture.Rotation = 0f;
				}
			}
			if (BorderLeft)
			{
				CornerTexture.Flip = SpriteEffects.None;
				CornerLTexture.Flip = SpriteEffects.None;
				if (!BorderBottom)
				{
					CornerTexture.Position = new Vector2(Bounds.Left, Bounds.Bottom - CornerTexture.Width);
					CornerTexture.Flip = SpriteEffects.FlipHorizontally;
					CornerTexture.Rotation = -90f;
					CornerTexture.Draw(camera);
					CornerTexture.Rotation = 0f;
				}
				if (!BorderTop)
				{
					CornerTexture.Position = new Vector2(Bounds.Left, Bounds.Top + CornerTexture.Width);
					CornerTexture.Flip = SpriteEffects.None;
					CornerTexture.Rotation = -90f;
					CornerTexture.Draw(camera);
					CornerTexture.Rotation = 0f;
				}
			}
			if (BorderRight)
			{
				CornerTexture.Flip = SpriteEffects.None;
				CornerLTexture.Flip = SpriteEffects.None;
				if (!BorderBottom)
				{
					CornerTexture.Position = new Vector2(Bounds.Right, Bounds.Bottom - CornerTexture.Width);
					CornerTexture.Rotation = 90f;
					CornerTexture.Draw(camera);
					CornerTexture.Rotation = 0f;
				}
				if (!BorderTop)
				{
					CornerTexture.Position = new Vector2(Bounds.Right, Bounds.Top + CornerTexture.Width);
					CornerTexture.Flip = SpriteEffects.FlipHorizontally;
					CornerTexture.Rotation = 90f;
					CornerTexture.Draw(camera);
					CornerTexture.Rotation = 0f;
				}
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new BorderObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			BorderObj borderObj = obj as BorderObj;
			borderObj.LevelType = LevelType;
			borderObj.BorderTop = BorderTop;
			borderObj.BorderBottom = BorderBottom;
			borderObj.BorderLeft = BorderLeft;
			borderObj.BorderRight = BorderRight;
			borderObj.SetHeight(_height);
			borderObj.SetWidth(_width);
			borderObj.NeoTexture = NeoTexture;
			borderObj.SetBorderTextures(BorderTexture, CornerTexture.SpriteName, CornerLTexture.SpriteName);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				BorderTexture = null;
				CornerTexture.Dispose();
				CornerTexture = null;
				CornerLTexture.Dispose();
				CornerLTexture = null;
				NeoTexture = null;
				base.Dispose();
			}
		}
		public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
		{
			base.PopulateFromXMLReader(reader, ci);
			SetWidth(_width);
			SetHeight(_height);
			if (reader.MoveToAttribute("CollidesTop"))
			{
				BorderTop = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("CollidesBottom"))
			{
				BorderBottom = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("CollidesLeft"))
			{
				BorderLeft = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("CollidesRight"))
			{
				BorderRight = bool.Parse(reader.Value);
			}
		}
	}
}
