using System.Globalization;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class BlobChallengeRoom : ChallengeBossRoomObj
{
    private EnemyObj_Blob _boss1;
    private EnemyObj_Blob _boss2;

    public BlobChallengeRoom()
    {
        m_roomActivityDelay = 0.5f;
    }

    public override bool BossKilled => NumActiveBlobs == 0;

    public int NumActiveBlobs =>
        EnemyList.Count(enemy => enemy.Type == (byte) EnemyType.Blob && !enemy.IsKilled) +
        TempEnemyList.Count(enemy => enemy.Type == (byte) EnemyType.Blob && !enemy.IsKilled);

    public override void Initialize()
    {
        // These should be the only two enemies in the room.
        _boss1 = (EnemyObj_Blob) EnemyList[0];
        _boss2 = (EnemyObj_Blob) EnemyList[1];
        _boss1.SaveToFile = false;
        _boss2.SaveToFile = false;
        Zone = Zone.Dungeon;
        base.Initialize();
    }

    private void SetRoomData()
    {
        _boss1.Name = "Astrodotus";
        _boss1.GetChildAt(0).TextureColor = Color.Green;
        _boss1.GetChildAt(2).TextureColor = Color.LightGreen;
        _boss1.GetChildAt(2).Opacity = 0.8f;
        ((SpriteObj) _boss1.GetChildAt(1)).OutlineColour = Color.Red;
        _boss1.GetChildAt(1).TextureColor = Color.Red;
        _boss2.GetChildAt(0).TextureColor = Color.Red;
        _boss2.GetChildAt(2).TextureColor = Color.LightPink;
        _boss2.GetChildAt(2).Opacity = 0.8f;
        ((SpriteObj) _boss2.GetChildAt(1)).OutlineColour = Color.Black;
        _boss2.GetChildAt(1).TextureColor = Color.DarkGray;
        _boss1.MaxHealth = 64;
        _boss1.Damage = 20;
        _boss1.IsWeighted = false;
        _boss1.TurnSpeed = 0.015f;
        _boss1.Speed = 400f;
        _boss1.IsNeo = true;
        _boss1.ChangeNeoStats(0.8f, 1.06f, 6);
        _boss1.Scale = new Vector2(2f, 2f);
        _boss2.Level = _boss1.Level;
        _boss2.MaxHealth = _boss1.MaxHealth;
        _boss2.Damage = _boss1.Damage;
        _boss2.IsWeighted = _boss1.IsWeighted;
        _boss2.TurnSpeed = 0.01f;
        _boss2.Speed = 625f;
        _boss2.IsNeo = _boss1.IsNeo;
        _boss2.ChangeNeoStats(0.75f, 1.16f, 5);
        _boss2.Scale = _boss1.Scale;

        // Soft-lock prevention.
        Player.CanBeKnockedBack = false;

        if (_boss1 != null)
        {
            _boss1.CurrentHealth = _boss1.MaxHealth;
        }

        if (_boss2 != null)
        {
            _boss2.CurrentHealth = _boss2.MaxHealth;
        }
    }

    public override void OnEnter()
    {
        Player.Flip = SpriteEffects.FlipHorizontally;
        SetRoomData();
        _cutsceneRunning = true;
        SoundManager.StopMusic(0.5f);
        _boss1.AnimationDelay = 0.1f;
        _boss1.ChangeSprite("EnemyBlobBossAir_Character");
        _boss1.PlayAnimation();
        _boss2.AnimationDelay = 0.1f;
        _boss2.ChangeSprite("EnemyBlobBossAir_Character");
        _boss2.PlayAnimation();
        Player.AttachedLevel.UpdateCamera();
        Player.LockControls();
        Player.AttachedLevel.RunCinematicBorders(6f);
        Player.AttachedLevel.CameraLockedToPlayer = false;
        Player.AttachedLevel.Camera.Y = Player.Y;
        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, "Y", _boss1.Y.ToString(CultureInfo.InvariantCulture),
            "X", _boss1.X.ToString(CultureInfo.InvariantCulture));
        Tween.RunFunction(1.2f, this, "DisplayBossTitle", "The Infinite and Beyond", _boss1.Name, "Intro2");
        base.OnEnter();
        _bossChest.ForcedItemType = ItemDropType.TripStatDrop;
    }

    public void Intro2()
    {
        var args = new[]
        {
            "delay", "0.5",
            "X", Bounds.Center.X.ToString(),
            "Y", Bounds.Center.Y.ToString(),
            "Zoom", "0.5"
        };

        Tween.To(Player.AttachedLevel.Camera, 1f, Quad.EaseInOut, args);
        Tween.AddEndHandlerToLastTween(this, "EndCutscene");
    }

    public void EndCutscene()
    {
        _boss1.Rotation = 0f;
        Player.IsWeighted = true;
        SoundManager.PlayMusic("DungeonBoss", false, 1f);
        Player.AttachedLevel.CameraLockedToPlayer = false;
        Player.UnlockControls();
        _cutsceneRunning = false;
    }

    public override void Update(GameTime gameTime)
    {
        // Wrap player position if they go off screen.
        if (Player.Y > Bounds.Bottom)
        {
            Player.Y = Bounds.Top + 20;
        }
        else if (Player.Y < Bounds.Top)
        {
            Player.Y = Bounds.Bottom - 20;
        }

        if (Player.X > Bounds.Right)
        {
            Player.X = Bounds.Left + 20;
        }
        else if (Player.X < Bounds.Left)
        {
            Player.X = Bounds.Right - 20;
        }

        // Wrap enemies position if they go off screen.
        foreach (var blob in Player.AttachedLevel.CurrentRoom.EnemyList)
        {
            if (blob.Y > Bounds.Bottom - 10)
            {
                blob.Y = Bounds.Top + 20;
            }
            else if (blob.Y < Bounds.Top + 10)
            {
                blob.Y = Bounds.Bottom - 20;
            }

            if (blob.X > Bounds.Right - 10)
            {
                blob.X = Bounds.Left + 20;
            }
            else if (blob.X < Bounds.Left + 10)
            {
                blob.X = Bounds.Right - 20;
            }
        }

        foreach (var blob in Player.AttachedLevel.CurrentRoom.TempEnemyList)
        {
            if (blob.Y > Bounds.Bottom - 10)
            {
                blob.Y = Bounds.Top + 20;
            }
            else if (blob.Y < Bounds.Top + 10)
            {
                blob.Y = Bounds.Bottom - 20;
            }

            if (blob.X > Bounds.Right - 10)
            {
                blob.X = Bounds.Left + 20;
            }
            else if (blob.X < Bounds.Left + 10)
            {
                blob.X = Bounds.Right - 20;
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(Camera2D camera)
    {
        base.Draw(camera);
        var position = Player.Position;
        if (Player.X - Player.Width / 2f < X)
        {
            Player.Position = new Vector2(Player.X + Width, Player.Y);
            Player.Draw(camera);
        }
        else if (Player.X + Player.Width / 2f > X + Width)
        {
            Player.Position = new Vector2(Player.X - Width, Player.Y);
            Player.Draw(camera);
        }

        if (Player.Y - Player.Height / 2f < Y)
        {
            Player.Position = new Vector2(Player.X, Player.Y + Height);
            Player.Draw(camera);
        }
        else if (Player.Y + Player.Height / 2f > Y + Height)
        {
            Player.Position = new Vector2(Player.X, Player.Y - Height);
            Player.Draw(camera);
        }

        Player.Position = position;
        foreach (var blob in EnemyList)
        {
            var position2 = blob.Position;
            var pureBounds = blob.PureBounds;
            if (blob.X - blob.Width / 2f < X)
            {
                blob.Position = new Vector2(blob.X + Width, blob.Y);
                blob.Draw(camera);
            }
            else if (blob.X + blob.Width / 2f > X + Width)
            {
                blob.Position = new Vector2(blob.X - Width, blob.Y);
                blob.Draw(camera);
            }

            if (pureBounds.Top < Y)
            {
                blob.Position = new Vector2(blob.X, blob.Y + Height);
                blob.Draw(camera);
            }
            else if (pureBounds.Bottom > Y + Height)
            {
                blob.Position = new Vector2(blob.X, blob.Y - Height);
                blob.Draw(camera);
            }

            blob.Position = position2;
        }

        foreach (var blob in TempEnemyList)
        {
            blob.ForceDraw = true;
            var position3 = blob.Position;
            var pureBounds2 = blob.PureBounds;
            if (blob.X - blob.Width / 2f < X)
            {
                blob.Position = new Vector2(blob.X + Width, blob.Y);
                blob.Draw(camera);
            }
            else if (blob.X + blob.Width / 2f > X + Width)
            {
                blob.Position = new Vector2(blob.X - Width, blob.Y);
                blob.Draw(camera);
            }

            if (pureBounds2.Top < Y)
            {
                blob.Position = new Vector2(blob.X, blob.Y + Height);
                blob.Draw(camera);
            }
            else if (pureBounds2.Bottom > Y + Height)
            {
                blob.Position = new Vector2(blob.X, blob.Y - Height);
                blob.Draw(camera);
            }

            blob.Position = position3;
        }
    }

    public override void OnExit()
    {
        if (!BossKilled)
        {
            foreach (var current in EnemyList)
            {
                current.Reset();
            }
        }

        foreach (var current2 in TempEnemyList)
        {
            current2.KillSilently();
            current2.Dispose();
        }

        TempEnemyList.Clear();
        Player.CanBeKnockedBack = true;
        base.OnExit();
    }

    protected override void SaveCompletionData()
    {
        Game.PlayerStats.BlobBossBeaten = true;
    }

    protected override GameObj CreateCloneInstance()
    {
        return new BlobChallengeRoom();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _boss1 = null;
        _boss2 = null;
        base.Dispose();
    }
}
