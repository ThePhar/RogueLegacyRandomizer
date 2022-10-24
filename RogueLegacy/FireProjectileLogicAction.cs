// Rogue Legacy Randomizer - FireProjectileLogicAction.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;

namespace RogueLegacy
{
    public class FireProjectileLogicAction : LogicAction
    {
        private ProjectileData m_data;
        private ProjectileManager m_projectileManager;

        public FireProjectileLogicAction(ProjectileManager projectileManager, ProjectileData data)
        {
            m_projectileManager = projectileManager;
            m_data = data.Clone();
        }

        public override void Execute()
        {
            if (ParentLogicSet != null && ParentLogicSet.IsActive)
            {
                var arg_20_0 = ParentLogicSet.ParentGameObj;
                m_projectileManager.FireProjectile(m_data);
                base.Execute();
            }
        }

        public override object Clone()
        {
            return new FireProjectileLogicAction(m_projectileManager, m_data);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_projectileManager = null;
                m_data = null;
                base.Dispose();
            }
        }
    }
}
