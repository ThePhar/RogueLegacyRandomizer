using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
				return this.m_itemDropPool.CurrentPoolSize;
			}
		}
		public bool IsDisposed
		{
			get
			{
				return this.m_isDisposed;
			}
		}
		public ItemDropManager(int poolSize, PhysicsManager physicsManager)
		{
			this.m_itemDropPool = new DS2DPool<ItemDropObj>();
			this.m_itemDropsToRemoveList = new List<ItemDropObj>();
			this.m_poolSize = poolSize;
			this.m_physicsManager = physicsManager;
		}
		public void Initialize()
		{
			for (int i = 0; i < this.m_poolSize; i++)
			{
				ItemDropObj itemDropObj = new ItemDropObj("Coin_Sprite");
				itemDropObj.Visible = false;
				this.m_itemDropPool.AddToPool(itemDropObj);
			}
		}
		public void DropItem(Vector2 position, int dropType, float amount)
		{
			ItemDropObj itemDropObj = this.m_itemDropPool.CheckOutReturnNull();
			if (itemDropObj == null)
			{
				return;
			}
			itemDropObj.ConvertDrop(dropType, amount);
			this.m_physicsManager.AddObject(itemDropObj);
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
			ItemDropObj itemDropObj = this.m_itemDropPool.CheckOutReturnNull();
			if (itemDropObj == null)
			{
				return;
			}
			itemDropObj.ConvertDrop(dropType, amount);
			this.m_physicsManager.AddObject(itemDropObj);
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
			if (this.m_itemDropPool.ActiveObjsList.Contains(itemDrop))
			{
				itemDrop.Visible = false;
				itemDrop.TextureColor = Color.White;
				this.m_physicsManager.RemoveObject(itemDrop);
				this.m_itemDropPool.CheckIn(itemDrop);
			}
		}
		public void DestroyAllItemDrops()
		{
			ItemDropObj[] array = this.m_itemDropPool.ActiveObjsList.ToArray();
			ItemDropObj[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				ItemDropObj itemDrop = array2[i];
				this.DestroyItemDrop(itemDrop);
			}
		}
		public void PauseAllAnimations()
		{
			foreach (ItemDropObj current in this.m_itemDropPool.ActiveObjsList)
			{
				current.PauseAnimation();
			}
		}
		public void ResumeAllAnimations()
		{
			foreach (ItemDropObj current in this.m_itemDropPool.ActiveObjsList)
			{
				current.ResumeAnimation();
			}
		}
		public void Draw(Camera2D camera)
		{
			foreach (ItemDropObj current in this.m_itemDropPool.ActiveObjsList)
			{
				current.Draw(camera);
			}
		}
		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				Console.WriteLine("Disposing Item Drop Manager");
				this.DestroyAllItemDrops();
				this.m_itemDropsToRemoveList.Clear();
				this.m_itemDropsToRemoveList = null;
				this.m_itemDropPool.Dispose();
				this.m_itemDropPool = null;
				this.m_physicsManager = null;
				this.m_isDisposed = true;
			}
		}
	}
}
