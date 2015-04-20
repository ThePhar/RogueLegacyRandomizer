/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class ItemDropManager : IDisposableObj
	{
		private DS2DPool<ItemDropObj> m_itemDropPool;
		private List<ItemDropObj> m_itemDropsToRemoveList;
		private int m_poolSize;
		private PhysicsManager m_physicsManager;
		private bool m_isDisposed;
		public int AvailableItems
		{
			get
			{
				return m_itemDropPool.CurrentPoolSize;
			}
		}
		public bool IsDisposed
		{
			get
			{
				return m_isDisposed;
			}
		}
		public ItemDropManager(int poolSize, PhysicsManager physicsManager)
		{
			m_itemDropPool = new DS2DPool<ItemDropObj>();
			m_itemDropsToRemoveList = new List<ItemDropObj>();
			m_poolSize = poolSize;
			m_physicsManager = physicsManager;
		}
		public void Initialize()
		{
			for (int i = 0; i < m_poolSize; i++)
			{
				ItemDropObj itemDropObj = new ItemDropObj("Coin_Sprite");
				itemDropObj.Visible = false;
				m_itemDropPool.AddToPool(itemDropObj);
			}
		}
		public void DropItem(Vector2 position, int dropType, float amount)
		{
			ItemDropObj itemDropObj = m_itemDropPool.CheckOutReturnNull();
			if (itemDropObj == null)
			{
				return;
			}
			itemDropObj.ConvertDrop(dropType, amount);
			m_physicsManager.AddObject(itemDropObj);
			itemDropObj.Position = position;
			itemDropObj.AccelerationY = CDGMath.RandomFloat(-720f, -480f);
			itemDropObj.AccelerationX = CDGMath.RandomFloat(-120f, 120f);
			itemDropObj.Visible = true;
			itemDropObj.IsWeighted = true;
			itemDropObj.IsCollidable = true;
			itemDropObj.AnimationDelay = 0.05f;
			itemDropObj.Opacity = 1f;
			itemDropObj.CollectionCounter = 0.2f;
			SoundManager.Play3DSound(itemDropObj, Game.ScreenManager.Player, new string[]
			{
				"CoinCollect1",
				"CoinCollect2",
				"CoinCollect3"
			});
		}
		public void DropItemWide(Vector2 position, int dropType, float amount)
		{
			ItemDropObj itemDropObj = m_itemDropPool.CheckOutReturnNull();
			if (itemDropObj == null)
			{
				return;
			}
			itemDropObj.ConvertDrop(dropType, amount);
			m_physicsManager.AddObject(itemDropObj);
			itemDropObj.Position = position;
			itemDropObj.AccelerationY = CDGMath.RandomFloat(-1200f, -300f);
			itemDropObj.AccelerationX = CDGMath.RandomFloat(-600f, 600f);
			itemDropObj.Visible = true;
			itemDropObj.IsWeighted = true;
			itemDropObj.IsCollidable = true;
			itemDropObj.AnimationDelay = 0.05f;
			itemDropObj.Opacity = 1f;
			itemDropObj.CollectionCounter = 0.2f;
		}
		public void DestroyItemDrop(ItemDropObj itemDrop)
		{
			if (m_itemDropPool.ActiveObjsList.Contains(itemDrop))
			{
				itemDrop.Visible = false;
				itemDrop.TextureColor = Color.White;
				m_physicsManager.RemoveObject(itemDrop);
				m_itemDropPool.CheckIn(itemDrop);
			}
		}
		public void DestroyAllItemDrops()
		{
			ItemDropObj[] array = m_itemDropPool.ActiveObjsList.ToArray();
			ItemDropObj[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				ItemDropObj itemDrop = array2[i];
				DestroyItemDrop(itemDrop);
			}
		}
		public void PauseAllAnimations()
		{
			foreach (ItemDropObj current in m_itemDropPool.ActiveObjsList)
			{
				current.PauseAnimation();
			}
		}
		public void ResumeAllAnimations()
		{
			foreach (ItemDropObj current in m_itemDropPool.ActiveObjsList)
			{
				current.ResumeAnimation();
			}
		}
		public void Draw(Camera2D camera)
		{
			foreach (ItemDropObj current in m_itemDropPool.ActiveObjsList)
			{
				current.Draw(camera);
			}
		}
		public void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Item Drop Manager");
				DestroyAllItemDrops();
				m_itemDropsToRemoveList.Clear();
				m_itemDropsToRemoveList = null;
				m_itemDropPool.Dispose();
				m_itemDropPool = null;
				m_physicsManager = null;
				m_isDisposed = true;
			}
		}
	}
}
