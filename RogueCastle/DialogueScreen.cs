// 
// RogueLegacyArchipelago - DialogueScreen.cs
// Last Modified 2021-12-28
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Reflection;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class DialogueScreen : Screen
    {
        private readonly float m_textScrollSpeed = 0.03f;
        private object[] m_cancelArgs;
        private MethodInfo m_cancelMethodInfo;
        private object m_cancelMethodObj;
        private object[] m_confirmArgs;
        private MethodInfo m_confirmMethodInfo;
        private object m_confirmMethodObj;
        private ObjContainer m_dialogChoiceContainer;
        private ObjContainer m_dialogContainer;
        private float m_dialogContinueIconY;
        private byte m_dialogCounter;
        private string[] m_dialogText;
        private string[] m_dialogTitles;
        private string m_dialogueObjName;
        private byte m_highlightedChoice = 2;
        private float m_inputDelayTimer;
        private bool m_lockControls;
        private bool m_runCancelEndHandler;
        private bool m_runChoiceDialogue;
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
            m_dialogContainer = new ObjContainer("DialogBox_Character");
            m_dialogContainer.Position = new Vector2(660f, 100f);
            m_dialogContainer.AddChild(textObj2);
            m_dialogContainer.ForceDraw = true;
            textObj2.Position = new Vector2(-(float) m_dialogContainer.Width/2.2f,
                -(float) m_dialogContainer.Height/1.6f);
            m_dialogContainer.AddChild(textObj);
            textObj.Position = new Vector2(-(float) m_dialogContainer.Width/2.15f,
                -(float) m_dialogContainer.Height/3.5f);
            textObj.Text =
                "This is a test to see how much text I can fit onto this dialog box without it running out of space. The text needs to be defined after the dialog text position is set, because the dialogtext width affects the entire width of the dialog container, which in END.";
            textObj.WordWrap(850);
            textObj.DropShadow = new Vector2(2f, 3f);
            var spriteObj = new SpriteObj("ContinueTextIcon_Sprite");
            spriteObj.Position = new Vector2(m_dialogContainer.GetChildAt(2).Bounds.Right,
                m_dialogContainer.GetChildAt(2).Bounds.Bottom);
            m_dialogContainer.AddChild(spriteObj);
            m_dialogContinueIconY = spriteObj.Y;
            var textObj3 = new TextObj(Game.JunicodeFont);
            textObj3.FontSize = 12f;
            textObj3.Text = "Yes";
            textObj3.Align = Types.TextAlign.Centre;
            var textObj4 = new TextObj(Game.JunicodeFont);
            textObj4.FontSize = 12f;
            textObj4.Text = "No";
            textObj4.Align = Types.TextAlign.Centre;
            m_dialogChoiceContainer = new ObjContainer();
            var obj = new SpriteObj("GameOverStatPlate_Sprite");
            m_dialogChoiceContainer.AddChild(obj);
            var spriteObj2 = new SpriteObj("DialogueChoiceHighlight_Sprite");
            m_dialogChoiceContainer.AddChild(spriteObj2);
            m_dialogChoiceContainer.ForceDraw = true;
            m_dialogChoiceContainer.Position = new Vector2(660f, 360f);
            m_dialogChoiceContainer.AddChild(textObj3);
            textObj3.Y -= 40f;
            m_dialogChoiceContainer.AddChild(textObj4);
            textObj4.Y += 7f;
            spriteObj2.Position = new Vector2(textObj3.X, textObj3.Y + spriteObj2.Height/2 + 3f);
            m_dialogChoiceContainer.Scale = Vector2.Zero;
            m_dialogChoiceContainer.Visible = false;
            base.LoadContent();
        }

        public void SetDialogue(string dialogueObjName)
        {
            m_dialogueObjName = dialogueObjName;
            m_confirmMethodObj = null;
            m_confirmMethodInfo = null;
            if (m_confirmArgs != null)
            {
                Array.Clear(m_confirmArgs, 0, m_confirmArgs.Length);
            }
            m_cancelMethodObj = null;
            m_cancelMethodInfo = null;
            if (m_cancelArgs != null)
            {
                Array.Clear(m_cancelArgs, 0, m_cancelArgs.Length);
            }
        }

        public void SetConfirmEndHandler(object methodObject, string functionName, params object[] args)
        {
            m_confirmMethodObj = methodObject;
            m_confirmMethodInfo = methodObject.GetType().GetMethod(functionName);
            m_confirmArgs = args;
        }

        public void SetConfirmEndHandler(Type methodType, string functionName, params object[] args)
        {
            var array = new Type[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                array[i] = args[i].GetType();
            }
            m_confirmMethodInfo = methodType.GetMethod(functionName, array);
            m_confirmArgs = args;
            if (m_confirmMethodInfo == null)
            {
                m_confirmMethodInfo = methodType.GetMethod(functionName, new[]
                {
                    args[0].GetType().MakeArrayType()
                });
                m_confirmArgs = new object[1];
                m_confirmArgs[0] = args;
            }
            m_confirmMethodObj = null;
        }

        public void SetCancelEndHandler(object methodObject, string functionName, params object[] args)
        {
            m_cancelMethodObj = methodObject;
            m_cancelMethodInfo = methodObject.GetType().GetMethod(functionName);
            m_cancelArgs = args;
        }

        public void SetCancelEndHandler(Type methodType, string functionName, params object[] args)
        {
            var array = new Type[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                array[i] = args[i].GetType();
            }
            m_cancelMethodInfo = methodType.GetMethod(functionName, array);
            m_cancelArgs = args;
            if (m_cancelMethodInfo == null)
            {
                m_cancelMethodInfo = methodType.GetMethod(functionName, new[]
                {
                    args[0].GetType().MakeArrayType()
                });
                m_cancelArgs = new object[1];
                m_cancelArgs[0] = args;
            }
            m_cancelMethodObj = null;
        }

        public void SetDialogueChoice(string dialogueObjName)
        {
            var text = DialogueManager.GetText(dialogueObjName);
            (m_dialogChoiceContainer.GetChildAt(2) as TextObj).Text = text.Speakers[0];
            (m_dialogChoiceContainer.GetChildAt(3) as TextObj).Text = text.Dialogue[0];
            if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
            {
                (m_dialogChoiceContainer.GetChildAt(2) as TextObj).RandomizeSentence(false);
                (m_dialogChoiceContainer.GetChildAt(3) as TextObj).RandomizeSentence(false);
            }
            m_runChoiceDialogue = true;
        }

        public override void HandleInput()
        {
            if (!m_lockControls && m_inputDelayTimer <= 0f)
            {
                if (!m_dialogChoiceContainer.Visible)
                {
                    if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                        Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                    {
                        if (m_dialogCounter < m_dialogText.Length - 1)
                        {
                            var textObj = m_dialogContainer.GetChildAt(2) as TextObj;
                            if (!textObj.IsTypewriting)
                            {
                                m_dialogCounter += 1;
                                (m_dialogContainer.GetChildAt(1) as TextObj).Text = m_dialogTitles[m_dialogCounter];
                                textObj.Text = m_dialogText[m_dialogCounter];
                                if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
                                {
                                    textObj.RandomizeSentence(false);
                                    (m_dialogContainer.GetChildAt(1) as TextObj).RandomizeSentence(false);
                                }
                                textObj.WordWrap(850);
                                textObj.BeginTypeWriting(m_dialogText[m_dialogCounter].Length*m_textScrollSpeed,
                                    "dialogue_tap");
                            }
                            else
                            {
                                textObj.StopTypeWriting(true);
                            }
                        }
                        else if (!m_runChoiceDialogue && !(m_dialogContainer.GetChildAt(2) as TextObj).IsTypewriting)
                        {
                            m_lockControls = true;
                            SoundManager.PlaySound("DialogMenuClose");
                            Tween.To(m_dialogContainer, 0.3f, Quad.EaseIn, "Opacity", "0", "Y", "0");
                            Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0");
                            Tween.AddEndHandlerToLastTween(this, "ExitScreen");
                        }
                        else
                        {
                            (m_dialogContainer.GetChildAt(2) as TextObj).StopTypeWriting(true);
                        }
                        var spriteObj = m_dialogContainer.GetChildAt(3) as SpriteObj;
                        if (m_dialogCounter == m_dialogText.Length - 1)
                        {
                            spriteObj.ChangeSprite("EndTextIcon_Sprite");
                            if (m_runChoiceDialogue)
                            {
                                var textObj2 = m_dialogContainer.GetChildAt(2) as TextObj;
                                textObj2.StopTypeWriting(true);
                                m_dialogChoiceContainer.Visible = true;
                                Tween.To(m_dialogChoiceContainer, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
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
                        m_highlightedChoice += 1;
                        if (m_highlightedChoice > 3)
                        {
                            m_highlightedChoice = 2;
                        }
                        m_dialogChoiceContainer.GetChildAt(1).Y =
                            m_dialogChoiceContainer.GetChildAt(m_highlightedChoice).Y +
                            m_dialogChoiceContainer.GetChildAt(1).Height/2 + 3f;
                    }
                    else if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
                    {
                        SoundManager.PlaySound("frame_swap");
                        m_highlightedChoice -= 1;
                        if (m_highlightedChoice < 2)
                        {
                            m_highlightedChoice = 3;
                        }
                        m_dialogChoiceContainer.GetChildAt(1).Y =
                            m_dialogChoiceContainer.GetChildAt(m_highlightedChoice).Y +
                            m_dialogChoiceContainer.GetChildAt(1).Height/2 + 3f;
                    }
                    if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
                    {
                        m_runCancelEndHandler = false;
                        if (m_highlightedChoice == 3)
                        {
                            m_runCancelEndHandler = true;
                            SoundManager.PlaySound("DialogueMenuCancel");
                        }
                        else
                        {
                            SoundManager.PlaySound("DialogueMenuConfirm");
                        }
                        m_lockControls = true;
                        SoundManager.PlaySound("DialogMenuClose");
                        Tween.To(m_dialogContainer, 0.3f, Quad.EaseInOut, "Opacity", "0", "Y", "100");
                        Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0");
                        Tween.To(m_dialogChoiceContainer, 0.3f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
                        Tween.AddEndHandlerToLastTween(this, "ExitScreen");
                    }
                    else if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
                    {
                        m_highlightedChoice = 3;
                        m_dialogChoiceContainer.GetChildAt(1).Y =
                            m_dialogChoiceContainer.GetChildAt(m_highlightedChoice).Y +
                            m_dialogChoiceContainer.GetChildAt(1).Height/2 + 3f;
                        m_runCancelEndHandler = true;
                        SoundManager.PlaySound("DialogueMenuCancel");
                        m_lockControls = true;
                        SoundManager.PlaySound("DialogMenuClose");
                        Tween.To(m_dialogContainer, 0.3f, Quad.EaseInOut, "Opacity", "0", "Y", "100");
                        Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0");
                        Tween.To(m_dialogChoiceContainer, 0.3f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
                        Tween.AddEndHandlerToLastTween(this, "ExitScreen");
                    }
                }
            }
            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            if (m_inputDelayTimer > 0f)
            {
                m_inputDelayTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (!m_dialogChoiceContainer.Visible && m_dialogCounter == m_dialogText.Length - 1 && m_runChoiceDialogue)
            {
                var spriteObj = m_dialogContainer.GetChildAt(3) as SpriteObj;
                spriteObj.ChangeSprite("EndTextIcon_Sprite");
                if (m_runChoiceDialogue)
                {
                    var textObj = m_dialogContainer.GetChildAt(2) as TextObj;
                    textObj.StopTypeWriting(true);
                    m_dialogChoiceContainer.Visible = true;
                    Tween.To(m_dialogChoiceContainer, 0.3f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
                    Tween.RunFunction(0.1f, typeof (SoundManager), "PlaySound", "DialogOpenBump");
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black*BackBufferOpacity);
            m_dialogContainer.Draw(Camera);
            if (m_dialogContainer.ScaleX > 0f)
            {
                m_dialogContainer.GetChildAt(3).Y = m_dialogContinueIconY + (float) Math.Sin(Game.TotalGameTimeSeconds*20f)*2f;
            }
            m_dialogChoiceContainer.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void OnEnter()
        {
            m_inputDelayTimer = 0.5f;
            SoundManager.PlaySound("DialogOpen");
            m_lockControls = false;
            m_runCancelEndHandler = false;
            m_highlightedChoice = 2;
            m_dialogChoiceContainer.Scale = new Vector2(1f, 1f);
            m_dialogChoiceContainer.GetChildAt(1).Y = m_dialogChoiceContainer.GetChildAt(m_highlightedChoice).Y +
                                                      m_dialogChoiceContainer.GetChildAt(1).Height/2 + 3f;
            m_dialogChoiceContainer.Scale = Vector2.Zero;
            var text = DialogueManager.GetText(m_dialogueObjName);
            var speakers = text.Speakers;
            var dialogue = text.Dialogue;
            var spriteObj = m_dialogContainer.GetChildAt(3) as SpriteObj;
            if (dialogue.Length > 1)
            {
                spriteObj.ChangeSprite("ContinueTextIcon_Sprite");
            }
            else
            {
                spriteObj.ChangeSprite("EndTextIcon_Sprite");
            }
            m_dialogCounter = 0;
            m_dialogTitles = speakers;
            m_dialogText = dialogue;
            m_dialogContainer.Scale = Vector2.One;
            m_dialogContainer.Opacity = 0f;
            (m_dialogContainer.GetChildAt(2) as TextObj).Text = dialogue[m_dialogCounter] ?? "";
            (m_dialogContainer.GetChildAt(2) as TextObj).WordWrap(850);
            (m_dialogContainer.GetChildAt(1) as TextObj).Text = speakers[m_dialogCounter] ?? "";
            if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
            {
                (m_dialogContainer.GetChildAt(2) as TextObj).RandomizeSentence(false);
                (m_dialogContainer.GetChildAt(1) as TextObj).RandomizeSentence(false);
            }
            (m_dialogContainer.GetChildAt(2) as TextObj).BeginTypeWriting(
                dialogue[m_dialogCounter].Length*m_textScrollSpeed, "dialogue_tap");
            Tween.To(m_dialogContainer, 0.3f, Quad.EaseInOut, "Opacity", "1", "Y", "150");
            Tween.To(this, 0.3f, Linear.EaseNone, "BackBufferOpacity", "0.5");
            base.OnEnter();
        }

        public void ExitScreen()
        {
            (ScreenManager as RCScreenManager).HideCurrentScreen();
            m_runChoiceDialogue = false;
            m_dialogChoiceContainer.Visible = false;
            m_dialogChoiceContainer.Scale = Vector2.Zero;
            if (!m_runCancelEndHandler)
            {
                if (m_confirmMethodInfo != null)
                {
                    m_confirmMethodInfo.Invoke(m_confirmMethodObj, m_confirmArgs);
                }
            }
            else if (m_cancelMethodInfo != null)
            {
                m_cancelMethodInfo.Invoke(m_cancelMethodObj, m_cancelArgs);
            }
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Dialogue Screen");
                m_confirmMethodObj = null;
                m_confirmMethodInfo = null;
                if (m_confirmArgs != null)
                {
                    Array.Clear(m_confirmArgs, 0, m_confirmArgs.Length);
                }
                m_confirmArgs = null;
                m_cancelMethodObj = null;
                m_cancelMethodInfo = null;
                if (m_cancelArgs != null)
                {
                    Array.Clear(m_cancelArgs, 0, m_cancelArgs.Length);
                }
                m_cancelArgs = null;
                m_dialogContainer.Dispose();
                m_dialogContainer = null;
                m_dialogChoiceContainer.Dispose();
                m_dialogChoiceContainer = null;
                if (m_dialogText != null)
                {
                    Array.Clear(m_dialogText, 0, m_dialogText.Length);
                }
                m_dialogText = null;
                if (m_dialogTitles != null)
                {
                    Array.Clear(m_dialogTitles, 0, m_dialogTitles.Length);
                }
                m_dialogTitles = null;
                base.Dispose();
            }
        }
    }
}
