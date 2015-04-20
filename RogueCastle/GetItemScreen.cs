/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
			DrawIfCovered = true;
			BackBufferOpacity = 0f;
			m_itemEndPos = new Vector2(660f, 410f);
		}
		public override void LoadContent()
		{
			m_levelUpBGImage = new SpriteObj("BlueprintFoundBG_Sprite");
			m_levelUpBGImage.ForceDraw = true;
			m_levelUpBGImage.Visible = false;
			m_levelUpParticles = new SpriteObj[10];
			for (int i = 0; i < m_levelUpParticles.Length; i++)
			{
				m_levelUpParticles[i] = new SpriteObj("LevelUpParticleFX_Sprite");
				m_levelUpParticles[i].AnimationDelay = 0.0416666679f;
				m_levelUpParticles[i].ForceDraw = true;
				m_levelUpParticles[i].Visible = false;
			}
			m_itemSprite = new SpriteObj("BlueprintIcon_Sprite");
			m_itemSprite.ForceDraw = true;
			m_itemSprite.OutlineWidth = 2;
			m_tripStat1 = (m_itemSprite.Clone() as SpriteObj);
			m_tripStat2 = (m_itemSprite.Clone() as SpriteObj);
			m_itemFoundText = new TextObj(Game.JunicodeFont);
			m_itemFoundText.FontSize = 18f;
			m_itemFoundText.Align = Types.TextAlign.Centre;
			m_itemFoundText.Text = "";
			m_itemFoundText.Position = m_itemEndPos;
			m_itemFoundText.Y += 70f;
			m_itemFoundText.ForceDraw = true;
			m_itemFoundText.OutlineWidth = 2;
			m_tripStat1FoundText = (m_itemFoundText.Clone() as TextObj);
			m_tripStat2FoundText = (m_itemFoundText.Clone() as TextObj);
			m_itemFoundSprite = new SpriteObj("BlueprintFoundText_Sprite");
			m_itemFoundSprite.ForceDraw = true;
			m_itemFoundSprite.Visible = false;
			m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			m_continueText.FontSize = 14f;
			m_continueText.Text = "to continue";
			m_continueText.Align = Types.TextAlign.Centre;
			m_continueText.Position = new Vector2(1320 - m_continueText.Width, 720 - m_continueText.Height - 10);
			m_continueText.ForceDraw = true;
			base.LoadContent();
		}
		public override void PassInData(List<object> objList)
		{
			m_itemStartPos = (Vector2)objList[0];
			m_itemType = (byte)objList[1];
			m_itemInfo = (Vector2)objList[2];
			if (m_itemType == 6)
			{
				m_tripStatData = (Vector2)objList[3];
			}
			base.PassInData(objList);
		}
		public override void OnEnter()
		{
			m_tripStat1.Visible = false;
			m_tripStat2.Visible = false;
			m_tripStat1.Scale = Vector2.One;
			m_tripStat2.Scale = Vector2.One;
			if (m_itemType != 7)
			{
				(ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.PlayerData,
					SaveType.UpgradeData
				});
			}
			m_itemSprite.Rotation = 0f;
			m_itemSprite.Scale = Vector2.One;
			m_itemStartPos.X = m_itemStartPos.X - Camera.TopLeftCorner.X;
			m_itemStartPos.Y = m_itemStartPos.Y - Camera.TopLeftCorner.Y;
			m_storedMusicVolume = SoundManager.GlobalMusicVolume;
			m_songName = SoundManager.GetCurrentMusicName();
			m_lockControls = true;
			m_continueText.Opacity = 0f;
			m_continueText.Text = "[Input:" + 0 + "]  to continue";
			m_itemFoundText.Position = m_itemEndPos;
			m_itemFoundText.Y += 70f;
			m_itemFoundText.Scale = Vector2.Zero;
			m_tripStat1FoundText.Position = m_itemFoundText.Position;
			m_tripStat2FoundText.Position = m_itemFoundText.Position;
			m_tripStat1FoundText.Scale = Vector2.Zero;
			m_tripStat2FoundText.Scale = Vector2.Zero;
			m_tripStat1FoundText.Visible = false;
			m_tripStat2FoundText.Visible = false;
			switch (m_itemType)
			{
			case 1:
				m_itemSpinning = true;
				m_itemSprite.ChangeSprite("BlueprintIcon_Sprite");
				m_itemFoundSprite.ChangeSprite("BlueprintFoundText_Sprite");
				m_itemFoundText.Text = EquipmentBaseType.ToString((int)m_itemInfo.Y) + " " + EquipmentCategoryType.ToString2((int)m_itemInfo.X);
				break;
			case 2:
				m_itemSpinning = true;
				m_itemSprite.ChangeSprite("RuneIcon_Sprite");
				m_itemFoundSprite.ChangeSprite("RuneFoundText_Sprite");
				m_itemFoundText.Text = EquipmentAbilityType.ToString((int)m_itemInfo.Y) + " Rune (" + EquipmentCategoryType.ToString2((int)m_itemInfo.X) + ")";
				m_itemSprite.AnimationDelay = 0.05f;
				GameUtil.UnlockAchievement("LOVE_OF_MAGIC");
				break;
			case 3:
			case 6:
				m_itemSprite.ChangeSprite(GetStatSpriteName((int)m_itemInfo.X));
				m_itemFoundText.Text = GetStatText((int)m_itemInfo.X);
				m_itemSprite.AnimationDelay = 0.05f;
				m_itemFoundSprite.ChangeSprite("StatFoundText_Sprite");
				if (m_itemType == 6)
				{
					m_tripStat1FoundText.Visible = true;
					m_tripStat2FoundText.Visible = true;
					m_tripStat1.ChangeSprite(GetStatSpriteName((int)m_tripStatData.X));
					m_tripStat2.ChangeSprite(GetStatSpriteName((int)m_tripStatData.Y));
					m_tripStat1.Visible = true;
					m_tripStat2.Visible = true;
					m_tripStat1.AnimationDelay = 0.05f;
					m_tripStat2.AnimationDelay = 0.05f;
					Tween.RunFunction(0.1f, m_tripStat1, "PlayAnimation", new object[]
					{
						true
					});
					Tween.RunFunction(0.2f, m_tripStat2, "PlayAnimation", new object[]
					{
						true
					});
					m_tripStat1FoundText.Text = GetStatText((int)m_tripStatData.X);
					m_tripStat2FoundText.Text = GetStatText((int)m_tripStatData.Y);
					m_itemFoundText.Y += 50f;
					m_tripStat1FoundText.Y = m_itemFoundText.Y + 50f;
				}
				break;
			case 4:
				m_itemSprite.ChangeSprite(SpellType.Icon((byte)m_itemInfo.X));
				m_itemFoundSprite.ChangeSprite("SpellFoundText_Sprite");
				m_itemFoundText.Text = SpellType.ToString((byte)m_itemInfo.X);
				break;
			case 5:
				m_itemSprite.ChangeSprite(SpecialItemType.SpriteName((byte)m_itemInfo.X));
				m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
				m_itemFoundText.Text = SpecialItemType.ToString((byte)m_itemInfo.X);
				break;
			case 7:
				m_itemSprite.ChangeSprite(GetMedallionImage((int)m_itemInfo.X));
				m_itemFoundSprite.ChangeSprite("ItemFoundText_Sprite");
				if (m_itemInfo.X == 19f)
				{
					m_itemFoundText.Text = "Medallion completed!";
				}
				else
				{
					m_itemFoundText.Text = "You've collected a medallion piece!";
				}
				break;
			}
			m_itemSprite.PlayAnimation(true);
			ItemSpinAnimation();
			base.OnEnter();
		}
		private void ItemSpinAnimation()
		{
			m_itemSprite.Scale = Vector2.One;
			m_itemSprite.Position = m_itemStartPos;
			m_buildUpSound = SoundManager.PlaySound("GetItemBuildupStinger");
			Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new string[]
			{
				"GlobalMusicVolume",
				(m_storedMusicVolume * 0.1f).ToString()
			});
			m_itemSprite.Scale = new Vector2(35f / m_itemSprite.Height, 35f / m_itemSprite.Height);
			Tween.By(m_itemSprite, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"-150"
			});
			Tween.RunFunction(0.7f, this, "ItemSpinAnimation2", new object[0]);
			m_tripStat1.Scale = Vector2.One;
			m_tripStat2.Scale = Vector2.One;
			m_tripStat1.Position = m_itemStartPos;
			m_tripStat2.Position = m_itemStartPos;
			Tween.By(m_tripStat1, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"-150",
				"X",
				"50"
			});
			m_tripStat1.Scale = new Vector2(35f / m_tripStat1.Height, 35f / m_tripStat1.Height);
			Tween.By(m_tripStat2, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"Y",
				"-150",
				"X",
				"-50"
			});
			m_tripStat2.Scale = new Vector2(35f / m_tripStat2.Height, 35f / m_tripStat2.Height);
		}
		public void ItemSpinAnimation2()
		{
			Tween.RunFunction(0.2f, typeof(SoundManager), "PlaySound", new object[]
			{
				"GetItemStinger3"
			});
			if (m_buildUpSound != null && m_buildUpSound.IsPlaying)
			{
				m_buildUpSound.Stop(AudioStopOptions.AsAuthored);
			}
			Tween.To(m_itemSprite, 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
			Tween.AddEndHandlerToLastTween(this, "ItemSpinAnimation3", new object[0]);
			Tween.To(m_tripStat1, 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
			Tween.To(m_tripStat2, 0.2f, new Easing(Quad.EaseOut), new string[]
			{
				"ScaleX",
				"0.1",
				"ScaleY",
				"0.1"
			});
		}
		public void ItemSpinAnimation3()
		{
			Vector2 scale = m_itemSprite.Scale;
			m_itemSprite.Scale = Vector2.One;
			float num = 130f / m_itemSprite.Height;
			m_itemSprite.Scale = scale;
			Tween.To(m_itemSprite, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				num.ToString(),
				"ScaleY",
				num.ToString()
			});
			Tween.To(m_itemSprite, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				660.ToString(),
				"Y",
				390.ToString()
			});
			Tween.To(m_itemFoundText, 0.3f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(m_continueText, 0.3f, new Easing(Linear.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			scale = m_tripStat1.Scale;
			m_tripStat1.Scale = Vector2.One;
			num = 130f / m_tripStat1.Height;
			m_tripStat1.Scale = scale;
			Tween.To(m_tripStat1, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				num.ToString(),
				"ScaleY",
				num.ToString()
			});
			Tween.To(m_tripStat1, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				830.ToString(),
				"Y",
				390.ToString()
			});
			scale = m_tripStat2.Scale;
			m_tripStat2.Scale = Vector2.One;
			num = 130f / m_tripStat2.Height;
			m_tripStat2.Scale = scale;
			Tween.To(m_tripStat2, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				num.ToString(),
				"ScaleY",
				num.ToString()
			});
			Tween.To(m_tripStat2, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				490.ToString(),
				"Y",
				390.ToString()
			});
			Tween.To(m_tripStat1FoundText, 0.3f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(m_tripStat2FoundText, 0.3f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			for (int i = 0; i < m_levelUpParticles.Length; i++)
			{
				m_levelUpParticles[i].AnimationDelay = 0f;
				m_levelUpParticles[i].Visible = true;
				m_levelUpParticles[i].Scale = new Vector2(0.1f, 0.1f);
				m_levelUpParticles[i].Opacity = 0f;
				m_levelUpParticles[i].Position = new Vector2(660f, 360f);
				m_levelUpParticles[i].Position += new Vector2(CDGMath.RandomInt(-100, 100), CDGMath.RandomInt(-50, 50));
				float duration = CDGMath.RandomFloat(0f, 0.5f);
				Tween.To(m_levelUpParticles[i], 0.2f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					duration.ToString(),
					"Opacity",
					"1"
				});
				Tween.To(m_levelUpParticles[i], 0.5f, new Easing(Linear.EaseNone), new string[]
				{
					"delay",
					duration.ToString(),
					"ScaleX",
					"2",
					"ScaleY",
					"2"
				});
				Tween.To(m_levelUpParticles[i], duration, new Easing(Linear.EaseNone), new string[0]);
				Tween.AddEndHandlerToLastTween(m_levelUpParticles[i], "PlayAnimation", new object[]
				{
					false
				});
			}
			m_itemFoundSprite.Position = new Vector2(660f, 190f);
			m_itemFoundSprite.Scale = Vector2.Zero;
			m_itemFoundSprite.Visible = true;
			Tween.To(m_itemFoundSprite, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.05",
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			m_levelUpBGImage.Position = m_itemFoundSprite.Position;
			m_levelUpBGImage.Y += 30f;
			m_levelUpBGImage.Scale = Vector2.Zero;
			m_levelUpBGImage.Visible = true;
			Tween.To(m_levelUpBGImage, 0.5f, new Easing(Back.EaseOut), new string[]
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
			if (m_itemSpinning)
			{
				m_itemSprite.Rotation = -25f;
			}
			m_itemSpinning = false;
			Tween.RunFunction(0.5f, this, "UnlockControls", new object[0]);
		}
		public void UnlockControls()
		{
			m_lockControls = false;
		}
		public void ExitScreenTransition()
		{
			if ((int)m_itemInfo.X == 19)
			{
				m_itemInfo = Vector2.Zero;
				List<object> list = new List<object>();
				list.Add(17);
				Game.ScreenManager.DisplayScreen(19, true, list);
				return;
			}
			m_lockControls = true;
			Tween.To(typeof(SoundManager), 1f, new Easing(Tween.EaseNone), new string[]
			{
				"GlobalMusicVolume",
				m_storedMusicVolume.ToString()
			});
			Tween.To(m_itemSprite, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(m_itemFoundText, 0.4f, new Easing(Back.EaseIn), new string[]
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
			Tween.To(m_levelUpBGImage, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(m_itemFoundSprite, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(m_continueText, 0.4f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.1",
				"Opacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(ScreenManager as RCScreenManager, "HideCurrentScreen", new object[0]);
			Tween.To(m_tripStat1, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(m_tripStat2, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(m_tripStat1FoundText, 0.4f, new Easing(Back.EaseIn), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(m_tripStat2FoundText, 0.4f, new Easing(Back.EaseIn), new string[]
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
			if (m_itemSpinning)
			{
				m_itemSprite.Rotation += 1200f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			base.Update(gameTime);
		}
		public override void HandleInput()
		{
			if (!m_lockControls && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)))
			{
				ExitScreenTransition();
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gameTime)
		{
			Camera.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearClamp, null, null);
			Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
			m_levelUpBGImage.Draw(Camera);
			SpriteObj[] levelUpParticles = m_levelUpParticles;
			for (int i = 0; i < levelUpParticles.Length; i++)
			{
				SpriteObj spriteObj = levelUpParticles[i];
				spriteObj.Draw(Camera);
			}
			m_itemFoundSprite.Draw(Camera);
			m_itemFoundText.Draw(Camera);
			m_tripStat1FoundText.Draw(Camera);
			m_tripStat2FoundText.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			m_itemSprite.Draw(Camera);
			m_tripStat1.Draw(Camera);
			m_tripStat2.Draw(Camera);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_continueText.Draw(Camera);
			Camera.End();
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
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Get Item Screen");
				m_continueText.Dispose();
				m_continueText = null;
				m_levelUpBGImage.Dispose();
				m_levelUpBGImage = null;
				SpriteObj[] levelUpParticles = m_levelUpParticles;
				for (int i = 0; i < levelUpParticles.Length; i++)
				{
					SpriteObj spriteObj = levelUpParticles[i];
					spriteObj.Dispose();
				}
				Array.Clear(m_levelUpParticles, 0, m_levelUpParticles.Length);
				m_levelUpParticles = null;
				m_buildUpSound = null;
				m_itemSprite.Dispose();
				m_itemSprite = null;
				m_itemFoundSprite.Dispose();
				m_itemFoundSprite = null;
				m_itemFoundText.Dispose();
				m_itemFoundText = null;
				m_tripStat1.Dispose();
				m_tripStat2.Dispose();
				m_tripStat1 = null;
				m_tripStat2 = null;
				m_tripStat1FoundText.Dispose();
				m_tripStat2FoundText.Dispose();
				m_tripStat1FoundText = null;
				m_tripStat2FoundText = null;
				base.Dispose();
			}
		}
	}
}
