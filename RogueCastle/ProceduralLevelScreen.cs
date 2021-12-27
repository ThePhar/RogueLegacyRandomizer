// 
// RogueLegacyArchipelago - ProceduralLevelScreen.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.Structs;
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
        private readonly float m_enemyHUDDuration = 2f;
        protected int BottomDoorPercent = 80;
        public TextObj DebugTextObj;
        protected int LeftDoorPercent = 80;
        public bool LoadGameData;
        private BackgroundObj m_backgroundParallaxSprite;
        private BackgroundObj m_backgroundSprite;
        private int m_bagsCollected;
        private RenderTarget2D m_bgRenderTarget;
        private SpriteObj m_blackBorder1;
        private SpriteObj m_blackBorder2;
        private int m_blueprintsCollected;
        private int m_borderSize;
        protected int m_bottomMostBorder = -2147483647;
        private Texture2D m_castleBorderTexture;
        private int m_coinsCollected;
        private SpriteObj m_compass;
        private SpriteObj m_compassBG;
        private bool m_compassDisplayed;
        private DoorObj m_compassDoor;
        private int m_creditsIndex;
        private TextObj m_creditsText;
        private string[] m_creditsTextList;
        private string[] m_creditsTextTitleList;
        private TextObj m_creditsTitleText;
        protected RoomObj m_currentRoom;
        private int m_diamondsCollected;
        private Texture2D m_dungeonBorderTexture;
        private SpriteObj m_dungeonLight;
        private EnemyHUDObj m_enemyHUD;
        private float m_enemyHUDCounter;
        private float m_enemyPauseDuration;
        private List<Vector2> m_enemyStartPositions;
        private RenderTarget2D m_fgRenderTarget;
        private SpriteObj m_filmGrain;
        private BackgroundObj m_foregroundSprite;
        private Texture2D m_gardenBorderTexture;
        private BackgroundObj m_gardenParallaxFG;
        private InputMap m_inputMap;
        protected ItemDropManager m_itemDropManager;
        private List<EnemyObj> m_killedEnemyObjList;
        private EnemyObj m_lastEnemyHit;
        protected int m_leftMostBorder = 2147483647;
        private RenderTarget2D m_lightSourceRenderTarget;
        private SpriteObj m_mapBG;
        protected MapObj m_miniMapDisplay;
        private Texture2D m_neoBorderTexture;
        private ObjContainer m_objectivePlate;
        private TweenObject m_objectivePlateTween;
        private GameObj m_objKilledPlayer;
        protected PhysicsManager m_physicsManager;
        private PlayerHUDObj m_playerHUD;
        private ProjectileIconPool m_projectileIconPool;
        protected ProjectileManager m_projectileManager;
        protected int m_rightMostBorder = -2147483647;
        private RenderTarget2D m_roomBWRenderTarget;
        private TextObj m_roomEnteringTitle;
        private TextObj m_roomTitle;
        private RenderTarget2D m_shadowRenderTarget;
        public SkyObj m_sky;
        private RenderTarget2D m_skyRenderTarget;
        private List<Vector2> m_tempEnemyStartPositions;
        protected TextManager m_textManager;
        protected int m_topMostBorder = 2147483647;
        private Texture2D m_towerBorderTexture;
        private SpriteObj m_traitAura;
        private RenderTarget2D m_traitAuraRenderTarget;
        private SpriteObj m_whiteBG;
        protected int RightDoorPercent = 80;
        protected int TopDoorPercent = 80;

        public ProceduralLevelScreen()
        {
            DisableRoomTransitioning = false;
            RoomList = new List<RoomObj>();
            m_textManager = new TextManager(700);
            m_projectileManager = new ProjectileManager(this, 700);
            m_enemyStartPositions = new List<Vector2>();
            m_tempEnemyStartPositions = new List<Vector2>();
            ImpactEffectPool = new ImpactEffectPool(2000);
            CameraLockedToPlayer = true;
            m_roomTitle = new TextObj();
            m_roomTitle.Font = Game.JunicodeLargeFont;
            m_roomTitle.Align = Types.TextAlign.Right;
            m_roomTitle.Opacity = 0f;
            m_roomTitle.FontSize = 40f;
            m_roomTitle.Position = new Vector2(1270f, 570f);
            m_roomTitle.OutlineWidth = 2;
            m_roomEnteringTitle = (m_roomTitle.Clone() as TextObj);
            m_roomEnteringTitle.Text = "Now Entering";
            m_roomEnteringTitle.FontSize = 24f;
            m_roomEnteringTitle.Y -= 50f;
            m_inputMap = new InputMap(PlayerIndex.One, false);
            m_inputMap.AddInput(0, Keys.Y);
            m_inputMap.AddInput(1, Keys.U);
            m_inputMap.AddInput(2, Keys.LeftControl);
            m_inputMap.AddInput(3, Keys.Left);
            m_inputMap.AddInput(4, Keys.Right);
            m_inputMap.AddInput(5, Keys.Up);
            m_inputMap.AddInput(6, Keys.Down);
            m_inputMap.AddInput(7, Keys.OemTilde);
            ChestList = new List<ChestObj>();
            m_miniMapDisplay = new MapObj(true, this);
            m_killedEnemyObjList = new List<EnemyObj>();
        }

        public float BackBufferOpacity { get; set; }
        public bool CameraLockedToPlayer { get; set; }
        public float ShoutMagnitude { get; set; }
        public bool DisableRoomOnEnter { get; set; }
        public bool DisableSongUpdating { get; set; }
        public bool DisableRoomTransitioning { get; set; }
        public bool JukeboxEnabled { get; set; }

        public List<RoomObj> MapRoomsUnveiled
        {
            get { return m_miniMapDisplay.AddedRoomsList; }
            set
            {
                m_miniMapDisplay.ClearRoomsAdded();
                m_miniMapDisplay.AddAllRooms(value);
            }
        }

        public List<RoomObj> MapRoomsAdded
        {
            get { return m_miniMapDisplay.AddedRoomsList; }
        }

        public PlayerObj Player { get; set; }
        public List<RoomObj> RoomList { get; private set; }

        public PhysicsManager PhysicsManager
        {
            get { return m_physicsManager; }
        }

        public RoomObj CurrentRoom
        {
            get { return m_currentRoom; }
        }

        public ProjectileManager ProjectileManager
        {
            get { return m_projectileManager; }
        }

        public List<EnemyObj> EnemyList
        {
            get { return CurrentRoom.EnemyList; }
        }

        public List<ChestObj> ChestList { get; private set; }

        public TextManager TextManager
        {
            get { return m_textManager; }
        }

        public ImpactEffectPool ImpactEffectPool { get; private set; }

        public ItemDropManager ItemDropManager
        {
            get { return m_itemDropManager; }
        }

        public LevelType CurrentLevelType
        {
            get { return m_currentRoom.LevelType; }
        }

        public int LeftBorder
        {
            get { return m_leftMostBorder; }
        }

        public int RightBorder
        {
            get { return m_rightMostBorder; }
        }

        public int TopBorder
        {
            get { return m_topMostBorder; }
        }

        public int BottomBorder
        {
            get { return m_bottomMostBorder; }
        }

        public RenderTarget2D RenderTarget { get; private set; }
        public bool EnemiesPaused { get; private set; }

        public override void LoadContent()
        {
            DebugTextObj = new TextObj(Game.JunicodeFont);
            DebugTextObj.FontSize = 26f;
            DebugTextObj.Align = Types.TextAlign.Centre;
            DebugTextObj.Text = "";
            DebugTextObj.ForceDraw = true;
            m_projectileIconPool = new ProjectileIconPool(200, m_projectileManager, ScreenManager as RCScreenManager);
            m_projectileIconPool.Initialize();
            m_textManager.Initialize();
            ImpactEffectPool.Initialize();
            m_physicsManager = (ScreenManager.Game as Game).PhysicsManager;
            m_physicsManager.SetGravity(0f, 1830f);
            m_projectileManager.Initialize();
            m_physicsManager.Initialize(ScreenManager.Camera);
            m_itemDropManager = new ItemDropManager(600, m_physicsManager);
            m_itemDropManager.Initialize();
            m_playerHUD = new PlayerHUDObj();
            m_playerHUD.SetPosition(new Vector2(20f, 40f));
            m_enemyHUD = new EnemyHUDObj();
            m_enemyHUD.Position = new Vector2(660 - m_enemyHUD.Width/2, 20f);
            m_miniMapDisplay.SetPlayer(Player);
            m_miniMapDisplay.InitializeAlphaMap(new Rectangle(1070, 50, 200, 100), Camera);
            InitializeAllRooms(true);
            InitializeEnemies();
            InitializeChests(true);
            InitializeRenderTargets();
            m_mapBG = new SpriteObj("MinimapBG_Sprite");
            m_mapBG.Position = new Vector2(1070f, 50f);
            m_mapBG.ForceDraw = true;
            UpdateCamera();
            m_borderSize = 100;
            m_blackBorder1 = new SpriteObj("Blank_Sprite");
            m_blackBorder1.TextureColor = Color.Black;
            m_blackBorder1.Scale = new Vector2(1340f/m_blackBorder1.Width, m_borderSize/m_blackBorder1.Height);
            m_blackBorder2 = new SpriteObj("Blank_Sprite");
            m_blackBorder2.TextureColor = Color.Black;
            m_blackBorder2.Scale = new Vector2(1340f/m_blackBorder2.Width, m_borderSize/m_blackBorder2.Height);
            m_blackBorder1.ForceDraw = true;
            m_blackBorder2.ForceDraw = true;
            m_blackBorder1.Y = -(float) m_borderSize;
            m_blackBorder2.Y = 720f;
            m_dungeonLight = new SpriteObj("LightSource_Sprite");
            m_dungeonLight.ForceDraw = true;
            m_dungeonLight.Scale = new Vector2(12f, 12f);
            m_traitAura = new SpriteObj("LightSource_Sprite");
            m_traitAura.ForceDraw = true;
            m_objectivePlate = new ObjContainer("DialogBox_Character");
            m_objectivePlate.ForceDraw = true;
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Position = new Vector2(-400f, -60f);
            textObj.OverrideParentScale = true;
            textObj.FontSize = 10f;
            textObj.Text = "Fairy Chest Objective:";
            textObj.TextureColor = Color.Red;
            textObj.OutlineWidth = 2;
            m_objectivePlate.AddChild(textObj);
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.OverrideParentScale = true;
            textObj2.Position = new Vector2(textObj.X, textObj.Y + 40f);
            textObj2.ForceDraw = true;
            textObj2.FontSize = 9f;
            textObj2.Text = "Reach the chest in 15 seconds:";
            textObj2.WordWrap(250);
            textObj2.OutlineWidth = 2;
            m_objectivePlate.AddChild(textObj2);
            var textObj3 = new TextObj(Game.JunicodeFont);
            textObj3.OverrideParentScale = true;
            textObj3.Position = new Vector2(textObj2.X, textObj2.Y + 35f);
            textObj3.ForceDraw = true;
            textObj3.FontSize = 9f;
            textObj3.Text = "Time Remaining:";
            textObj3.WordWrap(250);
            textObj3.OutlineWidth = 2;
            m_objectivePlate.AddChild(textObj3);
            m_objectivePlate.Scale = new Vector2(250f/m_objectivePlate.GetChildAt(0).Width,
                130f/m_objectivePlate.GetChildAt(0).Height);
            m_objectivePlate.Position = new Vector2(1470f, 250f);
            var spriteObj = new SpriteObj("Blank_Sprite");
            spriteObj.TextureColor = Color.Red;
            spriteObj.Position = new Vector2(textObj2.X, textObj2.Y + 20f);
            spriteObj.ForceDraw = true;
            spriteObj.OverrideParentScale = true;
            spriteObj.ScaleY = 0.5f;
            m_objectivePlate.AddChild(spriteObj);
            var spriteObj2 = new SpriteObj("Blank_Sprite");
            spriteObj2.TextureColor = Color.Red;
            spriteObj2.Position = new Vector2(textObj2.X, spriteObj.Y + 35f);
            spriteObj2.ForceDraw = true;
            spriteObj2.OverrideParentScale = true;
            spriteObj2.ScaleY = 0.5f;
            m_objectivePlate.AddChild(spriteObj2);
            base.LoadContent();
            m_sky = new SkyObj(this);
            m_sky.LoadContent(Camera);
            m_whiteBG = new SpriteObj("Blank_Sprite");
            m_whiteBG.Opacity = 0f;
            m_whiteBG.Scale = new Vector2(1320f/m_whiteBG.Width, 720f/m_whiteBG.Height);
            m_filmGrain = new SpriteObj("FilmGrain_Sprite");
            m_filmGrain.ForceDraw = true;
            m_filmGrain.Scale = new Vector2(2.015f, 2.05f);
            m_filmGrain.X -= 5f;
            m_filmGrain.Y -= 5f;
            m_filmGrain.PlayAnimation();
            m_filmGrain.AnimationDelay = 0.0333333351f;
            m_compassBG = new SpriteObj("CompassBG_Sprite");
            m_compassBG.ForceDraw = true;
            m_compassBG.Position = new Vector2(660f, 90f);
            m_compassBG.Scale = Vector2.Zero;
            m_compass = new SpriteObj("Compass_Sprite");
            m_compass.Position = m_compassBG.Position;
            m_compass.ForceDraw = true;
            m_compass.Scale = Vector2.Zero;
            InitializeCreditsText();
        }

        private void InitializeCreditsText()
        {
            m_creditsTextTitleList = new[]
            {
                "Developed by",
                "Design",
                "Programming",
                "Art",
                "Sound Design & Music",
                "Music",
                ""
            };
            m_creditsTextList = new[]
            {
                "Cellar Door Games",
                "Teddy Lee",
                "Kenny Lee",
                "Glauber Kotaki",
                "Gordon McGladdery",
                "Judson Cowan",
                "Rogue Legacy"
            };
            m_creditsText = new TextObj(Game.JunicodeFont);
            m_creditsText.FontSize = 20f;
            m_creditsText.Text = "Cellar Door Games";
            m_creditsText.DropShadow = new Vector2(2f, 2f);
            m_creditsText.Opacity = 0f;
            m_creditsTitleText = (m_creditsText.Clone() as TextObj);
            m_creditsTitleText.FontSize = 14f;
            m_creditsTitleText.Position = new Vector2(50f, 580f);
            m_creditsText.Position = m_creditsTitleText.Position;
            m_creditsText.Y += 35f;
            m_creditsTitleText.X += 5f;
        }

        public void DisplayCreditsText(bool resetIndex)
        {
            if (resetIndex)
            {
                m_creditsIndex = 0;
            }
            m_creditsTitleText.Opacity = 0f;
            m_creditsText.Opacity = 0f;
            if (m_creditsIndex < m_creditsTextList.Length)
            {
                m_creditsTitleText.Opacity = 0f;
                m_creditsText.Opacity = 0f;
                m_creditsTitleText.Text = m_creditsTextTitleList[m_creditsIndex];
                m_creditsText.Text = m_creditsTextList[m_creditsIndex];
                Tween.To(m_creditsTitleText, 0.5f, Tween.EaseNone, "Opacity", "1");
                Tween.To(m_creditsText, 0.5f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
                m_creditsTitleText.Opacity = 1f;
                m_creditsText.Opacity = 1f;
                Tween.To(m_creditsTitleText, 0.5f, Tween.EaseNone, "delay", "4", "Opacity", "0");
                Tween.To(m_creditsText, 0.5f, Tween.EaseNone, "delay", "4.2", "Opacity", "0");
                m_creditsTitleText.Opacity = 0f;
                m_creditsText.Opacity = 0f;
                m_creditsIndex++;
                Tween.RunFunction(8f, this, "DisplayCreditsText", false);
            }
        }

        public void StopCreditsText()
        {
            m_creditsIndex = 0;
            Tween.StopAllContaining(m_creditsTitleText, false);
            Tween.StopAllContaining(m_creditsText, false);
            Tween.StopAllContaining(this, false);
            m_creditsTitleText.Opacity = 0f;
        }

        public override void ReinitializeRTs()
        {
            m_sky.ReinitializeRT(Camera);
            m_miniMapDisplay.InitializeAlphaMap(new Rectangle(1070, 50, 200, 100), Camera);
            InitializeRenderTargets();
            InitializeAllRooms(false);
            if (CurrentRoom == null || CurrentRoom.Name != "Start")
            {
                if (CurrentRoom.Name == "ChallengeBoss")
                {
                    m_backgroundSprite.Scale = Vector2.One;
                    m_backgroundSprite.ChangeSprite("NeoBG_Sprite", ScreenManager.Camera);
                    m_backgroundSprite.Scale = new Vector2(2f, 2f);
                    m_foregroundSprite.Scale = Vector2.One;
                    m_foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                    m_foregroundSprite.Scale = new Vector2(2f, 2f);
                }
                else
                {
                    switch (CurrentRoom.LevelType)
                    {
                        case LevelType.Castle:
                            m_backgroundSprite.Scale = Vector2.One;
                            m_foregroundSprite.Scale = Vector2.One;
                            m_backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                            m_foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                            m_backgroundSprite.Scale = new Vector2(2f, 2f);
                            m_foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;
                        case LevelType.Garden:
                            m_backgroundSprite.Scale = Vector2.One;
                            m_foregroundSprite.Scale = Vector2.One;
                            m_backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                            m_foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                            m_backgroundSprite.Scale = new Vector2(2f, 2f);
                            m_foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;
                        case LevelType.Dungeon:
                            m_backgroundSprite.Scale = Vector2.One;
                            m_foregroundSprite.Scale = Vector2.One;
                            m_backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                            m_foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                            m_backgroundSprite.Scale = new Vector2(2f, 2f);
                            m_foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;
                        case LevelType.Tower:
                            m_backgroundSprite.Scale = Vector2.One;
                            m_foregroundSprite.Scale = Vector2.One;
                            m_backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                            m_foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                            m_backgroundSprite.Scale = new Vector2(2f, 2f);
                            m_foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;
                    }
                }
                if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
                {
                    m_foregroundSprite.Scale = Vector2.One;
                    m_foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                    m_foregroundSprite.Scale = new Vector2(2f, 2f);
                }
            }
            m_backgroundSprite.Position = CurrentRoom.Position;
            m_foregroundSprite.Position = CurrentRoom.Position;
            base.ReinitializeRTs();
        }

        private void LoadPhysicsObjects(RoomObj room)
        {
            var value = new Rectangle((int) room.X - 100, (int) room.Y - 100, room.Width + 200, room.Height + 200);
            m_physicsManager.RemoveAllObjects();
            foreach (var current in CurrentRoom.TerrainObjList)
            {
                m_physicsManager.AddObject(current);
            }
            foreach (var current2 in m_projectileManager.ActiveProjectileList)
            {
                m_physicsManager.AddObject(current2);
            }
            foreach (var current3 in CurrentRoom.GameObjList)
            {
                var physicsObj = current3 as IPhysicsObj;
                if (physicsObj != null && current3.Bounds.Intersects(value))
                {
                    var breakableObj = current3 as BreakableObj;
                    if (breakableObj == null || !breakableObj.Broken)
                    {
                        m_physicsManager.AddObject(physicsObj);
                    }
                }
            }
            foreach (var current4 in CurrentRoom.DoorList)
            {
                m_physicsManager.AddObject(current4);
            }
            foreach (var current5 in CurrentRoom.EnemyList)
            {
                m_physicsManager.AddObject(current5);
                if (current5 is EnemyObj_BallAndChain && !current5.IsKilled)
                {
                    m_physicsManager.AddObject((current5 as EnemyObj_BallAndChain).BallAndChain);
                    if (current5.Difficulty > EnemyDifficulty.Basic)
                    {
                        m_physicsManager.AddObject((current5 as EnemyObj_BallAndChain).BallAndChain2);
                    }
                }
            }
            foreach (var current6 in CurrentRoom.TempEnemyList)
            {
                m_physicsManager.AddObject(current6);
            }
            m_physicsManager.AddObject(Player);
        }

        public void InitializeEnemies()
        {
            var list = new List<TerrainObj>();
            foreach (var current in RoomList)
            {
                foreach (var current2 in current.EnemyList)
                {
                    current2.SetPlayerTarget(Player);
                    current2.SetLevelScreen(this);
                    var level = current.Level;
                    if (current.Name == "Boss" && current.LinkedRoom != null)
                    {
                        level = current.LinkedRoom.Level;
                        var level2 = (int) (level/(4f + Game.PlayerStats.GetNumberOfEquippedRunes(9)*0.75f));
                        current2.Level = level2;
                    }
                    else
                    {
                        var num = (int) (level/(4f + Game.PlayerStats.GetNumberOfEquippedRunes(9)*0.75f));
                        if (num < 1)
                        {
                            num = 1;
                        }
                        current2.Level = num;
                    }
                    var num2 = current2.Level/32;
                    if (num2 > 2)
                    {
                        num2 = 2;
                    }
                    if (current2.IsProcedural)
                    {
                        if (current2.Difficulty == EnemyDifficulty.Expert)
                        {
                            current2.Level += 4;
                        }
                        if (current2.Difficulty < (EnemyDifficulty) num2)
                        {
                            current2.SetDifficulty((EnemyDifficulty) num2, false);
                        }
                    }
                    else if (current2.Difficulty == EnemyDifficulty.MiniBoss)
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
                        var num3 = 3.40282347E+38f;
                        TerrainObj terrainObj = null;
                        list.Clear();
                        var rectangle = new Rectangle((int) current2.X, current2.TerrainBounds.Bottom, 1, 5000);
                        foreach (var current3 in current.TerrainObjList)
                        {
                            if (current3.Rotation == 0f)
                            {
                                if (current3.Bounds.Top >= current2.TerrainBounds.Bottom &&
                                    CollisionMath.Intersects(current3.Bounds, rectangle))
                                {
                                    list.Add(current3);
                                }
                            }
                            else if (CollisionMath.RotatedRectIntersects(rectangle, 0f, Vector2.Zero,
                                current3.TerrainBounds, current3.Rotation, Vector2.Zero))
                            {
                                list.Add(current3);
                            }
                        }
                        foreach (var current4 in list)
                        {
                            var flag = false;
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
                                    vector = CollisionMath.UpperLeftCorner(current4.TerrainBounds, current4.Rotation,
                                        Vector2.Zero);
                                    vector2 = CollisionMath.UpperRightCorner(current4.TerrainBounds, current4.Rotation,
                                        Vector2.Zero);
                                }
                                else if (current4.Rotation > 0f)
                                {
                                    vector = CollisionMath.LowerLeftCorner(current4.TerrainBounds, current4.Rotation,
                                        Vector2.Zero);
                                    vector2 = CollisionMath.UpperLeftCorner(current4.TerrainBounds, current4.Rotation,
                                        Vector2.Zero);
                                }
                                else
                                {
                                    vector = CollisionMath.UpperRightCorner(current4.TerrainBounds, current4.Rotation,
                                        Vector2.Zero);
                                    vector2 = CollisionMath.LowerRightCorner(current4.TerrainBounds, current4.Rotation,
                                        Vector2.Zero);
                                }
                                if (current2.X > vector.X && current2.X < vector2.X)
                                {
                                    flag = true;
                                }
                                var num5 = vector2.X - vector.X;
                                var num6 = vector2.Y - vector.Y;
                                var x = vector.X;
                                var y = vector.Y;
                                var x2 = current2.X;
                                num4 = (int) (y + (x2 - x)*(num6/num5)) - current2.TerrainBounds.Bottom;
                            }
                            if (flag && num4 < num3 && num4 > 0)
                            {
                                num3 = num4;
                                terrainObj = current4;
                            }
                        }
                        if (terrainObj != null)
                        {
                            current2.UpdateCollisionBoxes();
                            if (terrainObj.Rotation == 0f)
                            {
                                current2.Y = terrainObj.Y - (current2.TerrainBounds.Bottom - current2.Y);
                            }
                            else
                            {
                                HookEnemyToSlope(current2, terrainObj);
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
            var num = vector2.X - vector.X;
            var num2 = vector2.Y - vector.Y;
            var x = vector.X;
            var y = vector.Y;
            var x2 = enemy.X;
            var num3 = y + (x2 - x)*(num2/num);
            enemy.UpdateCollisionBoxes();
            num3 -= enemy.Bounds.Bottom - enemy.Y + 5f*(enemy as GameObj).ScaleX;
            enemy.Y = (float) Math.Round(num3, MidpointRounding.ToEven);
        }

        public void InitializeChests(bool resetChests)
        {
            ChestList.Clear();
            foreach (var current in RoomList)
            {
                foreach (var current2 in current.GameObjList)
                {
                    var chestObj = current2 as ChestObj;
                    if (chestObj != null && chestObj.ChestType != 4)
                    {
                        chestObj.Level = (int) (current.Level/(4f + Game.PlayerStats.GetNumberOfEquippedRunes(9)*0.75f));
                        if (chestObj.IsProcedural)
                        {
                            if (resetChests)
                            {
                                chestObj.ResetChest();
                            }
                            var num = CDGMath.RandomInt(1, 100);
                            var num2 = 0;
                            var i = 0;
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
                                i++;
                            }
                        }
                        ChestList.Add(chestObj);
                    }
                    else if (chestObj != null && chestObj.ChestType == 4)
                    {
                        var fairyChestObj = chestObj as FairyChestObj;
                        if (fairyChestObj != null)
                        {
                            if (chestObj.IsProcedural && resetChests)
                            {
                                fairyChestObj.ResetChest();
                            }
                            fairyChestObj.SetConditionType();
                        }
                    }
                    ChestList.Add(chestObj);
                    if (chestObj != null)
                    {
                        chestObj.X += chestObj.Width/2;
                        chestObj.Y += 60f;
                    }
                }
            }
        }

        public void InitializeAllRooms(bool loadContent)
        {
            m_castleBorderTexture = new SpriteObj("CastleBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString = "CastleCorner_Sprite";
            var cornerLTextureString = "CastleCornerL_Sprite";
            m_towerBorderTexture = new SpriteObj("TowerBorder2_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString2 = "TowerCorner_Sprite";
            var cornerLTextureString2 = "TowerCornerL_Sprite";
            m_dungeonBorderTexture = new SpriteObj("DungeonBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString3 = "DungeonCorner_Sprite";
            var cornerLTextureString3 = "DungeonCornerL_Sprite";
            m_gardenBorderTexture = new SpriteObj("GardenBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString4 = "GardenCorner_Sprite";
            var cornerLTextureString4 = "GardenCornerL_Sprite";
            m_neoBorderTexture = new SpriteObj("NeoBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var text = "NeoCorner_Sprite";
            var text2 = "NeoCornerL_Sprite";
            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
            {
                cornerLTextureString3 =
                    (cornerLTextureString = (cornerLTextureString2 = (cornerLTextureString4 = text2)));
                cornerTextureString3 = (cornerTextureString = (cornerTextureString2 = (cornerTextureString4 = text)));
            }
            var num = 0;
            num = Game.PlayerStats.GetNumberOfEquippedRunes(8)*8;
            if (m_roomBWRenderTarget != null)
            {
                m_roomBWRenderTarget.Dispose();
            }
            m_roomBWRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            foreach (var current in RoomList)
            {
                var num2 = 0;
                switch (current.LevelType)
                {
                    case LevelType.Castle:
                        num2 = 0;
                        break;
                    case LevelType.Garden:
                        num2 = 0;
                        break;
                    case LevelType.Dungeon:
                        num2 = 0;
                        break;
                    case LevelType.Tower:
                        num2 = 0;
                        break;
                }
                if (Game.PlayerStats.TimesCastleBeaten == 0)
                {
                    current.Level = num + num2;
                }
                else
                {
                    current.Level = num + num2 + (128 + (Game.PlayerStats.TimesCastleBeaten - 1)*128);
                }
                num++;
                if (loadContent)
                {
                    current.LoadContent(Camera.GraphicsDevice);
                }
                current.InitializeRenderTarget(m_roomBWRenderTarget);
                if (current.Name == "ChallengeBoss")
                {
                    using (var enumerator2 = current.BorderList.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            var current2 = enumerator2.Current;
                            current2.SetBorderTextures(m_neoBorderTexture, text, text2);
                            current2.NeoTexture = m_neoBorderTexture;
                        }
                        goto IL_3D6;
                    }
                    goto IL_311;
                }
                goto IL_311;
                IL_3D6:
                var flag = false;
                if (Game.PlayerStats.Traits.X == 33f || Game.PlayerStats.Traits.Y == 33f)
                {
                    flag = true;
                }
                foreach (var current3 in current.GameObjList)
                {
                    var hazardObj = current3 as HazardObj;
                    if (hazardObj != null)
                    {
                        hazardObj.InitializeTextures(Camera);
                    }
                    var hoverObj = current3 as HoverObj;
                    if (hoverObj != null)
                    {
                        hoverObj.SetStartingPos(hoverObj.Position);
                    }
                    if (flag)
                    {
                        var breakableObj = current3 as BreakableObj;
                        if (breakableObj != null && !breakableObj.HitBySpellsOnly && !breakableObj.HasTerrainHitBox)
                        {
                            breakableObj.CollisionBoxes.Add(new CollisionBox(breakableObj.RelativeBounds.X,
                                breakableObj.RelativeBounds.Y, breakableObj.Width, breakableObj.Height, 0, breakableObj));
                            breakableObj.DisableHitboxUpdating = true;
                            breakableObj.UpdateTerrainBox();
                        }
                    }
                }
                if (LevelENV.RunTestRoom && loadContent)
                {
                    foreach (var current4 in current.GameObjList)
                    {
                        if (current4 is PlayerStartObj)
                        {
                            Player.Position = current4.Position;
                        }
                    }
                }
                if ((current.Name == "Boss" || current.Name == "ChallengeBoss") && current.LinkedRoom != null)
                {
                    CloseBossDoor(current.LinkedRoom, current.LevelType);
                }
                continue;
                IL_311:
                foreach (var current5 in current.BorderList)
                {
                    switch (current.LevelType)
                    {
                        case LevelType.Castle:
                            goto IL_39E;
                        case LevelType.Garden:
                            current5.SetBorderTextures(m_gardenBorderTexture, cornerTextureString4,
                                cornerLTextureString4);
                            current5.TextureOffset = new Vector2(0f, -18f);
                            break;
                        case LevelType.Dungeon:
                            current5.SetBorderTextures(m_dungeonBorderTexture, cornerTextureString3,
                                cornerLTextureString3);
                            break;
                        case LevelType.Tower:
                            current5.SetBorderTextures(m_towerBorderTexture, cornerTextureString2, cornerLTextureString2);
                            break;
                        default:
                            goto IL_39E;
                    }
                    IL_3AD:
                    current5.NeoTexture = m_neoBorderTexture;
                    continue;
                    IL_39E:
                    current5.SetBorderTextures(m_castleBorderTexture, cornerTextureString, cornerLTextureString);
                    goto IL_3AD;
                }
                goto IL_3D6;
            }
        }

        public void CloseBossDoor(RoomObj linkedRoom, LevelType levelType)
        {
            var flag = false;
            switch (levelType)
            {
                case LevelType.Castle:
                    if (Game.PlayerStats.EyeballBossBeaten)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Garden:
                    if (Game.PlayerStats.FairyBossBeaten)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Dungeon:
                    if (Game.PlayerStats.BlobBossBeaten)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Tower:
                    if (Game.PlayerStats.FireballBossBeaten)
                    {
                        flag = true;
                    }
                    break;
            }
            if (flag)
            {
                foreach (var current in linkedRoom.DoorList)
                {
                    if (current.IsBossDoor)
                    {
                        foreach (var current2 in linkedRoom.GameObjList)
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
            OpenChallengeBossDoor(linkedRoom, levelType);
            if (Game.PlayerStats.ChallengeLastBossUnlocked)
            {
                OpenLastBossChallengeDoors();
            }
        }

        public void OpenLastBossChallengeDoors()
        {
            LastBossChallengeRoom linkedRoom = null;
            foreach (var current in RoomList)
            {
                if (current.Name == "ChallengeBoss" && current is LastBossChallengeRoom)
                {
                    linkedRoom = (current as LastBossChallengeRoom);
                    break;
                }
            }
            foreach (var current2 in RoomList)
            {
                if (current2.Name == "EntranceBoss")
                {
                    var flag = false;
                    if (current2.LevelType == LevelType.Castle && Game.PlayerStats.EyeballBossBeaten)
                    {
                        flag = true;
                    }
                    else if (current2.LevelType == LevelType.Dungeon && Game.PlayerStats.BlobBossBeaten)
                    {
                        flag = true;
                    }
                    else if (current2.LevelType == LevelType.Garden && Game.PlayerStats.FairyBossBeaten)
                    {
                        flag = true;
                    }
                    else if (current2.LevelType == LevelType.Tower && Game.PlayerStats.FireballBossBeaten)
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        foreach (var current3 in current2.DoorList)
                        {
                            if (current3.IsBossDoor)
                            {
                                current2.LinkedRoom = linkedRoom;
                                foreach (var current4 in current2.GameObjList)
                                {
                                    if (current4.Name == "BossDoor")
                                    {
                                        if (Game.PlayerStats.ChallengeLastBossBeaten)
                                        {
                                            if ((current4 as SpriteObj).SpriteName.Contains("Open"))
                                            {
                                                current4.ChangeSprite((current4 as SpriteObj).SpriteName.Replace(
                                                    "Open", ""));
                                            }
                                            current4.TextureColor = Color.White;
                                            current4.Opacity = 1f;
                                            current2.LinkedRoom = null;
                                            current3.Locked = true;
                                            break;
                                        }
                                        if (!(current4 as SpriteObj).SpriteName.Contains("Open"))
                                        {
                                            current4.ChangeSprite((current4 as SpriteObj).SpriteName.Replace("_Sprite",
                                                "Open_Sprite"));
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

        public void OpenChallengeBossDoor(RoomObj linkerRoom, LevelType levelType)
        {
            var flag = false;
            switch (levelType)
            {
                case LevelType.Castle:
                    if (Game.PlayerStats.EyeballBossBeaten && !Game.PlayerStats.ChallengeEyeballBeaten &&
                        Game.PlayerStats.ChallengeEyeballUnlocked)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Garden:
                    if (Game.PlayerStats.FairyBossBeaten && !Game.PlayerStats.ChallengeSkullBeaten &&
                        Game.PlayerStats.ChallengeSkullUnlocked)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Dungeon:
                    if (Game.PlayerStats.BlobBossBeaten && !Game.PlayerStats.ChallengeBlobBeaten &&
                        Game.PlayerStats.ChallengeBlobUnlocked)
                    {
                        flag = true;
                    }
                    break;
                case LevelType.Tower:
                    if (Game.PlayerStats.FireballBossBeaten && !Game.PlayerStats.ChallengeFireballBeaten &&
                        Game.PlayerStats.ChallengeFireballUnlocked)
                    {
                        flag = true;
                    }
                    break;
            }
            if (flag)
            {
                var challengeBossRoomFromRoomList = LevelBuilder2.GetChallengeBossRoomFromRoomList(levelType,
                    RoomList);
                linkerRoom.LinkedRoom = challengeBossRoomFromRoomList;
                foreach (var current in linkerRoom.DoorList)
                {
                    if (current.IsBossDoor)
                    {
                        foreach (var current2 in linkerRoom.GameObjList)
                        {
                            if (current2.Name == "BossDoor")
                            {
                                current2.ChangeSprite((current2 as SpriteObj).SpriteName.Replace("_Sprite",
                                    "Open_Sprite"));
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
            foreach (var current in roomsToAdd)
            {
                RoomList.Add(current);
                if (current.X < m_leftMostBorder)
                {
                    m_leftMostBorder = (int) current.X;
                }
                if (current.X + current.Width > m_rightMostBorder)
                {
                    m_rightMostBorder = (int) current.X + current.Width;
                }
                if (current.Y < m_topMostBorder)
                {
                    m_topMostBorder = (int) current.Y;
                }
                if (current.Y + current.Height > m_bottomMostBorder)
                {
                    m_bottomMostBorder = (int) current.Y + current.Height;
                }
            }
        }

        public void AddRoom(RoomObj room)
        {
            RoomList.Add(room);
            if (room.X < m_leftMostBorder)
            {
                m_leftMostBorder = (int) room.X;
            }
            if (room.X + room.Width > m_rightMostBorder)
            {
                m_rightMostBorder = (int) room.X + room.Width;
            }
            if (room.Y < m_topMostBorder)
            {
                m_topMostBorder = (int) room.Y;
            }
            if (room.Y + room.Height > m_bottomMostBorder)
            {
                m_bottomMostBorder = (int) room.Y + room.Height;
            }
        }

        private void CheckForRoomTransition()
        {
            if (Player != null)
            {
                foreach (var current in RoomList)
                {
                    if (current != CurrentRoom && current.Bounds.Contains((int) Player.X, (int) Player.Y))
                    {
                        ResetEnemyPositions();
                        if (CurrentRoom != null)
                        {
                            foreach (var current2 in EnemyList)
                            {
                                current2.ResetState();
                            }
                        }
                        if (EnemiesPaused)
                        {
                            UnpauseAllEnemies();
                        }
                        Player.RoomTransitionReset();
                        m_miniMapDisplay.AddRoom(current);
                        if (current.Name != "Start")
                        {
                            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.MapData);
                        }
                        if (current.Name == "ChallengeBoss")
                        {
                            m_backgroundSprite.Scale = Vector2.One;
                            m_backgroundSprite.ChangeSprite("NeoBG_Sprite", ScreenManager.Camera);
                            m_backgroundSprite.Scale = new Vector2(2f, 2f);
                            m_foregroundSprite.Scale = Vector2.One;
                            m_foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                            m_foregroundSprite.Scale = new Vector2(2f, 2f);
                        }
                        if ((CurrentRoom == null || CurrentLevelType != current.LevelType ||
                             (CurrentRoom != null && CurrentRoom.Name == "ChallengeBoss")) && current.Name != "Start")
                        {
                            if (current.Name != "ChallengeBoss")
                            {
                                switch (current.LevelType)
                                {
                                    case LevelType.Castle:
                                        m_backgroundSprite.Scale = Vector2.One;
                                        m_foregroundSprite.Scale = Vector2.One;
                                        m_backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                                        m_foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                                        m_backgroundSprite.Scale = new Vector2(2f, 2f);
                                        m_foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;
                                    case LevelType.Garden:
                                        m_backgroundSprite.Scale = Vector2.One;
                                        m_foregroundSprite.Scale = Vector2.One;
                                        m_backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                                        m_foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                                        m_backgroundSprite.Scale = new Vector2(2f, 2f);
                                        m_foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;
                                    case LevelType.Dungeon:
                                        m_backgroundSprite.Scale = Vector2.One;
                                        m_foregroundSprite.Scale = Vector2.One;
                                        m_backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                                        m_foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                                        m_backgroundSprite.Scale = new Vector2(2f, 2f);
                                        m_foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;
                                    case LevelType.Tower:
                                        m_backgroundSprite.Scale = Vector2.One;
                                        m_foregroundSprite.Scale = Vector2.One;
                                        m_backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                                        m_foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                                        m_backgroundSprite.Scale = new Vector2(2f, 2f);
                                        m_foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;
                                }
                            }
                            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
                            {
                                m_foregroundSprite.Scale = Vector2.One;
                                m_foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                                m_foregroundSprite.Scale = new Vector2(2f, 2f);
                            }
                            if (current.LevelType == LevelType.Dungeon || Game.PlayerStats.Traits.X == 35f ||
                                Game.PlayerStats.Traits.Y == 35f || current.Name == "Compass")
                            {
                                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
                            }
                            else
                            {
                                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
                            }
                            m_roomTitle.Text = WordBuilder.BuildDungeonName(current.LevelType);
                            if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
                            {
                                m_roomTitle.RandomizeSentence(false);
                            }
                            m_roomTitle.Opacity = 0f;
                            if (current.Name != "Boss" && current.Name != "Tutorial" && current.Name != "Ending" &&
                                current.Name != "ChallengeBoss")
                            {
                                Tween.StopAllContaining(m_roomEnteringTitle, false);
                                Tween.StopAllContaining(m_roomTitle, false);
                                m_roomTitle.Opacity = 0f;
                                m_roomEnteringTitle.Opacity = 0f;
                                if (Player.X > current.Bounds.Center.X)
                                {
                                    m_roomTitle.X = 50f;
                                    m_roomTitle.Align = Types.TextAlign.Left;
                                    m_roomEnteringTitle.X = 70f;
                                    m_roomEnteringTitle.Align = Types.TextAlign.Left;
                                }
                                else
                                {
                                    m_roomTitle.X = 1270f;
                                    m_roomTitle.Align = Types.TextAlign.Right;
                                    m_roomEnteringTitle.X = 1250f;
                                    m_roomEnteringTitle.Align = Types.TextAlign.Right;
                                }
                                Tween.To(m_roomTitle, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "1");
                                m_roomTitle.Opacity = 1f;
                                Tween.To(m_roomTitle, 0.5f, Linear.EaseNone, "delay", "2.2", "Opacity", "0");
                                m_roomTitle.Opacity = 0f;
                                Tween.To(m_roomEnteringTitle, 0.5f, Linear.EaseNone, "Opacity", "1");
                                m_roomEnteringTitle.Opacity = 1f;
                                Tween.To(m_roomEnteringTitle, 0.5f, Linear.EaseNone, "delay", "2", "Opacity", "0");
                                m_roomEnteringTitle.Opacity = 0f;
                            }
                            else
                            {
                                Tween.StopAllContaining(m_roomEnteringTitle, false);
                                Tween.StopAllContaining(m_roomTitle, false);
                                m_roomTitle.Opacity = 0f;
                                m_roomEnteringTitle.Opacity = 0f;
                            }
                            JukeboxEnabled = false;
                            Console.WriteLine("Now entering " + current.LevelType);
                        }
                        if (m_currentRoom != null)
                        {
                            m_currentRoom.OnExit();
                        }
                        m_currentRoom = current;
                        m_backgroundSprite.Position = CurrentRoom.Position;
                        m_foregroundSprite.Position = CurrentRoom.Position;
                        m_gardenParallaxFG.Position = CurrentRoom.Position;
                        if (SoundManager.IsMusicPaused)
                        {
                            SoundManager.ResumeMusic();
                        }
                        if (!DisableSongUpdating && !JukeboxEnabled)
                        {
                            UpdateLevelSong();
                        }
                        if (m_currentRoom.Player == null)
                        {
                            m_currentRoom.Player = Player;
                        }
                        if (m_currentRoom.Name != "Start" && m_currentRoom.Name != "Tutorial" &&
                            m_currentRoom.Name != "Ending" && m_currentRoom.Name != "CastleEntrance" &&
                            m_currentRoom.Name != "Bonus" && m_currentRoom.Name != "Throne" &&
                            m_currentRoom.Name != "Secret" && m_currentRoom.Name != "Boss" &&
                            m_currentRoom.LevelType != LevelType.None && m_currentRoom.Name != "ChallengeBoss" &&
                            (Game.PlayerStats.Traits.X == 26f || Game.PlayerStats.Traits.Y == 26f) &&
                            CDGMath.RandomFloat(0f, 1f) < 0.2f)
                        {
                            SpawnDementiaEnemy();
                        }
                        if (m_currentRoom.HasFairyChest)
                        {
                            m_currentRoom.DisplayFairyChestInfo();
                        }
                        m_tempEnemyStartPositions.Clear();
                        m_enemyStartPositions.Clear();
                        foreach (var current3 in CurrentRoom.EnemyList)
                        {
                            m_enemyStartPositions.Add(current3.Position);
                        }
                        foreach (var current4 in CurrentRoom.TempEnemyList)
                        {
                            m_tempEnemyStartPositions.Add(current4.Position);
                        }
                        m_projectileManager.DestroyAllProjectiles(false);
                        LoadPhysicsObjects(current);
                        m_itemDropManager.DestroyAllItemDrops();
                        m_projectileIconPool.DestroyAllIcons();
                        m_enemyPauseDuration = 0f;
                        if (LevelENV.ShowEnemyRadii)
                        {
                            foreach (var current5 in current.EnemyList)
                            {
                                current5.InitializeDebugRadii();
                            }
                        }
                        m_lastEnemyHit = null;
                        foreach (var current6 in m_currentRoom.GameObjList)
                        {
                            var fairyChestObj = current6 as FairyChestObj;
                            if (fairyChestObj != null)
                            {
                                var arg_BC6_0 = fairyChestObj.IsOpen;
                            }
                            var animateableObj = current6 as IAnimateableObj;
                            if (animateableObj != null && animateableObj.TotalFrames > 1 &&
                                !(animateableObj is ChestObj) && !(current6 is BreakableObj))
                            {
                                animateableObj.AnimationDelay = 0.1f;
                                animateableObj.PlayAnimation();
                            }
                        }
                        if (!DisableRoomOnEnter)
                        {
                            m_currentRoom.OnEnter();
                        }
                        break;
                    }
                }
            }
        }

        private void UpdateLevelSong()
        {
            if (!(CurrentRoom.Name != "Start") || !(CurrentRoom.Name != "Tutorial") || !(CurrentRoom.Name != "Ending") ||
                SoundManager.IsMusicPlaying)
            {
                if (!(m_currentRoom is StartingRoomObj) && SoundManager.IsMusicPlaying)
                {
                    if ((m_currentRoom is CarnivalShoot1BonusRoom || m_currentRoom is CarnivalShoot2BonusRoom) &&
                        SoundManager.GetCurrentMusicName() != "PooyanSong")
                    {
                        SoundManager.PlayMusic("PooyanSong", true, 1f);
                        return;
                    }
                    if (m_currentRoom.LevelType == LevelType.Castle &&
                        SoundManager.GetCurrentMusicName() != "CastleSong")
                    {
                        SoundManager.PlayMusic("CastleSong", true, 1f);
                        return;
                    }
                    if (m_currentRoom.LevelType == LevelType.Garden &&
                        SoundManager.GetCurrentMusicName() != "GardenSong")
                    {
                        SoundManager.PlayMusic("GardenSong", true, 1f);
                        return;
                    }
                    if (m_currentRoom.LevelType == LevelType.Dungeon &&
                        SoundManager.GetCurrentMusicName() != "DungeonSong")
                    {
                        SoundManager.PlayMusic("DungeonSong", true, 1f);
                        return;
                    }
                    if (m_currentRoom.LevelType == LevelType.Tower &&
                        SoundManager.GetCurrentMusicName() != "TowerSong")
                    {
                        SoundManager.PlayMusic("TowerSong", true, 1f);
                    }
                }
                return;
            }
            if (m_currentRoom is CarnivalShoot1BonusRoom || m_currentRoom is CarnivalShoot2BonusRoom)
            {
                SoundManager.PlayMusic("PooyanSong", true, 1f);
                return;
            }
            switch (m_currentRoom.LevelType)
            {
                case LevelType.Castle:
                    //IL_A8:
                    SoundManager.PlayMusic("CastleSong", true, 1f);
                    return;
                case LevelType.Garden:
                    SoundManager.PlayMusic("GardenSong", true, 1f);
                    return;
                case LevelType.Dungeon:
                    SoundManager.PlayMusic("DungeonSong", true, 1f);
                    return;
                case LevelType.Tower:
                    SoundManager.PlayMusic("TowerSong", true, 1f);
                    return;
                default:
                    SoundManager.PlayMusic("CastleSong", true, 1f);
                    return;
            }
            //goto IL_A8;
        }

        public override void Update(GameTime gameTime)
        {
            var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
            m_projectileIconPool.Update(Camera);
            if (!IsPaused)
            {
                m_sky.Update(gameTime);
                if (m_enemyPauseDuration > 0f)
                {
                    m_enemyPauseDuration -= num;
                    if (m_enemyPauseDuration <= 0f)
                    {
                        StopTimeStop();
                    }
                }
                CurrentRoom.Update(gameTime);
                if (Player != null)
                {
                    Player.Update(gameTime);
                }
                m_enemyHUD.Update(gameTime);
                m_playerHUD.Update(Player);
                m_projectileManager.Update(gameTime);
                m_physicsManager.Update(gameTime);
                if (!DisableRoomTransitioning &&
                    !CollisionMath.Intersects(new Rectangle((int) Player.X, (int) Player.Y, 1, 1), Camera.Bounds))
                {
                    CheckForRoomTransition();
                }
                if ((!m_inputMap.Pressed(2) ||
                     (m_inputMap.Pressed(2) && (LevelENV.RunDemoVersion || LevelENV.CreateRetailVersion))) &&
                    CameraLockedToPlayer)
                {
                    UpdateCamera();
                }
                if (Game.PlayerStats.SpecialItem == 6 && CurrentRoom.Name != "Start" && CurrentRoom.Name != "Tutorial" &&
                    CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Throne" && CurrentRoom.Name != "ChallengeBoss")
                {
                    if (!m_compassDisplayed)
                    {
                        DisplayCompass();
                    }
                    else
                    {
                        UpdateCompass();
                    }
                }
                else if (m_compassDisplayed && CurrentRoom.Name != "Compass")
                {
                    HideCompass();
                }
                if (m_objectivePlate.X == 1170f)
                {
                    var flag = false;
                    var bounds = m_objectivePlate.Bounds;
                    bounds.X += (int) Camera.TopLeftCorner.X;
                    bounds.Y += (int) Camera.TopLeftCorner.Y;
                    if (CollisionMath.Intersects(Player.Bounds, bounds))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        foreach (var current in CurrentRoom.EnemyList)
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
                        m_objectivePlate.Opacity = 0.5f;
                    }
                    else
                    {
                        m_objectivePlate.Opacity = 1f;
                    }
                }
            }
            base.Update(gameTime);
        }

        public void UpdateCamera()
        {
            if (Player != null)
            {
                ScreenManager.Camera.X = (int) (Player.Position.X + GlobalEV.Camera_XOffset);
                ScreenManager.Camera.Y = (int) (Player.Position.Y + GlobalEV.Camera_YOffset);
            }
            if (m_currentRoom != null)
            {
                if (ScreenManager.Camera.Width < m_currentRoom.Width)
                {
                    if (ScreenManager.Camera.Bounds.Left < m_currentRoom.Bounds.Left)
                    {
                        ScreenManager.Camera.X = (int) (m_currentRoom.Bounds.Left + ScreenManager.Camera.Width*0.5f);
                    }
                    else if (ScreenManager.Camera.Bounds.Right > m_currentRoom.Bounds.Right)
                    {
                        ScreenManager.Camera.X = (int) (m_currentRoom.Bounds.Right - ScreenManager.Camera.Width*0.5f);
                    }
                }
                else
                {
                    ScreenManager.Camera.X = (int) (m_currentRoom.X + m_currentRoom.Width*0.5f);
                }
                if (ScreenManager.Camera.Height < m_currentRoom.Height)
                {
                    if (ScreenManager.Camera.Bounds.Top < m_currentRoom.Bounds.Top)
                    {
                        ScreenManager.Camera.Y = (int) (m_currentRoom.Bounds.Top + ScreenManager.Camera.Height*0.5f);
                        return;
                    }
                    if (ScreenManager.Camera.Bounds.Bottom > m_currentRoom.Bounds.Bottom)
                    {
                        ScreenManager.Camera.Y = (int) (m_currentRoom.Bounds.Bottom - ScreenManager.Camera.Height*0.5f);
                    }
                }
                else
                {
                    ScreenManager.Camera.Y = (int) (m_currentRoom.Y + m_currentRoom.Height*0.5f);
                }
            }
        }

        public override void HandleInput()
        {
            if (Game.GlobalInput.JustPressed(8) && CurrentRoom.Name != "Ending")
            {
                (ScreenManager as RCScreenManager).DisplayScreen(16, true);
            }
            if (!LevelENV.RunDemoVersion && !LevelENV.CreateRetailVersion)
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
                if (m_inputMap.JustPressed(0))
                {
                    m_miniMapDisplay.AddAllRooms(RoomList);
                }
                if (m_inputMap.JustPressed(7))
                {
                    LevelENV.ShowDebugText = !LevelENV.ShowDebugText;
                }
                if (m_inputMap.JustPressed(1))
                {
                    if (Camera.Zoom < 1f)
                    {
                        Camera.Zoom = 1f;
                    }
                    else
                    {
                        Camera.Zoom = 0.05f;
                    }
                }
                var num = 2000f;
                if (m_inputMap.Pressed(2) && m_inputMap.Pressed(3))
                {
                    Camera.X -= num*(float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (m_inputMap.Pressed(2) && m_inputMap.Pressed(4))
                {
                    Camera.X += num*(float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
                if (m_inputMap.Pressed(2) && m_inputMap.Pressed(5))
                {
                    Camera.Y -= num*(float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (m_inputMap.Pressed(2) && m_inputMap.Pressed(6))
                {
                    Camera.Y += num*(float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (Player != null &&
                (!m_inputMap.Pressed(2) ||
                 (m_inputMap.Pressed(2) && (LevelENV.RunDemoVersion || LevelENV.CreateRetailVersion))) &&
                !Player.IsKilled)
            {
                Player.HandleInput();
            }
            base.HandleInput();
        }

        private void UpdateCompass()
        {
            if (m_compassDoor == null && CurrentRoom.Name != "Ending" && CurrentRoom.Name != "Boss" &&
                CurrentRoom.Name != "Start" && CurrentRoom.Name != "Tutorial" && CurrentRoom.Name != " ChallengeBoss")
            {
                Console.WriteLine("Creating new bonus room for compass");
                RoomObj roomObj = null;
                EnemyObj enemyObj = null;
                var list = new List<RoomObj>();
                foreach (var current in RoomList)
                {
                    var flag = false;
                    foreach (var current2 in current.EnemyList)
                    {
                        if (current2.IsWeighted)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (current.Name != "Ending" && current.Name != "Tutorial" && current.Name != "Boss" &&
                        current.Name != "Secret" && current.Name != "Bonus" && flag && current.Name != "ChallengeBoss")
                    {
                        list.Add(current);
                    }
                }
                if (list.Count > 0)
                {
                    roomObj = list[CDGMath.RandomInt(0, list.Count - 1)];
                    var num = 0;
                    while (enemyObj == null || !enemyObj.IsWeighted)
                    {
                        enemyObj = roomObj.EnemyList[num];
                        num++;
                    }
                    var doorObj = new DoorObj(roomObj, 120, 180, DoorType.Open);
                    doorObj.Position = enemyObj.Position;
                    doorObj.IsBossDoor = true;
                    doorObj.DoorPosition = "None";
                    doorObj.AddCollisionBox(0, 0, doorObj.Width, doorObj.Height, 0);
                    doorObj.AddCollisionBox(0, 0, doorObj.Width, doorObj.Height, 2);
                    var num2 = 3.40282347E+38f;
                    TerrainObj terrainObj = null;
                    foreach (var current3 in roomObj.TerrainObjList)
                    {
                        if (current3.Y >= doorObj.Y && current3.Y - doorObj.Y < num2 &&
                            CollisionMath.Intersects(current3.Bounds,
                                new Rectangle((int) doorObj.X, (int) (doorObj.Y + (current3.Y - doorObj.Y) + 5f),
                                    doorObj.Width, doorObj.Height/2)))
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
                            doorObj.Y = terrainObj.Y - (doorObj.TerrainBounds.Bottom - doorObj.Y);
                        }
                        else
                        {
                            HookEnemyToSlope(doorObj, terrainObj);
                        }
                    }
                    roomObj.DoorList.Add(doorObj);
                    roomObj.LinkedRoom = RoomList[RoomList.Count - 1];
                    roomObj.LinkedRoom.LinkedRoom = roomObj;
                    roomObj.LinkedRoom.LevelType = roomObj.LevelType;
                    var cornerTextureString = "CastleCorner_Sprite";
                    var cornerLTextureString = "CastleCornerL_Sprite";
                    var cornerTextureString2 = "TowerCorner_Sprite";
                    var cornerLTextureString2 = "TowerCornerL_Sprite";
                    var cornerTextureString3 = "DungeonCorner_Sprite";
                    var cornerLTextureString3 = "DungeonCornerL_Sprite";
                    var cornerTextureString4 = "GardenCorner_Sprite";
                    var cornerLTextureString4 = "GardenCornerL_Sprite";
                    if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
                    {
                        var text = "NeoCorner_Sprite";
                        var text2 = "NeoCornerL_Sprite";
                        cornerLTextureString3 =
                            (cornerLTextureString = (cornerLTextureString2 = (cornerLTextureString4 = text2)));
                        cornerTextureString3 =
                            (cornerTextureString = (cornerTextureString2 = (cornerTextureString4 = text)));
                    }
                    foreach (var current4 in roomObj.LinkedRoom.BorderList)
                    {
                        switch (roomObj.LinkedRoom.LevelType)
                        {
                            case LevelType.Garden:
                                current4.SetBorderTextures(m_gardenBorderTexture, cornerTextureString4,
                                    cornerLTextureString4);
                                current4.TextureOffset = new Vector2(0f, -18f);
                                continue;
                            case LevelType.Dungeon:
                                current4.SetBorderTextures(m_dungeonBorderTexture, cornerTextureString3,
                                    cornerLTextureString3);
                                continue;
                            case LevelType.Tower:
                                current4.SetBorderTextures(m_towerBorderTexture, cornerTextureString2,
                                    cornerLTextureString2);
                                continue;
                        }
                        current4.SetBorderTextures(m_castleBorderTexture, cornerTextureString, cornerLTextureString);
                    }
                    m_compassDoor = doorObj;
                }
            }
            if (m_compassDoor != null)
            {
                m_compass.Rotation = CDGMath.AngleBetweenPts(Player.Position,
                    new Vector2(m_compassDoor.Bounds.Center.X, m_compassDoor.Bounds.Center.Y));
            }
        }

        public void RemoveCompassDoor()
        {
            if (m_compassDoor != null)
            {
                m_compassDoor.Room.DoorList.Remove(m_compassDoor);
                m_compassDoor.Dispose();
                m_compassDoor = null;
            }
        }

        private void DisplayCompass()
        {
            Tween.StopAllContaining(m_compassBG, false);
            Tween.StopAllContaining(m_compass, false);
            Tween.To(m_compassBG, 0.5f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_compass, 0.5f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
            m_compassDisplayed = true;
        }

        private void HideCompass()
        {
            Tween.StopAllContaining(m_compassBG, false);
            Tween.StopAllContaining(m_compass, false);
            Tween.To(m_compassBG, 0.5f, Back.EaseInLarge, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_compass, 0.5f, Back.EaseInLarge, "ScaleX", "0", "ScaleY", "0");
            m_compassDisplayed = false;
            RemoveCompassDoor();
        }

        public void InitializeRenderTargets()
        {
            var num = 1320;
            var num2 = 720;
            if (LevelENV.SaveFrames)
            {
                num /= 2;
                num2 /= 2;
            }
            if (m_fgRenderTarget != null)
            {
                m_fgRenderTarget.Dispose();
            }
            m_fgRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra5551,
                DepthFormat.None);
            if (m_shadowRenderTarget != null)
            {
                m_shadowRenderTarget.Dispose();
            }
            m_shadowRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra4444,
                DepthFormat.None);
            Camera.Begin();
            Camera.GraphicsDevice.SetRenderTarget(m_shadowRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.End();
            if (m_lightSourceRenderTarget != null)
            {
                m_lightSourceRenderTarget.Dispose();
            }
            m_lightSourceRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2, false,
                SurfaceFormat.Bgra4444, DepthFormat.None);
            if (RenderTarget != null)
            {
                RenderTarget.Dispose();
            }
            RenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            if (m_skyRenderTarget != null)
            {
                m_skyRenderTarget.Dispose();
            }
            m_skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2);
            if (m_bgRenderTarget != null)
            {
                m_bgRenderTarget.Dispose();
            }
            m_bgRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color,
                DepthFormat.None);
            if (m_traitAuraRenderTarget != null)
            {
                m_traitAuraRenderTarget.Dispose();
            }
            m_traitAuraRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2);
            InitializeBackgroundObjs();
        }

        public void InitializeBackgroundObjs()
        {
            if (m_foregroundSprite != null)
            {
                m_foregroundSprite.Dispose();
            }
            m_foregroundSprite = new BackgroundObj("CastleFG1_Sprite");
            m_foregroundSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
            m_foregroundSprite.Scale = new Vector2(2f, 2f);
            if (m_backgroundSprite != null)
            {
                m_backgroundSprite.Dispose();
            }
            m_backgroundSprite = new BackgroundObj("CastleBG1_Sprite");
            m_backgroundSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
            m_backgroundSprite.Scale = new Vector2(2f, 2f);
            if (m_backgroundParallaxSprite != null)
            {
                m_backgroundParallaxSprite.Dispose();
            }
            m_backgroundParallaxSprite = new BackgroundObj("TowerBGFrame_Sprite");
            m_backgroundParallaxSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
            m_backgroundParallaxSprite.Scale = new Vector2(2f, 2f);
            if (m_gardenParallaxFG != null)
            {
                m_gardenParallaxFG.Dispose();
            }
            m_gardenParallaxFG = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
            m_gardenParallaxFG.SetRepeated(true, true, Camera, SamplerState.LinearWrap);
            m_gardenParallaxFG.TextureColor = Color.White;
            m_gardenParallaxFG.Scale = new Vector2(3f, 3f);
            m_gardenParallaxFG.Opacity = 0.7f;
            m_gardenParallaxFG.ParallaxSpeed = new Vector2(0.3f, 0f);
        }

        public void DrawRenderTargets()
        {
            if (m_backgroundSprite.Texture.IsContentLost)
            {
                ReinitializeRTs();
            }
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                Camera.GetTransformation());
            if (CurrentRoom != null)
            {
                CurrentRoom.DrawRenderTargets(Camera);
            }
            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                Camera.GetTransformation());
            Camera.GraphicsDevice.SetRenderTarget(m_fgRenderTarget);
            m_foregroundSprite.Draw(Camera);
            if (!EnemiesPaused)
            {
                if (Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f)
                {
                    m_traitAura.Scale = new Vector2(15f, 15f);
                }
                else if (Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f)
                {
                    m_traitAura.Scale = new Vector2(8f, 8f);
                }
                else
                {
                    m_traitAura.Scale = new Vector2(10f, 10f);
                }
            }
            Camera.GraphicsDevice.SetRenderTarget(m_traitAuraRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Transparent);
            if (CurrentRoom != null)
            {
                m_traitAura.Position = Player.Position;
                m_traitAura.Draw(Camera);
            }
            Camera.GraphicsDevice.SetRenderTarget(m_lightSourceRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Transparent);
            if (CurrentRoom != null)
            {
                m_dungeonLight.Position = Player.Position;
                m_dungeonLight.Draw(Camera);
            }
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            m_miniMapDisplay.DrawRenderTargets(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(m_skyRenderTarget);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            m_sky.Draw(Camera);
            Camera.End();
        }

        private static Vector2 MoveInCircle(GameTime gameTime, float speed)
        {
            double num = Game.TotalGameTimeSeconds*speed;
            var x = (float) Math.Cos(num);
            var y = (float) Math.Sin(num);
            return new Vector2(x, y);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawRenderTargets();
            Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                Camera.GetTransformation());
            m_backgroundSprite.Draw(Camera);
            if (CurrentRoom != null && Camera.Zoom == 1f &&
                (!m_inputMap.Pressed(2) ||
                 (m_inputMap.Pressed(2) && (LevelENV.RunDemoVersion || LevelENV.CreateRetailVersion))))
            {
                CurrentRoom.DrawBGObjs(Camera);
            }
            else
            {
                foreach (var current in RoomList)
                {
                    current.DrawBGObjs(Camera);
                }
            }
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            if (EnemiesPaused)
            {
                Camera.GraphicsDevice.Clear(Color.White);
            }
            Camera.GraphicsDevice.Textures[1] = m_skyRenderTarget;
            Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.ParallaxEffect);
            if (!EnemiesPaused)
            {
                Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
            }
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null,
                RasterizerState.CullNone, Game.BWMaskEffect, Camera.GetTransformation());
            Camera.GraphicsDevice.Textures[1] = m_fgRenderTarget;
            Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            Camera.Draw(CurrentRoom.BGRender, Camera.TopLeftCorner, Color.White);
            Camera.End();
            if (!LevelENV.ShowEnemyRadii)
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                    Camera.GetTransformation());
            }
            else
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                    Camera.GetTransformation());
            }
            CurrentRoom.Draw(Camera);
            if (LevelENV.ShowEnemyRadii)
            {
                foreach (var current2 in m_currentRoom.EnemyList)
                {
                    current2.DrawDetectionRadii(Camera);
                }
            }
            m_projectileManager.Draw(Camera);
            if (EnemiesPaused)
            {
                Camera.End();
                Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
                Camera.GraphicsDevice.Textures[1] = m_traitAuraRenderTarget;
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.InvertShader);
                Camera.Draw(RenderTarget, Vector2.Zero, Color.White);
                Camera.End();
                Game.HSVEffect.Parameters["Saturation"].SetValue(0);
                Game.HSVEffect.Parameters["UseMask"].SetValue(true);
                Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
                Camera.GraphicsDevice.Textures[1] = m_traitAuraRenderTarget;
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.HSVEffect);
                Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
            }
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.GetTransformation());
            Camera.Draw(Game.GenericTexture,
                new Rectangle((int) Camera.TopLeftCorner.X, (int) Camera.TopLeftCorner.Y, 1320, 720),
                Color.Black*BackBufferOpacity);
            if (!Player.IsKilled)
            {
                Player.Draw(Camera);
            }
            if (!LevelENV.CreateRetailVersion)
            {
                DebugTextObj.Position = new Vector2(Camera.X, Camera.Y - 300f);
                DebugTextObj.Draw(Camera);
            }
            m_itemDropManager.Draw(Camera);
            ImpactEffectPool.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,
                Camera.GetTransformation());
            m_textManager.Draw(Camera);
            if (CurrentRoom.LevelType == LevelType.Tower)
            {
                m_gardenParallaxFG.Draw(Camera);
            }
            m_whiteBG.Draw(Camera);
            Camera.End();
            if ((CurrentLevelType == LevelType.Dungeon || Game.PlayerStats.Traits.X == 35f ||
                 Game.PlayerStats.Traits.Y == 35f) &&
                (Game.PlayerStats.Class != 13 || (Game.PlayerStats.Class == 13 && !Player.LightOn)))
            {
                Camera.GraphicsDevice.Textures[1] = m_lightSourceRenderTarget;
                Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.ShadowEffect);
                if (LevelENV.SaveFrames)
                {
                    Camera.Draw(m_shadowRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero,
                        new Vector2(2f, 2f), SpriteEffects.None, 1f);
                }
                else
                {
                    Camera.Draw(m_shadowRenderTarget, Vector2.Zero, Color.White);
                }
                Camera.End();
            }
            if (CurrentRoom.Name != "Ending")
            {
                if ((Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f) &&
                    Game.PlayerStats.SpecialItem != 8)
                {
                    Game.GaussianBlur.InvertMask = true;
                    Game.GaussianBlur.Draw(RenderTarget, Camera, m_traitAuraRenderTarget);
                }
                else if ((Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f) &&
                         Game.PlayerStats.SpecialItem != 8)
                {
                    Game.GaussianBlur.InvertMask = false;
                    Game.GaussianBlur.Draw(RenderTarget, Camera, m_traitAuraRenderTarget);
                }
            }
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            m_projectileIconPool.Draw(Camera);
            m_playerHUD.Draw(Camera);
            if (m_lastEnemyHit != null && m_enemyHUDCounter > 0f)
            {
                m_enemyHUD.Draw(Camera);
            }
            if (m_enemyHUDCounter > 0f)
            {
                m_enemyHUDCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (CurrentRoom.Name != "Start" && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "ChallengeBoss" &&
                m_miniMapDisplay.Visible)
            {
                m_mapBG.Draw(Camera);
                m_miniMapDisplay.Draw(Camera);
            }
            if (CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Ending")
            {
                m_compassBG.Draw(Camera);
                m_compass.Draw(Camera);
            }
            m_objectivePlate.Draw(Camera);
            m_roomEnteringTitle.Draw(Camera);
            m_roomTitle.Draw(Camera);
            if (CurrentRoom.Name != "Ending" &&
                (!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f ||
                 Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
            {
                m_filmGrain.Draw(Camera);
            }
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            m_blackBorder1.Draw(Camera);
            m_blackBorder2.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
            Game.RippleEffect.Parameters["width"].SetValue(ShoutMagnitude);
            var vector = Player.Position - Camera.TopLeftCorner;
            if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            {
                Game.RippleEffect.Parameters["xcenter"].SetValue(vector.X/1320f);
                Game.RippleEffect.Parameters["ycenter"].SetValue(vector.Y/720f);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.RippleEffect);
            }
            else
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            }
            Camera.Draw(RenderTarget, Vector2.Zero, Color.White);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            if (CurrentRoom.Name != "Ending")
            {
                if ((Game.PlayerStats.Traits.X == 1f || Game.PlayerStats.Traits.Y == 1f) &&
                    Game.PlayerStats.SpecialItem != 8)
                {
                    Game.HSVEffect.Parameters["Saturation"].SetValue(0);
                    Game.HSVEffect.Parameters["Brightness"].SetValue(0);
                    Game.HSVEffect.Parameters["Contrast"].SetValue(0);
                    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                        Game.HSVEffect);
                }
                else if ((!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f ||
                          Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
                {
                    Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
                    Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
                    Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);
                    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null,
                        null, Game.HSVEffect);
                    Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
                    Camera.End();
                    Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
                    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null,
                        null, null);
                    var color = new Color(180, 150, 80);
                    Camera.Draw(RenderTarget, Vector2.Zero, color);
                    m_creditsText.Draw(Camera);
                    m_creditsTitleText.Draw(Camera);
                    Camera.End();
                    Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
                    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null,
                        null, null);
                }
                else
                {
                    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null,
                        null, null);
                }
            }
            else
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            }
            Camera.Draw(m_bgRenderTarget, Vector2.Zero, Color.White);
            Camera.End();
            base.Draw(gameTime);
        }

        public void RunWhiteSlashEffect()
        {
            m_whiteBG.Position = CurrentRoom.Position;
            m_whiteBG.Scale = Vector2.One;
            m_whiteBG.Scale = new Vector2(CurrentRoom.Width/m_whiteBG.Width, m_currentRoom.Height/m_whiteBG.Height);
            m_whiteBG.Opacity = 1f;
            Tween.To(m_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.RunFunction(0.2f, this, "RunWhiteSlash2");
        }

        public void RunWhiteSlash2()
        {
            m_whiteBG.Position = CurrentRoom.Position;
            m_whiteBG.Scale = Vector2.One;
            m_whiteBG.Scale = new Vector2(CurrentRoom.Width/m_whiteBG.Width, m_currentRoom.Height/m_whiteBG.Height);
            m_whiteBG.Opacity = 1f;
            Tween.To(m_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
        }

        public void LightningEffectTwice()
        {
            m_whiteBG.Position = CurrentRoom.Position;
            m_whiteBG.Scale = Vector2.One;
            m_whiteBG.Scale = new Vector2(CurrentRoom.Width/m_whiteBG.Width, m_currentRoom.Height/m_whiteBG.Height);
            m_whiteBG.Opacity = 1f;
            Tween.To(m_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.RunFunction(0.2f, this, "LightningEffectOnce");
        }

        public void LightningEffectOnce()
        {
            m_whiteBG.Position = CurrentRoom.Position;
            m_whiteBG.Scale = Vector2.One;
            m_whiteBG.Scale = new Vector2(CurrentRoom.Width/m_whiteBG.Width, m_currentRoom.Height/m_whiteBG.Height);
            m_whiteBG.Opacity = 1f;
            Tween.To(m_whiteBG, 1f, Tween.EaseNone, "Opacity", "0");
            SoundManager.PlaySound("LightningClap1", "LightningClap2");
        }

        public void SpawnDementiaEnemy()
        {
            var list = new List<EnemyObj>();
            foreach (var current in m_currentRoom.EnemyList)
            {
                if (current.Type != 17 && current.Type != 21 && current.Type != 27 && current.Type != 32 &&
                    current.Type != 6 && current.Type != 31)
                {
                    list.Add(current);
                }
            }
            if (list.Count > 0)
            {
                var enemyObj = list[CDGMath.RandomInt(0, list.Count - 1)];
                byte[] array;
                if (enemyObj.IsWeighted)
                {
                    array = LevelENV.DementiaGroundList;
                }
                else
                {
                    array = LevelENV.DementiaFlightList;
                }
                var enemyObj2 = EnemyBuilder.BuildEnemy(array[CDGMath.RandomInt(0, array.Length - 1)], null, null,
                    null, EnemyDifficulty.Basic, true);
                enemyObj2.Position = enemyObj.Position;
                enemyObj2.SaveToFile = false;
                enemyObj2.IsDemented = true;
                enemyObj2.NonKillable = true;
                enemyObj2.GivesLichHealth = false;
                AddEnemyToCurrentRoom(enemyObj2);
            }
        }

        public void AddEnemyToCurrentRoom(EnemyObj enemy)
        {
            m_currentRoom.TempEnemyList.Add(enemy);
            m_physicsManager.AddObject(enemy);
            m_tempEnemyStartPositions.Add(enemy.Position);
            enemy.SetPlayerTarget(Player);
            enemy.SetLevelScreen(this);
            enemy.Initialize();
        }

        public void RemoveEnemyFromCurrentRoom(EnemyObj enemy, Vector2 startingPos)
        {
            m_currentRoom.TempEnemyList.Remove(enemy);
            m_physicsManager.RemoveObject(enemy);
            m_tempEnemyStartPositions.Remove(startingPos);
        }

        public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room, Vector2 startingPos)
        {
            room.TempEnemyList.Remove(enemy);
            m_physicsManager.RemoveObject(enemy);
            m_tempEnemyStartPositions.Remove(startingPos);
        }

        public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room)
        {
            var num = room.TempEnemyList.IndexOf(enemy);
            if (num != -1)
            {
                room.TempEnemyList.RemoveAt(num);
                m_physicsManager.RemoveObject(enemy);
                m_tempEnemyStartPositions.RemoveAt(num);
            }
        }

        public void ResetEnemyPositions()
        {
            for (var i = 0; i < m_enemyStartPositions.Count; i++)
            {
                CurrentRoom.EnemyList[i].Position = m_enemyStartPositions[i];
            }
            for (var j = 0; j < m_tempEnemyStartPositions.Count; j++)
            {
                CurrentRoom.TempEnemyList[j].Position = m_tempEnemyStartPositions[j];
            }
        }

        public override void PauseScreen()
        {
            if (!IsPaused)
            {
                Tween.PauseAll();
                CurrentRoom.PauseRoom();
                ItemDropManager.PauseAllAnimations();
                ImpactEffectPool.PauseAllAnimations();
                if (!EnemiesPaused)
                {
                    m_projectileManager.PauseAllProjectiles(true);
                }
                SoundManager.PauseAllSounds("Pauseable");
                Player.PauseAnimation();
                base.PauseScreen();
            }
        }

        public override void UnpauseScreen()
        {
            if (IsPaused)
            {
                Tween.ResumeAll();
                CurrentRoom.UnpauseRoom();
                ItemDropManager.ResumeAllAnimations();
                ImpactEffectPool.ResumeAllAnimations();
                if (!EnemiesPaused)
                {
                    m_projectileManager.UnpauseAllProjectiles();
                }
                SoundManager.ResumeAllSounds("Pauseable");
                Player.ResumeAnimation();
                base.UnpauseScreen();
            }
        }

        public void RunGameOver()
        {
            Player.Opacity = 1f;
            m_killedEnemyObjList.Clear();
            var enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;
            var count = RoomList.Count;
            for (var i = 0; i < enemiesKilledInRun.Count; i++)
            {
                if (enemiesKilledInRun[i].X != -1f && enemiesKilledInRun[i].Y != -1f &&
                    (int) enemiesKilledInRun[i].X < count)
                {
                    var roomObj = RoomList[(int) enemiesKilledInRun[i].X];
                    var count2 = roomObj.EnemyList.Count;
                    if ((int) enemiesKilledInRun[i].Y < count2)
                    {
                        var item =
                            RoomList[(int) enemiesKilledInRun[i].X].EnemyList[(int) enemiesKilledInRun[i].Y];
                        m_killedEnemyObjList.Add(item);
                    }
                }
            }
            var list = new List<object>();
            list.Add(Player);
            list.Add(m_killedEnemyObjList);
            list.Add(m_coinsCollected);
            list.Add(m_bagsCollected);
            list.Add(m_diamondsCollected);
            list.Add(m_objKilledPlayer);
            Tween.RunFunction(0f, ScreenManager, "DisplayScreen", 7, true, list);
        }

        public void RunCinematicBorders(float duration)
        {
            StopCinematicBorders();
            m_blackBorder1.Opacity = 1f;
            m_blackBorder2.Opacity = 1f;
            m_blackBorder1.Y = 0f;
            m_blackBorder2.Y = 720 - m_borderSize;
            var num = 1f;
            Tween.By(m_blackBorder1, num, Quad.EaseInOut, "delay", (duration - num).ToString(), "Y",
                (-m_borderSize).ToString());
            Tween.By(m_blackBorder2, num, Quad.EaseInOut, "delay", (duration - num).ToString(), "Y",
                m_borderSize.ToString());
            Tween.To(m_blackBorder1, num, Linear.EaseNone, "delay", (duration - num + 0.2f).ToString(), "Opacity", "0");
            Tween.To(m_blackBorder2, num, Linear.EaseNone, "delay", (duration - num + 0.2f).ToString(), "Opacity", "0");
        }

        public void StopCinematicBorders()
        {
            Tween.StopAllContaining(m_blackBorder1, false);
            Tween.StopAllContaining(m_blackBorder2, false);
        }

        public void DisplayMap(bool isTeleporterScreen)
        {
            (ScreenManager as RCScreenManager).AddRoomsToMap(m_miniMapDisplay.AddedRoomsList);
            if (isTeleporterScreen)
            {
                (ScreenManager as RCScreenManager).ActivateMapScreenTeleporter();
            }
            (ScreenManager as RCScreenManager).DisplayScreen(14, true);
        }

        public void PauseAllEnemies()
        {
            EnemiesPaused = true;
            CurrentRoom.PauseRoom();
            foreach (var current in CurrentRoom.EnemyList)
            {
                current.PauseEnemy();
            }
            foreach (var current2 in CurrentRoom.TempEnemyList)
            {
                current2.PauseEnemy();
            }
            m_projectileManager.PauseAllProjectiles(false);
        }

        public void CastTimeStop(float duration)
        {
            SoundManager.PlaySound("Cast_TimeStart");
            SoundManager.PauseMusic();
            m_enemyPauseDuration = duration;
            PauseAllEnemies();
            Tween.To(m_traitAura, 0.2f, Tween.EaseNone, "ScaleX", "100", "ScaleY", "100");
        }

        public void StopTimeStop()
        {
            SoundManager.PlaySound("Cast_TimeStop");
            SoundManager.ResumeMusic();
            Tween.To(m_traitAura, 0.2f, Tween.EaseNone, "ScaleX", "0", "ScaleY", "0");
            Tween.AddEndHandlerToLastTween(this, "UnpauseAllEnemies");
        }

        public void UnpauseAllEnemies()
        {
            Game.HSVEffect.Parameters["UseMask"].SetValue(false);
            EnemiesPaused = false;
            CurrentRoom.UnpauseRoom();
            foreach (var current in CurrentRoom.EnemyList)
            {
                current.UnpauseEnemy();
            }
            foreach (var current2 in CurrentRoom.TempEnemyList)
            {
                current2.UnpauseEnemy();
            }
            m_projectileManager.UnpauseAllProjectiles();
        }

        public void DamageAllEnemies(int damage)
        {
            var list = new List<EnemyObj>();
            list.AddRange(CurrentRoom.TempEnemyList);
            foreach (var current in list)
            {
                if (!current.IsDemented && !current.IsKilled)
                {
                    current.HitEnemy(damage, current.Position, true);
                }
            }
            list.Clear();
            list = null;
            foreach (var current2 in CurrentRoom.EnemyList)
            {
                if (!current2.IsDemented && !current2.IsKilled)
                {
                    current2.HitEnemy(damage, current2.Position, true);
                }
            }
        }

        public virtual void Reset()
        {
            BackBufferOpacity = 0f;
            m_killedEnemyObjList.Clear();
            m_diamondsCollected = 0;
            m_coinsCollected = 0;
            m_bagsCollected = 0;
            m_blueprintsCollected = 0;
            if (Player != null)
            {
                Player.Reset();
                Player.ResetLevels();
                Player.Position = new Vector2(200f, 200f);
            }
            ResetEnemyPositions();
            foreach (var current in RoomList)
            {
                current.Reset();
            }
            InitializeChests(false);
            foreach (var current2 in RoomList)
            {
                foreach (var current3 in current2.GameObjList)
                {
                    var breakableObj = current3 as BreakableObj;
                    if (breakableObj != null)
                    {
                        breakableObj.Reset();
                    }
                }
            }
            m_projectileManager.DestroyAllProjectiles(true);
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }

        public override void DisposeRTs()
        {
            m_fgRenderTarget.Dispose();
            m_fgRenderTarget = null;
            m_bgRenderTarget.Dispose();
            m_bgRenderTarget = null;
            m_skyRenderTarget.Dispose();
            m_skyRenderTarget = null;
            RenderTarget.Dispose();
            RenderTarget = null;
            m_shadowRenderTarget.Dispose();
            m_shadowRenderTarget = null;
            m_lightSourceRenderTarget.Dispose();
            m_lightSourceRenderTarget = null;
            m_traitAuraRenderTarget.Dispose();
            m_traitAuraRenderTarget = null;
            m_foregroundSprite.Dispose();
            m_foregroundSprite = null;
            m_backgroundSprite.Dispose();
            m_backgroundSprite = null;
            m_backgroundParallaxSprite.Dispose();
            m_backgroundParallaxSprite = null;
            m_gardenParallaxFG.Dispose();
            m_gardenParallaxFG = null;
            m_roomBWRenderTarget.Dispose();
            m_roomBWRenderTarget = null;
            m_miniMapDisplay.DisposeRTs();
            base.DisposeRTs();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Procedural Level Screen");
                Tween.StopAll(false);
                m_currentRoom = null;
                DisposeRTs();
                foreach (var current in RoomList)
                {
                    current.Dispose();
                }
                RoomList.Clear();
                RoomList = null;
                m_enemyStartPositions.Clear();
                m_enemyStartPositions = null;
                m_tempEnemyStartPositions.Clear();
                m_tempEnemyStartPositions = null;
                m_textManager.Dispose();
                m_textManager = null;
                m_physicsManager = null;
                m_projectileManager.Dispose();
                m_projectileManager = null;
                m_itemDropManager.Dispose();
                m_itemDropManager = null;
                m_currentRoom = null;
                m_miniMapDisplay.Dispose();
                m_miniMapDisplay = null;
                m_mapBG.Dispose();
                m_mapBG = null;
                m_inputMap.Dispose();
                m_inputMap = null;
                m_lastEnemyHit = null;
                m_playerHUD.Dispose();
                m_playerHUD = null;
                Player = null;
                m_enemyHUD.Dispose();
                m_enemyHUD = null;
                ImpactEffectPool.Dispose();
                ImpactEffectPool = null;
                m_blackBorder1.Dispose();
                m_blackBorder1 = null;
                m_blackBorder2.Dispose();
                m_blackBorder2 = null;
                ChestList.Clear();
                ChestList = null;
                m_projectileIconPool.Dispose();
                m_projectileIconPool = null;
                m_objKilledPlayer = null;
                m_dungeonLight.Dispose();
                m_dungeonLight = null;
                m_traitAura.Dispose();
                m_traitAura = null;
                m_killedEnemyObjList.Clear();
                m_killedEnemyObjList = null;
                m_roomEnteringTitle.Dispose();
                m_roomEnteringTitle = null;
                m_roomTitle.Dispose();
                m_roomTitle = null;
                m_creditsText.Dispose();
                m_creditsText = null;
                m_creditsTitleText.Dispose();
                m_creditsTitleText = null;
                Array.Clear(m_creditsTextTitleList, 0, m_creditsTextTitleList.Length);
                Array.Clear(m_creditsTextList, 0, m_creditsTextList.Length);
                m_creditsTextTitleList = null;
                m_creditsTextList = null;
                m_filmGrain.Dispose();
                m_filmGrain = null;
                m_objectivePlate.Dispose();
                m_objectivePlate = null;
                m_objectivePlateTween = null;
                m_sky.Dispose();
                m_sky = null;
                m_whiteBG.Dispose();
                m_whiteBG = null;
                m_compassBG.Dispose();
                m_compassBG = null;
                m_compass.Dispose();
                m_compass = null;
                if (m_compassDoor != null)
                {
                    m_compassDoor.Dispose();
                }
                m_compassDoor = null;
                m_castleBorderTexture.Dispose();
                m_gardenBorderTexture.Dispose();
                m_towerBorderTexture.Dispose();
                m_dungeonBorderTexture.Dispose();
                m_neoBorderTexture.Dispose();
                m_castleBorderTexture = null;
                m_gardenBorderTexture = null;
                m_towerBorderTexture = null;
                m_dungeonBorderTexture = null;
                DebugTextObj.Dispose();
                DebugTextObj = null;
                base.Dispose();
            }
        }

        public void SetLastEnemyHit(EnemyObj enemy)
        {
            m_lastEnemyHit = enemy;
            m_enemyHUDCounter = m_enemyHUDDuration;
            m_enemyHUD.UpdateEnemyInfo(m_lastEnemyHit.Name, m_lastEnemyHit.Level,
                m_lastEnemyHit.CurrentHealth/(float) m_lastEnemyHit.MaxHealth);
        }

        public void KillEnemy(EnemyObj enemy)
        {
            if (enemy.SaveToFile)
            {
                var item = new Vector2(RoomList.IndexOf(CurrentRoom), CurrentRoom.EnemyList.IndexOf(enemy));
                if (item.X < 0f || item.Y < 0f)
                {
                    throw new Exception(
                        "Could not find killed enemy in either CurrentRoom or CurrentRoom.EnemyList. This may be because the enemy was a blob");
                }
                Game.PlayerStats.EnemiesKilledInRun.Add(item);
            }
        }

        public void ItemDropCollected(int itemDropType)
        {
            if (itemDropType == 1)
            {
                m_coinsCollected++;
                return;
            }
            switch (itemDropType)
            {
                case 10:
                    m_bagsCollected++;
                    return;
                case 11:
                    m_diamondsCollected++;
                    return;
                case 12:
                case 13:
                    m_blueprintsCollected++;
                    return;
                default:
                    return;
            }
        }

        public void RefreshMapChestIcons()
        {
            m_miniMapDisplay.RefreshChestIcons(CurrentRoom);
            (ScreenManager as RCScreenManager).RefreshMapScreenChestIcons(CurrentRoom);
        }

        public void DisplayObjective(string objectiveTitle, string objectiveDescription, string objectiveProgress,
            bool tween)
        {
            (m_objectivePlate.GetChildAt(4) as SpriteObj).ScaleX = 0f;
            (m_objectivePlate.GetChildAt(5) as SpriteObj).ScaleX = 0f;
            m_objectivePlate.GetChildAt(2).Opacity = 1f;
            m_objectivePlate.GetChildAt(3).Opacity = 1f;
            m_objectivePlate.X = 1470f;
            if (m_objectivePlateTween != null && m_objectivePlateTween.TweenedObject == m_objectivePlate &&
                m_objectivePlateTween.Active)
            {
                m_objectivePlateTween.StopTween(false);
            }
            (m_objectivePlate.GetChildAt(1) as TextObj).Text = objectiveTitle;
            (m_objectivePlate.GetChildAt(2) as TextObj).Text = objectiveDescription;
            (m_objectivePlate.GetChildAt(3) as TextObj).Text = objectiveProgress;
            if (tween)
            {
                m_objectivePlateTween = Tween.By(m_objectivePlate, 0.5f, Back.EaseOut, "X", "-300");
                return;
            }
            m_objectivePlate.X -= 300f;
        }

        public void ResetObjectivePlate(bool tween)
        {
            if (m_objectivePlate != null)
            {
                m_objectivePlate.X = 1170f;
                if (m_objectivePlateTween != null && m_objectivePlateTween.TweenedObject == m_objectivePlate &&
                    m_objectivePlateTween.Active)
                {
                    m_objectivePlateTween.StopTween(false);
                }
                if (tween)
                {
                    Tween.By(m_objectivePlate, 0.5f, Back.EaseIn, "X", "300");
                    return;
                }
                m_objectivePlate.X += 300f;
            }
        }

        public void UpdateObjectiveProgress(string progress)
        {
            (m_objectivePlate.GetChildAt(3) as TextObj).Text = progress;
        }

        public void ObjectiveFailed()
        {
            (m_objectivePlate.GetChildAt(1) as TextObj).Text = "Objective Failed";
            m_objectivePlate.GetChildAt(2).Opacity = 0.3f;
            m_objectivePlate.GetChildAt(3).Opacity = 0.3f;
        }

        public void ObjectiveComplete()
        {
            m_objectivePlate.GetChildAt(2).Opacity = 0.3f;
            m_objectivePlate.GetChildAt(3).Opacity = 0.3f;
            m_objectivePlate.X = 1170f;
            if (m_objectivePlateTween != null && m_objectivePlateTween.TweenedObject == m_objectivePlate &&
                m_objectivePlateTween.Active)
            {
                m_objectivePlateTween.StopTween(false);
            }
            (m_objectivePlate.GetChildAt(1) as TextObj).Text = "Objective Complete!";
        }

        public override void OnEnter()
        {
            (ScreenManager.Game as Game).SaveManager.ResetAutosave();
            Player.DisableAllWeight = false;
            Player.StopAllSpells();
            ShoutMagnitude = 3f;
            if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
            {
                Player.Scale = new Vector2(3f, 3f);
            }
            else if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
            {
                Player.Scale = new Vector2(1.35f, 1.35f);
            }
            else
            {
                Player.Scale = new Vector2(2f, 2f);
            }
            if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
            {
                Player.ScaleX *= 0.825f;
                Player.ScaleY *= 1.15f;
            }
            else if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
            {
                Player.ScaleX *= 1.25f;
                Player.ScaleY *= 1.175f;
            }
            Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            Player.CurrentMana = Game.PlayerStats.CurrentMana;
            if (LevelENV.RunTestRoom)
            {
                Game.ScreenManager.Player.CurrentHealth = Game.ScreenManager.Player.MaxHealth;
                Game.ScreenManager.Player.CurrentMana = Game.ScreenManager.Player.MaxMana;
            }
            Player.UpdateInternalScale();
            CheckForRoomTransition();
            UpdateCamera();
            UpdatePlayerHUDAbilities();
            Player.UpdateEquipmentColours();
            Player.StopAllSpells();
            if (Game.PlayerStats.Class == 13)
            {
                m_miniMapDisplay.AddAllIcons(RoomList);
                (ScreenManager as RCScreenManager).AddIconsToMap(RoomList);
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
            var flag = false;
            var flag2 = false;
            var flag3 = false;
            var flag4 = false;
            var flag5 = false;
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
            if (m_currentRoom != null)
            {
                m_currentRoom.OnExit();
            }
            SoundManager.StopAllSounds("Default");
            SoundManager.StopAllSounds("Pauseable");
            base.OnExit();
        }

        public void RevealMorning()
        {
            m_sky.MorningOpacity = 0f;
            Tween.To(m_sky, 2f, Tween.EaseNone, "MorningOpacity", "1");
        }

        public void ZoomOutAllObjects()
        {
            var vector = new Vector2(CurrentRoom.Bounds.Center.X, CurrentRoom.Bounds.Center.Y);
            var list = new List<Vector2>();
            var num = 0f;
            foreach (var current in CurrentRoom.GameObjList)
            {
                int num2;
                if (current.Y < vector.Y)
                {
                    num2 = CurrentRoom.Bounds.Top - (current.Bounds.Top + current.Bounds.Height);
                }
                else
                {
                    num2 = CurrentRoom.Bounds.Bottom - current.Bounds.Top;
                }
                int num3;
                if (current.X < vector.X)
                {
                    num3 = CurrentRoom.Bounds.Left - (current.Bounds.Left + current.Bounds.Width);
                }
                else
                {
                    num3 = CurrentRoom.Bounds.Right - current.Bounds.Left;
                }
                if (Math.Abs(num3) > Math.Abs(num2))
                {
                    list.Add(new Vector2(0f, num2));
                    Tween.By(current, 0.5f, Back.EaseIn, "delay", num.ToString(), "Y", num2.ToString());
                }
                else
                {
                    list.Add(new Vector2(num3, 0f));
                    Tween.By(current, 0.5f, Back.EaseIn, "delay", num.ToString(), "X", num3.ToString());
                }
                num += 0.05f;
            }
            Tween.RunFunction(num + 0.5f, this, "ZoomInAllObjects", list);
        }

        public void ZoomInAllObjects(List<Vector2> objPositions)
        {
            var num = 0;
            var num2 = 1f;
            foreach (var current in CurrentRoom.GameObjList)
            {
                Tween.By(current, 0.5f, Back.EaseOut, "delay", num2.ToString(), "X", (-objPositions[num].X).ToString(),
                    "Y", (-objPositions[num].Y).ToString());
                num++;
                num2 += 0.05f;
            }
        }

        public void UpdateLevel(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Castle:
                    m_backgroundSprite.Scale = Vector2.One;
                    m_foregroundSprite.Scale = Vector2.One;
                    m_backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                    m_foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                    m_backgroundSprite.Scale = new Vector2(2f, 2f);
                    m_foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;
                case LevelType.Garden:
                    m_backgroundSprite.Scale = Vector2.One;
                    m_foregroundSprite.Scale = Vector2.One;
                    m_backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                    m_foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                    m_backgroundSprite.Scale = new Vector2(2f, 2f);
                    m_foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;
                case LevelType.Dungeon:
                    m_backgroundSprite.Scale = Vector2.One;
                    m_foregroundSprite.Scale = Vector2.One;
                    m_backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                    m_foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                    m_backgroundSprite.Scale = new Vector2(2f, 2f);
                    m_foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;
                case LevelType.Tower:
                    m_backgroundSprite.Scale = Vector2.One;
                    m_foregroundSprite.Scale = Vector2.One;
                    m_backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                    m_foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                    m_backgroundSprite.Scale = new Vector2(2f, 2f);
                    m_foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;
            }
            if (levelType == LevelType.Dungeon)
            {
                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
                return;
            }
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }

        public void RefreshPlayerHUDPos()
        {
            m_playerHUD.SetPosition(new Vector2(20f, 40f));
        }

        public void UpdatePlayerHUD()
        {
            m_playerHUD.Update(Player);
        }

        public void UpdatePlayerHUDAbilities()
        {
            m_playerHUD.UpdateAbilityIcons();
        }

        public void UpdatePlayerHUDSpecialItem()
        {
            m_playerHUD.UpdateSpecialItemIcon();
        }

        public void UpdatePlayerSpellIcon()
        {
            m_playerHUD.UpdateSpellIcon();
        }

        public void SetMapDisplayVisibility(bool visible)
        {
            m_miniMapDisplay.Visible = visible;
        }

        public void SetPlayerHUDVisibility(bool visible)
        {
            m_playerHUD.Visible = visible;
        }

        public void SetObjectKilledPlayer(GameObj obj)
        {
            m_objKilledPlayer = obj;
        }
    }
}
