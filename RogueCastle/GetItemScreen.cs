using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class GetItemScreen : Screen
	{
		private SpriteObj m_levelUpBGImage;
		private SpriteObj[] m_levelUpParticles;
		private byte m_itemType;
		private Vector2 m_itemInfo;
		private SpriteObj m_itemSprite;
		private SpriteObj m_itemFoundSprite;
		private TextObj m_itemFoundText;
		private Vector2 m_itemStartPos;
		private Vector2 m_itemEndPos;
		private bool m_itemSpinning;
		private KeyIconTextObj m_continueText;
		private bool m_lockControls;
		private Cue m_buildUpSound;
		private string m_songName;
		private float m_storedMusicVolume;
		private SpriteObj m_tripStat1;
		private SpriteObj m_tripStat2;
		private TextObj m_tripStat1FoundText;
		private TextObj m_tripStat2FoundText;
		private Vector2 m_tripStatData;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public GetItemScreen()
		{
			base.DrawIfCovered = true;
			this.BackBufferOpacity = 0f;
			this.m_itemEndPos = new Vector2(660f, 410f);
		}
		public override void LoadContent()
		{
			this.m_levelUpBGImage = new SpriteObj("BlueprintFoundBG_Sprite");
			this.m_levelUpBGImage.ForceDraw = true;
			this.m_levelUpBGImage.Visible = false;
			this.m_levelUpParticles = new SpriteObj[10];
			for (int i = 0; i < this.m_levelUpParticles.Length; i++)
			{
				this.m_levelUpParticles[i] = new SpriteObj("LevelUpParticleFX_Sprite");
				this.m_levelUpParticles[i].AnimationDelay = 0.0416666679f;
				this.m_levelUpParticles[i].ForceDraw = true;
				this.m_levelUpParticles[i].Visible = false;
			}
			this.m_itemSprite = new SpriteObj("BlueprintIcon_Sprite");
			this.m_itemSprite.ForceDraw = true;
			this.m_itemSprite.OutlineWidth = 2;
			this.m_tripStat1 = (this.m_itemSprite.Clone() as SpriteObj);
			this.m_tripStat2 = (this.m_itemSprite.Clone() as SpriteObj);
			this.m_itemFoundText = new TextObj(Game.JunicodeFont);
			this.m_itemFoundText.FontSize = 18f;
			this.m_itemFoundText.Align = Types.TextAlign.Centre;
			this.m_itemFoundText.Text = "";
			this.m_itemFoundText.Position = this.m_itemEndPos;
			this.m_itemFoundText.Y += 70f;
			this.m_itemFoundText.ForceDraw = true;
			this.m_itemFoundText.OutlineWidth = 2;
			this.m_tripStat1FoundText = (this.m_itemFoundText.Clone() as TextObj);
			this.m_tripStat2FoundText = (this.m_itemFoundText.Clone() as TextObj);
			this.m_itemFoundSprite = new SpriteObj("BlueprintFoundText_Sprite");
			this.m_itemFoundSprite.ForceDraw = true;
			this.m_itemFoundSprite.Visible = false;
			this.m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_continueText.FontSize = 14f;
			this.m_continueText.Text = "to continue";
			this.m_continueText.Align = Types.TextAlign.Centre;
			this.m_continueText.Position = new Vector2((float)(1320 - this.m_continueText.Width), (float)(720 - this.m_continueText.Height - 10));
			this.m_continueText.ForceDraw = true;
			base.LoadContent();
		}
		public override void PassInData(List<object> objList)
		{
			this.m_itemStartPos = (Vector2)objList[0];
			this.m_itemType = (byte)objList[1];
			this.m_itemInfo = (Vector2)objList[2];
			if (this.m_itemType == 6)
			{
				this.m_tripStatData = (Vector2)objList[3];
			}
			base.PassInData(objList);
		}
		public override void OnEnter()
		{
			this.m_tripStat1.Visible = false;
			this.m_tripStat2.Visible = false;
			this.m_tripStat1.Scale = Vector2.One;
			this.m_tripStat2.Scale = Vector2.One;
			if (this.m_itemType != 7)
			{
				(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.PlayerData,
					SaveType.UpgradeData
				});
			}
			this.m_itemSprite.Rotation = 0f;
			this.m_itemSprite.Scale = Vector2.One;
			this.m_itemStartPos.X = this.m_itemStartPos.X - base.Camera.TopLeftCorner.X;
			this.m_itemStartPos.Y = this.m_itemStartPos.Y - base.Camera.TopLeftCorner.Y;
			this.m_storedMusicVolume = SoundManager.GlobalMusicVolume;
			this.m_songName = SoundManager.GetCurrentMusicName();
			this.m_lockControls = true;
			this.m_continueText.Opacity = 0f;
			this.m_continueText.Text = "[Input:" + 0 + "]  to continue";
			this.m_itemFoundText.Position = this.m_itemEndPos;
			this.m_itemFoundText.Y += 70f;
			this.m_itemFoundText.Scale = Vector2.Zero;
			this.m_tripStat1FoundText.Position = this.m_itemFoundText.Position;
			this.m_tripStat2FoundText.Position = this.m_itemFoundText.Position;
			this.m_tripStat1FoundText.Scale = Vector2.Zero;
			this.m_tripStat2FoundText.Scale = Vector2.Zero;
			this.m_tripStat1FoundText.Visible = false;
			this.m_tripStat2FoundText.Visible = false;
			switch (this.m_itemType)
			{
			case 1:
				this.m_itemSpinning = true;
				this.m_itemSprite.ChangeSprite("BlueprintIcon_Sprite");
				this.m_itemFoundSprite.ChangeSprite("BlueprintFoundText_Sprite");
				this.m_itemFoundText.Text = EquipmentBaseType.ToString((int)this.m_itemInfo.Y) + " " + EquipmentCategoryType.ToString2((int)this.m_itemInfo.X);
				break;
			case 2:
				this.m_itemSpinning = true;
				this.m_itemSprite.ChangeSprite("RuneIcon_Sprite");
				this.m_itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
				this.m_itemFoundText.Text = EquipmentAbilityType.ToString((int)this.m_itemInfo.Y) + " Rune (" + EquipmentCategoryType.ToString2((int)this.m_itemInfo.X) + ")";
				this.m_itemSprite.AnimationDelay = 0.05f;
				GameUtil.UnlockAchievement("LOVE_OF_MAGIC");
				break;
			case 3:
			case 6:
				this.m_itemSprite.ChangeSprite(this.GetStatSpriteName((int)this.m_itemInfo.X));
				this.m_itemFoundText.Text = this.GetStatText((int)this.m_itemInfo.X);
				this.m_itemSprite.AnimationDelay = 0.05f;
				this.m_itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
				if (this.m_itemType == 6)
				{
					this.m_tripStat1FoundText.Visible = true;
					this.m_tripStat2FoundText.Visible = true;
					this.m_tripStat1.ChangeSprite(this.GetStatSpriteName((int)this.m_tripStatData.X));
					this.m_tripStat2.ChangeSprite(this.GetStatSpriteName((int)this.m_tripStatData.Y));
					this.m_tripStat1.Visible = true;
					this.m_tripStat2.Visible = true;
					this.m_tripStat1.AnimationDelay = 0.05f;
					this.m_tripStat2.AnimationDelay = 0.05f;
					Tween.RunFunction(0.1f, this.m_tripStat1, "PlayAnimation", new object[]
					{
						true
					});
					Tween.RunFunction(0.2f, this.m_tripStat2, "PlayAnimation", new object[]
					{
						true
					});
					this.m_tripStat1FoundText.Text = this.GetStatText((int)this.m_tripStatData.X);
					this.m_tripStat2FoundText.Text = this.GetStatText((int)this.m_tripStatData.Y);
					this.m_itemFoundText.Y += 50f;
					this.m_tripStat1FoundText.Y = this.m_itemFoundText.Y + 50f;
				}
				break;
			case 4:
				this.m_itemSprite.ChangeSprite(SpellType.Icon((byte)this.m_itemInfo.X));
				this.m_itemFoundSprite.ChangeSprite("SpellFoundText_Sprite");
				this.m_itemFoundText.Text = SpellType.ToString((byte)this.m_itemInfo.X);
				break;
			case 5:
				this.m_itemSprite.ChangeSprite(SpecialItemType.SpriteName((byte)this.m_itemInfo.X));
				this.m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
				this.m_itemFoundText.Text = SpecialItemType.ToString((byte)this.m_itemInfo.X);
				break;
			case 7:
				this.m_itemSprite.ChangeSprite(this.GetMedallionImage((int)this.m_itemInfo.X));
				this.m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
				if (this.m_itemInfo.X == 19f)
				{
					this.m_itemFoundText.Text = "Medallion completed!";
				}
				else
				{
					this.m_itemFoundText.Text = "You've collected a medallion piece!";
				}
				break;
			}
			this.m_itemSprite.PlayAnimation(true);
			this.ItemSpinAnimation();
			base.OnEnter();
		}
		private void ItemSpinAnimation()
		{
			this.m_itemSprite.Scale = Vector2.One;
			this.m_itemSprite.Position = this.m_itemStartPos;
			this.m_buildUpSound = SoundManager.PlaySound("GetItemBuildupStinger");
			Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new string[]
			{
				"GlobalMusicVolume",
				(this.m_storedMusicVolume * 0.1f).ToString()
			});
			this.m_itemSprite.Scale = new Vector2(35f / (float)this.m_itemSprite.Height, 35f / (float)this.m_itemSprite.Height);
			Tween.By(this.m_itemSprite, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"-150"
			});
			Tween.RunFunction(0.7f, this, "ItemSpinAnimation2", new object[0]);
			this.m_tripStat1.Scale = Vector2.One;
			this.m_tripStat2.Scale = Vector2.One;
			this.m_tripStat1.Position = this.m_itemStartPos;
			this.m_tripStat2.Position = this.m_itemStartPos;
			Tween.By(this.m_tripStat1, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"-150",
				"X",
				"50"
			});
			this.m_tripStat1.Scale = new Vector2(35f / (float)this.m_tripStat1.Height, 35f / (float)this.m_tripStat1.Height);
			Tween.By(this.m_tripStat2, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"-150",
				"X",
				"-50"
			});
			this.m_tripStat2.Scale = new Vector2(35f / (float)this.m_tripStat2.Height, 35f / (float)this.m_tripStat2.Height);
		}
		public void ItemSpinAnimation2()
		{
			Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"GetItemStinger3"
			});
			if (this.m_buildUpSound != null && this.m_buildUpSound.IsPlaying)
			{
				this.m_buildUpSound.Stop(AudioStopOptions.AsAuthored);
			}
			Tween.To(this.m_itemSprite, 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
			Tween.AddEndHandlerToLastTween(this, "ItemSpinAnimation3", new object[0]);
			Tween.To(this.m_tripStat1, 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
			Tween.To(this.m_tripStat2, 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
		}
		public void ItemSpinAnimation3()
		{
			Vector2 scale = this.m_itemSprite.Scale;
			this.m_itemSprite.Scale = Vector2.One;
			float num = 130f / (float)this.m_itemSprite.Height;
			this.m_itemSprite.Scale = scale;
			Tween.To(this.m_itemSprite, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				num.ToString(),
				"ScaleY",
				num.ToString()
			});
			Tween.To(this.m_itemSprite, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				660.ToString(),
				"Y",
				390.ToString()
			});
			Tween.To(this.m_itemFoundText, 0.3f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_continueText, 0.3f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			scale = this.m_tripStat1.Scale;
			this.m_tripStat1.Scale = Vector2.One;
			num = 130f / (float)this.m_tripStat1.Height;
			this.m_tripStat1.Scale = scale;
			Tween.To(this.m_tripStat1, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				num.ToString(),
				"ScaleY",
				num.ToString()
			});
			Tween.To(this.m_tripStat1, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				830.ToString(),
				"Y",
				390.ToString()
			});
			scale = this.m_tripStat2.Scale;
			this.m_tripStat2.Scale = Vector2.One;
			num = 130f / (float)this.m_tripStat2.Height;
			this.m_tripStat2.Scale = scale;
			Tween.To(this.m_tripStat2, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				num.ToString(),
				"ScaleY",
				num.ToString()
			});
			Tween.To(this.m_tripStat2, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				490.ToString(),
				"Y",
				390.ToString()
			});
			Tween.To(this.m_tripStat1FoundText, 0.3f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_tripStat2FoundText, 0.3f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			for (int i = 0; i < this.m_levelUpParticles.Length; i++)
			{
				this.m_levelUpParticles[i].AnimationDelay = 0f;
				this.m_levelUpParticles[i].Visible = true;
				this.m_levelUpParticles[i].Scale = new Vector2(0.1f, 0.1f);
				this.m_levelUpParticles[i].Opacity = 0f;
				this.m_levelUpParticles[i].Position = new Vector2(660f, 360f);
				this.m_levelUpParticles[i].Position += new Vector2((float)CDGMath.RandomInt(-100, 100), (float)CDGMath.RandomInt(-50, 50));
				float duration = CDGMath.RandomFloat(0f, 0.5f);
				Tween.To(this.m_levelUpParticles[i], 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					duration.ToString(),
					"Opacity",
					"1"
				});
				Tween.To(this.m_levelUpParticles[i], 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					duration.ToString(),
					"ScaleX",
					"2",
					"ScaleY",
					"2"
				});
				Tween.To(this.m_levelUpParticles[i], duration, new Easing(Linear.EaseNone), new string[0]);
				Tween.AddEndHandlerToLastTween(this.m_levelUpParticles[i], "PlayAnimation", new object[]
				{
					false
				});
			}
			this.m_itemFoundSprite.Position = new Vector2(660f, 190f);
			this.m_itemFoundSprite.Scale = Vector2.Zero;
			this.m_itemFoundSprite.Visible = true;
			Tween.To(this.m_itemFoundSprite, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.05",
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			this.m_levelUpBGImage.Position = this.m_itemFoundSprite.Position;
			this.m_levelUpBGImage.Y += 30f;
			this.m_levelUpBGImage.Scale = Vector2.Zero;
			this.m_levelUpBGImage.Visible = true;
			Tween.To(this.m_levelUpBGImage, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this, 0.5f, new Easing(Linear.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.5"
			});
			if (this.m_itemSpinning)
			{
				this.m_itemSprite.Rotation = -25f;
			}
			this.m_itemSpinning = false;
			Tween.RunFunction(0.5f, this, "UnlockControls", new object[0]);
		}
		public void UnlockControls()
		{
			this.m_lockControls = false;
		}
		public void ExitScreenTransition()
		{
			if ((int)this.m_itemInfo.X == 19)
			{
				this.m_itemInfo = Vector2.Zero;
				List<object> list = new List<object>();
				list.Add(17);
				Game.ScreenManager.DisplayScreen(19, true, list);
				return;
			}
			this.m_lockControls = true;
			Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new string[]
			{
				"GlobalMusicVolume",
				this.m_storedMusicVolume.ToString()
			});
			Tween.To(this.m_itemSprite, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_itemFoundText, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"BackBufferOpacity",
				"0"
			});
			Tween.To(this.m_levelUpBGImage, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_itemFoundSprite, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_continueText, 0.4f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.1",
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager as RCScreenManager, "HideCurrentScreen", new object[0]);
			Tween.To(this.m_tripStat1, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_tripStat2, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_tripStat1FoundText, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_tripStat2FoundText, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_itemSpinning)
			{
				this.m_itemSprite.Rotation += 1200f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Update(gameTime);
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)))
			{
				this.ExitScreenTransition();
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearClamp, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_levelUpBGImage.Draw(base.Camera);
			SpriteObj[] levelUpParticles = this.m_levelUpParticles;
			for (int i = 0; i < levelUpParticles.Length; i++)
			{
				SpriteObj spriteObj = levelUpParticles[i];
				spriteObj.Draw(base.Camera);
			}
			this.m_itemFoundSprite.Draw(base.Camera);
			this.m_itemFoundText.Draw(base.Camera);
			this.m_tripStat1FoundText.Draw(base.Camera);
			this.m_tripStat2FoundText.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_itemSprite.Draw(base.Camera);
			this.m_tripStat1.Draw(base.Camera);
			this.m_tripStat2.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_continueText.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		private string GetStatSpriteName(int type)
		{
			switch (type)
			{
			case 4:
				return "Sword_Sprite";
			case 5:
				return "MagicBook_Sprite";
			case 6:
				return "Shield_Sprite";
			case 7:
				return "Heart_Sprite";
			case 8:
				return "ManaCrystal_Sprite";
			case 9:
				return "Backpack_Sprite";
			default:
				return "";
			}
		}
		private string GetStatText(int type)
		{
			switch (type)
			{
			case 4:
				return "Strength Increased: +" + 1;
			case 5:
				return "Magic Damage Increased: +" + 1;
			case 6:
				return "Armor Increased: +" + 2;
			case 7:
				return "HP Increased: +" + 5;
			case 8:
				return "MP Increased: +" + 5;
			case 9:
				return "Max Weight Load Increased: +" + 5;
			default:
				return "";
			}
		}
		private string GetMedallionImage(int medallionType)
		{
			switch (medallionType)
			{
			case 15:
				return "MedallionPiece1_Sprite";
			case 16:
				return "MedallionPiece2_Sprite";
			case 17:
				return "MedallionPiece3_Sprite";
			case 18:
				return "MedallionPiece4_Sprite";
			case 19:
				return "MedallionPiece5_Sprite";
			default:
				return "";
			}
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Get Item Screen");
				this.m_continueText.Dispose();
				this.m_continueText = null;
				this.m_levelUpBGImage.Dispose();
				this.m_levelUpBGImage = null;
				SpriteObj[] levelUpParticles = this.m_levelUpParticles;
				for (int i = 0; i < levelUpParticles.Length; i++)
				{
					SpriteObj spriteObj = levelUpParticles[i];
					spriteObj.Dispose();
				}
				Array.Clear(this.m_levelUpParticles, 0, this.m_levelUpParticles.Length);
				this.m_levelUpParticles = null;
				this.m_buildUpSound = null;
				this.m_itemSprite.Dispose();
				this.m_itemSprite = null;
				this.m_itemFoundSprite.Dispose();
				this.m_itemFoundSprite = null;
				this.m_itemFoundText.Dispose();
				this.m_itemFoundText = null;
				this.m_tripStat1.Dispose();
				this.m_tripStat2.Dispose();
				this.m_tripStat1 = null;
				this.m_tripStat2 = null;
				this.m_tripStat1FoundText.Dispose();
				this.m_tripStat2FoundText.Dispose();
				this.m_tripStat1FoundText = null;
				this.m_tripStat2FoundText = null;
				base.Dispose();
			}
		}
	}
}
