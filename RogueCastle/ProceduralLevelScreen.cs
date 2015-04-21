using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class ProceduralLevelScreen : Screen
	{
		private const byte INPUT_TOGGLEMAP = 0;
		private const byte INPUT_TOGGLEZOOM = 1;
		private const byte INPUT_LEFTCONTROL = 2;
		private const byte INPUT_LEFT = 3;
		private const byte INPUT_RIGHT = 4;
		private const byte INPUT_UP = 5;
		private const byte INPUT_DOWN = 6;
		private const byte INPUT_DISPLAYROOMINFO = 7;
		private List<RoomObj> m_roomList;
		private PlayerObj m_player;
		protected int m_leftMostBorder = 2147483647;
		protected int m_rightMostBorder = -2147483647;
		protected int m_topMostBorder = 2147483647;
		protected int m_bottomMostBorder = -2147483647;
		protected int LeftDoorPercent = 80;
		protected int RightDoorPercent = 80;
		protected int TopDoorPercent = 80;
		protected int BottomDoorPercent = 80;
		protected TextManager m_textManager;
		protected PhysicsManager m_physicsManager;
		protected ProjectileManager m_projectileManager;
		protected ItemDropManager m_itemDropManager;
		protected RoomObj m_currentRoom;
		protected MapObj m_miniMapDisplay;
		private SpriteObj m_mapBG;
		private InputMap m_inputMap;
		private List<Vector2> m_enemyStartPositions;
		private List<Vector2> m_tempEnemyStartPositions;
		private PlayerHUDObj m_playerHUD;
		private EnemyHUDObj m_enemyHUD;
		private EnemyObj m_lastEnemyHit;
		private float m_enemyHUDDuration = 2f;
		private float m_enemyHUDCounter;
		private List<EnemyObj> m_killedEnemyObjList;
		private int m_coinsCollected;
		private int m_bagsCollected;
		private int m_diamondsCollected;
		private int m_blueprintsCollected;
		private GameObj m_objKilledPlayer;
		private RenderTarget2D m_roomBWRenderTarget;
		private SpriteObj m_dungeonLight;
		private SpriteObj m_traitAura;
		private ImpactEffectPool m_impactEffectPool;
		private TextObj m_roomTitle;
		private TextObj m_roomEnteringTitle;
		private SpriteObj m_blackBorder1;
		private SpriteObj m_blackBorder2;
		private int m_borderSize;
		private List<ChestObj> m_chestList;
		public bool LoadGameData;
		private ProjectileIconPool m_projectileIconPool;
		private float m_enemyPauseDuration;
		private bool m_enemiesPaused;
		private ObjContainer m_objectivePlate;
		private TweenObject m_objectivePlateTween;
		public SkyObj m_sky;
		private SpriteObj m_whiteBG;
		private TextObj m_creditsText;
		private TextObj m_creditsTitleText;
		private string[] m_creditsTextList;
		private string[] m_creditsTextTitleList;
		private int m_creditsIndex;
		private SpriteObj m_filmGrain;
		private SpriteObj m_compassBG;
		private SpriteObj m_compass;
		private DoorObj m_compassDoor;
		private bool m_compassDisplayed;
		public TextObj DebugTextObj;
		private Texture2D m_castleBorderTexture;
		private Texture2D m_towerBorderTexture;
		private Texture2D m_dungeonBorderTexture;
		private Texture2D m_gardenBorderTexture;
		private Texture2D m_neoBorderTexture;
		private RenderTarget2D m_finalRenderTarget;
		private RenderTarget2D m_fgRenderTarget;
		private RenderTarget2D m_bgRenderTarget;
		private RenderTarget2D m_skyRenderTarget;
		private RenderTarget2D m_shadowRenderTarget;
		private RenderTarget2D m_lightSourceRenderTarget;
		private RenderTarget2D m_traitAuraRenderTarget;
		private BackgroundObj m_foregroundSprite;
		private BackgroundObj m_backgroundSprite;
		private BackgroundObj m_backgroundParallaxSprite;
		private BackgroundObj m_gardenParallaxFG;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public bool CameraLockedToPlayer
		{
			get;
			set;
		}
		public float ShoutMagnitude
		{
			get;
			set;
		}
		public bool DisableRoomOnEnter
		{
			get;
			set;
		}
		public bool DisableSongUpdating
		{
			get;
			set;
		}
		public bool DisableRoomTransitioning
		{
			get;
			set;
		}
		public bool JukeboxEnabled
		{
			get;
			set;
		}
		public List<RoomObj> MapRoomsUnveiled
		{
			get
			{
				return this.m_miniMapDisplay.AddedRoomsList;
			}
			set
			{
				this.m_miniMapDisplay.ClearRoomsAdded();
				this.m_miniMapDisplay.AddAllRooms(value);
			}
		}
		public List<RoomObj> MapRoomsAdded
		{
			get
			{
				return this.m_miniMapDisplay.AddedRoomsList;
			}
		}
		public PlayerObj Player
		{
			get
			{
				return this.m_player;
			}
			set
			{
				this.m_player = value;
			}
		}
		public List<RoomObj> RoomList
		{
			get
			{
				return this.m_roomList;
			}
		}
		public PhysicsManager PhysicsManager
		{
			get
			{
				return this.m_physicsManager;
			}
		}
		public RoomObj CurrentRoom
		{
			get
			{
				return this.m_currentRoom;
			}
		}
		public ProjectileManager ProjectileManager
		{
			get
			{
				return this.m_projectileManager;
			}
		}
		public List<EnemyObj> EnemyList
		{
			get
			{
				return this.CurrentRoom.EnemyList;
			}
		}
		public List<ChestObj> ChestList
		{
			get
			{
				return this.m_chestList;
			}
		}
		public TextManager TextManager
		{
			get
			{
				return this.m_textManager;
			}
		}
		public ImpactEffectPool ImpactEffectPool
		{
			get
			{
				return this.m_impactEffectPool;
			}
		}
		public ItemDropManager ItemDropManager
		{
			get
			{
				return this.m_itemDropManager;
			}
		}
		public GameTypes.LevelType CurrentLevelType
		{
			get
			{
				return this.m_currentRoom.LevelType;
			}
		}
		public int LeftBorder
		{
			get
			{
				return this.m_leftMostBorder;
			}
		}
		public int RightBorder
		{
			get
			{
				return this.m_rightMostBorder;
			}
		}
		public int TopBorder
		{
			get
			{
				return this.m_topMostBorder;
			}
		}
		public int BottomBorder
		{
			get
			{
				return this.m_bottomMostBorder;
			}
		}
		public RenderTarget2D RenderTarget
		{
			get
			{
				return this.m_finalRenderTarget;
			}
		}
		public bool EnemiesPaused
		{
			get
			{
				return this.m_enemiesPaused;
			}
		}
		public ProceduralLevelScreen()
		{
			this.DisableRoomTransitioning = false;
			this.m_roomList = new List<RoomObj>();
			this.m_textManager = new TextManager(700);
			this.m_projectileManager = new ProjectileManager(this, 700);
			this.m_enemyStartPositions = new List<Vector2>();
			this.m_tempEnemyStartPositions = new List<Vector2>();
			this.m_impactEffectPool = new ImpactEffectPool(2000);
			this.CameraLockedToPlayer = true;
			this.m_roomTitle = new TextObj(null);
			this.m_roomTitle.Font = Game.JunicodeLargeFont;
			this.m_roomTitle.Align = Types.TextAlign.Right;
			this.m_roomTitle.Opacity = 0f;
			this.m_roomTitle.FontSize = 40f;
			this.m_roomTitle.Position = new Vector2(1270f, 570f);
			this.m_roomTitle.OutlineWidth = 2;
			this.m_roomEnteringTitle = (this.m_roomTitle.Clone() as TextObj);
			this.m_roomEnteringTitle.Text = "Now Entering";
			this.m_roomEnteringTitle.FontSize = 24f;
			this.m_roomEnteringTitle.Y -= 50f;
			this.m_inputMap = new InputMap(PlayerIndex.One, false);
			this.m_inputMap.AddInput(0, Keys.Y);
			this.m_inputMap.AddInput(1, Keys.U);
			this.m_inputMap.AddInput(2, Keys.LeftControl);
			this.m_inputMap.AddInput(3, Keys.Left);
			this.m_inputMap.AddInput(4, Keys.Right);
			this.m_inputMap.AddInput(5, Keys.Up);
			this.m_inputMap.AddInput(6, Keys.Down);
			this.m_inputMap.AddInput(7, Keys.OemTilde);
			this.m_chestList = new List<ChestObj>();
			this.m_miniMapDisplay = new MapObj(true, this);
			this.m_killedEnemyObjList = new List<EnemyObj>();
		}
		public override void LoadContent()
		{
			this.DebugTextObj = new TextObj(Game.JunicodeFont);
			this.DebugTextObj.FontSize = 26f;
			this.DebugTextObj.Align = Types.TextAlign.Centre;
			this.DebugTextObj.Text = "";
			this.DebugTextObj.ForceDraw = true;
			this.m_projectileIconPool = new ProjectileIconPool(200, this.m_projectileManager, base.ScreenManager as RCScreenManager);
			this.m_projectileIconPool.Initialize();
			this.m_textManager.Initialize();
			this.m_impactEffectPool.Initialize();
			this.m_physicsManager = (base.ScreenManager.Game as Game).PhysicsManager;
			this.m_physicsManager.SetGravity(0f, 1830f);
			this.m_projectileManager.Initialize();
			this.m_physicsManager.Initialize(base.ScreenManager.Camera);
			this.m_itemDropManager = new ItemDropManager(600, this.m_physicsManager);
			this.m_itemDropManager.Initialize();
			this.m_playerHUD = new PlayerHUDObj();
			this.m_playerHUD.SetPosition(new Vector2(20f, 40f));
			this.m_enemyHUD = new EnemyHUDObj();
			this.m_enemyHUD.Position = new Vector2((float)(660 - this.m_enemyHUD.Width / 2), 20f);
			this.m_miniMapDisplay.SetPlayer(this.m_player);
			this.m_miniMapDisplay.InitializeAlphaMap(new Rectangle(1070, 50, 200, 100), base.Camera);
			this.InitializeAllRooms(true);
			this.InitializeEnemies();
			this.InitializeChests(true);
			this.InitializeRenderTargets();
			this.m_mapBG = new SpriteObj("MinimapBG_Sprite");
			this.m_mapBG.Position = new Vector2(1070f, 50f);
			this.m_mapBG.ForceDraw = true;
			this.UpdateCamera();
			this.m_borderSize = 100;
			this.m_blackBorder1 = new SpriteObj("Blank_Sprite");
			this.m_blackBorder1.TextureColor = Color.Black;
			this.m_blackBorder1.Scale = new Vector2(1340f / (float)this.m_blackBorder1.Width, (float)(this.m_borderSize / this.m_blackBorder1.Height));
			this.m_blackBorder2 = new SpriteObj("Blank_Sprite");
			this.m_blackBorder2.TextureColor = Color.Black;
			this.m_blackBorder2.Scale = new Vector2(1340f / (float)this.m_blackBorder2.Width, (float)(this.m_borderSize / this.m_blackBorder2.Height));
			this.m_blackBorder1.ForceDraw = true;
			this.m_blackBorder2.ForceDraw = true;
			this.m_blackBorder1.Y = (float)(-(float)this.m_borderSize);
			this.m_blackBorder2.Y = 720f;
			this.m_dungeonLight = new SpriteObj("LightSource_Sprite");
			this.m_dungeonLight.ForceDraw = true;
			this.m_dungeonLight.Scale = new Vector2(12f, 12f);
			this.m_traitAura = new SpriteObj("LightSource_Sprite");
			this.m_traitAura.ForceDraw = true;
			this.m_objectivePlate = new ObjContainer("DialogBox_Character");
			this.m_objectivePlate.ForceDraw = true;
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.Position = new Vector2(-400f, -60f);
			textObj.OverrideParentScale = true;
			textObj.FontSize = 10f;
			textObj.Text = "Fairy Chest Objective:";
			textObj.TextureColor = Color.Red;
			textObj.OutlineWidth = 2;
			this.m_objectivePlate.AddChild(textObj);
			TextObj textObj2 = new TextObj(Game.JunicodeFont);
			textObj2.OverrideParentScale = true;
			textObj2.Position = new Vector2(textObj.X, textObj.Y + 40f);
			textObj2.ForceDraw = true;
			textObj2.FontSize = 9f;
			textObj2.Text = "Reach the chest in 15 seconds:";
			textObj2.WordWrap(250);
			textObj2.OutlineWidth = 2;
			this.m_objectivePlate.AddChild(textObj2);
			TextObj textObj3 = new TextObj(Game.JunicodeFont);
			textObj3.OverrideParentScale = true;
			textObj3.Position = new Vector2(textObj2.X, textObj2.Y + 35f);
			textObj3.ForceDraw = true;
			textObj3.FontSize = 9f;
			textObj3.Text = "Time Remaining:";
			textObj3.WordWrap(250);
			textObj3.OutlineWidth = 2;
			this.m_objectivePlate.AddChild(textObj3);
			this.m_objectivePlate.Scale = new Vector2(250f / (float)this.m_objectivePlate.GetChildAt(0).Width, 130f / (float)this.m_objectivePlate.GetChildAt(0).Height);
			this.m_objectivePlate.Position = new Vector2(1470f, 250f);
			SpriteObj spriteObj = new SpriteObj("Blank_Sprite");
			spriteObj.TextureColor = Color.Red;
			spriteObj.Position = new Vector2(textObj2.X, textObj2.Y + 20f);
			spriteObj.ForceDraw = true;
			spriteObj.OverrideParentScale = true;
			spriteObj.ScaleY = 0.5f;
			this.m_objectivePlate.AddChild(spriteObj);
			SpriteObj spriteObj2 = new SpriteObj("Blank_Sprite");
			spriteObj2.TextureColor = Color.Red;
			spriteObj2.Position = new Vector2(textObj2.X, spriteObj.Y + 35f);
			spriteObj2.ForceDraw = true;
			spriteObj2.OverrideParentScale = true;
			spriteObj2.ScaleY = 0.5f;
			this.m_objectivePlate.AddChild(spriteObj2);
			base.LoadContent();
			this.m_sky = new SkyObj(this);
			this.m_sky.LoadContent(base.Camera);
			this.m_whiteBG = new SpriteObj("Blank_Sprite");
			this.m_whiteBG.Opacity = 0f;
			this.m_whiteBG.Scale = new Vector2(1320f / (float)this.m_whiteBG.Width, 720f / (float)this.m_whiteBG.Height);
			this.m_filmGrain = new SpriteObj("FilmGrain_Sprite");
			this.m_filmGrain.ForceDraw = true;
			this.m_filmGrain.Scale = new Vector2(2.015f, 2.05f);
			this.m_filmGrain.X -= 5f;
			this.m_filmGrain.Y -= 5f;
			this.m_filmGrain.PlayAnimation(true);
			this.m_filmGrain.AnimationDelay = 0.0333333351f;
			this.m_compassBG = new SpriteObj("CompassBG_Sprite");
			this.m_compassBG.ForceDraw = true;
			this.m_compassBG.Position = new Vector2(660f, 90f);
			this.m_compassBG.Scale = Vector2.Zero;
			this.m_compass = new SpriteObj("Compass_Sprite");
			this.m_compass.Position = this.m_compassBG.Position;
			this.m_compass.ForceDraw = true;
			this.m_compass.Scale = Vector2.Zero;
			this.InitializeCreditsText();
		}
		private void InitializeCreditsText()
		{
			this.m_creditsTextTitleList = new string[]
			{
				"Developed by",
				"Design",
				"Programming",
				"Art",
				"Sound Design & Music",
				"Music",
				""
			};
			this.m_creditsTextList = new string[]
			{
				"Cellar Door Games",
				"Teddy Lee",
				"Kenny Lee",
				"Glauber Kotaki",
				"Gordon McGladdery",
				"Judson Cowan",
				"Rogue Legacy"
			};
			this.m_creditsText = new TextObj(Game.JunicodeFont);
			this.m_creditsText.FontSize = 20f;
			this.m_creditsText.Text = "Cellar Door Games";
			this.m_creditsText.DropShadow = new Vector2(2f, 2f);
			this.m_creditsText.Opacity = 0f;
			this.m_creditsTitleText = (this.m_creditsText.Clone() as TextObj);
			this.m_creditsTitleText.FontSize = 14f;
			this.m_creditsTitleText.Position = new Vector2(50f, 580f);
			this.m_creditsText.Position = this.m_creditsTitleText.Position;
			this.m_creditsText.Y += 35f;
			this.m_creditsTitleText.X += 5f;
		}
		public void DisplayCreditsText(bool resetIndex)
		{
			if (resetIndex)
			{
				this.m_creditsIndex = 0;
			}
			this.m_creditsTitleText.Opacity = 0f;
			this.m_creditsText.Opacity = 0f;
			if (this.m_creditsIndex < this.m_creditsTextList.Length)
			{
				this.m_creditsTitleText.Opacity = 0f;
				this.m_creditsText.Opacity = 0f;
				this.m_creditsTitleText.Text = this.m_creditsTextTitleList[this.m_creditsIndex];
				this.m_creditsText.Text = this.m_creditsTextList[this.m_creditsIndex];
				Tween.To(this.m_creditsTitleText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"Opacity",
					"1"
				});
				Tween.To(this.m_creditsText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"0.2",
					"Opacity",
					"1"
				});
				this.m_creditsTitleText.Opacity = 1f;
				this.m_creditsText.Opacity = 1f;
				Tween.To(this.m_creditsTitleText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"4",
					"Opacity",
					"0"
				});
				Tween.To(this.m_creditsText, 0.5f, new Easing(Tween.EaseNone), new string[]
				{
					"delay",
					"4.2",
					"Opacity",
					"0"
				});
				this.m_creditsTitleText.Opacity = 0f;
				this.m_creditsText.Opacity = 0f;
				this.m_creditsIndex++;
				Tween.RunFunction(8f, this, "DisplayCreditsText", new object[]
				{
					false
				});
			}
		}
		public void StopCreditsText()
		{
			this.m_creditsIndex = 0;
			Tween.StopAllContaining(this.m_creditsTitleText, false);
			Tween.StopAllContaining(this.m_creditsText, false);
			Tween.StopAllContaining(this, false);
			this.m_creditsTitleText.Opacity = 0f;
		}
		public override void ReinitializeRTs()
		{
			this.m_sky.ReinitializeRT(base.Camera);
			this.m_miniMapDisplay.InitializeAlphaMap(new Rectangle(1070, 50, 200, 100), base.Camera);
			this.InitializeRenderTargets();
			this.InitializeAllRooms(false);
			if (this.CurrentRoom == null || this.CurrentRoom.Name != "Start")
			{
				if (this.CurrentRoom.Name == "ChallengeBoss")
				{
					this.m_backgroundSprite.Scale = Vector2.One;
					this.m_backgroundSprite.ChangeSprite("NeoBG_Sprite", base.ScreenManager.Camera);
					this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
					this.m_foregroundSprite.Scale = Vector2.One;
					this.m_foregroundSprite.ChangeSprite("NeoFG_Sprite", base.ScreenManager.Camera);
					this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
				}
				else
				{
					switch (this.CurrentRoom.LevelType)
					{
					case GameTypes.LevelType.CASTLE:
						this.m_backgroundSprite.Scale = Vector2.One;
						this.m_foregroundSprite.Scale = Vector2.One;
						this.m_backgroundSprite.ChangeSprite("CastleBG1_Sprite", base.ScreenManager.Camera);
						this.m_foregroundSprite.ChangeSprite("CastleFG1_Sprite", base.ScreenManager.Camera);
						this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
						this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
						break;
					case GameTypes.LevelType.GARDEN:
						this.m_backgroundSprite.Scale = Vector2.One;
						this.m_foregroundSprite.Scale = Vector2.One;
						this.m_backgroundSprite.ChangeSprite("GardenBG_Sprite", base.ScreenManager.Camera);
						this.m_foregroundSprite.ChangeSprite("GardenFG_Sprite", base.ScreenManager.Camera);
						this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
						this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
						break;
					case GameTypes.LevelType.DUNGEON:
						this.m_backgroundSprite.Scale = Vector2.One;
						this.m_foregroundSprite.Scale = Vector2.One;
						this.m_backgroundSprite.ChangeSprite("DungeonBG1_Sprite", base.ScreenManager.Camera);
						this.m_foregroundSprite.ChangeSprite("DungeonFG1_Sprite", base.ScreenManager.Camera);
						this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
						this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
						break;
					case GameTypes.LevelType.TOWER:
						this.m_backgroundSprite.Scale = Vector2.One;
						this.m_foregroundSprite.Scale = Vector2.One;
						this.m_backgroundSprite.ChangeSprite("TowerBG2_Sprite", base.ScreenManager.Camera);
						this.m_foregroundSprite.ChangeSprite("TowerFG2_Sprite", base.ScreenManager.Camera);
						this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
						this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
						break;
					}
				}
				if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
				{
					this.m_foregroundSprite.Scale = Vector2.One;
					this.m_foregroundSprite.ChangeSprite("NeoFG_Sprite", base.ScreenManager.Camera);
					this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
				}
			}
			this.m_backgroundSprite.Position = this.CurrentRoom.Position;
			this.m_foregroundSprite.Position = this.CurrentRoom.Position;
			base.ReinitializeRTs();
		}
		private void LoadPhysicsObjects(RoomObj room)
		{
			Rectangle value = new Rectangle((int)room.X - 100, (int)room.Y - 100, room.Width + 200, room.Height + 200);
			this.m_physicsManager.RemoveAllObjects();
			foreach (TerrainObj current in this.CurrentRoom.TerrainObjList)
			{
				this.m_physicsManager.AddObject(current);
			}
			foreach (ProjectileObj current2 in this.m_projectileManager.ActiveProjectileList)
			{
				this.m_physicsManager.AddObject(current2);
			}
			foreach (GameObj current3 in this.CurrentRoom.GameObjList)
			{
				IPhysicsObj physicsObj = current3 as IPhysicsObj;
				if (physicsObj != null && current3.Bounds.Intersects(value))
				{
					BreakableObj breakableObj = current3 as BreakableObj;
					if (breakableObj == null || !breakableObj.Broken)
					{
						this.m_physicsManager.AddObject(physicsObj);
					}
				}
			}
			foreach (DoorObj current4 in this.CurrentRoom.DoorList)
			{
				this.m_physicsManager.AddObject(current4);
			}
			foreach (EnemyObj current5 in this.CurrentRoom.EnemyList)
			{
				this.m_physicsManager.AddObject(current5);
				if (current5 is EnemyObj_BallAndChain && !current5.IsKilled)
				{
					this.m_physicsManager.AddObject((current5 as EnemyObj_BallAndChain).BallAndChain);
					if (current5.Difficulty > GameTypes.EnemyDifficulty.BASIC)
					{
						this.m_physicsManager.AddObject((current5 as EnemyObj_BallAndChain).BallAndChain2);
					}
				}
			}
			foreach (EnemyObj current6 in this.CurrentRoom.TempEnemyList)
			{
				this.m_physicsManager.AddObject(current6);
			}
			this.m_physicsManager.AddObject(this.m_player);
		}
		public void InitializeEnemies()
		{
			List<TerrainObj> list = new List<TerrainObj>();
			foreach (RoomObj current in this.m_roomList)
			{
				foreach (EnemyObj current2 in current.EnemyList)
				{
					current2.SetPlayerTarget(this.m_player);
					current2.SetLevelScreen(this);
					int level = current.Level;
					if (current.Name == "Boss" && current.LinkedRoom != null)
					{
						level = current.LinkedRoom.Level;
						int level2 = (int)((float)level / (4f + (float)Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f));
						current2.Level = level2;
					}
					else
					{
						int num = (int)((float)level / (4f + (float)Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f));
						if (num < 1)
						{
							num = 1;
						}
						current2.Level = num;
					}
					int num2 = current2.Level / 32;
					if (num2 > 2)
					{
						num2 = 2;
					}
					if (current2.IsProcedural)
					{
						if (current2.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
						{
							current2.Level += 4;
						}
						if (current2.Difficulty < (GameTypes.EnemyDifficulty)num2)
						{
							current2.SetDifficulty((GameTypes.EnemyDifficulty)num2, false);
						}
					}
					else if (current2.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
					{
						if (current is ArenaBonusRoom)
						{
							current2.Level += 4;
						}
						else
						{
							current2.Level += 7;
						}
					}
					current2.Initialize();
					if (current2.IsWeighted)
					{
						float num3 = 3.40282347E+38f;
						TerrainObj terrainObj = null;
						list.Clear();
						Rectangle rectangle = new Rectangle((int)current2.X, current2.TerrainBounds.Bottom, 1, 5000);
						foreach (TerrainObj current3 in current.TerrainObjList)
						{
							if (current3.Rotation == 0f)
							{
								if (current3.Bounds.Top >= current2.TerrainBounds.Bottom && CollisionMath.Intersects(current3.Bounds, rectangle))
								{
									list.Add(current3);
								}
							}
							else if (CollisionMath.RotatedRectIntersects(rectangle, 0f, Vector2.Zero, current3.TerrainBounds, current3.Rotation, Vector2.Zero))
							{
								list.Add(current3);
							}
						}
						foreach (TerrainObj current4 in list)
						{
							bool flag = false;
							int num4;
							if (current4.Rotation == 0f)
							{
								flag = true;
								num4 = current4.TerrainBounds.Top - current2.TerrainBounds.Bottom;
							}
							else
							{
								Vector2 vector;
								Vector2 vector2;
								if (current4.Width > current4.Height)
								{
									vector = CollisionMath.UpperLeftCorner(current4.TerrainBounds, current4.Rotation, Vector2.Zero);
									vector2 = CollisionMath.UpperRightCorner(current4.TerrainBounds, current4.Rotation, Vector2.Zero);
								}
								else if (current4.Rotation > 0f)
								{
									vector = CollisionMath.LowerLeftCorner(current4.TerrainBounds, current4.Rotation, Vector2.Zero);
									vector2 = CollisionMath.UpperLeftCorner(current4.TerrainBounds, current4.Rotation, Vector2.Zero);
								}
								else
								{
									vector = CollisionMath.UpperRightCorner(current4.TerrainBounds, current4.Rotation, Vector2.Zero);
									vector2 = CollisionMath.LowerRightCorner(current4.TerrainBounds, current4.Rotation, Vector2.Zero);
								}
								if (current2.X > vector.X && current2.X < vector2.X)
								{
									flag = true;
								}
								float num5 = vector2.X - vector.X;
								float num6 = vector2.Y - vector.Y;
								float x = vector.X;
								float y = vector.Y;
								float x2 = current2.X;
								num4 = (int)(y + (x2 - x) * (num6 / num5)) - current2.TerrainBounds.Bottom;
							}
							if (flag && (float)num4 < num3 && num4 > 0)
							{
								num3 = (float)num4;
								terrainObj = current4;
							}
						}
						if (terrainObj != null)
						{
							current2.UpdateCollisionBoxes();
							if (terrainObj.Rotation == 0f)
							{
								current2.Y = terrainObj.Y - ((float)current2.TerrainBounds.Bottom - current2.Y);
							}
							else
							{
								this.HookEnemyToSlope(current2, terrainObj);
							}
						}
					}
				}
			}
		}
		private void HookEnemyToSlope(IPhysicsObj enemy, TerrainObj terrain)
		{
			Vector2 vector;
			Vector2 vector2;
			if (terrain.Width > terrain.Height)
			{
				vector = CollisionMath.UpperLeftCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
				vector2 = CollisionMath.UpperRightCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
			}
			else if (terrain.Rotation > 0f)
			{
				vector = CollisionMath.LowerLeftCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
				vector2 = CollisionMath.UpperLeftCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
			}
			else
			{
				vector = CollisionMath.UpperRightCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
				vector2 = CollisionMath.LowerRightCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
			}
			float num = vector2.X - vector.X;
			float num2 = vector2.Y - vector.Y;
			float x = vector.X;
			float y = vector.Y;
			float x2 = enemy.X;
			float num3 = y + (x2 - x) * (num2 / num);
			enemy.UpdateCollisionBoxes();
			num3 -= (float)enemy.Bounds.Bottom - enemy.Y + 5f * (enemy as GameObj).ScaleX;
			enemy.Y = (float)Math.Round((double)num3, MidpointRounding.ToEven);
		}
		public void InitializeChests(bool resetChests)
		{
			this.m_chestList.Clear();
			foreach (RoomObj current in this.RoomList)
			{
				foreach (GameObj current2 in current.GameObjList)
				{
					ChestObj chestObj = current2 as ChestObj;
					if (chestObj != null && chestObj.ChestType != 4)
					{
						chestObj.Level = (int)((float)current.Level / (4f + (float)Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f));
						if (chestObj.IsProcedural)
						{
							if (resetChests)
							{
								chestObj.ResetChest();
							}
							int num = CDGMath.RandomInt(1, 100);
							int num2 = 0;
							int i = 0;
							while (i < GameEV.CHEST_TYPE_CHANCE.Length)
							{
								num2 += GameEV.CHEST_TYPE_CHANCE[i];
								if (num <= num2)
								{
									if (i == 0)
									{
										chestObj.ChestType = 1;
										break;
									}
									if (i == 1)
									{
										chestObj.ChestType = 2;
										break;
									}
									chestObj.ChestType = 3;
									break;
								}
								else
								{
									i++;
								}
							}
						}
						this.m_chestList.Add(chestObj);
					}
					else if (chestObj != null && chestObj.ChestType == 4)
					{
						FairyChestObj fairyChestObj = chestObj as FairyChestObj;
						if (fairyChestObj != null)
						{
							if (chestObj.IsProcedural && resetChests)
							{
								fairyChestObj.ResetChest();
							}
							fairyChestObj.SetConditionType(0);
						}
					}
					this.m_chestList.Add(chestObj);
					if (chestObj != null)
					{
						chestObj.X += (float)(chestObj.Width / 2);
						chestObj.Y += 60f;
					}
				}
			}
		}
		public void InitializeAllRooms(bool loadContent)
		{
			this.m_castleBorderTexture = new SpriteObj("CastleBorder_Sprite")
			{
				Scale = new Vector2(2f, 2f)
			}.ConvertToTexture(base.Camera, true, SamplerState.PointWrap);
			string cornerTextureString = "CastleCorner_Sprite";
			string cornerLTextureString = "CastleCornerL_Sprite";
			this.m_towerBorderTexture = new SpriteObj("TowerBorder2_Sprite")
			{
				Scale = new Vector2(2f, 2f)
			}.ConvertToTexture(base.Camera, true, SamplerState.PointWrap);
			string cornerTextureString2 = "TowerCorner_Sprite";
			string cornerLTextureString2 = "TowerCornerL_Sprite";
			this.m_dungeonBorderTexture = new SpriteObj("DungeonBorder_Sprite")
			{
				Scale = new Vector2(2f, 2f)
			}.ConvertToTexture(base.Camera, true, SamplerState.PointWrap);
			string cornerTextureString3 = "DungeonCorner_Sprite";
			string cornerLTextureString3 = "DungeonCornerL_Sprite";
			this.m_gardenBorderTexture = new SpriteObj("GardenBorder_Sprite")
			{
				Scale = new Vector2(2f, 2f)
			}.ConvertToTexture(base.Camera, true, SamplerState.PointWrap);
			string cornerTextureString4 = "GardenCorner_Sprite";
			string cornerLTextureString4 = "GardenCornerL_Sprite";
			this.m_neoBorderTexture = new SpriteObj("NeoBorder_Sprite")
			{
				Scale = new Vector2(2f, 2f)
			}.ConvertToTexture(base.Camera, true, SamplerState.PointWrap);
			string text = "NeoCorner_Sprite";
			string text2 = "NeoCornerL_Sprite";
			if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
			{
				cornerLTextureString3 = (cornerLTextureString = (cornerLTextureString2 = (cornerLTextureString4 = text2)));
				cornerTextureString3 = (cornerTextureString = (cornerTextureString2 = (cornerTextureString4 = text)));
			}
			int num = 0;
			num = (int)(Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8);
			if (this.m_roomBWRenderTarget != null)
			{
				this.m_roomBWRenderTarget.Dispose();
			}
			this.m_roomBWRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
			foreach (RoomObj current in this.RoomList)
			{
				int num2 = 0;
				switch (current.LevelType)
				{
				case GameTypes.LevelType.CASTLE:
					num2 = 0;
					break;
				case GameTypes.LevelType.GARDEN:
					num2 = 0;
					break;
				case GameTypes.LevelType.DUNGEON:
					num2 = 0;
					break;
				case GameTypes.LevelType.TOWER:
					num2 = 0;
					break;
				}
				if (Game.PlayerStats.TimesCastleBeaten == 0)
				{
					current.Level = num + num2;
				}
				else
				{
					current.Level = num + num2 + (128 + (Game.PlayerStats.TimesCastleBeaten - 1) * 128);
				}
				num++;
				if (loadContent)
				{
					current.LoadContent(base.Camera.GraphicsDevice);
				}
				current.InitializeRenderTarget(this.m_roomBWRenderTarget);
				if (current.Name == "ChallengeBoss")
				{
					using (List<BorderObj>.Enumerator enumerator2 = current.BorderList.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BorderObj current2 = enumerator2.Current;
							current2.SetBorderTextures(this.m_neoBorderTexture, text, text2);
							current2.NeoTexture = this.m_neoBorderTexture;
						}
						goto IL_3D6;
					}
					goto IL_311;
				}
				goto IL_311;
				IL_3D6:
				bool flag = false;
				if (Game.PlayerStats.Traits.X == 33f || Game.PlayerStats.Traits.Y == 33f)
				{
					flag = true;
				}
				foreach (GameObj current3 in current.GameObjList)
				{
					HazardObj hazardObj = current3 as HazardObj;
					if (hazardObj != null)
					{
						hazardObj.InitializeTextures(base.Camera);
					}
					HoverObj hoverObj = current3 as HoverObj;
					if (hoverObj != null)
					{
						hoverObj.SetStartingPos(hoverObj.Position);
					}
					if (flag)
					{
						BreakableObj breakableObj = current3 as BreakableObj;
						if (breakableObj != null && !breakableObj.HitBySpellsOnly && !breakableObj.HasTerrainHitBox)
						{
							breakableObj.CollisionBoxes.Add(new CollisionBox(breakableObj.RelativeBounds.X, breakableObj.RelativeBounds.Y, breakableObj.Width, breakableObj.Height, 0, breakableObj));
							breakableObj.DisableHitboxUpdating = true;
							breakableObj.UpdateTerrainBox();
						}
					}
				}
				if (LevelEV.RUN_TESTROOM && loadContent)
				{
					foreach (GameObj current4 in current.GameObjList)
					{
						if (current4 is PlayerStartObj)
						{
							this.m_player.Position = current4.Position;
						}
					}
				}
				if ((current.Name == "Boss" || current.Name == "ChallengeBoss") && current.LinkedRoom != null)
				{
					this.CloseBossDoor(current.LinkedRoom, current.LevelType);
					continue;
				}
				continue;
				IL_311:
				foreach (BorderObj current5 in current.BorderList)
				{
					switch (current.LevelType)
					{
					case GameTypes.LevelType.CASTLE:
						goto IL_39E;
					case GameTypes.LevelType.GARDEN:
						current5.SetBorderTextures(this.m_gardenBorderTexture, cornerTextureString4, cornerLTextureString4);
						current5.TextureOffset = new Vector2(0f, -18f);
						break;
					case GameTypes.LevelType.DUNGEON:
						current5.SetBorderTextures(this.m_dungeonBorderTexture, cornerTextureString3, cornerLTextureString3);
						break;
					case GameTypes.LevelType.TOWER:
						current5.SetBorderTextures(this.m_towerBorderTexture, cornerTextureString2, cornerLTextureString2);
						break;
					default:
						goto IL_39E;
					}
					IL_3AD:
					current5.NeoTexture = this.m_neoBorderTexture;
					continue;
					IL_39E:
					current5.SetBorderTextures(this.m_castleBorderTexture, cornerTextureString, cornerLTextureString);
					goto IL_3AD;
				}
				goto IL_3D6;
			}
		}
		public void CloseBossDoor(RoomObj linkedRoom, GameTypes.LevelType levelType)
		{
			bool flag = false;
			switch (levelType)
			{
			case GameTypes.LevelType.CASTLE:
				if (Game.PlayerStats.EyeballBossBeaten)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.GARDEN:
				if (Game.PlayerStats.FairyBossBeaten)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.DUNGEON:
				if (Game.PlayerStats.BlobBossBeaten)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.TOWER:
				if (Game.PlayerStats.FireballBossBeaten)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				foreach (DoorObj current in linkedRoom.DoorList)
				{
					if (current.IsBossDoor)
					{
						foreach (GameObj current2 in linkedRoom.GameObjList)
						{
							if (current2.Name == "BossDoor")
							{
								current2.ChangeSprite((current2 as SpriteObj).SpriteName.Replace("Open", ""));
								current2.TextureColor = Color.White;
								current2.Opacity = 1f;
								linkedRoom.LinkedRoom = null;
								break;
							}
						}
						current.Locked = true;
						break;
					}
				}
			}
			this.OpenChallengeBossDoor(linkedRoom, levelType);
			if (Game.PlayerStats.ChallengeLastBossUnlocked)
			{
				this.OpenLastBossChallengeDoors();
			}
		}
		public void OpenLastBossChallengeDoors()
		{
			LastBossChallengeRoom linkedRoom = null;
			foreach (RoomObj current in this.RoomList)
			{
				if (current.Name == "ChallengeBoss" && current is LastBossChallengeRoom)
				{
					linkedRoom = (current as LastBossChallengeRoom);
					break;
				}
			}
			foreach (RoomObj current2 in this.RoomList)
			{
				if (current2.Name == "EntranceBoss")
				{
					bool flag = false;
					if (current2.LevelType == GameTypes.LevelType.CASTLE && Game.PlayerStats.EyeballBossBeaten)
					{
						flag = true;
					}
					else if (current2.LevelType == GameTypes.LevelType.DUNGEON && Game.PlayerStats.BlobBossBeaten)
					{
						flag = true;
					}
					else if (current2.LevelType == GameTypes.LevelType.GARDEN && Game.PlayerStats.FairyBossBeaten)
					{
						flag = true;
					}
					else if (current2.LevelType == GameTypes.LevelType.TOWER && Game.PlayerStats.FireballBossBeaten)
					{
						flag = true;
					}
					if (flag)
					{
						foreach (DoorObj current3 in current2.DoorList)
						{
							if (current3.IsBossDoor)
							{
								current2.LinkedRoom = linkedRoom;
								foreach (GameObj current4 in current2.GameObjList)
								{
									if (current4.Name == "BossDoor")
									{
										if (Game.PlayerStats.ChallengeLastBossBeaten)
										{
											if ((current4 as SpriteObj).SpriteName.Contains("Open"))
											{
												current4.ChangeSprite((current4 as SpriteObj).SpriteName.Replace("Open", ""));
											}
											current4.TextureColor = Color.White;
											current4.Opacity = 1f;
											current2.LinkedRoom = null;
											current3.Locked = true;
											break;
										}
										if (!(current4 as SpriteObj).SpriteName.Contains("Open"))
										{
											current4.ChangeSprite((current4 as SpriteObj).SpriteName.Replace("_Sprite", "Open_Sprite"));
										}
										current4.TextureColor = new Color(0, 255, 255);
										current4.Opacity = 0.6f;
										current3.Locked = false;
										break;
									}
								}
							}
						}
					}
				}
			}
		}
		public void OpenChallengeBossDoor(RoomObj linkerRoom, GameTypes.LevelType levelType)
		{
			bool flag = false;
			switch (levelType)
			{
			case GameTypes.LevelType.CASTLE:
				if (Game.PlayerStats.EyeballBossBeaten && !Game.PlayerStats.ChallengeEyeballBeaten && Game.PlayerStats.ChallengeEyeballUnlocked)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.GARDEN:
				if (Game.PlayerStats.FairyBossBeaten && !Game.PlayerStats.ChallengeSkullBeaten && Game.PlayerStats.ChallengeSkullUnlocked)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.DUNGEON:
				if (Game.PlayerStats.BlobBossBeaten && !Game.PlayerStats.ChallengeBlobBeaten && Game.PlayerStats.ChallengeBlobUnlocked)
				{
					flag = true;
				}
				break;
			case GameTypes.LevelType.TOWER:
				if (Game.PlayerStats.FireballBossBeaten && !Game.PlayerStats.ChallengeFireballBeaten && Game.PlayerStats.ChallengeFireballUnlocked)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				RoomObj challengeBossRoomFromRoomList = LevelBuilder2.GetChallengeBossRoomFromRoomList(levelType, this.m_roomList);
				linkerRoom.LinkedRoom = challengeBossRoomFromRoomList;
				foreach (DoorObj current in linkerRoom.DoorList)
				{
					if (current.IsBossDoor)
					{
						foreach (GameObj current2 in linkerRoom.GameObjList)
						{
							if (current2.Name == "BossDoor")
							{
								current2.ChangeSprite((current2 as SpriteObj).SpriteName.Replace("_Sprite", "Open_Sprite"));
								current2.TextureColor = new Color(0, 255, 255);
								current2.Opacity = 0.6f;
								break;
							}
						}
						current.Locked = false;
						break;
					}
				}
			}
		}
		public void AddRooms(List<RoomObj> roomsToAdd)
		{
			foreach (RoomObj current in roomsToAdd)
			{
				this.m_roomList.Add(current);
				if (current.X < (float)this.m_leftMostBorder)
				{
					this.m_leftMostBorder = (int)current.X;
				}
				if (current.X + (float)current.Width > (float)this.m_rightMostBorder)
				{
					this.m_rightMostBorder = (int)current.X + current.Width;
				}
				if (current.Y < (float)this.m_topMostBorder)
				{
					this.m_topMostBorder = (int)current.Y;
				}
				if (current.Y + (float)current.Height > (float)this.m_bottomMostBorder)
				{
					this.m_bottomMostBorder = (int)current.Y + current.Height;
				}
			}
		}
		public void AddRoom(RoomObj room)
		{
			this.m_roomList.Add(room);
			if (room.X < (float)this.m_leftMostBorder)
			{
				this.m_leftMostBorder = (int)room.X;
			}
			if (room.X + (float)room.Width > (float)this.m_rightMostBorder)
			{
				this.m_rightMostBorder = (int)room.X + room.Width;
			}
			if (room.Y < (float)this.m_topMostBorder)
			{
				this.m_topMostBorder = (int)room.Y;
			}
			if (room.Y + (float)room.Height > (float)this.m_bottomMostBorder)
			{
				this.m_bottomMostBorder = (int)room.Y + room.Height;
			}
		}
		private void CheckForRoomTransition()
		{
			if (this.m_player != null)
			{
				foreach (RoomObj current in this.m_roomList)
				{
					if (current != this.CurrentRoom && current.Bounds.Contains((int)this.m_player.X, (int)this.m_player.Y))
					{
						this.ResetEnemyPositions();
						if (this.CurrentRoom != null)
						{
							foreach (EnemyObj current2 in this.EnemyList)
							{
								current2.ResetState();
							}
						}
						if (this.m_enemiesPaused)
						{
							this.UnpauseAllEnemies();
						}
						this.m_player.RoomTransitionReset();
						this.m_miniMapDisplay.AddRoom(current);
						if (current.Name != "Start")
						{
							(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
							{
								SaveType.PlayerData,
								SaveType.MapData
							});
						}
						if (current.Name == "ChallengeBoss")
						{
							this.m_backgroundSprite.Scale = Vector2.One;
							this.m_backgroundSprite.ChangeSprite("NeoBG_Sprite", base.ScreenManager.Camera);
							this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
							this.m_foregroundSprite.Scale = Vector2.One;
							this.m_foregroundSprite.ChangeSprite("NeoFG_Sprite", base.ScreenManager.Camera);
							this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
						}
						if ((this.CurrentRoom == null || this.CurrentLevelType != current.LevelType || (this.CurrentRoom != null && this.CurrentRoom.Name == "ChallengeBoss")) && current.Name != "Start")
						{
							if (current.Name != "ChallengeBoss")
							{
								switch (current.LevelType)
								{
								case GameTypes.LevelType.CASTLE:
									this.m_backgroundSprite.Scale = Vector2.One;
									this.m_foregroundSprite.Scale = Vector2.One;
									this.m_backgroundSprite.ChangeSprite("CastleBG1_Sprite", base.ScreenManager.Camera);
									this.m_foregroundSprite.ChangeSprite("CastleFG1_Sprite", base.ScreenManager.Camera);
									this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
									this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
									break;
								case GameTypes.LevelType.GARDEN:
									this.m_backgroundSprite.Scale = Vector2.One;
									this.m_foregroundSprite.Scale = Vector2.One;
									this.m_backgroundSprite.ChangeSprite("GardenBG_Sprite", base.ScreenManager.Camera);
									this.m_foregroundSprite.ChangeSprite("GardenFG_Sprite", base.ScreenManager.Camera);
									this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
									this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
									break;
								case GameTypes.LevelType.DUNGEON:
									this.m_backgroundSprite.Scale = Vector2.One;
									this.m_foregroundSprite.Scale = Vector2.One;
									this.m_backgroundSprite.ChangeSprite("DungeonBG1_Sprite", base.ScreenManager.Camera);
									this.m_foregroundSprite.ChangeSprite("DungeonFG1_Sprite", base.ScreenManager.Camera);
									this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
									this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
									break;
								case GameTypes.LevelType.TOWER:
									this.m_backgroundSprite.Scale = Vector2.One;
									this.m_foregroundSprite.Scale = Vector2.One;
									this.m_backgroundSprite.ChangeSprite("TowerBG2_Sprite", base.ScreenManager.Camera);
									this.m_foregroundSprite.ChangeSprite("TowerFG2_Sprite", base.ScreenManager.Camera);
									this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
									this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
									break;
								}
							}
							if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
							{
								this.m_foregroundSprite.Scale = Vector2.One;
								this.m_foregroundSprite.ChangeSprite("NeoFG_Sprite", base.ScreenManager.Camera);
								this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
							}
							if (current.LevelType == GameTypes.LevelType.DUNGEON || Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f || current.Name == "Compass")
							{
								Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
							}
							else
							{
								Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
							}
							this.m_roomTitle.Text = WordBuilder.BuildDungeonName(current.LevelType);
							if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
							{
								this.m_roomTitle.RandomizeSentence(false);
							}
							this.m_roomTitle.Opacity = 0f;
							if (current.Name != "Boss" && current.Name != "Tutorial" && current.Name != "Ending" && current.Name != "ChallengeBoss")
							{
								Tween.StopAllContaining(this.m_roomEnteringTitle, false);
								Tween.StopAllContaining(this.m_roomTitle, false);
								this.m_roomTitle.Opacity = 0f;
								this.m_roomEnteringTitle.Opacity = 0f;
								if (this.m_player.X > (float)current.Bounds.Center.X)
								{
									this.m_roomTitle.X = 50f;
									this.m_roomTitle.Align = Types.TextAlign.Left;
									this.m_roomEnteringTitle.X = 70f;
									this.m_roomEnteringTitle.Align = Types.TextAlign.Left;
								}
								else
								{
									this.m_roomTitle.X = 1270f;
									this.m_roomTitle.Align = Types.TextAlign.Right;
									this.m_roomEnteringTitle.X = 1250f;
									this.m_roomEnteringTitle.Align = Types.TextAlign.Right;
								}
								Tween.To(this.m_roomTitle, 0.5f, new Easing(Linear.EaseNone), new string[]
								{
									"delay",
									"0.2",
									"Opacity",
									"1"
								});
								this.m_roomTitle.Opacity = 1f;
								Tween.To(this.m_roomTitle, 0.5f, new Easing(Linear.EaseNone), new string[]
								{
									"delay",
									"2.2",
									"Opacity",
									"0"
								});
								this.m_roomTitle.Opacity = 0f;
								Tween.To(this.m_roomEnteringTitle, 0.5f, new Easing(Linear.EaseNone), new string[]
								{
									"Opacity",
									"1"
								});
								this.m_roomEnteringTitle.Opacity = 1f;
								Tween.To(this.m_roomEnteringTitle, 0.5f, new Easing(Linear.EaseNone), new string[]
								{
									"delay",
									"2",
									"Opacity",
									"0"
								});
								this.m_roomEnteringTitle.Opacity = 0f;
							}
							else
							{
								Tween.StopAllContaining(this.m_roomEnteringTitle, false);
								Tween.StopAllContaining(this.m_roomTitle, false);
								this.m_roomTitle.Opacity = 0f;
								this.m_roomEnteringTitle.Opacity = 0f;
							}
							this.JukeboxEnabled = false;
							Console.WriteLine("Now entering " + current.LevelType);
						}
						if (this.m_currentRoom != null)
						{
							this.m_currentRoom.OnExit();
						}
						this.m_currentRoom = current;
						this.m_backgroundSprite.Position = this.CurrentRoom.Position;
						this.m_foregroundSprite.Position = this.CurrentRoom.Position;
						this.m_gardenParallaxFG.Position = this.CurrentRoom.Position;
						if (SoundManager.IsMusicPaused)
						{
							SoundManager.ResumeMusic();
						}
						if (!this.DisableSongUpdating && !this.JukeboxEnabled)
						{
							this.UpdateLevelSong();
						}
						if (this.m_currentRoom.Player == null)
						{
							this.m_currentRoom.Player = this.m_player;
						}
						if (this.m_currentRoom.Name != "Start" && this.m_currentRoom.Name != "Tutorial" && this.m_currentRoom.Name != "Ending" && this.m_currentRoom.Name != "CastleEntrance" && this.m_currentRoom.Name != "Bonus" && this.m_currentRoom.Name != "Throne" && this.m_currentRoom.Name != "Secret" && this.m_currentRoom.Name != "Boss" && this.m_currentRoom.LevelType != GameTypes.LevelType.NONE && this.m_currentRoom.Name != "ChallengeBoss" && (Game.PlayerStats.Traits.X == 26f || Game.PlayerStats.Traits.Y == 26f) && CDGMath.RandomFloat(0f, 1f) < 0.2f)
						{
							this.SpawnDementiaEnemy();
						}
						if (this.m_currentRoom.HasFairyChest)
						{
							this.m_currentRoom.DisplayFairyChestInfo();
						}
						this.m_tempEnemyStartPositions.Clear();
						this.m_enemyStartPositions.Clear();
						foreach (EnemyObj current3 in this.CurrentRoom.EnemyList)
						{
							this.m_enemyStartPositions.Add(current3.Position);
						}
						foreach (EnemyObj current4 in this.CurrentRoom.TempEnemyList)
						{
							this.m_tempEnemyStartPositions.Add(current4.Position);
						}
						this.m_projectileManager.DestroyAllProjectiles(false);
						this.LoadPhysicsObjects(current);
						this.m_itemDropManager.DestroyAllItemDrops();
						this.m_projectileIconPool.DestroyAllIcons();
						this.m_enemyPauseDuration = 0f;
						if (LevelEV.SHOW_ENEMY_RADII)
						{
							foreach (EnemyObj current5 in current.EnemyList)
							{
								current5.InitializeDebugRadii();
							}
						}
						this.m_lastEnemyHit = null;
						foreach (GameObj current6 in this.m_currentRoom.GameObjList)
						{
							FairyChestObj fairyChestObj = current6 as FairyChestObj;
							if (fairyChestObj != null)
							{
								bool arg_BC6_0 = fairyChestObj.IsOpen;
							}
							IAnimateableObj animateableObj = current6 as IAnimateableObj;
							if (animateableObj != null && animateableObj.TotalFrames > 1 && !(animateableObj is ChestObj) && !(current6 is BreakableObj))
							{
								animateableObj.AnimationDelay = 0.1f;
								animateableObj.PlayAnimation(true);
							}
						}
						if (!this.DisableRoomOnEnter)
						{
							this.m_currentRoom.OnEnter();
							break;
						}
						break;
					}
				}
			}
		}
		private void UpdateLevelSong()
		{
			if (!(this.CurrentRoom.Name != "Start") || !(this.CurrentRoom.Name != "Tutorial") || !(this.CurrentRoom.Name != "Ending") || SoundManager.IsMusicPlaying)
			{
				if (!(this.m_currentRoom is StartingRoomObj) && SoundManager.IsMusicPlaying)
				{
					if ((this.m_currentRoom is CarnivalShoot1BonusRoom || this.m_currentRoom is CarnivalShoot2BonusRoom) && SoundManager.GetCurrentMusicName() != "PooyanSong")
					{
						SoundManager.PlayMusic("PooyanSong", true, 1f);
						return;
					}
					if (this.m_currentRoom.LevelType == GameTypes.LevelType.CASTLE && SoundManager.GetCurrentMusicName() != "CastleSong")
					{
						SoundManager.PlayMusic("CastleSong", true, 1f);
						return;
					}
					if (this.m_currentRoom.LevelType == GameTypes.LevelType.GARDEN && SoundManager.GetCurrentMusicName() != "GardenSong")
					{
						SoundManager.PlayMusic("GardenSong", true, 1f);
						return;
					}
					if (this.m_currentRoom.LevelType == GameTypes.LevelType.DUNGEON && SoundManager.GetCurrentMusicName() != "DungeonSong")
					{
						SoundManager.PlayMusic("DungeonSong", true, 1f);
						return;
					}
					if (this.m_currentRoom.LevelType == GameTypes.LevelType.TOWER && SoundManager.GetCurrentMusicName() != "TowerSong")
					{
						SoundManager.PlayMusic("TowerSong", true, 1f);
					}
				}
				return;
			}
			if (this.m_currentRoom is CarnivalShoot1BonusRoom || this.m_currentRoom is CarnivalShoot2BonusRoom)
			{
				SoundManager.PlayMusic("PooyanSong", true, 1f);
				return;
			}
			switch (this.m_currentRoom.LevelType)
			{
			case GameTypes.LevelType.CASTLE:
				IL_A8:
				SoundManager.PlayMusic("CastleSong", true, 1f);
				return;
			case GameTypes.LevelType.GARDEN:
				SoundManager.PlayMusic("GardenSong", true, 1f);
				return;
			case GameTypes.LevelType.DUNGEON:
				SoundManager.PlayMusic("DungeonSong", true, 1f);
				return;
			case GameTypes.LevelType.TOWER:
				SoundManager.PlayMusic("TowerSong", true, 1f);
				return;
			}
			goto IL_A8;
		}
		public override void Update(GameTime gameTime)
		{
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			this.m_projectileIconPool.Update(base.Camera);
			if (!base.IsPaused)
			{
				this.m_sky.Update(gameTime);
				if (this.m_enemyPauseDuration > 0f)
				{
					this.m_enemyPauseDuration -= num;
					if (this.m_enemyPauseDuration <= 0f)
					{
						this.StopTimeStop();
					}
				}
				this.CurrentRoom.Update(gameTime);
				if (this.m_player != null)
				{
					this.m_player.Update(gameTime);
				}
				this.m_enemyHUD.Update(gameTime);
				this.m_playerHUD.Update(this.m_player);
				this.m_projectileManager.Update(gameTime);
				this.m_physicsManager.Update(gameTime);
				if (!this.DisableRoomTransitioning && !CollisionMath.Intersects(new Rectangle((int)this.m_player.X, (int)this.m_player.Y, 1, 1), base.Camera.Bounds))
				{
					this.CheckForRoomTransition();
				}
				if ((!this.m_inputMap.Pressed(2) || (this.m_inputMap.Pressed(2) && (LevelEV.RUN_DEMO_VERSION || LevelEV.CREATE_RETAIL_VERSION))) && this.CameraLockedToPlayer)
				{
					this.UpdateCamera();
				}
				if (Game.PlayerStats.SpecialItem == 6 && this.CurrentRoom.Name != "Start" && this.CurrentRoom.Name != "Tutorial" && this.CurrentRoom.Name != "Boss" && this.CurrentRoom.Name != "Throne" && this.CurrentRoom.Name != "ChallengeBoss")
				{
					if (!this.m_compassDisplayed)
					{
						this.DisplayCompass();
					}
					else
					{
						this.UpdateCompass();
					}
				}
				else if (this.m_compassDisplayed && this.CurrentRoom.Name != "Compass")
				{
					this.HideCompass();
				}
				if (this.m_objectivePlate.X == 1170f)
				{
					bool flag = false;
					Rectangle bounds = this.m_objectivePlate.Bounds;
					bounds.X += (int)base.Camera.TopLeftCorner.X;
					bounds.Y += (int)base.Camera.TopLeftCorner.Y;
					if (CollisionMath.Intersects(this.m_player.Bounds, bounds))
					{
						flag = true;
					}
					if (!flag)
					{
						foreach (EnemyObj current in this.CurrentRoom.EnemyList)
						{
							if (CollisionMath.Intersects(current.Bounds, bounds))
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						this.m_objectivePlate.Opacity = 0.5f;
					}
					else
					{
						this.m_objectivePlate.Opacity = 1f;
					}
				}
			}
			base.Update(gameTime);
		}
		public void UpdateCamera()
		{
			if (this.m_player != null)
			{
				base.ScreenManager.Camera.X = (float)((int)(this.m_player.Position.X + GlobalEV.Camera_XOffset));
				base.ScreenManager.Camera.Y = (float)((int)(this.m_player.Position.Y + GlobalEV.Camera_YOffset));
			}
			if (this.m_currentRoom != null)
			{
				if (base.ScreenManager.Camera.Width < this.m_currentRoom.Width)
				{
					if (base.ScreenManager.Camera.Bounds.Left < this.m_currentRoom.Bounds.Left)
					{
						base.ScreenManager.Camera.X = (float)((int)((float)this.m_currentRoom.Bounds.Left + (float)base.ScreenManager.Camera.Width * 0.5f));
					}
					else if (base.ScreenManager.Camera.Bounds.Right > this.m_currentRoom.Bounds.Right)
					{
						base.ScreenManager.Camera.X = (float)((int)((float)this.m_currentRoom.Bounds.Right - (float)base.ScreenManager.Camera.Width * 0.5f));
					}
				}
				else
				{
					base.ScreenManager.Camera.X = (float)((int)(this.m_currentRoom.X + (float)this.m_currentRoom.Width * 0.5f));
				}
				if (base.ScreenManager.Camera.Height < this.m_currentRoom.Height)
				{
					if (base.ScreenManager.Camera.Bounds.Top < this.m_currentRoom.Bounds.Top)
					{
						base.ScreenManager.Camera.Y = (float)((int)((float)this.m_currentRoom.Bounds.Top + (float)base.ScreenManager.Camera.Height * 0.5f));
						return;
					}
					if (base.ScreenManager.Camera.Bounds.Bottom > this.m_currentRoom.Bounds.Bottom)
					{
						base.ScreenManager.Camera.Y = (float)((int)((float)this.m_currentRoom.Bounds.Bottom - (float)base.ScreenManager.Camera.Height * 0.5f));
						return;
					}
				}
				else
				{
					base.ScreenManager.Camera.Y = (float)((int)(this.m_currentRoom.Y + (float)this.m_currentRoom.Height * 0.5f));
				}
			}
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.JustPressed(8) && this.CurrentRoom.Name != "Ending")
			{
				(base.ScreenManager as RCScreenManager).DisplayScreen(16, true, null);
			}
			if (!LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION)
			{
				if (InputManager.JustPressed(Keys.RightControl, null))
				{
					if (SoundManager.GetCurrentMusicName() == "CastleSong")
					{
						SoundManager.PlayMusic("TowerSong", true, 0.5f);
					}
					else if (SoundManager.GetCurrentMusicName() == "TowerSong")
					{
						SoundManager.PlayMusic("DungeonBoss", true, 0.5f);
					}
					else
					{
						SoundManager.PlayMusic("CastleSong", true, 0.5f);
					}
				}
				if (this.m_inputMap.JustPressed(0))
				{
					this.m_miniMapDisplay.AddAllRooms(this.m_roomList);
				}
				if (this.m_inputMap.JustPressed(7))
				{
					LevelEV.SHOW_DEBUG_TEXT = !LevelEV.SHOW_DEBUG_TEXT;
				}
				if (this.m_inputMap.JustPressed(1))
				{
					if (base.Camera.Zoom < 1f)
					{
						base.Camera.Zoom = 1f;
					}
					else
					{
						base.Camera.Zoom = 0.05f;
					}
				}
				float num = 2000f;
				if (this.m_inputMap.Pressed(2) && this.m_inputMap.Pressed(3))
				{
					base.Camera.X -= num * (float)base.Camera.GameTime.ElapsedGameTime.TotalSeconds;
				}
				else if (this.m_inputMap.Pressed(2) && this.m_inputMap.Pressed(4))
				{
					base.Camera.X += num * (float)base.Camera.GameTime.ElapsedGameTime.TotalSeconds;
				}
				if (this.m_inputMap.Pressed(2) && this.m_inputMap.Pressed(5))
				{
					base.Camera.Y -= num * (float)base.Camera.GameTime.ElapsedGameTime.TotalSeconds;
				}
				else if (this.m_inputMap.Pressed(2) && this.m_inputMap.Pressed(6))
				{
					base.Camera.Y += num * (float)base.Camera.GameTime.ElapsedGameTime.TotalSeconds;
				}
			}
			if (this.m_player != null && (!this.m_inputMap.Pressed(2) || (this.m_inputMap.Pressed(2) && (LevelEV.RUN_DEMO_VERSION || LevelEV.CREATE_RETAIL_VERSION))) && !this.m_player.IsKilled)
			{
				this.m_player.HandleInput();
			}
			base.HandleInput();
		}
		private void UpdateCompass()
		{
			if (this.m_compassDoor == null && this.CurrentRoom.Name != "Ending" && this.CurrentRoom.Name != "Boss" && this.CurrentRoom.Name != "Start" && this.CurrentRoom.Name != "Tutorial" && this.CurrentRoom.Name != " ChallengeBoss")
			{
				Console.WriteLine("Creating new bonus room for compass");
				RoomObj roomObj = null;
				EnemyObj enemyObj = null;
				List<RoomObj> list = new List<RoomObj>();
				foreach (RoomObj current in this.m_roomList)
				{
					bool flag = false;
					foreach (EnemyObj current2 in current.EnemyList)
					{
						if (current2.IsWeighted)
						{
							flag = true;
							break;
						}
					}
					if (current.Name != "Ending" && current.Name != "Tutorial" && current.Name != "Boss" && current.Name != "Secret" && current.Name != "Bonus" && flag && current.Name != "ChallengeBoss")
					{
						list.Add(current);
					}
				}
				if (list.Count > 0)
				{
					roomObj = list[CDGMath.RandomInt(0, list.Count - 1)];
					int num = 0;
					while (enemyObj == null || !enemyObj.IsWeighted)
					{
						enemyObj = roomObj.EnemyList[num];
						num++;
					}
					DoorObj doorObj = new DoorObj(roomObj, 120, 180, GameTypes.DoorType.OPEN);
					doorObj.Position = enemyObj.Position;
					doorObj.IsBossDoor = true;
					doorObj.DoorPosition = "None";
					doorObj.AddCollisionBox(0, 0, doorObj.Width, doorObj.Height, 0);
					doorObj.AddCollisionBox(0, 0, doorObj.Width, doorObj.Height, 2);
					float num2 = 3.40282347E+38f;
					TerrainObj terrainObj = null;
					foreach (TerrainObj current3 in roomObj.TerrainObjList)
					{
						if (current3.Y >= doorObj.Y && current3.Y - doorObj.Y < num2 && CollisionMath.Intersects(current3.Bounds, new Rectangle((int)doorObj.X, (int)(doorObj.Y + (current3.Y - doorObj.Y) + 5f), doorObj.Width, doorObj.Height / 2)))
						{
							num2 = current3.Y - doorObj.Y;
							terrainObj = current3;
						}
					}
					if (terrainObj != null)
					{
						doorObj.UpdateCollisionBoxes();
						if (terrainObj.Rotation == 0f)
						{
							doorObj.Y = terrainObj.Y - ((float)doorObj.TerrainBounds.Bottom - doorObj.Y);
						}
						else
						{
							this.HookEnemyToSlope(doorObj, terrainObj);
						}
					}
					roomObj.DoorList.Add(doorObj);
					roomObj.LinkedRoom = this.m_roomList[this.m_roomList.Count - 1];
					roomObj.LinkedRoom.LinkedRoom = roomObj;
					roomObj.LinkedRoom.LevelType = roomObj.LevelType;
					string cornerTextureString = "CastleCorner_Sprite";
					string cornerLTextureString = "CastleCornerL_Sprite";
					string cornerTextureString2 = "TowerCorner_Sprite";
					string cornerLTextureString2 = "TowerCornerL_Sprite";
					string cornerTextureString3 = "DungeonCorner_Sprite";
					string cornerLTextureString3 = "DungeonCornerL_Sprite";
					string cornerTextureString4 = "GardenCorner_Sprite";
					string cornerLTextureString4 = "GardenCornerL_Sprite";
					if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
					{
						string text = "NeoCorner_Sprite";
						string text2 = "NeoCornerL_Sprite";
						cornerLTextureString3 = (cornerLTextureString = (cornerLTextureString2 = (cornerLTextureString4 = text2)));
						cornerTextureString3 = (cornerTextureString = (cornerTextureString2 = (cornerTextureString4 = text)));
					}
					foreach (BorderObj current4 in roomObj.LinkedRoom.BorderList)
					{
						switch (roomObj.LinkedRoom.LevelType)
						{
						case GameTypes.LevelType.GARDEN:
							current4.SetBorderTextures(this.m_gardenBorderTexture, cornerTextureString4, cornerLTextureString4);
							current4.TextureOffset = new Vector2(0f, -18f);
							continue;
						case GameTypes.LevelType.DUNGEON:
							current4.SetBorderTextures(this.m_dungeonBorderTexture, cornerTextureString3, cornerLTextureString3);
							continue;
						case GameTypes.LevelType.TOWER:
							current4.SetBorderTextures(this.m_towerBorderTexture, cornerTextureString2, cornerLTextureString2);
							continue;
						}
						current4.SetBorderTextures(this.m_castleBorderTexture, cornerTextureString, cornerLTextureString);
					}
					this.m_compassDoor = doorObj;
				}
			}
			if (this.m_compassDoor != null)
			{
				this.m_compass.Rotation = CDGMath.AngleBetweenPts(this.m_player.Position, new Vector2((float)this.m_compassDoor.Bounds.Center.X, (float)this.m_compassDoor.Bounds.Center.Y));
			}
		}
		public void RemoveCompassDoor()
		{
			if (this.m_compassDoor != null)
			{
				this.m_compassDoor.Room.DoorList.Remove(this.m_compassDoor);
				this.m_compassDoor.Dispose();
				this.m_compassDoor = null;
			}
		}
		private void DisplayCompass()
		{
			Tween.StopAllContaining(this.m_compassBG, false);
			Tween.StopAllContaining(this.m_compass, false);
			Tween.To(this.m_compassBG, 0.5f, new Easing(Back.EaseOutLarge), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_compass, 0.5f, new Easing(Back.EaseOutLarge), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			this.m_compassDisplayed = true;
		}
		private void HideCompass()
		{
			Tween.StopAllContaining(this.m_compassBG, false);
			Tween.StopAllContaining(this.m_compass, false);
			Tween.To(this.m_compassBG, 0.5f, new Easing(Back.EaseInLarge), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_compass, 0.5f, new Easing(Back.EaseInLarge), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			this.m_compassDisplayed = false;
			this.RemoveCompassDoor();
		}
		public void InitializeRenderTargets()
		{
			int num = 1320;
			int num2 = 720;
			if (LevelEV.SAVE_FRAMES)
			{
				num /= 2;
				num2 /= 2;
			}
			if (this.m_fgRenderTarget != null)
			{
				this.m_fgRenderTarget.Dispose();
			}
			this.m_fgRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra5551, DepthFormat.None);
			if (this.m_shadowRenderTarget != null)
			{
				this.m_shadowRenderTarget.Dispose();
			}
			this.m_shadowRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra4444, DepthFormat.None);
			base.Camera.Begin();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_shadowRenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			base.Camera.End();
			if (this.m_lightSourceRenderTarget != null)
			{
				this.m_lightSourceRenderTarget.Dispose();
			}
			this.m_lightSourceRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra4444, DepthFormat.None);
			if (this.m_finalRenderTarget != null)
			{
				this.m_finalRenderTarget.Dispose();
			}
			this.m_finalRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
			if (this.m_skyRenderTarget != null)
			{
				this.m_skyRenderTarget.Dispose();
			}
			this.m_skyRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, num, num2);
			if (this.m_bgRenderTarget != null)
			{
				this.m_bgRenderTarget.Dispose();
			}
			this.m_bgRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color, DepthFormat.None);
			if (this.m_traitAuraRenderTarget != null)
			{
				this.m_traitAuraRenderTarget.Dispose();
			}
			this.m_traitAuraRenderTarget = new RenderTarget2D(base.Camera.GraphicsDevice, num, num2);
			this.InitializeBackgroundObjs();
		}
		public void InitializeBackgroundObjs()
		{
			if (this.m_foregroundSprite != null)
			{
				this.m_foregroundSprite.Dispose();
			}
			this.m_foregroundSprite = new BackgroundObj("CastleFG1_Sprite");
			this.m_foregroundSprite.SetRepeated(true, true, base.Camera, SamplerState.PointWrap);
			this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
			if (this.m_backgroundSprite != null)
			{
				this.m_backgroundSprite.Dispose();
			}
			this.m_backgroundSprite = new BackgroundObj("CastleBG1_Sprite");
			this.m_backgroundSprite.SetRepeated(true, true, base.Camera, SamplerState.PointWrap);
			this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
			if (this.m_backgroundParallaxSprite != null)
			{
				this.m_backgroundParallaxSprite.Dispose();
			}
			this.m_backgroundParallaxSprite = new BackgroundObj("TowerBGFrame_Sprite");
			this.m_backgroundParallaxSprite.SetRepeated(true, true, base.Camera, SamplerState.PointWrap);
			this.m_backgroundParallaxSprite.Scale = new Vector2(2f, 2f);
			if (this.m_gardenParallaxFG != null)
			{
				this.m_gardenParallaxFG.Dispose();
			}
			this.m_gardenParallaxFG = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
			this.m_gardenParallaxFG.SetRepeated(true, true, base.Camera, SamplerState.LinearWrap);
			this.m_gardenParallaxFG.TextureColor = Color.White;
			this.m_gardenParallaxFG.Scale = new Vector2(3f, 3f);
			this.m_gardenParallaxFG.Opacity = 0.7f;
			this.m_gardenParallaxFG.ParallaxSpeed = new Vector2(0.3f, 0f);
		}
		public void DrawRenderTargets()
		{
			if (this.m_backgroundSprite.Texture.IsContentLost)
			{
				this.ReinitializeRTs();
			}
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, base.Camera.GetTransformation());
			if (this.CurrentRoom != null)
			{
				this.CurrentRoom.DrawRenderTargets(base.Camera);
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, base.Camera.GetTransformation());
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_fgRenderTarget);
			this.m_foregroundSprite.Draw(base.Camera);
			if (!this.m_enemiesPaused)
			{
				if (Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f)
				{
					this.m_traitAura.Scale = new Vector2(15f, 15f);
				}
				else if (Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f)
				{
					this.m_traitAura.Scale = new Vector2(8f, 8f);
				}
				else
				{
					this.m_traitAura.Scale = new Vector2(10f, 10f);
				}
			}
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_traitAuraRenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Transparent);
			if (this.CurrentRoom != null)
			{
				this.m_traitAura.Position = this.m_player.Position;
				this.m_traitAura.Draw(base.Camera);
			}
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_lightSourceRenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Transparent);
			if (this.CurrentRoom != null)
			{
				this.m_dungeonLight.Position = this.m_player.Position;
				this.m_dungeonLight.Draw(base.Camera);
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			this.m_miniMapDisplay.DrawRenderTargets(base.Camera);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_skyRenderTarget);
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
			this.m_sky.Draw(base.Camera);
			base.Camera.End();
		}
		private static Vector2 MoveInCircle(GameTime gameTime, float speed)
		{
			double num = (double)(Game.TotalGameTime * speed);
			float x = (float)Math.Cos(num);
			float y = (float)Math.Sin(num);
			return new Vector2(x, y);
		}
		public override void Draw(GameTime gameTime)
		{
			this.DrawRenderTargets();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_bgRenderTarget);
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, base.Camera.GetTransformation());
			this.m_backgroundSprite.Draw(base.Camera);
			if (this.CurrentRoom != null && base.Camera.Zoom == 1f && (!this.m_inputMap.Pressed(2) || (this.m_inputMap.Pressed(2) && (LevelEV.RUN_DEMO_VERSION || LevelEV.CREATE_RETAIL_VERSION))))
			{
				this.CurrentRoom.DrawBGObjs(base.Camera);
			}
			else
			{
				foreach (RoomObj current in this.m_roomList)
				{
					current.DrawBGObjs(base.Camera);
				}
			}
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_finalRenderTarget);
			base.Camera.GraphicsDevice.Clear(Color.Black);
			if (this.m_enemiesPaused)
			{
				base.Camera.GraphicsDevice.Clear(Color.White);
			}
			base.Camera.GraphicsDevice.Textures[1] = this.m_skyRenderTarget;
			base.Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.ParallaxEffect);
			if (!this.m_enemiesPaused)
			{
				base.Camera.Draw(this.m_bgRenderTarget, Vector2.Zero, Color.White);
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, Game.BWMaskEffect, base.Camera.GetTransformation());
			base.Camera.GraphicsDevice.Textures[1] = this.m_fgRenderTarget;
			base.Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
			base.Camera.Draw(this.CurrentRoom.BGRender, base.Camera.TopLeftCorner, Color.White);
			base.Camera.End();
			if (!LevelEV.SHOW_ENEMY_RADII)
			{
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, base.Camera.GetTransformation());
			}
			else
			{
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, base.Camera.GetTransformation());
			}
			this.CurrentRoom.Draw(base.Camera);
			if (LevelEV.SHOW_ENEMY_RADII)
			{
				foreach (EnemyObj current2 in this.m_currentRoom.EnemyList)
				{
					current2.DrawDetectionRadii(base.Camera);
				}
			}
			this.m_projectileManager.Draw(base.Camera);
			if (this.m_enemiesPaused)
			{
				base.Camera.End();
				base.Camera.GraphicsDevice.SetRenderTarget(this.m_bgRenderTarget);
				base.Camera.GraphicsDevice.Textures[1] = this.m_traitAuraRenderTarget;
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.InvertShader);
				base.Camera.Draw(this.m_finalRenderTarget, Vector2.Zero, Color.White);
				base.Camera.End();
				Game.HSVEffect.Parameters["Saturation"].SetValue(0);
				Game.HSVEffect.Parameters["UseMask"].SetValue(true);
				base.Camera.GraphicsDevice.SetRenderTarget(this.m_finalRenderTarget);
				base.Camera.GraphicsDevice.Textures[1] = this.m_traitAuraRenderTarget;
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
				base.Camera.Draw(this.m_bgRenderTarget, Vector2.Zero, Color.White);
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, base.Camera.GetTransformation());
			base.Camera.Draw(Game.GenericTexture, new Rectangle((int)base.Camera.TopLeftCorner.X, (int)base.Camera.TopLeftCorner.Y, 1320, 720), Color.Black * this.BackBufferOpacity);
			if (!this.m_player.IsKilled)
			{
				this.m_player.Draw(base.Camera);
			}
			if (!LevelEV.CREATE_RETAIL_VERSION)
			{
				this.DebugTextObj.Position = new Vector2(base.Camera.X, base.Camera.Y - 300f);
				this.DebugTextObj.Draw(base.Camera);
			}
			this.m_itemDropManager.Draw(base.Camera);
			this.m_impactEffectPool.Draw(base.Camera);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, base.Camera.GetTransformation());
			this.m_textManager.Draw(base.Camera);
			if (this.CurrentRoom.LevelType == GameTypes.LevelType.TOWER)
			{
				this.m_gardenParallaxFG.Draw(base.Camera);
			}
			this.m_whiteBG.Draw(base.Camera);
			base.Camera.End();
			if ((this.CurrentLevelType == GameTypes.LevelType.DUNGEON || Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f) && (Game.PlayerStats.Class != 13 || (Game.PlayerStats.Class == 13 && !this.Player.LightOn)))
			{
				base.Camera.GraphicsDevice.Textures[1] = this.m_lightSourceRenderTarget;
				base.Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.ShadowEffect);
				if (LevelEV.SAVE_FRAMES)
				{
					base.Camera.Draw(this.m_shadowRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(2f, 2f), SpriteEffects.None, 1f);
				}
				else
				{
					base.Camera.Draw(this.m_shadowRenderTarget, Vector2.Zero, Color.White);
				}
				base.Camera.End();
			}
			if (this.CurrentRoom.Name != "Ending")
			{
				if ((Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f) && Game.PlayerStats.SpecialItem != 8)
				{
					Game.GaussianBlur.InvertMask = true;
					Game.GaussianBlur.Draw(this.m_finalRenderTarget, base.Camera, this.m_traitAuraRenderTarget);
				}
				else if ((Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f) && Game.PlayerStats.SpecialItem != 8)
				{
					Game.GaussianBlur.InvertMask = false;
					Game.GaussianBlur.Draw(this.m_finalRenderTarget, base.Camera, this.m_traitAuraRenderTarget);
				}
			}
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
			this.m_projectileIconPool.Draw(base.Camera);
			this.m_playerHUD.Draw(base.Camera);
			if (this.m_lastEnemyHit != null && this.m_enemyHUDCounter > 0f)
			{
				this.m_enemyHUD.Draw(base.Camera);
			}
			if (this.m_enemyHUDCounter > 0f)
			{
				this.m_enemyHUDCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (this.CurrentRoom.Name != "Start" && this.CurrentRoom.Name != "Boss" && this.CurrentRoom.Name != "ChallengeBoss" && this.m_miniMapDisplay.Visible)
			{
				this.m_mapBG.Draw(base.Camera);
				this.m_miniMapDisplay.Draw(base.Camera);
			}
			if (this.CurrentRoom.Name != "Boss" && this.CurrentRoom.Name != "Ending")
			{
				this.m_compassBG.Draw(base.Camera);
				this.m_compass.Draw(base.Camera);
			}
			this.m_objectivePlate.Draw(base.Camera);
			this.m_roomEnteringTitle.Draw(base.Camera);
			this.m_roomTitle.Draw(base.Camera);
			if (this.CurrentRoom.Name != "Ending" && (!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
			{
				this.m_filmGrain.Draw(base.Camera);
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			this.m_blackBorder1.Draw(base.Camera);
			this.m_blackBorder2.Draw(base.Camera);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_bgRenderTarget);
			Game.RippleEffect.Parameters["width"].SetValue(this.ShoutMagnitude);
			Vector2 vector = this.m_player.Position - base.Camera.TopLeftCorner;
			if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
			{
				Game.RippleEffect.Parameters["xcenter"].SetValue(vector.X / 1320f);
				Game.RippleEffect.Parameters["ycenter"].SetValue(vector.Y / 720f);
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.RippleEffect);
			}
			else
			{
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
			}
			base.Camera.Draw(this.m_finalRenderTarget, Vector2.Zero, Color.White);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
			if (this.CurrentRoom.Name != "Ending")
			{
				if ((Game.PlayerStats.Traits.X == 1f || Game.PlayerStats.Traits.Y == 1f) && Game.PlayerStats.SpecialItem != 8)
				{
					Game.HSVEffect.Parameters["Saturation"].SetValue(0);
					Game.HSVEffect.Parameters["Brightness"].SetValue(0);
					Game.HSVEffect.Parameters["Contrast"].SetValue(0);
					base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
				}
				else if ((!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f || Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
				{
					base.Camera.GraphicsDevice.SetRenderTarget(this.m_finalRenderTarget);
					Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
					Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);
					base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
					base.Camera.Draw(this.m_bgRenderTarget, Vector2.Zero, Color.White);
					base.Camera.End();
					base.Camera.GraphicsDevice.SetRenderTarget(this.m_bgRenderTarget);
					base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
					Color color = new Color(180, 150, 80);
					base.Camera.Draw(this.m_finalRenderTarget, Vector2.Zero, color);
					this.m_creditsText.Draw(base.Camera);
					this.m_creditsTitleText.Draw(base.Camera);
					base.Camera.End();
					base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
					base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
				}
				else
				{
					base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
				}
			}
			else
			{
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
			}
			base.Camera.Draw(this.m_bgRenderTarget, Vector2.Zero, Color.White);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public void RunWhiteSlashEffect()
		{
			this.m_whiteBG.Position = this.CurrentRoom.Position;
			this.m_whiteBG.Scale = Vector2.One;
			this.m_whiteBG.Scale = new Vector2((float)(this.CurrentRoom.Width / this.m_whiteBG.Width), (float)(this.m_currentRoom.Height / this.m_whiteBG.Height));
			this.m_whiteBG.Opacity = 1f;
			Tween.To(this.m_whiteBG, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.RunFunction(0.2f, this, "RunWhiteSlash2", new object[0]);
		}
		public void RunWhiteSlash2()
		{
			this.m_whiteBG.Position = this.CurrentRoom.Position;
			this.m_whiteBG.Scale = Vector2.One;
			this.m_whiteBG.Scale = new Vector2((float)(this.CurrentRoom.Width / this.m_whiteBG.Width), (float)(this.m_currentRoom.Height / this.m_whiteBG.Height));
			this.m_whiteBG.Opacity = 1f;
			Tween.To(this.m_whiteBG, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
		}
		public void LightningEffectTwice()
		{
			this.m_whiteBG.Position = this.CurrentRoom.Position;
			this.m_whiteBG.Scale = Vector2.One;
			this.m_whiteBG.Scale = new Vector2((float)(this.CurrentRoom.Width / this.m_whiteBG.Width), (float)(this.m_currentRoom.Height / this.m_whiteBG.Height));
			this.m_whiteBG.Opacity = 1f;
			Tween.To(this.m_whiteBG, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			Tween.RunFunction(0.2f, this, "LightningEffectOnce", new object[0]);
		}
		public void LightningEffectOnce()
		{
			this.m_whiteBG.Position = this.CurrentRoom.Position;
			this.m_whiteBG.Scale = Vector2.One;
			this.m_whiteBG.Scale = new Vector2((float)(this.CurrentRoom.Width / this.m_whiteBG.Width), (float)(this.m_currentRoom.Height / this.m_whiteBG.Height));
			this.m_whiteBG.Opacity = 1f;
			Tween.To(this.m_whiteBG, 1f, new Easing(Tween.EaseNone), new string[]
			{
				"Opacity",
				"0"
			});
			SoundManager.PlaySound(new string[]
			{
				"LightningClap1",
				"LightningClap2"
			});
		}
		public void SpawnDementiaEnemy()
		{
			List<EnemyObj> list = new List<EnemyObj>();
			foreach (EnemyObj current in this.m_currentRoom.EnemyList)
			{
				if (current.Type != 17 && current.Type != 21 && current.Type != 27 && current.Type != 32 && current.Type != 6 && current.Type != 31)
				{
					list.Add(current);
				}
			}
			if (list.Count > 0)
			{
				EnemyObj enemyObj = list[CDGMath.RandomInt(0, list.Count - 1)];
				byte[] array;
				if (enemyObj.IsWeighted)
				{
					array = LevelEV.DEMENTIA_GROUND_LIST;
				}
				else
				{
					array = LevelEV.DEMENTIA_FLIGHT_LIST;
				}
				EnemyObj enemyObj2 = EnemyBuilder.BuildEnemy((int)array[CDGMath.RandomInt(0, array.Length - 1)], null, null, null, GameTypes.EnemyDifficulty.BASIC, true);
				enemyObj2.Position = enemyObj.Position;
				enemyObj2.SaveToFile = false;
				enemyObj2.IsDemented = true;
				enemyObj2.NonKillable = true;
				enemyObj2.GivesLichHealth = false;
				this.AddEnemyToCurrentRoom(enemyObj2);
			}
		}
		public void AddEnemyToCurrentRoom(EnemyObj enemy)
		{
			this.m_currentRoom.TempEnemyList.Add(enemy);
			this.m_physicsManager.AddObject(enemy);
			this.m_tempEnemyStartPositions.Add(enemy.Position);
			enemy.SetPlayerTarget(this.m_player);
			enemy.SetLevelScreen(this);
			enemy.Initialize();
		}
		public void RemoveEnemyFromCurrentRoom(EnemyObj enemy, Vector2 startingPos)
		{
			this.m_currentRoom.TempEnemyList.Remove(enemy);
			this.m_physicsManager.RemoveObject(enemy);
			this.m_tempEnemyStartPositions.Remove(startingPos);
		}
		public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room, Vector2 startingPos)
		{
			room.TempEnemyList.Remove(enemy);
			this.m_physicsManager.RemoveObject(enemy);
			this.m_tempEnemyStartPositions.Remove(startingPos);
		}
		public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room)
		{
			int num = room.TempEnemyList.IndexOf(enemy);
			if (num != -1)
			{
				room.TempEnemyList.RemoveAt(num);
				this.m_physicsManager.RemoveObject(enemy);
				this.m_tempEnemyStartPositions.RemoveAt(num);
			}
		}
		public void ResetEnemyPositions()
		{
			for (int i = 0; i < this.m_enemyStartPositions.Count; i++)
			{
				this.CurrentRoom.EnemyList[i].Position = this.m_enemyStartPositions[i];
			}
			for (int j = 0; j < this.m_tempEnemyStartPositions.Count; j++)
			{
				this.CurrentRoom.TempEnemyList[j].Position = this.m_tempEnemyStartPositions[j];
			}
		}
		public override void PauseScreen()
		{
			if (!base.IsPaused)
			{
				Tween.PauseAll();
				this.CurrentRoom.PauseRoom();
				this.ItemDropManager.PauseAllAnimations();
				this.m_impactEffectPool.PauseAllAnimations();
				if (!this.m_enemiesPaused)
				{
					this.m_projectileManager.PauseAllProjectiles(true);
				}
				SoundManager.PauseAllSounds("Pauseable");
				this.m_player.PauseAnimation();
				base.PauseScreen();
			}
		}
		public override void UnpauseScreen()
		{
			if (base.IsPaused)
			{
				Tween.ResumeAll();
				this.CurrentRoom.UnpauseRoom();
				this.ItemDropManager.ResumeAllAnimations();
				this.m_impactEffectPool.ResumeAllAnimations();
				if (!this.m_enemiesPaused)
				{
					this.m_projectileManager.UnpauseAllProjectiles();
				}
				SoundManager.ResumeAllSounds("Pauseable");
				this.m_player.ResumeAnimation();
				base.UnpauseScreen();
			}
		}
		public void RunGameOver()
		{
			this.m_player.Opacity = 1f;
			this.m_killedEnemyObjList.Clear();
			List<Vector2> enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;
			int count = this.m_roomList.Count;
			for (int i = 0; i < enemiesKilledInRun.Count; i++)
			{
				if (enemiesKilledInRun[i].X != -1f && enemiesKilledInRun[i].Y != -1f && (int)enemiesKilledInRun[i].X < count)
				{
					RoomObj roomObj = this.m_roomList[(int)enemiesKilledInRun[i].X];
					int count2 = roomObj.EnemyList.Count;
					if ((int)enemiesKilledInRun[i].Y < count2)
					{
						EnemyObj item = this.m_roomList[(int)enemiesKilledInRun[i].X].EnemyList[(int)enemiesKilledInRun[i].Y];
						this.m_killedEnemyObjList.Add(item);
					}
				}
			}
			List<object> list = new List<object>();
			list.Add(this.m_player);
			list.Add(this.m_killedEnemyObjList);
			list.Add(this.m_coinsCollected);
			list.Add(this.m_bagsCollected);
			list.Add(this.m_diamondsCollected);
			list.Add(this.m_objKilledPlayer);
			Tween.RunFunction(0f, base.ScreenManager, "DisplayScreen", new object[]
			{
				7,
				true,
				list
			});
		}
		public void RunCinematicBorders(float duration)
		{
			this.StopCinematicBorders();
			this.m_blackBorder1.Opacity = 1f;
			this.m_blackBorder2.Opacity = 1f;
			this.m_blackBorder1.Y = 0f;
			this.m_blackBorder2.Y = (float)(720 - this.m_borderSize);
			float num = 1f;
			Tween.By(this.m_blackBorder1, num, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				(duration - num).ToString(),
				"Y",
				(-this.m_borderSize).ToString()
			});
			Tween.By(this.m_blackBorder2, num, new Easing(Quad.EaseInOut), new string[]
			{
				"delay",
				(duration - num).ToString(),
				"Y",
				this.m_borderSize.ToString()
			});
			Tween.To(this.m_blackBorder1, num, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				(duration - num + 0.2f).ToString(),
				"Opacity",
				"0"
			});
			Tween.To(this.m_blackBorder2, num, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				(duration - num + 0.2f).ToString(),
				"Opacity",
				"0"
			});
		}
		public void StopCinematicBorders()
		{
			Tween.StopAllContaining(this.m_blackBorder1, false);
			Tween.StopAllContaining(this.m_blackBorder2, false);
		}
		public void DisplayMap(bool isTeleporterScreen)
		{
			(base.ScreenManager as RCScreenManager).AddRoomsToMap(this.m_miniMapDisplay.AddedRoomsList);
			if (isTeleporterScreen)
			{
				(base.ScreenManager as RCScreenManager).ActivateMapScreenTeleporter();
			}
			(base.ScreenManager as RCScreenManager).DisplayScreen(14, true, null);
		}
		public void PauseAllEnemies()
		{
			this.m_enemiesPaused = true;
			this.CurrentRoom.PauseRoom();
			foreach (EnemyObj current in this.CurrentRoom.EnemyList)
			{
				current.PauseEnemy(false);
			}
			foreach (EnemyObj current2 in this.CurrentRoom.TempEnemyList)
			{
				current2.PauseEnemy(false);
			}
			this.m_projectileManager.PauseAllProjectiles(false);
		}
		public void CastTimeStop(float duration)
		{
			SoundManager.PlaySound("Cast_TimeStart");
			SoundManager.PauseMusic();
			this.m_enemyPauseDuration = duration;
			this.PauseAllEnemies();
			Tween.To(this.m_traitAura, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				"100",
				"ScaleY",
				"100"
			});
		}
		public void StopTimeStop()
		{
			SoundManager.PlaySound("Cast_TimeStop");
			SoundManager.ResumeMusic();
			Tween.To(this.m_traitAura, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.AddEndHandlerToLastTween(this, "UnpauseAllEnemies", new object[0]);
		}
		public void UnpauseAllEnemies()
		{
			Game.HSVEffect.Parameters["UseMask"].SetValue(false);
			this.m_enemiesPaused = false;
			this.CurrentRoom.UnpauseRoom();
			foreach (EnemyObj current in this.CurrentRoom.EnemyList)
			{
				current.UnpauseEnemy(false);
			}
			foreach (EnemyObj current2 in this.CurrentRoom.TempEnemyList)
			{
				current2.UnpauseEnemy(false);
			}
			this.m_projectileManager.UnpauseAllProjectiles();
		}
		public void DamageAllEnemies(int damage)
		{
			List<EnemyObj> list = new List<EnemyObj>();
			list.AddRange(this.CurrentRoom.TempEnemyList);
			foreach (EnemyObj current in list)
			{
				if (!current.IsDemented && !current.IsKilled)
				{
					current.HitEnemy(damage, current.Position, true);
				}
			}
			list.Clear();
			list = null;
			foreach (EnemyObj current2 in this.CurrentRoom.EnemyList)
			{
				if (!current2.IsDemented && !current2.IsKilled)
				{
					current2.HitEnemy(damage, current2.Position, true);
				}
			}
		}
		public virtual void Reset()
		{
			this.BackBufferOpacity = 0f;
			this.m_killedEnemyObjList.Clear();
			this.m_diamondsCollected = 0;
			this.m_coinsCollected = 0;
			this.m_bagsCollected = 0;
			this.m_blueprintsCollected = 0;
			if (this.m_player != null)
			{
				this.m_player.Reset();
				this.m_player.ResetLevels();
				this.m_player.Position = new Vector2(200f, 200f);
			}
			this.ResetEnemyPositions();
			foreach (RoomObj current in this.m_roomList)
			{
				current.Reset();
			}
			this.InitializeChests(false);
			foreach (RoomObj current2 in this.RoomList)
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
			this.m_projectileManager.DestroyAllProjectiles(true);
			Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
		}
		public override void DisposeRTs()
		{
			this.m_fgRenderTarget.Dispose();
			this.m_fgRenderTarget = null;
			this.m_bgRenderTarget.Dispose();
			this.m_bgRenderTarget = null;
			this.m_skyRenderTarget.Dispose();
			this.m_skyRenderTarget = null;
			this.m_finalRenderTarget.Dispose();
			this.m_finalRenderTarget = null;
			this.m_shadowRenderTarget.Dispose();
			this.m_shadowRenderTarget = null;
			this.m_lightSourceRenderTarget.Dispose();
			this.m_lightSourceRenderTarget = null;
			this.m_traitAuraRenderTarget.Dispose();
			this.m_traitAuraRenderTarget = null;
			this.m_foregroundSprite.Dispose();
			this.m_foregroundSprite = null;
			this.m_backgroundSprite.Dispose();
			this.m_backgroundSprite = null;
			this.m_backgroundParallaxSprite.Dispose();
			this.m_backgroundParallaxSprite = null;
			this.m_gardenParallaxFG.Dispose();
			this.m_gardenParallaxFG = null;
			this.m_roomBWRenderTarget.Dispose();
			this.m_roomBWRenderTarget = null;
			this.m_miniMapDisplay.DisposeRTs();
			base.DisposeRTs();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Procedural Level Screen");
				Tween.StopAll(false);
				this.m_currentRoom = null;
				this.DisposeRTs();
				foreach (RoomObj current in this.m_roomList)
				{
					current.Dispose();
				}
				this.m_roomList.Clear();
				this.m_roomList = null;
				this.m_enemyStartPositions.Clear();
				this.m_enemyStartPositions = null;
				this.m_tempEnemyStartPositions.Clear();
				this.m_tempEnemyStartPositions = null;
				this.m_textManager.Dispose();
				this.m_textManager = null;
				this.m_physicsManager = null;
				this.m_projectileManager.Dispose();
				this.m_projectileManager = null;
				this.m_itemDropManager.Dispose();
				this.m_itemDropManager = null;
				this.m_currentRoom = null;
				this.m_miniMapDisplay.Dispose();
				this.m_miniMapDisplay = null;
				this.m_mapBG.Dispose();
				this.m_mapBG = null;
				this.m_inputMap.Dispose();
				this.m_inputMap = null;
				this.m_lastEnemyHit = null;
				this.m_playerHUD.Dispose();
				this.m_playerHUD = null;
				this.m_player = null;
				this.m_enemyHUD.Dispose();
				this.m_enemyHUD = null;
				this.m_impactEffectPool.Dispose();
				this.m_impactEffectPool = null;
				this.m_blackBorder1.Dispose();
				this.m_blackBorder1 = null;
				this.m_blackBorder2.Dispose();
				this.m_blackBorder2 = null;
				this.m_chestList.Clear();
				this.m_chestList = null;
				this.m_projectileIconPool.Dispose();
				this.m_projectileIconPool = null;
				this.m_objKilledPlayer = null;
				this.m_dungeonLight.Dispose();
				this.m_dungeonLight = null;
				this.m_traitAura.Dispose();
				this.m_traitAura = null;
				this.m_killedEnemyObjList.Clear();
				this.m_killedEnemyObjList = null;
				this.m_roomEnteringTitle.Dispose();
				this.m_roomEnteringTitle = null;
				this.m_roomTitle.Dispose();
				this.m_roomTitle = null;
				this.m_creditsText.Dispose();
				this.m_creditsText = null;
				this.m_creditsTitleText.Dispose();
				this.m_creditsTitleText = null;
				Array.Clear(this.m_creditsTextTitleList, 0, this.m_creditsTextTitleList.Length);
				Array.Clear(this.m_creditsTextList, 0, this.m_creditsTextList.Length);
				this.m_creditsTextTitleList = null;
				this.m_creditsTextList = null;
				this.m_filmGrain.Dispose();
				this.m_filmGrain = null;
				this.m_objectivePlate.Dispose();
				this.m_objectivePlate = null;
				this.m_objectivePlateTween = null;
				this.m_sky.Dispose();
				this.m_sky = null;
				this.m_whiteBG.Dispose();
				this.m_whiteBG = null;
				this.m_compassBG.Dispose();
				this.m_compassBG = null;
				this.m_compass.Dispose();
				this.m_compass = null;
				if (this.m_compassDoor != null)
				{
					this.m_compassDoor.Dispose();
				}
				this.m_compassDoor = null;
				this.m_castleBorderTexture.Dispose();
				this.m_gardenBorderTexture.Dispose();
				this.m_towerBorderTexture.Dispose();
				this.m_dungeonBorderTexture.Dispose();
				this.m_neoBorderTexture.Dispose();
				this.m_castleBorderTexture = null;
				this.m_gardenBorderTexture = null;
				this.m_towerBorderTexture = null;
				this.m_dungeonBorderTexture = null;
				this.DebugTextObj.Dispose();
				this.DebugTextObj = null;
				base.Dispose();
			}
		}
		public void SetLastEnemyHit(EnemyObj enemy)
		{
			this.m_lastEnemyHit = enemy;
			this.m_enemyHUDCounter = this.m_enemyHUDDuration;
			this.m_enemyHUD.UpdateEnemyInfo(this.m_lastEnemyHit.Name, this.m_lastEnemyHit.Level, (float)this.m_lastEnemyHit.CurrentHealth / (float)this.m_lastEnemyHit.MaxHealth);
		}
		public void KillEnemy(EnemyObj enemy)
		{
			if (enemy.SaveToFile)
			{
				Vector2 item = new Vector2((float)this.m_roomList.IndexOf(this.CurrentRoom), (float)this.CurrentRoom.EnemyList.IndexOf(enemy));
				if (item.X < 0f || item.Y < 0f)
				{
					throw new Exception("Could not find killed enemy in either CurrentRoom or CurrentRoom.EnemyList. This may be because the enemy was a blob");
				}
				Game.PlayerStats.EnemiesKilledInRun.Add(item);
			}
		}
		public void ItemDropCollected(int itemDropType)
		{
			if (itemDropType == 1)
			{
				this.m_coinsCollected++;
				return;
			}
			switch (itemDropType)
			{
			case 10:
				this.m_bagsCollected++;
				return;
			case 11:
				this.m_diamondsCollected++;
				return;
			case 12:
			case 13:
				this.m_blueprintsCollected++;
				return;
			default:
				return;
			}
		}
		public void RefreshMapChestIcons()
		{
			this.m_miniMapDisplay.RefreshChestIcons(this.CurrentRoom);
			(base.ScreenManager as RCScreenManager).RefreshMapScreenChestIcons(this.CurrentRoom);
		}
		public void DisplayObjective(string objectiveTitle, string objectiveDescription, string objectiveProgress, bool tween)
		{
			(this.m_objectivePlate.GetChildAt(4) as SpriteObj).ScaleX = 0f;
			(this.m_objectivePlate.GetChildAt(5) as SpriteObj).ScaleX = 0f;
			this.m_objectivePlate.GetChildAt(2).Opacity = 1f;
			this.m_objectivePlate.GetChildAt(3).Opacity = 1f;
			this.m_objectivePlate.X = 1470f;
			if (this.m_objectivePlateTween != null && this.m_objectivePlateTween.TweenedObject == this.m_objectivePlate && this.m_objectivePlateTween.Active)
			{
				this.m_objectivePlateTween.StopTween(false);
			}
			(this.m_objectivePlate.GetChildAt(1) as TextObj).Text = objectiveTitle;
			(this.m_objectivePlate.GetChildAt(2) as TextObj).Text = objectiveDescription;
			(this.m_objectivePlate.GetChildAt(3) as TextObj).Text = objectiveProgress;
			if (tween)
			{
				this.m_objectivePlateTween = Tween.By(this.m_objectivePlate, 0.5f, new Easing(Back.EaseOut), new string[]
				{
					"X",
					"-300"
				});
				return;
			}
			this.m_objectivePlate.X -= 300f;
		}
		public void ResetObjectivePlate(bool tween)
		{
			if (this.m_objectivePlate != null)
			{
				this.m_objectivePlate.X = 1170f;
				if (this.m_objectivePlateTween != null && this.m_objectivePlateTween.TweenedObject == this.m_objectivePlate && this.m_objectivePlateTween.Active)
				{
					this.m_objectivePlateTween.StopTween(false);
				}
				if (tween)
				{
					Tween.By(this.m_objectivePlate, 0.5f, new Easing(Back.EaseIn), new string[]
					{
						"X",
						"300"
					});
					return;
				}
				this.m_objectivePlate.X += 300f;
			}
		}
		public void UpdateObjectiveProgress(string progress)
		{
			(this.m_objectivePlate.GetChildAt(3) as TextObj).Text = progress;
		}
		public void ObjectiveFailed()
		{
			(this.m_objectivePlate.GetChildAt(1) as TextObj).Text = "Objective Failed";
			this.m_objectivePlate.GetChildAt(2).Opacity = 0.3f;
			this.m_objectivePlate.GetChildAt(3).Opacity = 0.3f;
		}
		public void ObjectiveComplete()
		{
			this.m_objectivePlate.GetChildAt(2).Opacity = 0.3f;
			this.m_objectivePlate.GetChildAt(3).Opacity = 0.3f;
			this.m_objectivePlate.X = 1170f;
			if (this.m_objectivePlateTween != null && this.m_objectivePlateTween.TweenedObject == this.m_objectivePlate && this.m_objectivePlateTween.Active)
			{
				this.m_objectivePlateTween.StopTween(false);
			}
			(this.m_objectivePlate.GetChildAt(1) as TextObj).Text = "Objective Complete!";
		}
		public override void OnEnter()
		{
			(base.ScreenManager.Game as Game).SaveManager.ResetAutosave();
			this.m_player.DisableAllWeight = false;
			this.m_player.StopAllSpells();
			this.ShoutMagnitude = 3f;
			if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
			{
				this.m_player.Scale = new Vector2(3f, 3f);
			}
			else if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
			{
				this.m_player.Scale = new Vector2(1.35f, 1.35f);
			}
			else
			{
				this.m_player.Scale = new Vector2(2f, 2f);
			}
			if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
			{
				this.m_player.ScaleX *= 0.825f;
				this.m_player.ScaleY *= 1.15f;
			}
			else if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
			{
				this.m_player.ScaleX *= 1.25f;
				this.m_player.ScaleY *= 1.175f;
			}
			this.m_player.CurrentHealth = Game.PlayerStats.CurrentHealth;
			this.m_player.CurrentMana = (float)Game.PlayerStats.CurrentMana;
			if (LevelEV.RUN_TESTROOM)
			{
				Game.ScreenManager.Player.CurrentHealth = Game.ScreenManager.Player.MaxHealth;
				Game.ScreenManager.Player.CurrentMana = Game.ScreenManager.Player.MaxMana;
			}
			this.m_player.UpdateInternalScale();
			this.CheckForRoomTransition();
			this.UpdateCamera();
			this.UpdatePlayerHUDAbilities();
			this.m_player.UpdateEquipmentColours();
			this.m_player.StopAllSpells();
			if (Game.PlayerStats.Class == 13)
			{
				this.m_miniMapDisplay.AddAllIcons(this.RoomList);
				(base.ScreenManager as RCScreenManager).AddIconsToMap(this.RoomList);
			}
			if (Game.PlayerStats.EyeballBossBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_EYES");
			}
			if (Game.PlayerStats.FairyBossBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");
			}
			if (Game.PlayerStats.BlobBossBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_SLIME");
			}
			if (Game.PlayerStats.FireballBossBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_FIRE");
			}
			if (Game.PlayerStats.LastbossBeaten || Game.PlayerStats.TimesCastleBeaten > 0)
			{
				GameUtil.UnlockAchievement("FEAR_OF_FATHERS");
			}
			if (Game.PlayerStats.TimesCastleBeaten > 1)
			{
				GameUtil.UnlockAchievement("FEAR_OF_TWINS");
			}
			if (Game.PlayerStats.ChallengeEyeballBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_BLINDNESS");
			}
			if (Game.PlayerStats.ChallengeSkullBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_BONES");
			}
			if (Game.PlayerStats.ChallengeFireballBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_CHEMICALS");
			}
			if (Game.PlayerStats.ChallengeBlobBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_SPACE");
			}
			if (Game.PlayerStats.ChallengeLastBossBeaten)
			{
				GameUtil.UnlockAchievement("FEAR_OF_RELATIVES");
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (Game.PlayerStats.EnemiesKilledList[15].W > 0f)
			{
				flag = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[22].W > 0f)
			{
				flag2 = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[32].W > 0f)
			{
				flag3 = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[12].W > 0f)
			{
				flag4 = true;
			}
			if (Game.PlayerStats.EnemiesKilledList[5].W > 0f)
			{
				flag5 = true;
			}
			if (flag && flag2 && flag3 && flag4 && flag5)
			{
				GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");
			}
			if (Game.PlayerStats.TotalHoursPlayed + Game.PlaySessionLength >= 20f)
			{
				GameUtil.UnlockAchievement("FEAR_OF_SLEEP");
			}
			if (Game.PlayerStats.TotalRunesFound > 10)
			{
				GameUtil.UnlockAchievement("LOVE_OF_MAGIC");
			}
			base.OnEnter();
		}
		public override void OnExit()
		{
			if (this.m_currentRoom != null)
			{
				this.m_currentRoom.OnExit();
			}
			SoundManager.StopAllSounds("Default");
			SoundManager.StopAllSounds("Pauseable");
			base.OnExit();
		}
		public void RevealMorning()
		{
			this.m_sky.MorningOpacity = 0f;
			Tween.To(this.m_sky, 2f, new Easing(Tween.EaseNone), new string[]
			{
				"MorningOpacity",
				"1"
			});
		}
		public void ZoomOutAllObjects()
		{
			Vector2 vector = new Vector2((float)this.CurrentRoom.Bounds.Center.X, (float)this.CurrentRoom.Bounds.Center.Y);
			List<Vector2> list = new List<Vector2>();
			float num = 0f;
			foreach (GameObj current in this.CurrentRoom.GameObjList)
			{
				int num2;
				if (current.Y < vector.Y)
				{
					num2 = this.CurrentRoom.Bounds.Top - (current.Bounds.Top + current.Bounds.Height);
				}
				else
				{
					num2 = this.CurrentRoom.Bounds.Bottom - current.Bounds.Top;
				}
				int num3;
				if (current.X < vector.X)
				{
					num3 = this.CurrentRoom.Bounds.Left - (current.Bounds.Left + current.Bounds.Width);
				}
				else
				{
					num3 = this.CurrentRoom.Bounds.Right - current.Bounds.Left;
				}
				if (Math.Abs(num3) > Math.Abs(num2))
				{
					list.Add(new Vector2(0f, (float)num2));
					Tween.By(current, 0.5f, new Easing(Back.EaseIn), new string[]
					{
						"delay",
						num.ToString(),
						"Y",
						num2.ToString()
					});
				}
				else
				{
					list.Add(new Vector2((float)num3, 0f));
					Tween.By(current, 0.5f, new Easing(Back.EaseIn), new string[]
					{
						"delay",
						num.ToString(),
						"X",
						num3.ToString()
					});
				}
				num += 0.05f;
			}
			Tween.RunFunction(num + 0.5f, this, "ZoomInAllObjects", new object[]
			{
				list
			});
		}
		public void ZoomInAllObjects(List<Vector2> objPositions)
		{
			int num = 0;
			float num2 = 1f;
			foreach (GameObj current in this.CurrentRoom.GameObjList)
			{
				Tween.By(current, 0.5f, new Easing(Back.EaseOut), new string[]
				{
					"delay",
					num2.ToString(),
					"X",
					(-objPositions[num].X).ToString(),
					"Y",
					(-objPositions[num].Y).ToString()
				});
				num++;
				num2 += 0.05f;
			}
		}
		public void UpdateLevel(GameTypes.LevelType levelType)
		{
			switch (levelType)
			{
			case GameTypes.LevelType.CASTLE:
				this.m_backgroundSprite.Scale = Vector2.One;
				this.m_foregroundSprite.Scale = Vector2.One;
				this.m_backgroundSprite.ChangeSprite("CastleBG1_Sprite", base.ScreenManager.Camera);
				this.m_foregroundSprite.ChangeSprite("CastleFG1_Sprite", base.ScreenManager.Camera);
				this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
				this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
				break;
			case GameTypes.LevelType.GARDEN:
				this.m_backgroundSprite.Scale = Vector2.One;
				this.m_foregroundSprite.Scale = Vector2.One;
				this.m_backgroundSprite.ChangeSprite("GardenBG_Sprite", base.ScreenManager.Camera);
				this.m_foregroundSprite.ChangeSprite("GardenFG_Sprite", base.ScreenManager.Camera);
				this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
				this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
				break;
			case GameTypes.LevelType.DUNGEON:
				this.m_backgroundSprite.Scale = Vector2.One;
				this.m_foregroundSprite.Scale = Vector2.One;
				this.m_backgroundSprite.ChangeSprite("DungeonBG1_Sprite", base.ScreenManager.Camera);
				this.m_foregroundSprite.ChangeSprite("DungeonFG1_Sprite", base.ScreenManager.Camera);
				this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
				this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
				break;
			case GameTypes.LevelType.TOWER:
				this.m_backgroundSprite.Scale = Vector2.One;
				this.m_foregroundSprite.Scale = Vector2.One;
				this.m_backgroundSprite.ChangeSprite("TowerBG2_Sprite", base.ScreenManager.Camera);
				this.m_foregroundSprite.ChangeSprite("TowerFG2_Sprite", base.ScreenManager.Camera);
				this.m_backgroundSprite.Scale = new Vector2(2f, 2f);
				this.m_foregroundSprite.Scale = new Vector2(2f, 2f);
				break;
			}
			if (levelType == GameTypes.LevelType.DUNGEON)
			{
				Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
				return;
			}
			Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
		}
		public void RefreshPlayerHUDPos()
		{
			this.m_playerHUD.SetPosition(new Vector2(20f, 40f));
		}
		public void UpdatePlayerHUD()
		{
			this.m_playerHUD.Update(this.m_player);
		}
		public void UpdatePlayerHUDAbilities()
		{
			this.m_playerHUD.UpdateAbilityIcons();
		}
		public void UpdatePlayerHUDSpecialItem()
		{
			this.m_playerHUD.UpdateSpecialItemIcon();
		}
		public void UpdatePlayerSpellIcon()
		{
			this.m_playerHUD.UpdateSpellIcon();
		}
		public void SetMapDisplayVisibility(bool visible)
		{
			this.m_miniMapDisplay.Visible = visible;
		}
		public void SetPlayerHUDVisibility(bool visible)
		{
			this.m_playerHUD.Visible = visible;
		}
		public void SetObjectKilledPlayer(GameObj obj)
		{
			this.m_objKilledPlayer = obj;
		}
	}
}
