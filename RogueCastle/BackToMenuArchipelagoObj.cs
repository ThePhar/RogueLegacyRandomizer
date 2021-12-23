// 
// RogueLegacyArchipelago - BackToMenuArchipelagoObj.cs
// Last Modified 2021-12-23
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

namespace RogueCastle
{
    public class BackToMenuArchipelagoObj : ArchipelagoOptionsObj
    {
        public BackToMenuArchipelagoObj(ArchipelagoScreen parentScreen) : base(parentScreen, "Exit Archipelago Menu")
        {
        }

        public override bool IsActive
        {
            get { return base.IsActive; }
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
            m_nameText.Text = "Exit Archipelago Menu";
            base.Initialize();
        }

        public void GoBackToTitle()
        {
            IsActive = false;
            m_parentScreen.ExitTransition();
        }
    }
}
