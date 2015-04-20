/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
				foreach (BreakableObj current in m_targetList)
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
			m_elf = new NpcObj("Clown_Character");
			m_elf.Scale = new Vector2(2f, 2f);
			m_balloonList = new List<ObjContainer>();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			m_daggerIcons = new ObjContainer();
			int num = 0;
			int num2 = 10;
			for (int i = 0; i < m_numTries; i++)
			{
				SpriteObj spriteObj = new SpriteObj("SpellDagger_Sprite");
				spriteObj.Scale = new Vector2(2f, 2f);
				spriteObj.X = num + 10;
				spriteObj.Y = num2;
				num += spriteObj.Width;
				if (i == m_numTries / 2 - 1)
				{
					num = 0;
					num2 += 20;
				}
				m_daggerIcons.AddChild(spriteObj);
			}
			m_daggerIcons.OutlineWidth = 2;
			m_targetIcons = new ObjContainer();
			for (int j = 0; j < m_numTargets; j++)
			{
				SpriteObj spriteObj2 = new SpriteObj("Target2Piece1_Sprite");
				spriteObj2.Scale = new Vector2(2f, 2f);
				spriteObj2.X += j * (spriteObj2.Width + 10);
				m_targetIcons.AddChild(spriteObj2);
			}
			m_targetIcons.OutlineWidth = 2;
			GameObjList.Add(m_targetIcons);
			GameObjList.Add(m_daggerIcons);
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
			foreach (GameObj current in GameObjList)
			{
				if (current is WaypointObj)
				{
					m_elf.X = current.X;
				}
				if (current.Name == "Line")
				{
					m_line = current;
				}
				if (current.Name == "Balloon")
				{
					m_balloonList.Add(current as ObjContainer);
					(current as ObjContainer).GetChildAt(1).TextureColor = array[CDGMath.RandomInt(0, array.Length - 1)];
				}
			}
			float num = 0f;
			foreach (TerrainObj current2 in TerrainObjList)
			{
				if (current2.Name == "Floor")
				{
					m_elf.Y = current2.Y - (m_elf.Bounds.Bottom - m_elf.Y);
					num = current2.Y;
					break;
				}
			}
			if (!IsReversed)
			{
				m_elf.Flip = SpriteEffects.FlipHorizontally;
			}
			GameObjList.Add(m_elf);
			m_elf.Y -= 2f;
			m_targetList = new List<BreakableObj>();
			for (int i = 0; i < m_numTargets; i++)
			{
				BreakableObj breakableObj = new BreakableObj("Target1_Character");
				breakableObj.Scale = new Vector2(2f, 2f);
				breakableObj.Visible = false;
				breakableObj.DropItem = false;
				breakableObj.HitBySpellsOnly = true;
				breakableObj.Position = m_line.Position;
				if (!IsReversed)
				{
					breakableObj.Flip = SpriteEffects.FlipHorizontally;
				}
				else
				{
					breakableObj.Flip = SpriteEffects.None;
				}
				m_targetList.Add(breakableObj);
				GameObjList.Add(breakableObj);
			}
			m_rewardChest = new ChestObj(null);
			m_rewardChest.ChestType = 3;
			if (!IsReversed)
			{
				m_rewardChest.Flip = SpriteEffects.FlipHorizontally;
				m_rewardChest.Position = new Vector2(m_elf.X + 100f, num - m_rewardChest.Height - 8f);
			}
			else
			{
				m_rewardChest.Position = new Vector2(m_elf.X - 150f, num - m_rewardChest.Height - 8f);
			}
			m_rewardChest.Visible = false;
			GameObjList.Add(m_rewardChest);
			base.Initialize();
		}
		public override void OnEnter()
		{
			m_rewardChest.ChestType = 3;
			if (m_rewardChest.PhysicsMngr == null)
			{
				Player.PhysicsMngr.AddObject(m_rewardChest);
			}
			m_spokeToNPC = false;
			Player.AttachedLevel.CameraLockedToPlayer = false;
			if (!IsReversed)
			{
				Player.AttachedLevel.Camera.Position = new Vector2(Bounds.Left + Player.AttachedLevel.Camera.Width / 2, Bounds.Top + Player.AttachedLevel.Camera.Height / 2);
			}
			else
			{
				Player.AttachedLevel.Camera.Position = new Vector2(Bounds.Right - Player.AttachedLevel.Camera.Width / 2, Bounds.Top + Player.AttachedLevel.Camera.Height / 2);
			}
			m_currentTargetIndex = 0;
			m_daggersThrown = 0;
			m_storedPlayerMana = Player.CurrentMana;
			m_storedPlayerSpell = Game.PlayerStats.Spell;
			InitializeTargetSystem();
			if (!IsReversed)
			{
				m_targetIcons.Position = new Vector2(Bounds.Right - 100 - m_targetIcons.Width, Bounds.Bottom - 40);
				m_daggerIcons.Position = m_targetIcons.Position;
				m_daggerIcons.X -= 400 + m_daggerIcons.Width;
			}
			else
			{
				m_targetIcons.Position = new Vector2(Bounds.Left + 150, Bounds.Bottom - 40);
				m_daggerIcons.Position = m_targetIcons.Position;
				m_daggerIcons.X += m_targetIcons.Width + 400;
			}
			m_daggerIcons.Y -= 30f;
			ReflipPosters();
			base.OnEnter();
		}
		private void ReflipPosters()
		{
			foreach (GameObj current in GameObjList)
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
			Player.AttachedLevel.ProjectileManager.DestroyAllProjectiles(true);
			Player.StopAllSpells();
			m_isPlayingGame = true;
			m_spokeToNPC = true;
			Player.AttachedLevel.CameraLockedToPlayer = false;
			Player.AttachedLevel.Camera.Y = Bounds.Center.Y;
			if (!IsReversed)
			{
				Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(m_line.X + 500f).ToString()
				});
			}
			else
			{
				Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(m_line.X - 500f).ToString()
				});
			}
			EquipPlayer();
			ActivateTarget();
		}
		public void EndGame()
		{
			if (!IsReversed)
			{
				Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(X + Player.AttachedLevel.Camera.Width / 2f).ToString()
				});
			}
			else
			{
				Tween.To(Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					(Bounds.Right - Player.AttachedLevel.Camera.Width / 2f).ToString()
				});
			}
			Tween.AddEndHandlerToLastTween(this, "CheckPlayerReward", new object[0]);
			m_isPlayingGame = false;
			Game.PlayerStats.Spell = 0;
			Player.AttachedLevel.UpdatePlayerSpellIcon();
			RoomCompleted = true;
		}
		public void CheckPlayerReward()
		{
			if (ActiveTargets <= 0)
			{
				RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
				rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Reward");
				(Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
				RevealChest();
				GameUtil.UnlockAchievement("LOVE_OF_CLOWNS");
				return;
			}
			RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
			rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom1-Fail");
			(Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
		}
		public void RevealChest()
		{
			Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(m_rewardChest.Position);
			m_rewardChest.Visible = true;
		}
		private void InitializeTargetSystem()
		{
			foreach (BreakableObj current in m_targetList)
			{
				current.Reset();
				current.Visible = false;
				if (!IsReversed)
				{
					current.Position = new Vector2(Bounds.Right, Bounds.Center.Y);
					current.Flip = SpriteEffects.FlipHorizontally;
				}
				else
				{
					current.Position = new Vector2(Bounds.Left, Bounds.Center.Y);
					current.Flip = SpriteEffects.None;
				}
			}
		}
		private void EquipPlayer()
		{
			Game.PlayerStats.Spell = 1;
			Player.AttachedLevel.UpdatePlayerSpellIcon();
			Player.CurrentMana = Player.MaxMana;
		}
		public void UnequipPlayer()
		{
			Game.PlayerStats.Spell = m_storedPlayerSpell;
			Player.AttachedLevel.UpdatePlayerSpellIcon();
			Player.CurrentMana = m_storedPlayerMana;
		}
		public override void OnExit()
		{
			UnequipPlayer();
			Player.AttachedLevel.CameraLockedToPlayer = true;
			base.OnExit();
		}
		private void HandleInput()
		{
			if (m_isPlayingGame && (Game.GlobalInput.JustPressed(24) || (Game.GlobalInput.JustPressed(12) && Game.PlayerStats.Class == 16)) && Player.SpellCastDelay <= 0f)
			{
				m_daggersThrown++;
				Player.CurrentMana = Player.MaxMana;
				if (m_daggersThrown <= m_numTries)
				{
					m_daggerIcons.GetChildAt(m_numTries - m_daggersThrown).Visible = false;
				}
				if (m_daggersThrown > m_numTries)
				{
					Game.PlayerStats.Spell = 0;
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
			m_elf.Update(gameTime, Player);
			if (!IsReversed)
			{
				if (Player.X >= m_line.X - 150f)
				{
					Player.X = (int)m_line.X - 150;
				}
			}
			else if (Player.X < m_line.X + 150f)
			{
				Player.X = m_line.X + 150f;
			}
			if (!IsReversed)
			{
				if (m_isPlayingGame && Player.X < Player.AttachedLevel.Camera.Bounds.Left)
				{
					Player.X = Player.AttachedLevel.Camera.Bounds.Left;
				}
				if (Player.X > Bounds.Right - 1320)
				{
					Player.X = Bounds.Right - 1320;
				}
			}
			else
			{
				if (m_isPlayingGame && Player.X > Player.AttachedLevel.Camera.Bounds.Right)
				{
					Player.X = Player.AttachedLevel.Camera.Bounds.Right;
				}
				if (Player.X < Bounds.Left + 1320)
				{
					Player.X = Bounds.Left + 1320;
				}
			}
			if (m_currentTarget != null && !m_currentTarget.Broken)
			{
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (m_targetMovingUp && m_currentTarget.Bounds.Top > Bounds.Top + 80)
				{
					m_currentTarget.Y -= num * m_targetSpeed;
				}
				else if (m_targetMovingUp)
				{
					m_currentTarget.Y += num * m_targetSpeed;
					m_targetMovingUp = false;
				}
				if (!m_targetMovingUp && m_currentTarget.Bounds.Bottom < Bounds.Bottom - 140)
				{
					m_currentTarget.Y += num * m_targetSpeed;
				}
				else if (!m_targetMovingUp)
				{
					m_currentTarget.Y -= num * m_targetSpeed;
					m_targetMovingUp = true;
				}
			}
			if (m_isPlayingGame && ((m_daggersThrown >= m_numTries && Player.AttachedLevel.ProjectileManager.ActiveProjectiles < 1 && ActiveTargets > 0) || ActiveTargets <= 0))
			{
				EndGame();
			}
			if (m_currentTarget != null && m_currentTarget.Broken && ActiveTargets >= 0)
			{
				m_currentTargetIndex++;
				ActivateTarget();
			}
			if (m_elf.IsTouching && !RoomCompleted && !m_spokeToNPC)
			{
				if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					RCScreenManager rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
					rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom1-Start");
					rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
					rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "BeginGame", new object[0]);
					rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
					{
						"Canceling Selection"
					});
					(Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
				}
			}
			else if (m_elf.IsTouching && RoomCompleted && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
			{
				RCScreenManager rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
				rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom1-End");
				(Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
			}
			if (m_isPlayingGame)
			{
				m_elf.CanTalk = false;
			}
			else
			{
				m_elf.CanTalk = true;
			}
			float totalGameTime = Game.TotalGameTime;
			float num2 = 2f;
			foreach (ObjContainer current in m_balloonList)
			{
				current.Rotation = (float)Math.Sin(totalGameTime * num2) * num2;
				num2 += 0.2f;
			}
			HandleInput();
			base.Update(gameTime);
		}
		public void ActivateTarget()
		{
			if (m_numTargets - m_currentTargetIndex < m_targetIcons.NumChildren)
			{
				m_targetIcons.GetChildAt(m_numTargets - m_currentTargetIndex).Visible = false;
				GiveGold();
			}
			if (m_currentTargetIndex >= m_numTargets)
			{
				m_currentTarget = null;
				return;
			}
			if (m_currentTarget != null)
			{
				m_targetSpeed += m_targetSpeedMod;
			}
			m_currentTarget = m_targetList[m_currentTargetIndex];
			m_currentTarget.Visible = true;
			if (!IsReversed)
			{
				Tween.By(m_currentTarget, 2f, new Easing(Quad.EaseOut), new string[]
				{
					"X",
					(-400 + CDGMath.RandomInt(-200, 200)).ToString()
				});
				return;
			}
			Tween.By(m_currentTarget, 2f, new Easing(Quad.EaseOut), new string[]
			{
				"X",
				(400 + CDGMath.RandomInt(-200, 200)).ToString()
			});
		}
		public void GiveGold()
		{
			int num = m_numTargets - ActiveTargets;
			if (ActiveTargets > 0)
			{
				Player.AttachedLevel.ImpactEffectPool.CarnivalGoldEffect(m_currentTarget.Position, new Vector2(Player.AttachedLevel.Camera.TopLeftCorner.X + 50f, Player.AttachedLevel.Camera.TopLeftCorner.Y + 135f), num);
			}
			Player.AttachedLevel.TextManager.DisplayNumberStringText(num * 10, " gold", Color.Yellow, m_currentTarget.Position);
			Game.PlayerStats.Gold += num * 10;
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_targetList.Clear();
				m_targetList = null;
				m_line = null;
				m_currentTarget = null;
				m_elf = null;
				m_daggerIcons = null;
				m_targetIcons = null;
				m_balloonList.Clear();
				m_balloonList = null;
				m_rewardChest = null;
				base.Dispose();
			}
		}
	}
}
