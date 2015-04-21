using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
namespace RogueCastle
{
	public class ThroneRoomObj : RoomObj
	{
		private KeyIconTextObj m_tutorialText;
		private bool m_displayText;
		private KingObj m_king;
		private bool m_kingKilled;
		public override void Initialize()
		{
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "fountain")
				{
					(current as ObjContainer).OutlineWidth = 2;
					current.Y -= 2f;
					break;
				}
			}
			foreach (DoorObj current2 in base.DoorList)
			{
				current2.Locked = true;
			}
			base.Initialize();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			this.m_tutorialText = new KeyIconTextObj(Game.JunicodeLargeFont);
			this.m_tutorialText.FontSize = 28f;
			this.m_tutorialText.Text = "Press [Input:" + 12 + "] to Attack";
			this.m_tutorialText.Align = Types.TextAlign.Centre;
			this.m_tutorialText.OutlineWidth = 2;
			this.m_king = new KingObj("King_Sprite");
			this.m_king.OutlineWidth = 2;
			this.m_king.AnimationDelay = 0.1f;
			this.m_king.PlayAnimation(true);
			this.m_king.IsWeighted = false;
			this.m_king.IsCollidable = true;
			this.m_king.Scale = new Vector2(2f, 2f);
			base.LoadContent(graphics);
		}
		public override void OnEnter()
		{
			SoundManager.StopMusic(1f);
			if (this.m_king.PhysicsMngr == null)
			{
				this.Player.PhysicsMngr.AddObject(this.m_king);
			}
			this.m_kingKilled = false;
			this.Player.UnlockControls();
			this.m_displayText = false;
			this.m_tutorialText.Opacity = 0f;
			this.m_king.UpdateCollisionBoxes();
			this.m_king.Position = new Vector2((float)(this.Bounds.Right - 500), base.Y + 1440f - ((float)this.m_king.Bounds.Bottom - this.m_king.Y) - 182f);
		}
		public override void OnExit()
		{
			base.OnExit();
		}
		public override void Update(GameTime gameTime)
		{
			if (!this.m_displayText && CDGMath.DistanceBetweenPts(this.Player.Position, this.m_king.Position) < 200f)
			{
				this.m_displayText = true;
				Tween.StopAllContaining(this.m_tutorialText, false);
				Tween.To(this.m_tutorialText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
			}
			else if (this.m_displayText && CDGMath.DistanceBetweenPts(this.Player.Position, this.m_king.Position) > 200f)
			{
				this.m_displayText = false;
				Tween.StopAllContaining(this.m_tutorialText, false);
				Tween.To(this.m_tutorialText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
			}
			if (this.Player.X > this.m_king.X - 100f)
			{
				this.Player.X = this.m_king.X - 100f;
			}
			if (!this.m_kingKilled && this.m_king.WasHit)
			{
				this.m_kingKilled = true;
				Game.ScreenManager.DisplayScreen(27, false, null);
			}
			base.Update(gameTime);
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			this.m_king.Draw(camera);
			this.m_tutorialText.Position = Game.ScreenManager.Camera.Position;
			this.m_tutorialText.Y -= 200f;
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_tutorialText.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_tutorialText.Dispose();
				this.m_tutorialText = null;
				this.m_king.Dispose();
				this.m_king = null;
				base.Dispose();
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new ThroneRoomObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
	}
}
