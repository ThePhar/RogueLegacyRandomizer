using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueCastle
{
	public class DiaryRoomObj : BonusRoomObj
	{
		private SpriteObj m_speechBubble;
		private SpriteObj m_diary;
		private int m_diaryIndex;
		public override void Initialize()
		{
			this.m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			base.GameObjList.Add(this.m_speechBubble);
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "diary")
				{
					this.m_diary = (current as SpriteObj);
					break;
				}
			}
			this.m_diary.OutlineWidth = 2;
			this.m_speechBubble.Position = new Vector2(this.m_diary.X, this.m_diary.Y - (float)this.m_speechBubble.Height - 20f);
			base.Initialize();
		}
		public override void OnEnter()
		{
			if (!base.RoomCompleted)
			{
				this.m_speechBubble.Visible = true;
			}
			else
			{
				this.m_speechBubble.Visible = false;
			}
			if (!base.RoomCompleted)
			{
				this.m_diaryIndex = (int)Game.PlayerStats.DiaryEntry;
			}
			if (this.m_diaryIndex >= 24)
			{
				this.m_speechBubble.Visible = false;
			}
			base.OnEnter();
		}
		public override void Update(GameTime gameTime)
		{
			this.m_speechBubble.Y = this.m_diary.Y - (float)this.m_speechBubble.Height - 20f - 30f + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
			Rectangle bounds = this.m_diary.Bounds;
			bounds.X -= 50;
			bounds.Width += 100;
			if (!CollisionMath.Intersects(this.Player.Bounds, bounds) && this.m_speechBubble.SpriteName == "UpArrowSquare_Sprite")
			{
				this.m_speechBubble.ChangeSprite("ExclamationSquare_Sprite");
			}
			if (!base.RoomCompleted || CollisionMath.Intersects(this.Player.Bounds, bounds))
			{
				this.m_speechBubble.Visible = true;
			}
			else if (base.RoomCompleted && !CollisionMath.Intersects(this.Player.Bounds, bounds))
			{
				this.m_speechBubble.Visible = false;
			}
			if (this.m_diaryIndex >= 24)
			{
				this.m_speechBubble.Visible = false;
			}
			if (CollisionMath.Intersects(this.Player.Bounds, bounds) && this.Player.IsTouchingGround)
			{
				if (this.m_speechBubble.SpriteName == "ExclamationSquare_Sprite")
				{
					this.m_speechBubble.ChangeSprite("UpArrowSquare_Sprite");
				}
				if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					if (!base.RoomCompleted && Game.PlayerStats.DiaryEntry < 24)
					{
						RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
						rCScreenManager.DialogueScreen.SetDialogue("DiaryEntry" + this.m_diaryIndex);
						rCScreenManager.DisplayScreen(13, true, null);
						PlayerStats expr_1DB = Game.PlayerStats;
						expr_1DB.DiaryEntry += 1;
						base.RoomCompleted = true;
					}
					else
					{
						base.RoomCompleted = true;
						RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
						rCScreenManager2.DisplayScreen(20, true, null);
					}
				}
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_diary = null;
				this.m_speechBubble = null;
				base.Dispose();
			}
		}
	}
}
