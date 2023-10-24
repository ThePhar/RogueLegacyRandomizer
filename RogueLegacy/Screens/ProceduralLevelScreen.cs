//  RogueLegacyRandomizer - ProceduralLevelScreen.cs
//  Last Modified 2023-10-24 5:32 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Randomizer;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using RogueLegacy.GameObjects;
using RogueLegacy.GameObjects.HUD;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class ProceduralLevelScreen : Screen
    {
        private const byte INPUT_TOGGLE_MAP       = 0;
        private const byte INPUT_TOGGLE_ZOOM      = 1;
        private const byte INPUT_LEFT_CONTROL     = 2;
        private const byte INPUT_LEFT             = 3;
        private const byte INPUT_RIGHT            = 4;
        private const byte INPUT_UP               = 5;
        private const byte INPUT_DOWN             = 6;
        private const byte INPUT_DISPLAY_ROO_INFO = 7;

        private readonly float              _enemyHUDDuration = 2f;
        private          BackgroundObj      _backgroundParallaxSprite;
        private          BackgroundObj      _backgroundSprite;
        private          int                _bagsCollected;
        private          RenderTarget2D     _bgRenderTarget;
        private          SpriteObj          _blackBorder1;
        private          SpriteObj          _blackBorder2;
        private          int                _blueprintsCollected;
        private          int                _borderSize;
        protected        int                _bottomMostBorder = -2147483647;
        private          Texture2D          _castleBorderTexture;
        private          int                _coinsCollected;
        private          SpriteObj          _compass;
        private          SpriteObj          _compassBG;
        private          bool               _compassDisplayed;
        private          DoorObj            _compassDoor;
        private          int                _creditsIndex;
        private          TextObj            _creditsText;
        private          string[]           _creditsTextList;
        private          string[]           _creditsTextTitleList;
        private          TextObj            _creditsTitleText;
        protected        RoomObj            _currentRoom;
        private          int                _diamondsCollected;
        private          Texture2D          _dungeonBorderTexture;
        private          SpriteObj          _dungeonLight;
        private          EnemyHUDObj        _enemyHUD;
        private          float              _enemyHUDCounter;
        private          float              _enemyPauseDuration;
        private          List<Vector2>      _enemyStartPositions;
        private          RenderTarget2D     _fgRenderTarget;
        private          SpriteObj          _filmGrain;
        private          BackgroundObj      _foregroundSprite;
        private          Texture2D          _gardenBorderTexture;
        private          BackgroundObj      _gardenParallaxFG;
        private          InputMap           _inputMap;
        protected        ItemDropManager    _itemDropManager;
        private          List<EnemyObj>     _killedEnemyObjList;
        private          EnemyObj           _lastEnemyHit;
        protected        int                _leftMostBorder = 2147483647;
        private          RenderTarget2D     _lightSourceRenderTarget;
        private          SpriteObj          _mapBG;
        public           MapObj             MiniMapDisplay;
        private          Texture2D          _neoBorderTexture;
        private          ObjContainer       _objectivePlate;
        private          TweenObject        _objectivePlateTween;
        private          GameObj            _objKilledPlayer;
        protected        PhysicsManager     _physicsManager;
        private          PlayerHUD          _playerHUD;
        private          ProjectileIconPool _projectileIconPool;
        protected        ProjectileManager  _projectileManager;
        protected        int                _rightMostBorder = -2147483647;
        private          RenderTarget2D     _roomBWRenderTarget;
        private          TextObj            _roomEnteringTitle;
        private          TextObj            _roomTitle;
        private          RenderTarget2D     _shadowRenderTarget;
        private          RenderTarget2D     _skyRenderTarget;
        private          List<Vector2>      _tempEnemyStartPositions;
        protected        TextManager        _textManager;
        protected        int                _topMostBorder = 2147483647;
        private          Texture2D          _towerBorderTexture;
        private          SpriteObj          _traitAura;
        private          RenderTarget2D     _traitAuraRenderTarget;
        private          SpriteObj          _whiteBG;

        protected int     BottomDoorPercent = 80;
        public    TextObj DebugTextObj;
        protected int     LeftDoorPercent = 80;
        public    bool    LoadGameData;
        protected int     RightDoorPercent = 80;
        public    SkyObj  Sky;
        protected int     TopDoorPercent = 80;

        public ProceduralLevelScreen()
        {
            DisableRoomTransitioning = false;
            RoomList = new List<RoomObj>();
            _textManager = new TextManager(700);
            _projectileManager = new ProjectileManager(this, 700);
            _enemyStartPositions = new List<Vector2>();
            _tempEnemyStartPositions = new List<Vector2>();
            ImpactEffectPool = new ImpactEffectPool(2000);
            CameraLockedToPlayer = true;
            _roomTitle = new TextObj();
            _roomTitle.Font = Game.JunicodeLargeFont;
            _roomTitle.Align = Types.TextAlign.Right;
            _roomTitle.Opacity = 0f;
            _roomTitle.FontSize = 40f;
            _roomTitle.Position = new Vector2(1270f, 570f);
            _roomTitle.OutlineWidth = 2;
            _roomEnteringTitle = _roomTitle.Clone() as TextObj;
            _roomEnteringTitle.Text = "Now Entering";
            _roomEnteringTitle.FontSize = 24f;
            _roomEnteringTitle.Y -= 50f;
            _inputMap = new InputMap(PlayerIndex.One, false);
            _inputMap.AddInput(0, Keys.Y);
            _inputMap.AddInput(1, Keys.U);
            _inputMap.AddInput(2, Keys.LeftControl);
            _inputMap.AddInput(3, Keys.Left);
            _inputMap.AddInput(4, Keys.Right);
            _inputMap.AddInput(5, Keys.Up);
            _inputMap.AddInput(6, Keys.Down);
            _inputMap.AddInput(7, Keys.OemTilde);
            ChestList = new List<ChestObj>();
            MiniMapDisplay = new MapObj(true, this);
            _killedEnemyObjList = new List<EnemyObj>();
        }

        public float BackBufferOpacity        { get; set; }
        public bool  CameraLockedToPlayer     { get; set; }
        public float ShoutMagnitude           { get; set; }
        public bool  DisableRoomOnEnter       { get; set; }
        public bool  DisableSongUpdating      { get; set; }
        public bool  DisableRoomTransitioning { get; set; }
        public bool  JukeboxEnabled           { get; set; }

        public List<RoomObj> MapRoomsUnveiled
        {
            get => MiniMapDisplay.AddedRoomsList;
            set
            {
                MiniMapDisplay.ClearRoomsAdded();
                MiniMapDisplay.AddAllRooms(value);
            }
        }

        public List<RoomObj> MapRoomsAdded => MiniMapDisplay.AddedRoomsList;

        public PlayerObj     Player   { get; set; }
        public List<RoomObj> RoomList { get; private set; }

        public PhysicsManager PhysicsManager => _physicsManager;

        public RoomObj CurrentRoom => _currentRoom;

        public ProjectileManager ProjectileManager => _projectileManager;

        public List<EnemyObj> EnemyList => CurrentRoom.EnemyList;

        public List<ChestObj> ChestList { get; private set; }

        public TextManager TextManager => _textManager;

        public ImpactEffectPool ImpactEffectPool { get; private set; }

        public ItemDropManager ItemDropManager => _itemDropManager;

        public Zone CurrentZone => _currentRoom?.Zone ?? Zone.None;

        public int LeftBorder => _leftMostBorder;

        public int RightBorder => _rightMostBorder;

        public int TopBorder => _topMostBorder;

        public int BottomBorder => _bottomMostBorder;

        public RenderTarget2D RenderTarget  { get; private set; }
        public bool           EnemiesPaused { get; private set; }

        public override void LoadContent()
        {
            DebugTextObj = new TextObj(Game.JunicodeFont);
            DebugTextObj.FontSize = 26f;
            DebugTextObj.Align = Types.TextAlign.Centre;
            DebugTextObj.Text = "";
            DebugTextObj.ForceDraw = true;
            _projectileIconPool = new ProjectileIconPool(200, _projectileManager, ScreenManager as RCScreenManager);
            _projectileIconPool.Initialize();
            _textManager.Initialize();
            ImpactEffectPool.Initialize();
            _physicsManager = (ScreenManager.Game as Game).PhysicsManager;
            _physicsManager.SetGravity(0f, 1830f);
            _projectileManager.Initialize();
            _physicsManager.Initialize(ScreenManager.Camera);
            _itemDropManager = new ItemDropManager(600, _physicsManager);
            _itemDropManager.Initialize();
            _playerHUD = new PlayerHUD();
            _playerHUD.SetPosition(new Vector2(20f, 40f));
            _enemyHUD = new EnemyHUDObj();
            _enemyHUD.Position = new Vector2(660 - _enemyHUD.Width / 2, 20f);
            MiniMapDisplay.SetPlayer(Player);
            MiniMapDisplay.InitializeAlphaMap(new Rectangle(1070, 50, 200, 100), Camera);
            InitializeAllRooms(true);
            InitializeEnemies();
            InitializeChests(true);
            InitializeRenderTargets();
            _mapBG = new SpriteObj("MinimapBG_Sprite");
            _mapBG.Position = new Vector2(1070f, 50f);
            _mapBG.ForceDraw = true;
            UpdateCamera();
            _borderSize = 100;
            _blackBorder1 = new SpriteObj("Blank_Sprite");
            _blackBorder1.TextureColor = Color.Black;
            _blackBorder1.Scale = new Vector2(1340f / _blackBorder1.Width, _borderSize / _blackBorder1.Height);
            _blackBorder2 = new SpriteObj("Blank_Sprite");
            _blackBorder2.TextureColor = Color.Black;
            _blackBorder2.Scale = new Vector2(1340f / _blackBorder2.Width, _borderSize / _blackBorder2.Height);
            _blackBorder1.ForceDraw = true;
            _blackBorder2.ForceDraw = true;
            _blackBorder1.Y = -(float) _borderSize;
            _blackBorder2.Y = 720f;
            _dungeonLight = new SpriteObj("LightSource_Sprite");
            _dungeonLight.ForceDraw = true;
            _dungeonLight.Scale = new Vector2(12f, 12f);
            _traitAura = new SpriteObj("LightSource_Sprite");
            _traitAura.ForceDraw = true;
            _objectivePlate = new ObjContainer("DialogBox_Character");
            _objectivePlate.ForceDraw = true;
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Position = new Vector2(-400f, -60f);
            textObj.OverrideParentScale = true;
            textObj.FontSize = 10f;
            textObj.Text = "Fairy Chest Objective:";
            textObj.TextureColor = Color.Red;
            textObj.OutlineWidth = 2;
            _objectivePlate.AddChild(textObj);
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.OverrideParentScale = true;
            textObj2.Position = new Vector2(textObj.X, textObj.Y + 40f);
            textObj2.ForceDraw = true;
            textObj2.FontSize = 9f;
            textObj2.Text = "Reach the chest in 15 seconds:";
            textObj2.WordWrap(250);
            textObj2.OutlineWidth = 2;
            _objectivePlate.AddChild(textObj2);
            var textObj3 = new TextObj(Game.JunicodeFont);
            textObj3.OverrideParentScale = true;
            textObj3.Position = new Vector2(textObj2.X, textObj2.Y + 35f);
            textObj3.ForceDraw = true;
            textObj3.FontSize = 9f;
            textObj3.Text = "Time Remaining:";
            textObj3.WordWrap(250);
            textObj3.OutlineWidth = 2;
            _objectivePlate.AddChild(textObj3);
            _objectivePlate.Scale = new Vector2(250f / _objectivePlate.GetChildAt(0).Width,
                130f / _objectivePlate.GetChildAt(0).Height);
            _objectivePlate.Position = new Vector2(1470f, 250f);
            var spriteObj = new SpriteObj("Blank_Sprite");
            spriteObj.TextureColor = Color.Red;
            spriteObj.Position = new Vector2(textObj2.X, textObj2.Y + 20f);
            spriteObj.ForceDraw = true;
            spriteObj.OverrideParentScale = true;
            spriteObj.ScaleY = 0.5f;
            _objectivePlate.AddChild(spriteObj);
            var spriteObj2 = new SpriteObj("Blank_Sprite");
            spriteObj2.TextureColor = Color.Red;
            spriteObj2.Position = new Vector2(textObj2.X, spriteObj.Y + 35f);
            spriteObj2.ForceDraw = true;
            spriteObj2.OverrideParentScale = true;
            spriteObj2.ScaleY = 0.5f;
            _objectivePlate.AddChild(spriteObj2);
            base.LoadContent();
            Sky = new SkyObj(this);
            Sky.LoadContent(Camera);
            _whiteBG = new SpriteObj("Blank_Sprite");
            _whiteBG.Opacity = 0f;
            _whiteBG.Scale = new Vector2(1320f / _whiteBG.Width, 720f / _whiteBG.Height);
            _filmGrain = new SpriteObj("FilmGrain_Sprite");
            _filmGrain.ForceDraw = true;
            _filmGrain.Scale = new Vector2(2.015f, 2.05f);
            _filmGrain.X -= 5f;
            _filmGrain.Y -= 5f;
            _filmGrain.PlayAnimation();
            _filmGrain.AnimationDelay = 0.0333333351f;
            _compassBG = new SpriteObj("CompassBG_Sprite");
            _compassBG.ForceDraw = true;
            _compassBG.Position = new Vector2(660f, 90f);
            _compassBG.Scale = Vector2.Zero;
            _compass = new SpriteObj("Compass_Sprite");
            _compass.Position = _compassBG.Position;
            _compass.ForceDraw = true;
            _compass.Scale = Vector2.Zero;
            InitializeCreditsText();
        }

        private void InitializeCreditsText()
        {
            _creditsTextTitleList = new[]
            {
                "Developed by",
                "Design",
                "Programming",
                "Art",
                "Sound Design & Music",
                "Music",
                ""
            };
            _creditsTextList = new[]
            {
                "Cellar Door Games",
                "Teddy Lee",
                "Kenny Lee",
                "Glauber Kotaki",
                "Gordon McGladdery",
                "Judson Cowan",
                "Rogue Legacy"
            };
            _creditsText = new TextObj(Game.JunicodeFont);
            _creditsText.FontSize = 20f;
            _creditsText.Text = "Cellar Door Games";
            _creditsText.DropShadow = new Vector2(2f, 2f);
            _creditsText.Opacity = 0f;
            _creditsTitleText = _creditsText.Clone() as TextObj;
            _creditsTitleText.FontSize = 14f;
            _creditsTitleText.Position = new Vector2(50f, 580f);
            _creditsText.Position = _creditsTitleText.Position;
            _creditsText.Y += 35f;
            _creditsTitleText.X += 5f;
        }

        public void DisplayCreditsText(bool resetIndex)
        {
            if (resetIndex)
            {
                _creditsIndex = 0;
            }

            _creditsTitleText.Opacity = 0f;
            _creditsText.Opacity = 0f;
            if (_creditsIndex < _creditsTextList.Length)
            {
                _creditsTitleText.Opacity = 0f;
                _creditsText.Opacity = 0f;
                _creditsTitleText.Text = _creditsTextTitleList[_creditsIndex];
                _creditsText.Text = _creditsTextList[_creditsIndex];
                Tween.To(_creditsTitleText, 0.5f, Tween.EaseNone, "Opacity", "1");
                Tween.To(_creditsText, 0.5f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
                _creditsTitleText.Opacity = 1f;
                _creditsText.Opacity = 1f;
                Tween.To(_creditsTitleText, 0.5f, Tween.EaseNone, "delay", "4", "Opacity", "0");
                Tween.To(_creditsText, 0.5f, Tween.EaseNone, "delay", "4.2", "Opacity", "0");
                _creditsTitleText.Opacity = 0f;
                _creditsText.Opacity = 0f;
                _creditsIndex++;
                Tween.RunFunction(8f, this, "DisplayCreditsText", false);
            }
        }

        public void StopCreditsText()
        {
            _creditsIndex = 0;
            Tween.StopAllContaining(_creditsTitleText, false);
            Tween.StopAllContaining(_creditsText, false);
            Tween.StopAllContaining(this, false);
            _creditsTitleText.Opacity = 0f;
        }

        public override void ReinitializeRTs()
        {
            Sky.ReinitializeRT(Camera);
            MiniMapDisplay.InitializeAlphaMap(new Rectangle(1070, 50, 200, 100), Camera);
            InitializeRenderTargets();
            InitializeAllRooms(false);
            if (CurrentRoom == null || CurrentRoom.Name != "Start")
            {
                if (CurrentRoom.Name == "ChallengeBoss")
                {
                    _backgroundSprite.Scale = Vector2.One;
                    _backgroundSprite.ChangeSprite("NeoBG_Sprite", ScreenManager.Camera);
                    _backgroundSprite.Scale = new Vector2(2f, 2f);
                    _foregroundSprite.Scale = Vector2.One;
                    _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                    _foregroundSprite.Scale = new Vector2(2f, 2f);
                }
                else
                {
                    switch (CurrentRoom.Zone)
                    {
                        case Zone.Castle:
                            _backgroundSprite.Scale = Vector2.One;
                            _foregroundSprite.Scale = Vector2.One;
                            _backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                            _foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                            _backgroundSprite.Scale = new Vector2(2f, 2f);
                            _foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;

                        case Zone.Garden:
                            _backgroundSprite.Scale = Vector2.One;
                            _foregroundSprite.Scale = Vector2.One;
                            _backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                            _foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                            _backgroundSprite.Scale = new Vector2(2f, 2f);
                            _foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;

                        case Zone.Dungeon:
                            _backgroundSprite.Scale = Vector2.One;
                            _foregroundSprite.Scale = Vector2.One;
                            _backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                            _foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                            _backgroundSprite.Scale = new Vector2(2f, 2f);
                            _foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;

                        case Zone.Tower:
                            _backgroundSprite.Scale = Vector2.One;
                            _foregroundSprite.Scale = Vector2.One;
                            _backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                            _foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                            _backgroundSprite.Scale = new Vector2(2f, 2f);
                            _foregroundSprite.Scale = new Vector2(2f, 2f);
                            break;
                    }
                }

                if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
                {
                    _foregroundSprite.Scale = Vector2.One;
                    _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                    _foregroundSprite.Scale = new Vector2(2f, 2f);
                }
            }

            _backgroundSprite.Position = CurrentRoom.Position;
            _foregroundSprite.Position = CurrentRoom.Position;
            base.ReinitializeRTs();
        }

        private void LoadPhysicsObjects(RoomObj room)
        {
            var value = new Rectangle((int) room.X - 100, (int) room.Y - 100, room.Width + 200, room.Height + 200);
            _physicsManager.RemoveAllObjects();
            foreach (var current in CurrentRoom.TerrainObjList)
            {
                _physicsManager.AddObject(current);
            }

            foreach (var current2 in _projectileManager.ActiveProjectileList)
            {
                _physicsManager.AddObject(current2);
            }

            foreach (var current3 in CurrentRoom.GameObjList)
            {
                var physicsObj = current3 as IPhysicsObj;
                if (physicsObj != null && current3.Bounds.Intersects(value))
                {
                    var breakableObj = current3 as BreakableObj;
                    if (breakableObj == null || !breakableObj.Broken)
                    {
                        _physicsManager.AddObject(physicsObj);
                    }
                }
            }

            foreach (var current4 in CurrentRoom.DoorList)
            {
                _physicsManager.AddObject(current4);
            }

            foreach (var current5 in CurrentRoom.EnemyList)
            {
                _physicsManager.AddObject(current5);
                if (current5 is EnemyObj_BallAndChain && !current5.IsKilled)
                {
                    _physicsManager.AddObject((current5 as EnemyObj_BallAndChain).BallAndChain);
                    if (current5.Difficulty > EnemyDifficulty.Basic)
                    {
                        _physicsManager.AddObject((current5 as EnemyObj_BallAndChain).BallAndChain2);
                    }
                }
            }

            foreach (var current6 in CurrentRoom.TempEnemyList)
            {
                _physicsManager.AddObject(current6);
            }

            _physicsManager.AddObject(Player);
        }

        public void InitializeEnemies()
        {
            var list = new List<TerrainObj>();
            foreach (var current in RoomList)
            foreach (var current2 in current.EnemyList)
            {
                current2.SetPlayerTarget(Player);
                current2.SetLevelScreen(this);
                var level = current.Level;
                if (current.Name == "Boss" && current.LinkedRoom != null)
                {
                    level = current.LinkedRoom.Level;
                    var level2 = (int) (level / (4f + Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f));
                    current2.Level = level2;
                }
                else
                {
                    var num = (int) (level / (4f + Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f));
                    if (num < 1)
                    {
                        num = 1;
                    }

                    current2.Level = num;
                }

                var difficulty = current2.Level / 32;
                if (difficulty > 2)
                {
                    difficulty = 2;
                }

                if (current2.IsProcedural)
                {
                    if (current2.Difficulty == EnemyDifficulty.Expert)
                    {
                        current2.Level += 4;
                    }

                    if (current2.Difficulty < (EnemyDifficulty) difficulty)
                    {
                        current2.SetDifficulty((EnemyDifficulty) difficulty, false);
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
                            num4 = (int) (y + (x2 - x) * (num6 / num5)) - current2.TerrainBounds.Bottom;
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
            var num3 = y + (x2 - x) * (num2 / num);
            enemy.UpdateCollisionBoxes();
            num3 -= enemy.Bounds.Bottom - enemy.Y + 5f * (enemy as GameObj).ScaleX;
            enemy.Y = (float) Math.Round(num3, MidpointRounding.ToEven);
        }

        public void InitializeChests(bool resetChests)
        {
            ChestList.Clear();
            foreach (var current in RoomList)
            foreach (var current2 in current.GameObjList)
            {
                var chestObj = current2 as ChestObj;
                if (chestObj != null && chestObj.ChestType != ChestType.Fairy)
                {
                    chestObj.Level =
                        (int) (current.Level / (4f + Game.PlayerStats.GetNumberOfEquippedRunes(9) * 0.75f));
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
                                    chestObj.ChestType = ChestType.Brown;
                                    break;
                                }

                                if (i == 1)
                                {
                                    chestObj.ChestType = ChestType.Silver;
                                    break;
                                }

                                chestObj.ChestType = ChestType.Gold;
                                break;
                            }

                            i++;
                        }
                    }

                    ChestList.Add(chestObj);
                }
                else if (chestObj != null && chestObj.ChestType == ChestType.Fairy)
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
                    chestObj.X += chestObj.Width / 2;
                    chestObj.Y += 60f;
                }
            }
        }

        public void InitializeAllRooms(bool loadContent)
        {
            _castleBorderTexture = new SpriteObj("CastleBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString = "CastleCorner_Sprite";
            var cornerLTextureString = "CastleCornerL_Sprite";
            _towerBorderTexture = new SpriteObj("TowerBorder2_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString2 = "TowerCorner_Sprite";
            var cornerLTextureString2 = "TowerCornerL_Sprite";
            _dungeonBorderTexture = new SpriteObj("DungeonBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString3 = "DungeonCorner_Sprite";
            var cornerLTextureString3 = "DungeonCornerL_Sprite";
            _gardenBorderTexture = new SpriteObj("GardenBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var cornerTextureString4 = "GardenCorner_Sprite";
            var cornerLTextureString4 = "GardenCornerL_Sprite";
            _neoBorderTexture = new SpriteObj("NeoBorder_Sprite")
            {
                Scale = new Vector2(2f, 2f)
            }.ConvertToTexture(Camera, true, SamplerState.PointWrap);
            var text = "NeoCorner_Sprite";
            var text2 = "NeoCornerL_Sprite";
            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
            {
                cornerLTextureString3 =
                    cornerLTextureString = cornerLTextureString2 = cornerLTextureString4 = text2;
                cornerTextureString3 = cornerTextureString = cornerTextureString2 = cornerTextureString4 = text;
            }

            var num = 0;
            num = Game.PlayerStats.GetNumberOfEquippedRunes(8) * 8;
            if (_roomBWRenderTarget != null)
            {
                _roomBWRenderTarget.Dispose();
            }

            _roomBWRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            foreach (var current in RoomList)
            {
                var num2 = 0;
                switch (current.Zone)
                {
                    case Zone.Castle:
                        num2 = 0;
                        break;

                    case Zone.Garden:
                        num2 = 0;
                        break;

                    case Zone.Dungeon:
                        num2 = 0;
                        break;

                    case Zone.Tower:
                        num2 = 0;
                        break;
                }

                if (Game.PlayerStats.TimesCastleBeaten == 0)
                {
                    current.Level = num + num2;
                }
                else
                {
                    current.Level = num + num2 + 128 + (Game.PlayerStats.TimesCastleBeaten - 1) * 128;
                }

                num++;
                if (loadContent)
                {
                    current.LoadContent(Camera.GraphicsDevice);
                }

                current.InitializeRenderTarget(_roomBWRenderTarget);
                if (current.Name == "ChallengeBoss")
                {
                    using (var enumerator2 = current.BorderList.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            var current2 = enumerator2.Current;
                            current2.SetBorderTextures(_neoBorderTexture, text, text2);
                            current2.NeoTexture = _neoBorderTexture;
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
                                breakableObj.RelativeBounds.Y, breakableObj.Width, breakableObj.Height, 0,
                                breakableObj));
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
                    CloseBossDoor(current.LinkedRoom, current.Zone);
                }

                continue;
                IL_311:
                foreach (var current5 in current.BorderList)
                {
                    switch (current.Zone)
                    {
                        case Zone.Castle:
                            goto IL_39E;

                        case Zone.Garden:
                            current5.SetBorderTextures(_gardenBorderTexture, cornerTextureString4,
                                cornerLTextureString4);
                            current5.TextureOffset = new Vector2(0f, -18f);
                            break;

                        case Zone.Dungeon:
                            current5.SetBorderTextures(_dungeonBorderTexture, cornerTextureString3,
                                cornerLTextureString3);
                            break;

                        case Zone.Tower:
                            current5.SetBorderTextures(_towerBorderTexture, cornerTextureString2,
                                cornerLTextureString2);
                            break;

                        default:
                            goto IL_39E;
                    }

                    IL_3AD:
                    current5.NeoTexture = _neoBorderTexture;
                    continue;
                    IL_39E:
                    current5.SetBorderTextures(_castleBorderTexture, cornerTextureString, cornerLTextureString);
                    goto IL_3AD;
                }

                goto IL_3D6;
            }
        }

        public void CloseBossDoor(RoomObj linkedRoom, Zone zone)
        {
            var flag = false;
            switch (zone)
            {
                case Zone.Castle:
                    if (Game.PlayerStats.EyeballBossBeaten ||
                        RandomizerData.ChallengeKhidr)
                    {
                        flag = true;
                    }

                    break;

                case Zone.Garden:
                    if (Game.PlayerStats.FairyBossBeaten ||
                        RandomizerData.ChallengeAlexander)
                    {
                        flag = true;
                    }

                    break;

                case Zone.Dungeon:
                    if (Game.PlayerStats.BlobBossBeaten ||
                        RandomizerData.ChallengeHerodotus)
                    {
                        flag = true;
                    }

                    break;

                case Zone.Tower:
                    if (Game.PlayerStats.FireballBossBeaten ||
                        RandomizerData.ChallengeLeon)
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

            OpenChallengeBossDoor(linkedRoom, zone);
            if (Game.PlayerStats.ChallengeLastBossUnlocked)
            {
                // OpenLastBossChallengeDoors();
            }
        }

        public void OpenLastBossChallengeDoors()
        {
            LastBossChallengeRoom linkedRoom = null;
            foreach (var current in RoomList)
            {
                if (current.Name == "ChallengeBoss" && current is LastBossChallengeRoom)
                {
                    linkedRoom = current as LastBossChallengeRoom;
                    break;
                }
            }

            foreach (var current2 in RoomList)
            {
                if (current2.Name == "EntranceBoss")
                {
                    var flag = false;
                    if (current2.Zone == Zone.Castle && Game.PlayerStats.EyeballBossBeaten)
                    {
                        flag = true;
                    }
                    else if (current2.Zone == Zone.Dungeon && Game.PlayerStats.BlobBossBeaten)
                    {
                        flag = true;
                    }
                    else if (current2.Zone == Zone.Garden && Game.PlayerStats.FairyBossBeaten)
                    {
                        flag = true;
                    }
                    else if (current2.Zone == Zone.Tower && Game.PlayerStats.FireballBossBeaten)
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

        public void OpenChallengeBossDoor(RoomObj linkerRoom, Zone zone)
        {
            var flag = false;
            switch (zone)
            {
                case Zone.Castle:
                    if (!Game.PlayerStats.EyeballBossBeaten &&
                        RandomizerData.ChallengeKhidr)
                    {
                        flag = true;
                    }

                    break;

                case Zone.Garden:
                    if (!Game.PlayerStats.FairyBossBeaten &&
                        RandomizerData.ChallengeAlexander)
                    {
                        flag = true;
                    }

                    break;

                case Zone.Dungeon:
                    if (!Game.PlayerStats.BlobBossBeaten &&
                        RandomizerData.ChallengeHerodotus)
                    {
                        flag = true;
                    }

                    break;

                case Zone.Tower:
                    if (!Game.PlayerStats.FireballBossBeaten &&
                        RandomizerData.ChallengeLeon)
                    {
                        flag = true;
                    }

                    break;
            }

            if (flag)
            {
                var challengeBossRoomFromRoomList = LevelBuilder2.GetChallengeBossRoomFromRoomList(zone,
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
                if (current.X < _leftMostBorder)
                {
                    _leftMostBorder = (int) current.X;
                }

                if (current.X + current.Width > _rightMostBorder)
                {
                    _rightMostBorder = (int) current.X + current.Width;
                }

                if (current.Y < _topMostBorder)
                {
                    _topMostBorder = (int) current.Y;
                }

                if (current.Y + current.Height > _bottomMostBorder)
                {
                    _bottomMostBorder = (int) current.Y + current.Height;
                }
            }
        }

        public void AddRoom(RoomObj room)
        {
            RoomList.Add(room);
            if (room.X < _leftMostBorder)
            {
                _leftMostBorder = (int) room.X;
            }

            if (room.X + room.Width > _rightMostBorder)
            {
                _rightMostBorder = (int) room.X + room.Width;
            }

            if (room.Y < _topMostBorder)
            {
                _topMostBorder = (int) room.Y;
            }

            if (room.Y + room.Height > _bottomMostBorder)
            {
                _bottomMostBorder = (int) room.Y + room.Height;
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
                        MiniMapDisplay.AddRoom(current);

                        if (current.Name != "Start")
                        {
                            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.MapData);
                        }

                        if (current.Name == "ChallengeBoss")
                        {
                            _backgroundSprite.Scale = Vector2.One;
                            _backgroundSprite.ChangeSprite("NeoBG_Sprite", ScreenManager.Camera);
                            _backgroundSprite.Scale = new Vector2(2f, 2f);
                            _foregroundSprite.Scale = Vector2.One;
                            _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                            _foregroundSprite.Scale = new Vector2(2f, 2f);
                        }

                        if ((CurrentRoom == null || CurrentZone != current.Zone ||
                             CurrentRoom != null && CurrentRoom.Name == "ChallengeBoss") && current.Name != "Start")
                        {
                            if (current.Name != "ChallengeBoss")
                            {
                                switch (current.Zone)
                                {
                                    case Zone.Castle:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2f, 2f);
                                        _foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;

                                    case Zone.Garden:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2f, 2f);
                                        _foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;

                                    case Zone.Dungeon:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2f, 2f);
                                        _foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;

                                    case Zone.Tower:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2f, 2f);
                                        _foregroundSprite.Scale = new Vector2(2f, 2f);
                                        break;
                                }
                            }

                            if (Game.PlayerStats.Traits.X == 32f || Game.PlayerStats.Traits.Y == 32f)
                            {
                                _foregroundSprite.Scale = Vector2.One;
                                _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                                _foregroundSprite.Scale = new Vector2(2f, 2f);
                            }

                            if (current.Zone == Zone.Dungeon || Game.PlayerStats.Traits.X == 35f ||
                                Game.PlayerStats.Traits.Y == 35f || current.Name == "Compass")
                            {
                                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
                            }
                            else
                            {
                                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
                            }

                            _roomTitle.Text = WordBuilder.BuildDungeonName(current.Zone);
                            if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
                            {
                                _roomTitle.RandomizeSentence(false);
                            }

                            _roomTitle.Opacity = 0f;
                            if (current.Name != "Boss" && current.Name != "Tutorial" && current.Name != "Ending" &&
                                current.Name != "ChallengeBoss")
                            {
                                Tween.StopAllContaining(_roomEnteringTitle, false);
                                Tween.StopAllContaining(_roomTitle, false);
                                _roomTitle.Opacity = 0f;
                                _roomEnteringTitle.Opacity = 0f;
                                if (Player.X > current.Bounds.Center.X)
                                {
                                    _roomTitle.X = 50f;
                                    _roomTitle.Align = Types.TextAlign.Left;
                                    _roomEnteringTitle.X = 70f;
                                    _roomEnteringTitle.Align = Types.TextAlign.Left;
                                }
                                else
                                {
                                    _roomTitle.X = 1270f;
                                    _roomTitle.Align = Types.TextAlign.Right;
                                    _roomEnteringTitle.X = 1250f;
                                    _roomEnteringTitle.Align = Types.TextAlign.Right;
                                }

                                Tween.To(_roomTitle, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "1");
                                _roomTitle.Opacity = 1f;
                                Tween.To(_roomTitle, 0.5f, Linear.EaseNone, "delay", "2.2", "Opacity", "0");
                                _roomTitle.Opacity = 0f;
                                Tween.To(_roomEnteringTitle, 0.5f, Linear.EaseNone, "Opacity", "1");
                                _roomEnteringTitle.Opacity = 1f;
                                Tween.To(_roomEnteringTitle, 0.5f, Linear.EaseNone, "delay", "2", "Opacity", "0");
                                _roomEnteringTitle.Opacity = 0f;
                            }
                            else
                            {
                                Tween.StopAllContaining(_roomEnteringTitle, false);
                                Tween.StopAllContaining(_roomTitle, false);
                                _roomTitle.Opacity = 0f;
                                _roomEnteringTitle.Opacity = 0f;
                            }

                            JukeboxEnabled = false;
                            Console.WriteLine("Now entering " + current.Zone);
                        }

                        if (_currentRoom != null)
                        {
                            _currentRoom.OnExit();
                        }

                        _currentRoom = current;
                        _backgroundSprite.Position = CurrentRoom.Position;
                        _foregroundSprite.Position = CurrentRoom.Position;
                        _gardenParallaxFG.Position = CurrentRoom.Position;
                        if (SoundManager.IsMusicPaused)
                        {
                            SoundManager.ResumeMusic();
                        }

                        if (!DisableSongUpdating && !JukeboxEnabled)
                        {
                            UpdateLevelSong();
                        }

                        if (_currentRoom.Player == null)
                        {
                            _currentRoom.Player = Player;
                        }

                        if (_currentRoom.Name != "Start" && _currentRoom.Name != "Tutorial" &&
                            _currentRoom.Name != "Ending" && _currentRoom.Name != "CastleEntrance" &&
                            _currentRoom.Name != "Bonus" && _currentRoom.Name != "Throne" &&
                            _currentRoom.Name != "Secret" && _currentRoom.Name != "Boss" &&
                            _currentRoom.Zone != Zone.None && _currentRoom.Name != "ChallengeBoss" &&
                            (Game.PlayerStats.Traits.X == 26f || Game.PlayerStats.Traits.Y == 26f) &&
                            CDGMath.RandomFloat(0f, 1f) < 0.2f)
                        {
                            SpawnDementiaEnemy();
                        }

                        if (_currentRoom.HasFairyChest)
                        {
                            _currentRoom.DisplayFairyChestInfo();
                        }

                        _tempEnemyStartPositions.Clear();
                        _enemyStartPositions.Clear();
                        foreach (var current3 in CurrentRoom.EnemyList)
                        {
                            _enemyStartPositions.Add(current3.Position);
                        }

                        foreach (var current4 in CurrentRoom.TempEnemyList)
                        {
                            _tempEnemyStartPositions.Add(current4.Position);
                        }

                        _projectileManager.DestroyAllProjectiles(false);
                        LoadPhysicsObjects(current);
                        _itemDropManager.DestroyAllItemDrops();
                        _projectileIconPool.DestroyAllIcons();
                        _enemyPauseDuration = 0f;
                        if (LevelENV.ShowEnemyRadii)
                        {
                            foreach (var current5 in current.EnemyList)
                            {
                                current5.InitializeDebugRadii();
                            }
                        }

                        _lastEnemyHit = null;
                        foreach (var current6 in _currentRoom.GameObjList)
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
                            _currentRoom.OnEnter();
                        }

                        break;
                    }
                }
            }
        }

        private void UpdateLevelSong()
        {
            if (!(CurrentRoom.Name != "Start") || !(CurrentRoom.Name != "Tutorial") ||
                !(CurrentRoom.Name != "Ending") ||
                SoundManager.IsMusicPlaying)
            {
                if (!(_currentRoom is StartingRoomObj) && SoundManager.IsMusicPlaying)
                {
                    if ((_currentRoom is CarnivalShoot1BonusRoom || _currentRoom is CarnivalShoot2BonusRoom) &&
                        SoundManager.GetCurrentMusicName() != "PooyanSong")
                    {
                        SoundManager.PlayMusic("PooyanSong", true, 1f);
                        return;
                    }

                    if (_currentRoom.Zone == Zone.Castle &&
                        SoundManager.GetCurrentMusicName() != "CastleSong")
                    {
                        SoundManager.PlayMusic("CastleSong", true, 1f);
                        return;
                    }

                    if (_currentRoom.Zone == Zone.Garden &&
                        SoundManager.GetCurrentMusicName() != "GardenSong")
                    {
                        SoundManager.PlayMusic("GardenSong", true, 1f);
                        return;
                    }

                    if (_currentRoom.Zone == Zone.Dungeon &&
                        SoundManager.GetCurrentMusicName() != "DungeonSong")
                    {
                        SoundManager.PlayMusic("DungeonSong", true, 1f);
                        return;
                    }

                    if (_currentRoom.Zone == Zone.Tower &&
                        SoundManager.GetCurrentMusicName() != "TowerSong")
                    {
                        SoundManager.PlayMusic("TowerSong", true, 1f);
                    }
                }

                return;
            }

            if (_currentRoom is CarnivalShoot1BonusRoom || _currentRoom is CarnivalShoot2BonusRoom)
            {
                SoundManager.PlayMusic("PooyanSong", true, 1f);
                return;
            }

            switch (_currentRoom.Zone)
            {
                case Zone.Castle:
                    //IL_A8:
                    SoundManager.PlayMusic("CastleSong", true, 1f);
                    return;

                case Zone.Garden:
                    SoundManager.PlayMusic("GardenSong", true, 1f);
                    return;

                case Zone.Dungeon:
                    SoundManager.PlayMusic("DungeonSong", true, 1f);
                    return;

                case Zone.Tower:
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
            _projectileIconPool.Update(Camera);
            if (!IsPaused)
            {
                Sky.Update(gameTime);
                if (_enemyPauseDuration > 0f)
                {
                    _enemyPauseDuration -= num;
                    if (_enemyPauseDuration <= 0f)
                    {
                        StopTimeStop();
                    }
                }

                CurrentRoom.Update(gameTime);
                if (Player != null)
                {
                    Player.Update(gameTime);
                }

                _enemyHUD.Update(gameTime);
                _playerHUD.Update(Player);
                _projectileManager.Update(gameTime);
                _physicsManager.Update(gameTime);
                if (!DisableRoomTransitioning &&
                    !CollisionMath.Intersects(new Rectangle((int) Player.X, (int) Player.Y, 1, 1), Camera.Bounds))
                {
                    CheckForRoomTransition();
                }

                if ((!_inputMap.Pressed(2) ||
                     _inputMap.Pressed(2) && (LevelENV.RunDemoVersion || LevelENV.CreateRetailVersion)) &&
                    CameraLockedToPlayer)
                {
                    UpdateCamera();
                }

                if (Game.PlayerStats.SpecialItem == 6 && CurrentRoom.Name != "Start" &&
                    CurrentRoom.Name != "Tutorial" &&
                    CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Throne" && CurrentRoom.Name != "ChallengeBoss")
                {
                    if (!_compassDisplayed)
                    {
                        DisplayCompass();
                    }
                    else
                    {
                        UpdateCompass();
                    }
                }
                else if (_compassDisplayed && CurrentRoom.Name != "Compass")
                {
                    HideCompass();
                }

                if (_objectivePlate.X == 1170f)
                {
                    var flag = false;
                    var bounds = _objectivePlate.Bounds;
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
                        _objectivePlate.Opacity = 0.5f;
                    }
                    else
                    {
                        _objectivePlate.Opacity = 1f;
                    }
                }

                // Check for bosses defeated and add linker rooms if not already in.
                var gardenLinker = RoomList.Find(r => r.LinkerRoomZone == Zone.Garden);
                var towerLinker = RoomList.Find(r => r.LinkerRoomZone == Zone.Tower);
                var dungeonLinker = RoomList.Find(r => r.LinkerRoomZone == Zone.Dungeon);

                if (gardenLinker != null && Game.PlayerStats.EyeballBossBeaten &&
                    !MiniMapDisplay.AddedRoomsList.Contains(gardenLinker))
                    MiniMapDisplay.AddRoom(gardenLinker);
                if (towerLinker != null && Game.PlayerStats.FairyBossBeaten &&
                    !MiniMapDisplay.AddedRoomsList.Contains(towerLinker))
                    MiniMapDisplay.AddRoom(towerLinker);
                if (dungeonLinker != null && Game.PlayerStats.FireballBossBeaten &&
                    !MiniMapDisplay.AddedRoomsList.Contains(dungeonLinker))
                    MiniMapDisplay.AddRoom(dungeonLinker);
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

            if (_currentRoom != null)
            {
                if (ScreenManager.Camera.Width < _currentRoom.Width)
                {
                    if (ScreenManager.Camera.Bounds.Left < _currentRoom.Bounds.Left)
                    {
                        ScreenManager.Camera.X = (int) (_currentRoom.Bounds.Left + ScreenManager.Camera.Width * 0.5f);
                    }
                    else if (ScreenManager.Camera.Bounds.Right > _currentRoom.Bounds.Right)
                    {
                        ScreenManager.Camera.X = (int) (_currentRoom.Bounds.Right - ScreenManager.Camera.Width * 0.5f);
                    }
                }
                else
                {
                    ScreenManager.Camera.X = (int) (_currentRoom.X + _currentRoom.Width * 0.5f);
                }

                if (ScreenManager.Camera.Height < _currentRoom.Height)
                {
                    if (ScreenManager.Camera.Bounds.Top < _currentRoom.Bounds.Top)
                    {
                        ScreenManager.Camera.Y = (int) (_currentRoom.Bounds.Top + ScreenManager.Camera.Height * 0.5f);
                        return;
                    }

                    if (ScreenManager.Camera.Bounds.Bottom > _currentRoom.Bounds.Bottom)
                    {
                        ScreenManager.Camera.Y =
                            (int) (_currentRoom.Bounds.Bottom - ScreenManager.Camera.Height * 0.5f);
                    }
                }
                else
                {
                    ScreenManager.Camera.Y = (int) (_currentRoom.Y + _currentRoom.Height * 0.5f);
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

                if (_inputMap.JustPressed(0))
                {
                    MiniMapDisplay.AddAllRooms(RoomList);
                }

                if (_inputMap.JustPressed(7))
                {
                    LevelENV.ShowDebugText = !LevelENV.ShowDebugText;
                }

                if (_inputMap.JustPressed(1))
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
                if (_inputMap.Pressed(2) && _inputMap.Pressed(3))
                {
                    Camera.X -= num * (float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (_inputMap.Pressed(2) && _inputMap.Pressed(4))
                {
                    Camera.X += num * (float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }

                if (_inputMap.Pressed(2) && _inputMap.Pressed(5))
                {
                    Camera.Y -= num * (float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (_inputMap.Pressed(2) && _inputMap.Pressed(6))
                {
                    Camera.Y += num * (float) Camera.GameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (Player != null &&
                (!_inputMap.Pressed(2) ||
                 _inputMap.Pressed(2) && (LevelENV.RunDemoVersion || LevelENV.CreateRetailVersion)) &&
                !Player.IsKilled)
            {
                Player.HandleInput();
            }

            base.HandleInput();
        }

        private void UpdateCompass()
        {
            if (_compassDoor == null && CurrentRoom.Name != "Ending" && CurrentRoom.Name != "Boss" &&
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

                    var doorObj = new DoorObj(roomObj, 120, 180, DoorState.Open);
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
                                    doorObj.Width, doorObj.Height / 2)))
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
                    roomObj.LinkedRoom.Zone = roomObj.Zone;
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
                            cornerLTextureString = cornerLTextureString2 = cornerLTextureString4 = text2;
                        cornerTextureString3 =
                            cornerTextureString = cornerTextureString2 = cornerTextureString4 = text;
                    }

                    foreach (var current4 in roomObj.LinkedRoom.BorderList)
                    {
                        switch (roomObj.LinkedRoom.Zone)
                        {
                            case Zone.Garden:
                                current4.SetBorderTextures(_gardenBorderTexture, cornerTextureString4,
                                    cornerLTextureString4);
                                current4.TextureOffset = new Vector2(0f, -18f);
                                continue;

                            case Zone.Dungeon:
                                current4.SetBorderTextures(_dungeonBorderTexture, cornerTextureString3,
                                    cornerLTextureString3);
                                continue;

                            case Zone.Tower:
                                current4.SetBorderTextures(_towerBorderTexture, cornerTextureString2,
                                    cornerLTextureString2);
                                continue;
                        }

                        current4.SetBorderTextures(_castleBorderTexture, cornerTextureString, cornerLTextureString);
                    }

                    _compassDoor = doorObj;
                }
            }

            if (_compassDoor != null)
            {
                _compass.Rotation = CDGMath.AngleBetweenPts(Player.Position,
                    new Vector2(_compassDoor.Bounds.Center.X, _compassDoor.Bounds.Center.Y));
            }
        }

        public void RemoveCompassDoor()
        {
            if (_compassDoor != null)
            {
                _compassDoor.Room.DoorList.Remove(_compassDoor);
                _compassDoor.Dispose();
                _compassDoor = null;
            }
        }

        private void DisplayCompass()
        {
            Tween.StopAllContaining(_compassBG, false);
            Tween.StopAllContaining(_compass, false);
            Tween.To(_compassBG, 0.5f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_compass, 0.5f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
            _compassDisplayed = true;
        }

        private void HideCompass()
        {
            Tween.StopAllContaining(_compassBG, false);
            Tween.StopAllContaining(_compass, false);
            Tween.To(_compassBG, 0.5f, Back.EaseInLarge, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_compass, 0.5f, Back.EaseInLarge, "ScaleX", "0", "ScaleY", "0");
            _compassDisplayed = false;
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

            if (_fgRenderTarget != null)
            {
                _fgRenderTarget.Dispose();
            }

            _fgRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra5551,
                DepthFormat.None);
            if (_shadowRenderTarget != null)
            {
                _shadowRenderTarget.Dispose();
            }

            _shadowRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2, false, SurfaceFormat.Bgra4444,
                DepthFormat.None);
            Camera.Begin();
            Camera.GraphicsDevice.SetRenderTarget(_shadowRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.End();
            if (_lightSourceRenderTarget != null)
            {
                _lightSourceRenderTarget.Dispose();
            }

            _lightSourceRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2, false,
                SurfaceFormat.Bgra4444, DepthFormat.None);
            if (RenderTarget != null)
            {
                RenderTarget.Dispose();
            }

            RenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            if (_skyRenderTarget != null)
            {
                _skyRenderTarget.Dispose();
            }

            _skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2);
            if (_bgRenderTarget != null)
            {
                _bgRenderTarget.Dispose();
            }

            _bgRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color,
                DepthFormat.None);
            if (_traitAuraRenderTarget != null)
            {
                _traitAuraRenderTarget.Dispose();
            }

            _traitAuraRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, num, num2);
            InitializeBackgroundObjs();
        }

        public void InitializeBackgroundObjs()
        {
            if (_foregroundSprite != null)
            {
                _foregroundSprite.Dispose();
            }

            _foregroundSprite = new BackgroundObj("CastleFG1_Sprite");
            _foregroundSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
            _foregroundSprite.Scale = new Vector2(2f, 2f);
            if (_backgroundSprite != null)
            {
                _backgroundSprite.Dispose();
            }

            _backgroundSprite = new BackgroundObj("CastleBG1_Sprite");
            _backgroundSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
            _backgroundSprite.Scale = new Vector2(2f, 2f);
            if (_backgroundParallaxSprite != null)
            {
                _backgroundParallaxSprite.Dispose();
            }

            _backgroundParallaxSprite = new BackgroundObj("TowerBGFrame_Sprite");
            _backgroundParallaxSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
            _backgroundParallaxSprite.Scale = new Vector2(2f, 2f);
            if (_gardenParallaxFG != null)
            {
                _gardenParallaxFG.Dispose();
            }

            _gardenParallaxFG = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
            _gardenParallaxFG.SetRepeated(true, true, Camera, SamplerState.LinearWrap);
            _gardenParallaxFG.TextureColor = Color.White;
            _gardenParallaxFG.Scale = new Vector2(3f, 3f);
            _gardenParallaxFG.Opacity = 0.7f;
            _gardenParallaxFG.ParallaxSpeed = new Vector2(0.3f, 0f);
        }

        public void DrawRenderTargets()
        {
            if (_backgroundSprite.Texture.IsContentLost)
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
            Camera.GraphicsDevice.SetRenderTarget(_fgRenderTarget);
            _foregroundSprite.Draw(Camera);
            if (!EnemiesPaused)
            {
                if (Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f)
                {
                    _traitAura.Scale = new Vector2(15f, 15f);
                }
                else if (Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f)
                {
                    _traitAura.Scale = new Vector2(8f, 8f);
                }
                else
                {
                    _traitAura.Scale = new Vector2(10f, 10f);
                }
            }

            Camera.GraphicsDevice.SetRenderTarget(_traitAuraRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Transparent);
            if (CurrentRoom != null)
            {
                _traitAura.Position = Player.Position;
                _traitAura.Draw(Camera);
            }

            Camera.GraphicsDevice.SetRenderTarget(_lightSourceRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Transparent);
            if (CurrentRoom != null)
            {
                _dungeonLight.Position = Player.Position;
                _dungeonLight.Draw(Camera);
            }

            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            MiniMapDisplay.DrawRenderTargets(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(_skyRenderTarget);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            Sky.Draw(Camera);
            Camera.End();
        }

        private static Vector2 MoveInCircle(GameTime gameTime, float speed)
        {
            double num = Game.TotalGameTimeSeconds * speed;
            var x = (float) Math.Cos(num);
            var y = (float) Math.Sin(num);
            return new Vector2(x, y);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawRenderTargets();
            Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                Camera.GetTransformation());
            _backgroundSprite.Draw(Camera);
            if (CurrentRoom != null && Camera.Zoom == 1f &&
                (!_inputMap.Pressed(2) ||
                 _inputMap.Pressed(2) && (LevelENV.RunDemoVersion || LevelENV.CreateRetailVersion)))
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

            Camera.GraphicsDevice.Textures[1] = _skyRenderTarget;
            Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.ParallaxEffect);
            if (!EnemiesPaused)
            {
                Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
            }

            Camera.End();

            if (CurrentRoom != null)
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null,
                    RasterizerState.CullNone, Game.BWMaskEffect, Camera.GetTransformation());
                Camera.GraphicsDevice.Textures[1] = _fgRenderTarget;
                Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                Camera.Draw(CurrentRoom.BGRender, Camera.TopLeftCorner, Color.White);
                Camera.End();
            }

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

            if (CurrentRoom != null)
            {
                CurrentRoom.Draw(Camera);
                if (LevelENV.ShowEnemyRadii)
                {
                    foreach (var current2 in _currentRoom.EnemyList)
                    {
                        current2.DrawDetectionRadii(Camera);
                    }
                }
            }

            _projectileManager.Draw(Camera);
            if (EnemiesPaused)
            {
                Camera.End();
                Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
                Camera.GraphicsDevice.Textures[1] = _traitAuraRenderTarget;
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.InvertShader);
                Camera.Draw(RenderTarget, Vector2.Zero, Color.White);
                Camera.End();
                Game.HSVEffect.Parameters["Saturation"].SetValue(0);
                Game.HSVEffect.Parameters["UseMask"].SetValue(true);
                Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
                Camera.GraphicsDevice.Textures[1] = _traitAuraRenderTarget;
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.HSVEffect);
                Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
            }

            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.GetTransformation());
            Camera.Draw(Game.GenericTexture,
                new Rectangle((int) Camera.TopLeftCorner.X, (int) Camera.TopLeftCorner.Y, 1320, 720),
                Color.Black * BackBufferOpacity);
            if (!Player.IsKilled)
            {
                Player.Draw(Camera);
            }

            if (!LevelENV.CreateRetailVersion)
            {
                DebugTextObj.Position = new Vector2(Camera.X, Camera.Y - 300f);
                DebugTextObj.Draw(Camera);
            }

            _itemDropManager.Draw(Camera);
            ImpactEffectPool.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,
                Camera.GetTransformation());
            _textManager.Draw(Camera);
            if (CurrentRoom is { Zone: Zone.Tower })
            {
                _gardenParallaxFG.Draw(Camera);
            }

            _whiteBG.Draw(Camera);
            Camera.End();
            if ((CurrentZone == Zone.Dungeon || Game.PlayerStats.Traits.X == 35f ||
                 Game.PlayerStats.Traits.Y == 35f) &&
                (Game.PlayerStats.Class != 13 || Game.PlayerStats.Class == 13 && !Player.LightOn))
            {
                Camera.GraphicsDevice.Textures[1] = _lightSourceRenderTarget;
                Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.ShadowEffect);
                if (LevelENV.SaveFrames)
                {
                    Camera.Draw(_shadowRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero,
                        new Vector2(2f, 2f), SpriteEffects.None, 1f);
                }
                else
                {
                    Camera.Draw(_shadowRenderTarget, Vector2.Zero, Color.White);
                }

                Camera.End();
            }

            if (CurrentRoom is { Name: not "Ending" })
            {
                if ((Game.PlayerStats.Traits.X == 3f || Game.PlayerStats.Traits.Y == 3f) &&
                    Game.PlayerStats.SpecialItem != 8)
                {
                    Game.GaussianBlur.InvertMask = true;
                    Game.GaussianBlur.Draw(RenderTarget, Camera, _traitAuraRenderTarget);
                }
                else if ((Game.PlayerStats.Traits.X == 4f || Game.PlayerStats.Traits.Y == 4f) &&
                         Game.PlayerStats.SpecialItem != 8)
                {
                    Game.GaussianBlur.InvertMask = false;
                    Game.GaussianBlur.Draw(RenderTarget, Camera, _traitAuraRenderTarget);
                }
            }

            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            _projectileIconPool.Draw(Camera);
            _playerHUD.Draw(Camera);
            if (_lastEnemyHit != null && _enemyHUDCounter > 0f)
            {
                _enemyHUD.Draw(Camera);
            }

            if (_enemyHUDCounter > 0f)
            {
                _enemyHUDCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (CurrentRoom != null && CurrentRoom.Name != "Start" && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "ChallengeBoss" &&
                MiniMapDisplay.Visible)
            {
                _mapBG.Draw(Camera);
                MiniMapDisplay.Draw(Camera);
            }

            if (CurrentRoom != null && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Ending")
            {
                _compassBG.Draw(Camera);
                _compass.Draw(Camera);
            }

            _objectivePlate.Draw(Camera);
            _roomEnteringTitle.Draw(Camera);
            _roomTitle.Draw(Camera);
            if (CurrentRoom != null &&
                CurrentRoom.Name != "Ending" &&
                (!Game.PlayerStats.TutorialComplete || Game.PlayerStats.Traits.X == 29f ||
                 Game.PlayerStats.Traits.Y == 29f) && Game.PlayerStats.SpecialItem != 8)
            {
                _filmGrain.Draw(Camera);
            }

            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            _blackBorder1.Draw(Camera);
            _blackBorder2.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
            Game.RippleEffect.Parameters["width"].SetValue(ShoutMagnitude);
            var vector = Player.Position - Camera.TopLeftCorner;
            if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            {
                Game.RippleEffect.Parameters["xcenter"].SetValue(vector.X / 1320f);
                Game.RippleEffect.Parameters["ycenter"].SetValue(vector.Y / 720f);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.RippleEffect);
            }
            else
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    null);
            }

            Camera.Draw(RenderTarget, Vector2.Zero, Color.White);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            if (CurrentRoom != null && CurrentRoom.Name != "Ending")
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
                    Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
                    Camera.End();
                    Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
                    Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null,
                        null, null);
                    var color = new Color(180, 150, 80);
                    Camera.Draw(RenderTarget, Vector2.Zero, color);
                    _creditsText.Draw(Camera);
                    _creditsTitleText.Draw(Camera);
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
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    null);
            }

            Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
            Camera.End();
            base.Draw(gameTime);
        }

        public void RunWhiteSlashEffect()
        {
            _whiteBG.Position = CurrentRoom.Position;
            _whiteBG.Scale = Vector2.One;
            _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
            _whiteBG.Opacity = 1f;
            Tween.To(_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.RunFunction(0.2f, this, "RunWhiteSlash2");
        }

        public void RunWhiteSlash2()
        {
            _whiteBG.Position = CurrentRoom.Position;
            _whiteBG.Scale = Vector2.One;
            _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
            _whiteBG.Opacity = 1f;
            Tween.To(_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
        }

        public void LightningEffectTwice()
        {
            _whiteBG.Position = CurrentRoom.Position;
            _whiteBG.Scale = Vector2.One;
            _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
            _whiteBG.Opacity = 1f;
            Tween.To(_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
            Tween.RunFunction(0.2f, this, "LightningEffectOnce");
        }

        public void LightningEffectOnce()
        {
            _whiteBG.Position = CurrentRoom.Position;
            _whiteBG.Scale = Vector2.One;
            _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
            _whiteBG.Opacity = 1f;
            Tween.To(_whiteBG, 1f, Tween.EaseNone, "Opacity", "0");
            SoundManager.PlaySound("LightningClap1", "LightningClap2");
        }

        public void SpawnDementiaEnemy()
        {
            var list = new List<EnemyObj>();
            foreach (var current in _currentRoom.EnemyList)
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
            _currentRoom.TempEnemyList.Add(enemy);
            _physicsManager.AddObject(enemy);
            _tempEnemyStartPositions.Add(enemy.Position);
            enemy.SetPlayerTarget(Player);
            enemy.SetLevelScreen(this);
            enemy.Initialize();
        }

        public void RemoveEnemyFromCurrentRoom(EnemyObj enemy, Vector2 startingPos)
        {
            _currentRoom.TempEnemyList.Remove(enemy);
            _physicsManager.RemoveObject(enemy);
            _tempEnemyStartPositions.Remove(startingPos);
        }

        public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room, Vector2 startingPos)
        {
            room.TempEnemyList.Remove(enemy);
            _physicsManager.RemoveObject(enemy);
            _tempEnemyStartPositions.Remove(startingPos);
        }

        public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room)
        {
            var num = room.TempEnemyList.IndexOf(enemy);
            if (num != -1)
            {
                room.TempEnemyList.RemoveAt(num);
                _physicsManager.RemoveObject(enemy);
                _tempEnemyStartPositions.RemoveAt(num);
            }
        }

        public void ResetEnemyPositions()
        {
            for (var i = 0; i < _enemyStartPositions.Count; i++)
            {
                CurrentRoom.EnemyList[i].Position = _enemyStartPositions[i];
            }

            for (var j = 0; j < _tempEnemyStartPositions.Count; j++)
            {
                CurrentRoom.TempEnemyList[j].Position = _tempEnemyStartPositions[j];
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
                    _projectileManager.PauseAllProjectiles(true);
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
                    _projectileManager.UnpauseAllProjectiles();
                }

                SoundManager.ResumeAllSounds("Pauseable");
                Player.ResumeAnimation();
                base.UnpauseScreen();
            }
        }

        public void RunGameOver()
        {
            Player.Opacity = 1f;
            _killedEnemyObjList.Clear();
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
                        _killedEnemyObjList.Add(item);
                    }
                }
            }

            var list = new List<object>();
            list.Add(Player);
            list.Add(_killedEnemyObjList);
            list.Add(_coinsCollected);
            list.Add(_bagsCollected);
            list.Add(_diamondsCollected);
            list.Add(_objKilledPlayer);
            Tween.RunFunction(0f, ScreenManager, "DisplayScreen", 7, true, list);
        }

        public void RunCinematicBorders(float duration)
        {
            StopCinematicBorders();
            _blackBorder1.Opacity = 1f;
            _blackBorder2.Opacity = 1f;
            _blackBorder1.Y = 0f;
            _blackBorder2.Y = 720 - _borderSize;
            var num = 1f;
            Tween.By(_blackBorder1, num, Quad.EaseInOut, "delay", (duration - num).ToString(), "Y",
                (-_borderSize).ToString());
            Tween.By(_blackBorder2, num, Quad.EaseInOut, "delay", (duration - num).ToString(), "Y",
                _borderSize.ToString());
            Tween.To(_blackBorder1, num, Linear.EaseNone, "delay", (duration - num + 0.2f).ToString(), "Opacity", "0");
            Tween.To(_blackBorder2, num, Linear.EaseNone, "delay", (duration - num + 0.2f).ToString(), "Opacity", "0");
        }

        public void StopCinematicBorders()
        {
            Tween.StopAllContaining(_blackBorder1, false);
            Tween.StopAllContaining(_blackBorder2, false);
        }

        public void DisplayMap(bool isTeleporterScreen)
        {
            (ScreenManager as RCScreenManager).AddRoomsToMap(MiniMapDisplay.AddedRoomsList);
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

            _projectileManager.PauseAllProjectiles(false);
        }

        public void CastTimeStop(float duration)
        {
            SoundManager.PlaySound("Cast_TimeStart");
            SoundManager.PauseMusic();
            _enemyPauseDuration = duration;
            PauseAllEnemies();
            Tween.To(_traitAura, 0.2f, Tween.EaseNone, "ScaleX", "100", "ScaleY", "100");
        }

        public void StopTimeStop()
        {
            SoundManager.PlaySound("Cast_TimeStop");
            SoundManager.ResumeMusic();
            Tween.To(_traitAura, 0.2f, Tween.EaseNone, "ScaleX", "0", "ScaleY", "0");
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

            _projectileManager.UnpauseAllProjectiles();
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
            _killedEnemyObjList.Clear();
            _diamondsCollected = 0;
            _coinsCollected = 0;
            _bagsCollected = 0;
            _blueprintsCollected = 0;
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
            foreach (var current3 in current2.GameObjList)
            {
                var breakableObj = current3 as BreakableObj;
                if (breakableObj != null)
                {
                    breakableObj.Reset();
                }
            }

            _projectileManager.DestroyAllProjectiles(true);
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }

        public override void DisposeRTs()
        {
            _fgRenderTarget.Dispose();
            _fgRenderTarget = null;
            _bgRenderTarget.Dispose();
            _bgRenderTarget = null;
            _skyRenderTarget.Dispose();
            _skyRenderTarget = null;
            RenderTarget.Dispose();
            RenderTarget = null;
            _shadowRenderTarget.Dispose();
            _shadowRenderTarget = null;
            _lightSourceRenderTarget.Dispose();
            _lightSourceRenderTarget = null;
            _traitAuraRenderTarget.Dispose();
            _traitAuraRenderTarget = null;
            _foregroundSprite.Dispose();
            _foregroundSprite = null;
            _backgroundSprite.Dispose();
            _backgroundSprite = null;
            _backgroundParallaxSprite.Dispose();
            _backgroundParallaxSprite = null;
            _gardenParallaxFG.Dispose();
            _gardenParallaxFG = null;
            _roomBWRenderTarget.Dispose();
            _roomBWRenderTarget = null;
            MiniMapDisplay.DisposeRTs();
            base.DisposeRTs();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Procedural Level Screen");
            Tween.StopAll(false);
            _currentRoom = null;
            DisposeRTs();
            foreach (var current in RoomList)
            {
                current.Dispose();
            }

            RoomList.Clear();
            RoomList = null;
            _enemyStartPositions.Clear();
            _enemyStartPositions = null;
            _tempEnemyStartPositions.Clear();
            _tempEnemyStartPositions = null;
            _textManager.Dispose();
            _textManager = null;
            _physicsManager = null;
            _projectileManager.Dispose();
            _projectileManager = null;
            _itemDropManager.Dispose();
            _itemDropManager = null;
            _currentRoom = null;
            MiniMapDisplay.Dispose();
            MiniMapDisplay = null;
            _mapBG.Dispose();
            _mapBG = null;
            _inputMap.Dispose();
            _inputMap = null;
            _lastEnemyHit = null;
            _playerHUD.Dispose();
            _playerHUD = null;
            Player = null;
            _enemyHUD.Dispose();
            _enemyHUD = null;
            ImpactEffectPool.Dispose();
            ImpactEffectPool = null;
            _blackBorder1.Dispose();
            _blackBorder1 = null;
            _blackBorder2.Dispose();
            _blackBorder2 = null;
            ChestList.Clear();
            ChestList = null;
            _projectileIconPool.Dispose();
            _projectileIconPool = null;
            _objKilledPlayer = null;
            _dungeonLight.Dispose();
            _dungeonLight = null;
            _traitAura.Dispose();
            _traitAura = null;
            _killedEnemyObjList.Clear();
            _killedEnemyObjList = null;
            _roomEnteringTitle.Dispose();
            _roomEnteringTitle = null;
            _roomTitle.Dispose();
            _roomTitle = null;
            _creditsText.Dispose();
            _creditsText = null;
            _creditsTitleText.Dispose();
            _creditsTitleText = null;
            Array.Clear(_creditsTextTitleList, 0, _creditsTextTitleList.Length);
            Array.Clear(_creditsTextList, 0, _creditsTextList.Length);
            _creditsTextTitleList = null;
            _creditsTextList = null;
            _filmGrain.Dispose();
            _filmGrain = null;
            _objectivePlate.Dispose();
            _objectivePlate = null;
            _objectivePlateTween = null;
            Sky.Dispose();
            Sky = null;
            _whiteBG.Dispose();
            _whiteBG = null;
            _compassBG.Dispose();
            _compassBG = null;
            _compass.Dispose();
            _compass = null;
            if (_compassDoor != null)
            {
                _compassDoor.Dispose();
            }

            _compassDoor = null;
            _castleBorderTexture.Dispose();
            _gardenBorderTexture.Dispose();
            _towerBorderTexture.Dispose();
            _dungeonBorderTexture.Dispose();
            _neoBorderTexture.Dispose();
            _castleBorderTexture = null;
            _gardenBorderTexture = null;
            _towerBorderTexture = null;
            _dungeonBorderTexture = null;
            DebugTextObj.Dispose();
            DebugTextObj = null;
            base.Dispose();
        }

        public void SetLastEnemyHit(EnemyObj enemy)
        {
            _lastEnemyHit = enemy;
            _enemyHUDCounter = _enemyHUDDuration;
            _enemyHUD.UpdateEnemyInfo(_lastEnemyHit.Name, _lastEnemyHit.Level,
                _lastEnemyHit.CurrentHealth / (float) _lastEnemyHit.MaxHealth);
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
                _coinsCollected++;
                return;
            }

            switch (itemDropType)
            {
                case 10:
                    _bagsCollected++;
                    return;

                case 11:
                    _diamondsCollected++;
                    return;

                case 12:
                case 13:
                    _blueprintsCollected++;
                    return;

                default:
                    return;
            }
        }

        public void RefreshMapChestIcons()
        {
            MiniMapDisplay.RefreshChestIcons(CurrentRoom);
            (ScreenManager as RCScreenManager).RefreshMapScreenChestIcons(CurrentRoom);
        }

        public void DisplayObjective(string objectiveTitle, string objectiveDescription, string objectiveProgress,
            bool tween)
        {
            (_objectivePlate.GetChildAt(4) as SpriteObj).ScaleX = 0f;
            (_objectivePlate.GetChildAt(5) as SpriteObj).ScaleX = 0f;
            _objectivePlate.GetChildAt(2).Opacity = 1f;
            _objectivePlate.GetChildAt(3).Opacity = 1f;
            _objectivePlate.X = 1470f;
            if (_objectivePlateTween != null && _objectivePlateTween.TweenedObject == _objectivePlate &&
                _objectivePlateTween.Active)
            {
                _objectivePlateTween.StopTween(false);
            }

            (_objectivePlate.GetChildAt(1) as TextObj).Text = objectiveTitle;
            (_objectivePlate.GetChildAt(2) as TextObj).Text = objectiveDescription;
            (_objectivePlate.GetChildAt(3) as TextObj).Text = objectiveProgress;
            if (tween)
            {
                _objectivePlateTween = Tween.By(_objectivePlate, 0.5f, Back.EaseOut, "X", "-300");
                return;
            }

            _objectivePlate.X -= 300f;
        }

        public void ResetObjectivePlate(bool tween)
        {
            if (_objectivePlate != null)
            {
                _objectivePlate.X = 1170f;
                if (_objectivePlateTween != null && _objectivePlateTween.TweenedObject == _objectivePlate &&
                    _objectivePlateTween.Active)
                {
                    _objectivePlateTween.StopTween(false);
                }

                if (tween)
                {
                    Tween.By(_objectivePlate, 0.5f, Back.EaseIn, "X", "300");
                    return;
                }

                _objectivePlate.X += 300f;
            }
        }

        public void UpdateObjectiveProgress(string progress)
        {
            (_objectivePlate.GetChildAt(3) as TextObj).Text = progress;
        }

        public void ObjectiveFailed()
        {
            (_objectivePlate.GetChildAt(1) as TextObj).Text = "Objective Failed";
            _objectivePlate.GetChildAt(2).Opacity = 0.3f;
            _objectivePlate.GetChildAt(3).Opacity = 0.3f;
        }

        public void ObjectiveComplete()
        {
            _objectivePlate.GetChildAt(2).Opacity = 0.3f;
            _objectivePlate.GetChildAt(3).Opacity = 0.3f;
            _objectivePlate.X = 1170f;
            if (_objectivePlateTween != null && _objectivePlateTween.TweenedObject == _objectivePlate &&
                _objectivePlateTween.Active)
            {
                _objectivePlateTween.StopTween(false);
            }

            (_objectivePlate.GetChildAt(1) as TextObj).Text = "Objective Complete!";
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
            if (Game.PlayerStats.Class == 13 || LevelENV.EnablePlayerDebug)
            {
                MiniMapDisplay.AddAllIcons(RoomList);
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
            if (_currentRoom != null)
            {
                _currentRoom.OnExit();
            }

            SoundManager.StopAllSounds("Default");
            SoundManager.StopAllSounds("Pauseable");
            base.OnExit();
        }

        public void RevealMorning()
        {
            Sky.MorningOpacity = 0f;
            Tween.To(Sky, 2f, Tween.EaseNone, "MorningOpacity", "1");
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

        public void UpdateLevel(Zone zone)
        {
            switch (zone)
            {
                case Zone.Castle:
                    _backgroundSprite.Scale = Vector2.One;
                    _foregroundSprite.Scale = Vector2.One;
                    _backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                    _foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                    _backgroundSprite.Scale = new Vector2(2f, 2f);
                    _foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;

                case Zone.Garden:
                    _backgroundSprite.Scale = Vector2.One;
                    _foregroundSprite.Scale = Vector2.One;
                    _backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                    _foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                    _backgroundSprite.Scale = new Vector2(2f, 2f);
                    _foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;

                case Zone.Dungeon:
                    _backgroundSprite.Scale = Vector2.One;
                    _foregroundSprite.Scale = Vector2.One;
                    _backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                    _foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                    _backgroundSprite.Scale = new Vector2(2f, 2f);
                    _foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;

                case Zone.Tower:
                    _backgroundSprite.Scale = Vector2.One;
                    _foregroundSprite.Scale = Vector2.One;
                    _backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                    _foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                    _backgroundSprite.Scale = new Vector2(2f, 2f);
                    _foregroundSprite.Scale = new Vector2(2f, 2f);
                    break;
            }

            if (zone == Zone.Dungeon)
            {
                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
                return;
            }

            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }

        public void RefreshPlayerHUDPos()
        {
            _playerHUD.SetPosition(new Vector2(20f, 40f));
        }

        public void UpdatePlayerHUD()
        {
            _playerHUD.Update(Player);
        }

        public void UpdatePlayerHUDAbilities()
        {
            _playerHUD.UpdateAbilityIcons();
        }

        public void UpdatePlayerHUDSpecialItem()
        {
            _playerHUD.UpdateSpecialItemIcon();
        }

        public void AddReceivedItem(ItemCode.ItemType type, long item, string receivedFrom,
            Tuple<float, float, float, float> stats)
        {
            _playerHUD.AddReceivedItem(type, item, receivedFrom, stats);
        }

        public void UpdatePlayerSpellIcon()
        {
            _playerHUD.UpdateSpellIcon();
        }

        public void SetMapDisplayVisibility(bool visible)
        {
            MiniMapDisplay.Visible = visible;
        }

        public void SetPlayerHUDVisibility(bool visible)
        {
            _playerHUD.Visible = visible;
        }

        public void SetObjectKilledPlayer(GameObj obj)
        {
            _objKilledPlayer = obj;
        }
    }
}
