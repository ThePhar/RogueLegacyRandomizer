// RogueLegacyRandomizer - CastleEntranceRoomObj.cs
// Last Modified 2023-07-30 12:35 PM by
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using Randomizer.Definitions;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class CastleEntranceRoomObj : RoomObj
{
    private bool          _allFilesSaved;
    private DoorObj       _bossDoor;
    private bool          _bossDoorOpening;
    private ObjContainer  _bossDoorSprite;
    private PhysicsObj    _castleGate;
    private SpriteObj     _diary;
    private bool          _gateClosed;
    private KeyIconObj    _mapIcon;
    private TextObj       _mapText;
    private SpriteObj     _speechBubble;
    private TeleporterObj _teleporter;

    public CastleEntranceRoomObj()
    {
        _castleGate = new PhysicsObj("CastleEntranceGate_Sprite");
        _castleGate.IsWeighted = false;
        _castleGate.IsCollidable = true;
        _castleGate.CollisionTypeTag = 1;
        _castleGate.Layer = -1f;
        _castleGate.OutlineWidth = 2;
        GameObjList.Add(_castleGate);
        _teleporter = new TeleporterObj();
        GameObjList.Add(_teleporter);
    }

    public bool RoomCompleted { get; set; }

    public override void Initialize()
    {
        _speechBubble = new SpriteObj("ExclamationSquare_Sprite");
        _speechBubble.Flip = SpriteEffects.FlipHorizontally;
        _speechBubble.Scale = new Vector2(1.2f, 1.2f);
        GameObjList.Add(_speechBubble);
        _mapText = new KeyIconTextObj(Game.JunicodeFont);
        _mapText.Text = "view map any time";
        _mapText.Align = Types.TextAlign.Centre;
        _mapText.FontSize = 12f;
        _mapText.OutlineWidth = 2;
        GameObjList.Add(_mapText);
        _mapIcon = new KeyIconObj();
        _mapIcon.Scale = new Vector2(0.5f, 0.5f);
        GameObjList.Add(_mapIcon);
        foreach (var current in GameObjList)
        {
            if (current.Name == "diary") _diary = current as SpriteObj;

            if (current.Name == "map")
            {
                (current as SpriteObj).OutlineWidth = 2;
                _mapText.Position = new Vector2(current.X, current.Bounds.Top - 50);
                _mapIcon.Position = new Vector2(_mapText.X, _mapText.Y - 20f);
            }
        }

        _diary.OutlineWidth = 2;
        _speechBubble.Position = new Vector2(_diary.X, _diary.Y - _speechBubble.Height - 20f);
        DoorObj doorObj = null;
        foreach (var current2 in GameObjList)
            if (current2.Name == "LastDoor")
            {
                _bossDoorSprite = current2 as ObjContainer;
                break;
            }

        foreach (var current3 in DoorList)
        {
            if (current3.DoorPosition == "Left") doorObj = current3;

            if (current3.IsBossDoor)
            {
                _bossDoor = current3;
                _bossDoor.Locked = true;
            }
        }

        for (var i = 1; i < _bossDoorSprite.NumChildren; i++) _bossDoorSprite.GetChildAt(i).Opacity = 0f;

        _bossDoorSprite.AnimationDelay = 0.1f;
        _castleGate.Position = new Vector2(doorObj.Bounds.Right - _castleGate.Width,
            doorObj.Y - _castleGate.Height);
        _teleporter.Position = new Vector2(X + Width / 2f - 600f, Y + 720f - 120f);
        base.Initialize();
    }

    public void RevealSymbol(Zone zone, bool tween)
    {
        var flag = false;
        int index;
        switch (zone)
        {
            case Zone.Castle:
                index = 1;
                if (Game.PlayerStats.ChallengeEyeballBeaten) flag = true;

                break;

            case Zone.Garden:
                index = 3;
                if (Game.PlayerStats.ChallengeSkullBeaten) flag = true;

                break;

            case Zone.Dungeon:
                index = 4;
                if (Game.PlayerStats.ChallengeBlobBeaten) flag = true;

                break;

            case Zone.Tower:
                index = 2;
                if (Game.PlayerStats.ChallengeFireballBeaten) flag = true;

                break;

            default:
                index = 5;
                if (Game.PlayerStats.ChallengeLastBossBeaten) flag = true;

                break;
        }

        if (flag)
            _bossDoorSprite.GetChildAt(index).TextureColor = Color.Yellow;
        else
            _bossDoorSprite.GetChildAt(index).TextureColor = Color.White;

        if (tween)
        {
            _bossDoorSprite.GetChildAt(index).Opacity = 0f;
            Tween.To(_bossDoorSprite.GetChildAt(index), 0.5f, Quad.EaseInOut, "delay", "1.5", "Opacity", "1");
            return;
        }

        _bossDoorSprite.GetChildAt(index).Opacity = 1f;
    }

    public override void OnEnter()
    {
        _bossDoorOpening = false;
        if (Game.PlayerStats.ReadLastDiary && LinkedRoom.LinkedRoom != null) LinkedRoom = LinkedRoom.LinkedRoom;

        Game.PlayerStats.LoadStartingRoom = false;
        if (Game.PlayerStats.DiaryEntry < 1)
            _speechBubble.Visible = true;
        else
            _speechBubble.Visible = false;

        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            _mapIcon.SetButton(Game.GlobalInput.ButtonList[9]);
            _mapIcon.Scale = new Vector2(1f, 1f);
        }
        else
        {
            _mapIcon.SetKey(Game.GlobalInput.KeyList[9]);
            _mapIcon.Scale = new Vector2(0.5f, 0.5f);
        }

        if (!_allFilesSaved)
        {
            Player.Game.SaveManager.SaveAllFileTypes(false);
            _allFilesSaved = true;
        }

        if (Game.PlayerStats.EyeballBossBeaten) RevealSymbol(Zone.Castle, false);

        if (Game.PlayerStats.FairyBossBeaten) RevealSymbol(Zone.Garden, false);

        if (Game.PlayerStats.BlobBossBeaten) RevealSymbol(Zone.Dungeon, false);

        if (Game.PlayerStats.FireballBossBeaten) RevealSymbol(Zone.Tower, false);

        var hasFountainRequirement = ArchipelagoManager.RandomizerData.FountainPieceRequirement == 0 || Game.PlayerStats.FountainPieces >= ArchipelagoManager.RandomizerData.FountainPieceRequirement;
        var bossesKilled = !ArchipelagoManager.RandomizerData.RequireBosses ||
                           (Game.PlayerStats.EyeballBossBeaten &&
                            Game.PlayerStats.FairyBossBeaten &&
                            Game.PlayerStats.BlobBossBeaten &&
                            Game.PlayerStats.FireballBossBeaten);

        if (bossesKilled && hasFountainRequirement && !Game.PlayerStats.FinalDoorOpened && Player.ScaleX > 0.1f)
        {
            PlayBossDoorAnimation();
        }
        else if (Game.PlayerStats.FinalDoorOpened)
        {
            _bossDoor.Locked = false;
            _bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
            _bossDoorSprite.GoToFrame(_bossDoorSprite.TotalFrames);
        }

        if (!_gateClosed) CloseGate(true);

        if (bossesKilled && hasFountainRequirement && !Game.PlayerStats.FinalDoorOpened && Player.ScaleX > 0.1f)
        {
            Game.PlayerStats.FinalDoorOpened = true;
            Player.AttachedLevel.RunCinematicBorders(6f);
        }

        // Enable death links now that we are in the castle.
        ArchipelagoManager.DeathLinkSafe = true;
        base.OnEnter();
    }

    public void PlayBossDoorAnimation()
    {
        Player.StopDash();
        _bossDoorOpening = true;
        _bossDoor.Locked = false;
        Player.AttachedLevel.UpdateCamera();
        RevealSymbol(Zone.None, true);
        Player.CurrentSpeed = 0f;
        Player.LockControls();
        Player.AttachedLevel.CameraLockedToPlayer = false;
        var x = Player.AttachedLevel.Camera.X;
        object arg_C7_0 = Player.AttachedLevel.Camera;
        var arg_C7_1 = 1f;
        Easing arg_C7_2 = Quad.EaseInOut;
        var array = new string[2];
        array[0] = "X";
        var arg_C5_0 = array;
        var arg_C5_1 = 1;
        var x2 = Bounds.Center.X;
        arg_C5_0[arg_C5_1] = x2.ToString();
        Tween.To(arg_C7_0, arg_C7_1, arg_C7_2, array);
        Tween.RunFunction(2.2f, this, "PlayBossDoorAnimation2", x);
    }

    public void PlayBossDoorAnimation2(float storedX)
    {
        _bossDoorSprite.ChangeSprite("LastDoorOpen_Character");
        _bossDoorSprite.PlayAnimation(false);
        SoundManager.PlaySound("LastDoor_Open");
        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "delay", "2", "X", storedX.ToString());
        Tween.RunFunction(3.1f, this, "BossDoorAnimationComplete");
    }

    public void BossDoorAnimationComplete()
    {
        _bossDoorOpening = false;
        Player.UnlockControls();
        Player.AttachedLevel.CameraLockedToPlayer = true;
    }

    public void ForceGateClosed()
    {
        _castleGate.Y += _castleGate.Height;
        _gateClosed = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (_bossDoorOpening && !Player.ControlsLocked) Player.LockControls();

        if (!SoundManager.IsMusicPlaying) SoundManager.PlayMusic("CastleSong", true);

        if (Player.X < _castleGate.Bounds.Right)
        {
            Player.X = _castleGate.Bounds.Right + 20;
            Player.AttachedLevel.UpdateCamera();
        }

        // Diary Logic
        var bounds = _diary.Bounds;
        bounds.X -= 50;
        bounds.Width += 100;
        _speechBubble.Y = _diary.Y - _speechBubble.Height - 20f - 30f +
                          (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 2f;

        if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround)
        {
            if (_speechBubble.SpriteName == "ExclamationSquare_Sprite")
                _speechBubble.ChangeSprite("UpArrowSquare_Sprite");
        }
        else if (_speechBubble.SpriteName == "UpArrowSquare_Sprite")
        {
            _speechBubble.ChangeSprite("ExclamationSquare_Sprite");
        }

        var freeDiary = ArchipelagoManager.RandomizerData.FreeDiaryOnGeneration;

        if ((!RoomCompleted && freeDiary) || (Game.PlayerStats.DiaryEntry < 1 && !freeDiary) ||
            CollisionMath.Intersects(Player.Bounds, bounds))
            _speechBubble.Visible = true;
        else if (RoomCompleted && freeDiary &&
                 !CollisionMath.Intersects(Player.Bounds, bounds))
            _speechBubble.Visible = false;
        else if (Game.PlayerStats.DiaryEntry >= 1 && !freeDiary)
            _speechBubble.Visible = false;

        if (Game.PlayerStats.DiaryEntry >= 25) _speechBubble.Visible = false;

        if (CollisionMath.Intersects(Player.Bounds, bounds) && Player.IsTouchingGround && InputTypeHelper.PressedUp)
        {
            if ((!RoomCompleted && Game.PlayerStats.DiaryEntry < 25 && freeDiary) || Game.PlayerStats.DiaryEntry < 1)
            {
                // We're going to set diaries based on lower ones completed.
                Game.PlayerStats.DiaryEntry = 0;

                for (var diary = 0; diary < LocationCode.MAX_DIARIES; diary++)
                {
                    var location = LocationCode.STARTING_DIARY + diary;

                    // Check if we already checked this location and try to get the next item in the sequence if so.
                    if (ArchipelagoManager.IsLocationChecked(location))
                    {
                        continue;
                    }

                    Game.PlayerStats.DiaryEntry = (byte) (diary + 1);
                    Program.Game.CollectItemFromLocation(location);
                    RoomCompleted = true;
                    break;
                }
            }
            else
            {
                RoomCompleted = true;
                var rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                rCScreenManager2.DisplayScreen(20, true);
            }
        }

        base.Update(gameTime);
    }

    public void CloseGate(bool animate)
    {
        if (animate)
        {
            Player.Y = 381f;
            Player.X += 10f;
            Player.State = 1;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
            logicSet.AddAction(new PlayAnimationLogicAction());
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 200));
            logicSet.AddAction(new DelayLogicAction(0.2f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
            Player.RunExternalLogicSet(logicSet);
            Tween.By(_castleGate, 1.5f, Quad.EaseOut, "Y", _castleGate.Height.ToString());
            Tween.AddEndHandlerToLastTween(Player, "UnlockControls");
            Player.AttachedLevel.RunCinematicBorders(1.5f);
        }
        else
        {
            _castleGate.Y += _castleGate.Height;
        }

        _gateClosed = true;
    }

    public override void Reset()
    {
        if (_gateClosed)
        {
            _castleGate.Y -= _castleGate.Height;
            _gateClosed = false;
        }

        base.Reset();
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            _castleGate = null;
            _teleporter = null;
            _bossDoor = null;
            _bossDoorSprite = null;
            _diary = null;
            _speechBubble = null;
            _mapText = null;
            _mapIcon = null;
            base.Dispose();
        }
    }

    protected override GameObj CreateCloneInstance()
    {
        return new CastleEntranceRoomObj();
    }

    protected override void FillCloneInstance(object obj)
    {
        base.FillCloneInstance(obj);
    }
}