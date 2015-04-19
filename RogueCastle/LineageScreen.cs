using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
			m_startingPoint = new Vector2(660f, 360f);
			m_currentPoint = m_startingPoint;
		}
		public override void LoadContent()
		{
			Game.HSVEffect.Parameters["Saturation"].SetValue(0);
			m_background = new BackgroundObj("LineageScreenBG_Sprite");
			m_background.SetRepeated(true, true, Camera, null);
			m_background.X -= 6600f;
			m_bgShadow = new SpriteObj("LineageScreenShadow_Sprite");
			m_bgShadow.Scale = new Vector2(11f, 11f);
			m_bgShadow.Y -= 10f;
			m_bgShadow.ForceDraw = true;
			m_bgShadow.Opacity = 0.9f;
			m_bgShadow.Position = new Vector2(660f, 360f);
			m_titleText = new SpriteObj("LineageTitleText_Sprite");
			m_titleText.X = 660f;
			m_titleText.Y = 72f;
			m_titleText.ForceDraw = true;
			int num = 20;
			m_descriptionPlate = new ObjContainer("LineageScreenPlate_Character");
			m_descriptionPlate.ForceDraw = true;
			m_descriptionPlate.Position = new Vector2(1320 - m_descriptionPlate.Width - 30, (720 - m_descriptionPlate.Height) / 2f);
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 12f;
			textObj.Align = Types.TextAlign.Centre;
			textObj.OutlineColour = new Color(181, 142, 39);
			textObj.OutlineWidth = 2;
			textObj.Text = "Sir Skunky the IV";
			textObj.OverrideParentScale = true;
			textObj.Position = new Vector2(m_descriptionPlate.Width / 2f, 15f);
			textObj.LimitCorners = true;
			m_descriptionPlate.AddChild(textObj);
			TextObj textObj2 = textObj.Clone() as TextObj;
			textObj2.FontSize = 10f;
			textObj2.Text = "Knight";
			textObj2.Align = Types.TextAlign.Left;
			textObj2.X = num;
			textObj2.Y += 40f;
			m_descriptionPlate.AddChild(textObj2);
			KeyIconTextObj keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
			keyIconTextObj.FontSize = 8f;
			keyIconTextObj.OutlineColour = textObj2.OutlineColour;
			keyIconTextObj.OutlineWidth = 2;
			keyIconTextObj.OverrideParentScale = true;
			keyIconTextObj.Position = textObj2.Position;
			keyIconTextObj.Text = "Class description goes here";
			keyIconTextObj.Align = Types.TextAlign.Left;
			keyIconTextObj.Y += 30f;
			keyIconTextObj.X = num + 20;
			keyIconTextObj.LimitCorners = true;
			m_descriptionPlate.AddChild(keyIconTextObj);
			for (int i = 0; i < 2; i++)
			{
				TextObj textObj3 = textObj2.Clone() as TextObj;
				textObj3.Text = "TraitName";
				textObj3.X = num;
				textObj3.Align = Types.TextAlign.Left;
				if (i > 0)
				{
					textObj3.Y = m_descriptionPlate.GetChildAt(m_descriptionPlate.NumChildren - 1).Y + 50f;
				}
				m_descriptionPlate.AddChild(textObj3);
				TextObj textObj4 = textObj2.Clone() as TextObj;
				textObj4.Text = "TraitDescription";
				textObj4.X = num + 20;
				textObj4.FontSize = 8f;
				textObj4.Align = Types.TextAlign.Left;
				m_descriptionPlate.AddChild(textObj4);
			}
			TextObj textObj5 = textObj2.Clone() as TextObj;
			textObj5.Text = "SpellName";
			textObj5.FontSize = 10f;
			textObj5.X = num;
			textObj5.Align = Types.TextAlign.Left;
			m_descriptionPlate.AddChild(textObj5);
			KeyIconTextObj keyIconTextObj2 = new KeyIconTextObj(Game.JunicodeFont);
			keyIconTextObj2.OutlineColour = new Color(181, 142, 39);
			keyIconTextObj2.OutlineWidth = 2;
			keyIconTextObj2.OverrideParentScale = true;
			keyIconTextObj2.Position = new Vector2(m_descriptionPlate.Width / 2f, 15f);
			keyIconTextObj2.Y += 40f;
			keyIconTextObj2.Text = "SpellDescription";
			keyIconTextObj2.X = num + 20;
			keyIconTextObj2.FontSize = 8f;
			keyIconTextObj2.Align = Types.TextAlign.Left;
			keyIconTextObj2.LimitCorners = true;
			m_descriptionPlate.AddChild(keyIconTextObj2);
			m_masterArray = new List<LineageObj>();
			m_currentBranchArray = new List<LineageObj>();
			Vector2 arg_47E_0 = Vector2.Zero;
			m_confirmText = new KeyIconTextObj(Game.JunicodeFont);
			m_confirmText.ForceDraw = true;
			m_confirmText.FontSize = 12f;
			m_confirmText.DropShadow = new Vector2(2f, 2f);
			m_confirmText.Position = new Vector2(1280f, 630f);
			m_confirmText.Align = Types.TextAlign.Right;
			m_navigationText = new KeyIconTextObj(Game.JunicodeFont);
			m_navigationText.Align = Types.TextAlign.Right;
			m_navigationText.FontSize = 12f;
			m_navigationText.DropShadow = new Vector2(2f, 2f);
			m_navigationText.Position = new Vector2(m_confirmText.X, m_confirmText.Y + 40f);
			m_navigationText.ForceDraw = true;
			m_rerollText = new KeyIconTextObj(Game.JunicodeFont);
			m_rerollText.Align = Types.TextAlign.Right;
			m_rerollText.FontSize = 12f;
			m_rerollText.DropShadow = new Vector2(2f, 2f);
			m_rerollText.ForceDraw = true;
			m_rerollText.Position = new Vector2(1280f, 40f);
			base.LoadContent();
		}
		public override void ReinitializeRTs()
		{
			m_background.SetRepeated(true, true, Camera, null);
			base.ReinitializeRTs();
		}
		private void UpdateDescriptionPlate()
		{
			LineageObj lineageObj = m_currentBranchArray[m_selectedLineageIndex];
			TextObj textObj = m_descriptionPlate.GetChildAt(1) as TextObj;
			textObj.Text = lineageObj.PlayerName;
			TextObj textObj2 = m_descriptionPlate.GetChildAt(2) as TextObj;
			textObj2.Text = "Class - " + ClassType.ToString(lineageObj.Class, lineageObj.IsFemale);
			KeyIconTextObj keyIconTextObj = m_descriptionPlate.GetChildAt(3) as KeyIconTextObj;
			keyIconTextObj.Text = ClassType.Description(lineageObj.Class);
			keyIconTextObj.WordWrap(340);
			TextObj textObj3 = m_descriptionPlate.GetChildAt(4) as TextObj;
			textObj3.Y = keyIconTextObj.Y + keyIconTextObj.Height + 5f;
			TextObj textObj4 = m_descriptionPlate.GetChildAt(5) as TextObj;
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
			TextObj textObj5 = m_descriptionPlate.GetChildAt(6) as TextObj;
			textObj5.Y = textObj4.Y + textObj4.Height + 5f;
			TextObj textObj6 = m_descriptionPlate.GetChildAt(7) as TextObj;
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
			TextObj textObj7 = m_descriptionPlate.GetChildAt(8) as TextObj;
			textObj7.Text = "Spell - " + SpellType.ToString(lineageObj.Spell);
			textObj7.Y = num;
			KeyIconTextObj keyIconTextObj2 = m_descriptionPlate.GetChildAt(9) as KeyIconTextObj;
			keyIconTextObj2.Text = SpellType.Description(lineageObj.Spell);
			keyIconTextObj2.Y = textObj7.Y + 30f;
			keyIconTextObj2.WordWrap(340);
		}
		private void AddLineageRow(int numLineages, Vector2 position, bool createEmpty, bool randomizePortrait)
		{
			if (m_selectedLineageObj != null)
			{
				m_selectedLineageObj.ForceDraw = false;
				m_selectedLineageObj.Y = 0f;
			}
			m_currentPoint = position;
			m_currentBranchArray.Clear();
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
				lineageObj.X = position.X + m_xPosOffset;
				int[] array3 = array;
				if (numLineages == 2)
				{
					array3 = array2;
				}
				lineageObj.Y = array3[i];
				m_currentBranchArray.Add(lineageObj);
				if (lineageObj.Traits.X == 20f || lineageObj.Traits.Y == 20f)
				{
					lineageObj.FlipPortrait = true;
				}
			}
			m_currentPoint = m_currentBranchArray[1].Position;
			Camera.Position = m_currentPoint;
			m_selectedLineageObj = m_currentBranchArray[1];
			m_selectedLineageIndex = 1;
		}
		public override void OnEnter()
		{
			m_lockControls = false;
			SoundManager.PlayMusic("SkillTreeSong", true, 1f);
			m_storedMusicVol = SoundManager.GlobalMusicVolume;
			SoundManager.GlobalMusicVolume = 0f;
			if (SoundManager.AudioEngine != null)
			{
				SoundManager.AudioEngine.GetCategory("Legacy").SetVolume(m_storedMusicVol);
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
			LoadFamilyTreeData();
			LoadCurrentBranches();
			Camera.Position = m_selectedLineageObj.Position;
			UpdateDescriptionPlate();
			m_confirmText.Text = "[Input:" + 0 + "] to select a child";
			if (InputManager.GamePadIsConnected(PlayerIndex.One))
			{
				m_navigationText.Text = "[Button:LeftStick] to view family tree";
			}
			else
			{
				m_navigationText.Text = "Arrow keys to view family tree";
			}
			m_rerollText.Text = "[Input:" + 9 + "] to re-roll your children once";
			if (SkillSystem.GetSkill(SkillType.Randomize_Children).ModifierAmount > 0f && !Game.PlayerStats.RerolledChildren)
			{
				m_rerollText.Visible = true;
			}
			else
			{
				m_rerollText.Visible = false;
			}
			base.OnEnter();
		}
		public void LoadFamilyTreeData()
		{
			m_masterArray.Clear();
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
						num += lineageObj.Age;
						lineageObj.X = num2;
						num2 += m_xPosOffset;
						m_masterArray.Add(lineageObj);
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
			num += lineageObj2.Age;
			lineageObj2.X = num3;
			num3 += m_xPosOffset;
			m_masterArray.Add(lineageObj2);
			if (lineageObj2.Traits.X == 20f || lineageObj2.Traits.Y == 20f)
			{
				lineageObj2.FlipPortrait = true;
			}
		}
		public void LoadCurrentBranches()
		{
			if (Game.PlayerStats.CurrentBranches == null || Game.PlayerStats.CurrentBranches.Count < 1)
			{
				AddLineageRow(3, m_masterArray[m_masterArray.Count - 1].Position, false, true);
				List<PlayerLineageData> list = new List<PlayerLineageData>();
				for (int i = 0; i < m_currentBranchArray.Count; i++)
				{
					list.Add(new PlayerLineageData
					{
						Name = m_currentBranchArray[i].PlayerName,
						HeadPiece = m_currentBranchArray[i].HeadPiece,
						ShoulderPiece = m_currentBranchArray[i].ShoulderPiece,
						ChestPiece = m_currentBranchArray[i].ChestPiece,
						IsFemale = m_currentBranchArray[i].IsFemale,
						Class = m_currentBranchArray[i].Class,
						Spell = m_currentBranchArray[i].Spell,
						Traits = m_currentBranchArray[i].Traits,
						Age = m_currentBranchArray[i].Age,
						ChildAge = m_currentBranchArray[i].ChildAge,
						//IsFemale = this.m_currentBranchArray[i].IsFemale
					});
				}
				Game.PlayerStats.CurrentBranches = list;
				(ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.Lineage
				});
				return;
			}
			AddLineageRow(3, m_masterArray[m_masterArray.Count - 1].Position, true, true);
			List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
			for (int j = 0; j < m_currentBranchArray.Count; j++)
			{
				m_currentBranchArray[j].PlayerName = currentBranches[j].Name;
				m_currentBranchArray[j].SetPortrait(currentBranches[j].HeadPiece, currentBranches[j].ShoulderPiece, currentBranches[j].ChestPiece);
				m_currentBranchArray[j].Spell = currentBranches[j].Spell;
				m_currentBranchArray[j].Class = currentBranches[j].Class;
				m_currentBranchArray[j].ClearTraits();
				m_currentBranchArray[j].Traits = currentBranches[j].Traits;
				m_currentBranchArray[j].Age = currentBranches[j].Age;
				m_currentBranchArray[j].ChildAge = currentBranches[j].ChildAge;
				m_currentBranchArray[j].IsFemale = currentBranches[j].IsFemale;
				m_currentBranchArray[j].UpdateData();
			}
		}
		public override void OnExit()
		{
			float num = 0.0166666675f;
			float num2 = m_storedMusicVol;
			float num3 = m_storedMusicVol / 120f;
			for (int i = 0; i < 120; i++)
			{
				Tween.RunFunction(num * i, this, "ReduceMusic", new object[]
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
				SoundManager.GlobalMusicVolume += m_storedMusicVol - newVolume;
				if (SoundManager.GlobalMusicVolume > m_storedMusicVol)
				{
					SoundManager.GlobalMusicVolume = m_storedMusicVol;
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
			SoundManager.GlobalMusicVolume = m_storedMusicVol;
		}
		public override void Update(GameTime gameTime)
		{
			m_bgShadow.Opacity = 0.8f + 0.05f * (float)Math.Sin(Game.TotalGameTime * 4f);
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
			if (!m_lockControls && (m_selectTween == null || (m_selectTween != null && !m_selectTween.Active)))
			{
				LineageObj selectedLineageObj = m_selectedLineageObj;
				int selectedLineageIndex = m_selectedLineageIndex;
				if (Game.GlobalInput.JustPressed(9) && SkillSystem.GetSkill(SkillType.Randomize_Children).ModifierAmount > 0f && !Game.PlayerStats.RerolledChildren)
				{
					m_lockControls = true;
					SoundManager.PlaySound(new string[]
					{
						"frame_woosh_01",
						"frame_woosh_02"
					});
					if (m_xShift != 0)
					{
						m_xShift = 0;
						Tween.By(m_descriptionPlate, 0.2f, new Easing(Back.EaseOut), new string[]
						{
							"delay",
							"0.2",
							"X",
							"-600"
						});
						m_selectTween = Tween.To(Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
						{
							"delay",
							"0.2",
							"X",
							(m_masterArray.Count * m_xPosOffset).ToString()
						});
					}
					(ScreenManager as RCScreenManager).StartWipeTransition();
					Tween.RunFunction(0.2f, this, "RerollCurrentBranch", new object[0]);
				}
				if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
				{
					if (Camera.X > m_masterArray[0].X + 10f)
					{
						SoundManager.PlaySound("frame_swoosh_01");
						m_selectTween = Tween.By(Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
						{
							"X",
							(-m_xPosOffset).ToString()
						});
						if (m_xShift == 0)
						{
							Tween.By(m_descriptionPlate, 0.2f, new Easing(Back.EaseIn), new string[]
							{
								"X",
								"600"
							});
						}
						m_xShift--;
					}
				}
				else if ((Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) && m_xShift < 0)
				{
					SoundManager.PlaySound("frame_swoosh_01");
					m_selectTween = Tween.By(Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
					{
						"X",
						m_xPosOffset.ToString()
					});
					m_xShift++;
					if (m_xShift == 0)
					{
						Tween.By(m_descriptionPlate, 0.2f, new Easing(Back.EaseOut), new string[]
						{
							"X",
							"-600"
						});
					}
				}
				if (m_xShift == 0)
				{
					if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						if (m_selectedLineageIndex > 0)
						{
							SoundManager.PlaySound("frame_swap");
						}
						m_selectedLineageIndex--;
						if (m_selectedLineageIndex < 0)
						{
							m_selectedLineageIndex = 0;
						}
						if (m_selectedLineageIndex != selectedLineageIndex)
						{
							UpdateDescriptionPlate();
							m_selectTween = Tween.By(m_currentBranchArray[0], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"450"
							});
							Tween.By(m_currentBranchArray[1], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"450"
							});
							Tween.By(m_currentBranchArray[2], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"450"
							});
						}
					}
					else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
					{
						if (m_selectedLineageIndex < m_currentBranchArray.Count - 1)
						{
							SoundManager.PlaySound("frame_swap");
						}
						m_selectedLineageIndex++;
						if (m_selectedLineageIndex > m_currentBranchArray.Count - 1)
						{
							m_selectedLineageIndex = m_currentBranchArray.Count - 1;
						}
						if (m_selectedLineageIndex != selectedLineageIndex)
						{
							UpdateDescriptionPlate();
							m_selectTween = Tween.By(m_currentBranchArray[0], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"-450"
							});
							Tween.By(m_currentBranchArray[1], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"-450"
							});
							Tween.By(m_currentBranchArray[2], 0.3f, new Easing(Quad.EaseOut), new string[]
							{
								"Y",
								"-450"
							});
						}
					}
				}
				m_selectedLineageObj = m_currentBranchArray[m_selectedLineageIndex];
				if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
				{
					if (m_xShift == 0)
					{
						if (selectedLineageObj == m_selectedLineageObj)
						{
							RCScreenManager rCScreenManager = ScreenManager as RCScreenManager;
							rCScreenManager.DialogueScreen.SetDialogue("LineageChoiceWarning");
							rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
							rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "StartGame", new object[0]);
							rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", new object[]
							{
								"Canceling Selection"
							});
							(ScreenManager as RCScreenManager).DisplayScreen(13, true, null);
						}
					}
					else
					{
						m_xShift = 0;
						SoundManager.PlaySound(new string[]
						{
							"frame_woosh_01",
							"frame_woosh_02"
						});
						Tween.By(m_descriptionPlate, 0.2f, new Easing(Back.EaseOut), new string[]
						{
							"X",
							"-600"
						});
						m_selectTween = Tween.To(Camera, 0.3f, new Easing(Quad.EaseOut), new string[]
						{
							"X",
							(m_masterArray.Count * m_xPosOffset).ToString()
						});
					}
				}
				base.HandleInput();
			}
		}
		public void RerollCurrentBranch()
		{
			m_rerollText.Visible = false;
			Game.PlayerStats.RerolledChildren = true;
			(ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData
			});
			Game.PlayerStats.CurrentBranches.Clear();
			LoadCurrentBranches();
			(ScreenManager as RCScreenManager).EndWipeTransition();
			UpdateDescriptionPlate();
			m_lockControls = false;
		}
		public void StartGame()
		{
			Game.PlayerStats.HeadPiece = m_selectedLineageObj.HeadPiece;
			Game.PlayerStats.ShoulderPiece = m_selectedLineageObj.ShoulderPiece;
			Game.PlayerStats.ChestPiece = m_selectedLineageObj.ChestPiece;
			Game.PlayerStats.IsFemale = m_selectedLineageObj.IsFemale;
			Game.PlayerStats.Class = m_selectedLineageObj.Class;
			Game.PlayerStats.Traits = m_selectedLineageObj.Traits;
			Game.PlayerStats.Spell = m_selectedLineageObj.Spell;
			Game.PlayerStats.PlayerName = m_selectedLineageObj.PlayerName;
			Game.PlayerStats.Age = m_selectedLineageObj.Age;
			Game.PlayerStats.ChildAge = m_selectedLineageObj.ChildAge;
			if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
			{
				Game.PlayerStats.WizardSpellList = SpellType.GetNext3Spells();
			}
			Game.PlayerStats.CurrentBranches.Clear();
			(ScreenManager as RCScreenManager).DisplayScreen(15, true, null);
		}
		public override void Draw(GameTime gameTime)
		{
			if (Camera.X > m_background.X + 6600f)
			{
				m_background.X = Camera.X;
			}
			if (Camera.X < m_background.X)
			{
				m_background.X = Camera.X - 1320f;
			}
			Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Camera.GetTransformation());
			m_background.Draw(Camera);
			Camera.End();
			Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.GetTransformation());
			foreach (LineageObj current in m_masterArray)
			{
				current.Draw(Camera);
			}
			foreach (LineageObj current2 in m_currentBranchArray)
			{
				current2.Draw(Camera);
			}
			Camera.End();
			if (Camera.Zoom >= 1f)
			{
				Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
				m_bgShadow.Draw(Camera);
				m_titleText.Draw(Camera);
				m_confirmText.Draw(Camera);
				m_navigationText.Draw(Camera);
				m_rerollText.Draw(Camera);
				m_descriptionPlate.Draw(Camera);
				Camera.End();
			}
			base.Draw(gameTime);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Console.WriteLine("Disposing Lineage Screen");
				m_titleText.Dispose();
				m_titleText = null;
				m_selectedLineageObj = null;
				foreach (LineageObj current in m_currentBranchArray)
				{
					current.Dispose();
				}
				m_currentBranchArray.Clear();
				m_currentBranchArray = null;
				foreach (LineageObj current2 in m_masterArray)
				{
					if (!current2.IsDisposed)
					{
						current2.Dispose();
					}
				}
				m_masterArray.Clear();
				m_masterArray = null;
				if (m_startingLineageObj != null)
				{
					m_startingLineageObj.Dispose();
				}
				m_startingLineageObj = null;
				m_background.Dispose();
				m_background = null;
				m_bgShadow.Dispose();
				m_bgShadow = null;
				m_selectTween = null;
				m_descriptionPlate.Dispose();
				m_descriptionPlate = null;
				m_confirmText.Dispose();
				m_confirmText = null;
				m_navigationText.Dispose();
				m_navigationText = null;
				m_rerollText.Dispose();
				m_rerollText = null;
				base.Dispose();
			}
		}
		public int NameCopies(string name)
		{
			int num = 0;
			foreach (LineageObj current in m_masterArray)
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
			foreach (LineageObj current in m_currentBranchArray)
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
