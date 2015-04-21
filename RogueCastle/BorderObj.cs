using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Xml;
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
			this.TextureScale = new Vector2(1f, 1f);
			this.CornerTexture = new SpriteObj("Blank_Sprite");
			this.CornerTexture.Scale = new Vector2(2f, 2f);
			this.CornerLTexture = new SpriteObj("Blank_Sprite");
			this.CornerLTexture.Scale = new Vector2(2f, 2f);
		}
		public void SetBorderTextures(Texture2D borderTexture, string cornerTextureString, string cornerLTextureString)
		{
			this.BorderTexture = borderTexture;
			this.CornerTexture.ChangeSprite(cornerTextureString);
			this.CornerLTexture.ChangeSprite(cornerLTextureString);
		}
		public void SetWidth(int width)
		{
			this._width = width;
		}
		public void SetHeight(int height)
		{
			this._height = height;
			if (height < 60)
			{
				this.BorderBottom = false;
				this.BorderLeft = false;
				this.BorderRight = false;
				base.TextureColor = new Color(150, 150, 150);
			}
		}
		public override void Draw(Camera2D camera)
		{
			Texture2D texture2D = this.BorderTexture;
			if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
			{
				this.TextureOffset = Vector2.Zero;
				texture2D = this.NeoTexture;
			}
			if (this.BorderBottom)
			{
				camera.Draw(texture2D, new Vector2((float)(this.Bounds.Right - this.CornerTexture.Width) + this.TextureOffset.X, (float)this.Bounds.Bottom - this.TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / this.TextureScale.X) - this.CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(180f), Vector2.Zero, this.TextureScale, SpriteEffects.None, 0f);
			}
			if (this.BorderLeft)
			{
				camera.Draw(texture2D, new Vector2(base.X + this.TextureOffset.Y, (float)(this.Bounds.Bottom - this.CornerTexture.Width) - this.TextureOffset.X), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Height / this.TextureScale.Y) - this.CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(-90f), Vector2.Zero, this.TextureScale, SpriteEffects.None, 0f);
			}
			if (this.BorderRight)
			{
				camera.Draw(texture2D, new Vector2((float)this.Bounds.Right - this.TextureOffset.Y, base.Y + (float)this.CornerTexture.Width + this.TextureOffset.X), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Height / this.TextureScale.Y) - this.CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(90f), Vector2.Zero, this.TextureScale, SpriteEffects.None, 0f);
			}
			if (this.BorderTop)
			{
				if (base.Rotation == 0f)
				{
					camera.Draw(texture2D, new Vector2(base.X + (float)this.CornerTexture.Width + this.TextureOffset.X, base.Y + this.TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / this.TextureScale.X) - this.CornerTexture.Width * 2, texture2D.Height)), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, this.TextureScale, SpriteEffects.None, 0f);
				}
				else
				{
					Vector2 position = CollisionMath.UpperLeftCorner(new Rectangle((int)base.X, (int)base.Y, this._width, this._height), base.Rotation, Vector2.Zero);
					Vector2 position2 = CollisionMath.UpperRightCorner(new Rectangle((int)base.X, (int)base.Y, this._width, this._height), base.Rotation, Vector2.Zero);
					if (base.Rotation > 0f && base.Rotation < 80f)
					{
						this.CornerTexture.Flip = SpriteEffects.FlipHorizontally;
						this.CornerTexture.Position = position;
						this.CornerTexture.Rotation = 0f;
						this.CornerTexture.Draw(camera);
						this.CornerTexture.Flip = SpriteEffects.None;
						this.CornerTexture.Position = new Vector2(position2.X - (float)this.CornerTexture.Width / 2f, position2.Y);
						this.CornerTexture.Rotation = 0f;
						this.CornerTexture.Draw(camera);
					}
					if (base.Rotation < 0f && base.Rotation > -80f)
					{
						this.CornerTexture.Flip = SpriteEffects.FlipHorizontally;
						this.CornerTexture.Position = position;
						this.CornerTexture.X += (float)this.CornerTexture.Width / 2f;
						this.CornerTexture.Rotation = 0f;
						this.CornerTexture.Draw(camera);
						this.CornerTexture.Flip = SpriteEffects.None;
						this.CornerTexture.Position = position2;
						this.CornerTexture.Rotation = 0f;
						this.CornerTexture.Draw(camera);
					}
					camera.Draw(texture2D, new Vector2(base.X + this.TextureOffset.X - (float)Math.Sin((double)MathHelper.ToRadians(base.Rotation)) * this.TextureOffset.Y, base.Y + (float)Math.Cos((double)MathHelper.ToRadians(base.Rotation)) * this.TextureOffset.Y), new Rectangle?(new Rectangle(0, 0, (int)((float)this.Width / this.TextureScale.X), texture2D.Height)), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, this.TextureScale, SpriteEffects.None, 0f);
				}
			}
			base.Draw(camera);
		}
		public void DrawCorners(Camera2D camera)
		{
			this.CornerTexture.TextureColor = base.TextureColor;
			this.CornerLTexture.TextureColor = base.TextureColor;
			this.CornerLTexture.Flip = SpriteEffects.None;
			this.CornerTexture.Flip = SpriteEffects.None;
			this.CornerLTexture.Rotation = 0f;
			if (this.BorderTop)
			{
				if (this.BorderRight)
				{
					this.CornerLTexture.Position = new Vector2((float)(this.Bounds.Right - this.CornerLTexture.Width), (float)this.Bounds.Top);
					this.CornerLTexture.Draw(camera);
				}
				else
				{
					this.CornerTexture.Position = new Vector2((float)(this.Bounds.Right - this.CornerTexture.Width), (float)this.Bounds.Top);
					this.CornerTexture.Draw(camera);
				}
				this.CornerLTexture.Flip = SpriteEffects.FlipHorizontally;
				this.CornerTexture.Flip = SpriteEffects.FlipHorizontally;
				if (this.BorderLeft)
				{
					this.CornerLTexture.Position = new Vector2((float)(this.Bounds.Left + this.CornerLTexture.Width), (float)this.Bounds.Top);
					this.CornerLTexture.Draw(camera);
				}
				else
				{
					this.CornerTexture.Position = new Vector2((float)(this.Bounds.Left + this.CornerTexture.Width), (float)this.Bounds.Top);
					this.CornerTexture.Draw(camera);
				}
			}
			if (this.BorderBottom)
			{
				this.CornerTexture.Flip = SpriteEffects.FlipVertically;
				this.CornerLTexture.Flip = SpriteEffects.FlipVertically;
				if (this.BorderRight)
				{
					this.CornerLTexture.Position = new Vector2((float)(this.Bounds.Right - this.CornerLTexture.Width), (float)(this.Bounds.Bottom - this.CornerLTexture.Height));
					this.CornerLTexture.Draw(camera);
				}
				else
				{
					this.CornerTexture.Flip = SpriteEffects.FlipVertically;
					this.CornerTexture.Position = new Vector2((float)(this.Bounds.Right - this.CornerTexture.Width), (float)(this.Bounds.Bottom - this.CornerTexture.Height));
					this.CornerTexture.Draw(camera);
				}
				if (this.BorderLeft)
				{
					this.CornerLTexture.Position = new Vector2((float)(this.Bounds.Left + this.CornerLTexture.Width), (float)(this.Bounds.Bottom - this.CornerLTexture.Height));
					this.CornerLTexture.Rotation = 90f;
					this.CornerLTexture.Draw(camera);
					this.CornerLTexture.Rotation = 0f;
				}
				else
				{
					this.CornerTexture.Flip = SpriteEffects.None;
					this.CornerTexture.Position = new Vector2((float)(this.Bounds.Left + this.CornerTexture.Width), (float)this.Bounds.Bottom);
					this.CornerTexture.Rotation = 180f;
					this.CornerTexture.Draw(camera);
					this.CornerTexture.Rotation = 0f;
				}
			}
			if (this.BorderLeft)
			{
				this.CornerTexture.Flip = SpriteEffects.None;
				this.CornerLTexture.Flip = SpriteEffects.None;
				if (!this.BorderBottom)
				{
					this.CornerTexture.Position = new Vector2((float)this.Bounds.Left, (float)(this.Bounds.Bottom - this.CornerTexture.Width));
					this.CornerTexture.Flip = SpriteEffects.FlipHorizontally;
					this.CornerTexture.Rotation = -90f;
					this.CornerTexture.Draw(camera);
					this.CornerTexture.Rotation = 0f;
				}
				if (!this.BorderTop)
				{
					this.CornerTexture.Position = new Vector2((float)this.Bounds.Left, (float)(this.Bounds.Top + this.CornerTexture.Width));
					this.CornerTexture.Flip = SpriteEffects.None;
					this.CornerTexture.Rotation = -90f;
					this.CornerTexture.Draw(camera);
					this.CornerTexture.Rotation = 0f;
				}
			}
			if (this.BorderRight)
			{
				this.CornerTexture.Flip = SpriteEffects.None;
				this.CornerLTexture.Flip = SpriteEffects.None;
				if (!this.BorderBottom)
				{
					this.CornerTexture.Position = new Vector2((float)this.Bounds.Right, (float)(this.Bounds.Bottom - this.CornerTexture.Width));
					this.CornerTexture.Rotation = 90f;
					this.CornerTexture.Draw(camera);
					this.CornerTexture.Rotation = 0f;
				}
				if (!this.BorderTop)
				{
					this.CornerTexture.Position = new Vector2((float)this.Bounds.Right, (float)(this.Bounds.Top + this.CornerTexture.Width));
					this.CornerTexture.Flip = SpriteEffects.FlipHorizontally;
					this.CornerTexture.Rotation = 90f;
					this.CornerTexture.Draw(camera);
					this.CornerTexture.Rotation = 0f;
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
			borderObj.LevelType = this.LevelType;
			borderObj.BorderTop = this.BorderTop;
			borderObj.BorderBottom = this.BorderBottom;
			borderObj.BorderLeft = this.BorderLeft;
			borderObj.BorderRight = this.BorderRight;
			borderObj.SetHeight(this._height);
			borderObj.SetWidth(this._width);
			borderObj.NeoTexture = this.NeoTexture;
			borderObj.SetBorderTextures(this.BorderTexture, this.CornerTexture.SpriteName, this.CornerLTexture.SpriteName);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.BorderTexture = null;
				this.CornerTexture.Dispose();
				this.CornerTexture = null;
				this.CornerLTexture.Dispose();
				this.CornerLTexture = null;
				this.NeoTexture = null;
				base.Dispose();
			}
		}
		public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
		{
			base.PopulateFromXMLReader(reader, ci);
			this.SetWidth(this._width);
			this.SetHeight(this._height);
			if (reader.MoveToAttribute("CollidesTop"))
			{
				this.BorderTop = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("CollidesBottom"))
			{
				this.BorderBottom = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("CollidesLeft"))
			{
				this.BorderLeft = bool.Parse(reader.Value);
			}
			if (reader.MoveToAttribute("CollidesRight"))
			{
				this.BorderRight = bool.Parse(reader.Value);
			}
		}
	}
}
