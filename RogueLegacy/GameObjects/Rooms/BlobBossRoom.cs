//  RogueLegacyRandomizer - BlobBossRoom.cs
//  Last Modified 2023-10-26 11:30 AM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.GameObjects.Rooms;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class BlobBossRoom : BossRoomObj
{
    private const int NUM_INTRO_BLOBS = 10;

    private List<ObjContainer> _blobArray;
    private EnemyObj_Blob      _boss;
    private float              _desiredBossScale;

    protected override bool BossKilled => ActiveEnemies == 0;

    public int NumActiveBlobs =>
        EnemyList.Count(enemy => enemy.Type == (byte) EnemyType.Blob && !enemy.IsKilled) +
        TempEnemyList.Count(enemy => enemy.Type == (byte) EnemyType.Blob && !enemy.IsKilled);

    public override void Initialize()
    {
        // Should be the only enemy in the room when initialized.
        _boss = (EnemyObj_Blob) EnemyList[0];
        _boss.PauseEnemy(true);
        _boss.DisableAllWeight = false;
        _desiredBossScale = _boss.Scale.X;
        _blobArray = new List<ObjContainer>();
        for (var i = 0; i < NUM_INTRO_BLOBS; i++)
        {
            var objContainer = new ObjContainer("EnemyBlobBossAir_Character")
            {
                Position = _boss.Position,
                Scale = new Vector2(0.4f, 0.4f)
            };

            objContainer.GetChildAt(0).TextureColor = Color.White;
            objContainer.GetChildAt(2).TextureColor = Color.LightSkyBlue;
            objContainer.GetChildAt(2).Opacity = 0.8f;
            ((SpriteObj) objContainer.GetChildAt(1)).OutlineColour = Color.Black;
            objContainer.Y -= 1000f;

            _blobArray.Add(objContainer);
            GameObjList.Add(objContainer);
        }

        base.Initialize();
    }

    public override void OnEnter()
    {
        _boss.Name = "Herodotus";
        _boss.GetChildAt(0).TextureColor = Color.White;
        _boss.GetChildAt(2).TextureColor = Color.LightSkyBlue;
        _boss.GetChildAt(2).Opacity = 0.8f;
        ((SpriteObj) _boss.GetChildAt(1)).OutlineColour = Color.Black;
        _boss.GetChildAt(1).TextureColor = Color.Black;
        SoundManager.StopMusic(0.5f);
        Player.LockControls();
        Player.AttachedLevel.UpdateCamera();
        Player.AttachedLevel.CameraLockedToPlayer = false;
        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", (Bounds.Left + 700).ToString(), "Y",
            _boss.Y.ToString(CultureInfo.InvariantCulture));
        Tween.By(_blobArray[0], 1f, Quad.EaseIn, "delay", "0.5", "Y", "1150");
        Tween.AddEndHandlerToLastTween(this, "GrowBlob", _blobArray[0]);
        Tween.By(_blobArray[1], 1f, Quad.EaseIn, "delay", "1.5", "Y", "1150");
        Tween.AddEndHandlerToLastTween(this, "GrowBlob", _blobArray[1]);
        Tween.RunFunction(1f, this, "DropBlobs");
        _boss.Scale = new Vector2(0.5f, 0.5f);
        Player.AttachedLevel.RunCinematicBorders(9f);
        base.OnEnter();
    }

    public void DropBlobs()
    {
        var num = 1f;
        for (var i = 2; i < _blobArray.Count; i++)
        {
            Tween.By(_blobArray[i], 1f, Quad.EaseIn, "delay", num.ToString(CultureInfo.InvariantCulture), "Y", "1150");
            Tween.AddEndHandlerToLastTween(this, "GrowBlob", _blobArray[i]);
            num += 0.5f * (_blobArray.Count - i) / _blobArray.Count;
        }

        Tween.RunFunction(num + 1f, _boss, "PlayAnimation", true);
        Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", "Boss_Blob_Idle_Loop");
        Tween.RunFunction(num + 1f, this, "DisplayBossTitle", "The Infinite", _boss.Name, "Intro2");
        Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", "Boss_Blob_Spawn");
    }

    public void GrowBlob(GameObj blob)
    {
        var num = (_desiredBossScale - 0.5f) / NUM_INTRO_BLOBS;
        blob.Visible = false;
        _boss.PlayAnimation(false);
        _boss.ScaleX += num;
        _boss.ScaleY += num;
        SoundManager.PlaySound("Boss_Blob_Spawn_01", "Boss_Blob_Spawn_02", "Boss_Blob_Spawn_03");
    }

    public void Intro2()
    {
        _boss.PlayAnimation();
        Tween.To(Player.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, "delay", "0.5", "X",
            (Player.X + GlobalEV.Camera_XOffset).ToString(CultureInfo.InvariantCulture), "Y",
            (Bounds.Bottom - (Player.AttachedLevel.Camera.Bounds.Bottom - Player.AttachedLevel.Camera.Y))
            .ToString(CultureInfo.InvariantCulture));
        Tween.AddEndHandlerToLastTween(this, "BeginBattle");
    }

    public void BeginBattle()
    {
        SoundManager.PlayMusic("DungeonBoss", true, 1f);
        Player.AttachedLevel.CameraLockedToPlayer = true;
        Player.UnlockControls();
        _boss.UnpauseEnemy(true);
        _boss.PlayAnimation();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var enemy in EnemyList)
        {
            if (enemy.Type == (byte) EnemyType.Blob && !enemy.IsKilled &&
                (enemy.X > Bounds.Right - 20 || enemy.X < Bounds.Left + 20 || enemy.Y > Bounds.Bottom - 20 ||
                 enemy.Y < Bounds.Top + 20))
            {
                enemy.Position = new Vector2(Bounds.Center.X, Bounds.Center.Y);
            }
        }

        foreach (var enemy in TempEnemyList)
        {
            if (enemy.Type == (byte) EnemyType.Blob && !enemy.IsKilled &&
                (enemy.X > Bounds.Right - 20 || enemy.X < Bounds.Left + 20 ||
                 enemy.Y > Bounds.Bottom - 20 ||
                 enemy.Y < Bounds.Top + 20))
            {
                enemy.Position = new Vector2(Bounds.Center.X, Bounds.Center.Y);
            }
        }

        base.Update(gameTime);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _blobArray.Clear();
        _blobArray = null;
        _boss = null;
        base.Dispose();
    }
}
