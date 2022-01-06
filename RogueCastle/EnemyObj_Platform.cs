// 
// RogueLegacyArchipelago - EnemyObj_Platform.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using Tweener;

namespace RogueCastle
{
    public class EnemyObj_Platform : EnemyObj
    {
        private bool m_blinkedWarning;
        private bool m_isExtended;
        private float m_retractCounter;
        private float RetractDelay;

        public EnemyObj_Platform(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo,
            EnemyDifficulty difficulty)
            : base("EnemyPlatform_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            CollisionTypeTag = 1;
            Type = 27;
            CollidesBottom = false;
            CollidesLeft = false;
            CollidesRight = false;
            StopAnimation();
            PlayAnimationOnRestart = false;
            NonKillable = true;
            DisableCollisionBoxRotations = false;
        }

        protected override void InitializeEV()
        {
            Scale = new Vector2(2f, 2f);
            AnimationDelay = 0.0333333351f;
            Speed = 0f;
            MaxHealth = 999;
            EngageRadius = 30;
            ProjectileRadius = 20;
            MeleeRadius = 10;
            CooldownTime = 2f;
            KnockBack = new Vector2(1f, 2f);
            Damage = 25;
            JumpHeight = 20.5f;
            AlwaysFaceTarget = false;
            CanFallOffLedges = false;
            XPValue = 2;
            CanBeKnockedBack = false;
            LockFlip = true;
            IsWeighted = false;
            RetractDelay = 3f;
            Name = "Platform";
            /*switch (Difficulty)
            {
                case EnemyDifficulty.BASIC:
                case EnemyDifficulty.ADVANCED:
                case EnemyDifficulty.EXPERT:
                case EnemyDifficulty.MINIBOSS:*/
            //IL_DE:
            if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
            {
                m_isExtended = true;
                PlayAnimation("EndRetract", "EndRetract");
            }
            //return;
            //}
            //goto IL_DE;
        }

        public override void Update(GameTime gameTime)
        {
            var flag = Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f;
            if (!flag)
            {
                if (m_retractCounter > 0f)
                {
                    m_retractCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_retractCounter <= 1.5f && !m_blinkedWarning)
                    {
                        m_blinkedWarning = true;
                        var num = 0f;
                        for (var i = 0; i < 10; i++)
                        {
                            Tween.RunFunction(num, this, "Blink", Color.Red, 0.05f);
                            num += 0.1f;
                        }
                    }

                    if (m_retractCounter <= 0f)
                    {
                        m_isExtended = false;
                        PlayAnimation("StartExtract", "EndExtract");
                        SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Platform_Activate_03",
                            "Platform_Activate_04");
                    }
                }
            }
            else if (!m_isExtended)
            {
                m_isExtended = true;
                PlayAnimation("EndRetract", "EndRetract");
            }

            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer = true)
        {
            if (m_target.IsAirAttacking && !m_isExtended && (CurrentFrame == 1 || CurrentFrame == TotalFrames))
            {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "EnemyHit1", "EnemyHit2", "EnemyHit3",
                    "EnemyHit4", "EnemyHit5", "EnemyHit6");
                Blink(Color.Red, 0.1f);
                m_levelScreen.ImpactEffectPool.DisplayEnemyImpactEffect(position);
                m_isExtended = true;
                m_blinkedWarning = false;
                PlayAnimation("StartRetract", "EndRetract");
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Platform_Activate_01",
                    "Platform_Activate_02");
                m_retractCounter = RetractDelay;
                if (m_target.IsAirAttacking)
                {
                    m_target.IsAirAttacking = false;
                    m_target.AccelerationY = -m_target.AirAttackKnockBack;
                    m_target.NumAirBounces++;
                }
            }
        }

        public override void ResetState()
        {
            if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
            {
                return;
            }

            PlayAnimation(1, 1);
            m_isExtended = false;
            m_blinkedWarning = false;
            m_retractCounter = 0f;
            base.ResetState();
        }

        public override void Reset()
        {
            if (Game.PlayerStats.Traits.X == 34f || Game.PlayerStats.Traits.Y == 34f)
            {
                return;
            }

            PlayAnimation(1, 1);
            m_isExtended = false;
            m_blinkedWarning = false;
            m_retractCounter = 0f;
            base.Reset();
            StopAnimation();
        }
    }
}