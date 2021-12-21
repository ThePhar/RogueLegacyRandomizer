using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public static class ArchipelagoManager
    {
        public static TextScreen TextScreen = new TextScreen();
        
        public static void GiveTrait(SkillObj trait, PlayerObj player)
        {

            // Do not upgrade traits past their max level.
            if (trait.CurrentLevel >= trait.MaxLevel)
                return;

            // Get a reference to our max health/mana incase we upgrade those stats later.
            var maxHp = player.MaxHealth;
            var maxMp = player.MaxMana;

            // var item = new TextObj(Game.JunicodeFont);
            // item.Text = "You received a Health Up from Phar";
            // item.FontSize = 18f;
            // item.ForceDraw = true;
            // item.Align = Types.TextAlign.Centre;
            // item.Position = new Vector2(660f, 260f);
            // item.OutlineWidth = 2;
            //
            // var list = new List<object>();
            // list.Add(new Vector2(50, 50));
            // list.Add(2);
            // list.Add(new Vector2(50, 50));
            // list.Add(true);
            // list.Add(item);
            // list.Add(false);
            // Game.ScreenManager.DisplayScreen(13, true, list);
            
            // Increase the player and trait level.
            Game.PlayerStats.CurrentLevel++;
            trait.CurrentLevel++;
            
            // If the user got a HP or MP upgrade, we need to also give them the appropriate amount of health.
            if (trait.TraitType == SkillType.Health_Up || trait.TraitType == SkillType.Mana_Up)
            {
                player.CurrentHealth += player.MaxHealth - maxHp;
                player.CurrentMana += player.MaxMana - maxMp;
            }
        }
    }
}