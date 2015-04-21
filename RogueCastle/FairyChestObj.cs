using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class FairyChestObj : ChestObj
	{
		private const float SparkleDelay = 1f;
		private SpriteObj m_lockSprite;
		private SpriteObj m_errorSprite;
		private int m_conditionType;
		private PlayerObj m_player;
		private float m_sparkleCounter = 1f;
		private TextObj m_timerText;
		public int State
		{
			get;
			internal set;
		}
		public float Timer
		{
			get;
			set;
		}
		public int ConditionType
		{
			get
			{
				return this.m_conditionType;
			}
		}
		public FairyChestObj(PhysicsManager physicsManager) : base(physicsManager)
		{
			this.m_lockSprite = new SpriteObj("Chest4Unlock_Sprite");
			this.m_errorSprite = new SpriteObj("CancelIcon_Sprite");
			this.m_errorSprite.Visible = false;
			this.m_timerText = new TextObj(Game.JunicodeFont);
			this.m_timerText.FontSize = 18f;
			this.m_timerText.DropShadow = new Vector2(2f, 2f);
			this.m_timerText.Align = Types.TextAlign.Centre;
			this.m_player = Game.ScreenManager.Player;
		}
		public void SetConditionType(int conditionType = 0)
		{
			if (conditionType != 0)
			{
				this.m_conditionType = conditionType;
			}
			else
			{
				int.TryParse(this.Tag, out this.m_conditionType);
			}
			if (this.m_conditionType == 8)
			{
				this.Timer = 5f;
			}
		}
		public void SetChestUnlocked()
		{
			if (this.ConditionType != 10 && this.ConditionType != 0)
			{
				this.m_player.AttachedLevel.ObjectiveComplete();
			}
			this.State = 1;
			this.m_lockSprite.PlayAnimation(false);
			Tween.By(this.m_lockSprite, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"Y",
				"40"
			});
			Tween.To(this.m_lockSprite, 0.2f, new Easing(Linear.EaseNone), new string[]
			{
				"delay",
				"0.1",
				"Opacity",
				"0"
			});
		}
		public void SetChestFailed(bool skipTween = false)
		{
			if (!skipTween)
			{
				this.m_player.AttachedLevel.ObjectiveFailed();
			}
			this.State = 2;
			this.m_errorSprite.Visible = true;
			this.m_errorSprite.Opacity = 0f;
			this.m_errorSprite.Scale = Vector2.One;
			this.m_errorSprite.Position = new Vector2(base.X, base.Y - (float)(this.Height / 2));
			if (!skipTween)
			{
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyChest_Fail");
				Tween.To(this.m_errorSprite, 0.5f, new Easing(Quad.EaseIn), new string[]
				{
					"ScaleX",
					"0.5",
					"ScaleY",
					"0.5",
					"Opacity",
					"1"
				});
				return;
			}
			this.m_errorSprite.Scale = new Vector2(0.5f, 0.5f);
			this.m_errorSprite.Opacity = 1f;
		}
		public override void OpenChest(ItemDropManager itemDropManager, PlayerObj player)
		{
			if (this.State == 1 && !base.IsOpen && !base.IsLocked)
			{
				base.GoToFrame(2);
				SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");
				if (Game.PlayerStats.TotalRunesFound >= 55)
				{
					base.GiveStatDrop(itemDropManager, this.m_player, 1, 0);
					player.AttachedLevel.RefreshMapChestIcons();
					return;
				}
				List<byte[]> getRuneArray = Game.PlayerStats.GetRuneArray;
				List<Vector2> list = new List<Vector2>();
				int num = 0;
				foreach (byte[] current in getRuneArray)
				{
					int num2 = 0;
					byte[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] == 0)
						{
							list.Add(new Vector2((float)num, (float)num2));
						}
						num2++;
					}
					num++;
				}
				if (list.Count > 0)
				{
					Vector2 vector = list[CDGMath.RandomInt(0, list.Count - 1)];
					Game.PlayerStats.GetRuneArray[(int)vector.X][(int)vector.Y] = 1;
					List<object> list2 = new List<object>();
					list2.Add(new Vector2(base.X, base.Y - (float)this.Height / 2f));
					list2.Add(2);
					list2.Add(new Vector2(vector.X, vector.Y));
					(player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(12, true, list2);
					player.RunGetItemAnimation();
					Console.WriteLine(string.Concat(new object[]
					{
						"Unlocked item index ",
						vector.X,
						" of type ",
						vector.Y
					}));
				}
				else
				{
					base.GiveGold(itemDropManager, 0);
				}
				player.AttachedLevel.RefreshMapChestIcons();
			}
		}
		public override void Draw(Camera2D camera)
		{
			if (this.State == 0)
			{
				ChestConditionChecker.SetConditionState(this, this.m_player);
			}
			if (!base.IsOpen)
			{
				if (Game.ScreenManager.CurrentScreen is ProceduralLevelScreen && this.m_sparkleCounter > 0f)
				{
					this.m_sparkleCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
					if (this.m_sparkleCounter <= 0f)
					{
						this.m_sparkleCounter = 1f;
						float num = 0f;
						for (int i = 0; i < 2; i++)
						{
							Tween.To(this, num, new Easing(Linear.EaseNone), new string[0]);
							Tween.AddEndHandlerToLastTween(this.m_player.AttachedLevel.ImpactEffectPool, "DisplayChestSparkleEffect", new object[]
							{
								new Vector2(base.X, base.Y - (float)(this.Height / 2))
							});
							num += 0.5f;
						}
					}
				}
				if (this.ConditionType == 8 && this.State == 0)
				{
					if (!this.m_player.AttachedLevel.IsPaused)
					{
						this.Timer -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
					}
					this.m_timerText.Position = new Vector2(base.Position.X, base.Y - 50f);
					this.m_timerText.Text = ((int)this.Timer + 1).ToString();
					this.m_timerText.Draw(camera);
					this.m_player.AttachedLevel.UpdateObjectiveProgress((DialogueManager.GetText("Chest_Locked " + this.ConditionType).Dialogue[0] + (int)(this.Timer + 1f)).ToString());
				}
			}
			if (this.ConditionType != 10 || base.IsOpen)
			{
				base.Draw(camera);
				this.m_lockSprite.Flip = this.Flip;
				if (this.Flip == SpriteEffects.None)
				{
					this.m_lockSprite.Position = new Vector2(base.X - 10f, base.Y - (float)(this.Height / 2));
				}
				else
				{
					this.m_lockSprite.Position = new Vector2(base.X + 10f, base.Y - (float)(this.Height / 2));
				}
				this.m_lockSprite.Draw(camera);
				this.m_errorSprite.Position = new Vector2(base.X, base.Y - (float)(this.Height / 2));
				this.m_errorSprite.Draw(camera);
			}
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (this.State == 1)
			{
				base.CollisionResponse(thisBox, otherBox, collisionResponseType);
			}
		}
		public override void ForceOpen()
		{
			this.State = 1;
			this.m_errorSprite.Visible = false;
			this.m_lockSprite.Visible = false;
			base.ForceOpen();
		}
		public override void ResetChest()
		{
			this.State = 0;
			this.m_errorSprite.Visible = false;
			this.m_lockSprite.Visible = true;
			this.m_lockSprite.Opacity = 1f;
			base.Opacity = 1f;
			this.m_lockSprite.PlayAnimation(1, 1, false);
			base.TextureColor = Color.White;
			if (this.ConditionType == 8)
			{
				this.Timer = 5f;
			}
			base.ResetChest();
		}
		protected override GameObj CreateCloneInstance()
		{
			return new FairyChestObj(base.PhysicsMngr);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			FairyChestObj fairyChestObj = obj as FairyChestObj;
			fairyChestObj.State = this.State;
			this.SetConditionType(0);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_player = null;
				this.m_lockSprite.Dispose();
				this.m_lockSprite = null;
				this.m_errorSprite.Dispose();
				this.m_errorSprite = null;
				this.m_timerText.Dispose();
				this.m_timerText = null;
				base.Dispose();
			}
		}
	}
}
