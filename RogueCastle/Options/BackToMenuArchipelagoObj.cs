// 
//  Rogue Legacy Randomizer - BackToMenuArchipelagoObj.cs
//  Last Modified 2022-04-05
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using RogueCastle.Screens;

namespace RogueCastle.Options
{
    public class BackToMenuArchipelagoObj : RandomizerOption
    {
        public BackToMenuArchipelagoObj(RandomizerScreen parentScreen) : base(parentScreen, "Close Menu") { }

        public override bool IsActive
        {
            get => base.IsActive;
            set
            {
                base.IsActive = value;
                if (IsActive)
                {
                    GoBackToTitle();
                }
            }
        }

        public override void Initialize()
        {
            _nameText.Text = "Close Menu";
            base.Initialize();
        }

        public void GoBackToTitle()
        {
            IsActive = false;
            _parentScreen.ExitTransition();
        }
    }
}
