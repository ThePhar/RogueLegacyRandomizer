/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			foreach (GameObj current in GameObjList)
			{
				if (current.Name == "fountain")
				{
					(current as ObjContainer).OutlineWidth = 2;
					current.Y -= 2f;
					break;
				}
			}
			foreach (DoorObj current2 in DoorList)
			{
				current2.Locked = true;
			}
			base.Initialize();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			m_tutorialText = new KeyIconTextObj(Game.JunicodeLargeFont);
			m_tutorialText.FontSize = 28f;
			m_tutorialText.Text = "Press [Input:" + 12 + "] to Attack";
			m_tutorialText.Align = Types.TextAlign.Centre;
			m_tutorialText.OutlineWidth = 2;
			m_king = new KingObj("King_Sprite");
			m_king.OutlineWidth = 2;
			m_king.AnimationDelay = 0.1f;
			m_king.PlayAnimation(true);
			m_king.IsWeighted = false;
			m_king.IsCollidable = true;
			m_king.Scale = new Vector2(2f, 2f);
			base.LoadContent(graphics);
		}
		public override void OnEnter()
		{
			SoundManager.StopMusic(1f);
			if (m_king.PhysicsMngr == null)
			{
				Player.PhysicsMngr.AddObject(m_king);
			}
			m_kingKilled = false;
			Player.UnlockControls();
			m_displayText = false;
			m_tutorialText.Opacity = 0f;
			m_king.UpdateCollisionBoxes();
			m_king.Position = new Vector2(Bounds.Right - 500, Y + 1440f - (m_king.Bounds.Bottom - m_king.Y) - 182f);
		}
		public override void OnExit()
		{
			base.OnExit();
		}
		public override void Update(GameTime gameTime)
		{
			if (!m_displayText && CDGMath.DistanceBetweenPts(Player.Position, m_king.Position) < 200f)
			{
				m_displayText = true;
				Tween.StopAllContaining(m_tutorialText, false);
				Tween.To(m_tutorialText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
			}
			else if (m_displayText && CDGMath.DistanceBetweenPts(Player.Position, m_king.Position) > 200f)
			{
				m_displayText = false;
				Tween.StopAllContaining(m_tutorialText, false);
				Tween.To(m_tutorialText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
			}
			if (Player.X > m_king.X - 100f)
			{
				Player.X = m_king.X - 100f;
			}
			if (!m_kingKilled && m_king.WasHit)
			{
				m_kingKilled = true;
				Game.ScreenManager.DisplayScreen(27, false, null);
			}
			base.Update(gameTime);
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			m_king.Draw(camera);
			m_tutorialText.Position = Game.ScreenManager.Camera.Position;
			m_tutorialText.Y -= 200f;
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			m_tutorialText.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_tutorialText.Dispose();
				m_tutorialText = null;
				m_king.Dispose();
				m_king = null;
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
