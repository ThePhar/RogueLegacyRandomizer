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
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			m_waypointList = new List<GameObj>();
		}
		public override void Initialize()
		{
			foreach (GameObj current in GameObjList)
			{
				if (current.Name == "diary")
				{
					m_diary = (current as SpriteObj);
				}
				if (current.Name == "doorsprite")
				{
					m_doorSprite = (current as SpriteObj);
				}
			}
			m_door = DoorList[0];
			m_speechBubble = new SpriteObj("ExclamationSquare_Sprite");
			m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			m_speechBubble.Scale = new Vector2(1.2f, 1.2f);
			GameObjList.Add(m_speechBubble);
			m_diary.OutlineWidth = 2;
			m_speechBubble.Position = new Vector2(m_diary.X, m_diary.Y - m_speechBubble.Height - 20f);
			m_tutorialTextList = new string[]
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
			m_tutorialControllerTextList = new string[]
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
			m_creditsTextTitleList = new string[]
			{
				"Developed by",
				"Design",
				"Programming",
				"Art",
				"Sound Design & Music",
				"Music",
				""
			};
			m_creditsTextList = new string[]
			{
				"Cellar Door Games",
				"Teddy Lee",
				"Kenny Lee",
				"Glauber Kotaki",
				"Gordon McGladdery",
				"Judson Cowan",
				"Rogue Legacy"
			};
			m_creditsPosition = new Vector2(50f, 580f);
			foreach (GameObj current2 in GameObjList)
			{
				if (current2.Name == "waypoint1")
				{
					m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint2")
				{
					m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint3")
				{
					m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint4")
				{
					m_waypointList.Add(current2);
				}
				if (current2.Name == "waypoint5")
				{
					m_waypointList.Add(current2);
				}
			}
			base.Initialize();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			m_tutorialText = new KeyIconTextObj(Game.JunicodeLargeFont);
			m_tutorialText.FontSize = 28f;
			m_tutorialText.Text = "[Input:" + 10 + "] to Jump";
			m_tutorialText.Align = Types.TextAlign.Centre;
			m_tutorialText.OutlineWidth = 2;
			m_tutorialText.ForcedScale = new Vector2(0.8f, 0.8f);
			m_creditsText = new TextObj(Game.JunicodeFont);
			m_creditsText.FontSize = 20f;
			m_creditsText.Text = "Cellar Door Games";
			m_creditsText.DropShadow = new Vector2(2f, 2f);
			m_creditsTitleText = (m_creditsText.Clone() as TextObj);
			m_creditsTitleText.FontSize = 14f;
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 12f;
			textObj.Text = "Down Attack this";
			textObj.OutlineWidth = 2;
			textObj.Align = Types.TextAlign.Centre;
			textObj.Position = m_waypointList[m_waypointList.Count - 1].Position;
			textObj.X -= 25f;
			textObj.Y -= 70f;
			GameObjList.Add(textObj);
			base.LoadContent(graphics);
		}
		public override void OnEnter()
		{
			m_speechBubble.Visible = false;
			m_diary.Visible = false;
			m_doorSprite.ChangeSprite("CastleDoorOpen_Sprite");
			if (Game.PlayerStats.TutorialComplete)
			{
				if (!Game.PlayerStats.ReadLastDiary)
				{
					m_door.Locked = true;
					m_doorSprite.ChangeSprite("CastleDoor_Sprite");
				}
				else
				{
					m_door.Locked = false;
				}
				m_diary.Visible = true;
				Player.UpdateCollisionBoxes();
				Player.Position = new Vector2(X + 240f + Player.Width, Bounds.Bottom - 120 - (Player.Bounds.Bottom - Player.Y));
			}
			m_creditsTitleText.Opacity = 0f;
			m_creditsText.Opacity = 0f;
			foreach (EnemyObj current in EnemyList)
			{
				current.Damage = 0;
			}
			m_tutorialText.Opacity = 0f;
			Player.UnlockControls();
			if (!Game.PlayerStats.TutorialComplete)
			{
				SoundManager.PlayMusic("EndSong", true, 4f);
			}
			else
			{
				SoundManager.StopMusic(4f);
			}
			Tween.RunFunction(2f, Player.AttachedLevel, "DisplayCreditsText", new object[]
			{
				true
			});
			base.OnEnter();
		}
		public void DisplayCreditsText()
		{
			if (m_creditsIndex < m_creditsTextList.Length)
			{
				m_creditsTitleText.Opacity = 0f;
				m_creditsText.Opacity = 0f;
				m_creditsTitleText.Text = m_creditsTextTitleList[m_creditsIndex];
				m_creditsText.Text = m_creditsTextList[m_creditsIndex];
				Tween.To(m_creditsTitleText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.To(m_creditsText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"1"
				});
				m_creditsTitleText.Opacity = 1f;
				m_creditsText.Opacity = 1f;
				Tween.To(m_creditsTitleText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"4",
					"Opacity",
					"0"
				});
				Tween.To(m_creditsText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"4.2",
					"Opacity",
					"0"
				});
				m_creditsTitleText.Opacity = 0f;
				m_creditsText.Opacity = 0f;
				m_creditsIndex++;
				Tween.RunFunction(8f, this, "DisplayCreditsText", new object[0]);
			}
		}
		private int PlayerNearWaypoint()
		{
			for (int i = 0; i < m_waypointList.Count; i++)
			{
				if (CDGMath.DistanceBetweenPts(Player.Position, m_waypointList[i].Position) < 500f)
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
				int waypointIndex = m_waypointIndex;
				m_waypointIndex = PlayerNearWaypoint();
				if (m_waypointIndex != waypointIndex)
				{
					Tween.StopAllContaining(m_tutorialText, false);
					if (m_waypointIndex != -1)
					{
						if (!InputManager.GamePadIsConnected(PlayerIndex.One))
						{
							m_tutorialText.Text = m_tutorialTextList[m_waypointIndex];
						}
						else
						{
							m_tutorialText.Text = m_tutorialControllerTextList[m_waypointIndex];
						}
						Tween.To(m_tutorialText, 0.25f, new Easing(Tween.EaseNone), new string[]
						{
							"Opacity",
							"1"
						});
					}
					else
					{
						Tween.To(m_tutorialText, 0.25f, new Easing(Tween.EaseNone), new string[]
						{
							"Opacity",
							"0"
						});
					}
				}
			}
			else
			{
				Rectangle bounds = m_diary.Bounds;
				bounds.X -= 50;
				bounds.Width += 100;
				m_speechBubble.Y = m_diary.Y - m_speechBubble.Height - 20f - 30f + (float)Math.Sin(Game.TotalGameTime * 20f) * 2f;
				if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
				{
					if (m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
					{
						m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");
					}
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						if (!Game.PlayerStats.ReadLastDiary)
						{
							RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
							rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + 24);
							rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "RunFlashback", new object[0]);
							rCScreenManager.DisplayScreen(13, true, null);
						}
						else
						{
							RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
							rCScreenManager2.DisplayScreen(20, true, null);
						}
					}
				}
				else if (m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
				{
					m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
				}
				if (!Game.PlayerStats.ReadLastDiary || CollisionMath.Intersects(Player.Bounds, bounds))
				{
					m_speechBubble.Visible = true;
				}
				else if (Game.PlayerStats.ReadLastDiary && !CollisionMath.Intersects(Player.Bounds, bounds))
				{
					m_speechBubble.Visible = false;
				}
			}
			base.Update(gameTime);
		}
		public void RunFlashback()
		{
			Player.LockControls();
			(Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(25, true, null);
			Tween.RunFunction(0.5f, this, "OpenDoor", new object[0]);
		}
		public void OpenDoor()
		{
			Player.UnlockControls();
			m_doorSprite.ChangeSprite("CastleDoorOpen_Sprite");
			m_door.Locked = false;
			Game.PlayerStats.ReadLastDiary = true;
			Game.PlayerStats.DiaryEntry = 25;
			(Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
		}
		public override void Draw(Camera2D camera)
		{
			Vector2 topLeftCorner = Game.ScreenManager.Camera.TopLeftCorner;
			m_creditsTitleText.Position = new Vector2(topLeftCorner.X + m_creditsPosition.X, topLeftCorner.Y + m_creditsPosition.Y);
			m_creditsText.Position = m_creditsTitleText.Position;
			m_creditsText.Y += 35f;
			m_creditsTitleText.X += 5f;
			base.Draw(camera);
			m_tutorialText.Position = Game.ScreenManager.Camera.Position;
			m_tutorialText.Y -= 200f;
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_tutorialText.Draw(camera);
			m_creditsText.Draw(camera);
			m_creditsTitleText.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_tutorialText.Dispose();
				m_tutorialText = null;
				m_waypointList.Clear();
				m_waypointList = null;
				m_creditsText.Dispose();
				m_creditsText = null;
				m_creditsTitleText.Dispose();
				m_creditsTitleText = null;
				Array.Clear(m_tutorialTextList, 0, m_tutorialTextList.Length);
				Array.Clear(m_tutorialControllerTextList, 0, m_tutorialControllerTextList.Length);
				Array.Clear(m_creditsTextTitleList, 0, m_creditsTextTitleList.Length);
				Array.Clear(m_creditsTextList, 0, m_creditsTextList.Length);
				m_tutorialTextList = null;
				m_creditsTextTitleList = null;
				m_creditsTextList = null;
				m_tutorialControllerTextList = null;
				m_door = null;
				m_doorSprite = null;
				m_diary = null;
				m_speechBubble = null;
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
