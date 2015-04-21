using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
namespace RogueCastle
{
	public class TutorialRoomObj : RoomObj
	{
		private KeyIconTextObj m_tutorialText;
		private int m_waypointIndex;
		private List<GameObj> m_waypointList;
		private string[] m_tutorialTextList;
		private string[] m_tutorialControllerTextList;
		private TextObj m_creditsText;
		private TextObj m_creditsTitleText;
		private string[] m_creditsTextList;
		private string[] m_creditsTextTitleList;
		private Vector2 m_creditsPosition;
		private int m_creditsIndex;
		private SpriteObj m_diary;
		private SpriteObj m_doorSprite;
		private DoorObj m_door;
		private SpriteObj m_speechBubble;
		public TutorialRoomObj()
		{
			this.m_waypointList = new List<GameObj>();
		}
		public override void Initialize()
		{
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "diary")
				{
					this.m_diary = (current as SpriteObj);
				}
				if (current.Name == "doorsprite")
				{
					this.m_doorSprite = (current as SpriteObj);
				}
			}
			this.m_door = base.DoorList[0];
			this.m_speechBubble = new SpriteObj("ExclamationSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			this.m_speechBubble.Scale = new Vector2(1.2f, 1.2f);
			base.GameObjList.Add(this.m_speechBubble);
			this.m_diary.OutlineWidth = 2;
			this.m_speechBubble.Position = new Vector2(this.m_diary.X, this.m_diary.Y - (float)this.m_speechBubble.Height - 20f);
			this.m_tutorialTextList = new string[]
			{
				"Tap [Input:" + 11 + "] to Jump",
				"Hold [Input:" + 11 + "] to Jump Higher",
				"Tap [Input:" + 12 + "] to Attack",
				string.Concat(new object[]
				{
					"Hold [Input:",
					19,
					"] and Tap [Input:",
					11,
					"] to Drop Ledges"
				}),
				string.Concat(new object[]
				{
					"(Air) Hold [Input:",
					19,
					"] and Tap [Input:",
					12,
					"] to Attack Down"
				})
			};
			this.m_tutorialControllerTextList = new string[]
			{
				"Tap [Input:" + 10 + "] to Jump",
				"Hold [Input:" + 10 + "] to Jump Higher",
				"Tap [Input:" + 12 + "] to Attack",
				string.Concat(new object[]
				{
					"Hold [Input:",
					18,
					"] and Tap [Input:",
					10,
					"] to Drop Ledges"
				}),
				string.Concat(new object[]
				{
					"(Air) Hold [Input:",
					18,
					"] and Tap [Input:",
					12,
					"] to Attack Down"
				})
			};
			this.m_creditsTextTitleList = new string[]
			{
				"Developed by",
				"Design",
				"Programming",
				"Art",
				"Sound Design & Music",
				"Music",
				""
			};
			this.m_creditsTextList = new string[]
			{
				"Cellar Door Games",
				"Teddy Lee",
				"Kenny Lee",
				"Glauber Kotaki",
				"Gordon McGladdery",
				"Judson Cowan",
				"Rogue Legacy"
			};
			this.m_creditsPosition = new Vector2(50f, 580f);
			foreach (GameObj current2 in base.GameObjList)
			{
				if (current2.Name == "waypoint1")
				{
					this.m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint2")
				{
					this.m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint3")
				{
					this.m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint4")
				{
					this.m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint5")
				{
					this.m_waypointList.Add(current2);
				}
			}
			base.Initialize();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			this.m_tutorialText = new KeyIconTextObj(Game.JunicodeLargeFont);
			this.m_tutorialText.FontSize = 28f;
			this.m_tutorialText.Text = "[Input:" + 10 + "] to Jump";
			this.m_tutorialText.Align = Types.TextAlign.Centre;
			this.m_tutorialText.OutlineWidth = 2;
			this.m_tutorialText.ForcedScale = new Vector2(0.8f, 0.8f);
			this.m_creditsText = new TextObj(Game.JunicodeFont);
			this.m_creditsText.FontSize = 20f;
			this.m_creditsText.Text = "Cellar Door Games";
			this.m_creditsText.DropShadow = new Vector2(2f, 2f);
			this.m_creditsTitleText = (this.m_creditsText.Clone() as TextObj);
			this.m_creditsTitleText.FontSize = 14f;
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 12f;
			textObj.Text = "Down Attack this";
			textObj.OutlineWidth = 2;
			textObj.Align = Types.TextAlign.Centre;
			textObj.Position = this.m_waypointList[this.m_waypointList.Count - 1].Position;
			textObj.X -= 25f;
			textObj.Y -= 70f;
			base.GameObjList.Add(textObj);
			base.LoadContent(graphics);
		}
		public override void OnEnter()
		{
			this.m_speechBubble.Visible = false;
			this.m_diary.Visible = false;
			this.m_doorSprite.ChangeSprite("CastleDoorOpen_Sprite");
			if (Game.PlayerStats.TutorialComplete)
			{
				if (!Game.PlayerStats.ReadLastDiary)
				{
					this.m_door.Locked = true;
					this.m_doorSprite.ChangeSprite("CastleDoor_Sprite");
				}
				else
				{
					this.m_door.Locked = false;
				}
				this.m_diary.Visible = true;
				this.Player.UpdateCollisionBoxes();
				this.Player.Position = new Vector2(base.X + 240f + (float)this.Player.Width, (float)(this.Bounds.Bottom - 120) - ((float)this.Player.Bounds.Bottom - this.Player.Y));
			}
			this.m_creditsTitleText.Opacity = 0f;
			this.m_creditsText.Opacity = 0f;
			foreach (EnemyObj current in base.EnemyList)
			{
				current.Damage = 0;
			}
			this.m_tutorialText.Opacity = 0f;
			this.Player.UnlockControls();
			if (!Game.PlayerStats.TutorialComplete)
			{
				SoundManager.PlayMusic("EndSong", true, 4f);
			}
			else
			{
				SoundManager.StopMusic(4f);
			}
			Tween.RunFunction(2f, this.Player.AttachedLevel, "DisplayCreditsText", new object[]
			{
				true
			});
			base.OnEnter();
		}
		public void DisplayCreditsText()
		{
			if (this.m_creditsIndex < this.m_creditsTextList.Length)
			{
				this.m_creditsTitleText.Opacity = 0f;
				this.m_creditsText.Opacity = 0f;
				this.m_creditsTitleText.Text = this.m_creditsTextTitleList[this.m_creditsIndex];
				this.m_creditsText.Text = this.m_creditsTextList[this.m_creditsIndex];
				Tween.To(this.m_creditsTitleText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.To(this.m_creditsText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"1"
				});
				this.m_creditsTitleText.Opacity = 1f;
				this.m_creditsText.Opacity = 1f;
				Tween.To(this.m_creditsTitleText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"4",
					"Opacity",
					"0"
				});
				Tween.To(this.m_creditsText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"4.2",
					"Opacity",
					"0"
				});
				this.m_creditsTitleText.Opacity = 0f;
				this.m_creditsText.Opacity = 0f;
				this.m_creditsIndex++;
				Tween.RunFunction(8f, this, "DisplayCreditsText", new object[0]);
			}
		}
		private int PlayerNearWaypoint()
		{
			for (int i = 0; i < this.m_waypointList.Count; i++)
			{
				if (CDGMath.DistanceBetweenPts(this.Player.Position, this.m_waypointList[i].Position) < 500f)
				{
					return i;
				}
			}
			return -1;
		}
		public override void Update(GameTime gameTime)
		{
			if (!Game.PlayerStats.TutorialComplete)
			{
				int waypointIndex = this.m_waypointIndex;
				this.m_waypointIndex = this.PlayerNearWaypoint();
				if (this.m_waypointIndex != waypointIndex)
				{
					Tween.StopAllContaining(this.m_tutorialText, false);
					if (this.m_waypointIndex != -1)
					{
						if (!InputManager.GamePadIsConnected(PlayerIndex.One))
						{
							this.m_tutorialText.Text = this.m_tutorialTextList[this.m_waypointIndex];
						}
						else
						{
							this.m_tutorialText.Text = this.m_tutorialControllerTextList[this.m_waypointIndex];
						}
						Tween.To(this.m_tutorialText, 0.25f, new Easing(Tween.EaseNone), new string[]
						{
							"Opacity",
							"1"
						});
					}
					else
					{
						Tween.To(this.m_tutorialText, 0.25f, new Easing(Tween.EaseNone), new string[]
						{
							"Opacity",
							"0"
						});
					}
				}
			}
			else
			{
				Rectangle bounds = this.m_diary.Bounds;
				bounds.X -= 50;
				bounds.Width += 100;
				this.m_speechBubble.Y = this.m_diary.Y - (float)this.m_speechBubble.Height - 20f - 30f + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
				if (CollisionMath.Intersects(this.Player.Bounds, bounds) && this.Player.IsTouchingGround)
				{
					if (this.m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
					{
						this.m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");
					}
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						if (!Game.PlayerStats.ReadLastDiary)
						{
							RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
							rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + 24);
							rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "RunFlashback", new object[0]);
							rCScreenManager.DisplayScreen(13, true, null);
						}
						else
						{
							RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
							rCScreenManager2.DisplayScreen(20, true, null);
						}
					}
				}
				else if (this.m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
				{
					this.m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
				}
				if (!Game.PlayerStats.ReadLastDiary || CollisionMath.Intersects(this.Player.Bounds, bounds))
				{
					this.m_speechBubble.Visible = true;
				}
				else if (Game.PlayerStats.ReadLastDiary && !CollisionMath.Intersects(this.Player.Bounds, bounds))
				{
					this.m_speechBubble.Visible = false;
				}
			}
			base.Update(gameTime);
		}
		public void RunFlashback()
		{
			this.Player.LockControls();
			(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(25, true, null);
			Tween.RunFunction(0.5f, this, "OpenDoor", new object[0]);
		}
		public void OpenDoor()
		{
			this.Player.UnlockControls();
			this.m_doorSprite.ChangeSprite("CastleDoorOpen_Sprite");
			this.m_door.Locked = false;
			Game.PlayerStats.ReadLastDiary = true;
			Game.PlayerStats.DiaryEntry = 25;
			(this.Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
		}
		public override void Draw(Camera2D camera)
		{
			Vector2 topLeftCorner = Game.ScreenManager.Camera.TopLeftCorner;
			this.m_creditsTitleText.Position = new Vector2(topLeftCorner.X + this.m_creditsPosition.X, topLeftCorner.Y + this.m_creditsPosition.Y);
			this.m_creditsText.Position = this.m_creditsTitleText.Position;
			this.m_creditsText.Y += 35f;
			this.m_creditsTitleText.X += 5f;
			base.Draw(camera);
			this.m_tutorialText.Position = Game.ScreenManager.Camera.Position;
			this.m_tutorialText.Y -= 200f;
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_tutorialText.Draw(camera);
			this.m_creditsText.Draw(camera);
			this.m_creditsTitleText.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_tutorialText.Dispose();
				this.m_tutorialText = null;
				this.m_waypointList.Clear();
				this.m_waypointList = null;
				this.m_creditsText.Dispose();
				this.m_creditsText = null;
				this.m_creditsTitleText.Dispose();
				this.m_creditsTitleText = null;
				Array.Clear(this.m_tutorialTextList, 0, this.m_tutorialTextList.Length);
				Array.Clear(this.m_tutorialControllerTextList, 0, this.m_tutorialControllerTextList.Length);
				Array.Clear(this.m_creditsTextTitleList, 0, this.m_creditsTextTitleList.Length);
				Array.Clear(this.m_creditsTextList, 0, this.m_creditsTextList.Length);
				this.m_tutorialTextList = null;
				this.m_creditsTextTitleList = null;
				this.m_creditsTextList = null;
				this.m_tutorialControllerTextList = null;
				this.m_door = null;
				this.m_doorSprite = null;
				this.m_diary = null;
				this.m_speechBubble = null;
				base.Dispose();
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new TutorialRoomObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
	}
}
