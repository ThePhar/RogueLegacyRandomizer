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
