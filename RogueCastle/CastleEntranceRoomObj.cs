using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class CastleEntranceRoomObj : RoomObj
	{
		private bool m_gateClosed;
		private PhysicsObj m_castleGate;
		private TeleporterObj m_teleporter;
		private ObjContainer m_bossDoorSprite;
		private DoorObj m_bossDoor;
		private SpriteObj m_diary;
		private SpriteObj m_speechBubble;
		private TextObj m_mapText;
		private KeyIconObj m_mapIcon;
		private bool m_allFilesSaved;
		private bool m_bossDoorOpening;
		public CastleEntranceRoomObj()
		{
			this.m_castleGate = new PhysicsObj("CastleEntranceGate_Sprite", null);
			this.m_castleGate.IsWeighted = false;
			this.m_castleGate.IsCollidable = true;
			this.m_castleGate.CollisionTypeTag = 1;
			this.m_castleGate.Layer = -1f;
			this.m_castleGate.OutlineWidth = 2;
			base.GameObjList.Add(this.m_castleGate);
			this.m_teleporter = new TeleporterObj();
			base.GameObjList.Add(this.m_teleporter);
		}
		public override void Initialize()
		{
			this.m_speechBubble = new SpriteObj("ExclamationSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			this.m_speechBubble.Scale = new Vector2(1.2f, 1.2f);
			base.GameObjList.Add(this.m_speechBubble);
			this.m_mapText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_mapText.Text = "view map any time";
			this.m_mapText.Align = Types.TextAlign.Centre;
			this.m_mapText.FontSize = 12f;
			this.m_mapText.OutlineWidth = 2;
			base.GameObjList.Add(this.m_mapText);
			this.m_mapIcon = new KeyIconObj();
			this.m_mapIcon.Scale = new Vector2(0.5f, 0.5f);
			base.GameObjList.Add(this.m_mapIcon);
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "diary")
				{
					this.m_diary = (current as SpriteObj);
				}
				if (current.Name == "map")
				{
					(current as SpriteObj).OutlineWidth = 2;
					this.m_mapText.Position = new Vector2(current.X, (float)(current.Bounds.Top - 50));
					this.m_mapIcon.Position = new Vector2(this.m_mapText.X, this.m_mapText.Y - 20f);
				}
			}
			this.m_diary.OutlineWidth = 2;
			this.m_speechBubble.Position = new Vector2(this.m_diary.X, this.m_diary.Y - (float)this.m_speechBubble.Height - 20f);
			DoorObj doorObj = null;
			foreach (GameObj current2 in base.GameObjList)
			{
				if (current2.Name == "LastDoor")
				{
					this.m_bossDoorSprite = (current2 as ObjContainer);
					break;
				}
			}
			foreach (DoorObj current3 in base.DoorList)
			{
				if (current3.DoorPosition == "Left")
				{
					doorObj = current3;
				}
				if (current3.IsBossDoor)
				{
					this.m_bossDoor = current3;
					this.m_bossDoor.Locked = true;
				}
			}
			for (int i = 1; i < this.m_bossDoorSprite.NumChildren; i++)
			{
				this.m_bossDoorSprite.GetChildAt(i).Opacity = 0f;
			}
			this.m_bossDoorSprite.AnimationDelay = 0.1f;
			this.m_castleGate.Position = new Vector2((float)(doorObj.Bounds.Right - this.m_castleGate.Width), doorObj.Y - (float)this.m_castleGate.Height);
			this.m_teleporter.Position = new Vector2(base.X + (float)this.Width / 2f - 600f, base.Y + 720f - 120f);
			base.Initialize();
		}
		public void RevealSymbol(GameTypes.LevelType levelType, bool tween)
		{
			bool flag = false;
			int index;
			switch (levelType)
			{
			case GameTypes.LevelType.CASTLE:
				index = 1;
				if (Game.PlayerStats.ChallengeEyeballBeaten)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.GARDEN:
				index = 3;
				if (Game.PlayerStats.ChallengeSkullBeaten)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.DUNGEON:
				index = 4;
				if (Game.PlayerStats.ChallengeBlobBeaten)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.TOWER:
				index = 2;
				if (Game.PlayerStats.ChallengeFireballBeaten)
				{
					flag = true;
				}
				break;
			default:
				index = 5;
				if (Game.PlayerStats.ChallengeLastBossBeaten)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				this.m_bossDoorSprite.GetChildAt(index).TextureColor = Color.Yellow;
			}
			else
			{
				this.m_bossDoorSprite.GetChildAt(index).TextureColor = Color.White;
			}
			if (tween)
			{
				this.m_bossDoorSprite.GetChildAt(index).Opacity = 0f;
				Tween.To(this.m_bossDoorSprite.GetChildAt(index), 0.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"delay",
					"1.5",
					"Opacity",
					"1"
				});
				return;
			}
			this.m_bossDoorSprite.GetChildAt(index).Opacity = 1f;
		}
		public override void OnEnter()
		{
			this.m_bossDoorOpening = false;
			if (Game.PlayerStats.ReadLastDiary && base.LinkedRoom.LinkedRoom != null)
			{
				base.LinkedRoom = base.LinkedRoom.LinkedRoom;
			}
			Game.PlayerStats.LoadStartingRoom = false;
			if (Game.PlayerStats.DiaryEntry < 1)
			{
				this.m_speechBubble.Visible = true;
			}
			else
			{
				this.m_speechBubble.Visible = false;
			}
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_mapIcon.SetButton(Game.GlobalInput.ButtonList[9]);
				this.m_mapIcon.Scale = new Vector2(1f, 1f);
			}
			else
			{
				this.m_mapIcon.SetKey(new Keys?(Game.GlobalInput.KeyList[9]), true);
				this.m_mapIcon.Scale = new Vector2(0.5f, 0.5f);
			}
			if (!this.m_allFilesSaved)
			{
				this.Player.Game.SaveManager.SaveAllFileTypes(false);
				this.m_allFilesSaved = true;
			}
			if (Game.PlayerStats.EyeballBossBeaten)
			{
				this.RevealSymbol(GameTypes.LevelType.CASTLE, false);
			}
			if (Game.PlayerStats.FairyBossBeaten)
			{
				this.RevealSymbol(GameTypes.LevelType.GARDEN, false);
			}
			if (Game.PlayerStats.BlobBossBeaten)
			{
				this.RevealSymbol(GameTypes.LevelType.DUNGEON, false);
			}
			if (Game.PlayerStats.FireballBossBeaten)
			{
				this.RevealSymbol(GameTypes.LevelType.TOWER, false);
			}
			if (Game.PlayerStats.EyeballBossBeaten && Game.PlayerStats.FairyBossBeaten && Game.PlayerStats.BlobBossBeaten && Game.PlayerStats.FireballBossBeaten && !Game.PlayerStats.FinalDoorOpened && this.Player.ScaleX > 0.1f)
			{
				this.PlayBossDoorAnimation();
			}
			else if (Game.PlayerStats.FinalDoorOpened)
			{
				this.m_bossDoor.Locked = false;
				this.m_bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
				this.m_bossDoorSprite.GoToFrame(this.m_bossDoorSprite.TotalFrames);
			}
			if (!this.m_gateClosed)
			{
				this.CloseGate(true);
			}
			if (Game.PlayerStats.EyeballBossBeaten && Game.PlayerStats.FairyBossBeaten && Game.PlayerStats.BlobBossBeaten && Game.PlayerStats.FireballBossBeaten && !Game.PlayerStats.FinalDoorOpened && this.Player.ScaleX > 0.1f)
			{
				Game.PlayerStats.FinalDoorOpened = true;
				this.Player.AttachedLevel.RunCinematicBorders(6f);
			}
			base.OnEnter();
		}
		public void PlayBossDoorAnimation()
		{
			this.Player.StopDash();
			this.m_bossDoorOpening = true;
			this.m_bossDoor.Locked = false;
			this.Player.AttachedLevel.UpdateCamera();
			this.RevealSymbol(GameTypes.LevelType.NONE, true);
			this.Player.CurrentSpeed = 0f;
			this.Player.LockControls();
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			float x = this.Player.AttachedLevel.Camera.X;
			object arg_C7_0 = this.Player.AttachedLevel.Camera;
			float arg_C7_1 = 1f;
			Easing arg_C7_2 = new Easing(Quad.EaseInOut);
			string[] array = new string[2];
			array[0] = "X";
			string[] arg_C5_0 = array;
			int arg_C5_1 = 1;
			int x2 = this.Bounds.Center.X;
			arg_C5_0[arg_C5_1] = x2.ToString();
			Tween.To(arg_C7_0, arg_C7_1, arg_C7_2, array);
			Tween.RunFunction(2.2f, this, "PlayBossDoorAnimation2", new object[]
			{
				x
			});
		}
		public void PlayBossDoorAnimation2(float storedX)
		{
			this.m_bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
			this.m_bossDoorSprite.PlayAnimation(false);
			SoundManager.PlaySound("LastDoor_Open");
			Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				"2",
				"X",
				storedX.ToString()
			});
			Tween.RunFunction(3.1f, this, "BossDoorAnimationComplete", new object[0]);
		}
		public void BossDoorAnimationComplete()
		{
			this.m_bossDoorOpening = false;
			this.Player.UnlockControls();
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
		}
		public void ForceGateClosed()
		{
			this.m_castleGate.Y += (float)this.m_castleGate.Height;
			this.m_gateClosed = true;
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_bossDoorOpening && !this.Player.ControlsLocked)
			{
				this.Player.LockControls();
			}
			if (!SoundManager.IsMusicPlaying)
			{
				SoundManager.PlayMusic("CastleSong", true, 0f);
			}
			if (this.Player.X < (float)this.m_castleGate.Bounds.Right)
			{
				this.Player.X = (float)(this.m_castleGate.Bounds.Right + 20);
				this.Player.AttachedLevel.UpdateCamera();
			}
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
			}
			else if (this.m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
			{
				this.m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
			}
			if (Game.PlayerStats.DiaryEntry < 1 || CollisionMath.Intersects(this.Player.Bounds, bounds))
			{
				this.m_speechBubble.Visible = true;
			}
			else if (Game.PlayerStats.DiaryEntry >= 1 && !CollisionMath.Intersects(this.Player.Bounds, bounds))
			{
				this.m_speechBubble.Visible = false;
			}
			if (CollisionMath.Intersects(this.Player.Bounds, bounds) && this.Player.IsTouchingGround && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
			{
				if (Game.PlayerStats.DiaryEntry < 1)
				{
					RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
					rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry0");
					rCScreenManager.DisplayScreen(13, true, null);
					PlayerStats expr_24E = Game.PlayerStats;
					expr_24E.DiaryEntry += 1;
				}
				else
				{
					RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
					rCScreenManager2.DisplayScreen(20, true, null);
				}
			}
			base.Update(gameTime);
		}
		public void CloseGate(bool animate)
		{
			if (animate)
			{
				this.Player.Y = 381f;
				this.Player.X += 10f;
				this.Player.State = 1;
				LogicSet logicSet = new LogicSet(this.Player);
				logicSet.AddAction(new RunFunctionLogicAction(this.Player, "LockControls", new object[0]), Types.Sequence.Serial);
				logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
				logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
				logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "CurrentSpeed", 200), Types.Sequence.Serial);
				logicSet.AddAction(new DelayLogicAction(0.2f, false), Types.Sequence.Serial);
				logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "CurrentSpeed", 0), Types.Sequence.Serial);
				this.Player.RunExternalLogicSet(logicSet);
				Tween.By(this.m_castleGate, 1.5f, new Easing(Quad.EaseOut), new string[]
				{
					"Y",
					this.m_castleGate.Height.ToString()
				});
				Tween.AddEndHandlerToLastTween(this.Player, "UnlockControls", new object[0]);
				this.Player.AttachedLevel.RunCinematicBorders(1.5f);
			}
			else
			{
				this.m_castleGate.Y += (float)this.m_castleGate.Height;
			}
			this.m_gateClosed = true;
		}
		public override void Reset()
		{
			if (this.m_gateClosed)
			{
				this.m_castleGate.Y -= (float)this.m_castleGate.Height;
				this.m_gateClosed = false;
			}
			base.Reset();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_castleGate = null;
				this.m_teleporter = null;
				this.m_bossDoor = null;
				this.m_bossDoorSprite = null;
				this.m_diary = null;
				this.m_speechBubble = null;
				this.m_mapText = null;
				this.m_mapIcon = null;
				base.Dispose();
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new CastleEntranceRoomObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
	}
}
