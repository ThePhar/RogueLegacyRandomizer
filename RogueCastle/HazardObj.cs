/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle
{
    public class HazardObj : PhysicsObj, IDealsDamageObj
    {
        private Texture2D m_texture;

        public override int Width
        {
            get { return _width; }
        }

        public override int Height
        {
            get { return _height; }
        }

        public override Rectangle TerrainBounds
        {
            get
            {
                foreach (CollisionBox current in CollisionBoxes)
                {
                    if (current.Type == 0)
                    {
                        return current.AbsRect;
                    }
                }
                return Bounds;
            }
        }

        public int Damage
        {
            get
            {
                PlayerObj player = Game.ScreenManager.Player;
                int num =
                    (int)
                        Math.Round(
                            player.BaseHealth + player.GetEquipmentHealth() + Game.PlayerStats.BonusHealth*5 +
                            SkillSystem.GetSkill(SkillType.Health_Up).ModifierAmount +
                            SkillSystem.GetSkill(SkillType.Health_Up_Final).ModifierAmount,
                            MidpointRounding.AwayFromZero);
                int num2 = (int) (num*0.2f);
                if (num2 < 1)
                {
                    num2 = 1;
                }
                return num2;
            }
        }

        public HazardObj(int width, int height) : base("Spikes_Sprite", null)
        {
            IsWeighted = false;
            IsCollidable = true;
            CollisionTypeTag = 10;
            DisableHitboxUpdating = true;
        }

        public void InitializeTextures(Camera2D camera)
        {
            Vector2 vector = new Vector2(60f/_width, 60f/_height);
            _width = (int) (_width*vector.X);
            _height = (int) (_height*vector.Y);
            m_texture = ConvertToTexture(camera);
            _width = (int) Math.Ceiling(_width/vector.X);
            _height = (int) Math.Ceiling(_height/vector.Y);
            Scale = new Vector2(_width/(_width/60f*64f), 1f);
        }

        public void SetWidth(int width)
        {
            _width = width;
            foreach (CollisionBox current in CollisionBoxes)
            {
                if (current.Type == 0)
                {
                    current.Width = _width - 25;
                }
                else
                {
                    current.Width = _width;
                }
            }
        }

        public void SetHeight(int height)
        {
            _height = height;
        }

        public override void Draw(Camera2D camera)
        {
            camera.Draw(m_texture, Position, new Rectangle(0, 0, (int) (Width/60f*64f), Height), TextureColor,
                MathHelper.ToRadians(Rotation), Vector2.Zero, Scale, SpriteEffects.None, 1f);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new HazardObj(_width, _height);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            HazardObj hazardObj = obj as HazardObj;
            hazardObj.SetWidth(Width);
            hazardObj.SetHeight(Height);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_texture.Dispose();
                m_texture = null;
                base.Dispose();
            }
        }

        public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
        {
            base.PopulateFromXMLReader(reader, ci);
            SetWidth(_width);
            SetHeight(_height);
        }
    }
}