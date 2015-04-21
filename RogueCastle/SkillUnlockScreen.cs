using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class SkillUnlockScreen : Screen
	{
		private ObjContainer m_plate;
		private ObjContainer m_picturePlate;
		private SpriteObj m_picture;
		private TextObj m_text;
		private SpriteObj m_titlePlate;
		private SpriteObj m_title;
		private byte m_skillUnlockType;
		public float BackBufferOpacity
		{
			get;
			set;
		}
		public override void LoadContent()
		{
			this.m_plate = new ObjContainer("SkillUnlockPlate_Character");
			this.m_plate.Position = new Vector2(660f, 360f);
			this.m_plate.ForceDraw = true;
			this.m_text = new TextObj(Game.JunicodeFont);
			this.m_text.ForceDraw = true;
			this.m_text.Text = "This is temporary text to see how many sentences can be fit into this area.  Hopefully it is quite a bit, as this is the area where we will be comprehensively describing the unlock.  It will also be used to determine word wrap length.";
			this.m_text.FontSize = 10f;
			this.m_text.WordWrap(340);
			this.m_text.DropShadow = new Vector2(2f, 2f);
			this.m_plate.AddChild(this.m_text);
			this.m_text.Position = new Vector2(-110f, -100f);
			this.m_picturePlate = new ObjContainer("SkillUnlockPicturePlate_Character");
			this.m_picturePlate.Position = new Vector2(360f, 410f);
			this.m_picturePlate.Rotation = -15f;
			this.m_picturePlate.ForceDraw = true;
			this.m_picture = new SpriteObj("BlacksmithUnlockPicture_Sprite");
			this.m_picture.ForceDraw = true;
			this.m_picture.OutlineWidth = 1;
			this.m_picture.Position = this.m_picturePlate.Position;
			this.m_titlePlate = new SpriteObj("SkillUnlockTitlePlate_Sprite");
			this.m_titlePlate.Position = new Vector2(660f, 160f);
			this.m_titlePlate.ForceDraw = true;
			this.m_title = new SpriteObj("ClassUnlockedText_Sprite");
			this.m_title.Position = this.m_titlePlate.Position;
			this.m_title.Y -= 40f;
			this.m_title.ForceDraw = true;
			base.LoadContent();
		}
		public override void PassInData(List<object> objList)
		{
			this.m_skillUnlockType = (byte)objList[0];
		}
		public override void OnEnter()
		{
			SoundManager.PlaySound("Upgrade_Splash_In");
			this.SetData();
			this.BackBufferOpacity = 0f;
			this.m_plate.Scale = Vector2.Zero;
			this.m_titlePlate.Scale = Vector2.Zero;
			this.m_title.Scale = Vector2.Zero;
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"BackBufferOpacity",
				"0.7"
			});
			Tween.To(this.m_titlePlate, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_title, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.1",
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			Tween.To(this.m_plate, 0.5f, new Easing(Back.EaseOut), new string[]
			{
				"delay",
				"0.3",
				"ScaleX",
				"1",
				"ScaleY",
				"1"
			});
			this.m_picturePlate.Scale = new Vector2(2f, 2f);
			this.m_picturePlate.Opacity = 0f;
			this.m_picturePlate.Rotation = 0f;
			Tween.To(this.m_picturePlate, 0.3f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.4",
				"ScaleX",
				"1",
				"ScaleY",
				"1",
				"Rotation",
				"-15"
			});
			Tween.To(this.m_picturePlate, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.4",
				"Opacity",
				"1"
			});
			this.m_picture.Scale = new Vector2(2f, 2f);
			this.m_picture.Opacity = 0f;
			this.m_picture.Rotation = 0f;
			Tween.To(this.m_picture, 0.3f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.4",
				"ScaleX",
				"1",
				"ScaleY",
				"1",
				"Rotation",
				"-15"
			});
			Tween.To(this.m_picture, 0.1f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.4",
				"Opacity",
				"1"
			});
			base.OnEnter();
		}
		private void SetData()
		{
			switch (this.m_skillUnlockType)
			{
			case 1:
				this.m_picture.ChangeSprite("BlacksmithUnlockPicture_Sprite");
				this.m_title.ChangeSprite("SmithyUnlockedText_Sprite");
				break;
			case 2:
				this.m_picture.ChangeSprite("EnchantressUnlockPicture_Sprite");
				this.m_title.ChangeSprite("EnchantressUnlockedText_Sprite");
				break;
			case 3:
				this.m_picture.ChangeSprite("ArchitectUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ArchitectUnlockedText_Sprite");
				break;
			case 4:
				this.m_picture.ChangeSprite("NinjaUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ClassUnlockedText_Sprite");
				break;
			case 5:
				this.m_picture.ChangeSprite("BankerUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ClassUnlockedText_Sprite");
				break;
			case 6:
				this.m_picture.ChangeSprite("SpellSwordUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ClassUnlockedText_Sprite");
				break;
			case 7:
				this.m_picture.ChangeSprite("LichUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ClassUnlockedText_Sprite");
				break;
			case 8:
				this.m_picture.ChangeSprite("KnightUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 9:
				this.m_picture.ChangeSprite("MageUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 10:
				this.m_picture.ChangeSprite("BarbarianUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 11:
				this.m_picture.ChangeSprite("NinjaUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 12:
				this.m_picture.ChangeSprite("AssassinUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 13:
				this.m_picture.ChangeSprite("BankerUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 14:
				this.m_picture.ChangeSprite("SpellSwordUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 15:
				this.m_picture.ChangeSprite("LichUpgradePicture_Sprite");
				this.m_title.ChangeSprite("ClassUpgradedText_Sprite");
				break;
			case 16:
				this.m_picture.ChangeSprite("DragonUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ClassUnlockedText_Sprite");
				break;
			case 17:
				this.m_picture.ChangeSprite("TraitorUnlockPicture_Sprite");
				this.m_title.ChangeSprite("ClassUnlockedText_Sprite");
				break;
			}
			this.m_text.Text = SkillUnlockType.Description(this.m_skillUnlockType);
			this.m_text.WordWrap(340);
		}
		private void ExitTransition()
		{
			SoundManager.PlaySound("Upgrade_Splash_Out");
			Tween.To(this.m_picture, 0.5f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_picturePlate, 0.5f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_titlePlate, 0.5f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_title, 0.5f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this.m_plate, 0.5f, new Easing(Back.EaseIn), new string[]
			{
				"ScaleX",
				"0",
				"ScaleY",
				"0"
			});
			Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
			{
				"delay",
				"0.4",
				"BackBufferOpacity",
				"0"
			});
			Tween.AddEndHandlerToLastTween(base.ScreenManager, "HideCurrentScreen", new object[0]);
		}
		public override void HandleInput()
		{
			if (this.m_plate.ScaleX == 1f && (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3)))
			{
				this.ExitTransition();
			}
			base.HandleInput();
		}
		public override void Draw(GameTime gametime)
		{
			base.Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
			base.Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * this.BackBufferOpacity);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
			this.m_plate.Draw(base.Camera);
			this.m_titlePlate.Draw(base.Camera);
			this.m_title.Draw(base.Camera);
			this.m_picturePlate.Draw(base.Camera);
			base.Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			this.m_picture.Draw(base.Camera);
			base.Camera.End();
			base.Draw(gametime);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				Console.WriteLine("Disposing SkillUnlock Screen");
				this.m_picturePlate.Dispose();
				this.m_picturePlate = null;
				this.m_picture.Dispose();
				this.m_picture = null;
				this.m_text = null;
				this.m_title.Dispose();
				this.m_title = null;
				this.m_titlePlate.Dispose();
				this.m_titlePlate = null;
				this.m_plate.Dispose();
				this.m_plate = null;
				base.Dispose();
			}
		}
	}
}
