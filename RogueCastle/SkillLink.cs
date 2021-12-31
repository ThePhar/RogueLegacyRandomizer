// 
//  RogueLegacyArchipelago - SkillLink.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using RogueCastle.Structs;

namespace RogueCastle
{
    public class SkillLink
    {
        public SkillType? BottomLink;
        public SkillType? LeftLink;
        public SkillType? RightLink;
        public SkillType? TopLink;
    }
}