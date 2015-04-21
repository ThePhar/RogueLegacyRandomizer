/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;

namespace RogueCastle
{
    public class FireProjectileLogicAction : LogicAction
    {
        private ProjectileManager m_projectileManager;
        private ProjectileData m_data;

        public FireProjectileLogicAction(ProjectileManager projectileManager, ProjectileData data)
        {
            m_projectileManager = projectileManager;
            m_data = data.Clone();
        }

        public override void Execute()
        {
            if (ParentLogicSet != null && ParentLogicSet.IsActive)
            {
                GameObj arg_20_0 = ParentLogicSet.ParentGameObj;
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