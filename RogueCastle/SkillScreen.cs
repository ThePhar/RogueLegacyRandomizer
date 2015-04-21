using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class SkillScreen : Screen
	{
		private SpriteObj m_bg;
		private SpriteObj m_cloud1;
		private SpriteObj m_cloud2;
		private SpriteObj m_cloud3;
		private SpriteObj m_cloud4;
		private SpriteObj m_cloud5;
		private SpriteObj m_selectionIcon;
		private Vector2 m_selectedTraitIndex;
		private KeyIconTextObj m_continueText;
		private KeyIconTextObj m_toggleIconsText;
		private KeyIconTextObj m_confirmText;
		private KeyIconTextObj m_navigationText;
		private SpriteObj m_titleText;
		private ObjContainer m_dialoguePlate;
		private ObjContainer m_manor;
		private bool m_fadingIn;
		private bool m_cameraTweening;
		private bool m_lockControls;
		private ImpactEffectPool m_impactEffectPool;
		private GameObj m_shakeObj;
		private float m_shakeTimer;
		private int m_shakeAmount = 2;
		private float m_shakeDelay = 0.01f;
		private bool m_shookLeft;
		private float m_shakeDuration;
		private TextObj m_playerMoney;
		private SpriteObj m_coinIcon;
		private SpriteObj m_skillIcon;
		private TextObj m_skillTitle;
		private TextObj m_skillDescription;
		private KeyIconTextObj m_inputDescription;
		private SpriteObj m_descriptionDivider;
		private TextObj m_skillCurrent;
		private TextObj m_skillUpgrade;
		private TextObj m_skillLevel;
		private TextObj m_skillCost;
		private SpriteObj m_skillCostBG;
		private bool m_horizontalShake;
		private bool m_verticalShake;
		private bool m_shakeScreen;
		private float m_screenShakeMagnitude;
		public SkillScreen()
		{
			this.m_selectedTraitIndex = new Vector2(5f, 9f);
			this.m_impactEffectPool = new ImpactEffectPool(1000);
			base.DrawIfCovered = true;
		}
		public override void LoadContent()
		{
			this.m_impactEffectPool.Initialize();
			this.m_manor = new ObjContainer("TraitsCastle_Character");
			this.m_manor.Scale = new Vector2(2f, 2f);
			this.m_manor.ForceDraw = true;
			for (int i = 0; i < this.m_manor.NumChildren; i++)
			{
				this.m_manor.GetChildAt(i).Visible = false;
				this.m_manor.GetChildAt(i).Opacity = 0f;
			}
			this.m_dialoguePlate = new ObjContainer("TraitsScreenPlate_Container");
			this.m_dialoguePlate.ForceDraw = true;
			this.m_dialoguePlate.Position = new Vector2((float)(1320 - this.m_dialoguePlate.Width / 2), 360f);
			this.m_skillIcon = new SpriteObj("Icon_Health_Up_Sprite");
			this.m_skillIcon.Position = new Vector2(-110f, -200f);
			this.m_dialoguePlate.AddChild(this.m_skillIcon);
			this.m_skillTitle = new TextObj(Game.JunicodeFont);
			this.m_skillTitle.Text = "Skill name";
			this.m_skillTitle.DropShadow = new Vector2(2f, 2f);
			this.m_skillTitle.TextureColor = new Color(236, 197, 132);
			this.m_skillTitle.Position = new Vector2((float)(this.m_skillIcon.Bounds.Right + 15), this.m_skillIcon.Y);
			this.m_skillTitle.FontSize = 12f;
			this.m_dialoguePlate.AddChild(this.m_skillTitle);
			this.m_skillDescription = new TextObj(Game.JunicodeFont);
			this.m_skillDescription.Text = "Description text goes here.  Let's see how well the word wrap function works.";
			this.m_skillDescription.Position = new Vector2(this.m_dialoguePlate.GetChildAt(1).X - 30f, (float)(this.m_dialoguePlate.GetChildAt(1).Bounds.Bottom + 20));
			this.m_skillDescription.FontSize = 10f;
			this.m_skillDescription.DropShadow = new Vector2(2f, 2f);
			this.m_skillDescription.TextureColor = new Color(228, 218, 208);
			this.m_skillDescription.WordWrap(this.m_dialoguePlate.Width - 50);
			this.m_dialoguePlate.AddChild(this.m_skillDescription);
			this.m_inputDescription = new KeyIconTextObj(Game.JunicodeFont);
			this.m_inputDescription.Text = "Input descriptions go here..";
			this.m_inputDescription.Position = new Vector2(this.m_skillIcon.X - 30f, (float)(this.m_skillDescription.Bounds.Bottom + 20));
			this.m_inputDescription.FontSize = 10f;
			this.m_inputDescription.DropShadow = new Vector2(2f, 2f);
			this.m_inputDescription.TextureColor = new Color(228, 218, 208);
			this.m_inputDescription.WordWrap(this.m_dialoguePlate.Width - 50);
			this.m_dialoguePlate.AddChild(this.m_inputDescription);
			this.m_descriptionDivider = new SpriteObj("Blank_Sprite");
			this.m_descriptionDivider.ScaleX = 250f / (float)this.m_descriptionDivider.Width;
			this.m_descriptionDivider.ScaleY = 0.25f;
			this.m_descriptionDivider.ForceDraw = true;
			this.m_descriptionDivider.DropShadow = new Vector2(2f, 2f);
			this.m_skillCurrent = new TextObj(Game.JunicodeFont);
			this.m_skillCurrent.Position = new Vector2(this.m_inputDescription.X, (float)(this.m_inputDescription.Bounds.Bottom + 10));
			this.m_skillCurrent.FontSize = 10f;
			this.m_skillCurrent.DropShadow = new Vector2(2f, 2f);
			this.m_skillCurrent.TextureColor = new Color(228, 218, 208);
			this.m_skillCurrent.WordWrap(this.m_dialoguePlate.Width - 50);
			this.m_dialoguePlate.AddChild(this.m_skillCurrent);
			this.m_skillUpgrade = (this.m_skillCurrent.Clone() as TextObj);
			this.m_skillUpgrade.Y += 15f;
			this.m_dialoguePlate.AddChild(this.m_skillUpgrade);
			this.m_skillLevel = (this.m_skillUpgrade.Clone() as TextObj);
			this.m_skillLevel.Y += 15f;
			this.m_dialoguePlate.AddChild(this.m_skillLevel);
			this.m_skillCost = new TextObj(Game.JunicodeFont);
			this.m_skillCost.X = this.m_skillIcon.X;
			this.m_skillCost.Y = 182f;
			this.m_skillCost.FontSize = 10f;
			this.m_skillCost.DropShadow = new Vector2(2f, 2f);
			this.m_skillCost.TextureColor = Color.Yellow;
			this.m_dialoguePlate.AddChild(this.m_skillCost);
			this.m_skillCostBG = new SpriteObj("SkillTreeGoldIcon_Sprite");
			this.m_skillCostBG.Position = new Vector2(-180f, 180f);
			this.m_dialoguePlate.AddChild(this.m_skillCostBG);
			this.m_dialoguePlate.ForceDraw = true;
			this.m_bg = new SpriteObj("TraitsBG_Sprite");
			this.m_bg.Scale = new Vector2(1320f / (float)this.m_bg.Width, 1320f / (float)this.m_bg.Width);
			this.m_bg.ForceDraw = true;
			this.m_cloud1 = new SpriteObj("TraitsCloud1_Sprite")
			{
				ForceDraw = true
			};
			this.m_cloud2 = new SpriteObj("TraitsCloud2_Sprite")
			{
				ForceDraw = true
			};
			this.m_cloud3 = new SpriteObj("TraitsCloud3_Sprite")
			{
				ForceDraw = true
			};
			this.m_cloud4 = new SpriteObj("TraitsCloud4_Sprite")
			{
				ForceDraw = true
			};
			this.m_cloud5 = new SpriteObj("TraitsCloud5_Sprite")
			{
				ForceDraw = true
			};
			float opacity = 1f;
			this.m_cloud1.Opacity = opacity;
			this.m_cloud2.Opacity = opacity;
			this.m_cloud3.Opacity = opacity;
			this.m_cloud4.Opacity = opacity;
			this.m_cloud5.Opacity = opacity;
			this.m_cloud1.Position = new Vector2((float)CDGMath.RandomInt(0, 1520), (float)CDGMath.RandomInt(0, 360));
			this.m_cloud2.Position = new Vector2((float)CDGMath.RandomInt(0, 1520), (float)CDGMath.RandomInt(0, 360));
			this.m_cloud3.Position = new Vector2((float)CDGMath.RandomInt(0, 1520), (float)CDGMath.RandomInt(0, 360));
			this.m_cloud4.Position = new Vector2((float)CDGMath.RandomInt(0, 1520), (float)CDGMath.RandomInt(0, 360));
			this.m_cloud5.Position = new Vector2((float)CDGMath.RandomInt(0, 1520), (float)CDGMath.RandomInt(0, 360));
			this.m_selectionIcon = new SpriteObj("IconHalo_Sprite");
			this.m_selectionIcon.ForceDraw = true;
			this.m_selectionIcon.AnimationDelay = 0.1f;
			this.m_selectionIcon.PlayAnimation(true);
			this.m_selectionIcon.Scale = new Vector2(1.1f, 1.1f);
			this.m_titleText = new SpriteObj("ManorTitleText_Sprite");
			this.m_titleText.X = (float)this.m_titleText.Width / 2f + 20f;
			this.m_titleText.Y = 64.8f;
			this.m_titleText.ForceDraw = true;
			this.m_continueText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_continueText.ForceDraw = true;
			this.m_continueText.FontSize = 12f;
			this.m_continueText.DropShadow = new Vector2(2f, 2f);
			this.m_continueText.Position = new Vector2(1300f, 630f);
			this.m_continueText.Align = Types.TextAlign.Right;
			this.m_toggleIconsText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_toggleIconsText.ForceDraw = true;
			this.m_toggleIconsText.FontSize = 12f;
			this.m_toggleIconsText.DropShadow = new Vector2(2f, 2f);
			this.m_toggleIconsText.Position = new Vector2(this.m_continueText.X, this.m_continueText.Y + 40f);
			this.m_toggleIconsText.Align = Types.TextAlign.Right;
			this.m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_confirmText.Align = Types.TextAlign.Right;
			this.m_confirmText.FontSize = 12f;
			this.m_confirmText.DropShadow = new Vector2(2f, 2f);
			this.m_confirmText.Position = new Vector2(1300f, 10f);
			this.m_confirmText.ForceDraw = true;
			this.m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_navigationText.Align = Types.TextAlign.Right;
			this.m_navigationText.FontSize = 12f;
			this.m_navigationText.DropShadow = new Vector2(2f, 2f);
			this.m_navigationText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 40f);
			this.m_navigationText.ForceDraw = true;
			this.m_coinIcon = new SpriteObj("CoinIcon_Sprite");
			this.m_coinIcon.Position = new Vector2(1100f, 585f);
			this.m_coinIcon.Scale = new Vector2(0.9f, 0.9f);
			this.m_coinIcon.ForceDraw = true;
			this.m_playerMoney = new TextObj(Game.GoldFont);
			this.m_playerMoney.Align = Types.TextAlign.Left;
			this.m_playerMoney.Text = "1000";
			this.m_playerMoney.FontSize = 30f;
			this.m_playerMoney.Position = new Vector2(this.m_coinIcon.X + 35f, this.m_coinIcon.Y);
			this.m_playerMoney.ForceDraw = true;
			base.LoadContent();
		}
		public override void OnEnter()
		{
			bool flag = true;
			foreach (SkillObj current in SkillSystem.SkillArray)
			{
				if (current.CurrentLevel < 1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				GameUtil.UnlockAchievement("FEAR_OF_DECISIONS");
			}
			if (Game.PlayerStats.CurrentLevel >= 50)
			{
				GameUtil.UnlockAchievement("FEAR_OF_WEALTH");
			}
			this.m_lockControls = false;
			this.m_manor.GetChildAt(23).Visible = true;
			this.m_manor.GetChildAt(23).Opacity = 1f;
			base.Camera.Position = new Vector2(660f, 360f);
			SkillObj[] skillArray = SkillSystem.GetSkillArray();
			for (int i = 0; i < skillArray.Length; i++)
			{
				if (skillArray[i].CurrentLevel > 0)
				{
					this.SetVisible(skillArray[i], false);
				}
			}
			if (!SoundManager.IsMusicPlaying)
			{
				SoundManager.PlayMusic("SkillTreeSong", true, 1f);
			}
			SkillObj skill = SkillSystem.GetSkill((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y);
			this.m_selectionIcon.Position = SkillSystem.GetSkillPosition(skill);
			this.UpdateDescriptionPlate(skill);
			this.m_dialoguePlate.Visible = true;
			this.m_confirmText.Text = "[Input:" + 0 + "] to purchase/upgrade skill";
			this.m_toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons off";
			this.m_continueText.Text = "[Input:" + 2 + "] to exit the manor";
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_navigationText.Text = "[Button:LeftStick] to navigate skills";
			}
			else
			{
				this.m_navigationText.Text = "Arrow keys to navigate skills";
			}
			SkillSystem.UpdateAllTraitSprites();
			base.OnEnter();
		}
		public override void OnExit()
		{
			Game.ScreenManager.Player.AttachedLevel.UpdatePlayerSpellIcon();
			SoundManager.StopMusic(0.5f);
			(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.UpgradeData,
				SaveType.PlayerData
			});
			base.OnExit();
		}
		public void SetVisible(SkillObj trait, bool fadeIn)
		{
			int manorPiece = SkillSystem.GetManorPiece(trait);
			if (fadeIn)
			{
				this.SetManorPieceVisible(manorPiece, trait);
				return;
			}
			GameObj childAt = this.m_manor.GetChildAt(manorPiece);
			childAt.Opacity = 1f;
			childAt.Visible = true;
			foreach (SkillObj current in SkillSystem.GetAllConnectingTraits(trait))
			{
				if (!current.Visible)
				{
					current.Visible = true;
					current.Opacity = 1f;
				}
			}
			if (this.m_manor.GetChildAt(7).Visible && this.m_manor.GetChildAt(16).Visible)
			{
				(this.m_manor.GetChildAt(7) as SpriteObj).GoToFrame(2);
			}
			if (this.m_manor.GetChildAt(6).Visible && this.m_manor.GetChildAt(16).Visible)
			{
				(this.m_manor.GetChildAt(6) as SpriteObj).GoToFrame(2);
			}
			if (this.m_manor.GetChildAt(2).Visible)
			{
				SpriteObj spriteObj = this.m_manor.GetChildAt(32) as SpriteObj;
				spriteObj.Visible = true;
				spriteObj.Opacity = 1f;
				spriteObj.PlayAnimation(true);
				spriteObj.OverrideParentAnimationDelay = true;
				spriteObj.AnimationDelay = 0.0333333351f;
				spriteObj.Visible = true;
			}
		}
		public void SetManorPieceVisible(int manorIndex, SkillObj skillObj)
		{
			GameObj childAt = this.m_manor.GetChildAt(manorIndex);
			float num = 0f;
			if (!childAt.Visible)
			{
				this.m_lockControls = true;
				childAt.Visible = true;
				Vector2 pos = new Vector2(childAt.AbsPosition.X, (float)childAt.AbsBounds.Bottom);
				switch (manorIndex)
				{
				case 0:
				case 11:
				case 17:
				case 22:
				case 24:
				case 27:
				case 28:
					num = 0.5f;
					childAt.Opacity = 0f;
					Tween.To(childAt, num, new Easing(Tween.EaseNone), new string[]
					{
						"Opacity",
						"1"
					});
					goto IL_A26;
				case 1:
				case 5:
					childAt.Opacity = 1f;
					num = 1f;
					childAt.X -= (float)(childAt.Width * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_short_01",
						"skill_tree_reveal_short_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"X",
						(childAt.Width * 2).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, false, (float)(childAt.Height * 2), num);
					goto IL_A26;
				case 2:
				{
					childAt.Opacity = 1f;
					num = 1.5f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_short_01",
						"skill_tree_reveal_short_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2), num);
					SpriteObj spriteObj = this.m_manor.GetChildAt(32) as SpriteObj;
					spriteObj.PlayAnimation(true);
					spriteObj.OverrideParentAnimationDelay = true;
					spriteObj.AnimationDelay = 0.0333333351f;
					spriteObj.Visible = true;
					spriteObj.Opacity = 0f;
					Tween.To(spriteObj, 0.5f, new Easing(Tween.EaseNone), new string[]
					{
						"delay",
						num.ToString(),
						"Opacity",
						"1"
					});
					goto IL_A26;
				}
				case 3:
				case 6:
				case 9:
				case 13:
				case 15:
				case 20:
				case 25:
					childAt.Opacity = 1f;
					num = 1f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_short_01",
						"skill_tree_reveal_short_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2), num);
					goto IL_A26;
				case 4:
					pos.Y -= 50f;
					childAt.Opacity = 1f;
					num = 3f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_01",
						"skill_tree_reveal_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2) * 0.25f, num);
					goto IL_A26;
				case 7:
					pos.X = (float)childAt.AbsBounds.Right - (float)(childAt.Width * 2) * 0.25f;
					childAt.Opacity = 1f;
					num = 3f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_01",
						"skill_tree_reveal_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2) * 0.25f, num);
					goto IL_A26;
				case 8:
					pos.X = (float)childAt.AbsBounds.Right - (float)(childAt.Width * 2) * 0.25f;
					childAt.Opacity = 1f;
					num = 3f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_01",
						"skill_tree_reveal_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2) * 0.25f, num);
					goto IL_A26;
				case 10:
				case 21:
					childAt.Opacity = 1f;
					num = 3f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_01",
						"skill_tree_reveal_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2), num);
					goto IL_A26;
				case 12:
				case 14:
					childAt.Opacity = 1f;
					num = 1f;
					childAt.X += (float)(childAt.Width * 2);
					pos.X = childAt.AbsPosition.X - 60f;
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_short_01",
						"skill_tree_reveal_short_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"X",
						(-(childAt.Width * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, false, (float)(childAt.Height * 2), num);
					goto IL_A26;
				case 16:
					childAt.Opacity = 1f;
					num = 3f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_01",
						"skill_tree_reveal_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2) * 0.5f, num);
					goto IL_A26;
				case 18:
				case 19:
					childAt.Opacity = 1f;
					num = 3f;
					childAt.Y += (float)(childAt.Height * 2);
					SoundManager.PlaySound(new string[]
					{
						"skill_tree_reveal_01",
						"skill_tree_reveal_02"
					});
					Tween.By(childAt, num, new Easing(Quad.EaseOut), new string[]
					{
						"Y",
						(-(childAt.Height * 2)).ToString()
					});
					this.m_impactEffectPool.SkillTreeDustDuration(pos, true, (float)(childAt.Width * 2) * 0.2f, num);
					goto IL_A26;
				case 23:
					goto IL_A26;
				case 29:
				case 30:
				case 31:
					Tween.RunFunction(0.25f, typeof(SoundManager), "PlaySound", new object[]
					{
						"skill_tree_reveal_bounce"
					});
					childAt.Opacity = 1f;
					childAt.Scale = Vector2.Zero;
					num = 1f;
					Tween.To(childAt, num, new Easing(Bounce.EaseOut), new string[]
					{
						"ScaleX",
						"1",
						"ScaleY",
						"1"
					});
					goto IL_A26;
				}
				num = 0.7f;
				Vector2 vector = new Vector2(childAt.AbsPosition.X, (float)childAt.AbsBounds.Bottom);
				childAt.Opacity = 1f;
				childAt.Y -= 720f;
				Tween.By(childAt, num, new Easing(Quad.EaseIn), new string[]
				{
					"Y",
					"720"
				});
				Tween.AddEndHandlerToLastTween(this.m_impactEffectPool, "SkillTreeDustEffect", new object[]
				{
					vector,
					true,
					childAt.Width * 2
				});
				Tween.RunFunction(num, this, "ShakeScreen", new object[]
				{
					5,
					true,
					true
				});
				Tween.RunFunction(num + 0.2f, this, "StopScreenShake", new object[0]);
			}
			IL_A26:
			Tween.RunFunction(num, this, "SetSkillIconVisible", new object[]
			{
				skillObj
			});
			if (this.m_manor.GetChildAt(7).Visible && this.m_manor.GetChildAt(16).Visible)
			{
				(this.m_manor.GetChildAt(7) as SpriteObj).GoToFrame(2);
			}
			if (this.m_manor.GetChildAt(6).Visible && this.m_manor.GetChildAt(16).Visible)
			{
				(this.m_manor.GetChildAt(6) as SpriteObj).GoToFrame(2);
			}
		}
		public void SetSkillIconVisible(SkillObj skill)
		{
			float num = 0f;
			foreach (SkillObj current in SkillSystem.GetAllConnectingTraits(skill))
			{
				if (!current.Visible)
				{
					current.Visible = true;
					current.Opacity = 0f;
					Tween.To(current, 0.2f, new Easing(Linear.EaseNone), new string[]
					{
						"Opacity",
						"1"
					});
					num += 0.2f;
				}
			}
			Tween.RunFunction(num, this, "UnlockControls", new object[0]);
			Tween.RunFunction(num, this, "CheckForSkillUnlock", new object[]
			{
				skill,
				true
			});
		}
		public void CheckForSkillUnlock(SkillObj skill, bool displayScreen)
		{
			byte b = 0;
			switch (skill.TraitType)
			{
			case SkillType.Smithy:
				b = 1;
				break;
			case SkillType.Enchanter:
				b = 2;
				break;
			case SkillType.Architect:
				b = 3;
				break;
			case SkillType.Lich_Unlock:
				b = 7;
				break;
			case SkillType.Banker_Unlock:
				b = 5;
				break;
			case SkillType.Spellsword_Unlock:
				b = 6;
				break;
			case SkillType.Ninja_Unlock:
				b = 4;
				break;
			case SkillType.Knight_Up:
				b = 8;
				if (Game.PlayerStats.Class == 0)
				{
					Game.PlayerStats.Class = 8;
				}
				break;
			case SkillType.Mage_Up:
				b = 9;
				if (Game.PlayerStats.Class == 1)
				{
					Game.PlayerStats.Class = 9;
				}
				break;
			case SkillType.Assassin_Up:
				b = 12;
				if (Game.PlayerStats.Class == 3)
				{
					Game.PlayerStats.Class = 11;
				}
				break;
			case SkillType.Banker_Up:
				b = 13;
				if (Game.PlayerStats.Class == 5)
				{
					Game.PlayerStats.Class = 13;
				}
				break;
			case SkillType.Barbarian_Up:
				b = 10;
				if (Game.PlayerStats.Class == 2)
				{
					Game.PlayerStats.Class = 10;
				}
				break;
			case SkillType.Lich_Up:
				b = 15;
				if (Game.PlayerStats.Class == 7)
				{
					Game.PlayerStats.Class = 15;
				}
				break;
			case SkillType.Ninja_Up:
				b = 11;
				if (Game.PlayerStats.Class == 4)
				{
					Game.PlayerStats.Class = 12;
				}
				break;
			case SkillType.SpellSword_Up:
				b = 14;
				if (Game.PlayerStats.Class == 6)
				{
					Game.PlayerStats.Class = 14;
				}
				break;
			case SkillType.SuperSecret:
				b = 16;
				break;
			}
			if (b != 0 && displayScreen)
			{
				List<object> list = new List<object>();
				list.Add(b);
				(base.ScreenManager as RCScreenManager).DisplayScreen(19, true, list);
			}
		}
		public void UnlockControls()
		{
			this.m_lockControls = false;
		}
		public void StartShake(GameObj obj, float shakeDuration)
		{
			this.m_shakeDuration = shakeDuration;
			this.m_shakeObj = obj;
			this.m_shakeTimer = this.m_shakeDelay;
			this.m_shookLeft = false;
		}
		public void EndShake()
		{
			if (this.m_shookLeft)
			{
				this.m_shakeObj.X += (float)this.m_shakeAmount;
			}
			this.m_shakeObj = null;
			this.m_shakeTimer = 0f;
		}
		public void FadingComplete()
		{
			this.m_fadingIn = false;
		}
		public override void Update(GameTime gameTime)
		{
			if (!this.m_cameraTweening && this.m_selectedTraitIndex != new Vector2(7f, 1f) && base.Camera.Y != 360f)
			{
				this.m_cameraTweening = true;
				Tween.To(base.Camera, 0.5f, new Easing(Quad.EaseOut), new string[]
				{
					"Y",
					360f.ToString()
				});
				Tween.AddEndHandlerToLastTween(this, "EndCameraTween", new object[0]);
			}
			float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (this.m_cloud1.Bounds.Right < -100)
			{
				this.m_cloud1.Position = new Vector2((float)CDGMath.RandomInt(1420, 1520), (float)CDGMath.RandomInt(0, 360));
			}
			if (this.m_cloud2.Bounds.Right < -100)
			{
				this.m_cloud2.Position = new Vector2((float)CDGMath.RandomInt(1420, 1520), (float)CDGMath.RandomInt(0, 360));
			}
			if (this.m_cloud3.Bounds.Right < -100)
			{
				this.m_cloud3.Position = new Vector2((float)CDGMath.RandomInt(1420, 1520), (float)CDGMath.RandomInt(0, 360));
			}
			if (this.m_cloud4.Bounds.Right < -100)
			{
				this.m_cloud4.Position = new Vector2((float)CDGMath.RandomInt(1420, 1520), (float)CDGMath.RandomInt(0, 360));
			}
			if (this.m_cloud5.Bounds.Right < -100)
			{
				this.m_cloud5.Position = new Vector2((float)CDGMath.RandomInt(1420, 1520), (float)CDGMath.RandomInt(0, 360));
			}
			this.m_cloud1.X -= 20f * num;
			this.m_cloud2.X -= 16f * num;
			this.m_cloud3.X -= 15f * num;
			this.m_cloud4.X -= 5f * num;
			this.m_cloud5.X -= 10f * num;
			if (this.m_shakeDuration > 0f)
			{
				this.m_shakeDuration -= num;
				if (this.m_shakeTimer > 0f && this.m_shakeObj != null)
				{
					this.m_shakeTimer -= num;
					if (this.m_shakeTimer <= 0f)
					{
						this.m_shakeTimer = this.m_shakeDelay;
						if (this.m_shookLeft)
						{
							this.m_shookLeft = false;
							this.m_shakeObj.X += (float)this.m_shakeAmount;
						}
						else
						{
							this.m_shakeObj.X -= (float)this.m_shakeAmount;
							this.m_shookLeft = true;
						}
					}
				}
			}
			if (this.m_shakeScreen)
			{
				this.UpdateShake();
			}
			base.Update(gameTime);
		}
		public override void HandleInput()
		{
			if (!this.m_cameraTweening && !this.m_lockControls)
			{
				bool flag = false;
				if (Game.GlobalInput.JustPressed(9))
				{
					if (SkillSystem.IconsVisible)
					{
						this.m_toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons on";
						this.m_confirmText.Visible = false;
						this.m_continueText.Visible = false;
						this.m_navigationText.Visible = false;
						SkillSystem.HideAllIcons();
						this.m_selectionIcon.Opacity = 0f;
						this.m_dialoguePlate.Opacity = 0f;
						this.m_descriptionDivider.Opacity = 0f;
						this.m_coinIcon.Opacity = 0f;
						this.m_playerMoney.Opacity = 0f;
						this.m_titleText.Opacity = 0f;
					}
					else
					{
						this.m_toggleIconsText.Text = "[Input:" + 9 + "] to toggle icons off";
						this.m_confirmText.Visible = true;
						this.m_continueText.Visible = true;
						this.m_navigationText.Visible = true;
						SkillSystem.ShowAllIcons();
						this.m_selectionIcon.Opacity = 1f;
						this.m_dialoguePlate.Opacity = 1f;
						this.m_descriptionDivider.Opacity = 1f;
						this.m_coinIcon.Opacity = 1f;
						this.m_playerMoney.Opacity = 1f;
						this.m_titleText.Opacity = 1f;
					}
					flag = true;
				}
				if (SkillSystem.IconsVisible)
				{
					Vector2 selectedTraitIndex = this.m_selectedTraitIndex;
					Vector2 vector = new Vector2(-1f, -1f);
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						vector = SkillSystem.GetSkillLink((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y).TopLink;
						SkillObj skill = SkillSystem.GetSkill(SkillType.SuperSecret);
						if (!this.m_cameraTweening && skill.Visible && vector == new Vector2(7f, 1f))
						{
							this.m_cameraTweening = true;
							Tween.To(base.Camera, 0.5f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								60f.ToString()
							});
							Tween.AddEndHandlerToLastTween(this, "EndCameraTween", new object[0]);
						}
					}
					else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
					{
						vector = SkillSystem.GetSkillLink((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y).BottomLink;
					}
					if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
					{
						vector = SkillSystem.GetSkillLink((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y).LeftLink;
					}
					else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
					{
						vector = SkillSystem.GetSkillLink((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y).RightLink;
					}
					if (vector.X != -1f && vector.Y != -1f)
					{
						SkillObj skill2 = SkillSystem.GetSkill((int)vector.X, (int)vector.Y);
						if (skill2.TraitType != SkillType.Null && skill2.Visible)
						{
							this.m_selectedTraitIndex = vector;
						}
					}
					if (selectedTraitIndex != this.m_selectedTraitIndex)
					{
						SkillObj skill3 = SkillSystem.GetSkill((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y);
						this.m_selectionIcon.Position = SkillSystem.GetSkillPosition(skill3);
						this.UpdateDescriptionPlate(skill3);
						SoundManager.PlaySound("ShopMenuMove");
						skill3.Scale = new Vector2(1.1f, 1.1f);
						Tween.To(skill3, 0.1f, new Easing(Back.EaseOutLarge), new string[]
						{
							"ScaleX",
							"1",
							"ScaleY",
							"1"
						});
						this.m_dialoguePlate.Visible = true;
					}
					SkillObj skill4 = SkillSystem.GetSkill((int)this.m_selectedTraitIndex.X, (int)this.m_selectedTraitIndex.Y);
					if ((Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) && Game.PlayerStats.Gold >= skill4.TotalCost && skill4.CurrentLevel < skill4.MaxLevel)
					{
						SoundManager.PlaySound("TraitUpgrade");
						if (!this.m_fadingIn)
						{
							Game.PlayerStats.Gold -= skill4.TotalCost;
							this.SetVisible(skill4, true);
							SkillSystem.LevelUpTrait(skill4, true);
							if (skill4.CurrentLevel >= skill4.MaxLevel)
							{
								SoundManager.PlaySound("TraitMaxxed");
							}
							this.UpdateDescriptionPlate(skill4);
						}
					}
					else if ((Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1)) && Game.PlayerStats.Gold < skill4.TotalCost)
					{
						SoundManager.PlaySound("TraitPurchaseFail");
					}
					if (Game.GlobalInput.JustPressed(2) || (Game.GlobalInput.JustPressed(3) && !flag))
					{
						this.m_lockControls = true;
						RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
						ProceduralLevelScreen levelScreen = rCScreenManager.GetLevelScreen();
						levelScreen.Reset();
						if (levelScreen.CurrentRoom is StartingRoomObj)
						{
							rCScreenManager.StartWipeTransition();
							Tween.RunFunction(0.2f, rCScreenManager, "HideCurrentScreen", new object[0]);
							Tween.RunFunction(0.2f, levelScreen.CurrentRoom, "OnEnter", new object[0]);
						}
						else
						{
							(base.ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
						}
					}
					if (!LevelEV.RUN_DEMO_VERSION && !LevelEV.CREATE_RETAIL_VERSION && InputManager.JustPressed(Keys.Q, new PlayerIndex?(PlayerIndex.One)))
					{
						foreach (SkillObj current in SkillSystem.SkillArray)
						{
							if (current.CurrentLevel < current.MaxLevel)
							{
								this.SetVisible(current, false);
								SkillSystem.LevelUpTrait(current, false);
								this.CheckForSkillUnlock(current, false);
							}
						}
					}
				}
				base.HandleInput();
			}
		}
		public void EndCameraTween()
		{
			this.m_cameraTweening = false;
		}
		public void UpdateDescriptionPlate(SkillObj trait)
		{
			string text = trait.IconName;
			text = text.Replace("Locked", "");
			text = text.Replace("Max", "");
			this.m_skillIcon.ChangeSprite(text);
			this.m_skillTitle.Text = trait.Name;
			this.m_skillDescription.Text = trait.Description;
			this.m_skillDescription.WordWrap(280);
			this.m_inputDescription.Text = trait.InputDescription;
			this.m_inputDescription.WordWrap(280);
			this.m_inputDescription.Y = (float)(this.m_skillDescription.Bounds.Bottom + 10);
			float num = TraitStatType.GetTraitStat(trait.TraitType);
			if (num > -1f)
			{
				if (num < 1f)
				{
					num *= 100f;
					num = (float)((int)Math.Round((double)num, MidpointRounding.AwayFromZero));
				}
				if (num == 0f)
				{
					num = trait.ModifierAmount;
					if (trait.TraitType == SkillType.Crit_Chance_Up)
					{
						num *= 100f;
						num = (float)((int)Math.Round((double)num, MidpointRounding.AwayFromZero));
					}
				}
				this.m_skillCurrent.Text = "Current: " + num + trait.UnitOfMeasurement;
				if (trait.CurrentLevel < trait.MaxLevel)
				{
					float num2 = trait.PerLevelModifier;
					if (num2 < 1f && trait.TraitType != SkillType.Invuln_Time_Up)
					{
						num2 *= 100f;
						if (trait.TraitType != SkillType.Death_Dodge)
						{
							num2 = (float)((int)Math.Round((double)num2, MidpointRounding.AwayFromZero));
						}
					}
					this.m_skillUpgrade.Text = "Upgrade: +" + num2 + trait.UnitOfMeasurement;
				}
				else
				{
					this.m_skillUpgrade.Text = "Upgrade: --";
				}
				this.m_skillLevel.Text = string.Concat(new object[]
				{
					"Level: ",
					trait.CurrentLevel,
					"/",
					trait.MaxLevel
				});
				string arg = "unlock";
				if (trait.CurrentLevel > 0)
				{
					arg = "upgrade";
				}
				this.m_skillCost.Text = trait.TotalCost + " gold to " + arg;
				if (this.m_inputDescription.Text != " " && this.m_inputDescription.Text != "")
				{
					this.m_skillCurrent.Y = (float)(this.m_inputDescription.Bounds.Bottom + 40);
				}
				else
				{
					this.m_skillCurrent.Y = (float)(this.m_skillDescription.Bounds.Bottom + 40);
				}
				this.m_skillUpgrade.Y = this.m_skillCurrent.Y + 30f;
				this.m_skillLevel.Y = this.m_skillUpgrade.Y + 30f;
				this.m_descriptionDivider.Visible = true;
			}
			else
			{
				this.m_skillCurrent.Text = "";
				this.m_skillUpgrade.Text = "";
				this.m_skillLevel.Text = "";
				this.m_descriptionDivider.Visible = false;
				string arg2 = "unlock";
				if (trait.CurrentLevel > 0)
				{
					arg2 = "upgrade";
				}
				this.m_skillCost.Text = trait.TotalCost + " gold to " + arg2;
			}
			this.m_descriptionDivider.Position = new Vector2(this.m_skillCurrent.AbsX, this.m_skillCurrent.AbsY - 20f);
			if (trait.CurrentLevel >= trait.MaxLevel)
			{
				this.m_skillCost.Visible = false;
				this.m_skillCostBG.Visible = false;
			}
			else
			{
				this.m_skillCost.Visible = true;
				this.m_skillCostBG.Visible = true;
			}
			this.m_playerMoney.Text = Game.PlayerStats.Gold.ToString();
		}
		public override void Draw(GameTime gameTime)
		{
			this.m_cloud1.Y = (this.m_cloud2.Y = (this.m_cloud3.Y = (this.m_cloud4.Y = (this.m_cloud5.Y = base.Camera.TopLeftCorner.Y * 0.2f))));
			this.m_bg.Y = base.Camera.TopLeftCorner.Y * 0.2f;
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, base.Camera.GetTransformation());
			this.m_bg.Draw(base.Camera);
			this.m_cloud1.Draw(base.Camera);
			this.m_cloud2.Draw(base.Camera);
			this.m_cloud3.Draw(base.Camera);
			this.m_cloud4.Draw(base.Camera);
			this.m_cloud5.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_manor.Draw(base.Camera);
			this.m_impactEffectPool.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_selectionIcon.Draw(base.Camera);
			SkillObj[] skillArray = SkillSystem.GetSkillArray();
			for (int i = 0; i < skillArray.Length; i++)
			{
				SkillObj skillObj = skillArray[i];
				if (skillObj.TraitType != SkillType.Filler && skillObj.TraitType != SkillType.Null && skillObj.Visible)
				{
					skillObj.Draw(base.Camera);
				}
			}
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
			this.m_dialoguePlate.Draw(base.Camera);
			this.m_continueText.Draw(base.Camera);
			this.m_toggleIconsText.Draw(base.Camera);
			this.m_confirmText.Draw(base.Camera);
			this.m_navigationText.Draw(base.Camera);
			this.m_playerMoney.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_descriptionDivider.Draw(base.Camera);
			this.m_coinIcon.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Skill Screen");
				this.m_titleText.Dispose();
				this.m_titleText = null;
				this.m_bg.Dispose();
				this.m_bg = null;
				this.m_cloud1.Dispose();
				this.m_cloud1 = null;
				this.m_cloud2.Dispose();
				this.m_cloud2 = null;
				this.m_cloud3.Dispose();
				this.m_cloud3 = null;
				this.m_cloud4.Dispose();
				this.m_cloud4 = null;
				this.m_cloud5.Dispose();
				this.m_cloud5 = null;
				this.m_continueText.Dispose();
				this.m_continueText = null;
				this.m_toggleIconsText.Dispose();
				this.m_toggleIconsText = null;
				this.m_confirmText.Dispose();
				this.m_confirmText = null;
				this.m_navigationText.Dispose();
				this.m_navigationText = null;
				this.m_dialoguePlate.Dispose();
				this.m_dialoguePlate = null;
				this.m_selectionIcon.Dispose();
				this.m_selectionIcon = null;
				this.m_impactEffectPool.Dispose();
				this.m_impactEffectPool = null;
				this.m_manor.Dispose();
				this.m_manor = null;
				this.m_shakeObj = null;
				this.m_playerMoney.Dispose();
				this.m_playerMoney = null;
				this.m_coinIcon.Dispose();
				this.m_coinIcon = null;
				this.m_skillCurrent = null;
				this.m_skillCost = null;
				this.m_skillCostBG = null;
				this.m_skillDescription = null;
				this.m_inputDescription = null;
				this.m_skillUpgrade = null;
				this.m_skillLevel = null;
				this.m_skillIcon = null;
				this.m_skillTitle = null;
				this.m_descriptionDivider.Dispose();
				this.m_descriptionDivider = null;
				base.Dispose();
			}
		}
		public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
		{
			SoundManager.PlaySound("TowerLand");
			this.m_screenShakeMagnitude = magnitude;
			this.m_horizontalShake = horizontalShake;
			this.m_verticalShake = verticalShake;
			this.m_shakeScreen = true;
		}
		public void UpdateShake()
		{
			if (this.m_horizontalShake)
			{
				this.m_bg.X = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
				this.m_manor.X = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
			}
			if (this.m_verticalShake)
			{
				this.m_bg.Y = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
				this.m_manor.Y = (float)CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0f, 1f) * this.m_screenShakeMagnitude);
			}
		}
		public void StopScreenShake()
		{
			this.m_shakeScreen = false;
			this.m_bg.X = 0f;
			this.m_bg.Y = 0f;
			this.m_manor.X = 0f;
			this.m_manor.Y = 0f;
		}
	}
}
