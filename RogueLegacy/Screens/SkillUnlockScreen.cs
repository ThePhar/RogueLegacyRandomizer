// Rogue Legacy Randomizer - SkillUnlockScreen.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Enums;
using RogueLegacy.Systems;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class SkillUnlockScreen : Screen
    {
        private int          _locationId;
        private SpriteObj    _picture;
        private ObjContainer _picturePlate;
        private ObjContainer _plate;
        private byte         _skillUnlockType;
        private TextObj      _text;
        private SpriteObj    _title;
        private SpriteObj    _titlePlate;
        public  float        BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            _plate = new ObjContainer("SkillUnlockPlate_Character");
            _plate.Position = new Vector2(660f, 360f);
            _plate.ForceDraw = true;
            _text = new TextObj(Game.JunicodeFont);
            _text.ForceDraw = true;
            _text.Text = "This is temporary text to see how many sentences can be fit into this area.  Hopefully it is quite a bit, as this is the area where we will be comprehensively describing the unlock.  It will also be used to determine word wrap length.";
            _text.FontSize = 10f;
            _text.WordWrap(340);
            _text.DropShadow = new Vector2(2f, 2f);
            _plate.AddChild(_text);
            _text.Position = new Vector2(-110f, -100f);
            _picturePlate = new ObjContainer("SkillUnlockPicturePlate_Character");
            _picturePlate.Position = new Vector2(360f, 410f);
            _picturePlate.Rotation = -15f;
            _picturePlate.ForceDraw = true;
            _picture = new SpriteObj("BlacksmithUnlockPicture_Sprite");
            _picture.ForceDraw = true;
            _picture.OutlineWidth = 1;
            _picture.Position = _picturePlate.Position;
            _titlePlate = new SpriteObj("SkillUnlockTitlePlate_Sprite");
            _titlePlate.Position = new Vector2(660f, 160f);
            _titlePlate.ForceDraw = true;
            _title = new SpriteObj("ItemFoundText_Sprite");
            _title.Position = _titlePlate.Position;
            _title.Y -= 40f;
            _title.ForceDraw = true;
            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            _skillUnlockType = (byte) objList[0];

            if (_skillUnlockType == (byte) SkillUnlock.NetworkItem)
            {
                _locationId = (int) objList[1];
            }
        }

        public override void OnEnter()
        {
            SoundManager.PlaySound("Upgrade_Splash_In");
            SetData();
            BackBufferOpacity = 0f;
            _plate.Scale = Vector2.Zero;
            _titlePlate.Scale = Vector2.Zero;
            _title.Scale = Vector2.Zero;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.7");
            Tween.To(_titlePlate, 0.5f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(_title, 0.5f, Back.EaseOut, "delay", "0.1", "ScaleX", "1", "ScaleY", "1");
            Tween.To(_plate, 0.5f, Back.EaseOut, "delay", "0.3", "ScaleX", "1", "ScaleY", "1");
            _picturePlate.Scale = new Vector2(2f, 2f);
            _picturePlate.Opacity = 0f;
            _picturePlate.Rotation = 0f;
            Tween.To(_picturePlate, 0.3f, Tween.EaseNone, "delay", "0.4", "ScaleX", "1", "ScaleY", "1", "Rotation",
                "-15");
            Tween.To(_picturePlate, 0.1f, Tween.EaseNone, "delay", "0.4", "Opacity", "1");
            _picture.Scale = new Vector2(2f, 2f);
            _picture.Opacity = 0f;
            _picture.Rotation = 0f;
            Tween.To(_picture, 0.3f, Tween.EaseNone, "delay", "0.4", "ScaleX", "1", "ScaleY", "1", "Rotation", "-15");
            Tween.To(_picture, 0.1f, Tween.EaseNone, "delay", "0.4", "Opacity", "1");
            base.OnEnter();
        }

        private void SetData()
        {
            _text.Text = ((SkillUnlock) _skillUnlockType).Description();

            switch (_skillUnlockType)
            {
                case 1:
                    _picture.ChangeSprite("BlacksmithUnlockPicture_Sprite");
                    _title.ChangeSprite("SmithyUnlockedText_Sprite");
                    break;

                case 2:
                    _picture.ChangeSprite("EnchantressUnlockPicture_Sprite");
                    _title.ChangeSprite("EnchantressUnlockedText_Sprite");
                    break;

                case 3:
                    _picture.ChangeSprite("ArchitectUnlockPicture_Sprite");
                    _title.ChangeSprite("ArchitectUnlockedText_Sprite");
                    break;

                case 4:
                    _picture.ChangeSprite("NinjaUnlockPicture_Sprite");
                    _title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case 5:
                    _picture.ChangeSprite("BankerUnlockPicture_Sprite");
                    _title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case 6:
                    _picture.ChangeSprite("SpellSwordUnlockPicture_Sprite");
                    _title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case 7:
                    _picture.ChangeSprite("LichUnlockPicture_Sprite");
                    _title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case 8:
                    _picture.ChangeSprite("KnightUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 9:
                    _picture.ChangeSprite("MageUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 10:
                    _picture.ChangeSprite("BarbarianUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 11:
                    _picture.ChangeSprite("NinjaUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 12:
                    _picture.ChangeSprite("AssassinUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 13:
                    _picture.ChangeSprite("BankerUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 14:
                    _picture.ChangeSprite("SpellSwordUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 15:
                    _picture.ChangeSprite("LichUpgradePicture_Sprite");
                    _title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;

                case 16:
                    _picture.ChangeSprite("DragonUnlockPicture_Sprite");
                    _title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case 17:
                    _picture.ChangeSprite("TraitorUnlockPicture_Sprite");
                    _title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case (byte) SkillUnlock.NetworkItem:
                    var item = Program.Game.ArchipelagoManager.LocationCache[_locationId];

                    var location = ManorContainer.ArchipelagoLocationTable.First(kp => kp.Value == _locationId).Key;
                    _text.Text = string.Format(
                        "\"I just finished building your {0} and found this {1} for {2} while building. You may as well take it.\"",
                        location,
                        Program.Game.ArchipelagoManager.GetItemName(item.Item),
                        Program.Game.ArchipelagoManager.GetPlayerName(item.Player));
                    break;
            }

            _text.WordWrap(340);
        }

        private void ExitTransition()
        {
            SoundManager.PlaySound("Upgrade_Splash_Out");
            Tween.To(_picture, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_picturePlate, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_titlePlate, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_title, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(_plate, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.4", "BackBufferOpacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
        }

        public override void HandleInput()
        {
            if (_plate.ScaleX == 1f &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                 Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)))
            {
                ExitTransition();
            }

            base.HandleInput();
        }

        public override void Draw(GameTime gametime)
        {
            try
            {
                Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                    null);
                Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
                Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                _plate.Draw(Camera);
                _titlePlate.Draw(Camera);
                _title.Draw(Camera);
                _picturePlate.Draw(Camera);
                Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                _picture.Draw(Camera);
                Camera.End();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            } // I won't tell if you won't.

            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing SkillUnlock Screen");
            _picturePlate.Dispose();
            _picturePlate = null;
            _picture.Dispose();
            _picture = null;
            _text = null;
            _title.Dispose();
            _title = null;
            _titlePlate.Dispose();
            _titlePlate = null;
            _plate.Dispose();
            _plate = null;
            base.Dispose();
        }
    }
}
