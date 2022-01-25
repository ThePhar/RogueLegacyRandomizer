// 
//  Rogue Legacy Randomizer - DialogueScreen.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Reflection;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens
{
    public class DialogueScreen : Screen
    {
        private const float TEXT_SCROLL_SPEED = 0.03f;

        private object[]     _cancelArgs;
        private MethodInfo   _cancelMethodInfo;
        private object       _cancelMethodObj;
        private object[]     _confirmArgs;
        private MethodInfo   _confirmMethodInfo;
        private object       _confirmMethodObj;
        private ObjContainer _dialogChoiceContainer;
        private ObjContainer _dialogContainer;
        private float        _dialogContinueIconY;
        private byte         _dialogCounter;
        private string[]     _dialogText;
        private string[]     _dialogTitles;
        private string       _dialogueObjName;
        private byte         _highlightedChoice = 2;
        private float        _inputDelayTimer;
        private bool         _lockControls;
        private bool         _runCancelEndHandler;
        private bool         _runChoiceDialogue;

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.FontSize = 12f;
            textObj.Align = Types.TextAlign.Left;
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.FontSize = 14f;
            textObj2.Text = "Blacksmith";
            textObj2.DropShadow = new Vector2(2f, 2f);
            textObj2.TextureColor = new Color(236, 197, 132);
            _dialogContainer = new ObjContainer("DialogBox_Character");
            _dialogContainer.Position = new Vector2(660f, 100f);
            _dialogContainer.AddChild(textObj2);
            _dialogContainer.ForceDraw = true;
            textObj2.Position = new Vector2(-(float) _dialogContainer.Width / 2.2f,
                -(float) _dialogContainer.Height / 1.6f);
            _dialogContainer.AddChild(textObj);
            textObj.Position = new Vector2(-(float) _dialogContainer.Width / 2.15f,
                -(float) _dialogContainer.Height / 3.5f);
            textObj.Text =
                "This is a test to see how much text I can fit onto this dialog box without it running out of space. The text needs to be defined after the dialog text position is set, because the dialogtext width affects the entire width of the dialog container, which in END.";
            textObj.WordWrap(850);
            textObj.DropShadow = new Vector2(2f, 3f);
            var spriteObj = new SpriteObj("ContinueTextIcon_Sprite");
            spriteObj.Position = new Vector2(_dialogContainer.GetChildAt(2).Bounds.Right,
                _dialogContainer.GetChildAt(2).Bounds.Bottom);
            _dialogContainer.AddChild(spriteObj);
            _dialogContinueIconY = spriteObj.Y;
            var textObj3 = new TextObj(Game.JunicodeFont);
            textObj3.FontSize = 12f;
            textObj3.Text = "Yes";
            textObj3.Align = Types.TextAlign.Centre;
            var textObj4 = new TextObj(Game.JunicodeFont);
            textObj4.FontSize = 12f;
            textObj4.Text = "No";
            textObj4.Align = Types.TextAlign.Centre;
            _dialogChoiceContainer = new ObjContainer();
            var obj = new SpriteObj("GameOverStatPlate_Sprite");
            _dialogChoiceContainer.AddChild(obj);
            var spriteObj2 = new SpriteObj("DialogueChoiceHighlight_Sprite");
            _dialogChoiceContainer.AddChild(spriteObj2);
            _dialogChoiceContainer.ForceDraw = true;
            _dialogChoiceContainer.Position = new Vector2(660f, 360f);
            _dialogChoiceContainer.AddChild(textObj3);
            textObj3.Y -= 40f;
            _dialogChoiceContainer.AddChild(textObj4);
            textObj4.Y += 7f;
            spriteObj2.Position = new Vector2(textObj3.X, textObj3.Y + spriteObj2.Height / 2 + 3f);
            _dialogChoiceContainer.Scale = Vector2.Zero;
            _dialogChoiceContainer.Visible = false;
            base.LoadContent();
        }

        public void SetDialogue(string dialogueObjName)
        {
            _dialogueObjName = dialogueObjName;
            _confirmMethodObj = null;
            _confirmMethodInfo = null;
            if (_confirmArgs != null)
            {
                Array.Clear(_confirmArgs, 0, _confirmArgs.Length);
            }

            _cancelMethodObj = null;
            _cancelMethodInfo = null;
            if (_cancelArgs != null)
            {
                Array.Clear(_cancelArgs, 0, _cancelArgs.Length);
            }
        }

        public void SetConfirmEndHandler(object methodObject, string functionName, params object[] args)
        {
            _confirmMethodObj = methodObject;
            _confirmMethodInfo = methodObject.GetType().GetMethod(functionName);
            _confirmArgs = args;
        }

        public void SetConfirmEndHandler(Type methodType, string functionName, params object[] args)
        {
            var array = new Type[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                array[i] = args[i].GetType();
            }

            _confirmMethodInfo = methodType.GetMethod(functionName, array);
            _confirmArgs = args;
            if (_confirmMethodInfo == null)
            {
                _confirmMethodInfo = methodType.GetMethod(functionName, new[]
                {
                    args[0].GetType().MakeArrayType()
                });
                _confirmArgs = new object[1];
                _confirmArgs[0] = args;
            }

            _confirmMethodObj = null;
        }

        public void SetCancelEndHandler(object methodObject, string functionName, params object[] args)
        {
            _cancelMethodObj = methodObject;
            _cancelMethodInfo = methodObject.GetType().GetMethod(functionName);
            _cancelArgs = args;
        }

        public void SetCancelEndHandler(Type methodType, string functionName, params object[] args)
        {
            var array = new Type[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                array[i] = args[i].GetType();
            }

            _cancelMethodInfo = methodType.GetMethod(functionName, array);
            _cancelArgs = args;
            if (_cancelMethodInfo == null)
            {
                _cancelMethodInfo = methodType.GetMethod(functionName, new[]
                {
                    args[0].GetType().MakeArrayType()
                });
                _cancelArgs = new object[1];
                _cancelArgs[0] = args;
            }

            _cancelMethodObj = null;
        }

        public void SetDialogueChoice(string dialogueObjName)
        {
            var text = DialogueManager.GetText(dialogueObjName);
            (_dialogChoiceContainer.GetChildAt(2) as TextObj).Text = text.Speakers[0];
            (_dialogChoiceContainer.GetChildAt(3) as TextObj).Text = text.Dialogue[0];
            if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
            {
                (_dialogChoiceContainer.GetChildAt(2) as TextObj).RandomizeSentence(false);
                (_dialogChoiceContainer.GetChildAt(3) as TextObj).RandomizeSentence(false);
            }

            _runChoiceDialogue = true;
        }

        public override void HandleInput()
        {
            if (!_lockControls && _inputDelayTimer <= 0f)
            {
                if (!_dialogChoiceContainer.Visible)
                {
                    if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                        Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                    {
                        if (_dialogCounter < _dialogText.Length - 1)
                        {
                            var textObj = _dialogContainer.GetChildAt(2) as TextObj;
                            if (!textObj.IsTypewriting)
                            {
                                _dialogCounter += 1;
                                (_dialogContainer.GetChildAt(1) as TextObj).Text = _dialogTitles[_dialogCounter];
                                textObj.Text = _dialogText[_dialogCounter];
                                if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
                                {
                                    textObj.RandomizeSentence(false);
                                    (_dialogContainer.GetChildAt(1) as TextObj).RandomizeSentence(false);
                                }

                                textObj.WordWrap(850);
                                textObj.BeginTypeWriting(_dialogText[_dialogCounter].Length * TEXT_SCROLL_SPEED,
                                    "dialogue_tap");
                            }
                            else
                            {
                                textObj.StopTypeWriting(true);
                            }
                        }
                        else if (!_runChoiceDialogue && !(_dialogContainer.GetChildAt(2) as TextObj).IsTypewriting)
                        {
                            _lockControls = true;
                            SoundManager.PlaySound("DialogMenuClose");
                            Tween.To(_dialogContainer, 0.3f, Quad.EaseIn, "Opacity", "0", "Y", "0");
                            Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0");
                            Tween.AddEndHandlerToLastTween(this, "ExitScreen");
                        }
                        else
                        {
                            (_dialogContainer.GetChildAt(2) as TextObj).StopTypeWriting(true);
                        }

                        var spriteObj = _dialogContainer.GetChildAt(3) as SpriteObj;
                        if (_dialogCounter == _dialogText.Length - 1)
                        {
                            spriteObj.ChangeSprite("EndTextIcon_Sprite");
                            if (_runChoiceDialogue)
                            {
                                var textObj2 = _dialogContainer.GetChildAt(2) as TextObj;
                                textObj2.StopTypeWriting(true);
                                _dialogChoiceContainer.Visible = true;
                                Tween.To(_dialogChoiceContainer, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
                                SoundManager.PlaySound("DialogOpenBump");
                            }
                        }
                        else
                        {
                            spriteObj.ChangeSprite("ContinueTextIcon_Sprite");
                        }
                    }
                }
                else
                {
                    if (Game.GlobalInput.JustPressed(18) || Game.GlobalInput.JustPressed(19))
                    {
                        SoundManager.PlaySound("frame_swap");
                        _highlightedChoice += 1;
                        if (_highlightedChoice > 3)
                        {
                            _highlightedChoice = 2;
                        }

                        _dialogChoiceContainer.GetChildAt(1).Y =
                            _dialogChoiceContainer.GetChildAt(_highlightedChoice).Y +
                            _dialogChoiceContainer.GetChildAt(1).Height / 2 + 3f;
                    }
                    else if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                    {
                        SoundManager.PlaySound("frame_swap");
                        _highlightedChoice -= 1;
                        if (_highlightedChoice < 2)
                        {
                            _highlightedChoice = 3;
                        }

                        _dialogChoiceContainer.GetChildAt(1).Y =
                            _dialogChoiceContainer.GetChildAt(_highlightedChoice).Y +
                            _dialogChoiceContainer.GetChildAt(1).Height / 2 + 3f;
                    }

                    if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                    {
                        _runCancelEndHandler = false;
                        if (_highlightedChoice == 3)
                        {
                            _runCancelEndHandler = true;
                            SoundManager.PlaySound("DialogueMenuCancel");
                        }
                        else
                        {
                            SoundManager.PlaySound("DialogueMenuConfirm");
                        }

                        _lockControls = true;
                        SoundManager.PlaySound("DialogMenuClose");
                        Tween.To(_dialogContainer, 0.3f, Quad.EaseInOut, "Opacity", "0", "Y", "100");
                        Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0");
                        Tween.To(_dialogChoiceContainer, 0.3f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
                        Tween.AddEndHandlerToLastTween(this, "ExitScreen");
                    }
                    else if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                    {
                        _highlightedChoice = 3;
                        _dialogChoiceContainer.GetChildAt(1).Y =
                            _dialogChoiceContainer.GetChildAt(_highlightedChoice).Y +
                            _dialogChoiceContainer.GetChildAt(1).Height / 2 + 3f;
                        _runCancelEndHandler = true;
                        SoundManager.PlaySound("DialogueMenuCancel");
                        _lockControls = true;
                        SoundManager.PlaySound("DialogMenuClose");
                        Tween.To(_dialogContainer, 0.3f, Quad.EaseInOut, "Opacity", "0", "Y", "100");
                        Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0");
                        Tween.To(_dialogChoiceContainer, 0.3f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
                        Tween.AddEndHandlerToLastTween(this, "ExitScreen");
                    }
                }
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            if (_inputDelayTimer > 0f)
            {
                _inputDelayTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (!_dialogChoiceContainer.Visible && _dialogCounter == _dialogText.Length - 1 && _runChoiceDialogue)
            {
                var spriteObj = _dialogContainer.GetChildAt(3) as SpriteObj;
                spriteObj.ChangeSprite("EndTextIcon_Sprite");
                if (_runChoiceDialogue)
                {
                    var textObj = _dialogContainer.GetChildAt(2) as TextObj;
                    textObj.StopTypeWriting(true);
                    _dialogChoiceContainer.Visible = true;
                    Tween.To(_dialogChoiceContainer, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
                    Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "DialogOpenBump");
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
            _dialogContainer.Draw(Camera);
            if (_dialogContainer.ScaleX > 0f)
            {
                _dialogContainer.GetChildAt(3).Y =
                    _dialogContinueIconY + (float) Math.Sin(Game.TotalGameTimeSeconds * 20f) * 2f;
            }

            _dialogChoiceContainer.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void OnEnter()
        {
            _inputDelayTimer = 0.5f;
            SoundManager.PlaySound("DialogOpen");
            _lockControls = false;
            _runCancelEndHandler = false;
            _highlightedChoice = 2;
            _dialogChoiceContainer.Scale = new Vector2(1f, 1f);
            _dialogChoiceContainer.GetChildAt(1).Y = _dialogChoiceContainer.GetChildAt(_highlightedChoice).Y +
                                                      _dialogChoiceContainer.GetChildAt(1).Height / 2 + 3f;
            _dialogChoiceContainer.Scale = Vector2.Zero;
            var text = DialogueManager.GetText(_dialogueObjName);
            var speakers = text.Speakers;
            var dialogue = text.Dialogue;
            var spriteObj = _dialogContainer.GetChildAt(3) as SpriteObj;
            if (dialogue.Length > 1)
            {
                spriteObj.ChangeSprite("ContinueTextIcon_Sprite");
            }
            else
            {
                spriteObj.ChangeSprite("EndTextIcon_Sprite");
            }

            _dialogCounter = 0;
            _dialogTitles = speakers;
            _dialogText = dialogue;
            _dialogContainer.Scale = Vector2.One;
            _dialogContainer.Opacity = 0f;
            (_dialogContainer.GetChildAt(2) as TextObj).Text = dialogue[_dialogCounter] ?? "";
            (_dialogContainer.GetChildAt(2) as TextObj).WordWrap(850);
            (_dialogContainer.GetChildAt(1) as TextObj).Text = speakers[_dialogCounter] ?? "";
            if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
            {
                (_dialogContainer.GetChildAt(2) as TextObj).RandomizeSentence(false);
                (_dialogContainer.GetChildAt(1) as TextObj).RandomizeSentence(false);
            }

            (_dialogContainer.GetChildAt(2) as TextObj).BeginTypeWriting(
                dialogue[_dialogCounter].Length * TEXT_SCROLL_SPEED, "dialogue_tap");
            Tween.To(_dialogContainer, 0.3f, Quad.EaseInOut, "Opacity", "1", "Y", "150");
            Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            base.OnEnter();
        }

        public void ExitScreen()
        {
            (ScreenManager as RCScreenManager).HideCurrentScreen();
            _runChoiceDialogue = false;
            _dialogChoiceContainer.Visible = false;
            _dialogChoiceContainer.Scale = Vector2.Zero;
            if (!_runCancelEndHandler)
            {
                if (_confirmMethodInfo != null)
                {
                    _confirmMethodInfo.Invoke(_confirmMethodObj, _confirmArgs);
                }
            }
            else if (_cancelMethodInfo != null)
            {
                _cancelMethodInfo.Invoke(_cancelMethodObj, _cancelArgs);
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Dialogue Screen");
            _confirmMethodObj = null;
            _confirmMethodInfo = null;
            if (_confirmArgs != null)
            {
                Array.Clear(_confirmArgs, 0, _confirmArgs.Length);
            }

            _confirmArgs = null;
            _cancelMethodObj = null;
            _cancelMethodInfo = null;
            if (_cancelArgs != null)
            {
                Array.Clear(_cancelArgs, 0, _cancelArgs.Length);
            }

            _cancelArgs = null;
            _dialogContainer.Dispose();
            _dialogContainer = null;
            _dialogChoiceContainer.Dispose();
            _dialogChoiceContainer = null;
            if (_dialogText != null)
            {
                Array.Clear(_dialogText, 0, _dialogText.Length);
            }

            _dialogText = null;
            if (_dialogTitles != null)
            {
                Array.Clear(_dialogTitles, 0, _dialogTitles.Length);
            }

            _dialogTitles = null;
            base.Dispose();
        }
    }
}
