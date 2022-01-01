/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class ArenaBonusRoom : BonusRoomObj
    {
        private ChestObj m_chest;
        private bool m_chestRevealed;
        private float m_chestStartingY;

        public override void Initialize()
        {
            foreach (var current in GameObjList)
                if (current is ChestObj)
                {
                    m_chest = current as ChestObj;
                    break;
                }

            m_chest.ChestType = 3;
            m_chestStartingY = m_chest.Y - 200f + m_chest.Height + 6f;
            base.Initialize();
        }

        public override void OnEnter()
        {
            UpdateEnemyNames();
            m_chest.Y = m_chestStartingY;
            m_chest.ChestType = 3;
            if (RoomCompleted)
            {
                m_chest.Opacity = 1f;
                m_chest.Y = m_chestStartingY + 200f;
                m_chest.IsEmpty = true;
                m_chest.ForceOpen();
                m_chestRevealed = true;
                using (var enumerator = EnemyList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        if (!current.IsKilled)
                        {
                            current.KillSilently();
                        }
                    }
                }
            }
            else
            {
                if (ActiveEnemies == 0)
                {
                    m_chest.Opacity = 1f;
                    m_chest.Y = m_chestStartingY + 200f;
                    m_chest.IsEmpty = false;
                    m_chest.IsLocked = false;
                    m_chestRevealed = true;
                }
                else
                {
                    m_chest.Opacity = 0f;
                    m_chest.Y = m_chestStartingY;
                    m_chest.IsLocked = true;
                    m_chestRevealed = false;
                }
            }

            if (m_chest.PhysicsMngr == null)
            {
                Player.PhysicsMngr.AddObject(m_chest);
            }

            base.OnEnter();
        }

        private void UpdateEnemyNames()
        {
            var flag = false;
            foreach (var current in EnemyList)
                if (current is EnemyObj_EarthWizard)
                {
                    if (!flag)
                    {
                        current.Name = "Barbatos";
                        flag = true;
                    }
                    else
                    {
                        current.Name = "Amon";
                    }
                }
                else if (current is EnemyObj_Skeleton)
                {
                    if (!flag)
                    {
                        current.Name = "Berith";
                        flag = true;
                    }
                    else
                    {
                        current.Name = "Halphas";
                    }
                }
                else if (current is EnemyObj_Plant)
                {
                    if (!flag)
                    {
                        current.Name = "Stolas";
                        flag = true;
                    }
                    else
                    {
                        current.Name = "Focalor";
                    }
                }
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_chest.IsOpen)
            {
                if (ActiveEnemies == 0 && m_chest.Opacity == 0f && !m_chestRevealed)
                {
                    m_chestRevealed = true;
                    DisplayChest();
                }
            }
            else if (!RoomCompleted)
            {
                RoomCompleted = true;
            }

            base.Update(gameTime);
        }

        public override void OnExit()
        {
            var flag = false;
            var flag2 = false;
            var flag3 = false;
            var flag4 = false;
            var flag5 = false;
            if (Game.PlayerStats.EnemiesKilledList[15].W > 0f)
            {
                flag = true;
            }

            if (Game.PlayerStats.EnemiesKilledList[22].W > 0f)
            {
                flag2 = true;
            }

            if (Game.PlayerStats.EnemiesKilledList[32].W > 0f)
            {
                flag3 = true;
            }

            if (Game.PlayerStats.EnemiesKilledList[12].W > 0f)
            {
                flag4 = true;
            }

            if (Game.PlayerStats.EnemiesKilledList[5].W > 0f)
            {
                flag5 = true;
            }

            if (flag && flag2 && flag3 && flag4 && flag5)
            {
                GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");
            }

            base.OnExit();
        }

        private void DisplayChest()
        {
            m_chest.IsLocked = false;
            Tween.To(m_chest, 2f, Tween.EaseNone, "Opacity", "1");
            Tween.By(m_chest, 2f, Quad.EaseOut, "Y", "200");
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            m_chest = null;
            base.Dispose();
        }
    }
}