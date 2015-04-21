using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class CarnivalShoot1BonusRoom : BonusRoomObj
	{
		private GameObj m_line;
		private List<BreakableObj> m_targetList;
		private byte m_storedPlayerSpell;
		private int m_daggersThrown;
		private float m_storedPlayerMana;
		private int m_currentTargetIndex;
		private BreakableObj m_currentTarget;
		private bool m_targetMovingUp;
		private bool m_isPlayingGame;
		private bool m_spokeToNPC;
		private NpcObj m_elf;
		private ObjContainer m_daggerIcons;
		private ObjContainer m_targetIcons;
		private int m_numTries = 12;
		private int m_numTargets = 8;
		private float m_targetSpeed = 200f;
		private float m_targetSpeedMod = 100f;
		private List<ObjContainer> m_balloonList;
		private ChestObj m_rewardChest;
		private int ActiveTargets
		{
			get
			{
				int num = 0;
				foreach (BreakableObj current in this.m_targetList)
				{
					if (!current.Broken)
					{
						num++;
					}
				}
				return num;
			}
		}
		public CarnivalShoot1BonusRoom()
		{
			this.m_elf = new NpcObj("Clown_Character");
			this.m_elf.Scale = new Vector2(2f, 2f);
			this.m_balloonList = new List<ObjContainer>();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			this.m_daggerIcons = new ObjContainer();
			int num = 0;
			int num2 = 10;
			for (int i = 0; i < this.m_numTries; i++)
			{
				SpriteObj spriteObj = new SpriteObj("SpellDagger_Sprite");
				spriteObj.Scale = new Vector2(2f, 2f);
				spriteObj.X = (float)(num + 10);
				spriteObj.Y = (float)num2;
				num += spriteObj.Width;
				if (i == this.m_numTries / 2 - 1)
				{
					num = 0;
					num2 += 20;
				}
				this.m_daggerIcons.AddChild(spriteObj);
			}
			this.m_daggerIcons.OutlineWidth = 2;
			this.m_targetIcons = new ObjContainer();
			for (int j = 0; j < this.m_numTargets; j++)
			{
				SpriteObj spriteObj2 = new SpriteObj("Target2Piece1_Sprite");
				spriteObj2.Scale = new Vector2(2f, 2f);
				spriteObj2.X += (float)(j * (spriteObj2.Width + 10));
				this.m_targetIcons.AddChild(spriteObj2);
			}
			this.m_targetIcons.OutlineWidth = 2;
			base.GameObjList.Add(this.m_targetIcons);
			base.GameObjList.Add(this.m_daggerIcons);
			base.LoadContent(graphics);
		}
		public override void Initialize()
		{
			Color[] array = new Color[]
			{
				Color.Red,
				Color.Blue,
				Color.Green,
				Color.Yellow,
				Color.Orange,
				Color.Purple,
				Color.Pink,
				Color.MediumTurquoise,
				Color.CornflowerBlue
			};
			foreach (GameObj current in base.GameObjList)
			{
				if (current is WaypointObj)
				{
					this.m_elf.X = current.X;
				}
				if (current.Name == "Line")
				{
					this.m_line = current;
				}
				if (current.Name == "Balloon")
				{
					this.m_balloonList.Add(current as ObjContainer);
					(current as ObjContainer).GetChildAt(1).TextureColor = array[CDGMath.RandomInt(0, array.Length - 1)];
				}
			}
			float num = 0f;
			foreach (TerrainObj current2 in base.TerrainObjList)
			{
				if (current2.Name == "Floor")
				{
					this.m_elf.Y = current2.Y - ((float)this.m_elf.Bounds.Bottom - this.m_elf.Y);
					num = current2.Y;
					break;
				}
			}
			if (!base.IsReversed)
			{
				this.m_elf.Flip = SpriteEffects.FlipHorizontally;
			}
			base.GameObjList.Add(this.m_elf);
			this.m_elf.Y -= 2f;
			this.m_targetList = new List<BreakableObj>();
			for (int i = 0; i < this.m_numTargets; i++)
			{
				BreakableObj breakableObj = new BreakableObj("Target1_Character");
				breakableObj.Scale = new Vector2(2f, 2f);
				breakableObj.Visible = false;
				breakableObj.DropItem = false;
				breakableObj.HitBySpellsOnly = true;
				breakableObj.Position = this.m_line.Position;
				if (!base.IsReversed)
				{
					breakableObj.Flip = SpriteEffects.FlipHorizontally;
				}
				else
				{
					breakableObj.Flip = SpriteEffects.None;
				}
				this.m_targetList.Add(breakableObj);
				base.GameObjList.Add(breakableObj);
			}
			this.m_rewardChest = new ChestObj(null);
			this.m_rewardChest.ChestType = 3;
			if (!base.IsReversed)
			{
				this.m_rewardChest.Flip = SpriteEffects.FlipHorizontally;
				this.m_rewardChest.Position = new Vector2(this.m_elf.X + 100f, num - (float)this.m_rewardChest.Height - 8f);
			}
			else
			{
				this.m_rewardChest.Position = new Vector2(this.m_elf.X - 150f, num - (float)this.m_rewardChest.Height - 8f);
			}
			this.m_rewardChest.Visible = false;
			base.GameObjList.Add(this.m_rewardChest);
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.m_rewardChest.ChestType = 3;
			if (this.m_rewardChest.PhysicsMngr == null)
			{
				this.Player.PhysicsMngr.AddObject(this.m_rewardChest);
			}
			this.m_spokeToNPC = false;
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			if (!base.IsReversed)
			{
				this.Player.AttachedLevel.Camera.Position = new Vector2((float)(this.Bounds.Left + this.Player.AttachedLevel.Camera.Width / 2), (float)(this.Bounds.Top + this.Player.AttachedLevel.Camera.Height / 2));
			}
			else
			{
				this.Player.AttachedLevel.Camera.Position = new Vector2((float)(this.Bounds.Right - this.Player.AttachedLevel.Camera.Width / 2), (float)(this.Bounds.Top + this.Player.AttachedLevel.Camera.Height / 2));
			}
			this.m_currentTargetIndex = 0;
			this.m_daggersThrown = 0;
			this.m_storedPlayerMana = this.Player.CurrentMana;
			this.m_storedPlayerSpell = Game.PlayerStats.Spell;
			this.InitializeTargetSystem();
			if (!base.IsReversed)
			{
				this.m_targetIcons.Position = new Vector2((float)(this.Bounds.Right - 100 - this.m_targetIcons.Width), (float)(this.Bounds.Bottom - 40));
				this.m_daggerIcons.Position = this.m_targetIcons.Position;
				this.m_daggerIcons.X -= (float)(400 + this.m_daggerIcons.Width);
			}
			else
			{
				this.m_targetIcons.Position = new Vector2((float)(this.Bounds.Left + 150), (float)(this.Bounds.Bottom - 40));
				this.m_daggerIcons.Position = this.m_targetIcons.Position;
				this.m_daggerIcons.X += (float)(this.m_targetIcons.Width + 400);
			}
			this.m_daggerIcons.Y -= 30f;
			this.ReflipPosters();
			base.OnEnter();
		}
		private void ReflipPosters()
		{
			foreach (GameObj current in base.GameObjList)
			{
				SpriteObj spriteObj = current as SpriteObj;
				if (spriteObj != null && spriteObj.Flip == SpriteEffects.FlipHorizontally && (spriteObj.SpriteName == "CarnivalPoster1_Sprite" || spriteObj.SpriteName == "CarnivalPoster2_Sprite" || spriteObj.SpriteName == "CarnivalPoster3_Sprite" || spriteObj.SpriteName == "CarnivalTent_Sprite"))
				{
					spriteObj.Flip = SpriteEffects.None;
				}
			}
		}
		public void BeginGame()
		{
			this.Player.AttachedLevel.ProjectileManager.DestroyAllProjectiles(true);
			this.Player.StopAllSpells();
			this.m_isPlayingGame = true;
			this.m_spokeToNPC = true;
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.Player.AttachedLevel.Camera.Y = (float)this.Bounds.Center.Y;
			if (!base.IsReversed)
			{
				Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(this.m_line.X + 500f).ToString()
				});
			}
			else
			{
				Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(this.m_line.X - 500f).ToString()
				});
			}
			this.EquipPlayer();
			this.ActivateTarget();
		}
		public void EndGame()
		{
			if (!base.IsReversed)
			{
				Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(base.X + (float)this.Player.AttachedLevel.Camera.Width / 2f).ToString()
				});
			}
			else
			{
				Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					((float)this.Bounds.Right - (float)this.Player.AttachedLevel.Camera.Width / 2f).ToString()
				});
			}
			Tween.AddEndHandlerToLastTween(this, "CheckPlayerReward", new object[0]);
			this.m_isPlayingGame = false;
			Game.PlayerStats.Spell = 0;
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			base.RoomCompleted = true;
		}
		public void CheckPlayerReward()
		{
			if (this.ActiveTargets <= 0)
			{
				RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
				rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Reward");
				(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
				this.RevealChest();
				GameUtil.UnlockAchievement("LOVE_OF_CLOWNS");
				return;
			}
			RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
			rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom1-Fail");
			(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
		}
		public void RevealChest()
		{
			this.Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(this.m_rewardChest.Position);
			this.m_rewardChest.Visible = true;
		}
		private void InitializeTargetSystem()
		{
			foreach (BreakableObj current in this.m_targetList)
			{
				current.Reset();
				current.Visible = false;
				if (!base.IsReversed)
				{
					current.Position = new Vector2((float)this.Bounds.Right, (float)this.Bounds.Center.Y);
					current.Flip = SpriteEffects.FlipHorizontally;
				}
				else
				{
					current.Position = new Vector2((float)this.Bounds.Left, (float)this.Bounds.Center.Y);
					current.Flip = SpriteEffects.None;
				}
			}
		}
		private void EquipPlayer()
		{
			Game.PlayerStats.Spell = 1;
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.CurrentMana = this.Player.MaxMana;
		}
		public void UnequipPlayer()
		{
			Game.PlayerStats.Spell = this.m_storedPlayerSpell;
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.CurrentMana = this.m_storedPlayerMana;
		}
		public override void OnExit()
		{
			this.UnequipPlayer();
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			base.OnExit();
		}
		private void HandleInput()
		{
			if (this.m_isPlayingGame && (Game.GlobalInput.JustPressed(24) || (Game.GlobalInput.JustPressed(12) && Game.PlayerStats.Class == 16)) && this.Player.SpellCastDelay <= 0f)
			{
				this.m_daggersThrown++;
				this.Player.CurrentMana = this.Player.MaxMana;
				if (this.m_daggersThrown <= this.m_numTries)
				{
					this.m_daggerIcons.GetChildAt(this.m_numTries - this.m_daggersThrown).Visible = false;
				}
				if (this.m_daggersThrown > this.m_numTries)
				{
					Game.PlayerStats.Spell = 0;
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			this.m_elf.Update(gameTime, this.Player);
			if (!base.IsReversed)
			{
				if (this.Player.X >= this.m_line.X - 150f)
				{
					this.Player.X = (float)((int)this.m_line.X - 150);
				}
			}
			else if (this.Player.X < this.m_line.X + 150f)
			{
				this.Player.X = this.m_line.X + 150f;
			}
			if (!base.IsReversed)
			{
				if (this.m_isPlayingGame && this.Player.X < (float)this.Player.AttachedLevel.Camera.Bounds.Left)
				{
					this.Player.X = (float)this.Player.AttachedLevel.Camera.Bounds.Left;
				}
				if (this.Player.X > (float)(this.Bounds.Right - 1320))
				{
					this.Player.X = (float)(this.Bounds.Right - 1320);
				}
			}
			else
			{
				if (this.m_isPlayingGame && this.Player.X > (float)this.Player.AttachedLevel.Camera.Bounds.Right)
				{
					this.Player.X = (float)this.Player.AttachedLevel.Camera.Bounds.Right;
				}
				if (this.Player.X < (float)(this.Bounds.Left + 1320))
				{
					this.Player.X = (float)(this.Bounds.Left + 1320);
				}
			}
			if (this.m_currentTarget != null && !this.m_currentTarget.Broken)
			{
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_targetMovingUp && this.m_currentTarget.Bounds.Top > this.Bounds.Top + 80)
				{
					this.m_currentTarget.Y -= num * this.m_targetSpeed;
				}
				else if (this.m_targetMovingUp)
				{
					this.m_currentTarget.Y += num * this.m_targetSpeed;
					this.m_targetMovingUp = false;
				}
				if (!this.m_targetMovingUp && this.m_currentTarget.Bounds.Bottom < this.Bounds.Bottom - 140)
				{
					this.m_currentTarget.Y += num * this.m_targetSpeed;
				}
				else if (!this.m_targetMovingUp)
				{
					this.m_currentTarget.Y -= num * this.m_targetSpeed;
					this.m_targetMovingUp = true;
				}
			}
			if (this.m_isPlayingGame && ((this.m_daggersThrown >= this.m_numTries && this.Player.AttachedLevel.ProjectileManager.ActiveProjectiles < 1 && this.ActiveTargets > 0) || this.ActiveTargets <= 0))
			{
				this.EndGame();
			}
			if (this.m_currentTarget != null && this.m_currentTarget.Broken && this.ActiveTargets >= 0)
			{
				this.m_currentTargetIndex++;
				this.ActivateTarget();
			}
			if (this.m_elf.IsTouching && !base.RoomCompleted && !this.m_spokeToNPC)
			{
				if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
					rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Start");
					rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
					rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "BeginGame", new object[0]);
					rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
					{
						"Canceling Selection"
					});
					(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
				}
			}
			else if (this.m_elf.IsTouching && base.RoomCompleted && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
			{
				RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
				rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom1-End");
				(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
			}
			if (this.m_isPlayingGame)
			{
				this.m_elf.CanTalk = false;
			}
			else
			{
				this.m_elf.CanTalk = true;
			}
			float totalGameTime = Game.TotalGameTime;
			float num2 = 2f;
			foreach (ObjContainer current in this.m_balloonList)
			{
				current.Rotation = (float)Math.Sin((double)(totalGameTime * num2)) * num2;
				num2 += 0.2f;
			}
			this.HandleInput();
			base.Update(gameTime);
		}
		public void ActivateTarget()
		{
			if (this.m_numTargets - this.m_currentTargetIndex < this.m_targetIcons.NumChildren)
			{
				this.m_targetIcons.GetChildAt(this.m_numTargets - this.m_currentTargetIndex).Visible = false;
				this.GiveGold();
			}
			if (this.m_currentTargetIndex >= this.m_numTargets)
			{
				this.m_currentTarget = null;
				return;
			}
			if (this.m_currentTarget != null)
			{
				this.m_targetSpeed += this.m_targetSpeedMod;
			}
			this.m_currentTarget = this.m_targetList[this.m_currentTargetIndex];
			this.m_currentTarget.Visible = true;
			if (!base.IsReversed)
			{
				Tween.By(this.m_currentTarget, 2f, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					(-400 + CDGMath.RandomInt(-200, 200)).ToString()
				});
				return;
			}
			Tween.By(this.m_currentTarget, 2f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				(400 + CDGMath.RandomInt(-200, 200)).ToString()
			});
		}
		public void GiveGold()
		{
			int num = this.m_numTargets - this.ActiveTargets;
			if (this.ActiveTargets > 0)
			{
				this.Player.AttachedLevel.ImpactEffectPool.CarnivalGoldEffect(this.m_currentTarget.Position, new Vector2(this.Player.AttachedLevel.Camera.TopLeftCorner.X + 50f, this.Player.AttachedLevel.Camera.TopLeftCorner.Y + 135f), num);
			}
			this.Player.AttachedLevel.TextManager.DisplayNumberStringText(num * 10, " gold", Color.Yellow, this.m_currentTarget.Position);
			Game.PlayerStats.Gold += num * 10;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_targetList.Clear();
				this.m_targetList = null;
				this.m_line = null;
				this.m_currentTarget = null;
				this.m_elf = null;
				this.m_daggerIcons = null;
				this.m_targetIcons = null;
				this.m_balloonList.Clear();
				this.m_balloonList = null;
				this.m_rewardChest = null;
				base.Dispose();
			}
		}
	}
}
