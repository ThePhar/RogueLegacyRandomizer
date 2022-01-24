// 
//  Rogue Legacy Randomizer - ConnectArchipelagoOptionObj.cs
//  Last Modified 2022-01-24
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using RogueCastle.Screens;
using RogueCastle.Enums;

namespace RogueCastle.Options
{
    public class ConnectArchipelagoOptionObj : ArchipelagoOptionsObj
    {
        public ConnectArchipelagoOptionObj(ArchipelagoScreen parentScreen) : base(parentScreen, "Connect")
        {
            m_parentScreen = parentScreen;
        }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (IsActive)
                {
                    var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
                    // Add our dialogue if it's not there.
                    DialogueManager.AddText("Ready to Start", new[] { "" }, new[] { "Are you ready to start?" });

                    m_parentScreen.LockControls = true;

                    rCScreenManager.DialogueScreen.SetDialogue("Ready to Start");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "StartGame");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    rCScreenManager.DisplayScreen((int) ScreenType.Dialogue, false);
                }
            }
        }

        public override void Initialize()
        {
            m_nameText.Text = "Connect";
            base.Initialize();
        }

        public void StartGame()
        {
            IsActive = false;
            m_parentScreen.Connect();
        }

        public void CancelCommand()
        {
            IsActive = false;
            m_parentScreen.LockControls = false;
        }
    }
}