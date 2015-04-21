using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
namespace RogueCastle
{
	public class SpellSwapRoomObj : BonusRoomObj
	{
		private SpriteObj m_pedestal;
		private SpriteObj m_icon;
		private float m_iconYPos;
		private SpriteObj m_speechBubble;
		public byte Spell
		{
			get;
			set;
		}
		public override void Initialize()
		{
			this.m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			this.m_icon = new SpriteObj("Blank_Sprite");
			foreach (GameObj current in base.GameObjList)
			{
				if (current.Name == "pedestal")
				{
					this.m_pedestal = (current as SpriteObj);
					break;
				}
			}
			this.m_pedestal.OutlineWidth = 2;
			this.m_icon.X = this.m_pedestal.X;
			this.m_icon.Y = this.m_pedestal.Y - (this.m_pedestal.Y - (float)this.m_pedestal.Bounds.Top) - (float)this.m_icon.Height / 2f - 20f;
			this.m_icon.OutlineWidth = 2;
			this.m_iconYPos = this.m_icon.Y;
			base.GameObjList.Add(this.m_icon);
			this.m_speechBubble.Y = this.m_icon.Y - 30f;
			this.m_speechBubble.X = this.m_icon.X;
			this.m_speechBubble.Visible = false;
			base.GameObjList.Add(this.m_speechBubble);
			base.Initialize();
		}
		private void RandomizeItem()
		{
			if (Game.PlayerStats.Class != 16 && Game.PlayerStats.Class != 17)
			{
				byte[] spellList = ClassType.GetSpellList(Game.PlayerStats.Class);
				do
				{
					this.Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
				}
				while ((Game.PlayerStats.Traits.X == 31f || Game.PlayerStats.Traits.Y == 31f) && (this.Spell == 6 || this.Spell == 4 || this.Spell == 11));
				Array.Clear(spellList, 0, spellList.Length);
				base.ID = (int)this.Spell;
			}
			else if (Game.PlayerStats.Class == 16)
			{
				base.ID = 13;
				this.Spell = 13;
			}
			else if (Game.PlayerStats.Class == 17)
			{
				base.ID = 14;
				this.Spell = 14;
			}
			this.m_icon.ChangeSprite(SpellType.Icon(this.Spell));
		}
		public override void OnEnter()
		{
			if (base.ID == -1 && !base.RoomCompleted)
			{
				while (true)
				{
					this.RandomizeItem();
					if ((this.Spell != Game.PlayerStats.Spell || Game.PlayerStats.Class == 16 || Game.PlayerStats.Class == 17) && (this.Spell != 13 || Game.PlayerStats.Class == 16))
					{
						if (this.Spell != 14)
						{
							break;
						}
						if (Game.PlayerStats.Class == 17)
						{
							break;
						}
					}
				}
			}
			else if (base.ID != -1)
			{
				this.Spell = (byte)base.ID;
				this.m_icon.ChangeSprite(SpellType.Icon(this.Spell));
				if (base.RoomCompleted)
				{
					this.m_icon.Visible = false;
					this.m_speechBubble.Visible = false;
				}
			}
			base.OnEnter();
		}
		public override void Update(GameTime gameTime)
		{
			this.m_icon.Y = this.m_iconYPos - 10f + (float)Math.Sin((double)(Game.TotalGameTime * 2f)) * 5f;
			this.m_speechBubble.Y = this.m_iconYPos - 90f + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
			if (!base.RoomCompleted)
			{
				this.m_icon.Visible = true;
				if (CollisionMath.Intersects(this.Player.Bounds, this.m_pedestal.Bounds))
				{
					this.m_speechBubble.Visible = true;
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						this.TakeItem();
					}
				}
				else
				{
					this.m_speechBubble.Visible = false;
				}
			}
			else
			{
				this.m_icon.Visible = false;
			}
			base.Update(gameTime);
		}
		public void TakeItem()
		{
			base.RoomCompleted = true;
			Game.PlayerStats.Spell = this.Spell;
			if (Game.PlayerStats.Class == 9)
			{
				Game.PlayerStats.WizardSpellList = SpellType.GetNext3Spells();
			}
			this.Spell = 0;
			this.m_speechBubble.Visible = false;
			this.m_icon.Visible = false;
			(Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerSpellIcon();
			List<object> list = new List<object>();
			list.Add(new Vector2(this.m_icon.X, this.m_icon.Y - (float)this.m_icon.Height / 2f));
			list.Add(4);
			list.Add(new Vector2((float)Game.PlayerStats.Spell, 0f));
			(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list);
			Tween.RunFunction(0f, this.Player, "RunGetItemAnimation", new object[0]);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_pedestal = null;
				this.m_icon = null;
				this.m_speechBubble = null;
				base.Dispose();
			}
		}
	}
}
