using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class MapObj : GameObj
	{
		private PlayerObj m_player;
		private ProceduralLevelScreen m_level;
		private List<SpriteObj> m_roomSpriteList;
		private List<SpriteObj> m_doorSpriteList;
		private List<SpriteObj> m_iconSpriteList;
		private List<Vector2> m_roomSpritePosList;
		private List<Vector2> m_doorSpritePosList;
		private List<Vector2> m_iconSpritePosList;
		private SpriteObj m_playerSprite;
		public Vector2 CameraOffset;
		private Vector2 m_spriteScale;
		private List<RoomObj> m_addedRooms;
		private RenderTarget2D m_alphaMaskRT;
		private RenderTarget2D m_mapScreenRT;
		private Rectangle m_alphaMaskRect;
		private List<SpriteObj> m_teleporterList;
		private List<Vector2> m_teleporterPosList;
		private TweenObject m_xOffsetTween;
		private TweenObject m_yOffsetTween;
		public bool DrawTeleportersOnly
		{
			get;
			set;
		}
		public bool DrawNothing
		{
			get;
			set;
		}
		public bool FollowPlayer
		{
			get;
			set;
		}
		public List<RoomObj> AddedRoomsList
		{
			get
			{
				return this.m_addedRooms;
			}
		}
		public float CameraOffsetX
		{
			get
			{
				return this.CameraOffset.X;
			}
			set
			{
				this.CameraOffset.X = value;
			}
		}
		public float CameraOffsetY
		{
			get
			{
				return this.CameraOffset.Y;
			}
			set
			{
				this.CameraOffset.Y = value;
			}
		}
		public MapObj(bool followPlayer, ProceduralLevelScreen level)
		{
			this.m_level = level;
			this.FollowPlayer = followPlayer;
			base.Opacity = 0.3f;
			this.m_roomSpriteList = new List<SpriteObj>();
			this.m_doorSpriteList = new List<SpriteObj>();
			this.m_iconSpriteList = new List<SpriteObj>();
			this.m_roomSpritePosList = new List<Vector2>();
			this.m_doorSpritePosList = new List<Vector2>();
			this.m_iconSpritePosList = new List<Vector2>();
			this.CameraOffset = new Vector2(20f, 560f);
			this.m_playerSprite = new SpriteObj("MapPlayerIcon_Sprite");
			this.m_playerSprite.AnimationDelay = 0.0333333351f;
			this.m_playerSprite.ForceDraw = true;
			this.m_playerSprite.PlayAnimation(true);
			this.m_spriteScale = new Vector2(22f, 22.5f);
			this.m_addedRooms = new List<RoomObj>();
			this.m_teleporterList = new List<SpriteObj>();
			this.m_teleporterPosList = new List<Vector2>();
		}
		public void InitializeAlphaMap(Rectangle mapSize, Camera2D camera)
		{
			this.m_alphaMaskRect = mapSize;
			this.m_mapScreenRT = new RenderTarget2D(camera.GraphicsDevice, 1320, 720);
			this.m_alphaMaskRT = new RenderTarget2D(camera.GraphicsDevice, 1320, 720);
			this.CameraOffset = new Vector2((float)mapSize.X, (float)mapSize.Y);
			SpriteObj spriteObj = new SpriteObj("MapMask_Sprite");
			spriteObj.ForceDraw = true;
			spriteObj.Position = new Vector2((float)mapSize.X, (float)mapSize.Y);
			spriteObj.Scale = new Vector2((float)mapSize.Width / (float)spriteObj.Width, (float)mapSize.Height / (float)spriteObj.Height);
			camera.GraphicsDevice.SetRenderTarget(this.m_alphaMaskRT);
			camera.GraphicsDevice.Clear(Color.White);
			camera.Begin();
			spriteObj.Draw(camera);
			camera.End();
			camera.GraphicsDevice.SetRenderTarget(Game.ScreenManager.RenderTarget);
		}
		public void InitializeAlphaMap(RenderTarget2D mapScreenRT, RenderTarget2D alphaMaskRT, Rectangle mapSize)
		{
			this.m_mapScreenRT = mapScreenRT;
			this.m_alphaMaskRT = alphaMaskRT;
			this.m_alphaMaskRect = mapSize;
			this.CameraOffset = new Vector2((float)mapSize.X, (float)mapSize.Y);
		}
		public void DisposeRTs()
		{
			this.m_mapScreenRT.Dispose();
			this.m_mapScreenRT = null;
			this.m_alphaMaskRT.Dispose();
			this.m_alphaMaskRT = null;
		}
		public void SetPlayer(PlayerObj player)
		{
			this.m_player = player;
		}
		public void AddRoom(RoomObj room)
		{
			if (!this.m_addedRooms.Contains(room) && room.Width / 1320 < 5)
			{
				SpriteObj spriteObj = new SpriteObj(string.Concat(new object[]
				{
					"MapRoom",
					room.Width / 1320,
					"x",
					room.Height / 720,
					"_Sprite"
				}));
				spriteObj.Position = new Vector2(room.X / this.m_spriteScale.X, room.Y / this.m_spriteScale.Y);
				spriteObj.Scale = new Vector2(((float)spriteObj.Width - 3f) / (float)spriteObj.Width, ((float)spriteObj.Height - 3f) / (float)spriteObj.Height);
				spriteObj.ForceDraw = true;
				spriteObj.TextureColor = room.TextureColor;
				this.m_roomSpriteList.Add(spriteObj);
				this.m_roomSpritePosList.Add(spriteObj.Position);
				foreach (DoorObj current in room.DoorList)
				{
					if (!(room.Name == "CastleEntrance") || !(current.DoorPosition == "Left"))
					{
						bool flag = false;
						SpriteObj spriteObj2 = new SpriteObj("MapDoor_Sprite");
						spriteObj2.ForceDraw = true;
						string doorPosition;
						if ((doorPosition = current.DoorPosition) != null)
						{
							if (!(doorPosition == "Left"))
							{
								if (!(doorPosition == "Right"))
								{
									if (!(doorPosition == "Bottom"))
									{
										if (doorPosition == "Top")
										{
											spriteObj2.Rotation = -90f;
											spriteObj2.Position = new Vector2(current.X / this.m_spriteScale.X, current.Y / this.m_spriteScale.Y + 2f);
											flag = true;
										}
									}
									else
									{
										spriteObj2.Rotation = -90f;
										spriteObj2.Position = new Vector2(current.X / this.m_spriteScale.X, (current.Y + (float)current.Height) / this.m_spriteScale.Y + 2f);
										flag = true;
									}
								}
								else
								{
									spriteObj2.Position = new Vector2((float)room.Bounds.Right / this.m_spriteScale.X - 5f, current.Y / this.m_spriteScale.Y - 2f);
									flag = true;
								}
							}
							else
							{
								spriteObj2.Position = new Vector2((float)room.Bounds.Left / this.m_spriteScale.X - (float)spriteObj2.Width + 2f, current.Y / this.m_spriteScale.Y - 2f);
								flag = true;
							}
						}
						if (flag)
						{
							this.m_doorSpritePosList.Add(spriteObj2.Position);
							this.m_doorSpriteList.Add(spriteObj2);
						}
					}
				}
				if (room.Name != "Bonus" && Game.PlayerStats.Class != 13)
				{
					foreach (GameObj current2 in room.GameObjList)
					{
						ChestObj chestObj = current2 as ChestObj;
						if (chestObj != null)
						{
							SpriteObj spriteObj3;
							if (chestObj.IsOpen)
							{
								spriteObj3 = new SpriteObj("MapChestUnlocked_Sprite");
							}
							else if (chestObj is FairyChestObj)
							{
								spriteObj3 = new SpriteObj("MapFairyChestIcon_Sprite");
								if ((chestObj as FairyChestObj).ConditionType == 10)
								{
									spriteObj3.Opacity = 0.2f;
								}
							}
							else
							{
								spriteObj3 = new SpriteObj("MapLockedChestIcon_Sprite");
							}
							this.m_iconSpriteList.Add(spriteObj3);
							spriteObj3.AnimationDelay = 0.0333333351f;
							spriteObj3.PlayAnimation(true);
							spriteObj3.ForceDraw = true;
							spriteObj3.Position = new Vector2(current2.X / this.m_spriteScale.X - 8f, current2.Y / this.m_spriteScale.Y - 12f);
							if (room.IsReversed)
							{
								spriteObj3.X -= (float)current2.Width / this.m_spriteScale.X;
							}
							this.m_iconSpritePosList.Add(spriteObj3.Position);
						}
					}
				}
				if (room.Name == "EntranceBoss")
				{
					SpriteObj spriteObj4 = new SpriteObj("MapBossIcon_Sprite");
					spriteObj4.AnimationDelay = 0.0333333351f;
					spriteObj4.ForceDraw = true;
					spriteObj4.PlayAnimation(true);
					spriteObj4.Position = new Vector2((room.X + (float)room.Width / 2f) / this.m_spriteScale.X - (float)(spriteObj4.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / this.m_spriteScale.Y - (float)(spriteObj4.Height / 2) - 2f);
					this.m_iconSpriteList.Add(spriteObj4);
					this.m_iconSpritePosList.Add(spriteObj4.Position);
					this.m_teleporterList.Add(spriteObj4);
					this.m_teleporterPosList.Add(spriteObj4.Position);
				}
				else if (room.Name == "Linker")
				{
					SpriteObj spriteObj5 = new SpriteObj("MapTeleporterIcon_Sprite");
					spriteObj5.AnimationDelay = 0.0333333351f;
					spriteObj5.ForceDraw = true;
					spriteObj5.PlayAnimation(true);
					spriteObj5.Position = new Vector2((room.X + (float)room.Width / 2f) / this.m_spriteScale.X - (float)(spriteObj5.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / this.m_spriteScale.Y - (float)(spriteObj5.Height / 2) - 2f);
					this.m_iconSpriteList.Add(spriteObj5);
					this.m_iconSpritePosList.Add(spriteObj5.Position);
					this.m_teleporterList.Add(spriteObj5);
					this.m_teleporterPosList.Add(spriteObj5.Position);
				}
				else if (room.Name == "CastleEntrance")
				{
					SpriteObj spriteObj6 = new SpriteObj("MapTeleporterIcon_Sprite");
					spriteObj6.AnimationDelay = 0.0333333351f;
					spriteObj6.ForceDraw = true;
					spriteObj6.PlayAnimation(true);
					spriteObj6.Position = new Vector2((room.X + (float)room.Width / 2f) / this.m_spriteScale.X - (float)(spriteObj6.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / this.m_spriteScale.Y - (float)(spriteObj6.Height / 2) - 2f);
					this.m_iconSpriteList.Add(spriteObj6);
					this.m_iconSpritePosList.Add(spriteObj6.Position);
					this.m_teleporterList.Add(spriteObj6);
					this.m_teleporterPosList.Add(spriteObj6.Position);
				}
				if (Game.PlayerStats.Class != 13 && room.Name == "Bonus")
				{
					SpriteObj spriteObj7 = new SpriteObj("MapBonusIcon_Sprite");
					spriteObj7.PlayAnimation(true);
					spriteObj7.AnimationDelay = 0.0333333351f;
					spriteObj7.ForceDraw = true;
					spriteObj7.Position = new Vector2((room.X + (float)room.Width / 2f) / this.m_spriteScale.X - (float)(spriteObj7.Width / 2) - 1f, (room.Y + (float)room.Height / 2f) / this.m_spriteScale.Y - (float)(spriteObj7.Height / 2) - 2f);
					this.m_iconSpriteList.Add(spriteObj7);
					this.m_iconSpritePosList.Add(spriteObj7.Position);
				}
				this.m_addedRooms.Add(room);
			}
		}
		public void AddAllRooms(List<RoomObj> roomList)
		{
			foreach (RoomObj current in roomList)
			{
				this.AddRoom(current);
			}
		}
		public void AddAllIcons(List<RoomObj> roomList)
		{
			foreach (RoomObj current in roomList)
			{
				if (!this.m_addedRooms.Contains(current))
				{
					if (current.Name != "Bonus")
					{
						using (List<GameObj>.Enumerator enumerator2 = current.GameObjList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								GameObj current2 = enumerator2.Current;
								ChestObj chestObj = current2 as ChestObj;
								if (chestObj != null)
								{
									SpriteObj spriteObj;
									if (chestObj.IsOpen)
									{
										spriteObj = new SpriteObj("MapChestUnlocked_Sprite");
									}
									else if (chestObj is FairyChestObj)
									{
										spriteObj = new SpriteObj("MapFairyChestIcon_Sprite");
										if ((chestObj as FairyChestObj).ConditionType == 10)
										{
											spriteObj.Opacity = 0.2f;
										}
									}
									else
									{
										spriteObj = new SpriteObj("MapLockedChestIcon_Sprite");
									}
									this.m_iconSpriteList.Add(spriteObj);
									spriteObj.AnimationDelay = 0.0333333351f;
									spriteObj.PlayAnimation(true);
									spriteObj.ForceDraw = true;
									spriteObj.Position = new Vector2(current2.X / this.m_spriteScale.X - 8f, current2.Y / this.m_spriteScale.Y - 12f);
									if (current.IsReversed)
									{
										spriteObj.X -= (float)current2.Width / this.m_spriteScale.X;
									}
									this.m_iconSpritePosList.Add(spriteObj.Position);
								}
							}
							continue;
						}
					}
					if (current.Name == "Bonus")
					{
						SpriteObj spriteObj2 = new SpriteObj("MapBonusIcon_Sprite");
						spriteObj2.PlayAnimation(true);
						spriteObj2.AnimationDelay = 0.0333333351f;
						spriteObj2.ForceDraw = true;
						spriteObj2.Position = new Vector2((current.X + (float)current.Width / 2f) / this.m_spriteScale.X - (float)(spriteObj2.Width / 2) - 1f, (current.Y + (float)current.Height / 2f) / this.m_spriteScale.Y - (float)(spriteObj2.Height / 2) - 2f);
						this.m_iconSpriteList.Add(spriteObj2);
						this.m_iconSpritePosList.Add(spriteObj2.Position);
					}
				}
			}
		}
		public void AddLinkerRoom(GameTypes.LevelType levelType, List<RoomObj> roomList)
		{
			foreach (RoomObj current in roomList)
			{
				if (current.Name == "Linker" && current.LevelType == levelType)
				{
					this.AddRoom(current);
				}
			}
		}
		public void RefreshChestIcons(RoomObj room)
		{
			foreach (GameObj current in room.GameObjList)
			{
				ChestObj chestObj = current as ChestObj;
				if (chestObj != null && chestObj.IsOpen)
				{
					Vector2 pt = new Vector2(chestObj.X / this.m_spriteScale.X - 8f, chestObj.Y / this.m_spriteScale.Y - 12f);
					for (int i = 0; i < this.m_iconSpritePosList.Count; i++)
					{
						if (CDGMath.DistanceBetweenPts(pt, this.m_iconSpritePosList[i]) < 15f)
						{
							this.m_iconSpriteList[i].ChangeSprite("MapChestUnlocked_Sprite");
							this.m_iconSpriteList[i].Opacity = 1f;
							break;
						}
					}
				}
			}
		}
		public void CentreAroundPos(Vector2 pos, bool tween = false)
		{
			if (!tween)
			{
				this.CameraOffset.X = (float)this.m_alphaMaskRect.X + (float)this.m_alphaMaskRect.Width / 2f - pos.X / 1320f * 60f;
				this.CameraOffset.Y = (float)this.m_alphaMaskRect.Y + (float)this.m_alphaMaskRect.Height / 2f - pos.Y / 720f * 32f;
				return;
			}
			if (this.m_xOffsetTween != null && this.m_xOffsetTween.TweenedObject == this)
			{
				this.m_xOffsetTween.StopTween(false);
			}
			if (this.m_yOffsetTween != null && this.m_yOffsetTween.TweenedObject == this)
			{
				this.m_yOffsetTween.StopTween(false);
			}
			this.m_xOffsetTween = Tween.To(this, 0.3f, new Easing(Quad.EaseOut), new string[]
			{
				"CameraOffsetX",
				((float)this.m_alphaMaskRect.X + (float)this.m_alphaMaskRect.Width / 2f - pos.X / 1320f * 60f).ToString()
			});
			this.m_yOffsetTween = Tween.To(this, 0.3f, new Easing(Quad.EaseOut), new string[]
			{
				"CameraOffsetY",
				((float)this.m_alphaMaskRect.Y + (float)this.m_alphaMaskRect.Height / 2f - pos.Y / 720f * 32f).ToString()
			});
		}
		public void CentreAroundObj(GameObj obj)
		{
			this.CentreAroundPos(obj.Position, false);
		}
		public void CentreAroundPlayer()
		{
			this.CentreAroundObj(this.m_player);
		}
		public void TeleportPlayer(int index)
		{
			if (this.m_teleporterList.Count > 0)
			{
				Vector2 position = this.m_teleporterPosList[index];
				position.X += 10f;
				position.Y += 10f;
				position.X *= this.m_spriteScale.X;
				position.Y *= this.m_spriteScale.Y;
				position.X += 30f;
				if (index == 0)
				{
					position.X -= 610f;
					position.Y += 230f;
				}
				else
				{
					position.Y += 290f;
				}
				this.m_player.TeleportPlayer(position, null);
			}
		}
		public void CentreAroundTeleporter(int index, bool tween = false)
		{
			Vector2 pos = this.m_teleporterPosList[index];
			pos.X *= this.m_spriteScale.X;
			pos.Y *= this.m_spriteScale.Y;
			this.CentreAroundPos(pos, tween);
		}
		public void DrawRenderTargets(Camera2D camera)
		{
			if (this.FollowPlayer)
			{
				this.CameraOffset.X = (float)((int)((float)this.m_alphaMaskRect.X + (float)this.m_alphaMaskRect.Width / 2f - this.m_player.X / 1320f * 60f));
				this.CameraOffset.Y = (float)this.m_alphaMaskRect.Y + (float)this.m_alphaMaskRect.Height / 2f - (float)((int)this.m_player.Y) / 720f * 32f;
			}
			camera.GraphicsDevice.SetRenderTarget(this.m_mapScreenRT);
			camera.GraphicsDevice.Clear(Color.Transparent);
			for (int i = 0; i < this.m_roomSpriteList.Count; i++)
			{
				this.m_roomSpriteList[i].Position = this.CameraOffset + this.m_roomSpritePosList[i];
				this.m_roomSpriteList[i].Draw(camera);
			}
			for (int j = 0; j < this.m_doorSpriteList.Count; j++)
			{
				this.m_doorSpriteList[j].Position = this.CameraOffset + this.m_doorSpritePosList[j];
				this.m_doorSpriteList[j].Draw(camera);
			}
			if (!this.DrawTeleportersOnly)
			{
				for (int k = 0; k < this.m_iconSpriteList.Count; k++)
				{
					this.m_iconSpriteList[k].Position = this.CameraOffset + this.m_iconSpritePosList[k];
					this.m_iconSpriteList[k].Draw(camera);
				}
			}
			else
			{
				for (int l = 0; l < this.m_teleporterList.Count; l++)
				{
					this.m_teleporterList[l].Position = this.CameraOffset + this.m_teleporterPosList[l];
					this.m_teleporterList[l].Draw(camera);
				}
			}
			if (Game.PlayerStats.Traits.X == 28f || Game.PlayerStats.Traits.Y == 28f)
			{
				this.m_playerSprite.TextureColor = Color.Red;
				foreach (RoomObj current in this.m_addedRooms)
				{
					foreach (EnemyObj current2 in current.EnemyList)
					{
						if (!current2.IsKilled && !current2.IsDemented && current2.SaveToFile && current2.Type != 21 && current2.Type != 27 && current2.Type != 17)
						{
							this.m_playerSprite.Position = new Vector2(current2.X / this.m_spriteScale.X - 9f, current2.Y / this.m_spriteScale.Y - 10f) + this.CameraOffset;
							this.m_playerSprite.Draw(camera);
						}
					}
				}
			}
			this.m_playerSprite.TextureColor = Color.White;
			this.m_playerSprite.Position = new Vector2(this.m_level.Player.X / this.m_spriteScale.X - 9f, this.m_level.Player.Y / this.m_spriteScale.Y - 10f) + this.CameraOffset;
			this.m_playerSprite.Draw(camera);
		}
		public override void Draw(Camera2D camera)
		{
			if (base.Visible)
			{
				camera.End();
				camera.GraphicsDevice.Textures[1] = this.m_alphaMaskRT;
				camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.MaskEffect);
				if (!this.DrawNothing)
				{
					camera.Draw(this.m_mapScreenRT, Vector2.Zero, Color.White);
				}
				camera.End();
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
				if (this.DrawNothing)
				{
					this.m_playerSprite.Draw(camera);
				}
			}
		}
		public void ClearRoomsAdded()
		{
			this.m_addedRooms.Clear();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_player = null;
				this.m_level = null;
				if (this.m_alphaMaskRT != null && !this.m_alphaMaskRT.IsDisposed)
				{
					this.m_alphaMaskRT.Dispose();
				}
				this.m_alphaMaskRT = null;
				if (this.m_mapScreenRT != null && !this.m_mapScreenRT.IsDisposed)
				{
					this.m_mapScreenRT.Dispose();
				}
				this.m_mapScreenRT = null;
				foreach (SpriteObj current in this.m_roomSpriteList)
				{
					current.Dispose();
				}
				this.m_roomSpriteList.Clear();
				this.m_roomSpriteList = null;
				foreach (SpriteObj current2 in this.m_doorSpriteList)
				{
					current2.Dispose();
				}
				this.m_doorSpriteList.Clear();
				this.m_doorSpriteList = null;
				foreach (SpriteObj current3 in this.m_iconSpriteList)
				{
					current3.Dispose();
				}
				this.m_iconSpriteList.Clear();
				this.m_iconSpriteList = null;
				this.m_addedRooms.Clear();
				this.m_addedRooms = null;
				this.m_roomSpritePosList.Clear();
				this.m_roomSpritePosList = null;
				this.m_doorSpritePosList.Clear();
				this.m_doorSpritePosList = null;
				this.m_iconSpritePosList.Clear();
				this.m_iconSpritePosList = null;
				this.m_playerSprite.Dispose();
				this.m_playerSprite = null;
				foreach (SpriteObj current4 in this.m_teleporterList)
				{
					current4.Dispose();
				}
				this.m_teleporterList.Clear();
				this.m_teleporterList = null;
				this.m_teleporterPosList.Clear();
				this.m_teleporterPosList = null;
				this.m_xOffsetTween = null;
				this.m_yOffsetTween = null;
				base.Dispose();
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new MapObj(this.FollowPlayer, this.m_level);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			MapObj mapObj = obj as MapObj;
			mapObj.DrawTeleportersOnly = this.DrawTeleportersOnly;
			mapObj.CameraOffsetX = this.CameraOffsetX;
			mapObj.CameraOffsetY = this.CameraOffsetY;
			mapObj.InitializeAlphaMap(this.m_mapScreenRT, this.m_alphaMaskRT, this.m_alphaMaskRect);
			mapObj.SetPlayer(this.m_player);
			mapObj.AddAllRooms(this.m_addedRooms);
		}
		public SpriteObj[] TeleporterList()
		{
			return this.m_teleporterList.ToArray();
		}
	}
}
