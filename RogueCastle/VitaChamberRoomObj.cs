/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
	public class VitaChamberRoomObj : BonusRoomObj
	{
		private GameObj m_fountain;
		private SpriteObj m_speechBubble;
		public override void Initialize()
		{
			m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
			m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			GameObjList.Add(m_speechBubble);
			foreach (GameObj current in GameObjList)
			{
				if (current.Name == "fountain")
				{
					m_fountain = current;
					break;
				}
			}
			(m_fountain as SpriteObj).OutlineWidth = 2;
			m_speechBubble.X = m_fountain.X;
			base.Initialize();
		}
		public override void OnEnter()
		{
			if (RoomCompleted)
			{
				m_speechBubble.Visible = false;
				m_fountain.TextureColor = new Color(100, 100, 100);
			}
			else
			{
				m_fountain.TextureColor = Color.White;
			}
			base.OnEnter();
		}
		public override void Update(GameTime gameTime)
		{
			if (!RoomCompleted)
			{
				Rectangle bounds = m_fountain.Bounds;
				bounds.X -= 50;
				bounds.Width += 100;
				if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
				{
					m_speechBubble.Y = m_fountain.Y - 150f + (float)Math.Sin(Game.TotalGameTime * 20f) * 2f;
					m_speechBubble.Visible = true;
				}
				else
				{
					m_speechBubble.Visible = false;
				}
				if (m_speechBubble.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
				{
					int num = (int)(Player.MaxHealth * 0.3f);
					int num2 = (int)(Player.MaxMana * 0.3f);
					Player.CurrentHealth += num;
					Player.CurrentMana += num2;
					Console.WriteLine("Healed");
					SoundManager.PlaySound("Collect_Mana");
					Player.AttachedLevel.TextManager.DisplayNumberStringText(num, "hp recovered", Color.LawnGreen, new Vector2(Player.X, Player.Bounds.Top - 30));
					Player.AttachedLevel.TextManager.DisplayNumberStringText(num2, "mp recovered", Color.CornflowerBlue, new Vector2(Player.X, Player.Bounds.Top));
					RoomCompleted = true;
					m_fountain.TextureColor = new Color(100, 100, 100);
					m_speechBubble.Visible = false;
				}
			}
			base.Update(gameTime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_fountain = null;
				m_speechBubble = null;
				base.Dispose();
			}
		}
	}
}
