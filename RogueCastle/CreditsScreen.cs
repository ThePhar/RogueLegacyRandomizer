using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
			this.m_bgOutside = new SpriteObj("TraitsBG_Sprite");
			this.m_bgOutside.ForceDraw = true;
			this.m_bgOutside.Visible = false;
			this.m_bgOutside.Scale = new Vector2(1320f / (float)this.m_bgOutside.Width, 1320f / (float)this.m_bgOutside.Width);
			this.m_bg1 = new SpriteObj("CastleBG1_Sprite");
			this.m_bg1.Position = new Vector2(660f, 200f);
			this.m_bg1.Scale = new Vector2(2f, 2f);
			this.m_bg2 = (this.m_bg1.Clone() as SpriteObj);
			this.m_bg2.X -= (float)this.m_bg1.Width;
			this.m_bg3 = (this.m_bg2.Clone() as SpriteObj);
			this.m_bg3.X -= (float)this.m_bg2.Width;
			this.m_ground1 = new SpriteObj("CastleFG1_Sprite");
			this.m_ground1.Position = new Vector2(660f, 440f);
			this.m_ground1.Scale = new Vector2(2f, 2f);
			this.m_ground2 = (this.m_ground1.Clone() as SpriteObj);
			this.m_ground2.X -= (float)this.m_ground1.Width;
			this.m_ground3 = (this.m_ground2.Clone() as SpriteObj);
			this.m_ground3.X -= (float)this.m_ground2.Width;
			this.m_border1 = new SpriteObj("CastleBorder_Sprite");
			this.m_border1.Position = new Vector2(660f, 440f);
			this.m_border1.Scale = new Vector2(2f, 2f);
			this.m_border2 = (this.m_border1.Clone() as SpriteObj);
			this.m_border2.X -= (float)this.m_border1.Width;
			this.m_border3 = (this.m_border2.Clone() as SpriteObj);
			this.m_border3.X -= (float)this.m_border2.Width;
			this.m_prop1 = new SpriteObj("CastleAssetWindow1_Sprite");
			this.m_prop1.Position = new Vector2(0f, 220f);
			this.m_prop1.Scale = new Vector2(2f, 2f);
			this.m_prop2 = new ObjContainer("CastleAssetBackTorch_Character");
			this.m_prop2.Position = new Vector2(500f, 330f);
			this.m_prop2.Scale = new Vector2(2f, 2f);
			this.m_prop2.AnimationDelay = 0.1f;
			this.m_prop2.PlayAnimation(true);
			this.m_prop3 = new ObjContainer("CastleAssetCandle1_Character");
			this.m_prop3.Position = new Vector2(1000f, 440f);
			this.m_prop3.Scale = new Vector2(2f, 2f);
			this.m_playerSprite = new ObjContainer("PlayerWalking_Character");
			this.m_playerSprite.Position = new Vector2(640f, 400f);
			this.m_playerSprite.PlayAnimation(true);
			this.m_playerSprite.AnimationDelay = 0.1f;
			this.m_playerSprite.Flip = SpriteEffects.FlipHorizontally;
			this.m_playerSprite.OutlineWidth = 2;
			Color textureColor = new Color(251, 156, 172);
			this.m_wifeSprite = new ObjContainer("PlayerWalking_Character");
			this.m_wifeSprite.Position = new Vector2(-200f, 400f);
			this.m_wifeSprite.PlayAnimation(true);
			this.m_wifeSprite.AnimationDelay = 0.1f;
			this.m_wifeSprite.OutlineWidth = 2;
			this.m_wifeSprite.Scale = new Vector2(2f, 2f);
			this.m_wifeSprite.GetChildAt(13).TextureColor = textureColor;
			this.m_wifeSprite.GetChildAt(7).TextureColor = Color.Red;
			this.m_wifeSprite.GetChildAt(1).TextureColor = Color.Red;
			this.m_wifeSprite.GetChildAt(8).TextureColor = Color.Red;
			this.m_wifeSprite.GetChildAt(11).TextureColor = new Color(11, 172, 239);
			this.m_childSprite1 = new ObjContainer("PlayerWalking_Character");
			this.m_childSprite1.Position = new Vector2(-270f, 415f);
			this.m_childSprite1.PlayAnimation(true);
			this.m_childSprite1.AnimationDelay = 0.1f;
			this.m_childSprite1.OutlineWidth = 2;
			this.m_childSprite1.Scale = new Vector2(1.2f, 1.2f);
			this.m_childSprite1.GetChildAt(13).TextureColor = textureColor;
			this.m_childSprite1.GetChildAt(7).TextureColor = Color.Red;
			this.m_childSprite1.GetChildAt(1).TextureColor = Color.Red;
			this.m_childSprite1.GetChildAt(8).TextureColor = Color.Red;
			this.m_childSprite1.GetChildAt(11).TextureColor = new Color(11, 172, 239);
			this.m_childSprite2 = new ObjContainer("PlayerWalking_Character");
			this.m_childSprite2.Position = new Vector2(-330f, 420f);
			this.m_childSprite2.PlayAnimation(true);
			this.m_childSprite2.AnimationDelay = 0.1f;
			this.m_childSprite2.OutlineWidth = 2;
			this.m_childSprite2.Scale = new Vector2(1f, 1f);
			this.m_childSprite2.GetChildAt(13).TextureColor = textureColor;
			this.m_childSprite2.GetChildAt(7).TextureColor = Color.Red;
			this.m_childSprite2.GetChildAt(1).TextureColor = Color.Red;
			this.m_childSprite2.GetChildAt(8).TextureColor = Color.Red;
			this.m_childSprite2.GetChildAt(11).TextureColor = new Color(11, 172, 239);
			this.m_sideBorderLeft = new SpriteObj("Blank_Sprite");
			this.m_sideBorderLeft.Scale = new Vector2(900f / (float)this.m_sideBorderLeft.Width, 500f / (float)this.m_sideBorderLeft.Height);
			this.m_sideBorderLeft.Position = new Vector2(-450f, 0f);
			this.m_sideBorderLeft.TextureColor = Color.Black;
			this.m_sideBorderLeft.ForceDraw = true;
			this.m_sideBorderRight = (this.m_sideBorderLeft.Clone() as SpriteObj);
			this.m_sideBorderRight.Position = new Vector2(850f, 0f);
			this.m_sideBorderTop = (this.m_sideBorderLeft.Clone() as SpriteObj);
			this.m_sideBorderTop.Scale = new Vector2(1f, 1f);
			this.m_sideBorderTop.Scale = new Vector2(1320f / (float)this.m_sideBorderTop.Width, (float)(240 / this.m_sideBorderTop.Height));
			this.m_sideBorderTop.Position = Vector2.Zero;
			this.m_sideBorderBottom = (this.m_sideBorderLeft.Clone() as SpriteObj);
			this.m_sideBorderBottom.Scale = new Vector2(1f, 1f);
			this.m_sideBorderBottom.Scale = new Vector2(1340f / (float)this.m_sideBorderBottom.Width, 720f / (float)this.m_sideBorderBottom.Height);
			this.m_sideBorderBottom.Position = new Vector2(0f, 460f);
			this.m_manor = new ObjContainer("TraitsCastle_Character");
			this.m_manor.Scale = new Vector2(2f, 2f);
			this.m_manor.Visible = false;
			for (int i = 0; i < this.m_manor.NumChildren; i++)
			{
				this.m_manor.GetChildAt(i).Visible = false;
			}
			foreach (SkillObj current in SkillSystem.SkillArray)
			{
				if (current.CurrentLevel > 0)
				{
					this.m_manor.GetChildAt(SkillSystem.GetManorPiece(current)).Visible = true;
				}
			}
			this.m_thanksForPlayingText = new TextObj(Game.JunicodeLargeFont);
			this.m_thanksForPlayingText.FontSize = 32f;
			this.m_thanksForPlayingText.Align = Types.TextAlign.Centre;
			this.m_thanksForPlayingText.Text = "Thanks for playing!";
			this.m_thanksForPlayingText.DropShadow = new Vector2(2f, 2f);
			this.m_thanksForPlayingText.Position = new Vector2(660f, 480f);
			this.m_thanksForPlayingText.Opacity = 0f;
			this.m_totalDeaths = (this.m_thanksForPlayingText.Clone() as TextObj);
			this.m_totalDeaths.FontSize = 20f;
			this.m_totalDeaths.Position = this.m_thanksForPlayingText.Position;
			this.m_totalDeaths.Y += 90f;
			this.m_totalDeaths.Opacity = 0f;
			this.m_totalPlayTime = (this.m_thanksForPlayingText.Clone() as TextObj);
			this.m_totalPlayTime.FontSize = 20f;
			this.m_totalPlayTime.Position = this.m_totalDeaths.Position;
			this.m_totalPlayTime.Y += 50f;
			this.m_totalPlayTime.Opacity = 0f;
			this.m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_continueText.FontSize = 14f;
			this.m_continueText.Align = Types.TextAlign.Right;
			this.m_continueText.Position = new Vector2(1270f, 650f);
			this.m_continueText.ForceDraw = true;
			this.m_continueText.Opacity = 0f;
			int num = 200;
			this.m_glauber = new SpriteObj("Glauber_Sprite");
			this.m_glauber.Scale = new Vector2(2f, 2f);
			this.m_glauber.ForceDraw = true;
			this.m_glauber.OutlineWidth = 2;
			this.m_glauber.X = (float)num;
			this.m_teddy = new SpriteObj("Teddy_Sprite");
			this.m_teddy.Scale = new Vector2(2f, 2f);
			this.m_teddy.ForceDraw = true;
			this.m_teddy.OutlineWidth = 2;
			this.m_teddy.X = (float)num;
			this.m_kenny = new SpriteObj("Kenny_Sprite");
			this.m_kenny.Scale = new Vector2(2f, 2f);
			this.m_kenny.ForceDraw = true;
			this.m_kenny.OutlineWidth = 2;
			this.m_kenny.X = (float)num;
			this.m_gordon = new SpriteObj("Gordon_Sprite");
			this.m_gordon.Scale = new Vector2(2f, 2f);
			this.m_gordon.ForceDraw = true;
			this.m_gordon.OutlineWidth = 2;
			this.m_gordon.X = (float)num;
			this.m_judson = new SpriteObj("Judson_Sprite");
			this.m_judson.Scale = new Vector2(2f, 2f);
			this.m_judson.ForceDraw = true;
			this.m_judson.OutlineWidth = 2;
			this.m_judson.X = (float)num;
			this.InitializeCredits();
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			this.m_sky.ReinitializeRT(base.Camera);
			this.m_skyRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
			this.m_backgroundRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
			base.ReinitializeRTs();
		}
		private void InitializeCredits()
		{
			this.m_creditsNameList = new List<TextObj>();
			this.m_creditsTitleList = new List<TextObj>();
			this.m_backgroundStrings = new string[]
			{
				"Garden",
				"Tower",
				"Dungeon",
				"Outside",
				"Manor"
			};
			string[] array = new string[]
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
			string[] array2 = new string[]
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
				textObj.Position = new Vector2(660f, (float)(720 + num));
				if (i < array2.Length)
				{
					textObj.Text = array2[i];
					this.m_creditsTitleList.Add(textObj);
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
				this.m_creditsNameList.Add(textObj2);
				this.PositionTeam(array[i], new Vector2((float)(textObj2.Bounds.Left - 50), textObj2.Y));
			}
		}
		private void PositionTeam(string name, Vector2 position)
		{
			if (name.Contains("Teddy"))
			{
				this.m_teddy.Position = position;
				return;
			}
			if (name.Contains("Kenny"))
			{
				this.m_kenny.Position = position;
				this.m_kenny.X -= 50f;
				return;
			}
			if (name.Contains("Glauber"))
			{
				this.m_glauber.Position = position;
				this.m_glauber.X -= 20f;
				return;
			}
			if (name.Contains("Gordon"))
			{
				this.m_gordon.Position = position;
				this.m_gordon.X += 75f;
				return;
			}
			if (name.Contains("Judson"))
			{
				this.m_judson.Position = position;
				this.m_judson.X += 40f;
			}
		}
		public override void OnEnter()
		{
			this.m_allowExit = false;
			float num = Game.PlayerStats.TotalHoursPlayed + Game.GetTotalGameTimeHours();
			int num2 = (int)((num - (float)((int)num)) * 60f);
			Console.WriteLine(string.Concat(new object[]
			{
				"Hours played: ",
				num,
				" minutes: ",
				num2
			}));
			this.m_totalDeaths.Text = "Total Children: " + Game.PlayerStats.TimesDead.ToString();
			if (num2 < 10)
			{
				this.m_totalPlayTime.Text = string.Concat(new object[]
				{
					"Time Played - ",
					(int)num,
					":0",
					num2
				});
			}
			else
			{
				this.m_totalPlayTime.Text = string.Concat(new object[]
				{
					"Time Played - ",
					(int)num,
					":",
					num2
				});
			}
			base.Camera.Position = Vector2.Zero;
			this.m_displayingContinueText = false;
			this.m_continueText.Text = "Press [Input:" + 0 + "] to exit";
			if (this.m_sky == null)
			{
				this.m_sky = new SkyObj(null);
				this.m_skyRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
				this.m_sky.LoadContent(base.Camera);
				this.m_backgroundRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
			}
			this.SetPlayerStyle("Walking");
			if (!this.IsEnding)
			{
				SoundManager.PlayMusic("CreditsSong", true, 1f);
			}
			this.m_scrollDistance = -(this.m_creditsNameList[this.m_creditsNameList.Count - 1].Y + 100f);
			foreach (TextObj current in this.m_creditsTitleList)
			{
				Tween.By(current, 75f, new Easing(Tween.EaseNone), new string[]
				{
					"Y",
					this.m_scrollDistance.ToString()
				});
			}
			foreach (TextObj current2 in this.m_creditsNameList)
			{
				Tween.By(current2, 75f, new Easing(Tween.EaseNone), new string[]
				{
					"Y",
					this.m_scrollDistance.ToString()
				});
			}
			Tween.By(this.m_teddy, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_kenny, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_glauber, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_gordon, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_judson, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			if (!this.IsEnding)
			{
				this.m_sideBorderLeft.X += 200f;
				this.m_sideBorderRight.X -= 200f;
				Tween.RunFunction(76f, this, "ResetScroll", new object[0]);
			}
			base.OnEnter();
		}
		public void SetPlayerStyle(string animationType)
		{
			this.m_playerSprite.ChangeSprite("Player" + animationType + "_Character");
			PlayerObj player = (base.ScreenManager as RCScreenManager).Player;
			for (int i = 0; i < this.m_playerSprite.NumChildren; i++)
			{
				this.m_playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
				this.m_playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
			}
			this.m_playerSprite.GetChildAt(16).Visible = false;
			this.m_playerSprite.Scale = player.Scale;
			if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
			{
				this.m_playerSprite.GetChildAt(7).Visible = false;
			}
			this.m_playerSprite.GetChildAt(14).Visible = false;
			if (Game.PlayerStats.SpecialItem == 8)
			{
				this.m_playerSprite.GetChildAt(14).Visible = true;
			}
			if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Shield_Sprite");
			}
			else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Lamp_Sprite");
			}
			else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Beard_Sprite");
			}
			else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Headband_Sprite");
			}
			else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
			{
				this.m_playerSprite.GetChildAt(15).Visible = true;
				this.m_playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Horns_Sprite");
			}
			else
			{
				this.m_playerSprite.GetChildAt(15).Visible = false;
			}
			this.m_playerSprite.GetChildAt(0).Visible = false;
			if (Game.PlayerStats.Class == 16)
			{
				this.m_playerSprite.GetChildAt(0).Visible = true;
				this.m_playerSprite.GetChildAt(12).ChangeSprite(string.Concat(new object[]
				{
					"Player",
					animationType,
					"Head",
					6,
					"_Sprite"
				}));
			}
			if (Game.PlayerStats.Class == 17)
			{
				this.m_playerSprite.GetChildAt(12).ChangeSprite(string.Concat(new object[]
				{
					"Player",
					animationType,
					"Head",
					7,
					"_Sprite"
				}));
			}
			if (!Game.PlayerStats.IsFemale)
			{
				this.m_playerSprite.GetChildAt(5).Visible = false;
				this.m_playerSprite.GetChildAt(13).Visible = false;
			}
			else
			{
				this.m_playerSprite.GetChildAt(5).Visible = true;
				this.m_playerSprite.GetChildAt(13).Visible = true;
			}
			if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
			{
				this.m_playerSprite.Scale = new Vector2(3f, 3f);
			}
			if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
			{
				this.m_playerSprite.Scale = new Vector2(1.35f, 1.35f);
			}
			if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
			{
				this.m_playerSprite.ScaleX *= 0.825f;
				this.m_playerSprite.ScaleY *= 1.25f;
			}
			if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
			{
				this.m_playerSprite.ScaleX *= 1.25f;
				this.m_playerSprite.ScaleY *= 1.175f;
			}
			if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
			{
				this.m_playerSprite.OutlineColour = Color.White;
				this.m_playerSprite.GetChildAt(10).Visible = false;
				this.m_playerSprite.GetChildAt(11).Visible = false;
			}
			else
			{
				this.m_playerSprite.OutlineColour = Color.Black;
				this.m_playerSprite.GetChildAt(10).Visible = true;
				this.m_playerSprite.GetChildAt(11).Visible = true;
			}
			string text = (this.m_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
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
			this.m_playerSprite.GetChildAt(12).ChangeSprite(text);
			string text2 = (this.m_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
			startIndex = text2.IndexOf("_") - 1;
			text2 = text2.Remove(startIndex, 1);
			text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
			this.m_playerSprite.GetChildAt(4).ChangeSprite(text2);
			string text3 = (this.m_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
			startIndex = text3.IndexOf("_") - 1;
			text3 = text3.Remove(startIndex, 1);
			text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			this.m_playerSprite.GetChildAt(9).ChangeSprite(text3);
			string text4 = (this.m_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
			startIndex = text4.IndexOf("_") - 1;
			text4 = text4.Remove(startIndex, 1);
			text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
			this.m_playerSprite.GetChildAt(3).ChangeSprite(text4);
			this.m_playerSprite.PlayAnimation(true);
			this.m_playerSprite.CalculateBounds();
			this.m_playerSprite.Y = 435f - ((float)this.m_playerSprite.Bounds.Bottom - this.m_playerSprite.Y);
		}
		public override void OnExit()
		{
			Tween.StopAllContaining(this, false);
			base.OnExit();
		}
		public override void Update(GameTime gameTime)
		{
			if (this.IsEnding)
			{
				this.m_sky.Update(gameTime);
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				this.UpdateBackground(num);
				if (this.m_backgroundIndex < this.m_backgroundStrings.Length)
				{
					this.m_backgroundSwapTimer += num;
					if (this.m_backgroundSwapTimer >= 75f / (float)this.m_backgroundStrings.Length)
					{
						this.SwapBackground(this.m_backgroundStrings[this.m_backgroundIndex]);
						this.m_backgroundIndex++;
						this.m_backgroundSwapTimer = 0f;
					}
				}
			}
			base.Update(gameTime);
		}
		public void ResetScroll()
		{
			foreach (TextObj current in this.m_creditsTitleList)
			{
				current.Y += -this.m_scrollDistance;
				Tween.By(current, 75f, new Easing(Tween.EaseNone), new string[]
				{
					"Y",
					this.m_scrollDistance.ToString()
				});
			}
			foreach (TextObj current2 in this.m_creditsNameList)
			{
				current2.Y += -this.m_scrollDistance;
				Tween.By(current2, 75f, new Easing(Tween.EaseNone), new string[]
				{
					"Y",
					this.m_scrollDistance.ToString()
				});
				this.PositionTeam(current2.Text, new Vector2((float)(current2.Bounds.Left - 50), current2.Y));
			}
			Tween.By(this.m_teddy, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_kenny, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_glauber, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_gordon, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.By(this.m_judson, 75f, new Easing(Tween.EaseNone), new string[]
			{
				"Y",
				this.m_scrollDistance.ToString()
			});
			Tween.RunFunction(76f, this, "ResetScroll", new object[0]);
		}
		public override void HandleInput()
		{
			if ((!this.IsEnding || (this.IsEnding && this.m_allowExit)) && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)))
			{
				if (this.m_displayingContinueText)
				{
					Tween.StopAll(false);
					(base.ScreenManager as RCScreenManager).DisplayScreen(3, true, null);
				}
				else
				{
					this.m_displayingContinueText = true;
					Tween.StopAllContaining(this.m_continueText, false);
					Tween.To(this.m_continueText, 0.5f, new Easing(Tween.EaseNone), new string[]
					{
						"Opacity",
						"1"
					});
					Tween.RunFunction(4f, this, "HideContinueText", new object[0]);
				}
			}
			base.HandleInput();
		}
		public void HideContinueText()
		{
			this.m_displayingContinueText = false;
			Tween.To(this.m_continueText, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0",
				"Opacity",
				"0"
			});
		}
		private void UpdateBackground(float elapsedTime)
		{
			int num = 200;
			this.m_bg1.X += (float)num * elapsedTime;
			this.m_bg2.X += (float)num * elapsedTime;
			this.m_bg3.X += (float)num * elapsedTime;
			if (this.m_bg1.X > 930f)
			{
				this.m_bg1.X = this.m_bg3.X - (float)this.m_bg3.Width;
			}
			if (this.m_bg2.X > 930f)
			{
				this.m_bg2.X = this.m_bg1.X - (float)this.m_bg1.Width;
			}
			if (this.m_bg3.X > 930f)
			{
				this.m_bg3.X = this.m_bg2.X - (float)this.m_bg2.Width;
			}
			this.m_ground1.X += (float)num * elapsedTime;
			this.m_ground2.X += (float)num * elapsedTime;
			this.m_ground3.X += (float)num * elapsedTime;
			if (this.m_ground1.X > 930f)
			{
				this.m_ground1.X = this.m_ground3.X - (float)this.m_ground3.Width;
			}
			if (this.m_ground2.X > 930f)
			{
				this.m_ground2.X = this.m_ground1.X - (float)this.m_ground1.Width;
			}
			if (this.m_ground3.X > 930f)
			{
				this.m_ground3.X = this.m_ground2.X - (float)this.m_ground2.Width;
			}
			this.m_border1.X += (float)num * elapsedTime;
			this.m_border2.X += (float)num * elapsedTime;
			this.m_border3.X += (float)num * elapsedTime;
			if (this.m_border1.X > 930f)
			{
				this.m_border1.X = this.m_border3.X - (float)this.m_border3.Width;
			}
			if (this.m_border2.X > 930f)
			{
				this.m_border2.X = this.m_border1.X - (float)this.m_border1.Width;
			}
			if (this.m_border3.X > 930f)
			{
				this.m_border3.X = this.m_border2.X - (float)this.m_border2.Width;
			}
			this.m_prop1.X += (float)num * elapsedTime;
			this.m_prop2.X += (float)num * elapsedTime;
			this.m_prop3.X += (float)num * elapsedTime;
			if (this.m_prop1.X > 930f)
			{
				this.m_prop1.X -= (float)CDGMath.RandomInt(1000, 3000);
			}
			if (this.m_prop2.X > 930f)
			{
				this.m_prop2.X -= (float)CDGMath.RandomInt(1000, 3000);
			}
			if (this.m_prop3.X > 930f)
			{
				this.m_prop3.X -= (float)CDGMath.RandomInt(1000, 3000);
			}
		}
		public void SwapBackground(string levelType)
		{
			Tween.By(this.m_sideBorderLeft, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"200"
			});
			Tween.By(this.m_sideBorderRight, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"-200"
			});
			Tween.AddEndHandlerToLastTween(this, "PerformSwap", new object[]
			{
				levelType
			});
		}
		public void PerformSwap(string levelType)
		{
			this.m_manor.Y = 0f;
			this.m_bgOutside.Y = 0f;
			this.m_bgOutside.Visible = false;
			this.m_manor.Visible = false;
			this.m_ground1.Visible = (this.m_ground2.Visible = (this.m_ground3.Visible = true));
			this.m_border1.Visible = (this.m_border2.Visible = (this.m_border3.Visible = true));
			this.m_prop1.Visible = (this.m_prop2.Visible = (this.m_prop3.Visible = true));
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
										this.m_bgOutside.Visible = true;
										this.m_manor.Visible = true;
										this.m_ground1.Visible = (this.m_ground2.Visible = (this.m_ground3.Visible = false));
										this.m_border1.Visible = (this.m_border2.Visible = (this.m_border3.Visible = false));
										this.m_manor.Y = -260f;
										this.m_bgOutside.Y = -260f;
										this.m_manor.X -= 300f;
										this.m_bgOutside.X -= 300f;
										this.m_prop1.Visible = false;
										this.m_prop2.Visible = false;
										this.m_prop3.Visible = false;
										Tween.By(this.m_manor, 3f, new Easing(Tween.EaseNone), new string[]
										{
											"X",
											"300"
										});
										Tween.By(this.m_bgOutside, 3f, new Easing(Tween.EaseNone), new string[]
										{
											"X",
											"300"
										});
										Tween.By(this.m_playerSprite, 3.5f, new Easing(Tween.EaseNone), new string[]
										{
											"X",
											"-150"
										});
										Tween.AddEndHandlerToLastTween(this, "CreditsComplete", new object[0]);
										Tween.By(this.m_sideBorderTop, 2.5f, new Easing(Tween.EaseNone), new string[]
										{
											"Y",
											"-500"
										});
									}
								}
								else
								{
									this.m_bg1.ChangeSprite("GardenBG_Sprite");
									this.m_bg2.ChangeSprite("GardenBG_Sprite");
									this.m_bg3.ChangeSprite("GardenBG_Sprite");
									this.m_ground1.ChangeSprite("GardenFG_Sprite");
									this.m_ground2.ChangeSprite("GardenFG_Sprite");
									this.m_ground3.ChangeSprite("GardenFG_Sprite");
									this.m_border1.ChangeSprite("StartingRoomFloor_Sprite");
									this.m_border2.ChangeSprite("StartingRoomFloor_Sprite");
									this.m_border3.ChangeSprite("StartingRoomFloor_Sprite");
									this.m_prop1.ChangeSprite("DungeonPrison1_Sprite");
									this.m_prop1.Visible = false;
									this.m_prop2.ChangeSprite("CreditsGrass_Character");
									this.m_prop2.Y = 440f;
									this.m_prop3.ChangeSprite("CreditsTree_Character");
									this.m_bgOutside.Visible = true;
								}
							}
							else
							{
								this.m_bg1.ChangeSprite("GardenBG_Sprite");
								this.m_bg2.ChangeSprite("GardenBG_Sprite");
								this.m_bg3.ChangeSprite("GardenBG_Sprite");
								this.m_ground1.ChangeSprite("GardenFG_Sprite");
								this.m_ground2.ChangeSprite("GardenFG_Sprite");
								this.m_ground3.ChangeSprite("GardenFG_Sprite");
								this.m_border1.ChangeSprite("GardenBorder_Sprite");
								this.m_border2.ChangeSprite("GardenBorder_Sprite");
								this.m_border3.ChangeSprite("GardenBorder_Sprite");
								this.m_prop1.ChangeSprite("GardenFloatingRock3_Sprite");
								this.m_prop2.ChangeSprite("GardenFairy_Character");
								this.m_prop2.PlayAnimation(true);
								this.m_prop2.AnimationDelay = 0.1f;
								this.m_prop3.ChangeSprite("GardenLampPost1_Character");
							}
						}
						else
						{
							this.m_bg1.ChangeSprite("DungeonBG1_Sprite");
							this.m_bg2.ChangeSprite("DungeonBG1_Sprite");
							this.m_bg3.ChangeSprite("DungeonBG1_Sprite");
							this.m_ground1.ChangeSprite("DungeonFG1_Sprite");
							this.m_ground2.ChangeSprite("DungeonFG1_Sprite");
							this.m_ground3.ChangeSprite("DungeonFG1_Sprite");
							this.m_border1.ChangeSprite("DungeonBorder_Sprite");
							this.m_border2.ChangeSprite("DungeonBorder_Sprite");
							this.m_border3.ChangeSprite("DungeonBorder_Sprite");
							this.m_prop1.ChangeSprite("DungeonPrison1_Sprite");
							this.m_prop2.ChangeSprite("DungeonChain2_Character");
							this.m_prop3.ChangeSprite("DungeonTorch2_Character");
						}
					}
					else
					{
						this.m_bg1.ChangeSprite("TowerBG2_Sprite");
						this.m_bg2.ChangeSprite("TowerBG2_Sprite");
						this.m_bg3.ChangeSprite("TowerBG2_Sprite");
						this.m_ground1.ChangeSprite("TowerFG2_Sprite");
						this.m_ground2.ChangeSprite("TowerFG2_Sprite");
						this.m_ground3.ChangeSprite("TowerFG2_Sprite");
						this.m_border1.ChangeSprite("TowerBorder2_Sprite");
						this.m_border2.ChangeSprite("TowerBorder2_Sprite");
						this.m_border3.ChangeSprite("TowerBorder2_Sprite");
						this.m_prop1.ChangeSprite("TowerHole4_Sprite");
						this.m_prop2.Visible = false;
						this.m_prop3.ChangeSprite("TowerPedestal2_Character");
					}
				}
				else
				{
					this.m_bg1.ChangeSprite("CastleBG1_Sprite");
					this.m_bg2.ChangeSprite("CastleBG1_Sprite");
					this.m_bg3.ChangeSprite("CastleBG1_Sprite");
					this.m_ground1.ChangeSprite("CastleFG1_Sprite");
					this.m_ground2.ChangeSprite("CastleFG1_Sprite");
					this.m_ground3.ChangeSprite("CastleFG1_Sprite");
					this.m_border1.ChangeSprite("CastleBorder_Sprite");
					this.m_border2.ChangeSprite("CastleBorder_Sprite");
					this.m_border3.ChangeSprite("CastleBorder_Sprite");
					this.m_prop1.ChangeSprite("CastleAssetWindow1_Sprite");
					this.m_prop2.ChangeSprite("CastleAssetBackTorch_Character");
					this.m_prop2.PlayAnimation(true);
					this.m_prop2.AnimationDelay = 0.1f;
					this.m_prop3.ChangeSprite("CastleAssetCandle1_Character");
				}
			}
			if (levelType != "Manor")
			{
				Tween.By(this.m_sideBorderLeft, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					"-200"
				});
				Tween.By(this.m_sideBorderRight, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					"200"
				});
				return;
			}
			Tween.By(this.m_sideBorderLeft, 3f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"-800"
			});
			Tween.By(this.m_sideBorderRight, 3f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"800"
			});
		}
		public void CreditsComplete()
		{
			this.SetPlayerStyle("Idle");
			Tween.RunFunction(0.5f, this, "SetPlayerStyle", new object[]
			{
				"LevelUp"
			});
			Tween.RunFunction(0.6f, this.m_playerSprite, "PlayAnimation", new object[]
			{
				false
			});
			Tween.To(this.m_thanksForPlayingText, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
			Tween.To(this.m_totalDeaths, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.2",
				"Opacity",
				"1"
			});
			Tween.To(this.m_totalPlayTime, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.4",
				"Opacity",
				"1"
			});
			Tween.RunFunction(1f, this, "BringWife", new object[0]);
			Tween.RunFunction(1.1f, this, "BringChild1", new object[0]);
			Tween.RunFunction(3f, this, "BringChild2", new object[0]);
		}
		public void BringWife()
		{
			this.m_wifeSprite.GetChildAt(14).Visible = false;
			this.m_wifeSprite.GetChildAt(15).Visible = false;
			this.m_wifeSprite.GetChildAt(16).Visible = false;
			this.m_wifeSprite.GetChildAt(0).Visible = false;
			this.m_wifeChest = CDGMath.RandomInt(1, 5);
			this.m_wifeHead = CDGMath.RandomInt(1, 5);
			this.m_wifeShoulders = CDGMath.RandomInt(1, 5);
			this.m_wifeSprite.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + this.m_wifeChest + "_Sprite");
			this.m_wifeSprite.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + this.m_wifeHead + "_Sprite");
			this.m_wifeSprite.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + this.m_wifeShoulders + "_Sprite");
			this.m_wifeSprite.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + this.m_wifeShoulders + "_Sprite");
			if ((!Game.PlayerStats.IsFemale && Game.PlayerStats.Traits.X != 2f && Game.PlayerStats.Traits.Y != 2f) || (Game.PlayerStats.IsFemale && (Game.PlayerStats.Traits.X == 2f || Game.PlayerStats.Traits.Y == 2f)))
			{
				this.m_wifeSprite.GetChildAt(13).Visible = true;
				this.m_wifeSprite.GetChildAt(5).Visible = true;
			}
			else
			{
				this.m_wifeSprite.GetChildAt(13).Visible = false;
				this.m_wifeSprite.GetChildAt(5).Visible = false;
			}
			this.m_wifeSprite.PlayAnimation(true);
			Tween.By(this.m_wifeSprite, 3f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"600"
			});
			Tween.AddEndHandlerToLastTween(this, "LevelUpWife", new object[0]);
		}
		public void LevelUpWife()
		{
			this.m_wifeSprite.ChangeSprite("PlayerLevelUp_Character");
			this.m_wifeSprite.GetChildAt(14).Visible = false;
			this.m_wifeSprite.GetChildAt(15).Visible = false;
			this.m_wifeSprite.GetChildAt(16).Visible = false;
			this.m_wifeSprite.GetChildAt(0).Visible = false;
			if ((!Game.PlayerStats.IsFemale && Game.PlayerStats.Traits.X != 2f && Game.PlayerStats.Traits.Y != 2f) || (Game.PlayerStats.IsFemale && (Game.PlayerStats.Traits.X == 2f || Game.PlayerStats.Traits.Y == 2f)))
			{
				this.m_wifeSprite.GetChildAt(13).Visible = true;
				this.m_wifeSprite.GetChildAt(5).Visible = true;
			}
			else
			{
				this.m_wifeSprite.GetChildAt(13).Visible = false;
				this.m_wifeSprite.GetChildAt(5).Visible = false;
			}
			this.m_wifeSprite.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + this.m_wifeChest + "_Sprite");
			this.m_wifeSprite.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + this.m_wifeHead + "_Sprite");
			this.m_wifeSprite.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + this.m_wifeShoulders + "_Sprite");
			this.m_wifeSprite.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + this.m_wifeShoulders + "_Sprite");
			this.m_wifeSprite.PlayAnimation(false);
		}
		public void BringChild1()
		{
			this.m_childSprite1.GetChildAt(14).Visible = false;
			this.m_childSprite1.GetChildAt(15).Visible = false;
			this.m_childSprite1.GetChildAt(16).Visible = false;
			this.m_childSprite1.GetChildAt(0).Visible = false;
			this.m_child1Chest = CDGMath.RandomInt(1, 5);
			this.m_child1Head = CDGMath.RandomInt(1, 5);
			this.m_child1Shoulders = CDGMath.RandomInt(1, 5);
			bool flag = false;
			if (CDGMath.RandomInt(0, 1) > 0)
			{
				flag = true;
			}
			if (flag)
			{
				this.m_childSprite1.GetChildAt(13).Visible = true;
				this.m_childSprite1.GetChildAt(5).Visible = true;
			}
			else
			{
				this.m_childSprite1.GetChildAt(13).Visible = false;
				this.m_childSprite1.GetChildAt(5).Visible = false;
			}
			this.m_childSprite1.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + this.m_child1Chest + "_Sprite");
			this.m_childSprite1.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + this.m_child1Head + "_Sprite");
			this.m_childSprite1.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + this.m_child1Shoulders + "_Sprite");
			this.m_childSprite1.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + this.m_child1Shoulders + "_Sprite");
			this.m_childSprite1.PlayAnimation(true);
			Tween.By(this.m_childSprite1, 3f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"600"
			});
			Tween.AddEndHandlerToLastTween(this, "LevelUpChild1", new object[]
			{
				flag
			});
		}
		public void LevelUpChild1(bool isFemale)
		{
			this.m_childSprite1.ChangeSprite("PlayerLevelUp_Character");
			this.m_childSprite1.GetChildAt(14).Visible = false;
			this.m_childSprite1.GetChildAt(15).Visible = false;
			this.m_childSprite1.GetChildAt(16).Visible = false;
			this.m_childSprite1.GetChildAt(0).Visible = false;
			if (isFemale)
			{
				this.m_childSprite1.GetChildAt(13).Visible = true;
				this.m_childSprite1.GetChildAt(5).Visible = true;
			}
			else
			{
				this.m_childSprite1.GetChildAt(13).Visible = false;
				this.m_childSprite1.GetChildAt(5).Visible = false;
			}
			this.m_childSprite1.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + this.m_child1Chest + "_Sprite");
			this.m_childSprite1.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + this.m_child1Head + "_Sprite");
			this.m_childSprite1.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + this.m_child1Shoulders + "_Sprite");
			this.m_childSprite1.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + this.m_child1Shoulders + "_Sprite");
			this.m_childSprite1.PlayAnimation(false);
		}
		public void BringChild2()
		{
			this.m_childSprite2.GetChildAt(14).Visible = false;
			this.m_childSprite2.GetChildAt(15).Visible = false;
			this.m_childSprite2.GetChildAt(16).Visible = false;
			this.m_childSprite2.GetChildAt(0).Visible = false;
			bool flag = false;
			if (CDGMath.RandomInt(0, 1) > 0)
			{
				flag = true;
			}
			if (flag)
			{
				this.m_childSprite2.GetChildAt(13).Visible = true;
				this.m_childSprite2.GetChildAt(5).Visible = true;
			}
			else
			{
				this.m_childSprite2.GetChildAt(13).Visible = false;
				this.m_childSprite2.GetChildAt(5).Visible = false;
			}
			this.m_child2Chest = CDGMath.RandomInt(1, 5);
			this.m_child2Head = CDGMath.RandomInt(1, 5);
			this.m_child2Shoulders = CDGMath.RandomInt(1, 5);
			this.m_childSprite2.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + this.m_child2Chest + "_Sprite");
			this.m_childSprite2.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + this.m_child2Head + "_Sprite");
			this.m_childSprite2.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + this.m_child2Shoulders + "_Sprite");
			this.m_childSprite2.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + this.m_child2Shoulders + "_Sprite");
			this.m_childSprite2.PlayAnimation(true);
			Tween.By(this.m_childSprite2, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"X",
				"600"
			});
			Tween.AddEndHandlerToLastTween(this, "LevelUpChild2", new object[]
			{
				flag
			});
		}
		public void LevelUpChild2(bool isFemale)
		{
			this.m_childSprite2.ChangeSprite("PlayerLevelUp_Character");
			this.m_childSprite2.GetChildAt(14).Visible = false;
			this.m_childSprite2.GetChildAt(15).Visible = false;
			this.m_childSprite2.GetChildAt(16).Visible = false;
			this.m_childSprite2.GetChildAt(0).Visible = false;
			if (isFemale)
			{
				this.m_childSprite2.GetChildAt(13).Visible = true;
				this.m_childSprite2.GetChildAt(5).Visible = true;
			}
			else
			{
				this.m_childSprite2.GetChildAt(13).Visible = false;
				this.m_childSprite2.GetChildAt(5).Visible = false;
			}
			this.m_childSprite2.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + this.m_child2Chest + "_Sprite");
			this.m_childSprite2.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + this.m_child2Head + "_Sprite");
			this.m_childSprite2.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + this.m_child2Shoulders + "_Sprite");
			this.m_childSprite2.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + this.m_child2Shoulders + "_Sprite");
			this.m_childSprite2.PlayAnimation(false);
			this.m_allowExit = true;
			this.m_displayingContinueText = true;
			Tween.StopAllContaining(this.m_continueText, false);
			Tween.To(this.m_continueText, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"1"
			});
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_skyRenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
			this.m_sky.Draw(base.Camera);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_backgroundRenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			this.m_bg1.Draw(base.Camera);
			this.m_bg2.Draw(base.Camera);
			this.m_bg3.Draw(base.Camera);
			this.m_bgOutside.Draw(base.Camera);
			this.m_ground1.Draw(base.Camera);
			this.m_ground2.Draw(base.Camera);
			this.m_ground3.Draw(base.Camera);
			this.m_border1.Draw(base.Camera);
			this.m_border2.Draw(base.Camera);
			this.m_border3.Draw(base.Camera);
			this.m_manor.Draw(base.Camera);
			this.m_prop1.Draw(base.Camera);
			this.m_prop2.Draw(base.Camera);
			this.m_prop3.Draw(base.Camera);
			this.m_playerSprite.Draw(base.Camera);
			Game.ColourSwapShader.Parameters["desiredTint"].SetValue(this.m_playerSprite.GetChildAt(12).TextureColor.ToVector4());
			if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
			{
				Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
				Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_lichColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_lichColour2.ToVector4());
			}
			else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
			{
				Game.ColourSwapShader.Parameters["Opacity"].SetValue(this.m_playerSprite.Opacity);
				Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
			}
			else
			{
				Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
				Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(this.m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(this.m_skinColour1.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(this.m_skinColour2.ToVector4());
				Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(this.m_skinColour2.ToVector4());
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader);
			this.m_playerSprite.GetChildAt(12).Draw(base.Camera);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			if (Game.PlayerStats.IsFemale)
			{
				this.m_playerSprite.GetChildAt(13).Draw(base.Camera);
			}
			this.m_playerSprite.GetChildAt(15).Draw(base.Camera);
			this.m_wifeSprite.Draw(base.Camera);
			this.m_childSprite1.Draw(base.Camera);
			this.m_childSprite2.Draw(base.Camera);
			this.m_sideBorderLeft.Draw(base.Camera);
			this.m_sideBorderRight.Draw(base.Camera);
			this.m_sideBorderTop.Draw(base.Camera);
			this.m_sideBorderBottom.Draw(base.Camera);
			this.m_teddy.Draw(base.Camera);
			this.m_kenny.Draw(base.Camera);
			this.m_glauber.Draw(base.Camera);
			this.m_gordon.Draw(base.Camera);
			this.m_judson.Draw(base.Camera);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
			base.Camera.GraphicsDevice.Textures[1] = this.m_skyRenderTarget;
			base.Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ParallaxEffect);
			base.Camera.Draw(this.m_backgroundRenderTarget, Vector2.Zero, Color.White);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			Rectangle b = new Rectangle(0, 0, 1320, 720);
			foreach (TextObj current in this.m_creditsTitleList)
			{
				if (CollisionMath.Intersects(current.Bounds, b))
				{
					current.Draw(base.Camera);
				}
			}
			foreach (TextObj current2 in this.m_creditsNameList)
			{
				if (CollisionMath.Intersects(current2.Bounds, b))
				{
					current2.Draw(base.Camera);
				}
			}
			this.m_thanksForPlayingText.Draw(base.Camera);
			this.m_totalDeaths.Draw(base.Camera);
			this.m_totalPlayTime.Draw(base.Camera);
			this.m_continueText.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Credits Screen");
				Array.Clear(this.m_backgroundStrings, 0, this.m_backgroundStrings.Length);
				this.m_backgroundStrings = null;
				this.m_playerSprite.Dispose();
				this.m_playerSprite = null;
				this.m_wifeSprite.Dispose();
				this.m_wifeSprite = null;
				this.m_childSprite1.Dispose();
				this.m_childSprite1 = null;
				this.m_childSprite2.Dispose();
				this.m_childSprite2 = null;
				this.m_manor.Dispose();
				this.m_manor = null;
				this.m_thanksForPlayingText.Dispose();
				this.m_thanksForPlayingText = null;
				this.m_sideBorderRight.Dispose();
				this.m_sideBorderRight = null;
				this.m_sideBorderLeft.Dispose();
				this.m_sideBorderLeft = null;
				this.m_sideBorderTop.Dispose();
				this.m_sideBorderTop = null;
				this.m_sideBorderBottom.Dispose();
				this.m_sideBorderBottom = null;
				this.m_bgOutside.Dispose();
				this.m_bgOutside = null;
				this.m_sky.Dispose();
				this.m_sky = null;
				this.m_skyRenderTarget.Dispose();
				this.m_skyRenderTarget = null;
				this.m_backgroundRenderTarget.Dispose();
				this.m_backgroundRenderTarget = null;
				foreach (TextObj current in this.m_creditsTitleList)
				{
					current.Dispose();
				}
				this.m_creditsTitleList.Clear();
				this.m_creditsTitleList = null;
				foreach (TextObj current2 in this.m_creditsNameList)
				{
					current2.Dispose();
				}
				this.m_creditsNameList.Clear();
				this.m_creditsNameList = null;
				this.m_bg1.Dispose();
				this.m_bg2.Dispose();
				this.m_bg3.Dispose();
				this.m_ground1.Dispose();
				this.m_ground2.Dispose();
				this.m_ground3.Dispose();
				this.m_border1.Dispose();
				this.m_border2.Dispose();
				this.m_border3.Dispose();
				this.m_prop1.Dispose();
				this.m_prop2.Dispose();
				this.m_prop3.Dispose();
				this.m_prop1 = null;
				this.m_prop2 = null;
				this.m_prop3 = null;
				this.m_bg1 = null;
				this.m_bg2 = null;
				this.m_bg3 = null;
				this.m_ground1 = null;
				this.m_ground2 = null;
				this.m_ground3 = null;
				this.m_border1 = null;
				this.m_border2 = null;
				this.m_border3 = null;
				this.m_teddy.Dispose();
				this.m_kenny.Dispose();
				this.m_glauber.Dispose();
				this.m_gordon.Dispose();
				this.m_judson.Dispose();
				this.m_teddy = null;
				this.m_kenny = null;
				this.m_glauber = null;
				this.m_gordon = null;
				this.m_judson = null;
				this.m_continueText.Dispose();
				this.m_continueText = null;
				this.m_totalDeaths.Dispose();
				this.m_totalDeaths = null;
				this.m_totalPlayTime.Dispose();
				this.m_totalPlayTime = null;
				base.Dispose();
			}
		}
	}
}
