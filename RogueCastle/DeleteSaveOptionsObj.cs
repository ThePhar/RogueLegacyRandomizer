// 
//  Rogue Legacy Randomizer - DeleteSaveOptionsObj.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using DS2DEngine;
using RogueCastle.Screens;

namespace RogueCastle
{
    public class DeleteSaveOptionsObj : OptionsObj
    {
        public DeleteSaveOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Delete Save") { }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (IsActive)
                {
                    var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
                    rCScreenManager.DialogueScreen.SetDialogue("Delete Save");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSaveAskAgain");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    rCScreenManager.DisplayScreen(13, false);
                }
            }
        }

        public void CancelCommand()
        {
            IsActive = false;
        }

        public void DeleteSaveAskAgain()
        {
            var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("Delete Save2");
            rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
            rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave");
            rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
            rCScreenManager.DisplayScreen(13, false);
        }

        public void DeleteSave()
        {
            IsActive = false;
            Game.PlayerStats.Dispose();
            (m_parentScreen.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(false);
            (m_parentScreen.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(true);
            SkillSystem.ResetAllTraits();
            Game.PlayerStats = new PlayerStats();
            (m_parentScreen.ScreenManager as RCScreenManager).Player.Reset();
            SoundManager.StopMusic(1f);
            (m_parentScreen.ScreenManager as RCScreenManager).DisplayScreen(23, true);
        }
    }
}