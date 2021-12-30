// 
//  RogueLegacyArchipelago - SkillMeta.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using Microsoft.Xna.Framework;

namespace RogueCastle.Systems
{
    public class SkillMeta
    {
        public Vector2 Position { get; set; }
        public ManorPiece ManorPiece { get; set; }
        public SkillLink SkillLink { get; set; }

        public SkillMeta()
        {
            Position = new Vector2(0, 0);
            ManorPiece = ManorPiece.None;
            SkillLink = new SkillLink()
            {
                BottomLink = null,
                LeftLink = null,
                RightLink = null,
                TopLink = null,
            };
        }
    }
}
