using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class LineageObj : ObjContainer
	{
		private ObjContainer m_playerSprite;
		private TextObj m_playerName;
		private TextObj m_trait1Title;
		private TextObj m_trait2Title;
		private TextObj m_ageText;
		private TextObj m_classTextObj;
		private SpriteObj m_frameSprite;
		private SpriteObj m_plaqueSprite;
		private SpriteObj m_spellIcon;
		private SpriteObj m_spellIconHolder;
		private bool m_isDead;
		public bool BeatenABoss;
		public int NumEnemiesKilled;
		public byte Age = 30;
		public byte ChildAge = 4;
		public byte Class;
		public byte Spell;
		public bool IsFemale;
		public bool FlipPortrait;
		private int m_textYPos = 140;
		private Color m_skinColour1 = new Color(231, 175, 131, 255);
		private Color m_skinColour2 = new Color(199, 109, 112, 255);
		private Color m_lichColour1 = new Color(255, 255, 255, 255);
		private Color m_lichColour2 = new Color(198, 198, 198, 255);
		private bool m_frameDropping;
		public string PlayerName
		{
			get
			{
				return this.m_playerName.Text;
			}
			set
			{
				this.m_playerName.Text = value;
			}
		}
		public byte ChestPiece
		{
			get;
			set;
		}
		public byte HeadPiece
		{
			get;
			set;
		}
		public byte ShoulderPiece
		{
			get;
			set;
		}
		public bool DisablePlaque
		{
			get;
			set;
		}
		public Vector2 Traits
		{
			get;
			internal set;
		}
		public bool IsDead
		{
			get
			{
				return this.m_isDead;
			}
			set
			{
				this.m_isDead = value;
				if (value)
				{
					this.m_trait1Title.Visible = false;
					this.m_trait2Title.Visible = false;
					this.m_ageText.Visible = true;
					return;
				}
				this.m_trait1Title.Visible = true;
				this.m_trait2Title.Visible = true;
				this.m_ageText.Visible = false;
			}
		}
		public override Rectangle Bounds
		{
			get
			{
				return this.m_playerSprite.Bounds;
			}
		}
		public override Rectangle AbsBounds
		{
			get
			{
				return this.m_playerSprite.Bounds;
			}
		}
		public LineageObj(LineageScreen screen, bool createEmpty = false)
		{
			base.Name = "";
			this.m_frameSprite = new SpriteObj("LineageScreenFrame_Sprite");
			this.m_frameSprite.Scale = new Vector2(2.8f, 2.8f);
			this.m_frameSprite.DropShadow = new Vector2(4f, 6f);
			this.m_plaqueSprite = new SpriteObj("LineageScreenPlaque1Long_Sprite");
			this.m_plaqueSprite.Scale = new Vector2(1.8f, 2f);
			this.m_playerSprite = new ObjContainer("PlayerIdle_Character");
			this.m_playerSprite.AnimationDelay = 0.1f;
			this.m_playerSprite.Scale = new Vector2(2f, 2f);
			this.m_playerSprite.OutlineWidth = 2;
			this.m_playerSprite.GetChildAt(10).Visible = false;
			this.m_playerSprite.GetChildAt(11).Visible = false;
			this.m_playerSprite.GetChildAt(1).TextureColor = Color.Red;
			this.m_playerSprite.GetChildAt(7).TextureColor = Color.Red;
			this.m_playerSprite.GetChildAt(14).Visible = false;
			this.m_playerSprite.GetChildAt(16).Visible = false;
			Color textureColor = new Color(251, 156, 172);
			this.m_playerSprite.GetChildAt(13).TextureColor = textureColor;
			this.m_playerName = new TextObj(Game.JunicodeFont);
			this.m_playerName.FontSize = 10f;
			this.m_playerName.Text = "Sir Skunky IV";
			this.m_playerName.Align = Types.TextAlign.Centre;
			this.m_playerName.OutlineColour = new Color(181, 142, 39);
			this.m_playerName.OutlineWidth = 2;
			this.m_playerName.Y = (float)this.m_textYPos;
			this.m_playerName.LimitCorners = true;
			this.AddChild(this.m_playerName);
			this.m_classTextObj = new TextObj(Game.JunicodeFont);
			this.m_classTextObj.FontSize = 8f;
			this.m_classTextObj.Align = Types.TextAlign.Centre;
			this.m_classTextObj.OutlineColour = new Color(181, 142, 39);
			this.m_classTextObj.OutlineWidth = 2;
			this.m_classTextObj.Text = "the Knight";
			this.m_classTextObj.Y = this.m_playerName.Y + (float)this.m_playerName.Height - 8f;
			this.m_classTextObj.LimitCorners = true;
			this.AddChild(this.m_classTextObj);
			this.m_trait1Title = new TextObj(Game.JunicodeFont);
			this.m_trait1Title.FontSize = 8f;
			this.m_trait1Title.Align = Types.TextAlign.Centre;
			this.m_trait1Title.OutlineColour = new Color(181, 142, 39);
			this.m_trait1Title.OutlineWidth = 2;
			this.m_trait1Title.Y = this.m_classTextObj.Y + (float)this.m_classTextObj.Height + 5f;
			this.m_trait1Title.Text = "";
			this.m_trait1Title.LimitCorners = true;
			this.AddChild(this.m_trait1Title);
			this.m_trait2Title = (this.m_trait1Title.Clone() as TextObj);
			this.m_trait2Title.Y += 20f;
			this.m_trait2Title.Text = "";
			this.m_trait2Title.LimitCorners = true;
			this.AddChild(this.m_trait2Title);
			this.m_ageText = (this.m_trait1Title.Clone() as TextObj);
			this.m_ageText.Text = "xxx - xxx";
			this.m_ageText.Visible = false;
			this.m_ageText.LimitCorners = true;
			this.AddChild(this.m_ageText);
			this.m_spellIcon = new SpriteObj("Blank_Sprite");
			this.m_spellIcon.OutlineWidth = 1;
			this.m_spellIconHolder = new SpriteObj("BlacksmithUI_IconBG_Sprite");
			if (!createEmpty)
			{
				this.IsFemale = false;
				if (CDGMath.RandomInt(0, 1) > 0)
				{
					this.IsFemale = true;
				}
				if (this.IsFemale)
				{
					this.CreateFemaleName(screen);
				}
				else
				{
					this.CreateMaleName(screen);
				}
				this.Traits = TraitType.CreateRandomTraits();
				this.Class = ClassType.GetRandomClass();
				this.m_classTextObj.Text = "the " + ClassType.ToString(this.Class, this.IsFemale);
				while (this.Class == 7 || this.Class == 15)
				{
					if (this.Traits.X != 12f && this.Traits.Y != 12f)
					{
						break;
					}
					this.Traits = TraitType.CreateRandomTraits();
				}
				while ((this.Class == 1 || this.Class == 9 || this.Class == 16) && (this.Traits.X == 31f || this.Traits.Y == 31f))
				{
					this.Traits = TraitType.CreateRandomTraits();
				}
				byte[] spellList = ClassType.GetSpellList(this.Class);
				do
				{
					this.Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
				}
				while ((this.Spell == 11 || this.Spell == 4 || this.Spell == 6) && (this.Traits.X == 31f || this.Traits.Y == 31f));
				Array.Clear(spellList, 0, spellList.Length);
				this.Age = (byte)CDGMath.RandomInt(18, 30);
				this.ChildAge = (byte)CDGMath.RandomInt(2, 5);
				this.UpdateData();
			}
		}
		private void CreateMaleName(LineageScreen screen)
		{
			string text = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)];
			if (screen != null)
			{
				int num = 0;
				while (screen.CurrentBranchNameCopyFound(text))
				{
					text = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)];
					num++;
					if (num > 20)
					{
						break;
					}
				}
			}
			if (text != null)
			{
				if (text.Length > 10)
				{
					text = text.Substring(0, 9) + ".";
				}
				int num2 = 0;
				string text2 = "";
				if (screen != null)
				{
					num2 = screen.NameCopies(text);
				}
				if (num2 > 0)
				{
					text2 = CDGMath.ToRoman(num2 + 1);
				}
				this.m_playerName.Text = "Sir " + text;
				if (text2 != "")
				{
					TextObj expr_BD = this.m_playerName;
					expr_BD.Text = expr_BD.Text + " " + text2;
					return;
				}
			}
			else
			{
				this.m_playerName.Text = "Sir Hero";
			}
		}
		private void CreateFemaleName(LineageScreen screen)
		{
			string text = Game.FemaleNameArray[CDGMath.RandomInt(0, Game.FemaleNameArray.Count - 1)];
			if (screen != null)
			{
				int num = 0;
				while (screen.CurrentBranchNameCopyFound(text))
				{
					text = Game.FemaleNameArray[CDGMath.RandomInt(0, Game.FemaleNameArray.Count - 1)];
					num++;
					if (num > 20)
					{
						break;
					}
				}
			}
			if (text != null)
			{
				if (text.Length > 10)
				{
					text = text.Substring(0, 9) + ".";
				}
				int num2 = 0;
				string text2 = "";
				if (screen != null)
				{
					num2 = screen.NameCopies(text);
				}
				if (num2 > 0)
				{
					text2 = CDGMath.ToRoman(num2 + 1);
				}
				this.m_playerName.Text = "Lady " + text;
				if (text2 != "")
				{
					TextObj expr_BD = this.m_playerName;
					expr_BD.Text = expr_BD.Text + " " + text2;
					return;
				}
			}
			else
			{
				this.m_playerName.Text = "Lady Heroine";
			}
		}
		public void RandomizePortrait()
		{
			int num = CDGMath.RandomInt(1, 5);
			int num2 = CDGMath.RandomInt(1, 5);
			int num3 = CDGMath.RandomInt(1, 5);
			if (this.Class == 17)
			{
				num = 7;
			}
			else if (this.Class == 16)
			{
				num = 6;
			}
			this.SetPortrait((byte)num, (byte)num2, (byte)num3);
		}
		public void SetPortrait(byte headPiece, byte shoulderPiece, byte chestPiece)
		{
			this.HeadPiece = headPiece;
			this.ShoulderPiece = shoulderPiece;
			this.ChestPiece = chestPiece;
			string text = (this.m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
			int startIndex = text.IndexOf("_") - 1;
			text = text.Remove(startIndex, 1);
			text = text.Replace("_", this.HeadPiece + "_");
			this.m_playerSprite.GetChildAt(12).ChangeSprite(text);
			string text2 = (this.m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
			startIndex = text2.IndexOf("_") - 1;
			text2 = text2.Remove(startIndex, 1);
			text2 = text2.Replace("_", this.ChestPiece + "_");
			this.m_playerSprite.GetChildAt(4).ChangeSprite(text2);
			string text3 = (this.m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
			startIndex = text3.IndexOf("_") - 1;
			text3 = text3.Remove(startIndex, 1);
			text3 = text3.Replace("_", this.ShoulderPiece + "_");
			this.m_playerSprite.GetChildAt(9).ChangeSprite(text3);
			string text4 = (this.m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
			startIndex = text4.IndexOf("_") - 1;
			text4 = text4.Remove(startIndex, 1);
			text4 = text4.Replace("_", this.ShoulderPiece + "_");
			this.m_playerSprite.GetChildAt(3).ChangeSprite(text4);
		}
		public void UpdateAge(int currentEra)
		{
			int num = currentEra - (int)this.ChildAge;
			int num2 = currentEra + (int)this.Age;
			this.m_ageText.Text = num + " - " + num2;
		}
		public void UpdateData()
		{
			this.SetTraits(this.Traits);
			if (this.Traits.X == 8f || this.Traits.Y == 8f)
			{
				this.m_playerSprite.GetChildAt(7).Visible = false;
			}
			if (this.Traits.X == 20f || this.Traits.Y == 20f)
			{
				this.FlipPortrait = true;
			}
			this.m_classTextObj.Text = "the " + ClassType.ToString(this.Class, this.IsFemale);
			this.m_spellIcon.ChangeSprite(SpellType.Icon(this.Spell));
			if (this.Class == 0 || this.Class == 8)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleShield_Sprite");
			}
			else if (this.Class == 5 || this.Class == 13)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleLamp_Sprite");
			}
			else if (this.Class == 1 || this.Class == 9)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleBeard_Sprite");
			}
			else if (this.Class == 4 || this.Class == 12)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleHeadband_Sprite");
			}
			else if (this.Class == 2 || this.Class == 10)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("PlayerIdleHorns_Sprite");
			}
			else
			{
				this.m_playerSprite.GetChildAt(15).Visible = false;
			}
			this.m_playerSprite.GetChildAt(0).Visible = false;
			if (this.Class == 16)
			{
				this.m_playerSprite.GetChildAt(0).Visible = true;
				this.m_playerSprite.GetChildAt(12).ChangeSprite("PlayerIdleHead" + 6 + "_Sprite");
			}
			if (this.Class == 17)
			{
				this.m_playerSprite.GetChildAt(12).ChangeSprite("PlayerIdleHead" + 7 + "_Sprite");
			}
			if (!this.IsFemale)
			{
				this.m_playerSprite.GetChildAt(5).Visible = false;
				this.m_playerSprite.GetChildAt(13).Visible = false;
			}
			else
			{
				this.m_playerSprite.GetChildAt(5).Visible = true;
				this.m_playerSprite.GetChildAt(13).Visible = true;
			}
			if (this.Traits.X == 6f || this.Traits.Y == 6f)
			{
				this.m_playerSprite.Scale = new Vector2(3f, 3f);
			}
			if (this.Traits.X == 7f || this.Traits.Y == 7f)
			{
				this.m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
			}
			if (this.Traits.X == 10f || this.Traits.Y == 10f)
			{
				this.m_playerSprite.ScaleX *= 0.825f;
				this.m_playerSprite.ScaleY *= 1.25f;
			}
			if (this.Traits.X == 9f || this.Traits.Y == 9f)
			{
				this.m_playerSprite.ScaleX *= 1.25f;
				this.m_playerSprite.ScaleY *= 1.175f;
			}
			if (this.Class == 6 || this.Class == 14)
			{
				this.m_playerSprite.OutlineColour = Color.White;
				return;
			}
			this.m_playerSprite.OutlineColour = Color.Black;
		}
		public void DropFrame()
		{
			this.m_frameDropping = true;
			Tween.By(this.m_frameSprite, 0.7f, new Easing(Back.EaseOut), new string[]
			{
				"X",
				((float)(-(float)this.m_frameSprite.Width) / 2f - 2f).ToString(),
				"Y",
				"30",
				"Rotation",
				"45"
			});
			Tween.By(this.m_playerSprite, 0.7f, new Easing(Back.EaseOut), new string[]
			{
				"X",
				((float)(-(float)this.m_frameSprite.Width) / 2f - 2f).ToString(),
				"Y",
				"30",
				"Rotation",
				"45"
			});
			Tween.RunFunction(1.5f, this, "DropFrame2", new object[0]);
		}
		public void DropFrame2()
		{
			SoundManager.PlaySound("Cutsc_Picture_Fall");
			Tween.By(this.m_frameSprite, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Y",
				"1000"
			});
			Tween.By(this.m_playerSprite, 0.5f, new Easing(Quad.EaseIn), new string[]
			{
				"Y",
				"1000"
			});
		}
		public override void Draw(Camera2D camera)
		{
			if (this.FlipPortrait)
			{
				this.m_playerSprite.Rotation = 180f;
			}
			if (!this.m_frameDropping)
			{
				this.m_frameSprite.Position = base.Position;
				this.m_frameSprite.Y -= 12f;
				this.m_frameSprite.X += 5f;
			}
			this.m_frameSprite.Opacity = base.Opacity;
			this.m_frameSprite.Draw(camera);
			if (!this.IsDead && this.Spell != 0)
			{
				this.m_spellIconHolder.Position = new Vector2(this.m_frameSprite.X, (float)(this.m_frameSprite.Bounds.Bottom - 20));
				this.m_spellIcon.Position = this.m_spellIconHolder.Position;
				this.m_spellIconHolder.Draw(camera);
				this.m_spellIcon.Draw(camera);
			}
			this.m_playerSprite.OutlineColour = this.OutlineColour;
			this.m_playerSprite.OutlineWidth = base.OutlineWidth;
			if (!this.m_frameDropping)
			{
				this.m_playerSprite.Position = base.Position;
				this.m_playerSprite.X += 10f;
				if (this.FlipPortrait)
				{
					this.m_playerSprite.X -= 10f;
					this.m_playerSprite.Y -= 30f;
				}
			}
			this.m_playerSprite.Opacity = base.Opacity;
			this.m_playerSprite.Draw(camera);
			if (CollisionMath.Intersects(this.Bounds, camera.Bounds))
			{
				if (this.Class == 7 || this.Class == 15)
				{
					Game.ColourSwapShader.Parameters["desiredTint"].SetValue(Color.White.ToVector4());
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_lichColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_lichColour2.ToVector4());
					camera.End();
					camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader, camera.GetTransformation());
					this.m_playerSprite.GetChildAt(12).Draw(camera);
					camera.End();
					camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
					if (this.IsFemale)
					{
						this.m_playerSprite.GetChildAt(13).Draw(camera);
					}
					this.m_playerSprite.GetChildAt(15).Draw(camera);
				}
				else if (this.Class == 3 || this.Class == 11)
				{
					Game.ColourSwapShader.Parameters["desiredTint"].SetValue(Color.White.ToVector4());
					Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
					Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
					Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
					camera.End();
					camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader, camera.GetTransformation());
					this.m_playerSprite.GetChildAt(12).Draw(camera);
					camera.End();
					camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
					if (this.IsFemale)
					{
						this.m_playerSprite.GetChildAt(13).Draw(camera);
					}
					this.m_playerSprite.GetChildAt(15).Draw(camera);
				}
			}
			if (!this.DisablePlaque)
			{
				if (!this.m_frameDropping)
				{
					this.m_plaqueSprite.Position = base.Position;
					this.m_plaqueSprite.X += 5f;
					this.m_plaqueSprite.Y = this.m_frameSprite.Y + (float)this.m_frameSprite.Height - 30f;
				}
				this.m_plaqueSprite.Draw(camera);
				camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
				base.Draw(camera);
				camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			}
			if (this.m_frameDropping)
			{
				this.m_frameSprite.Draw(camera);
				this.m_playerSprite.Draw(camera);
			}
		}
		public void SetTraits(Vector2 traits)
		{
			this.Traits = traits;
			string text = "";
			if (this.Traits.X != 0f)
			{
				text += TraitType.ToString((byte)this.Traits.X);
			}
			else
			{
				this.m_trait1Title.Text = "";
			}
			if (this.Traits.Y != 0f)
			{
				text = text + ", " + TraitType.ToString((byte)this.Traits.Y);
			}
			this.m_trait1Title.Text = text;
		}
		public void ClearTraits()
		{
			this.Traits = Vector2.Zero;
			this.m_trait1Title.Text = "No Traits";
			this.m_trait2Title.Text = "";
		}
		public void OutlineLineageObj(Color color, int width)
		{
			this.m_plaqueSprite.OutlineColour = color;
			this.m_plaqueSprite.OutlineWidth = width;
			this.m_frameSprite.OutlineColour = color;
			this.m_frameSprite.OutlineWidth = width;
		}
		public void UpdateClassRank()
		{
			string text = "the ";
			if (this.BeatenABoss)
			{
				text += "Legendary ";
			}
			else if (this.NumEnemiesKilled < 5)
			{
				text += "Useless ";
			}
			else if (this.NumEnemiesKilled >= 5 && this.NumEnemiesKilled < 10)
			{
				text += "Feeble ";
			}
			else if (this.NumEnemiesKilled >= 10 && this.NumEnemiesKilled < 15)
			{
				text += "Determined ";
			}
			else if (this.NumEnemiesKilled >= 15 && this.NumEnemiesKilled < 20)
			{
				text += "Stout ";
			}
			else if (this.NumEnemiesKilled >= 20 && this.NumEnemiesKilled < 25)
			{
				text += "Gallant ";
			}
			else if (this.NumEnemiesKilled >= 25 && this.NumEnemiesKilled < 30)
			{
				text += "Valiant ";
			}
			else if (this.NumEnemiesKilled >= 30 && this.NumEnemiesKilled < 35)
			{
				text += "Heroic ";
			}
			else
			{
				text += "Divine ";
			}
			text += ClassType.ToString(this.Class, this.IsFemale);
			this.m_classTextObj.Text = text;
		}
		protected override GameObj CreateCloneInstance()
		{
			return new LineageObj(null, false);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_playerSprite.Dispose();
				this.m_playerSprite = null;
				this.m_trait1Title = null;
				this.m_trait2Title = null;
				this.m_ageText = null;
				this.m_playerName = null;
				this.m_classTextObj = null;
				this.m_frameSprite.Dispose();
				this.m_frameSprite = null;
				this.m_plaqueSprite.Dispose();
				this.m_plaqueSprite = null;
				this.m_spellIcon.Dispose();
				this.m_spellIcon = null;
				this.m_spellIconHolder.Dispose();
				this.m_spellIconHolder = null;
				base.Dispose();
			}
		}
	}
}
