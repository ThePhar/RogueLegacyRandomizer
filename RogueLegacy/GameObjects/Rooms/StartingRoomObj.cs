//  RogueLegacyRandomizer - StartingRoomObj.cs
//  Last Modified 2023-10-26 12:09 PM
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.GameObjects.Rooms;

public class StartingRoomObj : RoomObj
{
    private const    int                 ENCHANTRESS_HEAD_LAYER = 4;
    private const    byte                ARCHITECT_HEAD_LAYER   = 1;
    private          ObjContainer        _architect;
    private          TerrainObj          _architectBlock;
    private          SpriteObj           _architectIcon;
    private readonly Vector2             _architectIconPosition;
    private          bool                _architectRenovating;
    private          BlacksmithObj       _blacksmith;
    private          FrameSoundObj       _blacksmithAnvilSound;
    private          TerrainObj          _blacksmithBlock;
    private          SpriteObj           _blacksmithBoard;
    private          SpriteObj           _blacksmithIcon;
    private readonly Vector2             _blacksmithIconPosition;
    private          SpriteObj           _blacksmithNewIcon;
    private          bool                _controlsLocked;
    private          ObjContainer        _enchantress;
    private          TerrainObj          _enchantressBlock;
    private          SpriteObj           _enchantressIcon;
    private readonly Vector2             _enchantressIconPosition;
    private          SpriteObj           _enchantressNewIcon;
    private          GameObj             _fern1;
    private          GameObj             _fern2;
    private          GameObj             _fern3;
    private          bool                _horizontalShake;
    private          bool                _isRaining;
    private          bool                _isSnowing;
    private          float               _lightningTimer;
    private          SpriteObj           _mountain1;
    private          SpriteObj           _mountain2;
    private          bool                _playerWalkedOut;
    private          List<RaindropObj>   _rainFg;
    private          Cue                 _rainSfx;
    private          float               _screenShakeCounter;
    private          float               _screenShakeMagnitude;
    private          SpriteObj           _screw;
    private          bool                _shakeScreen;
    private          Vector2             _shakeStartingPos;
    private          SpriteObj           _tent;
    private          PhysicsObjContainer _tollCollector;
    private          SpriteObj           _tollCollectorIcon;
    private          GameObj             _tree1;
    private          GameObj             _tree2;
    private          GameObj             _tree3;
    private          bool                _verticalShake;

    public StartingRoomObj()
    {
        _blacksmith = new BlacksmithObj();
        _blacksmith.Flip = SpriteEffects.FlipHorizontally;
        _blacksmith.Scale = new Vector2(2.5f, 2.5f);
        _blacksmith.Position = new Vector2(700f, 660f - (_blacksmith.Bounds.Bottom - _blacksmith.Y) - 1f);
        _blacksmith.OutlineWidth = 2;
        _blacksmithBoard = new SpriteObj("StartRoomBlacksmithBoard_Sprite");
        _blacksmithBoard.Scale = new Vector2(2f, 2f);
        _blacksmithBoard.OutlineWidth = 2;
        _blacksmithBoard.Position = new Vector2(_blacksmith.X - _blacksmithBoard.Width / 2 - 35f,
            _blacksmith.Bounds.Bottom - _blacksmithBoard.Height - 1);
        _blacksmithIcon = new SpriteObj("UpArrowBubble_Sprite");
        _blacksmithIcon.Scale = new Vector2(2f, 2f);
        _blacksmithIcon.Visible = false;
        _blacksmithIconPosition = new Vector2(_blacksmith.X - 60f, _blacksmith.Y - 10f);
        _blacksmithIcon.Flip = _blacksmith.Flip;
        _blacksmithIcon.OutlineWidth = 2;
        _blacksmithNewIcon = new SpriteObj("ExclamationSquare_Sprite");
        _blacksmithNewIcon.Visible = false;
        _blacksmithNewIcon.OutlineWidth = 2;
        _enchantressNewIcon = _blacksmithNewIcon.Clone() as SpriteObj;
        _enchantress = new ObjContainer("Enchantress_Character");
        _enchantress.Scale = new Vector2(2f, 2f);
        _enchantress.Flip = SpriteEffects.FlipHorizontally;
        _enchantress.Position = new Vector2(1150f,
            660f - (_enchantress.Bounds.Bottom - _enchantress.AnchorY) - 2f);
        _enchantress.PlayAnimation();
        _enchantress.AnimationDelay = 0.1f;
        (_enchantress.GetChildAt(4) as IAnimateableObj).StopAnimation();
        _enchantress.OutlineWidth = 2;
        _tent = new SpriteObj("StartRoomGypsyTent_Sprite");
        _tent.Scale = new Vector2(1.5f, 1.5f);
        _tent.OutlineWidth = 2;
        _tent.Position = new Vector2(_enchantress.X - _tent.Width / 2 + 5f,
            _enchantress.Bounds.Bottom - _tent.Height);
        _enchantressIcon = new SpriteObj("UpArrowBubble_Sprite");
        _enchantressIcon.Scale = new Vector2(2f, 2f);
        _enchantressIcon.Visible = false;
        _enchantressIconPosition = new Vector2(_enchantress.X - 60f, _enchantress.Y - 100f);
        _enchantressIcon.Flip = _enchantress.Flip;
        _enchantressIcon.OutlineWidth = 2;
        _architect = new ObjContainer("ArchitectIdle_Character");
        _architect.Flip = SpriteEffects.FlipHorizontally;
        _architect.Scale = new Vector2(2f, 2f);
        _architect.Position = new Vector2(1550f, 660f - (_architect.Bounds.Bottom - _architect.AnchorY) - 2f);
        _architect.PlayAnimation();
        _architect.AnimationDelay = 0.1f;
        _architect.OutlineWidth = 2;
        (_architect.GetChildAt(1) as IAnimateableObj).StopAnimation();
        _architectIcon = new SpriteObj("UpArrowBubble_Sprite");
        _architectIcon.Scale = new Vector2(2f, 2f);
        _architectIcon.Visible = false;
        _architectIconPosition = new Vector2(_architect.X - 60f, _architect.Y - 100f);
        _architectIcon.Flip = _architect.Flip;
        _architectIcon.OutlineWidth = 2;
        _architectRenovating = false;
        _screw = new SpriteObj("ArchitectGear_Sprite");
        _screw.Scale = new Vector2(2f, 2f);
        _screw.OutlineWidth = 2;
        _screw.Position = new Vector2(_architect.X + 30f, _architect.Bounds.Bottom - 1);
        _screw.AnimationDelay = 0.1f;
        _tollCollector = new PhysicsObjContainer("NPCTollCollectorIdle_Character");
        _tollCollector.Flip = SpriteEffects.FlipHorizontally;
        _tollCollector.Scale = new Vector2(2.5f, 2.5f);
        _tollCollector.IsWeighted = false;
        _tollCollector.IsCollidable = true;
        _tollCollector.Position = new Vector2(2565f,
            420f - (_tollCollector.Bounds.Bottom - _tollCollector.AnchorY));
        _tollCollector.PlayAnimation();
        _tollCollector.AnimationDelay = 0.1f;
        _tollCollector.OutlineWidth = 2;
        _tollCollector.CollisionTypeTag = 1;
        _tollCollectorIcon = new SpriteObj("UpArrowBubble_Sprite");
        _tollCollectorIcon.Scale = new Vector2(2f, 2f);
        _tollCollectorIcon.Visible = false;
        _tollCollectorIcon.Flip = _tollCollector.Flip;
        _tollCollectorIcon.OutlineWidth = 2;
        _rainFg = new List<RaindropObj>();
        var num = 400;
        if (LevelENV.SaveFrames)
        {
            num /= 2;
        }

        for (var i = 0; i < num; i++)
        {
            var item =
                new RaindropObj(new Vector2(CDGMath.RandomInt(-100, 2540), CDGMath.RandomInt(-400, 720)));
            _rainFg.Add(item);
        }
    }

    private bool BlacksmithNewIconVisible
    {
        get
        {
            foreach (var current in Game.PlayerStats.GetBlueprintArray)
            {
                var array = current;
                for (var i = 0; i < array.Length; i++)
                {
                    var b = array[i];
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
            foreach (var current in Game.PlayerStats.GetRuneArray)
            {
                var array = current;
                for (var i = 0; i < array.Length; i++)
                {
                    var b = array[i];
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
        get { return SkillSystem.GetSkill(SkillType.Smithy).ModifierAmount > 0f; }
    }

    private bool EnchantressAvailable
    {
        get { return SkillSystem.GetSkill(SkillType.Enchanter).ModifierAmount > 0f; }
    }

    private bool ArchitectAvailable
    {
        get { return SkillSystem.GetSkill(SkillType.Architect).ModifierAmount > 0f; }
    }

    private bool TollCollectorAvailable
    {
        get { return Game.PlayerStats.TimesDead > 0 && _tollCollector.Visible && !RandomizerData.DisableCharon; }
    }

    public override void Initialize()
    {
        foreach (var current in TerrainObjList)
        {
            if (current.Name == "BlacksmithBlock")
            {
                _blacksmithBlock = current;
            }

            if (current.Name == "EnchantressBlock")
            {
                _enchantressBlock = current;
            }

            if (current.Name == "ArchitectBlock")
            {
                _architectBlock = current;
            }

            if (current.Name == "bridge")
            {
                current.ShowTerrain = false;
            }
        }

        for (var i = 0; i < GameObjList.Count; i++)
        {
            if (GameObjList[i].Name == "Mountains 1")
            {
                _mountain1 = GameObjList[i] as SpriteObj;
            }

            if (GameObjList[i].Name == "Mountains 2")
            {
                _mountain2 = GameObjList[i] as SpriteObj;
            }
        }

        base.Initialize();
    }

    public override void LoadContent(GraphicsDevice graphics)
    {
        if (_tree1 == null)
        {
            foreach (var current in GameObjList)
                if (current.Name == "Tree1")
                {
                    _tree1 = current;
                }
                else if (current.Name == "Tree2")
                {
                    _tree2 = current;
                }
                else if (current.Name == "Tree3")
                {
                    _tree3 = current;
                }
                else if (current.Name == "Fern1")
                {
                    _fern1 = current;
                }
                else if (current.Name == "Fern2")
                {
                    _fern2 = current;
                }
                else if (current.Name == "Fern3")
                {
                    _fern3 = current;
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

        Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
        _isSnowing = DateTime.Now.Month == 12 || DateTime.Now.Month == 1;
        if (_isSnowing)
        {
            foreach (var current in _rainFg) current.ChangeToSnowflake();
        }

        if (!(Game.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map) &&
            Game.PlayerStats.HasArchitectFee)
        {
            Game.PlayerStats.HasArchitectFee = false;
        }

        Game.PlayerStats.TutorialComplete = true;
        Game.PlayerStats.IsDead = false;
        _lightningTimer = 5f;
        Player.CurrentHealth = Player.MaxHealth;
        Player.CurrentMana = Player.MaxMana;
        Player.ForceInvincible = false;
        (Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData);
        if (TollCollectorAvailable && !Player.AttachedLevel.PhysicsManager.ObjectList.Contains(_tollCollector))
        {
            Player.AttachedLevel.PhysicsManager.AddObject(_tollCollector);
        }

        if (_blacksmithAnvilSound == null)
        {
            _blacksmithAnvilSound = new FrameSoundObj(_blacksmith.GetChildAt(5) as IAnimateableObj, Player, 7,
                "Anvil1", "Anvil2", "Anvil3");
        }

        if (Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f)
        {
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
        }
        else
        {
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }

        _playerWalkedOut = false;
        Player.UpdateCollisionBoxes();
        Player.Position = new Vector2(0f, 660f - (Player.Bounds.Bottom - Player.Y));
        Player.State = 1;
        Player.IsWeighted = false;
        Player.IsCollidable = false;
        var logicSet = new LogicSet(Player);
        logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
        logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
        logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
        logicSet.AddAction(new PlayAnimationLogicAction());
        logicSet.AddAction(new DelayLogicAction(0.5f));
        logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
        logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", true));
        logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", true));
        Player.RunExternalLogicSet(logicSet);
        Tween.By(this, 1f, Linear.EaseNone);
        Tween.AddEndHandlerToLastTween(Player, "UnlockControls");
        SoundManager.StopMusic(1f);
        _isRaining = CDGMath.RandomPlusMinus() > 0;
        _isRaining = true;
        if (_isRaining)
        {
            if (_rainSfx != null)
            {
                _rainSfx.Dispose();
            }

            if (!_isSnowing)
            {
                _rainSfx = SoundManager.PlaySound("Rain1");
            }
            else
            {
                _rainSfx = SoundManager.PlaySound("snowloop_filtered");
            }
        }

        _tent.TextureColor = new Color(200, 200, 200);
        _blacksmithBoard.TextureColor = new Color(200, 200, 200);
        _screw.TextureColor = new Color(200, 200, 200);
        if (Game.PlayerStats.LockCastle)
        {
            _screw.GoToFrame(_screw.TotalFrames);
            _architectBlock.Position = new Vector2(1492f, 579f);
        }
        else
        {
            _screw.GoToFrame(1);
            _architectBlock.Position = new Vector2(1492f, 439f);
        }

        Player.UpdateEquipmentColours();
        base.OnEnter();
    }

    public override void OnExit()
    {
        if (_rainSfx != null && !_rainSfx.IsDisposed)
        {
            _rainSfx.Stop(AudioStopOptions.Immediate);
        }
    }

    public override void Update(GameTime gameTime)
    {
        Player.CurrentMana = Player.MaxMana;
        Player.CurrentHealth = Player.MaxHealth;
        _enchantressBlock.Visible = EnchantressAvailable;
        _blacksmithBlock.Visible = SmithyAvailable;
        _architectBlock.Visible = ArchitectAvailable;
        var totalGameTime = Game.TotalGameTimeSeconds;
        if (!_playerWalkedOut)
        {
            if (!Player.ControlsLocked && Player.X < Bounds.Left)
            {
                _playerWalkedOut = true;
                (Player.AttachedLevel.ScreenManager as RCScreenManager).StartWipeTransition();
                Tween.RunFunction(0.2f, Player.AttachedLevel.ScreenManager, "DisplayScreen", 6, true,
                    typeof(List<object>));
            }
            else if (!Player.ControlsLocked && Player.X > Bounds.Right && !TollCollectorAvailable)
            {
                _playerWalkedOut = true;
                LoadLevel();
            }
        }

        if (_isRaining)
        {
            foreach (var current in TerrainObjList) current.UseCachedValues = true;
            foreach (var current2 in _rainFg) current2.Update(TerrainObjList, gameTime);
        }

        _tree1.Rotation = -(float) Math.Sin(totalGameTime) * 2f;
        _tree2.Rotation = (float) Math.Sin(totalGameTime * 2f);
        _tree3.Rotation = (float) Math.Sin(totalGameTime * 2f) * 2f;
        _fern1.Rotation = (float) Math.Sin(totalGameTime * 3f) / 2f;
        _fern2.Rotation = -(float) Math.Sin(totalGameTime * 4f);
        _fern3.Rotation = (float) Math.Sin(totalGameTime * 4f) / 2f;
        if (!_architectRenovating)
        {
            HandleInput();
        }

        if (SmithyAvailable)
        {
            if (_blacksmithAnvilSound != null)
            {
                _blacksmithAnvilSound.Update();
            }

            _blacksmith.Update(gameTime);
        }

        _blacksmithIcon.Visible = false;
        if (Player != null && CollisionMath.Intersects(Player.TerrainBounds, _blacksmith.Bounds) &&
            Player.IsTouchingGround && SmithyAvailable)
        {
            _blacksmithIcon.Visible = true;
        }

        _blacksmithIcon.Position = new Vector2(_blacksmithIconPosition.X,
            _blacksmithIconPosition.Y - 70f + (float) Math.Sin(totalGameTime * 20f) * 2f);
        _enchantressIcon.Visible = false;
        var b = new Rectangle((int) (_enchantress.X - 100f), (int) _enchantress.Y,
            _enchantress.Bounds.Width + 100, _enchantress.Bounds.Height);
        if (Player != null && CollisionMath.Intersects(Player.TerrainBounds, b) && Player.IsTouchingGround &&
            EnchantressAvailable)
        {
            _enchantressIcon.Visible = true;
        }

        _enchantressIcon.Position = new Vector2(_enchantressIconPosition.X + 20f,
            _enchantressIconPosition.Y + (float) Math.Sin(totalGameTime * 20f) * 2f);
        if (Player != null &&
            CollisionMath.Intersects(Player.TerrainBounds,
                new Rectangle((int) _architect.X - 100, (int) _architect.Y, _architect.Width + 200,
                    _architect.Height)) && Player.X < _architect.X && Player.Flip == SpriteEffects.None &&
            ArchitectAvailable)
        {
            _architectIcon.Visible = true;
        }
        else
        {
            _architectIcon.Visible = false;
        }

        _architectIcon.Position = new Vector2(_architectIconPosition.X,
            _architectIconPosition.Y + (float) Math.Sin(totalGameTime * 20f) * 2f);
        if (Player != null &&
            CollisionMath.Intersects(Player.TerrainBounds,
                new Rectangle((int) _tollCollector.X - 100, (int) _tollCollector.Y, _tollCollector.Width + 200,
                    _tollCollector.Height)) && Player.X < _tollCollector.X && Player.Flip == SpriteEffects.None &&
            TollCollectorAvailable && _tollCollector.SpriteName == "NPCTollCollectorIdle_Character")
        {
            _tollCollectorIcon.Visible = true;
        }
        else
        {
            _tollCollectorIcon.Visible = false;
        }

        _tollCollectorIcon.Position = new Vector2(_tollCollector.X - _tollCollector.Width / 2 - 10f,
            _tollCollector.Y - _tollCollectorIcon.Height - _tollCollector.Height / 2 +
            (float) Math.Sin(totalGameTime * 20f) * 2f);
        _blacksmithNewIcon.Visible = false;
        if (SmithyAvailable)
        {
            if (_blacksmithIcon.Visible && _blacksmithNewIcon.Visible)
            {
                _blacksmithNewIcon.Visible = false;
            }
            else if (!_blacksmithIcon.Visible && BlacksmithNewIconVisible)
            {
                _blacksmithNewIcon.Visible = true;
            }

            _blacksmithNewIcon.Position = new Vector2(_blacksmithIcon.X + 50f, _blacksmithIcon.Y - 30f);
        }

        _enchantressNewIcon.Visible = false;
        if (EnchantressAvailable)
        {
            if (_enchantressIcon.Visible && _enchantressNewIcon.Visible)
            {
                _enchantressNewIcon.Visible = false;
            }
            else if (!_enchantressIcon.Visible && EnchantressNewIconVisible)
            {
                _enchantressNewIcon.Visible = true;
            }

            _enchantressNewIcon.Position = new Vector2(_enchantressIcon.X + 40f, _enchantressIcon.Y - 0f);
        }

        if (_isRaining && !_isSnowing && _lightningTimer > 0f)
        {
            _lightningTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_lightningTimer <= 0f)
            {
                if (CDGMath.RandomInt(0, 100) > 70)
                {
                    if (CDGMath.RandomInt(0, 1) > 0)
                    {
                        Player.AttachedLevel.LightningEffectTwice();
                    }
                    else
                    {
                        Player.AttachedLevel.LightningEffectOnce();
                    }
                }

                _lightningTimer = 5f;
            }
        }

        if (_shakeScreen)
        {
            UpdateShake();
        }

        if (Player.Bounds.Right > _tollCollector.Bounds.Left && TollCollectorAvailable)
        {
            Player.X = _tollCollector.Bounds.Left - (Player.Bounds.Right - Player.X);
            Player.AttachedLevel.UpdateCamera();
        }

        base.Update(gameTime);
    }

    private void LoadLevel()
    {
        Game.ScreenManager.DisplayScreen(5, true);
    }

    private void HandleInput()
    {
        if (!_controlsLocked)
        {
            if (Player.State != 4)
            {
                if (_blacksmithIcon.Visible &&
                    (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                {
                    MovePlayerTo(_blacksmith);
                }

                if (_enchantressIcon.Visible &&
                    (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                {
                    MovePlayerTo(_enchantress);
                }

                if (_architectIcon.Visible &&
                    (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                {
                    var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
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
                                DialogueManager.AddText("Meet Architect AP", new[] { "" },
                                    new[]
                                    {
                                        $"Do you want to lock the castle and get only {100 - RandomizerData.ArchitectFee}% gold?"
                                    });
                                rCScreenManager.DialogueScreen.SetDialogue("Meet Architect AP");
                            }

                            rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "ActivateArchitect");
                            rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine",
                                "Canceling Selection");
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

                    rCScreenManager.DisplayScreen(13, true);
                }
            }

            if (_tollCollectorIcon.Visible &&
                (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
            {
                var rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                if (Game.PlayerStats.SpecialItem == 1)
                {
                    Tween.RunFunction(0.1f, this, "TollPaid", false);
                    rCScreenManager2.DialogueScreen.SetDialogue("Toll Collector Obol");
                    rCScreenManager2.DisplayScreen(13, true);
                    return;
                }

                if (Game.PlayerStats.SpecialItem == 9)
                {
                    rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Eyeball");
                    RunTollPaidSelection(rCScreenManager2);
                    return;
                }

                if (Game.PlayerStats.SpecialItem == 10)
                {
                    rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Skull");
                    RunTollPaidSelection(rCScreenManager2);
                    return;
                }

                if (Game.PlayerStats.SpecialItem == 11)
                {
                    rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Fireball");
                    RunTollPaidSelection(rCScreenManager2);
                    return;
                }

                if (Game.PlayerStats.SpecialItem == 12)
                {
                    rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Blob");
                    RunTollPaidSelection(rCScreenManager2);
                    return;
                }

                if (Game.PlayerStats.SpecialItem == 13)
                {
                    rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Last Boss");
                    RunTollPaidSelection(rCScreenManager2);
                    return;
                }

                if (RandomizerData.DisableCharon)
                {
                    Tween.RunFunction(0.1f, this, "TollPaid", false);
                    DialogueManager.AddText("Disabled Toll Collector",
                        new[] { "Charon" },
                        new[] { "I'm in a good mood today, so I'll let you in for free this time. Besides, you look like you could use the extra money anyway." });
                    rCScreenManager2.DialogueScreen.SetDialogue("Disabled Toll Collector");
                    rCScreenManager2.DisplayScreen((int)ScreenType.Dialogue, true);
                    return;
                }

                if (!Game.PlayerStats.SpokeToTollCollector)
                {
                    rCScreenManager2.DialogueScreen.SetDialogue("Meet Toll Collector 1");
                }
                else
                {
                    var num = SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount * 100f;
                    rCScreenManager2.DialogueScreen.SetDialogue("Meet Toll Collector Skip" +
                                                                (int) Math.Round(num,
                                                                    MidpointRounding.AwayFromZero));
                }

                RunTollPaidSelection(rCScreenManager2);
            }
        }
    }

    private void RunTollPaidSelection(RCScreenManager manager)
    {
        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
        manager.DialogueScreen.SetConfirmEndHandler(this, "TollPaid", true);
        manager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", "Canceling Selection");
        manager.DisplayScreen(13, true);
    }

    public void MovePlayerTo(GameObj target)
    {
        _controlsLocked = true;
        if (Player.X != target.X - 150f)
        {
            if (Player.X > target.Position.X - 150f)
            {
                Player.Flip = SpriteEffects.FlipHorizontally;
            }

            var num = CDGMath.DistanceBetweenPts(Player.Position, new Vector2(target.X - 150f, target.Y)) /
                      Player.Speed;
            Player.UpdateCollisionBoxes();
            Player.State = 1;
            Player.IsWeighted = false;
            Player.AccelerationY = 0f;
            Player.AccelerationX = 0f;
            Player.IsCollidable = false;
            Player.CurrentSpeed = 0f;
            Player.LockControls();
            Player.ChangeSprite("PlayerWalking_Character");
            Player.PlayAnimation();
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new DelayLogicAction(num));
            Player.RunExternalLogicSet(logicSet);
            Tween.To(Player, num, Tween.EaseNone, "X", (target.Position.X - 150f).ToString());
            Tween.AddEndHandlerToLastTween(this, "MovePlayerComplete", target);
            return;
        }

        MovePlayerComplete(target);
    }

    public void MovePlayerComplete(GameObj target)
    {
        _controlsLocked = false;
        Player.IsWeighted = true;
        Player.IsCollidable = true;
        Player.UnlockControls();
        Player.Flip = SpriteEffects.None;
        if (target != _blacksmith)
        {
            if (target == _enchantress)
            {
                if (!Game.PlayerStats.SpokeToEnchantress)
                {
                    Game.PlayerStats.SpokeToEnchantress = true;
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue(
                        "Meet Enchantress");
                    var arg_1A1_0 =
                        (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen;
                    object arg_1A1_1 = Player.AttachedLevel.ScreenManager;
                    var arg_1A1_2 = "DisplayScreen";
                    var array = new object[3];
                    array[0] = 11;
                    array[1] = true;
                    arg_1A1_0.SetConfirmEndHandler(arg_1A1_1, arg_1A1_2, array);
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true);
                    return;
                }

                (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(11, true);
            }

            return;
        }

        if (!Game.PlayerStats.SpokeToBlacksmith)
        {
            Game.PlayerStats.SpokeToBlacksmith = true;
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue("Meet Blacksmith");
            var arg_CA_0 = (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen;
            object arg_CA_1 = Player.AttachedLevel.ScreenManager;
            var arg_CA_2 = "DisplayScreen";
            var array2 = new object[3];
            array2[0] = 10;
            array2[1] = true;
            arg_CA_0.SetConfirmEndHandler(arg_CA_1, arg_CA_2, array2);
            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true);
            return;
        }

        (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(10, true);
    }

    public void TollPaid(bool chargeFee)
    {
        if (RandomizerData.DisableCharon)
            chargeFee = false;

        if (chargeFee)
        {
            var num = Game.PlayerStats.Gold * (1f - SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount);
            Game.PlayerStats.Gold -= (int) num;
            if (num > 0f)
            {
                Player.AttachedLevel.TextManager.DisplayNumberStringText(-(int) num, "gold", Color.Yellow,
                    new Vector2(Player.X, Player.Bounds.Top));
            }
        }

        if (Game.PlayerStats.SpokeToTollCollector && Game.PlayerStats.SpecialItem != 1 &&
            Game.PlayerStats.SpecialItem != 12 && Game.PlayerStats.SpecialItem != 13 &&
            Game.PlayerStats.SpecialItem != 11 && Game.PlayerStats.SpecialItem != 9 &&
            Game.PlayerStats.SpecialItem != 10)
        {
            Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(_tollCollector.Position);
            SoundManager.PlaySound("Charon_Laugh");
            HideTollCollector();
        }
        else
        {
            Game.PlayerStats.SpokeToTollCollector = true;
            SoundManager.PlaySound("Charon_Laugh");
            _tollCollector.ChangeSprite("NPCTollCollectorLaugh_Character");
            _tollCollector.AnimationDelay = 0.05f;
            _tollCollector.PlayAnimation();
            Tween.RunFunction(1f, Player.AttachedLevel.ImpactEffectPool, "DisplayDeathEffect",
                _tollCollector.Position);
            Tween.RunFunction(1f, this, "HideTollCollector");
        }

        if (Game.PlayerStats.SpecialItem == 1 || Game.PlayerStats.SpecialItem == 10 ||
            Game.PlayerStats.SpecialItem == 9 || Game.PlayerStats.SpecialItem == 13 ||
            Game.PlayerStats.SpecialItem == 11 || Game.PlayerStats.SpecialItem == 12)
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
            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
        }
    }

    public void HideTollCollector()
    {
        SoundManager.Play3DSound(this, Player, "Charon_Poof");
        _tollCollector.Visible = false;
        Player.AttachedLevel.PhysicsManager.RemoveObject(_tollCollector);
    }

    public void ActivateArchitect()
    {
        Player.LockControls();
        Player.CurrentSpeed = 0f;
        _architectIcon.Visible = false;
        _architectRenovating = true;
        _architect.ChangeSprite("ArchitectPull_Character");
        (_architect.GetChildAt(1) as SpriteObj).PlayAnimation(false);
        _screw.AnimationDelay = 0.0333333351f;
        Tween.RunFunction(0.5f, _architect.GetChildAt(0), "PlayAnimation", true);
        Tween.RunFunction(0.5f, typeof(SoundManager), "PlaySound", "Architect_Lever");
        Tween.RunFunction(1f, typeof(SoundManager), "PlaySound", "Architect_Screw");
        Tween.RunFunction(1f, _screw, "PlayAnimation", false);
        Tween.By(_architectBlock, 0.8f, Tween.EaseNone, "delay", "1.1", "Y", "135");
        Tween.RunFunction(1f, this, "ShakeScreen", 2, true, false);
        Tween.RunFunction(1.5f, this, "StopScreenShake");
        Tween.RunFunction(1.5f, Player.AttachedLevel.ImpactEffectPool, "SkillTreeDustEffect",
            new Vector2(_screw.X - _screw.Width / 2f, _screw.Y - 40f), true, _screw.Width);
        Tween.RunFunction(3f, this, "StopArchitectActivation");
    }

    public void StopArchitectActivation()
    {
        _architectRenovating = false;
        _architectIcon.Visible = true;
        Player.UnlockControls();
        Game.PlayerStats.LockCastle = true;
        Game.PlayerStats.HasArchitectFee = true;
        foreach (var current in Player.AttachedLevel.ChestList)
        {
            var fairyChestObj = current as FairyChestObj;
            if (fairyChestObj != null && fairyChestObj.State == 2)
            {
                fairyChestObj.ResetChest();
            }
        }

        foreach (var current2 in Player.AttachedLevel.RoomList)
        foreach (var current3 in current2.GameObjList)
        {
            var breakableObj = current3 as BreakableObj;
            if (breakableObj != null)
            {
                breakableObj.Reset();
            }
        }

        var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
        rCScreenManager.DialogueScreen.SetDialogue("Castle Lock Complete Architect");
        rCScreenManager.DisplayScreen(13, true);
    }

    public override void Draw(Camera2D camera)
    {
        _mountain1.X = camera.TopLeftCorner.X * 0.5f;
        _mountain2.X = _mountain1.X + 2640f;
        base.Draw(camera);
        if (_isRaining)
        {
            camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 2640, 720), Color.Black * 0.3f);
        }

        if (_screenShakeCounter > 0f)
        {
            camera.X += CDGMath.RandomPlusMinus();
            camera.Y += CDGMath.RandomPlusMinus();
            _screenShakeCounter -= (float) camera.GameTime.ElapsedGameTime.TotalSeconds;
        }

        if (SmithyAvailable)
        {
            _blacksmithBoard.Draw(camera);
            _blacksmith.Draw(camera);
            _blacksmithIcon.Draw(camera);
        }

        if (EnchantressAvailable)
        {
            _tent.Draw(camera);
            _enchantress.Draw(camera);
            _enchantressIcon.Draw(camera);
        }

        if (ArchitectAvailable)
        {
            _screw.Draw(camera);
            _architect.Draw(camera);
            _architectIcon.Draw(camera);
        }

        if (TollCollectorAvailable)
        {
            _tollCollector.Draw(camera);
            _tollCollectorIcon.Draw(camera);
        }

        _blacksmithNewIcon.Draw(camera);
        _enchantressNewIcon.Draw(camera);
        if (_isRaining)
        {
            foreach (var current in _rainFg) current.Draw(camera);
        }
    }

    public override void PauseRoom()
    {
        foreach (var current in _rainFg) current.PauseAnimation();
        if (_rainSfx != null)
        {
            _rainSfx.Pause();
        }

        _enchantress.PauseAnimation();
        _blacksmith.PauseAnimation();
        _architect.PauseAnimation();
        _tollCollector.PauseAnimation();
        base.PauseRoom();
    }

    public override void UnpauseRoom()
    {
        foreach (var current in _rainFg) current.ResumeAnimation();
        if (_rainSfx != null && _rainSfx.IsPaused)
        {
            _rainSfx.Resume();
        }

        _enchantress.ResumeAnimation();
        _blacksmith.ResumeAnimation();
        _architect.ResumeAnimation();
        _tollCollector.ResumeAnimation();
        base.UnpauseRoom();
    }

    public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
    {
        _shakeStartingPos = Player.AttachedLevel.Camera.Position;
        Player.AttachedLevel.CameraLockedToPlayer = false;
        _screenShakeMagnitude = magnitude;
        _horizontalShake = horizontalShake;
        _verticalShake = verticalShake;
        _shakeScreen = true;
    }

    public void UpdateShake()
    {
        if (_horizontalShake)
        {
            Player.AttachedLevel.Camera.X = _shakeStartingPos.X +
                                            CDGMath.RandomPlusMinus() *
                                            (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
        }

        if (_verticalShake)
        {
            Player.AttachedLevel.Camera.Y = _shakeStartingPos.Y +
                                            CDGMath.RandomPlusMinus() *
                                            (CDGMath.RandomFloat(0f, 1f) * _screenShakeMagnitude);
        }
    }

    public void StopScreenShake()
    {
        Player.AttachedLevel.CameraLockedToPlayer = true;
        _shakeScreen = false;
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
        if (!IsDisposed)
        {
            _blacksmith.Dispose();
            _blacksmith = null;
            _blacksmithIcon.Dispose();
            _blacksmithIcon = null;
            _blacksmithNewIcon.Dispose();
            _blacksmithNewIcon = null;
            _blacksmithBoard.Dispose();
            _blacksmithBoard = null;
            _enchantress.Dispose();
            _enchantress = null;
            _enchantressIcon.Dispose();
            _enchantressIcon = null;
            _enchantressNewIcon.Dispose();
            _enchantressNewIcon = null;
            _tent.Dispose();
            _tent = null;
            _architect.Dispose();
            _architect = null;
            _architectIcon.Dispose();
            _architectIcon = null;
            _screw.Dispose();
            _screw = null;
            if (_blacksmithAnvilSound != null)
            {
                _blacksmithAnvilSound.Dispose();
            }

            _blacksmithAnvilSound = null;
            _tree1 = null;
            _tree2 = null;
            _tree3 = null;
            _fern1 = null;
            _fern2 = null;
            _fern3 = null;
            foreach (var current in _rainFg) current.Dispose();
            _rainFg.Clear();
            _rainFg = null;
            _mountain1 = null;
            _mountain2 = null;
            _tollCollector.Dispose();
            _tollCollector = null;
            _tollCollectorIcon.Dispose();
            _tollCollectorIcon = null;
            _blacksmithBlock = null;
            _enchantressBlock = null;
            _architectBlock = null;
            if (_rainSfx != null)
            {
                _rainSfx.Dispose();
            }

            _rainSfx = null;
            base.Dispose();
        }
    }
}
