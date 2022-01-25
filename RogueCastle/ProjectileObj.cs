// 
//  Rogue Legacy Randomizer - ProjectileObj.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Screens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class ProjectileObj : PhysicsObj, IDealsDamageObj
    {
        public float LifeSpan;
        private Color m_blinkColour = Color.White;
        private float m_blinkTimer;
        private float m_elapsedLifeSpan;

        public ProjectileObj(string spriteName) : base(spriteName)
        {
            CollisionTypeTag = 3;
            CollidesWithTerrain = true;
            ChaseTarget = false;
            IsDying = false;
            DestroysWithEnemy = true;
            DestroyOnRoomTransition = true;
        }

        public float RotationSpeed { get; set; }
        public bool IsAlive { get; internal set; }
        public bool CollidesWithTerrain { get; set; }
        public bool DestroysWithTerrain { get; set; }
        public bool DestroysWithEnemy { get; set; }
        public GameObj Target { get; set; }
        public bool ChaseTarget { get; set; }
        public bool FollowArc { get; set; }
        public ProjectileIconObj AttachedIcon { get; set; }
        public bool ShowIcon { get; set; }
        public bool IgnoreBoundsCheck { get; set; }
        public bool CollidesWith1Ways { get; set; }
        public bool GamePaused { get; set; }
        public bool DestroyOnRoomTransition { get; set; }
        public bool CanBeFusRohDahed { get; set; }
        public bool IgnoreInvincibleCounter { get; set; }
        public bool IsDying { get; internal set; }
        public int Spell { get; set; }
        public float AltX { get; set; }
        public float AltY { get; set; }
        public float BlinkTime { get; set; }
        public GameObj Source { get; set; }
        public bool WrapProjectile { get; set; }

        public bool IsDemented
        {
            get
            {
                var enemyObj = Source as EnemyObj;
                return enemyObj != null && enemyObj.IsDemented;
            }
        }

        public int Damage { get; set; }

        public void Reset()
        {
            Source = null;
            CollidesWithTerrain = true;
            DestroysWithTerrain = true;
            DestroysWithEnemy = true;
            IsCollidable = true;
            IsWeighted = true;
            IsDying = false;
            IsAlive = true;
            m_elapsedLifeSpan = 0f;
            Rotation = 0f;
            TextureColor = Color.White;
            Spell = 0;
            AltY = 0f;
            AltX = 0f;
            BlinkTime = 0f;
            IgnoreBoundsCheck = false;
            Scale = Vector2.One;
            DisableHitboxUpdating = false;
            m_blinkColour = Color.White;
            m_blinkTimer = 0f;
            AccelerationYEnabled = true;
            AccelerationXEnabled = true;
            GamePaused = false;
            DestroyOnRoomTransition = true;
            CanBeFusRohDahed = true;
            Flip = SpriteEffects.None;
            IgnoreInvincibleCounter = false;
            WrapProjectile = false;
            DisableCollisionBoxRotations = true;
            Tween.StopAllContaining(this, false);
        }

        public void UpdateHeading()
        {
            if (ChaseTarget && Target != null)
            {
                var position = Target.Position;
                TurnToFace(position, TurnSpeed, 0.0166666675f);
                HeadingX = (float) Math.Cos(Orientation);
                HeadingY = (float) Math.Sin(Orientation);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPaused)
            {
                var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
                var spell = Spell;
                switch (spell)
                {
                    case 3:
                        if (BlinkTime >= AltX && AltX != 0f)
                        {
                            Blink(Color.Red, 0.1f);
                            BlinkTime = AltX / 1.5f;
                        }

                        if (AltX > 0f)
                        {
                            AltX -= num;
                            if (AltX <= 0f)
                            {
                                ActivateEffect();
                            }
                        }

                        break;

                    case 4:
                        break;

                    case 5:
                        if (AltY > 0f)
                        {
                            AltY -= num;
                            if (AltY <= 0f)
                            {
                                var proceduralLevelScreen =
                                    Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
                                if (proceduralLevelScreen != null)
                                {
                                    proceduralLevelScreen.ImpactEffectPool.CrowSmokeEffect(Position);
                                    AltY = 0.05f;
                                }
                            }
                        }

                        if (AltX <= 0f)
                        {
                            var position = Target.Position;
                            TurnToFace(position, TurnSpeed, num);
                        }
                        else
                        {
                            AltX -= num;
                            Orientation = MathHelper.WrapAngle(Orientation);
                        }

                        HeadingX = (float) Math.Cos(Orientation);
                        HeadingY = (float) Math.Sin(Orientation);
                        AccelerationX = 0f;
                        AccelerationY = 0f;
                        Position += Heading * (CurrentSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds);
                        if (HeadingX > 0f)
                        {
                            Flip = SpriteEffects.None;
                            Rotation = MathHelper.ToDegrees(Orientation);
                        }
                        else
                        {
                            Flip = SpriteEffects.FlipHorizontally;
                            var num2 = MathHelper.ToDegrees(Orientation);
                            if (num2 < 0f)
                            {
                                Rotation = (180f + num2) * 60f * num;
                            }
                            else
                            {
                                Rotation = (-180f + num2) * 60f * num;
                            }
                        }

                        Rotation = MathHelper.Clamp(Rotation, -90f, 90f);
                        if (Target != null)
                        {
                            var enemyObj = Target as EnemyObj;
                            if (enemyObj != null && enemyObj.IsKilled)
                            {
                                RunDestroyAnimation(false);
                            }
                        }
                        else
                        {
                            RunDestroyAnimation(false);
                        }

                        break;

                    default:
                        switch (spell)
                        {
                            case 8:
                                AccelerationX -= AltX * 60f * num;
                                if (AltY > 0f)
                                {
                                    AltY -= num;
                                    if (AltY <= 0f)
                                    {
                                        ActivateEffect();
                                    }
                                }

                                break;

                            case 9:
                            case 10:
                                break;

                            case 11:
                            {
                                var player = Game.ScreenManager.Player;
                                if (player.CastingDamageShield || Source is EnemyObj)
                                {
                                    AltX += CurrentSpeed * 60f * num;
                                    Position = CDGMath.GetCirclePosition(AltX, AltY, Target.Position);
                                }
                                else
                                {
                                    KillProjectile();
                                }

                                break;
                            }

                            case 12:
                                AccelerationX = 0f;
                                AccelerationY = 0f;
                                HeadingX = (float) Math.Cos(Orientation);
                                HeadingY = (float) Math.Sin(Orientation);
                                Position += Heading * (CurrentSpeed * num);
                                if (AltY > 0f)
                                {
                                    AltY -= num;
                                    if (AltY <= 0f)
                                    {
                                        ActivateEffect();
                                    }
                                }

                                break;

                            default:
                                if (spell == 100)
                                {
                                    if (AltX > 0f)
                                    {
                                        AltX -= num;
                                        Opacity = 0.9f - AltX;
                                        ScaleY = 1f - AltX;
                                        if (AltX <= 0f)
                                        {
                                            ActivateEffect();
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                }

                if (ChaseTarget && Target != null)
                {
                    var position2 = Target.Position;
                    TurnToFace(position2, TurnSpeed, num);
                    HeadingX = (float) Math.Cos(Orientation);
                    HeadingY = (float) Math.Sin(Orientation);
                    AccelerationX = 0f;
                    AccelerationY = 0f;
                    Position += Heading * (CurrentSpeed * num);
                    Rotation = MathHelper.ToDegrees(Orientation);
                }

                if (FollowArc && !ChaseTarget && !IsDying)
                {
                    var radians = (float) Math.Atan2(AccelerationY, AccelerationX);
                    Rotation = MathHelper.ToDegrees(radians);
                }
                else if (!ChaseTarget)
                {
                    Rotation += RotationSpeed * 60f * num;
                }

                m_elapsedLifeSpan += num;
                if (m_elapsedLifeSpan >= LifeSpan)
                {
                    IsAlive = false;
                }

                if (m_blinkTimer > 0f)
                {
                    m_blinkTimer -= num;
                    TextureColor = m_blinkColour;
                    return;
                }

                if (TextureColor == m_blinkColour)
                {
                    TextureColor = Color.White;
                }
            }
        }

        public void Blink(Color blinkColour, float duration)
        {
            m_blinkColour = blinkColour;
            m_blinkTimer = duration;
        }

        private void TurnToFace(Vector2 facePosition, float turnSpeed, float elapsedSeconds)
        {
            var num = facePosition.X - Position.X;
            var num2 = facePosition.Y - Position.Y;
            var num3 = (float) Math.Atan2(num2, num);
            var num4 = MathHelper.WrapAngle(num3 - Orientation);
            var num5 = turnSpeed * 60f * elapsedSeconds;
            num4 = MathHelper.Clamp(num4, -num5, num5);
            Orientation = MathHelper.WrapAngle(Orientation + num4);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (Spell == 20)
            {
                var projectileObj = otherBox.AbsParent as ProjectileObj;
                if (projectileObj != null && projectileObj.CollisionTypeTag != 2 && projectileObj.CanBeFusRohDahed)
                {
                    projectileObj.RunDestroyAnimation(false);
                }
            }

            var terrainObj = otherBox.Parent as TerrainObj;
            if (CollidesWithTerrain && !(otherBox.Parent is DoorObj) && terrainObj != null &&
                (terrainObj.CollidesTop && terrainObj.CollidesBottom && terrainObj.CollidesLeft &&
                    terrainObj.CollidesRight || CollidesWith1Ways))
            {
                var spell = Spell;
                if (spell != 3)
                {
                    if (spell == 7)
                    {
                        base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                        IsWeighted = false;
                        ActivateEffect();
                        return;
                    }

                    if (spell != 12)
                    {
                        if (DestroysWithTerrain)
                        {
                            RunDestroyAnimation(false);
                            return;
                        }

                        AccelerationY = 0f;
                        AccelerationX = 0f;
                        IsWeighted = false;
                    }
                    else
                    {
                        var value = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation,
                            Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                        if (value != Vector2.Zero)
                        {
                            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Spike_Bounce_01",
                                "Spike_Bounce_02", "Spike_Bounce_03");
                            var heading = Heading;
                            var vector = new Vector2(value.Y, value.X * -1f);
                            var pt = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) *
                                vector - heading;
                            X += value.X;
                            Y += value.Y;
                            Orientation = MathHelper.ToRadians(CDGMath.VectorToAngle(pt));
                        }
                    }
                }
                else if (terrainObj.CollidesBottom && terrainObj.CollidesTop && terrainObj.CollidesLeft &&
                         terrainObj.CollidesRight)
                {
                    var vector2 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation,
                        Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                    if (vector2.Y <= 0f && vector2.X == 0f || otherBox.AbsRotation != 0f)
                    {
                        AccelerationY = 0f;
                        AccelerationX = 0f;
                        IsWeighted = false;
                    }
                }
                else if (!terrainObj.CollidesBottom && terrainObj.CollidesTop && !terrainObj.CollidesLeft &&
                         !terrainObj.CollidesRight)
                {
                    var vector3 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect,
                        thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
                    if (vector3.Y <= 0f && AccelerationY > 0f)
                    {
                        base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                        if (vector3.Y <= 0f && vector3.X == 0f || otherBox.AbsRotation != 0f)
                        {
                            AccelerationY = 0f;
                            AccelerationX = 0f;
                            IsWeighted = false;
                        }
                    }
                }
            }
            else if (otherBox.Type != 0)
            {
                var spell2 = Spell;
                if (spell2 == 5)
                {
                    if (otherBox.AbsParent == Target)
                    {
                        CollisionTypeTag = 2;
                    }
                }
                else
                {
                    base.CollisionResponse(thisBox, otherBox, collisionResponseType);
                }
            }
        }

        public void RunDisplacerEffect(RoomObj room, PlayerObj player)
        {
            var num = 2147483647;
            TerrainObj terrainObj = null;
            var value = Vector2.Zero;
            foreach (var current in room.TerrainObjList)
            {
                value = Vector2.Zero;
                var num2 = 3.40282347E+38f;
                if (player.Flip == SpriteEffects.None)
                {
                    if (current.X > X && current.Bounds.Top < Bounds.Bottom && current.Bounds.Bottom > Bounds.Top)
                    {
                        if (current.Rotation < 0f)
                        {
                            value = CollisionMath.LineToLineIntersect(Position, new Vector2(X + 6600f, Y),
                                CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero),
                                CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation,
                                    Vector2.Zero));
                        }
                        else if (current.Rotation > 0f)
                        {
                            value = CollisionMath.LineToLineIntersect(Position, new Vector2(X + 6600f, Y),
                                CollisionMath.LowerLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero),
                                CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation,
                                    Vector2.Zero));
                        }

                        if (value != Vector2.Zero)
                        {
                            num2 = value.X - X;
                        }
                        else
                        {
                            num2 = current.Bounds.Left - Bounds.Right;
                        }
                    }
                }
                else if (current.X < X && current.Bounds.Top < Bounds.Bottom && current.Bounds.Bottom > Bounds.Top)
                {
                    if (current.Rotation < 0f)
                    {
                        value = CollisionMath.LineToLineIntersect(new Vector2(X - 6600f, Y), Position,
                            CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero),
                            CollisionMath.LowerRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                    }
                    else if (current.Rotation > 0f)
                    {
                        value = CollisionMath.LineToLineIntersect(new Vector2(X - 6600f, Y), Position,
                            CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero),
                            CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
                    }

                    if (value != Vector2.Zero)
                    {
                        num2 = X - value.X;
                    }
                    else
                    {
                        num2 = Bounds.Left - current.Bounds.Right;
                    }
                }

                if (num2 < num)
                {
                    num = (int) num2;
                    terrainObj = current;
                }
            }

            if (terrainObj != null)
            {
                if (player.Flip == SpriteEffects.None)
                {
                    if (terrainObj.Rotation == 0f)
                    {
                        player.X += num - player.TerrainBounds.Width / 2f;
                        return;
                    }

                    player.X += num - player.Width / 2f;
                }
                else
                {
                    if (terrainObj.Rotation == 0f)
                    {
                        player.X -= num - player.TerrainBounds.Width / 2f;
                        return;
                    }

                    player.X -= num - player.Width / 2f;
                }
            }
        }

        public void RunDestroyAnimation(bool hitPlayer)
        {
            if (!IsDying && !IsDemented)
            {
                CurrentSpeed = 0f;
                AccelerationX = 0f;
                AccelerationY = 0f;
                IsDying = true;
                string spriteName;
                switch (spriteName = SpriteName)
                {
                    case "ArrowProjectile_Sprite":
                    case "SpellClose_Sprite":
                    case "SpellDagger_Sprite":
                        if (hitPlayer)
                        {
                            Tween.By(this, 0.3f, Linear.EaseNone, "Rotation", "270");
                            var num2 = CDGMath.RandomInt(-50, 50);
                            var num3 = CDGMath.RandomInt(-100, -50);
                            Tween.By(this, 0.3f, Linear.EaseNone, "X", num2.ToString(), "Y", num3.ToString());
                            Tween.To(this, 0.3f, Linear.EaseNone, "Opacity", "0");
                            Tween.AddEndHandlerToLastTween(this, "KillProjectile");
                            return;
                        }

                        IsWeighted = false;
                        IsCollidable = false;
                        Tween.To(this, 0.3f, Linear.EaseNone, "delay", "0.3", "Opacity", "0");
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile");
                        return;

                    case "ShurikenProjectile1_Sprite":
                    case "BoneProjectile_Sprite":
                    case "SpellBounce_Sprite":
                    case "LastBossSwordVerticalProjectile_Sprite":
                    {
                        Tween.StopAllContaining(this, false);
                        IsCollidable = false;
                        var num4 = CDGMath.RandomInt(-50, 50);
                        var num5 = CDGMath.RandomInt(-100, 100);
                        Tween.By(this, 0.3f, Linear.EaseNone, "X", num4.ToString(), "Y", num5.ToString());
                        Tween.To(this, 0.3f, Linear.EaseNone, "Opacity", "0");
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile");
                        return;
                    }

                    case "HomingProjectile_Sprite":
                    {
                        var proceduralLevelScreen =
                            Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
                        if (proceduralLevelScreen != null)
                        {
                            proceduralLevelScreen.ImpactEffectPool.DisplayExplosionEffect(Position);
                        }

                        SoundManager.Play3DSound(this, Game.ScreenManager.Player, "MissileExplosion_01",
                            "MissileExplosion_02");
                        KillProjectile();
                        return;
                    }

                    case "SpellAxe_Sprite":
                    case "SpellDualBlades_Sprite":
                        IsCollidable = false;
                        AccelerationX = 0f;
                        AccelerationY = 0f;
                        Tween.To(this, 0.3f, Tween.EaseNone, "Opacity", "0");
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile");
                        return;

                    case "SpellDamageShield_Sprite":
                    case "SpellDisplacer_Sprite":
                        Tween.To(this, 0.2f, Tween.EaseNone, "Opacity", "0");
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile");
                        return;

                    case "LastBossSwordProjectile_Sprite":
                    {
                        IsCollidable = false;
                        Tween.StopAllContaining(this, false);
                        Tween.By(this, 0.3f, Linear.EaseNone, "Rotation", "270");
                        var num6 = CDGMath.RandomInt(-100, -50);
                        Tween.By(this, 0.3f, Linear.EaseNone, "Y", num6.ToString());
                        Tween.To(this, 0.3f, Linear.EaseNone, "Opacity", "0");
                        Tween.AddEndHandlerToLastTween(this, "KillProjectile");
                        return;
                    }

                    case "SpellNuke_Sprite":
                    {
                        var proceduralLevelScreen2 =
                            Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
                        if (proceduralLevelScreen2 != null)
                        {
                            proceduralLevelScreen2.ImpactEffectPool.CrowDestructionEffect(Position);
                        }

                        KillProjectile();
                        return;
                    }

                    case "EnemyFlailKnightBall_Sprite":
                    case "WizardIceSpell_Sprite":
                    case "WizardEarthSpell_Sprite":
                    case "SpellTimeBomb_Sprite":
                    case "SpellLaser_Sprite":
                    case "SpellBoomerang_Sprite":
                        KillProjectile();
                        return;
                }

                if (SpriteName == "WizardIceProjectile_Sprite")
                {
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Ice_Wizard_Break_01",
                        "Ice_Wizard_Break_02", "Ice_Wizard_Break_03");
                }

                var text = SpriteName.Replace("_", "Explosion_");
                ChangeSprite(text);
                AnimationDelay = 0.0333333351f;
                PlayAnimation(false);
                IsWeighted = false;
                IsCollidable = false;
                if (text != "EnemySpearKnightWaveExplosion_Sprite" && text != "WizardIceProjectileExplosion_Sprite")
                {
                    Rotation = 0f;
                }

                Tween.RunFunction(0.5f, this, "KillProjectile");
            }
        }

        public void ActivateEffect()
        {
            var spell = Spell;
            switch (spell)
            {
                case 3:
                    IsWeighted = false;
                    ChangeSprite("SpellTimeBombExplosion_Sprite");
                    PlayAnimation(false);
                    IsDying = true;
                    CollisionTypeTag = 10;
                    AnimationDelay = 0.0333333351f;
                    Scale = new Vector2(4f, 4f);
                    Tween.RunFunction(0.5f, this, "KillProjectile");
                    (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).ImpactEffectPool.DisplayExplosionEffect(
                        Position);
                    return;

                case 4:
                case 6:
                    return;

                case 5:
                    RunDestroyAnimation(false);
                    (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).DamageAllEnemies(Damage);
                    (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).ImpactEffectPool.DisplayDeathEffect(
                        Position);
                    return;

                case 7:
                {
                    RunDestroyAnimation(false);
                    var player = (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).Player;
                    player.Translocate(Position);
                    return;
                }

                case 8:
                    break;

                default:
                    if (spell != 12)
                    {
                        if (spell != 100)
                        {
                            return;
                        }

                        CollisionTypeTag = 10;
                        LifeSpan = AltY;
                        m_elapsedLifeSpan = 0f;
                        IsCollidable = true;
                        Opacity = 1f;
                        return;
                    }

                    break;
            }

            CollisionTypeTag = 10;
        }

        public void KillProjectile()
        {
            IsAlive = false;
            IsDying = false;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Target = null;
                AttachedIcon = null;
                Source = null;
                base.Dispose();
            }
        }

        protected override GameObj CreateCloneInstance()
        {
            return new ProjectileObj(_spriteName);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var projectileObj = obj as ProjectileObj;
            projectileObj.Target = Target;
            projectileObj.CollidesWithTerrain = CollidesWithTerrain;
            projectileObj.ChaseTarget = ChaseTarget;
            projectileObj.FollowArc = FollowArc;
            projectileObj.ShowIcon = ShowIcon;
            projectileObj.DestroysWithTerrain = DestroysWithTerrain;
            projectileObj.AltX = AltX;
            projectileObj.AltY = AltY;
            projectileObj.Spell = Spell;
            projectileObj.IgnoreBoundsCheck = IgnoreBoundsCheck;
            projectileObj.DestroysWithEnemy = DestroysWithEnemy;
            projectileObj.DestroyOnRoomTransition = DestroyOnRoomTransition;
            projectileObj.Source = Source;
            projectileObj.CanBeFusRohDahed = CanBeFusRohDahed;
            projectileObj.WrapProjectile = WrapProjectile;
            projectileObj.IgnoreInvincibleCounter = IgnoreInvincibleCounter;
        }
    }
}