using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	internal class DiaryFlashbackScreen : Screen
	{
		private BackgroundObj m_background;
		private List<LineageObj> m_lineageArray;
		private Vector2 m_storedCameraPos;
		private RenderTarget2D m_sepiaRT;
		private SpriteObj m_filmGrain;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public DiaryFlashbackScreen()
		{
			this.m_lineageArray = new List<LineageObj>();
		}
		public override void LoadContent()
		{
			this.m_filmGrain = new SpriteObj("FilmGrain_Sprite");
			this.m_filmGrain.ForceDraw = true;
			this.m_filmGrain.Scale = new Vector2(2.015f, 2.05f);
			this.m_filmGrain.X -= 5f;
			this.m_filmGrain.Y -= 5f;
			this.m_filmGrain.PlayAnimation(true);
			this.m_filmGrain.AnimationDelay = 0.0333333351f;
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			this.m_sepiaRT = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
			if (this.m_background != null)
			{
				this.m_background.Dispose();
			}
			this.m_background = new BackgroundObj("LineageScreenBG_Sprite");
			this.m_background.SetRepeated(true, true, base.Camera, null);
			this.m_background.X -= 6600f;
			base.ReinitializeRTs();
		}
		public override void OnEnter()
		{
			GameUtil.UnlockAchievement("LOVE_OF_BOOKS");
			this.BackBufferOpacity = 0f;
			Tween.To(this, 0.05f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"1"
			});
			this.BackBufferOpacity = 1f;
			Tween.To(this, 1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.1",
				"BackBufferOpacity",
				"0"
			});
			this.BackBufferOpacity = 0f;
			this.m_storedCameraPos = base.Camera.Position;
			base.Camera.Position = Vector2.Zero;
			if (this.m_background == null)
			{
				this.m_sepiaRT = new RenderTarget2D(base.Camera.GraphicsDevice, 1320, 720);
				this.m_background = new BackgroundObj("LineageScreenBG_Sprite");
				this.m_background.SetRepeated(true, true, base.Camera, null);
				this.m_background.X -= 6600f;
			}
			this.CreateLineageObjDebug();
			base.Camera.X = this.m_lineageArray[this.m_lineageArray.Count - 1].X;
			SoundManager.PlaySound("Cutsc_Thunder");
			Tween.RunFunction(1f, this, "Cutscene1", new object[0]);
			base.OnEnter();
		}
		public void Cutscene1()
		{
			SoundManager.PlaySound("Cutsc_PictureMove");
			Tween.To(base.Camera, (float)this.m_lineageArray.Count * 0.2f, new Easing(Quad.EaseInOut), new string[]
			{
				"X",
				this.m_lineageArray[0].X.ToString()
			});
			Tween.AddEndHandlerToLastTween(this, "Cutscene2", new object[0]);
		}
		public void Cutscene2()
		{
			LineageObj lineageObj = this.m_lineageArray[0];
			lineageObj.ForceDraw = true;
			Tween.RunFunction(1f, lineageObj, "DropFrame", new object[0]);
			Tween.RunFunction(4.5f, this, "ExitTransition", new object[0]);
		}
		public void ExitTransition()
		{
			SoundManager.PlaySound("Cutsc_Picture_Break");
			Tween.To(this, 0.05f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"1"
			});
			Tween.RunFunction(0.1f, base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		public override void OnExit()
		{
			foreach (LineageObj current in this.m_lineageArray)
			{
				current.Dispose();
			}
			this.m_lineageArray.Clear();
			base.Camera.Position = this.m_storedCameraPos;
			base.OnExit();
		}
		private void CreateLineageObjs()
		{
			int num = 700;
			int num2 = 400;
			int num3 = 0;
			if (Game.PlayerStats.FamilyTreeArray.Count > 10)
			{
				for (int i = 0; i < 10; i++)
				{
					FamilyTreeNode familyTreeNode = Game.PlayerStats.FamilyTreeArray[i];
					LineageObj lineageObj = new LineageObj(null, true);
					lineageObj.IsDead = true;
					lineageObj.Age = familyTreeNode.Age;
					lineageObj.ChildAge = familyTreeNode.ChildAge;
					lineageObj.Class = familyTreeNode.Class;
					lineageObj.PlayerName = familyTreeNode.Name;
					lineageObj.IsFemale = familyTreeNode.IsFemale;
					lineageObj.SetPortrait(familyTreeNode.HeadPiece, familyTreeNode.ShoulderPiece, familyTreeNode.ChestPiece);
					lineageObj.NumEnemiesKilled = familyTreeNode.NumEnemiesBeaten;
					lineageObj.BeatenABoss = familyTreeNode.BeatenABoss;
					lineageObj.SetTraits(familyTreeNode.Traits);
					lineageObj.UpdateAge(num);
					lineageObj.UpdateData();
					lineageObj.UpdateClassRank();
					num += (int)lineageObj.Age;
					lineageObj.X = (float)num3;
					num3 += num2;
					this.m_lineageArray.Add(lineageObj);
				}
				return;
			}
			foreach (FamilyTreeNode current in Game.PlayerStats.FamilyTreeArray)
			{
				LineageObj lineageObj2 = new LineageObj(null, true);
				lineageObj2.IsDead = true;
				lineageObj2.Age = current.Age;
				lineageObj2.ChildAge = current.ChildAge;
				lineageObj2.Class = current.Class;
				lineageObj2.PlayerName = current.Name;
				lineageObj2.IsFemale = current.IsFemale;
				lineageObj2.SetPortrait(current.HeadPiece, current.ShoulderPiece, current.ChestPiece);
				lineageObj2.NumEnemiesKilled = current.NumEnemiesBeaten;
				lineageObj2.BeatenABoss = current.BeatenABoss;
				lineageObj2.SetTraits(current.Traits);
				lineageObj2.UpdateAge(num);
				lineageObj2.UpdateData();
				lineageObj2.UpdateClassRank();
				num += (int)lineageObj2.Age;
				lineageObj2.X = (float)num3;
				num3 += num2;
				this.m_lineageArray.Add(lineageObj2);
			}
		}
		private void CreateLineageObjDebug()
		{
			int num = 700;
			int num2 = 400;
			int num3 = 0;
			for (int i = 0; i < 10; i++)
			{
				FamilyTreeNode familyTreeNode;
				if (i > Game.PlayerStats.FamilyTreeArray.Count - 1)
				{
					familyTreeNode = new FamilyTreeNode
					{
						Age = (byte)CDGMath.RandomInt(15, 30),
						ChildAge = (byte)CDGMath.RandomInt(15, 30),
						Name = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)],
						HeadPiece = (byte)CDGMath.RandomInt(1, 5),
						ShoulderPiece = (byte)CDGMath.RandomInt(1, 5),
						ChestPiece = (byte)CDGMath.RandomInt(1, 5)
					};
				}
				else
				{
					familyTreeNode = Game.PlayerStats.FamilyTreeArray[i];
				}
				LineageObj lineageObj = new LineageObj(null, true);
				lineageObj.IsDead = true;
				lineageObj.Age = familyTreeNode.Age;
				lineageObj.ChildAge = familyTreeNode.ChildAge;
				lineageObj.Class = familyTreeNode.Class;
				lineageObj.PlayerName = familyTreeNode.Name;
				lineageObj.IsFemale = familyTreeNode.IsFemale;
				lineageObj.SetPortrait(familyTreeNode.HeadPiece, familyTreeNode.ShoulderPiece, familyTreeNode.ChestPiece);
				lineageObj.NumEnemiesKilled = familyTreeNode.NumEnemiesBeaten;
				lineageObj.BeatenABoss = familyTreeNode.BeatenABoss;
				lineageObj.SetTraits(familyTreeNode.Traits);
				lineageObj.UpdateAge(num);
				lineageObj.UpdateData();
				lineageObj.UpdateClassRank();
				num += (int)lineageObj.Age;
				lineageObj.X = (float)num3;
				num3 += num2;
				this.m_lineageArray.Add(lineageObj);
			}
		}
		public override void Draw(GameTime gametime)
		{
			if (base.Camera.X > this.m_background.X + 6600f)
			{
				this.m_background.X = base.Camera.X;
			}
			if (base.Camera.X < this.m_background.X)
			{
				this.m_background.X = base.Camera.X - 1320f;
			}
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, base.Camera.GetTransformation());
			this.m_background.Draw(base.Camera);
			foreach (LineageObj current in this.m_lineageArray)
			{
				current.Draw(base.Camera);
			}
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget(this.m_sepiaRT);
			Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
			Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Game.HSVEffect);
			base.Camera.Draw((base.ScreenManager as RCScreenManager).RenderTarget, Vector2.Zero, Color.White);
			base.Camera.End();
			base.Camera.GraphicsDevice.SetRenderTarget((base.ScreenManager as RCScreenManager).RenderTarget);
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
			Color color = new Color(180, 150, 80);
			base.Camera.Draw(this.m_sepiaRT, Vector2.Zero, color);
			this.m_filmGrain.Draw(base.Camera);
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.White * this.BackBufferOpacity);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Diary Flashback Screen");
				this.m_background.Dispose();
				this.m_background = null;
				foreach (LineageObj current in this.m_lineageArray)
				{
					current.Dispose();
				}
				this.m_lineageArray.Clear();
				this.m_lineageArray = null;
				this.m_filmGrain.Dispose();
				this.m_filmGrain = null;
				this.m_sepiaRT.Dispose();
				this.m_sepiaRT = null;
				base.Dispose();
			}
		}
	}
}
