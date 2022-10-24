// Rogue Legacy Randomizer - ImpactEffectPool.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy
{
    public class ImpactEffectPool : IDisposableObj
    {
        private readonly int m_poolSize;
        private bool m_isPaused;
        private DS2DPool<SpriteObj> m_resourcePool;

        public ImpactEffectPool(int poolSize)
        {
            m_poolSize = poolSize;
            m_resourcePool = new DS2DPool<SpriteObj>();
        }

        public int ActiveTextObjs
        {
            get { return m_resourcePool.NumActiveObjs; }
        }

        public int TotalPoolSize
        {
            get { return m_resourcePool.TotalPoolSize; }
        }

        public int CurrentPoolSize
        {
            get { return TotalPoolSize - ActiveTextObjs; }
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Impact Effect Pool");
                IsDisposed = true;
                m_resourcePool.Dispose();
                m_resourcePool = null;
            }
        }

        public void Initialize()
        {
            for (var i = 0; i < m_poolSize; i++)
            {
                var spriteObj = new SpriteObj("Blank_Sprite");
                spriteObj.AnimationDelay = 0.0333333351f;
                spriteObj.Visible = false;
                spriteObj.TextureColor = Color.White;
                m_resourcePool.AddToPool(spriteObj);
            }
        }

        public SpriteObj DisplayEffect(Vector2 position, string spriteName)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite(spriteName);
            spriteObj.TextureColor = Color.White;
            spriteObj.Visible = true;
            spriteObj.Position = position;
            spriteObj.PlayAnimation(false);
            return spriteObj;
        }

        public void DisplayEnemyImpactEffect(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ImpactEnemy_Sprite");
            spriteObj.TextureColor = Color.White;
            spriteObj.Rotation = CDGMath.RandomInt(0, 360);
            spriteObj.Visible = true;
            spriteObj.Position = position;
            spriteObj.PlayAnimation(false);
        }

        public void DisplayPlayerImpactEffect(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ImpactEnemy_Sprite");
            spriteObj.TextureColor = Color.Orange;
            spriteObj.Rotation = CDGMath.RandomInt(0, 360);
            spriteObj.Visible = true;
            spriteObj.Position = position;
            spriteObj.PlayAnimation(false);
        }

        public void DisplayBlockImpactEffect(Vector2 position, Vector2 scale)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ImpactBlock_Sprite");
            spriteObj.TextureColor = Color.White;
            spriteObj.Rotation = CDGMath.RandomInt(0, 360);
            spriteObj.Visible = true;
            spriteObj.Position = position;
            spriteObj.PlayAnimation(false);
            spriteObj.Scale = scale;
        }

        public void DisplayDeathEffect(Vector2 position)
        {
            for (var i = 0; i < 10; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("ExplosionBlue_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = position;
                var num = CDGMath.RandomFloat(0.7f, 0.8f);
                var num2 = 50;
                spriteObj.Scale = new Vector2(num, num);
                spriteObj.Rotation = CDGMath.RandomInt(0, 90);
                spriteObj.PlayAnimation();
                var num3 = CDGMath.RandomFloat(0.5f, 1f);
                var num4 = CDGMath.RandomFloat(0f, 0.1f);
                Tween.To(spriteObj, num3 - 0.2f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
                Tween.To(spriteObj, num3, Back.EaseIn, "ScaleX", num4.ToString(), "ScaleY", num4.ToString());
                Tween.By(spriteObj, num3, Quad.EaseOut, "X", CDGMath.RandomInt(-num2, num2).ToString(), "Y",
                    CDGMath.RandomInt(-num2, num2).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                Tween.By(spriteObj, num3 - 0.1f, Quad.EaseOut, "Rotation", CDGMath.RandomInt(145, 190).ToString());
            }
        }

        public void DisplaySpawnEffect(Vector2 position)
        {
            for (var i = 0; i < 10; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("ExplosionOrange_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = position;
                var num = CDGMath.RandomFloat(0.7f, 0.8f);
                var num2 = 50;
                spriteObj.Scale = new Vector2(num, num);
                spriteObj.Rotation = CDGMath.RandomInt(0, 90);
                spriteObj.PlayAnimation();
                var num3 = CDGMath.RandomFloat(0.5f, 1f);
                var num4 = CDGMath.RandomFloat(0f, 0.1f);
                Tween.To(spriteObj, num3 - 0.2f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
                Tween.To(spriteObj, num3, Back.EaseIn, "ScaleX", num4.ToString(), "ScaleY", num4.ToString());
                Tween.By(spriteObj, num3, Quad.EaseOut, "X", CDGMath.RandomInt(-num2, num2).ToString(), "Y",
                    CDGMath.RandomInt(-num2, num2).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                Tween.By(spriteObj, num3 - 0.1f, Quad.EaseOut, "Rotation", CDGMath.RandomInt(145, 190).ToString());
            }
        }

        public void DisplayChestSparkleEffect(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("LevelUpParticleFX_Sprite");
            spriteObj.Visible = true;
            var num = CDGMath.RandomFloat(0.2f, 0.5f);
            spriteObj.Scale = new Vector2(num, num);
            spriteObj.Opacity = 0f;
            spriteObj.Position = position;
            spriteObj.Rotation = CDGMath.RandomInt(0, 90);
            spriteObj.PlayAnimation(false);
            spriteObj.Position += new Vector2(CDGMath.RandomInt(-40, 40), CDGMath.RandomInt(-40, 40));
            Tween.To(spriteObj, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
        }

        public void DisplayDoubleJumpEffect(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("DoubleJumpFX_Sprite");
            spriteObj.Visible = true;
            spriteObj.Position = position;
            spriteObj.PlayAnimation(false);
        }

        public void DisplayDashEffect(Vector2 position, bool flip)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("DashFX_Sprite");
            if (flip)
            {
                spriteObj.Flip = SpriteEffects.FlipHorizontally;
            }

            spriteObj.Position = position;
            spriteObj.Visible = true;
            spriteObj.PlayAnimation(false);
        }

        public void DisplayTeleportEffect(Vector2 position)
        {
            var num = 0.1f;
            for (var i = 0; i < 5; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.Visible = true;
                spriteObj.ChangeSprite("TeleportRock" + (i + 1) + "_Sprite");
                spriteObj.PlayAnimation();
                spriteObj.Position = new Vector2(CDGMath.RandomFloat(position.X - 70f, position.X + 70f),
                    position.Y + CDGMath.RandomInt(-50, -30));
                spriteObj.Opacity = 0f;
                var num2 = 1f;
                Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", num.ToString(), "Opacity", "1");
                Tween.By(spriteObj, num2, Linear.EaseNone, "delay", num.ToString(), "Y", "-150");
                spriteObj.Opacity = 1f;
                Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", (num2 + num - 0.5f).ToString(), "Opacity", "0");
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                spriteObj.Opacity = 0f;
                num += CDGMath.RandomFloat(0.1f, 0.3f);
            }

            var spriteObj2 = m_resourcePool.CheckOut();
            spriteObj2.AnimationDelay = 0.05f;
            spriteObj2.Opacity = 0.8f;
            spriteObj2.Visible = true;
            spriteObj2.ChangeSprite("TeleporterBeam_Sprite");
            spriteObj2.Position = position;
            spriteObj2.ScaleY = 0f;
            spriteObj2.PlayAnimation();
            Tween.To(spriteObj2, 0.05f, Linear.EaseNone, "ScaleY", "1");
            Tween.To(spriteObj2, 2f, Linear.EaseNone);
            Tween.AddEndHandlerToLastTween(spriteObj2, "StopAnimation");
        }

        public void DisplayThrustDustEffect(GameObj obj, int numClouds, float duration)
        {
            var num = duration / numClouds;
            var num2 = 0f;
            for (var i = 0; i < numClouds; i++)
            {
                Tween.RunFunction(num2, this, "DisplayDustEffect", obj);
                Tween.RunFunction(num2, this, "DisplayDustEffect", obj);
                num2 += num;
            }
        }

        public void DisplayDustEffect(GameObj obj)
        {
            var num = CDGMath.RandomInt(-30, 30);
            var num2 = CDGMath.RandomInt(-30, 30);
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ExplosionBrown_Sprite");
            spriteObj.Opacity = 0f;
            spriteObj.Visible = true;
            spriteObj.Rotation = CDGMath.RandomInt(0, 270);
            spriteObj.Position = new Vector2(obj.X, obj.Bounds.Bottom);
            spriteObj.Scale = new Vector2(0.8f, 0.8f);
            spriteObj.PlayAnimation();
            Tween.To(spriteObj, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 0.7f, Linear.EaseNone, "Rotation", "180");
            Tween.By(spriteObj, 0.7f, Quad.EaseOut, "X", num.ToString(), "Y", num2.ToString());
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
            spriteObj.Opacity = 0f;
        }

        public void DisplayDustEffect(Vector2 pos)
        {
            var num = CDGMath.RandomInt(-30, 30);
            var num2 = CDGMath.RandomInt(-30, 30);
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ExplosionBrown_Sprite");
            spriteObj.Opacity = 0f;
            spriteObj.Visible = true;
            spriteObj.Rotation = CDGMath.RandomInt(0, 270);
            spriteObj.Position = pos;
            spriteObj.Scale = new Vector2(0.8f, 0.8f);
            spriteObj.PlayAnimation();
            Tween.To(spriteObj, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 0.7f, Linear.EaseNone, "Rotation", "180");
            Tween.By(spriteObj, 0.7f, Quad.EaseOut, "X", num.ToString(), "Y", num2.ToString());
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
            spriteObj.Opacity = 0f;
        }

        public void TurretFireEffect(Vector2 pos, Vector2 scale)
        {
            var num = CDGMath.RandomInt(-20, 20);
            var num2 = CDGMath.RandomInt(-20, 20);
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ExplosionBrown_Sprite");
            spriteObj.Opacity = 0f;
            spriteObj.Visible = true;
            spriteObj.Rotation = CDGMath.RandomInt(0, 270);
            spriteObj.Position = pos;
            spriteObj.Scale = scale;
            spriteObj.PlayAnimation();
            Tween.To(spriteObj, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 0.7f, Linear.EaseNone, "Rotation", CDGMath.RandomInt(-50, 50).ToString());
            Tween.By(spriteObj, 0.7f, Quad.EaseOut, "X", num.ToString(), "Y", num2.ToString());
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.4f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
            spriteObj.Opacity = 0f;
        }

        public void DisplayFartEffect(GameObj obj)
        {
            var num = CDGMath.RandomInt(-10, 10);
            if (obj.Flip == SpriteEffects.FlipHorizontally)
            {
                num = 20;
            }
            else
            {
                num = -20;
            }

            var num2 = CDGMath.RandomInt(-10, 10);
            num2 = 0;
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("ExplosionBrown_Sprite");
            spriteObj.Opacity = 0f;
            spriteObj.Visible = true;
            spriteObj.Rotation = CDGMath.RandomInt(0, 270);
            spriteObj.Position = new Vector2(obj.X, obj.Bounds.Bottom);
            if (obj.Flip == SpriteEffects.FlipHorizontally)
            {
                spriteObj.X += 30f;
            }
            else
            {
                spriteObj.X -= 30f;
            }

            spriteObj.Scale = new Vector2(0.8f, 0.8f);
            spriteObj.PlayAnimation();
            Tween.To(spriteObj, 0.2f, Linear.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 0.7f, Linear.EaseNone, "Rotation", "180");
            Tween.By(spriteObj, 0.7f, Quad.EaseOut, "X", num.ToString(), "Y", num2.ToString());
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
            spriteObj.Opacity = 0f;
        }

        public void DisplayExplosionEffect(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("EnemyDeathFX1_Sprite");
            spriteObj.Visible = true;
            spriteObj.Position = position;
            spriteObj.PlayAnimation(false);
            spriteObj.AnimationDelay = 0.0333333351f;
            spriteObj.Scale = new Vector2(2f, 2f);
        }

        public void StartInverseEmit(Vector2 pos)
        {
            var num = 0.4f;
            var num2 = 30;
            var num3 = num / num2;
            var num4 = 0f;
            for (var i = 0; i < num2; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("Blank_Sprite");
                spriteObj.TextureColor = Color.Black;
                spriteObj.Visible = true;
                spriteObj.PlayAnimation();
                spriteObj.Scale = Vector2.Zero;
                spriteObj.X = pos.X + CDGMath.RandomInt(-100, 100);
                spriteObj.Y = pos.Y + CDGMath.RandomInt(-100, 100);
                Tween.To(spriteObj, num4, Linear.EaseNone, "ScaleX", "2", "ScaleY", "2");
                Tween.To(spriteObj, num - num4, Quad.EaseInOut, "delay", num4.ToString(), "X", pos.X.ToString(), "Y",
                    pos.Y.ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                num4 += num3;
            }
        }

        public void StartTranslocateEmit(Vector2 pos)
        {
            var num = 30;
            for (var i = 0; i < num; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("Blank_Sprite");
                spriteObj.TextureColor = Color.White;
                spriteObj.Visible = true;
                spriteObj.PlayAnimation();
                spriteObj.Scale = new Vector2(2f, 2f);
                spriteObj.Position = pos;
                Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", "0.5", "ScaleX", "0", "ScaleY", "0");
                Tween.By(spriteObj, 1f, Quad.EaseIn, "X", CDGMath.RandomInt(-250, 250).ToString());
                Tween.By(spriteObj, 1f, Quad.EaseIn, "Y", CDGMath.RandomInt(500, 800).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
            }
        }

        public void BlackSmokeEffect(GameObj obj)
        {
            for (var i = 0; i < 2; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("BlackSmoke_Sprite");
                spriteObj.Visible = true;
                spriteObj.Y = obj.Y;
                spriteObj.Y += CDGMath.RandomInt(-40, 40);
                spriteObj.X = obj.Bounds.Left;
                spriteObj.Opacity = 0f;
                spriteObj.PlayAnimation();
                spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
                if (CDGMath.RandomPlusMinus() < 0)
                {
                    spriteObj.Flip = SpriteEffects.FlipHorizontally;
                }

                if (CDGMath.RandomPlusMinus() < 0)
                {
                    spriteObj.Flip = SpriteEffects.FlipVertically;
                }

                var num = -1;
                if (obj.Flip == SpriteEffects.FlipHorizontally)
                {
                    num = 1;
                    spriteObj.X = obj.Bounds.Right;
                }

                Tween.To(spriteObj, 0.4f, Tween.EaseNone, "Opacity", "1");
                Tween.By(spriteObj, 1.5f, Quad.EaseInOut, "X", (CDGMath.RandomInt(50, 100) * num).ToString(), "Y",
                    CDGMath.RandomInt(-100, 100).ToString(), "Rotation", CDGMath.RandomInt(-45, 45).ToString());
                spriteObj.Opacity = 1f;
                Tween.To(spriteObj, 1f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
                spriteObj.Opacity = 0f;
                Tween.RunFunction(2f, spriteObj, "StopAnimation");
            }
        }

        public void BlackSmokeEffect(Vector2 pos, Vector2 scale)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("BlackSmoke_Sprite");
            spriteObj.Visible = true;
            spriteObj.Y = pos.Y;
            spriteObj.Y += CDGMath.RandomInt(-40, 40);
            spriteObj.X = pos.X;
            spriteObj.X += CDGMath.RandomInt(-40, 40);
            spriteObj.Scale = scale;
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
            Tween.To(spriteObj, 0.4f, Tween.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 1.5f, Quad.EaseInOut, "X", CDGMath.RandomInt(50, 100).ToString(), "Y",
                CDGMath.RandomInt(-100, 100).ToString(), "Rotation", CDGMath.RandomInt(-45, 45).ToString());
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 1f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
            spriteObj.Opacity = 0f;
            Tween.RunFunction(2f, spriteObj, "StopAnimation");
        }

        public void CrowDestructionEffect(Vector2 pos)
        {
            for (var i = 0; i < 2; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("BlackSmoke_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = pos;
                spriteObj.Opacity = 0f;
                spriteObj.PlayAnimation();
                spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
                Tween.To(spriteObj, 0.4f, Tween.EaseNone, "Opacity", "1");
                Tween.By(spriteObj, 1.5f, Quad.EaseInOut, "X", CDGMath.RandomInt(-50, 50).ToString(), "Y",
                    CDGMath.RandomInt(-50, 50).ToString(), "Rotation", CDGMath.RandomInt(-45, 45).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                spriteObj.Opacity = 1f;
                Tween.To(spriteObj, 1f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
                spriteObj.Opacity = 0f;
            }

            for (var j = 0; j < 4; j++)
            {
                var spriteObj2 = m_resourcePool.CheckOut();
                spriteObj2.ChangeSprite("CrowFeather_Sprite");
                spriteObj2.Visible = true;
                spriteObj2.Scale = new Vector2(2f, 2f);
                spriteObj2.Position = pos;
                spriteObj2.X += CDGMath.RandomInt(-20, 20);
                spriteObj2.Y += CDGMath.RandomInt(-20, 20);
                spriteObj2.Opacity = 0f;
                spriteObj2.PlayAnimation();
                spriteObj2.Rotation = CDGMath.RandomInt(-30, 30);
                Tween.To(spriteObj2, 0.1f, Tween.EaseNone, "Opacity", "1");
                Tween.By(spriteObj2, 1.5f, Quad.EaseInOut, "X", CDGMath.RandomInt(-50, 50).ToString(), "Y",
                    CDGMath.RandomInt(-50, 50).ToString(), "Rotation", CDGMath.RandomInt(-180, 180).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj2, "StopAnimation");
                spriteObj2.Opacity = 1f;
                Tween.To(spriteObj2, 1f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
                spriteObj2.Opacity = 0f;
            }
        }

        public void CrowSmokeEffect(Vector2 pos)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("BlackSmoke_Sprite");
            spriteObj.Visible = true;
            spriteObj.Y = pos.Y;
            spriteObj.Y += CDGMath.RandomInt(-40, 40);
            spriteObj.X = pos.X;
            spriteObj.X += CDGMath.RandomInt(-40, 40);
            spriteObj.Scale = new Vector2(0.7f, 0.7f);
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 1f, Quad.EaseInOut, "X", CDGMath.RandomInt(50, 100).ToString(), "Y",
                CDGMath.RandomInt(-100, 100).ToString(), "Rotation", CDGMath.RandomInt(-45, 45).ToString());
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.5f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
            spriteObj.Opacity = 0f;
            Tween.RunFunction(1.05f, spriteObj, "StopAnimation");
        }

        public void WoodChipEffect(Vector2 pos)
        {
            for (var i = 0; i < 5; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("WoodChip_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = pos;
                spriteObj.PlayAnimation();
                spriteObj.Scale = new Vector2(2f, 2f);
                var num = CDGMath.RandomInt(-360, 360);
                spriteObj.Rotation = num;
                var vector = new Vector2(CDGMath.RandomInt(-60, 60), CDGMath.RandomInt(-60, 60));
                Tween.By(spriteObj, 0.3f, Tween.EaseNone, "X", vector.X.ToString(), "Y", vector.Y.ToString());
                Tween.By(spriteObj, 0.3f, Tween.EaseNone, "Rotation", CDGMath.RandomInt(-360, 360).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", "0.1", "Opacity", "0");
            }
        }

        public void SpellCastEffect(Vector2 pos, float angle, bool megaSpell)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("SpellPortal_Sprite");
            if (megaSpell)
            {
                spriteObj.TextureColor = Color.Red;
            }

            spriteObj.Visible = true;
            spriteObj.Position = pos;
            spriteObj.PlayAnimation(false);
            spriteObj.Scale = new Vector2(2f, 2f);
            spriteObj.Rotation = angle;
            spriteObj.OutlineWidth = 2;
        }

        public void LastBossSpellCastEffect(GameObj obj, float angle, bool megaSpell)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("SpellPortal_Sprite");
            if (megaSpell)
            {
                spriteObj.TextureColor = Color.Red;
            }

            spriteObj.Visible = true;
            spriteObj.Position = obj.Position;
            spriteObj.PlayAnimation(false);
            spriteObj.Scale = new Vector2(2f, 2f);
            if (obj.Flip == SpriteEffects.None)
            {
                spriteObj.Rotation = -angle;
            }
            else
            {
                spriteObj.Rotation = angle;
            }

            spriteObj.OutlineWidth = 2;
        }

        public void LoadingGateSmokeEffect(int numEntities)
        {
            var num = 1320f / numEntities;
            for (var i = 0; i < numEntities; i++)
            {
                var num2 = CDGMath.RandomInt(-50, 50);
                var num3 = CDGMath.RandomInt(-50, 0);
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("ExplosionBrown_Sprite");
                spriteObj.Visible = true;
                spriteObj.ForceDraw = true;
                spriteObj.Position = new Vector2(num * i, 720f);
                spriteObj.PlayAnimation();
                spriteObj.Opacity = 0f;
                spriteObj.Scale = new Vector2(1.5f, 1.5f);
                Tween.To(spriteObj, 0.2f, Linear.EaseNone, "Opacity", "0.8");
                Tween.By(spriteObj, 0.7f, Linear.EaseNone, "Rotation", CDGMath.RandomFloat(-180f, 180f).ToString());
                Tween.By(spriteObj, 0.7f, Quad.EaseOut, "X", num2.ToString(), "Y", num3.ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                spriteObj.Opacity = 0.8f;
                Tween.To(spriteObj, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
                spriteObj.Opacity = 0f;
            }
        }

        public void MegaTeleport(Vector2 pos, Vector2 scale)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("MegaTeleport_Sprite");
            spriteObj.TextureColor = Color.LightSkyBlue;
            spriteObj.Scale = scale;
            spriteObj.Visible = true;
            spriteObj.Position = pos;
            spriteObj.PlayAnimation(false);
            spriteObj.AnimationDelay = 0.0166666675f;
            Tween.By(spriteObj, 0.1f, Tween.EaseNone, "delay", "0.15", "Y", "-720");
        }

        public void MegaTeleportReverse(Vector2 pos, Vector2 scale)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("MegaTeleportReverse_Sprite");
            spriteObj.TextureColor = Color.LightSkyBlue;
            spriteObj.Scale = scale;
            spriteObj.Visible = true;
            spriteObj.Position = pos;
            spriteObj.Y -= 720f;
            spriteObj.PlayAnimation(1, 1, true);
            spriteObj.AnimationDelay = 0.0166666675f;
            Tween.By(spriteObj, 0.1f, Tween.EaseNone, "Y", "720");
            Tween.AddEndHandlerToLastTween(spriteObj, "PlayAnimation", false);
        }

        public void DestroyFireballBoss(Vector2 pos)
        {
            var num = 0f;
            var num2 = 24f;
            for (var i = 0; i < 15; i++)
            {
                var num3 = CDGMath.RandomFloat(0.5f, 1.1f);
                var num4 = CDGMath.RandomInt(50, 200);
                var num5 = CDGMath.RandomFloat(2f, 5f);
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("SpellDamageShield_Sprite");
                spriteObj.Visible = true;
                spriteObj.Scale = new Vector2(num5, num5);
                spriteObj.Position = pos;
                spriteObj.PlayAnimation();
                var vector = CDGMath.AngleToVector(num);
                Tween.By(spriteObj, 1.5f, Quad.EaseOut, "X", (vector.X * num4).ToString(), "Y",
                    (vector.Y * num4).ToString());
                Tween.To(spriteObj, 0.5f, Tween.EaseNone, "delay", num3.ToString(), "Opacity", "0");
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                num += num2;
            }
        }

        public void SkillTreeDustEffect(Vector2 pos, bool horizontal, float length)
        {
            var num = (int) (length / 20f);
            var scaleFactor = length / num;
            var zero = Vector2.Zero;
            if (horizontal)
            {
                zero = new Vector2(1f, 0f);
            }
            else
            {
                zero = new Vector2(0f, -1f);
            }

            for (var i = 0; i < num; i++)
            {
                var num2 = CDGMath.RandomInt(-10, 10);
                var num3 = CDGMath.RandomInt(-20, 0);
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("ExplosionBrown_Sprite");
                spriteObj.Opacity = 0f;
                spriteObj.Visible = true;
                spriteObj.Scale = new Vector2(0.5f, 0.5f);
                spriteObj.Rotation = CDGMath.RandomInt(0, 270);
                spriteObj.Position = pos + zero * scaleFactor * (i + 1);
                spriteObj.PlayAnimation();
                Tween.To(spriteObj, 0.5f, Linear.EaseNone, "Opacity", "1");
                Tween.By(spriteObj, 1.5f, Linear.EaseNone, "Rotation", CDGMath.RandomFloat(-30f, 30f).ToString());
                Tween.By(spriteObj, 1.5f, Quad.EaseOut, "X", num2.ToString(), "Y", num3.ToString());
                var num4 = CDGMath.RandomFloat(0.5f, 0.8f);
                Tween.To(spriteObj, 1.5f, Quad.EaseOut, "ScaleX", num4.ToString(), "ScaleY", num4.ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                spriteObj.Opacity = 1f;
                Tween.To(spriteObj, 0.7f, Linear.EaseNone, "delay", "0.6", "Opacity", "0");
                spriteObj.Opacity = 0f;
            }
        }

        public void SkillTreeDustDuration(Vector2 pos, bool horizontal, float length, float duration)
        {
            var num = 0.25f;
            var num2 = (int) (duration / num);
            for (var i = 0; i < num2; i++)
                Tween.RunFunction(num * i, this, "SkillTreeDustEffect", pos, horizontal, length);
        }

        public void CarnivalGoldEffect(Vector2 startPos, Vector2 endPos, int numCoins)
        {
            var num = 0.32f;
            for (var i = 0; i < numCoins; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("Coin_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = startPos;
                spriteObj.PlayAnimation();
                var num2 = CDGMath.RandomInt(-30, 30);
                var num3 = CDGMath.RandomInt(-30, 30);
                Tween.By(spriteObj, 0.3f, Quad.EaseInOut, "X", num2.ToString(), "Y", num3.ToString());
                spriteObj.X += num2;
                spriteObj.Y += num3;
                Tween.To(spriteObj, 0.5f, Quad.EaseIn, "delay", num.ToString(), "X", endPos.X.ToString(), "Y",
                    endPos.Y.ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                spriteObj.X -= num2;
                spriteObj.Y -= num3;
                num += 0.05f;
            }
        }

        public void AssassinCastEffect(Vector2 pos)
        {
            var num = 10;
            var num2 = 0f;
            float num3 = 360 / num;
            for (var i = 0; i < num; i++)
            {
                var vector = CDGMath.AngleToVector(num2);
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("ExplosionBrown_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = pos;
                spriteObj.TextureColor = new Color(20, 20, 20);
                spriteObj.Opacity = 0f;
                spriteObj.PlayAnimation();
                var num4 = CDGMath.RandomFloat(0.5f, 1.5f);
                spriteObj.Scale = new Vector2(num4, num4);
                spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
                vector.X += CDGMath.RandomInt(-5, 5);
                vector.Y += CDGMath.RandomInt(-5, 5);
                Tween.To(spriteObj, 0.1f, Tween.EaseNone, "Opacity", "0.5");
                Tween.By(spriteObj, 1f, Quad.EaseOut, "X", (vector.X * CDGMath.RandomInt(20, 25)).ToString(), "Y",
                    (vector.Y * CDGMath.RandomInt(20, 25)).ToString(), "Rotation",
                    CDGMath.RandomInt(-180, 180).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                spriteObj.Opacity = 0.5f;
                Tween.To(spriteObj, 0.5f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
                spriteObj.Opacity = 0f;
                num2 += num3;
            }
        }

        public void NinjaDisappearEffect(GameObj obj)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("Log_Sprite");
            spriteObj.AnimationDelay = 0.05f;
            spriteObj.Position = obj.Position;
            spriteObj.Visible = true;
            spriteObj.Scale = obj.Scale / 2f;
            spriteObj.PlayAnimation();
            Tween.By(spriteObj, 0.3f, Quad.EaseIn, "delay", "0.2", "Y", "50");
            Tween.To(spriteObj, 0.2f, Linear.EaseNone, "delay", "0.3", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
            var spriteObj2 = m_resourcePool.CheckOut();
            spriteObj2.ChangeSprite("NinjaSmoke_Sprite");
            spriteObj2.AnimationDelay = 0.05f;
            spriteObj2.Position = obj.Position;
            spriteObj2.Visible = true;
            spriteObj2.Scale = obj.Scale * 2f;
            spriteObj2.PlayAnimation(false);
        }

        public void NinjaAppearEffect(GameObj obj)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("NinjaSmoke_Sprite");
            spriteObj.AnimationDelay = 0.05f;
            spriteObj.Position = obj.Position;
            spriteObj.Visible = true;
            spriteObj.Scale = obj.Scale * 2f;
            spriteObj.PlayAnimation(false);
        }

        public void DisplayCriticalText(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("CriticalText_Sprite");
            spriteObj.Visible = true;
            spriteObj.Rotation = CDGMath.RandomInt(-20, 20);
            spriteObj.Position = CDGMath.GetCirclePosition(spriteObj.Rotation - 90f, 20f, position);
            spriteObj.Scale = Vector2.Zero;
            spriteObj.PlayAnimation();
            Tween.To(spriteObj, 0.2f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
        }

        public void DisplayFusRoDahText(Vector2 position)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("FusRoDahText_Sprite");
            spriteObj.Visible = true;
            spriteObj.Rotation = CDGMath.RandomInt(-20, 20);
            spriteObj.Position = CDGMath.GetCirclePosition(spriteObj.Rotation - 90f, 40f, position);
            spriteObj.Scale = Vector2.Zero;
            spriteObj.PlayAnimation();
            Tween.To(spriteObj, 0.2f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", "0.5", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
        }

        public void DisplayTanookiEffect(GameObj obj)
        {
            for (var i = 0; i < 10; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("ExplosionBrown_Sprite");
                spriteObj.Visible = true;
                spriteObj.Position = obj.Position;
                spriteObj.X += CDGMath.RandomInt(-30, 30);
                spriteObj.Y += CDGMath.RandomInt(-30, 30);
                var num = CDGMath.RandomFloat(0.7f, 0.8f);
                var num2 = 50;
                spriteObj.Scale = new Vector2(num, num);
                spriteObj.Rotation = CDGMath.RandomInt(0, 90);
                spriteObj.PlayAnimation();
                var num3 = CDGMath.RandomFloat(0.5f, 1f);
                var num4 = CDGMath.RandomFloat(0f, 0.1f);
                Tween.To(spriteObj, num3 - 0.2f, Linear.EaseNone, "delay", "0.2", "Opacity", "0");
                Tween.To(spriteObj, num3, Back.EaseIn, "ScaleX", num4.ToString(), "ScaleY", num4.ToString());
                Tween.By(spriteObj, num3, Quad.EaseOut, "X", CDGMath.RandomInt(-num2, num2).ToString(), "Y",
                    CDGMath.RandomInt(-num2, num2).ToString());
                Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
                Tween.By(spriteObj, num3 - 0.1f, Quad.EaseOut, "Rotation", CDGMath.RandomInt(145, 190).ToString());
            }
        }

        public void DisplayMusicNote(Vector2 pos)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("NoteWhite_Sprite");
            spriteObj.Visible = true;
            spriteObj.Position = pos;
            spriteObj.Scale = new Vector2(2f, 2f);
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            if (CDGMath.RandomPlusMinus() < 0)
            {
                spriteObj.Flip = SpriteEffects.FlipHorizontally;
            }

            Tween.By(spriteObj, 1f, Quad.EaseOut, "Y", "-50");
            Tween.To(spriteObj, 0.5f, Tween.EaseNone, "Opacity", "1");
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", "0.8", "Opacity", "0");
            spriteObj.Opacity = 0f;
            Tween.RunFunction(1f, spriteObj, "StopAnimation");
        }

        public void DisplayQuestionMark(Vector2 pos)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("QuestionMark_Sprite");
            spriteObj.Visible = true;
            spriteObj.Position = pos;
            var num = CDGMath.RandomFloat(0.8f, 2f);
            spriteObj.Scale = new Vector2(num, num);
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            Tween.By(spriteObj, 1f, Quad.EaseOut, "Y", CDGMath.RandomInt(-70, -50).ToString());
            Tween.By(spriteObj, 1f, Quad.EaseOut, "X", CDGMath.RandomInt(-20, 20).ToString());
            Tween.To(spriteObj, 0.5f, Tween.EaseNone, "Opacity", "1");
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", "0.8", "Opacity", "0");
            spriteObj.Opacity = 0f;
            Tween.RunFunction(1f, spriteObj, "StopAnimation");
        }

        public void DisplayMassiveSmoke(Vector2 topLeft)
        {
            var pos = topLeft;
            for (var i = 0; i < 20; i++)
            {
                IntroSmokeEffect(pos);
                pos.Y += 40f;
            }
        }

        public void IntroSmokeEffect(Vector2 pos)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("BlackSmoke_Sprite");
            spriteObj.Visible = true;
            spriteObj.Y = pos.Y;
            spriteObj.Y += CDGMath.RandomInt(-40, 40);
            spriteObj.X = pos.X;
            spriteObj.X += CDGMath.RandomInt(-40, 40);
            spriteObj.Scale = new Vector2(2.5f, 2.5f);
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            spriteObj.Rotation = CDGMath.RandomInt(-30, 30);
            var num = CDGMath.RandomFloat(0f, 0.2f);
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "1");
            Tween.By(spriteObj, 1f, Quad.EaseInOut, "delay", num.ToString(), "X", CDGMath.RandomInt(50, 100).ToString(),
                "Y", CDGMath.RandomInt(-100, 100).ToString(), "Rotation", CDGMath.RandomInt(-45, 45).ToString());
            spriteObj.Opacity = 1f;
            Tween.To(spriteObj, 0.5f, Tween.EaseNone, "delay", (num + 0.5f).ToString(), "Opacity", "0");
            spriteObj.Opacity = 0f;
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
        }

        public void DisplayIceParticleEffect(GameObj sprite)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("WizardIceParticle_Sprite");
            spriteObj.Visible = true;
            spriteObj.Scale = Vector2.Zero;
            spriteObj.Position = new Vector2(CDGMath.RandomInt(sprite.Bounds.Left, sprite.Bounds.Right),
                CDGMath.RandomInt(sprite.Bounds.Top, sprite.Bounds.Bottom));
            spriteObj.Opacity = 0f;
            Tween.To(spriteObj, 0.1f, Tween.EaseNone, "Opacity", "1");
            Tween.To(spriteObj, 0.9f, Tween.EaseNone, "ScaleX", "2.5", "ScaleY", "2.5");
            Tween.By(spriteObj, 0.9f, Tween.EaseNone, "Rotation",
                (CDGMath.RandomInt(90, 270) * CDGMath.RandomPlusMinus()).ToString());
            spriteObj.Opacity = 1f;
            var num = CDGMath.RandomFloat(0.4f, 0.7f);
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "0");
            Tween.By(spriteObj, 0.2f + num, Tween.EaseNone, "X", CDGMath.RandomInt(-20, 20).ToString(), "Y",
                CDGMath.RandomInt(-20, 20).ToString());
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            Tween.RunFunction(1f, spriteObj, "StopAnimation");
        }

        public void DisplayFireParticleEffect(GameObj sprite)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("WizardFireParticle_Sprite");
            spriteObj.Visible = true;
            spriteObj.Scale = Vector2.Zero;
            spriteObj.Position = new Vector2(CDGMath.RandomInt(sprite.Bounds.Left, sprite.Bounds.Right),
                CDGMath.RandomInt(sprite.Bounds.Top, sprite.Bounds.Bottom));
            spriteObj.Opacity = 0f;
            Tween.To(spriteObj, 0.1f, Tween.EaseNone, "Opacity", "1");
            Tween.To(spriteObj, 0.9f, Tween.EaseNone, "ScaleX", "4", "ScaleY", "4");
            spriteObj.Opacity = 1f;
            var num = CDGMath.RandomFloat(0.4f, 0.7f);
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "0");
            Tween.By(spriteObj, 0.2f + num, Tween.EaseNone, "Y", CDGMath.RandomInt(-20, -5).ToString());
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            Tween.RunFunction(1f, spriteObj, "StopAnimation");
        }

        public void DisplayEarthParticleEffect(GameObj sprite)
        {
            var num = CDGMath.RandomInt(1, 4);
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("Blossom" + num + "_Sprite");
            spriteObj.Visible = true;
            spriteObj.Scale = new Vector2(0.2f, 0.2f);
            spriteObj.Position = new Vector2(CDGMath.RandomInt(sprite.Bounds.Left, sprite.Bounds.Right),
                CDGMath.RandomInt(sprite.Bounds.Top, sprite.Bounds.Bottom));
            spriteObj.Opacity = 0f;
            Tween.To(spriteObj, 0.1f, Tween.EaseNone, "Opacity", "1");
            Tween.To(spriteObj, 0.9f, Tween.EaseNone, "ScaleX", "0.7", "ScaleY", "0.7");
            Tween.By(spriteObj, 0.9f, Tween.EaseNone, "Rotation",
                (CDGMath.RandomInt(10, 45) * CDGMath.RandomPlusMinus()).ToString());
            spriteObj.Opacity = 1f;
            var num2 = CDGMath.RandomFloat(0.4f, 0.7f);
            Tween.To(spriteObj, 0.2f, Tween.EaseNone, "delay", num2.ToString(), "Opacity", "0");
            Tween.By(spriteObj, 0.2f + num2, Tween.EaseNone, "Y", CDGMath.RandomInt(5, 20).ToString());
            spriteObj.Opacity = 0f;
            spriteObj.PlayAnimation();
            Tween.RunFunction(1f, spriteObj, "StopAnimation");
        }

        public void DisplayFountainShatterSmoke(GameObj sprite)
        {
            var num = 15;
            var num2 = sprite.Width / (float) num;
            float num3 = sprite.Bounds.Left;
            for (var i = 0; i < num; i++)
            {
                var spriteObj = m_resourcePool.CheckOut();
                spriteObj.ChangeSprite("FountainShatterSmoke_Sprite");
                spriteObj.Visible = true;
                spriteObj.PlayAnimation();
                spriteObj.Opacity = 0f;
                spriteObj.Scale = Vector2.Zero;
                spriteObj.Position = new Vector2(num3, sprite.Y);
                var num4 = CDGMath.RandomFloat(2f, 4f);
                Tween.To(spriteObj, 0.5f, Tween.EaseNone, "Opacity", "1");
                Tween.By(spriteObj, 4f, Tween.EaseNone, "Rotation", CDGMath.RandomInt(-40, 40).ToString());
                Tween.By(spriteObj, 3f, Tween.EaseNone, "X", CDGMath.RandomInt(-20, 20).ToString(), "Y",
                    CDGMath.RandomInt(-50, -30).ToString());
                Tween.To(spriteObj, 3f, Tween.EaseNone, "ScaleX", num4.ToString(), "ScaleY", num4.ToString());
                spriteObj.Opacity = 1f;
                Tween.To(spriteObj, 2f, Tween.EaseNone, "delay", CDGMath.RandomFloat(1f, 2f).ToString(), "Opacity",
                    "0");
                spriteObj.Opacity = 0f;
                Tween.RunFunction(4.5f, spriteObj, "StopAnimation");
                num3 += num2;
            }

            num /= 2;
            num3 = sprite.Bounds.Left + 50;
            num2 = (sprite.Width - 50) / (float) num;
            for (var j = 0; j < num; j++)
            {
                var spriteObj2 = m_resourcePool.CheckOut();
                spriteObj2.ChangeSprite("FountainShatterSmoke_Sprite");
                spriteObj2.Visible = true;
                spriteObj2.PlayAnimation();
                spriteObj2.Scale = Vector2.Zero;
                spriteObj2.Opacity = 0f;
                spriteObj2.Position = new Vector2(num3, sprite.Y - 100f);
                var num5 = CDGMath.RandomFloat(2f, 4f);
                Tween.To(spriteObj2, 0.5f, Tween.EaseNone, "Opacity", "1");
                Tween.By(spriteObj2, 4f, Tween.EaseNone, "Rotation", CDGMath.RandomInt(-180, 180).ToString());
                Tween.By(spriteObj2, 3f, Tween.EaseNone, "X", CDGMath.RandomInt(-20, 20).ToString(), "Y",
                    CDGMath.RandomInt(-50, -30).ToString());
                Tween.To(spriteObj2, 3f, Tween.EaseNone, "ScaleX", num5.ToString(), "ScaleY", num5.ToString());
                spriteObj2.Opacity = 1f;
                Tween.To(spriteObj2, 2f, Tween.EaseNone, "delay", CDGMath.RandomFloat(1f, 2f).ToString(), "Opacity",
                    "0");
                spriteObj2.Opacity = 0f;
                Tween.RunFunction(4.5f, spriteObj2, "StopAnimation");
                num3 += num2;
            }

            num /= 2;
            num3 = sprite.Bounds.Left + 100;
            num2 = (sprite.Width - 100) / (float) num;
            for (var k = 0; k < num; k++)
            {
                var spriteObj3 = m_resourcePool.CheckOut();
                spriteObj3.ChangeSprite("FountainShatterSmoke_Sprite");
                spriteObj3.Visible = true;
                spriteObj3.PlayAnimation();
                spriteObj3.Scale = Vector2.Zero;
                spriteObj3.Opacity = 0f;
                spriteObj3.Position = new Vector2(num3, sprite.Y - 200f);
                var num6 = CDGMath.RandomFloat(2f, 4f);
                Tween.To(spriteObj3, 0.5f, Tween.EaseNone, "Opacity", "1");
                Tween.By(spriteObj3, 4f, Tween.EaseNone, "Rotation", CDGMath.RandomInt(-180, 180).ToString());
                Tween.By(spriteObj3, 3f, Tween.EaseNone, "X", CDGMath.RandomInt(-20, 20).ToString(), "Y",
                    CDGMath.RandomInt(-50, -30).ToString());
                Tween.To(spriteObj3, 3f, Tween.EaseNone, "ScaleX", num6.ToString(), "ScaleY", num6.ToString());
                spriteObj3.Opacity = 1f;
                Tween.To(spriteObj3, 2f, Tween.EaseNone, "delay", CDGMath.RandomFloat(1f, 2f).ToString(), "Opacity",
                    "0");
                spriteObj3.Opacity = 0f;
                Tween.RunFunction(4.5f, spriteObj3, "StopAnimation");
                num3 += num2;
            }
        }

        public void DoorSparkleEffect(Rectangle rect)
        {
            var spriteObj = m_resourcePool.CheckOut();
            spriteObj.ChangeSprite("LevelUpParticleFX_Sprite");
            spriteObj.Visible = true;
            var num = CDGMath.RandomFloat(0.3f, 0.5f);
            spriteObj.Scale = new Vector2(num, num);
            spriteObj.Opacity = 0f;
            spriteObj.Position = new Vector2(CDGMath.RandomInt(rect.X, rect.X + rect.Width),
                CDGMath.RandomInt(rect.Y, rect.Y + rect.Height));
            spriteObj.Rotation = CDGMath.RandomInt(0, 90);
            spriteObj.PlayAnimation(false);
            Tween.To(spriteObj, 0.4f, Linear.EaseNone, "Opacity", "1");
            Tween.By(spriteObj, 0.6f, Linear.EaseNone, "Rotation", CDGMath.RandomInt(-45, 45).ToString(), "Y", "-50");
            Tween.To(spriteObj, 0.7f, Linear.EaseNone, "ScaleX", "0", "ScaleY", "0");
            Tween.AddEndHandlerToLastTween(spriteObj, "StopAnimation");
        }

        public void DestroyEffect(SpriteObj obj)
        {
            obj.OutlineWidth = 0;
            obj.Visible = false;
            obj.Rotation = 0f;
            obj.TextureColor = Color.White;
            obj.Opacity = 1f;
            m_resourcePool.CheckIn(obj);
            obj.Flip = SpriteEffects.None;
            obj.Scale = new Vector2(1f, 1f);
            obj.AnimationDelay = 0.0333333351f;
        }

        public void PauseAllAnimations()
        {
            m_isPaused = true;
            foreach (var current in m_resourcePool.ActiveObjsList) current.PauseAnimation();
        }

        public void ResumeAllAnimations()
        {
            m_isPaused = false;
            foreach (var current in m_resourcePool.ActiveObjsList) current.ResumeAnimation();
        }

        public void DestroyAllEffects()
        {
            foreach (var current in m_resourcePool.ActiveObjsList) current.StopAnimation();
        }

        public void Draw(Camera2D camera)
        {
            for (var i = 0; i < m_resourcePool.ActiveObjsList.Count; i++)
                if (!m_resourcePool.ActiveObjsList[i].IsAnimating && !m_isPaused)
                {
                    DestroyEffect(m_resourcePool.ActiveObjsList[i]);
                    i--;
                }
                else
                {
                    m_resourcePool.ActiveObjsList[i].Draw(camera);
                }
        }
    }
}
