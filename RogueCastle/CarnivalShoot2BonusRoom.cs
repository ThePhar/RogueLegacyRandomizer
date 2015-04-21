using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class CarnivalShoot2BonusRoom : BonusRoomObj
	{
		private int m_numTries = 5;
		private Rectangle m_targetBounds;
		private byte m_storedPlayerSpell;
		private float m_storedPlayerMana;
		private bool m_isPlayingGame;
		private int m_axesThrown;
		private List<BreakableObj> m_targetList;
		private ObjContainer m_axeIcons;
		private NpcObj m_elf;
		private PhysicsObj m_gate;
		private bool m_gateClosed;
		private List<TextObj> m_targetDataText;
		private List<TextObj> m_targetText;
		private List<ObjContainer> m_balloonList;
		private ChestObj m_rewardChest;
		public int TargetsDestroyed
		{
			get
			{
				int num = 0;
				foreach (BreakableObj current in this.m_targetList)
				{
					if (current.Broken)
					{
						num++;
					}
				}
				return num;
			}
		}
		public int TargetsRemaining
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
		public CarnivalShoot2BonusRoom()
		{
			this.m_targetList = new List<BreakableObj>();
			this.m_elf = new NpcObj("Clown_Character");
			this.m_elf.Scale = new Vector2(2f, 2f);
			this.m_targetText = new List<TextObj>();
			this.m_targetDataText = new List<TextObj>();
			this.m_balloonList = new List<ObjContainer>();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 25f;
			textObj.Text = "test text";
			textObj.DropShadow = new Vector2(2f, 2f);
			if (!base.IsReversed)
			{
				textObj.Position = new Vector2((float)(this.Bounds.Right - 1000), (float)(this.Bounds.Top + 200));
			}
			else
			{
				textObj.Position = new Vector2((float)(this.Bounds.Left + 300), (float)(this.Bounds.Top + 200));
			}
			for (int i = 0; i < 3; i++)
			{
				TextObj textObj2 = textObj.Clone() as TextObj;
				textObj2.Y += (float)(i * 100);
				this.m_targetText.Add(textObj2);
				TextObj textObj3 = textObj.Clone() as TextObj;
				textObj3.Y += (float)(i * 100);
				textObj3.X = textObj2.X + 500f;
				this.m_targetDataText.Add(textObj3);
			}
			this.m_axeIcons = new ObjContainer();
			int num = 0;
			int num2 = 10;
			for (int j = 0; j < this.m_numTries; j++)
			{
				SpriteObj spriteObj = new SpriteObj("SpellAxe_Sprite");
				spriteObj.Scale = new Vector2(2f, 2f);
				spriteObj.X = (float)(num + 10);
				spriteObj.Y = (float)num2;
				num += spriteObj.Width + 5;
				this.m_axeIcons.AddChild(spriteObj);
			}
			this.m_axeIcons.OutlineWidth = 2;
			base.GameObjList.Add(this.m_axeIcons);
			base.LoadContent(graphics);
		}
		public override void Initialize()
		{
			this.m_gate = new PhysicsObj("CastleEntranceGate_Sprite", null);
			this.m_gate.IsWeighted = false;
			this.m_gate.IsCollidable = true;
			this.m_gate.CollisionTypeTag = 1;
			this.m_gate.Layer = -1f;
			this.m_gate.OutlineWidth = 2;
			base.GameObjList.Add(this.m_gate);
			Rectangle rectangle = default(Rectangle);
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
				if (current.Name == "Balloon")
				{
					this.m_balloonList.Add(current as ObjContainer);
					(current as ObjContainer).GetChildAt(1).TextureColor = array[CDGMath.RandomInt(0, array.Length - 1)];
				}
			}
			foreach (TerrainObj current2 in base.TerrainObjList)
			{
				if (current2.Name == "Floor")
				{
					this.m_elf.Y = current2.Y - ((float)this.m_elf.Bounds.Bottom - this.m_elf.Y) - 2f;
				}
				if (current2.Name == "GatePosition")
				{
					rectangle = current2.Bounds;
				}
			}
			this.m_gate.X = (float)rectangle.X;
			this.m_gate.Y = (float)rectangle.Bottom;
			if (!base.IsReversed)
			{
				this.m_elf.Flip = SpriteEffects.FlipHorizontally;
			}
			base.GameObjList.Add(this.m_elf);
			foreach (TerrainObj current3 in base.TerrainObjList)
			{
				if (current3.Name == "Boundary")
				{
					current3.Visible = false;
					this.m_targetBounds = current3.Bounds;
					break;
				}
			}
			float num = 10f;
			float num2 = (float)(this.m_targetBounds.X + 40);
			float num3 = (float)this.m_targetBounds.Y;
			float num4 = (float)this.m_targetBounds.Width / num;
			float num5 = (float)this.m_targetBounds.Height / num;
			int num6 = 0;
			while ((float)num6 < num * num)
			{
				BreakableObj breakableObj = new BreakableObj("Target2_Character");
				breakableObj.X = num2;
				breakableObj.Y = num3;
				breakableObj.Scale = new Vector2(1.6f, 1.6f);
				breakableObj.OutlineWidth = 2;
				breakableObj.HitBySpellsOnly = true;
				breakableObj.IsWeighted = false;
				this.m_targetList.Add(breakableObj);
				breakableObj.SameTypesCollide = false;
				breakableObj.DropItem = false;
				base.GameObjList.Add(breakableObj);
				num2 += num4;
				if ((float)(num6 + 1) % num == 0f)
				{
					num2 = (float)(this.m_targetBounds.X + 40);
					num3 += num5;
				}
				num6++;
			}
			this.m_rewardChest = new ChestObj(null);
			this.m_rewardChest.ChestType = 3;
			if (!base.IsReversed)
			{
				this.m_rewardChest.Flip = SpriteEffects.FlipHorizontally;
				this.m_rewardChest.Position = new Vector2(this.m_elf.X + 100f, (float)(this.m_elf.Bounds.Bottom - this.m_rewardChest.Height - 8));
			}
			else
			{
				this.m_rewardChest.Position = new Vector2(this.m_elf.X - 150f, (float)(this.m_elf.Bounds.Bottom - this.m_rewardChest.Height - 8));
			}
			this.m_rewardChest.Visible = false;
			base.GameObjList.Add(this.m_rewardChest);
			base.Initialize();
		}
		public override void OnEnter()
		{
			this.m_rewardChest.ChestType = 3;
			if (!base.IsReversed)
			{
				this.m_axeIcons.Position = new Vector2((float)(this.Bounds.Right - 200 - this.m_axeIcons.Width), (float)(this.Bounds.Bottom - 60));
			}
			else
			{
				this.m_axeIcons.Position = new Vector2((float)(this.Bounds.Left + 900), (float)(this.Bounds.Bottom - 60));
			}
			this.m_targetText[0].Text = "Targets Destroyed:";
			this.m_targetText[1].Text = "Targets Remaining:";
			this.m_targetText[2].Text = "Reward:";
			this.m_targetDataText[0].Text = "50";
			this.m_targetDataText[1].Text = "10";
			this.m_targetDataText[2].Text = "100 gold";
			for (int i = 0; i < this.m_targetText.Count; i++)
			{
				this.m_targetText[i].Opacity = 0f;
				this.m_targetDataText[i].Opacity = 0f;
			}
			foreach (BreakableObj current in this.m_targetList)
			{
				current.Opacity = 0f;
				current.Visible = false;
			}
			this.m_gateClosed = true;
			this.m_storedPlayerSpell = Game.PlayerStats.Spell;
			this.m_storedPlayerMana = this.Player.CurrentMana;
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
		public override void OnExit()
		{
			Game.PlayerStats.Spell = this.m_storedPlayerSpell;
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.CurrentMana = this.m_storedPlayerMana;
			base.OnExit();
		}
		public void BeginGame()
		{
			this.Player.AttachedLevel.ProjectileManager.DestroyAllProjectiles(true);
			this.Player.StopAllSpells();
			this.m_gateClosed = false;
			Tween.By(this.m_gate, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				(-this.m_gate.Height).ToString()
			});
			this.m_isPlayingGame = true;
			this.EquipPlayer();
			float num = 0f;
			foreach (BreakableObj current in this.m_targetList)
			{
				current.Visible = true;
				Tween.To(current, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					num.ToString(),
					"Opacity",
					"1"
				});
				num += 0.01f;
			}
		}
		private void EndGame()
		{
			this.Player.LockControls();
			this.Player.CurrentSpeed = 0f;
			foreach (BreakableObj current in this.m_targetList)
			{
				Tween.To(current, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"0"
				});
			}
			Tween.AddEndHandlerToLastTween(this, "EndGame2", new object[0]);
			this.m_isPlayingGame = false;
		}
		public void EndGame2()
		{
			this.m_targetDataText[0].Text = this.TargetsDestroyed.ToString();
			this.m_targetDataText[1].Text = this.TargetsRemaining.ToString();
			int num = (int)((float)this.TargetsDestroyed / 2f) * 10;
			this.m_targetDataText[2].Text = num + " gold";
			float num2 = 0f;
			for (int i = 0; i < this.m_targetDataText.Count; i++)
			{
				Tween.To(this.m_targetText[i], 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					num2.ToString(),
					"Opacity",
					"1"
				});
				Tween.To(this.m_targetDataText[i], 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					num2.ToString(),
					"Opacity",
					"1"
				});
				num2 += 0.5f;
			}
			Tween.AddEndHandlerToLastTween(this, "GiveReward", new object[]
			{
				num
			});
		}
		public void GiveReward(int gold)
		{
			if ((!base.IsReversed && this.Player.X < (float)this.Bounds.Right - (float)this.Player.AttachedLevel.Camera.Width / 2f) || (base.IsReversed && this.Player.X > (float)this.Bounds.Left + (float)this.Player.AttachedLevel.Camera.Width / 2f))
			{
				Tween.To(this.Player.AttachedLevel.Camera, 0.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"X",
					this.Player.X.ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "ResetCamera", new object[0]);
			}
			else
			{
				this.ResetCamera();
			}
			this.Player.AttachedLevel.TextManager.DisplayNumberStringText(gold, " gold", Color.Yellow, this.Player.Position);
			Game.PlayerStats.Gold += gold;
			Tween.By(this.m_gate, 0.5f, new Easing(Quad.EaseInOut), new string[]
			{
				"Y",
				(-this.m_gate.Height).ToString()
			});
			this.m_gateClosed = false;
			base.RoomCompleted = true;
			Tween.AddEndHandlerToLastTween(this, "CheckPlayerReward", new object[0]);
		}
		public void CheckPlayerReward()
		{
			if (this.TargetsRemaining <= 10)
			{
				RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
				rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom2-Reward");
				(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
				this.RevealChest();
				GameUtil.UnlockAchievement("LOVE_OF_CLOWNS");
				return;
			}
			RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
			rCScreenManager2.DialogueScreen.SetDialogue("CarnivalRoom2-Fail");
			(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
		}
		public void RevealChest()
		{
			this.Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(this.m_rewardChest.Position);
			this.m_rewardChest.Visible = true;
		}
		public void ResetCamera()
		{
			this.Player.UnlockControls();
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
		}
		private void EquipPlayer()
		{
			Game.PlayerStats.Spell = 2;
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.CurrentMana = this.Player.MaxMana;
		}
		public void UnequipPlayer()
		{
			Game.PlayerStats.Spell = this.m_storedPlayerSpell;
			this.Player.AttachedLevel.UpdatePlayerSpellIcon();
			this.Player.CurrentMana = this.m_storedPlayerMana;
		}
		public override void Update(GameTime gameTime)
		{
			if ((this.m_axesThrown >= this.m_numTries && this.m_isPlayingGame && this.Player.AttachedLevel.ProjectileManager.ActiveProjectiles < 1) || (this.m_isPlayingGame && this.TargetsDestroyed >= 100))
			{
				this.EndGame();
			}
			if (this.m_isPlayingGame && !this.m_gateClosed && ((!base.IsReversed && this.Player.X > (float)this.m_gate.Bounds.Right) || (base.IsReversed && this.Player.X < (float)this.m_gate.Bounds.Left)))
			{
				this.Player.LockControls();
				this.Player.CurrentSpeed = 0f;
				this.Player.AccelerationX = 0f;
				Tween.By(this.m_gate, 0.5f, new Easing(Quad.EaseInOut), new string[]
				{
					"Y",
					this.m_gate.Height.ToString()
				});
				Tween.AddEndHandlerToLastTween(this.Player, "UnlockControls", new object[0]);
				this.m_gateClosed = true;
				this.Player.AttachedLevel.CameraLockedToPlayer = false;
				if (!base.IsReversed)
				{
					Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
					{
						"X",
						((float)this.Bounds.Right - (float)this.Player.AttachedLevel.Camera.Width / 2f).ToString()
					});
				}
				else
				{
					Tween.To(this.Player.AttachedLevel.Camera, 1f, new Easing(Quad.EaseInOut), new string[]
					{
						"X",
						((float)this.Bounds.Left + (float)this.Player.AttachedLevel.Camera.Width / 2f).ToString()
					});
				}
			}
			this.m_elf.Update(gameTime, this.Player);
			if (this.m_isPlayingGame)
			{
				this.m_elf.CanTalk = false;
			}
			else
			{
				this.m_elf.CanTalk = true;
			}
			if (this.m_elf.IsTouching && !base.RoomCompleted && !this.m_isPlayingGame)
			{
				if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
				{
					RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
					rCScreenManager.DialogueScreen.SetDialogue("CarnivalRoom2-Start");
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
			if (!base.IsReversed && this.m_isPlayingGame && this.Player.X < (float)(this.Bounds.Left + 10))
			{
				this.Player.X = (float)(this.Bounds.Left + 10);
			}
			else if (base.IsReversed && this.m_isPlayingGame && this.Player.X > (float)(this.Bounds.Right - 10))
			{
				this.Player.X = (float)(this.Bounds.Right - 10);
			}
			float totalGameTime = Game.TotalGameTime;
			float num = 2f;
			foreach (ObjContainer current in this.m_balloonList)
			{
				current.Rotation = (float)Math.Sin((double)(totalGameTime * num)) * num;
				num += 0.2f;
			}
			this.HandleInput();
			base.Update(gameTime);
		}
		private void HandleInput()
		{
			if (this.m_isPlayingGame && (Game.GlobalInput.JustPressed(24) || (Game.GlobalInput.JustPressed(12) && Game.PlayerStats.Class == 16)) && this.Player.SpellCastDelay <= 0f && this.m_gateClosed)
			{
				this.m_axesThrown++;
				this.Player.CurrentMana = this.Player.MaxMana;
				if (this.m_axesThrown <= this.m_numTries)
				{
					this.m_axeIcons.GetChildAt(this.m_numTries - this.m_axesThrown).Visible = false;
				}
				if (this.m_axesThrown > this.m_numTries)
				{
					Game.PlayerStats.Spell = 0;
				}
			}
		}
		public override void Draw(Camera2D camera)
		{
			base.Draw(camera);
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			for (int i = 0; i < this.m_targetText.Count; i++)
			{
				this.m_targetText[i].Draw(camera);
				this.m_targetDataText[i].Draw(camera);
			}
			camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_targetList.Clear();
				this.m_targetList = null;
				this.m_elf = null;
				this.m_axeIcons = null;
				this.m_gate = null;
				for (int i = 0; i < this.m_targetText.Count; i++)
				{
					this.m_targetText[i].Dispose();
					this.m_targetDataText[i].Dispose();
				}
				this.m_targetText.Clear();
				this.m_targetText = null;
				this.m_targetDataText.Clear();
				this.m_targetDataText = null;
				this.m_balloonList.Clear();
				this.m_balloonList = null;
				this.m_rewardChest = null;
				base.Dispose();
			}
		}
	}
}
