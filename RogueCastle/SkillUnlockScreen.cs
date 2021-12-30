// 
//  RogueLegacyArchipelago - SkillUnlockScreen.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using RogueCastle.Systems;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class SkillUnlockScreen : Screen
    {
        private SpriteObj    m_picture;
        private ObjContainer m_picturePlate;
        private ObjContainer m_plate;
        private byte         m_skillUnlockType;
        private int          m_locationId;
        private TextObj      m_text;
        private SpriteObj    m_title;
        private SpriteObj    m_titlePlate;
        public  float        BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_plate = new ObjContainer("SkillUnlockPlate_Character");
            m_plate.Position = new Vector2(660f, 360f);
            m_plate.ForceDraw = true;
            m_text = new TextObj(Game.JunicodeFont);
            m_text.ForceDraw = true;
            m_text.Text =
                "This is temporary text to see how many sentences can be fit into this area.  Hopefully it is quite a bit, as this is the area where we will be comprehensively describing the unlock.  It will also be used to determine word wrap length.";
            m_text.FontSize = 10f;
            m_text.WordWrap(340);
            m_text.DropShadow = new Vector2(2f, 2f);
            m_plate.AddChild(m_text);
            m_text.Position = new Vector2(-110f, -100f);
            m_picturePlate = new ObjContainer("SkillUnlockPicturePlate_Character");
            m_picturePlate.Position = new Vector2(360f, 410f);
            m_picturePlate.Rotation = -15f;
            m_picturePlate.ForceDraw = true;
            m_picture = new SpriteObj("BlacksmithUnlockPicture_Sprite");
            m_picture.ForceDraw = true;
            m_picture.OutlineWidth = 1;
            m_picture.Position = m_picturePlate.Position;
            m_titlePlate = new SpriteObj("SkillUnlockTitlePlate_Sprite");
            m_titlePlate.Position = new Vector2(660f, 160f);
            m_titlePlate.ForceDraw = true;
            m_title = new SpriteObj("ItemFoundText_Sprite");
            m_title.Position = m_titlePlate.Position;
            m_title.Y -= 40f;
            m_title.ForceDraw = true;
            base.LoadContent();
        }

        public override void PassInData(List<object> objList)
        {
            m_skillUnlockType = (byte) objList[0];

            if (m_skillUnlockType == SkillUnlockType.NetworkItem)
                m_locationId = (int) objList[1];
        }

        public override void OnEnter()
        {
            SoundManager.PlaySound("Upgrade_Splash_In");
            SetData();
            BackBufferOpacity = 0f;
            m_plate.Scale = Vector2.Zero;
            m_titlePlate.Scale = Vector2.Zero;
            m_title.Scale = Vector2.Zero;
            Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.7");
            Tween.To(m_titlePlate, 0.5f, Back.EaseOut, "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_title, 0.5f, Back.EaseOut, "delay", "0.1", "ScaleX", "1", "ScaleY", "1");
            Tween.To(m_plate, 0.5f, Back.EaseOut, "delay", "0.3", "ScaleX", "1", "ScaleY", "1");
            m_picturePlate.Scale = new Vector2(2f, 2f);
            m_picturePlate.Opacity = 0f;
            m_picturePlate.Rotation = 0f;
            Tween.To(m_picturePlate, 0.3f, Tween.EaseNone, "delay", "0.4", "ScaleX", "1", "ScaleY", "1", "Rotation",
                "-15");
            Tween.To(m_picturePlate, 0.1f, Tween.EaseNone, "delay", "0.4", "Opacity", "1");
            m_picture.Scale = new Vector2(2f, 2f);
            m_picture.Opacity = 0f;
            m_picture.Rotation = 0f;
            Tween.To(m_picture, 0.3f, Tween.EaseNone, "delay", "0.4", "ScaleX", "1", "ScaleY", "1", "Rotation", "-15");
            Tween.To(m_picture, 0.1f, Tween.EaseNone, "delay", "0.4", "Opacity", "1");
            base.OnEnter();
        }

        private void SetData()
        {
            m_text.Text = SkillUnlockType.Description(m_skillUnlockType);

            switch (m_skillUnlockType)
            {
                case 1:
                    m_picture.ChangeSprite("BlacksmithUnlockPicture_Sprite");
                    m_title.ChangeSprite("SmithyUnlockedText_Sprite");
                    break;
                case 2:
                    m_picture.ChangeSprite("EnchantressUnlockPicture_Sprite");
                    m_title.ChangeSprite("EnchantressUnlockedText_Sprite");
                    break;
                case 3:
                    m_picture.ChangeSprite("ArchitectUnlockPicture_Sprite");
                    m_title.ChangeSprite("ArchitectUnlockedText_Sprite");
                    break;
                case 4:
                    m_picture.ChangeSprite("NinjaUnlockPicture_Sprite");
                    m_title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;
                case 5:
                    m_picture.ChangeSprite("BankerUnlockPicture_Sprite");
                    m_title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;
                case 6:
                    m_picture.ChangeSprite("SpellSwordUnlockPicture_Sprite");
                    m_title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;
                case 7:
                    m_picture.ChangeSprite("LichUnlockPicture_Sprite");
                    m_title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;
                case 8:
                    m_picture.ChangeSprite("KnightUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 9:
                    m_picture.ChangeSprite("MageUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 10:
                    m_picture.ChangeSprite("BarbarianUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 11:
                    m_picture.ChangeSprite("NinjaUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 12:
                    m_picture.ChangeSprite("AssassinUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 13:
                    m_picture.ChangeSprite("BankerUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 14:
                    m_picture.ChangeSprite("SpellSwordUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 15:
                    m_picture.ChangeSprite("LichUpgradePicture_Sprite");
                    m_title.ChangeSprite("ClassUpgradedText_Sprite");
                    break;
                case 16:
                    m_picture.ChangeSprite("DragonUnlockPicture_Sprite");
                    m_title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;
                case 17:
                    m_picture.ChangeSprite("TraitorUnlockPicture_Sprite");
                    m_title.ChangeSprite("ClassUnlockedText_Sprite");
                    break;

                case SkillUnlockType.NetworkItem:
                    var item = Program.Game.ArchipelagoManager.LocationCache[m_locationId];
                    Console.WriteLine("{0}, {1}, {2}", item.Item, item.Location, item.Player);
                    var location = ManorContainer.ArchipelagoLocationTable.First(kp => kp.Value == (LocationCode) m_locationId).Key;
                    m_text.Text = string.Format("\"I just finished building your {0} and found this {1} for {2} while building. You may as well take it.\"", location, Program.Game.ArchipelagoManager.GetItemName(item.Item), Program.Game.ArchipelagoManager.GetPlayerName(item.Player));
                    break;
            }

            m_text.WordWrap(340);
        }

        private void ExitTransition()
        {
            SoundManager.PlaySound("Upgrade_Splash_Out");
            Tween.To(m_picture, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_picturePlate, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_titlePlate, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_title, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(m_plate, 0.5f, Back.EaseIn, "ScaleX", "0", "ScaleY", "0");
            Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.4", "BackBufferOpacity", "0");
            Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
        }

        public override void HandleInput()
        {
            if (m_plate.ScaleX == 1f &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) ||
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
                m_plate.Draw(Camera);
                m_titlePlate.Draw(Camera);
                m_title.Draw(Camera);
                m_picturePlate.Draw(Camera);
                Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                m_picture.Draw(Camera);
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
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing SkillUnlock Screen");
                m_picturePlate.Dispose();
                m_picturePlate = null;
                m_picture.Dispose();
                m_picture = null;
                m_text = null;
                m_title.Dispose();
                m_title = null;
                m_titlePlate.Dispose();
                m_titlePlate = null;
                m_plate.Dispose();
                m_plate = null;
                base.Dispose();
            }
        }
    }
}
