// 
// RogueLegacyArchipelago - HazardObj.cs
// Last Modified 2021-12-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Globalization;
using System.Xml;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Enums;

namespace RogueCastle
{
    public class HazardObj : PhysicsObj, IDealsDamageObj
    {
        private Texture2D m_texture;

        public HazardObj(int width, int height) : base("Spikes_Sprite")
        {
            IsWeighted = false;
            IsCollidable = true;
            CollisionTypeTag = 10;
            DisableHitboxUpdating = true;
        }

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
                foreach (var current in CollisionBoxes)
                    if (current.Type == 0)
                    {
                        return current.AbsRect;
                    }

                return Bounds;
            }
        }

        public int Damage
        {
            get
            {
                var player = Game.ScreenManager.Player;
                var num =
                    (int)
                    Math.Round(
                        player.BaseHealth + player.GetEquipmentHealth() + Game.PlayerStats.BonusHealth * 5 +
                        SkillSystem.GetSkill(Skill.HealthUp).ModifierAmount +
                        SkillSystem.GetSkill(Skill.HealthUpFinal).ModifierAmount,
                        MidpointRounding.AwayFromZero);
                var num2 = (int) (num * 0.2f);
                if (num2 < 1)
                {
                    num2 = 1;
                }

                return num2;
            }
        }

        public void InitializeTextures(Camera2D camera)
        {
            var vector = new Vector2(60f / _width, 60f / _height);
            _width = (int) (_width * vector.X);
            _height = (int) (_height * vector.Y);
            m_texture = ConvertToTexture(camera);
            _width = (int) Math.Ceiling(_width / vector.X);
            _height = (int) Math.Ceiling(_height / vector.Y);
            Scale = new Vector2(_width / (_width / 60f * 64f), 1f);
        }

        public void SetWidth(int width)
        {
            _width = width;
            foreach (var current in CollisionBoxes)
                if (current.Type == 0)
                {
                    current.Width = _width - 25;
                }
                else
                {
                    current.Width = _width;
                }
        }

        public void SetHeight(int height)
        {
            _height = height;
        }

        public override void Draw(Camera2D camera)
        {
            camera.Draw(m_texture, Position, new Rectangle(0, 0, (int) (Width / 60f * 64f), Height), TextureColor,
                MathHelper.ToRadians(Rotation), Vector2.Zero, Scale, SpriteEffects.None, 1f);
        }

        protected override GameObj CreateCloneInstance()
        {
            return new HazardObj(_width, _height);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var hazardObj = obj as HazardObj;
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