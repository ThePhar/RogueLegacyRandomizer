/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;

namespace RogueCastle
{
	public class CreditsScreen : Screen
	{
		private const float m_scrollDuration = 75f;
		private bool m_allowExit;
		public bool IsEnding;
		private List<TextObj> m_creditsNameList;
		private List<TextObj> m_creditsTitleList;
		private SpriteObj m_bg1;
		private SpriteObj m_bg2;
		private SpriteObj m_bg3;
		private SpriteObj m_bgOutside;
		private SpriteObj m_ground1;
		private SpriteObj m_ground2;
		private SpriteObj m_ground3;
		private SpriteObj m_border1;
		private SpriteObj m_border2;
		private SpriteObj m_border3;
		private SpriteObj m_prop1;
		private ObjContainer m_prop2;
		private ObjContainer m_prop3;
		private ObjContainer m_playerSprite;
		private ObjContainer m_wifeSprite;
		private ObjContainer m_childSprite1;
		private ObjContainer m_childSprite2;
		private int m_wifeChest;
		private int m_wifeShoulders;
		private int m_wifeHead;
		private int m_child1Chest;
		private int m_child1Shoulders;
		private int m_child1Head;
		private int m_child2Chest;
		private int m_child2Shoulders;
		private int m_child2Head;
		private SpriteObj m_sideBorderLeft;
		private SpriteObj m_sideBorderRight;
		private SpriteObj m_sideBorderTop;
		private SpriteObj m_sideBorderBottom;
		private ObjContainer m_manor;
		private string[] m_backgroundStrings;
		private int m_backgroundIndex;
		private float m_backgroundSwapTimer;
		private TextObj m_thanksForPlayingText;
		private TextObj m_totalPlayTime;
		private TextObj m_totalDeaths;
		private SkyObj m_sky;
		private RenderTarget2D m_skyRenderTarget;
		private RenderTarget2D m_backgroundRenderTarget;
		private KeyIconTextObj m_continueText;
		private bool m_displayingContinueText;
		private float m_scrollDistance;
		private SpriteObj m_glauber;
		private SpriteObj m_teddy;
		private SpriteObj m_kenny;
		private SpriteObj m_gordon;
		private SpriteObj m_judson;
		private Color m_skinColour1 = new Color(231, 175, 131, 255);
		private Color m_skinColour2 = new Color(199, 109, 112, 255);
		private Color m_lichColour1 = new Color(255, 255, 255, 255);
		private Color m_lichColour2 = new Color(198, 198, 198, 255);
		public override void LoadContent()
		{
			m_bgOutside = new SpriteObj("TraitsBG_Sprite");
			m_bgOutside.ForceDraw = true;
			m_bgOutside.Visible = false;
			m_bgOutside.Scale = new Vector2(1320f / m_bgOutside.Width, 1320f / m_bgOutside.Width);
			m_bg1 = new SpriteObj("CastleBG1_Sprite");
			m_bg1.Position = new Vector2(660f, 200f);
			m_bg1.Scale = new Vector2(2f, 2f);
			m_bg2 = (m_bg1.Clone() as SpriteObj);
			m_bg2.X -= m_bg1.Width;
			m_bg3 = (m_bg2.Clone() as SpriteObj);
			m_bg3.X -= m_bg2.Width;
			m_ground1 = new SpriteObj("CastleFG1_Sprite");
			m_ground1.Position = new Vector2(660f, 440f);
			m_ground1.Scale = new Vector2(2f, 2f);
			m_ground2 = (m_ground1.Clone() as SpriteObj);
			m_ground2.X -= m_ground1.Width;
			m_ground3 = (m_ground2.Clone() as SpriteObj);
			m_ground3.X -= m_ground2.Width;
			m_border1 = new SpriteObj("CastleBorder_Sprite");
			m_border1.Position = new Vector2(660f, 440f);
			m_border1.Scale = new Vector2(2f, 2f);
			m_border2 = (m_border1.Clone() as SpriteObj);
			m_border2.X -= m_border1.Width;
			m_border3 = (m_border2.Clone() as SpriteObj);
			m_border3.X -= m_border2.Width;
			m_prop1 = new SpriteObj("CastleAssetWindow1_Sprite");
			m_prop1.Position = new Vector2(0f, 220f);
			m_prop1.Scale = new Vector2(2f, 2f);
			m_prop2 = new ObjContainer("CastleAssetBackTorch_Character");
			m_prop2.Position = new Vector2(500f, 330f);
			m_prop2.Scale = new Vector2(2f, 2f);
			m_prop2.AnimationDelay = 0.1f;
			m_prop2.PlayAnimation();
			m_prop3 = new ObjContainer("CastleAssetCandle1_Character");
			m_prop3.Position = new Vector2(1000f, 440f);
			m_prop3.Scale = new Vector2(2f, 2f);
			m_playerSprite = new ObjContainer("PlayerWalking_Character");
			m_playerSprite.Position = new Vector2(640f, 400f);
			m_playerSprite.PlayAnimation();
			m_playerSprite.AnimationDelay = 0.1f;
			m_playerSprite.Flip = SpriteEffects.FlipHorizontally;
			m_playerSprite.OutlineWidth = 2;
			Color textureColor = new Color(251, 156, 172);
			m_wifeSprite = new ObjContainer("PlayerWalking_Character");
			m_wifeSprite.Position = new Vector2(-200f, 400f);
			m_wifeSprite.PlayAnimation();
			m_wifeSprite.AnimationDelay = 0.1f;
			m_wifeSprite.OutlineWidth = 2;
			m_wifeSprite.Scale = new Vector2(2f, 2f);
			m_wifeSprite.GetChildAt(13).TextureColor = textureColor;
			m_wifeSprite.GetChildAt(7).TextureColor = Color.Red;
			m_wifeSprite.GetChildAt(1).TextureColor = Color.Red;
			m_wifeSprite.GetChildAt(8).TextureColor = Color.Red;
			m_wifeSprite.GetChildAt(11).TextureColor = new Color(11, 172, 239);
			m_childSprite1 = new ObjContainer("PlayerWalking_Character");
			m_childSprite1.Position = new Vector2(-270f, 415f);
			m_childSprite1.PlayAnimation();
			m_childSprite1.AnimationDelay = 0.1f;
			m_childSprite1.OutlineWidth = 2;
			m_childSprite1.Scale = new Vector2(1.2f, 1.2f);
			m_childSprite1.GetChildAt(13).TextureColor = textureColor;
			m_childSprite1.GetChildAt(7).TextureColor = Color.Red;
			m_childSprite1.GetChildAt(1).TextureColor = Color.Red;
			m_childSprite1.GetChildAt(8).TextureColor = Color.Red;
			m_childSprite1.GetChildAt(11).TextureColor = new Color(11, 172, 239);
			m_childSprite2 = new ObjContainer("PlayerWalking_Character");
			m_childSprite2.Position = new Vector2(-330f, 420f);
			m_childSprite2.PlayAnimation();
			m_childSprite2.AnimationDelay = 0.1f;
			m_childSprite2.OutlineWidth = 2;
			m_childSprite2.Scale = new Vector2(1f, 1f);
			m_childSprite2.GetChildAt(13).TextureColor = textureColor;
			m_childSprite2.GetChildAt(7).TextureColor = Color.Red;
			m_childSprite2.GetChildAt(1).TextureColor = Color.Red;
			m_childSprite2.GetChildAt(8).TextureColor = Color.Red;
			m_childSprite2.GetChildAt(11).TextureColor = new Color(11, 172, 239);
			m_sideBorderLeft = new SpriteObj("Blank_Sprite");
			m_sideBorderLeft.Scale = new Vector2(900f / m_sideBorderLeft.Width, 500f / m_sideBorderLeft.Height);
			m_sideBorderLeft.Position = new Vector2(-450f, 0f);
			m_sideBorderLeft.TextureColor = Color.Black;
			m_sideBorderLeft.ForceDraw = true;
			m_sideBorderRight = (m_sideBorderLeft.Clone() as SpriteObj);
			m_sideBorderRight.Position = new Vector2(850f, 0f);
			m_sideBorderTop = (m_sideBorderLeft.Clone() as SpriteObj);
			m_sideBorderTop.Scale = new Vector2(1f, 1f);
			m_sideBorderTop.Scale = new Vector2(1320f / m_sideBorderTop.Width, 240 / m_sideBorderTop.Height);
			m_sideBorderTop.Position = Vector2.Zero;
			m_sideBorderBottom = (m_sideBorderLeft.Clone() as SpriteObj);
			m_sideBorderBottom.Scale = new Vector2(1f, 1f);
			m_sideBorderBottom.Scale = new Vector2(1340f / m_sideBorderBottom.Width, 720f / m_sideBorderBottom.Height);
			m_sideBorderBottom.Position = new Vector2(0f, 460f);
			m_manor = new ObjContainer("TraitsCastle_Character");
			m_manor.Scale = new Vector2(2f, 2f);
			m_manor.Visible = false;
			for (int i = 0; i < m_manor.NumChildren; i++)
			{
				m_manor.GetChildAt(i).Visible = false;
			}
			foreach (SkillObj current in SkillSystem.SkillArray)
			{
				if (current.CurrentLevel > 0)
				{
					m_manor.GetChildAt(SkillSystem.GetManorPiece(current)).Visible = true;
				}
			}
			m_thanksForPlayingText = new TextObj(Game.JunicodeLargeFont);
			m_thanksForPlayingText.FontSize = 32f;
			m_thanksForPlayingText.Align = Types.TextAlign.Centre;
			m_thanksForPlayingText.Text = "Thanks for playing!";
			m_thanksForPlayingText.DropShadow = new Vector2(2f, 2f);
			m_thanksForPlayingText.Position = new Vector2(660f, 480f);
			m_thanksForPlayingText.Opacity = 0f;
			m_totalDeaths = (m_thanksForPlayingText.Clone() as TextObj);
			m_totalDeaths.FontSize = 20f;
			m_totalDeaths.Position = m_thanksForPlayingText.Position;
			m_totalDeaths.Y += 90f;
			m_totalDeaths.Opacity = 0f;
			m_totalPlayTime = (m_thanksForPlayingText.Clone() as TextObj);
			m_totalPlayTime.FontSize = 20f;
			m_totalPlayTime.Position = m_totalDeaths.Position;
			m_totalPlayTime.Y += 50f;
			m_totalPlayTime.Opacity = 0f;
			m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			m_continueText.FontSize = 14f;
			m_continueText.Align = Types.TextAlign.Right;
			m_continueText.Position = new Vector2(1270f, 650f);
			m_continueText.ForceDraw = true;
			m_continueText.Opacity = 0f;
			int num = 200;
			m_glauber = new SpriteObj("Glauber_Sprite");
			m_glauber.Scale = new Vector2(2f, 2f);
			m_glauber.ForceDraw = true;
			m_glauber.OutlineWidth = 2;
			m_glauber.X = num;
			m_teddy = new SpriteObj("Teddy_Sprite");
			m_teddy.Scale = new Vector2(2f, 2f);
			m_teddy.ForceDraw = true;
			m_teddy.OutlineWidth = 2;
			m_teddy.X = num;
			m_kenny = new SpriteObj("Kenny_Sprite");
			m_kenny.Scale = new Vector2(2f, 2f);
			m_kenny.ForceDraw = true;
			m_kenny.OutlineWidth = 2;
			m_kenny.X = num;
			m_gordon = new SpriteObj("Gordon_Sprite");
			m_gordon.Scale = new Vector2(2f, 2f);
			m_gordon.ForceDraw = true;
			m_gordon.OutlineWidth = 2;
			m_gordon.X = num;
			m_judson = new SpriteObj("Judson_Sprite");
			m_judson.Scale = new Vector2(2f, 2f);
			m_judson.ForceDraw = true;
			m_judson.OutlineWidth = 2;
			m_judson.X = num;
			InitializeCredits();
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			m_sky.ReinitializeRT(Camera);
			m_skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
			m_backgroundRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
			base.ReinitializeRTs();
		}
		private void InitializeCredits()
		{
			m_creditsNameList = new List<TextObj>();
			m_creditsTitleList = new List<TextObj>();
			m_backgroundStrings = new[]
			{
				"Garden",
				"Tower",
				"Dungeon",
				"Outside",
				"Manor"
			};
			string[] array = new[]
			{
				"Cellar Door Games",
				"Teddy Lee",
				"Kenny Lee",
				"Glauber Kotaki",
				"Gordon McGladdery (A Shell in the Pit)",
				"Judson Cowan (Tettix)",
				"Benny Lee",
				"Ryan & Michelle Lee",
				"Alessio Mellina",
				"John Won",
				"Charles Humphrey",
				"Jenny Lee",
				"Ethan \"flibitijibibo\" Lee",
				"David Gow",
				"Forrest Loomis",
				"Jorgen Tjerno",
				"Marcus Moller",
				"Matthias Niess",
				"Stanislaw Gackowski",
				"Stefano Angeleri",
				"",
				"",
				"Special Thanks",
				"Amber Campbell (Phedran)",
				"Blair Hurm Cowan",
				"Caitlin Groves",
				"Doug Culp",
				"Eric Lee Lewis",
				"Priscila Garcia",
				"Scott Barcik",
				"Tyler Mayes",
				"Our Moms & Dads",
				"",
				"",
				"Additional Thanks",
				"Jake Hirshey",
				"Joshua Hornsby",
				"Mark Wallace",
				"Peter Lee",
				"Sean Fleming",
				"",
				"",
				"Thanks to all our fans for their support!"
			};
			string[] array2 = new[]
			{
				"Developed by",
				"Design & Story",
				"Programming & Production",
				"Art",
				"Music & Audio Design",
				"Music",
				"Marketing & Story",
				"Business Advisors",
				"Additional Audio Design",
				"Additional Background Art",
				"Crespuscular Ray Code From",
				"Super Special Thanks: Turbo Edition",
				"Mac/Linux Developer",
				"Mac/Linux QA Team"
			};
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				TextObj textObj = new TextObj(Game.JunicodeFont);
				textObj.FontSize = 12f;
				textObj.DropShadow = new Vector2(2f, 2f);
				textObj.Align = Types.TextAlign.Centre;
				textObj.Position = new Vector2(660f, 720 + num);
				if (i < array2.Length)
				{
					textObj.Text = array2[i];
					m_creditsTitleList.Add(textObj);
					if (i < array2.Length - 1)
					{
						num += 200;
					}
					else
					{
						num += 40;
					}
				}
				else
				{
					num += 40;
				}
				TextObj textObj2 = textObj.Clone() as TextObj;
				textObj2.Text = array[i];
				textObj2.FontSize = 16f;
				textObj2.Y += 40f;
				m_creditsNameList.Add(textObj2);
				PositionTeam(array[i], new Vector2(textObj2.Bounds.Left - 50, textObj2.Y));
			}
		}
		private void PositionTeam(string name, Vector2 position)
		{
			if (name.Contains("Teddy"))
			{
				m_teddy.Position = position;
				return;
			}
			if (name.Contains("Kenny"))
			{
				m_kenny.Position = position;
				m_kenny.X -= 50f;
				return;
			}
			if (name.Contains("Glauber"))
			{
				m_glauber.Position = position;
				m_glauber.X -= 20f;
				return;
			}
			if (name.Contains("Gordon"))
			{
				m_gordon.Position = position;
				m_gordon.X += 75f;
				return;
			}
			if (name.Contains("Judson"))
			{
				m_judson.Position = position;
				m_judson.X += 40f;
			}
		}
		public override void OnEnter()
		{
			m_allowExit = false;
			float num = Game.PlayerStats.TotalHoursPlayed + Game.GetTotalGameTimeHours();
			int num2 = (int)((num - (int)num) * 60f);
			Console.WriteLine(string.Concat("Hours played: ", num, " minutes: ", num2));
			m_totalDeaths.Text = "Total Children: " + Game.PlayerStats.TimesDead.ToString();
			if (num2 < 10)
			{
				m_totalPlayTime.Text = string.Concat("Time Played - ", (int)num, ":0", num2);
			}
			else
			{
				m_totalPlayTime.Text = string.Concat("Time Played - ", (int)num, ":", num2);
			}
			Camera.Position = Vector2.Zero;
			m_displayingContinueText = false;
			m_continueText.Text = "Press [Input:" + 0 + "] to exit";
			if (m_sky == null)
			{
				m_sky = new SkyObj(null);
				m_skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
				m_sky.LoadContent(Camera);
				m_backgroundRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
			}
			SetPlayerStyle("Walking");
			if (!IsEnding)
			{
				SoundManager.PlayMusic("CreditsSong", true, 1f);
			}
			m_scrollDistance = -(m_creditsNameList[m_creditsNameList.Count - 1].Y + 100f);
			foreach (TextObj current in m_creditsTitleList)
			{
				Tween.By(current, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			}
			foreach (TextObj current2 in m_creditsNameList)
			{
				Tween.By(current2, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			}
			Tween.By(m_teddy, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_kenny, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_glauber, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_gordon, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_judson, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			if (!IsEnding)
			{
				m_sideBorderLeft.X += 200f;
				m_sideBorderRight.X -= 200f;
				Tween.RunFunction(76f, this, "ResetScroll");
			}
			base.OnEnter();
		}
		public void SetPlayerStyle(string animationType)
		{
			m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
			PlayerObj player = (ScreenManager as RCScreenManager).Player;
			for (int i = 0; i < m_playerSprite.NumChildren; i++)
			{
				m_playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
				m_playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
			}
			m_playerSprite.GetChildAt(16).Visible = false;
			m_playerSprite.Scale = player.Scale;
			if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
			{
				m_playerSprite.GetChildAt(7).Visible = false;
			}
			m_playerSprite.GetChildAt(14).Visible = false;
			if (Game.PlayerStats.SpecialItem == 8)
			{
				m_playerSprite.GetChildAt(14).Visible = true;
			}
			if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Shield_Sprite");
			}
			else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Lamp_Sprite");
			}
			else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Beard_Sprite");
			}
			else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Headband_Sprite");
			}
			else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
			{
				m_playerSprite.GetChildAt(15).Visible = true;
				m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Horns_Sprite");
			}
			else
			{
				m_playerSprite.GetChildAt(15).Visible = false;
			}
			m_playerSprite.GetChildAt(0).Visible = false;
			if (Game.PlayerStats.Class == 16)
			{
				m_playerSprite.GetChildAt(0).Visible = true;
				m_playerSprite.GetChildAt(12).ChangeSprite(string.Concat("Player", animationType, "Head", 6, "_Sprite"));
			}
			if (Game.PlayerStats.Class == 17)
			{
				m_playerSprite.GetChildAt(12).ChangeSprite(string.Concat("Player", animationType, "Head", 7, "_Sprite"));
			}
			if (!Game.PlayerStats.IsFemale)
			{
				m_playerSprite.GetChildAt(5).Visible = false;
				m_playerSprite.GetChildAt(13).Visible = false;
			}
			else
			{
				m_playerSprite.GetChildAt(5).Visible = true;
				m_playerSprite.GetChildAt(13).Visible = true;
			}
			if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
			{
				m_playerSprite.Scale = new Vector2(3f, 3f);
			}
			if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
			{
				m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
			}
			if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
			{
				m_playerSprite.ScaleX *= 0.825f;
				m_playerSprite.ScaleY *= 1.25f;
			}
			if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
			{
				m_playerSprite.ScaleX *= 1.25f;
				m_playerSprite.ScaleY *= 1.175f;
			}
			if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
			{
				m_playerSprite.OutlineColour = Color.White;
				m_playerSprite.GetChildAt(10).Visible = false;
				m_playerSprite.GetChildAt(11).Visible = false;
			}
			else
			{
				m_playerSprite.OutlineColour = Color.Black;
				m_playerSprite.GetChildAt(10).Visible = true;
				m_playerSprite.GetChildAt(11).Visible = true;
			}
			string text = (m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
			int startIndex = text.IndexOf("_") - 1;
			text = text.Remove(startIndex, 1);
			if (Game.PlayerStats.Class == 16)
			{
				text = text.Replace("_", 6 + "_");
			}
			else if (Game.PlayerStats.Class == 17)
			{
				text = text.Replace("_", 7 + "_");
			}
			else
			{
				text = text.Replace("_", Game.PlayerStats.HeadPiece + "_");
			}
			m_playerSprite.GetChildAt(12).ChangeSprite(text);
			string text2 = (m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
			startIndex = text2.IndexOf("_") - 1;
			text2 = text2.Remove(startIndex, 1);
			text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
			m_playerSprite.GetChildAt(4).ChangeSprite(text2);
			string text3 = (m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
			startIndex = text3.IndexOf("_") - 1;
			text3 = text3.Remove(startIndex, 1);
			text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			m_playerSprite.GetChildAt(9).ChangeSprite(text3);
			string text4 = (m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
			startIndex = text4.IndexOf("_") - 1;
			text4 = text4.Remove(startIndex, 1);
			text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			m_playerSprite.GetChildAt(3).ChangeSprite(text4);
			m_playerSprite.PlayAnimation();
			m_playerSprite.CalculateBounds();
			m_playerSprite.Y = 435f - (m_playerSprite.Bounds.Bottom - m_playerSprite.Y);
		}
		public override void OnExit()
		{
			Tween.StopAllContaining(this, false);
			base.OnExit();
		}
		public override void Update(GameTime gameTime)
		{
			if (IsEnding)
			{
				m_sky.Update(gameTime);
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				UpdateBackground(num);
				if (m_backgroundIndex < m_backgroundStrings.Length)
				{
					m_backgroundSwapTimer += num;
					if (m_backgroundSwapTimer >= 75f / m_backgroundStrings.Length)
					{
						SwapBackground(m_backgroundStrings[m_backgroundIndex]);
						m_backgroundIndex++;
						m_backgroundSwapTimer = 0f;
					}
				}
			}
			base.Update(gameTime);
		}
		public void ResetScroll()
		{
			foreach (TextObj current in m_creditsTitleList)
			{
				current.Y += -m_scrollDistance;
				Tween.By(current, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			}
			foreach (TextObj current2 in m_creditsNameList)
			{
				current2.Y += -m_scrollDistance;
				Tween.By(current2, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
				PositionTeam(current2.Text, new Vector2(current2.Bounds.Left - 50, current2.Y));
			}
			Tween.By(m_teddy, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_kenny, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_glauber, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_gordon, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.By(m_judson, 75f, Tween.EaseNone, "Y", m_scrollDistance.ToString());
			Tween.RunFunction(76f, this, "ResetScroll");
		}
		public override void HandleInput()
		{
			if ((!IsEnding || (IsEnding && m_allowExit)) && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)))
			{
				if (m_displayingContinueText)
				{
					Tween.StopAll(false);
					(ScreenManager as RCScreenManager).DisplayScreen(3, true);
				}
				else
				{
					m_displayingContinueText = true;
					Tween.StopAllContaining(m_continueText, false);
					Tween.To(m_continueText, 0.5f, Tween.EaseNone, "Opacity", "1");
					Tween.RunFunction(4f, this, "HideContinueText");
				}
			}
			base.HandleInput();
		}
		public void HideContinueText()
		{
			m_displayingContinueText = false;
			Tween.To(m_continueText, 0.5f, Tween.EaseNone, "delay", "0", "Opacity", "0");
		}
		private void UpdateBackground(float elapsedTime)
		{
			int num = 200;
			m_bg1.X += num * elapsedTime;
			m_bg2.X += num * elapsedTime;
			m_bg3.X += num * elapsedTime;
			if (m_bg1.X > 930f)
			{
				m_bg1.X = m_bg3.X - m_bg3.Width;
			}
			if (m_bg2.X > 930f)
			{
				m_bg2.X = m_bg1.X - m_bg1.Width;
			}
			if (m_bg3.X > 930f)
			{
				m_bg3.X = m_bg2.X - m_bg2.Width;
			}
			m_ground1.X += num * elapsedTime;
			m_ground2.X += num * elapsedTime;
			m_ground3.X += num * elapsedTime;
			if (m_ground1.X > 930f)
			{
				m_ground1.X = m_ground3.X - m_ground3.Width;
			}
			if (m_ground2.X > 930f)
			{
				m_ground2.X = m_ground1.X - m_ground1.Width;
			}
			if (m_ground3.X > 930f)
			{
				m_ground3.X = m_ground2.X - m_ground2.Width;
			}
			m_border1.X += num * elapsedTime;
			m_border2.X += num * elapsedTime;
			m_border3.X += num * elapsedTime;
			if (m_border1.X > 930f)
			{
				m_border1.X = m_border3.X - m_border3.Width;
			}
			if (m_border2.X > 930f)
			{
				m_border2.X = m_border1.X - m_border1.Width;
			}
			if (m_border3.X > 930f)
			{
				m_border3.X = m_border2.X - m_border2.Width;
			}
			m_prop1.X += num * elapsedTime;
			m_prop2.X += num * elapsedTime;
			m_prop3.X += num * elapsedTime;
			if (m_prop1.X > 930f)
			{
				m_prop1.X -= CDGMath.RandomInt(1000, 3000);
			}
			if (m_prop2.X > 930f)
			{
				m_prop2.X -= CDGMath.RandomInt(1000, 3000);
			}
			if (m_prop3.X > 930f)
			{
				m_prop3.X -= CDGMath.RandomInt(1000, 3000);
			}
		}
		public void SwapBackground(string levelType)
		{
			Tween.By(m_sideBorderLeft, 0.5f, Tween.EaseNone, "X", "200");
			Tween.By(m_sideBorderRight, 0.5f, Tween.EaseNone, "X", "-200");
			Tween.AddEndHandlerToLastTween(this, "PerformSwap", levelType);
		}
		public void PerformSwap(string levelType)
		{
			m_manor.Y = 0f;
			m_bgOutside.Y = 0f;
			m_bgOutside.Visible = false;
			m_manor.Visible = false;
			m_ground1.Visible = (m_ground2.Visible = (m_ground3.Visible = true));
			m_border1.Visible = (m_border2.Visible = (m_border3.Visible = true));
			m_prop1.Visible = (m_prop2.Visible = (m_prop3.Visible = true));
			if (levelType != null)
			{
				if (!(levelType == "Castle"))
				{
					if (!(levelType == "Tower"))
					{
						if (!(levelType == "Dungeon"))
						{
							if (!(levelType == "Garden"))
							{
								if (!(levelType == "Outside"))
								{
									if (levelType == "Manor")
									{
										m_bgOutside.Visible = true;
										m_manor.Visible = true;
										m_ground1.Visible = (m_ground2.Visible = (m_ground3.Visible = false));
										m_border1.Visible = (m_border2.Visible = (m_border3.Visible = false));
										m_manor.Y = -260f;
										m_bgOutside.Y = -260f;
										m_manor.X -= 300f;
										m_bgOutside.X -= 300f;
										m_prop1.Visible = false;
										m_prop2.Visible = false;
										m_prop3.Visible = false;
										Tween.By(m_manor, 3f, Tween.EaseNone, "X", "300");
										Tween.By(m_bgOutside, 3f, Tween.EaseNone, "X", "300");
										Tween.By(m_playerSprite, 3.5f, Tween.EaseNone, "X", "-150");
										Tween.AddEndHandlerToLastTween(this, "CreditsComplete");
										Tween.By(m_sideBorderTop, 2.5f, Tween.EaseNone, "Y", "-500");
									}
								}
								else
								{
									m_bg1.ChangeSprite("GardenBG_Sprite");
									m_bg2.ChangeSprite("GardenBG_Sprite");
									m_bg3.ChangeSprite("GardenBG_Sprite");
									m_ground1.ChangeSprite("GardenFG_Sprite");
									m_ground2.ChangeSprite("GardenFG_Sprite");
									m_ground3.ChangeSprite("GardenFG_Sprite");
									m_border1.ChangeSprite("StartingRoomFloor_Sprite");
									m_border2.ChangeSprite("StartingRoomFloor_Sprite");
									m_border3.ChangeSprite("StartingRoomFloor_Sprite");
									m_prop1.ChangeSprite("DungeonPrison1_Sprite");
									m_prop1.Visible = false;
									m_prop2.ChangeSprite("CreditsGrass_Character");
									m_prop2.Y = 440f;
									m_prop3.ChangeSprite("CreditsTree_Character");
									m_bgOutside.Visible = true;
								}
							}
							else
							{
								m_bg1.ChangeSprite("GardenBG_Sprite");
								m_bg2.ChangeSprite("GardenBG_Sprite");
								m_bg3.ChangeSprite("GardenBG_Sprite");
								m_ground1.ChangeSprite("GardenFG_Sprite");
								m_ground2.ChangeSprite("GardenFG_Sprite");
								m_ground3.ChangeSprite("GardenFG_Sprite");
								m_border1.ChangeSprite("GardenBorder_Sprite");
								m_border2.ChangeSprite("GardenBorder_Sprite");
								m_border3.ChangeSprite("GardenBorder_Sprite");
								m_prop1.ChangeSprite("GardenFloatingRock3_Sprite");
								m_prop2.ChangeSprite("GardenFairy_Character");
								m_prop2.PlayAnimation();
								m_prop2.AnimationDelay = 0.1f;
								m_prop3.ChangeSprite("GardenLampPost1_Character");
							}
						}
						else
						{
							m_bg1.ChangeSprite("DungeonBG1_Sprite");
							m_bg2.ChangeSprite("DungeonBG1_Sprite");
							m_bg3.ChangeSprite("DungeonBG1_Sprite");
							m_ground1.ChangeSprite("DungeonFG1_Sprite");
							m_ground2.ChangeSprite("DungeonFG1_Sprite");
							m_ground3.ChangeSprite("DungeonFG1_Sprite");
							m_border1.ChangeSprite("DungeonBorder_Sprite");
							m_border2.ChangeSprite("DungeonBorder_Sprite");
							m_border3.ChangeSprite("DungeonBorder_Sprite");
							m_prop1.ChangeSprite("DungeonPrison1_Sprite");
							m_prop2.ChangeSprite("DungeonChain2_Character");
							m_prop3.ChangeSprite("DungeonTorch2_Character");
						}
					}
					else
					{
						m_bg1.ChangeSprite("TowerBG2_Sprite");
						m_bg2.ChangeSprite("TowerBG2_Sprite");
						m_bg3.ChangeSprite("TowerBG2_Sprite");
						m_ground1.ChangeSprite("TowerFG2_Sprite");
						m_ground2.ChangeSprite("TowerFG2_Sprite");
						m_ground3.ChangeSprite("TowerFG2_Sprite");
						m_border1.ChangeSprite("TowerBorder2_Sprite");
						m_border2.ChangeSprite("TowerBorder2_Sprite");
						m_border3.ChangeSprite("TowerBorder2_Sprite");
						m_prop1.ChangeSprite("TowerHole4_Sprite");
						m_prop2.Visible = false;
						m_prop3.ChangeSprite("TowerPedestal2_Character");
					}
				}
				else
				{
					m_bg1.ChangeSprite("CastleBG1_Sprite");
					m_bg2.ChangeSprite("CastleBG1_Sprite");
					m_bg3.ChangeSprite("CastleBG1_Sprite");
					m_ground1.ChangeSprite("CastleFG1_Sprite");
					m_ground2.ChangeSprite("CastleFG1_Sprite");
					m_ground3.ChangeSprite("CastleFG1_Sprite");
					m_border1.ChangeSprite("CastleBorder_Sprite");
					m_border2.ChangeSprite("CastleBorder_Sprite");
					m_border3.ChangeSprite("CastleBorder_Sprite");
					m_prop1.ChangeSprite("CastleAssetWindow1_Sprite");
					m_prop2.ChangeSprite("CastleAssetBackTorch_Character");
					m_prop2.PlayAnimation();
					m_prop2.AnimationDelay = 0.1f;
					m_prop3.ChangeSprite("CastleAssetCandle1_Character");
				}
			}
			if (levelType != "Manor")
			{
				Tween.By(m_sideBorderLeft, 0.5f, Tween.EaseNone, "X", "-200");
				Tween.By(m_sideBorderRight, 0.5f, Tween.EaseNone, "X", "200");
				return;
			}
			Tween.By(m_sideBorderLeft, 3f, Tween.EaseNone, "X", "-800");
			Tween.By(m_sideBorderRight, 3f, Tween.EaseNone, "X", "800");
		}
		public void CreditsComplete()
		{
			SetPlayerStyle("Idle");
			Tween.RunFunction(0.5f, this, "SetPlayerStyle", "LevelUp");
			Tween.RunFunction(0.6f, m_playerSprite, "PlayAnimation", false);
			Tween.To(m_thanksForPlayingText, 2f, Tween.EaseNone, "Opacity", "1");
			Tween.To(m_totalDeaths, 2f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
			Tween.To(m_totalPlayTime, 2f, Tween.EaseNone, "delay", "0.4", "Opacity", "1");
			Tween.RunFunction(1f, this, "BringWife");
			Tween.RunFunction(1.1f, this, "BringChild1");
			Tween.RunFunction(3f, this, "BringChild2");
		}
		public void BringWife()
		{
			m_wifeSprite.GetChildAt(14).Visible = false;
			m_wifeSprite.GetChildAt(15).Visible = false;
			m_wifeSprite.GetChildAt(16).Visible = false;
			m_wifeSprite.GetChildAt(0).Visible = false;
			m_wifeChest = CDGMath.RandomInt(1, 5);
			m_wifeHead = CDGMath.RandomInt(1, 5);
			m_wifeShoulders = CDGMath.RandomInt(1, 5);
			m_wifeSprite.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + m_wifeChest + "_Sprite");
			m_wifeSprite.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + m_wifeHead + "_Sprite");
			m_wifeSprite.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + m_wifeShoulders + "_Sprite");
			m_wifeSprite.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + m_wifeShoulders + "_Sprite");
			if ((!Game.PlayerStats.IsFemale && Game.PlayerStats.Traits.X != 2f && Game.PlayerStats.Traits.Y != 2f) || (Game.PlayerStats.IsFemale && (Game.PlayerStats.Traits.X == 2f || Game.PlayerStats.Traits.Y == 2f)))
			{
				m_wifeSprite.GetChildAt(13).Visible = true;
				m_wifeSprite.GetChildAt(5).Visible = true;
			}
			else
			{
				m_wifeSprite.GetChildAt(13).Visible = false;
				m_wifeSprite.GetChildAt(5).Visible = false;
			}
			m_wifeSprite.PlayAnimation();
			Tween.By(m_wifeSprite, 3f, Tween.EaseNone, "X", "600");
			Tween.AddEndHandlerToLastTween(this, "LevelUpWife");
		}
		public void LevelUpWife()
		{
			m_wifeSprite.ChangeSprite("PlayerLevelUp_Character");
			m_wifeSprite.GetChildAt(14).Visible = false;
			m_wifeSprite.GetChildAt(15).Visible = false;
			m_wifeSprite.GetChildAt(16).Visible = false;
			m_wifeSprite.GetChildAt(0).Visible = false;
			if ((!Game.PlayerStats.IsFemale && Game.PlayerStats.Traits.X != 2f && Game.PlayerStats.Traits.Y != 2f) || (Game.PlayerStats.IsFemale && (Game.PlayerStats.Traits.X == 2f || Game.PlayerStats.Traits.Y == 2f)))
			{
				m_wifeSprite.GetChildAt(13).Visible = true;
				m_wifeSprite.GetChildAt(5).Visible = true;
			}
			else
			{
				m_wifeSprite.GetChildAt(13).Visible = false;
				m_wifeSprite.GetChildAt(5).Visible = false;
			}
			m_wifeSprite.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + m_wifeChest + "_Sprite");
			m_wifeSprite.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + m_wifeHead + "_Sprite");
			m_wifeSprite.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + m_wifeShoulders + "_Sprite");
			m_wifeSprite.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + m_wifeShoulders + "_Sprite");
			m_wifeSprite.PlayAnimation(false);
		}
		public void BringChild1()
		{
			m_childSprite1.GetChildAt(14).Visible = false;
			m_childSprite1.GetChildAt(15).Visible = false;
			m_childSprite1.GetChildAt(16).Visible = false;
			m_childSprite1.GetChildAt(0).Visible = false;
			m_child1Chest = CDGMath.RandomInt(1, 5);
			m_child1Head = CDGMath.RandomInt(1, 5);
			m_child1Shoulders = CDGMath.RandomInt(1, 5);
			bool flag = false;
			if (CDGMath.RandomInt(0, 1) > 0)
			{
				flag = true;
			}
			if (flag)
			{
				m_childSprite1.GetChildAt(13).Visible = true;
				m_childSprite1.GetChildAt(5).Visible = true;
			}
			else
			{
				m_childSprite1.GetChildAt(13).Visible = false;
				m_childSprite1.GetChildAt(5).Visible = false;
			}
			m_childSprite1.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + m_child1Chest + "_Sprite");
			m_childSprite1.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + m_child1Head + "_Sprite");
			m_childSprite1.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + m_child1Shoulders + "_Sprite");
			m_childSprite1.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + m_child1Shoulders + "_Sprite");
			m_childSprite1.PlayAnimation();
			Tween.By(m_childSprite1, 3f, Tween.EaseNone, "X", "600");
			Tween.AddEndHandlerToLastTween(this, "LevelUpChild1", flag);
		}
		public void LevelUpChild1(bool isFemale)
		{
			m_childSprite1.ChangeSprite("PlayerLevelUp_Character");
			m_childSprite1.GetChildAt(14).Visible = false;
			m_childSprite1.GetChildAt(15).Visible = false;
			m_childSprite1.GetChildAt(16).Visible = false;
			m_childSprite1.GetChildAt(0).Visible = false;
			if (isFemale)
			{
				m_childSprite1.GetChildAt(13).Visible = true;
				m_childSprite1.GetChildAt(5).Visible = true;
			}
			else
			{
				m_childSprite1.GetChildAt(13).Visible = false;
				m_childSprite1.GetChildAt(5).Visible = false;
			}
			m_childSprite1.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + m_child1Chest + "_Sprite");
			m_childSprite1.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + m_child1Head + "_Sprite");
			m_childSprite1.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + m_child1Shoulders + "_Sprite");
			m_childSprite1.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + m_child1Shoulders + "_Sprite");
			m_childSprite1.PlayAnimation(false);
		}
		public void BringChild2()
		{
			m_childSprite2.GetChildAt(14).Visible = false;
			m_childSprite2.GetChildAt(15).Visible = false;
			m_childSprite2.GetChildAt(16).Visible = false;
			m_childSprite2.GetChildAt(0).Visible = false;
			bool flag = false;
			if (CDGMath.RandomInt(0, 1) > 0)
			{
				flag = true;
			}
			if (flag)
			{
				m_childSprite2.GetChildAt(13).Visible = true;
				m_childSprite2.GetChildAt(5).Visible = true;
			}
			else
			{
				m_childSprite2.GetChildAt(13).Visible = false;
				m_childSprite2.GetChildAt(5).Visible = false;
			}
			m_child2Chest = CDGMath.RandomInt(1, 5);
			m_child2Head = CDGMath.RandomInt(1, 5);
			m_child2Shoulders = CDGMath.RandomInt(1, 5);
			m_childSprite2.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + m_child2Chest + "_Sprite");
			m_childSprite2.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + m_child2Head + "_Sprite");
			m_childSprite2.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + m_child2Shoulders + "_Sprite");
			m_childSprite2.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + m_child2Shoulders + "_Sprite");
			m_childSprite2.PlayAnimation();
			Tween.By(m_childSprite2, 2f, Tween.EaseNone, "X", "600");
			Tween.AddEndHandlerToLastTween(this, "LevelUpChild2", flag);
		}
		public void LevelUpChild2(bool isFemale)
		{
			m_childSprite2.ChangeSprite("PlayerLevelUp_Character");
			m_childSprite2.GetChildAt(14).Visible = false;
			m_childSprite2.GetChildAt(15).Visible = false;
			m_childSprite2.GetChildAt(16).Visible = false;
			m_childSprite2.GetChildAt(0).Visible = false;
			if (isFemale)
			{
				m_childSprite2.GetChildAt(13).Visible = true;
				m_childSprite2.GetChildAt(5).Visible = true;
			}
			else
			{
				m_childSprite2.GetChildAt(13).Visible = false;
				m_childSprite2.GetChildAt(5).Visible = false;
			}
			m_childSprite2.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + m_child2Chest + "_Sprite");
			m_childSprite2.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + m_child2Head + "_Sprite");
			m_childSprite2.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + m_child2Shoulders + "_Sprite");
			m_childSprite2.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + m_child2Shoulders + "_Sprite");
			m_childSprite2.PlayAnimation(false);
			m_allowExit = true;
			m_displayingContinueText = true;
			Tween.StopAllContaining(m_continueText, false);
			Tween.To(m_continueText, 0.5f, Tween.EaseNone, "Opacity", "1");
		}
		public override void Draw(GameTime gametime)
		{
			Camera.GraphicsDevice.SetRenderTarget(m_skyRenderTarget);
			Camera.GraphicsDevice.Clear(Color.Black);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
			m_sky.Draw(Camera);
			Camera.End();
			Camera.GraphicsDevice.SetRenderTarget(m_backgroundRenderTarget);
			Camera.GraphicsDevice.Clear(Color.Black);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			m_bg1.Draw(Camera);
			m_bg2.Draw(Camera);
			m_bg3.Draw(Camera);
			m_bgOutside.Draw(Camera);
			m_ground1.Draw(Camera);
			m_ground2.Draw(Camera);
			m_ground3.Draw(Camera);
			m_border1.Draw(Camera);
			m_border2.Draw(Camera);
			m_border3.Draw(Camera);
			m_manor.Draw(Camera);
			m_prop1.Draw(Camera);
			m_prop2.Draw(Camera);
			m_prop3.Draw(Camera);
			m_playerSprite.Draw(Camera);
			Game.ColourSwapShader.Parameters["desiredTint"].SetValue(m_playerSprite.GetChildAt(12).TextureColor.ToVector4());
			if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
			{
				Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);
				Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_lichColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_lichColour2.ToVector4());
			}
			else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
			{
				Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);
				Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
			}
			else
			{
				Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
				Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_skinColour2.ToVector4());
			}
			Camera.End();
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
			m_playerSprite.GetChildAt(12).Draw(Camera);
			Camera.End();
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			if (Game.PlayerStats.IsFemale)
			{
				m_playerSprite.GetChildAt(13).Draw(Camera);
			}
			m_playerSprite.GetChildAt(15).Draw(Camera);
			m_wifeSprite.Draw(Camera);
			m_childSprite1.Draw(Camera);
			m_childSprite2.Draw(Camera);
			m_sideBorderLeft.Draw(Camera);
			m_sideBorderRight.Draw(Camera);
			m_sideBorderTop.Draw(Camera);
			m_sideBorderBottom.Draw(Camera);
			m_teddy.Draw(Camera);
			m_kenny.Draw(Camera);
			m_glauber.Draw(Camera);
			m_gordon.Draw(Camera);
			m_judson.Draw(Camera);
			Camera.End();
			Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
			Camera.GraphicsDevice.Textures[1] = m_skyRenderTarget;
			Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ParallaxEffect);
			Camera.Draw(m_backgroundRenderTarget, Vector2.Zero, Color.White);
			Camera.End();
			Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			Rectangle b = new Rectangle(0, 0, 1320, 720);
			foreach (TextObj current in m_creditsTitleList)
			{
				if (CollisionMath.Intersects(current.Bounds, b))
				{
					current.Draw(Camera);
				}
			}
			foreach (TextObj current2 in m_creditsNameList)
			{
				if (CollisionMath.Intersects(current2.Bounds, b))
				{
					current2.Draw(Camera);
				}
			}
			m_thanksForPlayingText.Draw(Camera);
			m_totalDeaths.Draw(Camera);
			m_totalPlayTime.Draw(Camera);
			m_continueText.Draw(Camera);
			Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Credits Screen");
				Array.Clear(m_backgroundStrings, 0, m_backgroundStrings.Length);
				m_backgroundStrings = null;
				m_playerSprite.Dispose();
				m_playerSprite = null;
				m_wifeSprite.Dispose();
				m_wifeSprite = null;
				m_childSprite1.Dispose();
				m_childSprite1 = null;
				m_childSprite2.Dispose();
				m_childSprite2 = null;
				m_manor.Dispose();
				m_manor = null;
				m_thanksForPlayingText.Dispose();
				m_thanksForPlayingText = null;
				m_sideBorderRight.Dispose();
				m_sideBorderRight = null;
				m_sideBorderLeft.Dispose();
				m_sideBorderLeft = null;
				m_sideBorderTop.Dispose();
				m_sideBorderTop = null;
				m_sideBorderBottom.Dispose();
				m_sideBorderBottom = null;
				m_bgOutside.Dispose();
				m_bgOutside = null;
				m_sky.Dispose();
				m_sky = null;
				m_skyRenderTarget.Dispose();
				m_skyRenderTarget = null;
				m_backgroundRenderTarget.Dispose();
				m_backgroundRenderTarget = null;
				foreach (TextObj current in m_creditsTitleList)
				{
					current.Dispose();
				}
				m_creditsTitleList.Clear();
				m_creditsTitleList = null;
				foreach (TextObj current2 in m_creditsNameList)
				{
					current2.Dispose();
				}
				m_creditsNameList.Clear();
				m_creditsNameList = null;
				m_bg1.Dispose();
				m_bg2.Dispose();
				m_bg3.Dispose();
				m_ground1.Dispose();
				m_ground2.Dispose();
				m_ground3.Dispose();
				m_border1.Dispose();
				m_border2.Dispose();
				m_border3.Dispose();
				m_prop1.Dispose();
				m_prop2.Dispose();
				m_prop3.Dispose();
				m_prop1 = null;
				m_prop2 = null;
				m_prop3 = null;
				m_bg1 = null;
				m_bg2 = null;
				m_bg3 = null;
				m_ground1 = null;
				m_ground2 = null;
				m_ground3 = null;
				m_border1 = null;
				m_border2 = null;
				m_border3 = null;
				m_teddy.Dispose();
				m_kenny.Dispose();
				m_glauber.Dispose();
				m_gordon.Dispose();
				m_judson.Dispose();
				m_teddy = null;
				m_kenny = null;
				m_glauber = null;
				m_gordon = null;
				m_judson = null;
				m_continueText.Dispose();
				m_continueText = null;
				m_totalDeaths.Dispose();
				m_totalDeaths = null;
				m_totalPlayTime.Dispose();
				m_totalPlayTime = null;
				base.Dispose();
			}
		}
	}
}
