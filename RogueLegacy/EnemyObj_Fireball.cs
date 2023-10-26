//  RogueLegacyRandomizer - EnemyObj_Fireball.cs
//  Last Modified 2023-10-26 12:01 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class EnemyObj_Fireball : EnemyObj
{
    private readonly int        m_bossCoins                    = 30;
    private readonly int        m_bossDiamonds                 = 5;
    private readonly int        m_bossMoneyBags                = 17;
    private readonly LogicBlock m_generalAdvancedLB            = new();
    private readonly LogicBlock m_generalBasicLB               = new();
    private readonly LogicBlock m_generalExpertLB              = new();
    private readonly LogicBlock m_generalMiniBossLB            = new();
    private readonly LogicBlock m_generalNeoLB                 = new();
    private readonly float      m_minibossFireTime             = 0.6f;
    private readonly float      m_MinibossProjectileLifspan    = 11f;
    private readonly float      m_MinibossProjectileLifspanNeo = 20f;
    private readonly float      m_shakeDuration                = 0.03f;
    private          float      DashDelay;
    private          float      DashDuration;
    private          float      DashSpeed;
    private          Color      FlameColour = Color.OrangeRed;
    private          bool       m_isNeo;
    private          float      m_minibossFireTimeCounter = 0.6f;
    private          bool       m_shake;
    private          float      m_shakeTimer;
    private          bool       m_shookLeft;

    public EnemyObj_Fireball(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
        EnemyDifficulty difficulty)
        : base("EnemyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
    {
        Type = 8;
        TintablePart = _objectList[0];
    }

    public bool BossVersionKilled => m_bossVersionKilled;

    public bool IsNeo
    {
        get => m_isNeo;
        set
        {
            m_isNeo = value;
            if (value)
            {
                HealthGainPerLevel = 0;
                DamageGainPerLevel = 0;
                MoneyDropChance = 0f;
                ItemDropChance = 0f;
                m_saveToEnemiesKilledList = false;
            }
        }
    }

    protected override void InitializeEV()
    {
        DashDelay = 0.75f;
        DashDuration = 0.5f;
        DashSpeed = 900f;
        Name = "Charite";
        MaxHealth = 35;
        Damage = 23;
        XPValue = 100;
        MinMoneyDropAmount = 1;
        MaxMoneyDropAmount = 2;
        MoneyDropChance = 0.4f;
        Speed = 400f;
        TurnSpeed = 0.0275f;
        ProjectileSpeed = 525f;
        JumpHeight = 950f;
        CooldownTime = 4f;
        AnimationDelay = 0.05f;
        AlwaysFaceTarget = true;
        CanFallOffLedges = false;
        CanBeKnockedBack = true;
        IsWeighted = false;
        Scale = EnemyEV.Fireball_Basic_Scale;
        ProjectileScale = EnemyEV.Fireball_Basic_ProjectileScale;
        TintablePart.TextureColor = EnemyEV.Fireball_Basic_Tint;
        MeleeRadius = 500;
        ProjectileRadius = 700;
        EngageRadius = 1350;
        ProjectileDamage = Damage;
        KnockBack = EnemyEV.Fireball_Basic_KnockBack;
        switch (Difficulty)
        {
            case EnemyDifficulty.Advanced:
                Name = "Pyrite";
                MaxHealth = 45;
                Damage = 25;
                XPValue = 175;
                MinMoneyDropAmount = 1;
                MaxMoneyDropAmount = 2;
                MoneyDropChance = 0.5f;
                Speed = 420f;
                TurnSpeed = 0.03f;
                ProjectileSpeed = 525f;
                JumpHeight = 950f;
                CooldownTime = 4f;
                AnimationDelay = 0.05f;
                AlwaysFaceTarget = true;
                CanFallOffLedges = false;
                CanBeKnockedBack = true;
                IsWeighted = false;
                Scale = EnemyEV.Fireball_Advanced_Scale;
                ProjectileScale = EnemyEV.Fireball_Advanced_ProjectileScale;
                TintablePart.TextureColor = EnemyEV.Fireball_Advanced_Tint;
                MeleeRadius = 500;
                EngageRadius = 1350;
                ProjectileRadius = 700;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.Fireball_Advanced_KnockBack;
                break;

            case EnemyDifficulty.Expert:
                Name = "Infernite";
                MaxHealth = 63;
                Damage = 27;
                XPValue = 350;
                MinMoneyDropAmount = 2;
                MaxMoneyDropAmount = 3;
                MoneyDropChance = 1f;
                Speed = 440f;
                TurnSpeed = 0.03f;
                ProjectileSpeed = 525f;
                JumpHeight = 950f;
                CooldownTime = 4f;
                AnimationDelay = 0.05f;
                AlwaysFaceTarget = true;
                CanFallOffLedges = false;
                CanBeKnockedBack = true;
                IsWeighted = false;
                Scale = EnemyEV.Fireball_Expert_Scale;
                ProjectileScale = EnemyEV.Fireball_Expert_ProjectileScale;
                TintablePart.TextureColor = EnemyEV.Fireball_Expert_Tint;
                MeleeRadius = 500;
                ProjectileRadius = 700;
                EngageRadius = 1350;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.Fireball_Expert_KnockBack;
                break;

            case EnemyDifficulty.MiniBoss:
                Name = "Ponce de Leon";
                MaxHealth = 505;
                Damage = 29;
                XPValue = 800;
                MinMoneyDropAmount = 15;
                MaxMoneyDropAmount = 20;
                MoneyDropChance = 1f;
                Speed = 380f;
                TurnSpeed = 0.03f;
                ProjectileSpeed = 0f;
                JumpHeight = 950f;
                CooldownTime = 4f;
                AnimationDelay = 0.05f;
                AlwaysFaceTarget = true;
                CanFallOffLedges = false;
                CanBeKnockedBack = false;
                IsWeighted = false;
                Scale = EnemyEV.Fireball_Miniboss_Scale;
                ProjectileScale = EnemyEV.Fireball_Miniboss_ProjectileScale;
                MeleeRadius = 500;
                ProjectileRadius = 775;
                EngageRadius = 1350;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.Fireball_Miniboss_KnockBack;
                if (LevelENV.WeakenBosses) MaxHealth = 1;

                break;
        }

        if (Difficulty == EnemyDifficulty.MiniBoss) m_resetSpriteName = "EnemyGhostBossIdle_Character";
    }

    protected override void InitializeLogic()
    {
        var logicSet = new LogicSet(this);
        logicSet.AddAction(new ChangeSpriteLogicAction("EnemyGhostChase_Character"));
        logicSet.AddAction(new ChaseLogicAction(m_target, true, 2f));
        var logicSet2 = new LogicSet(this);
        logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyGhostChase_Character"));
        logicSet2.AddAction(new ChaseLogicAction(m_target, false, 1f));
        var logicSet3 = new LogicSet(this);
        logicSet3.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character"));
        logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet3.AddAction(new DelayLogicAction(1f, 2f));
        var logicSet4 = new LogicSet(this);
        logicSet4.AddAction(new MoveLogicAction(m_target, true));
        logicSet4.Tag = 2;
        var logicSet5 = new LogicSet(this);
        logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostDashPrep_Character"));
        logicSet5.AddAction(new DelayLogicAction(DashDelay));
        logicSet5.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null));
        logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostDash_Character"));
        logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet5.AddAction(new MoveLogicAction(m_target, true, DashSpeed));
        logicSet5.AddAction(new DelayLogicAction(DashDuration));
        logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character"));
        logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet5.AddAction(new DelayLogicAction(0.75f));
        logicSet5.Tag = 2;
        var logicSet6 = new LogicSet(this);
        logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostDashPrep_Character"));
        logicSet6.AddAction(new DelayLogicAction(DashDelay));
        logicSet6.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null));
        logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostDash_Character"));
        ThrowProjectiles(logicSet6, true);
        logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet6.AddAction(new MoveLogicAction(m_target, true, DashSpeed));
        logicSet6.AddAction(new DelayLogicAction(DashDuration));
        logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyGhostIdle_Character"));
        logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet6.AddAction(new DelayLogicAction(0.75f));
        logicSet6.Tag = 2;
        var logicSet7 = new LogicSet(this);
        logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character"));
        logicSet7.AddAction(new ChaseLogicAction(m_target, true, 2f));
        var logicSet8 = new LogicSet(this);
        logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character"));
        logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet8.AddAction(new DelayLogicAction(1f, 2f));
        var logicSet9 = new LogicSet(this);
        logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet9.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet9.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossDashPrep_Character"));
        logicSet9.AddAction(new DelayLogicAction(DashDelay));
        logicSet9.AddAction(new RunFunctionLogicAction(this, "TurnToPlayer", null));
        logicSet9.AddAction(new ChangeSpriteLogicAction("EnemyGhostBossIdle_Character"));
        logicSet9.AddAction(new RunFunctionLogicAction(this, "ChangeFlameDirection"));
        logicSet9.AddAction(new MoveLogicAction(m_target, true, DashSpeed));
        logicSet9.AddAction(new DelayLogicAction(DashDuration));
        logicSet9.AddAction(new ChangePropertyLogicAction(_objectList[0], "Rotation", 0));
        logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet9.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet9.AddAction(new DelayLogicAction(0.75f));
        logicSet5.Tag = 2;
        m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet5);
        m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet5);
        m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4, logicSet6);
        m_generalMiniBossLB.AddLogicSet(logicSet7, logicSet2, logicSet8, logicSet4, logicSet9);
        m_generalNeoLB.AddLogicSet(logicSet7, logicSet2, logicSet8, logicSet4, logicSet9);
        logicBlocksToDispose.Add(m_generalBasicLB);
        logicBlocksToDispose.Add(m_generalAdvancedLB);
        logicBlocksToDispose.Add(m_generalExpertLB);
        logicBlocksToDispose.Add(m_generalMiniBossLB);
        logicBlocksToDispose.Add(m_generalNeoLB);
        var arg_600_1 = m_generalBasicLB;
        var array = new int[5];
        array[0] = 100;
        SetCooldownLogicBlock(arg_600_1, array);
        base.InitializeLogic();
    }

    public void ChangeFlameDirection()
    {
        if (m_target.X < X)
        {
            _objectList[0].Rotation = 90f;
            return;
        }

        _objectList[0].Rotation = -90f;
    }

    private void ThrowProjectiles(LogicSet ls, bool useBossProjectile = false)
    {
        var projectileData = new ProjectileData(this)
        {
            SpriteName = "GhostProjectile_Sprite",
            SourceAnchor = Vector2.Zero,
            Target = null,
            Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
            IsWeighted = false,
            RotationSpeed = 0f,
            Damage = Damage,
            AngleOffset = 0f,
            Angle = new Vector2(0f, 0f),
            CollidesWithTerrain = false,
            Scale = ProjectileScale
        };
        if (useBossProjectile) projectileData.SpriteName = "GhostProjectile_Sprite";

        ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
        projectileData.Angle = new Vector2(60f, 60f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(30f, 30f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(120f, 120f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(150f, 150f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-60f, -60f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-30f, -30f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-120f, -120f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-150f, -150f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Dispose();
    }

    private void ThrowStandingProjectile(bool useBossProjectile = false)
    {
        var projectileData = new ProjectileData(this)
        {
            SpriteName = "GhostProjectile_Sprite",
            SourceAnchor = Vector2.Zero,
            Target = null,
            Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
            IsWeighted = false,
            RotationSpeed = 0f,
            Damage = Damage,
            AngleOffset = 0f,
            Angle = new Vector2(0f, 0f),
            CollidesWithTerrain = false,
            Scale = ProjectileScale,
            Lifespan = m_MinibossProjectileLifspan
        };
        if (IsNeo) projectileData.Lifespan = m_MinibossProjectileLifspanNeo;

        if (useBossProjectile) projectileData.SpriteName = "GhostBossProjectile_Sprite";

        var projectileObj = _levelScreen.ProjectileManager.FireProjectile(projectileData);
        projectileObj.Rotation = 0f;
        if (IsNeo) projectileObj.TextureColor = Color.MediumSpringGreen;

        projectileData.Dispose();
    }

    protected override void RunBasicLogic()
    {
        switch (State)
        {
            case 0:
            {
                var arg_4E_1 = true;
                var arg_4E_2 = m_generalBasicLB;
                var array = new int[5];
                array[2] = 100;
                RunLogicBlock(arg_4E_1, arg_4E_2, array);
                return;
            }

            case 1:
            case 2:
            case 3:
            {
                var arg_33_1 = true;
                var arg_33_2 = m_generalBasicLB;
                var array2 = new int[5];
                array2[0] = 100;
                RunLogicBlock(arg_33_1, arg_33_2, array2);
                return;
            }

            default:
                return;
        }
    }

    protected override void RunAdvancedLogic()
    {
        switch (State)
        {
            case 0:
            {
                var arg_6E_1 = true;
                var arg_6E_2 = m_generalAdvancedLB;
                var array = new int[5];
                array[2] = 100;
                RunLogicBlock(arg_6E_1, arg_6E_2, array);
                return;
            }

            case 1:
            case 2:
            {
                var arg_53_1 = true;
                var arg_53_2 = m_generalAdvancedLB;
                var array2 = new int[5];
                array2[0] = 100;
                RunLogicBlock(arg_53_1, arg_53_2, array2);
                return;
            }

            case 3:
                RunLogicBlock(true, m_generalAdvancedLB, 40, 0, 0, 0, 60);
                return;

            default:
                return;
        }
    }

    protected override void RunExpertLogic()
    {
        switch (State)
        {
            case 0:
            {
                var arg_6E_1 = true;
                var arg_6E_2 = m_generalExpertLB;
                var array = new int[5];
                array[2] = 100;
                RunLogicBlock(arg_6E_1, arg_6E_2, array);
                return;
            }

            case 1:
            case 2:
            {
                var arg_53_1 = true;
                var arg_53_2 = m_generalExpertLB;
                var array2 = new int[5];
                array2[0] = 100;
                RunLogicBlock(arg_53_1, arg_53_2, array2);
                return;
            }

            case 3:
                RunLogicBlock(true, m_generalExpertLB, 40, 0, 0, 0, 60);
                return;

            default:
                return;
        }
    }

    protected override void RunMinibossLogic()
    {
        if (!IsNeo)
            switch (State)
            {
                case 0:
                {
                    var arg_76_1 = true;
                    var arg_76_2 = m_generalMiniBossLB;
                    var array = new int[5];
                    array[0] = 100;
                    RunLogicBlock(arg_76_1, arg_76_2, array);
                    return;
                }

                case 1:
                case 2:
                {
                    var arg_5B_1 = true;
                    var arg_5B_2 = m_generalMiniBossLB;
                    var array2 = new int[5];
                    array2[0] = 100;
                    RunLogicBlock(arg_5B_1, arg_5B_2, array2);
                    return;
                }

                case 3:
                    RunLogicBlock(true, m_generalMiniBossLB, 52, 0, 0, 0, 48);
                    return;

                default:
                    return;
            }

        switch (State)
        {
            case 0:
            {
                var arg_F6_1 = true;
                var arg_F6_2 = m_generalNeoLB;
                var array3 = new int[5];
                array3[0] = 100;
                RunLogicBlock(arg_F6_1, arg_F6_2, array3);
                return;
            }

            case 1:
            case 2:
            {
                var arg_D8_1 = true;
                var arg_D8_2 = m_generalNeoLB;
                var array4 = new int[5];
                array4[0] = 100;
                RunLogicBlock(arg_D8_1, arg_D8_2, array4);
                return;
            }

            case 3:
                RunLogicBlock(true, m_generalNeoLB, 45, 0, 0, 0, 55);
                return;

            default:
                return;
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (Difficulty == EnemyDifficulty.MiniBoss && !IsPaused && m_minibossFireTimeCounter > 0f &&
            !m_bossVersionKilled)
        {
            m_minibossFireTimeCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (m_minibossFireTimeCounter <= 0f)
            {
                ThrowStandingProjectile(true);
                m_minibossFireTimeCounter = m_minibossFireTime;
            }
        }

        if (m_shake && m_shakeTimer > 0f)
        {
            m_shakeTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (m_shakeTimer <= 0f)
            {
                m_shakeTimer = m_shakeDuration;
                if (m_shookLeft)
                {
                    m_shookLeft = false;
                    X += 5f;
                }
                else
                {
                    X -= 5f;
                    m_shookLeft = true;
                }
            }
        }

        base.Update(gameTime);
    }

    public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
    {
        if (Difficulty == EnemyDifficulty.MiniBoss && !m_bossVersionKilled)
        {
            var playerObj = otherBox.AbsParent as PlayerObj;
            if (playerObj != null && otherBox.Type == 1 && !playerObj.IsInvincible && playerObj.State == 8)
                playerObj.HitPlayer(this);
        }

        if (collisionResponseType != 1) base.CollisionResponse(thisBox, otherBox, collisionResponseType);
    }

    public void TurnToPlayer()
    {
        var turnSpeed = TurnSpeed;
        TurnSpeed = 2f;
        CDGMath.TurnToFace(this, m_target.Position);
        TurnSpeed = turnSpeed;
    }

    public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
    {
        if (!m_bossVersionKilled) base.HitEnemy(damage, collisionPt, isPlayer);
    }

    public override void Kill(bool giveXP = true)
    {
        if (Difficulty != EnemyDifficulty.MiniBoss)
        {
            base.Kill(giveXP);
            return;
        }

        if (m_target.CurrentHealth > 0)
        {
            Game.PlayerStats.FireballBossBeaten = true;
            SoundManager.StopMusic();
            SoundManager.PlaySound("PressStart");
            m_bossVersionKilled = true;
            m_target.LockControls();
            _levelScreen.PauseScreen();
            _levelScreen.ProjectileManager.DestroyAllProjectiles(false);
            _levelScreen.RunWhiteSlashEffect();
            Tween.RunFunction(1f, this, "Part2");
            SoundManager.PlaySound("Boss_Flash");
            SoundManager.PlaySound("Boss_Fireball_Freeze");
            GameUtil.UnlockAchievement("FEAR_OF_FIRE");

            Program.Game.ArchipelagoManager.IsDeathLinkSafe = false;
            var data = new List<object>
            {
                new Vector2(Game.ScreenManager.Player.X, Game.ScreenManager.Player.Y),
                ItemCategory.Teleporter,
                new Vector2(-1f, -1f),
                new Vector2(-1f, -1f),
                "You",
                null
            };

            Game.ScreenManager.DisplayScreen((int) ScreenType.GetItem, true, data);
            Game.ScreenManager.Player.RunGetItemAnimation();
        }
    }

    public void Part2()
    {
        _levelScreen.UnpauseScreen();
        m_target.UnlockControls();
        if (m_currentActiveLB != null) m_currentActiveLB.StopLogicBlock();

        PauseEnemy(true);
        ChangeSprite("EnemyGhostBossIdle_Character");
        PlayAnimation();
        m_target.CurrentSpeed = 0f;
        m_target.ForceInvincible = true;
        Tween.To(_levelScreen.Camera, 0.5f, Quad.EaseInOut, "X", X.ToString(), "Y", Y.ToString());
        m_shake = true;
        m_shakeTimer = m_shakeDuration;
        for (var i = 0; i < 40; i++)
        {
            var vector = new Vector2(CDGMath.RandomInt(Bounds.Left, Bounds.Right),
                CDGMath.RandomInt(Bounds.Top, Bounds.Bottom));
            Tween.RunFunction(i * 0.1f, typeof(SoundManager), "Play3DSound", this, m_target, new[]
            {
                "Boss_Explo_01",
                "Boss_Explo_02",
                "Boss_Explo_03"
            });
            Tween.RunFunction(i * 0.1f, _levelScreen.ImpactEffectPool, "DisplayExplosionEffect", vector);
        }

        Tween.AddEndHandlerToLastTween(this, "Part3");
        if (!IsNeo)
        {
            var list = new List<int>();
            for (var j = 0; j < m_bossCoins; j++) list.Add(0);
            for (var k = 0; k < m_bossMoneyBags; k++) list.Add(1);
            for (var l = 0; l < m_bossDiamonds; l++) list.Add(2);
            CDGMath.Shuffle(list);
            var num = 2.5f / list.Count;
            for (var m = 0; m < list.Count; m++)
            {
                var position = Position;
                if (list[m] == 0)
                    Tween.RunFunction(m * num, _levelScreen.ItemDropManager, "DropItem", position, 1, 10);
                else if (list[m] == 1)
                    Tween.RunFunction(m * num, _levelScreen.ItemDropManager, "DropItem", position, 10, 100);
                else
                    Tween.RunFunction(m * num, _levelScreen.ItemDropManager, "DropItem", position, 11, 500);
            }
        }
    }

    public void Part3()
    {
        SoundManager.PlaySound("Boss_Fireball_Death");
        _levelScreen.ImpactEffectPool.DestroyFireballBoss(Position);
        base.Kill();
    }
}
