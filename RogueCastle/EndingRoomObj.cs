using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	internal class EndingRoomObj : RoomObj
	{
		private SpriteObj m_endingMask;
		private List<Vector2> m_cameraPosList;
		private int m_waypointIndex;
		private List<SpriteObj> m_frameList;
		private List<TextObj> m_nameList;
		private List<TextObj> m_slainCountText;
		private List<SpriteObj> m_plaqueList;
		private BackgroundObj m_background;
		private KeyIconTextObj m_continueText;
		private bool m_displayingContinueText;
		private float m_waypointSpeed = 5f;
		private EnemyObj_Blob m_blobBoss;
		public EndingRoomObj()
		{
			this.m_plaqueList = new List<SpriteObj>();
		}
		public override void InitializeRenderTarget(RenderTarget2D bgRenderTarget)
		{
			if (this.m_background != null)
			{
				this.m_background.Dispose();
			}
			this.m_background = new BackgroundObj("LineageScreenBG_Sprite");
			this.m_background.SetRepeated(true, true, Game.ScreenManager.Camera, null);
			this.m_background.X -= 6600f;
			this.m_background.Opacity = 0.7f;
			base.InitializeRenderTarget(bgRenderTarget);
		}
		public override void LoadContent(GraphicsDevice graphics)
		{
			this.m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_continueText.FontSize = 14f;
			this.m_continueText.Align = Types.TextAlign.Right;
			this.m_continueText.Position = new Vector2(1270f, 650f);
			this.m_continueText.ForceDraw = true;
			this.m_continueText.Opacity = 0f;
			this.m_background = new BackgroundObj("LineageScreenBG_Sprite");
			this.m_background.SetRepeated(true, true, Game.ScreenManager.Camera, null);
			this.m_background.X -= 6600f;
			this.m_background.Opacity = 0.7f;
			this.m_endingMask = new SpriteObj("Blank_Sprite");
			this.m_endingMask.ForceDraw = true;
			this.m_endingMask.TextureColor = Color.Black;
			this.m_endingMask.Scale = new Vector2(1330f / (float)this.m_endingMask.Width, 730f / (float)this.m_endingMask.Height);
			this.m_cameraPosList = new List<Vector2>();
			this.m_frameList = new List<SpriteObj>();
			this.m_nameList = new List<TextObj>();
			this.m_slainCountText = new List<TextObj>();
			foreach (GameObj current in base.GameObjList)
			{
				if (current is WaypointObj)
				{
					this.m_cameraPosList.Add(default(Vector2));
				}
			}
			CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
			foreach (GameObj current2 in base.GameObjList)
			{
				if (current2 is WaypointObj)
				{
					int index = int.Parse(current2.Name, NumberStyles.Any, cultureInfo);
					this.m_cameraPosList[index] = current2.Position;
				}
			}
			float num = 150f;
			foreach (EnemyObj current3 in base.EnemyList)
			{
				current3.Initialize();
				current3.PauseEnemy(true);
				current3.IsWeighted = false;
				current3.PlayAnimation(true);
				current3.UpdateCollisionBoxes();
				SpriteObj spriteObj = new SpriteObj("LineageScreenFrame_Sprite");
				spriteObj.DropShadow = new Vector2(4f, 6f);
				if (current3.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
				{
					spriteObj.ChangeSprite("GiantPortrait_Sprite");
					this.FixMiniboss(current3);
				}
				spriteObj.Scale = new Vector2(((float)current3.Width + num) / (float)spriteObj.Width, ((float)current3.Height + num) / (float)spriteObj.Height);
				if (spriteObj.ScaleX < 1f)
				{
					spriteObj.ScaleX = 1f;
				}
				if (spriteObj.ScaleY < 1f)
				{
					spriteObj.ScaleY = 1f;
				}
				spriteObj.Position = new Vector2(current3.X, (float)current3.Bounds.Top + (float)current3.Height / 2f);
				this.m_frameList.Add(spriteObj);
				TextObj textObj = new TextObj(Game.JunicodeFont);
				textObj.FontSize = 12f;
				textObj.Align = Types.TextAlign.Centre;
				textObj.Text = current3.Name;
				textObj.OutlineColour = new Color(181, 142, 39);
				textObj.OutlineWidth = 2;
				textObj.Position = new Vector2(spriteObj.X, (float)(spriteObj.Bounds.Bottom + 40));
				this.m_nameList.Add(textObj);
				TextObj textObj2 = new TextObj(Game.JunicodeFont);
				textObj2.FontSize = 10f;
				textObj2.Align = Types.TextAlign.Centre;
				textObj2.OutlineColour = new Color(181, 142, 39);
				textObj2.Text = "Slain: 0";
				textObj2.OutlineWidth = 2;
				textObj2.HeadingX = (float)current3.Type;
				textObj2.HeadingY = (float)current3.Difficulty;
				textObj2.Position = new Vector2(spriteObj.X, (float)(spriteObj.Bounds.Bottom + 80));
				this.m_slainCountText.Add(textObj2);
				byte type = current3.Type;
				if (type <= 15)
				{
					if (type != 1)
					{
						if (type != 7)
						{
							if (type == 15)
							{
								if (current3.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
								{
									if (current3.Flip == SpriteEffects.None)
									{
										current3.X -= 25f;
									}
									else
									{
										current3.X += 25f;
									}
								}
							}
						}
						else if (current3.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
						{
							current3.X += 30f;
							current3.Y -= 20f;
						}
					}
					else
					{
						(current3 as EnemyObj_BallAndChain).BallAndChain.Visible = false;
						(current3 as EnemyObj_BallAndChain).BallAndChain2.Visible = false;
					}
				}
				else if (type != 20)
				{
					if (type != 29)
					{
						if (type == 32)
						{
							if (current3.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
							{
								spriteObj.Visible = false;
							}
						}
					}
					else
					{
						if (current3.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
						{
							(current3 as EnemyObj_LastBoss).ForceSecondForm(true);
							current3.ChangeSprite("EnemyLastBossIdle_Character");
							current3.PlayAnimation(true);
						}
						spriteObj.ChangeSprite("GiantPortrait_Sprite");
						spriteObj.Scale = Vector2.One;
						spriteObj.Scale = new Vector2(((float)current3.Width + num) / (float)spriteObj.Width, ((float)current3.Height + num) / (float)spriteObj.Height);
						textObj.Position = new Vector2(spriteObj.X, (float)(spriteObj.Bounds.Bottom + 40));
						textObj2.Position = new Vector2(spriteObj.X, (float)(spriteObj.Bounds.Bottom + 80));
					}
				}
				else
				{
					current3.ChangeSprite("EnemyZombieWalk_Character");
					current3.PlayAnimation(true);
				}
				SpriteObj spriteObj2 = new SpriteObj("LineageScreenPlaque1Long_Sprite");
				spriteObj2.Scale = new Vector2(1.8f, 1.8f);
				spriteObj2.Position = new Vector2(spriteObj.X, (float)(spriteObj.Bounds.Bottom + 80));
				this.m_plaqueList.Add(spriteObj2);
			}
			base.LoadContent(graphics);
		}
		private void FixMiniboss(EnemyObj enemy)
		{
			byte type = enemy.Type;
			switch (type)
			{
			case 2:
				this.m_blobBoss = (enemy as EnemyObj_Blob);
				enemy.ChangeSprite("EnemyBlobBossIdle_Character");
				enemy.GetChildAt(0).TextureColor = Color.White;
				enemy.GetChildAt(2).TextureColor = Color.LightSkyBlue;
				enemy.GetChildAt(2).Opacity = 0.8f;
				(enemy.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
				enemy.GetChildAt(1).TextureColor = Color.Black;
				break;
			case 3:
			case 4:
				break;
			case 5:
				if (enemy.Flip == SpriteEffects.None)
				{
					enemy.Name = "Amon";
				}
				else
				{
					enemy.Name = "Barbatos";
				}
				break;
			case 6:
				enemy.ChangeSprite("EnemyEyeballBossEye_Character");
				(enemy as EnemyObj_Eyeball).ChangeToBossPupil();
				break;
			case 7:
				enemy.ChangeSprite("EnemyFairyGhostBossIdle_Character");
				break;
			case 8:
				enemy.ChangeSprite("EnemyGhostBossIdle_Character");
				break;
			default:
				if (type != 15)
				{
					if (type == 22)
					{
						if (enemy.Flip == SpriteEffects.None)
						{
							enemy.Name = "Stolas";
						}
						else
						{
							enemy.Name = "Focalor";
						}
					}
				}
				else if (enemy.Flip == SpriteEffects.None)
				{
					enemy.Name = "Berith";
				}
				else
				{
					enemy.Name = "Halphas";
				}
				break;
			}
			enemy.PlayAnimation(true);
		}
		public override void OnEnter()
		{
			this.m_blobBoss.PlayAnimation(true);
			foreach (EnemyObj current in base.EnemyList)
			{
				if (current.Type == 5)
				{
					(current as EnemyObj_EarthWizard).EarthProjectile.Visible = false;
				}
			}
			this.m_displayingContinueText = false;
			this.UpdateEnemiesSlainText();
			this.m_continueText.Text = "Press [Input:" + 0 + "] to exit";
			this.m_continueText.Opacity = 0f;
			this.Player.AttachedLevel.Camera.Position = new Vector2(0f, 360f);
			this.Player.Position = new Vector2(100f, 100f);
			this.m_waypointIndex = 1;
			this.Player.ForceInvincible = true;
			this.Player.AttachedLevel.SetMapDisplayVisibility(false);
			this.Player.AttachedLevel.SetPlayerHUDVisibility(false);
			SoundManager.PlayMusic("EndSongDrums", true, 1f);
			Game.PlayerStats.TutorialComplete = true;
			this.Player.LockControls();
			this.Player.Visible = false;
			this.Player.Opacity = 0f;
			this.Player.AttachedLevel.CameraLockedToPlayer = false;
			base.OnEnter();
			this.ChangeWaypoints();
		}
		private void UpdateEnemiesSlainText()
		{
			foreach (TextObj current in this.m_slainCountText)
			{
				int index = (int)((byte)current.HeadingX);
				int num = (int)current.HeadingY;
				int num2 = 0;
				switch (num)
				{
				case 0:
					num2 = (int)Game.PlayerStats.EnemiesKilledList[index].X;
					break;
				case 1:
					num2 = (int)Game.PlayerStats.EnemiesKilledList[index].Y;
					break;
				case 2:
					num2 = (int)Game.PlayerStats.EnemiesKilledList[index].Z;
					break;
				case 3:
					num2 = (int)Game.PlayerStats.EnemiesKilledList[index].W;
					break;
				}
				current.Text = "Slain: " + num2;
			}
		}
		public void ChangeWaypoints()
		{
			if (this.m_waypointIndex < this.m_cameraPosList.Count)
			{
				object arg_91_0 = this.Player.AttachedLevel.Camera;
				float arg_91_1 = 1.5f;
				Easing arg_91_2 = new Easing(Quad.EaseInOut);
				string[] array = new string[4];
				array[0] = "X";
				string[] arg_66_0 = array;
				int arg_66_1 = 1;
				float x = this.m_cameraPosList[this.m_waypointIndex].X;
				arg_66_0[arg_66_1] = x.ToString();
				array[2] = "Y";
				string[] arg_8F_0 = array;
				int arg_8F_1 = 3;
				float y = this.m_cameraPosList[this.m_waypointIndex].Y;
				arg_8F_0[arg_8F_1] = y.ToString();
				Tween.To(arg_91_0, arg_91_1, arg_91_2, array);
				object arg_10A_0 = this.Player;
				float arg_10A_1 = 1.5f;
				Easing arg_10A_2 = new Easing(Quad.EaseInOut);
				string[] array2 = new string[4];
				array2[0] = "X";
				string[] arg_DE_0 = array2;
				int arg_DE_1 = 1;
				float x2 = this.m_cameraPosList[this.m_waypointIndex].X;
				arg_DE_0[arg_DE_1] = x2.ToString();
				array2[2] = "Y";
				string[] arg_108_0 = array2;
				int arg_108_1 = 3;
				float y2 = this.m_cameraPosList[this.m_waypointIndex].Y;
				arg_108_0[arg_108_1] = y2.ToString();
				Tween.To(arg_10A_0, arg_10A_1, arg_10A_2, array2);
				this.m_waypointIndex++;
				if (this.m_waypointIndex > this.m_cameraPosList.Count - 1)
				{
					this.m_waypointIndex = 0;
					Tween.RunFunction(0f, this.Player.AttachedLevel.ScreenManager, "DisplayScreen", new object[]
					{
						18,
						true,
						typeof(List<object>)
					});
					return;
				}
				Tween.RunFunction(this.m_waypointSpeed, this, "ChangeWaypoints", new object[0]);
			}
		}
		public void ChangeLevelType()
		{
			base.LevelType = GameTypes.LevelType.DUNGEON;
			this.Player.AttachedLevel.UpdateLevel(base.LevelType);
		}
		public override void Update(GameTime gameTime)
		{
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				if (this.m_displayingContinueText)
				{
					Tween.StopAll(false);
					Game.ScreenManager.DisplayScreen(18, true, null);
				}
				else
				{
					this.m_displayingContinueText = true;
					Tween.StopAllContaining(this.m_continueText, false);
					Tween.To(this.m_continueText, 0.5f, new Easing(Tween.EaseNone), new string[]
					{
						"Opacity",
						"1"
					});
					Tween.RunFunction(4f, this, "HideContinueText", new object[0]);
				}
			}
			base.Update(gameTime);
		}
		public void HideContinueText()
		{
			this.m_displayingContinueText = false;
			Tween.To(this.m_continueText, 0.5f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0",
				"Opacity",
				"0"
			});
		}
		public override void Draw(Camera2D camera)
		{
			this.m_continueText.Position = new Vector2((float)(camera.Bounds.Right - 50), (float)(camera.Bounds.Bottom - 70));
			this.m_endingMask.Position = camera.Position - new Vector2(660f, 360f);
			this.m_endingMask.Draw(camera);
			if (camera.X > this.m_background.X + 6600f)
			{
				this.m_background.X = camera.X;
			}
			if (camera.X < this.m_background.X)
			{
				this.m_background.X = camera.X - 1320f;
			}
			this.m_background.Draw(camera);
			foreach (SpriteObj current in this.m_frameList)
			{
				current.Draw(camera);
			}
			foreach (SpriteObj current2 in this.m_plaqueList)
			{
				current2.Draw(camera);
			}
			base.Draw(camera);
			camera.End();
			if (!LevelEV.SHOW_ENEMY_RADII)
			{
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.GetTransformation());
			}
			else
			{
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.GetTransformation());
			}
			foreach (TextObj current3 in this.m_nameList)
			{
				current3.Draw(camera);
			}
			foreach (TextObj current4 in this.m_slainCountText)
			{
				current4.Draw(camera);
			}
			this.m_continueText.Draw(camera);
			camera.End();
			if (!LevelEV.SHOW_ENEMY_RADII)
			{
				camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
				return;
			}
			camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
		}
		protected override GameObj CreateCloneInstance()
		{
			return new EndingRoomObj();
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				foreach (SpriteObj current in this.m_frameList)
				{
					current.Dispose();
				}
				this.m_frameList.Clear();
				this.m_frameList = null;
				foreach (SpriteObj current2 in this.m_plaqueList)
				{
					current2.Dispose();
				}
				this.m_plaqueList.Clear();
				this.m_plaqueList = null;
				this.m_cameraPosList.Clear();
				this.m_cameraPosList = null;
				foreach (TextObj current3 in this.m_nameList)
				{
					current3.Dispose();
				}
				this.m_nameList.Clear();
				this.m_nameList = null;
				foreach (TextObj current4 in this.m_slainCountText)
				{
					current4.Dispose();
				}
				this.m_slainCountText.Clear();
				this.m_slainCountText = null;
				this.m_endingMask.Dispose();
				this.m_endingMask = null;
				this.m_continueText.Dispose();
				this.m_continueText = null;
				this.m_background.Dispose();
				this.m_background = null;
				base.Dispose();
			}
		}
	}
}
