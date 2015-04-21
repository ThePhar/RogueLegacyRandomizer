using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class LineageScreen : Screen
	{
		private SpriteObj m_titleText;
		private LineageObj m_startingLineageObj;
		private LineageObj m_selectedLineageObj;
		private int m_selectedLineageIndex;
		private List<LineageObj> m_currentBranchArray;
		private List<LineageObj> m_masterArray;
		private Vector2 m_startingPoint;
		private Vector2 m_currentPoint;
		private BackgroundObj m_background;
		private SpriteObj m_bgShadow;
		private TweenObject m_selectTween;
		private int m_xPosOffset = 400;
		private ObjContainer m_descriptionPlate;
		private int m_xShift;
		private float m_storedMusicVol;
		private KeyIconTextObj m_confirmText;
		private KeyIconTextObj m_navigationText;
		private KeyIconTextObj m_rerollText;
		private bool m_lockControls;
		public LineageScreen()
		{
			this.m_startingPoint = new Vector2(660f, 360f);
			this.m_currentPoint = this.m_startingPoint;
		}
		public override void LoadContent()
		{
			Game.HSVEffect.Parameters["Saturation"].SetValue(0);
			this.m_background = new BackgroundObj("LineageScreenBG_Sprite");
			this.m_background.SetRepeated(true, true, base.Camera, null);
			this.m_background.X -= 6600f;
			this.m_bgShadow = new SpriteObj("LineageScreenShadow_Sprite");
			this.m_bgShadow.Scale = new Vector2(11f, 11f);
			this.m_bgShadow.Y -= 10f;
			this.m_bgShadow.ForceDraw = true;
			this.m_bgShadow.Opacity = 0.9f;
			this.m_bgShadow.Position = new Vector2(660f, 360f);
			this.m_titleText = new SpriteObj("LineageTitleText_Sprite");
			this.m_titleText.X = 660f;
			this.m_titleText.Y = 72f;
			this.m_titleText.ForceDraw = true;
			int num = 20;
			this.m_descriptionPlate = new ObjContainer("LineageScreenPlate_Character");
			this.m_descriptionPlate.ForceDraw = true;
			this.m_descriptionPlate.Position = new Vector2((float)(1320 - this.m_descriptionPlate.Width - 30), (float)(720 - this.m_descriptionPlate.Height) / 2f);
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 12f;
			textObj.Align = Types.TextAlign.Centre;
			textObj.OutlineColour = new Color(181, 142, 39);
			textObj.OutlineWidth = 2;
			textObj.Text = "Sir Skunky the IV";
			textObj.OverrideParentScale = true;
			textObj.Position = new Vector2((float)this.m_descriptionPlate.Width / 2f, 15f);
			textObj.LimitCorners = true;
			this.m_descriptionPlate.AddChild(textObj);
			TextObj textObj2 = textObj.Clone() as TextObj;
			textObj2.FontSize = 10f;
			textObj2.Text = "Knight";
			textObj2.Align = Types.TextAlign.Left;
			textObj2.X = (float)num;
			textObj2.Y += 40f;
			this.m_descriptionPlate.AddChild(textObj2);
			KeyIconTextObj keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
			keyIconTextObj.FontSize = 8f;
			keyIconTextObj.OutlineColour = textObj2.OutlineColour;
			keyIconTextObj.OutlineWidth = 2;
			keyIconTextObj.OverrideParentScale = true;
			keyIconTextObj.Position = textObj2.Position;
			keyIconTextObj.Text = "Class description goes here";
			keyIconTextObj.Align = Types.TextAlign.Left;
			keyIconTextObj.Y += 30f;
			keyIconTextObj.X = (float)(num + 20);
			keyIconTextObj.LimitCorners = true;
			this.m_descriptionPlate.AddChild(keyIconTextObj);
			for (int i = 0; i < 2; i++)
			{
				TextObj textObj3 = textObj2.Clone() as TextObj;
				textObj3.Text = "TraitName";
				textObj3.X = (float)num;
				textObj3.Align = Types.TextAlign.Left;
				if (i > 0)
				{
					textObj3.Y = this.m_descriptionPlate.GetChildAt(this.m_descriptionPlate.NumChildren - 1).Y + 50f;
				}
				this.m_descriptionPlate.AddChild(textObj3);
				TextObj textObj4 = textObj2.Clone() as TextObj;
				textObj4.Text = "TraitDescription";
				textObj4.X = (float)(num + 20);
				textObj4.FontSize = 8f;
				textObj4.Align = Types.TextAlign.Left;
				this.m_descriptionPlate.AddChild(textObj4);
			}
			TextObj textObj5 = textObj2.Clone() as TextObj;
			textObj5.Text = "SpellName";
			textObj5.FontSize = 10f;
			textObj5.X = (float)num;
			textObj5.Align = Types.TextAlign.Left;
			this.m_descriptionPlate.AddChild(textObj5);
			KeyIconTextObj keyIconTextObj2 = new KeyIconTextObj(Game.JunicodeFont);
			keyIconTextObj2.OutlineColour = new Color(181, 142, 39);
			keyIconTextObj2.OutlineWidth = 2;
			keyIconTextObj2.OverrideParentScale = true;
			keyIconTextObj2.Position = new Vector2((float)this.m_descriptionPlate.Width / 2f, 15f);
			keyIconTextObj2.Y += 40f;
			keyIconTextObj2.Text = "SpellDescription";
			keyIconTextObj2.X = (float)(num + 20);
			keyIconTextObj2.FontSize = 8f;
			keyIconTextObj2.Align = Types.TextAlign.Left;
			keyIconTextObj2.LimitCorners = true;
			this.m_descriptionPlate.AddChild(keyIconTextObj2);
			this.m_masterArray = new List<LineageObj>();
			this.m_currentBranchArray = new List<LineageObj>();
			Vector2 arg_47E_0 = Vector2.Zero;
			this.m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_confirmText.ForceDraw = true;
			this.m_confirmText.FontSize = 12f;
			this.m_confirmText.DropShadow = new Vector2(2f, 2f);
			this.m_confirmText.Position = new Vector2(1280f, 630f);
			this.m_confirmText.Align = Types.TextAlign.Right;
			this.m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_navigationText.Align = Types.TextAlign.Right;
			this.m_navigationText.FontSize = 12f;
			this.m_navigationText.DropShadow = new Vector2(2f, 2f);
			this.m_navigationText.Position = new Vector2(this.m_confirmText.X, this.m_confirmText.Y + 40f);
			this.m_navigationText.ForceDraw = true;
			this.m_rerollText = new KeyIconTextObj(Game.JunicodeFont);
			this.m_rerollText.Align = Types.TextAlign.Right;
			this.m_rerollText.FontSize = 12f;
			this.m_rerollText.DropShadow = new Vector2(2f, 2f);
			this.m_rerollText.ForceDraw = true;
			this.m_rerollText.Position = new Vector2(1280f, 40f);
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			this.m_background.SetRepeated(true, true, base.Camera, null);
			base.ReinitializeRTs();
		}
		private void UpdateDescriptionPlate()
		{
			LineageObj lineageObj = this.m_currentBranchArray[this.m_selectedLineageIndex];
			TextObj textObj = this.m_descriptionPlate.GetChildAt(1) as TextObj;
			textObj.Text = lineageObj.PlayerName;
			TextObj textObj2 = this.m_descriptionPlate.GetChildAt(2) as TextObj;
			textObj2.Text = "Class - " + ClassType.ToString(lineageObj.Class, lineageObj.IsFemale);
			KeyIconTextObj keyIconTextObj = this.m_descriptionPlate.GetChildAt(3) as KeyIconTextObj;
			keyIconTextObj.Text = ClassType.Description(lineageObj.Class);
			keyIconTextObj.WordWrap(340);
			TextObj textObj3 = this.m_descriptionPlate.GetChildAt(4) as TextObj;
			textObj3.Y = keyIconTextObj.Y + (float)keyIconTextObj.Height + 5f;
			TextObj textObj4 = this.m_descriptionPlate.GetChildAt(5) as TextObj;
			textObj4.Y = textObj3.Y + 30f;
			int num = (int)textObj3.Y;
			if (lineageObj.Traits.X > 0f)
			{
				textObj3.Text = "Trait - " + TraitType.ToString((byte)lineageObj.Traits.X);
				textObj4.Text = TraitType.Description((byte)lineageObj.Traits.X, lineageObj.IsFemale);
				textObj4.WordWrap(340);
				num = (int)textObj4.Y + textObj4.Height + 5;
			}
			else
			{
				num = (int)textObj3.Y + textObj3.Height + 5;
				textObj3.Text = "Traits - None";
				textObj4.Text = "";
			}
			TextObj textObj5 = this.m_descriptionPlate.GetChildAt(6) as TextObj;
			textObj5.Y = textObj4.Y + (float)textObj4.Height + 5f;
			TextObj textObj6 = this.m_descriptionPlate.GetChildAt(7) as TextObj;
			textObj6.Y = textObj5.Y + 30f;
			if (lineageObj.Traits.Y > 0f)
			{
				textObj5.Text = "Trait - " + TraitType.ToString((byte)lineageObj.Traits.Y);
				textObj6.Text = TraitType.Description((byte)lineageObj.Traits.Y, lineageObj.IsFemale);
				textObj6.WordWrap(340);
				num = (int)textObj6.Y + textObj6.Height + 5;
			}
			else
			{
				textObj5.Text = "";
				textObj6.Text = "";
			}
			TextObj textObj7 = this.m_descriptionPlate.GetChildAt(8) as TextObj;
			textObj7.Text = "Spell - " + SpellType.ToString(lineageObj.Spell);
			textObj7.Y = (float)num;
			KeyIconTextObj keyIconTextObj2 = this.m_descriptionPlate.GetChildAt(9) as KeyIconTextObj;
			keyIconTextObj2.Text = SpellType.Description(lineageObj.Spell);
			keyIconTextObj2.Y = textObj7.Y + 30f;
			keyIconTextObj2.WordWrap(340);
		}
		private void AddLineageRow(int numLineages, Vector2 position, bool createEmpty, bool randomizePortrait)
		{
			if (this.m_selectedLineageObj != null)
			{
				this.m_selectedLineageObj.ForceDraw = false;
				this.m_selectedLineageObj.Y = 0f;
			}
			this.m_currentPoint = position;
			this.m_currentBranchArray.Clear();
			int[] array = new int[]
			{
				-450,
				0,
				450
			};
			int[] array2 = new int[]
			{
				-200,
				200
			};
			for (int i = 0; i < numLineages; i++)
			{
				LineageObj lineageObj = new LineageObj(this, createEmpty);
				if (randomizePortrait)
				{
					lineageObj.RandomizePortrait();
				}
				lineageObj.ForceDraw = true;
				lineageObj.X = position.X + (float)this.m_xPosOffset;
				int[] array3 = array;
				if (numLineages == 2)
				{
					array3 = array2;
				}
				lineageObj.Y = (float)array3[i];
				this.m_currentBranchArray.Add(lineageObj);
				if (lineageObj.Traits.X == 20f || lineageObj.Traits.Y == 20f)
				{
					lineageObj.FlipPortrait = true;
				}
			}
			this.m_currentPoint = this.m_currentBranchArray[1].Position;
			base.Camera.Position = this.m_currentPoint;
			this.m_selectedLineageObj = this.m_currentBranchArray[1];
			this.m_selectedLineageIndex = 1;
		}
		public override void OnEnter()
		{
			this.m_lockControls = false;
			SoundManager.PlayMusic("SkillTreeSong", true, 1f);
			this.m_storedMusicVol = SoundManager.GlobalMusicVolume;
			SoundManager.GlobalMusicVolume = 0f;
			if (SoundManager.AudioEngine != null)
			{
				SoundManager.AudioEngine.GetCategory("Legacy").SetVolume(this.m_storedMusicVol);
			}
			if (Game.LineageSongCue != null && Game.LineageSongCue.IsPlaying)
			{
				Game.LineageSongCue.Stop(AudioStopOptions.Immediate);
				Game.LineageSongCue.Dispose();
			}
			Game.LineageSongCue = SoundManager.GetMusicCue("LegacySong");
			if (Game.LineageSongCue != null)
			{
				Game.LineageSongCue.Play();
			}
			this.LoadFamilyTreeData();
			this.LoadCurrentBranches();
			base.Camera.Position = this.m_selectedLineageObj.Position;
			this.UpdateDescriptionPlate();
			this.m_confirmText.Text = "[Input:" + 0 + "] to select a child";
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				this.m_navigationText.Text = "[Button:LeftStick] to view family tree";
			}
			else
			{
				this.m_navigationText.Text = "Arrow keys to view family tree";
			}
			this.m_rerollText.Text = "[Input:" + 9 + "] to re-roll your children once";
			if (SkillSystem.GetSkill(SkillType.Randomize_Children).ModifierAmount > 0f && !Game.PlayerStats.RerolledChildren)
			{
				this.m_rerollText.Visible = true;
			}
			else
			{
				this.m_rerollText.Visible = false;
			}
			base.OnEnter();
		}
		public void LoadFamilyTreeData()
		{
			this.m_masterArray.Clear();
			int num = 700;
			if (Game.PlayerStats.FamilyTreeArray != null && Game.PlayerStats.FamilyTreeArray.Count > 0)
			{
				int num2 = 0;
				using (List<FamilyTreeNode>.Enumerator enumerator = Game.PlayerStats.FamilyTreeArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FamilyTreeNode current = enumerator.Current;
						LineageObj lineageObj = new LineageObj(this, true);
						lineageObj.IsDead = true;
						lineageObj.Age = current.Age;
						lineageObj.ChildAge = current.ChildAge;
						lineageObj.Class = current.Class;
						lineageObj.PlayerName = current.Name;
						lineageObj.IsFemale = current.IsFemale;
						lineageObj.SetPortrait(current.HeadPiece, current.ShoulderPiece, current.ChestPiece);
						lineageObj.NumEnemiesKilled = current.NumEnemiesBeaten;
						lineageObj.BeatenABoss = current.BeatenABoss;
						lineageObj.SetTraits(current.Traits);
						lineageObj.UpdateAge(num);
						lineageObj.UpdateData();
						lineageObj.UpdateClassRank();
						num += (int)lineageObj.Age;
						lineageObj.X = (float)num2;
						num2 += this.m_xPosOffset;
						this.m_masterArray.Add(lineageObj);
						if (lineageObj.Traits.X == 20f || lineageObj.Traits.Y == 20f)
						{
							lineageObj.FlipPortrait = true;
						}
					}
					return;
				}
			}
			int num3 = 0;
			LineageObj lineageObj2 = new LineageObj(this, true);
			lineageObj2.IsDead = true;
			lineageObj2.Age = 30;
			lineageObj2.ChildAge = 5;
			lineageObj2.Class = 0;
			lineageObj2.PlayerName = "Sir Johannes";
			lineageObj2.SetPortrait(1, 1, 1);
			lineageObj2.NumEnemiesKilled = 50;
			lineageObj2.BeatenABoss = false;
			lineageObj2.UpdateAge(num);
			lineageObj2.UpdateData();
			lineageObj2.UpdateClassRank();
			num += (int)lineageObj2.Age;
			lineageObj2.X = (float)num3;
			num3 += this.m_xPosOffset;
			this.m_masterArray.Add(lineageObj2);
			if (lineageObj2.Traits.X == 20f || lineageObj2.Traits.Y == 20f)
			{
				lineageObj2.FlipPortrait = true;
			}
		}
		public void LoadCurrentBranches()
		{
			if (Game.PlayerStats.CurrentBranches == null || Game.PlayerStats.CurrentBranches.Count < 1)
			{
				this.AddLineageRow(3, this.m_masterArray[this.m_masterArray.Count - 1].Position, false, true);
				List<PlayerLineageData> list = new List<PlayerLineageData>();
				for (int i = 0; i < this.m_currentBranchArray.Count; i++)
				{
					list.Add(new PlayerLineageData
					{
						Name = this.m_currentBranchArray[i].PlayerName,
						HeadPiece = this.m_currentBranchArray[i].HeadPiece,
						ShoulderPiece = this.m_currentBranchArray[i].ShoulderPiece,
						ChestPiece = this.m_currentBranchArray[i].ChestPiece,
						IsFemale = this.m_currentBranchArray[i].IsFemale,
						Class = this.m_currentBranchArray[i].Class,
						Spell = this.m_currentBranchArray[i].Spell,
						Traits = this.m_currentBranchArray[i].Traits,
						Age = this.m_currentBranchArray[i].Age,
						ChildAge = this.m_currentBranchArray[i].ChildAge,
						IsFemale = this.m_currentBranchArray[i].IsFemale
					});
				}
				Game.PlayerStats.CurrentBranches = list;
				(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.Lineage
				});
				return;
			}
			this.AddLineageRow(3, this.m_masterArray[this.m_masterArray.Count - 1].Position, true, true);
			List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
			for (int j = 0; j < this.m_currentBranchArray.Count; j++)
			{
				this.m_currentBranchArray[j].PlayerName = currentBranches[j].Name;
				this.m_currentBranchArray[j].SetPortrait(currentBranches[j].HeadPiece, currentBranches[j].ShoulderPiece, currentBranches[j].ChestPiece);
				this.m_currentBranchArray[j].Spell = currentBranches[j].Spell;
				this.m_currentBranchArray[j].Class = currentBranches[j].Class;
				this.m_currentBranchArray[j].ClearTraits();
				this.m_currentBranchArray[j].Traits = currentBranches[j].Traits;
				this.m_currentBranchArray[j].Age = currentBranches[j].Age;
				this.m_currentBranchArray[j].ChildAge = currentBranches[j].ChildAge;
				this.m_currentBranchArray[j].IsFemale = currentBranches[j].IsFemale;
				this.m_currentBranchArray[j].UpdateData();
			}
		}
		public override void OnExit()
		{
			float num = 0.0166666675f;
			float num2 = this.m_storedMusicVol;
			float num3 = this.m_storedMusicVol / 120f;
			for (int i = 0; i < 120; i++)
			{
				Tween.RunFunction(num * (float)i, this, "ReduceMusic", new object[]
				{
					num2
				});
				num2 -= num3;
			}
			Tween.RunFunction(2f, this, "StopLegacySong", new object[0]);
			Game.PlayerStats.CurrentBranches = null;
			if (Game.PlayerStats.Class == 16)
			{
				GameUtil.UnlockAchievement("FEAR_OF_GRAVITY");
			}
			if (Game.PlayerStats.Traits == Vector2.Zero)
			{
				GameUtil.UnlockAchievement("FEAR_OF_IMPERFECTIONS");
			}
			base.OnExit();
		}
		public void ReduceMusic(float newVolume)
		{
			if (SoundManager.AudioEngine != null)
			{
				SoundManager.AudioEngine.GetCategory("Legacy").SetVolume(newVolume);
				SoundManager.GlobalMusicVolume += this.m_storedMusicVol - newVolume;
				if (SoundManager.GlobalMusicVolume > this.m_storedMusicVol)
				{
					SoundManager.GlobalMusicVolume = this.m_storedMusicVol;
				}
			}
		}
		public void StopLegacySong()
		{
			if (Game.LineageSongCue != null && Game.LineageSongCue.IsPlaying)
			{
				Game.LineageSongCue.Stop(AudioStopOptions.Immediate);
			}
			if (Game.LineageSongCue != null)
			{
				Game.LineageSongCue.Dispose();
				Game.LineageSongCue = null;
			}
			SoundManager.GlobalMusicVolume = this.m_storedMusicVol;
		}
		public override void Update(GameTime gameTime)
		{
			this.m_bgShadow.Opacity = 0.8f + 0.05f * (float)Math.Sin((double)(Game.TotalGameTime * 4f));
			if (Game.LineageSongCue != null && !Game.LineageSongCue.IsPlaying)
			{
				Game.LineageSongCue.Dispose();
				Game.LineageSongCue = SoundManager.GetMusicCue("LegacySong");
				Game.LineageSongCue.Play();
				SoundManager.StopMusic(0f);
				SoundManager.PlayMusic("SkillTreeSong", true, 1f);
			}
			base.Update(gameTime);
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls && (this.m_selectTween == null || (this.m_selectTween != null && !this.m_selectTween.Active)))
			{
				LineageObj selectedLineageObj = this.m_selectedLineageObj;
				int selectedLineageIndex = this.m_selectedLineageIndex;
				if (Game.GlobalInput.JustPressed(9) && SkillSystem.GetSkill(SkillType.Randomize_Children).ModifierAmount > 0f && !Game.PlayerStats.RerolledChildren)
				{
					this.m_lockControls = true;
					SoundManager.PlaySound(new string[]
					{
						"frame_woosh_01",
						"frame_woosh_02"
					});
					if (this.m_xShift != 0)
					{
						this.m_xShift = 0;
						Tween.By(this.m_descriptionPlate, 0.2f, new Easing(Back.EaseOut), new string[]
						{
							"delay",
							"0.2",
							"X",
							"-600"
						});
						this.m_selectTween = Tween.To(base.Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
						{
							"delay",
							"0.2",
							"X",
							(this.m_masterArray.Count * this.m_xPosOffset).ToString()
						});
					}
					(base.ScreenManager as RCScreenManager).StartWipeTransition();
					Tween.RunFunction(0.2f, this, "RerollCurrentBranch", new object[0]);
				}
				if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
				{
					if (base.Camera.X > this.m_masterArray[0].X + 10f)
					{
						SoundManager.PlaySound("frame_swoosh_01");
						this.m_selectTween = Tween.By(base.Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
						{
							"X",
							(-this.m_xPosOffset).ToString()
						});
						if (this.m_xShift == 0)
						{
							Tween.By(this.m_descriptionPlate, 0.2f, new Easing(Back.EaseIn), new string[]
							{
								"X",
								"600"
							});
						}
						this.m_xShift--;
					}
				}
				else if ((Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) && this.m_xShift < 0)
				{
					SoundManager.PlaySound("frame_swoosh_01");
					this.m_selectTween = Tween.By(base.Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
					{
						"X",
						this.m_xPosOffset.ToString()
					});
					this.m_xShift++;
					if (this.m_xShift == 0)
					{
						Tween.By(this.m_descriptionPlate, 0.2f, new Easing(Back.EaseOut), new string[]
						{
							"X",
							"-600"
						});
					}
				}
				if (this.m_xShift == 0)
				{
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						if (this.m_selectedLineageIndex > 0)
						{
							SoundManager.PlaySound("frame_swap");
						}
						this.m_selectedLineageIndex--;
						if (this.m_selectedLineageIndex < 0)
						{
							this.m_selectedLineageIndex = 0;
						}
						if (this.m_selectedLineageIndex != selectedLineageIndex)
						{
							this.UpdateDescriptionPlate();
							this.m_selectTween = Tween.By(this.m_currentBranchArray[0], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"450"
							});
							Tween.By(this.m_currentBranchArray[1], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"450"
							});
							Tween.By(this.m_currentBranchArray[2], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"450"
							});
						}
					}
					else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
					{
						if (this.m_selectedLineageIndex < this.m_currentBranchArray.Count - 1)
						{
							SoundManager.PlaySound("frame_swap");
						}
						this.m_selectedLineageIndex++;
						if (this.m_selectedLineageIndex > this.m_currentBranchArray.Count - 1)
						{
							this.m_selectedLineageIndex = this.m_currentBranchArray.Count - 1;
						}
						if (this.m_selectedLineageIndex != selectedLineageIndex)
						{
							this.UpdateDescriptionPlate();
							this.m_selectTween = Tween.By(this.m_currentBranchArray[0], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"-450"
							});
							Tween.By(this.m_currentBranchArray[1], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"-450"
							});
							Tween.By(this.m_currentBranchArray[2], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"-450"
							});
						}
					}
				}
				this.m_selectedLineageObj = this.m_currentBranchArray[this.m_selectedLineageIndex];
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					if (this.m_xShift == 0)
					{
						if (selectedLineageObj == this.m_selectedLineageObj)
						{
							RCScreenManager rCScreenManager = base.ScreenManager as RCScreenManager;
							rCScreenManager.DialogueScreen.SetDialogue("LineageChoiceWarning");
							rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
							rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "StartGame", new object[0]);
							rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
							{
								"Canceling Selection"
							});
							(base.ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
						}
					}
					else
					{
						this.m_xShift = 0;
						SoundManager.PlaySound(new string[]
						{
							"frame_woosh_01",
							"frame_woosh_02"
						});
						Tween.By(this.m_descriptionPlate, 0.2f, new Easing(Back.EaseOut), new string[]
						{
							"X",
							"-600"
						});
						this.m_selectTween = Tween.To(base.Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
						{
							"X",
							(this.m_masterArray.Count * this.m_xPosOffset).ToString()
						});
					}
				}
				base.HandleInput();
			}
		}
		public void RerollCurrentBranch()
		{
			this.m_rerollText.Visible = false;
			Game.PlayerStats.RerolledChildren = true;
			(base.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
			Game.PlayerStats.CurrentBranches.Clear();
			this.LoadCurrentBranches();
			(base.ScreenManager as RCScreenManager).EndWipeTransition();
			this.UpdateDescriptionPlate();
			this.m_lockControls = false;
		}
		public void StartGame()
		{
			Game.PlayerStats.HeadPiece = this.m_selectedLineageObj.HeadPiece;
			Game.PlayerStats.ShoulderPiece = this.m_selectedLineageObj.ShoulderPiece;
			Game.PlayerStats.ChestPiece = this.m_selectedLineageObj.ChestPiece;
			Game.PlayerStats.IsFemale = this.m_selectedLineageObj.IsFemale;
			Game.PlayerStats.Class = this.m_selectedLineageObj.Class;
			Game.PlayerStats.Traits = this.m_selectedLineageObj.Traits;
			Game.PlayerStats.Spell = this.m_selectedLineageObj.Spell;
			Game.PlayerStats.PlayerName = this.m_selectedLineageObj.PlayerName;
			Game.PlayerStats.Age = this.m_selectedLineageObj.Age;
			Game.PlayerStats.ChildAge = this.m_selectedLineageObj.ChildAge;
			if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
			{
				Game.PlayerStats.WizardSpellList = SpellType.GetNext3Spells();
			}
			Game.PlayerStats.CurrentBranches.Clear();
			(base.ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
		}
		public override void Draw(GameTime gameTime)
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
			base.Camera.End();
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, base.Camera.GetTransformation());
			foreach (LineageObj current in this.m_masterArray)
			{
				current.Draw(base.Camera);
			}
			foreach (LineageObj current2 in this.m_currentBranchArray)
			{
				current2.Draw(base.Camera);
			}
			base.Camera.End();
			if (base.Camera.Zoom >= 1f)
			{
				base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
				this.m_bgShadow.Draw(base.Camera);
				this.m_titleText.Draw(base.Camera);
				this.m_confirmText.Draw(base.Camera);
				this.m_navigationText.Draw(base.Camera);
				this.m_rerollText.Draw(base.Camera);
				this.m_descriptionPlate.Draw(base.Camera);
				base.Camera.End();
			}
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Lineage Screen");
				this.m_titleText.Dispose();
				this.m_titleText = null;
				this.m_selectedLineageObj = null;
				foreach (LineageObj current in this.m_currentBranchArray)
				{
					current.Dispose();
				}
				this.m_currentBranchArray.Clear();
				this.m_currentBranchArray = null;
				foreach (LineageObj current2 in this.m_masterArray)
				{
					if (!current2.IsDisposed)
					{
						current2.Dispose();
					}
				}
				this.m_masterArray.Clear();
				this.m_masterArray = null;
				if (this.m_startingLineageObj != null)
				{
					this.m_startingLineageObj.Dispose();
				}
				this.m_startingLineageObj = null;
				this.m_background.Dispose();
				this.m_background = null;
				this.m_bgShadow.Dispose();
				this.m_bgShadow = null;
				this.m_selectTween = null;
				this.m_descriptionPlate.Dispose();
				this.m_descriptionPlate = null;
				this.m_confirmText.Dispose();
				this.m_confirmText = null;
				this.m_navigationText.Dispose();
				this.m_navigationText = null;
				this.m_rerollText.Dispose();
				this.m_rerollText = null;
				base.Dispose();
			}
		}
		public int NameCopies(string name)
		{
			int num = 0;
			foreach (LineageObj current in this.m_masterArray)
			{
				if (current.PlayerName.Contains(" " + name))
				{
					num++;
				}
			}
			return num;
		}
		public bool CurrentBranchNameCopyFound(string name)
		{
			foreach (LineageObj current in this.m_currentBranchArray)
			{
				if (current.PlayerName.Contains(" " + name))
				{
					return true;
				}
			}
			return false;
		}
	}
}
