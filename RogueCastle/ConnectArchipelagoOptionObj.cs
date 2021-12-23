// 
// RogueLegacyArchipelago - ConnectArchipelagoOptionObj.cs
// Last Modified 2021-12-23
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Threading.Tasks;
using Archipelago;

namespace RogueCastle
{
    public class ConnectArchipelagoOptionObj : ArchipelagoOptionsObj
    {
        public ConnectArchipelagoOptionObj(ArchipelagoScreen parentScreen) : base(parentScreen, "Connect & Start")
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
                    var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
                    // Add our dialogue if it's not there.
                    DialogueManager.AddText("Ready to Start", new []{""}, new []{"Are you ready to start?"});

                    rCScreenManager.DialogueScreen.SetDialogue("Ready to Start");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "StartGame");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    rCScreenManager.DisplayScreen(13, false);
                }
            }
        }

        public override void Initialize()
        {
            m_nameText.Text = "Connect & Start";
            base.Initialize();
        }

        public void StartGame()
        {
            IsActive = false;

            var levelScreen = Game.ScreenManager.CurrentScreen as ArchipelagoScreen;
            if (levelScreen != null)
                Task.Run(() => levelScreen.Connect());
        }

        public void CancelCommand()
        {
            IsActive = false;
        }
    }
}
