using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class DialogueScreen : Screen
	{
		private ObjContainer m_dialogContainer;
		private byte m_dialogCounter;
		private string[] m_dialogTitles;
		private string[] m_dialogText;
		private float m_dialogContinueIconY;
		private string m_dialogueObjName;
		private MethodInfo m_confirmMethodInfo;
		private object m_confirmMethodObj;
		private object[] m_confirmArgs;
		private MethodInfo m_cancelMethodInfo;
		private object m_cancelMethodObj;
		private object[] m_cancelArgs;
		private bool m_runChoiceDialogue;
		private ObjContainer m_dialogChoiceContainer;
		private bool m_runCancelEndHandler;
		private byte m_highlightedChoice = 2;
		private bool m_lockControls;
		private float m_textScrollSpeed = 0.03f;
		private float m_inputDelayTimer;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public override void LoadContent()
		{
			TextObj textObj = new TextObj(Game.JunicodeFont);
			textObj.FontSize = 12f;
			textObj.Align = Types.TextAlign.Left;
			TextObj textObj2 = new TextObj(Game.JunicodeFont);
			textObj2.FontSize = 14f;
			textObj2.Text = "Blacksmith";
			textObj2.DropShadow = new Vector2(2f, 2f);
			textObj2.TextureColor = new Color(236, 197, 132);
			this.m_dialogContainer = new ObjContainer("DialogBox_Character");
			this.m_dialogContainer.Position = new Vector2(660f, 100f);
			this.m_dialogContainer.AddChild(textObj2);
			this.m_dialogContainer.ForceDraw = true;
			textObj2.Position = new Vector2((float)(-(float)this.m_dialogContainer.Width) / 2.2f, (float)(-(float)this.m_dialogContainer.Height) / 1.6f);
			this.m_dialogContainer.AddChild(textObj);
			textObj.Position = new Vector2((float)(-(float)this.m_dialogContainer.Width) / 2.15f, (float)(-(float)this.m_dialogContainer.Height) / 3.5f);
			textObj.Text = "This is a test to see how much text I can fit onto this dialog box without it running out of space. The text needs to be defined after the dialog text position is set, because the dialogtext width affects the entire width of the dialog container, which in END.";
			textObj.WordWrap(850);
			textObj.DropShadow = new Vector2(2f, 3f);
			SpriteObj spriteObj = new SpriteObj("ContinueTextIcon_Sprite");
			spriteObj.Position = new Vector2((float)this.m_dialogContainer.GetChildAt(2).Bounds.Right, (float)this.m_dialogContainer.GetChildAt(2).Bounds.Bottom);
			this.m_dialogContainer.AddChild(spriteObj);
			this.m_dialogContinueIconY = spriteObj.Y;
			TextObj textObj3 = new TextObj(Game.JunicodeFont);
			textObj3.FontSize = 12f;
			textObj3.Text = "Yes";
			textObj3.Align = Types.TextAlign.Centre;
			TextObj textObj4 = new TextObj(Game.JunicodeFont);
			textObj4.FontSize = 12f;
			textObj4.Text = "No";
			textObj4.Align = Types.TextAlign.Centre;
			this.m_dialogChoiceContainer = new ObjContainer();
			SpriteObj obj = new SpriteObj("GameOverStatPlate_Sprite");
			this.m_dialogChoiceContainer.AddChild(obj);
			SpriteObj spriteObj2 = new SpriteObj("DialogueChoiceHighlight_Sprite");
			this.m_dialogChoiceContainer.AddChild(spriteObj2);
			this.m_dialogChoiceContainer.ForceDraw = true;
			this.m_dialogChoiceContainer.Position = new Vector2(660f, 360f);
			this.m_dialogChoiceContainer.AddChild(textObj3);
			textObj3.Y -= 40f;
			this.m_dialogChoiceContainer.AddChild(textObj4);
			textObj4.Y += 7f;
			spriteObj2.Position = new Vector2(textObj3.X, textObj3.Y + (float)(spriteObj2.Height / 2) + 3f);
			this.m_dialogChoiceContainer.Scale = Vector2.Zero;
			this.m_dialogChoiceContainer.Visible = false;
			base.LoadContent();
		}
		public void SetDialogue(string dialogueObjName)
		{
			this.m_dialogueObjName = dialogueObjName;
			this.m_confirmMethodObj = null;
			this.m_confirmMethodInfo = null;
			if (this.m_confirmArgs != null)
			{
				Array.Clear(this.m_confirmArgs, 0, this.m_confirmArgs.Length);
			}
			this.m_cancelMethodObj = null;
			this.m_cancelMethodInfo = null;
			if (this.m_cancelArgs != null)
			{
				Array.Clear(this.m_cancelArgs, 0, this.m_cancelArgs.Length);
			}
		}
		public void SetConfirmEndHandler(object methodObject, string functionName, params object[] args)
		{
			this.m_confirmMethodObj = methodObject;
			this.m_confirmMethodInfo = methodObject.GetType().GetMethod(functionName);
			this.m_confirmArgs = args;
		}
		public void SetConfirmEndHandler(Type methodType, string functionName, params object[] args)
		{
			Type[] array = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				array[i] = args[i].GetType();
			}
			this.m_confirmMethodInfo = methodType.GetMethod(functionName, array);
			this.m_confirmArgs = args;
			if (this.m_confirmMethodInfo == null)
			{
				this.m_confirmMethodInfo = methodType.GetMethod(functionName, new Type[]
				{
					args[0].GetType().MakeArrayType()
				});
				this.m_confirmArgs = new object[1];
				this.m_confirmArgs[0] = args;
			}
			this.m_confirmMethodObj = null;
		}
		public void SetCancelEndHandler(object methodObject, string functionName, params object[] args)
		{
			this.m_cancelMethodObj = methodObject;
			this.m_cancelMethodInfo = methodObject.GetType().GetMethod(functionName);
			this.m_cancelArgs = args;
		}
		public void SetCancelEndHandler(Type methodType, string functionName, params object[] args)
		{
			Type[] array = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				array[i] = args[i].GetType();
			}
			this.m_cancelMethodInfo = methodType.GetMethod(functionName, array);
			this.m_cancelArgs = args;
			if (this.m_cancelMethodInfo == null)
			{
				this.m_cancelMethodInfo = methodType.GetMethod(functionName, new Type[]
				{
					args[0].GetType().MakeArrayType()
				});
				this.m_cancelArgs = new object[1];
				this.m_cancelArgs[0] = args;
			}
			this.m_cancelMethodObj = null;
		}
		public void SetDialogueChoice(string dialogueObjName)
		{
			DialogueObj text = DialogueManager.GetText(dialogueObjName);
			(this.m_dialogChoiceContainer.GetChildAt(2) as TextObj).Text = text.Speakers[0];
			(this.m_dialogChoiceContainer.GetChildAt(3) as TextObj).Text = text.Dialogue[0];
			if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
			{
				(this.m_dialogChoiceContainer.GetChildAt(2) as TextObj).RandomizeSentence(false);
				(this.m_dialogChoiceContainer.GetChildAt(3) as TextObj).RandomizeSentence(false);
			}
			this.m_runChoiceDialogue = true;
		}
		public override void HandleInput()
		{
			if (!this.m_lockControls && this.m_inputDelayTimer <= 0f)
			{
				if (!this.m_dialogChoiceContainer.Visible)
				{
					if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
					{
						if ((int)this.m_dialogCounter < this.m_dialogText.Length - 1)
						{
							TextObj textObj = this.m_dialogContainer.GetChildAt(2) as TextObj;
							if (!textObj.IsTypewriting)
							{
								this.m_dialogCounter += 1;
								(this.m_dialogContainer.GetChildAt(1) as TextObj).Text = this.m_dialogTitles[(int)this.m_dialogCounter];
								textObj.Text = this.m_dialogText[(int)this.m_dialogCounter];
								if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
								{
									textObj.RandomizeSentence(false);
									(this.m_dialogContainer.GetChildAt(1) as TextObj).RandomizeSentence(false);
								}
								textObj.WordWrap(850);
								textObj.BeginTypeWriting((float)this.m_dialogText[(int)this.m_dialogCounter].Length * this.m_textScrollSpeed, "dialogue_tap");
							}
							else
							{
								textObj.StopTypeWriting(true);
							}
						}
						else if (!this.m_runChoiceDialogue && !(this.m_dialogContainer.GetChildAt(2) as TextObj).IsTypewriting)
						{
							this.m_lockControls = true;
							SoundManager.PlaySound("DialogMenuClose");
							Tween.To(this.m_dialogContainer, 0.3f, new Easing(Quad.EaseIn), new string[]
							{
								"Opacity",
								"0",
								"Y",
								"0"
							});
							Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
							{
								"BackBufferOpacity",
								"0"
							});
							Tween.AddEndHandlerToLastTween(this, "ExitScreen", new object[0]);
						}
						else
						{
							(this.m_dialogContainer.GetChildAt(2) as TextObj).StopTypeWriting(true);
						}
						SpriteObj spriteObj = this.m_dialogContainer.GetChildAt(3) as SpriteObj;
						if ((int)this.m_dialogCounter == this.m_dialogText.Length - 1)
						{
							spriteObj.ChangeSprite("EndTextIcon_Sprite");
							if (this.m_runChoiceDialogue)
							{
								TextObj textObj2 = this.m_dialogContainer.GetChildAt(2) as TextObj;
								textObj2.StopTypeWriting(true);
								this.m_dialogChoiceContainer.Visible = true;
								Tween.To(this.m_dialogChoiceContainer, 0.3f, new Easing(Back.EaseOut), new string[]
								{
									"ScaleX",
									"1",
									"ScaleY",
									"1"
								});
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
						this.m_highlightedChoice += 1;
						if (this.m_highlightedChoice > 3)
						{
							this.m_highlightedChoice = 2;
						}
						this.m_dialogChoiceContainer.GetChildAt(1).Y = this.m_dialogChoiceContainer.GetChildAt((int)this.m_highlightedChoice).Y + (float)(this.m_dialogChoiceContainer.GetChildAt(1).Height / 2) + 3f;
					}
					else if (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17))
					{
						SoundManager.PlaySound("frame_swap");
						this.m_highlightedChoice -= 1;
						if (this.m_highlightedChoice < 2)
						{
							this.m_highlightedChoice = 3;
						}
						this.m_dialogChoiceContainer.GetChildAt(1).Y = this.m_dialogChoiceContainer.GetChildAt((int)this.m_highlightedChoice).Y + (float)(this.m_dialogChoiceContainer.GetChildAt(1).Height / 2) + 3f;
					}
					if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
					{
						this.m_runCancelEndHandler = false;
						if (this.m_highlightedChoice == 3)
						{
							this.m_runCancelEndHandler = true;
							SoundManager.PlaySound("DialogueMenuCancel");
						}
						else
						{
							SoundManager.PlaySound("DialogueMenuConfirm");
						}
						this.m_lockControls = true;
						SoundManager.PlaySound("DialogMenuClose");
						Tween.To(this.m_dialogContainer, 0.3f, new Easing(Quad.EaseInOut), new string[]
						{
							"Opacity",
							"0",
							"Y",
							"100"
						});
						Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
						{
							"BackBufferOpacity",
							"0"
						});
						Tween.To(this.m_dialogChoiceContainer, 0.3f, new Easing(Back.EaseIn), new string[]
						{
							"ScaleX",
							"0",
							"ScaleY",
							"0"
						});
						Tween.AddEndHandlerToLastTween(this, "ExitScreen", new object[0]);
					}
					else if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
					{
						this.m_highlightedChoice = 3;
						this.m_dialogChoiceContainer.GetChildAt(1).Y = this.m_dialogChoiceContainer.GetChildAt((int)this.m_highlightedChoice).Y + (float)(this.m_dialogChoiceContainer.GetChildAt(1).Height / 2) + 3f;
						this.m_runCancelEndHandler = true;
						SoundManager.PlaySound("DialogueMenuCancel");
						this.m_lockControls = true;
						SoundManager.PlaySound("DialogMenuClose");
						Tween.To(this.m_dialogContainer, 0.3f, new Easing(Quad.EaseInOut), new string[]
						{
							"Opacity",
							"0",
							"Y",
							"100"
						});
						Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
						{
							"BackBufferOpacity",
							"0"
						});
						Tween.To(this.m_dialogChoiceContainer, 0.3f, new Easing(Back.EaseIn), new string[]
						{
							"ScaleX",
							"0",
							"ScaleY",
							"0"
						});
						Tween.AddEndHandlerToLastTween(this, "ExitScreen", new object[0]);
					}
				}
			}
			base.HandleInput();
		}
		public override void Update(GameTime gameTime)
		{
			if (this.m_inputDelayTimer > 0f)
			{
				this.m_inputDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (!this.m_dialogChoiceContainer.Visible && (int)this.m_dialogCounter == this.m_dialogText.Length - 1 && this.m_runChoiceDialogue)
			{
				SpriteObj spriteObj = this.m_dialogContainer.GetChildAt(3) as SpriteObj;
				spriteObj.ChangeSprite("EndTextIcon_Sprite");
				if (this.m_runChoiceDialogue)
				{
					TextObj textObj = this.m_dialogContainer.GetChildAt(2) as TextObj;
					textObj.StopTypeWriting(true);
					this.m_dialogChoiceContainer.Visible = true;
					Tween.To(this.m_dialogChoiceContainer, 0.3f, new Easing(Back.EaseOut), new string[]
					{
						"ScaleX",
						"1",
						"ScaleY",
						"1"
					});
					Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", new object[]
					{
						"DialogOpenBump"
					});
				}
			}
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			this.m_dialogContainer.Draw(base.Camera);
			if (this.m_dialogContainer.ScaleX > 0f)
			{
				this.m_dialogContainer.GetChildAt(3).Y = this.m_dialogContinueIconY + (float)Math.Sin((double)(Game.TotalGameTime * 20f)) * 2f;
			}
			this.m_dialogChoiceContainer.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gameTime);
		}
		public override void OnEnter()
		{
			this.m_inputDelayTimer = 0.5f;
			SoundManager.PlaySound("DialogOpen");
			this.m_lockControls = false;
			this.m_runCancelEndHandler = false;
			this.m_highlightedChoice = 2;
			this.m_dialogChoiceContainer.Scale = new Vector2(1f, 1f);
			this.m_dialogChoiceContainer.GetChildAt(1).Y = this.m_dialogChoiceContainer.GetChildAt((int)this.m_highlightedChoice).Y + (float)(this.m_dialogChoiceContainer.GetChildAt(1).Height / 2) + 3f;
			this.m_dialogChoiceContainer.Scale = Vector2.Zero;
			DialogueObj text = DialogueManager.GetText(this.m_dialogueObjName);
			string[] speakers = text.Speakers;
			string[] dialogue = text.Dialogue;
			SpriteObj spriteObj = this.m_dialogContainer.GetChildAt(3) as SpriteObj;
			if (dialogue.Length > 1)
			{
				spriteObj.ChangeSprite("ContinueTextIcon_Sprite");
			}
			else
			{
				spriteObj.ChangeSprite("EndTextIcon_Sprite");
			}
			this.m_dialogCounter = 0;
			this.m_dialogTitles = speakers;
			this.m_dialogText = dialogue;
			this.m_dialogContainer.Scale = Vector2.One;
			this.m_dialogContainer.Opacity = 0f;
			(this.m_dialogContainer.GetChildAt(2) as TextObj).Text = dialogue[(int)this.m_dialogCounter];
			(this.m_dialogContainer.GetChildAt(2) as TextObj).WordWrap(850);
			(this.m_dialogContainer.GetChildAt(1) as TextObj).Text = speakers[(int)this.m_dialogCounter];
			if (Game.PlayerStats.Traits.X == 5f || Game.PlayerStats.Traits.Y == 5f)
			{
				(this.m_dialogContainer.GetChildAt(2) as TextObj).RandomizeSentence(false);
				(this.m_dialogContainer.GetChildAt(1) as TextObj).RandomizeSentence(false);
			}
			(this.m_dialogContainer.GetChildAt(2) as TextObj).BeginTypeWriting((float)dialogue[(int)this.m_dialogCounter].Length * this.m_textScrollSpeed, "dialogue_tap");
			Tween.To(this.m_dialogContainer, 0.3f, new Easing(Quad.EaseInOut), new string[]
			{
				"Opacity",
				"1",
				"Y",
				"150"
			});
			Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.5"
			});
			base.OnEnter();
		}
		public void ExitScreen()
		{
			(base.ScreenManager as RCScreenManager).HideCurrentScreen();
			this.m_runChoiceDialogue = false;
			this.m_dialogChoiceContainer.Visible = false;
			this.m_dialogChoiceContainer.Scale = Vector2.Zero;
			if (!this.m_runCancelEndHandler)
			{
				if (this.m_confirmMethodInfo != null)
				{
					this.m_confirmMethodInfo.Invoke(this.m_confirmMethodObj, this.m_confirmArgs);
					return;
				}
			}
			else if (this.m_cancelMethodInfo != null)
			{
				this.m_cancelMethodInfo.Invoke(this.m_cancelMethodObj, this.m_cancelArgs);
			}
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing Dialogue Screen");
				this.m_confirmMethodObj = null;
				this.m_confirmMethodInfo = null;
				if (this.m_confirmArgs != null)
				{
					Array.Clear(this.m_confirmArgs, 0, this.m_confirmArgs.Length);
				}
				this.m_confirmArgs = null;
				this.m_cancelMethodObj = null;
				this.m_cancelMethodInfo = null;
				if (this.m_cancelArgs != null)
				{
					Array.Clear(this.m_cancelArgs, 0, this.m_cancelArgs.Length);
				}
				this.m_cancelArgs = null;
				this.m_dialogContainer.Dispose();
				this.m_dialogContainer = null;
				this.m_dialogChoiceContainer.Dispose();
				this.m_dialogChoiceContainer = null;
				if (this.m_dialogText != null)
				{
					Array.Clear(this.m_dialogText, 0, this.m_dialogText.Length);
				}
				this.m_dialogText = null;
				if (this.m_dialogTitles != null)
				{
					Array.Clear(this.m_dialogTitles, 0, this.m_dialogTitles.Length);
				}
				this.m_dialogTitles = null;
				base.Dispose();
			}
		}
	}
}
