//  RogueLegacyRandomizer - ExitProgramOptionsObj.cs
//  Last Modified 2023-10-25 8:58 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using RogueLegacy.Screens;

namespace RogueLegacy
{
    public class ExitProgramOptionsObj : OptionsObj
    {
        public ExitProgramOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Quit Rogue Legacy") { }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (IsActive)
                {
                    var rCScreenManager = _parentScreen.ScreenManager as RCScreenManager;
                    rCScreenManager.DialogueScreen.SetDialogue("Quit Rogue Legacy");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "QuitProgram");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    rCScreenManager.DisplayScreen(13, false);
                }
            }
        }

        public void QuitProgram()
        {
            (_parentScreen.ScreenManager.Game as Game).SaveAndReset();
            _parentScreen.ScreenManager.Game.Exit();
        }

        public void CancelCommand()
        {
            IsActive = false;
        }
    }
}
