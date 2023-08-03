// RogueLegacyRandomizer - CreditsScreen.cs
// Last Modified 2023-08-03 11:33 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Randomizer;
using RogueLegacy.GameObjects;
using Tweener;

namespace RogueLegacy.Screens
{
    public class CreditsScreen : Screen
    {
        private const float M_SCROLL_DURATION = 75f;

        private bool           _allowExit;
        private int            _backgroundIndex;
        private RenderTarget2D _backgroundRenderTarget;
        private string[]       _backgroundStrings;
        private float          _backgroundSwapTimer;
        private SpriteObj      _bg1;
        private SpriteObj      _bg2;
        private SpriteObj      _bg3;
        private SpriteObj      _bgOutside;
        private SpriteObj      _border1;
        private SpriteObj      _border2;
        private SpriteObj      _border3;
        private int            _child1Chest;
        private int            _child1Head;
        private int            _child1Shoulders;
        private int            _child2Chest;
        private int            _child2Head;
        private int            _child2Shoulders;
        private ObjContainer   _childSprite1;
        private ObjContainer   _childSprite2;
        private KeyIconTextObj _continueText;
        private List<TextObj>  _creditsNameList;
        private List<TextObj>  _creditsTitleList;
        private bool           _displayingContinueText;
        private SpriteObj      _glauber;
        private SpriteObj      _gordon;
        private SpriteObj      _ground1;
        private SpriteObj      _ground2;
        private SpriteObj      _ground3;
        private SpriteObj      _judson;
        private SpriteObj      _kenny;
        private Color          _lichColour1 = new(255, 255, 255, 255);
        private Color          _lichColour2 = new(198, 198, 198, 255);
        private ObjContainer   _manor;
        private ObjContainer   _playerSprite;
        private SpriteObj      _prop1;
        private ObjContainer   _prop2;
        private ObjContainer   _prop3;
        private float          _scrollDistance;
        private SpriteObj      _sideBorderBottom;
        private SpriteObj      _sideBorderLeft;
        private SpriteObj      _sideBorderRight;
        private SpriteObj      _sideBorderTop;
        private Color          _skinColour1 = new(231, 175, 131, 255);
        private Color          _skinColour2 = new(199, 109, 112, 255);
        private SkyObj         _sky;
        private RenderTarget2D _skyRenderTarget;
        private SpriteObj      _teddy;
        private TextObj        _thanksForPlayingText;
        private TextObj        _totalDeaths;
        private TextObj        _totalPlayTime;
        private int            _wifeChest;
        private int            _wifeHead;
        private int            _wifeShoulders;
        private ObjContainer   _wifeSprite;
        public  bool           IsEnding;

        public override void LoadContent()
        {
            _bgOutside = new SpriteObj("TraitsBG_Sprite");
            _bgOutside.ForceDraw = true;
            _bgOutside.Visible = false;
            _bgOutside.Scale = new Vector2(1320f / _bgOutside.Width, 1320f / _bgOutside.Width);
            _bg1 = new SpriteObj("CastleBG1_Sprite");
            _bg1.Position = new Vector2(660f, 200f);
            _bg1.Scale = new Vector2(2f, 2f);
            _bg2 = _bg1.Clone() as SpriteObj;
            _bg2.X -= _bg1.Width;
            _bg3 = _bg2.Clone() as SpriteObj;
            _bg3.X -= _bg2.Width;
            _ground1 = new SpriteObj("CastleFG1_Sprite");
            _ground1.Position = new Vector2(660f, 440f);
            _ground1.Scale = new Vector2(2f, 2f);
            _ground2 = _ground1.Clone() as SpriteObj;
            _ground2.X -= _ground1.Width;
            _ground3 = _ground2.Clone() as SpriteObj;
            _ground3.X -= _ground2.Width;
            _border1 = new SpriteObj("CastleBorder_Sprite");
            _border1.Position = new Vector2(660f, 440f);
            _border1.Scale = new Vector2(2f, 2f);
            _border2 = _border1.Clone() as SpriteObj;
            _border2.X -= _border1.Width;
            _border3 = _border2.Clone() as SpriteObj;
            _border3.X -= _border2.Width;
            _prop1 = new SpriteObj("CastleAssetWindow1_Sprite");
            _prop1.Position = new Vector2(0f, 220f);
            _prop1.Scale = new Vector2(2f, 2f);
            _prop2 = new ObjContainer("CastleAssetBackTorch_Character");
            _prop2.Position = new Vector2(500f, 330f);
            _prop2.Scale = new Vector2(2f, 2f);
            _prop2.AnimationDelay = 0.1f;
            _prop2.PlayAnimation();
            _prop3 = new ObjContainer("CastleAssetCandle1_Character");
            _prop3.Position = new Vector2(1000f, 440f);
            _prop3.Scale = new Vector2(2f, 2f);
            _playerSprite = new ObjContainer("PlayerWalking_Character");
            _playerSprite.Position = new Vector2(640f, 400f);
            _playerSprite.PlayAnimation();
            _playerSprite.AnimationDelay = 0.1f;
            _playerSprite.Flip = SpriteEffects.FlipHorizontally;
            _playerSprite.OutlineWidth = 2;
            var textureColor = new Color(251, 156, 172);
            _wifeSprite = new ObjContainer("PlayerWalking_Character");
            _wifeSprite.Position = new Vector2(-200f, 400f);
            _wifeSprite.PlayAnimation();
            _wifeSprite.AnimationDelay = 0.1f;
            _wifeSprite.OutlineWidth = 2;
            _wifeSprite.Scale = new Vector2(2f, 2f);
            _wifeSprite.GetChildAt(13).TextureColor = textureColor;
            _wifeSprite.GetChildAt(7).TextureColor = Color.Red;
            _wifeSprite.GetChildAt(1).TextureColor = Color.Red;
            _wifeSprite.GetChildAt(8).TextureColor = Color.Red;
            _wifeSprite.GetChildAt(11).TextureColor = new Color(11, 172, 239);
            _childSprite1 = new ObjContainer("PlayerWalking_Character");
            _childSprite1.Position = new Vector2(-270f, 415f);
            _childSprite1.PlayAnimation();
            _childSprite1.AnimationDelay = 0.1f;
            _childSprite1.OutlineWidth = 2;
            _childSprite1.Scale = new Vector2(1.2f, 1.2f);
            _childSprite1.GetChildAt(13).TextureColor = textureColor;
            _childSprite1.GetChildAt(7).TextureColor = Color.Red;
            _childSprite1.GetChildAt(1).TextureColor = Color.Red;
            _childSprite1.GetChildAt(8).TextureColor = Color.Red;
            _childSprite1.GetChildAt(11).TextureColor = new Color(11, 172, 239);
            _childSprite2 = new ObjContainer("PlayerWalking_Character");
            _childSprite2.Position = new Vector2(-330f, 420f);
            _childSprite2.PlayAnimation();
            _childSprite2.AnimationDelay = 0.1f;
            _childSprite2.OutlineWidth = 2;
            _childSprite2.Scale = new Vector2(1f, 1f);
            _childSprite2.GetChildAt(13).TextureColor = textureColor;
            _childSprite2.GetChildAt(7).TextureColor = Color.Red;
            _childSprite2.GetChildAt(1).TextureColor = Color.Red;
            _childSprite2.GetChildAt(8).TextureColor = Color.Red;
            _childSprite2.GetChildAt(11).TextureColor = new Color(11, 172, 239);
            _sideBorderLeft = new SpriteObj("Blank_Sprite");
            _sideBorderLeft.Scale = new Vector2(900f / _sideBorderLeft.Width, 500f / _sideBorderLeft.Height);
            _sideBorderLeft.Position = new Vector2(-450f, 0f);
            _sideBorderLeft.TextureColor = Color.Black;
            _sideBorderLeft.ForceDraw = true;
            _sideBorderRight = _sideBorderLeft.Clone() as SpriteObj;
            _sideBorderRight.Position = new Vector2(850f, 0f);
            _sideBorderTop = _sideBorderLeft.Clone() as SpriteObj;
            _sideBorderTop.Scale = new Vector2(1f, 1f);
            _sideBorderTop.Scale = new Vector2(1320f / _sideBorderTop.Width, 240 / _sideBorderTop.Height);
            _sideBorderTop.Position = Vector2.Zero;
            _sideBorderBottom = _sideBorderLeft.Clone() as SpriteObj;
            _sideBorderBottom.Scale = new Vector2(1f, 1f);
            _sideBorderBottom.Scale = new Vector2(1340f / _sideBorderBottom.Width, 720f / _sideBorderBottom.Height);
            _sideBorderBottom.Position = new Vector2(0f, 460f);

            // Manor?
            _manor = new ObjContainer("TraitsCastle_Character");
            _manor.Scale = new Vector2(2f, 2f);
            _manor.Visible = false;
            for (var i = 0; i < _manor.NumChildren; i++)
            {
                _manor.GetChildAt(i).Visible = false;
            }

            for (var i = 0; i < SkillSystem.GetSkillArray().Length; i++)
            {
                if (SkillSystem.GetSkillArray()[i].CurrentLevel > 0)
                {
                    if (SkillSystem.GetManorPiece(SkillSystem.GetSkillArray()[i]) != -1)
                    {
                        _manor.GetChildAt(SkillSystem.GetManorPiece(SkillSystem.GetSkillArray()[i])).Visible = true;
                        _manor.GetChildAt(SkillSystem.GetManorPiece(SkillSystem.GetSkillArray()[i])).Opacity = 1f;
                    }
                }
            }

            _thanksForPlayingText = new TextObj(Game.JunicodeLargeFont);
            _thanksForPlayingText.FontSize = 32f;
            _thanksForPlayingText.Align = Types.TextAlign.Centre;
            _thanksForPlayingText.Text = "Thanks for playing!";
            _thanksForPlayingText.DropShadow = new Vector2(2f, 2f);
            _thanksForPlayingText.Position = new Vector2(660f, 480f);
            _thanksForPlayingText.Opacity = 0f;
            _totalDeaths = _thanksForPlayingText.Clone() as TextObj;
            _totalDeaths.FontSize = 20f;
            _totalDeaths.Position = _thanksForPlayingText.Position;
            _totalDeaths.Y += 90f;
            _totalDeaths.Opacity = 0f;
            _totalPlayTime = _thanksForPlayingText.Clone() as TextObj;
            _totalPlayTime.FontSize = 20f;
            _totalPlayTime.Position = _totalDeaths.Position;
            _totalPlayTime.Y += 50f;
            _totalPlayTime.Opacity = 0f;
            _continueText = new KeyIconTextObj(Game.JunicodeFont);
            _continueText.FontSize = 14f;
            _continueText.Align = Types.TextAlign.Right;
            _continueText.Position = new Vector2(1270f, 650f);
            _continueText.ForceDraw = true;
            _continueText.Opacity = 0f;
            var num = 200;
            _glauber = new SpriteObj("Glauber_Sprite");
            _glauber.Scale = new Vector2(2f, 2f);
            _glauber.ForceDraw = true;
            _glauber.OutlineWidth = 2;
            _glauber.X = num;
            _teddy = new SpriteObj("Teddy_Sprite");
            _teddy.Scale = new Vector2(2f, 2f);
            _teddy.ForceDraw = true;
            _teddy.OutlineWidth = 2;
            _teddy.X = num;
            _kenny = new SpriteObj("Kenny_Sprite");
            _kenny.Scale = new Vector2(2f, 2f);
            _kenny.ForceDraw = true;
            _kenny.OutlineWidth = 2;
            _kenny.X = num;
            _gordon = new SpriteObj("Gordon_Sprite");
            _gordon.Scale = new Vector2(2f, 2f);
            _gordon.ForceDraw = true;
            _gordon.OutlineWidth = 2;
            _gordon.X = num;
            _judson = new SpriteObj("Judson_Sprite");
            _judson.Scale = new Vector2(2f, 2f);
            _judson.ForceDraw = true;
            _judson.OutlineWidth = 2;
            _judson.X = num;
            InitializeCredits();
            base.LoadContent();
        }

        public override void ReinitializeRTs()
        {
            _sky.ReinitializeRT(Camera);
            _skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            _backgroundRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            base.ReinitializeRTs();
        }

        private void InitializeCredits()
        {
            _creditsNameList = new List<TextObj>();
            _creditsTitleList = new List<TextObj>();
            _backgroundStrings = new[]
            {
                "Garden",
                "Tower",
                "Dungeon",
                "Outside",
                "Manor"
            };
            string[] array =
            {
                "Cellar Door Games",
                "Teddy Lee",
                "Kenny Lee",
                "Glauber Kotaki",
                "Gordon McGladdery (A Shell in the Pit)",
                "Judson Cowan (Tettix)",
                "Benny Lee",
                "Ryan & Michelle Lee",
                "Alessio Mellina",
                "John Won",
                "Charles Humphrey",
                "Jenny Lee",
                "Ethan \"flibitijibibo\" Lee",
                "David Gow",
                "Forrest Loomis",
                "Jorgen Tjerno",
                "Marcus Moller",
                "Matthias Niess",
                "Stanislaw Gackowski",
                "Stefano Angeleri",
                "",
                "",
                "Special Thanks",
                "Amber Campbell (Phedran)",
                "Blair Hurm Cowan",
                "Caitlin Groves",
                "Doug Culp",
                "Eric Lee Lewis",
                "Priscila Garcia",
                "Scott Barcik",
                "Tyler Mayes",
                "Our Moms & Dads",
                "",
                "",
                "Additional Thanks",
                "Jake Hirshey",
                "Joshua Hornsby",
                "Mark Wallace",
                "Peter Lee",
                "Sean Fleming",
                "",
                "",
                "Thanks to all our fans for their support!",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "Rogue Legacy Randomizer Developed by",
                "Zach \"Phar\" Parks",
                "",
                "",
                "Additional Randomizer Contributions",
                "",
                "Aaron \"alwaysintreble\" Wagener",
                "Alchav",
                "",
                "",
                "Randomizer Additional Thanks",
                "",
                "Travis \"Cainsith\" Ingle",
                "",
                "Dorian \"Gesellshcaft\"",
                "",
                "Fabian \"Berserker\" Dill",
                "",
                "Hussein \"ijwu\" Farran",
                "",
                "Jarno",
                "",
                "My Friends and Family <3",
                "",
                "The Love Labyrinth Community <3",
                "",
                "The Archipelago Community <3",
                "",
                "Cellar Door Games",
                "",
                "",
                "",
                "",
                "",
                "And you for playing! :3"
            };
            string[] array2 =
            {
                "Developed by",
                "Design & Story",
                "Programming & Production",
                "Art",
                "Music & Audio Design",
                "Music",
                "Marketing & Story",
                "Business Advisors",
                "Additional Audio Design",
                "Additional Background Art",
                "Crespuscular Ray Code From",
                "Super Special Thanks: Turbo Edition",
                "Mac/Linux Developer",
                "Mac/Linux QA Team"
            };
            var num = 0;
            for (var i = 0; i < array.Length; i++)
            {
                var textObj = new TextObj(Game.JunicodeFont);
                textObj.FontSize = 12f;
                textObj.DropShadow = new Vector2(2f, 2f);
                textObj.Align = Types.TextAlign.Centre;
                textObj.Position = new Vector2(660f, 720 + num);
                if (i < array2.Length)
                {
                    textObj.Text = array2[i];
                    _creditsTitleList.Add(textObj);
                    if (i < array2.Length - 1)
                    {
                        num += 200;
                    }
                    else
                    {
                        num += 40;
                    }
                }
                else
                {
                    num += 40;
                }

                var textObj2 = textObj.Clone() as TextObj;
                textObj2.Text = array[i];
                textObj2.FontSize = 16f;
                textObj2.Y += 40f;
                _creditsNameList.Add(textObj2);
                PositionTeam(array[i], new Vector2(textObj2.Bounds.Left - 50, textObj2.Y));
            }
        }

        private void PositionTeam(string name, Vector2 position)
        {
            if (name.Contains("Teddy"))
            {
                _teddy.Position = position;
                return;
            }

            if (name.Contains("Kenny"))
            {
                _kenny.Position = position;
                _kenny.X -= 50f;
                return;
            }

            if (name.Contains("Glauber"))
            {
                _glauber.Position = position;
                _glauber.X -= 20f;
                return;
            }

            if (name.Contains("Gordon"))
            {
                _gordon.Position = position;
                _gordon.X += 75f;
                return;
            }

            if (name.Contains("Judson"))
            {
                _judson.Position = position;
                _judson.X += 40f;
            }
        }

        public override void OnEnter()
        {
            _allowExit = false;
            var num = Game.PlayerStats.TotalHoursPlayed + Game.GetTotalGameTimeHours();
            var num2 = (int) ((num - (int) num) * 60f);
            Console.WriteLine(string.Concat("Hours played: ", num, " minutes: ", num2));
            _totalDeaths.Text = "Total Children: " + Game.PlayerStats.TimesDead;
            if (num2 < 10)
            {
                _totalPlayTime.Text = string.Concat("Time Played - ", (int) num, ":0", num2);
            }
            else
            {
                _totalPlayTime.Text = string.Concat("Time Played - ", (int) num, ":", num2);
            }

            Camera.Position = Vector2.Zero;
            _displayingContinueText = false;
            _continueText.Text = "Press [Input:" + 0 + "] to return to title.";
            if (_sky == null)
            {
                _sky = new SkyObj(null);
                _skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
                _sky.LoadContent(Camera);
                _backgroundRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            }

            SetPlayerStyle("Walking");
            if (!IsEnding)
            {
                SoundManager.PlayMusic("CreditsSong", true, 1f);
            }

            _scrollDistance = -(_creditsNameList[_creditsNameList.Count - 1].Y + 100f);
            foreach (var current in _creditsTitleList)
            {
                Tween.By(current, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            }

            foreach (var current2 in _creditsNameList)
            {
                Tween.By(current2, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            }

            Tween.By(_teddy, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_kenny, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_glauber, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_gordon, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_judson, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            if (!IsEnding)
            {
                _sideBorderLeft.X += 200f;
                _sideBorderRight.X -= 200f;
                Tween.RunFunction(76f, this, "ResetScroll");
            }

            base.OnEnter();
        }

        public void SetPlayerStyle(string animationType)
        {
            _playerSprite.ChangeSprite("Player" + animationType + "_Character");
            var player = (ScreenManager as RCScreenManager).Player;
            for (var i = 0; i < _playerSprite.NumChildren; i++)
            {
                _playerSprite.GetChildAt(i).TextureColor = player.GetChildAt(i).TextureColor;
                _playerSprite.GetChildAt(i).Visible = player.GetChildAt(i).Visible;
            }

            _playerSprite.GetChildAt(16).Visible = false;
            _playerSprite.Scale = player.Scale;
            if (Game.PlayerStats.Traits.X == 8f || Game.PlayerStats.Traits.Y == 8f)
            {
                _playerSprite.GetChildAt(7).Visible = false;
            }

            _playerSprite.GetChildAt(14).Visible = false;
            if (Game.PlayerStats.SpecialItem == 8)
            {
                _playerSprite.GetChildAt(14).Visible = true;
            }

            if (Game.PlayerStats.Class == 0 || Game.PlayerStats.Class == 8)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Shield_Sprite");
            }
            else if (Game.PlayerStats.Class == 5 || Game.PlayerStats.Class == 13)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Lamp_Sprite");
            }
            else if (Game.PlayerStats.Class == 1 || Game.PlayerStats.Class == 9)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Beard_Sprite");
            }
            else if (Game.PlayerStats.Class == 4 || Game.PlayerStats.Class == 12)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Headband_Sprite");
            }
            else if (Game.PlayerStats.Class == 2 || Game.PlayerStats.Class == 10)
            {
                _playerSprite.GetChildAt(15).Visible = true;
                _playerSprite.GetChildAt(15).ChangeSprite("Player" + animationType + "Horns_Sprite");
            }
            else
            {
                _playerSprite.GetChildAt(15).Visible = false;
            }

            _playerSprite.GetChildAt(0).Visible = false;
            if (Game.PlayerStats.Class == 16)
            {
                _playerSprite.GetChildAt(0).Visible = true;
                _playerSprite.GetChildAt(12)
                    .ChangeSprite(string.Concat("Player", animationType, "Head", 6, "_Sprite"));
            }

            if (Game.PlayerStats.Class == 17)
            {
                _playerSprite.GetChildAt(12)
                    .ChangeSprite(string.Concat("Player", animationType, "Head", 7, "_Sprite"));
            }

            if (!Game.PlayerStats.IsFemale)
            {
                _playerSprite.GetChildAt(5).Visible = false;
                _playerSprite.GetChildAt(13).Visible = false;
            }
            else
            {
                _playerSprite.GetChildAt(5).Visible = true;
                _playerSprite.GetChildAt(13).Visible = true;
            }

            if (Game.PlayerStats.Traits.X == 6f || Game.PlayerStats.Traits.Y == 6f)
            {
                _playerSprite.Scale = new Vector2(3f, 3f);
            }

            if (Game.PlayerStats.Traits.X == 7f || Game.PlayerStats.Traits.Y == 7f)
            {
                _playerSprite.Scale = new Vector2(1.35f, 1.35f);
            }

            if (Game.PlayerStats.Traits.X == 10f || Game.PlayerStats.Traits.Y == 10f)
            {
                _playerSprite.ScaleX *= 0.825f;
                _playerSprite.ScaleY *= 1.25f;
            }

            if (Game.PlayerStats.Traits.X == 9f || Game.PlayerStats.Traits.Y == 9f)
            {
                _playerSprite.ScaleX *= 1.25f;
                _playerSprite.ScaleY *= 1.175f;
            }

            if (Game.PlayerStats.Class == 6 || Game.PlayerStats.Class == 14)
            {
                _playerSprite.OutlineColour = Color.White;
                _playerSprite.GetChildAt(10).Visible = false;
                _playerSprite.GetChildAt(11).Visible = false;
            }
            else
            {
                _playerSprite.OutlineColour = Color.Black;
                _playerSprite.GetChildAt(10).Visible = true;
                _playerSprite.GetChildAt(11).Visible = true;
            }

            var text = (_playerSprite.GetChildAt(12) as IAnimateableObj).SpriteName;
            var startIndex = text.IndexOf("_") - 1;
            text = text.Remove(startIndex, 1);
            if (Game.PlayerStats.Class == 16)
            {
                text = text.Replace("_", 6 + "_");
            }
            else if (Game.PlayerStats.Class == 17)
            {
                text = text.Replace("_", 7 + "_");
            }
            else
            {
                text = text.Replace("_", Game.PlayerStats.HeadPiece + "_");
            }

            _playerSprite.GetChildAt(12).ChangeSprite(text);
            var text2 = (_playerSprite.GetChildAt(4) as IAnimateableObj).SpriteName;
            startIndex = text2.IndexOf("_") - 1;
            text2 = text2.Remove(startIndex, 1);
            text2 = text2.Replace("_", Game.PlayerStats.ChestPiece + "_");
            _playerSprite.GetChildAt(4).ChangeSprite(text2);
            var text3 = (_playerSprite.GetChildAt(9) as IAnimateableObj).SpriteName;
            startIndex = text3.IndexOf("_") - 1;
            text3 = text3.Remove(startIndex, 1);
            text3 = text3.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
            _playerSprite.GetChildAt(9).ChangeSprite(text3);
            var text4 = (_playerSprite.GetChildAt(3) as IAnimateableObj).SpriteName;
            startIndex = text4.IndexOf("_") - 1;
            text4 = text4.Remove(startIndex, 1);
            text4 = text4.Replace("_", Game.PlayerStats.ShoulderPiece + "_");
            _playerSprite.GetChildAt(3).ChangeSprite(text4);
            _playerSprite.PlayAnimation();
            _playerSprite.CalculateBounds();
            _playerSprite.Y = 435f - (_playerSprite.Bounds.Bottom - _playerSprite.Y);
        }

        public override void OnExit()
        {
            Tween.StopAllContaining(this, false);
            base.OnExit();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsEnding)
            {
                _sky.Update(gameTime);
                var num = (float) gameTime.ElapsedGameTime.TotalSeconds;
                UpdateBackground(num);
                if (_backgroundIndex < _backgroundStrings.Length)
                {
                    _backgroundSwapTimer += num;
                    if (_backgroundSwapTimer >= 75f / _backgroundStrings.Length)
                    {
                        SwapBackground(_backgroundStrings[_backgroundIndex]);
                        _backgroundIndex++;
                        _backgroundSwapTimer = 0f;
                    }
                }
            }

            base.Update(gameTime);
        }

        public void ResetScroll()
        {
            foreach (var current in _creditsTitleList)
            {
                current.Y += -_scrollDistance;
                Tween.By(current, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            }

            foreach (var current2 in _creditsNameList)
            {
                current2.Y += -_scrollDistance;
                Tween.By(current2, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
                PositionTeam(current2.Text, new Vector2(current2.Bounds.Left - 50, current2.Y));
            }

            Tween.By(_teddy, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_kenny, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_glauber, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_gordon, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.By(_judson, 75f, Tween.EaseNone, "Y", _scrollDistance.ToString());
            Tween.RunFunction(76f, this, "ResetScroll");
        }

        public override void HandleInput()
        {
            if ((!IsEnding || IsEnding && _allowExit) &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                 Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)))
            {
                if (_displayingContinueText)
                {
                    Tween.StopAll(false);
                    Program.Game.SaveOnExit();
                    ArchipelagoManager.Disconnect();
                    Game.ScreenManager.DisplayScreen(3, true);
                }
                else
                {
                    _displayingContinueText = true;
                    Tween.StopAllContaining(_continueText, false);
                    Tween.To(_continueText, 0.5f, Tween.EaseNone, "Opacity", "1");
                    Tween.RunFunction(4f, this, "HideContinueText");
                }
            }

            base.HandleInput();
        }

        public void HideContinueText()
        {
            _displayingContinueText = false;
            Tween.To(_continueText, 0.5f, Tween.EaseNone, "delay", "0", "Opacity", "0");
        }

        private void UpdateBackground(float elapsedTime)
        {
            var num = 200;
            _bg1.X += num * elapsedTime;
            _bg2.X += num * elapsedTime;
            _bg3.X += num * elapsedTime;
            if (_bg1.X > 930f)
            {
                _bg1.X = _bg3.X - _bg3.Width;
            }

            if (_bg2.X > 930f)
            {
                _bg2.X = _bg1.X - _bg1.Width;
            }

            if (_bg3.X > 930f)
            {
                _bg3.X = _bg2.X - _bg2.Width;
            }

            _ground1.X += num * elapsedTime;
            _ground2.X += num * elapsedTime;
            _ground3.X += num * elapsedTime;
            if (_ground1.X > 930f)
            {
                _ground1.X = _ground3.X - _ground3.Width;
            }

            if (_ground2.X > 930f)
            {
                _ground2.X = _ground1.X - _ground1.Width;
            }

            if (_ground3.X > 930f)
            {
                _ground3.X = _ground2.X - _ground2.Width;
            }

            _border1.X += num * elapsedTime;
            _border2.X += num * elapsedTime;
            _border3.X += num * elapsedTime;
            if (_border1.X > 930f)
            {
                _border1.X = _border3.X - _border3.Width;
            }

            if (_border2.X > 930f)
            {
                _border2.X = _border1.X - _border1.Width;
            }

            if (_border3.X > 930f)
            {
                _border3.X = _border2.X - _border2.Width;
            }

            _prop1.X += num * elapsedTime;
            _prop2.X += num * elapsedTime;
            _prop3.X += num * elapsedTime;
            if (_prop1.X > 930f)
            {
                _prop1.X -= CDGMath.RandomInt(1000, 3000);
            }

            if (_prop2.X > 930f)
            {
                _prop2.X -= CDGMath.RandomInt(1000, 3000);
            }

            if (_prop3.X > 930f)
            {
                _prop3.X -= CDGMath.RandomInt(1000, 3000);
            }
        }

        public void SwapBackground(string levelType)
        {
            Tween.By(_sideBorderLeft, 0.5f, Tween.EaseNone, "X", "200");
            Tween.By(_sideBorderRight, 0.5f, Tween.EaseNone, "X", "-200");
            Tween.AddEndHandlerToLastTween(this, "PerformSwap", levelType);
        }

        public void PerformSwap(string levelType)
        {
            _manor.Y = 0f;
            _bgOutside.Y = 0f;
            _bgOutside.Visible = false;
            _manor.Visible = false;
            _ground1.Visible = _ground2.Visible = _ground3.Visible = true;
            _border1.Visible = _border2.Visible = _border3.Visible = true;
            _prop1.Visible = _prop2.Visible = _prop3.Visible = true;
            if (levelType != null)
            {
                if (!(levelType == "Castle"))
                {
                    if (!(levelType == "Tower"))
                    {
                        if (!(levelType == "Dungeon"))
                        {
                            if (!(levelType == "Garden"))
                            {
                                if (!(levelType == "Outside"))
                                {
                                    if (levelType == "Manor")
                                    {
                                        _bgOutside.Visible = true;
                                        _manor.Visible = true;
                                        _ground1.Visible = _ground2.Visible = _ground3.Visible = false;
                                        _border1.Visible = _border2.Visible = _border3.Visible = false;
                                        _manor.Y = -260f;
                                        _bgOutside.Y = -260f;
                                        _manor.X -= 300f;
                                        _bgOutside.X -= 300f;
                                        _prop1.Visible = false;
                                        _prop2.Visible = false;
                                        _prop3.Visible = false;
                                        Tween.By(_manor, 3f, Tween.EaseNone, "X", "300");
                                        Tween.By(_bgOutside, 3f, Tween.EaseNone, "X", "300");
                                        Tween.By(_playerSprite, 3.5f, Tween.EaseNone, "X", "-150");
                                        Tween.AddEndHandlerToLastTween(this, "CreditsComplete");
                                        Tween.By(_sideBorderTop, 2.5f, Tween.EaseNone, "Y", "-500");
                                    }
                                }
                                else
                                {
                                    _bg1.ChangeSprite("GardenBG_Sprite");
                                    _bg2.ChangeSprite("GardenBG_Sprite");
                                    _bg3.ChangeSprite("GardenBG_Sprite");
                                    _ground1.ChangeSprite("GardenFG_Sprite");
                                    _ground2.ChangeSprite("GardenFG_Sprite");
                                    _ground3.ChangeSprite("GardenFG_Sprite");
                                    _border1.ChangeSprite("StartingRoomFloor_Sprite");
                                    _border2.ChangeSprite("StartingRoomFloor_Sprite");
                                    _border3.ChangeSprite("StartingRoomFloor_Sprite");
                                    _prop1.ChangeSprite("DungeonPrison1_Sprite");
                                    _prop1.Visible = false;
                                    _prop2.ChangeSprite("CreditsGrass_Character");
                                    _prop2.Y = 440f;
                                    _prop3.ChangeSprite("CreditsTree_Character");
                                    _bgOutside.Visible = true;
                                }
                            }
                            else
                            {
                                _bg1.ChangeSprite("GardenBG_Sprite");
                                _bg2.ChangeSprite("GardenBG_Sprite");
                                _bg3.ChangeSprite("GardenBG_Sprite");
                                _ground1.ChangeSprite("GardenFG_Sprite");
                                _ground2.ChangeSprite("GardenFG_Sprite");
                                _ground3.ChangeSprite("GardenFG_Sprite");
                                _border1.ChangeSprite("GardenBorder_Sprite");
                                _border2.ChangeSprite("GardenBorder_Sprite");
                                _border3.ChangeSprite("GardenBorder_Sprite");
                                _prop1.ChangeSprite("GardenFloatingRock3_Sprite");
                                _prop2.ChangeSprite("GardenFairy_Character");
                                _prop2.PlayAnimation();
                                _prop2.AnimationDelay = 0.1f;
                                _prop3.ChangeSprite("GardenLampPost1_Character");
                            }
                        }
                        else
                        {
                            _bg1.ChangeSprite("DungeonBG1_Sprite");
                            _bg2.ChangeSprite("DungeonBG1_Sprite");
                            _bg3.ChangeSprite("DungeonBG1_Sprite");
                            _ground1.ChangeSprite("DungeonFG1_Sprite");
                            _ground2.ChangeSprite("DungeonFG1_Sprite");
                            _ground3.ChangeSprite("DungeonFG1_Sprite");
                            _border1.ChangeSprite("DungeonBorder_Sprite");
                            _border2.ChangeSprite("DungeonBorder_Sprite");
                            _border3.ChangeSprite("DungeonBorder_Sprite");
                            _prop1.ChangeSprite("DungeonPrison1_Sprite");
                            _prop2.ChangeSprite("DungeonChain2_Character");
                            _prop3.ChangeSprite("DungeonTorch2_Character");
                        }
                    }
                    else
                    {
                        _bg1.ChangeSprite("TowerBG2_Sprite");
                        _bg2.ChangeSprite("TowerBG2_Sprite");
                        _bg3.ChangeSprite("TowerBG2_Sprite");
                        _ground1.ChangeSprite("TowerFG2_Sprite");
                        _ground2.ChangeSprite("TowerFG2_Sprite");
                        _ground3.ChangeSprite("TowerFG2_Sprite");
                        _border1.ChangeSprite("TowerBorder2_Sprite");
                        _border2.ChangeSprite("TowerBorder2_Sprite");
                        _border3.ChangeSprite("TowerBorder2_Sprite");
                        _prop1.ChangeSprite("TowerHole4_Sprite");
                        _prop2.Visible = false;
                        _prop3.ChangeSprite("TowerPedestal2_Character");
                    }
                }
                else
                {
                    _bg1.ChangeSprite("CastleBG1_Sprite");
                    _bg2.ChangeSprite("CastleBG1_Sprite");
                    _bg3.ChangeSprite("CastleBG1_Sprite");
                    _ground1.ChangeSprite("CastleFG1_Sprite");
                    _ground2.ChangeSprite("CastleFG1_Sprite");
                    _ground3.ChangeSprite("CastleFG1_Sprite");
                    _border1.ChangeSprite("CastleBorder_Sprite");
                    _border2.ChangeSprite("CastleBorder_Sprite");
                    _border3.ChangeSprite("CastleBorder_Sprite");
                    _prop1.ChangeSprite("CastleAssetWindow1_Sprite");
                    _prop2.ChangeSprite("CastleAssetBackTorch_Character");
                    _prop2.PlayAnimation();
                    _prop2.AnimationDelay = 0.1f;
                    _prop3.ChangeSprite("CastleAssetCandle1_Character");
                }
            }

            if (levelType != "Manor")
            {
                Tween.By(_sideBorderLeft, 0.5f, Tween.EaseNone, "X", "-200");
                Tween.By(_sideBorderRight, 0.5f, Tween.EaseNone, "X", "200");
                return;
            }

            Tween.By(_sideBorderLeft, 3f, Tween.EaseNone, "X", "-800");
            Tween.By(_sideBorderRight, 3f, Tween.EaseNone, "X", "800");
        }

        public void CreditsComplete()
        {
            SetPlayerStyle("Idle");
            Tween.RunFunction(0.5f, this, "SetPlayerStyle", "LevelUp");
            Tween.RunFunction(0.6f, _playerSprite, "PlayAnimation", false);
            Tween.To(_thanksForPlayingText, 2f, Tween.EaseNone, "Opacity", "1");
            Tween.To(_totalDeaths, 2f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
            Tween.To(_totalPlayTime, 2f, Tween.EaseNone, "delay", "0.4", "Opacity", "1");
            Tween.RunFunction(1f, this, "BringWife");
            Tween.RunFunction(1.1f, this, "BringChild1");
            Tween.RunFunction(3f, this, "BringChild2");
        }

        public void BringWife()
        {
            _wifeSprite.GetChildAt(14).Visible = false;
            _wifeSprite.GetChildAt(15).Visible = false;
            _wifeSprite.GetChildAt(16).Visible = false;
            _wifeSprite.GetChildAt(0).Visible = false;
            _wifeChest = CDGMath.RandomInt(1, 5);
            _wifeHead = CDGMath.RandomInt(1, 5);
            _wifeShoulders = CDGMath.RandomInt(1, 5);
            _wifeSprite.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + _wifeChest + "_Sprite");
            _wifeSprite.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + _wifeHead + "_Sprite");
            _wifeSprite.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + _wifeShoulders + "_Sprite");
            _wifeSprite.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + _wifeShoulders + "_Sprite");
            if (!Game.PlayerStats.IsFemale && Game.PlayerStats.Traits.X != 2f && Game.PlayerStats.Traits.Y != 2f ||
                Game.PlayerStats.IsFemale && (Game.PlayerStats.Traits.X == 2f || Game.PlayerStats.Traits.Y == 2f))
            {
                _wifeSprite.GetChildAt(13).Visible = true;
                _wifeSprite.GetChildAt(5).Visible = true;
            }
            else
            {
                _wifeSprite.GetChildAt(13).Visible = false;
                _wifeSprite.GetChildAt(5).Visible = false;
            }

            _wifeSprite.PlayAnimation();
            Tween.By(_wifeSprite, 3f, Tween.EaseNone, "X", "600");
            Tween.AddEndHandlerToLastTween(this, "LevelUpWife");
        }

        public void LevelUpWife()
        {
            _wifeSprite.ChangeSprite("PlayerLevelUp_Character");
            _wifeSprite.GetChildAt(14).Visible = false;
            _wifeSprite.GetChildAt(15).Visible = false;
            _wifeSprite.GetChildAt(16).Visible = false;
            _wifeSprite.GetChildAt(0).Visible = false;
            if (!Game.PlayerStats.IsFemale && Game.PlayerStats.Traits.X != 2f && Game.PlayerStats.Traits.Y != 2f ||
                Game.PlayerStats.IsFemale && (Game.PlayerStats.Traits.X == 2f || Game.PlayerStats.Traits.Y == 2f))
            {
                _wifeSprite.GetChildAt(13).Visible = true;
                _wifeSprite.GetChildAt(5).Visible = true;
            }
            else
            {
                _wifeSprite.GetChildAt(13).Visible = false;
                _wifeSprite.GetChildAt(5).Visible = false;
            }

            _wifeSprite.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + _wifeChest + "_Sprite");
            _wifeSprite.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + _wifeHead + "_Sprite");
            _wifeSprite.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + _wifeShoulders + "_Sprite");
            _wifeSprite.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + _wifeShoulders + "_Sprite");
            _wifeSprite.PlayAnimation(false);
        }

        public void BringChild1()
        {
            _childSprite1.GetChildAt(14).Visible = false;
            _childSprite1.GetChildAt(15).Visible = false;
            _childSprite1.GetChildAt(16).Visible = false;
            _childSprite1.GetChildAt(0).Visible = false;
            _child1Chest = CDGMath.RandomInt(1, 5);
            _child1Head = CDGMath.RandomInt(1, 5);
            _child1Shoulders = CDGMath.RandomInt(1, 5);

            var flag = CDGMath.RandomInt(0, 1) > 0;
            if (flag)
            {
                _childSprite1.GetChildAt(13).Visible = true;
                _childSprite1.GetChildAt(5).Visible = true;
            }
            else
            {
                _childSprite1.GetChildAt(13).Visible = false;
                _childSprite1.GetChildAt(5).Visible = false;
            }

            _childSprite1.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + _child1Chest + "_Sprite");
            _childSprite1.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + _child1Head + "_Sprite");
            _childSprite1.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + _child1Shoulders + "_Sprite");
            _childSprite1.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + _child1Shoulders + "_Sprite");
            _childSprite1.PlayAnimation();
            Tween.By(_childSprite1, 3f, Tween.EaseNone, "X", "600");
            Tween.AddEndHandlerToLastTween(this, "LevelUpChild1", flag);
        }

        public void LevelUpChild1(bool isFemale)
        {
            _childSprite1.ChangeSprite("PlayerLevelUp_Character");
            _childSprite1.GetChildAt(14).Visible = false;
            _childSprite1.GetChildAt(15).Visible = false;
            _childSprite1.GetChildAt(16).Visible = false;
            _childSprite1.GetChildAt(0).Visible = false;
            if (isFemale)
            {
                _childSprite1.GetChildAt(13).Visible = true;
                _childSprite1.GetChildAt(5).Visible = true;
            }
            else
            {
                _childSprite1.GetChildAt(13).Visible = false;
                _childSprite1.GetChildAt(5).Visible = false;
            }

            _childSprite1.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + _child1Chest + "_Sprite");
            _childSprite1.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + _child1Head + "_Sprite");
            _childSprite1.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + _child1Shoulders + "_Sprite");
            _childSprite1.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + _child1Shoulders + "_Sprite");
            _childSprite1.PlayAnimation(false);
            _allowExit = true;
            _displayingContinueText = true;
            Tween.StopAllContaining(_continueText, false);
            Tween.To(_continueText, 0.5f, Tween.EaseNone, "Opacity", "1");
        }

        public void BringChild2()
        {
            _childSprite2.GetChildAt(14).Visible = false;
            _childSprite2.GetChildAt(15).Visible = false;
            _childSprite2.GetChildAt(16).Visible = false;
            _childSprite2.GetChildAt(0).Visible = false;

            var flag = CDGMath.RandomInt(0, 1) > 0;
            if (flag)
            {
                _childSprite2.GetChildAt(13).Visible = true;
                _childSprite2.GetChildAt(5).Visible = true;
            }
            else
            {
                _childSprite2.GetChildAt(13).Visible = false;
                _childSprite2.GetChildAt(5).Visible = false;
            }

            _child2Chest = CDGMath.RandomInt(1, 5);
            _child2Head = CDGMath.RandomInt(1, 5);
            _child2Shoulders = CDGMath.RandomInt(1, 5);
            _childSprite2.GetChildAt(4).ChangeSprite("PlayerWalkingChest" + _child2Chest + "_Sprite");
            _childSprite2.GetChildAt(12).ChangeSprite("PlayerWalkingHead" + _child2Head + "_Sprite");
            _childSprite2.GetChildAt(9).ChangeSprite("PlayerWalkingShoulderA" + _child2Shoulders + "_Sprite");
            _childSprite2.GetChildAt(3).ChangeSprite("PlayerWalkingShoulderB" + _child2Shoulders + "_Sprite");
            _childSprite2.PlayAnimation();
            Tween.By(_childSprite2, 2f, Tween.EaseNone, "X", "600");
            Tween.AddEndHandlerToLastTween(this, "LevelUpChild2", flag);
        }

        public void LevelUpChild2(bool isFemale)
        {
            _childSprite2.ChangeSprite("PlayerLevelUp_Character");
            _childSprite2.GetChildAt(14).Visible = false;
            _childSprite2.GetChildAt(15).Visible = false;
            _childSprite2.GetChildAt(16).Visible = false;
            _childSprite2.GetChildAt(0).Visible = false;
            if (isFemale)
            {
                _childSprite2.GetChildAt(13).Visible = true;
                _childSprite2.GetChildAt(5).Visible = true;
            }
            else
            {
                _childSprite2.GetChildAt(13).Visible = false;
                _childSprite2.GetChildAt(5).Visible = false;
            }

            _childSprite2.GetChildAt(4).ChangeSprite("PlayerLevelUpChest" + _child2Chest + "_Sprite");
            _childSprite2.GetChildAt(12).ChangeSprite("PlayerLevelUpHead" + _child2Head + "_Sprite");
            _childSprite2.GetChildAt(9).ChangeSprite("PlayerLevelUpShoulderA" + _child2Shoulders + "_Sprite");
            _childSprite2.GetChildAt(3).ChangeSprite("PlayerLevelUpShoulderB" + _child2Shoulders + "_Sprite");
            _childSprite2.PlayAnimation(false);
            _displayingContinueText = true;
            Tween.StopAllContaining(_continueText, false);
            Tween.To(_continueText, 0.5f, Tween.EaseNone, "Opacity", "1");
        }

        public override void Draw(GameTime gametime)
        {
            Camera.GraphicsDevice.SetRenderTarget(_skyRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            _sky.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(_backgroundRenderTarget);
            Camera.GraphicsDevice.Clear(Color.Black);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            _bg1.Draw(Camera);
            _bg2.Draw(Camera);
            _bg3.Draw(Camera);
            _bgOutside.Draw(Camera);
            _ground1.Draw(Camera);
            _ground2.Draw(Camera);
            _ground3.Draw(Camera);
            _border1.Draw(Camera);
            _border2.Draw(Camera);
            _border3.Draw(Camera);
            _manor.Draw(Camera);
            _prop1.Draw(Camera);
            _prop2.Draw(Camera);
            _prop3.Draw(Camera);
            _playerSprite.Draw(Camera);
            Game.ColourSwapShader.Parameters["desiredTint"].SetValue(
                _playerSprite.GetChildAt(12).TextureColor.ToVector4());
            if (Game.PlayerStats.Class == 7 || Game.PlayerStats.Class == 15)
            {
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(_playerSprite.Opacity);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(_lichColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(_lichColour2.ToVector4());
            }
            else if (Game.PlayerStats.Class == 3 || Game.PlayerStats.Class == 11)
            {
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(_playerSprite.Opacity);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());
            }
            else
            {
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(1);
                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(_skinColour2.ToVector4());
            }

            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                Game.ColourSwapShader);
            _playerSprite.GetChildAt(12).Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            if (Game.PlayerStats.IsFemale)
            {
                _playerSprite.GetChildAt(13).Draw(Camera);
            }

            _playerSprite.GetChildAt(15).Draw(Camera);
            _wifeSprite.Draw(Camera);
            _childSprite1.Draw(Camera);
            _childSprite2.Draw(Camera);
            _sideBorderLeft.Draw(Camera);
            _sideBorderRight.Draw(Camera);
            _sideBorderTop.Draw(Camera);
            _sideBorderBottom.Draw(Camera);
            _teddy.Draw(Camera);
            _kenny.Draw(Camera);
            _glauber.Draw(Camera);
            _gordon.Draw(Camera);
            _judson.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            Camera.GraphicsDevice.Textures[1] = _skyRenderTarget;
            Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                Game.ParallaxEffect);
            Camera.Draw(_backgroundRenderTarget, Vector2.Zero, Color.White);
            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
            Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            var b = new Rectangle(0, 0, 1320, 720);
            foreach (var current in _creditsTitleList)
            {
                if (CollisionMath.Intersects(current.Bounds, b))
                {
                    current.Draw(Camera);
                }
            }

            foreach (var current2 in _creditsNameList)
            {
                if (CollisionMath.Intersects(current2.Bounds, b))
                {
                    current2.Draw(Camera);
                }
            }

            _thanksForPlayingText.Draw(Camera);
            _totalDeaths.Draw(Camera);
            _totalPlayTime.Draw(Camera);
            _continueText.Draw(Camera);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Credits Screen");
            Array.Clear(_backgroundStrings, 0, _backgroundStrings.Length);
            _backgroundStrings = null;
            _playerSprite.Dispose();
            _playerSprite = null;
            _wifeSprite.Dispose();
            _wifeSprite = null;
            _childSprite1.Dispose();
            _childSprite1 = null;
            _childSprite2.Dispose();
            _childSprite2 = null;
            _manor.Dispose();
            _manor = null;
            _thanksForPlayingText.Dispose();
            _thanksForPlayingText = null;
            _sideBorderRight.Dispose();
            _sideBorderRight = null;
            _sideBorderLeft.Dispose();
            _sideBorderLeft = null;
            _sideBorderTop.Dispose();
            _sideBorderTop = null;
            _sideBorderBottom.Dispose();
            _sideBorderBottom = null;
            _bgOutside.Dispose();
            _bgOutside = null;
            _sky.Dispose();
            _sky = null;
            _skyRenderTarget.Dispose();
            _skyRenderTarget = null;
            _backgroundRenderTarget.Dispose();
            _backgroundRenderTarget = null;
            foreach (var current in _creditsTitleList)
            {
                current.Dispose();
            }

            _creditsTitleList.Clear();
            _creditsTitleList = null;
            foreach (var current2 in _creditsNameList)
            {
                current2.Dispose();
            }

            _creditsNameList.Clear();
            _creditsNameList = null;
            _bg1.Dispose();
            _bg2.Dispose();
            _bg3.Dispose();
            _ground1.Dispose();
            _ground2.Dispose();
            _ground3.Dispose();
            _border1.Dispose();
            _border2.Dispose();
            _border3.Dispose();
            _prop1.Dispose();
            _prop2.Dispose();
            _prop3.Dispose();
            _prop1 = null;
            _prop2 = null;
            _prop3 = null;
            _bg1 = null;
            _bg2 = null;
            _bg3 = null;
            _ground1 = null;
            _ground2 = null;
            _ground3 = null;
            _border1 = null;
            _border2 = null;
            _border3 = null;
            _teddy.Dispose();
            _kenny.Dispose();
            _glauber.Dispose();
            _gordon.Dispose();
            _judson.Dispose();
            _teddy = null;
            _kenny = null;
            _glauber = null;
            _gordon = null;
            _judson = null;
            _continueText.Dispose();
            _continueText = null;
            _totalDeaths.Dispose();
            _totalDeaths = null;
            _totalPlayTime.Dispose();
            _totalPlayTime = null;
            base.Dispose();
        }
    }
}