//
//  Rogue Legacy Randomizer - BlobBossRoom.cs
//  Last Modified 2022-01-23
//
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class BlobBossRoom : BossRoomObj
    {
        private readonly int _numIntroBlobs = 10;
        private List<ObjContainer> _blobArray;
        private EnemyObj_Blob _boss1;
        private float _desiredBossScale;

        public override bool BossKilled => ActiveEnemies == 0;

        public int NumActiveBlobs
        {
            get
            {
                var num = 0;
                foreach (var current in EnemyList)
                    if (current.Type == 2 && !current.IsKilled)
                    {
                        num++;
                    }

                foreach (var current2 in TempEnemyList)
                    if (current2.Type == 2 && !current2.IsKilled)
                    {
                        num++;
                    }

                return num;
            }
        }

        public override void Initialize()
        {
            _boss1 = EnemyList[0] as EnemyObj_Blob;
            _boss1.PauseEnemy(true);
            _boss1.DisableAllWeight = false;
            _desiredBossScale = _boss1.Scale.X;
            _blobArray = new List<ObjContainer>();
            for (var i = 0; i < _numIntroBlobs; i++)
            {
                var objContainer = new ObjContainer("EnemyBlobBossAir_Character");
                objContainer.Position = _boss1.Position;
                objContainer.Scale = new Vector2(0.4f, 0.4f);
                objContainer.GetChildAt(0).TextureColor = Color.White;
                objContainer.GetChildAt(2).TextureColor = Color.LightSkyBlue;
                objContainer.GetChildAt(2).Opacity = 0.8f;
                (objContainer.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
                objContainer.Y -= 1000f;
                _blobArray.Add(objContainer);
                GameObjList.Add(objContainer);
            }

            base.Initialize();
        }

        public override void OnEnter()
        {
            _boss1.Name = "Herodotus";
            _boss1.GetChildAt(0).TextureColor = Color.White;
            _boss1.GetChildAt(2).TextureColor = Color.LightSkyBlue;
            _boss1.GetChildAt(2).Opacity = 0.8f;
            (_boss1.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
            _boss1.GetChildAt(1).TextureColor = Color.Black;
            SoundManager.StopMusic(0.5f);
            Player.LockControls();
            Player.AttachedLevel.UpdateCamera();
            Player.AttachedLevel.CameraLockedToPlayer = false;
            Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "X", (Bounds.Left + 700).ToString(), "Y",
                _boss1.Y.ToString());
            Tween.By(_blobArray[0], 1f, Quad.EaseIn, "delay", "0.5", "Y", "1150");
            Tween.AddEndHandlerToLastTween(this, "GrowBlob", _blobArray[0]);
            Tween.By(_blobArray[1], 1f, Quad.EaseIn, "delay", "1.5", "Y", "1150");
            Tween.AddEndHandlerToLastTween(this, "GrowBlob", _blobArray[1]);
            Tween.RunFunction(1f, this, "DropBlobs");
            _boss1.Scale = new Vector2(0.5f, 0.5f);
            Player.AttachedLevel.RunCinematicBorders(9f);
            base.OnEnter();
        }

        public void DropBlobs()
        {
            var num = 1f;
            for (var i = 2; i < _blobArray.Count; i++)
            {
                Tween.By(_blobArray[i], 1f, Quad.EaseIn, "delay", num.ToString(), "Y", "1150");
                Tween.AddEndHandlerToLastTween(this, "GrowBlob", _blobArray[i]);
                num += 0.5f * (_blobArray.Count - i) / _blobArray.Count;
            }

            Tween.RunFunction(num + 1f, _boss1, "PlayAnimation", true);
            Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", "Boss_Blob_Idle_Loop");
            Tween.RunFunction(num + 1f, this, "DisplayBossTitle", "The Infinite", _boss1.Name, "Intro2");
            Tween.RunFunction(num + 1f, typeof(SoundManager), "PlaySound", "Boss_Blob_Spawn");
        }

        public void GrowBlob(GameObj blob)
        {
            var num = (_desiredBossScale - 0.5f) / _numIntroBlobs;
            blob.Visible = false;
            _boss1.PlayAnimation(false);
            _boss1.ScaleX += num;
            _boss1.ScaleY += num;
            SoundManager.PlaySound("Boss_Blob_Spawn_01", "Boss_Blob_Spawn_02", "Boss_Blob_Spawn_03");
        }

        public void Intro2()
        {
            _boss1.PlayAnimation();
            Tween.To(Player.AttachedLevel.Camera, 0.5f, Quad.EaseInOut, "delay", "0.5", "X",
                (Player.X + GlobalEV.Camera_XOffset).ToString(), "Y",
                (Bounds.Bottom - (Player.AttachedLevel.Camera.Bounds.Bottom - Player.AttachedLevel.Camera.Y))
                .ToString());
            Tween.AddEndHandlerToLastTween(this, "BeginBattle");
        }

        public void BeginBattle()
        {
            SoundManager.PlayMusic("DungeonBoss", true, 1f);
            Player.AttachedLevel.CameraLockedToPlayer = true;
            Player.UnlockControls();
            _boss1.UnpauseEnemy(true);
            _boss1.PlayAnimation();
        }

        public override void Update(GameTime gameTime)
        {
            var bounds = Bounds;
            foreach (var current in EnemyList)
                if (current.Type == 2 && !current.IsKilled &&
                    (current.X > Bounds.Right - 20 || current.X < Bounds.Left + 20 || current.Y > Bounds.Bottom - 20 ||
                     current.Y < Bounds.Top + 20))
                {
                    current.Position = new Vector2(bounds.Center.X, bounds.Center.Y);
                }

            foreach (var current2 in TempEnemyList)
                if (current2.Type == 2 && !current2.IsKilled &&
                    (current2.X > Bounds.Right - 20 || current2.X < Bounds.Left + 20 ||
                     current2.Y > Bounds.Bottom - 20 ||
                     current2.Y < Bounds.Top + 20))
                {
                    current2.Position = new Vector2(bounds.Center.X, bounds.Center.Y);
                }

            base.Update(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _blobArray.Clear();
                _blobArray = null;
                _boss1 = null;
                base.Dispose();
            }
        }
    }
}
