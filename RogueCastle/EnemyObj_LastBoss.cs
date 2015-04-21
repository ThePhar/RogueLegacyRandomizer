/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class EnemyObj_LastBoss : EnemyObj
    {
        private FrameSoundObj m_walkUpSoundFinalBoss;
        private FrameSoundObj m_walkDownSoundFinalBoss;
        private Vector2 AxeSpellScale = new Vector2(3f, 3f);
        private float AxeProjectileSpeed = 1100f;
        private Vector2 DaggerSpellScale = new Vector2(3.5f, 3.5f);
        private float DaggerProjectileSpeed = 900f;
        private float m_Spell_Close_Lifespan = 6f;
        private float m_Spell_Close_Scale = 3.5f;
        private int MegaFlyingDaggerProjectileSpeed = 2350;
        private int MegaFlyingSwordAmount = 29;
        private int MegaUpwardSwordProjectileSpeed = 2450;
        private int MegaUpwardSwordProjectileAmount = 8;
        private int m_Mega_Shield_Distance = 525;
        private float m_Mega_Shield_Scale = 4f;
        private float m_Mega_Shield_Speed = 1f;
        private int m_numSpears = 26;
        private float m_spearDuration = 1.75f;
        private bool m_isHurt;
        private bool m_isDashing;
        private bool m_inSecondForm;
        private float m_smokeCounter = 0.05f;
        private float m_castDelay = 0.25f;
        private int m_orbsEasy = 1;
        private int m_orbsNormal = 2;
        private int m_orbsHard = 3;
        private float m_lastBossAttackDelay = 0.35f;
        private bool m_shake;
        private bool m_shookLeft;
        private float m_shakeTimer;
        private float m_shakeDuration = 0.03f;
        private List<ProjectileObj> m_damageShieldProjectiles;
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_damageShieldLB = new LogicBlock();
        private LogicBlock m_cooldownLB = new LogicBlock();
        private LogicBlock m_secondFormCooldownLB = new LogicBlock();
        private LogicBlock m_firstFormDashAwayLB = new LogicBlock();
        private LogicBlock m_generalBasicNeoLB = new LogicBlock();
        private bool m_firstFormDying;
        private float m_teleportDuration;
        private BlankObj m_delayObj;
        private bool m_isNeo;
        private bool m_neoDying;
        private ProjectileData m_daggerProjData;
        private ProjectileData m_axeProjData;

        public bool IsSecondForm
        {
            get { return m_inSecondForm; }
        }

        public bool IsNeo
        {
            get { return m_isNeo; }
            set
            {
                m_isNeo = value;
                if (value)
                {
                    HealthGainPerLevel = 0;
                    DamageGainPerLevel = 0;
                    ItemDropChance = 0f;
                    MoneyDropChance = 0f;
                    m_saveToEnemiesKilledList = false;
                    CanFallOffLedges = true;
                }
            }
        }

        protected override void InitializeEV()
        {
            Name = "Johannes";
            MaxHealth = 300;
            Damage = 22;
            XPValue = 1000;
            MinMoneyDropAmount = 20;
            MaxMoneyDropAmount = 30;
            MoneyDropChance = 1f;
            Speed = 500f;
            TurnSpeed = 2f;
            ProjectileSpeed = 1100f;
            JumpHeight = 1050f;
            CooldownTime = 1f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            CanBeKnockedBack = true;
            IsWeighted = true;
            Scale = EnemyEV.LastBoss_Basic_Scale;
            ProjectileScale = EnemyEV.LastBoss_Basic_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.LastBoss_Basic_Tint;
            MeleeRadius = 300;
            ProjectileRadius = 650;
            EngageRadius = 900;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.LastBoss_Basic_KnockBack;
            switch (Difficulty)
            {
                case GameTypes.EnemyDifficulty.ADVANCED:
                    Name = "The Fountain";
                    MaxHealth = 530;
                    Damage = 22;
                    XPValue = 5000;
                    MinMoneyDropAmount = 20;
                    MaxMoneyDropAmount = 30;
                    MoneyDropChance = 1f;
                    Speed = 325f;
                    TurnSpeed = 2f;
                    ProjectileSpeed = 1000f;
                    JumpHeight = 1050f;
                    CooldownTime = 1f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = true;
                    Scale = EnemyEV.LastBoss_Advanced_Scale;
                    ProjectileScale = EnemyEV.LastBoss_Advanced_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.LastBoss_Advanced_Tint;
                    MeleeRadius = 300;
                    EngageRadius = 925;
                    ProjectileRadius = 675;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.LastBoss_Advanced_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.EXPERT:
                    Name = "Johannes";
                    MaxHealth = 100;
                    Damage = 30;
                    XPValue = 1000;
                    MinMoneyDropAmount = 20;
                    MaxMoneyDropAmount = 30;
                    MoneyDropChance = 1f;
                    Speed = 550f;
                    TurnSpeed = 2f;
                    ProjectileSpeed = 1100f;
                    JumpHeight = 1050f;
                    CooldownTime = 1f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = false;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = true;
                    IsWeighted = true;
                    Scale = EnemyEV.LastBoss_Expert_Scale;
                    ProjectileScale = EnemyEV.LastBoss_Expert_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.LastBoss_Expert_Tint;
                    MeleeRadius = 300;
                    ProjectileRadius = 650;
                    EngageRadius = 900;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.LastBoss_Expert_KnockBack;
                    return;
                case GameTypes.EnemyDifficulty.MINIBOSS:
                    Name = "Fountain of Youth";
                    MaxHealth = 100;
                    Damage = 38;
                    XPValue = 5000;
                    MinMoneyDropAmount = 20;
                    MaxMoneyDropAmount = 30;
                    MoneyDropChance = 1f;
                    Speed = 125f;
                    TurnSpeed = 2f;
                    ProjectileSpeed = 1000f;
                    JumpHeight = 1050f;
                    CooldownTime = 1f;
                    AnimationDelay = 0.1f;
                    AlwaysFaceTarget = true;
                    CanFallOffLedges = false;
                    CanBeKnockedBack = false;
                    IsWeighted = true;
                    Scale = EnemyEV.LastBoss_Miniboss_Scale;
                    ProjectileScale = EnemyEV.LastBoss_Miniboss_ProjectileScale;
                    TintablePart.TextureColor = EnemyEV.LastBoss_Miniboss_Tint;
                    MeleeRadius = 325;
                    ProjectileRadius = 675;
                    EngageRadius = 925;
                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.LastBoss_Miniboss_KnockBack;
                    return;
            }
            AnimationDelay = 0.1f;
            if (LevelEV.WEAKEN_BOSSES)
            {
                MaxHealth = 1;
            }
        }

        protected override void InitializeLogic()
        {
            LogicSet logicSet = new LogicSet(this);
            logicSet.AddAction(new DebugTraceLogicAction("WalkTowardSLS"));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet.AddAction(new GroundCheckLogicAction());
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
            logicSet.AddAction(new MoveLogicAction(m_target, true));
            logicSet.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet.AddAction(new DelayLogicAction(0.3f, 0.75f));
            logicSet.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet2 = new LogicSet(this);
            logicSet2.AddAction(new DebugTraceLogicAction("WalkAway"));
            logicSet2.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet2.AddAction(new GroundCheckLogicAction());
            logicSet2.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
            logicSet2.AddAction(new MoveLogicAction(m_target, false));
            logicSet2.AddAction(new DelayLogicAction(0.2f, 0.75f));
            logicSet2.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet3 = new LogicSet(this);
            logicSet3.AddAction(new DebugTraceLogicAction("walkStop"));
            logicSet3.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet3.AddAction(new GroundCheckLogicAction());
            logicSet3.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character"));
            logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet3.AddAction(new DelayLogicAction(0.25f, 0.5f));
            LogicSet logicSet4 = new LogicSet(this);
            logicSet4.AddAction(new DebugTraceLogicAction("attack"));
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet4.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.05f));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet4.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false));
            logicSet4.AddAction(new PlayAnimationLogicAction(2, 4));
            logicSet4.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Attack01", "Player_Attack02"));
            logicSet4.AddAction(new PlayAnimationLogicAction("AttackStart", "End"));
            logicSet4.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet4.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character"));
            logicSet4.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet4.Tag = 2;
            LogicSet logicSet5 = new LogicSet(this);
            logicSet5.AddAction(new DebugTraceLogicAction("moveattack"));
            logicSet5.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet5.AddAction(new MoveLogicAction(m_target, true));
            logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.05f));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet5.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false));
            logicSet5.AddAction(new PlayAnimationLogicAction(2, 4));
            logicSet5.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Attack01", "Player_Attack02"));
            logicSet5.AddAction(new PlayAnimationLogicAction("AttackStart", "End"));
            logicSet5.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet5.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character"));
            logicSet5.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet5.Tag = 2;
            LogicSet logicSet6 = new LogicSet(this);
            logicSet6.AddAction(new DebugTraceLogicAction("Throwing Daggers"));
            logicSet6.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet6.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character"));
            logicSet6.AddAction(new PlayAnimationLogicAction(false));
            logicSet6.AddAction(new RunFunctionLogicAction(this, "CastCloseShield"));
            logicSet6.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet6.Tag = 2;
            LogicSet logicSet7 = new LogicSet(this);
            logicSet7.AddAction(new DebugTraceLogicAction("Throwing Daggers"));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet7.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", false));
            logicSet7.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character"));
            logicSet7.AddAction(new PlayAnimationLogicAction(false));
            logicSet7.AddAction(new RunFunctionLogicAction(this, "ThrowDaggerProjectiles"));
            logicSet7.AddAction(new DelayLogicAction(0.25f));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet7.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet7.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet7.Tag = 2;
            LogicSet logicSet8 = new LogicSet(this);
            logicSet8.AddAction(new DebugTraceLogicAction("Throwing Daggers"));
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet8.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.0333333351f));
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", false));
            logicSet8.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character"));
            logicSet8.AddAction(new PlayAnimationLogicAction(false));
            logicSet8.AddAction(new RunFunctionLogicAction(this, "ThrowDaggerProjectilesNeo"));
            logicSet8.AddAction(new DelayLogicAction(0.25f));
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet8.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet8.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 0.1f));
            logicSet8.Tag = 2;
            LogicSet logicSet9 = new LogicSet(this);
            logicSet9.AddAction(new DebugTraceLogicAction("jumpLS"));
            logicSet9.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet9.AddAction(new GroundCheckLogicAction());
            logicSet9.AddAction(new MoveLogicAction(m_target, true));
            logicSet9.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet9.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Jump"));
            logicSet9.AddAction(new JumpLogicAction());
            logicSet9.AddAction(new DelayLogicAction(0.2f));
            logicSet9.AddAction(new RunFunctionLogicAction(this, "ThrowAxeProjectiles"));
            logicSet9.AddAction(new DelayLogicAction(0.75f));
            logicSet9.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet9.AddAction(new GroundCheckLogicAction());
            LogicSet logicSet10 = new LogicSet(this);
            logicSet10.AddAction(new DebugTraceLogicAction("jumpLS"));
            logicSet10.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
            logicSet10.AddAction(new GroundCheckLogicAction());
            logicSet10.AddAction(new MoveLogicAction(m_target, true));
            logicSet10.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet10.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Jump"));
            logicSet10.AddAction(new JumpLogicAction());
            logicSet10.AddAction(new DelayLogicAction(0.2f));
            logicSet10.AddAction(new RunFunctionLogicAction(this, "ThrowAxeProjectilesNeo"));
            logicSet10.AddAction(new DelayLogicAction(0.75f));
            logicSet10.AddAction(new LockFaceDirectionLogicAction(false));
            logicSet10.AddAction(new GroundCheckLogicAction());
            LogicSet logicSet11 = new LogicSet(this);
            logicSet11.AddAction(new DebugTraceLogicAction("dashLS"));
            logicSet11.AddAction(new RunFunctionLogicAction(this, "CastCloseShield"));
            logicSet11.AddAction(new RunFunctionLogicAction(this, "Dash", 0));
            logicSet11.AddAction(new DelayLogicAction(0.25f));
            logicSet11.AddAction(new RunFunctionLogicAction(this, "DashComplete"));
            LogicSet logicSet12 = new LogicSet(this);
            logicSet12.AddAction(new DebugTraceLogicAction("dashAwayRightLS"));
            logicSet12.AddAction(new RunFunctionLogicAction(this, "Dash", 1));
            logicSet12.AddAction(new DelayLogicAction(0.25f));
            logicSet12.AddAction(new RunFunctionLogicAction(this, "DashComplete"));
            LogicSet logicSet13 = new LogicSet(this);
            logicSet13.AddAction(new DebugTraceLogicAction("dashAwayLeftLS"));
            logicSet13.AddAction(new RunFunctionLogicAction(this, "Dash", -1));
            logicSet13.AddAction(new DelayLogicAction(0.25f));
            logicSet13.AddAction(new RunFunctionLogicAction(this, "DashComplete"));
            LogicSet logicSet14 = new LogicSet(this);
            logicSet14.AddAction(new GroundCheckLogicAction());
            logicSet14.AddAction(new ChangeSpriteLogicAction("EnemyLastBossRun_Character"));
            logicSet14.AddAction(new MoveLogicAction(m_target, true));
            logicSet14.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet14.AddAction(new DelayLogicAction(0.35f, 1.15f));
            logicSet14.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet15 = new LogicSet(this);
            logicSet15.AddAction(new GroundCheckLogicAction());
            logicSet15.AddAction(new ChangeSpriteLogicAction("EnemyLastBossRun_Character"));
            logicSet15.AddAction(new MoveLogicAction(m_target, false));
            logicSet15.AddAction(new DelayLogicAction(0.2f, 1f));
            logicSet15.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet16 = new LogicSet(this);
            logicSet16.AddAction(new GroundCheckLogicAction());
            logicSet16.AddAction(new ChangeSpriteLogicAction("EnemyLastBossIdle_Character"));
            logicSet16.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet16.AddAction(new DelayLogicAction(0.2f, 0.5f));
            LogicSet logicSet17 = new LogicSet(this);
            logicSet17.AddAction(new MoveLogicAction(m_target, true, 0f));
            logicSet17.AddAction(new LockFaceDirectionLogicAction(true));
            logicSet17.AddAction(new ChangeSpriteLogicAction("EnemyLastBossAttack_Character", false, false));
            logicSet17.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack"));
            logicSet17.AddAction(new DelayLogicAction(m_lastBossAttackDelay));
            logicSet17.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSwing"));
            logicSet17.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_Effort_01",
                "FinalBoss_St2_Effort_02", "FinalBoss_St2_Effort_03", "FinalBoss_St2_Effort_04",
                "FinalBoss_St2_Effort_05"));
            logicSet17.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            logicSet17.AddAction(new ChangeSpriteLogicAction("EnemyLastBossIdle_Character"));
            logicSet17.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet18 = new LogicSet(this);
            RunTeleportLS(logicSet18, "Centre");
            logicSet18.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false));
            logicSet18.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam_Prime"));
            logicSet18.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast"));
            logicSet18.AddAction(new DelayLogicAction(m_castDelay));
            logicSet18.AddAction(new RunFunctionLogicAction(this, "CastSpears", m_numSpears, m_spearDuration));
            logicSet18.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam"));
            logicSet18.AddAction(new PlayAnimationLogicAction("BeforeCast", "End"));
            logicSet18.AddAction(new DelayLogicAction(m_spearDuration + 1f));
            LogicSet logicSet19 = new LogicSet(this);
            logicSet19.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
            logicSet19.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell2_Character", false, false));
            logicSet19.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSummon_a"));
            logicSet19.AddAction(new PlayAnimationLogicAction("Start", "Cast"));
            logicSet19.AddAction(new DelayLogicAction(m_castDelay));
            logicSet19.AddAction(new RunFunctionLogicAction(this, "CastSwordsRandom"));
            logicSet19.AddAction(new PlayAnimationLogicAction("Cast", "End"));
            logicSet19.AddAction(new DelayLogicAction(1f));
            LogicSet logicSet20 = new LogicSet(this);
            logicSet20.AddAction(new LockFaceDirectionLogicAction(true, 1));
            RunTeleportLS(logicSet20, "Left");
            logicSet20.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false));
            logicSet20.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam_Prime"));
            logicSet20.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast"));
            logicSet20.AddAction(new DelayLogicAction(m_castDelay));
            logicSet20.AddAction(new RunFunctionLogicAction(this, "CastSwords", true));
            logicSet20.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam"));
            logicSet20.AddAction(new PlayAnimationLogicAction("BeforeCast", "End"));
            logicSet20.AddAction(new DelayLogicAction(1f));
            logicSet20.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet21 = new LogicSet(this);
            logicSet21.AddAction(new LockFaceDirectionLogicAction(true, -1));
            RunTeleportLS(logicSet21, "Right");
            logicSet21.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false));
            logicSet21.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam_Prime"));
            logicSet21.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast"));
            logicSet21.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_BlockLaugh"));
            logicSet21.AddAction(new DelayLogicAction(m_castDelay));
            logicSet21.AddAction(new RunFunctionLogicAction(this, "CastSwords", false));
            logicSet21.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam"));
            logicSet21.AddAction(new PlayAnimationLogicAction("BeforeCast", "End"));
            logicSet21.AddAction(new DelayLogicAction(1f));
            logicSet21.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet22 = new LogicSet(this);
            logicSet22.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", m_orbsEasy));
            logicSet22.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet23 = new LogicSet(this);
            logicSet23.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", m_orbsNormal));
            logicSet23.AddAction(new LockFaceDirectionLogicAction(false));
            LogicSet logicSet24 = new LogicSet(this);
            logicSet24.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", m_orbsHard));
            logicSet24.AddAction(new DelayLogicAction(0f));
            logicSet24.AddAction(new LockFaceDirectionLogicAction(false));
            m_generalBasicLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet9, logicSet5, logicSet6, logicSet7,
                logicSet11);
            m_generalAdvancedLB.AddLogicSet(logicSet14, logicSet15, logicSet16, logicSet17, logicSet18, logicSet19,
                logicSet20, logicSet21);
            m_damageShieldLB.AddLogicSet(logicSet22, logicSet23, logicSet24);
            m_cooldownLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet9, logicSet5, logicSet6, logicSet7,
                logicSet11);
            m_secondFormCooldownLB.AddLogicSet(logicSet14, logicSet15, logicSet16);
            m_generalBasicNeoLB.AddLogicSet(logicSet, logicSet2, logicSet3, logicSet10, logicSet5, logicSet6, logicSet8,
                logicSet11);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_damageShieldLB);
            logicBlocksToDispose.Add(m_cooldownLB);
            logicBlocksToDispose.Add(m_secondFormCooldownLB);
            logicBlocksToDispose.Add(m_generalBasicNeoLB);
            m_firstFormDashAwayLB.AddLogicSet(logicSet13, logicSet12);
            logicBlocksToDispose.Add(m_firstFormDashAwayLB);
            LogicBlock arg_139D_1 = m_cooldownLB;
            int[] array = new int[8];
            array[0] = 70;
            array[2] = 30;
            SetCooldownLogicBlock(arg_139D_1, array);
            base.InitializeLogic();
        }

        private void RunTeleportLS(LogicSet logicSet, string roomPosition)
        {
            logicSet.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", false));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Opacity", 0.5f));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
            logicSet.AddAction(new ChangeSpriteLogicAction("EnemyLastBossTeleport_Character", false, false));
            logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_BlockAction"));
            logicSet.AddAction(new DelayLogicAction(0.25f));
            logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportTo", roomPosition));
            logicSet.AddAction(new DelayObjLogicAction(m_delayObj));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true));
            logicSet.AddAction(new ChangePropertyLogicAction(this, "Opacity", 1));
        }

        public void ThrowAxeProjectiles()
        {
            if (m_axeProjData != null)
            {
                m_axeProjData.Dispose();
                m_axeProjData = null;
            }
            m_axeProjData = new ProjectileData(this)
            {
                SpriteName = "SpellAxe_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(AxeProjectileSpeed, AxeProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-90f, -90f),
                CollidesWithTerrain = false,
                Scale = AxeSpellScale
            };
            Tween.RunFunction(0f, this, "CastAxe", false);
            Tween.RunFunction(0.15f, this, "CastAxe", true);
            Tween.RunFunction(0.3f, this, "CastAxe", true);
            Tween.RunFunction(0.45f, this, "CastAxe", true);
            Tween.RunFunction(0.6f, this, "CastAxe", true);
        }

        public void ThrowAxeProjectilesNeo()
        {
            if (m_axeProjData != null)
            {
                m_axeProjData.Dispose();
                m_axeProjData = null;
            }
            m_axeProjData = new ProjectileData(this)
            {
                SpriteName = "SpellAxe_Sprite",
                SourceAnchor = new Vector2(20f, -20f),
                Target = null,
                Speed = new Vector2(AxeProjectileSpeed, AxeProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10f,
                Damage = Damage,
                AngleOffset = 0f,
                Angle = new Vector2(-90f, -90f),
                CollidesWithTerrain = false,
                Scale = AxeSpellScale
            };
            Tween.RunFunction(0.3f, this, "CastAxe", true);
            Tween.RunFunction(0.3f, this, "CastAxe", true);
            Tween.RunFunction(0.3f, this, "CastAxe", true);
        }

        public void CastAxe(bool randomize)
        {
            if (randomize)
            {
                m_axeProjData.AngleOffset = CDGMath.RandomInt(-70, 70);
            }
            m_levelScreen.ProjectileManager.FireProjectile(m_axeProjData);
            SoundManager.Play3DSound(this, m_target, "Cast_Axe");
            m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 45f, true);
        }

        public void ThrowDaggerProjectiles()
        {
            if (m_daggerProjData != null)
            {
                m_daggerProjData.Dispose();
                m_daggerProjData = null;
            }
            m_daggerProjData = new ProjectileData(this)
            {
                SpriteName = "SpellDagger_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = m_target,
                Speed = new Vector2(DaggerProjectileSpeed, DaggerProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = DaggerSpellScale
            };
            Tween.RunFunction(0f, this, "CastDaggers", false);
            Tween.RunFunction(0.05f, this, "CastDaggers", true);
            Tween.RunFunction(0.1f, this, "CastDaggers", true);
            Tween.RunFunction(0.15f, this, "CastDaggers", true);
            Tween.RunFunction(0.2f, this, "CastDaggers", true);
        }

        public void ThrowDaggerProjectilesNeo()
        {
            if (m_daggerProjData != null)
            {
                m_daggerProjData.Dispose();
                m_daggerProjData = null;
            }
            m_daggerProjData = new ProjectileData(this)
            {
                SpriteName = "SpellDagger_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = m_target,
                Speed = new Vector2(DaggerProjectileSpeed - 160f, DaggerProjectileSpeed - 160f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                Scale = DaggerSpellScale
            };
            Tween.RunFunction(0f, this, "CastDaggers", false);
            Tween.RunFunction(0.05f, this, "CastDaggers", true);
            Tween.RunFunction(0.1f, this, "CastDaggers", true);
        }

        public void CastDaggers(bool randomize)
        {
            if (randomize)
            {
                m_daggerProjData.AngleOffset = CDGMath.RandomInt(-8, 8);
            }
            m_levelScreen.ProjectileManager.FireProjectile(m_daggerProjData);
            SoundManager.Play3DSound(this, m_target, "Cast_Dagger");
            m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 0f, true);
        }

        public void CastCloseShield()
        {
            ProjectileData projectileData = new ProjectileData(this)
            {
                SpriteName = "SpellClose_Sprite",
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                DestroysWithEnemy = false,
                DestroysWithTerrain = false,
                CollidesWithTerrain = false,
                Scale = new Vector2(m_Spell_Close_Scale, m_Spell_Close_Scale),
                Damage = Damage,
                Lifespan = m_Spell_Close_Lifespan,
                LockPosition = true
            };
            m_levelScreen.ProjectileManager.FireProjectile(projectileData);
            SoundManager.Play3DSound(this, m_target, "Cast_GiantSword");
            m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 90f, true);
            projectileData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            if (CurrentHealth > 0)
            {
                if (!m_inSecondForm)
                {
                    if (!m_isHurt)
                    {
                        switch (State)
                        {
                            case 0:
                            {
                                if (!IsNeo)
                                {
                                    bool arg_182_1 = true;
                                    LogicBlock arg_182_2 = m_generalBasicLB;
                                    int[] array = new int[8];
                                    array[0] = 50;
                                    array[6] = 50;
                                    RunLogicBlock(arg_182_1, arg_182_2, array);
                                    return;
                                }
                                bool arg_1B2_1 = true;
                                LogicBlock arg_1B2_2 = m_generalBasicNeoLB;
                                int[] array2 = new int[8];
                                array2[0] = 50;
                                array2[2] = 10;
                                array2[3] = 10;
                                array2[6] = 30;
                                RunLogicBlock(arg_1B2_1, arg_1B2_2, array2);
                                return;
                            }
                            case 1:
                            {
                                if (!IsNeo)
                                {
                                    bool arg_126_1 = true;
                                    LogicBlock arg_126_2 = m_generalBasicLB;
                                    int[] array3 = new int[8];
                                    array3[0] = 40;
                                    array3[3] = 20;
                                    array3[6] = 40;
                                    RunLogicBlock(arg_126_1, arg_126_2, array3);
                                    return;
                                }
                                bool arg_156_1 = true;
                                LogicBlock arg_156_2 = m_generalBasicNeoLB;
                                int[] array4 = new int[8];
                                array4[0] = 40;
                                array4[2] = 20;
                                array4[3] = 20;
                                array4[6] = 20;
                                RunLogicBlock(arg_156_1, arg_156_2, array4);
                                return;
                            }
                            case 2:
                                if (!IsNeo)
                                {
                                    RunLogicBlock(true, m_generalBasicLB, 35, 0, 0, 25, 0, 0, 20, 20);
                                    return;
                                }
                                RunLogicBlock(true, m_generalBasicNeoLB, 25, 0, 20, 15, 0, 0, 15, 25);
                                return;
                            case 3:
                                if (!IsNeo)
                                {
                                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 0, 35, 35, 0, 0, 30);
                                    return;
                                }
                                RunLogicBlock(true, m_generalBasicNeoLB, 0, 0, 0, 50, 20, 0, 0, 30);
                                return;
                            default:
                                return;
                        }
                    }
                }
                else
                {
                    RunAdvancedLogic();
                }
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case 0:
                    RunLogicBlock(true, m_generalAdvancedLB, 63, 0, 0, 0, 15, 12, 5, 5);
                    return;
                case 1:
                    RunLogicBlock(true, m_generalAdvancedLB, 68, 0, 0, 0, 10, 12, 5, 5);
                    return;
                case 2:
                    RunLogicBlock(true, m_generalAdvancedLB, 52, 12, 0, 0, 11, 15, 5, 5);
                    return;
                case 3:
                    RunLogicBlock(true, m_generalAdvancedLB, 31, 15, 0, 26, 3, 13, 6, 6);
                    return;
                default:
                    return;
            }
        }

        protected override void RunExpertLogic()
        {
            if (CurrentHealth > 0)
            {
                if (!m_inSecondForm)
                {
                    if (!m_isHurt)
                    {
                        switch (State)
                        {
                            case 0:
                            {
                                if (!m_target.IsJumping)
                                {
                                    bool arg_1C2_1 = true;
                                    LogicBlock arg_1C2_2 = m_generalBasicLB;
                                    int[] array = new int[8];
                                    array[0] = 50;
                                    array[2] = 10;
                                    array[3] = 20;
                                    array[6] = 20;
                                    RunLogicBlock(arg_1C2_1, arg_1C2_2, array);
                                    return;
                                }
                                bool arg_1F2_1 = true;
                                LogicBlock arg_1F2_2 = m_generalBasicLB;
                                int[] array2 = new int[8];
                                array2[0] = 50;
                                array2[2] = 10;
                                array2[3] = 20;
                                array2[6] = 20;
                                RunLogicBlock(arg_1F2_1, arg_1F2_2, array2);
                                return;
                            }
                            case 1:
                                if (!m_target.IsJumping)
                                {
                                    RunLogicBlock(true, m_generalBasicLB, 30, 0, 15, 20, 0, 25, 0, 10);
                                    return;
                                }
                                RunLogicBlock(true, m_generalBasicLB, 50, 0, 15, 0, 0, 25, 0, 10);
                                return;
                            case 2:
                                if (!m_target.IsJumping)
                                {
                                    RunLogicBlock(true, m_generalBasicLB, 20, 0, 10, 10, 0, 15, 20, 10);
                                    return;
                                }
                                RunLogicBlock(true, m_generalBasicLB, 40, 0, 15, 0, 0, 15, 20, 10);
                                return;
                            case 3:
                                if (m_isTouchingGround)
                                {
                                    RunLogicBlock(true, m_generalBasicLB, 0, 10, 0, 20, 35, 10, 0, 25);
                                    return;
                                }
                                RunLogicBlock(true, m_generalBasicLB, 0, 10, 0, 0, 55, 10, 0, 25);
                                return;
                            default:
                                return;
                        }
                    }
                }
                else
                {
                    RunAdvancedLogic();
                }
            }
        }

        protected override void RunMinibossLogic()
        {
            bool arg_15_1 = true;
            LogicBlock arg_15_2 = m_generalAdvancedLB;
            int[] array = new int[8];
            array[4] = 100;
            RunLogicBlock(arg_15_1, arg_15_2, array);
        }

        public void TeleportTo(string roomPosition)
        {
            Vector2 zero = Vector2.Zero;
            float x = 0f;
            if (roomPosition != null)
            {
                if (!(roomPosition == "Left"))
                {
                    if (!(roomPosition == "Right"))
                    {
                        if (roomPosition == "Centre")
                        {
                            x = m_levelScreen.CurrentRoom.Bounds.Center.X;
                        }
                    }
                    else
                    {
                        x = m_levelScreen.CurrentRoom.Bounds.Right - 200;
                    }
                }
                else
                {
                    x = m_levelScreen.CurrentRoom.Bounds.Left + 200;
                }
            }
            zero = new Vector2(x, Y);
            float num = Math.Abs(CDGMath.DistanceBetweenPts(Position, zero));
            m_teleportDuration = num*0.001f;
            m_delayObj.X = m_teleportDuration;
            Tween.To(this, m_teleportDuration, Quad.EaseInOut, "X", zero.X.ToString());
            SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_BlockMove");
        }

        public void CastSwords(bool castLeft)
        {
            ProjectileData data = new ProjectileData(this)
            {
                SpriteName = "LastBossSwordProjectile_Sprite",
                Target = null,
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                StartingRotation = 0f,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false
            };
            float num = 1f;
            int num2 = MegaFlyingDaggerProjectileSpeed;
            if (!castLeft)
            {
                num2 = MegaFlyingDaggerProjectileSpeed*-1;
            }
            SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_SwordSummon_b");
            for (int i = 0; i < MegaFlyingSwordAmount; i++)
            {
                Vector2 vector = new Vector2(X, Y + CDGMath.RandomInt(-1320, 100));
                ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(data);
                projectileObj.Position = vector;
                Tween.By(projectileObj, 2.5f, Tween.EaseNone, "delay", num.ToString(), "X", num2.ToString());
                Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile");
                Tween.RunFunction(num, typeof (SoundManager), "Play3DSound", this, m_target, new[]
                {
                    "FinalBoss_St2_SwordSummon_c_01",
                    "FinalBoss_St2_SwordSummon_c_02",
                    "FinalBoss_St2_SwordSummon_c_03",
                    "FinalBoss_St2_SwordSummon_c_04",
                    "FinalBoss_St2_SwordSummon_c_05",
                    "FinalBoss_St2_SwordSummon_c_06",
                    "FinalBoss_St2_SwordSummon_c_07",
                    "FinalBoss_St2_SwordSummon_c_08"
                });
                m_levelScreen.ImpactEffectPool.SpellCastEffect(vector, 0f, false);
                num += 0.075f;
            }
        }

        public void CastSpears(int numSpears, float duration)
        {
            ProjectileData projectileData = new ProjectileData(this)
            {
                SpriteName = "LastBossSpearProjectile_Sprite",
                Target = null,
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                StartingRotation = 0f,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false,
                ShowIcon = false,
                LockPosition = true,
                CanBeFusRohDahed = false
            };
            int num = 0;
            int num2 = 0;
            float num3 = 0.5f;
            UpdateCollisionBoxes();
            Vector2 vector = new Vector2(m_levelScreen.CurrentRoom.Bounds.Center.X, Y);
            for (int i = 0; i < numSpears; i++)
            {
                ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                projectileObj.Scale = new Vector2(2f, 2f);
                projectileObj.X = vector.X + 50f + num;
                projectileObj.Y = Y + (Bounds.Bottom - Y);
                projectileObj.StopAnimation();
                num += projectileObj.Width;
                Tween.RunFunction(num3, typeof (SoundManager), "Play3DSound", this, m_target, new[]
                {
                    "FinalBoss_St2_Lance_01",
                    "FinalBoss_St2_Lance_02",
                    "FinalBoss_St2_Lance_03",
                    "FinalBoss_St2_Lance_04",
                    "FinalBoss_St2_Lance_05",
                    "FinalBoss_St2_Lance_06",
                    "FinalBoss_St2_Lance_07",
                    "FinalBoss_St2_Lance_08"
                });
                Tween.RunFunction(num3, projectileObj, "PlayAnimation", "Before", "End", false);
                Tween.RunFunction(num3 + duration, projectileObj, "PlayAnimation", "Retract", "RetractComplete", false);
                Tween.RunFunction(num3 + duration, typeof (SoundManager), "Play3DSound", this, m_target, new[]
                {
                    "FinalBoss_St2_Lance_Retract_01",
                    "FinalBoss_St2_Lance_Retract_02",
                    "FinalBoss_St2_Lance_Retract_03",
                    "FinalBoss_St2_Lance_Retract_04",
                    "FinalBoss_St2_Lance_Retract_05",
                    "FinalBoss_St2_Lance_Retract_06"
                });
                Tween.RunFunction(num3 + duration + 1f, projectileObj, "KillProjectile");
                ProjectileObj projectileObj2 = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                projectileObj2.Scale = new Vector2(2f, 2f);
                projectileObj2.X = vector.X - 50f + num2;
                projectileObj2.Y = Y + (Bounds.Bottom - Y);
                projectileObj2.StopAnimation();
                num2 -= projectileObj2.Width;
                Tween.RunFunction(num3, projectileObj2, "PlayAnimation", "Before", "End", false);
                Tween.RunFunction(num3 + duration, projectileObj2, "PlayAnimation", "Retract", "RetractComplete", false);
                Tween.RunFunction(num3 + duration + 1f, projectileObj2, "KillProjectile");
                num3 += 0.05f;
            }
            projectileData.Dispose();
        }

        public void CastSwordsRandom()
        {
            Vector2 vector = new Vector2(m_levelScreen.CurrentRoom.Bounds.Center.X, Y);
            UpdateCollisionBoxes();
            ProjectileData projectileData = new ProjectileData(this)
            {
                SpriteName = "LastBossSwordVerticalProjectile_Sprite",
                Target = null,
                Speed = new Vector2(0f, 0f),
                IsWeighted = false,
                RotationSpeed = 0f,
                Damage = Damage,
                StartingRotation = 0f,
                AngleOffset = 0f,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false,
                LockPosition = true
            };
            int megaUpwardSwordProjectileSpeed = MegaUpwardSwordProjectileSpeed;
            int num = 0;
            int num2 = 0;
            float num3 = 1f;
            for (int i = 0; i < MegaUpwardSwordProjectileAmount; i++)
            {
                ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                projectileObj.Scale = new Vector2(1.5f, 1.5f);
                projectileObj.X = vector.X + 50f + num;
                projectileObj.Y = vector.Y + (Bounds.Bottom - Y) + 120f;
                projectileObj.Opacity = 0f;
                Tween.To(projectileObj, 0.25f, Tween.EaseNone, "Opacity", "1");
                Tween.By(projectileObj, 2.5f, Quad.EaseIn, "delay", num3.ToString(), "Y",
                    (-megaUpwardSwordProjectileSpeed).ToString());
                Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile");
                num = CDGMath.RandomInt(50, 1000);
                ProjectileObj projectileObj2 = m_levelScreen.ProjectileManager.FireProjectile(projectileData);
                projectileObj2.Scale = new Vector2(2f, 2f);
                projectileObj2.X = vector.X - 50f + num2;
                projectileObj2.Y = vector.Y + (Bounds.Bottom - Y) + 120f;
                projectileObj2.Opacity = 0f;
                Tween.To(projectileObj2, 0.25f, Tween.EaseNone, "Opacity", "1");
                Tween.By(projectileObj2, 2.5f, Quad.EaseIn, "delay", num3.ToString(), "Y",
                    (-megaUpwardSwordProjectileSpeed).ToString());
                Tween.AddEndHandlerToLastTween(projectileObj, "KillProjectile");
                num2 = -CDGMath.RandomInt(50, 1000);
                num3 += 0.25f;
            }
            projectileData.Dispose();
        }

        public void ChangeProjectileSpeed(ProjectileObj proj, float speed, Vector2 heading)
        {
            proj.AccelerationX = heading.X*speed;
            proj.AccelerationY = -heading.Y*speed;
        }

        public void CastDamageShield(int numOrbs)
        {
            foreach (ProjectileObj current in m_damageShieldProjectiles)
            {
                current.KillProjectile();
            }
            m_damageShieldProjectiles.Clear();
            ProjectileData data = new ProjectileData(this)
            {
                SpriteName = "LastBossOrbProjectile_Sprite",
                Angle = new Vector2(-65f, -65f),
                Speed = new Vector2(m_Mega_Shield_Speed, m_Mega_Shield_Speed),
                Target = this,
                IsWeighted = false,
                RotationSpeed = 0f,
                CollidesWithTerrain = false,
                DestroysWithTerrain = false,
                DestroysWithEnemy = false,
                CanBeFusRohDahed = false,
                ShowIcon = false,
                Lifespan = 9999f,
                Damage = Damage/2
            };
            SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_SwordSummon_b");
            int mega_Shield_Distance = m_Mega_Shield_Distance;
            for (int i = 0; i < numOrbs; i++)
            {
                float num = 360f/numOrbs*i;
                ProjectileObj projectileObj = m_levelScreen.ProjectileManager.FireProjectile(data);
                projectileObj.AltX = num;
                projectileObj.AltY = mega_Shield_Distance;
                projectileObj.Spell = 11;
                projectileObj.AccelerationXEnabled = false;
                projectileObj.AccelerationYEnabled = false;
                projectileObj.IgnoreBoundsCheck = true;
                projectileObj.Scale = new Vector2(m_Mega_Shield_Scale, m_Mega_Shield_Scale);
                projectileObj.Position = CDGMath.GetCirclePosition(num, mega_Shield_Distance, Position);
                m_levelScreen.ImpactEffectPool.SpellCastEffect(projectileObj.Position, projectileObj.Rotation, false);
                m_damageShieldProjectiles.Add(projectileObj);
            }
        }

        public void Dash(int heading)
        {
            HeadingY = 0f;
            if (m_target.Position.X < X)
            {
                if (heading == 0)
                {
                    HeadingX = 1f;
                }
                if (Flip == SpriteEffects.None)
                {
                    ChangeSprite("PlayerFrontDash_Character");
                }
                else
                {
                    ChangeSprite("PlayerDash_Character");
                }
                m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(X, TerrainBounds.Bottom), false);
            }
            else
            {
                if (heading == 0)
                {
                    HeadingX = -1f;
                }
                if (Flip == SpriteEffects.None)
                {
                    ChangeSprite("PlayerDash_Character");
                }
                else
                {
                    ChangeSprite("PlayerFrontDash_Character");
                }
                m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(X, TerrainBounds.Bottom), true);
            }
            if (heading != 0)
            {
                HeadingX = heading;
            }
            SoundManager.Play3DSound(this, m_target, "Player_Dash");
            LockFlip = true;
            AccelerationX = 0f;
            AccelerationY = 0f;
            PlayAnimation(false);
            CurrentSpeed = 900f;
            AccelerationYEnabled = false;
            m_isDashing = true;
        }

        public void DashComplete()
        {
            LockFlip = false;
            CurrentSpeed = 500f;
            AccelerationYEnabled = true;
            m_isDashing = false;
            AnimationDelay = 0.1f;
        }

        public override void Update(GameTime gameTime)
        {
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
            if (m_smokeCounter > 0f && !m_inSecondForm)
            {
                m_smokeCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_smokeCounter <= 0f)
                {
                    m_smokeCounter = 0.25f;
                    if (CurrentSpeed > 0f)
                    {
                        m_smokeCounter = 0.05f;
                    }
                    m_levelScreen.ImpactEffectPool.BlackSmokeEffect(this);
                }
            }
            if (!m_inSecondForm)
            {
                if (!m_isTouchingGround && m_currentActiveLB != null && SpriteName != "PlayerAttacking3_Character" &&
                    !m_isDashing && SpriteName != "PlayerLevelUp_Character")
                {
                    if (AccelerationY < 0f && SpriteName != "PlayerJumping_Character")
                    {
                        ChangeSprite("PlayerJumping_Character");
                        PlayAnimation();
                    }
                    else if (AccelerationY > 0f && SpriteName != "PlayerFalling_Character")
                    {
                        ChangeSprite("PlayerFalling_Character");
                        PlayAnimation();
                    }
                }
                else if (m_isTouchingGround && m_currentActiveLB != null && SpriteName == "PlayerAttacking3_Character" &&
                         CurrentSpeed != 0f)
                {
                    SpriteObj spriteObj = GetChildAt(2) as SpriteObj;
                    if (spriteObj.SpriteName != "PlayerWalkingLegs_Sprite")
                    {
                        spriteObj.ChangeSprite("PlayerWalkingLegs_Sprite");
                        spriteObj.PlayAnimation(CurrentFrame, TotalFrames);
                        spriteObj.Y += 4f;
                        spriteObj.OverrideParentAnimationDelay = true;
                        spriteObj.AnimationDelay = 0.1f;
                    }
                }
            }
            else if (SpriteName == "EnemyLastBossRun_Character")
            {
                m_walkUpSoundFinalBoss.Update();
                m_walkDownSoundFinalBoss.Update();
            }
            if (!m_inSecondForm && CurrentHealth <= 0 && m_target.CurrentHealth > 0 && !IsNeo)
            {
                if (IsTouchingGround && !m_firstFormDying)
                {
                    m_firstFormDying = true;
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 2, 0.1f);
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 2, 0.1f);
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 2, 0.1f);
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 2, 0.1f);
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 3, 0.1f);
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 3, 0.1f);
                    m_levelScreen.ItemDropManager.DropItemWide(Position, 3, 0.1f);
                    IsWeighted = false;
                    IsCollidable = false;
                    AnimationDelay = 0.1f;
                    CurrentSpeed = 0f;
                    AccelerationX = 0f;
                    AccelerationY = 0f;
                    TextureColor = Color.White;
                    ChangeSprite("PlayerDeath_Character");
                    SoundManager.PlaySound("Boss_Flash");
                    SoundManager.StopMusic(1f);
                    m_target.StopAllSpells();
                    m_target.ForceInvincible = true;
                    if (m_target.X < X)
                    {
                        Flip = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        Flip = SpriteEffects.None;
                    }
                    if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
                    {
                        m_currentActiveLB.StopLogicBlock();
                    }
                }
                if (m_target.IsTouchingGround && !m_inSecondForm && SpriteName == "PlayerDeath_Character")
                {
                    MovePlayerTo();
                }
            }
            if ((!m_firstFormDying && !m_inSecondForm) || (m_firstFormDying && m_inSecondForm) ||
                (IsNeo && CurrentHealth > 0))
            {
                base.Update(gameTime);
            }
            if (!m_inSecondForm && CurrentHealth <= 0 && m_target.CurrentHealth > 0 && IsNeo && IsTouchingGround &&
                !m_firstFormDying)
            {
                KillPlayerNeo();
                m_firstFormDying = true;
            }
        }

        public void MovePlayerTo()
        {
            m_target.StopAllSpells();
            m_levelScreen.ProjectileManager.DestroyAllProjectiles(true);
            m_inSecondForm = true;
            m_isKilled = true;
            m_levelScreen.RunCinematicBorders(16f);
            m_currentActiveLB.StopLogicBlock();
            int num = 250;
            Vector2 zero = Vector2.Zero;
            if ((m_target.X < X && X > m_levelScreen.CurrentRoom.X + 500f) ||
                X > m_levelScreen.CurrentRoom.Bounds.Right - 500)
            {
                zero = new Vector2(X - num, Y);
                if (zero.X > m_levelScreen.CurrentRoom.Bounds.Right - 500)
                {
                    zero.X = m_levelScreen.CurrentRoom.Bounds.Right - 500;
                }
            }
            else
            {
                zero = new Vector2(X + num, Y);
            }
            m_target.Flip = SpriteEffects.None;
            if (zero.X < m_target.X)
            {
                m_target.Flip = SpriteEffects.FlipHorizontally;
            }
            float num2 = CDGMath.DistanceBetweenPts(m_target.Position, zero)/m_target.Speed;
            m_target.UpdateCollisionBoxes();
            m_target.State = 1;
            m_target.IsWeighted = false;
            m_target.AccelerationY = 0f;
            m_target.AccelerationX = 0f;
            m_target.IsCollidable = false;
            m_target.Y = m_levelScreen.CurrentRoom.Bounds.Bottom - 180 - (m_target.Bounds.Bottom - m_target.Y);
            m_target.CurrentSpeed = 0f;
            m_target.LockControls();
            m_target.ChangeSprite("PlayerWalking_Character");
            LogicSet logicSet = new LogicSet(m_target);
            logicSet.AddAction(new DelayLogicAction(num2));
            m_target.RunExternalLogicSet(logicSet);
            m_target.PlayAnimation();
            Tween.To(m_target, num2, Tween.EaseNone, "X", zero.X.ToString());
            Tween.AddEndHandlerToLastTween(this, "SecondFormDeath");
        }

        public void SecondFormDeath()
        {
            if (m_target.X < X)
            {
                m_target.Flip = SpriteEffects.None;
            }
            else
            {
                m_target.Flip = SpriteEffects.FlipHorizontally;
            }
            PlayAnimation(false);
            SoundManager.PlaySound("FinalBoss_St1_DeathGrunt");
            Tween.RunFunction(0.1f, typeof (SoundManager), "PlaySound", "Player_Death_SwordTwirl");
            Tween.RunFunction(0.7f, typeof (SoundManager), "PlaySound", "Player_Death_SwordLand");
            Tween.RunFunction(1.2f, typeof (SoundManager), "PlaySound", "Player_Death_BodyFall");
            float num = 2f;
            Tween.RunFunction(2f, this, "PlayBlackSmokeSounds");
            for (int i = 0; i < 30; i++)
            {
                Tween.RunFunction(num, m_levelScreen.ImpactEffectPool, "BlackSmokeEffect", Position,
                    new Vector2(1f + num*1f, 1f + num*1f));
                num += 0.05f;
            }
            Tween.RunFunction(3f, this, "HideEnemy");
            Tween.RunFunction(6f, this, "SecondFormDialogue");
        }

        public void PlayBlackSmokeSounds()
        {
            SoundManager.PlaySound("Cutsc_Smoke");
        }

        public void HideEnemy()
        {
            Visible = false;
        }

        public void SecondFormDialogue()
        {
            RCScreenManager rCScreenManager = m_levelScreen.ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk02");
            rCScreenManager.DialogueScreen.SetConfirmEndHandler(m_levelScreen.CurrentRoom, "RunFountainCutscene");
            rCScreenManager.DisplayScreen(13, true);
        }

        public void SecondFormComplete()
        {
            m_target.ForceInvincible = false;
            Level += 10;
            Flip = SpriteEffects.FlipHorizontally;
            Visible = true;
            MaxHealth = 530;
            Damage = 22;
            CurrentHealth = MaxHealth;
            Name = "The Fountain";
            if (LevelEV.WEAKEN_BOSSES)
            {
                CurrentHealth = 1;
            }
            MinMoneyDropAmount = 20;
            MaxMoneyDropAmount = 30;
            MoneyDropChance = 1f;
            Speed = 325f;
            TurnSpeed = 2f;
            ProjectileSpeed = 1000f;
            JumpHeight = 1050f;
            CooldownTime = 1f;
            AnimationDelay = 0.1f;
            AlwaysFaceTarget = true;
            CanFallOffLedges = false;
            CanBeKnockedBack = false;
            ProjectileScale = EnemyEV.LastBoss_Advanced_ProjectileScale;
            TintablePart.TextureColor = EnemyEV.LastBoss_Advanced_Tint;
            MeleeRadius = 300;
            EngageRadius = 925;
            ProjectileRadius = 675;
            ProjectileDamage = Damage;
            KnockBack = EnemyEV.LastBoss_Advanced_KnockBack;
            ChangeSprite("EnemyLastBossIdle_Character");
            SetCooldownLogicBlock(m_secondFormCooldownLB, 40, 20, 40);
            PlayAnimation();
            Name = "The Fountain";
            IsWeighted = true;
            IsCollidable = true;
        }

        public void SecondFormActive()
        {
            if (IsPaused)
            {
                UnpauseEnemy(true);
            }
            m_levelScreen.CameraLockedToPlayer = true;
            m_target.UnlockControls();
            m_target.IsWeighted = true;
            m_target.IsCollidable = true;
            m_isKilled = false;
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            if (!m_inSecondForm)
            {
                if (!m_isHurt && !m_isDashing)
                {
                    SoundManager.Play3DSound(this, m_target, "FinalBoss_St1_Dmg_01", "FinalBoss_St1_Dmg_02",
                        "FinalBoss_St1_Dmg_03", "FinalBoss_St1_Dmg_04");
                    base.HitEnemy(damage, collisionPt, isPlayer);
                }
            }
            else
            {
                SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_Hit_01", "FinalBoss_St2_Hit_03",
                    "FinalBoss_St2_Hit_04");
                SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_DmgVox_01", "FinalBoss_St2_DmgVox_02",
                    "FinalBoss_St2_DmgVox_03", "FinalBoss_St2_DmgVox_04", "FinalBoss_St2_DmgVox_05",
                    "FinalBoss_St2_DmgVox_06", "FinalBoss_St2_DmgVox_07", "FinalBoss_St2_DmgVox_08",
                    "FinalBoss_St2_DmgVox_09");
                base.HitEnemy(damage, collisionPt, isPlayer);
            }
        }

        public override void Kill(bool giveXP = true)
        {
            if (m_target.CurrentHealth > 0)
            {
                if (m_inSecondForm && !m_bossVersionKilled)
                {
                    m_bossVersionKilled = true;
                    SetPlayerData();
                    m_levelScreen.PauseScreen();
                    m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
                    m_target.StopAllSpells();
                    m_levelScreen.RunWhiteSlashEffect();
                    ChangeSprite("EnemyLastBossDeath_Character");
                    if (m_target.X < X)
                    {
                        Flip = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        Flip = SpriteEffects.None;
                    }
                    Tween.RunFunction(1f, this, "Part2");
                    SoundManager.PlaySound("Boss_Flash");
                    SoundManager.PlaySound("Boss_Eyeball_Freeze");
                    SoundManager.StopMusic();
                    m_target.LockControls();
                    GameUtil.UnlockAchievement("FEAR_OF_FATHERS");
                    if (Game.PlayerStats.TimesCastleBeaten > 1)
                    {
                        GameUtil.UnlockAchievement("FEAR_OF_TWINS");
                    }
                }
                if (IsNeo && !m_neoDying)
                {
                    m_neoDying = true;
                    m_levelScreen.PauseScreen();
                    SoundManager.PauseMusic();
                    m_levelScreen.RunWhiteSlashEffect();
                    SoundManager.PlaySound("Boss_Flash");
                    SoundManager.PlaySound("Boss_Eyeball_Freeze");
                    Tween.RunFunction(1f, m_levelScreen, "UnpauseScreen");
                    Tween.RunFunction(1f, typeof (SoundManager), "ResumeMusic");
                }
            }
        }

        public void KillPlayerNeo()
        {
            m_isKilled = true;
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
            {
                m_currentActiveLB.StopLogicBlock();
            }
            IsWeighted = false;
            IsCollidable = false;
            AnimationDelay = 0.1f;
            CurrentSpeed = 0f;
            AccelerationX = 0f;
            AccelerationY = 0f;
            ChangeSprite("PlayerDeath_Character");
            PlayAnimation(false);
            SoundManager.PlaySound("FinalBoss_St1_DeathGrunt");
            Tween.RunFunction(0.1f, typeof (SoundManager), "PlaySound", "Player_Death_SwordTwirl");
            Tween.RunFunction(0.7f, typeof (SoundManager), "PlaySound", "Player_Death_SwordLand");
            Tween.RunFunction(1.2f, typeof (SoundManager), "PlaySound", "Player_Death_BodyFall");
        }

        public void SetPlayerData()
        {
            FamilyTreeNode item = new FamilyTreeNode
            {
                Name = Game.PlayerStats.PlayerName,
                Age = Game.PlayerStats.Age,
                ChildAge = Game.PlayerStats.ChildAge,
                Class = Game.PlayerStats.Class,
                HeadPiece = Game.PlayerStats.HeadPiece,
                ChestPiece = Game.PlayerStats.ChestPiece,
                ShoulderPiece = Game.PlayerStats.ShoulderPiece,
                NumEnemiesBeaten = Game.PlayerStats.NumEnemiesBeaten,
                BeatenABoss = true,
                Traits = Game.PlayerStats.Traits,
                IsFemale = Game.PlayerStats.IsFemale
            };
            Game.PlayerStats.FamilyTreeArray.Add(item);
            Game.PlayerStats.NewBossBeaten = false;
            Game.PlayerStats.RerolledChildren = false;
            Game.PlayerStats.NumEnemiesBeaten = 0;
            Game.PlayerStats.LichHealth = 0;
            Game.PlayerStats.LichMana = 0;
            Game.PlayerStats.LichHealthMod = 1f;
            Game.PlayerStats.LoadStartingRoom = true;
            Game.PlayerStats.LastbossBeaten = true;
            Game.PlayerStats.CharacterFound = false;
            Game.PlayerStats.TimesCastleBeaten++;
            (m_target.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            Vector2 vector = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
            PlayerObj playerObj = otherBox.AbsParent as PlayerObj;
            if (playerObj != null && otherBox.Type == 0 && !playerObj.IsInvincible && playerObj.State != 3)
            {
                playerObj.HitPlayer(this);
            }
            if (m_isTouchingGround && m_isHurt)
            {
                m_isHurt = false;
                if (!m_inSecondForm)
                {
                    ChangeSprite("PlayerIdle_Character");
                }
            }
            if (!(otherBox.AbsParent is EnemyObj_Platform))
            {
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
            }
            TerrainObj terrainObj = otherBox.AbsParent as TerrainObj;
            if (terrainObj != null && !m_isTouchingGround && !(terrainObj is DoorObj) && !m_inSecondForm)
            {
                if (m_currentActiveLB != null && m_currentActiveLB.IsActive)
                {
                    m_currentActiveLB.StopLogicBlock();
                }
                if (vector.X > 0f)
                {
                    RunLogicBlock(true, m_firstFormDashAwayLB, 0, 100);
                    return;
                }
                if (vector.X < 0f)
                {
                    bool arg_11D_1 = true;
                    LogicBlock arg_11D_2 = m_firstFormDashAwayLB;
                    int[] array = new int[2];
                    array[0] = 100;
                    RunLogicBlock(arg_11D_1, arg_11D_2, array);
                }
            }
        }

        public void Part2()
        {
            SoundManager.PlaySound("FinalBoss_St2_WeatherChange_a");
            m_levelScreen.UnpauseScreen();
            if (m_currentActiveLB != null)
            {
                m_currentActiveLB.StopLogicBlock();
            }
            PauseEnemy(true);
            m_target.CurrentSpeed = 0f;
            m_target.ForceInvincible = true;
            Tween.RunFunction(1f, m_levelScreen, "RevealMorning");
            Tween.RunFunction(1f, m_levelScreen.CurrentRoom, "ChangeWindowOpacity");
            Tween.RunFunction(5f, this, "Part3");
        }

        public void Part3()
        {
            RCScreenManager rCScreenManager = m_levelScreen.ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("FinalBossTalk03");
            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "Part4");
            rCScreenManager.DisplayScreen(13, true);
        }

        public void Part4()
        {
            List<object> list = new List<object>();
            list.Add(this);
            (m_levelScreen.ScreenManager as RCScreenManager).DisplayScreen(26, true, list);
        }

        public EnemyObj_LastBoss(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            GameTypes.EnemyDifficulty difficulty)
            : base("PlayerIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            foreach (GameObj current in _objectList)
            {
                current.TextureColor = new Color(100, 100, 100);
            }
            Type = 29;
            m_damageShieldProjectiles = new List<ProjectileObj>();
            _objectList[5].Visible = false;
            _objectList[15].Visible = false;
            _objectList[16].Visible = false;
            _objectList[14].Visible = false;
            _objectList[13].Visible = false;
            _objectList[0].Visible = false;
            string text = (_objectList[12] as IAnimateableObj).SpriteName;
            int startIndex = text.IndexOf("_") - 1;
            text = text.Remove(startIndex, 1);
            text = text.Replace("_", 7 + "_");
            _objectList[12].ChangeSprite(text);
            PlayAnimation();
            m_delayObj = new BlankObj(0, 0);
            m_walkDownSoundFinalBoss = new FrameSoundObj(this, 3, "FinalBoss_St2_Foot_01", "FinalBoss_St2_Foot_02",
                "FinalBoss_St2_Foot_03");
            m_walkUpSoundFinalBoss = new FrameSoundObj(this, 6, "FinalBoss_St2_Foot_04", "FinalBoss_St2_Foot_05");
        }

        public override void ChangeSprite(string spriteName)
        {
            base.ChangeSprite(spriteName);
            if (!m_inSecondForm)
            {
                string text = (_objectList[12] as IAnimateableObj).SpriteName;
                int startIndex = text.IndexOf("_") - 1;
                text = text.Remove(startIndex, 1);
                text = text.Replace("_", 7 + "_");
                _objectList[12].ChangeSprite(text);
                _objectList[5].Visible = false;
                _objectList[15].Visible = false;
                _objectList[16].Visible = false;
                _objectList[14].Visible = false;
                _objectList[13].Visible = false;
                _objectList[0].Visible = false;
            }
        }

        public override void Draw(Camera2D camera)
        {
            if (IsKilled && TextureColor != Color.White)
            {
                m_blinkTimer = 0f;
                TextureColor = Color.White;
            }
            base.Draw(camera);
        }

        public override void Reset()
        {
            m_neoDying = false;
            m_inSecondForm = false;
            m_firstFormDying = false;
            CanBeKnockedBack = true;
            base.Reset();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_damageShieldProjectiles.Clear();
                m_damageShieldProjectiles = null;
                m_delayObj.Dispose();
                m_delayObj = null;
                if (m_daggerProjData != null)
                {
                    m_daggerProjData.Dispose();
                    m_daggerProjData = null;
                }
                if (m_axeProjData != null)
                {
                    m_axeProjData.Dispose();
                    m_axeProjData = null;
                }
                base.Dispose();
            }
        }

        public void ForceSecondForm(bool value)
        {
            m_inSecondForm = value;
        }
    }
}