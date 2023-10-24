//  RogueLegacyRandomizer - LineageScreen.cs
//  Last Modified 2023-10-24 4:19 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using RogueLegacy.Enums;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class LineageScreen : Screen
    {
        private readonly Vector2          _startingPoint;
        private readonly int              _xPosOffset = 400;
        private          BackgroundObj    _background;
        private          SpriteObj        _bgShadow;
        private          KeyIconTextObj   _confirmText;
        private          List<LineageObj> _currentBranchArray;
        private          Vector2          _currentPoint;
        private          ObjContainer     _descriptionPlate;
        private          bool             _lockControls;
        private          List<LineageObj> _masterArray;
        private          KeyIconTextObj   _navigationText;
        private          KeyIconTextObj   _rerollText;
        private          int              _selectedLineageIndex;
        private          LineageObj       _selectedLineageObj;
        private          TweenObject      _selectTween;
        private          LineageObj       _startingLineageObj;
        private          float            _storedMusicVol;
        private          SpriteObj        _titleText;
        private          int              _xShift;

        public LineageScreen()
        {
            _startingPoint = new Vector2(660f, 360f);
            _currentPoint = _startingPoint;
        }

        public override void LoadContent()
        {
            Game.HSVEffect.Parameters["Saturation"].SetValue(0);
            _background = new BackgroundObj("LineageScreenBG_Sprite");
            _background.SetRepeated(true, true, Camera);
            _background.X -= 6600f;
            _bgShadow = new SpriteObj("LineageScreenShadow_Sprite");
            _bgShadow.Scale = new Vector2(11f, 11f);
            _bgShadow.Y -= 10f;
            _bgShadow.ForceDraw = true;
            _bgShadow.Opacity = 0.9f;
            _bgShadow.Position = new Vector2(660f, 360f);
            _titleText = new SpriteObj("LineageTitleText_Sprite");
            _titleText.X = 660f;
            _titleText.Y = 72f;
            _titleText.ForceDraw = true;
            var num = 20;
            _descriptionPlate = new ObjContainer("LineageScreenPlate_Character");
            _descriptionPlate.ForceDraw = true;
            _descriptionPlate.Position = new Vector2(1320 - _descriptionPlate.Width - 30,
                (720 - _descriptionPlate.Height) / 2f);
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.FontSize = 12f;
            textObj.Align = Types.TextAlign.Centre;
            textObj.OutlineColour = new Color(181, 142, 39);
            textObj.OutlineWidth = 2;
            textObj.Text = "Sir Skunky the IV";
            textObj.OverrideParentScale = true;
            textObj.Position = new Vector2(_descriptionPlate.Width / 2f, 15f);
            textObj.LimitCorners = true;
            _descriptionPlate.AddChild(textObj);
            var textObj2 = textObj.Clone() as TextObj;
            textObj2.FontSize = 10f;
            textObj2.Text = "Knight";
            textObj2.Align = Types.TextAlign.Left;
            textObj2.X = num;
            textObj2.Y += 40f;
            _descriptionPlate.AddChild(textObj2);
            var keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
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
            _descriptionPlate.AddChild(keyIconTextObj);
            for (var i = 0; i < 2; i++)
            {
                var textObj3 = textObj2.Clone() as TextObj;
                textObj3.Text = "TraitName";
                textObj3.X = num;
                textObj3.Align = Types.TextAlign.Left;
                if (i > 0)
                {
                    textObj3.Y = _descriptionPlate.GetChildAt(_descriptionPlate.NumChildren - 1).Y + 50f;
                }

                _descriptionPlate.AddChild(textObj3);
                var textObj4 = textObj2.Clone() as TextObj;
                textObj4.Text = "TraitDescription";
                textObj4.X = num + 20;
                textObj4.FontSize = 8f;
                textObj4.Align = Types.TextAlign.Left;
                _descriptionPlate.AddChild(textObj4);
            }

            var textObj5 = textObj2.Clone() as TextObj;
            textObj5.Text = "SpellName";
            textObj5.FontSize = 10f;
            textObj5.X = num;
            textObj5.Align = Types.TextAlign.Left;
            _descriptionPlate.AddChild(textObj5);
            var keyIconTextObj2 = new KeyIconTextObj(Game.JunicodeFont);
            keyIconTextObj2.OutlineColour = new Color(181, 142, 39);
            keyIconTextObj2.OutlineWidth = 2;
            keyIconTextObj2.OverrideParentScale = true;
            keyIconTextObj2.Position = new Vector2(_descriptionPlate.Width / 2f, 15f);
            keyIconTextObj2.Y += 40f;
            keyIconTextObj2.Text = "SpellDescription";
            keyIconTextObj2.X = num + 20;
            keyIconTextObj2.FontSize = 8f;
            keyIconTextObj2.Align = Types.TextAlign.Left;
            keyIconTextObj2.LimitCorners = true;
            _descriptionPlate.AddChild(keyIconTextObj2);
            _masterArray = new List<LineageObj>();
            _currentBranchArray = new List<LineageObj>();
            var arg_47E_0 = Vector2.Zero;
            _confirmText = new KeyIconTextObj(Game.JunicodeFont);
            _confirmText.ForceDraw = true;
            _confirmText.FontSize = 12f;
            _confirmText.DropShadow = new Vector2(2f, 2f);
            _confirmText.Position = new Vector2(1280f, 630f);
            _confirmText.Align = Types.TextAlign.Right;
            _navigationText = new KeyIconTextObj(Game.JunicodeFont);
            _navigationText.Align = Types.TextAlign.Right;
            _navigationText.FontSize = 12f;
            _navigationText.DropShadow = new Vector2(2f, 2f);
            _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40f);
            _navigationText.ForceDraw = true;
            _rerollText = new KeyIconTextObj(Game.JunicodeFont);
            _rerollText.Align = Types.TextAlign.Right;
            _rerollText.FontSize = 12f;
            _rerollText.DropShadow = new Vector2(2f, 2f);
            _rerollText.ForceDraw = true;
            _rerollText.Position = new Vector2(1280f, 40f);
            base.LoadContent();
        }

        public override void ReinitializeRTs()
        {
            _background.SetRepeated(true, true, Camera);
            base.ReinitializeRTs();
        }

        private void UpdateDescriptionPlate()
        {
            var lineageObj = _currentBranchArray[_selectedLineageIndex];
            var textObj = _descriptionPlate.GetChildAt(1) as TextObj;
            textObj.Text = lineageObj.PlayerName;
            var textObj2 = _descriptionPlate.GetChildAt(2) as TextObj;
            textObj2.Text = "Class - " + ((ClassType) lineageObj.Class).Name(lineageObj.IsFemale);
            var keyIconTextObj = _descriptionPlate.GetChildAt(3) as KeyIconTextObj;
            keyIconTextObj.Text = ((ClassType) lineageObj.Class).Description();
            keyIconTextObj.WordWrap(340);
            var textObj3 = _descriptionPlate.GetChildAt(4) as TextObj;
            textObj3.Y = keyIconTextObj.Y + keyIconTextObj.Height + 5f;
            var textObj4 = _descriptionPlate.GetChildAt(5) as TextObj;
            textObj4.Y = textObj3.Y + 30f;
            var num = (int) textObj3.Y;
            if (lineageObj.Traits.X > 0f)
            {
                textObj3.Text = "Trait - " + (Trait) lineageObj.Traits.X;
                textObj4.Text = ((Trait) lineageObj.Traits.X).Description(lineageObj.IsFemale);
                textObj4.WordWrap(340);
                num = (int) textObj4.Y + textObj4.Height + 5;
            }
            else
            {
                num = (int) textObj3.Y + textObj3.Height + 5;
                textObj3.Text = "Traits - None";
                textObj4.Text = "";
            }

            var textObj5 = _descriptionPlate.GetChildAt(6) as TextObj;
            textObj5.Y = textObj4.Y + textObj4.Height + 5f;
            var textObj6 = _descriptionPlate.GetChildAt(7) as TextObj;
            textObj6.Y = textObj5.Y + 30f;
            if (lineageObj.Traits.Y > 0f)
            {
                textObj5.Text = "Trait - " + (Trait) lineageObj.Traits.Y;
                textObj6.Text = ((Trait) lineageObj.Traits.Y).Description(lineageObj.IsFemale);
                textObj6.WordWrap(340);
                num = (int) textObj6.Y + textObj6.Height + 5;
            }
            else
            {
                textObj5.Text = "";
                textObj6.Text = "";
            }

            var textObj7 = _descriptionPlate.GetChildAt(8) as TextObj;
            textObj7.Text = "Spell - " + (SpellType) lineageObj.Spell;
            textObj7.Y = num;
            var keyIconTextObj2 = _descriptionPlate.GetChildAt(9) as KeyIconTextObj;
            keyIconTextObj2.Text = ((SpellType) lineageObj.Spell).Description();
            keyIconTextObj2.Y = textObj7.Y + 30f;
            keyIconTextObj2.WordWrap(340);
        }

        private void AddLineageRow(int numLineages, Vector2 position, bool createEmpty, bool randomizePortrait)
        {
            if (_selectedLineageObj != null)
            {
                _selectedLineageObj.ForceDraw = false;
                _selectedLineageObj.Y = 0f;
            }

            _currentPoint = position;
            _currentBranchArray.Clear();
            int[] childrenFive =
            {
                -900,
                -450,
                0,
                450,
                900
            };
            int[] childrenFour =
            {
                -900,
                -450,
                0,
                450
            };
            int[] childrenThree =
            {
                -450,
                0,
                450
            };
            int[] childrenTwo =
            {
                -450,
                0
            };
            int[] childrenOne =
            {
                0
            };
            for (var i = 0; i < numLineages; i++)
            {
                var lineageObj = new LineageObj(this, createEmpty);
                if (randomizePortrait)
                {
                    lineageObj.RandomizePortrait();
                }

                lineageObj.ForceDraw = true;
                lineageObj.X = position.X + _xPosOffset;
                var children = childrenThree;
                switch (numLineages)
                {
                    case 1:
                        children = childrenOne;
                        break;

                    case 2:
                        children = childrenTwo;
                        break;

                    case 3:
                        break;

                    case 4:
                        children = childrenFour;
                        break;

                    case 5:
                        children = childrenFive;
                        break;
                }

                lineageObj.Y = children[i];
                _currentBranchArray.Add(lineageObj);
                if (Game.PlayerStats.HasVertigo)
                {
                    lineageObj.FlipPortrait = true;
                }
            }

            _currentPoint = _currentBranchArray[(int) Math.Floor(numLineages / 2.0)].Position;
            Camera.Position = _currentPoint;
            _selectedLineageObj = _currentBranchArray[(int) Math.Floor(numLineages / 2.0)];
            _selectedLineageIndex = (int) Math.Floor(numLineages / 2.0);
        }

        public override void OnEnter()
        {
            _lockControls = false;
            SoundManager.PlayMusic("SkillTreeSong", true, 1f);
            _storedMusicVol = SoundManager.GlobalMusicVolume;
            SoundManager.GlobalMusicVolume = 0f;
            if (SoundManager.AudioEngine != null)
            {
                SoundManager.AudioEngine.GetCategory("Legacy").SetVolume(_storedMusicVol);
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
            Camera.Position = _selectedLineageObj.Position;
            UpdateDescriptionPlate();
            _confirmText.Text = "[Input:" + 0 + "] to select a child";
            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                _navigationText.Text = "[Button:LeftStick] to view family tree";
            }
            else
            {
                _navigationText.Text = "Arrow keys to view family tree";
            }

            _rerollText.Text = "[Input:" + 9 + "] to re-roll your children once";
            if (SkillSystem.GetSkill(SkillType.RandomChildren).ModifierAmount > 0f &&
                !Game.PlayerStats.RerolledChildren)
            {
                _rerollText.Visible = true;
            }
            else
            {
                _rerollText.Visible = false;
            }

            // Disable death links now that we are in lineage screen.
            Program.Game.ArchipelagoManager.CanDeathLink = false;

            base.OnEnter();
        }

        public void LoadFamilyTreeData()
        {
            _masterArray.Clear();
            var num = 700;
            if (Game.PlayerStats.FamilyTreeArray != null && Game.PlayerStats.FamilyTreeArray.Count > 0)
            {
                var num2 = 0;
                using (var enumerator = Game.PlayerStats.FamilyTreeArray.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        var lineageObj = new LineageObj(this, true);
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
                        num2 += _xPosOffset;
                        _masterArray.Add(lineageObj);
                        if (Game.PlayerStats.HasVertigo)
                        {
                            lineageObj.FlipPortrait = true;
                        }
                    }

                    return;
                }
            }

            var num3 = 0;
            var lineageObj2 = new LineageObj(this, true);
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
            num3 += _xPosOffset;
            _masterArray.Add(lineageObj2);
            if (Game.PlayerStats.HasVertigo)
            {
                lineageObj2.FlipPortrait = true;
            }
        }

        public void LoadCurrentBranches()
        {
            var childrenCount = RandomizerData.NumberOfChildren;
            if (childrenCount == 0)
                childrenCount = CDGMath.RandomInt(1, 5);

            if (Game.PlayerStats.CurrentBranches == null || Game.PlayerStats.CurrentBranches.Count < 1)
            {
                AddLineageRow(childrenCount, _masterArray[_masterArray.Count - 1].Position, false, true);
                var list = new List<PlayerLineageData>();
                for (var i = 0; i < _currentBranchArray.Count; i++)
                {
                    list.Add(new PlayerLineageData
                    {
                        Name = _currentBranchArray[i].PlayerName,
                        HeadPiece = _currentBranchArray[i].HeadPiece,
                        ShoulderPiece = _currentBranchArray[i].ShoulderPiece,
                        ChestPiece = _currentBranchArray[i].ChestPiece,
                        IsFemale = _currentBranchArray[i].IsFemale,
                        Class = _currentBranchArray[i].Class,
                        Spell = _currentBranchArray[i].Spell,
                        Traits = _currentBranchArray[i].Traits,
                        Age = _currentBranchArray[i].Age,
                        ChildAge = _currentBranchArray[i].ChildAge
                        //IsFemale = this.m_currentBranchArray[i].IsFemale
                    });
                }

                Game.PlayerStats.CurrentBranches = list;
                (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.Lineage);
                return;
            }

            AddLineageRow(childrenCount, _masterArray[_masterArray.Count - 1].Position, true, true);
            var currentBranches = Game.PlayerStats.CurrentBranches;
            for (var j = 0; j < _currentBranchArray.Count; j++)
            {
                _currentBranchArray[j].PlayerName = currentBranches[j].Name;
                _currentBranchArray[j].SetPortrait(currentBranches[j].HeadPiece, currentBranches[j].ShoulderPiece,
                    currentBranches[j].ChestPiece);
                _currentBranchArray[j].Spell = currentBranches[j].Spell;
                _currentBranchArray[j].Class = currentBranches[j].Class;
                _currentBranchArray[j].ClearTraits();
                _currentBranchArray[j].Traits = currentBranches[j].Traits;
                _currentBranchArray[j].Age = currentBranches[j].Age;
                _currentBranchArray[j].ChildAge = currentBranches[j].ChildAge;
                _currentBranchArray[j].IsFemale = currentBranches[j].IsFemale;
                _currentBranchArray[j].UpdateData();
            }
        }

        public override void OnExit()
        {
            var num = 0.0166666675f;
            var num2 = _storedMusicVol;
            var num3 = _storedMusicVol / 120f;
            for (var i = 0; i < 120; i++)
            {
                Tween.RunFunction(num * i, this, "ReduceMusic", num2);
                num2 -= num3;
            }

            Tween.RunFunction(2f, this, "StopLegacySong");
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
                SoundManager.GlobalMusicVolume += _storedMusicVol - newVolume;
                if (SoundManager.GlobalMusicVolume > _storedMusicVol)
                {
                    SoundManager.GlobalMusicVolume = _storedMusicVol;
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

            SoundManager.GlobalMusicVolume = _storedMusicVol;
        }

        public override void Update(GameTime gameTime)
        {
            _bgShadow.Opacity = 0.8f + 0.05f * (float) Math.Sin(Game.TotalGameTimeSeconds * 4f);
            if (Game.LineageSongCue != null && !Game.LineageSongCue.IsPlaying)
            {
                Game.LineageSongCue.Dispose();
                Game.LineageSongCue = SoundManager.GetMusicCue("LegacySong");
                Game.LineageSongCue.Play();
                SoundManager.StopMusic();
                SoundManager.PlayMusic("SkillTreeSong", true, 1f);
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            if (!_lockControls && (_selectTween == null || _selectTween != null && !_selectTween.Active))
            {
                var selectedLineageObj = _selectedLineageObj;
                var selectedLineageIndex = _selectedLineageIndex;
                if (Game.GlobalInput.JustPressed(9) &&
                    SkillSystem.GetSkill(SkillType.RandomChildren).ModifierAmount > 0f &&
                    !Game.PlayerStats.RerolledChildren)
                {
                    _lockControls = true;
                    SoundManager.PlaySound("frame_woosh_01", "frame_woosh_02");
                    if (_xShift != 0)
                    {
                        _xShift = 0;
                        Tween.By(_descriptionPlate, 0.2f, Back.EaseOut, "delay", "0.2", "X", "-600");
                        _selectTween = Tween.To(Camera, 0.3f, Quad.EaseOut, "delay", "0.2", "X",
                            (_masterArray.Count * _xPosOffset).ToString());
                    }

                    (ScreenManager as RCScreenManager).StartWipeTransition();
                    Tween.RunFunction(0.2f, this, "RerollCurrentBranch");
                }

                if (Game.GlobalInput.Pressed(20) || Game.GlobalInput.Pressed(21))
                {
                    if (Camera.X > _masterArray[0].X + 10f)
                    {
                        SoundManager.PlaySound("frame_swoosh_01");
                        _selectTween = Tween.By(Camera, 0.3f, Quad.EaseOut, "X", (-_xPosOffset).ToString());
                        if (_xShift == 0)
                        {
                            Tween.By(_descriptionPlate, 0.2f, Back.EaseIn, "X", "600");
                        }

                        _xShift--;
                    }
                }
                else if ((Game.GlobalInput.Pressed(22) || Game.GlobalInput.Pressed(23)) && _xShift < 0)
                {
                    SoundManager.PlaySound("frame_swoosh_01");
                    _selectTween = Tween.By(Camera, 0.3f, Quad.EaseOut, "X", _xPosOffset.ToString());
                    _xShift++;
                    if (_xShift == 0)
                    {
                        Tween.By(_descriptionPlate, 0.2f, Back.EaseOut, "X", "-600");
                    }
                }

                if (_xShift == 0)
                {
                    if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                    {
                        if (_selectedLineageIndex > 0)
                        {
                            SoundManager.PlaySound("frame_swap");
                        }

                        _selectedLineageIndex--;
                        if (_selectedLineageIndex < 0)
                        {
                            _selectedLineageIndex = 0;
                        }

                        if (_selectedLineageIndex != selectedLineageIndex)
                        {
                            UpdateDescriptionPlate();

                            for (var i = 0; i < _currentBranchArray.Count; i++)
                            {
                                if (i == _selectedLineageIndex)
                                {
                                    _selectTween = Tween.By(_currentBranchArray[i], 0.3f, Quad.EaseOut, "Y", "450");
                                }
                                else
                                {
                                    Tween.By(_currentBranchArray[i], 0.3f, Quad.EaseOut, "Y", "450");
                                }
                            }
                        }
                    }
                    else if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
                    {
                        if (_selectedLineageIndex < _currentBranchArray.Count - 1)
                        {
                            SoundManager.PlaySound("frame_swap");
                        }

                        _selectedLineageIndex++;
                        if (_selectedLineageIndex > _currentBranchArray.Count - 1)
                        {
                            _selectedLineageIndex = _currentBranchArray.Count - 1;
                        }

                        if (_selectedLineageIndex != selectedLineageIndex)
                        {
                            UpdateDescriptionPlate();

                            for (var i = 0; i < _currentBranchArray.Count; i++)
                            {
                                if (i == _selectedLineageIndex)
                                {
                                    _selectTween = Tween.By(_currentBranchArray[i], 0.3f, Quad.EaseOut, "Y", "-450");
                                }
                                else
                                {
                                    Tween.By(_currentBranchArray[i], 0.3f, Quad.EaseOut, "Y", "-450");
                                }
                            }
                        }
                    }
                }

                _selectedLineageObj = _currentBranchArray[_selectedLineageIndex];
                if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                {
                    if (_xShift == 0)
                    {
                        if (selectedLineageObj == _selectedLineageObj)
                        {
                            var rCScreenManager = ScreenManager as RCScreenManager;
                            rCScreenManager.DialogueScreen.SetDialogue("LineageChoiceWarning");
                            rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "StartGame");
                            rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine",
                                "Canceling Selection");
                            (ScreenManager as RCScreenManager).DisplayScreen(13, true);
                        }
                    }
                    else
                    {
                        _xShift = 0;
                        SoundManager.PlaySound("frame_woosh_01", "frame_woosh_02");
                        Tween.By(_descriptionPlate, 0.2f, Back.EaseOut, "X", "-600");
                        _selectTween = Tween.To(Camera, 0.3f, Quad.EaseOut, "X",
                            (_masterArray.Count * _xPosOffset).ToString());
                    }
                }

                base.HandleInput();
            }
        }

        public void RerollCurrentBranch()
        {
            _rerollText.Visible = false;
            Game.PlayerStats.RerolledChildren = true;
            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData);
            Game.PlayerStats.CurrentBranches.Clear();
            LoadCurrentBranches();
            (ScreenManager as RCScreenManager).EndWipeTransition();
            UpdateDescriptionPlate();
            _lockControls = false;
        }

        public void StartGame()
        {
            Game.PlayerStats.HeadPiece = _selectedLineageObj.HeadPiece;
            Game.PlayerStats.ShoulderPiece = _selectedLineageObj.ShoulderPiece;
            Game.PlayerStats.ChestPiece = _selectedLineageObj.ChestPiece;
            Game.PlayerStats.IsFemale = _selectedLineageObj.IsFemale;
            Game.PlayerStats.Class = _selectedLineageObj.Class;
            Game.PlayerStats.Traits = _selectedLineageObj.Traits;
            Game.PlayerStats.Spell = _selectedLineageObj.Spell;
            Game.PlayerStats.PlayerName = _selectedLineageObj.PlayerName;
            Game.PlayerStats.Age = _selectedLineageObj.Age;
            Game.PlayerStats.ChildAge = _selectedLineageObj.ChildAge;
            if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
            {
                Game.PlayerStats.WizardSpellList = SpellExtensions.ArchmageSpellList();
            }

            Game.PlayerStats.CurrentBranches.Clear();
            (ScreenManager as RCScreenManager).DisplayScreen(15, true);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Camera.X > _background.X + 6600f)
            {
                _background.X = Camera.X;
            }

            if (Camera.X < _background.X)
            {
                _background.X = Camera.X - 1320f;
            }

            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,
                Camera.GetTransformation());
            _background.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.GetTransformation());
            foreach (var current in _masterArray)
            {
                current.Draw(Camera);
            }

            foreach (var current2 in _currentBranchArray)
            {
                current2.Draw(Camera);
            }

            Camera.End();
            if (Camera.Zoom >= 1f)
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    null);
                _bgShadow.Draw(Camera);
                _titleText.Draw(Camera);
                _confirmText.Draw(Camera);
                _navigationText.Draw(Camera);
                _rerollText.Draw(Camera);
                _descriptionPlate.Draw(Camera);
                Camera.End();
            }

            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Lineage Screen");
            _titleText.Dispose();
            _titleText = null;
            _selectedLineageObj = null;
            foreach (var current in _currentBranchArray)
            {
                current.Dispose();
            }

            _currentBranchArray.Clear();
            _currentBranchArray = null;
            foreach (var current2 in _masterArray)
            {
                if (!current2.IsDisposed)
                {
                    current2.Dispose();
                }
            }

            _masterArray.Clear();
            _masterArray = null;
            if (_startingLineageObj != null)
            {
                _startingLineageObj.Dispose();
            }

            _startingLineageObj = null;
            _background.Dispose();
            _background = null;
            _bgShadow.Dispose();
            _bgShadow = null;
            _selectTween = null;
            _descriptionPlate.Dispose();
            _descriptionPlate = null;
            _confirmText.Dispose();
            _confirmText = null;
            _navigationText.Dispose();
            _navigationText = null;
            _rerollText.Dispose();
            _rerollText = null;
            base.Dispose();
        }

        public int NameCopies(string name)
        {
            var num = 0;
            foreach (var current in _masterArray)
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
            foreach (var current in _currentBranchArray)
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
