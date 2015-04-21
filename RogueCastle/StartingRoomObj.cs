using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class StartingRoomObj : RoomObj
	{
		private const int ENCHANTRESS_HEAD_LAYER = 4;
		private const byte ARCHITECT_HEAD_LAYER = 1;
		private BlacksmithObj m_blacksmith;
		private SpriteObj m_blacksmithIcon;
		private Vector2 m_blacksmithIconPosition;
		private ObjContainer m_enchantress;
		private SpriteObj m_enchantressIcon;
		private Vector2 m_enchantressIconPosition;
		private ObjContainer m_architect;
		private SpriteObj m_architectIcon;
		private Vector2 m_architectIconPosition;
		private bool m_architectRenovating;
		private float m_screenShakeCounter;
		private FrameSoundObj m_blacksmithAnvilSound;
		private GameObj m_tree1;
		private GameObj m_tree2;
		private GameObj m_tree3;
		private GameObj m_fern1;
		private GameObj m_fern2;
		private GameObj m_fern3;
		private bool m_isRaining;
		private List<RaindropObj> m_rainFG;
		private Cue m_rainSFX;
		private SpriteObj m_tent;
		private SpriteObj m_blacksmithBoard;
		private SpriteObj m_screw;
		private PhysicsObjContainer m_tollCollector;
		private SpriteObj m_tollCollectorIcon;
		private bool m_playerWalkedOut;
		private SpriteObj m_mountain1;
		private SpriteObj m_mountain2;
		private float m_lightningTimer;
		private SpriteObj m_blacksmithNewIcon;
		private SpriteObj m_enchantressNewIcon;
		private TerrainObj m_blacksmithBlock;
		private TerrainObj m_enchantressBlock;
		private TerrainObj m_architectBlock;
		private bool m_controlsLocked;
		private bool m_isSnowing;
		private bool m_horizontalShake;
		private bool m_verticalShake;
		private bool m_shakeScreen;
		private float m_screenShakeMagnitude;
		private Vector2 m_shakeStartingPos;
		private bool BlacksmithNewIconVisible
		{
			get
			{
				foreach (byte[] current in Game.PlayerStats.GetBlueprintArray)
				{
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						byte b = array[i];
						if (b == 1)
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		private bool EnchantressNewIconVisible
		{
			get
			{
				foreach (byte[] current in Game.PlayerStats.GetRuneArray)
				{
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						byte b = array[i];
						if (b == 1)
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		private bool SmithyAvailable
		{
			get
			{
				return SkillSystem.GetSkill(SkillType.Smithy).ModifierAmount > 0f;
			}
		}
		private bool EnchantressAvailable
		{
			get
			{
				return SkillSystem.GetSkill(SkillType.Enchanter).ModifierAmount > 0f;
			}
		}
		private bool ArchitectAvailable
		{
			get
			{
				return SkillSystem.GetSkill(SkillType.Architect).ModifierAmount > 0f;
			}
		}
		private bool TollCollectorAvailable
		{
			get
			{
				return Game.PlayerStats.TimesDead > 0 && this.m_tollCollector.Visible;
			}
		}
		public StartingRoomObj()
		{
			this.m_blacksmith = new BlacksmithObj();
			this.m_blacksmith.Flip = SpriteEffects.FlipHorizontally;
			this.m_blacksmith.Scale = new Vector2(2.5f, 2.5f);
			this.m_blacksmith.Position = new Vector2(700f, 660f - ((float)this.m_blacksmith.Bounds.Bottom - this.m_blacksmith.Y) - 1f);
			this.m_blacksmith.OutlineWidth = 2;
			this.m_blacksmithBoard = new SpriteObj("StartRoomBlacksmithBoard_Sprite");
			this.m_blacksmithBoard.Scale = new Vector2(2f, 2f);
			this.m_blacksmithBoard.OutlineWidth = 2;
			this.m_blacksmithBoard.Position = new Vector2(this.m_blacksmith.X - (float)(this.m_blacksmithBoard.Width / 2) - 35f, (float)(this.m_blacksmith.Bounds.Bottom - this.m_blacksmithBoard.Height - 1));
			this.m_blacksmithIcon = new SpriteObj("UpArrowBubble_Sprite");
			this.m_blacksmithIcon.Scale = new Vector2(2f, 2f);
			this.m_blacksmithIcon.Visible = false;
			this.m_blacksmithIconPosition = new Vector2(this.m_blacksmith.X - 60f, this.m_blacksmith.Y - 10f);
			this.m_blacksmithIcon.Flip = this.m_blacksmith.Flip;
			this.m_blacksmithIcon.OutlineWidth = 2;
			this.m_blacksmithNewIcon = new SpriteObj("ExclamationSquare_Sprite");
			this.m_blacksmithNewIcon.Visible = false;
			this.m_blacksmithNewIcon.OutlineWidth = 2;
			this.m_enchantressNewIcon = (this.m_blacksmithNewIcon.Clone() as SpriteObj);
			this.m_enchantress = new ObjContainer("Enchantress_Character");
			this.m_enchantress.Scale = new Vector2(2f, 2f);
			this.m_enchantress.Flip = SpriteEffects.FlipHorizontally;
			this.m_enchantress.Position = new Vector2(1150f, 660f - ((float)this.m_enchantress.Bounds.Bottom - this.m_enchantress.AnchorY) - 2f);
			this.m_enchantress.PlayAnimation(true);
			this.m_enchantress.AnimationDelay = 0.1f;
			(this.m_enchantress.GetChildAt(4) as IAnimateableObj).StopAnimation();
			this.m_enchantress.OutlineWidth = 2;
			this.m_tent = new SpriteObj("StartRoomGypsyTent_Sprite");
			this.m_tent.Scale = new Vector2(1.5f, 1.5f);
			this.m_tent.OutlineWidth = 2;
			this.m_tent.Position = new Vector2(this.m_enchantress.X - (float)(this.m_tent.Width / 2) + 5f, (float)(this.m_enchantress.Bounds.Bottom - this.m_tent.Height));
			this.m_enchantressIcon = new SpriteObj("UpArrowBubble_Sprite");
			this.m_enchantressIcon.Scale = new Vector2(2f, 2f);
			this.m_enchantressIcon.Visible = false;
			this.m_enchantressIconPosition = new Vector2(this.m_enchantress.X - 60f, this.m_enchantress.Y - 100f);
			this.m_enchantressIcon.Flip = this.m_enchantress.Flip;
			this.m_enchantressIcon.OutlineWidth = 2;
			this.m_architect = new ObjContainer("ArchitectIdle_Character");
			this.m_architect.Flip = SpriteEffects.FlipHorizontally;
			this.m_architect.Scale = new Vector2(2f, 2f);
			this.m_architect.Position = new Vector2(1550f, 660f - ((float)this.m_architect.Bounds.Bottom - this.m_architect.AnchorY) - 2f);
			this.m_architect.PlayAnimation(true);
			this.m_architect.AnimationDelay = 0.1f;
			this.m_architect.OutlineWidth = 2;
			(this.m_architect.GetChildAt(1) as IAnimateableObj).StopAnimation();
			this.m_architectIcon = new SpriteObj("UpArrowBubble_Sprite");
			this.m_architectIcon.Scale = new Vector2(2f, 2f);
			this.m_architectIcon.Visible = false;
			this.m_architectIconPosition = new Vector2(this.m_architect.X - 60f, this.m_architect.Y - 100f);
			this.m_architectIcon.Flip = this.m_architect.Flip;
			this.m_architectIcon.OutlineWidth = 2;
			this.m_architectRenovating = false;
			this.m_screw = new SpriteObj("ArchitectGear_Sprite");
			this.m_screw.Scale = new Vector2(2f, 2f);
			this.m_screw.OutlineWidth = 2;
			this.m_screw.Position = new Vector2(this.m_architect.X + 30f, (float)(this.m_architect.Bounds.Bottom - 1));
			this.m_screw.AnimationDelay = 0.1f;
			this.m_tollCollector = new PhysicsObjContainer("NPCTollCollectorIdle_Character", null);
			this.m_tollCollector.Flip = SpriteEffects.FlipHorizontally;
			this.m_tollCollector.Scale = new Vector2(2.5f, 2.5f);
			this.m_tollCollector.IsWeighted = false;
			this.m_tollCollector.IsCollidable = true;
			this.m_tollCollector.Position = new Vector2(2565f, 420f - ((float)this.m_tollCollector.Bounds.Bottom - this.m_tollCollector.AnchorY));
			this.m_tollCollector.PlayAnimation(true);
			this.m_tollCollector.AnimationDelay = 0.1f;
			this.m_tollCollector.OutlineWidth = 2;
			this.m_tollCollector.CollisionTypeTag = 1;
			this.m_tollCollectorIcon = new SpriteObj("UpArrowBubble_Sprite");
			this.m_tollCollectorIcon.Scale = new Vector2(2f, 2f);
			this.m_tollCollectorIcon.Visible = false;
			this.m_tollCollectorIcon.Flip = this.m_tollCollector.Flip;
			this.m_tollCollectorIcon.OutlineWidth = 2;
			this.m_rainFG = new List<RaindropObj>();
			int num = 400;
			if (LevelEV.SAVE_FRAMES)
			{
				num /= 2;
			}
			for (int i = 0; i < num; i++)
			{
				RaindropObj item = new RaindropObj(new Vector2((float)CDGMath.RandomInt(-100, 2540), (float)CDGMath.RandomInt(-400, 720)));
				this.m_rainFG.Add(item);
			}
		}
		public override void Initialize()
		{
			foreach (TerrainObj current in base.TerrainObjList)
			{
				if (current.Name == "BlacksmithBlock")
				{
					this.m_blacksmithBlock = current;
				}
				if (current.Name == "EnchantressBlock")
				{
					this.m_enchantressBlock = current;
				}
				if (current.Name == "ArchitectBlock")
				{
					this.m_architectBlock = current;
				}
				if (current.Name == "bridge")
				{
					current.ShowTerrain = false;
				}
			}
			for (int i = 0; i < base.GameObjList.Count; i++)
			{
				if (base.GameObjList[i].Name == "Mountains 1")
				{
					this.m_mountain1 = (base.GameObjList[i] as SpriteObj);
				}
				if (base.GameObjList[i].Name == "Mountains 2")
				{
					this.m_mountain2 = (base.GameObjList[i] as SpriteObj);
				}
			}
			base.Initialize();
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			if (this.m_tree1 == null)
			{
				foreach (GameObj current in base.GameObjList)
				{
					if (current.Name == "Tree1")
					{
						this.m_tree1 = current;
					}
					else if (current.Name == "Tree2")
					{
						this.m_tree2 = current;
					}
					else if (current.Name == "Tree3")
					{
						this.m_tree3 = current;
					}
					else if (current.Name == "Fern1")
					{
						this.m_fern1 = current;
					}
					else if (current.Name == "Fern2")
					{
						this.m_fern2 = current;
					}
					else if (current.Name == "Fern3")
					{
						this.m_fern3 = current;
					}
				}
			}
			base.LoadContent(graphics);
		}
		public override void OnEnter()
		{
			if (Game.PlayerStats.SpecialItem == 9 && Game.PlayerStats.ChallengeEyeballBeaten)
			{
				Game.PlayerStats.SpecialItem = 0;
			}
			if (Game.PlayerStats.SpecialItem == 10 && Game.PlayerStats.ChallengeSkullBeaten)
			{
				Game.PlayerStats.SpecialItem = 0;
			}
			if (Game.PlayerStats.SpecialItem == 11 && Game.PlayerStats.ChallengeFireballBeaten)
			{
				Game.PlayerStats.SpecialItem = 0;
			}
			if (Game.PlayerStats.SpecialItem == 12 && Game.PlayerStats.ChallengeBlobBeaten)
			{
				Game.PlayerStats.SpecialItem = 0;
			}
			if (Game.PlayerStats.SpecialItem == 13 && Game.PlayerStats.ChallengeLastBossBeaten)
			{
				Game.PlayerStats.SpecialItem = 0;
			}
			this.Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
			this.m_isSnowing = (DateTime.Now.Month == 12 || DateTime.Now.Month == 1);
			if (this.m_isSnowing)
			{
				foreach (RaindropObj current in this.m_rainFG)
				{
					current.ChangeToSnowflake();
				}
			}
			if (!(Game.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map) && Game.PlayerStats.HasArchitectFee)
			{
				Game.PlayerStats.HasArchitectFee = false;
			}
			Game.PlayerStats.TutorialComplete = true;
			Game.PlayerStats.IsDead = false;
			this.m_lightningTimer = 5f;
			this.Player.CurrentHealth = this.Player.MaxHealth;
			this.Player.CurrentMana = this.Player.MaxMana;
			this.Player.ForceInvincible = false;
			(this.Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
			if (this.TollCollectorAvailable && !this.Player.AttachedLevel.PhysicsManager.ObjectList.Contains(this.m_tollCollector))
			{
				this.Player.AttachedLevel.PhysicsManager.AddObject(this.m_tollCollector);
			}
			if (this.m_blacksmithAnvilSound == null)
			{
				this.m_blacksmithAnvilSound = new FrameSoundObj(this.m_blacksmith.GetChildAt(5) as IAnimateableObj, this.Player, 7, new string[]
				{
					"Anvil1",
					"Anvil2",
					"Anvil3"
				});
			}
			if (Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f)
			{
				Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
			}
			else
			{
				Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
			}
			this.m_playerWalkedOut = false;
			this.Player.UpdateCollisionBoxes();
			this.Player.Position = new Vector2(0f, 660f - ((float)this.Player.Bounds.Bottom - this.Player.Y));
			this.Player.State = 1;
			this.Player.IsWeighted = false;
			this.Player.IsCollidable = false;
			LogicSet logicSet = new LogicSet(this.Player);
			logicSet.AddAction(new RunFunctionLogicAction(this.Player, "LockControls", new object[0]), Types.Sequence.Serial);
			logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f), -1f), Types.Sequence.Serial);
			logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character", true, true), Types.Sequence.Serial);
			logicSet.AddAction(new PlayAnimationLogicAction(true), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(0.5f, false), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "CurrentSpeed", 0), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsWeighted", true), Types.Sequence.Serial);
			logicSet.AddAction(new ChangePropertyLogicAction(this.Player, "IsCollidable", true), Types.Sequence.Serial);
			this.Player.RunExternalLogicSet(logicSet);
			Tween.By(this, 1f, new Easing(Linear.EaseNone), new string[0]);
			Tween.AddEndHandlerToLastTween(this.Player, "UnlockControls", new object[0]);
			SoundManager.StopMusic(1f);
			this.m_isRaining = (CDGMath.RandomPlusMinus() > 0);
			this.m_isRaining = true;
			if (this.m_isRaining)
			{
				if (this.m_rainSFX != null)
				{
					this.m_rainSFX.Dispose();
				}
				if (!this.m_isSnowing)
				{
					this.m_rainSFX = SoundManager.PlaySound("Rain1");
				}
				else
				{
					this.m_rainSFX = SoundManager.PlaySound("snowloop_filtered");
				}
			}
			this.m_tent.TextureColor = new Color(200, 200, 200);
			this.m_blacksmithBoard.TextureColor = new Color(200, 200, 200);
			this.m_screw.TextureColor = new Color(200, 200, 200);
			if (Game.PlayerStats.LockCastle)
			{
				this.m_screw.GoToFrame(this.m_screw.TotalFrames);
				this.m_architectBlock.Position = new Vector2(1492f, 579f);
			}
			else
			{
				this.m_screw.GoToFrame(1);
				this.m_architectBlock.Position = new Vector2(1492f, 439f);
			}
			this.Player.UpdateEquipmentColours();
			base.OnEnter();
		}
		public override void OnExit()
		{
			if (this.m_rainSFX != null && !this.m_rainSFX.IsDisposed)
			{
				this.m_rainSFX.Stop(AudioStopOptions.Immediate);
			}
		}
		public override void Update(GameTime gameTime)
		{
			this.Player.CurrentMana = this.Player.MaxMana;
			this.Player.CurrentHealth = this.Player.MaxHealth;
			this.m_enchantressBlock.Visible = this.EnchantressAvailable;
			this.m_blacksmithBlock.Visible = this.SmithyAvailable;
			this.m_architectBlock.Visible = this.ArchitectAvailable;
			float totalGameTime = Game.TotalGameTime;
			if (!this.m_playerWalkedOut)
			{
				if (!this.Player.ControlsLocked && this.Player.X < (float)this.Bounds.Left)
				{
					this.m_playerWalkedOut = true;
					(this.Player.AttachedLevel.ScreenManager as RCScreenManager).StartWipeTransition();
					Tween.RunFunction(0.2f, this.Player.AttachedLevel.ScreenManager, "DisplayScreen", new object[]
					{
						6,
						true,
						typeof(List<object>)
					});
				}
				else if (!this.Player.ControlsLocked && this.Player.X > (float)this.Bounds.Right && !this.TollCollectorAvailable)
				{
					this.m_playerWalkedOut = true;
					this.LoadLevel();
				}
			}
			if (this.m_isRaining)
			{
				foreach (TerrainObj current in base.TerrainObjList)
				{
					current.UseCachedValues = true;
				}
				foreach (RaindropObj current2 in this.m_rainFG)
				{
					current2.Update(base.TerrainObjList, gameTime);
				}
			}
			this.m_tree1.Rotation = -(float)Math.Sin((double)totalGameTime) * 2f;
			this.m_tree2.Rotation = (float)Math.Sin((double)(totalGameTime * 2f));
			this.m_tree3.Rotation = (float)Math.Sin((double)(totalGameTime * 2f)) * 2f;
			this.m_fern1.Rotation = (float)Math.Sin((double)(totalGameTime * 3f)) / 2f;
			this.m_fern2.Rotation = -(float)Math.Sin((double)(totalGameTime * 4f));
			this.m_fern3.Rotation = (float)Math.Sin((double)(totalGameTime * 4f)) / 2f;
			if (!this.m_architectRenovating)
			{
				this.HandleInput();
			}
			if (this.SmithyAvailable)
			{
				if (this.m_blacksmithAnvilSound != null)
				{
					this.m_blacksmithAnvilSound.Update();
				}
				this.m_blacksmith.Update(gameTime);
			}
			this.m_blacksmithIcon.Visible = false;
			if (this.Player != null && CollisionMath.Intersects(this.Player.TerrainBounds, this.m_blacksmith.Bounds) && this.Player.IsTouchingGround && this.SmithyAvailable)
			{
				this.m_blacksmithIcon.Visible = true;
			}
			this.m_blacksmithIcon.Position = new Vector2(this.m_blacksmithIconPosition.X, this.m_blacksmithIconPosition.Y - 70f + (float)Math.Sin((double)(totalGameTime * 20f)) * 2f);
			this.m_enchantressIcon.Visible = false;
			Rectangle b = new Rectangle((int)(this.m_enchantress.X - 100f), (int)this.m_enchantress.Y, this.m_enchantress.Bounds.Width + 100, this.m_enchantress.Bounds.Height);
			if (this.Player != null && CollisionMath.Intersects(this.Player.TerrainBounds, b) && this.Player.IsTouchingGround && this.EnchantressAvailable)
			{
				this.m_enchantressIcon.Visible = true;
			}
			this.m_enchantressIcon.Position = new Vector2(this.m_enchantressIconPosition.X + 20f, this.m_enchantressIconPosition.Y + (float)Math.Sin((double)(totalGameTime * 20f)) * 2f);
			if (this.Player != null && CollisionMath.Intersects(this.Player.TerrainBounds, new Rectangle((int)this.m_architect.X - 100, (int)this.m_architect.Y, this.m_architect.Width + 200, this.m_architect.Height)) && this.Player.X < this.m_architect.X && this.Player.Flip == SpriteEffects.None && this.ArchitectAvailable)
			{
				this.m_architectIcon.Visible = true;
			}
			else
			{
				this.m_architectIcon.Visible = false;
			}
			this.m_architectIcon.Position = new Vector2(this.m_architectIconPosition.X, this.m_architectIconPosition.Y + (float)Math.Sin((double)(totalGameTime * 20f)) * 2f);
			if (this.Player != null && CollisionMath.Intersects(this.Player.TerrainBounds, new Rectangle((int)this.m_tollCollector.X - 100, (int)this.m_tollCollector.Y, this.m_tollCollector.Width + 200, this.m_tollCollector.Height)) && this.Player.X < this.m_tollCollector.X && this.Player.Flip == SpriteEffects.None && this.TollCollectorAvailable && this.m_tollCollector.SpriteName == "NPCTollCollectorIdle_Character")
			{
				this.m_tollCollectorIcon.Visible = true;
			}
			else
			{
				this.m_tollCollectorIcon.Visible = false;
			}
			this.m_tollCollectorIcon.Position = new Vector2(this.m_tollCollector.X - (float)(this.m_tollCollector.Width / 2) - 10f, this.m_tollCollector.Y - (float)this.m_tollCollectorIcon.Height - (float)(this.m_tollCollector.Height / 2) + (float)Math.Sin((double)(totalGameTime * 20f)) * 2f);
			this.m_blacksmithNewIcon.Visible = false;
			if (this.SmithyAvailable)
			{
				if (this.m_blacksmithIcon.Visible && this.m_blacksmithNewIcon.Visible)
				{
					this.m_blacksmithNewIcon.Visible = false;
				}
				else if (!this.m_blacksmithIcon.Visible && this.BlacksmithNewIconVisible)
				{
					this.m_blacksmithNewIcon.Visible = true;
				}
				this.m_blacksmithNewIcon.Position = new Vector2(this.m_blacksmithIcon.X + 50f, this.m_blacksmithIcon.Y - 30f);
			}
			this.m_enchantressNewIcon.Visible = false;
			if (this.EnchantressAvailable)
			{
				if (this.m_enchantressIcon.Visible && this.m_enchantressNewIcon.Visible)
				{
					this.m_enchantressNewIcon.Visible = false;
				}
				else if (!this.m_enchantressIcon.Visible && this.EnchantressNewIconVisible)
				{
					this.m_enchantressNewIcon.Visible = true;
				}
				this.m_enchantressNewIcon.Position = new Vector2(this.m_enchantressIcon.X + 40f, this.m_enchantressIcon.Y - 0f);
			}
			if (this.m_isRaining && !this.m_isSnowing && this.m_lightningTimer > 0f)
			{
				this.m_lightningTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (this.m_lightningTimer <= 0f)
				{
					if (CDGMath.RandomInt(0, 100) > 70)
					{
						if (CDGMath.RandomInt(0, 1) > 0)
						{
							this.Player.AttachedLevel.LightningEffectTwice();
						}
						else
						{
							this.Player.AttachedLevel.LightningEffectOnce();
						}
					}
					this.m_lightningTimer = 5f;
				}
			}
			if (this.m_shakeScreen)
			{
				this.UpdateShake();
			}
			if (this.Player.Bounds.Right > this.m_tollCollector.Bounds.Left && this.TollCollectorAvailable)
			{
				this.Player.X = (float)this.m_tollCollector.Bounds.Left - ((float)this.Player.Bounds.Right - this.Player.X);
				this.Player.AttachedLevel.UpdateCamera();
			}
			base.Update(gameTime);
		}
		private void LoadLevel()
		{
			Game.ScreenManager.DisplayScreen(5, true, null);
		}
		private void HandleInput()
		{
			if (!this.m_controlsLocked)
			{
				if (this.Player.State != 4)
				{
					if (this.m_blacksmithIcon.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
					{
						this.MovePlayerTo(this.m_blacksmith);
					}
					if (this.m_enchantressIcon.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
					{
						this.MovePlayerTo(this.m_enchantress);
					}
					if (this.m_architectIcon.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
					{
						RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
						if ((Game.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map))
						{
							if (!Game.PlayerStats.LockCastle)
							{
								if (!Game.PlayerStats.SpokeToArchitect)
								{
									Game.PlayerStats.SpokeToArchitect = true;
									rCScreenManager.DialogueScreen.SetDialogue("Meet Architect");
								}
								else
								{
									rCScreenManager.DialogueScreen.SetDialogue("Meet Architect 2");
								}
								rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
								rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "ActivateArchitect", new object[0]);
								rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
								{
									"Canceling Selection"
								});
							}
							else
							{
								rCScreenManager.DialogueScreen.SetDialogue("Castle Already Locked Architect");
							}
						}
						else
						{
							rCScreenManager.DialogueScreen.SetDialogue("No Castle Architect");
						}
						rCScreenManager.DisplayScreen(13, true, null);
					}
				}
				if (this.m_tollCollectorIcon.Visible && (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
				{
					RCScreenManager rCScreenManager2 = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
					if (Game.PlayerStats.SpecialItem == 1)
					{
						Tween.RunFunction(0.1f, this, "TollPaid", new object[]
						{
							false
						});
						rCScreenManager2.DialogueScreen.SetDialogue("Toll Collector Obol");
						rCScreenManager2.DisplayScreen(13, true, null);
						return;
					}
					if (Game.PlayerStats.SpecialItem == 9)
					{
						rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Eyeball");
						this.RunTollPaidSelection(rCScreenManager2);
						return;
					}
					if (Game.PlayerStats.SpecialItem == 10)
					{
						rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Skull");
						this.RunTollPaidSelection(rCScreenManager2);
						return;
					}
					if (Game.PlayerStats.SpecialItem == 11)
					{
						rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Fireball");
						this.RunTollPaidSelection(rCScreenManager2);
						return;
					}
					if (Game.PlayerStats.SpecialItem == 12)
					{
						rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Blob");
						this.RunTollPaidSelection(rCScreenManager2);
						return;
					}
					if (Game.PlayerStats.SpecialItem == 13)
					{
						rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Last Boss");
						this.RunTollPaidSelection(rCScreenManager2);
						return;
					}
					if (!Game.PlayerStats.SpokeToTollCollector)
					{
						rCScreenManager2.DialogueScreen.SetDialogue("Meet Toll Collector 1");
					}
					else
					{
						float num = SkillSystem.GetSkill(SkillType.Prices_Down).ModifierAmount * 100f;
						rCScreenManager2.DialogueScreen.SetDialogue("Meet Toll Collector Skip" + (int)Math.Round((double)num, MidpointRounding.AwayFromZero));
					}
					this.RunTollPaidSelection(rCScreenManager2);
				}
			}
		}
		private void RunTollPaidSelection(RCScreenManager manager)
		{
			manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
			manager.DialogueScreen.SetConfirmEndHandler(this, "TollPaid", new object[]
			{
				true
			});
			manager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
			{
				"Canceling Selection"
			});
			manager.DisplayScreen(13, true, null);
		}
		public void MovePlayerTo(GameObj target)
		{
			this.m_controlsLocked = true;
			if (this.Player.X != target.X - 150f)
			{
				if (this.Player.X > target.Position.X - 150f)
				{
					this.Player.Flip = SpriteEffects.FlipHorizontally;
				}
				float num = CDGMath.DistanceBetweenPts(this.Player.Position, new Vector2(target.X - 150f, target.Y)) / this.Player.Speed;
				this.Player.UpdateCollisionBoxes();
				this.Player.State = 1;
				this.Player.IsWeighted = false;
				this.Player.AccelerationY = 0f;
				this.Player.AccelerationX = 0f;
				this.Player.IsCollidable = false;
				this.Player.CurrentSpeed = 0f;
				this.Player.LockControls();
				this.Player.ChangeSprite("PlayerWalking_Character");
				this.Player.PlayAnimation(true);
				LogicSet logicSet = new LogicSet(this.Player);
				logicSet.AddAction(new DelayLogicAction(num, false), Types.Sequence.Serial);
				this.Player.RunExternalLogicSet(logicSet);
				Tween.To(this.Player, num, new Easing(Tween.EaseNone), new string[]
				{
					"X",
					(target.Position.X - 150f).ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "MovePlayerComplete", new object[]
				{
					target
				});
				return;
			}
			this.MovePlayerComplete(target);
		}
		public void MovePlayerComplete(GameObj target)
		{
			this.m_controlsLocked = false;
			this.Player.IsWeighted = true;
			this.Player.IsCollidable = true;
			this.Player.UnlockControls();
			this.Player.Flip = SpriteEffects.None;
			if (target != this.m_blacksmith)
			{
				if (target == this.m_enchantress)
				{
					if (!Game.PlayerStats.SpokeToEnchantress)
					{
						Game.PlayerStats.SpokeToEnchantress = true;
						(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue("Meet Enchantress");
						DialogueScreen arg_1A1_0 = (this.Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen;
						object arg_1A1_1 = this.Player.AttachedLevel.ScreenManager;
						string arg_1A1_2 = "DisplayScreen";
						object[] array = new object[3];
						array[0] = 11;
						array[1] = true;
						arg_1A1_0.SetConfirmEndHandler(arg_1A1_1, arg_1A1_2, array);
						(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
						return;
					}
					(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(11, true, null);
				}
				return;
			}
			if (!Game.PlayerStats.SpokeToBlacksmith)
			{
				Game.PlayerStats.SpokeToBlacksmith = true;
				(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue("Meet Blacksmith");
				DialogueScreen arg_CA_0 = (this.Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen;
				object arg_CA_1 = this.Player.AttachedLevel.ScreenManager;
				string arg_CA_2 = "DisplayScreen";
				object[] array2 = new object[3];
				array2[0] = 10;
				array2[1] = true;
				arg_CA_0.SetConfirmEndHandler(arg_CA_1, arg_CA_2, array2);
				(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
				return;
			}
			(this.Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(10, true, null);
		}
		public void TollPaid(bool chargeFee)
		{
			if (chargeFee)
			{
				float num = (float)Game.PlayerStats.Gold * (1f - SkillSystem.GetSkill(SkillType.Prices_Down).ModifierAmount);
				Game.PlayerStats.Gold -= (int)num;
				if (num > 0f)
				{
					this.Player.AttachedLevel.TextManager.DisplayNumberStringText(-(int)num, "gold", Color.Yellow, new Vector2(this.Player.X, (float)this.Player.Bounds.Top));
				}
			}
			if (Game.PlayerStats.SpokeToTollCollector && Game.PlayerStats.SpecialItem != 1 && Game.PlayerStats.SpecialItem != 12 && Game.PlayerStats.SpecialItem != 13 && Game.PlayerStats.SpecialItem != 11 && Game.PlayerStats.SpecialItem != 9 && Game.PlayerStats.SpecialItem != 10)
			{
				this.Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(this.m_tollCollector.Position);
				SoundManager.PlaySound("Charon_Laugh");
				this.HideTollCollector();
			}
			else
			{
				Game.PlayerStats.SpokeToTollCollector = true;
				SoundManager.PlaySound("Charon_Laugh");
				this.m_tollCollector.ChangeSprite("NPCTollCollectorLaugh_Character");
				this.m_tollCollector.AnimationDelay = 0.05f;
				this.m_tollCollector.PlayAnimation(true);
				Tween.RunFunction(1f, this.Player.AttachedLevel.ImpactEffectPool, "DisplayDeathEffect", new object[]
				{
					this.m_tollCollector.Position
				});
				Tween.RunFunction(1f, this, "HideTollCollector", new object[0]);
			}
			if (Game.PlayerStats.SpecialItem == 1 || Game.PlayerStats.SpecialItem == 10 || Game.PlayerStats.SpecialItem == 9 || Game.PlayerStats.SpecialItem == 13 || Game.PlayerStats.SpecialItem == 11 || Game.PlayerStats.SpecialItem == 12)
			{
				if (Game.PlayerStats.SpecialItem == 9)
				{
					Game.PlayerStats.ChallengeEyeballUnlocked = true;
				}
				else if (Game.PlayerStats.SpecialItem == 10)
				{
					Game.PlayerStats.ChallengeSkullUnlocked = true;
				}
				else if (Game.PlayerStats.SpecialItem == 11)
				{
					Game.PlayerStats.ChallengeFireballUnlocked = true;
				}
				else if (Game.PlayerStats.SpecialItem == 12)
				{
					Game.PlayerStats.ChallengeBlobUnlocked = true;
				}
				else if (Game.PlayerStats.SpecialItem == 13)
				{
					Game.PlayerStats.ChallengeLastBossUnlocked = true;
				}
				Game.PlayerStats.SpecialItem = 0;
				this.Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
			}
		}
		public void HideTollCollector()
		{
			SoundManager.Play3DSound(this, this.Player, "Charon_Poof");
			this.m_tollCollector.Visible = false;
			this.Player.AttachedLevel.PhysicsManager.RemoveObject(this.m_tollCollector);
		}
		public void ActivateArchitect()
		{
			this.Player.LockControls();
			this.Player.CurrentSpeed = 0f;
			this.m_architectIcon.Visible = false;
			this.m_architectRenovating = true;
			this.m_architect.ChangeSprite("ArchitectPull_Character");
			(this.m_architect.GetChildAt(1) as SpriteObj).PlayAnimation(false);
			this.m_screw.AnimationDelay = 0.0333333351f;
			Tween.RunFunction(0.5f, this.m_architect.GetChildAt(0), "PlayAnimation", new object[]
			{
				true
			});
			Tween.RunFunction(0.5f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Architect_Lever"
			});
			Tween.RunFunction(1f, typeof(SoundManager), "PlaySound", new object[]
			{
				"Architect_Screw"
			});
			Tween.RunFunction(1f, this.m_screw, "PlayAnimation", new object[]
			{
				false
			});
			Tween.By(this.m_architectBlock, 0.8f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"1.1",
				"Y",
				"135"
			});
			Tween.RunFunction(1f, this, "ShakeScreen", new object[]
			{
				2,
				true,
				false
			});
			Tween.RunFunction(1.5f, this, "StopScreenShake", new object[0]);
			Tween.RunFunction(1.5f, this.Player.AttachedLevel.ImpactEffectPool, "SkillTreeDustEffect", new object[]
			{
				new Vector2(this.m_screw.X - (float)this.m_screw.Width / 2f, this.m_screw.Y - 40f),
				true,
				this.m_screw.Width
			});
			Tween.RunFunction(3f, this, "StopArchitectActivation", new object[0]);
		}
		public void StopArchitectActivation()
		{
			this.m_architectRenovating = false;
			this.m_architectIcon.Visible = true;
			this.Player.UnlockControls();
			Game.PlayerStats.LockCastle = true;
			Game.PlayerStats.HasArchitectFee = true;
			foreach (ChestObj current in this.Player.AttachedLevel.ChestList)
			{
				FairyChestObj fairyChestObj = current as FairyChestObj;
				if (fairyChestObj != null && fairyChestObj.State == 2)
				{
					fairyChestObj.ResetChest();
				}
			}
			foreach (RoomObj current2 in this.Player.AttachedLevel.RoomList)
			{
				foreach (GameObj current3 in current2.GameObjList)
				{
					BreakableObj breakableObj = current3 as BreakableObj;
					if (breakableObj != null)
					{
						breakableObj.Reset();
					}
				}
			}
			RCScreenManager rCScreenManager = this.Player.AttachedLevel.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("Castle Lock Complete Architect");
			rCScreenManager.DisplayScreen(13, true, null);
		}
		public override void Draw(Camera2D camera)
		{
			this.m_mountain1.X = camera.TopLeftCorner.X * 0.5f;
			this.m_mountain2.X = this.m_mountain1.X + 2640f;
			base.Draw(camera);
			if (this.m_isRaining)
			{
				camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 2640, 720), Color.Black * 0.3f);
			}
			if (this.m_screenShakeCounter > 0f)
			{
				camera.X += (float)CDGMath.RandomPlusMinus();
				camera.Y += (float)CDGMath.RandomPlusMinus();
				this.m_screenShakeCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
			}
			if (this.SmithyAvailable)
			{
				this.m_blacksmithBoard.Draw(camera);
				this.m_blacksmith.Draw(camera);
				this.m_blacksmithIcon.Draw(camera);
			}
			if (this.EnchantressAvailable)
			{
				this.m_tent.Draw(camera);
				this.m_enchantress.Draw(camera);
				this.m_enchantressIcon.Draw(camera);
			}
			if (this.ArchitectAvailable)
			{
				this.m_screw.Draw(camera);
				this.m_architect.Draw(camera);
				this.m_architectIcon.Draw(camera);
			}
			if (this.TollCollectorAvailable)
			{
				this.m_tollCollector.Draw(camera);
				this.m_tollCollectorIcon.Draw(camera);
			}
			this.m_blacksmithNewIcon.Draw(camera);
			this.m_enchantressNewIcon.Draw(camera);
			if (this.m_isRaining)
			{
				foreach (RaindropObj current in this.m_rainFG)
				{
					current.Draw(camera);
				}
			}
		}
		public override void PauseRoom()
		{
			foreach (RaindropObj current in this.m_rainFG)
			{
				current.PauseAnimation();
			}
			if (this.m_rainSFX != null)
			{
				this.m_rainSFX.Pause();
			}
			this.m_enchantress.PauseAnimation();
			this.m_blacksmith.PauseAnimation();
			this.m_architect.PauseAnimation();
			this.m_tollCollector.PauseAnimation();
			base.PauseRoom();
		}
		public override void UnpauseRoom()
		{
			foreach (RaindropObj current in this.m_rainFG)
			{
				current.ResumeAnimation();
			}
			if (this.m_rainSFX != null && this.m_rainSFX.IsPaused)
			{
				this.m_rainSFX.Resume();
			}
			this.m_enchantress.ResumeAnimation();
			this.m_blacksmith.ResumeAnimation();
			this.m_architect.ResumeAnimation();
			this.m_tollCollector.ResumeAnimation();
			base.UnpauseRoom();
		}
		public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
		{
			this.m_shakeStartingPos = this.Player.AttachedLevel.Camera.Position;
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			this.m_screenShakeMagnitude = magnitude;
			this.m_horizontalShake = horizontalShake;
			this.m_verticalShake = verticalShake;
			this.m_shakeScreen = true;
		}
		public void UpdateShake()
		{
			if (this.m_horizontalShake)
			{
				this.Player.AttachedLevel.Camera.X = this.m_shakeStartingPos.X + (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
			}
			if (this.m_verticalShake)
			{
				this.Player.AttachedLevel.Camera.Y = this.m_shakeStartingPos.Y + (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
			}
		}
		public void StopScreenShake()
		{
			this.Player.AttachedLevel.CameraLockedToPlayer = true;
			this.m_shakeScreen = false;
		}
		protected override GameObj CreateCloneInstance()
		{
			return new StartingRoomObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_blacksmith.Dispose();
				this.m_blacksmith = null;
				this.m_blacksmithIcon.Dispose();
				this.m_blacksmithIcon = null;
				this.m_blacksmithNewIcon.Dispose();
				this.m_blacksmithNewIcon = null;
				this.m_blacksmithBoard.Dispose();
				this.m_blacksmithBoard = null;
				this.m_enchantress.Dispose();
				this.m_enchantress = null;
				this.m_enchantressIcon.Dispose();
				this.m_enchantressIcon = null;
				this.m_enchantressNewIcon.Dispose();
				this.m_enchantressNewIcon = null;
				this.m_tent.Dispose();
				this.m_tent = null;
				this.m_architect.Dispose();
				this.m_architect = null;
				this.m_architectIcon.Dispose();
				this.m_architectIcon = null;
				this.m_screw.Dispose();
				this.m_screw = null;
				if (this.m_blacksmithAnvilSound != null)
				{
					this.m_blacksmithAnvilSound.Dispose();
				}
				this.m_blacksmithAnvilSound = null;
				this.m_tree1 = null;
				this.m_tree2 = null;
				this.m_tree3 = null;
				this.m_fern1 = null;
				this.m_fern2 = null;
				this.m_fern3 = null;
				foreach (RaindropObj current in this.m_rainFG)
				{
					current.Dispose();
				}
				this.m_rainFG.Clear();
				this.m_rainFG = null;
				this.m_mountain1 = null;
				this.m_mountain2 = null;
				this.m_tollCollector.Dispose();
				this.m_tollCollector = null;
				this.m_tollCollectorIcon.Dispose();
				this.m_tollCollectorIcon = null;
				this.m_blacksmithBlock = null;
				this.m_enchantressBlock = null;
				this.m_architectBlock = null;
				if (this.m_rainSFX != null)
				{
					this.m_rainSFX.Dispose();
				}
				this.m_rainSFX = null;
				base.Dispose();
			}
		}
	}
}
