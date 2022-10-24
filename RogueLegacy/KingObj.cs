// Rogue Legacy Randomizer - KingObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueLegacy
{
    public class KingObj : PhysicsObj
    {
        public KingObj(string spriteName) : base(spriteName) { }

        public bool WasHit { get; private set; }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            var physicsObj = otherBox.AbsParent as IPhysicsObj;
            if (collisionResponseType == 2 && physicsObj.CollisionTypeTag == 2 && !WasHit)
            {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "EnemyHit1", "EnemyHit2", "EnemyHit3",
                    "EnemyHit4", "EnemyHit5", "EnemyHit6");
                SoundManager.PlaySound("Boss_Title_Exit");
                SoundManager.PlaySound("Player_Death_Grunt");
                var center = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                if (thisBox.AbsRotation != 0f || otherBox.AbsRotation != 0f)
                {
                    center = Rectangle.Intersect(thisBox.AbsParent.Bounds, otherBox.AbsParent.Bounds).Center;
                }

                var position = new Vector2(center.X, center.Y);
                (otherBox.AbsParent as PlayerObj).AttachedLevel.ImpactEffectPool.DisplayEnemyImpactEffect(position);
                WasHit = true;
            }

            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new KingObj(SpriteName);
        }
    }
}
