using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
namespace RogueCastle
{
	public class PortraitRoomObj : BonusRoomObj
	{
		private SpriteObj m_portraitFrame;
		private int m_portraitIndex;
		private SpriteObj m_portrait;
		public override void Initialize()
		{
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "portrait")
				{
					this.m_portraitFrame = (current as SpriteObj);
					break;
				}
			}
			this.m_portraitFrame.ChangeSprite("GiantPortrait_Sprite");
			this.m_portraitFrame.Scale = new Vector2(2f, 2f);
			this.m_portrait = new SpriteObj("Blank_Sprite");
			this.m_portrait.Position = this.m_portraitFrame.Position;
			this.m_portrait.Scale = new Vector2(0.95f, 0.95f);
			base.GameObjList.Add(this.m_portrait);
			base.Initialize();
		}
		public override void OnEnter()
		{
			if (!base.RoomCompleted && base.ID == -1)
			{
				this.m_portraitIndex = CDGMath.RandomInt(0, 7);
				this.m_portrait.ChangeSprite("Portrait" + this.m_portraitIndex + "_Sprite");
				base.ID = this.m_portraitIndex;
				base.OnEnter();
				return;
			}
			if (base.ID != -1)
			{
				this.m_portraitIndex = base.ID;
				this.m_portrait.ChangeSprite("Portrait" + this.m_portraitIndex + "_Sprite");
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
			{
				Rectangle b = new Rectangle(this.Bounds.Center.X - 100, this.Bounds.Bottom - 300, 200, 200);
				if (CollisionMath.Intersects(this.Player.Bounds, b) && this.Player.IsTouchingGround && base.ID > -1)
				{
					RCScreenManager screenManager = Game.ScreenManager;
					screenManager.DialogueScreen.SetDialogue("PortraitRoomText" + base.ID);
					screenManager.DisplayScreen(13, true, null);
				}
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_portraitFrame = null;
				this.m_portrait = null;
				base.Dispose();
			}
		}
	}
}
