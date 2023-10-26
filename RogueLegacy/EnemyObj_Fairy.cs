//  RogueLegacyRandomizer - EnemyObj_Fairy.cs
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
using Microsoft.Xna.Framework.Audio;
using RogueLegacy.Enums;
using RogueLegacy.Screens;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy;

public class EnemyObj_Fairy : EnemyObj
{
    private readonly float      AttackDelay         = 0.5f;
    private readonly int        m_bossCoins         = 30;
    private readonly int        m_bossDiamonds      = 3;
    private readonly int        m_bossMoneyBags     = 12;
    private readonly LogicBlock m_generalAdvancedLB = new();
    private readonly LogicBlock m_generalBasicLB    = new();
    private readonly LogicBlock m_generalCooldownLB = new();
    private readonly LogicBlock m_generalExpertLB   = new();
    private readonly LogicBlock m_generalMiniBossLB = new();
    private readonly LogicBlock m_generalNeoLB      = new();
    private readonly int        m_numSummons        = 22;
    private readonly float      m_shakeDuration     = 0.03f;
    private readonly float      m_summonTimerNeo    = 3f;
    private          Cue        m_deathLoop;
    private          bool       m_isNeo;
    private          bool       m_playDeathLoop;
    private          bool       m_shake;
    private          float      m_shakeTimer;
    private          bool       m_shookLeft;
    private          float      m_summonCounter = 6f;
    private          float      m_summonTimer   = 6f;
    public           int        NumHits         = 1;
    public           RoomObj    SpawnRoom;

    public EnemyObj_Fairy(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
        EnemyDifficulty difficulty)
        : base("EnemyFairyGhostIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
    {
        PlayAnimation();
        MainFairy = true;
        TintablePart = _objectList[0];
        Type = 7;
    }

    public bool    MainFairy        { get; set; }
    public Vector2 SavedStartingPos { get; set; }

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
        Name = "Fury";
        MaxHealth = 27;
        Damage = 18;
        XPValue = 125;
        MinMoneyDropAmount = 1;
        MaxMoneyDropAmount = 2;
        MoneyDropChance = 0.4f;
        Speed = 250f;
        TurnSpeed = 0.0325f;
        ProjectileSpeed = 475f;
        JumpHeight = 300f;
        CooldownTime = 1.75f;
        AnimationDelay = 0.1f;
        AlwaysFaceTarget = true;
        CanFallOffLedges = true;
        CanBeKnockedBack = true;
        IsWeighted = false;
        Scale = EnemyEV.Fairy_Basic_Scale;
        ProjectileScale = EnemyEV.Fairy_Basic_ProjectileScale;
        TintablePart.TextureColor = EnemyEV.Fairy_Basic_Tint;
        MeleeRadius = 225;
        ProjectileRadius = 700;
        EngageRadius = 925;
        ProjectileDamage = Damage;
        KnockBack = EnemyEV.Fairy_Basic_KnockBack;
        switch (Difficulty)
        {
            case EnemyDifficulty.Advanced:
                Name = "Rage";
                MaxHealth = 37;
                Damage = 22;
                XPValue = 200;
                MinMoneyDropAmount = 1;
                MaxMoneyDropAmount = 2;
                MoneyDropChance = 0.5f;
                Speed = 265f;
                TurnSpeed = 0.0325f;
                ProjectileSpeed = 475f;
                JumpHeight = 300f;
                CooldownTime = 1.75f;
                AnimationDelay = 0.1f;
                AlwaysFaceTarget = true;
                CanFallOffLedges = true;
                CanBeKnockedBack = true;
                IsWeighted = false;
                Scale = EnemyEV.Fairy_Advanced_Scale;
                ProjectileScale = EnemyEV.Fairy_Advanced_ProjectileScale;
                TintablePart.TextureColor = EnemyEV.Fairy_Advanced_Tint;
                MeleeRadius = 225;
                EngageRadius = 925;
                ProjectileRadius = 700;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.Fairy_Advanced_KnockBack;
                break;

            case EnemyDifficulty.Expert:
                Name = "Wrath";
                MaxHealth = 72;
                Damage = 24;
                XPValue = 350;
                MinMoneyDropAmount = 2;
                MaxMoneyDropAmount = 4;
                MoneyDropChance = 1f;
                Speed = 280f;
                TurnSpeed = 0.0325f;
                ProjectileSpeed = 475f;
                JumpHeight = 300f;
                CooldownTime = 2.5f;
                AnimationDelay = 0.1f;
                AlwaysFaceTarget = true;
                CanFallOffLedges = true;
                CanBeKnockedBack = true;
                IsWeighted = false;
                Scale = EnemyEV.Fairy_Expert_Scale;
                ProjectileScale = EnemyEV.Fairy_Expert_ProjectileScale;
                TintablePart.TextureColor = EnemyEV.Fairy_Expert_Tint;
                MeleeRadius = 225;
                ProjectileRadius = 700;
                EngageRadius = 925;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.Fairy_Expert_KnockBack;
                break;

            case EnemyDifficulty.MiniBoss:
                Name = "Alexander";
                MaxHealth = 635;
                Damage = 30;
                XPValue = 1000;
                MinMoneyDropAmount = 15;
                MaxMoneyDropAmount = 20;
                MoneyDropChance = 1f;
                Speed = 220f;
                TurnSpeed = 0.0325f;
                ProjectileSpeed = 545f;
                JumpHeight = 300f;
                CooldownTime = 3f;
                AnimationDelay = 0.1f;
                AlwaysFaceTarget = true;
                CanFallOffLedges = true;
                CanBeKnockedBack = false;
                IsWeighted = false;
                Scale = new Vector2(2.5f, 2.5f);
                ProjectileScale = new Vector2(2f, 2f);
                MeleeRadius = 225;
                ProjectileRadius = 775;
                EngageRadius = 925;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.Fairy_Miniboss_KnockBack;
                NumHits = 1;
                if (LevelENV.WeakenBosses) MaxHealth = 1;

                break;
        }

        if (Difficulty == EnemyDifficulty.MiniBoss) m_resetSpriteName = "EnemyFairyGhostBossIdle_Character";
    }

    protected override void InitializeLogic()
    {
        var logicSet = new LogicSet(this);
        logicSet.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostMove_Character"));
        logicSet.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyMove1", "FairyMove2",
            "FairyMove3"));
        logicSet.AddAction(new ChaseLogicAction(m_target, true, 1f));
        var logicSet2 = new LogicSet(this);
        logicSet2.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet2.AddAction(new ChaseLogicAction(m_target, false, 0.5f));
        var logicSet3 = new LogicSet(this);
        logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet3.AddAction(new DelayLogicAction(0.5f, 0.75f));
        var logicSet4 = new LogicSet(this);
        logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false));
        logicSet4.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
        logicSet4.AddAction(new DelayLogicAction(AttackDelay));
        logicSet4.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
        logicSet4.AddAction(new PlayAnimationLogicAction("Attack", "End"));
        logicSet4.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet4.AddAction(new DelayLogicAction(0.25f));
        logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet4.Tag = 2;
        var logicSet5 = new LogicSet(this);
        logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet5.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false));
        logicSet5.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
        logicSet5.AddAction(new DelayLogicAction(AttackDelay));
        logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
        logicSet5.AddAction(new DelayLogicAction(0.15f));
        logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
        logicSet5.AddAction(new DelayLogicAction(0.15f));
        logicSet5.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", false));
        logicSet5.AddAction(new PlayAnimationLogicAction("Attack", "End"));
        logicSet5.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet5.AddAction(new DelayLogicAction(0.25f));
        logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet5.Tag = 2;
        var logicSet6 = new LogicSet(this);
        logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostShoot_Character", false, false));
        logicSet6.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
        logicSet6.AddAction(new DelayLogicAction(AttackDelay));
        ThrowEightProjectiles(logicSet6);
        logicSet6.AddAction(new DelayLogicAction(0.25f));
        ThrowEightProjectiles(logicSet6);
        logicSet6.AddAction(new DelayLogicAction(0.25f));
        ThrowEightProjectiles(logicSet6);
        logicSet6.AddAction(new DelayLogicAction(0.25f));
        ThrowEightProjectiles(logicSet6);
        logicSet6.AddAction(new PlayAnimationLogicAction("Attack", "End"));
        logicSet6.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostIdle_Character"));
        logicSet6.AddAction(new DelayLogicAction(0.25f));
        logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet6.Tag = 2;
        var logicSet7 = new LogicSet(this);
        logicSet7.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character"));
        logicSet7.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyMove1", "FairyMove2",
            "FairyMove3"));
        logicSet7.AddAction(new ChaseLogicAction(m_target, true, 1f));
        var logicSet8 = new LogicSet(this);
        logicSet8.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
        logicSet8.AddAction(new ChaseLogicAction(m_target, false, 0.5f));
        var logicSet9 = new LogicSet(this);
        logicSet9.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet9.AddAction(new DelayLogicAction(0.5f, 0.75f));
        var logicSet10 = new LogicSet(this);
        logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
        logicSet10.AddAction(new MoveLogicAction(m_target, true, 0f));
        logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
        logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false));
        logicSet10.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
        logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
        logicSet10.AddAction(new DelayLogicAction(AttackDelay));
        logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossShoot_Character", false, false));
        logicSet10.AddAction(new PlayAnimationLogicAction("Attack", "End"));
        logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossMove_Character"));
        logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
        logicSet10.AddAction(new DelayLogicAction(0.25f));
        logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
        logicSet10.AddAction(new DelayLogicAction(0.25f));
        logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
        logicSet10.AddAction(new DelayLogicAction(0.25f));
        logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
        logicSet10.AddAction(new DelayLogicAction(0.25f));
        logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
        logicSet10.AddAction(new DelayLogicAction(0.25f));
        logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowFourProjectiles", true));
        logicSet10.AddAction(new ChangeSpriteLogicAction("EnemyFairyGhostBossIdle_Character"));
        logicSet10.AddAction(new DelayLogicAction(0.25f));
        logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
        logicSet10.Tag = 2;
        m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet4);
        m_generalAdvancedLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet5);
        m_generalExpertLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet6);
        m_generalMiniBossLB.AddLogicSet(logicSet7, logicSet8, logicSet9, logicSet10);
        m_generalNeoLB.AddLogicSet(logicSet7, logicSet8, logicSet9, logicSet10);
        if (Difficulty == EnemyDifficulty.MiniBoss)
            m_generalCooldownLB.AddLogicSet(logicSet7, logicSet8, logicSet9);
        else
            m_generalCooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3);

        logicBlocksToDispose.Add(m_generalBasicLB);
        logicBlocksToDispose.Add(m_generalAdvancedLB);
        logicBlocksToDispose.Add(m_generalExpertLB);
        logicBlocksToDispose.Add(m_generalMiniBossLB);
        logicBlocksToDispose.Add(m_generalCooldownLB);
        logicBlocksToDispose.Add(m_generalNeoLB);
        var arg_975_1 = m_generalCooldownLB;
        var array = new int[3];
        array[0] = 70;
        array[1] = 30;
        SetCooldownLogicBlock(arg_975_1, array);
        base.InitializeLogic();
    }

    public void ThrowFourProjectiles(bool useBossProjectile)
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
        if (useBossProjectile) projectileData.SpriteName = "GhostProjectileBoss_Sprite";

        if (Difficulty == EnemyDifficulty.MiniBoss)
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Roar_01",
                "Boss_Flameskull_Roar_02", "Boss_Flameskull_Roar_03");
        else
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyAttack1");

        _levelScreen.ProjectileManager.FireProjectile(projectileData);
        projectileData.Angle = new Vector2(90f, 90f);
        _levelScreen.ProjectileManager.FireProjectile(projectileData);
        projectileData.Angle = new Vector2(180f, 180f);
        _levelScreen.ProjectileManager.FireProjectile(projectileData);
        projectileData.Angle = new Vector2(-90f, -90f);
        _levelScreen.ProjectileManager.FireProjectile(projectileData);
        projectileData.Dispose();
    }

    private void ThrowEightProjectiles(LogicSet ls)
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
        ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(90f, 90f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(180f, 180f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-90f, -90f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        ls.AddAction(new DelayLogicAction(0.125f));
        ls.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "FairyAttack1"));
        projectileData.Angle = new Vector2(135f, 135f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(45f, 45f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-45f, -45f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Angle = new Vector2(-135f, -135f);
        ls.AddAction(new FireProjectileLogicAction(_levelScreen.ProjectileManager, projectileData));
        projectileData.Dispose();
    }

    protected override void RunBasicLogic()
    {
        switch (State)
        {
            case 0:
            {
                var arg_73_1 = true;
                var arg_73_2 = m_generalBasicLB;
                var array = new int[4];
                array[2] = 100;
                RunLogicBlock(arg_73_1, arg_73_2, array);
                return;
            }

            case 1:
            {
                var arg_58_1 = true;
                var arg_58_2 = m_generalBasicLB;
                var array2 = new int[4];
                array2[0] = 100;
                RunLogicBlock(arg_58_1, arg_58_2, array2);
                return;
            }

            case 2:
            case 3:
                RunLogicBlock(true, m_generalBasicLB, 50, 10, 0, 40);
                return;

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
                var arg_73_1 = true;
                var arg_73_2 = m_generalAdvancedLB;
                var array = new int[4];
                array[2] = 100;
                RunLogicBlock(arg_73_1, arg_73_2, array);
                return;
            }

            case 1:
            {
                var arg_58_1 = true;
                var arg_58_2 = m_generalAdvancedLB;
                var array2 = new int[4];
                array2[0] = 100;
                RunLogicBlock(arg_58_1, arg_58_2, array2);
                return;
            }

            case 2:
            case 3:
                RunLogicBlock(true, m_generalAdvancedLB, 50, 10, 0, 40);
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
                var arg_73_1 = true;
                var arg_73_2 = m_generalExpertLB;
                var array = new int[4];
                array[2] = 100;
                RunLogicBlock(arg_73_1, arg_73_2, array);
                return;
            }

            case 1:
            {
                var arg_58_1 = true;
                var arg_58_2 = m_generalExpertLB;
                var array2 = new int[4];
                array2[0] = 100;
                RunLogicBlock(arg_58_1, arg_58_2, array2);
                return;
            }

            case 2:
            case 3:
                RunLogicBlock(true, m_generalExpertLB, 50, 10, 0, 40);
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
                    var arg_80_1 = true;
                    var arg_80_2 = m_generalMiniBossLB;
                    var array = new int[4];
                    array[0] = 80;
                    array[1] = 20;
                    RunLogicBlock(arg_80_1, arg_80_2, array);
                    return;
                }

                case 1:
                {
                    var arg_60_1 = true;
                    var arg_60_2 = m_generalMiniBossLB;
                    var array2 = new int[4];
                    array2[0] = 100;
                    RunLogicBlock(arg_60_1, arg_60_2, array2);
                    return;
                }

                case 2:
                case 3:
                    RunLogicBlock(true, m_generalMiniBossLB, 50, 10, 0, 40);
                    return;

                default:
                    return;
            }

        switch (State)
        {
            case 0:
            {
                var arg_10C_1 = true;
                var arg_10C_2 = m_generalNeoLB;
                var array3 = new int[4];
                array3[0] = 80;
                array3[1] = 20;
                RunLogicBlock(arg_10C_1, arg_10C_2, array3);
                return;
            }

            case 1:
            {
                var arg_E8_1 = true;
                var arg_E8_2 = m_generalNeoLB;
                var array4 = new int[4];
                array4[0] = 100;
                RunLogicBlock(arg_E8_1, arg_E8_2, array4);
                return;
            }

            case 2:
            case 3:
                RunLogicBlock(true, m_generalNeoLB, 50, 10, 0, 40);
                return;

            default:
                return;
        }
    }

    public override void Update(GameTime gameTime)
    {
        var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
        if (Difficulty == EnemyDifficulty.MiniBoss && !IsPaused)
        {
            if (m_summonCounter > 0f)
            {
                m_summonCounter -= num;
                if (m_summonCounter <= 0f)
                {
                    if (IsNeo) m_summonTimer = m_summonTimerNeo;

                    m_summonCounter = m_summonTimer;
                    NumHits--;
                    if (!IsKilled && NumHits <= 0 && _levelScreen.CurrentRoom.ActiveEnemies <= m_numSummons + 1)
                    {
                        if (Game.PlayerStats.TimesCastleBeaten <= 0 || IsNeo)
                        {
                            CreateFairy(EnemyDifficulty.Basic);
                            CreateFairy(EnemyDifficulty.Basic);
                        }
                        else
                        {
                            CreateFairy(EnemyDifficulty.Advanced);
                            CreateFairy(EnemyDifficulty.Advanced);
                        }

                        NumHits = 1;
                    }
                }
            }

            var currentRoom = _levelScreen.CurrentRoom;
            var bounds = Bounds;
            var bounds2 = currentRoom.Bounds;
            var num2 = bounds.Right - bounds2.Right;
            var num3 = bounds.Left - bounds2.Left;
            var num4 = bounds.Bottom - bounds2.Bottom;
            if (num2 > 0) X -= num2;

            if (num3 < 0) X -= num3;

            if (num4 > 0) Y -= num4;
        }

        if (m_shake && m_shakeTimer > 0f)
        {
            m_shakeTimer -= num;
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

        if (m_playDeathLoop && m_deathLoop != null && !m_deathLoop.IsPlaying)
            m_deathLoop = SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death_Loop");

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

    private void CreateFairy(EnemyDifficulty difficulty)
    {
        var enemyObj_Fairy = new EnemyObj_Fairy(null, null, null, difficulty);
        enemyObj_Fairy.Position = Position;
        enemyObj_Fairy.DropsItem = false;
        if (m_target.X < enemyObj_Fairy.X)
            enemyObj_Fairy.Orientation = MathHelper.ToRadians(0f);
        else
            enemyObj_Fairy.Orientation = MathHelper.ToRadians(180f);

        enemyObj_Fairy.Level = Level - 7 - 1;
        _levelScreen.AddEnemyToCurrentRoom(enemyObj_Fairy);
        enemyObj_Fairy.PlayAnimation();
        enemyObj_Fairy.MainFairy = false;
        enemyObj_Fairy.SavedStartingPos = enemyObj_Fairy.Position;
        enemyObj_Fairy.SaveToFile = false;
        if (LevelENV.ShowEnemyRadii) enemyObj_Fairy.InitializeDebugRadii();

        enemyObj_Fairy.SpawnRoom = _levelScreen.CurrentRoom;
        enemyObj_Fairy.GivesLichHealth = false;
    }

    public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
    {
        if (!m_bossVersionKilled)
        {
            base.HitEnemy(damage, position, isPlayer);
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
        }
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
            Game.PlayerStats.FairyBossBeaten = true;
            SoundManager.StopMusic();
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "PressStart");
            m_bossVersionKilled = true;
            m_target.LockControls();
            _levelScreen.PauseScreen();
            _levelScreen.ProjectileManager.DestroyAllProjectiles(false);
            _levelScreen.RunWhiteSlashEffect();
            Tween.RunFunction(1f, this, "Part2");
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flash");
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Freeze");
            GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");

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
        m_playDeathLoop = true;
        foreach (var current in _levelScreen.CurrentRoom.TempEnemyList)
            if (!current.IsKilled)
                current.Kill();

        _levelScreen.UnpauseScreen();
        m_target.UnlockControls();
        if (m_currentActiveLB != null) m_currentActiveLB.StopLogicBlock();

        PauseEnemy(true);
        ChangeSprite("EnemyFairyGhostBossShoot_Character");
        PlayAnimation();
        m_target.CurrentSpeed = 0f;
        m_target.ForceInvincible = true;
        if (IsNeo) m_target.InvincibleToSpikes = true;

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
        m_playDeathLoop = false;
        SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Boss_Flameskull_Death");
        if (m_deathLoop != null && m_deathLoop.IsPlaying) m_deathLoop.Stop(AudioStopOptions.Immediate);

        _levelScreen.RunWhiteSlash2();
        base.Kill();
    }

    public override void Reset()
    {
        if (!MainFairy)
        {
            _levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, SavedStartingPos);
            Dispose();
            return;
        }

        NumHits = 1;
        base.Reset();
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            SpawnRoom = null;
            if (m_deathLoop != null && !m_deathLoop.IsDisposed) m_deathLoop.Dispose();

            m_deathLoop = null;
            base.Dispose();
        }
    }
}
