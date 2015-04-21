using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
namespace RogueCastle
{
	public class SpecialItemRoomObj : BonusRoomObj
	{
		private SpriteObj m_pedestal;
		private SpriteObj m_icon;
		private float m_iconYPos;
		private SpriteObj m_speechBubble;
		public byte ItemType
		{
			get;
			set;
		}
		public override void Initialize()
		{
			this.m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
			this.m_speechBubble.Flip = SpriteEffects.FlipHorizontally;
			this.m_icon = new SpriteObj("Blank_Sprite");
			this.m_icon.Scale = new Vector2(2f, 2f);
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
			this.m_icon.Y = this.m_pedestal.Y - (this.m_pedestal.Y - (float)this.m_pedestal.Bounds.Top) - (float)this.m_icon.Height / 2f - 10f;
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
			if (Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f || Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f || Game.PlayerStats.Traits.X == 20f || Game.PlayerStats.Traits.Y == 20f || Game.PlayerStats.Traits.X == 1f || Game.PlayerStats.Traits.Y == 1f || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f)
			{
				if (CDGMath.RandomInt(1, 100) <= 30)
				{
					this.ItemType = 8;
				}
				else
				{
					this.ItemType = this.GetRandomItem();
				}
			}
			else
			{
				this.ItemType = this.GetRandomItem();
			}
			this.m_icon.ChangeSprite(SpecialItemType.SpriteName(this.ItemType));
			base.ID = (int)this.ItemType;
		}
		private byte GetRandomItem()
		{
			List<byte> list = new List<byte>();
			for (int i = 1; i < 7; i++)
			{
				list.Add((byte)i);
			}
			if ((Game.PlayerStats.EyeballBossBeaten || Game.PlayerStats.TimesCastleBeaten > 0) && !Game.PlayerStats.ChallengeEyeballUnlocked && !Game.PlayerStats.ChallengeEyeballBeaten)
			{
				list.Add(9);
				list.Add(9);
			}
			if ((Game.PlayerStats.FairyBossBeaten || Game.PlayerStats.TimesCastleBeaten > 0) && !Game.PlayerStats.ChallengeSkullUnlocked && !Game.PlayerStats.ChallengeSkullBeaten)
			{
				list.Add(10);
				list.Add(10);
			}
			if ((Game.PlayerStats.FireballBossBeaten || Game.PlayerStats.TimesCastleBeaten > 0) && !Game.PlayerStats.ChallengeFireballUnlocked && !Game.PlayerStats.ChallengeFireballBeaten)
			{
				list.Add(11);
				list.Add(11);
			}
			if ((Game.PlayerStats.BlobBossBeaten || Game.PlayerStats.TimesCastleBeaten > 0) && !Game.PlayerStats.ChallengeBlobUnlocked && !Game.PlayerStats.ChallengeBlobBeaten)
			{
				list.Add(12);
				list.Add(12);
				list.Add(12);
			}
			if (!Game.PlayerStats.ChallengeLastBossUnlocked && !Game.PlayerStats.ChallengeLastBossBeaten && Game.PlayerStats.ChallengeEyeballBeaten && Game.PlayerStats.ChallengeSkullBeaten && Game.PlayerStats.ChallengeFireballBeaten && Game.PlayerStats.ChallengeBlobBeaten)
			{
				list.Add(13);
				list.Add(13);
				list.Add(13);
				list.Add(13);
				list.Add(13);
			}
			return list[CDGMath.RandomInt(0, list.Count - 1)];
		}
		public override void OnEnter()
		{
			this.m_icon.Visible = false;
			if (base.ID == -1 && !base.RoomCompleted)
			{
				do
				{
					this.RandomizeItem();
				}
				while (this.ItemType == Game.PlayerStats.SpecialItem);
			}
			else if (base.ID != -1)
			{
				this.ItemType = (byte)base.ID;
				this.m_icon.ChangeSprite(SpecialItemType.SpriteName(this.ItemType));
				if (base.RoomCompleted)
				{
					this.m_icon.Visible = false;
					this.m_speechBubble.Visible = false;
				}
			}
			if (base.RoomCompleted)
			{
				this.m_pedestal.TextureColor = new Color(100, 100, 100);
			}
			else
			{
				this.m_pedestal.TextureColor = Color.White;
			}
			base.OnEnter();
		}
		public override void Update(GameTime gameTime)
		{
			this.m_icon.Y = this.m_iconYPos + (float)Math.Sin((double)(Game.TotalGameTime * 2f)) * 5f;
			this.m_speechBubble.Y = this.m_iconYPos - 30f + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
			if (!base.RoomCompleted)
			{
				if (CollisionMath.Intersects(this.Player.Bounds, this.m_pedestal.Bounds))
				{
					this.m_speechBubble.Visible = true;
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
						rCScreenManager.DialogueScreen.SetDialogue("Special Item Prayer");
						rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
						rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "TakeItem", new object[0]);
						rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
						{
							"Canceling Selection"
						});
						rCScreenManager.DisplayScreen(13, true, null);
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
			Game.PlayerStats.SpecialItem = this.ItemType;
			this.m_pedestal.TextureColor = new Color(100, 100, 100);
			if (this.ItemType == 8)
			{
				this.Player.GetChildAt(10).Visible = true;
				this.Player.PlayAnimation(true);
			}
			else
			{
				this.Player.GetChildAt(10).Visible = false;
			}
			this.ItemType = 0;
			this.m_speechBubble.Visible = false;
			this.m_icon.Visible = false;
			(Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerHUDSpecialItem();
			(Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerSpellIcon();
			List<object> list = new List<object>();
			list.Add(new Vector2(this.m_pedestal.X, this.m_pedestal.Y - (float)this.m_pedestal.Height / 2f));
			list.Add(5);
			list.Add(new Vector2((float)Game.PlayerStats.SpecialItem, 0f));
			(this.Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.MapData
			});
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
