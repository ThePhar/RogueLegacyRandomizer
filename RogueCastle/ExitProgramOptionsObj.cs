/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to the original disassembly and its modifications. 

  Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

namespace RogueCastle
{
	public class ExitProgramOptionsObj : OptionsObj
	{
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				base.IsActive = value;
				if (IsActive)
				{
					RCScreenManager rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
					rCScreenManager.DialogueScreen.SetDialogue("Quit Rogue Legacy");
					rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
					rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "QuitProgram", new object[0]);
					rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand", new object[0]);
					rCScreenManager.DisplayScreen(13, false, null);
				}
			}
		}
		public ExitProgramOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Quit Rogue Legacy")
		{
		}
		public void QuitProgram()
		{
			(m_parentScreen.ScreenManager.Game as Game).SaveOnExit();
			m_parentScreen.ScreenManager.Game.Exit();
		}
		public void CancelCommand()
		{
			IsActive = false;
		}
	}
}
