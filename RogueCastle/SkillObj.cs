// 
//  Rogue Legacy Randomizer - SkillObj.cs
//  Last Modified 2022-01-24
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enums;
using RogueCastle.Systems;

namespace RogueCastle
{
    public class SkillObj : SpriteObj
    {
        private TextObj LevelText;
        private SpriteObj m_coinIcon;
        private int m_currentLevel;

        public SkillObj(string spriteName) : base(spriteName)
        {
            StatType = 0;
            DisplayStat = false;
            CanPurchase = false;
            Visible = false;
            ForceDraw = true;
            LevelText = new TextObj(Game.JunicodeFont);
            LevelText.FontSize = 10f;
            LevelText.Align = Types.TextAlign.Centre;
            LevelText.OutlineWidth = 2;
            InputDescription = "";
            OutlineWidth = 2;
            m_coinIcon = new SpriteObj("UpgradeIcon_Sprite");
        }

        public string Description { get; set; }
        public string InputDescription { get; set; }
        public float PerLevelModifier { get; set; }
        public int BaseCost { get; set; }
        public int Appreciation { get; set; }
        public int MaxLevel { get; set; }
        public SkillType Trait { get; set; }
        public string IconName { get; set; }
        public string UnitOfMeasurement { get; set; }
        public byte StatType { get; set; }
        public bool DisplayStat { get; set; }
        public bool CanPurchase { get; set; }
        public ManorPiece ManorPiece { get; set; }

        public int CurrentLevel
        {
            get { return m_currentLevel; }
            set
            {
                if (value > MaxLevel)
                {
                    m_currentLevel = MaxLevel;
                    return;
                }

                m_currentLevel = value;
            }
        }

        public int TotalCost
        {
            get { return BaseCost + CurrentLevel * Appreciation; }
        }

        public float ModifierAmount
        {
            get { return CurrentLevel * PerLevelModifier; }
        }

        public override void Draw(Camera2D camera)
        {
            if (Opacity > 0f)
            {
                var opacity = Opacity;
                TextureColor = Color.Black;
                Opacity = 0.5f;
                X += 8f;
                Y += 8f;
                base.Draw(camera);
                X -= 8f;
                Y -= 8f;
                TextureColor = Color.White;
                Opacity = opacity;
            }

            base.Draw(camera);
            LevelText.Position = new Vector2(X, Bounds.Bottom - LevelText.Height / 2);
            LevelText.Text = CurrentLevel + "/" + MaxLevel;
            LevelText.Opacity = Opacity;
            if (CurrentLevel >= MaxLevel)
            {
                LevelText.TextureColor = Color.Yellow;
                LevelText.Text = "Max";
            }
            else
            {
                LevelText.TextureColor = Color.White;
            }

            LevelText.Draw(camera);
            if (Game.PlayerStats.Gold >= TotalCost && CurrentLevel < MaxLevel && CanPurchase)
            {
                m_coinIcon.Opacity = Opacity;
                m_coinIcon.Position = new Vector2(X + 18f, Y - 40f);
                m_coinIcon.Draw(camera);
            }
        }

        protected override GameObj CreateCloneInstance()
        {
            return new SkillObj(_spriteName);
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
            var skillObj = obj as SkillObj;
            skillObj.Description = Description;
            skillObj.PerLevelModifier = PerLevelModifier;
            skillObj.BaseCost = BaseCost;
            skillObj.Appreciation = Appreciation;
            skillObj.MaxLevel = MaxLevel;
            skillObj.CurrentLevel = CurrentLevel;
            skillObj.Trait = Trait;
            skillObj.InputDescription = InputDescription;
            skillObj.UnitOfMeasurement = UnitOfMeasurement;
            skillObj.StatType = StatType;
            skillObj.DisplayStat = DisplayStat;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                LevelText.Dispose();
                LevelText = null;
                m_coinIcon.Dispose();
                m_coinIcon = null;
                base.Dispose();
            }
        }
    }
}